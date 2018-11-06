/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="CurveId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveIdTest
	public class CurveIdTest
	{

	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		CurveId test = CurveId.of("Group", "Name");
		assertEquals(test.CurveGroupName, CurveGroupName.of("Group"));
		assertEquals(test.CurveName, CurveName.of("Name"));
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(Curve));
		assertEquals(test.MarketDataName, CurveName.of("Name"));
		assertEquals(test.ToString(), "CurveId:Group/Name");
	  }

	  public virtual void test_of_Types()
	  {
		CurveId test = CurveId.of(CurveGroupName.of("Group"), CurveName.of("Name"));
		assertEquals(test.CurveGroupName, CurveGroupName.of("Group"));
		assertEquals(test.CurveName, CurveName.of("Name"));
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(Curve));
		assertEquals(test.MarketDataName, CurveName.of("Name"));
		assertEquals(test.ToString(), "CurveId:Group/Name");
	  }

	  public virtual void test_of_TypesSource()
	  {
		CurveId test = CurveId.of(CurveGroupName.of("Group"), CurveName.of("Name"), OBS_SOURCE);
		assertEquals(test.CurveGroupName, CurveGroupName.of("Group"));
		assertEquals(test.CurveName, CurveName.of("Name"));
		assertEquals(test.ObservableSource, OBS_SOURCE);
		assertEquals(test.MarketDataType, typeof(Curve));
		assertEquals(test.MarketDataName, CurveName.of("Name"));
		assertEquals(test.ToString(), "CurveId:Group/Name/Vendor");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurveId test = CurveId.of("Group", "Name");
		coverImmutableBean(test);
		CurveId test2 = CurveId.of("Group2", "Name2");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CurveId test = CurveId.of("Group", "Name");
		assertSerialization(test);
	  }

	}

}