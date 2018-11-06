using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{


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
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using PriceIndexObservation = com.opengamma.strata.basics.index.PriceIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
	using InflationNodalCurve = com.opengamma.strata.market.curve.InflationNodalCurve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivities = com.opengamma.strata.market.param.UnitParameterSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides values for a Price index from a forward curve.
	/// <para>
	/// This provides historic and forward rates for a single <seealso cref="PriceIndex"/>, such as 'US-CPI-U'.
	/// </para>
	/// <para>
	/// This implementation is based on an underlying forward curve.
	/// Seasonality is included in the curve, see <seealso cref="InflationNodalCurve"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SimplePriceIndexValues implements PriceIndexValues, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SimplePriceIndexValues : PriceIndexValues, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.PriceIndex index;
		private readonly PriceIndex index;
	  /// <summary>
	  /// The valuation date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The underlying curve.
	  /// Each x-value on the curve is the number of months between the valuation month and the estimation month.
	  /// For example, zero represents the valuation month, one the next month and so on.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.NodalCurve curve;
	  private readonly NodalCurve curve;
	  /// <summary>
	  /// The monthly time-series of fixings.
	  /// This includes the known historical fixings and must not be empty.
	  /// <para>
	  /// Only one value is stored per month. The value is stored in the time-series on the
	  /// last date of each month (which may be a non-working day).
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings;
	  private readonly LocalDateDoubleTimeSeries fixings;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a curve with no seasonality adjustment.
	  /// <para>
	  /// Each x-value on the curve is the number of months between the valuation month and the estimation month.
	  /// For example, zero represents the valuation month, one the next month and so on.
	  /// </para>
	  /// <para>
	  /// The time-series contains one value per month and must have at least one entry.
	  /// The value is stored in the time-series on the last date of each month (which may be a non-working day).
	  /// </para>
	  /// <para>
	  /// The curve will be altered to be consistent with the time-series. The last element of the
	  /// series is added as the first point of the interpolated curve to ensure a coherent transition.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Price index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <param name="curve">  the underlying forward curve for index estimation </param>
	  /// <returns> the values instance </returns>
	  public static SimplePriceIndexValues of(PriceIndex index, LocalDate valuationDate, NodalCurve curve, LocalDateDoubleTimeSeries fixings)
	  {

		return new SimplePriceIndexValues(index, valuationDate, curve, fixings);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SimplePriceIndexValues(com.opengamma.strata.basics.index.PriceIndex index, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.NodalCurve curve, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings)
	  private SimplePriceIndexValues(PriceIndex index, LocalDate valuationDate, NodalCurve curve, LocalDateDoubleTimeSeries fixings)
	  {

		ArgChecker.isFalse(fixings.Empty, "Fixings must not be empty");
		curve.Metadata.XValueType.checkEquals(ValueType.MONTHS, "Incorrect x-value type for price curve");
		curve.Metadata.YValueType.checkEquals(ValueType.PRICE_INDEX, "Incorrect y-value type for price curve");
		this.index = ArgChecker.notNull(index, "index");
		this.valuationDate = ArgChecker.notNull(valuationDate, "valuationDate");
		this.fixings = ArgChecker.notNull(fixings, "fixings");
		this.curve = ArgChecker.notNull(curve, "curve");
	  }

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

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return curve.getParameterMetadata(parameterIndex);
	  }

	  public SimplePriceIndexValues withParameter(int parameterIndex, double newValue)
	  {
		return withCurve(curve.withParameter(parameterIndex, newValue));
	  }

	  public SimplePriceIndexValues withPerturbation(ParameterPerturbation perturbation)
	  {
		return withCurve(curve.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double value(PriceIndexObservation observation)
	  {
		YearMonth fixingMonth = observation.FixingMonth;
		// If fixing in the past, check time series and returns the historic month price index if present
		if (fixingMonth.isBefore(YearMonth.from(valuationDate)))
		{
		  double? fixing = fixings.get(fixingMonth.atEndOfMonth());
		  if (fixing.HasValue)
		  {
			return fixing.Value;
		  }
		}
		// otherwise, return the estimate from the curve.
		double nbMonth = numberOfMonths(fixingMonth);
		return curve.yValue(nbMonth);
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder valuePointSensitivity(PriceIndexObservation observation)
	  {
		YearMonth fixingMonth = observation.FixingMonth;
		// If fixing in the past, check time series and returns the historic month price index if present
		if (fixingMonth.isBefore(YearMonth.from(valuationDate)))
		{
		  if (fixings.get(fixingMonth.atEndOfMonth()).HasValue)
		  {
			return PointSensitivityBuilder.none();
		  }
		}
		return InflationRateSensitivity.of(observation, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(InflationRateSensitivity pointSensitivity)
	  {
		UnitParameterSensitivities sens = unitParameterSensitivity(pointSensitivity.Observation.FixingMonth);
		return sens.multipliedBy(pointSensitivity.Currency, pointSensitivity.Sensitivity);
	  }

	  private UnitParameterSensitivities unitParameterSensitivity(YearMonth month)
	  {
		// If fixing in the past, check time series and returns the historic month price index if present
		if (month.isBefore(YearMonth.from(valuationDate)))
		{
		  if (fixings.get(month.atEndOfMonth()).HasValue)
		  {
			return UnitParameterSensitivities.empty();
		  }
		}
		double nbMonth = numberOfMonths(month);
		return UnitParameterSensitivities.of(curve.yValueParameterSensitivity(nbMonth));
	  }

	  public CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		return CurrencyParameterSensitivities.of(curve.createParameterSensitivity(currency, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new instance with a different curve. The new curve must include fixing.
	  /// </summary>
	  /// <param name="curve">  the new curve </param>
	  /// <returns> the new instance </returns>
	  public SimplePriceIndexValues withCurve(NodalCurve curve)
	  {
		return new SimplePriceIndexValues(index, valuationDate, curve, fixings);
	  }

	  private double numberOfMonths(YearMonth month)
	  {
		return YearMonth.from(valuationDate).until(month, MONTHS);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SimplePriceIndexValues}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SimplePriceIndexValues.Meta meta()
	  {
		return SimplePriceIndexValues.Meta.INSTANCE;
	  }

	  static SimplePriceIndexValues()
	  {
		MetaBean.register(SimplePriceIndexValues.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override SimplePriceIndexValues.Meta metaBean()
	  {
		return SimplePriceIndexValues.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index that the values are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PriceIndex Index
	  {
		  get
		  {
			return index;
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
	  /// Each x-value on the curve is the number of months between the valuation month and the estimation month.
	  /// For example, zero represents the valuation month, one the next month and so on. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NodalCurve Curve
	  {
		  get
		  {
			return curve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the monthly time-series of fixings.
	  /// This includes the known historical fixings and must not be empty.
	  /// <para>
	  /// Only one value is stored per month. The value is stored in the time-series on the
	  /// last date of each month (which may be a non-working day).
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDateDoubleTimeSeries Fixings
	  {
		  get
		  {
			return fixings;
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
		  SimplePriceIndexValues other = (SimplePriceIndexValues) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(curve, other.curve) && JodaBeanUtils.equal(fixings, other.fixings);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixings);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("SimplePriceIndexValues{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("curve").Append('=').Append(curve).Append(',').Append(' ');
		buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SimplePriceIndexValues}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(SimplePriceIndexValues), typeof(PriceIndex));
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(SimplePriceIndexValues), typeof(LocalDate));
			  curve_Renamed = DirectMetaProperty.ofImmutable(this, "curve", typeof(SimplePriceIndexValues), typeof(NodalCurve));
			  fixings_Renamed = DirectMetaProperty.ofImmutable(this, "fixings", typeof(SimplePriceIndexValues), typeof(LocalDateDoubleTimeSeries));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "valuationDate", "curve", "fixings");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PriceIndex> index_Renamed;
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
		/// The meta-property for the {@code fixings} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDateDoubleTimeSeries> fixings_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "valuationDate", "curve", "fixings");
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
			case 100346066: // index
			  return index_Renamed;
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case 95027439: // curve
			  return curve_Renamed;
			case -843784602: // fixings
			  return fixings_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SimplePriceIndexValues> builder()
		public override BeanBuilder<SimplePriceIndexValues> builder()
		{
		  return new SimplePriceIndexValues.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SimplePriceIndexValues);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PriceIndex> index()
		{
		  return index_Renamed;
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

		/// <summary>
		/// The meta-property for the {@code fixings} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDateDoubleTimeSeries> fixings()
		{
		  return fixings_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((SimplePriceIndexValues) bean).Index;
			case 113107279: // valuationDate
			  return ((SimplePriceIndexValues) bean).ValuationDate;
			case 95027439: // curve
			  return ((SimplePriceIndexValues) bean).Curve;
			case -843784602: // fixings
			  return ((SimplePriceIndexValues) bean).Fixings;
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
	  /// The bean-builder for {@code SimplePriceIndexValues}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SimplePriceIndexValues>
	  {

		internal PriceIndex index;
		internal LocalDate valuationDate;
		internal NodalCurve curve;
		internal LocalDateDoubleTimeSeries fixings;

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
			case 100346066: // index
			  return index;
			case 113107279: // valuationDate
			  return valuationDate;
			case 95027439: // curve
			  return curve;
			case -843784602: // fixings
			  return fixings;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index = (PriceIndex) newValue;
			  break;
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case 95027439: // curve
			  this.curve = (NodalCurve) newValue;
			  break;
			case -843784602: // fixings
			  this.fixings = (LocalDateDoubleTimeSeries) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SimplePriceIndexValues build()
		{
		  return new SimplePriceIndexValues(index, valuationDate, curve, fixings);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("SimplePriceIndexValues.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("curve").Append('=').Append(JodaBeanUtils.ToString(curve)).Append(',').Append(' ');
		  buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}