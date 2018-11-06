/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="EtdContractCode"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdContractCodeTest
	public class EtdContractCodeTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of()
	  {
		EtdContractCode test = EtdContractCode.of("test");
		assertEquals(test.ToString(), "test");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		EtdContractCode a = EtdContractCode.of("test");
		EtdContractCode a2 = EtdContractCode.of("test");
		EtdContractCode b = EtdContractCode.of("test2");
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	}

}