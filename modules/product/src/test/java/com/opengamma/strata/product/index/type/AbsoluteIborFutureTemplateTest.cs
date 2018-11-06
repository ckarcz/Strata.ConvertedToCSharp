/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DateSequences.QUARTERLY_IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Tests <seealso cref="AbsoluteIborFutureTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AbsoluteIborFutureTemplateTest
	public class AbsoluteIborFutureTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborFutureConvention CONVENTION = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
	  private static readonly IborFutureConvention CONVENTION2 = ImmutableIborFutureConvention.of(USD_LIBOR_6M, QUARTERLY_IMM);
	  private static readonly YearMonth YEAR_MONTH = YearMonth.of(2016, 6);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		AbsoluteIborFutureTemplate test = AbsoluteIborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		assertEquals(test.YearMonth, YEAR_MONTH);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.Index, CONVENTION.Index);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		LocalDate date = LocalDate.of(2015, 10, 20);
		double quantity = 3;
		double price = 0.99;
		double notional = 100.0;
		SecurityId secId = SecurityId.of("OG-Future", "GBP-LIBOR-3M-Jun16");
		IborFutureTrade trade = @base.createTrade(date, secId, quantity, notional, price, REF_DATA);
		IborFutureTrade expected = CONVENTION.createTrade(date, secId, YEAR_MONTH, quantity, notional, price, REF_DATA);
		assertEquals(trade, expected);
	  }

	  public virtual void test_calculateReferenceDateFromTradeDate()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		LocalDate date = LocalDate.of(2015, 10, 20);
		LocalDate expected = LocalDate.of(2016, 6, 15);
		assertEquals(@base.calculateReferenceDateFromTradeDate(date, REF_DATA), expected);
	  }

	  public virtual void test_approximateMaturity()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		assertEquals(@base.approximateMaturity(LocalDate.of(2015, 10, 20)), 8d / 12d, 0.1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		AbsoluteIborFutureTemplate test = AbsoluteIborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		coverImmutableBean(test);
		AbsoluteIborFutureTemplate test2 = AbsoluteIborFutureTemplate.of(YEAR_MONTH.plusMonths(1), CONVENTION2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		AbsoluteIborFutureTemplate test = AbsoluteIborFutureTemplate.of(YEAR_MONTH, CONVENTION);
		assertSerialization(test);
	  }

	}

}