/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
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
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Test <seealso cref="OvernightRateSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightRateSensitivityTest
	public class OvernightRateSensitivityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE = date(2015, 8, 27);
	  private static readonly LocalDate DATE2 = date(2015, 9, 27);
	  private static readonly OvernightIndexObservation GBP_SONIA_OBSERVATION = OvernightIndexObservation.of(GBP_SONIA, DATE, REF_DATA);
	  private static readonly OvernightIndexObservation GBP_SONIA_OBSERVATION2 = OvernightIndexObservation.of(GBP_SONIA, DATE2, REF_DATA);
	  private static readonly OvernightIndexObservation USD_FED_FUND_OBSERVATION2 = OvernightIndexObservation.of(USD_FED_FUND, DATE2, REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noCurrency()
	  {
		OvernightRateSensitivity test = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.Currency, GBP);
		assertEquals(test.EndDate, date(2015, 8, 28));
		assertEquals(test.Sensitivity, 32d);
		assertEquals(test.Index, GBP_SONIA);
	  }

	  public virtual void test_of_currency()
	  {
		OvernightRateSensitivity test = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, USD, 32d);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.Currency, USD);
		assertEquals(test.EndDate, date(2015, 8, 28));
		assertEquals(test.Sensitivity, 32d);
		assertEquals(test.Index, GBP_SONIA);
	  }

	  public virtual void test_ofPeriod()
	  {
		OvernightRateSensitivity test = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, date(2015, 10, 27), GBP, 32d);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.Currency, GBP);
		assertEquals(test.EndDate, date(2015, 10, 27));
		assertEquals(test.Sensitivity, 32d);
		assertEquals(test.Index, GBP_SONIA);
	  }

	  public virtual void test_badDateOrder()
	  {
		assertThrowsIllegalArg(() => OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, DATE, GBP, 32d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withCurrency()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		assertSame(@base.withCurrency(GBP), @base);

		LocalDate mat = GBP_SONIA_OBSERVATION.MaturityDate;
		OvernightRateSensitivity expected = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, mat, USD, 32d);
		OvernightRateSensitivity test = @base.withCurrency(USD);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 20d);
		OvernightRateSensitivity test = @base.withSensitivity(20d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_compareKey()
	  {
		OvernightRateSensitivity a1 = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, date(2015, 10, 27), GBP, 32d);
		OvernightRateSensitivity a2 = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, date(2015, 10, 27), GBP, 32d);
		OvernightRateSensitivity b = OvernightRateSensitivity.ofPeriod(USD_FED_FUND_OBSERVATION2, date(2015, 10, 27), GBP, 32d);
		OvernightRateSensitivity c = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, date(2015, 10, 27), USD, 32d);
		OvernightRateSensitivity d = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION2, date(2015, 10, 27), GBP, 32d);
		OvernightRateSensitivity e = OvernightRateSensitivity.ofPeriod(GBP_SONIA_OBSERVATION, date(2015, 11, 27), GBP, 32d);
		ZeroRateSensitivity other = ZeroRateSensitivity.of(GBP, 2d, 32d);
		assertEquals(a1.compareKey(a2), 0);
		assertEquals(a1.compareKey(b) < 0, true);
		assertEquals(b.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(c) < 0, true);
		assertEquals(c.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(e) < 0, true);
		assertEquals(d.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(d) < 0, true);
		assertEquals(e.compareKey(a1) > 0, true);
		assertEquals(a1.compareKey(other) < 0, true);
		assertEquals(other.compareKey(a1) > 0, true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		LocalDate fixingDate = DATE;
		LocalDate endDate = date(2015, 10, 27);
		double sensi = 32d;
		OvernightRateSensitivity @base = OvernightRateSensitivity.ofPeriod(OvernightIndexObservation.of(GBP_SONIA, fixingDate, REF_DATA), endDate, GBP, sensi);
		double rate = 1.5d;
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), rate);
		OvernightRateSensitivity test1 = (OvernightRateSensitivity) @base.convertedTo(USD, matrix);
		OvernightRateSensitivity expected = OvernightRateSensitivity.ofPeriod(OvernightIndexObservation.of(GBP_SONIA, fixingDate, REF_DATA), endDate, USD, rate * sensi);
		assertEquals(test1, expected);
		OvernightRateSensitivity test2 = (OvernightRateSensitivity) @base.convertedTo(GBP, matrix);
		assertEquals(test2, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d * 3.5d);
		OvernightRateSensitivity test = @base.multipliedBy(3.5d);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity expected = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 1 / 32d);
		OvernightRateSensitivity test = @base.mapSensitivity(s => 1 / s);
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity test = @base.normalize();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		OvernightRateSensitivity base1 = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity base2 = OvernightRateSensitivity.of(OvernightIndexObservation.of(GBP_SONIA, date(2015, 10, 27), REF_DATA), 22d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(base1).add(base2);
		PointSensitivityBuilder test = base1.combinedWith(base2);
		assertEquals(test, expected);
	  }

	  public virtual void test_combinedWith_mutable()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		MutablePointSensitivities expected = new MutablePointSensitivities();
		expected.add(@base);
		PointSensitivityBuilder test = @base.combinedWith(new MutablePointSensitivities());
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_build()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		PointSensitivities test = @base.build();
		assertEquals(test.Sensitivities, ImmutableList.of(@base));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		OvernightRateSensitivity @base = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		OvernightRateSensitivity test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightRateSensitivity test = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		coverImmutableBean(test);
		OvernightRateSensitivity test2 = OvernightRateSensitivity.of(USD_FED_FUND_OBSERVATION2, 16d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightRateSensitivity test = OvernightRateSensitivity.of(GBP_SONIA_OBSERVATION, 32d);
		assertSerialization(test);
	  }

	}

}