using System;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;

	/// <summary>
	/// The primary repository for Black-Scholes formulas, including the price and greeks.
	/// <para>
	/// When the formula involves ambiguous quantities, a reference value (rather than NaN) is returned 
	/// Note that the formulas are expressed in terms of interest rate (r) and cost of carry (b),
	/// then d_1 and d_2 are d_{1,2} = \frac{\ln(S/X) + (b \pm \sigma^2 ) T}{\sigma \sqrt{T}}.
	/// </para>
	/// </summary>
	public sealed class BlackScholesFormulaRepository
	{

	  private static readonly ProbabilityDistribution<double> NORMAL = new NormalDistribution(0, 1);
	  private const double SMALL = 1e-13;
	  private const double LARGE = 1e13;

	  // restricted constructor
	  private BlackScholesFormulaRepository()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the spot price.
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the spot price </returns>
	  public static double price(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		if (interestRate > LARGE)
		{
		  return 0d;
		}
		if (-interestRate > LARGE)
		{
		  return double.PositiveInfinity;
		}
		double discount = Math.Abs(interestRate) < SMALL ? 1d : Math.Exp(-interestRate * timeToExpiry);

		if (costOfCarry > LARGE)
		{
		  return isCall ? double.PositiveInfinity : 0d;
		}
		if (-costOfCarry > LARGE)
		{
		  double res = isCall ? 0d : (discount > SMALL ? strike * discount : 0d);
		  return double.IsNaN(res) ? discount : res;
		}
		double factor = Math.Exp(costOfCarry * timeToExpiry);

		if (spot > LARGE * strike)
		{
		  double tmp = Math.Exp((costOfCarry - interestRate) * timeToExpiry);
		  return isCall ? (tmp > SMALL ? spot * tmp : 0d) : 0d;
		}
		if (LARGE * spot < strike)
		{
		  return (isCall || discount < SMALL) ? 0d : strike * discount;
		}
		if (spot > LARGE && strike > LARGE)
		{
		  double tmp = Math.Exp((costOfCarry - interestRate) * timeToExpiry);
		  return isCall ? (tmp > SMALL ? spot * tmp : 0d) : (discount > SMALL ? strike * discount : 0d);
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		int sign = isCall ? 1 : -1;
		double rescaledSpot = factor * spot;
		if (sigmaRootT < SMALL)
		{
		  double res = isCall ? (rescaledSpot > strike ? discount * (rescaledSpot - strike) : 0d) : (rescaledSpot < strike ? discount * (strike - rescaledSpot) : 0d);
		  return double.IsNaN(res) ? sign * (spot - discount * strike) : res;
		}

		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE)
		{
		  double coefD1 = (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double coefD2 = (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD1 = coefD1 * rootT;
		  double tmpD2 = coefD2 * rootT;
		  d1 = double.IsNaN(tmpD1) ? 0d : tmpD1;
		  d2 = double.IsNaN(tmpD2) ? 0d : tmpD2;
		}
		else
		{
		  double tmp = costOfCarry * rootT / lognormalVol;
		  double sig = (costOfCarry >= 0d) ? 1d : -1d;
		  double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
		  d1 = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
		  d2 = d1 - sigmaRootT;
		}
		double res = sign * discount * (rescaledSpot * NORMAL.getCDF(sign * d1) - strike * NORMAL.getCDF(sign * d2));
		return double.IsNaN(res) ? 0d : Math.Max(res, 0d);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the spot delta.
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the spot delta </returns>
	  public static double delta(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return isCall ? double.PositiveInfinity : (costOfCarry > LARGE ? 0d : double.NegativeInfinity);
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		if (spot > LARGE * strike)
		{
		  return isCall ? coef : 0d;
		}
		if (spot < SMALL * strike)
		{
		  return isCall ? 0d : -coef;
		}

		int sign = isCall ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d; //ref value is returned
		}
		double rescaledSpot = spot * factor;

		double d1 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d1 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			return isCall ? (rescaledSpot > strike ? coef : 0d) : (rescaledSpot < strike ? -coef : 0d);
		  }
		  double tmp = costOfCarry * rootT / lognormalVol;
		  double sig = (costOfCarry >= 0d) ? 1d : -1d;
		  double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
		  d1 = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
		}
		double norm = NORMAL.getCDF(sign * d1);

		return norm < SMALL ? 0d : sign * coef * norm;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the strike for the delta.
	  /// <para>
	  /// Note that the parameter range is more restricted for this method because the
	  /// strike is undetermined for infinite/zero valued parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="spotDelta"> The spot delta </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the strike </returns>
	  public static double strikeForDelta(double spot, double spotDelta, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot > 0d, "non-positive/NaN spot; have {}", spot);
		ArgChecker.isTrue(timeToExpiry > 0d, "non-positive/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol > 0d, "non-positive/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		ArgChecker.isFalse(double.IsInfinity(spot), "spot is infinite");
		ArgChecker.isFalse(double.IsInfinity(spotDelta), "spotDelta is infinite");
		ArgChecker.isFalse(double.IsInfinity(timeToExpiry), "timeToExpiry is infinite");
		ArgChecker.isFalse(double.IsInfinity(lognormalVol), "lognormalVol is infinite");
		ArgChecker.isFalse(double.IsInfinity(interestRate), "interestRate is infinite");
		ArgChecker.isFalse(double.IsInfinity(costOfCarry), "costOfCarry is infinite");

		double rescaledDelta = spotDelta * Math.Exp((-costOfCarry + interestRate) * timeToExpiry);
		ArgChecker.isTrue((isCall && rescaledDelta > 0d && rescaledDelta < 1.0) || (!isCall && spotDelta < 0d && rescaledDelta > -1.0), "delta/Math.exp((costOfCarry - interestRate) * timeToExpiry) out of range, ", rescaledDelta);

		double sigmaRootT = lognormalVol * Math.Sqrt(timeToExpiry);
		double rescaledSpot = spot * Math.Exp(costOfCarry * timeToExpiry);

		int sign = isCall ? 1 : -1;
		double d1 = sign * NORMAL.getInverseCDF(sign * rescaledDelta);
		return rescaledSpot * Math.Exp(-d1 * sigmaRootT + 0.5 * sigmaRootT * sigmaRootT);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the dual delta.
	  /// <para>
	  /// This is the first derivative of option price with respect to strike.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the dual delta </returns>
	  public static double dualDelta(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double discount = 0d;
		if (-interestRate > LARGE)
		{
		  return isCall ? double.NegativeInfinity : (costOfCarry > LARGE ? 0d : double.PositiveInfinity);
		}
		if (interestRate > LARGE)
		{
		  return 0d;
		}
		discount = (Math.Abs(interestRate) < SMALL && timeToExpiry > LARGE) ? 1d : Math.Exp(-interestRate * timeToExpiry);

		if (spot > LARGE * strike)
		{
		  return isCall ? -discount : 0d;
		}
		if (spot < SMALL * strike)
		{
		  return isCall ? 0d : discount;
		}

		int sign = isCall ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d; //ref value is returned
		}
		double rescaledSpot = spot * factor;

		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE || (spot > LARGE && strike > LARGE))
		{
		  double coefD2 = (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmp = coefD2 * rootT;
		  d2 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			return isCall ? (rescaledSpot > strike ? -discount : 0d) : (rescaledSpot < strike ? discount : 0d);
		  }
		  double tmp = costOfCarry * rootT / lognormalVol;
		  double sig = (costOfCarry >= 0d) ? 1d : -1d;
		  double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
		  d2 = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
		}
		double norm = NORMAL.getCDF(sign * d2);

		return norm < SMALL ? 0d : -sign * discount * norm;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the spot gamma.
	  /// <para>
	  /// This is the second order sensitivity of the spot option value to the spot.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 FV}{\partial^2 f}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot gamma </returns>
	  public static double gamma(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return costOfCarry > LARGE ? 0d : double.PositiveInfinity;
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}
		if (spot > LARGE * strike || spot < SMALL * strike || sigmaRootT > LARGE)
		{
		  return 0d;
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d; //ref value is returned
		}

		double d1 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d1 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			d1 = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
		  }
		}
		double norm = NORMAL.getPDF(d1);

		double res = norm < SMALL ? 0d : coef * norm / spot / sigmaRootT;
		return double.IsNaN(res) ? double.PositiveInfinity : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the dual gamma.
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the dual gamma </returns>
	  public static double dualGamma(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		if (-interestRate > LARGE)
		{
		  return costOfCarry > LARGE ? 0d : double.PositiveInfinity;
		}
		if (interestRate > LARGE)
		{
		  return 0d;
		}
		double discount = (Math.Abs(interestRate) < SMALL && timeToExpiry > LARGE) ? 1d : Math.Exp(-interestRate * timeToExpiry);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}
		if (spot > LARGE * strike || spot < SMALL * strike || sigmaRootT > LARGE)
		{
		  return 0d;
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d;
		}

		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d2 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d2 = double.IsNaN(tmp) ? 0d : tmp;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			d2 = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
		  }
		}
		double norm = NORMAL.getPDF(d2);

		double res = norm < SMALL ? 0d : discount * norm / strike / sigmaRootT;
		return double.IsNaN(res) ? double.PositiveInfinity : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the cross gamma.
	  /// <para>
	  /// This is the sensitivity of the delta to the strike.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 V}{\partial f \partial K}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the cross gamma </returns>
	  public static double crossGamma(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {
		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		if (-interestRate > LARGE)
		{
		  return costOfCarry > LARGE ? 0d : double.NegativeInfinity;
		}
		if (interestRate > LARGE)
		{
		  return 0d;
		}
		double discount = (Math.Abs(interestRate) < SMALL && timeToExpiry > LARGE) ? 1d : Math.Exp(-interestRate * timeToExpiry);

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}
		if (spot > LARGE * strike || spot < SMALL * strike || sigmaRootT > LARGE)
		{
		  return 0d;
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d; //ref value is returned
		}

		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d2 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d2 = double.IsNaN(tmp) ? 0d : tmp;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			d2 = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
		  }
		}
		double norm = NORMAL.getPDF(d2);

		double res = norm < SMALL ? 0d : -discount * norm / spot / sigmaRootT;
		return double.IsNaN(res) ? double.NegativeInfinity : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the theta.
	  /// <para>
	  /// This is the sensitivity of the present value to a change in time to maturity.
	  /// </para>
	  /// <para>
	  /// $\-frac{\partial V}{\partial T}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> theta </returns>
	  public static double theta(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		if (Math.Abs(interestRate) > LARGE)
		{
		  return 0d;
		}
		double discount = (Math.Abs(interestRate) < SMALL && timeToExpiry > LARGE) ? 1d : Math.Exp(-interestRate * timeToExpiry);

		if (costOfCarry > LARGE)
		{
		  return isCall ? double.NegativeInfinity : 0d;
		}
		if (-costOfCarry > LARGE)
		{
		  double res = isCall ? 0d : (discount > SMALL ? strike * discount * interestRate : 0d);
		  return double.IsNaN(res) ? discount : res;
		}

		if (spot > LARGE * strike)
		{
		  double tmp = Math.Exp((costOfCarry - interestRate) * timeToExpiry);
		  double res = isCall ? (tmp > SMALL ? -(costOfCarry - interestRate) * spot * tmp : 0d) : 0d;
		  return double.IsNaN(res) ? tmp : res;
		}
		if (LARGE * spot < strike)
		{
		  double res = isCall ? 0d : (discount > SMALL ? strike * discount * interestRate : 0d);
		  return double.IsNaN(res) ? discount : res;
		}
		if (spot > LARGE && strike > LARGE)
		{
		  return double.PositiveInfinity;
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		int sign = isCall ? 1 : -1;
		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE)
		{
		  double coefD1 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmpD1 = Math.Abs(coefD1) < SMALL ? 0d : coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? Math.Sign(coefD1) : tmpD1;
		  double coefD2 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD2 = Math.Abs(coefD2) < SMALL ? 0d : coefD2 * rootT;
		  d2 = double.IsNaN(tmpD2) ? Math.Sign(coefD2) : tmpD2;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			d1 = (Math.Log(spot / strike) / rootT + costOfCarry * rootT) / lognormalVol;
			d2 = d1;
		  }
		  else
		  {
			double tmp = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? rootT : ((Math.Abs(costOfCarry) < SMALL && rootT > LARGE) ? 1d / lognormalVol : costOfCarry / lognormalVol * rootT);
			d1 = Math.Log(spot / strike) / sigmaRootT + tmp + 0.5 * sigmaRootT;
			d2 = d1 - sigmaRootT;
		  }
		}
		double norm = NORMAL.getPDF(d1);
		double rescaledSpot = Math.Exp((costOfCarry - interestRate) * timeToExpiry) * spot;
		double rescaledStrike = discount * strike;
		double normForSpot = NORMAL.getCDF(sign * d1);
		double normForStrike = NORMAL.getCDF(sign * d2);
		double spotTerm = normForSpot < SMALL ? 0d : (double.IsNaN(rescaledSpot) ? -sign * Math.Sign((costOfCarry - interestRate)) * rescaledSpot : -sign * ((costOfCarry - interestRate) * rescaledSpot * normForSpot));
		double strikeTerm = normForStrike < SMALL ? 0d : (double.IsNaN(rescaledSpot) ? sign * (-Math.Sign(interestRate) * discount) : sign * (-interestRate * rescaledStrike * normForStrike));

		double coef = rescaledSpot * lognormalVol / rootT;
		if (double.IsNaN(coef))
		{
		  coef = 1d; //ref value is returned
		}
		double dlTerm = norm < SMALL ? 0d : -0.5 * norm * coef;

		double res = dlTerm + spotTerm + strikeTerm;
		return double.IsNaN(res) ? 0d : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the charm.
	  /// <para>
	  /// This is the minus of second order derivative of option value, once spot and once time to maturity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  The interest rate </param>
	  /// <param name="costOfCarry">  The cost of carry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the charm </returns>
	  public static double charm(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double coeff = Math.Exp((costOfCarry - interestRate) * timeToExpiry);
		if (coeff < SMALL)
		{
		  return 0d;
		}
		if (double.IsNaN(coeff))
		{
		  coeff = 1d; //ref value is returned
		}

		int sign = isCall ? 1 : -1;
		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || sigmaRootT > LARGE)
		{
		  double coefD1 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmpD1 = Math.Abs(coefD1) < SMALL ? 0d : coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? Math.Sign(coefD1) : tmpD1;
		  double coefD2 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD2 = Math.Abs(coefD2) < SMALL ? 0d : coefD2 * rootT;
		  d2 = double.IsNaN(tmpD2) ? Math.Sign(coefD2) : tmpD2;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
			d2 = d1;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			double d1Tmp = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
			double d2Tmp = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
			d1 = double.IsNaN(d1Tmp) ? 0d : d1Tmp;
			d2 = double.IsNaN(d2Tmp) ? 0d : d2Tmp;
		  }
		}
		double cocMod = costOfCarry / sigmaRootT;
		if (double.IsNaN(cocMod))
		{
		  cocMod = 1d; //ref value is returned
		}

		double tmp = d2 / timeToExpiry;
		tmp = double.IsNaN(tmp) ? (d2 >= 0d ? 1d : -1.0) : tmp;
		double coefPdf = cocMod - 0.5 * tmp;

		double normPdf = NORMAL.getPDF(d1);
		double normCdf = NORMAL.getCDF(sign * d1);
		double first = normPdf < SMALL ? 0d : (double.IsNaN(coefPdf) ? 0d : normPdf * coefPdf);
		double second = normCdf < SMALL ? 0d : (costOfCarry - interestRate) * normCdf;
		double res = -coeff * (first + sign * second);

		return double.IsNaN(res) ? 0d : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the dual charm.
	  /// <para>
	  /// This is the minus of second order derivative of option value, once strike and once time to maturity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost of carry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the dual charm </returns>
	  public static double dualCharm(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double discount = Math.Exp(-interestRate * timeToExpiry);
		if (discount < SMALL)
		{
		  return 0d;
		}
		if (double.IsNaN(discount))
		{
		  discount = 1d; //ref value is returned
		}

		int sign = isCall ? 1 : -1;
		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || sigmaRootT > LARGE)
		{
		  double coefD1 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmpD1 = Math.Abs(coefD1) < SMALL ? 0d : coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? Math.Sign(coefD1) : tmpD1;
		  double coefD2 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD2 = Math.Abs(coefD2) < SMALL ? 0d : coefD2 * rootT;
		  d2 = double.IsNaN(tmpD2) ? Math.Sign(coefD2) : tmpD2;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
			d2 = d1;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			double d1Tmp = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
			double d2Tmp = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
			d1 = double.IsNaN(d1Tmp) ? 0d : d1Tmp;
			d2 = double.IsNaN(d2Tmp) ? 0d : d2Tmp;
		  }
		}
		double coefPdf = 0d;
		if (timeToExpiry < SMALL)
		{
		  coefPdf = (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE)) ? 1d / sigmaRootT : Math.Log(spot / strike) / sigmaRootT / timeToExpiry;
		}
		else
		{
		  double cocMod = costOfCarry / sigmaRootT;
		  if (double.IsNaN(cocMod))
		  {
			cocMod = 1d;
		  }
		  double tmp = d1 / timeToExpiry;
		  tmp = double.IsNaN(tmp) ? (d1 >= 0d ? 1d : -1.0) : tmp;
		  coefPdf = cocMod - 0.5 * tmp;
		}

		double normPdf = NORMAL.getPDF(d2);
		double normCdf = NORMAL.getCDF(sign * d2);
		double first = normPdf < SMALL ? 0d : (double.IsNaN(coefPdf) ? 0d : normPdf * coefPdf);
		double second = normCdf < SMALL ? 0d : interestRate * normCdf;
		double res = discount * (first - sign * second);

		return double.IsNaN(res) ? 0d : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the spot vega.
	  /// <para>
	  /// This is the sensitivity of the option's spot price wrt the implied volatility
	  /// (which is just the spot vega divided by the numeraire).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot vega </returns>
	  public static double vega(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return costOfCarry > LARGE ? 0d : double.PositiveInfinity;
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double factor = Math.Exp(costOfCarry * timeToExpiry);
		if (double.IsNaN(factor))
		{
		  factor = 1d; //ref value is returned
		}

		double d1 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || sigmaRootT > LARGE)
		{
		  double coefD1 = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d1 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL || spot > LARGE * strike || strike > LARGE * spot)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			d1 = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
		  }
		}
		double norm = NORMAL.getPDF(d1);

		double res = norm < SMALL ? 0d : coef * norm * spot * rootT;
		return double.IsNaN(res) ? double.PositiveInfinity : res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the vanna.
	  /// <para>
	  /// This is the second order derivative of the option value, once to the underlying spot and once to volatility.
	  /// </para>
	  /// <para>
	  /// $\frac{\partial^2 FV}{\partial f \partial \sigma}$.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot vanna </returns>
	  public static double vanna(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || sigmaRootT > LARGE)
		{
		  double coefD1 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmpD1 = Math.Abs(coefD1) < SMALL ? 0d : coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? Math.Sign(coefD1) : tmpD1;
		  double coefD2 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD2 = Math.Abs(coefD2) < SMALL ? 0d : coefD2 * rootT;
		  d2 = double.IsNaN(tmpD2) ? Math.Sign(coefD2) : tmpD2;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
			d2 = d1;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			double d1Tmp = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
			double d2Tmp = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
			d1 = double.IsNaN(d1Tmp) ? 0d : d1Tmp;
			d2 = double.IsNaN(d2Tmp) ? 0d : d2Tmp;
		  }
		}

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return costOfCarry > LARGE ? 0d : (d2 >= 0d ? double.NegativeInfinity : double.PositiveInfinity);
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		double norm = NORMAL.getPDF(d1);
		double tmp = d2 * coef / lognormalVol;
		if (double.IsNaN(tmp))
		{
		  tmp = coef;
		}
		return norm < SMALL ? 0d : -norm * tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the dual vanna.
	  /// <para>
	  /// This is the second order derivative of the option value, once to the strike and once to volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot dual vanna </returns>
	  public static double dualVanna(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		double d1 = 0d;
		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || sigmaRootT > LARGE)
		{
		  double coefD1 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) + 0.5 * lognormalVol : (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmpD1 = Math.Abs(coefD1) < SMALL ? 0d : coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? Math.Sign(coefD1) : tmpD1;
		  double coefD2 = double.IsNaN(Math.Abs(costOfCarry) / lognormalVol) ? Math.Sign(costOfCarry) - 0.5 * lognormalVol : (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmpD2 = Math.Abs(coefD2) < SMALL ? 0d : coefD2 * rootT;
		  d2 = double.IsNaN(tmpD2) ? Math.Sign(coefD2) : tmpD2;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			double scnd = (Math.Abs(costOfCarry) > LARGE && rootT < SMALL) ? Math.Sign(costOfCarry) : costOfCarry * rootT;
			double tmp = (Math.Log(spot / strike) / rootT + scnd) / lognormalVol;
			d1 = double.IsNaN(tmp) ? 0d : tmp;
			d2 = d1;
		  }
		  else
		  {
			double tmp = costOfCarry * rootT / lognormalVol;
			double sig = (costOfCarry >= 0d) ? 1d : -1d;
			double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
			double d1Tmp = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
			double d2Tmp = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
			d1 = double.IsNaN(d1Tmp) ? 0d : d1Tmp;
			d2 = double.IsNaN(d2Tmp) ? 0d : d2Tmp;
		  }
		}

		double coef = Math.Exp(-interestRate * timeToExpiry);
		if (coef < SMALL)
		{
		  return 0d;
		}
		if (double.IsNaN(coef))
		{
		  coef = 1d; //ref value is returned
		}

		double norm = NORMAL.getPDF(d2);
		double tmp = d1 * coef / lognormalVol;
		if (double.IsNaN(tmp))
		{
		  tmp = coef;
		}

		return norm < SMALL ? 0d : norm * tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the vomma (aka volga).
	  /// <para>
	  /// This is the second order derivative of the option spot price with respect to the implied volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot vomma </returns>
	  public static double vomma(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}

		if (spot > LARGE * strike || strike > LARGE * spot || rootT < SMALL)
		{
		  return 0d;
		}

		double d1 = 0d;
		double d1d2Mod = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || rootT > LARGE)
		{
		  double costOvVol = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) : costOfCarry / lognormalVol;
		  double coefD1 = costOvVol + 0.5 * lognormalVol;
		  double coefD1D2Mod = costOvVol * costOvVol / lognormalVol - 0.25 * lognormalVol;
		  double tmpD1 = coefD1 * rootT;
		  double tmpD1d2Mod = coefD1D2Mod * rootT * timeToExpiry;
		  d1 = double.IsNaN(tmpD1) ? 0d : tmpD1;
		  d1d2Mod = double.IsNaN(tmpD1d2Mod) ? 1d : tmpD1d2Mod;
		}
		else
		{
		  if (lognormalVol > LARGE)
		  {
			d1 = 0.5 * sigmaRootT;
			d1d2Mod = -0.25 * sigmaRootT * timeToExpiry;
		  }
		  else
		  {
			if (lognormalVol < SMALL)
			{
			  double d1Tmp = (Math.Log(spot / strike) / rootT + costOfCarry * rootT) / lognormalVol;
			  d1 = double.IsNaN(d1Tmp) ? 1d : d1Tmp;
			  d1d2Mod = d1 * d1 * rootT / lognormalVol;
			}
			else
			{
			  double tmp = Math.Log(spot / strike) / sigmaRootT + costOfCarry * rootT / lognormalVol;
			  d1 = tmp + 0.5 * sigmaRootT;
			  d1d2Mod = (tmp * tmp - 0.25 * sigmaRootT * sigmaRootT) * rootT / lognormalVol;
			}
		  }
		}

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return costOfCarry > LARGE ? 0d : (d1d2Mod >= 0d ? double.PositiveInfinity : double.NegativeInfinity);
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		double norm = NORMAL.getPDF(d1);
		double tmp = d1d2Mod * spot * coef;
		if (double.IsNaN(tmp))
		{
		  tmp = coef;
		}

		return norm < SMALL ? 0d : norm * tmp;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the vega bleed.
	  /// <para>
	  /// This is the second order derivative of the option spot price, once to the volatility and once to the time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="costOfCarry">  the cost-of-carry rate </param>
	  /// <returns> the spot vomma </returns>
	  public static double vegaBleed(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		if (double.IsNaN(sigmaRootT))
		{
		  sigmaRootT = 1d; //ref value is returned
		}
		if (spot > LARGE * strike || strike > LARGE * spot || rootT < SMALL)
		{
		  return 0d;
		}

		double d1 = 0d;
		double extra = 0d;
		if (Math.Abs(spot - strike) < SMALL || (spot > LARGE && strike > LARGE) || rootT > LARGE)
		{
		  double costOvVol = (Math.Abs(costOfCarry) < SMALL && lognormalVol < SMALL) ? Math.Sign(costOfCarry) : costOfCarry / lognormalVol;
		  double coefD1 = costOvVol + 0.5 * lognormalVol;
		  double tmpD1 = coefD1 * rootT;
		  d1 = double.IsNaN(tmpD1) ? 0d : tmpD1;
		  double coefExtra = interestRate - 0.5 * costOfCarry + 0.5 * costOvVol * costOvVol + 0.125 * lognormalVol * lognormalVol;
		  double tmpExtra = double.IsNaN(coefExtra) ? rootT : coefExtra * rootT;
		  extra = double.IsNaN(tmpExtra) ? 1d - 0.5 / rootT : tmpExtra - 0.5 / rootT;
		}
		else
		{
		  if (lognormalVol > LARGE)
		  {
			d1 = 0.5 * sigmaRootT;
			extra = 0.125 * lognormalVol * sigmaRootT;
		  }
		  else
		  {
			if (lognormalVol < SMALL)
			{
			  double resLogRatio = Math.Log(spot / strike) / rootT;
			  double d1Tmp = (resLogRatio + costOfCarry * rootT) / lognormalVol;
			  d1 = double.IsNaN(d1Tmp) ? 1d : d1Tmp;
			  double tmpExtra = (-0.5 * resLogRatio * resLogRatio / rootT + 0.5 * costOfCarry * costOfCarry * rootT) / lognormalVol / lognormalVol;
			  extra = double.IsNaN(tmpExtra) ? 1d : extra;
			}
			else
			{
			  double resLogRatio = Math.Log(spot / strike) / sigmaRootT;
			  double tmp = resLogRatio + costOfCarry * rootT / lognormalVol;
			  d1 = tmp + 0.5 * sigmaRootT;
			  double pDivTmp = interestRate - 0.5 * costOfCarry * (1d - costOfCarry / lognormalVol / lognormalVol);
			  double pDiv = double.IsNaN(pDivTmp) ? rootT : pDivTmp * rootT;
			  extra = pDiv - 0.5 / rootT - 0.5 * resLogRatio * resLogRatio / rootT + 0.125 * lognormalVol * sigmaRootT;
			}
		  }
		}
		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return costOfCarry > LARGE ? 0d : (extra >= 0d ? double.PositiveInfinity : double.NegativeInfinity);
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		double norm = NORMAL.getPDF(d1);
		double tmp = spot * coef * extra;
		if (double.IsNaN(tmp))
		{
		  tmp = coef;
		}

		return norm < SMALL ? 0d : tmp * norm;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the rho.
	  /// <para>
	  /// This is the derivative of the option value with respect to the risk free interest rate .
	  /// Note that costOfCarry = interestRate - dividend, which the derivative also acts on.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  The interest rate </param>
	  /// <param name="costOfCarry">  the cost of carry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the rho </returns>
	  public static double rho(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double discount = 0d;
		if (-interestRate > LARGE)
		{
		  return isCall ? double.PositiveInfinity : double.NegativeInfinity;
		}
		if (interestRate > LARGE)
		{
		  return 0d;
		}
		discount = (Math.Abs(interestRate) < SMALL && timeToExpiry > LARGE) ? 1d : Math.Exp(-interestRate * timeToExpiry);

		if (LARGE * spot < strike || timeToExpiry > LARGE)
		{
		  double res = isCall ? 0d : -discount * strike * timeToExpiry;
		  return double.IsNaN(res) ? -discount : res;
		}
		if (spot > LARGE * strike || timeToExpiry < SMALL)
		{
		  double res = isCall ? discount * strike * timeToExpiry : 0d;
		  return double.IsNaN(res) ? discount : res;
		}

		int sign = isCall ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		double factor = Math.Exp(costOfCarry * timeToExpiry);
		double rescaledSpot = spot * factor;

		double d2 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (costOfCarry / lognormalVol - 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d2 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			return isCall ? (rescaledSpot > strike ? discount * strike * timeToExpiry : 0d) : (rescaledSpot < strike ? -discount * strike * timeToExpiry : 0d);
		  }
		  double tmp = costOfCarry * rootT / lognormalVol;
		  double sig = (costOfCarry >= 0d) ? 1d : -1d;
		  double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
		  d2 = Math.Log(spot / strike) / sigmaRootT + scnd - 0.5 * sigmaRootT;
		}
		double norm = NORMAL.getCDF(sign * d2);
		double result = norm < SMALL ? 0d : sign * discount * strike * timeToExpiry * norm;
		return double.IsNaN(result) ? sign * discount : result;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the carry rho.
	  /// <para>
	  /// This is the derivative of the option value with respect to the cost of carry .
	  /// Note that costOfCarry = interestRate - dividend, which the derivative also acts on.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot value of the underlying </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="lognormalVol">  the log-normal volatility </param>
	  /// <param name="interestRate">  The interest rate </param>
	  /// <param name="costOfCarry">  The cost of carry </param>
	  /// <param name="isCall">  true for call, false for put </param>
	  /// <returns> the carry rho </returns>
	  public static double carryRho(double spot, double strike, double timeToExpiry, double lognormalVol, double interestRate, double costOfCarry, bool isCall)
	  {

		ArgChecker.isTrue(spot >= 0d, "negative/NaN spot; have {}", spot);
		ArgChecker.isTrue(strike >= 0d, "negative/NaN strike; have {}", strike);
		ArgChecker.isTrue(timeToExpiry >= 0d, "negative/NaN timeToExpiry; have {}", timeToExpiry);
		ArgChecker.isTrue(lognormalVol >= 0d, "negative/NaN lognormalVol; have {}", lognormalVol);
		ArgChecker.isFalse(double.IsNaN(interestRate), "interestRate is NaN");
		ArgChecker.isFalse(double.IsNaN(costOfCarry), "costOfCarry is NaN");

		double coef = 0d;
		if ((interestRate > LARGE && costOfCarry > LARGE) || (-interestRate > LARGE && -costOfCarry > LARGE) || Math.Abs(costOfCarry - interestRate) < SMALL)
		{
		  coef = 1d; //ref value is returned
		}
		else
		{
		  double rate = costOfCarry - interestRate;
		  if (rate > LARGE)
		  {
			return isCall ? double.PositiveInfinity : (costOfCarry > LARGE ? 0d : double.NegativeInfinity);
		  }
		  if (-rate > LARGE)
		  {
			return 0d;
		  }
		  coef = Math.Exp(rate * timeToExpiry);
		}

		if (spot > LARGE * strike || timeToExpiry > LARGE)
		{
		  double res = isCall ? coef * spot * timeToExpiry : 0d;
		  return double.IsNaN(res) ? coef : res;
		}
		if (LARGE * spot < strike || timeToExpiry < SMALL)
		{
		  double res = isCall ? 0d : -coef * spot * timeToExpiry;
		  return double.IsNaN(res) ? -coef : res;
		}

		int sign = isCall ? 1 : -1;
		double rootT = Math.Sqrt(timeToExpiry);
		double sigmaRootT = lognormalVol * rootT;
		double factor = Math.Exp(costOfCarry * timeToExpiry);
		double rescaledSpot = spot * factor;

		double d1 = 0d;
		if (Math.Abs(spot - strike) < SMALL || sigmaRootT > LARGE || (spot > LARGE && strike > LARGE))
		{
		  double coefD1 = (costOfCarry / lognormalVol + 0.5 * lognormalVol);
		  double tmp = coefD1 * rootT;
		  d1 = double.IsNaN(tmp) ? 0d : tmp;
		}
		else
		{
		  if (sigmaRootT < SMALL)
		  {
			return isCall ? (rescaledSpot > strike ? coef * timeToExpiry * spot : 0d) : (rescaledSpot < strike ? -coef * timeToExpiry * spot : 0d);
		  }
		  double tmp = costOfCarry * rootT / lognormalVol;
		  double sig = (costOfCarry >= 0d) ? 1d : -1d;
		  double scnd = double.IsNaN(tmp) ? ((lognormalVol < LARGE && lognormalVol > SMALL) ? sig / lognormalVol : sig * rootT) : tmp;
		  d1 = Math.Log(spot / strike) / sigmaRootT + scnd + 0.5 * sigmaRootT;
		}
		double norm = NORMAL.getCDF(sign * d1);

		double result = norm < SMALL ? 0d : sign * coef * timeToExpiry * spot * norm;
		return double.IsNaN(result) ? sign * coef : result;
	  }

	}

}