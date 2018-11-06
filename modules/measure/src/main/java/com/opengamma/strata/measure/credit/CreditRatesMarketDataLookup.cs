/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;

	/// <summary>
	/// The lookup that provides access to credit rates in market data.
	/// <para>
	/// The credit rates market lookup provides access to credit, discount and recovery rate curves.
	/// </para>
	/// <para>
	/// The lookup implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface CreditRatesMarketDataLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains an instance based on a maps for credit, discount and recovery rate curves.
	  /// </summary>
	  /// <param name="creditCurveIds">  the credit curve identifiers, keyed by legal entity ID and currency </param>
	  /// <param name="discountCurveIds">  the discount curve identifiers, keyed by currency </param>
	  /// <param name="recoveryRateCurveIds">  the recovery rate curve identifiers, keyed by legal entity ID </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CreditRatesMarketDataLookup of(java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.basics.StandardId, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> creditCurveIds, java.util.Map<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurveIds, java.util.Map<com.opengamma.strata.basics.StandardId, com.opengamma.strata.market.curve.CurveId> recoveryRateCurveIds)
	//  {
	//
	//	return DefaultCreditRatesMarketDataLookup.of(creditCurveIds, discountCurveIds, recoveryRateCurveIds, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance based on a maps for credit, discount and recovery rate curves.
	  /// </summary>
	  /// <param name="creditCurveIds">  the credit curve identifiers, keyed by legal entity ID and currency </param>
	  /// <param name="discountCurveIds">  the discount curve identifiers, keyed by currency </param>
	  /// <param name="recoveryRateCurveIds">  the recovery rate curve identifiers, keyed by legal entity ID </param>
	  /// <param name="observableSource">  the source of market data for quotes and other observable market data </param>
	  /// <returns> the rates lookup containing the specified curves </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CreditRatesMarketDataLookup of(java.util.Map<com.opengamma.strata.collect.tuple.Pair<com.opengamma.strata.basics.StandardId, com.opengamma.strata.basics.currency.Currency>, com.opengamma.strata.market.curve.CurveId> creditCurveIds, java.util.Map<com.opengamma.strata.basics.currency.Currency, com.opengamma.strata.market.curve.CurveId> discountCurveIds, java.util.Map<com.opengamma.strata.basics.StandardId, com.opengamma.strata.market.curve.CurveId> recoveryRateCurveIds, com.opengamma.strata.data.ObservableSource observableSource)
	//  {
	//
	//	return DefaultCreditRatesMarketDataLookup.of(creditCurveIds, discountCurveIds, recoveryRateCurveIds, observableSource);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type that the lookup will be queried by.
	  /// <para>
	  /// This returns {@code CreditRatesMarketDataLookup.class}.
	  /// When querying parameters using <seealso cref="CalculationParameters#findParameter(Class)"/>,
	  /// {@code CreditRatesMarketDataLookup.class} must be passed in to find the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the parameter implementation </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return CreditRatesMarketDataLookup.class;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates market data requirements for the specified standard ID and currency.
	  /// </summary>
	  /// <param name="legalEntityId">  legal entity ID </param>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the requirements </returns>
	  /// <exception cref="IllegalArgumentException"> if unable to create requirements </exception>
	  FunctionRequirements requirements(StandardId legalEntityId, Currency currency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="ScenarioMarketData"/>, which contains market data for all scenarios.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for all scenarios </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CreditRatesScenarioMarketData marketDataView(com.opengamma.strata.data.scenario.ScenarioMarketData marketData)
	//  {
	//	return DefaultCreditRatesScenarioMarketData.of(this, marketData);
	//  }

	  /// <summary>
	  /// Obtains a filtered view of the complete set of market data.
	  /// <para>
	  /// This method returns an instance that binds the lookup to the market data.
	  /// The input is <seealso cref="MarketData"/>, which contains market data for one scenario.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the filtered market data </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CreditRatesMarketData marketDataView(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	return DefaultCreditRatesMarketData.of(this, marketData);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains credit rates provider based on the specified market data.
	  /// <para>
	  /// This provides <seealso cref="CreditRatesProvider"/> suitable for pricing credit products.
	  /// Although this method can be used directly, it is typically invoked indirectly
	  /// via <seealso cref="CreditRatesMarketData"/>:
	  /// <pre>
	  ///  // bind the baseData to this lookup
	  ///  CreditRatesMarketData view = lookup.marketView(baseData);
	  /// 
	  ///  // pass around CreditRatesMarketData within the function to use in pricing
	  ///  CreditRatesProvider provider = view.creditRatesProvider();
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the rates provider </returns>
	  CreditRatesProvider creditRatesProvider(MarketData marketData);

	}

}