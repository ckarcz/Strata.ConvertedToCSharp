/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index
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
//	import static com.opengamma.strata.product.common.PutCall.CALL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="ResolvedIborFutureOption"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborFutureOptionTest
	public class ResolvedIborFutureOptionTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborFutureOption PRODUCT = IborFutureOptionTest.sut();
	  private static readonly IborFutureOption PRODUCT2 = IborFutureOptionTest.sut2();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		ResolvedIborFutureOption test = sut();
		assertEquals(test.SecurityId, PRODUCT.SecurityId);
		assertEquals(test.PutCall, PRODUCT.PutCall);
		assertEquals(test.StrikePrice, PRODUCT.StrikePrice);
		assertEquals(test.PremiumStyle, PRODUCT.PremiumStyle);
		assertEquals(test.Expiry, PRODUCT.Expiry);
		assertEquals(test.ExpiryDate, PRODUCT.ExpiryDate);
		assertEquals(test.Rounding, PRODUCT.Rounding);
		assertEquals(test.UnderlyingFuture, PRODUCT.UnderlyingFuture.resolve(REF_DATA));
		assertEquals(test.Index, PRODUCT.UnderlyingFuture.Index);
	  }

	  public virtual void test_builder_expiryNotAfterTradeDate()
	  {
		assertThrowsIllegalArg(() => ResolvedIborFutureOption.builder().securityId(PRODUCT.SecurityId).putCall(CALL).expiry(PRODUCT.UnderlyingFuture.LastTradeDate.plusDays(1).atStartOfDay(ZoneOffset.UTC)).strikePrice(PRODUCT.StrikePrice).underlyingFuture(PRODUCT.UnderlyingFuture.resolve(REF_DATA)).build());
	  }

	  public virtual void test_builder_badPrice()
	  {
		assertThrowsIllegalArg(() => sut().toBuilder().strikePrice(2.1).build());
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
	  internal static ResolvedIborFutureOption sut()
	  {
		return PRODUCT.resolve(REF_DATA);
	  }

	  internal static ResolvedIborFutureOption sut2()
	  {
		return PRODUCT2.resolve(REF_DATA);
	  }

	}

}