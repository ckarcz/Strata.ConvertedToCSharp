using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.YEAR_FRACTION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.ZERO_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.CurveInfoType.DAY_COUNT;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// ISDA compliant zero rate discount factors.
	/// <para>
	/// This is used to price credit default swaps under ISDA standard model. 
	/// </para>
	/// <para>
	/// The underlying curve must be zero rate curve represented by {@code InterpolatedNodalCurve} for multiple data points 
	/// and {@code InterpolatedNodalCurve} for a single data point. 
	/// The zero rates must be interpolated by {@code ProductLinearCurveInterpolator} and extrapolated by 
	/// {@code FlatCurveExtrapolator} on the left and {@code ProductLinearCurveExtrapolator} on the right.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IsdaCreditDiscountFactors implements CreditDiscountFactors, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IsdaCreditDiscountFactors : CreditDiscountFactors, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The valuation date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The underlying curve.
	  /// <para>
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.NodalCurve curve;
	  private readonly NodalCurve curve;
	  /// <summary>
	  /// The day count convention of the curve.
	  /// </summary>
	  private readonly DayCount dayCount; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance from the underlying curve.
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="curve">  the underlying curve </param>
	  /// <returns> the instance </returns>
	  public static IsdaCreditDiscountFactors of(Currency currency, LocalDate valuationDate, NodalCurve curve)
	  {

		curve.Metadata.XValueType.checkEquals(YEAR_FRACTION, "Incorrect x-value type for zero-rate discount curve");
		curve.Metadata.YValueType.checkEquals(ZERO_RATE, "Incorrect y-value type for zero-rate discount curve");
		if (curve is InterpolatedNodalCurve)
		{
		  InterpolatedNodalCurve interpolatedCurve = (InterpolatedNodalCurve) curve;
		  ArgChecker.isTrue(interpolatedCurve.Interpolator.Equals(CurveInterpolators.PRODUCT_LINEAR), "Interpolator must be PRODUCT_LINEAR");
		  ArgChecker.isTrue(interpolatedCurve.ExtrapolatorLeft.Equals(CurveExtrapolators.FLAT), "Left extrapolator must be FLAT");
		  ArgChecker.isTrue(interpolatedCurve.ExtrapolatorRight.Equals(CurveExtrapolators.PRODUCT_LINEAR), "Right extrapolator must be PRODUCT_LINEAR");
		}
		else
		{
		  ArgChecker.isTrue(curve is ConstantNodalCurve, "the underlying curve must be InterpolatedNodalCurve or ConstantNodalCurve");
		}
		return new IsdaCreditDiscountFactors(currency, valuationDate, curve);
	  }

	  /// <summary>
	  /// Creates an instance from year fraction and zero rate values.
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="curveName">  the curve name </param>
	  /// <param name="yearFractions">  the year fractions </param>
	  /// <param name="zeroRates">  the zero rates </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the instance </returns>
	  public static IsdaCreditDiscountFactors of(Currency currency, LocalDate valuationDate, CurveName curveName, DoubleArray yearFractions, DoubleArray zeroRates, DayCount dayCount)
	  {

		ArgChecker.notNull(yearFractions, "yearFractions");
		ArgChecker.notNull(zeroRates, "zeroRates");
		DefaultCurveMetadata metadata = DefaultCurveMetadata.builder().xValueType(YEAR_FRACTION).yValueType(ZERO_RATE).curveName(curveName).dayCount(dayCount).build();
		NodalCurve curve = (yearFractions.size() == 1 && zeroRates.size() == 1) ? ConstantNodalCurve.of(metadata, yearFractions.get(0), zeroRates.get(0)) : InterpolatedNodalCurve.of(metadata, yearFractions, zeroRates, CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);

		return new IsdaCreditDiscountFactors(currency, valuationDate, curve);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private IsdaCreditDiscountFactors(com.opengamma.strata.basics.currency.Currency currency, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.NodalCurve curve)
	  private IsdaCreditDiscountFactors(Currency currency, LocalDate valuationDate, NodalCurve curve)
	  {

		ArgChecker.notNull(currency, "currency");
		ArgChecker.notNull(valuationDate, "valuationDate");
		ArgChecker.notNull(curve, "curve");
		DayCount dayCount = curve.Metadata.findInfo(DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect curve metadata, missing DayCount"));

		this.currency = currency;
		this.valuationDate = valuationDate;
		this.curve = curve;
		this.dayCount = dayCount;
	  }

	  //-------------------------------------------------------------------------
	  public override bool IsdaCompliant
	  {
		  get
		  {
			return true;
		  }
	  };

	  //-------------------------------------------------------------------------
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

	  public DoubleArray ParameterKeys
	  {
		  get
		  {
			return curve.XValues;
		  }
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return curve.getParameterMetadata(parameterIndex);
	  }

	  public IsdaCreditDiscountFactors withParameter(int parameterIndex, double newValue)
	  {
		return withCurve(curve.withParameter(parameterIndex, newValue));
	  }

	  public IsdaCreditDiscountFactors withPerturbation(ParameterPerturbation perturbation)
	  {
		return withCurve(curve.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  public double relativeYearFraction(LocalDate date)
	  {
		return dayCount.relativeYearFraction(valuationDate, date);
	  }

	  public double discountFactor(double yearFraction)
	  {
		// convert zero rate to discount factor
		return Math.Exp(-yearFraction * curve.yValue(yearFraction));
	  }

	  public double zeroRate(double yearFraction)
	  {
		return curve.yValue(yearFraction);
	  }

	  //-------------------------------------------------------------------------
	  public ZeroRateSensitivity zeroRatePointSensitivity(double yearFraction, Currency sensitivityCurrency)
	  {
		double discountFactor = this.discountFactor(yearFraction);
		return ZeroRateSensitivity.of(currency, yearFraction, sensitivityCurrency, -discountFactor * yearFraction);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(ZeroRateSensitivity pointSensitivity)
	  {
		double yearFraction = pointSensitivity.YearFraction;
		UnitParameterSensitivity unitSens = curve.yValueParameterSensitivity(yearFraction);
		CurrencyParameterSensitivity curSens = unitSens.multipliedBy(pointSensitivity.Currency, pointSensitivity.Sensitivity);
		return CurrencyParameterSensitivities.of(curSens);
	  }

	  public CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		return CurrencyParameterSensitivities.of(curve.createParameterSensitivity(currency, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  public DiscountFactors toDiscountFactors()
	  {
		return DiscountFactors.of(currency, valuationDate, curve);
	  }

	  /// <summary>
	  /// Returns a new instance with a different curve.
	  /// </summary>
	  /// <param name="curve">  the new curve </param>
	  /// <returns> the new instance </returns>
	  public IsdaCreditDiscountFactors withCurve(NodalCurve curve)
	  {
		return IsdaCreditDiscountFactors.of(currency, valuationDate, curve);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IsdaCreditDiscountFactors}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IsdaCreditDiscountFactors.Meta meta()
	  {
		return IsdaCreditDiscountFactors.Meta.INSTANCE;
	  }

	  static IsdaCreditDiscountFactors()
	  {
		MetaBean.register(IsdaCreditDiscountFactors.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override IsdaCreditDiscountFactors.Meta metaBean()
	  {
		return IsdaCreditDiscountFactors.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency that the discount factors are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying curve.
	  /// <para>
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NodalCurve Curve
	  {
		  get
		  {
			return curve;
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
		  IsdaCreditDiscountFactors other = (IsdaCreditDiscountFactors) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(curve, other.curve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("IsdaCreditDiscountFactors{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("curve").Append('=').Append(JodaBeanUtils.ToString(curve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IsdaCreditDiscountFactors}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(IsdaCreditDiscountFactors), typeof(Currency));
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(IsdaCreditDiscountFactors), typeof(LocalDate));
			  curve_Renamed = DirectMetaProperty.ofImmutable(this, "curve", typeof(IsdaCreditDiscountFactors), typeof(NodalCurve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "valuationDate", "curve");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code curve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NodalCurve> curve_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "valuationDate", "curve");
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
			case 575402001: // currency
			  return currency_Renamed;
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case 95027439: // curve
			  return curve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IsdaCreditDiscountFactors> builder()
		public override BeanBuilder<IsdaCreditDiscountFactors> builder()
		{
		  return new IsdaCreditDiscountFactors.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IsdaCreditDiscountFactors);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code curve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NodalCurve> curve()
		{
		  return curve_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((IsdaCreditDiscountFactors) bean).Currency;
			case 113107279: // valuationDate
			  return ((IsdaCreditDiscountFactors) bean).ValuationDate;
			case 95027439: // curve
			  return ((IsdaCreditDiscountFactors) bean).Curve;
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
	  /// The bean-builder for {@code IsdaCreditDiscountFactors}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IsdaCreditDiscountFactors>
	  {

		internal Currency currency;
		internal LocalDate valuationDate;
		internal NodalCurve curve;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency;
			case 113107279: // valuationDate
			  return valuationDate;
			case 95027439: // curve
			  return curve;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case 95027439: // curve
			  this.curve = (NodalCurve) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IsdaCreditDiscountFactors build()
		{
		  return new IsdaCreditDiscountFactors(currency, valuationDate, curve);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("IsdaCreditDiscountFactors.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("curve").Append('=').Append(JodaBeanUtils.ToString(curve));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}