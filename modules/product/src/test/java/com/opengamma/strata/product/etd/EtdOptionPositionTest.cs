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

	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Test <seealso cref="EtdOptionPosition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdOptionPositionTest
	public class EtdOptionPositionTest
	{

	  private static readonly PositionInfo POSITION_INFO = PositionInfo.of(StandardId.of("A", "B"));
	  private static readonly EtdOptionSecurity SECURITY = EtdOptionSecurityTest.sut();
	  private const int LONG_QUANTITY = 3000;
	  private const int SHORT_QUANTITY = 2000;

	  public virtual void test_ofNet()
	  {
		EtdOptionPosition test = EtdOptionPosition.ofNet(SECURITY, 1000);
		assertEquals(test.LongQuantity, 1000d, 0d);
		assertEquals(test.ShortQuantity, 0d, 0d);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, 1000d, 0d);
		assertEquals(test.withInfo(POSITION_INFO).Info, POSITION_INFO);
		assertEquals(test.withQuantity(129).Quantity, 129d, 0d);
		assertEquals(test.withQuantity(-129).Quantity, -129d, 0d);
	  }

	  public virtual void test_ofNet_short()
	  {
		EtdOptionPosition test = EtdOptionPosition.ofNet(SECURITY, -1000);
		assertEquals(test.LongQuantity, 0d, 0d);
		assertEquals(test.ShortQuantity, 1000d, 0d);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, -1000d, 0d);
	  }

	  public virtual void test_ofNet_withInfo()
	  {
		EtdOptionPosition test = EtdOptionPosition.ofNet(POSITION_INFO, SECURITY, 1000);
		assertEquals(test.LongQuantity, 1000d, 0d);
		assertEquals(test.ShortQuantity, 0d, 0d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, 1000d, 0d);
	  }

	  public virtual void test_ofLongShort()
	  {
		EtdOptionPosition test = EtdOptionPosition.ofLongShort(SECURITY, 2000, 1000);
		assertEquals(test.LongQuantity, 2000d, 0d);
		assertEquals(test.ShortQuantity, 1000d, 0d);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, 1000d, 0d);
	  }

	  public virtual void test_ofLongShort_withInfo()
	  {
		EtdOptionPosition test = EtdOptionPosition.ofLongShort(POSITION_INFO, SECURITY, 2000, 1000);
		assertEquals(test.LongQuantity, 2000d, 0d);
		assertEquals(test.ShortQuantity, 1000d, 0d);
		assertEquals(test.Info, POSITION_INFO);
		assertEquals(test.Security, SECURITY);
		assertEquals(test.Quantity, 1000d, 0d);
	  }

	  public virtual void test_methods()
	  {
		EtdOptionPosition test = sut();
		assertEquals(test.Type, EtdType.OPTION);
		assertEquals(test.Currency, Currency.GBP);
		assertEquals(test.SecurityId, test.Security.SecurityId);
		assertEquals(test.Quantity, 1000d, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		EtdOptionPosition trade = sut();
		PortfolioItemSummary expected = PortfolioItemSummary.builder().id(POSITION_INFO.Id.orElse(null)).portfolioItemType(PortfolioItemType.POSITION).productType(ProductType.ETD_OPTION).currencies(SECURITY.Currency).description(SECURITY.SecurityId.StandardId.Value + " x 1000, Jun17 P2").build();
		assertEquals(trade.summarize(), expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_resolveTarget()
	  {
		EtdOptionPosition position = sut();
		GenericSecurity resolvedSecurity = GenericSecurity.of(SECURITY.Info);
		ImmutableReferenceData refData = ImmutableReferenceData.of(SECURITY.SecurityId, resolvedSecurity);
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
	  internal static EtdOptionPosition sut()
	  {
		return EtdOptionPosition.builder().info(POSITION_INFO).security(SECURITY).longQuantity(LONG_QUANTITY).shortQuantity(SHORT_QUANTITY).build();
	  }

	  internal static EtdOptionPosition sut2()
	  {
		return EtdOptionPosition.builder().security(EtdOptionSecurityTest.sut2()).longQuantity(4000).shortQuantity(1000).build();
	  }

	}

}