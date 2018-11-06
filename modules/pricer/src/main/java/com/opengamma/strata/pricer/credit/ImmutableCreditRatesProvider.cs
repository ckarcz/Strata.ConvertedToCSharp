using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;

	/// <summary>
	/// The immutable rates provider, used to calculate analytic measures.
	/// <para>
	/// The primary usage of this provider is to price credit default swaps on a legal entity.
	/// This includes credit curves, discounting curves and recovery rate curves.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableCreditRatesProvider implements CreditRatesProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableCreditRatesProvider : CreditRatesProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
		private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The credit curves.
	  /// <para>
	  /// The curve data, predicting the survival probability, associated with each legal entity and currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", get = "private") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.basics.StandardId, com.opengamma.strata.basics.currency.Currency>, LegalEntitySurvivalProbabilities> creditCurves;
	  private readonly ImmutableMap<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> creditCurves;
	  /// <summary>
	  /// The discounting curves.
	  /// <para>
	  /// The curve data, predicting the discount factor, associated with each currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", get = "private") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, CreditDiscountFactors> discountCurves;
	  private readonly ImmutableMap<Currency, CreditDiscountFactors> discountCurves;
	  /// <summary>
	  /// The credit rate curves.
	  /// <para>
	  /// The curve date, predicting the recovery rate, associated with each legal entity.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", get = "private") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.StandardId, RecoveryRates> recoveryRateCurves;
	  private readonly ImmutableMap<StandardId, RecoveryRates> recoveryRateCurves;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		foreach (KeyValuePair<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> entry in creditCurves.entrySet())
		{
		  if (!entry.Value.ValuationDate.isEqual(valuationDate))
		  {
			throw new System.ArgumentException("Invalid valuation date for the credit curve: " + entry.Value);
		  }
		}
		foreach (KeyValuePair<Currency, CreditDiscountFactors> entry in discountCurves.entrySet())
		{
		  if (!entry.Value.ValuationDate.isEqual(valuationDate))
		  {
			throw new System.ArgumentException("Invalid valuation date for the discount curve: " + entry.Value);
		  }
		}
		foreach (KeyValuePair<StandardId, RecoveryRates> entry in recoveryRateCurves.entrySet())
		{
		  if (!entry.Value.ValuationDate.isEqual(valuationDate))
		  {
			throw new System.ArgumentException("Invalid valuation date for the recovery rate curve: " + entry.Value);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public LegalEntitySurvivalProbabilities survivalProbabilities(StandardId legalEntityId, Currency currency)
	  {
		LegalEntitySurvivalProbabilities survivalProbabilities = creditCurves.get(Pair.of(legalEntityId, currency));
		if (survivalProbabilities == null)
		{
		  throw new System.ArgumentException("Unable to find credit curve: " + legalEntityId + ", " + currency);
		}
		return survivalProbabilities;
	  }

	  public CreditDiscountFactors discountFactors(Currency currency)
	  {
		CreditDiscountFactors discountFactors = discountCurves.get(currency);
		if (discountFactors == null)
		{
		  throw new System.ArgumentException("Unable to find discount curve: " + currency);
		}
		return discountFactors;
	  }

	  public RecoveryRates recoveryRates(StandardId legalEntityId)
	  {
		RecoveryRates recoveryRates = recoveryRateCurves.get(legalEntityId);
		if (recoveryRates == null)
		{
		  throw new System.ArgumentException("Unable to find recovery rate curve: " + legalEntityId);
		}
		return recoveryRates;
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is CreditCurveZeroRateSensitivity)
		  {
			CreditCurveZeroRateSensitivity pt = (CreditCurveZeroRateSensitivity) point;
			LegalEntitySurvivalProbabilities factors = survivalProbabilities(pt.LegalEntityId, pt.CurveCurrency);
			sens = sens.combinedWith(factors.parameterSensitivity(pt));
		  }
		  else if (point is ZeroRateSensitivity)
		  {
			ZeroRateSensitivity pt = (ZeroRateSensitivity) point;
			CreditDiscountFactors factors = discountFactors(pt.CurveCurrency);
			sens = sens.combinedWith(factors.parameterSensitivity(pt));
		  }
		}
		return sens;
	  }

	  public CurrencyParameterSensitivity singleCreditCurveParameterSensitivity(PointSensitivities pointSensitivities, StandardId legalEntityId, Currency currency)
	  {

		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is CreditCurveZeroRateSensitivity)
		  {
			CreditCurveZeroRateSensitivity pt = (CreditCurveZeroRateSensitivity) point;
			if (pt.LegalEntityId.Equals(legalEntityId) && pt.Currency.Equals(currency))
			{
			  LegalEntitySurvivalProbabilities factors = survivalProbabilities(pt.LegalEntityId, pt.CurveCurrency);
			  sens = sens.combinedWith(factors.parameterSensitivity(pt));
			}
		  }
		}
		ArgChecker.isTrue(sens.size() == 1, "sensitivity must be unique");
		return sens.Sensitivities.get(0);
	  }

	  public CurrencyParameterSensitivity singleDiscountCurveParameterSensitivity(PointSensitivities pointSensitivities, Currency currency)
	  {

		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is ZeroRateSensitivity)
		  {
			ZeroRateSensitivity pt = (ZeroRateSensitivity) point;
			if (pt.CurveCurrency.Equals(currency))
			{
			  CreditDiscountFactors factors = discountFactors(pt.CurveCurrency);
			  sens = sens.combinedWith(factors.parameterSensitivity(pt));
			}
		  }
		}
		ArgChecker.isTrue(sens.size() == 1, "sensitivity must be unique");
		return sens.Sensitivities.get(0);
	  }

	  //-------------------------------------------------------------------------
	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (name is CurveName)
		{
		  return Stream.concat(discountCurves.values().stream(), creditCurves.values().Select(cc => cc.SurvivalProbabilities)).map(df => df.findData(name)).filter(op => op.Present).map(op => op.get()).findFirst();
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableCreditRatesProvider toImmutableCreditRatesProvider()
	  {
		return this;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableCreditRatesProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableCreditRatesProvider.Meta meta()
	  {
		return ImmutableCreditRatesProvider.Meta.INSTANCE;
	  }

	  static ImmutableCreditRatesProvider()
	  {
		MetaBean.register(ImmutableCreditRatesProvider.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableCreditRatesProvider.Builder builder()
	  {
		return new ImmutableCreditRatesProvider.Builder();
	  }

	  private ImmutableCreditRatesProvider(LocalDate valuationDate, IDictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> creditCurves, IDictionary<Currency, CreditDiscountFactors> discountCurves, IDictionary<StandardId, RecoveryRates> recoveryRateCurves)
	  {
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		JodaBeanUtils.notNull(creditCurves, "creditCurves");
		JodaBeanUtils.notEmpty(discountCurves, "discountCurves");
		JodaBeanUtils.notEmpty(recoveryRateCurves, "recoveryRateCurves");
		this.valuationDate = valuationDate;
		this.creditCurves = ImmutableMap.copyOf(creditCurves);
		this.discountCurves = ImmutableMap.copyOf(discountCurves);
		this.recoveryRateCurves = ImmutableMap.copyOf(recoveryRateCurves);
		validate();
	  }

	  public override ImmutableCreditRatesProvider.Meta metaBean()
	  {
		return ImmutableCreditRatesProvider.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date.
	  /// <para>
	  /// All curves and other data items in this provider are calibrated for this date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the credit curves.
	  /// <para>
	  /// The curve data, predicting the survival probability, associated with each legal entity and currency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  private ImmutableMap<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> CreditCurves
	  {
		  get
		  {
			return creditCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discounting curves.
	  /// <para>
	  /// The curve data, predicting the discount factor, associated with each currency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  private ImmutableMap<Currency, CreditDiscountFactors> DiscountCurves
	  {
		  get
		  {
			return discountCurves;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the credit rate curves.
	  /// <para>
	  /// The curve date, predicting the recovery rate, associated with each legal entity.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  private ImmutableMap<StandardId, RecoveryRates> RecoveryRateCurves
	  {
		  get
		  {
			return recoveryRateCurves;
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
		  ImmutableCreditRatesProvider other = (ImmutableCreditRatesProvider) obj;
		  return JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(creditCurves, other.creditCurves) && JodaBeanUtils.equal(discountCurves, other.discountCurves) && JodaBeanUtils.equal(recoveryRateCurves, other.recoveryRateCurves);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(creditCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurves);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(recoveryRateCurves);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ImmutableCreditRatesProvider{");
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("creditCurves").Append('=').Append(creditCurves).Append(',').Append(' ');
		buf.Append("discountCurves").Append('=').Append(discountCurves).Append(',').Append(' ');
		buf.Append("recoveryRateCurves").Append('=').Append(JodaBeanUtils.ToString(recoveryRateCurves));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableCreditRatesProvider}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ImmutableCreditRatesProvider), typeof(LocalDate));
			  creditCurves_Renamed = DirectMetaProperty.ofImmutable(this, "creditCurves", typeof(ImmutableCreditRatesProvider), (Type) typeof(ImmutableMap));
			  discountCurves_Renamed = DirectMetaProperty.ofImmutable(this, "discountCurves", typeof(ImmutableCreditRatesProvider), (Type) typeof(ImmutableMap));
			  recoveryRateCurves_Renamed = DirectMetaProperty.ofImmutable(this, "recoveryRateCurves", typeof(ImmutableCreditRatesProvider), (Type) typeof(ImmutableMap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "valuationDate", "creditCurves", "discountCurves", "recoveryRateCurves");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code creditCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.basics.StandardId, com.opengamma.strata.basics.currency.Currency>, LegalEntitySurvivalProbabilities>> creditCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "creditCurves", ImmutableCreditRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities>> creditCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, CreditDiscountFactors>> discountCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "discountCurves", ImmutableCreditRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<Currency, CreditDiscountFactors>> discountCurves_Renamed;
		/// <summary>
		/// The meta-property for the {@code recoveryRateCurves} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.StandardId, RecoveryRates>> recoveryRateCurves = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "recoveryRateCurves", ImmutableCreditRatesProvider.class, (Class) com.google.common.collect.ImmutableMap.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableMap<StandardId, RecoveryRates>> recoveryRateCurves_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "valuationDate", "creditCurves", "discountCurves", "recoveryRateCurves");
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
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1612130883: // creditCurves
			  return creditCurves_Renamed;
			case -624113147: // discountCurves
			  return discountCurves_Renamed;
			case 1744098265: // recoveryRateCurves
			  return recoveryRateCurves_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableCreditRatesProvider.Builder builder()
		{
		  return new ImmutableCreditRatesProvider.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableCreditRatesProvider);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code creditCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities>> creditCurves()
		{
		  return creditCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<Currency, CreditDiscountFactors>> discountCurves()
		{
		  return discountCurves_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code recoveryRateCurves} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableMap<StandardId, RecoveryRates>> recoveryRateCurves()
		{
		  return recoveryRateCurves_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return ((ImmutableCreditRatesProvider) bean).ValuationDate;
			case -1612130883: // creditCurves
			  return ((ImmutableCreditRatesProvider) bean).CreditCurves;
			case -624113147: // discountCurves
			  return ((ImmutableCreditRatesProvider) bean).DiscountCurves;
			case 1744098265: // recoveryRateCurves
			  return ((ImmutableCreditRatesProvider) bean).RecoveryRateCurves;
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
	  /// The bean-builder for {@code ImmutableCreditRatesProvider}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableCreditRatesProvider>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate valuationDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> creditCurves_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<Currency, CreditDiscountFactors> discountCurves_Renamed = ImmutableMap.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IDictionary<StandardId, RecoveryRates> recoveryRateCurves_Renamed = ImmutableMap.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableCreditRatesProvider beanToCopy)
		{
		  this.valuationDate_Renamed = beanToCopy.ValuationDate;
		  this.creditCurves_Renamed = beanToCopy.CreditCurves;
		  this.discountCurves_Renamed = beanToCopy.DiscountCurves;
		  this.recoveryRateCurves_Renamed = beanToCopy.RecoveryRateCurves;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case -1612130883: // creditCurves
			  return creditCurves_Renamed;
			case -624113147: // discountCurves
			  return discountCurves_Renamed;
			case 1744098265: // recoveryRateCurves
			  return recoveryRateCurves_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 113107279: // valuationDate
			  this.valuationDate_Renamed = (LocalDate) newValue;
			  break;
			case -1612130883: // creditCurves
			  this.creditCurves_Renamed = (IDictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities>) newValue;
			  break;
			case -624113147: // discountCurves
			  this.discountCurves_Renamed = (IDictionary<Currency, CreditDiscountFactors>) newValue;
			  break;
			case 1744098265: // recoveryRateCurves
			  this.recoveryRateCurves_Renamed = (IDictionary<StandardId, RecoveryRates>) newValue;
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

		public override ImmutableCreditRatesProvider build()
		{
		  return new ImmutableCreditRatesProvider(valuationDate_Renamed, creditCurves_Renamed, discountCurves_Renamed, recoveryRateCurves_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the valuation date.
		/// <para>
		/// All curves and other data items in this provider are calibrated for this date.
		/// </para>
		/// </summary>
		/// <param name="valuationDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDate(LocalDate valuationDate)
		{
		  JodaBeanUtils.notNull(valuationDate, "valuationDate");
		  this.valuationDate_Renamed = valuationDate;
		  return this;
		}

		/// <summary>
		/// Sets the credit curves.
		/// <para>
		/// The curve data, predicting the survival probability, associated with each legal entity and currency.
		/// </para>
		/// </summary>
		/// <param name="creditCurves">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder creditCurves(IDictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> creditCurves)
		{
		  JodaBeanUtils.notNull(creditCurves, "creditCurves");
		  this.creditCurves_Renamed = creditCurves;
		  return this;
		}

		/// <summary>
		/// Sets the discounting curves.
		/// <para>
		/// The curve data, predicting the discount factor, associated with each currency.
		/// </para>
		/// </summary>
		/// <param name="discountCurves">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discountCurves(IDictionary<Currency, CreditDiscountFactors> discountCurves)
		{
		  JodaBeanUtils.notEmpty(discountCurves, "discountCurves");
		  this.discountCurves_Renamed = discountCurves;
		  return this;
		}

		/// <summary>
		/// Sets the credit rate curves.
		/// <para>
		/// The curve date, predicting the recovery rate, associated with each legal entity.
		/// </para>
		/// </summary>
		/// <param name="recoveryRateCurves">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder recoveryRateCurves(IDictionary<StandardId, RecoveryRates> recoveryRateCurves)
		{
		  JodaBeanUtils.notEmpty(recoveryRateCurves, "recoveryRateCurves");
		  this.recoveryRateCurves_Renamed = recoveryRateCurves;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ImmutableCreditRatesProvider.Builder{");
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate_Renamed)).Append(',').Append(' ');
		  buf.Append("creditCurves").Append('=').Append(JodaBeanUtils.ToString(creditCurves_Renamed)).Append(',').Append(' ');
		  buf.Append("discountCurves").Append('=').Append(JodaBeanUtils.ToString(discountCurves_Renamed)).Append(',').Append(' ');
		  buf.Append("recoveryRateCurves").Append('=').Append(JodaBeanUtils.ToString(recoveryRateCurves_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}