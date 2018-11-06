/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
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

	/// <summary>
	/// Test <seealso cref="CurveParameterSize"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveParameterSizeTest
	public class CurveParameterSizeTest
	{

	  private static readonly CurveName CURVE_NAME = CurveName.of("Test");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		CurveParameterSize test = CurveParameterSize.of(CURVE_NAME, 3);
		assertEquals(test.Name, CURVE_NAME);
		assertEquals(test.ParameterCount, 3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurveParameterSize test = CurveParameterSize.of(CURVE_NAME, 3);
		coverImmutableBean(test);
		CurveParameterSize test2 = CurveParameterSize.of(CurveName.of("Foo"), 4);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CurveParameterSize test = CurveParameterSize.of(CURVE_NAME, 3);
		assertSerialization(test);
	  }

	}

}