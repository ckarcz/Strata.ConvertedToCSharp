/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
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
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="BillPosition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BillPositionTest
	public class BillPositionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly PositionInfo POSITION_INFO1 = PositionInfo.builder().id(StandardId.of("A", "B")).build();
	  private static readonly PositionInfo POSITION_INFO2 = PositionInfo.builder().id(StandardId.of("A", "C")).build();
	  private const double QUANTITY1 = 10;
	  private const double QUANTITY2 = 30;
	  private static readonly Bill PRODUCT1 = BillTest.US_BILL;
	  private static readonly Bill PRODUCT2 = BillTest.BILL_2;

	  public virtual void test_builder_of()
	  {
		BillPosition test = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).shortQuantity(QUANTITY2).build();
		assertEquals(test.Currency, USD);
		assertEquals(test.Id, POSITION_INFO1.Id);
		assertEquals(test.Info, POSITION_INFO1);
		assertEquals(test.LongQuantity, QUANTITY1);
		assertEquals(test.ShortQuantity, QUANTITY2);
		assertEquals(test.Product, PRODUCT1);
		assertEquals(test.Quantity, QUANTITY1 - QUANTITY2);
		assertEquals(test.SecurityId, PRODUCT1.SecurityId);
		BillPosition test1 = BillPosition.ofLongShort(POSITION_INFO1, PRODUCT1, QUANTITY1, QUANTITY2);
		assertEquals(test, test1);
	  }

	  public virtual void test_summarize()
	  {
		BillPosition @base = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(POSITION_INFO1.Id.orElse(null)).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.BILL).currencies(USD).description("Bill2019-05-23 x 10").build();
		assertEquals(@base.summarize(), expected);
	  }

	  public virtual void test_withInfo()
	  {
		BillPosition @base = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		BillPosition computed1 = @base.withInfo(POSITION_INFO2);
		BillPosition expected1 = BillPosition.builder().info(POSITION_INFO2).product(PRODUCT1).longQuantity(QUANTITY1).build();
		assertEquals(computed1, expected1);
	  }

	  public virtual void test_withQuantity()
	  {
		BillPosition @base = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		double quantity = 1234d;
		BillPosition computed1 = @base.withQuantity(quantity);
		BillPosition expected1 = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(quantity).build();
		assertEquals(computed1, expected1);
		BillPosition computed2 = @base.withQuantity(-quantity);
		BillPosition expected2 = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).shortQuantity(quantity).build();
		assertEquals(computed2, expected2);
	  }

	  public virtual void test_resolve()
	  {
		BillPosition @base = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		ResolvedBillTrade computed = @base.resolve(REF_DATA);
		ResolvedBillTrade expected = ResolvedBillTrade.builder().info(POSITION_INFO1).product(PRODUCT1.resolve(REF_DATA)).quantity(QUANTITY1).build();
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BillPosition test1 = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		coverImmutableBean(test1);
		BillPosition test2 = BillPosition.builder().info(POSITION_INFO2).product(PRODUCT2).shortQuantity(QUANTITY1).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		BillPosition test = BillPosition.builder().info(POSITION_INFO1).product(PRODUCT1).longQuantity(QUANTITY1).build();
		assertSerialization(test);
	  }

	}

}