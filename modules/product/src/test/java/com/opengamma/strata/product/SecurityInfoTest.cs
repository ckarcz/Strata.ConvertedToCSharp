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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="SecurityInfo"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SecurityInfoTest
	public class SecurityInfoTest
	{

	  private static readonly SecurityId ID = SecurityId.of("OG-Test", "Test");
	  private static readonly SecurityId ID2 = SecurityId.of("OG-Test", "Test2");
	  private static readonly SecurityPriceInfo PRICE_INFO = SecurityPriceInfo.of(0.01, CurrencyAmount.of(GBP, 0.01));
	  private static readonly SecurityPriceInfo PRICE_INFO2 = SecurityPriceInfo.of(0.02, CurrencyAmount.of(GBP, 1));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.google.common.collect.ImmutableMap<AttributeType<?>, Object> INFO_MAP = com.google.common.collect.ImmutableMap.of(AttributeType.NAME, "A");
	  private static readonly ImmutableMap<AttributeType<object>, object> INFO_MAP = ImmutableMap.of(AttributeType.NAME, "A");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_priceInfoFields()
	  {
		SecurityInfo test = SecurityInfo.of(ID, PRICE_INFO.TickSize, PRICE_INFO.TickValue);
		assertEquals(test.Id, ID);
		assertEquals(test.PriceInfo, PRICE_INFO);
		assertEquals(test.Attributes, ImmutableMap.of());
		assertThrowsIllegalArg(() => test.getAttribute(AttributeType.NAME));
		assertEquals(test.findAttribute(AttributeType.NAME), null);
	  }

	  public virtual void test_of_priceInfo()
	  {
		SecurityInfo test = SecurityInfo.of(ID, PRICE_INFO);
		assertEquals(test.Id, ID);
		assertEquals(test.PriceInfo, PRICE_INFO);
		assertEquals(test.Attributes, ImmutableMap.of());
		assertThrowsIllegalArg(() => test.getAttribute(AttributeType.NAME));
		assertEquals(test.findAttribute(AttributeType.NAME), null);
	  }

	  public virtual void test_of_withAdditionalInfo()
	  {
		SecurityInfo test = SecurityInfo.of(ID, PRICE_INFO).withAttribute(AttributeType.NAME, "B").withAttribute(AttributeType.NAME, "A"); // overwrites "B"
		assertEquals(test.Id, ID);
		assertEquals(test.PriceInfo, PRICE_INFO);
		assertEquals(test.Attributes, INFO_MAP);
		assertEquals(test.getAttribute(AttributeType.NAME), "A");
		assertEquals(test.findAttribute(AttributeType.NAME), ("A"));
	  }

	  public virtual void test_builder()
	  {
		SecurityInfo test = SecurityInfo.builder().id(ID).priceInfo(PRICE_INFO).addAttribute(AttributeType.NAME, "B").addAttribute(AttributeType.NAME, "A").build();
		assertEquals(test.Id, ID);
		assertEquals(test.PriceInfo, PRICE_INFO);
		assertEquals(test.Attributes, INFO_MAP);
		assertEquals(test.getAttribute(AttributeType.NAME), "A");
		assertEquals(test.findAttribute(AttributeType.NAME), ("A"));
	  }

	  public virtual void test_toBuilder()
	  {
		SecurityInfo test = SecurityInfo.builder().addAttribute(AttributeType.NAME, "name").id(ID).priceInfo(PRICE_INFO).build().toBuilder().id(ID2).build();
		assertEquals(test.Id, ID2);
		assertEquals(test.getAttribute(AttributeType.NAME), "name");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SecurityInfo test = SecurityInfo.of(ID, PRICE_INFO);
		coverImmutableBean(test);
		SecurityInfo test2 = SecurityInfo.of(ID2, PRICE_INFO2).withAttribute(AttributeType.NAME, "A");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SecurityInfo test = SecurityInfo.of(ID, PRICE_INFO);
		assertSerialization(test);
	  }

	}

}