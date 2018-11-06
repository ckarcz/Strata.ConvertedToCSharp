/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="EtdContractSpecId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdContractSpecIdTest
	public class EtdContractSpecIdTest
	{

	  private const object ANOTHER_TYPE = "";

	  public virtual void test_of()
	  {
		EtdContractSpecId test = EtdContractSpecId.of(StandardId.of("A", "B"));
		assertEquals(test.StandardId, StandardId.of("A", "B"));
		assertEquals(test.ReferenceDataType, typeof(EtdContractSpec));
		assertEquals(test.ToString(), "A~B");
	  }

	  public virtual void test_parse()
	  {
		EtdContractSpecId test = EtdContractSpecId.parse("A~B");
		assertEquals(test.StandardId, StandardId.of("A", "B"));
		assertEquals(test.ToString(), "A~B");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		EtdContractSpecId a = EtdContractSpecId.of(StandardId.of("A", "B"));
		EtdContractSpecId a2 = EtdContractSpecId.of(StandardId.of("A", "B"));
		EtdContractSpecId b = EtdContractSpecId.of(StandardId.of("C", "D"));
		assertEquals(a.GetHashCode(), a2.GetHashCode());
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		EtdContractSpecId test = EtdContractSpecId.of("A", "B");
		assertSerialization(test);
	  }

	}

}