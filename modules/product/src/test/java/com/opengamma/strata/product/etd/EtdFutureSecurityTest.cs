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

	/// <summary>
	/// Test <seealso cref="EtdFutureSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdFutureSecurityTest
	public class EtdFutureSecurityTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  public virtual void test()
	  {
		EtdFutureSecurity test = sut();
		assertEquals(test.Variant, EtdVariant.MONTHLY);
		assertEquals(test.Type, EtdType.FUTURE);
		assertEquals(test.Currency, Currency.GBP);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
		assertEquals(test.createProduct(REF_DATA), test);
		assertEquals(test.createTrade(TradeInfo.empty(), 1, 2, ReferenceData.empty()), EtdFutureTrade.of(TradeInfo.empty(), test, 1, 2));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, ReferenceData.empty()), EtdFuturePosition.ofNet(PositionInfo.empty(), test, 1));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, 2, ReferenceData.empty()), EtdFuturePosition.ofLongShort(PositionInfo.empty(), test, 1, 2));
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
		assertEquals(sut().summaryDescription(), "Jun17");
		assertEquals(sut2().summaryDescription(), "W2Sep17");
	  }

	  //-------------------------------------------------------------------------
	  internal static EtdFutureSecurity sut()
	  {
		return EtdFutureSecurity.builder().info(SecurityInfo.of(SecurityId.of("A", "B"), SecurityPriceInfo.of(Currency.GBP, 100))).contractSpecId(EtdContractSpecId.of("test", "123")).expiry(YearMonth.of(2017, 6)).build();
	  }

	  internal static EtdFutureSecurity sut2()
	  {
		return EtdFutureSecurity.builder().info(SecurityInfo.of(SecurityId.of("B", "C"), SecurityPriceInfo.of(Currency.EUR, 10))).contractSpecId(EtdContractSpecId.of("test", "234")).expiry(YearMonth.of(2017, 9)).variant(EtdVariant.ofWeekly(2)).build();
	  }

	}

}