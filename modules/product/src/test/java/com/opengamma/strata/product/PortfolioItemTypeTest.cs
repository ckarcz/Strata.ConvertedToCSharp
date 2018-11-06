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
	/// Test <seealso cref="PortfolioItemType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PortfolioItemTypeTest
	public class PortfolioItemTypeTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_constants()
	  {
		assertEquals(PortfolioItemType.POSITION.Name, "Position");
		assertEquals(PortfolioItemType.TRADE.Name, "Trade");
		assertEquals(PortfolioItemType.OTHER.Name, "Other");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		assertEquals(PortfolioItemType.of("Position"), PortfolioItemType.POSITION);
		assertEquals(PortfolioItemType.of("position"), PortfolioItemType.POSITION);
		assertEquals(PortfolioItemType.of("POSITION"), PortfolioItemType.POSITION);
	  }

	}

}