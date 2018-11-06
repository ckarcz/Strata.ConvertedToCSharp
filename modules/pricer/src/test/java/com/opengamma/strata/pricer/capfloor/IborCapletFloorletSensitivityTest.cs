/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
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
	/// Test <seealso cref="IborCapletFloorletSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapletFloorletSensitivityTest
	public class IborCapletFloorletSensitivityTest
	{

	  private const double EXPIRY = 1d;
	  private const double FORWARD = 0.015d;
	  private const double STRIKE = 0.001d;
	  private const double SENSITIVITY = 0.54d;
	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("Test");
	  private static readonly IborCapletFloorletVolatilitiesName NAME2 = IborCapletFloorletVolatilitiesName.of("Test2");

	  public virtual void test_of()
	  {
		IborCapletFloorletSensitivity test = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		assertEquals(test.VolatilitiesName, NAME);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Expiry, EXPIRY);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.Forward, FORWARD);
		assertEquals(test.Sensitivity, SENSITIVITY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity expected = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, USD, SENSITIVITY);
		IborCapletFloorletSensitivity test = @base.withCurrency(USD);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		double sensi = 23.5;
		IborCapletFloorletSensitivity expected = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, sensi);
		IborCapletFloorletSensitivity test = @base.withSensitivity(sensi);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		IborCapletFloorletSensitivity a1 = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity a2 = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity b = IborCapletFloorletSensitivity.of(NAME2, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity c = IborCapletFloorletSensitivity.of(NAME, EXPIRY + 1, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity d = IborCapletFloorletSensitivity.of(NAME, EXPIRY, 0.009, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity e = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, 0.005, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity f = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, USD, SENSITIVITY);
		ZeroRateSensitivity other = ZeroRateSensitivity.of(GBP, 2d, 32d);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) < 0, true);
		assertEquals(b.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(d.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(e) > 0, true);
		assertEquals(e.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(f) < 0, true);
		assertEquals(f.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		IborCapletFloorletSensitivity test1 = @base.convertedTo(USD, matrix);
		IborCapletFloorletSensitivity expected = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, USD, SENSITIVITY * rate);
		assertEquals(test1, expected);
		IborCapletFloorletSensitivity test2 = @base.convertedTo(GBP, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		double factor = 3.5d;
		IborCapletFloorletSensitivity expected = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY * factor);
		IborCapletFloorletSensitivity test = @base.multipliedBy(3.5d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity expected = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, 1d / SENSITIVITY);
		IborCapletFloorletSensitivity test = @base.mapSensitivity(s => 1 / s);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity test = @base.normalize();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		IborCapletFloorletSensitivity @base = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		IborCapletFloorletSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborCapletFloorletSensitivity test1 = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		coverImmutableBean(test1);
		IborCapletFloorletSensitivity test2 = IborCapletFloorletSensitivity.of(NAME2, EXPIRY + 2d, 0.98, 0.99, USD, 32d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborCapletFloorletSensitivity test = IborCapletFloorletSensitivity.of(NAME, EXPIRY, STRIKE, FORWARD, GBP, SENSITIVITY);
		assertSerialization(test);
	  }

	}

}