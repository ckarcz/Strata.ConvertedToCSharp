using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedDataCombiner = com.opengamma.strata.market.param.ParameterizedDataCombiner;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Provides access to discount factors for currencies.
	/// <para>
	/// This provides discount factors for a single currency pair.
	/// </para>
	/// <para>
	/// This implementation is based on two underlying <seealso cref="DiscountFactors"/> objects,
	/// one for each currency, and an <seealso cref="FxRateProvider"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class DiscountFxForwardRates implements FxForwardRates, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DiscountFxForwardRates : FxForwardRates, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
		private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The provider of FX rates.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.FxRateProvider fxRateProvider;
	  private readonly FxRateProvider fxRateProvider;
	  /// <summary>
	  /// The discount factors for the base currency of the currency pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.DiscountFactors baseCurrencyDiscountFactors;
	  private readonly DiscountFactors baseCurrencyDiscountFactors;
	  /// <summary>
	  /// The discount factors for the counter currency of the currency pair.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.DiscountFactors counterCurrencyDiscountFactors;
	  private readonly DiscountFactors counterCurrencyDiscountFactors;
	  /// <summary>
	  /// The valuation date.
	  /// </summary>
	  [NonSerialized]
	  private readonly LocalDate valuationDate; // not a property, derived and cached from input data
	  /// <summary>
	  /// The parameter combiner.
	  /// </summary>
	  [NonSerialized]
	  private readonly ParameterizedDataCombiner paramCombiner; // not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on two discount factors, one for each currency.
	  /// <para>
	  /// The instance is based on the discount factors for each currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="fxRateProvider">  the provider of FX rates </param>
	  /// <param name="baseCurrencyFactors">  the discount factors in the base currency of the index </param>
	  /// <param name="counterCurrencyFactors">  the discount factors in the counter currency of the index </param>
	  /// <returns> the rates instance </returns>
	  public static DiscountFxForwardRates of(CurrencyPair currencyPair, FxRateProvider fxRateProvider, DiscountFactors baseCurrencyFactors, DiscountFactors counterCurrencyFactors)
	  {

		return new DiscountFxForwardRates(currencyPair, fxRateProvider, baseCurrencyFactors, counterCurrencyFactors);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DiscountFxForwardRates(com.opengamma.strata.basics.currency.CurrencyPair currencyPair, com.opengamma.strata.basics.currency.FxRateProvider fxRateProvider, com.opengamma.strata.pricer.DiscountFactors baseCurrencyDiscountFactors, com.opengamma.strata.pricer.DiscountFactors counterCurrencyDiscountFactors)
	  private DiscountFxForwardRates(CurrencyPair currencyPair, FxRateProvider fxRateProvider, DiscountFactors baseCurrencyDiscountFactors, DiscountFactors counterCurrencyDiscountFactors)
	  {
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(fxRateProvider, "fxRateProvider");
		JodaBeanUtils.notNull(baseCurrencyDiscountFactors, "baseCurrencyDiscountFactors");
		JodaBeanUtils.notNull(counterCurrencyDiscountFactors, "counterCurrencyDiscountFactors");
		if (!baseCurrencyDiscountFactors.Currency.Equals(currencyPair.Base))
		{
		  throw new System.ArgumentException(Messages.format("Index base currency {} did not match discount factor base currency {}", currencyPair.Base, baseCurrencyDiscountFactors.Currency));
		}
		if (!counterCurrencyDiscountFactors.Currency.Equals(currencyPair.Counter))
		{
		  throw new System.ArgumentException(Messages.format("Index counter currency {} did not match discount factor counter currency {}", currencyPair.Counter, counterCurrencyDiscountFactors.Currency));
		}
		if (!baseCurrencyDiscountFactors.ValuationDate.Equals(counterCurrencyDiscountFactors.ValuationDate))
		{
		  throw new System.ArgumentException("Curves must have the same valuation date");
		}
		this.currencyPair = currencyPair;
		this.fxRateProvider = fxRateProvider;
		this.baseCurrencyDiscountFactors = baseCurrencyDiscountFactors;
		this.counterCurrencyDiscountFactors = counterCurrencyDiscountFactors;
		this.valuationDate = baseCurrencyDiscountFactors.ValuationDate;
		this.paramCombiner = ParameterizedDataCombiner.of(baseCurrencyDiscountFactors, counterCurrencyDiscountFactors);
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new DiscountFxForwardRates(currencyPair, fxRateProvider, baseCurrencyDiscountFactors, counterCurrencyDiscountFactors);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return baseCurrencyDiscountFactors.findData(name).map(Optional.of).orElse(counterCurrencyDiscountFactors.findData(name));
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return paramCombiner.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return paramCombiner.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return paramCombiner.getParameterMetadata(parameterIndex);
	  }

	  public DiscountFxForwardRates withParameter(int parameterIndex, double newValue)
	  {
		return new DiscountFxForwardRates(currencyPair, fxRateProvider, paramCombiner.underlyingWithParameter(0, typeof(DiscountFactors), parameterIndex, newValue), paramCombiner.underlyingWithParameter(1, typeof(DiscountFactors), parameterIndex, newValue));
	  }

	  public DiscountFxForwardRates withPerturbation(ParameterPerturbation perturbation)
	  {
		return new DiscountFxForwardRates(currencyPair, fxRateProvider, paramCombiner.underlyingWithPerturbation(0, typeof(DiscountFactors), perturbation), paramCombiner.underlyingWithPerturbation(1, typeof(DiscountFactors), perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double rate(Currency baseCurrency, LocalDate referenceDate)
	  {
		ArgChecker.isTrue(currencyPair.contains(baseCurrency), "Currency {} invalid for CurrencyPair {}", baseCurrency, currencyPair);
		bool inverse = baseCurrency.Equals(currencyPair.Counter);
		double dfCcyBaseAtMaturity = baseCurrencyDiscountFactors.discountFactor(referenceDate);
		double dfCcyCounterAtMaturity = counterCurrencyDiscountFactors.discountFactor(referenceDate);
		double forwardRate = fxRateProvider.fxRate(currencyPair) * (dfCcyBaseAtMaturity / dfCcyCounterAtMaturity);
		return inverse ? 1d / forwardRate : forwardRate;
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder ratePointSensitivity(Currency baseCurrency, LocalDate referenceDate)
	  {
		ArgChecker.isTrue(currencyPair.contains(baseCurrency), "Currency {} invalid for CurrencyPair {}", baseCurrency, currencyPair);
		return FxForwardSensitivity.of(currencyPair, baseCurrency, referenceDate, 1d);
	  }

	  //-------------------------------------------------------------------------
	  public double rateFxSpotSensitivity(Currency baseCurrency, LocalDate referenceDate)
	  {
		ArgChecker.isTrue(currencyPair.contains(baseCurrency), "Currency {} invalid for CurrencyPair {}", baseCurrency, currencyPair);
		bool inverse = baseCurrency.Equals(currencyPair.Counter);
		double dfCcyBaseAtMaturity = baseCurrencyDiscountFactors.discountFactor(referenceDate);
		double dfCcyCounterAtMaturity = counterCurrencyDiscountFactors.discountFactor(referenceDate);
		double forwardRateDelta = dfCcyBaseAtMaturity / dfCcyCounterAtMaturity;
		return inverse ? 1d / forwardRateDelta : forwardRateDelta;
	  }

	  //-------------------------------------------------------------------------
	  public CurrencyParameterSensitivities parameterSensitivity(FxForwardSensitivity pointSensitivity)
	  {
		// use the specified base currency to determine the desired currency pair
		// then derive sensitivity from discount factors based off desired currency pair, not that of the index
		CurrencyPair currencyPair = pointSensitivity.CurrencyPair;
		Currency refBaseCurrency = pointSensitivity.ReferenceCurrency;
		Currency refCounterCurrency = pointSensitivity.ReferenceCounterCurrency;
		Currency sensitivityCurrency = pointSensitivity.Currency;
		LocalDate referenceDate = pointSensitivity.ReferenceDate;

		bool inverse = refBaseCurrency.Equals(currencyPair.Counter);
		DiscountFactors discountFactorsRefBase = (inverse ? counterCurrencyDiscountFactors : baseCurrencyDiscountFactors);
		DiscountFactors discountFactorsRefCounter = (inverse ? baseCurrencyDiscountFactors : counterCurrencyDiscountFactors);
		double dfCcyBaseAtMaturity = discountFactorsRefBase.discountFactor(referenceDate);
		double dfCcyCounterAtMaturityInv = 1d / discountFactorsRefCounter.discountFactor(referenceDate);

		double fxRate = fxRateProvider.fxRate(refBaseCurrency, refCounterCurrency);
		ZeroRateSensitivity dfCcyBaseAtMaturitySensitivity = discountFactorsRefBase.zeroRatePointSensitivity(referenceDate, sensitivityCurrency).multipliedBy(fxRate * dfCcyCounterAtMaturityInv * pointSensitivity.Sensitivity);

		ZeroRateSensitivity dfCcyCounterAtMaturitySensitivity = discountFactorsRefCounter.zeroRatePointSensitivity(referenceDate, sensitivityCurrency).multipliedBy(-fxRate * dfCcyBaseAtMaturity * dfCcyCounterAtMaturityInv * dfCcyCounterAtMaturityInv * pointSensitivity.Sensitivity);

		return discountFactorsRefBase.parameterSensitivity(dfCcyBaseAtMaturitySensitivity).combinedWith(discountFactorsRefCounter.parameterSensitivity(dfCcyCounterAtMaturitySensitivity));
	  }

	  public MultiCurrencyAmount currencyExposure(FxForwardSensitivity pointSensitivity)
	  {
		ArgChecker.isTrue(pointSensitivity.Currency.Equals(pointSensitivity.ReferenceCurrency), "Currency exposure defined only when sensitivity currency equal reference currency");
		Currency ccyRef = pointSensitivity.ReferenceCurrency;
		CurrencyPair pair = pointSensitivity.CurrencyPair;
		double s = pointSensitivity.Sensitivity;
		LocalDate d = pointSensitivity.ReferenceDate;
		double f = fxRateProvider.fxRate(pair.Base, pair.Counter);
		double pA = baseCurrencyDiscountFactors.discountFactor(d);
		double pB = counterCurrencyDiscountFactors.discountFactor(d);
		if (ccyRef.Equals(pair.Base))
		{
		  CurrencyAmount amountCounter = CurrencyAmount.of(pair.Base, s * f * pA / pB);
		  CurrencyAmount amountBase = CurrencyAmount.of(pair.Counter, -s * f * f * pA / pB);
		  return MultiCurrencyAmount.of(amountBase, amountCounter);
		}
		else
		{
		  CurrencyAmount amountBase = CurrencyAmount.of(pair.Base, -s * pB / (pA * f * f));
		  CurrencyAmount amountCounter = CurrencyAmount.of(pair.Counter, s * pB / (pA * f));
		  return MultiCurrencyAmount.of(amountBase, amountCounter);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a new instance with different discount factors.
	  /// </summary>
	  /// <param name="baseCurrencyFactors">  the new base currency discount factors </param>
	  /// <param name="counterCurrencyFactors">  the new counter currency discount factors </param>
	  /// <returns> the new instance </returns>
	  public DiscountFxForwardRates withDiscountFactors(DiscountFactors baseCurrencyFactors, DiscountFactors counterCurrencyFactors)
	  {
		return new DiscountFxForwardRates(currencyPair, fxRateProvider, baseCurrencyFactors, counterCurrencyFactors);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DiscountFxForwardRates}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DiscountFxForwardRates.Meta meta()
	  {
		return DiscountFxForwardRates.Meta.INSTANCE;
	  }

	  static DiscountFxForwardRates()
	  {
		MetaBean.register(DiscountFxForwardRates.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override DiscountFxForwardRates.Meta metaBean()
	  {
		return DiscountFxForwardRates.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that the rates are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the provider of FX rates. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxRateProvider FxRateProvider
	  {
		  get
		  {
			return fxRateProvider;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factors for the base currency of the currency pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DiscountFactors BaseCurrencyDiscountFactors
	  {
		  get
		  {
			return baseCurrencyDiscountFactors;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factors for the counter currency of the currency pair. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DiscountFactors CounterCurrencyDiscountFactors
	  {
		  get
		  {
			return counterCurrencyDiscountFactors;
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
		  DiscountFxForwardRates other = (DiscountFxForwardRates) obj;
		  return JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(fxRateProvider, other.fxRateProvider) && JodaBeanUtils.equal(baseCurrencyDiscountFactors, other.baseCurrencyDiscountFactors) && JodaBeanUtils.equal(counterCurrencyDiscountFactors, other.counterCurrencyDiscountFactors);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRateProvider);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(baseCurrencyDiscountFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(counterCurrencyDiscountFactors);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("DiscountFxForwardRates{");
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("fxRateProvider").Append('=').Append(fxRateProvider).Append(',').Append(' ');
		buf.Append("baseCurrencyDiscountFactors").Append('=').Append(baseCurrencyDiscountFactors).Append(',').Append(' ');
		buf.Append("counterCurrencyDiscountFactors").Append('=').Append(JodaBeanUtils.ToString(counterCurrencyDiscountFactors));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DiscountFxForwardRates}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(DiscountFxForwardRates), typeof(CurrencyPair));
			  fxRateProvider_Renamed = DirectMetaProperty.ofImmutable(this, "fxRateProvider", typeof(DiscountFxForwardRates), typeof(FxRateProvider));
			  baseCurrencyDiscountFactors_Renamed = DirectMetaProperty.ofImmutable(this, "baseCurrencyDiscountFactors", typeof(DiscountFxForwardRates), typeof(DiscountFactors));
			  counterCurrencyDiscountFactors_Renamed = DirectMetaProperty.ofImmutable(this, "counterCurrencyDiscountFactors", typeof(DiscountFxForwardRates), typeof(DiscountFactors));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "currencyPair", "fxRateProvider", "baseCurrencyDiscountFactors", "counterCurrencyDiscountFactors");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxRateProvider} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxRateProvider> fxRateProvider_Renamed;
		/// <summary>
		/// The meta-property for the {@code baseCurrencyDiscountFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DiscountFactors> baseCurrencyDiscountFactors_Renamed;
		/// <summary>
		/// The meta-property for the {@code counterCurrencyDiscountFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DiscountFactors> counterCurrencyDiscountFactors_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "currencyPair", "fxRateProvider", "baseCurrencyDiscountFactors", "counterCurrencyDiscountFactors");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -1499624221: // fxRateProvider
			  return fxRateProvider_Renamed;
			case 1151357473: // baseCurrencyDiscountFactors
			  return baseCurrencyDiscountFactors_Renamed;
			case -453959018: // counterCurrencyDiscountFactors
			  return counterCurrencyDiscountFactors_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends DiscountFxForwardRates> builder()
		public override BeanBuilder<DiscountFxForwardRates> builder()
		{
		  return new DiscountFxForwardRates.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DiscountFxForwardRates);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxRateProvider} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxRateProvider> fxRateProvider()
		{
		  return fxRateProvider_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code baseCurrencyDiscountFactors} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DiscountFactors> baseCurrencyDiscountFactors()
		{
		  return baseCurrencyDiscountFactors_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code counterCurrencyDiscountFactors} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DiscountFactors> counterCurrencyDiscountFactors()
		{
		  return counterCurrencyDiscountFactors_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  return ((DiscountFxForwardRates) bean).CurrencyPair;
			case -1499624221: // fxRateProvider
			  return ((DiscountFxForwardRates) bean).FxRateProvider;
			case 1151357473: // baseCurrencyDiscountFactors
			  return ((DiscountFxForwardRates) bean).BaseCurrencyDiscountFactors;
			case -453959018: // counterCurrencyDiscountFactors
			  return ((DiscountFxForwardRates) bean).CounterCurrencyDiscountFactors;
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
	  /// The bean-builder for {@code DiscountFxForwardRates}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<DiscountFxForwardRates>
	  {

		internal CurrencyPair currencyPair;
		internal FxRateProvider fxRateProvider;
		internal DiscountFactors baseCurrencyDiscountFactors;
		internal DiscountFactors counterCurrencyDiscountFactors;

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
			case 1005147787: // currencyPair
			  return currencyPair;
			case -1499624221: // fxRateProvider
			  return fxRateProvider;
			case 1151357473: // baseCurrencyDiscountFactors
			  return baseCurrencyDiscountFactors;
			case -453959018: // counterCurrencyDiscountFactors
			  return counterCurrencyDiscountFactors;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1005147787: // currencyPair
			  this.currencyPair = (CurrencyPair) newValue;
			  break;
			case -1499624221: // fxRateProvider
			  this.fxRateProvider = (FxRateProvider) newValue;
			  break;
			case 1151357473: // baseCurrencyDiscountFactors
			  this.baseCurrencyDiscountFactors = (DiscountFactors) newValue;
			  break;
			case -453959018: // counterCurrencyDiscountFactors
			  this.counterCurrencyDiscountFactors = (DiscountFactors) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override DiscountFxForwardRates build()
		{
		  return new DiscountFxForwardRates(currencyPair, fxRateProvider, baseCurrencyDiscountFactors, counterCurrencyDiscountFactors);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("DiscountFxForwardRates.Builder{");
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair)).Append(',').Append(' ');
		  buf.Append("fxRateProvider").Append('=').Append(JodaBeanUtils.ToString(fxRateProvider)).Append(',').Append(' ');
		  buf.Append("baseCurrencyDiscountFactors").Append('=').Append(JodaBeanUtils.ToString(baseCurrencyDiscountFactors)).Append(',').Append(' ');
		  buf.Append("counterCurrencyDiscountFactors").Append('=').Append(JodaBeanUtils.ToString(counterCurrencyDiscountFactors));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}