/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CurveName = com.opengamma.strata.market.curve.CurveName;

	/// <summary>
	/// Test <seealso cref="ParameterSize"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterSizeTest
	public class ParameterSizeTest
	{

	  private static readonly CurveName CURVE_NAME = CurveName.of("Test");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ParameterSize test = ParameterSize.of(CURVE_NAME, 3);
		assertEquals(test.Name, CURVE_NAME);
		assertEquals(test.ParameterCount, 3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ParameterSize test = ParameterSize.of(CURVE_NAME, 3);
		coverImmutableBean(test);
		ParameterSize test2 = ParameterSize.of(CurveName.of("Foo"), 4);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ParameterSize test = ParameterSize.of(CURVE_NAME, 3);
		assertSerialization(test);
	  }

	}

}