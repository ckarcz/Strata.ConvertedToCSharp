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
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.GBP_USD_WM;
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
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Test <seealso cref="FxIndexSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxIndexSensitivityTest
	public class FxIndexSensitivityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2015, 8, 27);
	  private const double SENSITIVITY_VALUE = 1.342d;
	  private static readonly FxIndexObservation GBP_USD_WM_OBS = FxIndexObservation.of(GBP_USD_WM, FIXING_DATE, REF_DATA);
	  private static readonly FxIndexObservation GBP_USD_WM_OBS2 = FxIndexObservation.of(GBP_USD_WM, date(2015, 9, 27), REF_DATA);
	  private static readonly FxIndexObservation EUR_GBP_ECB_OBS = FxIndexObservation.of(EUR_GBP_ECB, FIXING_DATE, REF_DATA);

	  public virtual void test_of()
	  {
		FxIndexSensitivity test = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		assertEquals(test.ReferenceCurrency, USD);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.Sensitivity, SENSITIVITY_VALUE);
	  }

	  public virtual void test_of_noCurrency()
	  {
		FxIndexSensitivity test = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, SENSITIVITY_VALUE);
		assertEquals(test.ReferenceCurrency, GBP);
		assertEquals(test.Currency, USD);
		assertEquals(test.Index, GBP_USD_WM);
		assertEquals(test.Sensitivity, SENSITIVITY_VALUE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		assertSame(@base.withCurrency(GBP), @base);

		FxIndexSensitivity test = @base.withCurrency(JPY);
		FxIndexSensitivity expected = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, JPY, SENSITIVITY_VALUE);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity test = @base.withSensitivity(2.5d);
		FxIndexSensitivity expected = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, 2.5d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		FxIndexSensitivity a1 = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, USD, SENSITIVITY_VALUE);
		FxIndexSensitivity a2 = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, USD, SENSITIVITY_VALUE);
		FxIndexSensitivity b = FxIndexSensitivity.of(EUR_GBP_ECB_OBS, USD, SENSITIVITY_VALUE);
		FxIndexSensitivity c = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity d = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, JPY, SENSITIVITY_VALUE);
		FxIndexSensitivity e = FxIndexSensitivity.of(GBP_USD_WM_OBS2, USD, SENSITIVITY_VALUE);
		ZeroRateSensitivity other = ZeroRateSensitivity.of(GBP, 2d, SENSITIVITY_VALUE);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) > 0, true);
		assertEquals(b.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(c) > 0, true);
		assertEquals(c.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(d) > 0, true);
		assertEquals(d.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(e) > 0, true);
		assertEquals(e.compareKey(a1) < 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, SENSITIVITY_VALUE);
		double rate = 1.35d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(EUR, USD), rate);
		FxIndexSensitivity test1 = (FxIndexSensitivity) @base.convertedTo(EUR, matrix);
		FxIndexSensitivity expected = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, EUR, SENSITIVITY_VALUE / rate);
		assertEquals(test1, expected);
		FxIndexSensitivity test2 = (FxIndexSensitivity) @base.convertedTo(USD, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toFxForwardSensitivity()
	  {
		FxIndexSensitivity test = FxIndexSensitivity.of(GBP_USD_WM_OBS, GBP, USD, SENSITIVITY_VALUE);
		LocalDate maturityDate = GBP_USD_WM.calculateMaturityFromFixing(FIXING_DATE, REF_DATA);
		FxForwardSensitivity expected = FxForwardSensitivity.of(CurrencyPair.of(GBP, USD), GBP, maturityDate, USD, SENSITIVITY_VALUE);
		assertEquals(test.toFxForwardSensitivity(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity test = @base.multipliedBy(2.4d);
		FxIndexSensitivity expected = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE * 2.4d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity test = @base.mapSensitivity(s => 1d / s);
		FxIndexSensitivity expected = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, 1d / SENSITIVITY_VALUE);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity test = @base.normalize();
		assertEquals(test, test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		FxIndexSensitivity base1 = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity base2 = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, 4.25d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(base1).add(base2);
		PointSensitivityBuilder test = base1.combinedWith(base2);
		assertEquals(test, expected);
	  }

	  public virtual void test_combinedWith_mutable()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(@base);
		PointSensitivityBuilder test = @base.combinedWith(new MutablePointSensitivities());
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		FxIndexSensitivity @base = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		FxIndexSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxIndexSensitivity test1 = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		coverImmutableBean(test1);
		FxIndexSensitivity test2 = FxIndexSensitivity.of(EUR_GBP_ECB_OBS, GBP, 4.25d);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxIndexSensitivity test = FxIndexSensitivity.of(GBP_USD_WM_OBS, USD, GBP, SENSITIVITY_VALUE);
		assertSerialization(test);
	  }

	}

}