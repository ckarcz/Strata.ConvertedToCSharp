/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// Test <seealso cref="ResolvedCms"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedCmsTest
	public class ResolvedCmsTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  internal static readonly ResolvedCmsLeg CMS_LEG = CmsTest.sutCap().CmsLeg.resolve(REF_DATA);
	  internal static readonly ResolvedSwapLeg PAY_LEG = CmsTest.sutCap().PayLeg.get().resolve(REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_twoLegs()
	  {
		ResolvedCms test = sut();
		assertEquals(test.CmsLeg, CMS_LEG);
		assertEquals(test.PayLeg.get(), PAY_LEG);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
	  }

	  public virtual void test_of_oneLeg()
	  {
		ResolvedCms test = ResolvedCms.of(CMS_LEG);
		assertEquals(test.CmsLeg, CMS_LEG);
		assertFalse(test.PayLeg.Present);
		assertEquals(test.allPaymentCurrencies(), ImmutableSet.of(CMS_LEG.Currency));
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
	  internal static ResolvedCms sut()
	  {
		return ResolvedCms.of(CMS_LEG, PAY_LEG);
	  }

	  internal static ResolvedCms sut2()
	  {
		return ResolvedCms.of(CmsTest.sutFloor().CmsLeg.resolve(REF_DATA));
	  }

	}

}