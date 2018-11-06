/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
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

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;

	/// <summary>
	/// Test <seealso cref="FxSwap"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSwapTest
	public class FxSwapTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount USD_P1550 = CurrencyAmount.of(USD, 1_550);
	  private static readonly CurrencyAmount USD_M1600 = CurrencyAmount.of(USD, -1_600);
	  private static readonly CurrencyAmount EUR_P1590 = CurrencyAmount.of(EUR, 1_590);
	  private static readonly LocalDate DATE_2011_11_21 = date(2011, 11, 21);
	  private static readonly LocalDate DATE_2011_12_21 = date(2011, 12, 21);
	  private static readonly FxSingle NEAR_LEG = FxSingle.of(GBP_P1000, USD_M1600, DATE_2011_11_21);
	  private static readonly FxSingle FAR_LEG = FxSingle.of(GBP_M1000, USD_P1550, DATE_2011_12_21);
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(FOLLOWING, GBLO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		FxSwap test = sut();
		assertEquals(test.NearLeg, NEAR_LEG);
		assertEquals(test.FarLeg, FAR_LEG);
		assertEquals(test.CrossCurrency, true);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(GBP, USD));
		assertEquals(test.allCurrencies(), ImmutableSet.of(GBP, USD));
	  }

	  public virtual void test_of_wrongOrder()
	  {
		assertThrowsIllegalArg(() => FxSwap.of(FAR_LEG, NEAR_LEG));
	  }

	  public virtual void test_of_wrongBaseCurrency()
	  {
		FxSingle nearLeg = FxSingle.of(EUR_P1590, USD_M1600, DATE_2011_11_21);
		assertThrowsIllegalArg(() => FxSwap.of(nearLeg, FAR_LEG));
	  }

	  public virtual void test_of_wrongCounterCurrency()
	  {
		FxSingle nearLeg = FxSingle.of(USD_P1550, EUR_P1590.negated(), DATE_2011_11_21);
		FxSingle farLeg = FxSingle.of(GBP_M1000, EUR_P1590, DATE_2011_12_21);
		assertThrowsIllegalArg(() => FxSwap.of(nearLeg, farLeg));
	  }

	  public virtual void test_of_sameSign()
	  {
		FxSingle farLeg = FxSingle.of(GBP_M1000.negated(), USD_P1550.negated(), DATE_2011_12_21);
		assertThrowsIllegalArg(() => FxSwap.of(NEAR_LEG, farLeg));
	  }

	  public virtual void test_of_ratesCurrencyAmountMismatch()
	  {
		assertThrowsIllegalArg(() => FxSwap.of(GBP_P1000, FxRate.of(EUR, USD, 1.1), date(2018, 6, 1), FxRate.of(EUR, USD, 1.15), date(2018, 7, 1)));
	  }

	  public virtual void test_of_ratesRateMismatch()
	  {
		assertThrowsIllegalArg(() => FxSwap.of(GBP_P1000, FxRate.of(GBP, USD, 1.1), date(2018, 6, 1), FxRate.of(EUR, USD, 1.15), date(2018, 7, 1)));
	  }

	  public virtual void test_ofForwardPoints()
	  {
		double nearRate = 1.6;
		double fwdPoint = 0.1;
		FxSwap test = FxSwap.ofForwardPoints(GBP_P1000, FxRate.of(GBP, USD, nearRate), fwdPoint, DATE_2011_11_21, DATE_2011_12_21);
		FxSingle nearLegExp = FxSingle.of(GBP_P1000, CurrencyAmount.of(USD, -1000.0 * nearRate), DATE_2011_11_21);
		FxSingle farLegExp = FxSingle.of(GBP_M1000, CurrencyAmount.of(USD, 1000.0 * (nearRate + fwdPoint)), DATE_2011_12_21);
		assertEquals(test.NearLeg, nearLegExp);
		assertEquals(test.FarLeg, farLegExp);
	  }

	  public virtual void test_ofForwardPoints_withAdjustment()
	  {
		double nearRate = 1.6;
		double fwdPoint = 0.1;
		FxSwap test = FxSwap.ofForwardPoints(GBP_P1000, FxRate.of(GBP, USD, nearRate), fwdPoint, DATE_2011_11_21, DATE_2011_12_21, BDA);
		FxSingle nearLegExp = FxSingle.of(GBP_P1000, CurrencyAmount.of(USD, -1000.0 * nearRate), DATE_2011_11_21, BDA);
		FxSingle farLegExp = FxSingle.of(GBP_M1000, CurrencyAmount.of(USD, 1000.0 * (nearRate + fwdPoint)), DATE_2011_12_21, BDA);
		assertEquals(test.NearLeg, nearLegExp);
		assertEquals(test.FarLeg, farLegExp);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolve()
	  {
		FxSwap @base = sut();
		ResolvedFxSwap test = @base.resolve(REF_DATA);
		assertEquals(test.NearLeg, NEAR_LEG.resolve(REF_DATA));
		assertEquals(test.FarLeg, FAR_LEG.resolve(REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static FxSwap sut()
	  {
		return FxSwap.of(NEAR_LEG, FAR_LEG);
	  }

	  internal static FxSwap sut2()
	  {
		FxSingle nearLeg = FxSingle.of(CurrencyAmount.of(GBP, 1_100), CurrencyAmount.of(USD, -1_650), DATE_2011_11_21);
		FxSingle farLeg = FxSingle.of(CurrencyAmount.of(GBP, -1_100), CurrencyAmount.of(USD, 1_750), DATE_2011_12_21);
		return FxSwap.of(nearLeg, farLeg);
	  }

	}

}