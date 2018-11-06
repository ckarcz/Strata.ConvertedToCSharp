/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using RenameHandler = org.joda.convert.RenameHandler;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="AttributeType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AttributeTypeTest
	public class AttributeTypeTest
	{

	  private const object ANOTHER_TYPE = "";

	  //-------------------------------------------------------------------------
	  public virtual void test_constant_description()
	  {
		AttributeType<string> test = AttributeType.DESCRIPTION;
		assertEquals(test.ToString(), "description");
	  }

	  public virtual void test_constant_name()
	  {
		AttributeType<string> test = AttributeType.NAME;
		assertEquals(test.ToString(), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		AttributeType<string> test = AttributeType.of("test");
		assertEquals(test.ToString(), "test");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		AttributeType<string> a = AttributeType.of("test");
		AttributeType<string> a2 = AttributeType.of("test");
		AttributeType<string> b = AttributeType.of("test2");
		assertEquals(a.Equals(a), true);
		assertEquals(a.Equals(a2), true);
		assertEquals(a.Equals(b), false);
		assertEquals(a.Equals(null), false);
		assertEquals(a.Equals(ANOTHER_TYPE), false);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_jodaConvert() throws Exception
	  public virtual void test_jodaConvert()
	  {
		assertEquals(RenameHandler.INSTANCE.lookupType("com.opengamma.strata.product.PositionAttributeType"), typeof(AttributeType));
		assertEquals(RenameHandler.INSTANCE.lookupType("com.opengamma.strata.product.SecurityAttributeType"), typeof(AttributeType));
		assertEquals(RenameHandler.INSTANCE.lookupType("com.opengamma.strata.product.TradeAttributeType"), typeof(AttributeType));
	  }

	}

}