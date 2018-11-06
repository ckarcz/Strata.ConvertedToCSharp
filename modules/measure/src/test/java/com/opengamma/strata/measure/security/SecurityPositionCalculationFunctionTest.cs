using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.security
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using FunctionRequirements = com.opengamma.strata.calc.runner.FunctionRequirements;
	using Result = com.opengamma.strata.collect.result.Result;
	using CurrencyScenarioArray = com.opengamma.strata.data.scenario.CurrencyScenarioArray;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using TestMarketDataMap = com.opengamma.strata.measure.curve.TestMarketDataMap;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;

	/// <summary>
	/// Test <seealso cref="SecurityPositionCalculationFunction"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityPositionCalculationFunctionTest
	public class SecurityPositionCalculationFunctionTest
	{

	  private static readonly CalculationParameters PARAMS = CalculationParameters.empty();
	  private const double MARKET_PRICE = 99.42;
	  private const double TICK_SIZE = 0.01;
	  private const int TICK_VALUE = 10;
	  private const int QUANTITY = 20;
	  private static readonly SecurityId SEC_ID = SecurityId.of("OG-Future", "Foo-Womble-Mar14");
	  public static readonly SecurityPosition POSITION = SecurityPosition.ofNet(SEC_ID, QUANTITY);
	  private static readonly GenericSecurity FUTURE = GenericSecurity.of(SecurityInfo.of(SEC_ID, TICK_SIZE, CurrencyAmount.of(EUR, TICK_VALUE)));
	  private static readonly ReferenceData REF_DATA = ImmutableReferenceData.of(SEC_ID, FUTURE);
	  private static readonly Currency CURRENCY = FUTURE.Currency;
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2013, 12, 8);

	  //-------------------------------------------------------------------------
	  public virtual void test_requirementsAndCurrency()
	  {
		SecurityPositionCalculationFunction function = new SecurityPositionCalculationFunction();
		ISet<Measure> measures = function.supportedMeasures();
		FunctionRequirements reqs = function.requirements(POSITION, measures, PARAMS, REF_DATA);
		assertThat(reqs.OutputCurrencies).containsOnly(CURRENCY);
		assertThat(reqs.ValueRequirements).isEqualTo(ImmutableSet.of(QuoteId.of(SEC_ID.StandardId)));
		assertThat(reqs.TimeSeriesRequirements).Empty;
		assertThat(function.naturalCurrency(POSITION, REF_DATA)).isEqualTo(CURRENCY);
	  }

	  public virtual void test_presentValue()
	  {
		SecurityPositionCalculationFunction function = new SecurityPositionCalculationFunction();
		ScenarioMarketData md = marketData();

		double unitPv = (MARKET_PRICE / TICK_SIZE) * TICK_VALUE;
		CurrencyAmount expectedPv = CurrencyAmount.of(CURRENCY, unitPv * QUANTITY);

		ISet<Measure> measures = ImmutableSet.of(Measures.PRESENT_VALUE);
		assertThat(function.calculate(POSITION, measures, PARAMS, md, REF_DATA)).containsEntry(Measures.PRESENT_VALUE, Result.success(CurrencyScenarioArray.of(ImmutableList.of(expectedPv))));
	  }

	  //-------------------------------------------------------------------------
	  private ScenarioMarketData marketData()
	  {
		TestMarketDataMap md = new TestMarketDataMap(VAL_DATE, ImmutableMap.of(QuoteId.of(SEC_ID.StandardId), MARKET_PRICE), ImmutableMap.of());
		return md;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(SecurityMeasureCalculations));
	  }

	}

}