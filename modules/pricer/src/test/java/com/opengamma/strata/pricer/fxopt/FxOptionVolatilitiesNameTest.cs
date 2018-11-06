/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="FxOptionVolatilitiesName"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionVolatilitiesNameTest
	public class FxOptionVolatilitiesNameTest
	{

	  public virtual void test_of()
	  {
		FxOptionVolatilitiesName test = FxOptionVolatilitiesName.of("Foo");
		assertEquals(test.Name, "Foo");
		assertEquals(test.MarketDataType, typeof(FxOptionVolatilities));
		assertEquals(test.ToString(), "Foo");
		assertEquals(test.CompareTo(FxOptionVolatilitiesName.of("Goo")) < 0, true);
	  }

	}

}