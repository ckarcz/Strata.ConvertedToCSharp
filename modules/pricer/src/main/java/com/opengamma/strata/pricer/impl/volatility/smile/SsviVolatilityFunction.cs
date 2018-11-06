using System;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Surface Stochastic Volatility Inspired (SSVI) formula.
	/// <para>
	/// Reference: Gatheral, Jim and Jacquier, Antoine. Arbitrage-free SVI volatility surfaces. arXiv:1204.0646v4, 2013. Section 4.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class SsviVolatilityFunction extends VolatilityFunctionProvider<SsviFormulaData> implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SsviVolatilityFunction : VolatilityFunctionProvider<SsviFormulaData>, ImmutableBean
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly SsviVolatilityFunction DEFAULT = new SsviVolatilityFunction();

	  /// <summary>
	  /// SSVI volatility description diverge for theta -> 0. Lower bound for which time to expiry is accepted. </summary>
	  public const double MIN_TIME_TO_EXPIRY = 1.0E-3;

	  //-------------------------------------------------------------------------
	  public override double volatility(double forward, double strike, double timeToExpiry, SsviFormulaData data)
	  {
		ArgChecker.isTrue(timeToExpiry > MIN_TIME_TO_EXPIRY, "time to expiry must not be zero to be able to compute volatility");
		double volatilityAtm = data.Sigma;
		double rho = data.Rho;
		double eta = data.Eta;
		double theta = volatilityAtm * volatilityAtm * timeToExpiry;
		double phi = eta / Math.Sqrt(theta);
		double k = Math.Log(strike / forward);
		double w = 0.5 * theta * (1.0d + rho * phi * k + Math.Sqrt(1.0d + 2 * rho * phi * k + phi * k * phi * k));
		return Math.Sqrt(w / timeToExpiry);
	  }

	  /// <summary>
	  /// Computes the implied volatility in the SSVI formula and its derivatives.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the strike
	  /// <li>[2] derivative with respect to the time to expiry
	  /// <li>[3] derivative with respect to the sigma (ATM volatility)
	  /// <li>[4] derivative with respect to the rho
	  /// <li>[5] derivative with respect to the eta
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the SSVI data </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public override ValueDerivatives volatilityAdjoint(double forward, double strike, double timeToExpiry, SsviFormulaData data)
	  {
		ArgChecker.isTrue(timeToExpiry > MIN_TIME_TO_EXPIRY, "time to expiry must not be zero to be able to compute volatility");
		double volatilityAtm = data.Sigma;
		double rho = data.Rho;
		double eta = data.Eta;
		double theta = volatilityAtm * volatilityAtm * timeToExpiry;
		double stheta = Math.Sqrt(theta);
		double phi = eta / stheta;
		double k = Math.Log(strike / forward);
		double s = Math.Sqrt(1.0d + 2 * rho * phi * k + phi * k * phi * k);
		double w = 0.5 * theta * (1.0d + rho * phi * k + s);
		double volatility = Math.Sqrt(w / timeToExpiry);
		// Backward sweep.
		double[] derivatives = new double[6]; // 6 inputs
		double volatilityBar = 1.0;
		double wBar = 0.5 * volatility / w * volatilityBar;
		derivatives[2] += -0.5 * volatility / timeToExpiry * volatilityBar;
		double thetaBar = w / theta * wBar;
		derivatives[4] += 0.5 * theta * phi * k * wBar;
		double phiBar = 0.5 * theta * rho * k * wBar;
		double kBar = 0.5 * theta * rho * phi * wBar;
		double sBar = 0.5 * theta * wBar;
		derivatives[4] += phi * k / s * sBar;
		phiBar += (rho * k + phi * k * k) / s * sBar;
		kBar += (rho * phi + phi * phi * k) / s * sBar;
		derivatives[1] += 1.0d / strike * kBar;
		derivatives[0] += -1.0d / forward * kBar;
		derivatives[5] += phiBar / stheta;
		double sthetaBar = -eta / (stheta * stheta) * phiBar;
		thetaBar += 0.5 / stheta * sthetaBar;
		derivatives[3] += 2 * volatilityAtm * timeToExpiry * thetaBar;
		derivatives[2] += volatilityAtm * volatilityAtm * thetaBar;
		return ValueDerivatives.of(volatility, DoubleArray.ofUnsafe(derivatives));
	  }

	  public override double volatilityAdjoint2(double forward, double strike, double timeToExpiry, SsviFormulaData data, double[] volatilityD, double[][] volatilityD2)
	  {
		throw new System.NotSupportedException("Not implemented");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SsviVolatilityFunction}.
	  /// </summary>
	  private static readonly TypedMetaBean<SsviVolatilityFunction> META_BEAN = LightMetaBean.of(typeof(SsviVolatilityFunction), MethodHandles.lookup());

	  /// <summary>
	  /// The meta-bean for {@code SsviVolatilityFunction}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SsviVolatilityFunction> meta()
	  {
		return META_BEAN;
	  }

	  static SsviVolatilityFunction()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SsviVolatilityFunction()
	  {
	  }

	  public override TypedMetaBean<SsviVolatilityFunction> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  return true;
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(32);
		buf.Append("SsviVolatilityFunction{");
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}