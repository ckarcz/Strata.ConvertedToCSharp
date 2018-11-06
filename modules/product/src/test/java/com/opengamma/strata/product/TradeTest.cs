/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="Trade"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeTest
	public class TradeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_methods()
	  {
		Trade test = sut();
		assertEquals(test.Id, null);
		assertEquals(test.Info, TradeInfo.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		Trade trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.TRADE).productType(ProductType.OTHER).description("Unknown: MockTrade").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  internal static Trade sut()
	  {
		return new MockTrade();
	  }

	  private sealed class MockTrade : Trade
	  {
		public TradeInfo Info
		{
			get
			{
			  return TradeInfo.empty();
			}
		}

		public Trade withInfo(TradeInfo info)
		{
		  return this;
		}
	  }

	}

}