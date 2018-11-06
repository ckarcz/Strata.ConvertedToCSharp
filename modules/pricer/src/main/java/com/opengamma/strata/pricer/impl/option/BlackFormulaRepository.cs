using System;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using NewtonRaphsonSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.NewtonRaphsonSingleRootFinder;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;

	/// <summary>
	/// The primary repository for Black formulas, including the price, common greeks and implied volatility.
	/// <para>
	/// Other classes that have higher level abstractions (e.g. option data bundles) should call these functions.
	/// As the numeraire (e.g. the zero bond p(0,T) in the T-forward measure) in the Black formula is just a multiplication
	/// factor, all prices, input/output, are <b>forward</b> prices, i.e. (spot price)/numeraire.
	/// Note that a "reference value" is returned if computation comes across an ambiguous expression.
	/// </para>
	/// </summary>
	public sealed class BlackFormulaRepository
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(BlackFormulaRepository));

	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1);
	  private const double LARGE = 1e13;
	  private const double SMALL = 1e-13;
	  /// <summary>
	  /// The comparison value used to determine near-zero. </summary>
	  private const double NEAR_ZERO = 1e-16;
	  /// <summary>
	  /// Limit defining "close of ATM forward" to avoid the formula singularity. * </summary>
	  private const double ATM_LIMIT = 1.0E-3;
	  private const double ROOT_ACCURACY = 1.0E-7;
	  private static readonly NewtonRaphsonSingleRootFinder ROOT_FINDER = new NewtonRaphsonSingleRootFinder(ROOT_ACCURACY);

	  // restricted constructor
	  private BlackFormulaRepository()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward price.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the forward price </returns>
	  public static double price(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		if (bFwd && bStr)
		{
		  log.info("(large value)/(large value) ambiguous");
		  return isCall ? (forward >= strike ? forward : 0d) : (strike >= forward ? strike : 0d);
		}
		if (sigmaRootT < SMALL)
		{
		  return Math.Max(sign * (forward - strike), 0d);
		}
		if (Math.Abs(forward - strike) < SMALL || bSigRt)
		{
		  d1 = 0.5 * sigmaRootT;
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		  d2 = d1 - sigmaRootT;
		}

		double nF = NORMAL.getCDF(sign * d1);
		double nS = NORMAL.getCDF(sign * d2);
		double first = nF == 0d ? 0d : forward * nF;
		double second = nS == 0d ? 0d : strike * nS;

		double res = sign * (first - second);
		return Math.Max(0.0, res);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the price without numeraire and its derivatives.
	  /// <para>
	  /// The derivatives are in the following order:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the time to expiry
	  /// <li>[3] derivative with respect to the volatility
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the forward price and its derivatives  </returns>
	  public static ValueDerivatives priceAdjoint(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		if (bFwd && bStr)
		{
		  log.info("(large value)/(large value) ambiguous");
		  double price = isCall ? (forward >= strike ? forward : 0d) : (strike >= forward ? strike : 0d); // ???
		  return ValueDerivatives.of(price, DoubleArray.filled(4)); // ??
		}
		if (sigmaRootT < SMALL)
		{
		  bool isItm = (sign * (forward - strike)) > 0;
		  double price = isItm ? sign * (forward - strike) : 0d;
		  return ValueDerivatives.of(price, DoubleArray.of(isItm ? sign : 0d, isItm ? -sign : 0d, 0d, 0d));
		}
		if (Math.Abs(forward - strike) < SMALL || bSigRt)
		{
		  d1 = 0.5 * sigmaRootT;
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d2 = Math.Log(forward / strike) / sigmaRootT - 0.5 * sigmaRootT;
		  d1 = d2 + sigmaRootT;
		}

		double nF = NORMAL.getCDF(sign * d1);
		double nS = NORMAL.getCDF(sign * d2);
		double first = nF == 0d ? 0d : forward * nF;
		double second = nS == 0d ? 0d : strike * nS;
		double res = sign * (first - second);
		double price = Math.Max(0.0d, res);

		// Backward sweep
		double resBar = 1.0;
		double firstBar = sign * resBar;
		double secondBar = -sign * resBar;
		double forwardBar = nF * firstBar;
		double strikeBar = nS * secondBar;
		double nFBar = forward * firstBar;
		double d1Bar = sign * NORMAL.getPDF(sign * d1) * nFBar;
		// Implementation Note: d2Bar = 0; no need to implement it.
		// Methodology Note: d2Bar is optimal exercise boundary. The derivative at the optimal point is 0.
		double sigmaRootTBar = d1Bar;
		double lognormalVolBar = Math.Sqrt(timeToExpiry) * sigmaRootTBar;
		double timeToExpiryBar = 0.5 / Math.Sqrt(timeToExpiry) * lognormalVol * sigmaRootTBar;
		return ValueDerivatives.of(price, DoubleArray.of(forwardBar, strikeBar, timeToExpiryBar, lognormalVolBar));
	  }

	  /// <summary>
	  /// Computes the price without numeraire and its derivatives of the first and second order.
	  /// <para>
	  /// The first order derivatives are in the following order:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the time to expiry
	  /// <li>[3] derivative with respect to the volatility
	  /// </ul>
	  /// The price and the second order derivatives are in the ValueDerivatives which is the first element of the returned pair.
	  /// </para>
	  /// <para>
	  /// The second order derivatives are in the following order:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the volatility
	  /// </ul>
	  /// The second order derivatives are in the double[][] which is the second element of the returned pair.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the forward price and its derivatives  </returns>
	  public static Pair<ValueDerivatives, double[][]> priceAdjoint2(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {
		// Forward sweep
		double discountFactor = 1.0;
		double sqrttheta = Math.Sqrt(timeToExpiry);
		double omega = isCall ? 1 : -1;
		// Implementation Note: Forward sweep.
		double volPeriod = 0, kappa = 0, d1 = 0, d2 = 0;
		double x = 0;
		double p;
		if (strike < NEAR_ZERO || sqrttheta < NEAR_ZERO)
		{
		  x = omega * (forward - strike);
		  p = (x > 0 ? discountFactor * x : 0.0);
		  volPeriod = sqrttheta < NEAR_ZERO ? 0 : (lognormalVol * sqrttheta);
		}
		else
		{
		  volPeriod = lognormalVol * sqrttheta;
		  kappa = Math.Log(forward / strike) / volPeriod - 0.5 * volPeriod;
		  d1 = NORMAL.getCDF(omega * (kappa + volPeriod));
		  d2 = NORMAL.getCDF(omega * kappa);
		  p = discountFactor * omega * (forward * d1 - strike * d2);
		}
		// Implementation Note: Backward sweep.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] bsD2 = new double[3][3];
		double[][] bsD2 = RectangularArrays.ReturnRectangularDoubleArray(3, 3);
		double pBar = 1.0;
		double density1 = 0.0;
		double d1Bar = 0.0;
		double forwardBar = 0, strikeBar = 0, volPeriodBar = 0, lognormalVolBar = 0, sqrtthetaBar = 0, timeToExpiryBar = 0;
		if (strike < NEAR_ZERO || sqrttheta < NEAR_ZERO)
		{
		  forwardBar = (x > 0 ? discountFactor * omega : 0.0);
		  strikeBar = (x > 0 ? -discountFactor * omega : 0.0);
		}
		else
		{
		  d1Bar = discountFactor * omega * forward * pBar;
		  density1 = NORMAL.getPDF(omega * (kappa + volPeriod));
		  // Implementation Note: kappa_bar = 0; no need to implement it.
		  // Methodology Note: kappa_bar is optimal exercise boundary. The
		  // derivative at the optimal point is 0.
		  forwardBar = discountFactor * omega * d1 * pBar;
		  strikeBar = -discountFactor * omega * d2 * pBar;
		  volPeriodBar = density1 * omega * d1Bar;
		  lognormalVolBar = sqrttheta * volPeriodBar;
		  sqrtthetaBar = lognormalVol * volPeriodBar;
		  timeToExpiryBar = 0.5 / sqrttheta * sqrtthetaBar;
		}
		DoubleArray bsD = DoubleArray.of(forwardBar, strikeBar, timeToExpiryBar, lognormalVolBar);
		if (strike < NEAR_ZERO || sqrttheta < NEAR_ZERO)
		{
		  return Pair.of(ValueDerivatives.of(p, bsD), bsD2);
		}
		// Backward sweep: second derivative
		double d2Bar = -discountFactor * omega * strike;
		double density2 = NORMAL.getPDF(omega * kappa);
		double d1Kappa = omega * density1;
		double d1KappaKappa = -(kappa + volPeriod) * d1Kappa;
		double d2Kappa = omega * density2;
		double d2KappaKappa = -kappa * d2Kappa;
		double kappaKappaBar2 = d1KappaKappa * d1Bar + d2KappaKappa * d2Bar;
		double kappaV = -Math.Log(forward / strike) / (volPeriod * volPeriod) - 0.5;
		double kappaVV = 2 * Math.Log(forward / strike) / (volPeriod * volPeriod * volPeriod);
		double d1TotVV = density1 * omega * (-(kappa + volPeriod) * (kappaV + 1) * (kappaV + 1) + kappaVV);
		double d2TotVV = d2KappaKappa * kappaV * kappaV + d2Kappa * kappaVV;
		double vVbar2 = d1Bar * d1TotVV + d2Bar * d2TotVV;
		double volVolBar2 = vVbar2 * timeToExpiry;
		double kappaStrikeBar2 = -discountFactor * omega * d2Kappa;
		double kappaStrike = -1.0 / (strike * volPeriod);
		double strikeStrikeBar2 = (kappaKappaBar2 * kappaStrike + 2 * kappaStrikeBar2) * kappaStrike;
		double kappaStrikeV = 1.0 / strike / (volPeriod * volPeriod);
		double d1VK = -omega * (kappa + volPeriod) * density1 * (kappaV + 1) * kappaStrike + omega * density1 * kappaStrikeV;
		double d2V = d2Kappa * kappaV;
		double d2VK = -omega * kappa * density2 * kappaV * kappaStrike + omega * density2 * kappaStrikeV;
		double strikeD2Bar2 = -discountFactor * omega;
		double strikeVolblackBar2 = strikeD2Bar2 * d2V + d1Bar * d1VK + d2Bar * d2VK;
		double strikeVolBar2 = strikeVolblackBar2 * sqrttheta;
		double kappaForward = 1.0 / (forward * volPeriod);
		double forwardForwardBar2 = discountFactor * omega * d1Kappa * kappaForward;
		double strikeForwardBar2 = discountFactor * omega * d1Kappa * kappaStrike;
		double volForwardBar2 = discountFactor * omega * d1Kappa * (kappaV + 1) * sqrttheta;
		bsD2[0][0] = forwardForwardBar2;
		bsD2[0][2] = volForwardBar2;
		bsD2[2][0] = volForwardBar2;
		bsD2[0][1] = strikeForwardBar2;
		bsD2[1][0] = strikeForwardBar2;
		bsD2[2][2] = volVolBar2;
		bsD2[1][2] = strikeVolBar2;
		bsD2[2][1] = strikeVolBar2;
		bsD2[1][1] = strikeStrikeBar2;
		return Pair.of(ValueDerivatives.of(p, bsD), bsD2);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward driftless delta.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the forward driftless delta </returns>
	  public static double delta(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;

		double d1 = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return isCall ? 1d : 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return (isCall ? (forward > strike ? 1d : 0d) : (forward > strike ? 0d : -1d));
		  }
		  log.info("(log 1d)/0., ambiguous value");
		  return isCall ? 0.5 : -0.5;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		}

		return sign * NORMAL.getCDF(sign * d1);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the strike for the delta.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="forwardDelta">  the forward delta </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the strike </returns>
	  public static double strikeForDelta(double forward, double forwardDelta, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue((isCall && forwardDelta > 0 && forwardDelta < 1) || (!isCall && forwardDelta > -1 && forwardDelta < 0), "delta out of range", forwardDelta);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		int sign = isCall ? 1 : -1;
		double d1 = sign * NORMAL.getInverseCDF(sign * forwardDelta);

		double sigmaSqT = lognormalVol * lognormalVol * timeToExpiry;
		if (double.IsNaN(sigmaSqT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaSqT = 1d;
		}

		return forward * Math.Exp(-d1 * Math.Sqrt(sigmaSqT) + 0.5 * sigmaSqT);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless dual delta.
	  /// <para>
	  /// This is the first derivative of option price with respect to strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the driftless dual delta </returns>
	  public static double dualDelta(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;

		double d2 = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return isCall ? 0d : 1d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return (isCall ? (forward > strike ? -1d : 0d) : (forward > strike ? 0d : 1d));
		  }
		  log.info("(log 1d)/0., ambiguous value");
		  return isCall ? -0.5 : 0.5;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d2 = Math.Log(forward / strike) / sigmaRootT - 0.5 * sigmaRootT;
		}

		return -sign * NORMAL.getCDF(sign * d2);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the simple delta.
	  /// <para>
	  /// Note that this is not the standard delta one is accustomed to.
	  /// The argument of the cumulative normal is simply {@code d = Math.log(forward / strike) / sigmaRootT}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the simple delta </returns>
	  public static double simpleDelta(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;

		double d = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return isCall ? 0.5 : -0.5;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return (isCall ? (forward > strike ? 1d : 0d) : (forward > strike ? 0d : -1d));
		  }
		  log.info("(log 1d)/0., ambiguous");
		  return isCall ? 0.5 : -0.5;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d = 0d;
		}
		else
		{
		  d = Math.Log(forward / strike) / sigmaRootT;
		}

		return sign * NORMAL.getCDF(sign * d);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward driftless gamma.
	  /// <para>
	  /// This is the second order sensitivity of the forward option value to the forward.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 FV}{\partial^2 f}$
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the forward driftless gamma </returns>
	  public static double gamma(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		double d1 = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("(log 1d)/0d ambiguous");
		  return bFwd ? NORMAL.getPDF(0d) : NORMAL.getPDF(0d) / forward / sigmaRootT;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d1);
		return nVal == 0d ? 0d : nVal / forward / sigmaRootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless dual gamma.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless dual gamma </returns>
	  public static double dualGamma(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		double d2 = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("(log 1d)/0d ambiguous");
		  return bStr ? NORMAL.getPDF(0d) : NORMAL.getPDF(0d) / strike / sigmaRootT;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d2 = Math.Log(forward / strike) / sigmaRootT - 0.5 * sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d2);
		return nVal == 0d ? 0d : nVal / strike / sigmaRootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless cross gamma.
	  /// <para>
	  /// This is the sensitity of the delta to the strike.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 V}{\partial f \partial K}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless cross gamma </returns>
	  public static double crossGamma(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		double d2 = 0d;
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("(log 1d)/0d ambiguous");
		  return bFwd ? -NORMAL.getPDF(0d) : -NORMAL.getPDF(0d) / forward / sigmaRootT;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d2 = Math.Log(forward / strike) / sigmaRootT - 0.5 * sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d2);
		return nVal == 0d ? 0d : -nVal / forward / sigmaRootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the theta (non-forward).
	  /// <para>
	  /// This is the sensitivity of the present value to a change in time to maturity.
	  /// </para>
	  /// <para>
	  /// $\-frac{\partial * V}{\partial T}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <returns> theta </returns>
	  public static double theta(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall, double interestRate)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");

		if (-interestRate > LARGE)
		{
		  return 0d;
		}
		double driftLess = driftlessTheta(forward, strike, timeToExpiry, lognormalVol);
		if (Math.Abs(interestRate) < SMALL)
		{
		  return driftLess;
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		double priceLike = Double.NaN;
		double rt = (timeToExpiry < SMALL && Math.Abs(interestRate) > LARGE) ? (interestRate > 0d ? 1d : -1d) : interestRate * timeToExpiry;
		if (bFwd && bStr)
		{
		  log.info("(large value)/(large value) ambiguous");
		  priceLike = isCall ? (forward >= strike ? forward : 0d) : (strike >= forward ? strike : 0d);
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			if (rt > LARGE)
			{
			  priceLike = isCall ? (forward > strike ? forward : 0d) : (forward > strike ? 0d : -forward);
			}
			else
			{
			  priceLike = isCall ? (forward > strike ? forward - strike * Math.Exp(-rt) : 0d) : (forward > strike ? 0d : -forward + strike * Math.Exp(-rt));
			}
		  }
		  else
		  {
			if (Math.Abs(forward - strike) < SMALL | bSigRt)
			{
			  d1 = 0.5 * sigmaRootT;
			  d2 = -0.5 * sigmaRootT;
			}
			else
			{
			  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
			  d2 = d1 - sigmaRootT;
			}
			double nF = NORMAL.getCDF(sign * d1);
			double nS = NORMAL.getCDF(sign * d2);
			double first = nF == 0d ? 0d : forward * nF;
			double second = ((nS == 0d) | (Math.Exp(-interestRate * timeToExpiry) == 0d)) ? 0d : strike * Math.Exp(-interestRate * timeToExpiry) * nS;
			priceLike = sign * (first - second);
		  }
		}

		double res = (interestRate > LARGE && Math.Abs(priceLike) < SMALL) ? 0d : interestRate * priceLike;
		return Math.Abs(res) > LARGE ? res : driftLess + res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the theta (non-forward).
	  /// <para>
	  /// This is the sensitivity of the present value to a change in time to maturity
	  /// </para>
	  /// <para>
	  /// $\-frac{\partial * V}{\partial T}$.
	  /// </para>
	  /// <para>
	  /// This is consistent with <seealso cref="BlackScholesFormulaRepository"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <returns> theta </returns>
	  public static double thetaMod(double forward, double strike, double timeToExpiry, double lognormalVol, bool isCall, double interestRate)
	  {

		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");

		if (-interestRate > LARGE)
		{
		  return 0d;
		}
		double driftLess = driftlessTheta(forward, strike, timeToExpiry, lognormalVol);
		if (Math.Abs(interestRate) < SMALL)
		{
		  return driftLess;
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		int sign = isCall ? 1 : -1;

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d2 = 0d;

		double priceLike = Double.NaN;
		double rt = (timeToExpiry < SMALL && Math.Abs(interestRate) > LARGE) ? (interestRate > 0d ? 1d : -1d) : interestRate * timeToExpiry;
		if (bFwd && bStr)
		{
		  log.info("(large value)/(large value) ambiguous");
		  priceLike = isCall ? 0d : (strike >= forward ? strike : 0d);
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			if (rt > LARGE)
			{
			  priceLike = 0d;
			}
			else
			{
			  priceLike = isCall ? (forward > strike ? -strike : 0d) : (forward > strike ? 0d : +strike);
			}
		  }
		  else
		  {
			if (Math.Abs(forward - strike) < SMALL | bSigRt)
			{
			  d2 = -0.5 * sigmaRootT;
			}
			else
			{
			  d2 = Math.Log(forward / strike) / sigmaRootT - 0.5 * sigmaRootT;
			}
			double nS = NORMAL.getCDF(sign * d2);
			priceLike = (nS == 0d) ? 0d : -sign * strike * nS;
		  }
		}

		double res = (interestRate > LARGE && Math.Abs(priceLike) < SMALL) ? 0d : interestRate * priceLike;
		return Math.Abs(res) > LARGE ? res : driftLess + res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward driftless theta.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless theta </returns>
	  public static double driftlessTheta(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("log(1)/0 ambiguous");
		  if (rootT < SMALL)
		  {
			return forward < SMALL ? -NORMAL.getPDF(0d) * lognormalVol / 2.0 : (lognormalVol < SMALL ? -forward * NORMAL.getPDF(0d) / 2.0 : -forward * NORMAL.getPDF(0d) * lognormalVol / 2.0 / rootT);
		  }
		  if (lognormalVol < SMALL)
		  {
			return bFwd ? -NORMAL.getPDF(0d) / 2.0 / rootT : -forward * NORMAL.getPDF(0d) * lognormalVol / 2.0 / rootT;
		  }
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d1);
		return nVal == 0d ? 0d : -forward * nVal * lognormalVol / 2.0 / rootT;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the forward vega.
	  /// <para>
	  /// This is the sensitivity of the option's forward price wrt the implied volatility (which
	  /// is just the spot vega divided by the numeraire).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the forward vega </returns>
	  public static double vega(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}
		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("log(1)/0 ambiguous");
		  return (rootT < SMALL && forward > LARGE) ? NORMAL.getPDF(0d) : forward * rootT * NORMAL.getPDF(0d);
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d1);
		return nVal == 0d ? 0d : forward * rootT * nVal;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless vanna.
	  /// <para>
	  /// This is the second order derivative of the option value, once to the underlying forward
	  /// and once to volatility.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 FV}{\partial f \partial \sigma}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless vanna </returns>
	  public static double vanna(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("log(1)/0 ambiguous");
		  return lognormalVol < SMALL ? -NORMAL.getPDF(0d) / lognormalVol : NORMAL.getPDF(0d) * rootT;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		  d2 = d1 - sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d1);
		return nVal == 0d ? 0d : -nVal * d2 / lognormalVol;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless dual vanna.
	  /// <para>
	  /// This is the second order derivative of the option value, once to the strike and
	  /// once to volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless dual vanna </returns>
	  public static double dualVanna(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("log(1)/0 ambiguous");
		  return lognormalVol < SMALL ? -NORMAL.getPDF(0d) / lognormalVol : -NORMAL.getPDF(0d) * rootT;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		  d2 = d1 - sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d2);
		return nVal == 0d ? 0d : nVal * d1 / lognormalVol;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless vomma (aka volga).
	  /// <para>
	  /// This is the second order derivative of the option forward price with respect
	  /// to the implied volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless vomma </returns>
	  public static double vomma(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  log.info("lognormalVol * Math.sqrt(timeToExpiry) ambiguous");
		  sigmaRootT = 1d;
		}

		bool bFwd = (forward > LARGE);
		bool bStr = (strike > LARGE);
		bool bSigRt = (sigmaRootT > LARGE);
		double d1 = 0d;
		double d2 = 0d;

		if (bSigRt)
		{
		  return 0d;
		}
		if (sigmaRootT < SMALL)
		{
		  if (Math.Abs(forward - strike) >= SMALL && !(bFwd && bStr))
		  {
			return 0d;
		  }
		  log.info("log(1)/0 ambiguous");
		  if (bFwd)
		  {
			return rootT < SMALL ? NORMAL.getPDF(0d) / lognormalVol : forward * NORMAL.getPDF(0d) * rootT / lognormalVol;
		  }
		  return lognormalVol < SMALL ? forward * NORMAL.getPDF(0d) * rootT / lognormalVol : -forward * NORMAL.getPDF(0d) * timeToExpiry * lognormalVol / 4.0;
		}
		if (Math.Abs(forward - strike) < SMALL | (bFwd && bStr))
		{
		  d1 = 0.5 * sigmaRootT;
		  d2 = -0.5 * sigmaRootT;
		}
		else
		{
		  d1 = Math.Log(forward / strike) / sigmaRootT + 0.5 * sigmaRootT;
		  d2 = d1 - sigmaRootT;
		}

		double nVal = NORMAL.getPDF(d1);
		double res = nVal == 0d ? 0d : forward * nVal * rootT * d1 * d2 / lognormalVol;
		return res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the driftless volga (aka vomma).
	  /// <para>
	  /// This is the second order derivative of the option forward price with respect
	  /// to the implied volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <returns> the driftless volga </returns>
	  public static double volga(double forward, double strike, double timeToExpiry, double lognormalVol)
	  {
		return vomma(forward, strike, timeToExpiry, lognormalVol);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the log-normal implied volatility.
	  /// </summary>
	  /// <param name="price"> The forward price, which is the market price divided by the numeraire,
	  ///   for example the zero bond p(0,T) for the T-forward measure </param>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> log-normal (Black) implied volatility </returns>
	  public static double impliedVolatility(double price, double forward, double strike, double timeToExpiry, bool isCall)
	  {

		ArgChecker.isTrue(price >= 0d, "negative/NaN price; have {}", price);
		ArgChecker.isTrue(forward > 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);

		ArgChecker.isFalse(double.IsInfinity(forward), "forward is Infinity");
		ArgChecker.isFalse(double.IsInfinity(strike), "strike is Infinity");
		ArgChecker.isFalse(double.IsInfinity(timeToExpiry), "timeToExpiry is Infinity");

		double intrinsicPrice = Math.Max(0.0, (isCall ? 1 : -1) * (forward - strike));

		double targetPrice = price - intrinsicPrice;
		// Math.max(0., price - intrinsicPrice) should not used for least chi square
		double sigmaGuess = 0.3;
		return impliedVolatility(targetPrice, forward, strike, timeToExpiry, sigmaGuess);
	  }

	  /// <summary>
	  /// Computes the log-normal implied volatility and its derivative with respect to price.
	  /// </summary>
	  /// <param name="price"> The forward price, which is the market price divided by the numeraire,
	  ///   for example the zero bond p(0,T) for the T-forward measure </param>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> log-normal (Black) implied volatility and tis derivative w.r.t. the price </returns>
	  public static ValueDerivatives impliedVolatilityAdjoint(double price, double forward, double strike, double timeToExpiry, bool isCall)
	  {

		ArgChecker.isTrue(price >= 0d, "negative/NaN price; have {}", price);
		ArgChecker.isTrue(forward > 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);

		ArgChecker.isFalse(double.IsInfinity(forward), "forward is Infinity");
		ArgChecker.isFalse(double.IsInfinity(strike), "strike is Infinity");
		ArgChecker.isFalse(double.IsInfinity(timeToExpiry), "timeToExpiry is Infinity");

		double intrinsicPrice = Math.Max(0.0, (isCall ? 1 : -1) * (forward - strike));

		double targetPrice = price - intrinsicPrice;
		// Math.max(0., price - intrinsicPrice) should not used for least chi square
		double sigmaGuess = 0.3;
		return impliedVolatilityAdjoint(targetPrice, forward, strike, timeToExpiry, sigmaGuess);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the log-normal (Black) implied volatility of an out-the-money
	  /// European option starting from an initial guess.
	  /// </summary>
	  /// <param name="otmPrice"> The forward price, which is the market price divided by the numeraire,
	  ///   for example the zero bond p(0,T) for the T-forward measure
	  ///   This MUST be an OTM price, i.e. a call price for strike >= forward and a put price otherwise.
	  /// </param>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="volGuess">  a guess of the implied volatility </param>
	  /// <returns> log-normal (Black) implied volatility </returns>
	  public static double impliedVolatility(double otmPrice, double forward, double strike, double timeToExpiry, double volGuess)
	  {

		ArgChecker.isTrue(otmPrice >= 0d, "negative/NaN otmPrice; have {}", otmPrice);
		ArgChecker.isTrue(forward >= 0d, "negative/NaN forward; have {}", forward);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(volGuess >= 0d, "negative/NaN volGuess; have {}", volGuess);

		ArgChecker.isFalse(double.IsInfinity(otmPrice), "otmPrice is Infinity");
		ArgChecker.isFalse(double.IsInfinity(forward), "forward is Infinity");
		ArgChecker.isFalse(double.IsInfinity(strike), "strike is Infinity");
		ArgChecker.isFalse(double.IsInfinity(timeToExpiry), "timeToExpiry is Infinity");
		ArgChecker.isFalse(double.IsInfinity(volGuess), "volGuess is Infinity");

		if (otmPrice == 0)
		{
		  return 0;
		}
		ArgChecker.isTrue(otmPrice < Math.Min(forward, strike), "otmPrice of {} exceeded upper bound of {}", otmPrice, Math.Min(forward, strike));

		if (forward == strike)
		{
		  return NORMAL.getInverseCDF(0.5 * (otmPrice / forward + 1)) * 2 / Math.Sqrt(timeToExpiry);
		}

		bool isCall = strike >= forward;

		System.Func<double, double> priceFunc = (double? x) =>
		{
	return price(forward, strike, timeToExpiry, x.Value, isCall);
		};

		System.Func<double, double> vegaFunc = (double? x) =>
		{
	return vega(forward, strike, timeToExpiry, x.Value);
		};

		GenericImpliedVolatiltySolver solver = new GenericImpliedVolatiltySolver(priceFunc, vegaFunc);
		return solver.impliedVolatility(otmPrice, volGuess);
	  }

	  /// <summary>
	  /// Computes the log-normal (Black) implied volatility of an out-the-money European option starting 
	  /// from an initial guess and the derivative of the volatility w.r.t. the price.
	  /// </summary>
	  /// <param name="otmPrice"> The forward price, which is the market price divided by the numeraire,
	  ///   for example the zero bond p(0,T) for the T-forward measure
	  ///   This MUST be an OTM price, i.e. a call price for strike >= forward and a put price otherwise.
	  /// </param>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="volGuess">  a guess of the implied volatility </param>
	  /// <returns> log-normal (Black) implied volatility and derivative with respect to the price </returns>
	  public static ValueDerivatives impliedVolatilityAdjoint(double otmPrice, double forward, double strike, double timeToExpiry, double volGuess)
	  {
		double impliedVolatility = BlackFormulaRepository.impliedVolatility(otmPrice, forward, strike, timeToExpiry, volGuess);
		bool isCall = strike >= forward;
		ValueDerivatives price = priceAdjoint(forward, strike, timeToExpiry, impliedVolatility, isCall);
		double dpricedvol = price.getDerivative(3);
		double dvoldprice = 1.0d / dpricedvol;
		return ValueDerivatives.of(impliedVolatility, DoubleArray.of(dvoldprice));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied strike from delta and volatility in the Black formula.
	  /// </summary>
	  /// <param name="delta"> The option delta </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <param name="forward"> The forward. </param>
	  /// <param name="time"> The time to expiry. </param>
	  /// <param name="volatility"> The volatility. </param>
	  /// <returns> the strike. </returns>
	  public static double impliedStrike(double delta, bool isCall, double forward, double time, double volatility)
	  {
		ArgChecker.isTrue(delta > -1 && delta < 1, "Delta out of range");
		ArgChecker.isTrue(isCall ^ (delta < 0), "Delta incompatible with call/put: {}, {}", isCall, delta);
		ArgChecker.isTrue(forward > 0, "Forward negative");
		double omega = (isCall ? 1d : -1d);
		double strike = forward * Math.Exp(-volatility * Math.Sqrt(time) * omega * NORMAL.getInverseCDF(omega * delta) + volatility * volatility * time / 2);
		return strike;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied strike and its derivatives from delta and volatility in the Black formula.
	  /// </summary>
	  /// <param name="delta"> The option delta </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <param name="forward">  the forward </param>
	  /// <param name="time">  the time to expiry </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <param name="derivatives">  the mutated array of derivatives of the implied strike with respect to the input
	  ///   Derivatives with respect to: [0] delta, [1] forward, [2] time, [3] volatility. </param>
	  /// <returns> the strike </returns>
	  public static double impliedStrike(double delta, bool isCall, double forward, double time, double volatility, double[] derivatives)
	  {

		ArgChecker.isTrue(delta > -1 && delta < 1, "Delta out of range");
		ArgChecker.isTrue(isCall ^ (delta < 0), "Delta incompatible with call/put: {}, {}", isCall, delta);
		ArgChecker.isTrue(forward > 0, "Forward negative");
		double omega = (isCall ? 1d : -1d);
		double sqrtt = Math.Sqrt(time);
		double n = NORMAL.getInverseCDF(omega * delta);
		double part1 = Math.Exp(-volatility * sqrtt * omega * n + volatility * volatility * time / 2);
		double strike = forward * part1;
		// Backward sweep
		double strikeBar = 1d;
		double part1Bar = forward * strikeBar;
		double nBar = part1 * -volatility * Math.Sqrt(time) * omega * part1Bar;
		derivatives[0] = omega / NORMAL.getPDF(n) * nBar;
		derivatives[1] = part1 * strikeBar;
		derivatives[2] = part1 * (-volatility * omega * n * 0.5 / sqrtt + volatility * volatility / 2) * part1Bar;
		derivatives[3] = part1 * (-sqrtt * omega * n + volatility * time) * part1Bar;
		return strike;
	  }

	  /// <summary>
	  /// Compute the log-normal implied volatility from a normal volatility using an approximate initial guess and a root-finder.
	  /// <para>
	  /// The forward and the strike must be positive.
	  /// </para>
	  /// <para>
	  /// Reference: Hagan, P. S. Volatility conversion calculator. Technical report, Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate/price </param>
	  /// <param name="strike">  the option strike </param>
	  /// <param name="timeToExpiry">  the option time to expiration </param>
	  /// <param name="normalVolatility">  the normal implied volatility </param>
	  /// <returns> the Black implied volatility </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static double impliedVolatilityFromNormalApproximated(final double forward, final double strike, final double timeToExpiry, final double normalVolatility)
	  public static double impliedVolatilityFromNormalApproximated(double forward, double strike, double timeToExpiry, double normalVolatility)
	  {
		ArgChecker.isTrue(strike > 0, "strike must be strictly positive");
		ArgChecker.isTrue(forward > 0, "strike must be strictly positive");
		// initial guess
		double guess = impliedVolatilityFromNormalApproximated2(forward, strike, timeToExpiry, normalVolatility);
		// Newton-Raphson method
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = new java.util.function.Function<double, double>()
		System.Func<double, double> func = (double? volatility) =>
		{
	return NormalFormulaRepository.impliedVolatilityFromBlackApproximated(forward, strike, timeToExpiry, volatility.Value) - normalVolatility;
		};
		return ROOT_FINDER.getRoot(func, guess).Value;
	  }

	  /// <summary>
	  /// Compute the log-normal implied volatility from a normal volatility using an approximate initial guess and a 
	  /// root-finder and compute the derivative of the log-normal volatility with respect to the input normal volatility.
	  /// <para>
	  /// The forward and the strike must be positive.
	  /// </para>
	  /// <para>
	  /// Reference: Hagan, P. S. Volatility conversion calculator. Technical report, Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate/price </param>
	  /// <param name="strike">  the option strike </param>
	  /// <param name="timeToExpiry">  the option time to expiration </param>
	  /// <param name="normalVolatility">  the normal implied volatility </param>
	  /// <returns> the Black implied volatility and its derivative </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public static com.opengamma.strata.basics.value.ValueDerivatives impliedVolatilityFromNormalApproximatedAdjoint(final double forward, final double strike, final double timeToExpiry, final double normalVolatility)
	  public static ValueDerivatives impliedVolatilityFromNormalApproximatedAdjoint(double forward, double strike, double timeToExpiry, double normalVolatility)
	  {
		ArgChecker.isTrue(strike > 0, "strike must be strictly positive");
		ArgChecker.isTrue(forward > 0, "strike must be strictly positive");
		// initial guess
		double guess = impliedVolatilityFromNormalApproximated2(forward, strike, timeToExpiry, normalVolatility);
		// Newton-Raphson method
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> func = new java.util.function.Function<double, double>()
		System.Func<double, double> func = (double? volatility) =>
		{
	return NormalFormulaRepository.impliedVolatilityFromBlackApproximated(forward, strike, timeToExpiry, volatility.Value) - normalVolatility;
		};
		double impliedVolatilityBlack = ROOT_FINDER.getRoot(func, guess).Value;
		double derivativeInverse = NormalFormulaRepository.impliedVolatilityFromBlackApproximatedAdjoint(forward, strike, timeToExpiry, impliedVolatilityBlack).getDerivative(0);
		double derivative = 1.0 / derivativeInverse;
		return ValueDerivatives.of(impliedVolatilityBlack, DoubleArray.of(derivative));
	  }

	  /// <summary>
	  /// Compute the normal implied volatility from a normal volatility using an approximate explicit formula.
	  /// <para>
	  /// The formula is usually not good enough to be used as such, but provide a good initial guess for a 
	  /// root-finding procedure. Use <seealso cref="BlackFormulaRepository#impliedVolatilityFromNormalApproximated"/> for
	  /// more precision.
	  /// </para>
	  /// <para>
	  /// The forward and the strike must be positive.
	  /// </para>
	  /// <para>
	  /// Reference: Hagan, P. S. Volatility conversion calculator. Technical report, Bloomberg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate/price </param>
	  /// <param name="strike">  the option strike </param>
	  /// <param name="timeToExpiry">  the option time to expiration </param>
	  /// <param name="normalVolatility">  the normal implied volatility </param>
	  /// <returns> the Black implied volatility </returns>
	  public static double impliedVolatilityFromNormalApproximated2(double forward, double strike, double timeToExpiry, double normalVolatility)
	  {
		ArgChecker.isTrue(strike > 0, "strike must be strctly positive");
		ArgChecker.isTrue(forward > 0, "strike must be strctly positive");
		double lnFK = Math.Log(forward / strike);
		double s2t = normalVolatility * normalVolatility * timeToExpiry;
		if (Math.Abs((forward - strike) / strike) < ATM_LIMIT)
		{
		  double factor1 = 1.0d / Math.Sqrt(forward * strike);
		  double factor2 = (1.0d + s2t / (24.0d * forward * strike)) / (1.0d + lnFK * lnFK / 24.0d);
		  return normalVolatility * factor1 * factor2;
		}
		double factor1 = lnFK / (forward - strike);
		double factor2 = (1.0d + (1.0d - lnFK * lnFK / 120.0d) * s2t / (24.0d * forward * strike));
		return normalVolatility * factor1 * factor2;
	  }

	}

}