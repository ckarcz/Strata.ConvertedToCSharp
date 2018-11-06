/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="TradedPrice"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradedPriceTest
	public class TradedPriceTest
	{

	  private const double PRICE = 123d;
	  private static readonly LocalDate DATE = LocalDate.of(2018, 6, 1);

	  //-------------------------------------------------------------------------
	  public virtual void test_methods()
	  {
		TradedPrice test = sut();
		assertEquals(test.TradeDate, DATE);
		assertEquals(test.Price, PRICE);
	  }

	  public virtual void coverage()
	  {
		TradedPrice test = sut();
		coverImmutableBean(test);
		TradedPrice test2 = TradedPrice.of(DATE.plusDays(1), PRICE + 1d);
		coverBeanEquals(test, test2);
	  }

	  //-------------------------------------------------------------------------
	  internal static TradedPrice sut()
	  {
		return TradedPrice.of(DATE, PRICE);
	  }

	}

}