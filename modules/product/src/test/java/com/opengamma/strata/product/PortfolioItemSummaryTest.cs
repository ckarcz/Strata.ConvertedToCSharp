/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="PortfolioItemSummary"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PortfolioItemSummaryTest
	public class PortfolioItemSummaryTest
	{

	  private static readonly StandardId STANDARD_ID = StandardId.of("A", "B");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		assertEquals(sut().Id, STANDARD_ID);
		assertEquals(sut2().Id, null);
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
	  internal static PortfolioItemSummary sut()
	  {
		return PortfolioItemSummary.builder().id(STANDARD_ID).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.SECURITY).currencies(Currency.GBP).description("One").build();
	  }

	  internal static PortfolioItemSummary sut2()
	  {
		return PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.FRA).currencies(Currency.USD).description("Two").build();
	  }

	}

}