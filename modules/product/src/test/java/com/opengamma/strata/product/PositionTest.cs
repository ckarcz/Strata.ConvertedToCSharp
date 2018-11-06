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

	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="Position"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PositionTest
	public class PositionTest
	{

	  private static readonly StandardId STANDARD_ID = StandardId.of("A", "B");

	  //-------------------------------------------------------------------------
	  public virtual void test_methods()
	  {
		Position test = sut();
		assertEquals(test.Id, null);
		assertEquals(test.Info, PositionInfo.empty());
		assertEquals(test.Quantity, 123d);
		assertEquals(test.SecurityId, SecurityId.of(STANDARD_ID));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		Position trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.SECURITY).description("B x 123").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  internal static Position sut()
	  {
		return new PositionAnonymousInnerClass();
	  }

	  private class PositionAnonymousInnerClass : Position
	  {
		  public PositionAnonymousInnerClass()
		  {
		  }


		  public SecurityId SecurityId
		  {
			  get
			  {
				return SecurityId.of(STANDARD_ID);
			  }
		  }

		  public double Quantity
		  {
			  get
			  {
				return 123d;
			  }
		  }

		  public PositionInfo Info
		  {
			  get
			  {
				return PositionInfo.empty();
			  }
		  }

		  public Position withInfo(PositionInfo info)
		  {
			return this;
		  }

		  public Position withQuantity(double quantity)
		  {
			return this;
		  }
	  }

	}

}