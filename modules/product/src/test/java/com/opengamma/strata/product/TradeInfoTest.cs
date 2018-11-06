/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
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

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="TradeInfo"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TradeInfoTest
	public class TradeInfoTest
	{

	  private static readonly StandardId ID = StandardId.of("OG-Test", "123");
	  private static readonly StandardId COUNTERPARTY = StandardId.of("OG-Party", "Other");

	  public virtual void test_builder()
	  {
		TradeInfo test = TradeInfo.builder().counterparty(COUNTERPARTY).build();
		assertEquals(test.Id, null);
		assertEquals(test.Counterparty, COUNTERPARTY);
		assertEquals(test.TradeDate, null);
		assertEquals(test.TradeTime, null);
		assertEquals(test.Zone, null);
		assertEquals(test.SettlementDate, null);
		assertEquals(test.AttributeTypes, ImmutableSet.of());
		assertEquals(test.Attributes, ImmutableMap.of());
		assertThrowsIllegalArg(() => test.getAttribute(AttributeType.DESCRIPTION));
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), null);
	  }

	  public virtual void test_builder_withers()
	  {
		TradeInfo test = TradeInfo.builder().counterparty(COUNTERPARTY).build().withId(ID).withAttribute(AttributeType.DESCRIPTION, "A");
		assertEquals(test.Id, ID);
		assertEquals(test.Counterparty, COUNTERPARTY);
		assertEquals(test.TradeDate, null);
		assertEquals(test.TradeTime, null);
		assertEquals(test.Zone, null);
		assertEquals(test.SettlementDate, null);
		assertEquals(test.AttributeTypes, ImmutableSet.of(AttributeType.DESCRIPTION));
		assertEquals(test.Attributes, ImmutableMap.of(AttributeType.DESCRIPTION, "A"));
		assertEquals(test.getAttribute(AttributeType.DESCRIPTION), "A");
		assertEquals(test.findAttribute(AttributeType.DESCRIPTION), ("A"));
	  }

	  public virtual void test_toBuilder()
	  {
		TradeInfo test = TradeInfo.builder().counterparty(COUNTERPARTY).build().toBuilder().id(ID).build();
		assertEquals(test.Id, ID);
		assertEquals(test.Counterparty, COUNTERPARTY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TradeInfo test = TradeInfo.builder().addAttribute(AttributeType.DESCRIPTION, "A").counterparty(COUNTERPARTY).tradeDate(date(2014, 6, 20)).tradeTime(LocalTime.MIDNIGHT).zone(ZoneId.systemDefault()).settlementDate(date(2014, 6, 20)).build();
		coverImmutableBean(test);
		TradeInfo test2 = TradeInfo.builder().id(StandardId.of("OG-Id", "1")).counterparty(StandardId.of("OG-Party", "Other2")).tradeDate(date(2014, 6, 21)).tradeTime(LocalTime.NOON).zone(ZoneOffset.UTC).settlementDate(date(2014, 6, 21)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		TradeInfo test = TradeInfo.builder().counterparty(COUNTERPARTY).tradeDate(date(2014, 6, 20)).tradeTime(LocalTime.MIDNIGHT).zone(ZoneOffset.UTC).settlementDate(date(2014, 6, 20)).build();
		assertSerialization(test);
	  }

	}

}