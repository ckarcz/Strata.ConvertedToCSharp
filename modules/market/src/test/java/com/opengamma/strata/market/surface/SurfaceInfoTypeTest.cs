/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;

	/// <summary>
	/// Test <seealso cref="SurfaceInfoType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SurfaceInfoTypeTest
	public class SurfaceInfoTypeTest
	{

	  public virtual void test_DAY_COUNT()
	  {
		SurfaceInfoType<DayCount> test = SurfaceInfoType.DAY_COUNT;
		assertEquals(test.ToString(), "DayCount");
	  }

	  public virtual void test_MONEYNESS_TYPE()
	  {
		SurfaceInfoType<MoneynessType> test = SurfaceInfoType.MONEYNESS_TYPE;
		assertEquals(test.ToString(), "MoneynessType");
	  }

	  public virtual void coverage()
	  {
		SurfaceInfoType<string> test = SurfaceInfoType.of("Foo");
		assertEquals(test.ToString(), "Foo");
	  }

	}

}