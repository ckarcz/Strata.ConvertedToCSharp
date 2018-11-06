using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Caplet stripping setup.
	/// </summary>
	public class CapletStrippingSetup
	{

	  protected internal static readonly BlackIborCapFloorLegPricer LEG_PRICER_BLACK = BlackIborCapFloorLegPricer.DEFAULT;
	  protected internal static readonly NormalIborCapFloorLegPricer LEG_PRICER_NORMAL = NormalIborCapFloorLegPricer.DEFAULT;
	  protected internal static readonly SabrIborCapFloorLegPricer LEG_PRICER_SABR = SabrIborCapFloorLegPricer.DEFAULT;

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly double[] FWD_CURVE_NODES = new double[] {0.0438356164383561, 0.0876712328767123, 0.172602739726027, 0.254794520547945, 0.506849315068493, 0.758904109589041, 1.00547945205479, 2.01369863013698, 3.01020285949547, 4.00547945205479, 5.00547945205479, 6.00547945205479, 7.01839958080694, 8.01095890410959, 9.00821917808219, 10.0082191780821, 15.0074706190583, 20.0082191780821, 25.0109589041095, 30.0136986301369};
	  private static readonly double[] FWD_CURVE_VALUES = new double[] {0.00184088091044285, 0.00201024117395892, 0.00241264832694067, 0.00280755413825359, 0.0029541307818572, 0.00310125437814943, 0.00320054435838637, 0.00377914611772073, 0.00483320020067661, 0.00654829256979543, 0.00877749583222556, 0.0112470678648412, 0.0136301644164456, 0.0157618031582798, 0.0176836551757772, 0.0194174141169365, 0.0254011614777518, 0.0282527762712854, 0.0298620063409043, 0.031116719228976};
	  private static readonly CurveMetadata FWD_META = Curves.zeroRates("fwdCurve", ACT_ACT_ISDA);
	  protected internal static readonly InterpolatedNodalCurve FWD_CURVE = InterpolatedNodalCurve.of(FWD_META, DoubleArray.ofUnsafe(FWD_CURVE_NODES), DoubleArray.ofUnsafe(FWD_CURVE_VALUES), CurveInterpolators.LINEAR, CurveExtrapolators.LINEAR, CurveExtrapolators.LINEAR);
	  private static readonly double[] DIS_CURVE_NODES = new double[] {0.00273972602739726, 0.0876712328767123, 0.172602739726027, 0.254794520547945, 0.345205479452054, 0.424657534246575, 0.506849315068493, 0.758904109589041, 1.00547945205479, 2.00547945205479, 3.01020285949547, 4.00547945205479, 5.00547945205479, 10.0054794520547};
	  private static readonly double[] DIS_CURVE_VALUES = new double[] {0.00212916045658802, 0.00144265912946933, 0.00144567477491987, 0.00135441424749791, 0.00134009103595346, 0.00132773752749976, 0.00127592397233014, 0.00132302501180961, 0.00138688847322639, 0.00172748279241698, 0.00254381216780551, 0.00410024606039574, 0.00628782387356631, 0.0170033466745807};
	  private static readonly CurveMetadata DIS_META = Curves.zeroRates("dscCurve", ACT_ACT_ISDA);
	  private static readonly InterpolatedNodalCurve DIS_CURVE = InterpolatedNodalCurve.of(DIS_META, DoubleArray.ofUnsafe(DIS_CURVE_NODES), DoubleArray.ofUnsafe(DIS_CURVE_VALUES), CurveInterpolators.DOUBLE_QUADRATIC, CurveExtrapolators.LINEAR, CurveExtrapolators.LINEAR);
	  protected internal static readonly LocalDate CALIBRATION_DATE = LocalDate.of(2016, 3, 3);
	  protected internal static readonly ZonedDateTime CALIBRATION_TIME = CALIBRATION_DATE.atTime(10, 0).atZone(ZoneId.of("America/New_York"));
	  protected internal static readonly ImmutableRatesProvider RATES_PROVIDER = ImmutableRatesProvider.builder(CALIBRATION_DATE).discountCurve(USD, DIS_CURVE).iborIndexCurve(USD_LIBOR_3M, FWD_CURVE).build();

	  private static readonly double[] CAP_BLACK_STRIKES = new double[] {0.005, 0.01, 0.015, 0.02, 0.025, 0.03, 0.035, 0.04, 0.045, 0.05, 0.055, 0.06, 0.07, 0.08, 0.09, 0.1, 0.11, 0.12};
	  private static readonly int[] CAP_BLACK_END_TIMES = new int[] {1, 2, 3, 4, 5, 7, 10};
	  private static readonly int NUM_BLACK_MATURITIES = CAP_BLACK_END_TIMES.Length;
	  protected internal static readonly int NUM_BLACK_STRIKES = CAP_BLACK_STRIKES.Length;
	  private static readonly double[] CAP_NORMAL_STRIKES = new double[] {0.0025, 0.005, 0.0075, 0.01, 0.015, 0.02, 0.025, 0.03, 0.035, 0.04, 0.045, 0.05};
	  private static readonly int[] CAP_NORMAL_END_TIMES = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 15, 20};
	  private static readonly int NUM_NORMAL_MATURITIES = CAP_NORMAL_END_TIMES.Length;
	  protected internal static readonly int NUM_NORMAL_STRIKES = CAP_NORMAL_STRIKES.Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: private static readonly ResolvedIborCapFloorLeg[][] CAPS_BLACK = new ResolvedIborCapFloorLeg[NUM_BLACK_STRIKES][NUM_BLACK_MATURITIES];
	  private static readonly ResolvedIborCapFloorLeg[][] CAPS_BLACK = RectangularArrays.ReturnRectangularResolvedIborCapFloorLegArray(NUM_BLACK_STRIKES, NUM_BLACK_MATURITIES);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: private static readonly ResolvedIborCapFloorLeg[][] CAPS_NORMAL = new ResolvedIborCapFloorLeg[NUM_NORMAL_STRIKES][NUM_NORMAL_MATURITIES];
	  private static readonly ResolvedIborCapFloorLeg[][] CAPS_NORMAL = RectangularArrays.ReturnRectangularResolvedIborCapFloorLegArray(NUM_NORMAL_STRIKES, NUM_NORMAL_MATURITIES);
	  private static readonly LocalDate BASE_DATE = USD_LIBOR_3M.EffectiveDateOffset.adjust(CALIBRATION_DATE, REF_DATA);
	  static CapletStrippingSetup()
	  {
		LocalDate startDate = BASE_DATE.plus(USD_LIBOR_3M.Tenor);
		for (int i = 0; i < NUM_BLACK_MATURITIES; ++i)
		{
		  for (int j = 0; j < NUM_BLACK_STRIKES; ++j)
		  {
			CAPS_BLACK[j][i] = IborCapFloorLeg.builder().calculation(IborRateCalculation.of(USD_LIBOR_3M)).capSchedule(ValueSchedule.of(CAP_BLACK_STRIKES[j])).notional(ValueSchedule.ALWAYS_1).paymentSchedule(PeriodicSchedule.of(startDate, BASE_DATE.plusYears(CAP_BLACK_END_TIMES[i]), Frequency.P3M, BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, USD_LIBOR_3M.FixingCalendar), StubConvention.NONE, RollConventions.NONE)).payReceive(PayReceive.RECEIVE).build().resolve(REF_DATA);
		  }
		}
		for (int i = 0; i < NUM_NORMAL_MATURITIES; ++i)
		{
		  for (int j = 0; j < NUM_NORMAL_STRIKES; ++j)
		  {
			CAPS_NORMAL[j][i] = IborCapFloorLeg.builder().calculation(IborRateCalculation.of(USD_LIBOR_3M)).capSchedule(ValueSchedule.of(CAP_NORMAL_STRIKES[j])).notional(ValueSchedule.ALWAYS_1).paymentSchedule(PeriodicSchedule.of(startDate, BASE_DATE.plusYears(CAP_NORMAL_END_TIMES[i]), Frequency.P3M, BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, USD_LIBOR_3M.FixingCalendar), StubConvention.NONE, RollConventions.NONE)).payReceive(PayReceive.RECEIVE).build().resolve(REF_DATA);
		  }
		}
	  }

	  // cap vol at each strike - NaN represents missing data
	  private static readonly double[][] CAP_BLACK_VOLS = new double[][]
	  {
		  new double[] {0.7175, 0.7781, 0.8366, Double.NaN, 0.8101, 0.7633, 0.714},
		  new double[] {Double.NaN, Double.NaN, 0.7523, 0.7056, 0.66095, 0.5933, 0.5313},
		  new double[] {Double.NaN, 0.78086, 0.73987, 0.667, 0.61469, 0.53502, 0.4691},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.63455, 0.56975, 0.48235, 0.4369},
		  new double[] {Double.NaN, 0.80496, 0.73408, 0.6081, 0.5472, 0.46445, 0.38854},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.61055, 0.52865, 0.432, 0.365},
		  new double[] {0.96976, 0.82268, 0.73761, Double.NaN, 0.5168, 0.4183, 0.35485},
		  new double[] {0.98927, 0.8376, 0.7274, 0.5983, 0.5169, 0.4083, 0.3375},
		  new double[] {1.00627, 0.83618, Double.NaN, 0.6003, Double.NaN, Double.NaN, 0.34498},
		  new double[] {1.02132, 0.8391, 0.7226, 0.5921, 0.4984, 0.3914, 0.32155},
		  new double[] {1.0255, 0.8406, 0.7196, 0.5962, 0.5035, 0.3873, 0.3227},
		  new double[] {1.0476, 0.8411, 0.7072, 0.58845, 0.50055, 0.3856, 0.3135},
		  new double[] {1.0467, Double.NaN, 0.70655, 0.5855, 0.50165, 0.3824, 0.3093},
		  new double[] {Double.NaN, Double.NaN, 0.70495, 0.58455, 0.499, 0.3802, 0.316},
		  new double[] {Double.NaN, 0.8458, 0.70345, 0.58335, 0.4984, 0.38905, 0.3164},
		  new double[] {Double.NaN, 0.8489, 0.70205, 0.58245, 0.4999, 0.39155, 0.3239},
		  new double[] {Double.NaN, Double.NaN, 0.7009, 0.5824, 0.5059, 0.4005, 0.3255},
		  new double[] {Double.NaN, Double.NaN, 0.6997, 0.5818, 0.5059, 0.4014, 0.3271}
	  };
	  private static readonly double[][] CAP_BLACK_VOLS_INVALID = new double[][]
	  {
		  new double[] {0.7175, Double.NaN, 0.8366, Double.NaN, 0.8101, 0.7633, 0.714},
		  new double[] {Double.NaN, Double.NaN, 0.7523, 0.7056, 0.66095, 0.5933, 0.5313},
		  new double[] {Double.NaN, Double.NaN, 0.73987, 0.667, 0.61469, 0.53502, 0.4691},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.63455, 0.56975, 0.48235, 0.4369},
		  new double[] {Double.NaN, Double.NaN, 0.73408, 0.6081, 0.5472, 0.46445, 0.38854},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.61055, 0.52865, 0.432, 0.365},
		  new double[] {0.96976, Double.NaN, 0.73761, Double.NaN, 0.5168, 0.4183, 0.35485},
		  new double[] {0.98927, Double.NaN, 0.7274, 0.5983, 0.5169, 0.4083, 0.3375},
		  new double[] {1.00627, Double.NaN, Double.NaN, 0.6003, Double.NaN, Double.NaN, 0.34498},
		  new double[] {1.02132, Double.NaN, 0.7226, 0.5921, 0.4984, 0.3914, 0.32155},
		  new double[] {1.0255, Double.NaN, 0.7196, 0.5962, 0.5035, 0.3873, 0.3227},
		  new double[] {1.0476, Double.NaN, 0.7072, 0.58845, 0.50055, 0.3856, 0.3135},
		  new double[] {1.0467, Double.NaN, 0.70655, 0.5855, 0.50165, 0.3824, 0.3093},
		  new double[] {Double.NaN, Double.NaN, 0.70495, 0.58455, 0.499, 0.3802, 0.316},
		  new double[] {Double.NaN, Double.NaN, 0.70345, 0.58335, 0.4984, 0.38905, 0.3164},
		  new double[] {Double.NaN, Double.NaN, 0.70205, 0.58245, 0.4999, 0.39155, 0.3239},
		  new double[] {Double.NaN, Double.NaN, 0.7009, 0.5824, 0.5059, 0.4005, 0.3255},
		  new double[] {Double.NaN, Double.NaN, 0.6997, 0.5818, 0.5059, 0.4014, 0.3271}
	  };
	  private static readonly double[][] CAP_NORMAL_VOLS = new double[][]
	  {
		  new double[] {0.5463, 0.5827, 0.5589, 0.5339, 0.5224, 0.5685, 0.6396, 0.7184, 0.8029, 0.8908, 0.9804, 1.0705, 1.1608},
		  new double[] {0.6679, 0.7129, 0.6801, 0.6483, 0.6308, 0.6654, 0.7263, 0.7939, 0.8673, 0.9448, 1.0249, 1.1062, 1.1881},
		  new double[] {0.7402, 0.78, 0.7501, 0.7206, 0.7024, 0.7249, 0.7727, 0.8272, 0.8876, 0.9525, 1.0206, 1.0907, 1.1619},
		  new double[] {0.7864, 0.8236, 0.7962, 0.7693, 0.7514, 0.7649, 0.8019, 0.8454, 0.8943, 0.9479, 1.0051, 1.0649, 1.1264},
		  new double[] {0.8147, 0.8509, 0.8261, 0.8015, 0.7844, 0.7917, 0.8203, 0.8551, 0.8948, 0.9388, 0.9863, 1.0366, 1.0889},
		  new double[] {0.8318, 0.8676, 0.845, 0.8226, 0.8064, 0.8093, 0.8319, 0.8608, 0.8948, 0.9334, 0.9759, 1.0215, 1.0693},
		  new double[] {0.8354, 0.8722, 0.8514, 0.8308, 0.8154, 0.8151, 0.8324, 0.8562, 0.8851, 0.9189, 0.957, 0.9985, 1.0426},
		  new double[] {0.8374, 0.8753, 0.8559, 0.8368, 0.822, 0.8193, 0.8328, 0.8528, 0.8784, 0.9091, 0.9445, 0.9837, 1.0256},
		  new double[] {0.8385, 0.8776, 0.8593, 0.8412, 0.8271, 0.8225, 0.8331, 0.8503, 0.8734, 0.9021, 0.9357, 0.9733, 1.014},
		  new double[] {0.818, 0.8549, 0.8395, 0.8242, 0.8119, 0.8057, 0.8122, 0.8252, 0.8442, Double.NaN, 0.9, Double.NaN, 0.973},
		  new double[] {0.799, 0.8346, 0.8217, 0.8087, 0.7979, 0.7904, 0.7935, 0.803, 0.8192, 0.842, 0.8708, 0.9041, 0.9409},
		  new double[] {0.7555, 0.7827, 0.7733, 0.7638, 0.7557, 0.749, 0.7502, 0.7577, 0.7721, 0.7935, 0.821, 0.8532, 0.8887}
	  };
	  private static readonly double[][] CAP_NORMAL_EQUIV_VOLS = new double[][]
	  {
		  new double[] {0.0029237822734902524, 0.0034136169936817033, 0.004111899099433724, Double.NaN, 0.005158919846139429, 0.005880028654583701, 0.006602746804835127},
		  new double[] {Double.NaN, Double.NaN, 0.005555184661720685, 0.006060308783872109, 0.0064681825858204185, 0.007001834697977297, 0.007358023852028733},
		  new double[] {Double.NaN, 0.006482680497799072, 0.0069882805083619345, 0.007385823271192304, 0.007781995222353636, 0.00817176527480969, 0.008305654885373529},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.008463042042139829, 0.008721826257219168, 0.00890430941028613, 0.009204081837445803},
		  new double[] {Double.NaN, 0.009203852197689514, 0.009553295146839048, 0.009395277309796696, 0.009686438334724271, 0.009868950606606874, 0.00945527391744324},
		  new double[] {Double.NaN, Double.NaN, Double.NaN, 0.010577695837496428, 0.01054951965092822, 0.010372321165352374, 0.009951221255227807},
		  new double[] {0.012850988453383937, 0.011715502361745867, 0.011927433447893793, Double.NaN, 0.011406457995250105, 0.011085647553150682, 0.010618460011235616},
		  new double[] {0.014345934996892178, 0.013026457768347524, 0.012872814762320993, 0.012511992391841475, 0.012411501522441871, 0.011783024185340241, 0.010987968181224094},
		  new double[] {0.015814601727181077, 0.01409230561581168, Double.NaN, 0.013545642107440487, Double.NaN, Double.NaN, 0.011999970920509464},
		  new double[] {0.0172609696455313, 0.015198228467757167, 0.014862741487700476, 0.014349870473796244, 0.013886237101534537, 0.013037525141382612, 0.012007974059737487},
		  new double[] {0.01852996833366485, 0.016262287516603396, 0.015798297092384346, 0.01536970407073983, 0.014889560180401738, 0.013705316464075669, 0.012746046992219326},
		  new double[] {0.020108449603030307, 0.017290371749777253, 0.016515852031022907, 0.016107954766077858, 0.015676079961874777, 0.014412163759150191, 0.013087486389562407},
		  new double[] {0.02243813236022633, Double.NaN, 0.018351490528564194, 0.017785737349513116, 0.017367352778322714, 0.015758634740652772, 0.014188523203432134},
		  new double[] {Double.NaN, Double.NaN, 0.020100151305047054, 0.019441849545595233, 0.018882166214436967, 0.017060460613144583, 0.015683090275069138},
		  new double[] {Double.NaN, 0.023194060100211675, 0.021794416400983882, 0.021034125786394464, 0.020400475279279406, 0.018757169214515566, 0.01687872530111318},
		  new double[] {Double.NaN, 0.025120151928656194, 0.023442818163966796, 0.022587053666820456, 0.021949259948182788, 0.02018230434867113, 0.0183957935728747},
		  new double[] {Double.NaN, Double.NaN, 0.025056683365416332, 0.024128415181588148, 0.023645308722883313, 0.021888760133392547, 0.019611322604198053},
		  new double[] {Double.NaN, Double.NaN, 0.026632254625197683, 0.02561633184998322, 0.02508697262418348, 0.023212771122734382, 0.020807499010350925}
	  };

	  //-------------------------------------------------------------------------
	  protected internal static DoubleMatrix createFullBlackDataMatrix()
	  {
		DoubleMatrix matrix = DoubleMatrix.ofUnsafe(CAP_BLACK_VOLS);
		return matrix.transpose();
	  }

	  protected internal static DoubleMatrix createFullBlackDataMatrixInvalid()
	  {
		DoubleMatrix matrix = DoubleMatrix.ofUnsafe(CAP_BLACK_VOLS_INVALID);
		return matrix.transpose();
	  }

	  protected internal static DoubleMatrix createBlackDataMatrixForStrike(int strikeIndex)
	  {
		int nExpiry = CAP_BLACK_VOLS[0].Length;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[nExpiry][1];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(nExpiry, 1);
		for (int i = 0; i < nExpiry; ++i)
		{
		  res[i][0] = CAP_BLACK_VOLS[strikeIndex][i];
		}
		return DoubleMatrix.ofUnsafe(res);
	  }

	  protected internal static DoubleMatrix createFullFlatBlackDataMatrix()
	  {
		DoubleMatrix matrix = DoubleMatrix.filled(NUM_BLACK_STRIKES, NUM_BLACK_MATURITIES, 0.5);
		return matrix.transpose();
	  }

	  protected internal static ImmutableList<Period> createBlackMaturities()
	  {
		ImmutableList.Builder<Period> builder = ImmutableList.builder();
		for (int i = 0; i < NUM_BLACK_MATURITIES; ++i)
		{
		  builder.add(Period.ofYears(CAP_BLACK_END_TIMES[i]));
		}
		return builder.build();
	  }

	  protected internal static DoubleArray createBlackStrikes()
	  {
		return DoubleArray.copyOf(CAP_BLACK_STRIKES);
	  }

	  //-------------------------------------------------------------------------
	  protected internal static DoubleMatrix createFullNormalDataMatrix()
	  {
		DoubleMatrix matrix = DoubleMatrix.ofUnsafe(CAP_NORMAL_VOLS);
		return matrix.transpose();
	  }

	  protected internal static ImmutableList<Period> createNormalMaturities()
	  {
		ImmutableList.Builder<Period> builder = ImmutableList.builder();
		for (int i = 0; i < NUM_NORMAL_MATURITIES; ++i)
		{
		  builder.add(Period.ofYears(CAP_NORMAL_END_TIMES[i]));
		}
		return builder.build();
	  }

	  protected internal static DoubleArray createNormalStrikes()
	  {
		return DoubleArray.copyOf(CAP_NORMAL_STRIKES);
	  }

	  //-------------------------------------------------------------------------
	  protected internal static DoubleMatrix createFullNormalEquivDataMatrix()
	  {
		DoubleMatrix matrix = DoubleMatrix.ofUnsafe(CAP_NORMAL_EQUIV_VOLS);
		return matrix.transpose();
	  }

	  protected internal static ImmutableList<Period> createNormalEquivMaturities()
	  {
		ImmutableList.Builder<Period> builder = ImmutableList.builder();
		for (int i = 0; i < NUM_BLACK_MATURITIES; ++i)
		{
		  builder.add(Period.ofYears(CAP_BLACK_END_TIMES[i]));
		}
		return builder.build();
	  }

	  protected internal static DoubleArray createNormalEquivStrikes()
	  {
		return DoubleArray.copyOf(CAP_BLACK_STRIKES);
	  }

	  //-------------------------------------------------------------------------
	  protected internal static Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> getCapsBlackVols(int strikeIndex)
	  {
		ResolvedIborCapFloorLeg[] caps = CAPS_BLACK[strikeIndex];
		double[] vols = CAP_BLACK_VOLS[strikeIndex];
		ImmutableList.Builder<ResolvedIborCapFloorLeg> capBuilder = ImmutableList.builder();
		ImmutableList.Builder<double> volBuilder = ImmutableList.builder();
		int nVols = vols.Length;
		for (int i = 0; i < nVols; ++i)
		{
		  if (Double.isFinite(vols[i]))
		  {
			capBuilder.add(caps[i]);
			volBuilder.add(vols[i]);
		  }
		}
		return Pair.of(capBuilder.build(), volBuilder.build());
	  }

	  protected internal static Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> getCapsFlatBlackVols(int strikeIndex)
	  {
		ResolvedIborCapFloorLeg[] caps = CAPS_BLACK[strikeIndex];
		double[] vols = createFullFlatBlackDataMatrix().columnArray(strikeIndex);
		ImmutableList.Builder<ResolvedIborCapFloorLeg> capBuilder = ImmutableList.builder();
		ImmutableList.Builder<double> volBuilder = ImmutableList.builder();
		int nVols = vols.Length;
		for (int i = 0; i < nVols; ++i)
		{
		  if (Double.isFinite(vols[i]))
		  {
			capBuilder.add(caps[i]);
			volBuilder.add(vols[i]);
		  }
		}
		return Pair.of(capBuilder.build(), volBuilder.build());
	  }

	  protected internal static Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> getCapsNormalVols(int strikeIndex)
	  {
		ResolvedIborCapFloorLeg[] caps = CAPS_NORMAL[strikeIndex];
		double[] vols = CAP_NORMAL_VOLS[strikeIndex];
		ImmutableList.Builder<ResolvedIborCapFloorLeg> capBuilder = ImmutableList.builder();
		ImmutableList.Builder<double> volBuilder = ImmutableList.builder();
		int nVols = vols.Length;
		for (int i = 0; i < nVols; ++i)
		{
		  if (Double.isFinite(vols[i]))
		  {
			capBuilder.add(caps[i]);
			volBuilder.add(vols[i]);
		  }
		}
		return Pair.of(capBuilder.build(), volBuilder.build());
	  }

	  protected internal static Pair<IList<ResolvedIborCapFloorLeg>, IList<double>> getCapsNormalEquivVols(int strikeIndex)
	  {
		ResolvedIborCapFloorLeg[] caps = CAPS_BLACK[strikeIndex];
		double[] vols = CAP_NORMAL_EQUIV_VOLS[strikeIndex];
		ImmutableList.Builder<ResolvedIborCapFloorLeg> capBuilder = ImmutableList.builder();
		ImmutableList.Builder<double> volBuilder = ImmutableList.builder();
		int nVols = vols.Length;
		for (int i = 0; i < nVols; ++i)
		{
		  if (Double.isFinite(vols[i]))
		  {
			capBuilder.add(caps[i]);
			volBuilder.add(vols[i]);
		  }
		}
		return Pair.of(capBuilder.build(), volBuilder.build());
	  }

	  //-------------------------------------------------------------------------
	  // print for debugging
	  protected internal virtual void print(IborCapletFloorletVolatilityCalibrationResult res, DoubleArray strikes, double maxTime)
	  {
		Console.WriteLine("Print in CapletStrippingSetup \n");
		Console.WriteLine("Chi-square: " + res.ChiSquare);
		IborCapletFloorletVolatilities vols = res.Volatilities;
		const int nSamples = 51;
		const int nStrikeSamples = 51;
		Console.Write("\n");
		for (int i = 0; i < nStrikeSamples; i++)
		{
		  Console.Write("\t" + (strikes.get(0) + (strikes.get(strikes.size() - 1) - strikes.get(0)) * i) / (nStrikeSamples - 1));
		}
		Console.Write("\n");
		for (int index = 0; index < nSamples; index++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double t = 0.25 + index * maxTime / (nSamples - 1);
		  double t = 0.25 + index * maxTime / (nSamples - 1);
		  double forward = FWD_CURVE.yValue(t);
		  Console.Write(t);
		  for (int i = 0; i < nStrikeSamples; i++)
		  {
			double strike = (strikes.get(0) + (strikes.get(strikes.size() - 1) - strikes.get(0)) * i) / (nStrikeSamples - 1);
			Console.Write("\t" + vols.volatility(t, strike, forward));
		  }
		  Console.Write("\n");
		}
	  }

	}

}