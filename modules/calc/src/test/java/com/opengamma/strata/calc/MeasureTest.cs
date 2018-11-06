/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsRuntime;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using StringConvert = org.joda.convert.StringConvert;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="Measure"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MeasureTest
	public class MeasureTest
	{

	  public virtual void test_extendedEnum()
	  {
		ImmutableMap<string, Measure> map = Measure.extendedEnum().lookupAll();
		assertEquals(map.size(), 0);
	  }

	  public virtual void test_of_lookup_notFound()
	  {
		assertThrowsIllegalArg(() => Measure.of("Rubbish"));
	  }

	  public virtual void test_of_lookup_null()
	  {
		assertThrowsRuntime(() => Measure.of(null));
	  }

	  //-------------------------------------------------------------------------

	  public virtual void test_isConvertible()
	  {
		assertTrue(StringConvert.INSTANCE.isConvertible(typeof(Measure)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ImmutableMeasure test = ImmutableMeasure.of("A");
		coverImmutableBean(test);
		ImmutableMeasure test2 = ImmutableMeasure.of("B", false);
		coverBeanEquals(test, test2);

		coverPrivateConstructor(typeof(MeasureHelper));
	  }

	}

}