/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DateSequences.MONTHLY_IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DateSequences.QUARTERLY_IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;

	/// <summary>
	/// Tests <seealso cref="IborFutureConvention"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureConventionTest
	public class IborFutureConventionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double NOTIONAL_1M = 1_000_000d;
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(BusinessDayConventions.FOLLOWING, USD_LIBOR_3M.EffectiveDateOffset.Calendar);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ImmutableIborFutureConvention test = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
		assertEquals(test.Name, "USD-LIBOR-3M-Quarterly-IMM");
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.DateSequence, QUARTERLY_IMM);
		assertEquals(test.BusinessDayAdjustment, BDA);
	  }

	  public virtual void test_builder()
	  {
		ImmutableIborFutureConvention test = ImmutableIborFutureConvention.builder().name("USD-IMM").index(USD_LIBOR_3M).dateSequence(QUARTERLY_IMM).build();
		assertEquals(test.Name, "USD-IMM");
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.DateSequence, QUARTERLY_IMM);
		assertEquals(test.BusinessDayAdjustment, BDA);
	  }

	  public virtual void test_builder_incomplete()
	  {
		assertThrowsIllegalArg(() => ImmutableIborFutureConvention.builder().index(USD_LIBOR_3M).build());
		assertThrowsIllegalArg(() => ImmutableIborFutureConvention.builder().dateSequence(QUARTERLY_IMM).build());
	  }

	  public virtual void test_toTrade()
	  {
		LocalDate date = LocalDate.of(2015, 10, 20);
		Period start = Period.ofMonths(2);
		int number = 2; // Future should be 20 Dec 15 + 2 IMM = effective 15-Jun-2016, fixing 13-Jun-2016
		IborFutureConvention convention = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
		double quantity = 3;
		double price = 0.99;
		SecurityId secId = SecurityId.of("OG-Future", "GBP-LIBOR-3M-Jun16");
		IborFutureTrade trade = convention.createTrade(date, secId, start, number, quantity, NOTIONAL_1M, price, REF_DATA);
		assertEquals(trade.Product.FixingDate, LocalDate.of(2016, 6, 13));
		assertEquals(trade.Product.Index, USD_LIBOR_3M);
		assertEquals(trade.Product.Notional, NOTIONAL_1M);
		assertEquals(trade.Product.AccrualFactor, 0.25);
		assertEquals(trade.Quantity, quantity);
		assertEquals(trade.Price, price);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "name") public static Object[][] data_name()
	  public static object[][] data_name()
	  {
		return new object[][]
		{
			new object[] {IborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM, "USD-LIBOR-3M-Quarterly-IMM"},
			new object[] {IborFutureConventions.USD_LIBOR_3M_MONTHLY_IMM, "USD-LIBOR-3M-Monthly-IMM"}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_name(IborFutureConvention convention, String name)
	  public virtual void test_name(IborFutureConvention convention, string name)
	  {
		assertEquals(convention.Name, name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_toString(IborFutureConvention convention, String name)
	  public virtual void test_toString(IborFutureConvention convention, string name)
	  {
		assertEquals(convention.ToString(), name);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_of_lookup(IborFutureConvention convention, String name)
	  public virtual void test_of_lookup(IborFutureConvention convention, string name)
	  {
		assertEquals(IborFutureConvention.of(name), convention);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "name") public void test_extendedEnum(IborFutureConvention convention, String name)
	  public virtual void test_extendedEnum(IborFutureConvention convention, string name)
	  {
		IborFutureConvention.of(name); // ensures map is populated
		ImmutableMap<string, IborFutureConvention> map = IborFutureConvention.extendedEnum().lookupAll();
		assertEquals(map.get(name), convention);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => IborFutureConvention.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsIllegalArg(() => IborFutureConvention.of((string) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableIborFutureConvention test = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
		coverImmutableBean(test);
		ImmutableIborFutureConvention test2 = ImmutableIborFutureConvention.builder().index(USD_LIBOR_3M).dateSequence(MONTHLY_IMM).businessDayAdjustment(BDA).build();
		coverBeanEquals(test, test2);

		coverPrivateConstructor(typeof(IborFutureConventions));
		coverPrivateConstructor(typeof(StandardIborFutureConventions));
	  }

	  public virtual void test_serialization()
	  {
		IborFutureConvention test = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);
		assertSerialization(test);
	  }

	}

}