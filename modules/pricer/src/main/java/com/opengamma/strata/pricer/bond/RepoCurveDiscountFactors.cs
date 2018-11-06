using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// Provides access to discount factors for a repo curve.
	/// <para>
	/// The discount factor represents the time value of money for the specified security, issuer and currency
	/// when comparing the valuation date to the specified date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class RepoCurveDiscountFactors implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RepoCurveDiscountFactors : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.DiscountFactors discountFactors;
		private readonly DiscountFactors discountFactors;
	  /// <summary>
	  /// The repo group.
	  /// <para>
	  /// This defines the group that the discount factors are for.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.RepoGroup repoGroup;
	  private readonly RepoGroup repoGroup;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on discount factors and group.
	  /// </summary>
	  /// <param name="discountFactors">  the discount factors </param>
	  /// <param name="group">  the group </param>
	  /// <returns> the repo curve discount factors </returns>
	  public static RepoCurveDiscountFactors of(DiscountFactors discountFactors, RepoGroup group)
	  {
		return new RepoCurveDiscountFactors(discountFactors, group);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// <para>
	  /// The currency that discount factors are provided for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return discountFactors.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// The raw data in this provider is calibrated for this date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return discountFactors.ValuationDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factor.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency and bond
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the discount factor </returns>
	  public double discountFactor(LocalDate date)
	  {
		return discountFactors.discountFactor(date);
	  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the curve
	  /// used to determine the discount factor.
	  /// The sensitivity typically has the value {@code (-discountFactor * relativeYearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public RepoCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date)
	  {
		return zeroRatePointSensitivity(date, Currency);
	  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date specifying the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the curve
	  /// used to determine the discount factor.
	  /// The sensitivity typically has the value {@code (-discountFactor * relativeYearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public RepoCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date, Currency sensitivityCurrency)
	  {
		ZeroRateSensitivity zeroRateSensitivity = discountFactors.zeroRatePointSensitivity(date, sensitivityCurrency);
		return RepoCurveZeroRateSensitivity.of(zeroRateSensitivity, repoGroup);
	  }

	  /// <summary>
	  /// Calculates the curve parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to curve parameter sensitivity.
	  /// The calculation typically involves multiplying the point and unit sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CurrencyParameterSensitivities parameterSensitivity(RepoCurveZeroRateSensitivity pointSensitivity)
	  {
		return discountFactors.parameterSensitivity(pointSensitivity.createZeroRateSensitivity());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RepoCurveDiscountFactors}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RepoCurveDiscountFactors.Meta meta()
	  {
		return RepoCurveDiscountFactors.Meta.INSTANCE;
	  }

	  static RepoCurveDiscountFactors()
	  {
		MetaBean.register(RepoCurveDiscountFactors.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private RepoCurveDiscountFactors(DiscountFactors discountFactors, RepoGroup repoGroup)
	  {
		JodaBeanUtils.notNull(discountFactors, "discountFactors");
		JodaBeanUtils.notNull(repoGroup, "repoGroup");
		this.discountFactors = discountFactors;
		this.repoGroup = repoGroup;
	  }

	  public override RepoCurveDiscountFactors.Meta metaBean()
	  {
		return RepoCurveDiscountFactors.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying discount factors for a single currency.
	  /// <para>
	  /// This contains curve, curve currency, valuation date and day count convention.
	  /// The discount factor, its point sensitivity and curve sensitivity are computed by this {@code DiscountFactors}.
	  /// </para>
	  /// </summary>
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
	  /// Gets the repo group.
	  /// <para>
	  /// This defines the group that the discount factors are for.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public RepoGroup RepoGroup
	  {
		  get
		  {
			return repoGroup;
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
		  RepoCurveDiscountFactors other = (RepoCurveDiscountFactors) obj;
		  return JodaBeanUtils.equal(discountFactors, other.discountFactors) && JodaBeanUtils.equal(repoGroup, other.repoGroup);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(repoGroup);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("RepoCurveDiscountFactors{");
		buf.Append("discountFactors").Append('=').Append(discountFactors).Append(',').Append(' ');
		buf.Append("repoGroup").Append('=').Append(JodaBeanUtils.ToString(repoGroup));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RepoCurveDiscountFactors}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  discountFactors_Renamed = DirectMetaProperty.ofImmutable(this, "discountFactors", typeof(RepoCurveDiscountFactors), typeof(DiscountFactors));
			  repoGroup_Renamed = DirectMetaProperty.ofImmutable(this, "repoGroup", typeof(RepoCurveDiscountFactors), typeof(RepoGroup));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "discountFactors", "repoGroup");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code discountFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DiscountFactors> discountFactors_Renamed;
		/// <summary>
		/// The meta-property for the {@code repoGroup} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<RepoGroup> repoGroup_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "discountFactors", "repoGroup");
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
			case -91613053: // discountFactors
			  return discountFactors_Renamed;
			case -393084371: // repoGroup
			  return repoGroup_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends RepoCurveDiscountFactors> builder()
		public override BeanBuilder<RepoCurveDiscountFactors> builder()
		{
		  return new RepoCurveDiscountFactors.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RepoCurveDiscountFactors);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code discountFactors} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DiscountFactors> discountFactors()
		{
		  return discountFactors_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code repoGroup} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<RepoGroup> repoGroup()
		{
		  return repoGroup_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -91613053: // discountFactors
			  return ((RepoCurveDiscountFactors) bean).DiscountFactors;
			case -393084371: // repoGroup
			  return ((RepoCurveDiscountFactors) bean).RepoGroup;
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
	  /// The bean-builder for {@code RepoCurveDiscountFactors}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<RepoCurveDiscountFactors>
	  {

		internal DiscountFactors discountFactors;
		internal RepoGroup repoGroup;

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
			case -91613053: // discountFactors
			  return discountFactors;
			case -393084371: // repoGroup
			  return repoGroup;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -91613053: // discountFactors
			  this.discountFactors = (DiscountFactors) newValue;
			  break;
			case -393084371: // repoGroup
			  this.repoGroup = (RepoGroup) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override RepoCurveDiscountFactors build()
		{
		  return new RepoCurveDiscountFactors(discountFactors, repoGroup);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("RepoCurveDiscountFactors.Builder{");
		  buf.Append("discountFactors").Append('=').Append(JodaBeanUtils.ToString(discountFactors)).Append(',').Append(' ');
		  buf.Append("repoGroup").Append('=').Append(JodaBeanUtils.ToString(repoGroup));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}