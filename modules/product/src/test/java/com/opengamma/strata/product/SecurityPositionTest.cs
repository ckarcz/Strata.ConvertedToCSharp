/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="SecurityPosition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityPositionTest
	public class SecurityPositionTest
	{

	  private static readonly PositionInfo POSITION_INFO = PositionInfo.of(StandardId.of("A", "B"));
	  private static readonly SecurityId SECURITY_ID = SecurityId.of("OG-Test", "Id");
	  private static readonly SecurityId SECURITY_ID2 = SecurityId.of("OG-Test", "Id2");
	  private const double LONG_QUANTITY = 300;
	  private const double LONG_QUANTITY2 = 350;
	  private const double SHORT_QUANTITY = 200;
	  private const double SHORT_QUANTITY2 = 150;
	  private const double QUANTITY = 100;

	  //-------------------------------------------------------------------------
	  public virtual void test_ofNet_noInfo()
	  {
		SecurityPosition test = SecurityPosition.ofNet(SECURITY_ID, QUANTITY);
		assertEquals(test.Info, PositionInfo.empty());
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, QUANTITY);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.withInfo(POSITION_INFO).Info, POSITION_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withQuantity(-129).Quantity, -129d, 0d);
	  }

	  public virtual void test_ofNet_withInfo_positive()
	  {
		SecurityPosition test = SecurityPosition.ofNet(POSITION_INFO, SECURITY_ID, 100d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, 100d);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, 100d);
	  }

	  public virtual void test_ofNet_withInfo_zero()
	  {
		SecurityPosition test = SecurityPosition.ofNet(POSITION_INFO, SECURITY_ID, 0d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, 0d);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, 0d);
	  }

	  public virtual void test_ofNet_withInfo_negative()
	  {
		SecurityPosition test = SecurityPosition.ofNet(POSITION_INFO, SECURITY_ID, -100d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, 0d);
		assertEquals(test.ShortQuantity, 100d);
		assertEquals(test.Quantity, -100d);
	  }

	  public virtual void test_ofLongShort_noInfo()
	  {
		SecurityPosition test = SecurityPosition.ofLongShort(SECURITY_ID, LONG_QUANTITY, SHORT_QUANTITY);
		assertEquals(test.Info, PositionInfo.empty());
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  public virtual void test_ofLongShort_withInfo()
	  {
		SecurityPosition test = SecurityPosition.ofLongShort(POSITION_INFO, SECURITY_ID, LONG_QUANTITY, SHORT_QUANTITY);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  public virtual void test_builder()
	  {
		SecurityPosition test = sut();
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.SecurityId, SECURITY_ID);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		SecurityPosition trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(POSITION_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.SECURITY).description("Id x 100").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveTarget()
	  {
		SecurityPosition position = sut();
		GenericSecurity resolvedSecurity = GenericSecurity.of(SecurityInfo.of(SECURITY_ID, 1, CurrencyAmount.of(USD, 0.01)));
		ImmutableReferenceData refData = ImmutableReferenceData.of(SECURITY_ID, resolvedSecurity);
		GenericSecurityPosition expected = GenericSecurityPosition.ofLongShort(POSITION_INFO, resolvedSecurity, LONG_QUANTITY, SHORT_QUANTITY);
		assertEquals(position.resolveTarget(refData), expected);
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
	  internal static SecurityPosition sut()
	  {
		return SecurityPosition.builder().info(POSITION_INFO).securityId(SECURITY_ID).longQuantity(LONG_QUANTITY).shortQuantity(SHORT_QUANTITY).build();
	  }

	  internal static SecurityPosition sut2()
	  {
		return SecurityPosition.builder().info(PositionInfo.empty()).securityId(SECURITY_ID2).longQuantity(LONG_QUANTITY2).shortQuantity(SHORT_QUANTITY2).build();
	  }

	}

}