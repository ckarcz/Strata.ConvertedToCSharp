using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;
	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using DoubleMath = com.google.common.math.DoubleMath;
	using Doubles = com.google.common.primitives.Doubles;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;

	/// <summary>
	/// The Hagan SABR volatility function provider.
	/// <para>
	/// This class provides the functions of volatility and its sensitivity to the SABR model parameters based on the original
	/// Hagan SABR formula.
	/// </para>
	/// <para>
	/// Reference: Hagan, P.; Kumar, D.; Lesniewski, A. & Woodward, D. "Managing smile risk", Wilmott Magazine, 2002, September, 84-108
	/// </para>
	/// <para>
	/// OpenGamma documentation: SABR Implementation, OpenGamma documentation n. 33, April 2016.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class SabrHaganVolatilityFunctionProvider extends VolatilityFunctionProvider<SabrFormulaData> implements com.opengamma.strata.pricer.model.SabrVolatilityFormula, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SabrHaganVolatilityFunctionProvider : VolatilityFunctionProvider<SabrFormulaData>, SabrVolatilityFormula, ImmutableBean
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SabrHaganVolatilityFunctionProvider DEFAULT = new SabrHaganVolatilityFunctionProvider();

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(SabrHaganVolatilityFunctionProvider));

	  /* internal parameters */
	  private const double CUTOFF_MONEYNESS = 1e-12;
	  private const double SMALL_Z = 1e-6;
	  private const double LARGE_NEG_Z = -1e6;
	  private const double LARGE_POS_Z = 1e8;
	  private const double BETA_EPS = 1e-8;
	  private const double RHO_EPS = 1e-5;
	  private const double RHO_EPS_NEGATIVE = 1e-8;
	  private const double ATM_EPS = 1e-7;
	  private const double MIN_VOL = 1e-6; // Minimal volatility, to avoid negative volatility for extreme parameters

	  //-------------------------------------------------------------------------
	  public override double volatility(double forward, double strike, double timeToExpiry, SabrFormulaData data)
	  {
		ArgChecker.notNull(data, "data");
		double alpha = data.Alpha;
		double beta = data.Beta;
		double rho = data.Rho;
		double nu = data.Nu;
		return volatility(forward, strike, timeToExpiry, alpha, beta, rho, nu);
	  }

	  public double volatility(double forward, double strike, double timeToExpiry, double alpha, double beta, double rho, double nu)
	  {

		ArgChecker.isTrue(forward > 0.0, "forward must be greater than zero");
		ArgChecker.isTrue(strike >= 0.0, "strike must be greater than zero");
		ArgChecker.isTrue(timeToExpiry >= 0.0, "timeToExpiry must be greater than zero");

		if (alpha == 0.0)
		{
		  return 0.0;
		}
		double cutoff = forward * CUTOFF_MONEYNESS;
		double k;
		if (strike < cutoff)
		{
		  log.info("Given strike of {} is less than cutoff at {}, therefore the strike is taken as {}", new object[] {strike, cutoff, cutoff});
		  k = cutoff;
		}
		else
		{
		  k = strike;
		}
		double vol, z, zOverChi;
		double beta1 = 1 - beta;
		if (DoubleMath.fuzzyEquals(forward, k, ATM_EPS))
		{
		  double f1 = Math.Pow(forward, beta1);
		  vol = alpha * (1 + timeToExpiry * (beta1 * beta1 * alpha * alpha / 24 / f1 / f1 + rho * alpha * beta * nu / 4 / f1 + nu * nu * (2 - 3 * rho * rho) / 24)) / f1;
		}
		else
		{
		  if (DoubleMath.fuzzyEquals(beta, 0, BETA_EPS))
		  {
			double ln = Math.Log(forward / k);
			z = nu * Math.Sqrt(forward * k) * ln / alpha;
			zOverChi = getZOverChi(rho, z);
			vol = alpha * ln * zOverChi * (1 + timeToExpiry * (alpha * alpha / forward / k + nu * nu * (2 - 3 * rho * rho)) / 24) / (forward - k);
		  }
		  else if (DoubleMath.fuzzyEquals(beta, 1, BETA_EPS))
		  {
			double ln = Math.Log(forward / k);
			z = nu * ln / alpha;
			zOverChi = getZOverChi(rho, z);
			vol = alpha * zOverChi * (1 + timeToExpiry * (rho * alpha * nu / 4 + nu * nu * (2 - 3 * rho * rho) / 24));
		  }
		  else
		  {
			double ln = Math.Log(forward / k);
			double f1 = Math.Pow(forward * k, beta1);
			double f1Sqrt = Math.Sqrt(f1);
			double lnBetaSq = Math.Pow(beta1 * ln, 2);
			z = nu * f1Sqrt * ln / alpha;
			zOverChi = getZOverChi(rho, z);
			double first = alpha / (f1Sqrt * (1 + lnBetaSq / 24 + lnBetaSq * lnBetaSq / 1920));
			double second = zOverChi;
			double third = 1 + timeToExpiry * (beta1 * beta1 * alpha * alpha / 24 / f1 + rho * nu * beta * alpha / 4 / f1Sqrt + nu * nu * (2 - 3 * rho * rho) / 24);
			vol = first * second * third;
		  }
		}
		//There is nothing to prevent the nu * nu * (2 - 3 * rho * rho) / 24 to be large negative, and hence the volatility negative
		return Math.Max(MIN_VOL, vol);
	  }

	  /// <summary>
	  /// Computes the implied volatility in the SABR model and its derivatives.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the alpha
	  /// <li>[3] derivative with respect to the beta
	  /// <li>[4] derivative with respect to the rho
	  /// <li>[5] derivative with respect to the nu
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the SABR data </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public override ValueDerivatives volatilityAdjoint(double forward, double strike, double timeToExpiry, SabrFormulaData data)
	  {
		ArgChecker.notNull(data, "data");
		double alpha = data.Alpha;
		double beta = data.Beta;
		double rho = data.Rho;
		double nu = data.Nu;
		return volatilityAdjoint(forward, strike, timeToExpiry, alpha, beta, rho, nu);
	  }

	  /// <summary>
	  /// Computes the implied volatility in the SABR model and its derivatives.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the alpha
	  /// <li>[3] derivative with respect to the beta
	  /// <li>[4] derivative with respect to the rho
	  /// <li>[5] derivative with respect to the nu
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="alpha">  the SABR alpha value </param>
	  /// <param name="beta">  the SABR beta value </param>
	  /// <param name="rho">  the SABR rho value </param>
	  /// <param name="nu">  the SABR nu value </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public ValueDerivatives volatilityAdjoint(double forward, double strike, double timeToExpiry, double alpha, double beta, double rho, double nu)
	  {

		ArgChecker.isTrue(forward > 0.0, "forward must be greater than zero");
		ArgChecker.isTrue(strike >= 0.0, "strike must be greater than zero");
		ArgChecker.isTrue(timeToExpiry >= 0.0, "timeToExpiry must be greater than zero");
		double cutoff = forward * CUTOFF_MONEYNESS;
		double k = strike;
		if (k < cutoff)
		{
		  log.info("Given strike of {} is less than cutoff at {}, therefore the strike is taken as {}", new object[] {k, cutoff, cutoff});
		  k = cutoff;
		}
		double betaStar = 1 - beta;
		double rhoStar = 1.0 - rho;

		if (alpha == 0.0)
		{
		  double alphaBar;
		  if (DoubleMath.fuzzyEquals(forward, k, ATM_EPS))
		  { //TODO should this is relative
			alphaBar = (1 + (2 - 3 * rho * rho) * nu * nu / 24 * timeToExpiry) / Math.Pow(forward, betaStar);
		  }
		  else
		  {
			//for non-atm options the alpha sensitivity at alpha = 0 is infinite. Returning this will most likely break calibrations,
			// so we return an arbitrary large number
			alphaBar = 1e7;
		  }
		  return ValueDerivatives.of(0d, DoubleArray.of(0, 0, alphaBar, 0, 0, 0));
		}

		// Implementation note: Forward sweep.
		double sfK = Math.Pow(forward * k, betaStar / 2);
		double lnrfK = Math.Log(forward / k);
		double z = nu / alpha * sfK * lnrfK;
		double rzxz;
		double xz = 0;
		if (DoubleMath.fuzzyEquals(z, 0.0, SMALL_Z))
		{
		  rzxz = 1.0 - 0.5 * z * rho; //small z expansion to z^2 terms
		}
		else
		{
		  if (DoubleMath.fuzzyEquals(rhoStar, 0.0, RHO_EPS))
		  {
			if (z < 1.0)
			{
			  xz = -Math.Log(1.0d - z);
			  rzxz = z / xz;
			}
			else
			{
			  throw new System.ArgumentException("can't handle z>=1, rho=1");
			}
		  }
		  else
		  {
			double arg;
			if (z < LARGE_NEG_Z)
			{
			  arg = (rho * rho - 1) / 2 / z; //get rounding errors due to fine balanced cancellation for very large negative z
			}
			else if (z > LARGE_POS_Z)
			{
			  arg = 2 * (z - rho);
			}
			else
			{
			  arg = (Math.Sqrt(1 - 2 * rho * z + z * z) + z - rho);
			}
			if (arg <= 0.0)
			{ //Mathematically this cannot be less than zero, but you know what computers are like.
			  rzxz = 0.0;
			}
			else
			{
			  xz = Math.Log(arg / (1 - rho));
			  rzxz = z / xz;
			}
		  }
		}
		double sf1 = sfK * (1 + betaStar * betaStar / 24 * (lnrfK * lnrfK) + Math.Pow(betaStar, 4) / 1920 * Math.Pow(lnrfK, 4));
		double sf2 = (1 + (Math.Pow(betaStar * alpha / sfK, 2) / 24 + (rho * beta * nu * alpha) / (4 * sfK) + (2 - 3 * rho * rho) * nu * nu / 24) * timeToExpiry);
		double volatility = Math.Max(MIN_VOL, alpha / sf1 * rzxz * sf2);

		// Implementation note: Backward sweep.
		double vBar = 1;
		double sf2Bar = alpha / sf1 * rzxz * vBar;
		double sf1Bar = -alpha / (sf1 * sf1) * rzxz * sf2 * vBar;
		double rzxzBar = alpha / sf1 * sf2 * vBar;
		double zBar;
		double xzBar = 0.0;
		if (DoubleMath.fuzzyEquals(z, 0.0, SMALL_Z))
		{
		  zBar = -rho / 2 * rzxzBar;
		}
		else
		{
		  if (DoubleMath.fuzzyEquals(rhoStar, 0.0, RHO_EPS))
		  {
			if (z < 1.0)
			{
			  xzBar = -z / (xz * xz) * rzxzBar;
			  zBar = 1.0d / xz * rzxzBar + 1.0d / (1.0d - z) * xzBar;
			}
			else
			{
			  throw new System.ArgumentException("can't handle z>=1, rho=1");
			}
		  }
		  else
		  {
			if (z < LARGE_NEG_Z)
			{
			  zBar = 1 / xz * rzxzBar + xzBar / (xz * xz) * rzxzBar;
			}
			else if (z > LARGE_POS_Z)
			{
			  zBar = 1 / xz * rzxzBar - xzBar / (xz * xz) * rzxzBar;
			}
			else
			{
			  xzBar = -z / (xz * xz) * rzxzBar;
			  zBar = 1 / xz * rzxzBar + 1 / ((Math.Sqrt(1 - 2 * rho * z + z * z) + z - rho)) * (0.5 * Math.Pow(1 - 2 * rho * z + z * z, -0.5) * (-2 * rho + 2 * z) + 1) * xzBar;
			}
		  }
		}

		double lnrfKBar = sfK * (betaStar * betaStar / 12 * lnrfK + Math.Pow(betaStar, 4) / 1920 * 4 * Math.Pow(lnrfK, 3)) * sf1Bar + nu / alpha * sfK * zBar;
		double sfKBar = nu / alpha * lnrfK * zBar + sf1 / sfK * sf1Bar - (Math.Pow(betaStar * alpha, 2) / Math.Pow(sfK, 3) / 12 + (rho * beta * nu * alpha) / 4 / (sfK * sfK)) * timeToExpiry * sf2Bar;
		double strikeBar = -1 / k * lnrfKBar + betaStar * sfK / (2 * k) * sfKBar;
		double forwardBar = 1 / forward * lnrfKBar + betaStar * sfK / (2 * forward) * sfKBar;
		double nuBar = 1 / alpha * sfK * lnrfK * zBar + ((rho * beta * alpha) / (4 * sfK) + (2 - 3 * rho * rho) * nu / 12) * timeToExpiry * sf2Bar;

		double rhoBar;
		if (Math.Abs(forward - k) < ATM_EPS)
		{
		  rhoBar = -z / 2 * rzxzBar;
		}
		else
		{
		  if (DoubleMath.fuzzyEquals(rhoStar, 0.0, RHO_EPS))
		  {
			if (z >= 1)
			{
			  if (rhoStar == 0.0)
			  {
				rhoBar = double.NegativeInfinity; //the derivative at rho = 1 is infinite  - this sets it to some arbitrary large number
			  }
			  else
			  {
				rhoBar = xzBar * (1.0 / rhoStar + (0.5 - z) / (z - 1.0) / (z - 1.0));
			  }
			}
			else
			{
			  rhoBar = (0.5 * Math.Pow(z / (1 - z), 2) + 0.25 * (z - 4.0) * Math.Pow(z / (1.0 - z), 3) / (1.0 - z) * rhoStar) * xzBar;
			}
		  }
		  else
		  {
			rhoBar = (1 / (Math.Sqrt(1 - 2 * rho * z + z * z) + z - rho) * (-Math.Pow(1 - 2 * rho * z + z * z, -0.5) * z - 1) + 1 / rhoStar) * xzBar;
		  }
		}
		rhoBar += ((beta * nu * alpha) / (4 * sfK) - rho * nu * nu / 4) * timeToExpiry * sf2Bar;

		double alphaBar = -nu / (alpha * alpha) * sfK * lnrfK * zBar + ((betaStar * alpha / sfK) * (betaStar / sfK) / 12 + (rho * beta * nu) / (4 * sfK)) * timeToExpiry * sf2Bar + 1 / sf1 * rzxz * sf2 * vBar;
		double betaBar = -0.5 * Math.Log(forward * k) * sfK * sfKBar - sfK * (betaStar / 12 * (lnrfK * lnrfK) + Math.Pow(betaStar, 3) / 480 * Math.Pow(lnrfK, 4)) * sf1Bar + (-betaStar * alpha * alpha / sfK / sfK / 12 + rho * nu * alpha / 4 / sfK) * timeToExpiry * sf2Bar;

		return ValueDerivatives.of(volatility, DoubleArray.of(forwardBar, strikeBar, alphaBar, betaBar, rhoBar, nuBar));
	  }

	  /// <summary>
	  /// Computes the first and second order derivatives of the Black implied volatility in the SABR model.
	  /// <para>
	  /// The first derivative values will be stored in the input array {@code volatilityD} 
	  /// The array contains, [0] Derivative w.r.t the forward, [1] the derivative w.r.t the strike, [2] the derivative w.r.t. to alpha,
	  /// [3] the derivative w.r.t. to beta, [4] the derivative w.r.t. to rho, and [5] the derivative w.r.t. to nu.
	  /// Thus the length of the array should be 6.
	  /// </para>
	  /// <para>
	  /// The second derivative values will be stored in the input array {@code volatilityD2}. 
	  /// Only the second order derivative with respect to the forward and strike are implemented.
	  /// The array contains [0][0] forward-forward; [0][1] forward-strike; [1][1] strike-strike.
	  /// Thus the size should be 2 x 2.
	  /// </para>
	  /// <para>
	  /// Around ATM, a first order expansion is used to due to some 0/0-type indetermination.
	  /// The second order derivative produced is poor around ATM.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the SABR data. </param>
	  /// <param name="volatilityD">  the array used to return the first order derivative </param>
	  /// <param name="volatilityD2">  the array of array used to return the second order derivative </param>
	  /// <returns> the Black implied volatility </returns>
	  public override double volatilityAdjoint2(double forward, double strike, double timeToExpiry, SabrFormulaData data, double[] volatilityD, double[][] volatilityD2)
	  {
		double k = Math.Max(strike, 0.000001);
		double alpha = data.Alpha;
		double beta = data.Beta;
		double rho = data.Rho;
		double nu = data.Nu;
		// Forward
		double h0 = (1 - beta) / 2;
		double h1 = forward * k;
		double h1h0 = Math.Pow(h1, h0);
		double h12 = h1h0 * h1h0;
		double h2 = Math.Log(forward / k);
		double h22 = h2 * h2;
		double h23 = h22 * h2;
		double h24 = h23 * h2;
		double f1 = h1h0 * (1 + h0 * h0 / 6.0 * (h22 + h0 * h0 / 20.0 * h24));
		double f2 = nu / alpha * h1h0 * h2;
		double f3 = h0 * h0 / 6.0 * alpha * alpha / h12 + rho * beta * nu * alpha / 4.0 / h1h0 + (2 - 3 * rho * rho) / 24.0 * nu * nu;
		double sqrtf2 = Math.Sqrt(1 - 2 * rho * f2 + f2 * f2);
		double f2x = 0.0;
		double x = 0.0, xp = 0, xpp = 0;
		if (DoubleMath.fuzzyEquals(f2, 0.0, SMALL_Z))
		{
		  f2x = 1.0 - 0.5 * f2 * rho; //small f2 expansion to f2^2 terms
		}
		else
		{
		  if (DoubleMath.fuzzyEquals(rho, 1.0, RHO_EPS))
		  {
			x = f2 < 1.0 ? -Math.Log(1.0 - f2) - 0.5 * Math.Pow(f2 / (f2 - 1.0), 2) * (1.0 - rho) : Math.Log(2.0 * f2 - 2.0) - Math.Log(1.0 - rho);
		  }
		  else
		  {
			x = Math.Log((sqrtf2 + f2 - rho) / (1 - rho));
		  }
		  xp = 1.0 / sqrtf2;
		  xpp = (rho - f2) / Math.Pow(sqrtf2, 3.0);
		  f2x = f2 / x;
		}
		double sigma = Math.Max(MIN_VOL, alpha / f1 * f2x * (1 + f3 * timeToExpiry));
		// First level
		double h0Dbeta = -0.5;
		double sigmaDf1 = -sigma / f1;
		double sigmaDf2 = 0;
		if (DoubleMath.fuzzyEquals(f2, 0.0, SMALL_Z))
		{
		  sigmaDf2 = alpha / f1 * (1 + f3 * timeToExpiry) * -0.5 * rho;
		}
		else
		{
		  sigmaDf2 = alpha / f1 * (1 + f3 * timeToExpiry) * (1.0 / x - f2 * xp / (x * x));
		}
		double sigmaDf3 = alpha / f1 * f2x * timeToExpiry;
		double sigmaDf4 = f2x / f1 * (1 + f3 * timeToExpiry);
		double sigmaDx = -alpha / f1 * f2 / (x * x) * (1 + f3 * timeToExpiry);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] sigmaD2ff = new double[3][3];
		double[][] sigmaD2ff = RectangularArrays.ReturnRectangularDoubleArray(3, 3);
		sigmaD2ff[0][0] = -sigmaDf1 / f1 + sigma / (f1 * f1); //OK
		sigmaD2ff[0][1] = -sigmaDf2 / f1;
		sigmaD2ff[0][2] = -sigmaDf3 / f1;
		if (DoubleMath.fuzzyEquals(f2, 0.0, SMALL_Z))
		{
		  sigmaD2ff[1][2] = alpha / f1 * -0.5 * rho * timeToExpiry;
		}
		else
		{
		  sigmaD2ff[1][1] = alpha / f1 * (1 + f3 * timeToExpiry) * (-2 * xp / (x * x) - f2 * xpp / (x * x) + 2 * f2 * xp * xp / (x * x * x));
		  sigmaD2ff[1][2] = alpha / f1 * timeToExpiry * (1.0 / x - f2 * xp / (x * x));
		}
		sigmaD2ff[2][2] = 0.0;
		//      double sigma = alpha / f1 * f2x * (1 + f3 * theta);
		// Second level
		double[] f1Dh = new double[3];
		double[] f2Dh = new double[3];
		double[] f3Dh = new double[3];
		f1Dh[0] = h1h0 * (h0 * (h22 / 3.0 + h0 * h0 / 40.0 * h24)) + Math.Log(h1) * f1;
		f1Dh[1] = h0 * f1 / h1;
		f1Dh[2] = h1h0 * (h0 * h0 / 6.0 * (2.0 * h2 + h0 * h0 / 5.0 * h23));
		f2Dh[0] = Math.Log(h1) * f2;
		f2Dh[1] = h0 * f2 / h1;
		f2Dh[2] = nu / alpha * h1h0;
		f3Dh[0] = h0 / 3.0 * alpha * alpha / h12 - 2 * h0 * h0 / 6.0 * alpha * alpha / h12 * Math.Log(h1) - rho * beta * nu * alpha / 4.0 / h1h0 * Math.Log(h1);
		f3Dh[1] = -2 * h0 * h0 / 6.0 * alpha * alpha / h12 * h0 / h1 - rho * beta * nu * alpha / 4.0 / h1h0 * h0 / h1;
		f3Dh[2] = 0.0;
		double[] f1Dp = new double[4]; // Derivative to sabr parameters
		double[] f2Dp = new double[4];
		double[] f3Dp = new double[4];
		double[] f4Dp = new double[4];
		f1Dp[0] = 0.0;
		f1Dp[1] = f1Dh[0] * h0Dbeta;
		f1Dp[2] = 0.0;
		f1Dp[3] = 0.0;
		f2Dp[0] = -f2 / alpha;
		f2Dp[1] = f2Dh[0] * h0Dbeta;
		f2Dp[2] = 0.0;
		f2Dp[3] = h1h0 * h2 / alpha;
		f3Dp[0] = h0 * h0 / 3.0 * alpha / h12 + rho * beta * nu / 4.0 / h1h0;
		f3Dp[1] = rho * nu * alpha / 4.0 / h1h0 + f3Dh[0] * h0Dbeta;
		f3Dp[2] = beta * nu * alpha / 4.0 / h1h0 - rho / 4.0 * nu * nu;
		f3Dp[3] = rho * beta * alpha / 4.0 / h1h0 + (2 - 3 * rho * rho) / 12.0 * nu;
		f4Dp[0] = 1.0;
		f4Dp[1] = 0.0;
		f4Dp[2] = 0.0;
		f4Dp[3] = 0.0;
		double sigmaDh1 = sigmaDf1 * f1Dh[1] + sigmaDf2 * f2Dh[1] + sigmaDf3 * f3Dh[1];
		double sigmaDh2 = sigmaDf1 * f1Dh[2] + sigmaDf2 * f2Dh[2] + sigmaDf3 * f3Dh[2];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] f1D2hh = new double[2][2]; // No h0
		double[][] f1D2hh = RectangularArrays.ReturnRectangularDoubleArray(2, 2); // No h0
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] f2D2hh = new double[2][2];
		double[][] f2D2hh = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] f3D2hh = new double[2][2];
		double[][] f3D2hh = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		f1D2hh[0][0] = h0 * (h0 - 1) * f1 / (h1 * h1);
		f1D2hh[0][1] = h0 * h1h0 / h1 * h0 * h0 / 6.0 * (2.0 * h2 + 4.0 * h0 * h0 / 20.0 * h23);
		f1D2hh[1][1] = h1h0 * (h0 * h0 / 6.0 * (2.0 + 12.0 * h0 * h0 / 20.0 * h2));
		f2D2hh[0][0] = h0 * (h0 - 1) * f2 / (h1 * h1);
		f2D2hh[0][1] = nu / alpha * h0 * h1h0 / h1;
		f2D2hh[1][1] = 0.0;
		f3D2hh[0][0] = 2 * h0 * (2 * h0 + 1) * h0 * h0 / 6.0 * alpha * alpha / (h12 * h1 * h1) + h0 * (h0 + 1) * rho * beta * nu * alpha / 4.0 / (h1h0 * h1 * h1);
		f3D2hh[0][1] = 0.0;
		f3D2hh[1][1] = 0.0;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] sigmaD2hh = new double[2][2]; // No h0
		double[][] sigmaD2hh = RectangularArrays.ReturnRectangularDoubleArray(2, 2); // No h0
		for (int loopx = 0; loopx < 2; loopx++)
		{
		  for (int loopy = loopx; loopy < 2; loopy++)
		  {
			sigmaD2hh[loopx][loopy] = (sigmaD2ff[0][0] * f1Dh[loopy + 1] + sigmaD2ff[0][1] * f2Dh[loopy + 1] + sigmaD2ff[0][2] * f3Dh[loopy + 1]) * f1Dh[loopx + 1] + sigmaDf1 * f1D2hh[loopx][loopy] + (sigmaD2ff[0][1] * f1Dh[loopy + 1] + sigmaD2ff[1][1] * f2Dh[loopy + 1] + sigmaD2ff[1][2] * f3Dh[loopy + 1]) * f2Dh[loopx + 1] + sigmaDf2 * f2D2hh[loopx][loopy] + (sigmaD2ff[0][2] * f1Dh[loopy + 1] + sigmaD2ff[1][2] * f2Dh[loopy + 1] + sigmaD2ff[2][2] * f3Dh[loopy + 1]) * f3Dh[loopx + 1] + sigmaDf3 * f3D2hh[loopx][loopy];
		  }
		}
		// Third level
		double h1Df = k;
		double h1Dk = forward;
		double h1D2ff = 0.0;
		double h1D2kf = 1.0;
		double h1D2kk = 0.0;
		double h2Df = 1.0 / forward;
		double h2Dk = -1.0 / k;
		double h2D2ff = -1 / (forward * forward);
		double h2D2fk = 0.0;
		double h2D2kk = 1.0 / (k * k);
		volatilityD[0] = sigmaDh1 * h1Df + sigmaDh2 * h2Df;
		volatilityD[1] = sigmaDh1 * h1Dk + sigmaDh2 * h2Dk;
		volatilityD[2] = sigmaDf1 * f1Dp[0] + sigmaDf2 * f2Dp[0] + sigmaDf3 * f3Dp[0] + sigmaDf4 * f4Dp[0];
		volatilityD[3] = sigmaDf1 * f1Dp[1] + sigmaDf2 * f2Dp[1] + sigmaDf3 * f3Dp[1] + sigmaDf4 * f4Dp[1];
		if (DoubleMath.fuzzyEquals(f2, 0.0, SMALL_Z))
		{
		  volatilityD[4] = -0.5 * f2 + sigmaDf3 * f3Dp[2];
		}
		else
		{
		  double xDr;
		  if (DoubleMath.fuzzyEquals(rho, 1.0, RHO_EPS))
		  {
			xDr = f2 > 1.0 ? 1.0 / (1.0 - rho) + (0.5 - f2) / (f2 - 1.0) / (f2 - 1.0) : 0.5 * Math.Pow(f2 / (1.0 - f2), 2.0) + 0.25 * (f2 - 4.0) * Math.Pow(f2 / (f2 - 1.0), 3) / (f2 - 1.0) * (1.0 - rho);
			if (Doubles.isFinite(xDr))
			{
			  volatilityD[4] = sigmaDf1 * f1Dp[2] + sigmaDx * xDr + sigmaDf3 * f3Dp[2] + sigmaDf4 * f4Dp[2];
			}
			else
			{
			  volatilityD[4] = double.NegativeInfinity;
			}
		  }
		  else
		  {
			xDr = (-f2 / sqrtf2 - 1 + (sqrtf2 + f2 - rho) / (1 - rho)) / (sqrtf2 + f2 - rho);
			volatilityD[4] = sigmaDf1 * f1Dp[2] + sigmaDx * xDr + sigmaDf3 * f3Dp[2] + sigmaDf4 * f4Dp[2];
		  }
		}
		volatilityD[5] = sigmaDf1 * f1Dp[3] + sigmaDf2 * f2Dp[3] + sigmaDf3 * f3Dp[3] + sigmaDf4 * f4Dp[3];
		volatilityD2[0][0] = (sigmaD2hh[0][0] * h1Df + sigmaD2hh[0][1] * h2Df) * h1Df + sigmaDh1 * h1D2ff + (sigmaD2hh[0][1] * h1Df + sigmaD2hh[1][1] * h2Df) * h2Df + sigmaDh2 * h2D2ff;
		volatilityD2[0][1] = (sigmaD2hh[0][0] * h1Dk + sigmaD2hh[0][1] * h2Dk) * h1Df + sigmaDh1 * h1D2kf + (sigmaD2hh[0][1] * h1Dk + sigmaD2hh[1][1] * h2Dk) * h2Df + sigmaDh2 * h2D2fk;
		volatilityD2[1][0] = volatilityD2[0][1];
		volatilityD2[1][1] = (sigmaD2hh[0][0] * h1Dk + sigmaD2hh[0][1] * h2Dk) * h1Dk + sigmaDh1 * h1D2kk + (sigmaD2hh[0][1] * h1Dk + sigmaD2hh[1][1] * h2Dk) * h2Dk + sigmaDh2 * h2D2kk;
		return sigma;
	  }

	  private double getZOverChi(double rho, double z)
	  {

		// Implementation comment: To avoid numerical instability (0/0) around ATM the first order approximation is used.
		if (DoubleMath.fuzzyEquals(z, 0.0, SMALL_Z))
		{
		  return 1.0 - rho * z / 2.0;
		}

		double rhoStar = 1 - rho;
		if (DoubleMath.fuzzyEquals(rhoStar, 0.0, RHO_EPS))
		{
		  if (z < 1.0)
		  {
			return -z / Math.Log(1.0d - z);
		  }
		  else
		  {
			throw new System.ArgumentException("can't handle z>=1, rho=1");
		  }
		}

		double rhoHat = 1 + rho;
		if (DoubleMath.fuzzyEquals(rhoHat, 0.0, RHO_EPS_NEGATIVE))
		{
		  if (z > -1)
		  {
			return z / Math.Log(1 + z);
		  }
		  else if (z < -1)
		  {
			if (rhoHat == 0)
			{
			  return 0.0;
			}
			double chi = Math.Log(rhoHat) - Math.Log(-(1 + z) / rhoStar);
			return z / chi;
		  }
		  else
		  {
			return 0.0;
		  }
		}

		double arg;
		if (z < LARGE_NEG_Z)
		{
		  arg = (rho * rho - 1) / 2 / z; //get rounding errors due to fine balanced cancellation for very large negative z
		}
		else if (z > LARGE_POS_Z)
		{
		  arg = 2 * (z - rho);
		}
		else
		{
		  arg = (Math.Sqrt(1 - 2 * rho * z + z * z) + z - rho);
		  //Mathematically this cannot be less than zero, but you know what computers are like.
		  if (arg <= 0.0)
		  {
			return 0.0;
		  }
		}

		double chi = Math.Log(arg) - Math.Log(rhoStar);
		return z / chi;
	  }

	  public override int GetHashCode()
	  {
		return ToString().GetHashCode();
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == null)
		{
		  return false;
		}
		if (this == obj)
		{
		  return true;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		return true;
	  }

	  public override string ToString()
	  {
		return "SABR (Hagan)";
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrHaganVolatilityFunctionProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<SabrHaganVolatilityFunctionProvider> META_BEAN = LightMetaBean.of(typeof(SabrHaganVolatilityFunctionProvider), MethodHandles.lookup());

	  /// <summary>
	  /// The meta-bean for {@code SabrHaganVolatilityFunctionProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SabrHaganVolatilityFunctionProvider> meta()
	  {
		return META_BEAN;
	  }

	  static SabrHaganVolatilityFunctionProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SabrHaganVolatilityFunctionProvider()
	  {
	  }

	  public override TypedMetaBean<SabrHaganVolatilityFunctionProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  //-------------------------- AUTOGENERATED END --------------------------
	}

}