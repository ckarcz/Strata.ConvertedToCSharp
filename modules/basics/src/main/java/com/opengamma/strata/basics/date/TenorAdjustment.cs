using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
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


	/// <summary>
	/// An adjustment that alters a date by adding a tenor.
	/// <para>
	/// This adjustment adds a <seealso cref="Tenor"/> to the input date using an addition convention,
	/// followed by an adjustment to ensure the result is a valid business day.
	/// </para>
	/// <para>
	/// Addition is performed using standard calendar addition.
	/// It is not possible to add a number of business days using this class.
	/// See <seealso cref="DaysAdjustment"/> for an alternative that can handle addition of business days.
	/// </para>
	/// <para>
	/// There are two steps in the calculation:
	/// </para>
	/// <para>
	/// In step one, the period is added using the specified <seealso cref="PeriodAdditionConvention"/>.
	/// </para>
	/// <para>
	/// In step two, the result of step one is optionally adjusted to be a business day
	/// using a {@code BusinessDayAdjustment}.
	/// </para>
	/// <para>
	/// For example, a rule represented by this class might be: "the end date is 5 years after
	/// the start date, with end-of-month rule based on the last business day of the month,
	/// adjusted to be a valid London business day using the 'ModifiedFollowing' convention".
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class TenorAdjustment implements com.opengamma.strata.basics.Resolvable<DateAdjuster>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TenorAdjustment : Resolvable<DateAdjuster>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Tenor tenor;
		private readonly Tenor tenor;
	  /// <summary>
	  /// The addition convention to apply.
	  /// <para>
	  /// When the adjustment is performed, this convention is used to refine the adjusted date.
	  /// The most common convention is to move the end date to the last business day of the month
	  /// if the start date is the last business day of the month.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final PeriodAdditionConvention additionConvention;
	  private readonly PeriodAdditionConvention additionConvention;
	  /// <summary>
	  /// The business day adjustment that is performed to the result of the addition.
	  /// <para>
	  /// This adjustment is applied to the result of the addition calculation.
	  /// </para>
	  /// <para>
	  /// If no adjustment is required, use the 'None' business day adjustment.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final BusinessDayAdjustment adjustment;
	  private readonly BusinessDayAdjustment adjustment;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance that can adjust a date by the specified tenor.
	  /// <para>
	  /// When adjusting a date, the specified tenor is added to the input date.
	  /// The business day adjustment will then be used to ensure the result is a valid business day.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor to add to the input date </param>
	  /// <param name="additionConvention">  the convention used to perform the addition </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the result of the addition </param>
	  /// <returns> the tenor adjustment </returns>
	  public static TenorAdjustment of(Tenor tenor, PeriodAdditionConvention additionConvention, BusinessDayAdjustment adjustment)
	  {
		return new TenorAdjustment(tenor, additionConvention, adjustment);
	  }

	  /// <summary>
	  /// Obtains an instance that can adjust a date by the specified tenor using the
	  /// last day of month convention.
	  /// <para>
	  /// When adjusting a date, the specified tenor is added to the input date.
	  /// The business day adjustment will then be used to ensure the result is a valid business day.
	  /// </para>
	  /// <para>
	  /// The period must consist only of months and/or years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor to add to the input date </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the result of the addition </param>
	  /// <returns> the tenor adjustment </returns>
	  public static TenorAdjustment ofLastDay(Tenor tenor, BusinessDayAdjustment adjustment)
	  {
		return new TenorAdjustment(tenor, PeriodAdditionConventions.LAST_DAY, adjustment);
	  }

	  /// <summary>
	  /// Obtains an instance that can adjust a date by the specified tenor using the
	  /// last business day of month convention.
	  /// <para>
	  /// When adjusting a date, the specified tenor is added to the input date.
	  /// The business day adjustment will then be used to ensure the result is a valid business day.
	  /// </para>
	  /// <para>
	  /// The period must consist only of months and/or years.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tenor">  the tenor to add to the input date </param>
	  /// <param name="adjustment">  the business day adjustment to apply to the result of the addition </param>
	  /// <returns> the tenor adjustment </returns>
	  public static TenorAdjustment ofLastBusinessDay(Tenor tenor, BusinessDayAdjustment adjustment)
	  {
		return new TenorAdjustment(tenor, PeriodAdditionConventions.LAST_BUSINESS_DAY, adjustment);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (additionConvention.MonthBased && tenor.MonthBased == false)
		{
		  throw new System.ArgumentException("Tenor must not contain days when addition convention is month-based");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adjusts the date, adding the tenor and then applying the business day adjustment.
	  /// <para>
	  /// The calculation is performed in two steps.
	  /// </para>
	  /// <para>
	  /// Step one, use <seealso cref="PeriodAdditionConvention#adjust(LocalDate, Period, HolidayCalendar)"/> to add the period.
	  /// </para>
	  /// <para>
	  /// Step two, use <seealso cref="BusinessDayAdjustment#adjust(LocalDate, ReferenceData)"/> to adjust the result of step one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to adjust </param>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjusted date </returns>
	  public LocalDate adjust(LocalDate date, ReferenceData refData)
	  {
		HolidayCalendar holCal = adjustment.Calendar.resolve(refData);
		BusinessDayConvention bda = adjustment.Convention;
		return bda.adjust(additionConvention.adjust(date, tenor.Period, holCal), holCal);
	  }

	  /// <summary>
	  /// Resolves this adjustment using the specified reference data, returning an adjuster.
	  /// <para>
	  /// This returns a <seealso cref="DateAdjuster"/> that performs the same calculation as this adjustment.
	  /// It binds the holiday calendar, looked up from the reference data, into the result.
	  /// As such, there is no need to pass the reference data in again.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data, used to find the holiday calendar </param>
	  /// <returns> the adjuster, bound to a specific holiday calendar </returns>
	  public DateAdjuster resolve(ReferenceData refData)
	  {
		HolidayCalendar holCal = adjustment.Calendar.resolve(refData);
		BusinessDayConvention bda = adjustment.Convention;
		Period period = tenor.Period;
		return date => bda.adjust(additionConvention.adjust(date, period, holCal), holCal);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a string describing the adjustment.
	  /// </summary>
	  /// <returns> the descriptive string </returns>
	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append(tenor);
		if (additionConvention != PeriodAdditionConventions.NONE)
		{
		  buf.Append(" with ").Append(additionConvention);
		}
		if (adjustment.Equals(BusinessDayAdjustment.NONE) == false)
		{
		  buf.Append(" then apply ").Append(adjustment);
		}
		return buf.ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TenorAdjustment}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TenorAdjustment.Meta meta()
	  {
		return TenorAdjustment.Meta.INSTANCE;
	  }

	  static TenorAdjustment()
	  {
		MetaBean.register(TenorAdjustment.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static TenorAdjustment.Builder builder()
	  {
		return new TenorAdjustment.Builder();
	  }

	  private TenorAdjustment(Tenor tenor, PeriodAdditionConvention additionConvention, BusinessDayAdjustment adjustment)
	  {
		JodaBeanUtils.notNull(tenor, "tenor");
		JodaBeanUtils.notNull(additionConvention, "additionConvention");
		JodaBeanUtils.notNull(adjustment, "adjustment");
		this.tenor = tenor;
		this.additionConvention = additionConvention;
		this.adjustment = adjustment;
		validate();
	  }

	  public override TenorAdjustment.Meta metaBean()
	  {
		return TenorAdjustment.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tenor to be added.
	  /// <para>
	  /// When the adjustment is performed, this tenor will be added to the input date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Tenor Tenor
	  {
		  get
		  {
			return tenor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the addition convention to apply.
	  /// <para>
	  /// When the adjustment is performed, this convention is used to refine the adjusted date.
	  /// The most common convention is to move the end date to the last business day of the month
	  /// if the start date is the last business day of the month.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public PeriodAdditionConvention AdditionConvention
	  {
		  get
		  {
			return additionConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment that is performed to the result of the addition.
	  /// <para>
	  /// This adjustment is applied to the result of the addition calculation.
	  /// </para>
	  /// <para>
	  /// If no adjustment is required, use the 'None' business day adjustment.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment Adjustment
	  {
		  get
		  {
			return adjustment;
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
		  TenorAdjustment other = (TenorAdjustment) obj;
		  return JodaBeanUtils.equal(tenor, other.tenor) && JodaBeanUtils.equal(additionConvention, other.additionConvention) && JodaBeanUtils.equal(adjustment, other.adjustment);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tenor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(additionConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(adjustment);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TenorAdjustment}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  tenor_Renamed = DirectMetaProperty.ofImmutable(this, "tenor", typeof(TenorAdjustment), typeof(Tenor));
			  additionConvention_Renamed = DirectMetaProperty.ofImmutable(this, "additionConvention", typeof(TenorAdjustment), typeof(PeriodAdditionConvention));
			  adjustment_Renamed = DirectMetaProperty.ofImmutable(this, "adjustment", typeof(TenorAdjustment), typeof(BusinessDayAdjustment));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "tenor", "additionConvention", "adjustment");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code tenor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Tenor> tenor_Renamed;
		/// <summary>
		/// The meta-property for the {@code additionConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<PeriodAdditionConvention> additionConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code adjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> adjustment_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "tenor", "additionConvention", "adjustment");
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
			case 110246592: // tenor
			  return tenor_Renamed;
			case 1652975501: // additionConvention
			  return additionConvention_Renamed;
			case 1977085293: // adjustment
			  return adjustment_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override TenorAdjustment.Builder builder()
		{
		  return new TenorAdjustment.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TenorAdjustment);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code tenor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Tenor> tenor()
		{
		  return tenor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code additionConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<PeriodAdditionConvention> additionConvention()
		{
		  return additionConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code adjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> adjustment()
		{
		  return adjustment_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 110246592: // tenor
			  return ((TenorAdjustment) bean).Tenor;
			case 1652975501: // additionConvention
			  return ((TenorAdjustment) bean).AdditionConvention;
			case 1977085293: // adjustment
			  return ((TenorAdjustment) bean).Adjustment;
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
	  /// The bean-builder for {@code TenorAdjustment}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<TenorAdjustment>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Tenor tenor_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal PeriodAdditionConvention additionConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment adjustment_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(TenorAdjustment beanToCopy)
		{
		  this.tenor_Renamed = beanToCopy.Tenor;
		  this.additionConvention_Renamed = beanToCopy.AdditionConvention;
		  this.adjustment_Renamed = beanToCopy.Adjustment;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 110246592: // tenor
			  return tenor_Renamed;
			case 1652975501: // additionConvention
			  return additionConvention_Renamed;
			case 1977085293: // adjustment
			  return adjustment_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 110246592: // tenor
			  this.tenor_Renamed = (Tenor) newValue;
			  break;
			case 1652975501: // additionConvention
			  this.additionConvention_Renamed = (PeriodAdditionConvention) newValue;
			  break;
			case 1977085293: // adjustment
			  this.adjustment_Renamed = (BusinessDayAdjustment) newValue;
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

		public override TenorAdjustment build()
		{
		  return new TenorAdjustment(tenor_Renamed, additionConvention_Renamed, adjustment_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the tenor to be added.
		/// <para>
		/// When the adjustment is performed, this tenor will be added to the input date.
		/// </para>
		/// </summary>
		/// <param name="tenor">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder tenor(Tenor tenor)
		{
		  JodaBeanUtils.notNull(tenor, "tenor");
		  this.tenor_Renamed = tenor;
		  return this;
		}

		/// <summary>
		/// Sets the addition convention to apply.
		/// <para>
		/// When the adjustment is performed, this convention is used to refine the adjusted date.
		/// The most common convention is to move the end date to the last business day of the month
		/// if the start date is the last business day of the month.
		/// </para>
		/// </summary>
		/// <param name="additionConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder additionConvention(PeriodAdditionConvention additionConvention)
		{
		  JodaBeanUtils.notNull(additionConvention, "additionConvention");
		  this.additionConvention_Renamed = additionConvention;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment that is performed to the result of the addition.
		/// <para>
		/// This adjustment is applied to the result of the addition calculation.
		/// </para>
		/// <para>
		/// If no adjustment is required, use the 'None' business day adjustment.
		/// </para>
		/// </summary>
		/// <param name="adjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder adjustment(BusinessDayAdjustment adjustment)
		{
		  JodaBeanUtils.notNull(adjustment, "adjustment");
		  this.adjustment_Renamed = adjustment;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("TenorAdjustment.Builder{");
		  buf.Append("tenor").Append('=').Append(JodaBeanUtils.ToString(tenor_Renamed)).Append(',').Append(' ');
		  buf.Append("additionConvention").Append('=').Append(JodaBeanUtils.ToString(additionConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("adjustment").Append('=').Append(JodaBeanUtils.ToString(adjustment_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}