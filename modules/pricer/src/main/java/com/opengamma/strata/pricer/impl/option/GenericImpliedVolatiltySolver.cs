using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{

	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MathException = com.opengamma.strata.math.MathException;
	using BisectionSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BisectionSingleRootFinder;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;

	/// <summary>
	/// Finds an implied volatility (a parameter that put into a model gives the market pirce of an option)
	/// for any option pricing model that has a 'volatility' parameter.
	/// This included the Black-Scholes-Merton model (and derivatives) for European options and
	/// Barone-Adesi & Whaley and Bjeksund and Stensland for American options.
	/// </summary>
	public class GenericImpliedVolatiltySolver
	{

	  private const int MAX_ITERATIONS = 20; // something's wrong if Newton-Raphson taking longer than this
	  private const double VOL_TOL = 1e-9; // 1 part in 100,000 basis points will do for implied vol
	  private const double VOL_GUESS = 0.3;
	  private const double BRACKET_STEP = 0.1;
	  private const double MAX_CHANGE = 0.5;

	  /// <summary>
	  /// The price function.
	  /// </summary>
	  private readonly System.Func<double, double> priceFunc;
	  /// <summary>
	  /// The combined price and vega function.
	  /// </summary>
	  private readonly System.Func<double, double[]> priceAndVegaFunc;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="priceAndVegaFunc">  the combined price and vega function </param>
	  public GenericImpliedVolatiltySolver(System.Func<double, double[]> priceAndVegaFunc)
	  {
		ArgChecker.notNull(priceAndVegaFunc, "priceAndVegaFunc");
		this.priceAndVegaFunc = priceAndVegaFunc;
		this.priceFunc = (double? sigma) =>
		{

	return priceAndVegaFunc(sigma)[0];
		};
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="priceFunc">  the pricing function </param>
	  /// <param name="vegaFunc">  the vega function </param>
	  public GenericImpliedVolatiltySolver(System.Func<double, double> priceFunc, System.Func<double, double> vegaFunc)
	  {
		ArgChecker.notNull(priceFunc, "priceFunc");
		ArgChecker.notNull(vegaFunc, "vegaFunc");
		this.priceFunc = priceFunc;
		this.priceAndVegaFunc = (double? sigma) =>
		{

	return new double[] {priceFunc(sigma), vegaFunc(sigma)};
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the implied volatility.
	  /// </summary>
	  /// <param name="optionPrice">  the option price </param>
	  /// <returns> the volatility </returns>
	  public virtual double impliedVolatility(double optionPrice)
	  {
		return impliedVolatility(optionPrice, VOL_GUESS);
	  }

	  /// <summary>
	  /// Computes the implied volatility.
	  /// </summary>
	  /// <param name="optionPrice">  the option price </param>
	  /// <param name="volGuess">  the initial guess </param>
	  /// <returns> the volatility </returns>
	  public virtual double impliedVolatility(double optionPrice, double volGuess)
	  {
		ArgChecker.isTrue(volGuess >= 0.0, "volGuess must be positive; have {}", volGuess);
		ArgChecker.isTrue(Doubles.isFinite(volGuess), "volGuess must be finite; have {} ", volGuess);

		double lowerSigma;
		double upperSigma;

		try
		{
		  double[] temp = bracketRoot(optionPrice, volGuess);
		  lowerSigma = temp[0];
		  upperSigma = temp[1];
		}
		catch (MathException e)
		{
		  throw new System.ArgumentException(e.ToString() + " No implied Volatility for this price. [price: " + optionPrice + "]");
		}
		double sigma = (lowerSigma + upperSigma) / 2.0;

		double[] pnv = priceAndVegaFunc.apply(sigma);

		// This can happen for American options,
		// where low volatilities puts you in the early excise region which obviously has zero vega
		if (pnv[1] == 0 || double.IsNaN(pnv[1]))
		{
		  return solveByBisection(optionPrice, lowerSigma, upperSigma);
		}
		double diff = pnv[0] - optionPrice;
		bool above = diff > 0;
		if (above)
		{
		  upperSigma = sigma;
		}
		else
		{
		  lowerSigma = sigma;
		}

		double trialChange = -diff / pnv[1];
		double actChange;
		if (trialChange > 0.0)
		{
		  actChange = Math.Min(MAX_CHANGE, Math.Min(trialChange, upperSigma - sigma));
		}
		else
		{
		  actChange = Math.Max(-MAX_CHANGE, Math.Max(trialChange, lowerSigma - sigma));
		}

		int count = 0;
		while (Math.Abs(actChange) > VOL_TOL)
		{
		  sigma += actChange;
		  pnv = priceAndVegaFunc.apply(sigma);

		  if (pnv[1] == 0 || double.IsNaN(pnv[1]))
		  {
			return solveByBisection(optionPrice, lowerSigma, upperSigma);
		  }

		  diff = pnv[0] - optionPrice;
		  above = diff > 0;
		  if (above)
		  {
			upperSigma = sigma;
		  }
		  else
		  {
			lowerSigma = sigma;
		  }

		  trialChange = -diff / pnv[1];
		  if (trialChange > 0.0)
		  {
			actChange = Math.Min(MAX_CHANGE, Math.Min(trialChange, upperSigma - sigma));
		  }
		  else
		  {
			actChange = Math.Max(-MAX_CHANGE, Math.Max(trialChange, lowerSigma - sigma));
		  }

		  if (count++ > MAX_ITERATIONS)
		  {
			return solveByBisection(optionPrice, lowerSigma, upperSigma);
		  }
		}
		return sigma + actChange; // apply the final change

	  }

	  //-------------------------------------------------------------------------
	  private double[] bracketRoot(double optionPrice, double sigma)
	  {
		BracketRoot bracketer = new BracketRoot();
		System.Func<double, double> func = (double? volatility) =>
		{
	return priceFunc.apply(volatility) / optionPrice - 1.0;
		};
		return bracketer.getBracketedPoints(func, Math.Max(0.0, sigma - BRACKET_STEP), sigma + BRACKET_STEP, 0d, double.PositiveInfinity);
	  }

	  private double solveByBisection(double optionPrice, double lowerSigma, double upperSigma)
	  {
		BisectionSingleRootFinder rootFinder = new BisectionSingleRootFinder(VOL_TOL);
		System.Func<double, double> func = (double? volatility) =>
		{

	double trialPrice = priceFunc.apply(volatility);
	return trialPrice / optionPrice - 1.0;
		};
		return rootFinder.getRoot(func, lowerSigma, upperSigma).Value;
	  }

	}

}