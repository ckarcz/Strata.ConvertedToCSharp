using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.util.Epsilon.epsilon;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.math.impl.util.Epsilon.epsilonP;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using MathException = com.opengamma.strata.math.MathException;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using BrentSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.BrentSingleRootFinder;
	using RealSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RealSingleRootFinder;
	using PriceType = com.opengamma.strata.pricer.common.PriceType;
	using CreditCouponPaymentPeriod = com.opengamma.strata.product.credit.CreditCouponPaymentPeriod;
	using ResolvedCds = com.opengamma.strata.product.credit.ResolvedCds;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;

	/// <summary>
	/// Fast credit curve calibrator.
	/// <para>
	/// This is a fast bootstrapper for the credit curve that is consistent with ISDA 
	/// in that it will produce the same curve from the same inputs (up to numerical round-off).
	/// </para>
	/// <para>
	/// The CDS pricer is internally implemented for fast calibration.
	/// </para>
	/// </summary>
	public sealed class FastCreditCurveCalibrator : IsdaCompliantCreditCurveCalibrator
	{

	  /// <summary>
	  /// The default implementation.
	  /// </summary>
	  private static readonly FastCreditCurveCalibrator STANDARD = new FastCreditCurveCalibrator();

	  /// <summary>
	  /// The root bracket finder.
	  /// </summary>
	  private static readonly BracketRoot BRACKETER = new BracketRoot();
	  /// <summary>
	  /// The root finder.
	  /// </summary>
	  private static readonly RealSingleRootFinder ROOTFINDER = new BrentSingleRootFinder();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the standard calibrator.
	  /// <para>
	  /// The original ISDA accrual-on-default formula (version 1.8.2 and lower) is used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard calibrator </returns>
	  public static FastCreditCurveCalibrator standard()
	  {
		return FastCreditCurveCalibrator.STANDARD;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Constructs a default credit curve builder. 
	  /// <para>
	  /// The original ISDA accrual-on-default formula (version 1.8.2 and lower) and the arbitrage handling 'ignore' are used.
	  /// </para>
	  /// </summary>
	  private FastCreditCurveCalibrator() : base()
	  {
	  }

	  /// <summary>
	  /// Constructs a credit curve builder with the accrual-on-default formula specified.
	  /// <para>
	  /// The arbitrage handling 'ignore' is used. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="formula">  the accrual-on-default formula </param>
	  public FastCreditCurveCalibrator(AccrualOnDefaultFormula formula) : base(formula)
	  {
	  }

	  /// <summary>
	  /// Constructs a credit curve builder with accrual-on-default formula and arbitrage handing specified.
	  /// </summary>
	  /// <param name="formula">  the accrual on default formulae </param>
	  /// <param name="arbHandling">  the arbitrage handling </param>
	  public FastCreditCurveCalibrator(AccrualOnDefaultFormula formula, ArbitrageHandling arbHandling) : base(formula, arbHandling)
	  {
	  }

	  //-------------------------------------------------------------------------
	  public override NodalCurve calibrate(IList<ResolvedCdsTrade> calibrationCDSs, DoubleArray flactionalSpreads, DoubleArray pointsUpfront, CurveName name, LocalDate valuationDate, CreditDiscountFactors discountFactors, RecoveryRates recoveryRates, ReferenceData refData)
	  {

		int n = calibrationCDSs.Count;
		double[] guess = new double[n];
		double[] t = new double[n];
		double[] lgd = new double[n];
		for (int i = 0; i < n; i++)
		{
		  LocalDate endDate = calibrationCDSs[i].Product.ProtectionEndDate;
		  t[i] = discountFactors.relativeYearFraction(endDate);
		  lgd[i] = 1d - recoveryRates.recoveryRate(endDate);
		  guess[i] = (flactionalSpreads.get(i) + pointsUpfront.get(i) / t[i]) / lgd[i];
		}
		DoubleArray times = DoubleArray.ofUnsafe(t);
		CurveMetadata baseMetadata = DefaultCurveMetadata.builder().xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).curveName(name).dayCount(discountFactors.DayCount).build();
		NodalCurve creditCurve = n == 1 ? ConstantNodalCurve.of(baseMetadata, t[0], guess[0]) : InterpolatedNodalCurve.of(baseMetadata, times, DoubleArray.ofUnsafe(guess), CurveInterpolators.PRODUCT_LINEAR, CurveExtrapolators.FLAT, CurveExtrapolators.PRODUCT_LINEAR);

		for (int i = 0; i < n; i++)
		{
		  ResolvedCds cds = calibrationCDSs[i].Product;
		  LocalDate stepinDate = cds.StepinDateOffset.adjust(valuationDate, refData);
		  LocalDate effectiveStartDate = cds.calculateEffectiveStartDate(stepinDate);
		  LocalDate settlementDate = calibrationCDSs[i].Info.SettlementDate.orElse(cds.SettlementDateOffset.adjust(valuationDate, refData));
		  double accrued = cds.accruedYearFraction(stepinDate);

		  Pricer pricer = new Pricer(this, cds, discountFactors, times, flactionalSpreads.get(i), pointsUpfront.get(i), lgd[i], stepinDate, effectiveStartDate, settlementDate, accrued);
		  System.Func<double, double> func = pricer.getPointFunction(i, creditCurve);

		  switch (ArbitrageHandling)
		  {
			case IGNORE:
			{
			  try
			  {
				double[] bracket = BRACKETER.getBracketedPoints(func, 0.8 * guess[i], 1.25 * guess[i], double.NegativeInfinity, double.PositiveInfinity);
				double zeroRate = bracket[0] > bracket[1] ? ROOTFINDER.getRoot(func, bracket[1], bracket[0]) : ROOTFINDER.getRoot(func, bracket[0], bracket[1]); //Negative guess handled
				creditCurve = creditCurve.withParameter(i, zeroRate);
			  }
//JAVA TO C# CONVERTER WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.opengamma.strata.math.MathException e)
			  catch (MathException e)
			  { //handling bracketing failure due to small survival probability
				if (Math.Abs(func(creditCurve.YValues.get(i - 1))) < 1.e-12)
				{
				  creditCurve = creditCurve.withParameter(i, creditCurve.YValues.get(i - 1));
				}
				else
				{
				  throw new MathException(e);
				}
			  }
			  break;
			}
			case FAIL:
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double minValue = i == 0 ? 0d : creditCurve.getYValues().get(i - 1) * creditCurve.getXValues().get(i - 1) / creditCurve.getXValues().get(i);
			  double minValue = i == 0 ? 0d : creditCurve.YValues.get(i - 1) * creditCurve.XValues.get(i - 1) / creditCurve.XValues.get(i);
			  if (i > 0 && func(minValue) > 0.0)
			  { //can never fail on the first spread
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder msg = new StringBuilder();
				StringBuilder msg = new StringBuilder();
				if (pointsUpfront.get(i) == 0.0)
				{
				  msg.Append("The par spread of " + flactionalSpreads.get(i) + " at index " + i);
				}
				else
				{
				  msg.Append("The premium of " + flactionalSpreads.get(i) + "and points up-front of " + pointsUpfront.get(i) + " at index " + i);
				}
				msg.Append(" is an arbitrage; cannot fit a curve with positive forward hazard rate. ");
				throw new System.ArgumentException(msg.ToString());
			  }
			  guess[i] = Math.Max(minValue, guess[i]);
			  double[] bracket = BRACKETER.getBracketedPoints(func, guess[i], 1.2 * guess[i], minValue, double.PositiveInfinity);
			  double zeroRate = ROOTFINDER.getRoot(func, bracket[0], bracket[1]).Value;
			  creditCurve = creditCurve.withParameter(i, zeroRate);
			  break;
			}
			case ZERO_HAZARD_RATE:
			{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double minValue = i == 0 ? 0.0 : creditCurve.getYValues().get(i - 1) * creditCurve.getXValues().get(i - 1) / creditCurve.getXValues().get(i);
			  double minValue = i == 0 ? 0.0 : creditCurve.YValues.get(i - 1) * creditCurve.XValues.get(i - 1) / creditCurve.XValues.get(i);
			  if (i > 0 && func(minValue) > 0.0)
			  { //can never fail on the first spread
				creditCurve = creditCurve.withParameter(i, minValue);
			  }
			  else
			  {
				guess[i] = Math.Max(minValue, guess[i]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] bracket = BRACKETER.getBracketedPoints(func, guess[i], 1.2 * guess[i], minValue, Double.POSITIVE_INFINITY);
				double[] bracket = BRACKETER.getBracketedPoints(func, guess[i], 1.2 * guess[i], minValue, double.PositiveInfinity);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double zeroRate = ROOTFINDER.getRoot(func, bracket[0], bracket[1]);
				double zeroRate = ROOTFINDER.getRoot(func, bracket[0], bracket[1]).Value;
				creditCurve = creditCurve.withParameter(i, zeroRate);
			  }
			  break;
			}
			default:
			  throw new System.ArgumentException("unknown case " + ArbitrageHandling);
		  }
		}
		return creditCurve;
	  }

	  /* Prices the CDS */
	  internal sealed class Pricer
	  {
		  private readonly FastCreditCurveCalibrator outerInstance;


		internal readonly ResolvedCds cds;
		internal readonly double lgdDF;
		internal readonly double valuationDF;
		internal readonly double fracSpread;
		internal readonly double puf;
		// protection leg
		internal readonly int nProPoints;
		internal readonly double[] proLegIntPoints;
		internal readonly double[] proYieldCurveRT;
		internal readonly double[] proDF;
		// premium leg
		internal readonly int nPayments;
		internal readonly double[] paymentDF;
		internal readonly double[][] premLegIntPoints;
		internal readonly double[][] premDF;
		internal readonly double[][] rt;
		internal readonly double[][] premDt;
		internal readonly double[] accRate;
		internal readonly double[] offsetAccStart;
		internal readonly double[] offsetAccEnd;

		internal readonly double accYearFraction;
		internal readonly double productEffectiveStart;
		internal readonly int startPeriodIndex;

		public Pricer(FastCreditCurveCalibrator outerInstance, ResolvedCds nodeCds, CreditDiscountFactors yieldCurve, DoubleArray creditCurveKnots, double fractionalSpread, double pointsUpfront, double lgd, LocalDate stepinDate, LocalDate effectiveStartDate, LocalDate settlementDate, double accruedYearFraction)
		{
			this.outerInstance = outerInstance;

		  accYearFraction = accruedYearFraction;
		  cds = nodeCds;
		  fracSpread = fractionalSpread;
		  puf = pointsUpfront;
		  productEffectiveStart = yieldCurve.relativeYearFraction(effectiveStartDate);
		  double protectionEnd = yieldCurve.relativeYearFraction(cds.ProtectionEndDate);
		  // protection leg
		  proLegIntPoints = DoublesScheduleGenerator.getIntegrationsPoints(productEffectiveStart, protectionEnd, yieldCurve.ParameterKeys, creditCurveKnots).toArray();
		  nProPoints = proLegIntPoints.Length;
		  valuationDF = yieldCurve.discountFactor(settlementDate);
		  lgdDF = lgd / valuationDF;
		  proYieldCurveRT = new double[nProPoints];
		  proDF = new double[nProPoints];
		  for (int i = 0; i < nProPoints; i++)
		  {
			proYieldCurveRT[i] = yieldCurve.zeroRate(proLegIntPoints[i]) * proLegIntPoints[i];
			proDF[i] = Math.Exp(-proYieldCurveRT[i]);
		  }
		  // premium leg
		  nPayments = cds.PaymentPeriods.size();
		  paymentDF = new double[nPayments];
		  int indexTmp = -1;
		  for (int i = 0; i < nPayments; i++)
		  {
			if (stepinDate.isBefore(cds.PaymentPeriods.get(i).EndDate))
			{
			  paymentDF[i] = yieldCurve.discountFactor(cds.PaymentPeriods.get(i).PaymentDate);
			}
			else
			{
			  indexTmp = i;
			}
		  }
		  startPeriodIndex = indexTmp + 1;
		  // accrual on default
		  if (cds.PaymentOnDefault.AccruedInterest)
		  {
			LocalDate tmp = nPayments == 1 ? effectiveStartDate : cds.AccrualStartDate;
			DoubleArray integrationSchedule = DoublesScheduleGenerator.getIntegrationsPoints(yieldCurve.relativeYearFraction(tmp), protectionEnd, yieldCurve.ParameterKeys, creditCurveKnots);
			accRate = new double[nPayments];
			offsetAccStart = new double[nPayments];
			offsetAccEnd = new double[nPayments];
			premLegIntPoints = new double[nPayments][];
			premDF = new double[nPayments][];
			rt = new double[nPayments][];
			premDt = new double[nPayments][];
			for (int i = startPeriodIndex; i < nPayments; i++)
			{
			  CreditCouponPaymentPeriod coupon = cds.PaymentPeriods.get(i);
			  offsetAccStart[i] = yieldCurve.relativeYearFraction(coupon.EffectiveStartDate);
			  offsetAccEnd[i] = yieldCurve.relativeYearFraction(coupon.EffectiveEndDate);
			  accRate[i] = coupon.YearFraction / yieldCurve.DayCount.relativeYearFraction(coupon.StartDate, coupon.EndDate);
			  double start = Math.Max(productEffectiveStart, offsetAccStart[i]);
			  if (start >= offsetAccEnd[i])
			  {
				continue;
			  }
			  premLegIntPoints[i] = DoublesScheduleGenerator.truncateSetInclusive(start, offsetAccEnd[i], integrationSchedule).toArray();
			  int n = premLegIntPoints[i].Length;
			  rt[i] = new double[n];
			  premDF[i] = new double[n];
			  for (int k = 0; k < n; k++)
			  {
				rt[i][k] = yieldCurve.zeroRate(premLegIntPoints[i][k]) * premLegIntPoints[i][k];
				premDF[i][k] = Math.Exp(-rt[i][k]);
			  }
			  premDt[i] = new double[n - 1];

			  for (int k = 1; k < n; k++)
			  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double dt = premLegIntPoints[i][k] - premLegIntPoints[i][k - 1];
				double dt = premLegIntPoints[i][k] - premLegIntPoints[i][k - 1];
				premDt[i][k - 1] = dt;
			  }
			}
		  }
		  else
		  {
			accRate = null;
			offsetAccStart = null;
			offsetAccEnd = null;
			premDF = null;
			premDt = null;
			rt = null;
			premLegIntPoints = null;
		  }
		}

		public System.Func<double, double> getPointFunction(int index, NodalCurve creditCurve)
		{
		  return (double? x) =>
		  {
	  NodalCurve cc = creditCurve.withParameter(index, x.Value);
	  double rpv01 = this.rpv01(cc, PriceType.CLEAN);
	  double pro = protectionLeg(cc);
	  return pro - fracSpread * rpv01 - puf;
		  };
		}

		public double rpv01(NodalCurve creditCurve, PriceType cleanOrDirty)
		{
		  double pv = 0.0;
		  for (int i = startPeriodIndex; i < nPayments; i++)
		  {
			CreditCouponPaymentPeriod coupon = cds.PaymentPeriods.get(i);
			double yc = offsetAccEnd[i];
			double q = Math.Exp(-creditCurve.yValue(yc) * yc);
			pv += coupon.YearFraction * paymentDF[i] * q;
		  }

		  if (cds.PaymentOnDefault.AccruedInterest)
		  {
			double accPV = 0.0;
			for (int i = startPeriodIndex; i < nPayments; i++)
			{
			  accPV += calculateSinglePeriodAccrualOnDefault(i, creditCurve);
			}
			pv += accPV;
		  }
		  pv /= valuationDF;
		  if (cleanOrDirty == PriceType.CLEAN)
		  {
			pv -= accYearFraction;
		  }
		  return pv;
		}

		internal double calculateSinglePeriodAccrualOnDefault(int paymentIndex, NodalCurve creditCurve)
		{
		  double[] knots = premLegIntPoints[paymentIndex];
		  if (knots == null)
		  {
			return 0d;
		  }
		  double[] df = premDF[paymentIndex];
		  double[] deltaT = premDt[paymentIndex];
		  double[] rtCurrent = rt[paymentIndex];
		  double accRateCurrent = accRate[paymentIndex];
		  double accStart = offsetAccStart[paymentIndex];
		  double t = knots[0];
		  double ht0 = creditCurve.yValue(t) * t;
		  double rt0 = rtCurrent[0];
		  double b0 = df[0] * Math.Exp(-ht0);
		  double t0 = t - accStart + outerInstance.AccrualOnDefaultFormula.Omega;
		  double pv = 0d;
		  int nItems = knots.Length;
		  for (int j = 1; j < nItems; ++j)
		  {
			t = knots[j];
			double ht1 = creditCurve.yValue(t) * t;
			double rt1 = rtCurrent[j];
			double b1 = df[j] * Math.Exp(-ht1);
			double dt = deltaT[j - 1];
			double dht = ht1 - ht0;
			double drt = rt1 - rt0;
			double dhrt = dht + drt + 1e-50; // to keep consistent with ISDA c code
			double tPV;
			if (outerInstance.AccrualOnDefaultFormula == AccrualOnDefaultFormula.MARKIT_FIX)
			{
			  if (Math.Abs(dhrt) < 1e-5)
			  {
				tPV = dht * dt * b0 * epsilonP(-dhrt);
			  }
			  else
			  {
				tPV = dht * dt / dhrt * ((b0 - b1) / dhrt - b1);
			  }
			}
			else
			{
			  double t1 = t - accStart + outerInstance.AccrualOnDefaultFormula.Omega;
			  if (Math.Abs(dhrt) < 1e-5)
			  {
				tPV = dht * b0 * (t0 * epsilon(-dhrt) + dt * epsilonP(-dhrt));
			  }
			  else
			  {
				tPV = dht / dhrt * (t0 * b0 - t1 * b1 + dt / dhrt * (b0 - b1));
			  }
			  t0 = t1;
			}
			pv += tPV;
			ht0 = ht1;
			rt0 = rt1;
			b0 = b1;
		  }
		  return accRateCurrent * pv;
		}

		public double protectionLeg(NodalCurve creditCurve)
		{
		  double ht0 = creditCurve.yValue(proLegIntPoints[0]) * proLegIntPoints[0];
		  double rt0 = proYieldCurveRT[0];
		  double b0 = proDF[0] * Math.Exp(-ht0);
		  double pv = 0d;
		  for (int i = 1; i < nProPoints; ++i)
		  {
			double ht1 = creditCurve.yValue(proLegIntPoints[i]) * proLegIntPoints[i];
			double rt1 = proYieldCurveRT[i];
			double b1 = proDF[i] * Math.Exp(-ht1);
			double dht = ht1 - ht0;
			double drt = rt1 - rt0;
			double dhrt = dht + drt;
			// this is equivalent to the ISDA code without explicitly calculating the time step - it also handles the limit
			double dPV;
			if (Math.Abs(dhrt) < 1e-5)
			{
			  dPV = dht * b0 * epsilon(-dhrt);
			}
			else
			{
			  dPV = (b0 - b1) * dht / dhrt;
			}
			pv += dPV;
			ht0 = ht1;
			rt0 = rt1;
			b0 = b1;
		  }
		  pv *= lgdDF; // multiply by LGD and adjust to valuation date
		  return pv;
		}
	  }

	}

}