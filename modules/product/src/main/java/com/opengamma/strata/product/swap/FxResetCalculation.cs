using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using FxIndex = com.opengamma.strata.basics.index.FxIndex;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Defines the calculation of an FX rate conversion for the notional amount of a swap leg.
	/// <para>
	/// Interest rate swaps are based on a notional amount of money.
	/// The notional can be specified in a currency other than that of the swap leg,
	/// with an FX conversion applied at each payment period boundary.
	/// </para>
	/// <para>
	/// The two currencies involved are the swap leg currency and the reference currency.
	/// The swap leg currency is, in most cases, the currency that payment will occur in.
	/// The reference currency is the currency in which the notional is actually defined.
	/// ISDA refers to the payment currency as the <i>variable currency</i> and the reference
	/// currency as the <i>constant currency</i>.
	/// </para>
	/// <para>
	/// Defined by the 2006 ISDA definitions article 10.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxResetCalculation implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxResetCalculation : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.FxIndex index;
		private readonly FxIndex index;
	  /// <summary>
	  /// The currency of the notional amount defined in the contract.
	  /// <para>
	  /// This is the currency of notional amount as defined in the contract.
	  /// The amount will be converted from this reference currency to the swap leg currency
	  /// when calculating the value of the leg.
	  /// </para>
	  /// <para>
	  /// The reference currency must be one of the two currencies of the index.
	  /// </para>
	  /// <para>
	  /// The reference currency is also known as the <i>constant currency</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency referenceCurrency;
	  private readonly Currency referenceCurrency;
	  /// <summary>
	  /// The base date that each FX reset fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The FX reset fixing date is relative to either the start or end of each accrual period.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FxResetFixingRelativeTo fixingRelativeTo;
	  private readonly FxResetFixingRelativeTo fixingRelativeTo;
	  /// <summary>
	  /// The offset of the FX reset fixing date from each adjusted accrual date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The initial notional value, specified in the payment currency.
	  /// <para>
	  /// If present, this fixed amount represents the notional of the initial period of the
	  /// swap leg, with no FX reset being applied.
	  /// </para>
	  /// <para>
	  /// If not present, the initial notional amount is calculated by applying an fx conversion
	  /// to the reference currency in the same manner as all other period notional calculations.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> initialNotionalValue;
	  private readonly double? initialNotionalValue;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.fixingRelativeTo(FxResetFixingRelativeTo.PERIOD_START);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.fixingDateOffset_Renamed == null && builder.index_Renamed != null)
		{
		  builder.fixingDateOffset_Renamed = builder.index_Renamed.FixingDateOffset;
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (!index.CurrencyPair.contains(referenceCurrency))
		{
		  throw new System.ArgumentException(Messages.format("Reference currency {} must be one of those in the FxIndex {}", referenceCurrency, index));
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves this adjustment using the specified reference data.
	  /// <para>
	  /// Calling this method resolves the holiday calendar, returning a function that
	  /// can convert a {@code SchedulePeriod} and period index pair to an optional {@code FxReset}.
	  /// 
	  /// The conversion locks the fixing date based on the specified schedule period
	  /// and the data held in this object.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved function </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if the calculation is invalid </exception>
	  internal System.Func<int, SchedulePeriod, Optional<FxReset>> resolve(ReferenceData refData)
	  {
		DateAdjuster fixingDateAdjuster = fixingDateOffset.resolve(refData);
		System.Func<LocalDate, FxIndexObservation> obsFn = index.resolve(refData);
		return (periodIndex, period) => buildFxReset(periodIndex, period, fixingDateAdjuster, obsFn);
	  }

	  // build the FxReset
	  private Optional<FxReset> buildFxReset(int periodIndex, SchedulePeriod period, DateAdjuster fixingDateAdjuster, System.Func<LocalDate, FxIndexObservation> obsFn)
	  {

		if (periodIndex == 0 && initialNotionalValue != null)
		{
		  //if first notional is fixed then no FxReset is applied
		  return null;
		}
		LocalDate fixingDate = fixingDateAdjuster.adjust(fixingRelativeTo.selectBaseDate(period));
		return FxReset.of(obsFn(fixingDate), referenceCurrency);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxResetCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxResetCalculation.Meta meta()
	  {
		return FxResetCalculation.Meta.INSTANCE;
	  }

	  static FxResetCalculation()
	  {
		MetaBean.register(FxResetCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxResetCalculation.Builder builder()
	  {
		return new FxResetCalculation.Builder();
	  }

	  private FxResetCalculation(FxIndex index, Currency referenceCurrency, FxResetFixingRelativeTo fixingRelativeTo, DaysAdjustment fixingDateOffset, double? initialNotionalValue)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(referenceCurrency, "referenceCurrency");
		JodaBeanUtils.notNull(fixingRelativeTo, "fixingRelativeTo");
		JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		this.index = index;
		this.referenceCurrency = referenceCurrency;
		this.fixingRelativeTo = fixingRelativeTo;
		this.fixingDateOffset = fixingDateOffset;
		this.initialNotionalValue = initialNotionalValue;
		validate();
	  }

	  public override FxResetCalculation.Meta metaBean()
	  {
		return FxResetCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the FX index used to obtain the FX reset rate.
	  /// <para>
	  /// This is the index of FX used to obtain the FX reset rate.
	  /// An FX index is a daily rate of exchange between two currencies.
	  /// Note that the order of the currencies in the index does not matter, as the
	  /// conversion direction is fully defined by the reference and swap leg currencies.
	  /// </para>
	  /// </summary>
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
	  /// Gets the currency of the notional amount defined in the contract.
	  /// <para>
	  /// This is the currency of notional amount as defined in the contract.
	  /// The amount will be converted from this reference currency to the swap leg currency
	  /// when calculating the value of the leg.
	  /// </para>
	  /// <para>
	  /// The reference currency must be one of the two currencies of the index.
	  /// </para>
	  /// <para>
	  /// The reference currency is also known as the <i>constant currency</i>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency ReferenceCurrency
	  {
		  get
		  {
			return referenceCurrency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the base date that each FX reset fixing is made relative to, defaulted to 'PeriodStart'.
	  /// <para>
	  /// The FX reset fixing date is relative to either the start or end of each accrual period.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxResetFixingRelativeTo FixingRelativeTo
	  {
		  get
		  {
			return fixingRelativeTo;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the FX reset fixing date from each adjusted accrual date.
	  /// <para>
	  /// The offset is applied to the base date specified by {@code fixingRelativeTo}.
	  /// The offset is typically a negative number of business days.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing offset of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial notional value, specified in the payment currency.
	  /// <para>
	  /// If present, this fixed amount represents the notional of the initial period of the
	  /// swap leg, with no FX reset being applied.
	  /// </para>
	  /// <para>
	  /// If not present, the initial notional amount is calculated by applying an fx conversion
	  /// to the reference currency in the same manner as all other period notional calculations.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? InitialNotionalValue
	  {
		  get
		  {
			return initialNotionalValue != null ? double?.of(initialNotionalValue) : double?.empty();
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
		  FxResetCalculation other = (FxResetCalculation) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(referenceCurrency, other.referenceCurrency) && JodaBeanUtils.equal(fixingRelativeTo, other.fixingRelativeTo) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(initialNotionalValue, other.initialNotionalValue);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(referenceCurrency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingRelativeTo);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialNotionalValue);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("FxResetCalculation{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("referenceCurrency").Append('=').Append(referenceCurrency).Append(',').Append(' ');
		buf.Append("fixingRelativeTo").Append('=').Append(fixingRelativeTo).Append(',').Append(' ');
		buf.Append("fixingDateOffset").Append('=').Append(fixingDateOffset).Append(',').Append(' ');
		buf.Append("initialNotionalValue").Append('=').Append(JodaBeanUtils.ToString(initialNotionalValue));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxResetCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(FxResetCalculation), typeof(FxIndex));
			  referenceCurrency_Renamed = DirectMetaProperty.ofImmutable(this, "referenceCurrency", typeof(FxResetCalculation), typeof(Currency));
			  fixingRelativeTo_Renamed = DirectMetaProperty.ofImmutable(this, "fixingRelativeTo", typeof(FxResetCalculation), typeof(FxResetFixingRelativeTo));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(FxResetCalculation), typeof(DaysAdjustment));
			  initialNotionalValue_Renamed = DirectMetaProperty.ofImmutable(this, "initialNotionalValue", typeof(FxResetCalculation), typeof(Double));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "referenceCurrency", "fixingRelativeTo", "fixingDateOffset", "initialNotionalValue");
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
		/// The meta-property for the {@code referenceCurrency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> referenceCurrency_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxResetFixingRelativeTo> fixingRelativeTo_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialNotionalValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> initialNotionalValue_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "referenceCurrency", "fixingRelativeTo", "fixingDateOffset", "initialNotionalValue");
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
			case 727652476: // referenceCurrency
			  return referenceCurrency_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case -931164883: // initialNotionalValue
			  return initialNotionalValue_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxResetCalculation.Builder builder()
		{
		  return new FxResetCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxResetCalculation);
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
		/// The meta-property for the {@code referenceCurrency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> referenceCurrency()
		{
		  return referenceCurrency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingRelativeTo} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxResetFixingRelativeTo> fixingRelativeTo()
		{
		  return fixingRelativeTo_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialNotionalValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> initialNotionalValue()
		{
		  return initialNotionalValue_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((FxResetCalculation) bean).Index;
			case 727652476: // referenceCurrency
			  return ((FxResetCalculation) bean).ReferenceCurrency;
			case 232554996: // fixingRelativeTo
			  return ((FxResetCalculation) bean).FixingRelativeTo;
			case 873743726: // fixingDateOffset
			  return ((FxResetCalculation) bean).FixingDateOffset;
			case -931164883: // initialNotionalValue
			  return ((FxResetCalculation) bean).initialNotionalValue;
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
	  /// The bean-builder for {@code FxResetCalculation}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxResetCalculation>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency referenceCurrency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxResetFixingRelativeTo fixingRelativeTo_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? initialNotionalValue_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FxResetCalculation beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.referenceCurrency_Renamed = beanToCopy.ReferenceCurrency;
		  this.fixingRelativeTo_Renamed = beanToCopy.FixingRelativeTo;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.initialNotionalValue_Renamed = beanToCopy.initialNotionalValue;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 727652476: // referenceCurrency
			  return referenceCurrency_Renamed;
			case 232554996: // fixingRelativeTo
			  return fixingRelativeTo_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case -931164883: // initialNotionalValue
			  return initialNotionalValue_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (FxIndex) newValue;
			  break;
			case 727652476: // referenceCurrency
			  this.referenceCurrency_Renamed = (Currency) newValue;
			  break;
			case 232554996: // fixingRelativeTo
			  this.fixingRelativeTo_Renamed = (FxResetFixingRelativeTo) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -931164883: // initialNotionalValue
			  this.initialNotionalValue_Renamed = (double?) newValue;
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

		public override FxResetCalculation build()
		{
		  preBuild(this);
		  return new FxResetCalculation(index_Renamed, referenceCurrency_Renamed, fixingRelativeTo_Renamed, fixingDateOffset_Renamed, initialNotionalValue_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the FX index used to obtain the FX reset rate.
		/// <para>
		/// This is the index of FX used to obtain the FX reset rate.
		/// An FX index is a daily rate of exchange between two currencies.
		/// Note that the order of the currencies in the index does not matter, as the
		/// conversion direction is fully defined by the reference and swap leg currencies.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(FxIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the currency of the notional amount defined in the contract.
		/// <para>
		/// This is the currency of notional amount as defined in the contract.
		/// The amount will be converted from this reference currency to the swap leg currency
		/// when calculating the value of the leg.
		/// </para>
		/// <para>
		/// The reference currency must be one of the two currencies of the index.
		/// </para>
		/// <para>
		/// The reference currency is also known as the <i>constant currency</i>.
		/// </para>
		/// </summary>
		/// <param name="referenceCurrency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder referenceCurrency(Currency referenceCurrency)
		{
		  JodaBeanUtils.notNull(referenceCurrency, "referenceCurrency");
		  this.referenceCurrency_Renamed = referenceCurrency;
		  return this;
		}

		/// <summary>
		/// Sets the base date that each FX reset fixing is made relative to, defaulted to 'PeriodStart'.
		/// <para>
		/// The FX reset fixing date is relative to either the start or end of each accrual period.
		/// </para>
		/// </summary>
		/// <param name="fixingRelativeTo">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingRelativeTo(FxResetFixingRelativeTo fixingRelativeTo)
		{
		  JodaBeanUtils.notNull(fixingRelativeTo, "fixingRelativeTo");
		  this.fixingRelativeTo_Renamed = fixingRelativeTo;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the FX reset fixing date from each adjusted accrual date.
		/// <para>
		/// The offset is applied to the base date specified by {@code fixingRelativeTo}.
		/// The offset is typically a negative number of business days.
		/// </para>
		/// <para>
		/// When building, this will default to the fixing offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the initial notional value, specified in the payment currency.
		/// <para>
		/// If present, this fixed amount represents the notional of the initial period of the
		/// swap leg, with no FX reset being applied.
		/// </para>
		/// <para>
		/// If not present, the initial notional amount is calculated by applying an fx conversion
		/// to the reference currency in the same manner as all other period notional calculations.
		/// </para>
		/// </summary>
		/// <param name="initialNotionalValue">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialNotionalValue(double? initialNotionalValue)
		{
		  this.initialNotionalValue_Renamed = initialNotionalValue;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("FxResetCalculation.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("referenceCurrency").Append('=').Append(JodaBeanUtils.ToString(referenceCurrency_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingRelativeTo").Append('=').Append(JodaBeanUtils.ToString(fixingRelativeTo_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("initialNotionalValue").Append('=').Append(JodaBeanUtils.ToString(initialNotionalValue_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}