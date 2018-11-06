using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.finance
{

	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Swaption cube data.
	/// </summary>
	internal class SwaptionCubeData
	{

	  /// <summary>
	  /// Normal volatility for EUR - Data from 29-February-2016 </summary>
	  public static readonly LocalDate DATA_DATE = LocalDate.of(2016, 2, 29);
	  public static readonly FixedIborSwapConvention EUR_FIXED_1Y_EURIBOR_6M = FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
	  public static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.0200, -0.0100, -0.0050, -0.0025, 0.0000, 0.0025, 0.0050, 0.0100, 0.0200);
	  public static readonly IList<Period> EXPIRIES = new List<Period>();
	  public static readonly IList<Tenor> TENORS = new List<Tenor>();

	  static SwaptionCubeData()
	  {
		EXPIRIES.Add(Period.ofMonths(1));
		EXPIRIES.Add(Period.ofMonths(3));
		EXPIRIES.Add(Period.ofMonths(6));
		EXPIRIES.Add(Period.ofYears(1));
		EXPIRIES.Add(Period.ofYears(2));
		EXPIRIES.Add(Period.ofYears(5));
		TENORS.Add(Tenor.TENOR_1Y);
		TENORS.Add(Tenor.TENOR_2Y);
		TENORS.Add(Tenor.TENOR_5Y);
	  }

	  public static readonly double[][][] DATA_ARRAY_FULL = new double[][][]
	  {
		  new double[][]
		  {
			  new double[] {0.003998, 0.003145, 0.002245, 0.001741, 0.001394, 0.001781, 0.002393, 0.003589, 0.005804},
			  new double[] {0.004462, 0.003551, 0.002621, 0.002132, 0.001862, 0.002227, 0.002836, 0.004077, 0.006406},
			  new double[] {0.003918, 0.003098, 0.002411, 0.002104, 0.001982, 0.002185, 0.002563, 0.003409, 0.005046},
			  new double[] {0.003859, 0.003247, 0.002749, 0.002568, 0.002532, 0.002689, 0.00298, 0.003698, 0.005188},
			  new double[] {0.004848, 0.004276, 0.003843, 0.003722, 0.003738, 0.003913, 0.004212, 0.004986, 0.006688},
			  new double[] {0.005923, 0.006168, 0.006307, 0.006397, 0.006505, 0.00663, 0.00677, 0.007095, 0.007873}
		  },
		  new double[][]
		  {
			  new double[] {0.00471, 0.003704, 0.002744, 0.002253, 0.00201, 0.002384, 0.002995, 0.004249, 0.006605},
			  new double[] {0.004962, 0.003925, 0.002964, 0.002492, 0.00228, 0.002634, 0.003233, 0.004496, 0.006891},
			  new double[] {0.00446, 0.003534, 0.002833, 0.002542, 0.002439, 0.002627, 0.002993, 0.003854, 0.005565},
			  new double[] {0.004485, 0.003779, 0.003279, 0.003112, 0.003086, 0.003233, 0.00351, 0.004227, 0.005766},
			  new double[] {0.005405, 0.004738, 0.004308, 0.004196, 0.004217, 0.004389, 0.004682, 0.005457, 0.007197},
			  new double[] {0.005993, 0.006223, 0.006366, 0.006459, 0.006568, 0.006694, 0.006835, 0.00716, 0.007933}
		  },
		  new double[][]
		  {
			  new double[] {0.004347, 0.003809, 0.003197, 0.002959, 0.002945, 0.00325, 0.003744, 0.004882, 0.007179},
			  new double[] {0.004648, 0.00427, 0.003745, 0.00358, 0.003633, 0.003958, 0.004459, 0.005644, 0.008099},
			  new double[] {0.004695, 0.004414, 0.004025, 0.003942, 0.004034, 0.004325, 0.00476, 0.005812, 0.008058},
			  new double[] {0.00454, 0.004436, 0.004312, 0.004344, 0.004474, 0.004707, 0.00502, 0.005789, 0.007517},
			  new double[] {0.005106, 0.005107, 0.005145, 0.005224, 0.005351, 0.005527, 0.005745, 0.006278, 0.007537},
			  new double[] {0.00657, 0.006702, 0.006825, 0.006911, 0.007016, 0.00714, 0.007281, 0.00761, 0.008408}
		  }
	  };
	  public static readonly double[][][] DATA_ARRAY_SPARSE = new double[][][]
	  {
		  new double[][]
		  {
			  new double[] {Double.NaN, Double.NaN, 0.002245, 0.001741, 0.001394, 0.001781, 0.002393, Double.NaN, Double.NaN},
			  new double[] {Double.NaN, 0.003551, 0.002621, 0.002132, 0.001862, 0.002227, 0.002836, 0.004077, Double.NaN},
			  new double[] {0.003918, 0.003098, 0.002411, 0.002104, 0.001982, 0.002185, 0.002563, 0.003409, 0.005046},
			  new double[] {0.003859, 0.003247, Double.NaN, 0.002568, 0.002532, 0.002689, 0.00298, 0.003698, 0.005188},
			  new double[] {0.004848, 0.004276, 0.003843, 0.003722, 0.003738, 0.003913, 0.004212, 0.004986, 0.006688},
			  new double[] {0.005923, 0.006168, 0.006307, 0.006397, 0.006505, 0.00663, 0.00677, 0.007095, 0.007873}
		  },
		  new double[][]
		  {
			  new double[] {Double.NaN, 0.003704, 0.002744, 0.002253, 0.00201, 0.002384, 0.002995, Double.NaN, 0.006605},
			  new double[] {0.004962, 0.003925, 0.002964, 0.002492, 0.00228, 0.002634, 0.003233, 0.004496, Double.NaN},
			  new double[] {0.00446, 0.003534, 0.002833, 0.002542, 0.002439, 0.002627, 0.002993, 0.003854, 0.005565},
			  new double[] {0.004485, Double.NaN, 0.003279, 0.003112, 0.003086, 0.003233, 0.00351, 0.004227, 0.005766},
			  new double[] {0.005405, 0.004738, 0.004308, 0.004196, 0.004217, 0.004389, 0.004682, 0.005457, 0.007197},
			  new double[] {0.005993, 0.006223, 0.006366, 0.006459, 0.006568, 0.006694, 0.006835, 0.00716, 0.007933}
		  },
		  new double[][]
		  {
			  new double[] {Double.NaN, 0.003809, 0.003197, 0.002959, 0.002945, 0.00325, 0.003744, 0.004882, Double.NaN},
			  new double[] {Double.NaN, Double.NaN, 0.003745, Double.NaN, 0.003633, Double.NaN, 0.004459, Double.NaN, Double.NaN},
			  new double[] {0.004695, 0.004414, 0.004025, 0.003942, 0.004034, 0.004325, 0.00476, 0.005812, 0.008058},
			  new double[] {0.00454, 0.004436, 0.004312, 0.004344, 0.004474, 0.004707, 0.00502, 0.005789, 0.007517},
			  new double[] {0.005106, 0.005107, 0.005145, 0.005224, 0.005351, 0.005527, 0.005745, 0.006278, 0.007537},
			  new double[] {0.00657, 0.006702, 0.006825, 0.006911, 0.007016, 0.00714, 0.007281, Double.NaN, 0.008408}
		  }
	  };

	}

}