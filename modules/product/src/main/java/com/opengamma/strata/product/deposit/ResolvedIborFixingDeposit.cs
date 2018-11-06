using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// An Ibor fixing deposit, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="IborFixingDeposit"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedIborFixingDeposit} from a {@code IborFixingDeposit}
	/// using <seealso cref="IborFixingDeposit#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedIborFixingDeposit} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedIborFixingDeposit implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedIborFixingDeposit : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
		private readonly Currency currency;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// The amount that is deposited. It is a positive signed amount if the deposit is 'Buy',
	  /// and a negative signed amount if the deposit is 'Sell'.
	  /// </para>
	  /// <para>
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The start date of the deposit.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date of the deposit.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The year fraction between the start and end date.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/>.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double yearFraction;
	  private readonly double yearFraction;
	  /// <summary>
	  /// The fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The floating rate of interest.
	  /// <para>
	  /// The floating rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.rate.IborRateComputation floatingRate;
	  private readonly IborRateComputation floatingRate;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborFixingDeposit}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedIborFixingDeposit.Meta meta()
	  {
		return ResolvedIborFixingDeposit.Meta.INSTANCE;
	  }

	  static ResolvedIborFixingDeposit()
	  {
		MetaBean.register(ResolvedIborFixingDeposit.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedIborFixingDeposit.Builder builder()
	  {
		return new ResolvedIborFixingDeposit.Builder();
	  }

	  private ResolvedIborFixingDeposit(Currency currency, double notional, LocalDate startDate, LocalDate endDate, double yearFraction, double fixedRate, IborRateComputation floatingRate)
	  {
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		ArgChecker.notNegative(yearFraction, "yearFraction");
		JodaBeanUtils.notNull(floatingRate, "floatingRate");
		this.currency = currency;
		this.notional = notional;
		this.startDate = startDate;
		this.endDate = endDate;
		this.yearFraction = yearFraction;
		this.fixedRate = fixedRate;
		this.floatingRate = floatingRate;
		validate();
	  }

	  public override ResolvedIborFixingDeposit.Meta metaBean()
	  {
		return ResolvedIborFixingDeposit.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency.
	  /// <para>
	  /// This is the currency of the deposit and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// </summary>
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
	  /// Gets the notional amount.
	  /// <para>
	  /// The amount that is deposited. It is a positive signed amount if the deposit is 'Buy',
	  /// and a negative signed amount if the deposit is 'Sell'.
	  /// </para>
	  /// <para>
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date of the deposit.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date of the deposit.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This is an adjusted date, which should be a valid business day
	  /// </para>
	  /// </summary>
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
	  /// Gets the year fraction between the start and end date.
	  /// <para>
	  /// The value is usually calculated using a <seealso cref="DayCount"/>.
	  /// Typically the value will be close to 1 for one year and close to 0.5 for six months.
	  /// The fraction may be greater than 1, but not less than 0.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double YearFraction
	  {
		  get
		  {
			return yearFraction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05. </summary>
	  /// <returns> the value of the property </returns>
	  public double FixedRate
	  {
		  get
		  {
			return fixedRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the floating rate of interest.
	  /// <para>
	  /// The floating rate to be paid is based on this index.
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateComputation FloatingRate
	  {
		  get
		  {
			return floatingRate;
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
		  ResolvedIborFixingDeposit other = (ResolvedIborFixingDeposit) obj;
		  return JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(yearFraction, other.yearFraction) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(floatingRate, other.floatingRate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yearFraction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(floatingRate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("ResolvedIborFixingDeposit{");
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("yearFraction").Append('=').Append(yearFraction).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("floatingRate").Append('=').Append(JodaBeanUtils.ToString(floatingRate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedIborFixingDeposit}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ResolvedIborFixingDeposit), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(ResolvedIborFixingDeposit), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(ResolvedIborFixingDeposit), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(ResolvedIborFixingDeposit), typeof(LocalDate));
			  yearFraction_Renamed = DirectMetaProperty.ofImmutable(this, "yearFraction", typeof(ResolvedIborFixingDeposit), Double.TYPE);
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(ResolvedIborFixingDeposit), Double.TYPE);
			  floatingRate_Renamed = DirectMetaProperty.ofImmutable(this, "floatingRate", typeof(ResolvedIborFixingDeposit), typeof(IborRateComputation));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "yearFraction", "fixedRate", "floatingRate");
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
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code yearFraction} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> yearFraction_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code floatingRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateComputation> floatingRate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currency", "notional", "startDate", "endDate", "yearFraction", "fixedRate", "floatingRate");
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
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -2130225658: // floatingRate
			  return floatingRate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedIborFixingDeposit.Builder builder()
		{
		  return new ResolvedIborFixingDeposit.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedIborFixingDeposit);
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
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yearFraction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> yearFraction()
		{
		  return yearFraction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code floatingRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateComputation> floatingRate()
		{
		  return floatingRate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return ((ResolvedIborFixingDeposit) bean).Currency;
			case 1585636160: // notional
			  return ((ResolvedIborFixingDeposit) bean).Notional;
			case -2129778896: // startDate
			  return ((ResolvedIborFixingDeposit) bean).StartDate;
			case -1607727319: // endDate
			  return ((ResolvedIborFixingDeposit) bean).EndDate;
			case -1731780257: // yearFraction
			  return ((ResolvedIborFixingDeposit) bean).YearFraction;
			case 747425396: // fixedRate
			  return ((ResolvedIborFixingDeposit) bean).FixedRate;
			case -2130225658: // floatingRate
			  return ((ResolvedIborFixingDeposit) bean).FloatingRate;
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
	  /// The bean-builder for {@code ResolvedIborFixingDeposit}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedIborFixingDeposit>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double yearFraction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateComputation floatingRate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedIborFixingDeposit beanToCopy)
		{
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.yearFraction_Renamed = beanToCopy.YearFraction;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
		  this.floatingRate_Renamed = beanToCopy.FloatingRate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1731780257: // yearFraction
			  return yearFraction_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -2130225658: // floatingRate
			  return floatingRate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case -1731780257: // yearFraction
			  this.yearFraction_Renamed = (double?) newValue.Value;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
			  break;
			case -2130225658: // floatingRate
			  this.floatingRate_Renamed = (IborRateComputation) newValue;
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

		public override ResolvedIborFixingDeposit build()
		{
		  return new ResolvedIborFixingDeposit(currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, yearFraction_Renamed, fixedRate_Renamed, floatingRate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the primary currency.
		/// <para>
		/// This is the currency of the deposit and the currency that payment is made in.
		/// The data model permits this currency to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// The amount that is deposited. It is a positive signed amount if the deposit is 'Buy',
		/// and a negative signed amount if the deposit is 'Sell'.
		/// </para>
		/// <para>
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the start date of the deposit.
		/// <para>
		/// This is the first date that interest accrues.
		/// </para>
		/// <para>
		/// This is an adjusted date, which should be a valid business day
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date of the deposit.
		/// <para>
		/// This is the last day that interest accrues.
		/// This date must be after the start date.
		/// </para>
		/// <para>
		/// This is an adjusted date, which should be a valid business day
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the year fraction between the start and end date.
		/// <para>
		/// The value is usually calculated using a <seealso cref="DayCount"/>.
		/// Typically the value will be close to 1 for one year and close to 0.5 for six months.
		/// The fraction may be greater than 1, but not less than 0.
		/// </para>
		/// </summary>
		/// <param name="yearFraction">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yearFraction(double yearFraction)
		{
		  ArgChecker.notNegative(yearFraction, "yearFraction");
		  this.yearFraction_Renamed = yearFraction;
		  return this;
		}

		/// <summary>
		/// Sets the fixed rate of interest.
		/// A 5% rate will be expressed as 0.05. </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the floating rate of interest.
		/// <para>
		/// The floating rate to be paid is based on this index.
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="floatingRate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder floatingRate(IborRateComputation floatingRate)
		{
		  JodaBeanUtils.notNull(floatingRate, "floatingRate");
		  this.floatingRate_Renamed = floatingRate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("ResolvedIborFixingDeposit.Builder{");
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("yearFraction").Append('=').Append(JodaBeanUtils.ToString(yearFraction_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("floatingRate").Append('=').Append(JodaBeanUtils.ToString(floatingRate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}