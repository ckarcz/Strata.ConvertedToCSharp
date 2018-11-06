using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using SabrParameterType = com.opengamma.strata.market.model.SabrParameterType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using SabrInterestRateParameters = com.opengamma.strata.pricer.model.SabrInterestRateParameters;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Volatility environment for swaptions in the SABR model.
	/// <para>
	/// The volatility is represented in terms of SABR model parameters.
	/// </para>
	/// <para>
	/// The {@code parameterSensitivity()}, {@code priceGamma()} and {@code priceTheta()} methods are not implemented.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class SabrParametersSwaptionVolatilities implements SabrSwaptionVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SabrParametersSwaptionVolatilities : SabrSwaptionVolatilities, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final SwaptionVolatilitiesName name;
		private readonly SwaptionVolatilitiesName name;
	  /// <summary>
	  /// The swap convention that the volatilities are to be used for.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.swap.type.FixedIborSwapConvention convention;
	  private readonly FixedIborSwapConvention convention;
	  /// <summary>
	  /// The valuation date-time.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.ZonedDateTime valuationDateTime;
	  private readonly ZonedDateTime valuationDateTime;
	  /// <summary>
	  /// The SABR model parameters.
	  /// <para>
	  /// Each model parameter of SABR model is a surface.
	  /// The x-value of the surface is the expiry, as a year fraction.
	  /// The y-value of the surface is the swap tenor, as a year fraction rounded to the month.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.model.SabrInterestRateParameters parameters;
	  private readonly SabrInterestRateParameters parameters;
	  /// <summary>
	  /// The sensitivity of the Alpha parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray> dataSensitivityAlpha;
	  private readonly ImmutableList<DoubleArray> dataSensitivityAlpha;
	  /// <summary>
	  /// The sensitivity of the Beta parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray> dataSensitivityBeta;
	  private readonly ImmutableList<DoubleArray> dataSensitivityBeta;
	  /// <summary>
	  /// The sensitivity of the Rho parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray> dataSensitivityRho;
	  private readonly ImmutableList<DoubleArray> dataSensitivityRho;
	  /// <summary>
	  /// The sensitivity of the Nu parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray> dataSensitivityNu;
	  private readonly ImmutableList<DoubleArray> dataSensitivityNu;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the SABR model parameters and the date-time for which it is valid.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="convention">  the swap convention that the volatilities are to be used for </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="parameters">  the SABR model parameters </param>
	  /// <returns> the volatilities </returns>
	  public static SabrParametersSwaptionVolatilities of(SwaptionVolatilitiesName name, FixedIborSwapConvention convention, ZonedDateTime valuationDateTime, SabrInterestRateParameters parameters)
	  {

		return new SabrParametersSwaptionVolatilities(name, convention, valuationDateTime, parameters, null, null, null, null);
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
			return Parameters.DayCount;
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (parameters.AlphaSurface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(parameters.AlphaSurface));
		}
		if (parameters.BetaSurface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(parameters.BetaSurface));
		}
		if (parameters.RhoSurface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(parameters.RhoSurface));
		}
		if (parameters.NuSurface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(parameters.NuSurface));
		}
		if (parameters.ShiftSurface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(parameters.ShiftSurface));
		}
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return parameters.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return parameters.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return parameters.getParameterMetadata(parameterIndex);
	  }

	  public SabrParametersSwaptionVolatilities withParameter(int parameterIndex, double newValue)
	  {
		SabrInterestRateParameters updated = parameters.withParameter(parameterIndex, newValue);
		return new SabrParametersSwaptionVolatilities(name, convention, valuationDateTime, updated, dataSensitivityAlpha, dataSensitivityBeta, dataSensitivityRho, dataSensitivityNu);
	  }

	  public SabrParametersSwaptionVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		SabrInterestRateParameters updated = parameters.withPerturbation(perturbation);
		return new SabrParametersSwaptionVolatilities(name, convention, valuationDateTime, updated, dataSensitivityAlpha, dataSensitivityBeta, dataSensitivityRho, dataSensitivityNu);
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(double expiry, double tenor, double strike, double forwardRate)
	  {
		return parameters.volatility(expiry, tenor, strike, forwardRate);
	  }

	  public ValueDerivatives volatilityAdjoint(double expiry, double tenor, double strike, double forward)
	  {
		return parameters.volatilityAdjoint(expiry, tenor, strike, forward);
	  }

	  public double alpha(double expiry, double tenor)
	  {
		return parameters.alpha(expiry, tenor);
	  }

	  public double beta(double expiry, double tenor)
	  {
		return parameters.beta(expiry, tenor);
	  }

	  public double rho(double expiry, double tenor)
	  {
		return parameters.rho(expiry, tenor);
	  }

	  public double nu(double expiry, double tenor)
	  {
		return parameters.nu(expiry, tenor);
	  }

	  public double shift(double expiry, double tenor)
	  {
		return parameters.shift(expiry, tenor);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is SwaptionSabrSensitivity)
		  {
			SwaptionSabrSensitivity pt = (SwaptionSabrSensitivity) point;
			if (pt.VolatilitiesName.Equals(Name))
			{
			  sens = sens.combinedWith(parameterSensitivity(pt));
			}
		  }
		}
		return sens;
	  }

	  // convert a single point sensitivity
	  private CurrencyParameterSensitivity parameterSensitivity(SwaptionSabrSensitivity point)
	  {
		Surface surface = getSurface(point.SensitivityType);
		double expiry = point.Expiry;
		UnitParameterSensitivity unitSens = surface.zValueParameterSensitivity(expiry, point.Tenor);
		return unitSens.multipliedBy(point.Currency, point.Sensitivity);
	  }

	  // find surface
	  private Surface getSurface(SabrParameterType type)
	  {
		switch (type.innerEnumValue)
		{
		  case SabrParameterType.InnerEnum.ALPHA:
			return parameters.AlphaSurface;
		  case SabrParameterType.InnerEnum.BETA:
			return parameters.BetaSurface;
		  case SabrParameterType.InnerEnum.RHO:
			return parameters.RhoSurface;
		  case SabrParameterType.InnerEnum.NU:
			return parameters.NuSurface;
		  case SabrParameterType.InnerEnum.SHIFT:
			return parameters.ShiftSurface;
		  default:
			throw new System.InvalidOperationException("Invalid enum value");
		}
	  }

	  //-------------------------------------------------------------------------
	  public double price(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = parameters.shift(expiry, tenor);
		return BlackFormulaRepository.price(forward + shift, strike + shift, expiry, volatility, putCall.Call);
	  }

	  public double priceDelta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = parameters.shift(expiry, tenor);
		return BlackFormulaRepository.delta(forward + shift, strike + shift, expiry, volatility, putCall.Call);
	  }

	  public double priceGamma(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = parameters.shift(expiry, tenor);
		return BlackFormulaRepository.gamma(forward + shift, strike + shift, expiry, volatility);
	  }

	  public double priceTheta(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = parameters.shift(expiry, tenor);
		return BlackFormulaRepository.driftlessTheta(forward + shift, strike + shift, expiry, volatility);
	  }

	  public double priceVega(double expiry, double tenor, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = parameters.shift(expiry, tenor);
		return BlackFormulaRepository.vega(forward + shift, strike + shift, expiry, volatility);
	  }

	  //-------------------------------------------------------------------------
	  public double relativeTime(ZonedDateTime dateTime)
	  {
		ArgChecker.notNull(dateTime, "dateTime");
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		LocalDate date = dateTime.toLocalDate();
		return DayCount.relativeYearFraction(valuationDate, date);
	  }

	  public double tenor(LocalDate startDate, LocalDate endDate)
	  {
		// rounded number of months. the rounding is to ensure that an integer number of year even with holidays/leap year
		return (long)Math.Round((endDate.toEpochDay() - startDate.toEpochDay()) / 365.25 * 12, MidpointRounding.AwayFromZero) / 12;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrParametersSwaptionVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SabrParametersSwaptionVolatilities.Meta meta()
	  {
		return SabrParametersSwaptionVolatilities.Meta.INSTANCE;
	  }

	  static SabrParametersSwaptionVolatilities()
	  {
		MetaBean.register(SabrParametersSwaptionVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static SabrParametersSwaptionVolatilities.Builder builder()
	  {
		return new SabrParametersSwaptionVolatilities.Builder();
	  }

	  private SabrParametersSwaptionVolatilities(SwaptionVolatilitiesName name, FixedIborSwapConvention convention, ZonedDateTime valuationDateTime, SabrInterestRateParameters parameters, IList<DoubleArray> dataSensitivityAlpha, IList<DoubleArray> dataSensitivityBeta, IList<DoubleArray> dataSensitivityRho, IList<DoubleArray> dataSensitivityNu)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(convention, "convention");
		JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		JodaBeanUtils.notNull(parameters, "parameters");
		this.name = name;
		this.convention = convention;
		this.valuationDateTime = valuationDateTime;
		this.parameters = parameters;
		this.dataSensitivityAlpha = (dataSensitivityAlpha != null ? ImmutableList.copyOf(dataSensitivityAlpha) : null);
		this.dataSensitivityBeta = (dataSensitivityBeta != null ? ImmutableList.copyOf(dataSensitivityBeta) : null);
		this.dataSensitivityRho = (dataSensitivityRho != null ? ImmutableList.copyOf(dataSensitivityRho) : null);
		this.dataSensitivityNu = (dataSensitivityNu != null ? ImmutableList.copyOf(dataSensitivityNu) : null);
	  }

	  public override SabrParametersSwaptionVolatilities.Meta metaBean()
	  {
		return SabrParametersSwaptionVolatilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwaptionVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the swap convention that the volatilities are to be used for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedIborSwapConvention Convention
	  {
		  get
		  {
			return convention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date-time.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZonedDateTime ValuationDateTime
	  {
		  get
		  {
			return valuationDateTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the SABR model parameters.
	  /// <para>
	  /// Each model parameter of SABR model is a surface.
	  /// The x-value of the surface is the expiry, as a year fraction.
	  /// The y-value of the surface is the swap tenor, as a year fraction rounded to the month.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SabrInterestRateParameters Parameters
	  {
		  get
		  {
			return parameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sensitivity of the Alpha parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ImmutableList<DoubleArray>> DataSensitivityAlpha
	  {
		  get
		  {
			return Optional.ofNullable(dataSensitivityAlpha);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sensitivity of the Beta parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ImmutableList<DoubleArray>> DataSensitivityBeta
	  {
		  get
		  {
			return Optional.ofNullable(dataSensitivityBeta);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sensitivity of the Rho parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ImmutableList<DoubleArray>> DataSensitivityRho
	  {
		  get
		  {
			return Optional.ofNullable(dataSensitivityRho);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sensitivity of the Nu parameters to the raw data used for calibration.
	  /// <para>
	  /// The order of the sensitivities have to be coherent with the surface parameter metadata.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ImmutableList<DoubleArray>> DataSensitivityNu
	  {
		  get
		  {
			return Optional.ofNullable(dataSensitivityNu);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SabrParametersSwaptionVolatilities other = (SabrParametersSwaptionVolatilities) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(convention, other.convention) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(parameters, other.parameters) && JodaBeanUtils.equal(dataSensitivityAlpha, other.dataSensitivityAlpha) && JodaBeanUtils.equal(dataSensitivityBeta, other.dataSensitivityBeta) && JodaBeanUtils.equal(dataSensitivityRho, other.dataSensitivityRho) && JodaBeanUtils.equal(dataSensitivityNu, other.dataSensitivityNu);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dataSensitivityAlpha);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dataSensitivityBeta);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dataSensitivityRho);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dataSensitivityNu);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("SabrParametersSwaptionVolatilities{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(convention).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("parameters").Append('=').Append(parameters).Append(',').Append(' ');
		buf.Append("dataSensitivityAlpha").Append('=').Append(dataSensitivityAlpha).Append(',').Append(' ');
		buf.Append("dataSensitivityBeta").Append('=').Append(dataSensitivityBeta).Append(',').Append(' ');
		buf.Append("dataSensitivityRho").Append('=').Append(dataSensitivityRho).Append(',').Append(' ');
		buf.Append("dataSensitivityNu").Append('=').Append(JodaBeanUtils.ToString(dataSensitivityNu));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrParametersSwaptionVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(SabrParametersSwaptionVolatilities), typeof(SwaptionVolatilitiesName));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(SabrParametersSwaptionVolatilities), typeof(FixedIborSwapConvention));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(SabrParametersSwaptionVolatilities), typeof(ZonedDateTime));
			  parameters_Renamed = DirectMetaProperty.ofImmutable(this, "parameters", typeof(SabrParametersSwaptionVolatilities), typeof(SabrInterestRateParameters));
			  dataSensitivityAlpha_Renamed = DirectMetaProperty.ofImmutable(this, "dataSensitivityAlpha", typeof(SabrParametersSwaptionVolatilities), (Type) typeof(ImmutableList));
			  dataSensitivityBeta_Renamed = DirectMetaProperty.ofImmutable(this, "dataSensitivityBeta", typeof(SabrParametersSwaptionVolatilities), (Type) typeof(ImmutableList));
			  dataSensitivityRho_Renamed = DirectMetaProperty.ofImmutable(this, "dataSensitivityRho", typeof(SabrParametersSwaptionVolatilities), (Type) typeof(ImmutableList));
			  dataSensitivityNu_Renamed = DirectMetaProperty.ofImmutable(this, "dataSensitivityNu", typeof(SabrParametersSwaptionVolatilities), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "convention", "valuationDateTime", "parameters", "dataSensitivityAlpha", "dataSensitivityBeta", "dataSensitivityRho", "dataSensitivityNu");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwaptionVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedIborSwapConvention> convention_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> valuationDateTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameters} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SabrInterestRateParameters> parameters_Renamed;
		/// <summary>
		/// The meta-property for the {@code dataSensitivityAlpha} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray>> dataSensitivityAlpha = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "dataSensitivityAlpha", SabrParametersSwaptionVolatilities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleArray>> dataSensitivityAlpha_Renamed;
		/// <summary>
		/// The meta-property for the {@code dataSensitivityBeta} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray>> dataSensitivityBeta = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "dataSensitivityBeta", SabrParametersSwaptionVolatilities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleArray>> dataSensitivityBeta_Renamed;
		/// <summary>
		/// The meta-property for the {@code dataSensitivityRho} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray>> dataSensitivityRho = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "dataSensitivityRho", SabrParametersSwaptionVolatilities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleArray>> dataSensitivityRho_Renamed;
		/// <summary>
		/// The meta-property for the {@code dataSensitivityNu} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray>> dataSensitivityNu = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "dataSensitivityNu", SabrParametersSwaptionVolatilities.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleArray>> dataSensitivityNu_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "convention", "valuationDateTime", "parameters", "dataSensitivityAlpha", "dataSensitivityBeta", "dataSensitivityRho", "dataSensitivityNu");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
			case 1650101705: // dataSensitivityAlpha
			  return dataSensitivityAlpha_Renamed;
			case -85295067: // dataSensitivityBeta
			  return dataSensitivityBeta_Renamed;
			case 967095332: // dataSensitivityRho
			  return dataSensitivityRho_Renamed;
			case -1077182148: // dataSensitivityNu
			  return dataSensitivityNu_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SabrParametersSwaptionVolatilities.Builder builder()
		{
		  return new SabrParametersSwaptionVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SabrParametersSwaptionVolatilities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwaptionVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedIborSwapConvention> convention()
		{
		  return convention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> valuationDateTime()
		{
		  return valuationDateTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameters} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SabrInterestRateParameters> parameters()
		{
		  return parameters_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dataSensitivityAlpha} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleArray>> dataSensitivityAlpha()
		{
		  return dataSensitivityAlpha_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dataSensitivityBeta} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleArray>> dataSensitivityBeta()
		{
		  return dataSensitivityBeta_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dataSensitivityRho} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleArray>> dataSensitivityRho()
		{
		  return dataSensitivityRho_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dataSensitivityNu} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleArray>> dataSensitivityNu()
		{
		  return dataSensitivityNu_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((SabrParametersSwaptionVolatilities) bean).Name;
			case 2039569265: // convention
			  return ((SabrParametersSwaptionVolatilities) bean).Convention;
			case -949589828: // valuationDateTime
			  return ((SabrParametersSwaptionVolatilities) bean).ValuationDateTime;
			case 458736106: // parameters
			  return ((SabrParametersSwaptionVolatilities) bean).Parameters;
			case 1650101705: // dataSensitivityAlpha
			  return ((SabrParametersSwaptionVolatilities) bean).dataSensitivityAlpha;
			case -85295067: // dataSensitivityBeta
			  return ((SabrParametersSwaptionVolatilities) bean).dataSensitivityBeta;
			case 967095332: // dataSensitivityRho
			  return ((SabrParametersSwaptionVolatilities) bean).dataSensitivityRho;
			case -1077182148: // dataSensitivityNu
			  return ((SabrParametersSwaptionVolatilities) bean).dataSensitivityNu;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code SabrParametersSwaptionVolatilities}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<SabrParametersSwaptionVolatilities>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwaptionVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedIborSwapConvention convention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime valuationDateTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SabrInterestRateParameters parameters_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<DoubleArray> dataSensitivityAlpha_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<DoubleArray> dataSensitivityBeta_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<DoubleArray> dataSensitivityRho_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<DoubleArray> dataSensitivityNu_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(SabrParametersSwaptionVolatilities beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.convention_Renamed = beanToCopy.Convention;
		  this.valuationDateTime_Renamed = beanToCopy.ValuationDateTime;
		  this.parameters_Renamed = beanToCopy.Parameters;
		  this.dataSensitivityAlpha_Renamed = beanToCopy.dataSensitivityAlpha;
		  this.dataSensitivityBeta_Renamed = beanToCopy.dataSensitivityBeta;
		  this.dataSensitivityRho_Renamed = beanToCopy.dataSensitivityRho;
		  this.dataSensitivityNu_Renamed = beanToCopy.dataSensitivityNu;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 458736106: // parameters
			  return parameters_Renamed;
			case 1650101705: // dataSensitivityAlpha
			  return dataSensitivityAlpha_Renamed;
			case -85295067: // dataSensitivityBeta
			  return dataSensitivityBeta_Renamed;
			case 967095332: // dataSensitivityRho
			  return dataSensitivityRho_Renamed;
			case -1077182148: // dataSensitivityNu
			  return dataSensitivityNu_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (SwaptionVolatilitiesName) newValue;
			  break;
			case 2039569265: // convention
			  this.convention_Renamed = (FixedIborSwapConvention) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime_Renamed = (ZonedDateTime) newValue;
			  break;
			case 458736106: // parameters
			  this.parameters_Renamed = (SabrInterestRateParameters) newValue;
			  break;
			case 1650101705: // dataSensitivityAlpha
			  this.dataSensitivityAlpha_Renamed = (IList<DoubleArray>) newValue;
			  break;
			case -85295067: // dataSensitivityBeta
			  this.dataSensitivityBeta_Renamed = (IList<DoubleArray>) newValue;
			  break;
			case 967095332: // dataSensitivityRho
			  this.dataSensitivityRho_Renamed = (IList<DoubleArray>) newValue;
			  break;
			case -1077182148: // dataSensitivityNu
			  this.dataSensitivityNu_Renamed = (IList<DoubleArray>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override SabrParametersSwaptionVolatilities build()
		{
		  return new SabrParametersSwaptionVolatilities(name_Renamed, convention_Renamed, valuationDateTime_Renamed, parameters_Renamed, dataSensitivityAlpha_Renamed, dataSensitivityBeta_Renamed, dataSensitivityRho_Renamed, dataSensitivityNu_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(SwaptionVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the swap convention that the volatilities are to be used for. </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(FixedIborSwapConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		/// <summary>
		/// Sets the valuation date-time.
		/// <para>
		/// The volatilities are calibrated for this date-time.
		/// </para>
		/// </summary>
		/// <param name="valuationDateTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDateTime(ZonedDateTime valuationDateTime)
		{
		  JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		  this.valuationDateTime_Renamed = valuationDateTime;
		  return this;
		}

		/// <summary>
		/// Sets the SABR model parameters.
		/// <para>
		/// Each model parameter of SABR model is a surface.
		/// The x-value of the surface is the expiry, as a year fraction.
		/// The y-value of the surface is the swap tenor, as a year fraction rounded to the month.
		/// </para>
		/// </summary>
		/// <param name="parameters">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameters(SabrInterestRateParameters parameters)
		{
		  JodaBeanUtils.notNull(parameters, "parameters");
		  this.parameters_Renamed = parameters;
		  return this;
		}

		/// <summary>
		/// Sets the sensitivity of the Alpha parameters to the raw data used for calibration.
		/// <para>
		/// The order of the sensitivities have to be coherent with the surface parameter metadata.
		/// </para>
		/// </summary>
		/// <param name="dataSensitivityAlpha">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityAlpha(IList<DoubleArray> dataSensitivityAlpha)
		{
		  this.dataSensitivityAlpha_Renamed = dataSensitivityAlpha;
		  return this;
		}

		/// <summary>
		/// Sets the {@code dataSensitivityAlpha} property in the builder
		/// from an array of objects. </summary>
		/// <param name="dataSensitivityAlpha">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityAlpha(params DoubleArray[] dataSensitivityAlpha)
		{
		  return this.dataSensitivityAlpha(ImmutableList.copyOf(dataSensitivityAlpha));
		}

		/// <summary>
		/// Sets the sensitivity of the Beta parameters to the raw data used for calibration.
		/// <para>
		/// The order of the sensitivities have to be coherent with the surface parameter metadata.
		/// </para>
		/// </summary>
		/// <param name="dataSensitivityBeta">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityBeta(IList<DoubleArray> dataSensitivityBeta)
		{
		  this.dataSensitivityBeta_Renamed = dataSensitivityBeta;
		  return this;
		}

		/// <summary>
		/// Sets the {@code dataSensitivityBeta} property in the builder
		/// from an array of objects. </summary>
		/// <param name="dataSensitivityBeta">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityBeta(params DoubleArray[] dataSensitivityBeta)
		{
		  return this.dataSensitivityBeta(ImmutableList.copyOf(dataSensitivityBeta));
		}

		/// <summary>
		/// Sets the sensitivity of the Rho parameters to the raw data used for calibration.
		/// <para>
		/// The order of the sensitivities have to be coherent with the surface parameter metadata.
		/// </para>
		/// </summary>
		/// <param name="dataSensitivityRho">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityRho(IList<DoubleArray> dataSensitivityRho)
		{
		  this.dataSensitivityRho_Renamed = dataSensitivityRho;
		  return this;
		}

		/// <summary>
		/// Sets the {@code dataSensitivityRho} property in the builder
		/// from an array of objects. </summary>
		/// <param name="dataSensitivityRho">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityRho(params DoubleArray[] dataSensitivityRho)
		{
		  return this.dataSensitivityRho(ImmutableList.copyOf(dataSensitivityRho));
		}

		/// <summary>
		/// Sets the sensitivity of the Nu parameters to the raw data used for calibration.
		/// <para>
		/// The order of the sensitivities have to be coherent with the surface parameter metadata.
		/// </para>
		/// </summary>
		/// <param name="dataSensitivityNu">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityNu(IList<DoubleArray> dataSensitivityNu)
		{
		  this.dataSensitivityNu_Renamed = dataSensitivityNu;
		  return this;
		}

		/// <summary>
		/// Sets the {@code dataSensitivityNu} property in the builder
		/// from an array of objects. </summary>
		/// <param name="dataSensitivityNu">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dataSensitivityNu(params DoubleArray[] dataSensitivityNu)
		{
		  return this.dataSensitivityNu(ImmutableList.copyOf(dataSensitivityNu));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("SabrParametersSwaptionVolatilities.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime_Renamed)).Append(',').Append(' ');
		  buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters_Renamed)).Append(',').Append(' ');
		  buf.Append("dataSensitivityAlpha").Append('=').Append(JodaBeanUtils.ToString(dataSensitivityAlpha_Renamed)).Append(',').Append(' ');
		  buf.Append("dataSensitivityBeta").Append('=').Append(JodaBeanUtils.ToString(dataSensitivityBeta_Renamed)).Append(',').Append(' ');
		  buf.Append("dataSensitivityRho").Append('=').Append(JodaBeanUtils.ToString(dataSensitivityRho_Renamed)).Append(',').Append(' ');
		  buf.Append("dataSensitivityNu").Append('=').Append(JodaBeanUtils.ToString(dataSensitivityNu_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}