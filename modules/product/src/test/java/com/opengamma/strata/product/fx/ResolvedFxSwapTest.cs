/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="ResolvedFxSwap"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFxSwapTest
	public class ResolvedFxSwapTest
	{

	  private static readonly CurrencyAmount GBP_P1000 = CurrencyAmount.of(GBP, 1_000);
	  private static readonly CurrencyAmount GBP_M1000 = CurrencyAmount.of(GBP, -1_000);
	  private static readonly CurrencyAmount USD_P1550 = CurrencyAmount.of(USD, 1_550);
	  private static readonly CurrencyAmount USD_M1600 = CurrencyAmount.of(USD, -1_600);
	  private static readonly CurrencyAmount EUR_P1590 = CurrencyAmount.of(EUR, 1_590);
	  private static readonly LocalDate DATE_2011_11_21 = date(2011, 11, 21);
	  private static readonly LocalDate DATE_2011_12_21 = date(2011, 12, 21);
	  private static readonly ResolvedFxSingle NEAR_LEG = ResolvedFxSingle.of(GBP_P1000, USD_M1600, DATE_2011_11_21);
	  private static readonly ResolvedFxSingle FAR_LEG = ResolvedFxSingle.of(GBP_M1000, USD_P1550, DATE_2011_12_21);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ResolvedFxSwap test = sut();
		assertEquals(test.NearLeg, NEAR_LEG);
		assertEquals(test.FarLeg, FAR_LEG);
	  }

	  public virtual void test_of_wrongOrder()
	  {
		assertThrowsIllegalArg(() => ResolvedFxSwap.of(FAR_LEG, NEAR_LEG));
	  }

	  public virtual void test_of_wrongBaseCurrency()
	  {
		ResolvedFxSingle nearLeg = ResolvedFxSingle.of(EUR_P1590, USD_M1600, DATE_2011_11_21);
		assertThrowsIllegalArg(() => ResolvedFxSwap.of(nearLeg, FAR_LEG));
	  }

	  public virtual void test_of_wrongCounterCurrency()
	  {
		ResolvedFxSingle nearLeg = ResolvedFxSingle.of(USD_P1550, EUR_P1590.negated(), DATE_2011_11_21);
		ResolvedFxSingle farLeg = ResolvedFxSingle.of(GBP_M1000, EUR_P1590, DATE_2011_12_21);
		assertThrowsIllegalArg(() => ResolvedFxSwap.of(nearLeg, farLeg));
	  }

	  public virtual void test_of_sameSign()
	  {
		ResolvedFxSingle farLeg = ResolvedFxSingle.of(GBP_M1000.negated(), USD_P1550.negated(), DATE_2011_12_21);
		assertThrowsIllegalArg(() => ResolvedFxSwap.of(NEAR_LEG, farLeg));
	  }

	  public virtual void test_ofForwardPoints()
	  {
		double nearRate = 1.6;
		double fwdPoint = 0.1;
		ResolvedFxSwap test = ResolvedFxSwap.ofForwardPoints(GBP_P1000, USD, nearRate, fwdPoint, DATE_2011_11_21, DATE_2011_12_21);
		ResolvedFxSingle nearLegExp = ResolvedFxSingle.of(GBP_P1000, CurrencyAmount.of(USD, -1000.0 * nearRate), DATE_2011_11_21);
		ResolvedFxSingle farLegExp = ResolvedFxSingle.of(GBP_M1000, CurrencyAmount.of(USD, 1000.0 * (nearRate + fwdPoint)), DATE_2011_12_21);
		assertEquals(test.NearLeg, nearLegExp);
		assertEquals(test.FarLeg, farLegExp);
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
	  internal static ResolvedFxSwap sut()
	  {
		return ResolvedFxSwap.of(NEAR_LEG, FAR_LEG);
	  }

	  internal static ResolvedFxSwap sut2()
	  {
		ResolvedFxSingle nearLeg = ResolvedFxSingle.of(CurrencyAmount.of(GBP, 1_100), CurrencyAmount.of(USD, -1_650), DATE_2011_11_21);
		ResolvedFxSingle farLeg = ResolvedFxSingle.of(CurrencyAmount.of(GBP, -1_100), CurrencyAmount.of(USD, 1_750), DATE_2011_12_21);
		return ResolvedFxSwap.of(nearLeg, farLeg);
	  }

	}

}