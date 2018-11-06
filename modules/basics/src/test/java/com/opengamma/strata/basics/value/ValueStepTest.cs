/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ValueStep"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValueStepTest
	public class ValueStepTest
	{

	  private static ValueAdjustment DELTA_MINUS_2000 = ValueAdjustment.ofDeltaAmount(-2000);
	  private static ValueAdjustment ABSOLUTE_100 = ValueAdjustment.ofReplace(100);

	  public virtual void test_of_intAdjustment()
	  {
		ValueStep test = ValueStep.of(2, DELTA_MINUS_2000);
		assertEquals(test.Date, null);
		assertEquals(test.PeriodIndex, int?.of(2));
		assertEquals(test.Value, DELTA_MINUS_2000);
	  }

	  public virtual void test_of_dateAdjustment()
	  {
		ValueStep test = ValueStep.of(date(2014, 6, 30), DELTA_MINUS_2000);
		assertEquals(test.Date, date(2014, 6, 30));
		assertEquals(test.PeriodIndex, int?.empty());
		assertEquals(test.Value, DELTA_MINUS_2000);
	  }

	  public virtual void test_builder_invalid()
	  {
		assertThrowsIllegalArg(() => ValueStep.builder().value(DELTA_MINUS_2000).build());
		assertThrowsIllegalArg(() => ValueStep.builder().date(date(2014, 6, 30)).periodIndex(1).value(DELTA_MINUS_2000).build());
		assertThrowsIllegalArg(() => ValueStep.builder().periodIndex(0).value(DELTA_MINUS_2000).build());
		assertThrowsIllegalArg(() => ValueStep.builder().periodIndex(-1).value(DELTA_MINUS_2000).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void Equals()
	  {
		ValueStep a1 = ValueStep.of(2, DELTA_MINUS_2000);
		ValueStep a2 = ValueStep.of(2, DELTA_MINUS_2000);
		ValueStep b = ValueStep.of(1, DELTA_MINUS_2000);
		ValueStep c = ValueStep.of(2, ABSOLUTE_100);
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(c), false);

		ValueStep d1 = ValueStep.of(date(2014, 6, 30), DELTA_MINUS_2000);
		ValueStep d2 = ValueStep.of(date(2014, 6, 30), DELTA_MINUS_2000);
		ValueStep e = ValueStep.of(date(2014, 7, 30), DELTA_MINUS_2000);
		ValueStep f = ValueStep.of(date(2014, 7, 30), ABSOLUTE_100);
		assertEquals(d1.Equals(d1), true);
		assertEquals(d1.Equals(d2), true);
		assertEquals(d1.Equals(e), false);
		assertEquals(d1.Equals(f), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(ValueStep.of(2, DELTA_MINUS_2000));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ValueStep.of(2, DELTA_MINUS_2000));
	  }

	}

}