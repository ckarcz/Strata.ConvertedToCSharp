/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="SimpleCurveParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleCurveParameterMetadataTest
	public class SimpleCurveParameterMetadataTest
	{

	  public virtual void test_of()
	  {
		SimpleCurveParameterMetadata test = SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, 1d);
		assertEquals(test.XValueType, ValueType.YEAR_FRACTION);
		assertEquals(test.XValue, 1d);
		assertEquals(test.Label, "YearFraction=1.0");
		assertEquals(test.Identifier, "YearFraction=1.0");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleCurveParameterMetadata test = SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, 1d);
		coverImmutableBean(test);
		SimpleCurveParameterMetadata test2 = SimpleCurveParameterMetadata.of(ValueType.ZERO_RATE, 2d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SimpleCurveParameterMetadata test = SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, 1d);
		assertSerialization(test);
	  }

	}

}