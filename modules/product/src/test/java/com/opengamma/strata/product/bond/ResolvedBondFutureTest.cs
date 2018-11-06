/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Payment = com.opengamma.strata.basics.currency.Payment;

	/// <summary>
	/// Test <seealso cref="ResolvedBondFuture"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedBondFutureTest
	public class ResolvedBondFutureTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_noDeliveryDate()
	  {
		ResolvedBondFuture @base = sut();
		ResolvedBondFuture test = ResolvedBondFuture.builder().securityId(@base.SecurityId).deliveryBasket(@base.DeliveryBasket).conversionFactors(@base.ConversionFactors).firstNoticeDate(@base.FirstNoticeDate).lastNoticeDate(@base.LastNoticeDate).firstDeliveryDate(@base.FirstDeliveryDate).lastDeliveryDate(@base.LastDeliveryDate).lastTradeDate(@base.LastTradeDate).rounding(@base.Rounding).build();
		assertEquals(test, @base);
	  }

	  public virtual void test_builder_fail()
	  {
		ResolvedBondFuture @base = sut();
		// wrong size
		assertThrowsIllegalArg(() => ResolvedBondFuture.builder().securityId(@base.SecurityId).deliveryBasket(@base.DeliveryBasket.subList(0, 1)).conversionFactors(@base.ConversionFactors).firstNoticeDate(@base.FirstNoticeDate).lastNoticeDate(@base.LastNoticeDate).lastTradeDate(@base.LastTradeDate).build());
		// first notice date missing
		assertThrowsIllegalArg(() => ResolvedBondFuture.builder().securityId(@base.SecurityId).deliveryBasket(@base.DeliveryBasket).conversionFactors(@base.ConversionFactors).lastNoticeDate(@base.LastNoticeDate).lastTradeDate(@base.LastTradeDate).build());
		// last notice date missing
		assertThrowsIllegalArg(() => ResolvedBondFuture.builder().securityId(@base.SecurityId).deliveryBasket(@base.DeliveryBasket).conversionFactors(@base.ConversionFactors).firstNoticeDate(@base.FirstNoticeDate).lastTradeDate(@base.LastTradeDate).build());
		// basket list empty
		assertThrowsIllegalArg(() => ResolvedBondFuture.builder().securityId(@base.SecurityId).firstNoticeDate(@base.FirstNoticeDate).lastNoticeDate(@base.LastNoticeDate).lastTradeDate(@base.LastTradeDate).build());
		// notional mismatch
		ResolvedFixedCouponBond bond0 = @base.DeliveryBasket.get(0);
		ResolvedFixedCouponBond bond1 = bond0.toBuilder().nominalPayment(Payment.of(USD, 100, date(2016, 6, 30))).build();
		assertThrowsIllegalArg(() => ResolvedBondFuture.builder().securityId(@base.SecurityId).deliveryBasket(bond0, bond1).conversionFactors(1d, 2d).firstNoticeDate(@base.FirstNoticeDate).firstDeliveryDate(@base.FirstDeliveryDate).lastNoticeDate(@base.LastNoticeDate).lastDeliveryDate(@base.LastDeliveryDate).lastTradeDate(@base.LastTradeDate).rounding(@base.Rounding).build());
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
	  internal static ResolvedBondFuture sut()
	  {
		return BondFutureTest.sut().resolve(REF_DATA);
	  }

	  internal static ResolvedBondFuture sut2()
	  {
		return BondFutureTest.sut2().resolve(REF_DATA);
	  }

	}

}