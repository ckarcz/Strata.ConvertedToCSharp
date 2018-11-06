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
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// An Overnight index curve providing rates from discount factors.
	/// <para>
	/// This provides historic and forward rates for a single <seealso cref="OvernightIndex"/>, such as 'EUR-EONIA'.
	/// </para>
	/// <para>
	/// This implementation is based on an underlying curve that is stored with maturities
	/// and zero-coupon continuously-compounded rates.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DiscountOvernightIndexRates implements OvernightIndexRates, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DiscountOvernightIndexRates : OvernightIndexRates, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.OvernightIndex index;
		private readonly OvernightIndex index;
	  /// <summary>
	  /// The underlying discount factor curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.DiscountFactors discountFactors;
	  private readonly DiscountFactors discountFactors;
	  /// <summary>
	  /// The time-series of fixings, defaulted to an empty time-series.
	  /// This includes the known historical fixings and may be empty.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries fixings;
	  private readonly LocalDateDoubleTimeSeries fixings;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on discount factors with no historic fixings.
	  /// <para>
	  /// The forward curve is specified by an instance of <seealso cref="DiscountFactors"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Overnight index </param>
	  /// <param name="discountFactors">  the underlying discount factor forward curve </param>
	  /// <returns> the rates instance </returns>
	  public static DiscountOvernightIndexRates of(OvernightIndex index, DiscountFactors discountFactors)
	  {
		return of(index, discountFactors, LocalDateDoubleTimeSeries.empty());
	  }

	  /// <summary>
	  /// Obtains an instance based on discount factors and historic fixings.
	  /// <para>
	  /// The forward curve is specified by an instance of <seealso cref="DiscountFactors"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Overnight index </param>
	  /// <param name="discountFactors">  the underlying discount factor forward curve </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <returns> the rates instance </returns>
	  public static DiscountOvernightIndexRates of(OvernightIndex index, DiscountFactors discountFactors, LocalDateDoubleTimeSeries fixings)
	  {

		return new DiscountOvernightIndexRates(index, discountFactors, fixings);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fixings = LocalDateDoubleTimeSeries.empty();
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return discountFactors.ValuationDate;
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return discountFactors.findData(name);
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return discountFactors.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return discountFactors.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return discountFactors.getParameterMetadata(parameterIndex);
	  }

	  public DiscountOvernightIndexRates withParameter(int parameterIndex, double newValue)
	  {
		return withDiscountFactors(discountFactors.withParameter(parameterIndex, newValue));
	  }

	  public DiscountOvernightIndexRates withPerturbation(ParameterPerturbation perturbation)
	  {
		return withDiscountFactors(discountFactors.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double rate(OvernightIndexObservation observation)
	  {
		if (!observation.PublicationDate.isAfter(ValuationDate))
		{
		  return historicRate(observation);
		}
		return rateIgnoringFixings(observation);
	  }

	  // historic rate
	  private double historicRate(OvernightIndexObservation observation)
	  {
		LocalDate fixingDate = observation.FixingDate;
		double? fixedRate = fixings.get(fixingDate);
		if (fixedRate.HasValue)
		{
		  return fixedRate.Value;
		}
		else if (observation.PublicationDate.isBefore(ValuationDate))
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

	  public double rateIgnoringFixings(OvernightIndexObservation observation)
	  {
		LocalDate effectiveDate = observation.EffectiveDate;
		LocalDate maturityDate = observation.MaturityDate;
		double accrualFactor = observation.YearFraction;
		return simplyCompoundForwardRate(effectiveDate, maturityDate, accrualFactor);
	  }

	  // compounded from discount factors
	  private double simplyCompoundForwardRate(LocalDate startDate, LocalDate endDate, double accrualFactor)
	  {
		return (discountFactors.discountFactor(startDate) / discountFactors.discountFactor(endDate) - 1) / accrualFactor;
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder ratePointSensitivity(OvernightIndexObservation observation)
	  {
		LocalDate valuationDate = ValuationDate;
		LocalDate fixingDate = observation.FixingDate;
		LocalDate publicationDate = observation.PublicationDate;
		if (publicationDate.isBefore(valuationDate) || (publicationDate.Equals(valuationDate) && fixings.get(fixingDate).HasValue))
		{
		  return PointSensitivityBuilder.none();
		}
		return OvernightRateSensitivity.of(observation, 1d);
	  }

	  public PointSensitivityBuilder rateIgnoringFixingsPointSensitivity(OvernightIndexObservation observation)
	  {
		return OvernightRateSensitivity.of(observation, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public double periodRate(OvernightIndexObservation startDateObservation, LocalDate endDate)
	  {
		LocalDate effectiveDate = startDateObservation.EffectiveDate;
		ArgChecker.inOrderNotEqual(effectiveDate, endDate, "startDate", "endDate");
		double accrualFactor = startDateObservation.Index.DayCount.yearFraction(effectiveDate, endDate);
		return simplyCompoundForwardRate(effectiveDate, endDate, accrualFactor);
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder periodRatePointSensitivity(OvernightIndexObservation startDateObservation, LocalDate endDate)
	  {

		LocalDate startDate = startDateObservation.EffectiveDate;
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		return OvernightRateSensitivity.ofPeriod(startDateObservation, endDate, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(OvernightRateSensitivity pointSensitivity)
	  {
		OvernightIndex index = pointSensitivity.Index;
		LocalDate startDate = pointSensitivity.Observation.EffectiveDate;
		LocalDate endDate = pointSensitivity.EndDate;
		double accrualFactor = index.DayCount.yearFraction(startDate, endDate);
		double forwardBar = pointSensitivity.Sensitivity;
		double dfForwardStart = discountFactors.discountFactor(startDate);
		double dfForwardEnd = discountFactors.discountFactor(endDate);
		double dfStartBar = forwardBar / (accrualFactor * dfForwardEnd);
		double dfEndBar = -forwardBar * dfForwardStart / (accrualFactor * dfForwardEnd * dfForwardEnd);
		ZeroRateSensitivity zrsStart = discountFactors.zeroRatePointSensitivity(startDate, pointSensitivity.Currency);
		ZeroRateSensitivity zrsEnd = discountFactors.zeroRatePointSensitivity(endDate, pointSensitivity.Currency);
		CurrencyParameterSensitivities psStart = discountFactors.parameterSensitivity(zrsStart).multipliedBy(dfStartBar);
		CurrencyParameterSensitivities psEnd = discountFactors.parameterSensitivity(zrsEnd).multipliedBy(dfEndBar);
		return psStart.combinedWith(psEnd);
	  }

	  public CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities)
	  {
		return discountFactors.createParameterSensitivity(currency, sensitivities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new instance with different discount factors.
	  /// </summary>
	  /// <param name="factors">  the new discount factors </param>
	  /// <returns> the new instance </returns>
	  public DiscountOvernightIndexRates withDiscountFactors(DiscountFactors factors)
	  {
		return new DiscountOvernightIndexRates(index, factors, fixings);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DiscountOvernightIndexRates}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DiscountOvernightIndexRates.Meta meta()
	  {
		return DiscountOvernightIndexRates.Meta.INSTANCE;
	  }

	  static DiscountOvernightIndexRates()
	  {
		MetaBean.register(DiscountOvernightIndexRates.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DiscountOvernightIndexRates(OvernightIndex index, DiscountFactors discountFactors, LocalDateDoubleTimeSeries fixings)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(discountFactors, "discountFactors");
		JodaBeanUtils.notNull(fixings, "fixings");
		this.index = index;
		this.discountFactors = discountFactors;
		this.fixings = fixings;
	  }

	  public override DiscountOvernightIndexRates.Meta metaBean()
	  {
		return DiscountOvernightIndexRates.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index that the rates are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying discount factor curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DiscountFactors DiscountFactors
	  {
		  get
		  {
			return discountFactors;
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
		  DiscountOvernightIndexRates other = (DiscountOvernightIndexRates) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(discountFactors, other.discountFactors) && JodaBeanUtils.equal(fixings, other.fixings);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixings);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("DiscountOvernightIndexRates{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("discountFactors").Append('=').Append(discountFactors).Append(',').Append(' ');
		buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DiscountOvernightIndexRates}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(DiscountOvernightIndexRates), typeof(OvernightIndex));
			  discountFactors_Renamed = DirectMetaProperty.ofImmutable(this, "discountFactors", typeof(DiscountOvernightIndexRates), typeof(DiscountFactors));
			  fixings_Renamed = DirectMetaProperty.ofImmutable(this, "fixings", typeof(DiscountOvernightIndexRates), typeof(LocalDateDoubleTimeSeries));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "discountFactors", "fixings");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DiscountFactors> discountFactors_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixings} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDateDoubleTimeSeries> fixings_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "discountFactors", "fixings");
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
			case -91613053: // discountFactors
			  return discountFactors_Renamed;
			case -843784602: // fixings
			  return fixings_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DiscountOvernightIndexRates> builder()
		public override BeanBuilder<DiscountOvernightIndexRates> builder()
		{
		  return new DiscountOvernightIndexRates.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DiscountOvernightIndexRates);
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
		public MetaProperty<OvernightIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountFactors} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DiscountFactors> discountFactors()
		{
		  return discountFactors_Renamed;
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
			  return ((DiscountOvernightIndexRates) bean).Index;
			case -91613053: // discountFactors
			  return ((DiscountOvernightIndexRates) bean).DiscountFactors;
			case -843784602: // fixings
			  return ((DiscountOvernightIndexRates) bean).Fixings;
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
	  /// The bean-builder for {@code DiscountOvernightIndexRates}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DiscountOvernightIndexRates>
	  {

		internal OvernightIndex index;
		internal DiscountFactors discountFactors;
		internal LocalDateDoubleTimeSeries fixings;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index;
			case -91613053: // discountFactors
			  return discountFactors;
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
			  this.index = (OvernightIndex) newValue;
			  break;
			case -91613053: // discountFactors
			  this.discountFactors = (DiscountFactors) newValue;
			  break;
			case -843784602: // fixings
			  this.fixings = (LocalDateDoubleTimeSeries) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DiscountOvernightIndexRates build()
		{
		  return new DiscountOvernightIndexRates(index, discountFactors, fixings);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("DiscountOvernightIndexRates.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("discountFactors").Append('=').Append(JodaBeanUtils.ToString(discountFactors)).Append(',').Append(' ');
		  buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}