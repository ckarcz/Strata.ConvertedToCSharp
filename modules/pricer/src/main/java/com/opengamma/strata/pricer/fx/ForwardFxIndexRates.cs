using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to rates for an FX index.
	/// <para>
	/// This provides rates for a single currency pair FX index.
	/// </para>
	/// <para>
	/// This implementation is based on an underlying <seealso cref="FxForwardRates"/> instance.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ForwardFxIndexRates implements FxIndexRates, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ForwardFxIndexRates : FxIndexRates, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.FxIndex index;
		private readonly FxIndex index;
	  /// <summary>
	  /// The underlying FX forward rates.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxForwardRates fxForwardRates;
	  private readonly FxForwardRates fxForwardRates;
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
	  /// The instance is based on the discount factors for each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fxForwardRates">  the underlying forward FX rates </param>
	  /// <returns> the rates instance </returns>
	  public static ForwardFxIndexRates of(FxIndex index, FxForwardRates fxForwardRates)
	  {
		return of(index, fxForwardRates, LocalDateDoubleTimeSeries.empty());
	  }

	  /// <summary>
	  /// Obtains an instance based on discount factors and historic fixings.
	  /// <para>
	  /// The instance is based on the discount factors for each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fxForwardRates">  the underlying forward FX rates </param>
	  /// <param name="fixings">  the time-series of fixings </param>
	  /// <returns> the rates instance </returns>
	  public static ForwardFxIndexRates of(FxIndex index, FxForwardRates fxForwardRates, LocalDateDoubleTimeSeries fixings)
	  {
		return new ForwardFxIndexRates(index, fxForwardRates, fixings);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fixings = LocalDateDoubleTimeSeries.empty();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (!index.CurrencyPair.Equals(fxForwardRates.CurrencyPair))
		{
		  throw new System.ArgumentException("Underlying FxForwardRates must have same currency pair");
		}
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return fxForwardRates.ValuationDate;
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return fxForwardRates.findData(name);
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return fxForwardRates.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return fxForwardRates.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return fxForwardRates.getParameterMetadata(parameterIndex);
	  }

	  public ForwardFxIndexRates withParameter(int parameterIndex, double newValue)
	  {
		return withFxForwardRates(fxForwardRates.withParameter(parameterIndex, newValue));
	  }

	  public ForwardFxIndexRates withPerturbation(ParameterPerturbation perturbation)
	  {
		return withFxForwardRates(fxForwardRates.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double rate(FxIndexObservation observation, Currency baseCurrency)
	  {
		ArgChecker.isTrue(index.CurrencyPair.contains(baseCurrency), "Currency {} invalid for FxIndex {}", baseCurrency, index);
		LocalDate fixingDate = observation.FixingDate;
		double fxIndexRate = !fixingDate.isAfter(ValuationDate) ? historicRate(observation) : forwardRate(observation);
		bool inverse = baseCurrency.Equals(index.CurrencyPair.Counter);
		return (inverse ? 1d / fxIndexRate : fxIndexRate);
	  }

	  // historic rate
	  private double historicRate(FxIndexObservation observation)
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
		  return forwardRate(observation);
		}
	  }

	  // forward rate
	  private double forwardRate(FxIndexObservation observation)
	  {
		return fxForwardRates.rate(index.CurrencyPair.Base, observation.MaturityDate);
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder ratePointSensitivity(FxIndexObservation observation, Currency baseCurrency)
	  {
		ArgChecker.isTrue(index.CurrencyPair.contains(baseCurrency), "Currency {} invalid for FxIndex {}", baseCurrency, index);

		LocalDate fixingDate = observation.FixingDate;
		if (fixingDate.isBefore(ValuationDate) || (fixingDate.Equals(ValuationDate) && fixings.get(fixingDate).HasValue))
		{
		  return PointSensitivityBuilder.none();
		}
		return FxIndexSensitivity.of(observation, baseCurrency, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(FxIndexSensitivity pointSensitivity)
	  {
		return fxForwardRates.parameterSensitivity(pointSensitivity.toFxForwardSensitivity());
	  }

	  public MultiCurrencyAmount currencyExposure(FxIndexSensitivity pointSensitivity)
	  {
		return fxForwardRates.currencyExposure(pointSensitivity.toFxForwardSensitivity());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new instance with different FX forward rates.
	  /// </summary>
	  /// <param name="fxForwardRates">  the new FX forward rates </param>
	  /// <returns> the new instance </returns>
	  public ForwardFxIndexRates withFxForwardRates(FxForwardRates fxForwardRates)
	  {
		return new ForwardFxIndexRates(index, fxForwardRates, fixings);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ForwardFxIndexRates}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ForwardFxIndexRates.Meta meta()
	  {
		return ForwardFxIndexRates.Meta.INSTANCE;
	  }

	  static ForwardFxIndexRates()
	  {
		MetaBean.register(ForwardFxIndexRates.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ForwardFxIndexRates(FxIndex index, FxForwardRates fxForwardRates, LocalDateDoubleTimeSeries fixings)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fxForwardRates, "fxForwardRates");
		JodaBeanUtils.notNull(fixings, "fixings");
		this.index = index;
		this.fxForwardRates = fxForwardRates;
		this.fixings = fixings;
		validate();
	  }

	  public override ForwardFxIndexRates.Meta metaBean()
	  {
		return ForwardFxIndexRates.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index that the rates are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying FX forward rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxForwardRates FxForwardRates
	  {
		  get
		  {
			return fxForwardRates;
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
		  ForwardFxIndexRates other = (ForwardFxIndexRates) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fxForwardRates, other.fxForwardRates) && JodaBeanUtils.equal(fixings, other.fixings);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxForwardRates);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixings);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ForwardFxIndexRates{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("fxForwardRates").Append('=').Append(fxForwardRates).Append(',').Append(' ');
		buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ForwardFxIndexRates}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(ForwardFxIndexRates), typeof(FxIndex));
			  fxForwardRates_Renamed = DirectMetaProperty.ofImmutable(this, "fxForwardRates", typeof(ForwardFxIndexRates), typeof(FxForwardRates));
			  fixings_Renamed = DirectMetaProperty.ofImmutable(this, "fixings", typeof(ForwardFxIndexRates), typeof(LocalDateDoubleTimeSeries));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "fxForwardRates", "fixings");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxForwardRates} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxForwardRates> fxForwardRates_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixings} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDateDoubleTimeSeries> fixings_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "fxForwardRates", "fixings");
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
			case -1002932800: // fxForwardRates
			  return fxForwardRates_Renamed;
			case -843784602: // fixings
			  return fixings_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ForwardFxIndexRates> builder()
		public override BeanBuilder<ForwardFxIndexRates> builder()
		{
		  return new ForwardFxIndexRates.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ForwardFxIndexRates);
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
		public MetaProperty<FxIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxForwardRates} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxForwardRates> fxForwardRates()
		{
		  return fxForwardRates_Renamed;
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
			  return ((ForwardFxIndexRates) bean).Index;
			case -1002932800: // fxForwardRates
			  return ((ForwardFxIndexRates) bean).FxForwardRates;
			case -843784602: // fixings
			  return ((ForwardFxIndexRates) bean).Fixings;
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
	  /// The bean-builder for {@code ForwardFxIndexRates}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ForwardFxIndexRates>
	  {

		internal FxIndex index;
		internal FxForwardRates fxForwardRates;
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
			case -1002932800: // fxForwardRates
			  return fxForwardRates;
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
			  this.index = (FxIndex) newValue;
			  break;
			case -1002932800: // fxForwardRates
			  this.fxForwardRates = (FxForwardRates) newValue;
			  break;
			case -843784602: // fixings
			  this.fixings = (LocalDateDoubleTimeSeries) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ForwardFxIndexRates build()
		{
		  return new ForwardFxIndexRates(index, fxForwardRates, fixings);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ForwardFxIndexRates.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("fxForwardRates").Append('=').Append(JodaBeanUtils.ToString(fxForwardRates)).Append(',').Append(' ');
		  buf.Append("fixings").Append('=').Append(JodaBeanUtils.ToString(fixings));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}