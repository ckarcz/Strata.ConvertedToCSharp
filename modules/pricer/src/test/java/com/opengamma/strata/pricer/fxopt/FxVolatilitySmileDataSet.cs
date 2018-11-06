namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Sets of volatility data used in FX option tests.
	/// </summary>
	public class FxVolatilitySmileDataSet
	{

	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);
	  private static readonly DoubleArray DELTA = DoubleArray.of(0.10, 0.25);

	  private static readonly DoubleArray TIME_5 = DoubleArray.of(0.25205479452054796, 0.5013698630136987, 1.0015120892282356, 2.0, 5.001512089228235);
	  private static readonly DoubleArray ATM_5 = DoubleArray.of(0.185, 0.18, 0.17, 0.16, 0.16);
	  private static readonly DoubleArray ATM_5_FLAT = DoubleArray.of(0.18, 0.18, 0.18, 0.18, 0.18);
	  private static readonly DoubleMatrix RISK_REVERSAL_5 = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.011, -0.006},
		  new double[] {-0.012, -0.007},
		  new double[] {-0.013, -0.008},
		  new double[] {-0.014, -0.009},
		  new double[] {-0.014, -0.009}
	  });
	  private static readonly DoubleMatrix STRANGLE_5 = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0310, 0.0110},
		  new double[] {0.0320, 0.0120},
		  new double[] {0.0330, 0.0130},
		  new double[] {0.0340, 0.0140},
		  new double[] {0.0340, 0.0140}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM_5 = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_5, DELTA, ATM_5, RISK_REVERSAL_5, STRANGLE_5, ACT_ACT_ISDA);

	  private static readonly DoubleArray ATM_5_MRKT = DoubleArray.of(0.0875, 0.0925, 0.0950, 0.0950, 0.0950);
	  private static readonly DoubleMatrix RISK_REVERSAL_5_MRKT = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.0190, -0.0110},
		  new double[] {-0.0205, -0.0115},
		  new double[] {-0.0220, -0.0112},
		  new double[] {-0.0190, -0.0105},
		  new double[] {-0.0190, -0.0105}
	  });
	  private static readonly DoubleMatrix STRANGLE_5_MRKT = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0094, 0.0030},
		  new double[] {0.0110, 0.0035},
		  new double[] {0.0130, 0.0038},
		  new double[] {0.0130, 0.0038},
		  new double[] {0.0130, 0.0038}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM_5_MRKT = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_5, DELTA, ATM_5_MRKT, RISK_REVERSAL_5_MRKT, STRANGLE_5_MRKT, ACT_ACT_ISDA, DOUBLE_QUADRATIC, FLAT, FLAT);

	  private static readonly DoubleMatrix RISK_REVERSAL_5_FLAT = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0}
	  });
	  private static readonly DoubleMatrix STRANGLE_5_FLAT = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0},
		  new double[] {0.0, 0.0}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM_5_FLAT = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_5, DELTA, ATM_5, RISK_REVERSAL_5_FLAT, STRANGLE_5_FLAT, ACT_ACT_ISDA);
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM_5_FLAT_FLAT = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_5, DELTA, ATM_5_FLAT, RISK_REVERSAL_5_FLAT, STRANGLE_5_FLAT, ACT_ACT_ISDA);

	  private static readonly DoubleArray TIME_6 = DoubleArray.of(0.01, 0.252, 0.501, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray ATM_6 = DoubleArray.of(0.175, 0.185, 0.18, 0.17, 0.16, 0.16);
	  private static readonly DoubleMatrix RISK_REVERSAL_6 = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.010, -0.0050},
		  new double[] {-0.011, -0.0060},
		  new double[] {-0.012, -0.0070},
		  new double[] {-0.013, -0.0080},
		  new double[] {-0.014, -0.0090},
		  new double[] {-0.014, -0.0090}
	  });
	  private static readonly DoubleMatrix STRANGLE_6 = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0300, 0.0100},
		  new double[] {0.0310, 0.0110},
		  new double[] {0.0320, 0.0120},
		  new double[] {0.0330, 0.0130},
		  new double[] {0.0340, 0.0140},
		  new double[] {0.0340, 0.0140}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM_6 = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_6, DELTA, ATM_6, RISK_REVERSAL_6, STRANGLE_6, ACT_365F);
	  private static readonly FxOptionVolatilitiesName NAME = FxOptionVolatilitiesName.of("Test");

	  /// <summary>
	  /// Creates volatility provider with term structure of smile parameters.
	  /// <para>
	  /// The number of time slices are 5, and the day count convention is ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <returns>  the volatility provider </returns>
	  public static BlackFxOptionSmileVolatilities createVolatilitySmileProvider5(ZonedDateTime dateTime)
	  {
		return BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, dateTime, SMILE_TERM_5);
	  }

	  /// <summary>
	  /// Creates volatility provider with term structure of smile parameters.
	  /// <para>
	  /// The number of time slices are 5, and the day count convention is ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <returns>  the volatility provider </returns>
	  public static BlackFxOptionSmileVolatilities createVolatilitySmileProvider5Market(ZonedDateTime dateTime)
	  {
		return BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, dateTime, SMILE_TERM_5_MRKT);
	  }

	  /// <summary>
	  /// Creates volatility provider with term structure of smile parameters.
	  /// <para>
	  /// The resulting volatility surface is flat along the strike direction.
	  /// The number of time slices are 5, and the day count convention is ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <returns>  the volatility provider </returns>
	  public static BlackFxOptionSmileVolatilities createVolatilitySmileProvider5Flat(ZonedDateTime dateTime)
	  {
		return BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, dateTime, SMILE_TERM_5_FLAT);
	  }

	  /// <summary>
	  /// Creates volatility provider with term structure of smile parameters.
	  /// <para>
	  /// The resulting volatility surface is totally flat.
	  /// The number of time slices are 5, and the day count convention is ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <returns>  the volatility provider </returns>
	  public static BlackFxOptionSmileVolatilities createVolatilitySmileProvider5FlatFlat(ZonedDateTime dateTime)
	  {
		return BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, dateTime, SMILE_TERM_5_FLAT_FLAT);
	  }

	  /// <summary>
	  /// Creates volatility provider with term structure of smile parameters.
	  /// <para>
	  /// The number of time slices are 6, and the day count convention is ACT/365F.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dateTime">  the valuation date time </param>
	  /// <returns>  the volatility provider </returns>
	  public static BlackFxOptionSmileVolatilities createVolatilitySmileProvider6(ZonedDateTime dateTime)
	  {
		return BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, dateTime, SMILE_TERM_6);
	  }

	  /// <summary>
	  /// Get the underlying smile term structure.
	  /// <para>
	  /// The number of time slices are 5, and the day count convention is ACT/ACT ISDA.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the smile term structure </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure SmileDeltaTermStructure5
	  {
		  get
		  {
			return SMILE_TERM_5;
		  }
	  }

	  /// <summary>
	  /// Get the underlying smile term structure.
	  /// <para>
	  /// The number of time slices are 6, and the day count convention is ACT/365F.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the smile term structure </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure SmileDeltaTermStructure6
	  {
		  get
		  {
			return SMILE_TERM_6;
		  }
	  }

	}

}