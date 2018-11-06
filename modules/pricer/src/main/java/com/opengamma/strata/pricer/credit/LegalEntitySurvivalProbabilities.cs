using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;

	/// <summary>
	/// The legal entity survival probabilities. 
	/// <para>
	/// This represents the survival probabilities of a legal entity for a single currency.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class LegalEntitySurvivalProbabilities implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class LegalEntitySurvivalProbabilities : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.StandardId legalEntityId;
		private readonly StandardId legalEntityId;
	  /// <summary>
	  /// The underlying curve.
	  /// <para>
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CreditDiscountFactors survivalProbabilities;
	  private readonly CreditDiscountFactors survivalProbabilities;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="survivalProbabilities">  the survival probabilities </param>
	  /// <returns> the instance </returns>
	  public static LegalEntitySurvivalProbabilities of(StandardId legalEntityId, CreditDiscountFactors survivalProbabilities)
	  {
		return new LegalEntitySurvivalProbabilities(legalEntityId, survivalProbabilities);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// <para>
	  /// The currency that survival probabilities are provided for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return survivalProbabilities.Currency;
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
			return survivalProbabilities.ValuationDate;
		  }
	  }

	  /// <summary>
	  /// Obtains the parameter keys of the underlying curve.
	  /// </summary>
	  /// <returns> the parameter keys </returns>
	  public DoubleArray ParameterKeys
	  {
		  get
		  {
			return survivalProbabilities.ParameterKeys;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the survival probability for the specified date.
	  /// <para>
	  /// If the valuation date is on the specified date, the survival probability is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <returns> the survival probability </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  public double survivalProbability(LocalDate date)
	  {
		return survivalProbabilities.discountFactor(date);
	  }

	  /// <summary>
	  /// Gets the continuously compounded zero hazard rate for specified year fraction.
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the zero hazard rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  public double zeroRate(double yearFraction)
	  {
		return survivalProbabilities.zeroRate(yearFraction);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero hazard rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-survivalProbability * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#survivalProbability(LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CreditCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date)
	  {
		return zeroRatePointSensitivity(date, Currency);
	  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified year fraction.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero hazard rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-survivalProbability * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#survivalProbability(LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CreditCurveZeroRateSensitivity zeroRatePointSensitivity(double yearFraction)
	  {
		return zeroRatePointSensitivity(yearFraction, Currency);
	  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date specifying the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero hazard rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-survivalProbability * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#survivalProbability(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CreditCurveZeroRateSensitivity zeroRatePointSensitivity(LocalDate date, Currency sensitivityCurrency)
	  {
		ZeroRateSensitivity zeroRateSensitivity = survivalProbabilities.zeroRatePointSensitivity(date, sensitivityCurrency);
		return CreditCurveZeroRateSensitivity.of(legalEntityId, zeroRateSensitivity);
	  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified year fraction specifying the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero hazard rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-survivalProbability * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#survivalProbability(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CreditCurveZeroRateSensitivity zeroRatePointSensitivity(double yearFraction, Currency sensitivityCurrency)
	  {
		ZeroRateSensitivity zeroRateSensitivity = survivalProbabilities.zeroRatePointSensitivity(yearFraction, sensitivityCurrency);
		return CreditCurveZeroRateSensitivity.of(legalEntityId, zeroRateSensitivity);
	  }

	  /// <summary>
	  /// Calculates the parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to parameter sensitivity.
	  /// The calculation typically involves multiplying the point and unit sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  public CurrencyParameterSensitivities parameterSensitivity(CreditCurveZeroRateSensitivity pointSensitivity)
	  {
		return survivalProbabilities.parameterSensitivity(pointSensitivity.toZeroRateSensitivity());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code LegalEntitySurvivalProbabilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static LegalEntitySurvivalProbabilities.Meta meta()
	  {
		return LegalEntitySurvivalProbabilities.Meta.INSTANCE;
	  }

	  static LegalEntitySurvivalProbabilities()
	  {
		MetaBean.register(LegalEntitySurvivalProbabilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private LegalEntitySurvivalProbabilities(StandardId legalEntityId, CreditDiscountFactors survivalProbabilities)
	  {
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(survivalProbabilities, "survivalProbabilities");
		this.legalEntityId = legalEntityId;
		this.survivalProbabilities = survivalProbabilities;
	  }

	  public override LegalEntitySurvivalProbabilities.Meta metaBean()
	  {
		return LegalEntitySurvivalProbabilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of a credit derivative.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying curve.
	  /// <para>
	  /// The metadata of the curve must define a day count.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CreditDiscountFactors SurvivalProbabilities
	  {
		  get
		  {
			return survivalProbabilities;
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
		  LegalEntitySurvivalProbabilities other = (LegalEntitySurvivalProbabilities) obj;
		  return JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(survivalProbabilities, other.survivalProbabilities);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(survivalProbabilities);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("LegalEntitySurvivalProbabilities{");
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("survivalProbabilities").Append('=').Append(JodaBeanUtils.ToString(survivalProbabilities));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code LegalEntitySurvivalProbabilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(LegalEntitySurvivalProbabilities), typeof(StandardId));
			  survivalProbabilities_Renamed = DirectMetaProperty.ofImmutable(this, "survivalProbabilities", typeof(LegalEntitySurvivalProbabilities), typeof(CreditDiscountFactors));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "legalEntityId", "survivalProbabilities");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code survivalProbabilities} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CreditDiscountFactors> survivalProbabilities_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "legalEntityId", "survivalProbabilities");
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
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case -2020275979: // survivalProbabilities
			  return survivalProbabilities_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends LegalEntitySurvivalProbabilities> builder()
		public override BeanBuilder<LegalEntitySurvivalProbabilities> builder()
		{
		  return new LegalEntitySurvivalProbabilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(LegalEntitySurvivalProbabilities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code survivalProbabilities} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CreditDiscountFactors> survivalProbabilities()
		{
		  return survivalProbabilities_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 866287159: // legalEntityId
			  return ((LegalEntitySurvivalProbabilities) bean).LegalEntityId;
			case -2020275979: // survivalProbabilities
			  return ((LegalEntitySurvivalProbabilities) bean).SurvivalProbabilities;
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
	  /// The bean-builder for {@code LegalEntitySurvivalProbabilities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<LegalEntitySurvivalProbabilities>
	  {

		internal StandardId legalEntityId;
		internal CreditDiscountFactors survivalProbabilities;

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
			case 866287159: // legalEntityId
			  return legalEntityId;
			case -2020275979: // survivalProbabilities
			  return survivalProbabilities;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 866287159: // legalEntityId
			  this.legalEntityId = (StandardId) newValue;
			  break;
			case -2020275979: // survivalProbabilities
			  this.survivalProbabilities = (CreditDiscountFactors) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override LegalEntitySurvivalProbabilities build()
		{
		  return new LegalEntitySurvivalProbabilities(legalEntityId, survivalProbabilities);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("LegalEntitySurvivalProbabilities.Builder{");
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId)).Append(',').Append(' ');
		  buf.Append("survivalProbabilities").Append('=').Append(JodaBeanUtils.ToString(survivalProbabilities));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}