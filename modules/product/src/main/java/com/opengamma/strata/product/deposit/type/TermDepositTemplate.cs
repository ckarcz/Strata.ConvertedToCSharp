using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
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
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A template for creating a term deposit trade.
	/// <para>
	/// This defines almost all the data necessary to create a <seealso cref="TermDeposit"/>.
	/// The trade date, notional and fixed rate are required to complete the template and create the trade.
	/// As such, it is often possible to get a market price for a trade based on the template.
	/// The market price is typically quoted as a bid/ask on the fixed rate.
	/// </para>
	/// <para>
	/// The template is defined by three dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Start date or spot date, the date on which the deposit starts, typically 2 business days after the trade date
	/// <li>End date, the date on which the implied deposit ends, typically a number of months after the start date
	/// </ul>
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class TermDepositTemplate implements com.opengamma.strata.product.TradeTemplate, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TermDepositTemplate : TradeTemplate, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period depositPeriod;
		private readonly Period depositPeriod;
	  /// <summary>
	  /// The underlying term deposit convention.
	  /// <para>
	  /// This specifies the standard convention of the term deposit to be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final TermDepositConvention convention;
	  private readonly TermDepositConvention convention;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isFalse(depositPeriod.Negative, "Deposit Period must not be negative");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a template based on the specified period and convention.
	  /// </summary>
	  /// <param name="depositPeriod">  the period between the start date and the end date </param>
	  /// <param name="convention">  the market convention </param>
	  /// <returns> the template </returns>
	  public static TermDepositTemplate of(Period depositPeriod, TermDepositConvention convention)
	  {
		ArgChecker.notNull(depositPeriod, "depositPeriod");
		ArgChecker.notNull(convention, "convention");
		return TermDepositTemplate.builder().depositPeriod(depositPeriod).convention(convention).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified date.
	  /// The notional is unsigned, with buy/sell determining the direction of the trade.
	  /// If buying the term deposit, the principal is paid at the start date and the
	  /// principal plus interest is received at the end date.
	  /// If selling the term deposit, the principal is received at the start date and the
	  /// principal plus interest is paid at the end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="buySell">  the buy/sell flag, see <seealso cref="TermDeposit#getBuySell()"/> </param>
	  /// <param name="notional">  the notional amount, in the payment currency of the template </param>
	  /// <param name="rate">  the fixed rate, typically derived from the market </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  public TermDepositTrade createTrade(LocalDate tradeDate, BuySell buySell, double notional, double rate, ReferenceData refData)
	  {

		return convention.createTrade(tradeDate, depositPeriod, buySell, notional, rate, refData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TermDepositTemplate}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TermDepositTemplate.Meta meta()
	  {
		return TermDepositTemplate.Meta.INSTANCE;
	  }

	  static TermDepositTemplate()
	  {
		MetaBean.register(TermDepositTemplate.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static TermDepositTemplate.Builder builder()
	  {
		return new TermDepositTemplate.Builder();
	  }

	  private TermDepositTemplate(Period depositPeriod, TermDepositConvention convention)
	  {
		JodaBeanUtils.notNull(depositPeriod, "depositPeriod");
		JodaBeanUtils.notNull(convention, "convention");
		this.depositPeriod = depositPeriod;
		this.convention = convention;
		validate();
	  }

	  public override TermDepositTemplate.Meta metaBean()
	  {
		return TermDepositTemplate.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the period between the start date and the end date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period DepositPeriod
	  {
		  get
		  {
			return depositPeriod;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying term deposit convention.
	  /// <para>
	  /// This specifies the standard convention of the term deposit to be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public TermDepositConvention Convention
	  {
		  get
		  {
			return convention;
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
		  TermDepositTemplate other = (TermDepositTemplate) obj;
		  return JodaBeanUtils.equal(depositPeriod, other.depositPeriod) && JodaBeanUtils.equal(convention, other.convention);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(depositPeriod);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(convention);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("TermDepositTemplate{");
		buf.Append("depositPeriod").Append('=').Append(depositPeriod).Append(',').Append(' ');
		buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code TermDepositTemplate}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  depositPeriod_Renamed = DirectMetaProperty.ofImmutable(this, "depositPeriod", typeof(TermDepositTemplate), typeof(Period));
			  convention_Renamed = DirectMetaProperty.ofImmutable(this, "convention", typeof(TermDepositTemplate), typeof(TermDepositConvention));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "depositPeriod", "convention");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code depositPeriod} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Period> depositPeriod_Renamed;
		/// <summary>
		/// The meta-property for the {@code convention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<TermDepositConvention> convention_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "depositPeriod", "convention");
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
			case 14649855: // depositPeriod
			  return depositPeriod_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override TermDepositTemplate.Builder builder()
		{
		  return new TermDepositTemplate.Builder();
		}

		public override Type beanType()
		{
		  return typeof(TermDepositTemplate);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code depositPeriod} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Period> depositPeriod()
		{
		  return depositPeriod_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code convention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<TermDepositConvention> convention()
		{
		  return convention_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 14649855: // depositPeriod
			  return ((TermDepositTemplate) bean).DepositPeriod;
			case 2039569265: // convention
			  return ((TermDepositTemplate) bean).Convention;
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
	  /// The bean-builder for {@code TermDepositTemplate}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<TermDepositTemplate>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Period depositPeriod_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal TermDepositConvention convention_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(TermDepositTemplate beanToCopy)
		{
		  this.depositPeriod_Renamed = beanToCopy.DepositPeriod;
		  this.convention_Renamed = beanToCopy.Convention;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 14649855: // depositPeriod
			  return depositPeriod_Renamed;
			case 2039569265: // convention
			  return convention_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 14649855: // depositPeriod
			  this.depositPeriod_Renamed = (Period) newValue;
			  break;
			case 2039569265: // convention
			  this.convention_Renamed = (TermDepositConvention) newValue;
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

		public override TermDepositTemplate build()
		{
		  return new TermDepositTemplate(depositPeriod_Renamed, convention_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the period between the start date and the end date. </summary>
		/// <param name="depositPeriod">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder depositPeriod(Period depositPeriod)
		{
		  JodaBeanUtils.notNull(depositPeriod, "depositPeriod");
		  this.depositPeriod_Renamed = depositPeriod;
		  return this;
		}

		/// <summary>
		/// Sets the underlying term deposit convention.
		/// <para>
		/// This specifies the standard convention of the term deposit to be created.
		/// </para>
		/// </summary>
		/// <param name="convention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder convention(TermDepositConvention convention)
		{
		  JodaBeanUtils.notNull(convention, "convention");
		  this.convention_Renamed = convention;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("TermDepositTemplate.Builder{");
		  buf.Append("depositPeriod").Append('=').Append(JodaBeanUtils.ToString(depositPeriod_Renamed)).Append(',').Append(' ');
		  buf.Append("convention").Append('=').Append(JodaBeanUtils.ToString(convention_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}