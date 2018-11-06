/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Tests <seealso cref="RelativeIborFutureTemplate"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RelativeIborFutureTemplateTest
	public class RelativeIborFutureTemplateTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborFutureConvention CONVENTION = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
	  private static readonly IborFutureConvention CONVENTION2 = ImmutableIborFutureConvention.of(USD_LIBOR_6M, QUARTERLY_IMM);
	  private static readonly Period MIN_PERIOD = Period.ofMonths(2);
	  private const int NUMBER = 2;

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		RelativeIborFutureTemplate test = RelativeIborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		assertEquals(test.MinimumPeriod, MIN_PERIOD);
		assertEquals(test.SequenceNumber, NUMBER);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.Index, CONVENTION.Index);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createTrade()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		LocalDate date = LocalDate.of(2015, 10, 20);
		double quantity = 3;
		double price = 0.99;
		double notional = 100.0;
		SecurityId secId = SecurityId.of("OG-Future", "GBP-LIBOR-3M-Jun16");
		IborFutureTrade trade = @base.createTrade(date, secId, quantity, notional, price, REF_DATA);
		IborFutureTrade expected = CONVENTION.createTrade(date, secId, MIN_PERIOD, NUMBER, quantity, notional, price, REF_DATA);
		assertEquals(trade, expected);
	  }

	  public virtual void test_calculateReferenceDateFromTradeDate()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		LocalDate date = LocalDate.of(2015, 10, 20); // 2nd Quarterly IMM at least 2 months later from this date
		LocalDate expected = LocalDate.of(2016, 6, 15); // 1st is March 2016, 2nd is Jun 2016
		assertEquals(@base.calculateReferenceDateFromTradeDate(date, REF_DATA), expected);
	  }

	  public virtual void test_approximateMaturity()
	  {
		IborFutureTemplate @base = IborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		assertEquals(@base.approximateMaturity(LocalDate.of(2015, 10, 20)), 0.5d, 0.1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RelativeIborFutureTemplate test = RelativeIborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		coverImmutableBean(test);
		RelativeIborFutureTemplate test2 = RelativeIborFutureTemplate.of(Period.ofMonths(3), NUMBER + 1, CONVENTION2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RelativeIborFutureTemplate test = RelativeIborFutureTemplate.of(MIN_PERIOD, NUMBER, CONVENTION);
		assertSerialization(test);
	  }

	}

}