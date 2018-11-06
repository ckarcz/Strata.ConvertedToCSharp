/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using TestingMeasures = com.opengamma.strata.calc.TestingMeasures;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using TestParameter = com.opengamma.strata.calc.runner.TestParameter;
	using TestParameter2 = com.opengamma.strata.calc.runner.TestParameter2;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using GenericSecurityTrade = com.opengamma.strata.product.GenericSecurityTrade;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;

	/// <summary>
	/// Test <seealso cref="TradeCounterpartyCalculationParameter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeCounterpartyCalculationParameterTest
	public class TradeCounterpartyCalculationParameterTest
	{

	  private static readonly CalculationParameter PARAM1 = new TestParameter();
	  private static readonly CalculationParameter PARAM2 = new TestParameter();
	  private static readonly CalculationParameter PARAM3 = new TestParameter();
	  private static readonly CalculationParameter PARAM_OTHER = new TestParameter2();

	  private static readonly StandardId ID1 = StandardId.of("test", "cpty1");
	  private static readonly StandardId ID2 = StandardId.of("test", "cpty2");
	  private static readonly StandardId ID3 = StandardId.of("test", "cpty3");

	  private static readonly SecurityInfo SEC_INFO_1 = SecurityInfo.of(SecurityId.of("test", "sec1"), 1.0, CurrencyAmount.of(Currency.EUR, 1.0));
	  private static readonly GenericSecurity SEC_1 = GenericSecurity.of(SEC_INFO_1);
	  private static readonly TradeInfo TRADE_INFO_1 = TradeInfo.builder().counterparty(ID1).build();
	  private static readonly GenericSecurityTrade TRADE_1 = GenericSecurityTrade.of(TRADE_INFO_1, SEC_1, 1, 1.0);

	  private static readonly SecurityInfo SEC_INFO_2 = SecurityInfo.of(SecurityId.of("test", "sec2"), 2.0, CurrencyAmount.of(Currency.EUR, 2.0));
	  private static readonly GenericSecurity SEC_2 = GenericSecurity.of(SEC_INFO_2);
	  private static readonly TradeInfo TRADE_INFO_2 = TradeInfo.builder().counterparty(ID2).build();
	  private static readonly GenericSecurityTrade TRADE_2 = GenericSecurityTrade.of(TRADE_INFO_2, SEC_2, 2, 2.0);
	  private static readonly TradeInfo TRADE_INFO_3 = TradeInfo.builder().counterparty(ID3).build();
	  private static readonly GenericSecurityTrade TRADE_3 = GenericSecurityTrade.of(TRADE_INFO_3, SEC_2, 2, 2.0);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		TradeCounterpartyCalculationParameter test = TradeCounterpartyCalculationParameter.of(ImmutableMap.of(ID1, PARAM1, ID2, PARAM2), PARAM3);
		assertEquals(test.QueryType, typeof(TestParameter));
		assertEquals(test.Parameters.size(), 2);
		assertEquals(test.DefaultParameter, PARAM3);
		assertEquals(test.queryType(), typeof(TestParameter));
		assertEquals(test.filter(TRADE_1, TestingMeasures.PRESENT_VALUE), PARAM1);
		assertEquals(test.filter(TRADE_2, TestingMeasures.PRESENT_VALUE), PARAM2);
		assertEquals(test.filter(TRADE_3, TestingMeasures.PRESENT_VALUE), PARAM3);
	  }

	  public virtual void of_empty()
	  {
		assertThrowsIllegalArg(() => TradeCounterpartyCalculationParameter.of(ImmutableMap.of(), PARAM3));
	  }

	  public virtual void of_badType()
	  {
		assertThrowsIllegalArg(() => TradeCounterpartyCalculationParameter.of(ImmutableMap.of(ID1, PARAM_OTHER), PARAM3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TradeCounterpartyCalculationParameter test = TradeCounterpartyCalculationParameter.of(ImmutableMap.of(ID1, PARAM1, ID2, PARAM2), PARAM3);
		coverImmutableBean(test);
		TradeCounterpartyCalculationParameter test2 = TradeCounterpartyCalculationParameter.of(ImmutableMap.of(ID1, PARAM1), PARAM2);
		coverBeanEquals(test, test2);
	  }

	}

}