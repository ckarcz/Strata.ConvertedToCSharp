/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="RatesCurveGroupId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupIdTest
	public class RatesCurveGroupIdTest
	{

	  private static readonly CurveGroupName GROUP1 = CurveGroupName.of("Group1");
	  private static readonly CurveGroupName GROUP2 = CurveGroupName.of("Group2");
	  private static readonly ObservableSource OBS_SOURCE2 = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		RatesCurveGroupId test = RatesCurveGroupId.of(GROUP1.ToString());
		assertEquals(test.CurveGroupName, GROUP1);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(RatesCurveGroup));
		assertEquals(test.ToString(), "RatesCurveGroupId:Group1");
	  }

	  public virtual void test_of_Type()
	  {
		RatesCurveGroupId test = RatesCurveGroupId.of(GROUP1);
		assertEquals(test.CurveGroupName, GROUP1);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(RatesCurveGroup));
		assertEquals(test.ToString(), "RatesCurveGroupId:Group1");
	  }

	  public virtual void test_of_TypeSource()
	  {
		RatesCurveGroupId test = RatesCurveGroupId.of(GROUP1, OBS_SOURCE2);
		assertEquals(test.CurveGroupName, GROUP1);
		assertEquals(test.ObservableSource, OBS_SOURCE2);
		assertEquals(test.MarketDataType, typeof(RatesCurveGroup));
		assertEquals(test.ToString(), "RatesCurveGroupId:Group1/Vendor");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatesCurveGroupId test = RatesCurveGroupId.of(GROUP1);
		coverImmutableBean(test);
		RatesCurveGroupId test2 = RatesCurveGroupId.of(GROUP2, OBS_SOURCE2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatesCurveGroupId test = RatesCurveGroupId.of(GROUP1);
		assertSerialization(test);
	  }

	}

}