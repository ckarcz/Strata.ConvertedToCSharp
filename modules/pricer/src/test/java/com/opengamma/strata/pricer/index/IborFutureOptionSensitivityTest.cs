/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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

	/// <summary>
	/// Tests <seealso cref="IborFutureOptionSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborFutureOptionSensitivityTest
	public class IborFutureOptionSensitivityTest
	{

	  private static readonly IborFutureOptionVolatilitiesName NAME = IborFutureOptionVolatilitiesName.of("Test");
	  private static readonly IborFutureOptionVolatilitiesName NAME2 = IborFutureOptionVolatilitiesName.of("Test2");

	  public virtual void test_of()
	  {
		IborFutureOptionSensitivity test = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		assertEquals(test.VolatilitiesName, NAME);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Expiry, 12d);
		assertEquals(test.FixingDate, date(2015, 8, 28));
		assertEquals(test.StrikePrice, 0.98);
		assertEquals(test.FuturePrice, 0.99);
		assertEquals(test.Sensitivity, 32d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		assertSame(@base.withCurrency(GBP), @base);

		IborFutureOptionSensitivity expected = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, USD, 32d);
		IborFutureOptionSensitivity test = @base.withCurrency(USD);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity expected = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 20d);
		IborFutureOptionSensitivity test = @base.withSensitivity(20d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		IborFutureOptionSensitivity a1 = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity a2 = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity b = IborFutureOptionSensitivity.of(NAME2, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity c = IborFutureOptionSensitivity.of(NAME, 13d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity d = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 9, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity e = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.99, 0.99, GBP, 32d);
		IborFutureOptionSensitivity f = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 1.00, GBP, 32d);
		IborFutureOptionSensitivity g = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, USD, 32d);
		ZeroRateSensitivity other = ZeroRateSensitivity.of(GBP, 2d, 32d);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) < 0, true);
		assertEquals(b.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(d.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(e) < 0, true);
		assertEquals(e.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(f) < 0, true);
		assertEquals(f.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(g) < 0, true);
		assertEquals(g.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		LocalDate fixingDate = date(2015, 8, 28);
		double strike = 0.98d;
		double forward = 0.99d;
		double sensi = 32d;
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, fixingDate, strike, forward, GBP, sensi);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		IborFutureOptionSensitivity test1 = (IborFutureOptionSensitivity) @base.convertedTo(USD, matrix);
		IborFutureOptionSensitivity expected = IborFutureOptionSensitivity.of(NAME, 12d, fixingDate, strike, forward, USD, sensi * rate);
		assertEquals(test1, expected);
		IborFutureOptionSensitivity test2 = (IborFutureOptionSensitivity) @base.convertedTo(GBP, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity expected = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d * 3.5d);
		IborFutureOptionSensitivity test = @base.multipliedBy(3.5d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity expected = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 1 / 32d);
		IborFutureOptionSensitivity test = @base.mapSensitivity(s => 1 / s);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity test = @base.normalize();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		IborFutureOptionSensitivity @base = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		IborFutureOptionSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborFutureOptionSensitivity test = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		coverImmutableBean(test);
		IborFutureOptionSensitivity test2 = IborFutureOptionSensitivity.of(NAME2, 13d, date(2015, 8, 29), 0.99, 0.995, USD, 33d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborFutureOptionSensitivity test = IborFutureOptionSensitivity.of(NAME, 12d, date(2015, 8, 28), 0.98, 0.99, GBP, 32d);
		assertSerialization(test);
	  }

	}

}