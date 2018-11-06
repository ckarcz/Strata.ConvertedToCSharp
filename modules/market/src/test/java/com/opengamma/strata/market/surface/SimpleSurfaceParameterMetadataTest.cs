/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
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
	/// Test <seealso cref="SimpleSurfaceParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SimpleSurfaceParameterMetadataTest
	public class SimpleSurfaceParameterMetadataTest
	{

	  public virtual void test_of()
	  {
		SimpleSurfaceParameterMetadata test = SimpleSurfaceParameterMetadata.of(ValueType.YEAR_FRACTION, 1d, ValueType.STRIKE, 3d);
		assertEquals(test.XValueType, ValueType.YEAR_FRACTION);
		assertEquals(test.XValue, 1d);
		assertEquals(test.YValueType, ValueType.STRIKE);
		assertEquals(test.YValue, 3d);
		assertEquals(test.Label, "YearFraction=1.0, Strike=3.0");
		assertEquals(test.Identifier, "YearFraction=1.0, Strike=3.0");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SimpleSurfaceParameterMetadata test = SimpleSurfaceParameterMetadata.of(ValueType.YEAR_FRACTION, 1d, ValueType.STRIKE, 3d);
		coverImmutableBean(test);
		SimpleSurfaceParameterMetadata test2 = SimpleSurfaceParameterMetadata.of(ValueType.ZERO_RATE, 2d, ValueType.SIMPLE_MONEYNESS, 4d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SimpleSurfaceParameterMetadata test = SimpleSurfaceParameterMetadata.of(ValueType.YEAR_FRACTION, 1d, ValueType.STRIKE, 3d);
		assertSerialization(test);
	  }

	}

}