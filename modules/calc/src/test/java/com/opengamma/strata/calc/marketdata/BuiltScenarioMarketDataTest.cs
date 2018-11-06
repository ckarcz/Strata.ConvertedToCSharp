/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FailureException = com.opengamma.strata.collect.result.FailureException;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

	/// <summary>
	/// Test <seealso cref="BuiltScenarioMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BuiltScenarioMarketDataTest
	public class BuiltScenarioMarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2011, 3, 8);
	  private static readonly TestObservableId ID = TestObservableId.of("1");

	  //-------------------------------------------------------------------------
	  public virtual void test_getValue_fxIdentity()
	  {
		BuiltScenarioMarketData test = BuiltScenarioMarketData.builder(MarketDataBox.ofSingleValue(VAL_DATE)).build();

		assertEquals(test.ScenarioCount, 1);
		assertEquals(test.getValue(FxRateId.of(GBP, GBP)), MarketDataBox.ofSingleValue(FxRate.of(GBP, GBP, 1)));
	  }

	  public virtual void test_getValue_withKnownFailure()
	  {
		string failureMessage = "Something went wrong";
		BuiltScenarioMarketData test = BuiltScenarioMarketData.builder(MarketDataBox.ofSingleValue(VAL_DATE)).addResult(ID, Result.failure(FailureReason.ERROR, failureMessage)).build();

		assertEquals(test.ValuationDate, MarketDataBox.ofSingleValue(VAL_DATE));
		assertEquals(test.containsValue(ID), false);
		assertEquals(test.Ids, ImmutableSet.of());
		assertEquals(test.findValue(ID), null);
		assertThrows(() => test.getValue(ID), typeof(FailureException), failureMessage);
	  }

	  public virtual void test_getValue_withUnknownFailure()
	  {
		BuiltScenarioMarketData test = BuiltScenarioMarketData.builder(MarketDataBox.ofSingleValue(VAL_DATE)).build();

		assertEquals(test.ValuationDate, MarketDataBox.ofSingleValue(VAL_DATE));
		assertEquals(test.containsValue(ID), false);
		assertEquals(test.Ids, ImmutableSet.of());
		assertEquals(test.findValue(ID), null);
		assertThrows(() => test.getValue(ID), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(BuiltScenarioMarketData.empty());
	  }

	}

}