/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ResolvedFixedCouponBondSettlement"/>. 
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedFixedCouponBondSettlementTest
	public class ResolvedFixedCouponBondSettlementTest
	{

	  private static readonly LocalDate SETTLE_DATE = date(2018, 6, 1);
	  private static readonly LocalDate SETTLE_DATE2 = date(2018, 6, 2);
	  private const double PRICE = 99.2;
	  private const double PRICE2 = 99.5;

	  public virtual void test_of()
	  {
		ResolvedFixedCouponBondSettlement test = sut();
		assertEquals(test.SettlementDate, SETTLE_DATE);
		assertEquals(test.Price, PRICE);
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
	  internal static ResolvedFixedCouponBondSettlement sut()
	  {
		return ResolvedFixedCouponBondSettlement.of(SETTLE_DATE, PRICE);
	  }

	  internal static ResolvedFixedCouponBondSettlement sut2()
	  {
		return ResolvedFixedCouponBondSettlement.of(SETTLE_DATE2, PRICE2);
	  }

	}

}