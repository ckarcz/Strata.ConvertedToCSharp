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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using OvernightIndex = com.opengamma.strata.basics.index.OvernightIndex;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to a rate from an Overnight index curve.
	/// <para>
	/// Holds the sensitivity to the <seealso cref="OvernightIndex"/> curve for a fixing period.
	/// </para>
	/// <para>
	/// This class handles the common case where the rate for a period is approximated
	/// instead of computing the individual rate for each date in the period by storing
	/// the end date of the fixing period.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class OvernightRateSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class OvernightRateSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.OvernightIndexObservation observation;
		private readonly OvernightIndexObservation observation;
	  /// <summary>
	  /// The end date of the period.
	  /// This must be after the fixing date.
	  /// It may be the maturity date implied by the fixing date, but it may also be later.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The currency of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The value of the sensitivity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double sensitivity;
	  private readonly double sensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the observation and sensitivity value.
	  /// <para>
	  /// The currency is defaulted from the index.
	  /// The end date will be the maturity date of the observation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static OvernightRateSensitivity of(OvernightIndexObservation observation, double sensitivity)
	  {
		return of(observation, observation.Currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from the observation and sensitivity value,
	  /// specifying the currency of the value.
	  /// <para>
	  /// The end date will be the maturity date of the observation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static OvernightRateSensitivity of(OvernightIndexObservation observation, Currency sensitivityCurrency, double sensitivity)
	  {

		return new OvernightRateSensitivity(observation, observation.MaturityDate, sensitivityCurrency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance for a period observation of the index from the observation
	  /// and sensitivity value.
	  /// <para>
	  /// The currency is defaulted from the index.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static OvernightRateSensitivity ofPeriod(OvernightIndexObservation observation, LocalDate endDate, double sensitivity)
	  {

		return ofPeriod(observation, endDate, observation.Currency, sensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance for a period observation of the index from the observation
	  /// and sensitivity value, specifying the currency of the value.
	  /// </summary>
	  /// <param name="observation">  the rate observation, including the fixing date </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static OvernightRateSensitivity ofPeriod(OvernightIndexObservation observation, LocalDate endDate, Currency sensitivityCurrency, double sensitivity)
	  {

		return new OvernightRateSensitivity(observation, endDate, sensitivityCurrency, sensitivity);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(observation.FixingDate, endDate, "fixingDate", "endDate");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight index that the sensitivity refers to.
	  /// </summary>
	  /// <returns> the Overnight index </returns>
	  public OvernightIndex Index
	  {
		  get
		  {
			return observation.Index;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public OvernightRateSensitivity withCurrency(Currency currency)
	  {
		if (this.currency.Equals(currency))
		{
		  return this;
		}
		return new OvernightRateSensitivity(observation, endDate, currency, sensitivity);
	  }

	  public OvernightRateSensitivity withSensitivity(double sensitivity)
	  {
		return new OvernightRateSensitivity(observation, endDate, currency, sensitivity);
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is OvernightRateSensitivity)
		{
		  OvernightRateSensitivity otherOn = (OvernightRateSensitivity) other;
		  return ComparisonChain.start().compare(Index.ToString(), otherOn.Index.ToString()).compare(currency, otherOn.currency).compare(observation.FixingDate, otherOn.observation.FixingDate).compare(endDate, otherOn.endDate).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override OvernightRateSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return (OvernightRateSensitivity) PointSensitivity.this.convertedTo(resultCurrency, rateProvider);
	  }

	  //-------------------------------------------------------------------------
	  public override OvernightRateSensitivity multipliedBy(double factor)
	  {
		return new OvernightRateSensitivity(observation, endDate, currency, sensitivity * factor);
	  }

	  public OvernightRateSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new OvernightRateSensitivity(observation, endDate, currency, @operator(sensitivity));
	  }

	  public OvernightRateSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public OvernightRateSensitivity cloned()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightRateSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static OvernightRateSensitivity.Meta meta()
	  {
		return OvernightRateSensitivity.Meta.INSTANCE;
	  }

	  static OvernightRateSensitivity()
	  {
		MetaBean.register(OvernightRateSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private OvernightRateSensitivity(OvernightIndexObservation observation, LocalDate endDate, Currency currency, double sensitivity)
	  {
		JodaBeanUtils.notNull(observation, "observation");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(currency, "currency");
		this.observation = observation;
		this.endDate = endDate;
		this.currency = currency;
		this.sensitivity = sensitivity;
		validate();
	  }

	  public override OvernightRateSensitivity.Meta metaBean()
	  {
		return OvernightRateSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Overnight rate observation.
	  /// <para>
	  /// This includes the index and fixing date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public OvernightIndexObservation Observation
	  {
		  get
		  {
			return observation;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date of the period.
	  /// This must be after the fixing date.
	  /// It may be the maturity date implied by the fixing date, but it may also be later. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency of the sensitivity. </summary>
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
	  /// Gets the value of the sensitivity. </summary>
	  /// <returns> the value of the property </returns>
	  public double Sensitivity
	  {
		  get
		  {
			return sensitivity;
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
		  OvernightRateSensitivity other = (OvernightRateSensitivity) obj;
		  return JodaBeanUtils.equal(observation, other.observation) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(sensitivity, other.sensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observation);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("OvernightRateSensitivity{");
		buf.Append("observation").Append('=').Append(observation).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code OvernightRateSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  observation_Renamed = DirectMetaProperty.ofImmutable(this, "observation", typeof(OvernightRateSensitivity), typeof(OvernightIndexObservation));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(OvernightRateSensitivity), typeof(LocalDate));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(OvernightRateSensitivity), typeof(Currency));
			  sensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivity", typeof(OvernightRateSensitivity), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "observation", "endDate", "currency", "sensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code observation} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<OvernightIndexObservation> observation_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code sensitivity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> sensitivity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "observation", "endDate", "currency", "sensitivity");
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
			case 122345516: // observation
			  return observation_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 564403871: // sensitivity
			  return sensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends OvernightRateSensitivity> builder()
		public override BeanBuilder<OvernightRateSensitivity> builder()
		{
		  return new OvernightRateSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(OvernightRateSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code observation} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<OvernightIndexObservation> observation()
		{
		  return observation_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> sensitivity()
		{
		  return sensitivity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  return ((OvernightRateSensitivity) bean).Observation;
			case -1607727319: // endDate
			  return ((OvernightRateSensitivity) bean).EndDate;
			case 575402001: // currency
			  return ((OvernightRateSensitivity) bean).Currency;
			case 564403871: // sensitivity
			  return ((OvernightRateSensitivity) bean).Sensitivity;
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
	  /// The bean-builder for {@code OvernightRateSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<OvernightRateSensitivity>
	  {

		internal OvernightIndexObservation observation;
		internal LocalDate endDate;
		internal Currency currency;
		internal double sensitivity;

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
			case 122345516: // observation
			  return observation;
			case -1607727319: // endDate
			  return endDate;
			case 575402001: // currency
			  return currency;
			case 564403871: // sensitivity
			  return sensitivity;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 122345516: // observation
			  this.observation = (OvernightIndexObservation) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate = (LocalDate) newValue;
			  break;
			case 575402001: // currency
			  this.currency = (Currency) newValue;
			  break;
			case 564403871: // sensitivity
			  this.sensitivity = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override OvernightRateSensitivity build()
		{
		  return new OvernightRateSensitivity(observation, endDate, currency, sensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("OvernightRateSensitivity.Builder{");
		  buf.Append("observation").Append('=').Append(JodaBeanUtils.ToString(observation)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency)).Append(',').Append(' ');
		  buf.Append("sensitivity").Append('=').Append(JodaBeanUtils.ToString(sensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}