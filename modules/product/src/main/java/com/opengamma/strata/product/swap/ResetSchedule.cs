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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using RollConvention = com.opengamma.strata.basics.schedule.RollConvention;
	using Schedule = com.opengamma.strata.basics.schedule.Schedule;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;

	/// <summary>
	/// Defines the schedule of fixing dates relative to the accrual periods.
	/// <para>
	/// This defines the data necessary to create a schedule of reset periods.
	/// Most accrual periods only contain a single reset period.
	/// This schedule is used when there is more than one reset period in each accrual
	/// period, or where the rules around the reset period are unusual.
	/// </para>
	/// <para>
	/// The rate will be observed once for each reset period.
	/// If an accrual period contains more than one reset period then an averaging
	/// method will be used to combine the floating rates.
	/// </para>
	/// <para>
	/// This class defines reset periods using a periodic frequency.
	/// The frequency must match or be smaller than the accrual periodic frequency.
	/// The reset schedule is calculated forwards, potentially with a short stub at the end.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResetSchedule implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResetSchedule : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.schedule.Frequency resetFrequency;
		private readonly Frequency resetFrequency;
	  /// <summary>
	  /// The business day adjustment to apply to each reset date.
	  /// <para>
	  /// This adjustment is applied to each reset date to ensure it is a valid business day.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The rate reset method, defaulted to 'Unweighted'.
	  /// <para>
	  /// This is used when more than one fixing contributes to the accrual period.
	  /// </para>
	  /// <para>
	  /// Averaging may be weighted by the number of days that the fixing is applicable for.
	  /// The number of days is based on the reset period, not the period between two fixing dates.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2a.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final IborRateResetMethod resetMethod;
	  private readonly IborRateResetMethod resetMethod;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.resetMethod(IborRateResetMethod.UNWEIGHTED);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Resolves this schedule using the specified reference data.
	  /// <para>
	  /// Calling this method binds the reference data and roll convention, returning a
	  /// function that can convert a {@code SchedulePeriod} to an {@code FxReset}.
	  /// </para>
	  /// <para>
	  /// The reset schedule is created within the bounds of the specified accrual period.
	  /// The reset frequency is added repeatedly to the unadjusted start date of the period
	  /// in order to generate the schedule, potentially leaving a short final stub.
	  /// The dates are adjusted using the specified roll convention and the business
	  /// day adjustment of this class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rollConvention">  the applicable roll convention </param>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the reset schedule </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if the schedule is invalid </exception>
	  internal System.Func<SchedulePeriod, Schedule> createSchedule(RollConvention rollConvention, ReferenceData refData)
	  {
		return accrualPeriod => accrualPeriod.subSchedule(resetFrequency, rollConvention, StubConvention.SHORT_FINAL, businessDayAdjustment).createSchedule(refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResetSchedule}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResetSchedule.Meta meta()
	  {
		return ResetSchedule.Meta.INSTANCE;
	  }

	  static ResetSchedule()
	  {
		MetaBean.register(ResetSchedule.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResetSchedule.Builder builder()
	  {
		return new ResetSchedule.Builder();
	  }

	  private ResetSchedule(Frequency resetFrequency, BusinessDayAdjustment businessDayAdjustment, IborRateResetMethod resetMethod)
	  {
		JodaBeanUtils.notNull(resetFrequency, "resetFrequency");
		JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		JodaBeanUtils.notNull(resetMethod, "resetMethod");
		this.resetFrequency = resetFrequency;
		this.businessDayAdjustment = businessDayAdjustment;
		this.resetMethod = resetMethod;
	  }

	  public override ResetSchedule.Meta metaBean()
	  {
		return ResetSchedule.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodic frequency of reset dates.
	  /// <para>
	  /// Reset dates will be calculated within each accrual period based on unadjusted dates.
	  /// The frequency must be the same as, or smaller than, the accrual periodic frequency.
	  /// When calculating the reset dates, the roll convention of the accrual periods will be used.
	  /// Once the unadjusted date calculation is complete, the business day adjustment specified
	  /// here will be used.
	  /// </para>
	  /// <para>
	  /// Averaging applies if the reset frequency does not equal the accrual frequency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Frequency ResetFrequency
	  {
		  get
		  {
			return resetFrequency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply to each reset date.
	  /// <para>
	  /// This adjustment is applied to each reset date to ensure it is a valid business day.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rate reset method, defaulted to 'Unweighted'.
	  /// <para>
	  /// This is used when more than one fixing contributes to the accrual period.
	  /// </para>
	  /// <para>
	  /// Averaging may be weighted by the number of days that the fixing is applicable for.
	  /// The number of days is based on the reset period, not the period between two fixing dates.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2a.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborRateResetMethod ResetMethod
	  {
		  get
		  {
			return resetMethod;
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
		  ResetSchedule other = (ResetSchedule) obj;
		  return JodaBeanUtils.equal(resetFrequency, other.resetFrequency) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(resetMethod, other.resetMethod);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(resetFrequency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(resetMethod);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ResetSchedule{");
		buf.Append("resetFrequency").Append('=').Append(resetFrequency).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("resetMethod").Append('=').Append(JodaBeanUtils.ToString(resetMethod));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResetSchedule}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  resetFrequency_Renamed = DirectMetaProperty.ofImmutable(this, "resetFrequency", typeof(ResetSchedule), typeof(Frequency));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(ResetSchedule), typeof(BusinessDayAdjustment));
			  resetMethod_Renamed = DirectMetaProperty.ofImmutable(this, "resetMethod", typeof(ResetSchedule), typeof(IborRateResetMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "resetFrequency", "businessDayAdjustment", "resetMethod");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code resetFrequency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Frequency> resetFrequency_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code resetMethod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborRateResetMethod> resetMethod_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "resetFrequency", "businessDayAdjustment", "resetMethod");
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
			case 101322957: // resetFrequency
			  return resetFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -958176496: // resetMethod
			  return resetMethod_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResetSchedule.Builder builder()
		{
		  return new ResetSchedule.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResetSchedule);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code resetFrequency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Frequency> resetFrequency()
		{
		  return resetFrequency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code resetMethod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborRateResetMethod> resetMethod()
		{
		  return resetMethod_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 101322957: // resetFrequency
			  return ((ResetSchedule) bean).ResetFrequency;
			case -1065319863: // businessDayAdjustment
			  return ((ResetSchedule) bean).BusinessDayAdjustment;
			case -958176496: // resetMethod
			  return ((ResetSchedule) bean).ResetMethod;
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
	  /// The bean-builder for {@code ResetSchedule}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResetSchedule>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Frequency resetFrequency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborRateResetMethod resetMethod_Renamed;

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
		internal Builder(ResetSchedule beanToCopy)
		{
		  this.resetFrequency_Renamed = beanToCopy.ResetFrequency;
		  this.businessDayAdjustment_Renamed = beanToCopy.BusinessDayAdjustment;
		  this.resetMethod_Renamed = beanToCopy.ResetMethod;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 101322957: // resetFrequency
			  return resetFrequency_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -958176496: // resetMethod
			  return resetMethod_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 101322957: // resetFrequency
			  this.resetFrequency_Renamed = (Frequency) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -958176496: // resetMethod
			  this.resetMethod_Renamed = (IborRateResetMethod) newValue;
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

		public override ResetSchedule build()
		{
		  return new ResetSchedule(resetFrequency_Renamed, businessDayAdjustment_Renamed, resetMethod_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the periodic frequency of reset dates.
		/// <para>
		/// Reset dates will be calculated within each accrual period based on unadjusted dates.
		/// The frequency must be the same as, or smaller than, the accrual periodic frequency.
		/// When calculating the reset dates, the roll convention of the accrual periods will be used.
		/// Once the unadjusted date calculation is complete, the business day adjustment specified
		/// here will be used.
		/// </para>
		/// <para>
		/// Averaging applies if the reset frequency does not equal the accrual frequency.
		/// </para>
		/// </summary>
		/// <param name="resetFrequency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder resetFrequency(Frequency resetFrequency)
		{
		  JodaBeanUtils.notNull(resetFrequency, "resetFrequency");
		  this.resetFrequency_Renamed = resetFrequency;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to each reset date.
		/// <para>
		/// This adjustment is applied to each reset date to ensure it is a valid business day.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  JodaBeanUtils.notNull(businessDayAdjustment, "businessDayAdjustment");
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the rate reset method, defaulted to 'Unweighted'.
		/// <para>
		/// This is used when more than one fixing contributes to the accrual period.
		/// </para>
		/// <para>
		/// Averaging may be weighted by the number of days that the fixing is applicable for.
		/// The number of days is based on the reset period, not the period between two fixing dates.
		/// </para>
		/// <para>
		/// Defined by the 2006 ISDA definitions article 6.2a.
		/// </para>
		/// </summary>
		/// <param name="resetMethod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder resetMethod(IborRateResetMethod resetMethod)
		{
		  JodaBeanUtils.notNull(resetMethod, "resetMethod");
		  this.resetMethod_Renamed = resetMethod;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ResetSchedule.Builder{");
		  buf.Append("resetFrequency").Append('=').Append(JodaBeanUtils.ToString(resetFrequency_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("resetMethod").Append('=').Append(JodaBeanUtils.ToString(resetMethod_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}