/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
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
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="GenericSecurity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericSecurityTest
	public class GenericSecurityTest
	{

	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.1, CurrencyAmount.of(GBP, 25));
	  private static readonly SecurityInfo INFO = SecurityInfo.of(SecurityId.of("Test", "1"), PRICE_INFO);
	  private static readonly SecurityInfo INFO2 = SecurityInfo.of(SecurityId.of("Test", "2"), PRICE_INFO);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		GenericSecurity test = sut();
		assertEquals(test.Info, INFO);
		assertEquals(test.SecurityId, INFO.Id);
		assertEquals(test.Currency, INFO.PriceInfo.Currency);
		assertEquals(test.UnderlyingIds, ImmutableSet.of());
		assertEquals(test, GenericSecurity.of(INFO));
		assertEquals(test.createProduct(ReferenceData.empty()), test);
		assertEquals(test.createTrade(TradeInfo.empty(), 1, 2, ReferenceData.empty()), GenericSecurityTrade.of(TradeInfo.empty(), GenericSecurity.of(INFO), 1, 2));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, ReferenceData.empty()), GenericSecurityPosition.ofNet(PositionInfo.empty(), GenericSecurity.of(INFO), 1));
		assertEquals(test.createPosition(PositionInfo.empty(), 1, 2, ReferenceData.empty()), GenericSecurityPosition.ofLongShort(PositionInfo.empty(), GenericSecurity.of(INFO), 1, 2));
	  }

	  public virtual void test_withInfo()
	  {
		GenericSecurity @base = sut();
		assertEquals(@base.Info, INFO);
		GenericSecurity test = @base.withInfo(INFO2);
		assertEquals(test.Info, INFO2);
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
	  internal static GenericSecurity sut()
	  {
		return GenericSecurity.of(INFO);
	  }

	  internal static GenericSecurity sut2()
	  {
		return GenericSecurity.of(INFO2);
	  }

	}

}