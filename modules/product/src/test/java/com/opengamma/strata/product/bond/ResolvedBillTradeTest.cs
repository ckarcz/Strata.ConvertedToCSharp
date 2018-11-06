/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedBillTrade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedBillTradeTest
	public class ResolvedBillTradeTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedBillTrade test = sut();
		assertEquals(test.Settlement.Present, true);
	  }

	  public virtual void test_builder_quantitySettlement()
	  {
		assertThrows(() => ResolvedBillTrade.builder().info(TradeInfo.of(date(2015, 3, 25))).product(ResolvedBillTest.sut()).quantity(123).settlement(Payment.of(Currency.USD, 120, date(2015, 3, 27))).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static ResolvedBillTrade sut()
	  {
		return BillTradeTest.sut_yield().resolve(REF_DATA);
	  }

	  internal static ResolvedBillTrade sut2()
	  {
		return BillTradeTest.sut2().resolve(REF_DATA);
	  }

	}

}