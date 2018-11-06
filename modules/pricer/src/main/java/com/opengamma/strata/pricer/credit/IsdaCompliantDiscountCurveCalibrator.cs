using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Builder = com.google.common.collect.ImmutableList.Builder;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using DepositIsdaCreditCurveNode = com.opengamma.strata.market.curve.DepositIsdaCreditCurveNode;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using IsdaCreditCurveNode = com.opengamma.strata.market.curve.IsdaCreditCurveNode;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using SwapIsdaCreditCurveNode = com.opengamma.strata.market.curve.SwapIsdaCreditCurveNode;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivities = com.opengamma.strata.market.param.UnitParameterSensitivities;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using NewtonRaphsonSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.NewtonRaphsonSingleRootFinder;

	/// <summary>
	/// ISDA compliant discount curve calibrator.
	/// <para>
	/// A single discounting curve is calibrated for a specified currency.
	/// </para>
	/// <para>
	/// The curve is defined using two or more <seealso cref="CurveNode nodes"/>.
	/// Each node primarily defines enough information to produce a reference trade.
	/// Calibration involves pricing, and re-pricing, these trades to find the best fit using a root finder.
	/// </para>
	/// <para>
	/// Once calibrated, the curves are then available for use.
	/// Each node in the curve definition becomes a parameter in the matching output curve.
	/// </para>
	/// </summary>
	public sealed class IsdaCompliantDiscountCurveCalibrator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly IsdaCompliantDiscountCurveCalibrator STANDARD = IsdaCompliantDiscountCurveCalibrator.of(1.0e-12);

	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new CommonsMatrixAlgebra();
	  /// <summary>
	  /// The root bracket finder.
	  /// </summary>
	  private static readonly BracketRoot BRACKETER = new BracketRoot();
	  /// <summary>
	  /// The root finder.
	  /// </summary>
	  private readonly NewtonRaphsonSingleRootFinder rootFinder;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the standard curve calibrator.
	  /// <para>
	  /// The accuracy of the root finder is set to be its default, 1.0e-12;
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard curve calibrator </returns>
	  public static IsdaCompliantDiscountCurveCalibrator standard()
	  {
		return IsdaCompliantDiscountCurveCalibrator.STANDARD;
	  }

	  /// <summary>
	  /// Obtains the curve calibrator with the accuracy of the root finder specified. 
	  /// </summary>
	  /// <param name="accuracy">  the accuracy </param>
	  /// <returns> the curve calibrator </returns>
	  public static IsdaCompliantDiscountCurveCalibrator of(double accuracy)
	  {
		return new IsdaCompliantDiscountCurveCalibrator(accuracy);
	  }

	  // private constructor
	  private IsdaCompliantDiscountCurveCalibrator(double accuracy)
	  {
		this.rootFinder = new NewtonRaphsonSingleRootFinder(accuracy);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrates the ISDA compliant discount curve to the market data.
	  /// <para>
	  /// This creates the single discount curve for a specified currency.
	  /// The curve nodes in {@code IsdaCreditCurveDefinition} should be term deposit or fixed-for-Ibor swap, 
	  /// and the number of nodes should be greater than 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefinition">  the curve definition </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the ISDA compliant discount curve </returns>
	  public IsdaCreditDiscountFactors calibrate(IsdaCreditCurveDefinition curveDefinition, MarketData marketData, ReferenceData refData)
	  {

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<? extends com.opengamma.strata.market.curve.IsdaCreditCurveNode> curveNodes = curveDefinition.getCurveNodes();
		IList<IsdaCreditCurveNode> curveNodes = curveDefinition.CurveNodes;
		int nNodes = curveNodes.Count;
		ArgChecker.isTrue(nNodes > 1, "the number of curve nodes must be greater than 1");
		LocalDate curveSnapDate = marketData.ValuationDate;
		LocalDate curveValuationDate = curveDefinition.CurveValuationDate;
		DayCount curveDayCount = curveDefinition.DayCount;
		BasicFixedLeg[] swapLeg = new BasicFixedLeg[nNodes];
		double[] termDepositYearFraction = new double[nNodes];
		double[] curveNodeTime = new double[nNodes];
		double[] rates = new double[nNodes];
		ImmutableList.Builder<ParameterMetadata> paramMetadata = ImmutableList.builder();
		int nTermDeposit = 0;
		LocalDate curveSpotDate = null;
		for (int i = 0; i < nNodes; i++)
		{
		  LocalDate cvDateTmp;
		  IsdaCreditCurveNode node = curveNodes[i];
		  rates[i] = marketData.getValue(node.ObservableId);
		  LocalDate adjMatDate = node.date(curveSnapDate, refData);
		  paramMetadata.add(node.metadata(adjMatDate));
		  if (node is DepositIsdaCreditCurveNode)
		  {
			DepositIsdaCreditCurveNode termDeposit = (DepositIsdaCreditCurveNode) node;
			cvDateTmp = termDeposit.SpotDateOffset.adjust(curveSnapDate, refData);
			curveNodeTime[i] = curveDayCount.relativeYearFraction(cvDateTmp, adjMatDate);
			termDepositYearFraction[i] = termDeposit.DayCount.relativeYearFraction(cvDateTmp, adjMatDate);
			ArgChecker.isTrue(nTermDeposit == i, "TermDepositCurveNode should not be after FixedIborSwapCurveNode");
			++nTermDeposit;
		  }
		  else if (node is SwapIsdaCreditCurveNode)
		  {
			SwapIsdaCreditCurveNode swap = (SwapIsdaCreditCurveNode) node;
			cvDateTmp = swap.SpotDateOffset.adjust(curveSnapDate, refData);
			curveNodeTime[i] = curveDayCount.relativeYearFraction(cvDateTmp, adjMatDate);
			BusinessDayAdjustment busAdj = swap.BusinessDayAdjustment;
			swapLeg[i] = new BasicFixedLeg(this, cvDateTmp, cvDateTmp.plus(swap.Tenor), swap.PaymentFrequency.Period, swap.DayCount, curveDayCount, busAdj, refData);
		  }
		  else
		  {
			throw new System.ArgumentException("unsupported cuve node type");
		  }
		  if (i > 0)
		  {
			ArgChecker.isTrue(curveNodeTime[i] - curveNodeTime[i - 1] > 0, "curve nodes should be ascending in terms of tenor");
			ArgChecker.isTrue(cvDateTmp.Equals(curveSpotDate), "spot lag should be common for all of the curve nodes");
		  }
		  else
		  {
			ArgChecker.isTrue(curveNodeTime[i] >= 0d, "the first node should be after curve spot date");
			curveSpotDate = cvDateTmp;
		  }
		}
		ImmutableList<ParameterMetadata> parameterMetadata = paramMetadata.build();
		double[] ratesMod = Arrays.copyOf(rates, nNodes);
		for (int i = 0; i < nTermDeposit; ++i)
		{
		  double dfInv = 1d + ratesMod[i] * termDepositYearFraction[i];
		  ratesMod[i] = Math.Log(dfInv) / curveNodeTime[i];
		}
		InterpolatedNodalCurve curve = curveDefinition.curve(DoubleArray.ofUnsafe(curveNodeTime), DoubleArray.ofUnsafe(ratesMod));
		for (int i = nTermDeposit; i < nNodes; ++i)
		{
		  curve = fitSwap(i, swapLeg[i], curve, rates[i]);
		}

		Currency currency = curveDefinition.Currency;
		DoubleMatrix sensi = quoteValueSensitivity(nTermDeposit, termDepositYearFraction, swapLeg, ratesMod, curve, curveDefinition.ComputeJacobian);
		if (curveValuationDate.isEqual(curveSpotDate))
		{
		  if (curveDefinition.ComputeJacobian)
		  {
			JacobianCalibrationMatrix jacobian = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(curveDefinition.Name, nNodes)), MATRIX_ALGEBRA.getInverse(sensi));
			NodalCurve curveWithParamMetadata = curve.withMetadata(curve.Metadata.withInfo(CurveInfoType.JACOBIAN, jacobian).withParameterMetadata(parameterMetadata));
			return IsdaCreditDiscountFactors.of(currency, curveValuationDate, curveWithParamMetadata);
		  }
		  NodalCurve curveWithParamMetadata = curve.withMetadata(curve.Metadata.withParameterMetadata(parameterMetadata));
		  return IsdaCreditDiscountFactors.of(currency, curveValuationDate, curveWithParamMetadata);
		}
		double offset = curveDayCount.relativeYearFraction(curveSpotDate, curveValuationDate);
		return IsdaCreditDiscountFactors.of(currency, curveValuationDate, withShift(curve, parameterMetadata, sensi, curveDefinition.ComputeJacobian, offset));
	  }

	  //-------------------------------------------------------------------------
	  private InterpolatedNodalCurve fitSwap(int curveIndex, BasicFixedLeg swap, InterpolatedNodalCurve curve, double swapRate)
	  {
		int nPayments = swap.NumPayments;
		int nNodes = curve.ParameterCount;
		double t1 = curveIndex == 0 ? 0.0 : curve.XValues.get(curveIndex - 1);
		double t2 = curveIndex == nNodes - 1 ? double.PositiveInfinity : curve.XValues.get(curveIndex + 1);
		double temp = 0;
		double temp2 = 0;
		int i1 = 0;
		int i2 = nPayments;
		double[] paymentAmounts = new double[nPayments];
		for (int i = 0; i < nPayments; i++)
		{
		  double t = swap.getPaymentTime(i);
		  paymentAmounts[i] = swap.getPaymentAmounts(i, swapRate);
		  if (t <= t1)
		  {
			double df = Math.Exp(-curve.yValue(t) * t);
			temp += paymentAmounts[i] * df;
			temp2 += paymentAmounts[i] * t * df * curve.yValueParameterSensitivity(t).Sensitivity.get(curveIndex);
			i1++;
		  }
		  else if (t >= t2)
		  {
			double df = Math.Exp(-curve.yValue(t) * t);
			temp += paymentAmounts[i] * df;
			temp2 -= paymentAmounts[i] * t * df * curve.yValueParameterSensitivity(t).Sensitivity.get(curveIndex);
			i2--;
		  }
		}
		double cachedValues = temp;
		double cachedSense = temp2;
		int index1 = i1;
		int index2 = i2;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: java.util.function.Function<double, double> func = new java.util.function.Function<double, double>()
		System.Func<double, double> func = (double? x) =>
		{
	InterpolatedNodalCurve tempCurve = curve.withParameter(curveIndex, x);
	double sum = 1.0 - cachedValues; // Floating leg at par
	for (int i = index1; i < index2; i++)
	{
	  double t = swap.getPaymentTime(i);
	  sum -= paymentAmounts[i] * Math.Exp(-tempCurve.yValue(t) * t);
	}
	return sum;
		};

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: java.util.function.Function<double, double> grad = new java.util.function.Function<double, double>()
		System.Func<double, double> grad = (double? x) =>
		{
	InterpolatedNodalCurve tempCurve = curve.withParameter(curveIndex, x);
	double sum = cachedSense;
	for (int i = index1; i < index2; i++)
	{
	  double t = swap.getPaymentTime(i);
	  sum += swap.getPaymentAmounts(i, swapRate) * t * Math.Exp(-tempCurve.yValue(t) * t) * tempCurve.yValueParameterSensitivity(t).Sensitivity.get(curveIndex);
	}
	return sum;

		};

		double guess = curve.getParameter(curveIndex);
		if (guess == 0.0 && func(guess) == 0.0)
		{
		  return curve;
		}
		double[] bracket = guess > 0d ? BRACKETER.getBracketedPoints(func, 0.8 * guess, 1.25 * guess, double.NegativeInfinity, double.PositiveInfinity) : BRACKETER.getBracketedPoints(func, 1.25 * guess, 0.8 * guess, double.NegativeInfinity, double.PositiveInfinity);
		double r = rootFinder.getRoot(func, grad, bracket[0], bracket[1]).Value;
		return curve.withParameter(curveIndex, r);
	  }

	  //-------------------------------------------------------------------------
	  // market quote sensitivity calculators
	  private DoubleMatrix quoteValueSensitivity(int nTermDeposit, double[] termDepositYearFraction, BasicFixedLeg[] swapLeg, double[] rates, InterpolatedNodalCurve curve, bool computejacobian)
	  {

		if (computejacobian)
		{
		  int nNode = curve.ParameterCount;
		  DoubleMatrix sensiDeposit = DoubleMatrix.ofArrayObjects(nTermDeposit, nNode, i => sensitivityDeposit(curve, termDepositYearFraction[i], i, rates[i]));
		  DoubleMatrix sensiSwap = DoubleMatrix.ofArrayObjects(nNode - nTermDeposit, nNode, i => sensitivitySwap(swapLeg[i + nTermDeposit], curve, rates[i + nTermDeposit]));
		  double[][] sensiTotal = new double[nNode][];
		  for (int i = 0; i < nTermDeposit; ++i)
		  {
			sensiTotal[i] = sensiDeposit.rowArray(i);
		  }
		  for (int i = nTermDeposit; i < nNode; ++i)
		  {
			sensiTotal[i] = sensiSwap.rowArray(i - nTermDeposit);
		  }
		  return DoubleMatrix.ofUnsafe(sensiTotal);
		}
		return DoubleMatrix.EMPTY;
	  }

	  private DoubleArray sensitivityDeposit(InterpolatedNodalCurve curve, double termDepositYearFraction, int index, double fixedRate)
	  {

		int nNode = curve.ParameterCount;
		double[] sensi = new double[nNode];
		sensi[index] = curve.XValues.get(index) * (1d + fixedRate * termDepositYearFraction) / termDepositYearFraction;
		return DoubleArray.ofUnsafe(sensi);
	  }

	  private DoubleArray sensitivitySwap(BasicFixedLeg swap, NodalCurve curve, double swapRate)
	  {
		int nPayments = swap.NumPayments;
		double annuity = 0d;
		UnitParameterSensitivities sensi = UnitParameterSensitivities.empty();
		for (int i = 0; i < nPayments - 1; i++)
		{
		  double t = swap.getPaymentTime(i);
		  double df = Math.Exp(-curve.yValue(t) * t);
		  annuity += swap.getYearFraction(i) * df;
		  sensi = sensi.combinedWith(curve.yValueParameterSensitivity(t).multipliedBy(-df * t * swap.getYearFraction(i) * swapRate));
		}
		int lastIndex = nPayments - 1;
		double t = swap.getPaymentTime(lastIndex);
		double df = Math.Exp(-curve.yValue(t) * t);
		annuity += swap.getYearFraction(lastIndex) * df;
		sensi = sensi.combinedWith(curve.yValueParameterSensitivity(t).multipliedBy(-df * t * (1d + swap.getYearFraction(lastIndex) * swapRate)));
		sensi = sensi.multipliedBy(-1d / annuity);
		ArgChecker.isTrue(sensi.size() == 1);
		return sensi.Sensitivities.get(0).Sensitivity;
	  }

	  //-------------------------------------------------------------------------
	  /* crude swap fixed leg description */
	  private sealed class BasicFixedLeg
	  {
		  private readonly IsdaCompliantDiscountCurveCalibrator outerInstance;

		internal readonly int nPayment;
		internal readonly double[] swapPaymentTime;
		internal readonly double[] yearFraction;

		public BasicFixedLeg(IsdaCompliantDiscountCurveCalibrator outerInstance, LocalDate curveSpotDate, LocalDate maturityDate, Period swapInterval, DayCount swapDCC, DayCount curveDcc, BusinessDayAdjustment busAdj, ReferenceData refData)
		{
			this.outerInstance = outerInstance;

		  IList<LocalDate> list = new List<LocalDate>();
		  LocalDate tDate = maturityDate;
		  int step = 1;
		  while (tDate.isAfter(curveSpotDate))
		  {
			list.Add(tDate);
			tDate = maturityDate.minus(swapInterval.multipliedBy(step++));
		  }

		  // remove spotDate from list, if it ends up there
		  list.Remove(curveSpotDate);

		  nPayment = list.Count;
		  swapPaymentTime = new double[nPayment];
		  yearFraction = new double[nPayment];
		  LocalDate prev = curveSpotDate;
		  int j = nPayment - 1;
		  for (int i = 0; i < nPayment; i++, j--)
		  {
			LocalDate current = list[j];
			LocalDate adjCurr = busAdj.adjust(current, refData);
			yearFraction[i] = swapDCC.relativeYearFraction(prev, adjCurr);
			swapPaymentTime[i] = curveDcc.relativeYearFraction(curveSpotDate, adjCurr); // Payment times always good business days
			prev = adjCurr;
		  }
		}

		public int NumPayments
		{
			get
			{
			  return nPayment;
			}
		}

		public double getPaymentAmounts(int index, double rate)
		{
		  return index == nPayment - 1 ? 1 + rate * yearFraction[index] : rate * yearFraction[index];
		}

		public double getPaymentTime(int index)
		{
		  return swapPaymentTime[index];
		}

		public double getYearFraction(int index)
		{
		  return yearFraction[index];
		}

	  }

	  //-------------------------------------------------------------------------
	  // shift the curve 
	  private NodalCurve withShift(InterpolatedNodalCurve curve, IList<ParameterMetadata> parameterMetadata, DoubleMatrix sensitivity, bool computeJacobian, double shift)
	  {

		int nNode = curve.ParameterCount;
		if (shift < curve.XValues.get(0))
		{
		  //offset less than t value of 1st knot, so no knots are not removed 
		  double eta = curve.YValues.get(0) * shift;
		  DoubleArray time = DoubleArray.of(nNode, i => curve.XValues.get(i) - shift);
		  DoubleArray rate = DoubleArray.of(nNode, i => (curve.YValues.get(i) * curve.XValues.get(i) - eta) / time.get(i));
		  CurveMetadata metadata = curve.Metadata.withParameterMetadata(parameterMetadata);
		  if (computeJacobian)
		  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] transf = new double[nNode][nNode];
			double[][] transf = RectangularArrays.ReturnRectangularDoubleArray(nNode, nNode);
			for (int i = 0; i < nNode; ++i)
			{
			  transf[i][0] = -shift / time.get(i);
			  transf[i][i] += curve.XValues.get(i) / time.get(i);
			}
			DoubleMatrix jacobianMatrix = (DoubleMatrix) MATRIX_ALGEBRA.multiply(DoubleMatrix.ofUnsafe(transf), MATRIX_ALGEBRA.getInverse(sensitivity));
			JacobianCalibrationMatrix jacobian = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(curve.Name, nNode)), jacobianMatrix);
			return curve.withValues(time, rate).withMetadata(metadata.withInfo(CurveInfoType.JACOBIAN, jacobian));
		  }
		  return curve.withValues(time, rate).withMetadata(metadata);
		}
		if (shift >= curve.XValues.get(nNode - 1))
		{
		  //new base after last knot. The new 'curve' has a constant zero rate which we represent with a nominal knot at 1.0
		  double time = 1d;
		  double interval = curve.XValues.get(nNode - 1) - curve.XValues.get(nNode - 2);
		  double rate = (curve.YValues.get(nNode - 1) * curve.XValues.get(nNode - 1) - curve.YValues.get(nNode - 2) * curve.XValues.get(nNode - 2)) / interval;
		  if (computeJacobian)
		  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] transf = new double[1][nNode];
			double[][] transf = RectangularArrays.ReturnRectangularDoubleArray(1, nNode);
			transf[0][nNode - 2] = -curve.XValues.get(nNode - 2) / interval;
			transf[0][nNode - 1] = curve.XValues.get(nNode - 1) / interval;
			DoubleMatrix jacobianMatrix = (DoubleMatrix) MATRIX_ALGEBRA.multiply(DoubleMatrix.ofUnsafe(transf), MATRIX_ALGEBRA.getInverse(sensitivity));
			JacobianCalibrationMatrix jacobian = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(curve.Name, nNode)), jacobianMatrix);
			return ConstantNodalCurve.of(curve.Metadata.withInfo(CurveInfoType.JACOBIAN, jacobian), time, rate);
		  }
		  return ConstantNodalCurve.of(curve.Metadata, time, rate);
		}
		//offset greater than (or equal to) t value of 1st knot, so at least one knot must be removed  
		int index = Arrays.binarySearch(curve.XValues.toArray(), shift);
		if (index < 0)
		{
		  index = -(index + 1);
		}
		else
		{
		  index++;
		}
		double interval = curve.XValues.get(index) - curve.XValues.get(index - 1);
		double tt1 = curve.XValues.get(index - 1) * (curve.XValues.get(index) - shift);
		double tt2 = curve.XValues.get(index) * (shift - curve.XValues.get(index - 1));
		double eta = (curve.YValues.get(index - 1) * tt1 + curve.YValues.get(index) * tt2) / interval;
		int m = nNode - index;
		CurveMetadata metadata = curve.Metadata.withParameterMetadata(parameterMetadata.subList(index, nNode));
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int indexFinal = index;
		int indexFinal = index;
		DoubleArray time = DoubleArray.of(m, i => curve.XValues.get(i + indexFinal) - shift);
		DoubleArray rate = DoubleArray.of(m, i => (curve.YValues.get(i + indexFinal) * curve.XValues.get(i + indexFinal) - eta) / time.get(i));
		if (computeJacobian)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] transf = new double[m][nNode];
		  double[][] transf = RectangularArrays.ReturnRectangularDoubleArray(m, nNode);
		  for (int i = 0; i < m; ++i)
		  {
			transf[i][index - 1] -= tt1 / (time.get(i) * interval);
			transf[i][index] -= tt2 / (time.get(i) * interval);
			transf[i][i + index] += curve.XValues.get(i + index) / time.get(i);
		  }
		  DoubleMatrix jacobianMatrix = (DoubleMatrix) MATRIX_ALGEBRA.multiply(DoubleMatrix.ofUnsafe(transf), MATRIX_ALGEBRA.getInverse(sensitivity));
		  JacobianCalibrationMatrix jacobian = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(curve.Name, nNode)), jacobianMatrix);
		  return curve.withValues(time, rate).withMetadata(metadata.withInfo(CurveInfoType.JACOBIAN, jacobian));
		}
		return curve.withValues(time, rate).withMetadata(metadata);
	  }

	}

}