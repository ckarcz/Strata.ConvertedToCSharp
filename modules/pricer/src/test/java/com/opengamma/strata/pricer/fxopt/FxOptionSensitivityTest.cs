/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
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
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;

	/// <summary>
	/// Test <seealso cref="FxOptionSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxOptionSensitivityTest
	public class FxOptionSensitivityTest
	{

	  private const double EXPIRY = 2d;
	  private static readonly CurrencyPair PAIR = CurrencyPair.of(EUR, GBP);
	  private const double FORWARD = 0.8d;
	  private const double STRIKE = 0.95d;
	  private const double SENSI_VALUE = 1.24d;
	  private static readonly FxOptionVolatilitiesName NAME = FxOptionVolatilitiesName.of("Test");
	  private static readonly FxOptionVolatilitiesName NAME2 = FxOptionVolatilitiesName.of("Test2");

	  public virtual void test_of()
	  {
		FxOptionSensitivity test = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Expiry, EXPIRY);
		assertEquals(test.Forward, FORWARD);
		assertEquals(test.CurrencyPair, PAIR);
		assertEquals(test.Sensitivity, SENSI_VALUE);
		assertEquals(test.Strike, STRIKE);
	  }

	  public virtual void test_withCurrency()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity test1 = @base.withCurrency(EUR);
		assertEquals(test1.Currency, EUR);
		assertEquals(test1.Expiry, EXPIRY);
		assertEquals(test1.Forward, FORWARD);
		assertEquals(test1.CurrencyPair, PAIR);
		assertEquals(test1.Sensitivity, SENSI_VALUE);
		assertEquals(test1.Strike, STRIKE);
		FxOptionSensitivity test2 = @base.withCurrency(GBP);
		assertEquals(test2, @base);
	  }

	  public virtual void test_withSensitivity()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		double newSensi = 22.5;
		FxOptionSensitivity test = @base.withSensitivity(newSensi);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Expiry, EXPIRY);
		assertEquals(test.Forward, FORWARD);
		assertEquals(test.CurrencyPair, PAIR);
		assertEquals(test.Sensitivity, newSensi);
		assertEquals(test.Strike, STRIKE);
	  }

	  public virtual void test_compareExcludingSensitivity()
	  {
		FxOptionSensitivity a1 = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity a2 = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity b = FxOptionSensitivity.of(NAME, CurrencyPair.of(EUR, USD), EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity c = FxOptionSensitivity.of(NAME, PAIR, EXPIRY + 1, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity d = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, 0.96, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity e = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, 0.81, GBP, SENSI_VALUE);
		FxOptionSensitivity f = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, EUR, SENSI_VALUE);
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
		assertEquals(a1.compareKey(f) > 0, true);
		assertEquals(f.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  public virtual void test_multipliedBy()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		double factor = 5.2d;
		FxOptionSensitivity expected = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE * factor);
		FxOptionSensitivity test = @base.multipliedBy(factor);
		assertEquals(test, expected);
	  }

	  public virtual void test_mapSensitivity()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity expected = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, 1.0 / SENSI_VALUE);
		FxOptionSensitivity test = @base.mapSensitivity(s => 1 / s);
		assertEquals(test, expected);
	  }

	  public virtual void test_normalize()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity test = @base.normalize();
		assertSame(test, @base);
	  }

	  public virtual void test_buildInto()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  public virtual void test_build()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  public virtual void test_cloned()
	  {
		FxOptionSensitivity @base = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		FxOptionSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  public virtual void coverage()
	  {
		FxOptionSensitivity test1 = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		coverImmutableBean(test1);
		FxOptionSensitivity test2 = FxOptionSensitivity.of(NAME2, CurrencyPair.of(EUR, USD), EXPIRY, 0.8, 0.9, EUR, 1.1);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxOptionSensitivity test = FxOptionSensitivity.of(NAME, PAIR, EXPIRY, STRIKE, FORWARD, GBP, SENSI_VALUE);
		assertSerialization(test);
	  }

	}

}