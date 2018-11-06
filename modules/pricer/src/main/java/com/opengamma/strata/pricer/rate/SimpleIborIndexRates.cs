using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
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
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// An Ibor index curve providing rates directly from a forward rates curve.
	/// <para>
	/// This provides historic and forward rates for a single <seealso cref="IborIndex"/>, such as 'GBP-LIBOR-3M'.
	/// </para>
	/// <para>
	/// This implementation is based on an underlying curve that is stored with fixing and direct forward rates.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SimpleIborIndexRates implements IborIndexRates, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SimpleIborIndexRates : IborIndexRates, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
		private readonly IborIndex index;
	  /// <summary>
	  /// The valuation date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The underlying forward curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve curve;
	  private readonly Curve curve;
	  /// <summary>
	  /// The time-series of fixings, defaulted to an empty time-series.
	  /// This includes the known historical fixings and may be empty.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings;
	  private readonly LocalDateDoubleTimeSeries fixings;
	  /// <summary>
	  /// The day count convention of the curve.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property

	  /// <summary>
	  /// Obtains an instance from a curve, with an empty time-series of fixings.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#YEAR_FRACTION year fractions"/> with
	  /// the day count specified. The y-values must be <seealso cref="ValueType#FORWARD_RATE forward rates"/>.
	  /// A suitable metadata instance for the curve can be created by <seealso cref="Curves#forwardRates(String, DayCount)"/>.
	  /// In the curve the Ibor rates are indexed by the maturity date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="curve">  the curve of forward rates </param>
	  /// <returns> the rates view </returns>
	  public static SimpleIborIndexRates of(IborIndex index, LocalDate valuationDate, Curve curve)
	  {
		return new SimpleIborIndexRates(index, valuationDate, curve, LocalDateDoubleTimeSeries.empty());
	  }

	  /// <summary>
	  /// Obtains an instance from a curve and time-series of fixing.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#YEAR_FRACTION year fractions"/> with
	  /// the day count specified. The y-values must be <seealso cref="ValueType#FORWARD_RATE forward rates"/>.
	  /// In the curve the Ibor rates are indexed by the maturity date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="curve">  the curve of forward rates </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <returns> the rates view </returns>
	  public static SimpleIborIndexRates of(IborIndex index, LocalDate valuationDate, Curve curve, LocalDateDoubleTimeSeries fixings)
	  {
		return new SimpleIborIndexRates(index, valuationDate, curve, fixings);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private SimpleIborIndexRates(com.opengamma.strata.basics.index.IborIndex index, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve curve, com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings)
	  private SimpleIborIndexRates(IborIndex index, LocalDate valuationDate, Curve curve, LocalDateDoubleTimeSeries fixings)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(valuationDate, "valuationDate");
		ArgChecker.notNull(curve, "curve");
		ArgChecker.notNull(fixings, "fixings");
		curve.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for ibor curve");
		curve.Metadata.YValueType.checkEquals(ValueType.FORWARD_RATE, "Incorrect y-value type for ibor curve");
		DayCount dayCount = curve.Metadata.findInfo(CurveInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect curve metadata, missing DayCount"));

		this.valuationDate = valuationDate;
		this.index = index;
		this.curve = curve;
		this.fixings = fixings;
		this.dayCount = dayCount;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new SimpleIborIndexRates(index, valuationDate, curve, fixings);
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

	  public SimpleIborIndexRates withParameter(int parameterIndex, double newValue)
	  {
		return withCurve(curve.withParameter(parameterIndex, newValue));
	  }

	  public SimpleIborIndexRates withPerturbation(ParameterPerturbation perturbation)
	  {
		return withCurve(curve.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double rate(IborIndexObservation observation)
	  {
		if (!observation.FixingDate.isAfter(ValuationDate))
		{
		  return historicRate(observation);
		}
		return rateIgnoringFixings(observation);
	  }

	  // historic rate
	  private double historicRate(IborIndexObservation observation)
	  {
		LocalDate fixingDate = observation.FixingDate;
		double? fixedRate = fixings.get(fixingDate);
		if (fixedRate.HasValue)
		{
		  return fixedRate.Value;
		}
		else if (fixingDate.isBefore(ValuationDate))
		{ // the fixing is required
		  if (fixings.Empty)
		  {
			throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}, no time-series supplied", index, fixingDate));
		  }
		  throw new System.ArgumentException(Messages.format("Unable to get fixing for {} on date {}", index, fixingDate));
		}
		else
		{
		  return rateIgnoringFixings(observation);
		}
	  }

	  public double rateIgnoringFixings(IborIndexObservation observation)
	  {
		double relativeYearFraction = this.relativeYearFraction(observation.MaturityDate);
		return curve.yValue(relativeYearFraction);
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder ratePointSensitivity(IborIndexObservation observation)
	  {
		LocalDate fixingDate = observation.FixingDate;
		LocalDate valuationDate = ValuationDate;
		if (fixingDate.isBefore(valuationDate) || (fixingDate.Equals(valuationDate) && fixings.get(fixingDate).HasValue))
		{
		  return PointSensitivityBuilder.none();
		}
		return IborRateSensitivity.of(observation, 1d);
	  }

	  public PointSensitivityBuilder rateIgnoringFixingsPointSensitivity(IborIndexObservation observation)
	  {
		return IborRateSensitivity.of(observation, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(IborRateSensitivity pointSensitivity)
	  {
		LocalDate maturityDate = pointSensitivity.Observation.MaturityDate;
		double relativeYearFraction = this.relativeYearFraction(maturityDate);
		UnitParameterSensitivity unitSensitivity = curve.yValueParameterSensitivity(relativeYearFraction);
		CurrencyParameterSensitivity sensitivity = unitSensitivity.multipliedBy(pointSensitivity.Currency, pointSensitivity.Sensitivity);
		return CurrencyParameterSensitivities.of(sensitivity);
	  }

	  public CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		return CurrencyParameterSensitivities.of(curve.createParameterSensitivity(currency, sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new instance with a different curve.
	  /// </summary>
	  /// <param name="curve">  the new curve </param>
	  /// <returns> the new instance </returns>
	  public SimpleIborIndexRates withCurve(Curve curve)
	  {
		return new SimpleIborIndexRates(index, valuationDate, curve, fixings);
	  }

	  // calculate the relative time between the valuation date and the specified date using the day count of the curve
	  private double relativeYearFraction(LocalDate date)
	  {
		return dayCount.relativeYearFraction(valuationDate, date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SimpleIborIndexRates}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SimpleIborIndexRates.Meta meta()
	  {
		return SimpleIborIndexRates.Meta.INSTANCE;
	  }

	  static SimpleIborIndexRates()
	  {
		MetaBean.register(SimpleIborIndexRates.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override SimpleIborIndexRates.Meta metaBean()
	  {
		return SimpleIborIndexRates.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index that the rates are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
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
	  /// Gets the underlying forward curve. </summary>
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
	  /// Gets the time-series of fixings, defaulted to an empty time-series.
	  /// This includes the known historical fixings and may be empty. </summary>
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
		  SimpleIborIndexRates other = (SimpleIborIndexRates) obj;
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
		buf.Append("SimpleIborIndexRates{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("curve").Append('=').Append(curve).Append(',').Append(' ');
		buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SimpleIborIndexRates}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(SimpleIborIndexRates), typeof(IborIndex));
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(SimpleIborIndexRates), typeof(LocalDate));
			  curve_Renamed = DirectMetaProperty.ofImmutable(this, "curve", typeof(SimpleIborIndexRates), typeof(Curve));
			  fixings_Renamed = DirectMetaProperty.ofImmutable(this, "fixings", typeof(SimpleIborIndexRates), typeof(LocalDateDoubleTimeSeries));
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
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code curve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> curve_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SimpleIborIndexRates> builder()
		public override BeanBuilder<SimpleIborIndexRates> builder()
		{
		  return new SimpleIborIndexRates.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SimpleIborIndexRates);
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
		public MetaProperty<IborIndex> index()
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
		public MetaProperty<Curve> curve()
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
			  return ((SimpleIborIndexRates) bean).Index;
			case 113107279: // valuationDate
			  return ((SimpleIborIndexRates) bean).ValuationDate;
			case 95027439: // curve
			  return ((SimpleIborIndexRates) bean).Curve;
			case -843784602: // fixings
			  return ((SimpleIborIndexRates) bean).Fixings;
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
	  /// The bean-builder for {@code SimpleIborIndexRates}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SimpleIborIndexRates>
	  {

		internal IborIndex index;
		internal LocalDate valuationDate;
		internal Curve curve;
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
			  this.index = (IborIndex) newValue;
			  break;
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case 95027439: // curve
			  this.curve = (Curve) newValue;
			  break;
			case -843784602: // fixings
			  this.fixings = (LocalDateDoubleTimeSeries) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SimpleIborIndexRates build()
		{
		  return new SimpleIborIndexRates(index, valuationDate, curve, fixings);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("SimpleIborIndexRates.Builder{");
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