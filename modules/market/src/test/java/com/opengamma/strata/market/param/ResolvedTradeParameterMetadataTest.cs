/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using BulletPayment = com.opengamma.strata.product.payment.BulletPayment;
	using ResolvedBulletPaymentTrade = com.opengamma.strata.product.payment.ResolvedBulletPaymentTrade;

	/// <summary>
	/// Test <seealso cref="ResolvedTradeParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedTradeParameterMetadataTest
	public class ResolvedTradeParameterMetadataTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly ResolvedTrade TRADE = ResolvedBulletPaymentTrade.of(TradeInfo.empty(), BulletPayment.builder().date(AdjustableDate.of(LocalDate.of(2017, 1, 3))).value(CurrencyAmount.of(Currency.BHD, 100d)).payReceive(PayReceive.PAY).build().resolve(REF_DATA));

	  public virtual void test_of()
	  {
		ResolvedTradeParameterMetadata test = ResolvedTradeParameterMetadata.of(TRADE, "Label");
		assertEquals(test.Label, "Label");
		assertEquals(test.Identifier, "Label");
		assertEquals(test.Trade, TRADE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedTradeParameterMetadata test1 = ResolvedTradeParameterMetadata.of(TRADE, "Label");
		coverImmutableBean(test1);
		ResolvedTrade trade = ResolvedBulletPaymentTrade.of(TradeInfo.empty(), BulletPayment.builder().date(AdjustableDate.of(LocalDate.of(2017, 3, 3))).value(CurrencyAmount.of(Currency.USD, 100d)).payReceive(PayReceive.PAY).build().resolve(REF_DATA));
		ResolvedTradeParameterMetadata test2 = ResolvedTradeParameterMetadata.builder().trade(trade).label("Label2").build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedTradeParameterMetadata test = ResolvedTradeParameterMetadata.of(TRADE, "Label");
		assertSerialization(test);
	  }

	}

}