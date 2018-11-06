/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Test <seealso cref="FxForwardSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxForwardSensitivityTest
	public class FxForwardSensitivityTest
	{

	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, GBP);
	  private static readonly LocalDate REFERENCE_DATE = LocalDate.of(2015, 11, 23);
	  private const double SENSITIVITY = 1.34d;

	  public virtual void test_of_withoutCurrency()
	  {
		FxForwardSensitivity test = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		assertEquals(test.Currency, EUR);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.ReferenceCounterCurrency, EUR);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.ReferenceDate, REFERENCE_DATE);
		assertEquals(test.Sensitivity, SENSITIVITY);
	  }

	  public virtual void test_of_withCurrency()
	  {
		FxForwardSensitivity test = FxForwardSensitivity.of(CURRENCY_PAIR, EUR, REFERENCE_DATE, USD, SENSITIVITY);
		assertEquals(test.Currency, USD);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.ReferenceCounterCurrency, GBP);
		assertEquals(test.ReferenceCurrency, EUR);
		assertEquals(test.ReferenceDate, REFERENCE_DATE);
		assertEquals(test.Sensitivity, SENSITIVITY);
	  }

	  public virtual void test_of_wrongRefCurrency()
	  {
		assertThrowsIllegalArg(() => FxForwardSensitivity.of(CURRENCY_PAIR, USD, REFERENCE_DATE, SENSITIVITY));
		assertThrowsIllegalArg(() => FxForwardSensitivity.of(CURRENCY_PAIR, USD, REFERENCE_DATE, USD, SENSITIVITY));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency_same()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.withCurrency(EUR);
		assertEquals(test, @base);
	  }

	  public virtual void test_withCurrency_other()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.withCurrency(USD);
		assertEquals(test.Currency, USD);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.ReferenceCounterCurrency, EUR);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.ReferenceDate, REFERENCE_DATE);
		assertEquals(test.Sensitivity, SENSITIVITY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.withSensitivity(13.5d);
		assertEquals(test.Currency, EUR);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.ReferenceCounterCurrency, EUR);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.ReferenceDate, REFERENCE_DATE);
		assertEquals(test.Sensitivity, 13.5d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		FxForwardSensitivity a1 = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, EUR, SENSITIVITY);
		FxForwardSensitivity a2 = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, EUR, SENSITIVITY);
		FxForwardSensitivity b = FxForwardSensitivity.of(CurrencyPair.of(GBP, USD), GBP, REFERENCE_DATE, EUR, SENSITIVITY);
		FxForwardSensitivity c = FxForwardSensitivity.of(CURRENCY_PAIR, EUR, REFERENCE_DATE, GBP, SENSITIVITY);
		FxForwardSensitivity d = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, JPY, SENSITIVITY);
		FxForwardSensitivity e = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, date(2015, 9, 27), SENSITIVITY);
		ZeroRateSensitivity other = ZeroRateSensitivity.of(GBP, 2d, SENSITIVITY);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) < 0, true);
		assertEquals(b.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(d.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(e) > 0, true);
		assertEquals(e.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		double rate = 1.4d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(EUR, USD), rate);
		FxForwardSensitivity test1 = (FxForwardSensitivity) @base.convertedTo(USD, matrix);
		FxForwardSensitivity expected = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, USD, SENSITIVITY * rate);
		assertEquals(test1, expected);
		FxForwardSensitivity test2 = (FxForwardSensitivity) @base.convertedTo(EUR, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.multipliedBy(2.4d);
		FxForwardSensitivity expected = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY * 2.4d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.mapSensitivity(s => 1d / s);
		FxForwardSensitivity expected = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, 1d / SENSITIVITY);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.normalize();
		assertEquals(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		FxForwardSensitivity base1 = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity base2 = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, 1.56d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(base1).add(base2);
		PointSensitivityBuilder test = base1.combinedWith(base2);
		assertEquals(test, expected);
	  }

	  public virtual void test_combinedWith_mutable()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(@base);
		PointSensitivityBuilder test = @base.combinedWith(new MutablePointSensitivities());
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		FxForwardSensitivity @base = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		FxForwardSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxForwardSensitivity test1 = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		coverImmutableBean(test1);
		FxForwardSensitivity test2 = FxForwardSensitivity.of(CurrencyPair.of(USD, JPY), JPY, date(2015, 9, 27), 4.25d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxForwardSensitivity test = FxForwardSensitivity.of(CURRENCY_PAIR, GBP, REFERENCE_DATE, SENSITIVITY);
		assertSerialization(test);
	  }

	}

}