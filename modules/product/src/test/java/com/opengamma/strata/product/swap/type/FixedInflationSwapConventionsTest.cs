/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using PriceIndices = com.opengamma.strata.basics.index.PriceIndices;

	/// <summary>
	/// Test <seealso cref="FixedInflationSwapConventions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FixedInflationSwapConventionsTest
	public class FixedInflationSwapConventionsTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "floatLeg") public static Object[][] data_float_leg()
		public static object[][] data_float_leg()
		{
		return new object[][]
		{
			new object[] {FixedInflationSwapConventions.CHF_FIXED_ZC_CH_CPI, PriceIndices.CH_CPI},
			new object[] {FixedInflationSwapConventions.EUR_FIXED_ZC_EU_AI_CPI, PriceIndices.EU_AI_CPI},
			new object[] {FixedInflationSwapConventions.EUR_FIXED_ZC_EU_EXT_CPI, PriceIndices.EU_EXT_CPI},
			new object[] {FixedInflationSwapConventions.EUR_FIXED_ZC_FR_CPI, PriceIndices.FR_EXT_CPI},
			new object[] {FixedInflationSwapConventions.GBP_FIXED_ZC_GB_HCIP, PriceIndices.GB_HICP},
			new object[] {FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPI, PriceIndices.GB_RPI},
			new object[] {FixedInflationSwapConventions.GBP_FIXED_ZC_GB_RPIX, PriceIndices.GB_RPIX},
			new object[] {FixedInflationSwapConventions.JPY_FIXED_ZC_JP_CPI, PriceIndices.JP_CPI_EXF},
			new object[] {FixedInflationSwapConventions.USD_FIXED_ZC_US_CPI, PriceIndices.US_CPI_U}
		};
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "floatLeg") public void test_float_leg(FixedInflationSwapConvention convention, com.opengamma.strata.basics.index.PriceIndex floatLeg)
	  public virtual void test_float_leg(FixedInflationSwapConvention convention, PriceIndex floatLeg)
	  {
		assertEquals(convention.FloatingLeg.Index, floatLeg);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FixedIborSwapConventions));
		coverPrivateConstructor(typeof(StandardFixedIborSwapConventions));
	  }

	}

}