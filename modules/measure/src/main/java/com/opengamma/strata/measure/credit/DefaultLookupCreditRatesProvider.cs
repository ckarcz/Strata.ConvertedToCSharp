using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using ZeroRateSensitivity = com.opengamma.strata.pricer.ZeroRateSensitivity;
	using CreditCurveZeroRateSensitivity = com.opengamma.strata.pricer.credit.CreditCurveZeroRateSensitivity;
	using CreditDiscountFactors = com.opengamma.strata.pricer.credit.CreditDiscountFactors;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using ImmutableCreditRatesProvider = com.opengamma.strata.pricer.credit.ImmutableCreditRatesProvider;
	using LegalEntitySurvivalProbabilities = com.opengamma.strata.pricer.credit.LegalEntitySurvivalProbabilities;
	using RecoveryRates = com.opengamma.strata.pricer.credit.RecoveryRates;

	/// <summary>
	/// A credit rates provider based on a credit rates lookup.
	/// <para>
	/// This uses a <seealso cref="DefaultCreditRatesMarketDataLookup"/> to provide a view on <seealso cref="MarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultLookupCreditRatesProvider implements com.opengamma.strata.pricer.credit.CreditRatesProvider, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultLookupCreditRatesProvider : CreditRatesProvider, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final DefaultCreditRatesMarketDataLookup lookup;
		private readonly DefaultCreditRatesMarketDataLookup lookup;
	  /// <summary>
	  /// The market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.MarketData marketData;
	  private readonly MarketData marketData;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a lookup and market data.
	  /// </summary>
	  /// <param name="lookup">  the lookup </param>
	  /// <param name="marketData">  the market data </param>
	  /// <returns> the credit rates provider </returns>
	  public static DefaultLookupCreditRatesProvider of(DefaultCreditRatesMarketDataLookup lookup, MarketData marketData)
	  {

		return new DefaultLookupCreditRatesProvider(lookup, marketData);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private DefaultLookupCreditRatesProvider(DefaultCreditRatesMarketDataLookup lookup, com.opengamma.strata.data.MarketData marketData)
	  private DefaultLookupCreditRatesProvider(DefaultCreditRatesMarketDataLookup lookup, MarketData marketData)
	  {

		this.lookup = ArgChecker.notNull(lookup, "lookup");
		this.marketData = ArgChecker.notNull(marketData, "marketData");
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new DefaultLookupCreditRatesProvider(lookup, marketData);
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return marketData.ValuationDate;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public LegalEntitySurvivalProbabilities survivalProbabilities(StandardId legalEntityId, Currency currency)
	  {
		CurveId curveId = lookup.CreditCurveIds.get(Pair.of(legalEntityId, currency));
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException("Unable to find credit curve: " + legalEntityId + ", " + currency);
		}
		Curve curve = marketData.getValue(curveId);
		CreditDiscountFactors survivalProbabilities = CreditDiscountFactors.of(currency, ValuationDate, curve);
		return LegalEntitySurvivalProbabilities.of(legalEntityId, survivalProbabilities);
	  }

	  public CreditDiscountFactors discountFactors(Currency currency)
	  {
		CurveId curveId = lookup.DiscountCurveIds.get(currency);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException("Unable to find discount curve: " + currency);
		}
		Curve curve = marketData.getValue(curveId);
		return CreditDiscountFactors.of(currency, ValuationDate, curve);
	  }

	  public RecoveryRates recoveryRates(StandardId legalEntityId)
	  {
		CurveId curveId = lookup.RecoveryRateCurveIds.get(legalEntityId);
		if (curveId == null)
		{
		  throw new MarketDataNotFoundException("Unable to find recovery rate curve: " + legalEntityId);
		}
		Curve curve = marketData.getValue(curveId);
		return RecoveryRates.of(legalEntityId, ValuationDate, curve);
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
			if (pt.Currency.Equals(currency))
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
		  return Stream.concat(lookup.RecoveryRateCurveIds.values().stream(), Stream.concat(lookup.CreditCurveIds.values().stream(), lookup.DiscountCurveIds.values().stream())).filter(id => id.MarketDataName.Equals(name)).findFirst().flatMap(id => marketData.findValue(id)).map(v => name.MarketDataType.cast(v));
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableCreditRatesProvider toImmutableCreditRatesProvider()
	  {

		LocalDate valuationDate = ValuationDate;
		// credit curves
		IDictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities> creditCurves = new Dictionary<Pair<StandardId, Currency>, LegalEntitySurvivalProbabilities>();
		foreach (Pair<StandardId, Currency> pair in lookup.CreditCurveIds.Keys)
		{
		  CurveId curveId = lookup.CreditCurveIds.get(pair);
		  if (marketData.containsValue(curveId))
		  {
			Curve curve = marketData.getValue(curveId);
			CreditDiscountFactors survivalProbabilities = CreditDiscountFactors.of(pair.Second, valuationDate, curve);
			creditCurves[pair] = LegalEntitySurvivalProbabilities.of(pair.First, survivalProbabilities);
		  }
		}
		// discount curves
		IDictionary<Currency, CreditDiscountFactors> discountCurves = new Dictionary<Currency, CreditDiscountFactors>();
		foreach (Currency currency in lookup.DiscountCurveIds.Keys)
		{
		  CurveId curveId = lookup.DiscountCurveIds.get(currency);
		  if (marketData.containsValue(curveId))
		  {
			Curve curve = marketData.getValue(curveId);
			discountCurves[currency] = CreditDiscountFactors.of(currency, valuationDate, curve);
		  }
		}
		// recovery rate curves
		IDictionary<StandardId, RecoveryRates> recoveryRateCurves = new Dictionary<StandardId, RecoveryRates>();
		foreach (StandardId legalEntityId in lookup.RecoveryRateCurveIds.Keys)
		{
		  CurveId curveId = lookup.RecoveryRateCurveIds.get(legalEntityId);
		  if (marketData.containsValue(curveId))
		  {
			Curve curve = marketData.getValue(curveId);
			RecoveryRates recoveryRate = RecoveryRates.of(legalEntityId, valuationDate, curve);
			recoveryRateCurves[legalEntityId] = recoveryRate;
		  }
		}
		// build result
		return ImmutableCreditRatesProvider.builder().valuationDate(valuationDate).creditCurves(creditCurves).discountCurves(discountCurves).recoveryRateCurves(recoveryRateCurves).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupCreditRatesProvider}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultLookupCreditRatesProvider> META_BEAN = LightMetaBean.of(typeof(DefaultLookupCreditRatesProvider), MethodHandles.lookup(), new string[] {"lookup", "marketData"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DefaultLookupCreditRatesProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultLookupCreditRatesProvider> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultLookupCreditRatesProvider()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<DefaultLookupCreditRatesProvider> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the lookup. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DefaultCreditRatesMarketDataLookup Lookup
	  {
		  get
		  {
			return lookup;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public MarketData MarketData
	  {
		  get
		  {
			return marketData;
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
		  DefaultLookupCreditRatesProvider other = (DefaultLookupCreditRatesProvider) obj;
		  return JodaBeanUtils.equal(lookup, other.lookup) && JodaBeanUtils.equal(marketData, other.marketData);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lookup);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(marketData);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("DefaultLookupCreditRatesProvider{");
		buf.Append("lookup").Append('=').Append(lookup).Append(',').Append(' ');
		buf.Append("marketData").Append('=').Append(JodaBeanUtils.ToString(marketData));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}