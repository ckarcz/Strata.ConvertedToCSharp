using System;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using ParameterizedDataCombiner = com.opengamma.strata.market.param.ParameterizedDataCombiner;

	/// <summary>
	/// The volatility surface description under SABR model.
	/// <para>
	/// This is used in interest rate modeling.
	/// Each SABR parameter is a <seealso cref="Curve"/> defined by expiry.
	/// </para>
	/// <para>
	/// The implementation allows for shifted SABR model.
	/// The shift parameter is also <seealso cref="Curve"/> defined by expiry.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class SabrParameters implements com.opengamma.strata.market.param.ParameterizedData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SabrParameters : ParameterizedData, ImmutableBean
	{

	  /// <summary>
	  /// A Curve used to apply no shift.
	  /// </summary>
	  private static readonly ConstantCurve ZERO_SHIFT = ConstantCurve.of("Zero shift", 0d);

	  /// <summary>
	  /// The alpha (volatility level) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve alphaCurve;
	  private readonly Curve alphaCurve;
	  /// <summary>
	  /// The beta (elasticity) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve betaCurve;
	  private readonly Curve betaCurve;
	  /// <summary>
	  /// The rho (correlation) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve rhoCurve;
	  private readonly Curve rhoCurve;
	  /// <summary>
	  /// The nu (volatility of volatility) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve nuCurve;
	  private readonly Curve nuCurve;
	  /// <summary>
	  /// The shift parameter of shifted SABR model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// The shift is set to be 0 unless specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve shiftCurve;
	  private readonly Curve shiftCurve;
	  /// <summary>
	  /// The SABR volatility formula.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SabrVolatilityFormula sabrVolatilityFormula;
	  private readonly SabrVolatilityFormula sabrVolatilityFormula;
	  /// <summary>
	  /// The day count convention of the curves.
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
	  /// Obtains an instance without shift from nodal curves and volatility function provider.
	  /// <para>
	  /// Each curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curves must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#SABR_ALPHA"/>, <seealso cref="ValueType#SABR_BETA"/>,
	  ///   <seealso cref="ValueType#SABR_RHO"/> or <seealso cref="ValueType#SABR_NU"/>
	  /// <li>The day count must be set in the additional information of the Alpha curve using
	  ///   <seealso cref="CurveInfoType#DAY_COUNT"/>, if present on other curves it must match that on the Alpha
	  /// </ul>
	  /// Suitable curve metadata can be created using
	  /// <seealso cref="Curves#sabrParameterByExpiry(String, DayCount, ValueType)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="alphaCurve">  the alpha curve </param>
	  /// <param name="betaCurve">  the beta curve </param>
	  /// <param name="rhoCurve">  the rho curve </param>
	  /// <param name="nuCurve">  the nu curve </param>
	  /// <param name="sabrFormula">  the SABR formula </param>
	  /// <returns> {@code SabrParameters} </returns>
	  public static SabrParameters of(Curve alphaCurve, Curve betaCurve, Curve rhoCurve, Curve nuCurve, SabrVolatilityFormula sabrFormula)
	  {

		return new SabrParameters(alphaCurve, betaCurve, rhoCurve, nuCurve, ZERO_SHIFT, sabrFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with shift from nodal curves and volatility function provider.
	  /// <para>
	  /// Each curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curves must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#SABR_ALPHA"/>, <seealso cref="ValueType#SABR_BETA"/>,
	  ///   <seealso cref="ValueType#SABR_RHO"/> or <seealso cref="ValueType#SABR_NU"/> as appropriate
	  /// <li>The day count must be set in the additional information of the alpha curve using
	  ///   <seealso cref="CurveInfoType#DAY_COUNT"/>, if present on other curves it must match that on the alpha
	  /// </ul>
	  /// The shift curve does not have to contain any metadata.
	  /// If it does, the day count and convention must match that on the alpha curve.
	  /// </para>
	  /// <para>
	  /// Suitable curve metadata can be created using
	  /// <seealso cref="Curves#sabrParameterByExpiry(String, DayCount, ValueType)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="alphaCurve">  the alpha curve </param>
	  /// <param name="betaCurve">  the beta curve </param>
	  /// <param name="rhoCurve">  the rho curve </param>
	  /// <param name="nuCurve">  the nu curve </param>
	  /// <param name="shiftCurve">  the shift curve </param>
	  /// <param name="sabrFormula">  the SABR formula </param>
	  /// <returns> {@code SabrParameters} </returns>
	  public static SabrParameters of(Curve alphaCurve, Curve betaCurve, Curve rhoCurve, Curve nuCurve, Curve shiftCurve, SabrVolatilityFormula sabrFormula)
	  {

		return new SabrParameters(alphaCurve, betaCurve, rhoCurve, nuCurve, shiftCurve, sabrFormula);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SabrParameters(com.opengamma.strata.market.curve.Curve alphaCurve, com.opengamma.strata.market.curve.Curve betaCurve, com.opengamma.strata.market.curve.Curve rhoCurve, com.opengamma.strata.market.curve.Curve nuCurve, com.opengamma.strata.market.curve.Curve shiftCurve, SabrVolatilityFormula sabrFormula)
	  private SabrParameters(Curve alphaCurve, Curve betaCurve, Curve rhoCurve, Curve nuCurve, Curve shiftCurve, SabrVolatilityFormula sabrFormula)
	  {

		validate(alphaCurve, "alphaCurve", ValueType.SABR_ALPHA);
		validate(betaCurve, "betaCurve", ValueType.SABR_BETA);
		validate(rhoCurve, "rhoCurve", ValueType.SABR_RHO);
		validate(nuCurve, "nuCurve", ValueType.SABR_NU);
		ArgChecker.notNull(shiftCurve, "shiftCurve");
		ArgChecker.notNull(sabrFormula, "sabrFormula");
		DayCount dayCount = alphaCurve.Metadata.findInfo(CurveInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect curve metadata, missing DayCount"));
		validate(betaCurve, dayCount);
		validate(rhoCurve, dayCount);
		validate(nuCurve, dayCount);
		validate(shiftCurve, dayCount);

		this.alphaCurve = alphaCurve;
		this.betaCurve = betaCurve;
		this.rhoCurve = rhoCurve;
		this.nuCurve = nuCurve;
		this.shiftCurve = shiftCurve;
		this.sabrVolatilityFormula = sabrFormula;
		this.dayCount = dayCount;
		this.paramCombiner = ParameterizedDataCombiner.of(alphaCurve, betaCurve, rhoCurve, nuCurve, shiftCurve);
	  }

	  // basic value tpe checks
	  private static void validate(Curve curve, string name, ValueType yType)
	  {
		ArgChecker.notNull(curve, name);
		curve.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for SABR volatilities");
		ValueType yValueType = curve.Metadata.YValueType;
		yValueType.checkEquals(yType, "Incorrect y-value type for SABR volatilities");
	  }

	  // ensure all curves that specify convention or day count are consistent
	  private static void validate(Curve curve, DayCount dayCount)
	  {
		if (!curve.Metadata.findInfo(CurveInfoType.DAY_COUNT).orElse(dayCount).Equals(dayCount))
		{
		  throw new System.ArgumentException("SABR curves must have the same day count");
		}
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new SabrParameters(alphaCurve, betaCurve, rhoCurve, nuCurve, shiftCurve, sabrVolatilityFormula);
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

	  public SabrParameters withParameter(int parameterIndex, double newValue)
	  {
		return new SabrParameters(paramCombiner.underlyingWithParameter(0, typeof(Curve), parameterIndex, newValue), paramCombiner.underlyingWithParameter(1, typeof(Curve), parameterIndex, newValue), paramCombiner.underlyingWithParameter(2, typeof(Curve), parameterIndex, newValue), paramCombiner.underlyingWithParameter(3, typeof(Curve), parameterIndex, newValue), paramCombiner.underlyingWithParameter(4, typeof(Curve), parameterIndex, newValue), sabrVolatilityFormula);
	  }

	  public override SabrParameters withPerturbation(ParameterPerturbation perturbation)
	  {
		return new SabrParameters(paramCombiner.underlyingWithPerturbation(0, typeof(Curve), perturbation), paramCombiner.underlyingWithPerturbation(1, typeof(Curve), perturbation), paramCombiner.underlyingWithPerturbation(2, typeof(Curve), perturbation), paramCombiner.underlyingWithPerturbation(3, typeof(Curve), perturbation), paramCombiner.underlyingWithPerturbation(4, typeof(Curve), perturbation), sabrVolatilityFormula);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the alpha parameter for time to expiry.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <returns> the alpha parameter </returns>
	  public double alpha(double expiry)
	  {
		return alphaCurve.yValue(expiry);
	  }

	  /// <summary>
	  /// Calculates the beta parameter for time to expiry.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <returns> the beta parameter </returns>
	  public double beta(double expiry)
	  {
		return betaCurve.yValue(expiry);
	  }

	  /// <summary>
	  /// Calculates the rho parameter for time to expiry.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <returns> the rho parameter </returns>
	  public double rho(double expiry)
	  {
		return rhoCurve.yValue(expiry);
	  }

	  /// <summary>
	  /// Calculates the nu parameter for time to expiry.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <returns> the nu parameter </returns>
	  public double nu(double expiry)
	  {
		return nuCurve.yValue(expiry);
	  }

	  /// <summary>
	  /// Calculates the shift parameter for time to expiry.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <returns> the shift parameter </returns>
	  public double shift(double expiry)
	  {
		return shiftCurve.yValue(expiry);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility for given expiry, strike and forward rate.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility </returns>
	  public double volatility(double expiry, double strike, double forward)
	  {
		double alpha = this.alpha(expiry);
		double beta = this.beta(expiry);
		double rho = this.rho(expiry);
		double nu = this.nu(expiry);
		double shift = this.shift(expiry);
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
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public ValueDerivatives volatilityAdjoint(double expiry, double strike, double forward)
	  {
		double alpha = this.alpha(expiry);
		double beta = this.beta(expiry);
		double rho = this.rho(expiry);
		double nu = this.nu(expiry);
		double shift = this.shift(expiry);
		return sabrVolatilityFormula.volatilityAdjoint(forward + shift, strike + shift, expiry, alpha, beta, rho, nu);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrParameters}.
	  /// </summary>
	  private static readonly TypedMetaBean<SabrParameters> META_BEAN = LightMetaBean.of(typeof(SabrParameters), MethodHandles.lookup(), new string[] {"alphaCurve", "betaCurve", "rhoCurve", "nuCurve", "shiftCurve", "sabrVolatilityFormula"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code SabrParameters}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<SabrParameters> meta()
	  {
		return META_BEAN;
	  }

	  static SabrParameters()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<SabrParameters> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the alpha (volatility level) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve AlphaCurve
	  {
		  get
		  {
			return alphaCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the beta (elasticity) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve BetaCurve
	  {
		  get
		  {
			return betaCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rho (correlation) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve RhoCurve
	  {
		  get
		  {
			return rhoCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nu (volatility of volatility) curve.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve NuCurve
	  {
		  get
		  {
			return nuCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift parameter of shifted SABR model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// The shift is set to be 0 unless specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve ShiftCurve
	  {
		  get
		  {
			return shiftCurve;
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
		  SabrParameters other = (SabrParameters) obj;
		  return JodaBeanUtils.equal(alphaCurve, other.alphaCurve) && JodaBeanUtils.equal(betaCurve, other.betaCurve) && JodaBeanUtils.equal(rhoCurve, other.rhoCurve) && JodaBeanUtils.equal(nuCurve, other.nuCurve) && JodaBeanUtils.equal(shiftCurve, other.shiftCurve) && JodaBeanUtils.equal(sabrVolatilityFormula, other.sabrVolatilityFormula);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(alphaCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(betaCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rhoCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nuCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sabrVolatilityFormula);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("SabrParameters{");
		buf.Append("alphaCurve").Append('=').Append(alphaCurve).Append(',').Append(' ');
		buf.Append("betaCurve").Append('=').Append(betaCurve).Append(',').Append(' ');
		buf.Append("rhoCurve").Append('=').Append(rhoCurve).Append(',').Append(' ');
		buf.Append("nuCurve").Append('=').Append(nuCurve).Append(',').Append(' ');
		buf.Append("shiftCurve").Append('=').Append(shiftCurve).Append(',').Append(' ');
		buf.Append("sabrVolatilityFormula").Append('=').Append(JodaBeanUtils.ToString(sabrVolatilityFormula));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------

	}

}