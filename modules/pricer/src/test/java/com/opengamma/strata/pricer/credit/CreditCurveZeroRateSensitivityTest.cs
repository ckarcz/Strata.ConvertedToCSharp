/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;

	/// <summary>
	/// Test <seealso cref="CreditCurveZeroRateSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CreditCurveZeroRateSensitivityTest
	public class CreditCurveZeroRateSensitivityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private const double YEAR_FRACTION = 5d;
	  private const double VALUE = 1.5d;

	  public virtual void test_of_full()
	  {
		CreditCurveZeroRateSensitivity test = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, GBP, VALUE);
		assertEquals(test.Currency, GBP);
		assertEquals(test.CurveCurrency, USD);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Sensitivity, VALUE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
	  }

	  public virtual void test_of()
	  {
		CreditCurveZeroRateSensitivity test = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, VALUE);
		assertEquals(test.Currency, USD);
		assertEquals(test.CurveCurrency, USD);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Sensitivity, VALUE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
	  }

	  public virtual void test_of_ZeroRateSensitivity()
	  {
		ZeroRateSensitivity zeroPoint = ZeroRateSensitivity.of(USD, YEAR_FRACTION, GBP, VALUE);
		CreditCurveZeroRateSensitivity test = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, zeroPoint);
		assertEquals(test.Currency, GBP);
		assertEquals(test.CurveCurrency, USD);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.Sensitivity, VALUE);
		assertEquals(test.YearFraction, YEAR_FRACTION);
		assertEquals(test.toZeroRateSensitivity(), zeroPoint);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, VALUE);
		assertSame(@base.withCurrency(USD), @base);
		assertEquals(@base.withCurrency(GBP), CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, GBP, VALUE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, VALUE);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, 20d);
		CreditCurveZeroRateSensitivity test = @base.withSensitivity(20d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		CreditCurveZeroRateSensitivity a1 = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity a2 = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity b = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, 10d, 32d);
		CreditCurveZeroRateSensitivity c = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity d = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, USD, YEAR_FRACTION, GBP, 32d);
		IborRateSensitivity other = IborRateSensitivity.of(IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA), 32d);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) < 0, true);
		assertEquals(b.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		double sensi = 32d;
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, sensi);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		CreditCurveZeroRateSensitivity test1 = @base.convertedTo(USD, matrix);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, USD, rate * sensi);
		assertEquals(test1, expected);
		CreditCurveZeroRateSensitivity test2 = @base.convertedTo(GBP, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d * 3.5d);
		CreditCurveZeroRateSensitivity test = @base.multipliedBy(3.5d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity expected = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 1 / 32d);
		CreditCurveZeroRateSensitivity test = @base.mapSensitivity(s => 1 / s);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity test = @base.normalize();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		CreditCurveZeroRateSensitivity base1 = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity base2 = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 22d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(base1).add(base2);
		PointSensitivityBuilder test = base1.combinedWith(base2);
		assertEquals(test, expected);
	  }

	  public virtual void test_combinedWith_mutable()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(@base);
		PointSensitivityBuilder test = @base.combinedWith(new MutablePointSensitivities());
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		CreditCurveZeroRateSensitivity @base = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		CreditCurveZeroRateSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CreditCurveZeroRateSensitivity test = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		coverImmutableBean(test);
		CreditCurveZeroRateSensitivity test2 = CreditCurveZeroRateSensitivity.of(StandardId.of("OG", "AAA"), USD, YEAR_FRACTION, 16d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CreditCurveZeroRateSensitivity test = CreditCurveZeroRateSensitivity.of(LEGAL_ENTITY, GBP, YEAR_FRACTION, 32d);
		assertSerialization(test);
	  }

	}

}