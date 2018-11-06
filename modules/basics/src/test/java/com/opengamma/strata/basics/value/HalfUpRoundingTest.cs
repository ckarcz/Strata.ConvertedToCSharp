/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="HalfUpRounding"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HalfUpRoundingTest
	public class HalfUpRoundingTest
	{

	  public virtual void test_of_Currency()
	  {
		Rounding test = Rounding.of(Currency.USD);
		assertEquals(test.round(63.455d), 63.46d, 0d);
		assertEquals(test.round(63.454d), 63.45d, 0d);
	  }

	  public virtual void test_ofDecimalPlaces()
	  {
		HalfUpRounding test = HalfUpRounding.ofDecimalPlaces(4);
		assertEquals(test.DecimalPlaces, 4);
		assertEquals(test.Fraction, 0);
		assertEquals(test.ToString(), "Round to 4dp");
		assertEquals(Rounding.ofDecimalPlaces(4), test);
	  }

	  public virtual void test_ofDecimalPlaces_big()
	  {
		HalfUpRounding test = HalfUpRounding.ofDecimalPlaces(40);
		assertEquals(test.DecimalPlaces, 40);
		assertEquals(test.Fraction, 0);
		assertEquals(test.ToString(), "Round to 40dp");
		assertEquals(Rounding.ofDecimalPlaces(40), test);
	  }

	  public virtual void test_ofDecimalPlaces_invalid()
	  {
		assertThrowsIllegalArg(() => HalfUpRounding.ofDecimalPlaces(-1));
		assertThrowsIllegalArg(() => HalfUpRounding.ofDecimalPlaces(257));
	  }

	  public virtual void test_ofFractionalDecimalPlaces()
	  {
		HalfUpRounding test = HalfUpRounding.ofFractionalDecimalPlaces(4, 32);
		assertEquals(test.DecimalPlaces, 4);
		assertEquals(test.Fraction, 32);
		assertEquals(test.ToString(), "Round to 1/32 of 4dp");
		assertEquals(Rounding.ofFractionalDecimalPlaces(4, 32), test);
	  }

	  public virtual void test_ofFractionalDecimalPlaces_invalid()
	  {
		assertThrowsIllegalArg(() => HalfUpRounding.ofFractionalDecimalPlaces(-1, 0));
		assertThrowsIllegalArg(() => HalfUpRounding.ofFractionalDecimalPlaces(257, 0));
		assertThrowsIllegalArg(() => HalfUpRounding.ofFractionalDecimalPlaces(0, -1));
		assertThrowsIllegalArg(() => HalfUpRounding.ofFractionalDecimalPlaces(0, 257));
	  }

	  public virtual void test_builder()
	  {
		HalfUpRounding test = HalfUpRounding.meta().builder().set(HalfUpRounding.meta().decimalPlaces(), 4).set(HalfUpRounding.meta().fraction(), 1).build();
		assertEquals(test.DecimalPlaces, 4);
		assertEquals(test.Fraction, 0);
		assertEquals(test.ToString(), "Round to 4dp");
	  }

	  public virtual void test_builder_invalid()
	  {
		assertThrowsIllegalArg(() => HalfUpRounding.meta().builder().set(HalfUpRounding.meta().decimalPlaces(), -1).build());
		assertThrowsIllegalArg(() => HalfUpRounding.meta().builder().set(HalfUpRounding.meta().decimalPlaces(), 257).build());
		assertThrowsIllegalArg(() => HalfUpRounding.meta().builder().set(HalfUpRounding.meta().decimalPlaces(), 4).set(HalfUpRounding.meta().fraction(), -1).build());
		assertThrowsIllegalArg(() => HalfUpRounding.meta().builder().set(HalfUpRounding.meta().decimalPlaces(), 4).set(HalfUpRounding.meta().fraction(), 257).build());
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "round") public static Object[][] data_round()
	  public static object[][] data_round()
	  {
		return new object[][]
		{
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3449, 12.34},
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3450, 12.35},
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3451, 12.35},
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3500, 12.35},
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3549, 12.35},
			new object[] {HalfUpRounding.ofDecimalPlaces(2), 12.3550, 12.36},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3424, 12.340},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3425, 12.345},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3426, 12.345},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3449, 12.345},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3450, 12.345},
			new object[] {HalfUpRounding.ofFractionalDecimalPlaces(2, 2), 12.3451, 12.345}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "round") public void round_double_NONE(HalfUpRounding rounding, double input, double expected)
	  public virtual void round_double_NONE(HalfUpRounding rounding, double input, double expected)
	  {
		assertEquals(rounding.round(input), expected);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "round") public void round_BigDecimal_NONE(HalfUpRounding rounding, double input, double expected)
	  public virtual void round_BigDecimal_NONE(HalfUpRounding rounding, double input, double expected)
	  {
		assertEquals(rounding.round(decimal.valueOf(input)), decimal.valueOf(expected));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		HalfUpRounding test = HalfUpRounding.ofDecimalPlaces(4);
		coverImmutableBean(test);
		HalfUpRounding test2 = HalfUpRounding.ofFractionalDecimalPlaces(4, 32);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		HalfUpRounding test = HalfUpRounding.ofDecimalPlaces(4);
		assertSerialization(test);
	  }

	}

}