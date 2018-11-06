using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceInfoType = com.opengamma.strata.market.surface.SurfaceInfoType;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Volatility for FX options in the log-normal or Black model based on a surface.
	/// <para> 
	/// The volatility is represented by a surface on the expiry and strike value.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BlackFxOptionSurfaceVolatilities implements BlackFxOptionVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BlackFxOptionSurfaceVolatilities : BlackFxOptionVolatilities, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxOptionVolatilitiesName name;
		private readonly FxOptionVolatilitiesName name;
	  /// <summary>
	  /// The currency pair that the volatilities are for.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The valuation date-time.
	  /// All data items in this provider is calibrated for this date-time.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.ZonedDateTime valuationDateTime;
	  private readonly ZonedDateTime valuationDateTime;
	  /// <summary>
	  /// The Black volatility surface.
	  /// <para>
	  /// The x-values represent the expiry year-fraction.
	  /// The y-values represent the strike.
	  /// The metadata of the surface must define a day count.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface surface;
	  private readonly Surface surface;
	  /// <summary>
	  /// The day count convention of the curve.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the implied volatility surface and the date-time for which it is valid.
	  /// <para>
	  /// {@code FxOptionVolatilitiesName} is built from the name in {@code Surface}.
	  /// </para>
	  /// <para>
	  /// The surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surface must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#STRIKE"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#BLACK_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="SurfaceInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#blackVolatilityByExpiryStrike(String, DayCount)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="surface">  the volatility surface </param>
	  /// <returns> the volatilities </returns>
	  public static BlackFxOptionSurfaceVolatilities of(CurrencyPair currencyPair, ZonedDateTime valuationDateTime, Surface surface)
	  {

		FxOptionVolatilitiesName name = FxOptionVolatilitiesName.of(surface.Name.Name);
		return of(name, currencyPair, valuationDateTime, surface);
	  }

	  /// <summary>
	  /// Obtains an instance from the implied volatility surface and the date-time for which it is valid.
	  /// <para>
	  /// The surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surface must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#STRIKE"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#BLACK_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="SurfaceInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#blackVolatilityByExpiryStrike(String, DayCount)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="surface">  the volatility surface </param>
	  /// <returns> the volatilities </returns>
	  public static BlackFxOptionSurfaceVolatilities of(FxOptionVolatilitiesName name, CurrencyPair currencyPair, ZonedDateTime valuationDateTime, Surface surface)
	  {

		return new BlackFxOptionSurfaceVolatilities(name, currencyPair, valuationDateTime, surface);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.name_Renamed == null && builder.surface_Renamed != null)
		{
		  builder.name_Renamed = FxOptionVolatilitiesName.of(builder.surface_Renamed.Name.Name);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private BlackFxOptionSurfaceVolatilities(FxOptionVolatilitiesName name, com.opengamma.strata.basics.currency.CurrencyPair currencyPair, java.time.ZonedDateTime valuationDateTime, com.opengamma.strata.market.surface.Surface surface)
	  private BlackFxOptionSurfaceVolatilities(FxOptionVolatilitiesName name, CurrencyPair currencyPair, ZonedDateTime valuationDateTime, Surface surface)
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(currencyPair, "currencyPair");
		ArgChecker.notNull(valuationDateTime, "valuationDateTime");
		ArgChecker.notNull(surface, "surface");
		surface.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for Black volatilities");
		surface.Metadata.YValueType.checkEquals(ValueType.STRIKE, "Incorrect y-value type for Black volatilities");
		surface.Metadata.ZValueType.checkEquals(ValueType.BLACK_VOLATILITY, "Incorrect z-value type for Black volatilities");
		DayCount dayCount = surface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing DayCount"));

		this.name = name;
		this.currencyPair = currencyPair;
		this.valuationDateTime = valuationDateTime;
		this.surface = surface;
		this.dayCount = dayCount;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new BlackFxOptionSurfaceVolatilities(name, currencyPair, valuationDateTime, surface);
	  }

	  //-------------------------------------------------------------------------
	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (surface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(surface));
		}
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return surface.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return surface.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return surface.getParameterMetadata(parameterIndex);
	  }

	  public BlackFxOptionSurfaceVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new BlackFxOptionSurfaceVolatilities(name, currencyPair, valuationDateTime, surface.withParameter(parameterIndex, newValue));
	  }

	  public BlackFxOptionSurfaceVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new BlackFxOptionSurfaceVolatilities(name, currencyPair, valuationDateTime, surface.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(CurrencyPair currencyPair, double expiry, double strike, double forward)
	  {
		if (currencyPair.isInverse(this.currencyPair))
		{
		  return surface.zValue(expiry, 1d / strike);
		}
		return surface.zValue(expiry, strike);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is FxOptionSensitivity)
		  {
			FxOptionSensitivity pt = (FxOptionSensitivity) point;
			if (pt.VolatilitiesName.Equals(Name))
			{
			  sens = sens.combinedWith(parameterSensitivity(pt));
			}
		  }
		}
		return sens;
	  }

	  private CurrencyParameterSensitivity parameterSensitivity(FxOptionSensitivity point)
	  {
		double expiry = point.Expiry;
		double strike = point.CurrencyPair.isInverse(currencyPair) ? 1d / point.Strike : point.Strike;
		UnitParameterSensitivity unitSens = surface.zValueParameterSensitivity(expiry, strike);
		return unitSens.multipliedBy(point.Currency, point.Sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public double price(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return BlackFormulaRepository.price(forward, strike, expiry, volatility, putCall.Call);
	  }

	  //-------------------------------------------------------------------------
	  public double relativeTime(ZonedDateTime dateTime)
	  {
		ArgChecker.notNull(dateTime, "dateTime");
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		LocalDate date = dateTime.toLocalDate();
		return dayCount.relativeYearFraction(valuationDate, date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSurfaceVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BlackFxOptionSurfaceVolatilities.Meta meta()
	  {
		return BlackFxOptionSurfaceVolatilities.Meta.INSTANCE;
	  }

	  static BlackFxOptionSurfaceVolatilities()
	  {
		MetaBean.register(BlackFxOptionSurfaceVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BlackFxOptionSurfaceVolatilities.Builder builder()
	  {
		return new BlackFxOptionSurfaceVolatilities.Builder();
	  }

	  public override BlackFxOptionSurfaceVolatilities.Meta metaBean()
	  {
		return BlackFxOptionSurfaceVolatilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxOptionVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that the volatilities are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date-time.
	  /// All data items in this provider is calibrated for this date-time. </summary>
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
	  /// Gets the Black volatility surface.
	  /// <para>
	  /// The x-values represent the expiry year-fraction.
	  /// The y-values represent the strike.
	  /// The metadata of the surface must define a day count.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface Surface
	  {
		  get
		  {
			return surface;
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
		  BlackFxOptionSurfaceVolatilities other = (BlackFxOptionSurfaceVolatilities) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(surface, other.surface);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(surface);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("BlackFxOptionSurfaceVolatilities{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSurfaceVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(BlackFxOptionSurfaceVolatilities), typeof(FxOptionVolatilitiesName));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(BlackFxOptionSurfaceVolatilities), typeof(CurrencyPair));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(BlackFxOptionSurfaceVolatilities), typeof(ZonedDateTime));
			  surface_Renamed = DirectMetaProperty.ofImmutable(this, "surface", typeof(BlackFxOptionSurfaceVolatilities), typeof(Surface));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currencyPair", "valuationDateTime", "surface");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxOptionVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> valuationDateTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code surface} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Surface> surface_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currencyPair", "valuationDateTime", "surface");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case -1853231955: // surface
			  return surface_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BlackFxOptionSurfaceVolatilities.Builder builder()
		{
		  return new BlackFxOptionSurfaceVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BlackFxOptionSurfaceVolatilities);
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
		public MetaProperty<FxOptionVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> valuationDateTime()
		{
		  return valuationDateTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code surface} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Surface> surface()
		{
		  return surface_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((BlackFxOptionSurfaceVolatilities) bean).Name;
			case 1005147787: // currencyPair
			  return ((BlackFxOptionSurfaceVolatilities) bean).CurrencyPair;
			case -949589828: // valuationDateTime
			  return ((BlackFxOptionSurfaceVolatilities) bean).ValuationDateTime;
			case -1853231955: // surface
			  return ((BlackFxOptionSurfaceVolatilities) bean).Surface;
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
	  /// The bean-builder for {@code BlackFxOptionSurfaceVolatilities}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BlackFxOptionSurfaceVolatilities>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxOptionVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime valuationDateTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Surface surface_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(BlackFxOptionSurfaceVolatilities beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.valuationDateTime_Renamed = beanToCopy.ValuationDateTime;
		  this.surface_Renamed = beanToCopy.Surface;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case -1853231955: // surface
			  return surface_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (FxOptionVolatilitiesName) newValue;
			  break;
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime_Renamed = (ZonedDateTime) newValue;
			  break;
			case -1853231955: // surface
			  this.surface_Renamed = (Surface) newValue;
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

		public override BlackFxOptionSurfaceVolatilities build()
		{
		  preBuild(this);
		  return new BlackFxOptionSurfaceVolatilities(name_Renamed, currencyPair_Renamed, valuationDateTime_Renamed, surface_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the volatilities. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(FxOptionVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency pair that the volatilities are for. </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the valuation date-time.
		/// All data items in this provider is calibrated for this date-time. </summary>
		/// <param name="valuationDateTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDateTime(ZonedDateTime valuationDateTime)
		{
		  JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		  this.valuationDateTime_Renamed = valuationDateTime;
		  return this;
		}

		/// <summary>
		/// Sets the Black volatility surface.
		/// <para>
		/// The x-values represent the expiry year-fraction.
		/// The y-values represent the strike.
		/// The metadata of the surface must define a day count.
		/// </para>
		/// </summary>
		/// <param name="surface">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder surface(Surface surface)
		{
		  JodaBeanUtils.notNull(surface, "surface");
		  this.surface_Renamed = surface;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("BlackFxOptionSurfaceVolatilities.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime_Renamed)).Append(',').Append(' ');
		  buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}