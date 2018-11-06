/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using SettlementType = com.opengamma.strata.product.common.SettlementType;

	/// <summary>
	/// Test <seealso cref="CashSwaptionSettlement"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CashSwaptionSettlementTest
	public class CashSwaptionSettlementTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		CashSwaptionSettlement test = CashSwaptionSettlement.of(date(2015, 6, 30), CashSwaptionSettlementMethod.CASH_PRICE);
		assertEquals(test.Method, CashSwaptionSettlementMethod.CASH_PRICE);
		assertEquals(test.SettlementDate, date(2015, 6, 30));
		assertEquals(test.SettlementType, SettlementType.CASH);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CashSwaptionSettlement test = CashSwaptionSettlement.of(date(2015, 6, 30), CashSwaptionSettlementMethod.CASH_PRICE);
		coverImmutableBean(test);
		CashSwaptionSettlement test2 = CashSwaptionSettlement.of(date(2015, 7, 30), CashSwaptionSettlementMethod.PAR_YIELD);
		coverBeanEquals(test, test2);
		coverEnum(typeof(CashSwaptionSettlementMethod));
		coverEnum(typeof(SettlementType));
	  }

	  public virtual void test_serialization()
	  {
		CashSwaptionSettlement test = CashSwaptionSettlement.of(date(2015, 6, 30), CashSwaptionSettlementMethod.CASH_PRICE);
		assertSerialization(test);
	  }

	}

}