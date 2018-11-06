/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
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
	using FailureException = com.opengamma.strata.collect.result.FailureException;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using MarketDataNotFoundException = com.opengamma.strata.data.MarketDataNotFoundException;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

	/// <summary>
	/// Test <seealso cref="BuiltMarketData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BuiltMarketDataTest
	public class BuiltMarketDataTest
	{

	  private static readonly LocalDate VAL_DATE = date(2011, 3, 8);
	  private static readonly TestObservableId ID = TestObservableId.of("1");

	  //-------------------------------------------------------------------------
	  public virtual void test_withKnownFailure()
	  {
		string failureMessage = "Something went wrong";
		BuiltScenarioMarketData smd = BuiltScenarioMarketData.builder(MarketDataBox.ofSingleValue(VAL_DATE)).addResult(ID, Result.failure(FailureReason.ERROR, failureMessage)).build();
		BuiltMarketData test = new BuiltMarketData(smd);

		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.containsValue(ID), false);
		assertEquals(test.Ids, ImmutableSet.of());
		assertEquals(test.findValue(ID), null);
		assertThrows(() => test.getValue(ID), typeof(FailureException), failureMessage);
	  }

	  public virtual void test_withUnknownFailure()
	  {
		BuiltScenarioMarketData smd = BuiltScenarioMarketData.builder(MarketDataBox.ofSingleValue(VAL_DATE)).build();
		BuiltMarketData test = new BuiltMarketData(smd);

		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.containsValue(ID), false);
		assertEquals(test.Ids, ImmutableSet.of());
		assertEquals(test.findValue(ID), null);
		assertThrows(() => test.getValue(ID), typeof(MarketDataNotFoundException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(new BuiltMarketData(BuiltScenarioMarketData.empty()));
	  }

	}

}