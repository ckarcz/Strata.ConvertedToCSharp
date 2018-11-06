/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertJodaConvert;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ColumnName"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ColumnNameTest
	public class ColumnNameTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_columnNameFromMeasure()
	  {
		ColumnName test = ColumnName.of("Test");
		assertEquals(test.Name, "Test");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		ColumnName test = ColumnName.of("Test");
		assertSerialization(test);
		assertJodaConvert(typeof(ColumnName), test);
	  }

	}

}