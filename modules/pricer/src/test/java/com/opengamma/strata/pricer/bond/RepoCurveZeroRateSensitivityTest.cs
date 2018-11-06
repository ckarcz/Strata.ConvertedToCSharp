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
	using RepoGroup = com.opengamma.strata.market.curve.RepoGroup;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;

	/// <summary>
	/// Test <seealso cref="RepoCurveZeroRateSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RepoCurveZeroRateSensitivityTest
	public class RepoCurveZeroRateSensitivityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double YEARFRAC = 2d;
	  private const double YEARFRAC2 = 3d;
	  private const double VALUE = 32d;
	  private static readonly Currency CURRENCY = USD;
	  private static readonly RepoGroup GROUP = RepoGroup.of("ISSUER1 BND 10Y");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_withSensitivityCurrency()
	  {
		Currency sensiCurrency = GBP;
		RepoCurveZeroRateSensitivity test = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, sensiCurrency, GROUP, VALUE);
		assertEquals(test.RepoGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, sensiCurrency);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_of_withoutSensitivityCurrency()
	  {
		RepoCurveZeroRateSensitivity test = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		assertEquals(test.RepoGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, CURRENCY);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_of_zeroRateSensitivity()
	  {
		Currency sensiCurrency = GBP;
		ZeroRateSensitivity zeroSensi = ZeroRateSensitivity.of(CURRENCY, YEARFRAC, sensiCurrency, VALUE);
		RepoCurveZeroRateSensitivity test = RepoCurveZeroRateSensitivity.of(zeroSensi, GROUP);
		assertEquals(test.RepoGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, sensiCurrency);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity test = @base.withCurrency(GBP);
		assertEquals(test.RepoGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, GBP);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, VALUE);
	  }

	  public virtual void test_withSensitivity()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double newValue = 53d;
		RepoCurveZeroRateSensitivity test = @base.withSensitivity(newValue);
		assertEquals(test.RepoGroup, GROUP);
		assertEquals(test.CurveCurrency, CURRENCY);
		assertEquals(test.Currency, CURRENCY);
		assertEquals(test.YearFraction, YEARFRAC);
		assertEquals(test.Sensitivity, newValue);
	  }

	  public virtual void test_compareKey()
	  {
		RepoCurveZeroRateSensitivity a1 = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity a2 = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity b = RepoCurveZeroRateSensitivity.of(GBP, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity c = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC2, GROUP, VALUE);
		RepoCurveZeroRateSensitivity d = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, RepoGroup.of("ISSUER1 BND 3Y"), VALUE);
		IborRateSensitivity other = IborRateSensitivity.of(IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA), 32d);
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
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		RepoCurveZeroRateSensitivity test1 = @base.convertedTo(USD, matrix);
		assertEquals(test1, @base);
		RepoCurveZeroRateSensitivity test2 = @base.convertedTo(GBP, matrix);
		RepoCurveZeroRateSensitivity expected = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, GROUP, VALUE / rate);
		assertEquals(test2, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		double rate = 2.4d;
		RepoCurveZeroRateSensitivity test = @base.multipliedBy(rate);
		RepoCurveZeroRateSensitivity expected = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE * rate);
		assertEquals(test, expected);
	  }

	  public virtual void test_mapSensitivity()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity expected = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, 1d / VALUE);
		RepoCurveZeroRateSensitivity test = @base.mapSensitivity(s => 1d / s);
		assertEquals(test, expected);
	  }

	  public virtual void test_normalize()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity test = @base.normalize();
		assertEquals(test, @base);
	  }

	  public virtual void test_buildInto()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  public virtual void test_cloned()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		RepoCurveZeroRateSensitivity test = @base.cloned();
		assertEquals(test, @base);
	  }

	  public virtual void test_createZeroRateSensitivity()
	  {
		RepoCurveZeroRateSensitivity @base = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, GROUP, VALUE);
		ZeroRateSensitivity expected = ZeroRateSensitivity.of(CURRENCY, YEARFRAC, GBP, VALUE);
		ZeroRateSensitivity test = @base.createZeroRateSensitivity();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RepoCurveZeroRateSensitivity test1 = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		coverImmutableBean(test1);
		RepoCurveZeroRateSensitivity test2 = RepoCurveZeroRateSensitivity.of(GBP, YEARFRAC2, RepoGroup.of("ISSUER2 BND 5Y"), 12d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		RepoCurveZeroRateSensitivity test = RepoCurveZeroRateSensitivity.of(CURRENCY, YEARFRAC, GROUP, VALUE);
		assertSerialization(test);
	  }

	}

}