/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="EtdOptionSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdOptionSecurityTest
	public class EtdOptionSecurityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test()
	  {
		EtdOptionSecurity test = sut();
		assertEquals(test.Variant, EtdVariant.MONTHLY);
		assertEquals(test.Type, EtdType.OPTION);
		assertEquals(test.Currency, Currency.GBP);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
		assertEquals(test.createProduct(REF_DATA), test);
		assertEquals(test.createTrade(TradeInfo.empty(), 1, 2, ReferenceData.empty()), EtdOptionTrade.of(TradeInfo.empty(), test, 1, 2));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, ReferenceData.empty()), EtdOptionPosition.ofNet(PositionInfo.empty(), test, 1));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, 2, ReferenceData.empty()), EtdOptionPosition.ofLongShort(PositionInfo.empty(), test, 1, 2));
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
	  public virtual void test_summaryDescription()
	  {
		assertEquals(sut().summaryDescription(), "Jun17 P2");
		assertEquals(sut2().summaryDescription(), "Sep17W2 V4 C3");
	  }

	  //-------------------------------------------------------------------------
	  internal static EtdOptionSecurity sut()
	  {
		return EtdOptionSecurity.builder().info(SecurityInfo.of(SecurityId.of("A", "B"), SecurityPriceInfo.of(Currency.GBP, 100))).contractSpecId(EtdContractSpecId.of("test", "123")).expiry(YearMonth.of(2017, 6)).putCall(PutCall.PUT).strikePrice(2).build();
	  }

	  internal static EtdOptionSecurity sut2()
	  {
		return EtdOptionSecurity.builder().info(SecurityInfo.of(SecurityId.of("B", "C"), SecurityPriceInfo.of(Currency.EUR, 10))).contractSpecId(EtdContractSpecId.of("test", "234")).expiry(YearMonth.of(2017, 9)).variant(EtdVariant.ofWeekly(2)).version(4).putCall(PutCall.CALL).strikePrice(3).underlyingExpiryMonth(YearMonth.of(2017, 12)).build();
	  }

	}

}