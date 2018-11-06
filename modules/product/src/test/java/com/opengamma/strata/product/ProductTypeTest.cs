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
	/// Test <seealso cref="ProductType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ProductTypeTest
	public class ProductTypeTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_constants()
	  {
		assertEquals(ProductType.SECURITY.ToString(), "Security");
		assertEquals(ProductType.SECURITY.Name, "Security");
		assertEquals(ProductType.SECURITY.Description, "Security");
		assertEquals(ProductType.FRA.ToString(), "Fra");
		assertEquals(ProductType.FRA.Name, "Fra");
		assertEquals(ProductType.FRA.Description, "FRA");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ProductType test = ProductType.of("test");
		assertEquals(test.ToString(), "test");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		ProductType a = ProductType.of("test");
		ProductType a2 = ProductType.of("test");
		ProductType b = ProductType.of("test2");
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	}

}