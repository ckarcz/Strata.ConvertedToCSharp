/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="CurveInfoType"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveInfoTypeTest
	public class CurveInfoTypeTest
	{

	  public virtual void test_DAY_COUNT()
	  {
		CurveInfoType<DayCount> test = CurveInfoType.DAY_COUNT;
		assertEquals(test.ToString(), "DayCount");
	  }

	  public virtual void test_JACOBIAN()
	  {
		CurveInfoType<JacobianCalibrationMatrix> test = CurveInfoType.JACOBIAN;
		assertEquals(test.ToString(), "Jacobian");
	  }

	  public virtual void test_COMPOUNDING_PER_YEAR()
	  {
		CurveInfoType<int> test = CurveInfoType.COMPOUNDING_PER_YEAR;
		assertEquals(test.ToString(), "CompoundingPerYear");
	  }

	  public virtual void test_PV_SENSITIVITY_TO_MARKET_QUOTE()
	  {
		CurveInfoType<DoubleArray> test = CurveInfoType.PV_SENSITIVITY_TO_MARKET_QUOTE;
		assertEquals(test.ToString(), "PVSensitivityToMarketQuote");
	  }

	  public virtual void test_CDS_INDEX_FACTOR()
	  {
		CurveInfoType<double> test = CurveInfoType.CDS_INDEX_FACTOR;
		assertEquals(test.ToString(), "CdsIndexFactor");
	  }

	  public virtual void coverage()
	  {
		CurveInfoType<string> test = CurveInfoType.of("Foo");
		assertEquals(test.ToString(), "Foo");
	  }

	}

}