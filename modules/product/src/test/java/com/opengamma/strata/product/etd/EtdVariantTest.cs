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

	/// <summary>
	/// Test <seealso cref="EtdVariant"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdVariantTest
	public class EtdVariantTest
	{

	  public virtual void test_monthly()
	  {
		EtdVariant test = EtdVariant.ofMonthly();
		assertEquals(test.Type, EtdExpiryType.MONTHLY);
		assertEquals(test.DateCode.HasValue, false);
		assertEquals(test.SettlementType.Present, false);
		assertEquals(test.OptionType.Present, false);
		assertEquals(test.Flex, false);
		assertEquals(test.Code, "");
	  }

	  public virtual void test_weekly()
	  {
		EtdVariant test = EtdVariant.ofWeekly(2);
		assertEquals(test.Type, EtdExpiryType.WEEKLY);
		assertEquals(test.DateCode.Value, 2);
		assertEquals(test.SettlementType.Present, false);
		assertEquals(test.OptionType.Present, false);
		assertEquals(test.Flex, false);
		assertEquals(test.Code, "W2");
	  }

	  public virtual void test_daily()
	  {
		EtdVariant test = EtdVariant.ofDaily(24);
		assertEquals(test.Type, EtdExpiryType.DAILY);
		assertEquals(test.DateCode.Value, 24);
		assertEquals(test.SettlementType.Present, false);
		assertEquals(test.OptionType.Present, false);
		assertEquals(test.Flex, false);
		assertEquals(test.Code, "24");
	  }

	  public virtual void test_flexFuture()
	  {
		EtdVariant test = EtdVariant.ofFlexFuture(2, EtdSettlementType.CASH);
		assertEquals(test.Type, EtdExpiryType.DAILY);
		assertEquals(test.DateCode.Value, 2);
		assertEquals(test.SettlementType.get(), EtdSettlementType.CASH);
		assertEquals(test.OptionType.Present, false);
		assertEquals(test.Flex, true);
		assertEquals(test.Code, "02C");
	  }

	  public virtual void test_flexOption()
	  {
		EtdVariant test = EtdVariant.ofFlexOption(24, EtdSettlementType.CASH, EtdOptionType.AMERICAN);
		assertEquals(test.Type, EtdExpiryType.DAILY);
		assertEquals(test.DateCode.Value, 24);
		assertEquals(test.SettlementType.get(), EtdSettlementType.CASH);
		assertEquals(test.OptionType.get(), EtdOptionType.AMERICAN);
		assertEquals(test.Flex, true);
		assertEquals(test.Code, "24CA");
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
	  internal static EtdVariant sut()
	  {
		return EtdVariant.MONTHLY;
	  }

	  internal static EtdVariant sut2()
	  {
		return EtdVariant.ofFlexOption(6, EtdSettlementType.CASH, EtdOptionType.EUROPEAN);
	  }

	}

}