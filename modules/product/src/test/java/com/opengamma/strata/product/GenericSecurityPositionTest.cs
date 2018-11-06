/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
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

	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="GenericSecurityPosition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericSecurityPositionTest
	public class GenericSecurityPositionTest
	{

	  private static readonly PositionInfo POSITION_INFO = PositionInfo.of(StandardId.of("A", "B"));
	  private static readonly GenericSecurity SECURITY = GenericSecurityTest.sut();
	  private static readonly GenericSecurity SECURITY2 = GenericSecurityTest.sut2();
	  private const double LONG_QUANTITY = 300;
	  private const double LONG_QUANTITY2 = 350;
	  private const double SHORT_QUANTITY = 200;
	  private const double SHORT_QUANTITY2 = 150;
	  private const double QUANTITY = 100;

	  //-------------------------------------------------------------------------
	  public virtual void test_ofNet_noInfo()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofNet(SECURITY, QUANTITY);
		assertEquals(test.Info, PositionInfo.empty());
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, QUANTITY);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, QUANTITY);
		assertEquals(test.Product, SECURITY);
		assertEquals(test.SecurityId, SECURITY.SecurityId);
		assertEquals(test.Currency, SECURITY.Currency);
		assertEquals(test.withInfo(POSITION_INFO).Info, POSITION_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withQuantity(-129).Quantity, -129d, 0d);
	  }

	  public virtual void test_ofNet_withInfo_positive()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofNet(POSITION_INFO, SECURITY, 100d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, 100d);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, 100d);
	  }

	  public virtual void test_ofNet_withInfo_zero()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofNet(POSITION_INFO, SECURITY, 0d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, 0d);
		assertEquals(test.ShortQuantity, 0d);
		assertEquals(test.Quantity, 0d);
	  }

	  public virtual void test_ofNet_withInfo_negative()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofNet(POSITION_INFO, SECURITY, -100d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, 0d);
		assertEquals(test.ShortQuantity, 100d);
		assertEquals(test.Quantity, -100d);
	  }

	  public virtual void test_ofLongShort_noInfo()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofLongShort(SECURITY, LONG_QUANTITY, SHORT_QUANTITY);
		assertEquals(test.Info, PositionInfo.empty());
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  public virtual void test_ofLongShort_withInfo()
	  {
		GenericSecurityPosition test = GenericSecurityPosition.ofLongShort(POSITION_INFO, SECURITY, LONG_QUANTITY, SHORT_QUANTITY);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  public virtual void test_builder()
	  {
		GenericSecurityPosition test = sut();
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.LongQuantity, LONG_QUANTITY);
		assertEquals(test.ShortQuantity, SHORT_QUANTITY);
		assertEquals(test.Quantity, QUANTITY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		GenericSecurityPosition trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(POSITION_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.SECURITY).currencies(SECURITY.Currency).description("1 x 100").build();
		assertEquals(trade.summarize(), expected);
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
	  internal static GenericSecurityPosition sut()
	  {
		return GenericSecurityPosition.builder().info(POSITION_INFO).security(SECURITY).longQuantity(LONG_QUANTITY).shortQuantity(SHORT_QUANTITY).build();
	  }

	  internal static GenericSecurityPosition sut2()
	  {
		return GenericSecurityPosition.builder().info(PositionInfo.empty()).security(SECURITY2).longQuantity(LONG_QUANTITY2).shortQuantity(SHORT_QUANTITY2).build();
	  }

	}

}