/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="LegalEntityId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LegalEntityIdTest
	public class LegalEntityIdTest
	{

	  private static readonly StandardId STANDARD_ID = StandardId.of("A", "1");
	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_of_strings()
	  {
		LegalEntityId test = LegalEntityId.of("A", "1");
		assertEquals(test.StandardId, STANDARD_ID);
		assertEquals(test.ReferenceDataType, typeof(LegalEntity));
		assertEquals(test.ToString(), STANDARD_ID.ToString());
	  }

	  public virtual void test_of_standardId()
	  {
		LegalEntityId test = LegalEntityId.of(STANDARD_ID);
		assertEquals(test.StandardId, STANDARD_ID);
		assertEquals(test.ReferenceDataType, typeof(LegalEntity));
		assertEquals(test.ToString(), STANDARD_ID.ToString());
	  }

	  public virtual void test_parse()
	  {
		LegalEntityId test = LegalEntityId.parse(STANDARD_ID.ToString());
		assertEquals(test.StandardId, STANDARD_ID);
		assertEquals(test.ReferenceDataType, typeof(LegalEntity));
		assertEquals(test.ToString(), STANDARD_ID.ToString());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		LegalEntityId a = LegalEntityId.of("A", "1");
		LegalEntityId a2 = LegalEntityId.of("A", "1");
		LegalEntityId b = LegalEntityId.of("B", "1");
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

	}

}