using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using ParameterizedDataCombiner = com.opengamma.strata.market.param.ParameterizedDataCombiner;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceInfoType = com.opengamma.strata.market.surface.SurfaceInfoType;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;

	/// <summary>
	/// The volatility surface description under SABR model.
	/// <para>
	/// This is used in interest rate modeling.
	/// Each SABR parameter is a <seealso cref="Surface"/> defined by expiry and tenor.
	/// </para>
	/// <para>
	/// The implementation allows for shifted SABR model.
	/// The shift parameter is also <seealso cref="Surface"/> defined by expiry and tenor.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class SabrInterestRateParameters implements com.opengamma.strata.market.param.ParameterizedData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SabrInterestRateParameters : ParameterizedData, ImmutableBean
	{

	  /// <summary>
	  /// A surface used to apply no shift.
	  /// </summary>
	  private static readonly ConstantSurface ZERO_SHIFT = ConstantSurface.of("Zero shift", 0d);

	  /// <summary>
	  /// The alpha (volatility level) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface alphaSurface;
	  private readonly Surface alphaSurface;
	  /// <summary>
	  /// The beta (elasticity) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface betaSurface;
	  private readonly Surface betaSurface;
	  /// <summary>
	  /// The rho (correlation) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface rhoSurface;
	  private readonly Surface rhoSurface;
	  /// <summary>
	  /// The nu (volatility of volatility) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface nuSurface;
	  private readonly Surface nuSurface;
	  /// <summary>
	  /// The shift parameter of shifted SABR model.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// The shift is set to be 0 unless specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface shiftSurface;
	  private readonly Surface shiftSurface;
	  /// <summary>
	  /// The SABR volatility formula.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SabrVolatilityFormula sabrVolatilityFormula;
	  private readonly SabrVolatilityFormula sabrVolatilityFormula;
	  /// <summary>
	  /// The day count convention of the surfaces.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property
	  /// <summary>
	  /// The parameter combiner.
	  /// </summary>
	  [NonSerialized]
	  private readonly ParameterizedDataCombiner paramCombiner; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance without shift from nodal surfaces and volatility function provider.
	  /// <para>
	  /// Each surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surfaces must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#SABR_ALPHA"/>, <seealso cref="ValueType#SABR_BETA"/>,
	  ///   <seealso cref="ValueType#SABR_RHO"/> or <seealso cref="ValueType#SABR_NU"/>
	  /// <li>The day count must be set in the additional information of the Alpha surface using
	  ///   <seealso cref="SurfaceInfoType#DAY_COUNT"/>, if present on other surfaces it must match that on the Alpha
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#sabrParameterByExpiryTenor(String, DayCount, ValueType)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="alphaSurface">  the alpha surface </param>
	  /// <param name="betaSurface">  the beta surface </param>
	  /// <param name="rhoSurface">  the rho surface </param>
	  /// <param name="nuSurface">  the nu surface </param>
	  /// <param name="sabrFormula">  the SABR formula </param>
	  /// <returns> {@code SabrInterestRateParameters} </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("javadoc") public static SabrInterestRateParameters of(com.opengamma.strata.market.surface.Surface alphaSurface, com.opengamma.strata.market.surface.Surface betaSurface, com.opengamma.strata.market.surface.Surface rhoSurface, com.opengamma.strata.market.surface.Surface nuSurface, SabrVolatilityFormula sabrFormula)
	  public static SabrInterestRateParameters of(Surface alphaSurface, Surface betaSurface, Surface rhoSurface, Surface nuSurface, SabrVolatilityFormula sabrFormula)
	  {

		return new SabrInterestRateParameters(alphaSurface, betaSurface, rhoSurface, nuSurface, ZERO_SHIFT, sabrFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with shift from nodal surfaces and volatility function provider.
	  /// <para>
	  /// Each surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surfaces must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#SABR_ALPHA"/>, <seealso cref="ValueType#SABR_BETA"/>,
	  ///   <seealso cref="ValueType#SABR_RHO"/> or <seealso cref="ValueType#SABR_NU"/> as appropriate
	  /// <li>The day count must be set in the additional information of the alpha surface using
	  ///   <seealso cref="SurfaceInfoType#DAY_COUNT"/>, if present on other surfaces it must match that on the alpha
	  /// </ul>
	  /// The shift surface does not have to contain any metadata.
	  /// If it does, the day count and convention must match that on the alpha surface.
	  /// </para>
	  /// <para>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#sabrParameterByExpiryTenor(String, DayCount, ValueType)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="alphaSurface">  the alpha surface </param>
	  /// <param name="betaSurface">  the beta surface </param>
	  /// <param name="rhoSurface">  the rho surface </param>
	  /// <param name="nuSurface">  the nu surface </param>
	  /// <param name="shiftSurface">  the shift surface </param>
	  /// <param name="sabrFormula">  the SABR formula </param>
	  /// <returns> {@code SabrInterestRateParameters} </returns>
	  public static SabrInterestRateParameters of(Surface alphaSurface, Surface betaSurface, Surface rhoSurface, Surface nuSurface, Surface shiftSurface, SabrVolatilityFormula sabrFormula)
	  {

		return new SabrInterestRateParameters(alphaSurface, betaSurface, rhoSurface, nuSurface, shiftSurface, sabrFormula);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SabrInterestRateParameters(com.opengamma.strata.market.surface.Surface alphaSurface, com.opengamma.strata.market.surface.Surface betaSurface, com.opengamma.strata.market.surface.Surface rhoSurface, com.opengamma.strata.market.surface.Surface nuSurface, com.opengamma.strata.market.surface.Surface shiftSurface, SabrVolatilityFormula sabrFormula)
	  private SabrInterestRateParameters(Surface alphaSurface, Surface betaSurface, Surface rhoSurface, Surface nuSurface, Surface shiftSurface, SabrVolatilityFormula sabrFormula)
	  {

		validate(alphaSurface, "alphaSurface", ValueType.SABR_ALPHA);
		validate(betaSurface, "betaSurface", ValueType.SABR_BETA);
		validate(rhoSurface, "rhoSurface", ValueType.SABR_RHO);
		validate(nuSurface, "nuSurface", ValueType.SABR_NU);
		ArgChecker.notNull(shiftSurface, "shiftSurface");
		ArgChecker.notNull(sabrFormula, "sabrFormula");
		DayCount dayCount = alphaSurface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing DayCount"));
		validate(betaSurface, dayCount);
		validate(rhoSurface, dayCount);
		validate(nuSurface, dayCount);
		validate(shiftSurface, dayCount);

		this.alphaSurface = alphaSurface;
		this.betaSurface = betaSurface;
		this.rhoSurface = rhoSurface;
		this.nuSurface = nuSurface;
		this.shiftSurface = shiftSurface;
		this.sabrVolatilityFormula = sabrFormula;
		this.dayCount = dayCount;
		this.paramCombiner = ParameterizedDataCombiner.of(alphaSurface, betaSurface, rhoSurface, nuSurface, shiftSurface);
	  }

	  // basic value tpe checks
	  private static void validate(Surface surface, string name, ValueType zType)
	  {
		ArgChecker.notNull(surface, name);
		surface.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for SABR volatilities");
		surface.Metadata.YValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect y-value type for SABR volatilities");
		ValueType zValueType = surface.Metadata.ZValueType;
		zValueType.checkEquals(zType, "Incorrect z-value type for SABR volatilities");
	  }

	  // ensure all surfaces that specify convention or day count are consistent
	  private static void validate(Surface surface, DayCount dayCount)
	  {
		if (!surface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElse(dayCount).Equals(dayCount))
		{
		  throw new System.ArgumentException("SABR surfaces must have the same day count");
		}
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new SabrInterestRateParameters(alphaSurface, betaSurface, rhoSurface, nuSurface, shiftSurface, sabrVolatilityFormula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count used to calculate the expiry year fraction.
	  /// </summary>
	  /// <returns> the day count </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return paramCombiner.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return paramCombiner.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return paramCombiner.getParameterMetadata(parameterIndex);
	  }

	  public SabrInterestRateParameters withParameter(int parameterIndex, double newValue)
	  {
		return new SabrInterestRateParameters(paramCombiner.underlyingWithParameter(0, typeof(Surface), parameterIndex, newValue), paramCombiner.underlyingWithParameter(1, typeof(Surface), parameterIndex, newValue), paramCombiner.underlyingWithParameter(2, typeof(Surface), parameterIndex, newValue), paramCombiner.underlyingWithParameter(3, typeof(Surface), parameterIndex, newValue), paramCombiner.underlyingWithParameter(4, typeof(Surface), parameterIndex, newValue), sabrVolatilityFormula);
	  }

	  public override SabrInterestRateParameters withPerturbation(ParameterPerturbation perturbation)
	  {
		return new SabrInterestRateParameters(paramCombiner.underlyingWithPerturbation(0, typeof(Surface), perturbation), paramCombiner.underlyingWithPerturbation(1, typeof(Surface), perturbation), paramCombiner.underlyingWithPerturbation(2, typeof(Surface), perturbation), paramCombiner.underlyingWithPerturbation(3, typeof(Surface), perturbation), paramCombiner.underlyingWithPerturbation(4, typeof(Surface), perturbation), sabrVolatilityFormula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the alpha parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the alpha parameter </returns>
	  public double alpha(double expiry, double tenor)
	  {
		return alphaSurface.zValue(expiry, tenor);
	  }

	  /// <summary>
	  /// Calculates the beta parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the beta parameter </returns>
	  public double beta(double expiry, double tenor)
	  {
		return betaSurface.zValue(expiry, tenor);
	  }

	  /// <summary>
	  /// Calculates the rho parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the rho parameter </returns>
	  public double rho(double expiry, double tenor)
	  {
		return rhoSurface.zValue(expiry, tenor);
	  }

	  /// <summary>
	  /// Calculates the nu parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the nu parameter </returns>
	  public double nu(double expiry, double tenor)
	  {
		return nuSurface.zValue(expiry, tenor);
	  }

	  /// <summary>
	  /// Calculates the shift parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the shift parameter </returns>
	  public double shift(double expiry, double tenor)
	  {
		return shiftSurface.zValue(expiry, tenor);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility for given expiry, tenor, strike and forward rate.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor as a year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility </returns>
	  public double volatility(double expiry, double tenor, double strike, double forward)
	  {
		double alpha = this.alpha(expiry, tenor);
		double beta = this.beta(expiry, tenor);
		double rho = this.rho(expiry, tenor);
		double nu = this.nu(expiry, tenor);
		double shift = this.shift(expiry, tenor);
		return sabrVolatilityFormula.volatility(forward + shift, strike + shift, expiry, alpha, beta, rho, nu);
	  }

	  /// <summary>
	  /// Calculates the volatility and associated sensitivities.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the forward strike
	  /// <li>[2] derivative with respect to the alpha
	  /// <li>[3] derivative with respect to the beta
	  /// <li>[4] derivative with respect to the rho
	  /// <li>[5] derivative with respect to the nu
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public ValueDerivatives volatilityAdjoint(double expiry, double tenor, double strike, double forward)
	  {
		double alpha = this.alpha(expiry, tenor);
		double beta = this.beta(expiry, tenor);
		double rho = this.rho(expiry, tenor);
		double nu = this.nu(expiry, tenor);
		double shift = this.shift(expiry, tenor);
		return sabrVolatilityFormula.volatilityAdjoint(forward + shift, strike + shift, expiry, alpha, beta, rho, nu);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrInterestRateParameters}.
	  /// </summary>
	  private static readonly TypedMetaBean<SabrInterestRateParameters> META_BEAN = LightMetaBean.of(typeof(SabrInterestRateParameters), MethodHandles.lookup(), new string[] {"alphaSurface", "betaSurface", "rhoSurface", "nuSurface", "shiftSurface", "sabrVolatilityFormula"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code SabrInterestRateParameters}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SabrInterestRateParameters> meta()
	  {
		return META_BEAN;
	  }

	  static SabrInterestRateParameters()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<SabrInterestRateParameters> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the alpha (volatility level) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface AlphaSurface
	  {
		  get
		  {
			return alphaSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the beta (elasticity) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface BetaSurface
	  {
		  get
		  {
			return betaSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rho (correlation) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface RhoSurface
	  {
		  get
		  {
			return rhoSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nu (volatility of volatility) surface.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface NuSurface
	  {
		  get
		  {
			return nuSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift parameter of shifted SABR model.
	  /// <para>
	  /// The first dimension is the expiry and the second the tenor.
	  /// The shift is set to be 0 unless specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface ShiftSurface
	  {
		  get
		  {
			return shiftSurface;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the SABR volatility formula. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SabrVolatilityFormula SabrVolatilityFormula
	  {
		  get
		  {
			return sabrVolatilityFormula;
		  }
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
		  SabrInterestRateParameters other = (SabrInterestRateParameters) obj;
		  return JodaBeanUtils.equal(alphaSurface, other.alphaSurface) && JodaBeanUtils.equal(betaSurface, other.betaSurface) && JodaBeanUtils.equal(rhoSurface, other.rhoSurface) && JodaBeanUtils.equal(nuSurface, other.nuSurface) && JodaBeanUtils.equal(shiftSurface, other.shiftSurface) && JodaBeanUtils.equal(sabrVolatilityFormula, other.sabrVolatilityFormula);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(alphaSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(betaSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rhoSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nuSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftSurface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sabrVolatilityFormula);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("SabrInterestRateParameters{");
		buf.Append("alphaSurface").Append('=').Append(alphaSurface).Append(',').Append(' ');
		buf.Append("betaSurface").Append('=').Append(betaSurface).Append(',').Append(' ');
		buf.Append("rhoSurface").Append('=').Append(rhoSurface).Append(',').Append(' ');
		buf.Append("nuSurface").Append('=').Append(nuSurface).Append(',').Append(' ');
		buf.Append("shiftSurface").Append('=').Append(shiftSurface).Append(',').Append(' ');
		buf.Append("sabrVolatilityFormula").Append('=').Append(JodaBeanUtils.ToString(sabrVolatilityFormula));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}