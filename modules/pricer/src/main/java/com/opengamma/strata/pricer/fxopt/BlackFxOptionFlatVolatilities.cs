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
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Volatility for FX options in the log-normal or Black model based on a curve.
	/// <para> 
	/// The volatility is represented by a curve on the expiry and the volatility
	/// is flat along the strike direction.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BlackFxOptionFlatVolatilities implements BlackFxOptionVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BlackFxOptionFlatVolatilities : BlackFxOptionVolatilities, ImmutableBean
	{
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
	  /// The Black volatility curve.
	  /// <para>
	  /// The x-values represent the expiry year-fraction.
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve curve;
	  private readonly Curve curve;
	  /// <summary>
	  /// The day count convention of the curve.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from an expiry-volatility curve and the date-time for which it is valid.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#BLACK_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="CurveInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable curve metadata can be created using
	  /// <seealso cref="Curves#blackVolatilityByExpiry(String, DayCount)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="curve">  the volatility curve </param>
	  /// <returns> the volatilities </returns>
	  public static BlackFxOptionFlatVolatilities of(CurrencyPair currencyPair, ZonedDateTime valuationDateTime, Curve curve)
	  {

		return new BlackFxOptionFlatVolatilities(currencyPair, valuationDateTime, curve);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private BlackFxOptionFlatVolatilities(com.opengamma.strata.basics.currency.CurrencyPair currencyPair, java.time.ZonedDateTime valuationDateTime, com.opengamma.strata.market.curve.Curve curve)
	  private BlackFxOptionFlatVolatilities(CurrencyPair currencyPair, ZonedDateTime valuationDateTime, Curve curve)
	  {

		ArgChecker.notNull(currencyPair, "currencyPair");
		ArgChecker.notNull(valuationDateTime, "valuationDateTime");
		ArgChecker.notNull(curve, "curve");
		curve.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for Black volatilities");
		curve.Metadata.YValueType.checkEquals(ValueType.BLACK_VOLATILITY, "Incorrect y-value type for Black volatilities");
		DayCount dayCount = curve.Metadata.findInfo(CurveInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect curve metadata, missing DayCount"));

		this.currencyPair = currencyPair;
		this.valuationDateTime = valuationDateTime;
		this.curve = curve;
		this.dayCount = dayCount;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new BlackFxOptionFlatVolatilities(currencyPair, valuationDateTime, curve);
	  }

	  //-------------------------------------------------------------------------
	  public FxOptionVolatilitiesName Name
	  {
		  get
		  {
			return FxOptionVolatilitiesName.of(curve.Name.Name);
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (curve.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(curve));
		}
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return curve.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return curve.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return curve.getParameterMetadata(parameterIndex);
	  }

	  public BlackFxOptionFlatVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new BlackFxOptionFlatVolatilities(currencyPair, valuationDateTime, curve.withParameter(parameterIndex, newValue));
	  }

	  public BlackFxOptionFlatVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new BlackFxOptionFlatVolatilities(currencyPair, valuationDateTime, curve.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(CurrencyPair currencyPair, double expiry, double strike, double forward)
	  {
		return curve.yValue(expiry);
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
		UnitParameterSensitivity unitSens = curve.yValueParameterSensitivity(expiry);
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
	  /// The meta-bean for {@code BlackFxOptionFlatVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BlackFxOptionFlatVolatilities.Meta meta()
	  {
		return BlackFxOptionFlatVolatilities.Meta.INSTANCE;
	  }

	  static BlackFxOptionFlatVolatilities()
	  {
		MetaBean.register(BlackFxOptionFlatVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BlackFxOptionFlatVolatilities.Builder builder()
	  {
		return new BlackFxOptionFlatVolatilities.Builder();
	  }

	  public override BlackFxOptionFlatVolatilities.Meta metaBean()
	  {
		return BlackFxOptionFlatVolatilities.Meta.INSTANCE;
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
	  /// Gets the Black volatility curve.
	  /// <para>
	  /// The x-values represent the expiry year-fraction.
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve Curve
	  {
		  get
		  {
			return curve;
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
		  BlackFxOptionFlatVolatilities other = (BlackFxOptionFlatVolatilities) obj;
		  return JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(curve, other.curve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("BlackFxOptionFlatVolatilities{");
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("curve").Append('=').Append(JodaBeanUtils.ToString(curve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionFlatVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(BlackFxOptionFlatVolatilities), typeof(CurrencyPair));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(BlackFxOptionFlatVolatilities), typeof(ZonedDateTime));
			  curve_Renamed = DirectMetaProperty.ofImmutable(this, "curve", typeof(BlackFxOptionFlatVolatilities), typeof(Curve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencyPair", "valuationDateTime", "curve");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

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
		/// The meta-property for the {@code curve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> curve_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencyPair", "valuationDateTime", "curve");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 95027439: // curve
			  return curve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BlackFxOptionFlatVolatilities.Builder builder()
		{
		  return new BlackFxOptionFlatVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BlackFxOptionFlatVolatilities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
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
		/// The meta-property for the {@code curve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> curve()
		{
		  return curve_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return ((BlackFxOptionFlatVolatilities) bean).CurrencyPair;
			case -949589828: // valuationDateTime
			  return ((BlackFxOptionFlatVolatilities) bean).ValuationDateTime;
			case 95027439: // curve
			  return ((BlackFxOptionFlatVolatilities) bean).Curve;
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
	  /// The bean-builder for {@code BlackFxOptionFlatVolatilities}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BlackFxOptionFlatVolatilities>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime valuationDateTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Curve curve_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(BlackFxOptionFlatVolatilities beanToCopy)
		{
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.valuationDateTime_Renamed = beanToCopy.ValuationDateTime;
		  this.curve_Renamed = beanToCopy.Curve;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 95027439: // curve
			  return curve_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime_Renamed = (ZonedDateTime) newValue;
			  break;
			case 95027439: // curve
			  this.curve_Renamed = (Curve) newValue;
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

		public override BlackFxOptionFlatVolatilities build()
		{
		  return new BlackFxOptionFlatVolatilities(currencyPair_Renamed, valuationDateTime_Renamed, curve_Renamed);
		}

		//-----------------------------------------------------------------------
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
		/// Sets the Black volatility curve.
		/// <para>
		/// The x-values represent the expiry year-fraction.
		/// The metadata of the curve must define a day count.
		/// </para>
		/// </summary>
		/// <param name="curve">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder curve(Curve curve)
		{
		  JodaBeanUtils.notNull(curve, "curve");
		  this.curve_Renamed = curve;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("BlackFxOptionFlatVolatilities.Builder{");
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime_Renamed)).Append(',').Append(' ');
		  buf.Append("curve").Append('=').Append(JodaBeanUtils.ToString(curve_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}