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
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;

	/// <summary>
	/// The credit rates lookup, used to select curves for pricing.
	/// <para>
	/// This provides access to credit, discount and recovery rate curves.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultCreditRatesMarketDataLookup implements CreditRatesMarketDataLookup, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultCreditRatesMarketDataLookup : CreditRatesMarketDataLookup, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.basics.StandardId, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> creditCurveIds;
		private readonly ImmutableMap<Pair<StandardId, Currency>, CurveId> creditCurveIds;
	  /// <summary>
	  /// The discount curves, keyed by currency.
	  /// The curve data, predicting the future, associated with each currency.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurveIds;
	  private readonly ImmutableMap<Currency, CurveId> discountCurveIds;
	  /// <summary>
	  /// The recovery rate curves, keyed by standard ID.
	  /// The curve data, predicting the future, associated with each standard ID.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<com.opengamma.strata.basics.StandardId, com.opengamma.strata.market.curve.CurveId> recoveryRateCurveIds;
	  private readonly ImmutableMap<StandardId, CurveId> recoveryRateCurveIds;
	  /// <summary>
	  /// The source of market data for quotes and other observable market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  internal static DefaultCreditRatesMarketDataLookup of(IDictionary<Pair<StandardId, Currency>, CurveId> creditCurveIds, IDictionary<Currency, CurveId> discountCurveIds, IDictionary<StandardId, CurveId> recoveryRateCurveIds, ObservableSource observableSource)
	  {

		return new DefaultCreditRatesMarketDataLookup(creditCurveIds, discountCurveIds, recoveryRateCurveIds, observableSource);
	  }

	  //-------------------------------------------------------------------------
	  public FunctionRequirements requirements(StandardId legalEntityId, Currency currency)
	  {

		CurveId creditCurveId = creditCurveIds.get(Pair.of(legalEntityId, currency));
		if (creditCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Credit rates lookup has no credit curve defined for '{}' and '{}'", legalEntityId, currency));
		}
		CurveId discountCurveId = discountCurveIds.get(currency);
		if (discountCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Credit rates lookup has no discount curve defined for '{}'", currency));
		}
		CurveId recoveryRateCurveId = recoveryRateCurveIds.get(legalEntityId);
		if (recoveryRateCurveId == null)
		{
		  throw new System.ArgumentException(Messages.format("Credit rates lookup has no recovery rate curve defined for '{}'", legalEntityId));
		}

		return FunctionRequirements.builder().valueRequirements(ImmutableSet.of(creditCurveId, discountCurveId, recoveryRateCurveId)).outputCurrencies(currency).observableSource(observableSource).build();
	  }

	  //-------------------------------------------------------------------------
	  public CreditRatesProvider creditRatesProvider(MarketData marketData)
	  {
		return DefaultLookupCreditRatesProvider.of(this, marketData);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultCreditRatesMarketDataLookup}.
	  /// </summary>
	  private static readonly TypedMetaBean<DefaultCreditRatesMarketDataLookup> META_BEAN = LightMetaBean.of(typeof(DefaultCreditRatesMarketDataLookup), MethodHandles.lookup(), new string[] {"creditCurveIds", "discountCurveIds", "recoveryRateCurveIds", "observableSource"}, ImmutableMap.of(), ImmutableMap.of(), ImmutableMap.of(), null);

	  /// <summary>
	  /// The meta-bean for {@code DefaultCreditRatesMarketDataLookup}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultCreditRatesMarketDataLookup> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultCreditRatesMarketDataLookup()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DefaultCreditRatesMarketDataLookup(IDictionary<Pair<StandardId, Currency>, CurveId> creditCurveIds, IDictionary<Currency, CurveId> discountCurveIds, IDictionary<StandardId, CurveId> recoveryRateCurveIds, ObservableSource observableSource)
	  {
		JodaBeanUtils.notNull(creditCurveIds, "creditCurveIds");
		JodaBeanUtils.notNull(discountCurveIds, "discountCurveIds");
		JodaBeanUtils.notNull(recoveryRateCurveIds, "recoveryRateCurveIds");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		this.creditCurveIds = ImmutableMap.copyOf(creditCurveIds);
		this.discountCurveIds = ImmutableMap.copyOf(discountCurveIds);
		this.recoveryRateCurveIds = ImmutableMap.copyOf(recoveryRateCurveIds);
		this.observableSource = observableSource;
	  }

	  public override TypedMetaBean<DefaultCreditRatesMarketDataLookup> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the credit curves, keyed by standard ID and currency.
	  /// The curve data, predicting the future, associated with each standard ID and currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Pair<StandardId, Currency>, CurveId> CreditCurveIds
	  {
		  get
		  {
			return creditCurveIds;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount curves, keyed by currency.
	  /// The curve data, predicting the future, associated with each currency. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Currency, CurveId> DiscountCurveIds
	  {
		  get
		  {
			return discountCurveIds;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the recovery rate curves, keyed by standard ID.
	  /// The curve data, predicting the future, associated with each standard ID. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<StandardId, CurveId> RecoveryRateCurveIds
	  {
		  get
		  {
			return recoveryRateCurveIds;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of market data for quotes and other observable market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
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
		  DefaultCreditRatesMarketDataLookup other = (DefaultCreditRatesMarketDataLookup) obj;
		  return JodaBeanUtils.equal(creditCurveIds, other.creditCurveIds) && JodaBeanUtils.equal(discountCurveIds, other.discountCurveIds) && JodaBeanUtils.equal(recoveryRateCurveIds, other.recoveryRateCurveIds) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(creditCurveIds);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountCurveIds);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(recoveryRateCurveIds);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("DefaultCreditRatesMarketDataLookup{");
		buf.Append("creditCurveIds").Append('=').Append(creditCurveIds).Append(',').Append(' ');
		buf.Append("discountCurveIds").Append('=').Append(discountCurveIds).Append(',').Append(' ');
		buf.Append("recoveryRateCurveIds").Append('=').Append(recoveryRateCurveIds).Append(',').Append(' ');
		buf.Append("observableSource").Append('=').Append(JodaBeanUtils.ToString(observableSource));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}