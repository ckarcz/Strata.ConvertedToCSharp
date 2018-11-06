/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
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

	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// Test <seealso cref="IssuerCurveInputsId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IssuerCurveInputsIdTest
	public class IssuerCurveInputsIdTest
	{

	  private static readonly CurveGroupName GROUP1 = CurveGroupName.of("Group1");
	  private static readonly CurveGroupName GROUP2 = CurveGroupName.of("Group2");
	  private static readonly CurveName NAME1 = CurveName.of("Name1");
	  private static readonly CurveName NAME2 = CurveName.of("Name2");
	  private static readonly ObservableSource SOURCE2 = ObservableSource.of("Vendor2");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		IssuerCurveInputsId test = IssuerCurveInputsId.of(GROUP1, NAME1, ObservableSource.NONE);
		assertEquals(test.CurveGroupName, GROUP1);
		assertEquals(test.CurveName, NAME1);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(RatesCurveInputs));
		assertEquals(test.ToString(), "IssuerCurveInputsId:Group1/Name1");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IssuerCurveInputsId test = IssuerCurveInputsId.of(GROUP1, NAME1, ObservableSource.NONE);
		coverImmutableBean(test);
		IssuerCurveInputsId test2 = IssuerCurveInputsId.of(GROUP2, NAME2, SOURCE2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IssuerCurveInputsId test = IssuerCurveInputsId.of(GROUP1, NAME1, ObservableSource.NONE);
		assertSerialization(test);
	  }

	}

}