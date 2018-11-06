using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BisectionSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BisectionSingleRootFinder;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// The primary location for normal model formulas.
	/// </summary>
	public sealed class NormalFormulaRepository
	{

	  /// <summary>
	  /// The normal distribution implementation.
	  /// </summary>
	  private static readonly ProbabilityDistribution<double> DISTRIBUTION = new NormalDistribution(0, 1);
	  /// <summary>
	  /// The comparison value used to determine near-zero.
	  /// </summary>
	  private const double NEAR_ZERO = 1e-16;
	  /// <summary>
	  /// The maximal number of iterations in the root solving algorithm.
	  /// </summary>
	  private const int MAX_ITERATIONS = 100;
	  /// <summary>
	  /// The solution precision.
	  /// </summary>
	  private const double EPS = 1e-15;

	  /// <summary>
	  /// Limit defining "close to ATM forward" to avoid the formula singularity in the impliedVolatilityFromBlackVolatility. * </summary>
	  private const double ATM_LIMIT = 1.0E-3;

	  // restricted constructor
	  private NormalFormulaRepository()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward price.
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the forward price </returns>
	  public static double price(double forward, double strike, double timeToExpiry, double normalVol, PutCall putCall)
	  {
		double sigmaRootT = normalVol * Math.Sqrt(timeToExpiry);
		int sign = putCall.Call ? 1 : -1;
		if (sigmaRootT < NEAR_ZERO)
		{
		  double x = sign * (forward - strike);
		  return (x > 0 ? x : 0d);
		}
		double arg = sign * (forward - strike) / sigmaRootT;
		double cdf = DISTRIBUTION.getCDF(arg);
		double pdf = DISTRIBUTION.getPDF(arg);
		return sign * (forward - strike) * cdf + sigmaRootT * pdf;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the price and first order derivatives.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the volatility
	  /// <li>[2] derivative with respect to the strike
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="numeraire">  the numeraire </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the price and associated derivatives </returns>
	  public static ValueDerivatives priceAdjoint(double forward, double strike, double timeToExpiry, double normalVol, double numeraire, PutCall putCall)
	  {

		int sign = putCall.Call ? 1 : -1;
		double price;
		double cdf = 0d;
		double pdf = 0d;
		double arg = 0d;
		double x = 0d;
		// Implementation Note: Forward sweep.
		double sigmaRootT = normalVol * Math.Sqrt(timeToExpiry);
		if (sigmaRootT < NormalFormulaRepository.NEAR_ZERO)
		{
		  x = sign * (forward - strike);
		  price = (x > 0 ? numeraire * x : 0d);
		}
		else
		{
		  arg = sign * (forward - strike) / sigmaRootT;
		  cdf = NormalFormulaRepository.DISTRIBUTION.getCDF(arg);
		  pdf = NormalFormulaRepository.DISTRIBUTION.getPDF(arg);
		  price = numeraire * (sign * (forward - strike) * cdf + sigmaRootT * pdf);
		}
		// Implementation Note: Backward sweep.
		double forwardDerivative;
		double volatilityDerivative;
		double strikeDerivative;
		double priceBar = 1d;
		if (sigmaRootT < NormalFormulaRepository.NEAR_ZERO)
		{
		  double xBar = (x > 0 ? numeraire : 0d);
		  forwardDerivative = sign * xBar;
		  strikeDerivative = -forwardDerivative;
		  volatilityDerivative = 0d;
		}
		else
		{
		  double cdfBar = numeraire * (sign * (forward - strike)) * priceBar;
		  double pdfBar = numeraire * sigmaRootT * priceBar;
		  double argBar = pdf * cdfBar - pdf * arg * pdfBar;
		  forwardDerivative = numeraire * sign * cdf * priceBar + sign / sigmaRootT * argBar;
		  strikeDerivative = -forwardDerivative;
		  double sigmaRootTBar = -arg / sigmaRootT * argBar + numeraire * pdf * priceBar;
		  volatilityDerivative = Math.Sqrt(timeToExpiry) * sigmaRootTBar;
		}
		return ValueDerivatives.of(price, DoubleArray.of(forwardDerivative, volatilityDerivative, strikeDerivative));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the delta.
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the delta </returns>
	  public static double delta(double forward, double strike, double timeToExpiry, double normalVol, PutCall putCall)
	  {
		int sign = putCall.Call ? 1 : -1;
		double sigmaRootT = normalVol * Math.Sqrt(timeToExpiry);
		if (sigmaRootT < NEAR_ZERO)
		{
		  double x = sign * (forward - strike);
		  if (Math.Abs(x) <= NEAR_ZERO)
		  {
			// ambiguous if x and sigmaRootT are tiny, then reference number is returned
			return sign * 0.5;
		  }
		  return x > 0 ? sign : 0d;
		}
		double arg = sign * (forward - strike) / sigmaRootT;
		double cdf = DISTRIBUTION.getCDF(arg);
		return sign * cdf;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the gamma.
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the gamma </returns>
	  public static double gamma(double forward, double strike, double timeToExpiry, double normalVol, PutCall putCall)
	  {
		int sign = putCall.Call ? 1 : -1;
		double sigmaRootT = normalVol * Math.Sqrt(timeToExpiry);
		if (sigmaRootT < NEAR_ZERO)
		{
		  double x = sign * (forward - strike);
		  // ambiguous (tend to be infinite) if x and sigmaRootT are tiny, then reference number is returned
		  return Math.Abs(x) > NEAR_ZERO ? 0d : 1d / Math.Sqrt(2d * Math.PI) / sigmaRootT;
		}
		double arg = (forward - strike) / sigmaRootT;
		double pdf = DISTRIBUTION.getPDF(arg);
		return pdf / sigmaRootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the theta.
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the theta </returns>
	  public static double theta(double forward, double strike, double timeToExpiry, double normalVol, PutCall putCall)
	  {
		int sign = putCall.Call ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = normalVol * rootT;
		if (sigmaRootT < NEAR_ZERO)
		{
		  double x = sign * (forward - strike);
		  // ambiguous if x and sigmaRootT are tiny, then reference number is returned
		  return Math.Abs(x) > NEAR_ZERO ? 0d : -0.5 * normalVol / rootT / Math.Sqrt(2d * Math.PI);
		}
		double arg = (forward - strike) / sigmaRootT;
		double pdf = DISTRIBUTION.getPDF(arg);
		return -0.5 * pdf * normalVol / rootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the vega.
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="normalVol">  the normal volatility </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the vega </returns>
	  public static double vega(double forward, double strike, double timeToExpiry, double normalVol, PutCall putCall)
	  {
		int sign = putCall.Call ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = normalVol * rootT;
		if (sigmaRootT < NEAR_ZERO)
		{
		  double x = sign * (forward - strike);
		  // ambiguous if x and sigmaRootT are tiny, then reference number is returned
		  return Math.Abs(x) > NEAR_ZERO ? 0d : rootT / Math.Sqrt(2d * Math.PI);
		}
		double arg = (forward - strike) / sigmaRootT;
		double pdf = DISTRIBUTION.getPDF(arg);
		return pdf * rootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied volatility.
	  /// <para>
	  /// If the volatility data is not zero, it is used as a starting point for the volatility search.
	  /// </para>
	  /// <para>
	  /// Note that the 'numeraire' is a simple multiplier and is the responsibility of the caller.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="optionPrice">  the price of the option </param>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="initialNormalVol">  the normal volatility used to start the search </param>
	  /// <param name="numeraire">  the numeraire </param>
	  /// <param name="putCall">  whether it is put or call </param>
	  /// <returns> the implied volatility </returns>
	  public static double impliedVolatility(double optionPrice, double forward, double strike, double timeToExpiry, double initialNormalVol, double numeraire, PutCall putCall)
	  {

		double intrinsicPrice = numeraire * Math.Max(0, (putCall.Call ? 1 : -1) * (forward - strike));
		ArgChecker.isTrue(optionPrice > intrinsicPrice || DoubleMath.fuzzyEquals(optionPrice, intrinsicPrice, 1e-6), "Option price (" + optionPrice + ") less than intrinsic value (" + intrinsicPrice + ")");
		if (System.BitConverter.DoubleToInt64Bits(optionPrice) == Double.doubleToLongBits(intrinsicPrice))
		{
		  return 0d;
		}
		double sigma = (Math.Abs(initialNormalVol) < 1e-10 ? 0.3 * forward : initialNormalVol);
		double maxChange = 0.5 * forward;
		ValueDerivatives price = priceAdjoint(forward, strike, timeToExpiry, sigma, numeraire, putCall);
		double vega = price.getDerivative(1);
		double change = (price.Value - optionPrice) / vega;
		double sign = Math.Sign(change);
		change = sign * Math.Min(maxChange, Math.Abs(change));
		if (change > 0 && change > sigma)
		{
		  change = sigma;
		}
		int count = 0;
		while (Math.Abs(change) > EPS)
		{
		  sigma -= change;
		  price = priceAdjoint(forward, strike, timeToExpiry, sigma, numeraire, putCall);
		  vega = price.getDerivative(1);
		  change = (price.Value - optionPrice) / vega;
		  sign = Math.Sign(change);
		  change = sign * Math.Min(maxChange, Math.Abs(change));
		  if (change > 0 && change > sigma)
		  {
			change = sigma;
		  }
		  if (count++ > MAX_ITERATIONS)
		  {
			BracketRoot bracketer = new BracketRoot();
			BisectionSingleRootFinder rootFinder = new BisectionSingleRootFinder(EPS);
			System.Func<double, double> func = (double? volatility) =>
			{
		return numeraire * NormalFormulaRepository.price(forward, strike, timeToExpiry, volatility.Value, putCall) - optionPrice;
			};
			double[] range = bracketer.getBracketedPoints(func, 0d, 10d);
			return rootFinder.getRoot(func, range[0], range[1]).Value;
		  }
		}
		return sigma;
	  }

	  /// <summary>
	  /// Compute the implied volatility using an approximate explicit transformation formula.
	  /// <para>
	  /// Reference: Hagan, P. S. Volatility conversion calculator. Technical report, Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate/price </param>
	  /// <param name="strike">  the option strike </param>
	  /// <param name="timeToExpiry">  the option time to maturity </param>
	  /// <param name="blackVolatility">  the Black implied volatility </param>
	  /// <returns> the implied volatility </returns>
	  public static double impliedVolatilityFromBlackApproximated(double forward, double strike, double timeToExpiry, double blackVolatility)
	  {
		ArgChecker.isTrue(strike > 0, "strike must be strictly positive");
		ArgChecker.isTrue(forward > 0, "strike must be strictly positive");
		double lnFK = Math.Log(forward / strike);
		double s2t = blackVolatility * blackVolatility * timeToExpiry;
		if (Math.Abs((forward - strike) / strike) < ATM_LIMIT)
		{
		  double factor1 = Math.Sqrt(forward * strike);
		  double factor2 = (1.0d + lnFK * lnFK / 24.0d) / (1.0d + s2t / 24.0d + s2t * s2t / 5670.0d);
		  return blackVolatility * factor1 * factor2;
		}
		double factor1 = (forward - strike) / lnFK;
		double factor2 = 1.0d / (1.0d + (1.0d - lnFK * lnFK / 120.0d) / 24.0d * s2t + s2t * s2t / 5670.0d);
		return blackVolatility * factor1 * factor2;
	  }

	  /// <summary>
	  /// Compute the implied volatility using an approximate explicit transformation formula and its derivative 
	  /// with respect to the input Black volatility.
	  /// <para>
	  /// Reference: Hagan, P. S. Volatility conversion calculator. Technical report, Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate/price </param>
	  /// <param name="strike">  the option strike </param>
	  /// <param name="timeToExpiry">  the option time to maturity </param>
	  /// <param name="blackVolatility">  the Black implied volatility </param>
	  /// <returns> the implied volatility and its derivative </returns>
	  public static ValueDerivatives impliedVolatilityFromBlackApproximatedAdjoint(double forward, double strike, double timeToExpiry, double blackVolatility)
	  {
		ArgChecker.isTrue(strike > 0, "strike must be strictly positive");
		ArgChecker.isTrue(forward > 0, "strike must be strictly positive");
		double lnFK = Math.Log(forward / strike);
		double s2t = blackVolatility * blackVolatility * timeToExpiry;
		if (Math.Abs((forward - strike) / strike) < ATM_LIMIT)
		{
		  double factor1 = Math.Sqrt(forward * strike);
		  double factor2 = (1.0d + lnFK * lnFK / 24.0d) / (1.0d + s2t / 24.0d + s2t * s2t / 5670.0d);
		  double normalVol = blackVolatility * factor1 * factor2;
		  // Backward sweep
		  double blackVolatilityBar = factor1 * factor2;
		  double factor2Bar = blackVolatility * factor1;
		  double s2tBar = -(1.0d + lnFK * lnFK / 24.0d) / ((1.0d + s2t / 24.0d + s2t * s2t / 5670.0d) * (1.0d + s2t / 24.0d + s2t * s2t / 5670.0d)) * (1.0d / 24.0d + s2t / 2835.0d) * factor2Bar;
		  blackVolatilityBar += 2.0d * blackVolatility * timeToExpiry * s2tBar;
		  return ValueDerivatives.of(normalVol, DoubleArray.of(blackVolatilityBar));
		}
		double factor1 = (forward - strike) / lnFK;
		double factor2 = 1.0d / (1.0d + (1.0d - lnFK * lnFK / 120.0d) / 24.0d * s2t + s2t * s2t / 5670.0d);
		double normalVol = blackVolatility * factor1 * factor2;
		// Backward sweep
		double blackVolatilityBar = factor1 * factor2;
		double factor2Bar = blackVolatility * factor1;
		double s2tBar = -factor2 * factor2 * ((1.0d - lnFK * lnFK / 120.0d) / 24.0d + s2t / 2835.0d) * factor2Bar;
		blackVolatilityBar += 2.0d * blackVolatility * timeToExpiry * s2tBar;
		return ValueDerivatives.of(normalVol, DoubleArray.of(blackVolatilityBar));
	  }

	}

}