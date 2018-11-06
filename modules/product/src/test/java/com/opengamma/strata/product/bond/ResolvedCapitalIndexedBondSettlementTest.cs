/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
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

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;

	/// <summary>
	/// Test <seealso cref="ResolvedCapitalIndexedBondSettlement"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCapitalIndexedBondSettlementTest
	public class ResolvedCapitalIndexedBondSettlementTest
	{

	  private static readonly LocalDate SETTLE_DATE = date(2018, 6, 1);
	  private static readonly LocalDate SETTLE_DATE2 = date(2018, 6, 2);
	  private const double PRICE = 99.2;
	  private const double PRICE2 = 99.5;
	  private static readonly BondPaymentPeriod SETTLE_PERIOD = KnownAmountBondPaymentPeriod.of(Payment.of(Currency.GBP, 100, SETTLE_DATE), SchedulePeriod.of(SETTLE_DATE.minusMonths(1), SETTLE_DATE));
	  private static readonly BondPaymentPeriod SETTLE_PERIOD2 = KnownAmountBondPaymentPeriod.of(Payment.of(Currency.GBP, 200, SETTLE_DATE2), SchedulePeriod.of(SETTLE_DATE2.minusMonths(1), SETTLE_DATE2));

	  public virtual void test_of()
	  {
		ResolvedCapitalIndexedBondSettlement test = sut();
		assertEquals(test.SettlementDate, SETTLE_DATE);
		assertEquals(test.Price, PRICE);
		assertEquals(test.Payment, SETTLE_PERIOD);
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
	  internal static ResolvedCapitalIndexedBondSettlement sut()
	  {
		return ResolvedCapitalIndexedBondSettlement.of(SETTLE_DATE, PRICE, SETTLE_PERIOD);
	  }

	  internal static ResolvedCapitalIndexedBondSettlement sut2()
	  {
		return ResolvedCapitalIndexedBondSettlement.of(SETTLE_DATE2, PRICE2, SETTLE_PERIOD2);
	  }

	}

}