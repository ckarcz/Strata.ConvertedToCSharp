/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Test <seealso cref="ResolvedCmsLeg"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCmsLegTest
	public class ResolvedCmsLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  private static readonly CmsPeriod PERIOD_1 = CmsLegTest.sutCap().resolve(REF_DATA).CmsPeriods.get(0);
	  private static readonly CmsPeriod PERIOD_2 = CmsLegTest.sutCap().resolve(REF_DATA).CmsPeriods.get(1);

	  public virtual void test_builder()
	  {
		ResolvedCmsLeg test = sut();
		assertEquals(test.CmsPeriods.size(), 2);
		assertEquals(test.CmsPeriods.get(0), PERIOD_1);
		assertEquals(test.CmsPeriods.get(1), PERIOD_2);
		assertEquals(test.Currency, EUR);
		assertEquals(test.StartDate, PERIOD_1.StartDate);
		assertEquals(test.EndDate, PERIOD_2.EndDate);
		assertEquals(test.Index, PERIOD_1.Index);
		assertEquals(test.UnderlyingIndex, PERIOD_1.Index.Template.Convention.FloatingLeg.Index);
		assertEquals(test.PayReceive, RECEIVE);
	  }

	  public virtual void test_builder_multiCurrencyIndex()
	  {
		CmsPeriod period3 = CmsPeriodTest.sut2();
		assertThrowsIllegalArg(() => ResolvedCmsLeg.builder().payReceive(RECEIVE).cmsPeriods(PERIOD_1, period3).build());
		CmsPeriod period4 = CmsPeriodTest.sutCoupon();
		assertThrowsIllegalArg(() => ResolvedCmsLeg.builder().payReceive(RECEIVE).cmsPeriods(PERIOD_1, period4).build());
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
	  internal static ResolvedCmsLeg sut()
	  {
		return ResolvedCmsLeg.builder().payReceive(RECEIVE).cmsPeriods(PERIOD_1, PERIOD_2).build();
	  }

	  internal static ResolvedCmsLeg sut2()
	  {
		return ResolvedCmsLeg.builder().payReceive(PAY).cmsPeriods(CmsLegTest.sutFloor().resolve(REF_DATA).CmsPeriods).build();
	  }

	}

}