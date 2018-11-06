/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
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
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using LegalEntityGroup = com.opengamma.strata.market.curve.LegalEntityGroup;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;

	/// <summary>
	/// Test <seealso cref="IssuerCurveZeroRateSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IssuerCurveZeroRateSensitivityTest
	public class IssuerCurveZeroRateSensitivityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double YEARFRAC = 2d;
	  private const double YEARFRAC2 = 3d;
	  private const double VALUE = 32d;
	  private static readonly Currency CURRENCY = USD;
	  private static readonly LegalEntityGroup GROUP = LegalEntityGroup.of("ISSUER1");

	  public virtual void test_of_withSensitivityCurrency()
	  {
		Currency sensiCurrency = GBP;
		IssuerCurveZeroRateSensitivity test = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, sensiCurrency, GROUP, VALUE);
		assertEquals(test.LegalEntityGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, sensiCurrency);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_of_withoutSensitivityCurrency()
	  {
		IssuerCurveZeroRateSensitivity test = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		assertEquals(test.LegalEntityGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, CURRENCY);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_of_zeroRateSensitivity()
	  {
		Currency sensiCurrency = GBP;
		ZeroRateSensitivity zeroSensi = ZeroRateSensitivity.of(CURRENCY, YEARFRAC, sensiCurrency, VALUE);
		IssuerCurveZeroRateSensitivity test = IssuerCurveZeroRateSensitivity.of(zeroSensi, GROUP);
		assertEquals(test.LegalEntityGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, sensiCurrency);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity test = @base.withCurrency(GBP);
		assertEquals(test.LegalEntityGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, GBP);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_withSensitivity()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double newValue = 53d;
		IssuerCurveZeroRateSensitivity test = @base.withSensitivity(newValue);
		assertEquals(test.LegalEntityGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, CURRENCY);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, newValue);
	  }

	  public virtual void test_compareKey()
	  {
		IssuerCurveZeroRateSensitivity a1 = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity a2 = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity b = IssuerCurveZeroRateSensitivity.of(GBP, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity c = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC2, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity d = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, LegalEntityGroup.of("ISSUER2"), VALUE);
		IborRateSensitivity other = IborRateSensitivity.of(IborIndexObservation.of(GBP_LIBOR_3M, date(2015, 8, 27), REF_DATA), 32d);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) > 0, true);
		assertEquals(b.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(d.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(other) > 0, true);
		assertEquals(other.compareKey(a1) < 0, true);
	  }

	  public virtual void test_convertedTo()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		IssuerCurveZeroRateSensitivity test1 = @base.convertedTo(USD, matrix);
		assertEquals(test1, @base);
		IssuerCurveZeroRateSensitivity test2 = @base.convertedTo(GBP, matrix);
		IssuerCurveZeroRateSensitivity expected = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, GROUP, VALUE / rate);
		assertEquals(test2, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double rate = 2.4d;
		IssuerCurveZeroRateSensitivity test = @base.multipliedBy(rate);
		IssuerCurveZeroRateSensitivity expected = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE * rate);
		assertEquals(test, expected);
	  }

	  public virtual void test_mapSensitivity()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity expected = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, 1d / VALUE);
		IssuerCurveZeroRateSensitivity test = @base.mapSensitivity(s => 1d / s);
		assertEquals(test, expected);
	  }

	  public virtual void test_normalize()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity test = @base.normalize();
		assertEquals(test, @base);
	  }

	  public virtual void test_buildInto()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  public virtual void test_cloned()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		IssuerCurveZeroRateSensitivity test = @base.cloned();
		assertEquals(test, @base);
	  }

	  public virtual void test_createZeroRateSensitivity()
	  {
		IssuerCurveZeroRateSensitivity @base = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, GROUP, VALUE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, VALUE);
		ZeroRateSensitivity test = @base.createZeroRateSensitivity();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IssuerCurveZeroRateSensitivity test1 = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		coverImmutableBean(test1);
		IssuerCurveZeroRateSensitivity test2 = IssuerCurveZeroRateSensitivity.of(GBP, YEARFRAC2, LegalEntityGroup.of("ISSUER1"), 12d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IssuerCurveZeroRateSensitivity test = IssuerCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		assertSerialization(test);
	  }

	}

}