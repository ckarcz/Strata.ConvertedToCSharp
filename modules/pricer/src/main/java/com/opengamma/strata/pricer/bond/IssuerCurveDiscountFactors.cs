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
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// Provides access to discount factors for an issuer curve.
	/// <para>
	/// The discount factor represents the time value of money for the specified issuer and currency
	/// when comparing the valuation date to the specified date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class IssuerCurveDiscountFactors implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IssuerCurveDiscountFactors : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.DiscountFactors discountFactors;
		private readonly DiscountFactors discountFactors;
	  /// <summary>
	  /// The legal entity group.
	  /// <para>
	  /// The group defines the legal entity that the discount factors are for.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.LegalEntityGroup legalEntityGroup;
	  private readonly LegalEntityGroup legalEntityGroup;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on discount factors and legal entity group.
	  /// </summary>
	  /// <param name="discountFactors">  the discount factors </param>
	  /// <param name="legalEntityGroup">  the legal entity group </param>
	  /// <returns> the issuer curve discount factors </returns>
	  public static IssuerCurveDiscountFactors of(DiscountFactors discountFactors, LegalEntityGroup legalEntityGroup)
	  {
		return new IssuerCurveDiscountFactors(discountFactors, legalEntityGroup);
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
	  /// The discount factor represents the time value of money for the specified currency and legal entity
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
	  public IssuerCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date)
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
	  public IssuerCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date, Currency sensitivityCurrency)
	  {
		ZeroRateSensitivity zeroRateSensitivity = discountFactors.zeroRatePointSensitivity(date, sensitivityCurrency);
		return IssuerCurveZeroRateSensitivity.of(zeroRateSensitivity, legalEntityGroup);
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
	  public CurrencyParameterSensitivities parameterSensitivity(IssuerCurveZeroRateSensitivity pointSensitivity)
	  {
		return discountFactors.parameterSensitivity(pointSensitivity.createZeroRateSensitivity());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IssuerCurveDiscountFactors}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static IssuerCurveDiscountFactors.Meta meta()
	  {
		return IssuerCurveDiscountFactors.Meta.INSTANCE;
	  }

	  static IssuerCurveDiscountFactors()
	  {
		MetaBean.register(IssuerCurveDiscountFactors.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private IssuerCurveDiscountFactors(DiscountFactors discountFactors, LegalEntityGroup legalEntityGroup)
	  {
		JodaBeanUtils.notNull(discountFactors, "discountFactors");
		JodaBeanUtils.notNull(legalEntityGroup, "legalEntityGroup");
		this.discountFactors = discountFactors;
		this.legalEntityGroup = legalEntityGroup;
	  }

	  public override IssuerCurveDiscountFactors.Meta metaBean()
	  {
		return IssuerCurveDiscountFactors.Meta.INSTANCE;
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
	  /// Gets the legal entity group.
	  /// <para>
	  /// The group defines the legal entity that the discount factors are for.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LegalEntityGroup LegalEntityGroup
	  {
		  get
		  {
			return legalEntityGroup;
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
		  IssuerCurveDiscountFactors other = (IssuerCurveDiscountFactors) obj;
		  return JodaBeanUtils.equal(discountFactors, other.discountFactors) && JodaBeanUtils.equal(legalEntityGroup, other.legalEntityGroup);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityGroup);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("IssuerCurveDiscountFactors{");
		buf.Append("discountFactors").Append('=').Append(discountFactors).Append(',').Append(' ');
		buf.Append("legalEntityGroup").Append('=').Append(JodaBeanUtils.ToString(legalEntityGroup));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code IssuerCurveDiscountFactors}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  discountFactors_Renamed = DirectMetaProperty.ofImmutable(this, "discountFactors", typeof(IssuerCurveDiscountFactors), typeof(DiscountFactors));
			  legalEntityGroup_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityGroup", typeof(IssuerCurveDiscountFactors), typeof(LegalEntityGroup));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "discountFactors", "legalEntityGroup");
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
		/// The meta-property for the {@code legalEntityGroup} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LegalEntityGroup> legalEntityGroup_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "discountFactors", "legalEntityGroup");
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
			case -899047453: // legalEntityGroup
			  return legalEntityGroup_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends IssuerCurveDiscountFactors> builder()
		public override BeanBuilder<IssuerCurveDiscountFactors> builder()
		{
		  return new IssuerCurveDiscountFactors.Builder();
		}

		public override Type beanType()
		{
		  return typeof(IssuerCurveDiscountFactors);
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
		/// The meta-property for the {@code legalEntityGroup} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LegalEntityGroup> legalEntityGroup()
		{
		  return legalEntityGroup_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -91613053: // discountFactors
			  return ((IssuerCurveDiscountFactors) bean).DiscountFactors;
			case -899047453: // legalEntityGroup
			  return ((IssuerCurveDiscountFactors) bean).LegalEntityGroup;
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
	  /// The bean-builder for {@code IssuerCurveDiscountFactors}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<IssuerCurveDiscountFactors>
	  {

		internal DiscountFactors discountFactors;
		internal LegalEntityGroup legalEntityGroup;

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
			case -899047453: // legalEntityGroup
			  return legalEntityGroup;
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
			case -899047453: // legalEntityGroup
			  this.legalEntityGroup = (LegalEntityGroup) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override IssuerCurveDiscountFactors build()
		{
		  return new IssuerCurveDiscountFactors(discountFactors, legalEntityGroup);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("IssuerCurveDiscountFactors.Builder{");
		  buf.Append("discountFactors").Append('=').Append(JodaBeanUtils.ToString(discountFactors)).Append(',').Append(' ');
		  buf.Append("legalEntityGroup").Append('=').Append(JodaBeanUtils.ToString(legalEntityGroup));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}