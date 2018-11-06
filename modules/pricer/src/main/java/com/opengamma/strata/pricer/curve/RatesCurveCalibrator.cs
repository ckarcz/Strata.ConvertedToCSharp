using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Builder = com.google.common.collect.ImmutableMap.Builder;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataFxRateProvider = com.opengamma.strata.data.MarketDataFxRateProvider;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using NewtonVectorRootFinder = com.opengamma.strata.math.rootfind.NewtonVectorRootFinder;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Curve calibrator for rates curves.
	/// <para>
	/// This calibrator takes an abstract curve definition and produces real curves.
	/// </para>
	/// <para>
	/// Curves are calibrated in groups or one or more curves.
	/// In addition, more than one group may be calibrated together.
	/// </para>
	/// <para>
	/// Each curve is defined using two or more <seealso cref="CurveNode nodes"/>.
	/// Each node primarily defines enough information to produce a reference trade.
	/// Calibration involves pricing, and re-pricing, these trades to find the best fit
	/// using a root finder.
	/// </para>
	/// <para>
	/// Once calibrated, the curves are then available for use.
	/// Each node in the curve definition becomes a parameter in the matching output curve.
	/// </para>
	/// </summary>
	public sealed class RatesCurveCalibrator
	{

	  /// <summary>
	  /// The standard curve calibrator.
	  /// </summary>
	  private static readonly RatesCurveCalibrator STANDARD = RatesCurveCalibrator.of(1e-9, 1e-9, 1000, CalibrationMeasures.PAR_SPREAD, CalibrationMeasures.PRESENT_VALUE);
	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new CommonsMatrixAlgebra();

	  /// <summary>
	  /// The root finder used for curve calibration.
	  /// </summary>
	  private readonly NewtonVectorRootFinder rootFinder;
	  /// <summary>
	  /// The calibration measures.
	  /// This is used to compute the function for which the root is found.
	  /// </summary>
	  private readonly CalibrationMeasures measures;
	  /// <summary>
	  /// The present value measures.
	  /// This is used to compute the present value sensitivity to market quotes stored in the metadata.
	  /// </summary>
	  private readonly CalibrationMeasures pvMeasures;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The standard curve calibrator.
	  /// <para>
	  /// This uses the standard tolerance of 1e-9, a maximum of 1000 steps.
	  /// The default <seealso cref="CalibrationMeasures#PAR_SPREAD"/> measures are used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard curve calibrator </returns>
	  public static RatesCurveCalibrator standard()
	  {
		return RatesCurveCalibrator.STANDARD;
	  }

	  /// <summary>
	  /// Obtains an instance specifying tolerances to use.
	  /// <para>
	  /// This uses a Broyden root finder.
	  /// The standard <seealso cref="CalibrationMeasures#PAR_SPREAD"/> and <seealso cref="CalibrationMeasures#PRESENT_VALUE"/> measures are used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="toleranceAbs">  the absolute tolerance </param>
	  /// <param name="toleranceRel">  the relative tolerance </param>
	  /// <param name="stepMaximum">  the maximum steps </param>
	  /// <returns> the curve calibrator </returns>
	  public static RatesCurveCalibrator of(double toleranceAbs, double toleranceRel, int stepMaximum)
	  {

		return of(toleranceAbs, toleranceRel, stepMaximum, CalibrationMeasures.PAR_SPREAD, CalibrationMeasures.PRESENT_VALUE);
	  }

	  /// <summary>
	  /// Obtains an instance specifying tolerances and measures to use.
	  /// <para>
	  /// This uses a Broyden root finder.
	  /// The standard <seealso cref="CalibrationMeasures#PRESENT_VALUE"/> measures are used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="toleranceAbs">  the absolute tolerance </param>
	  /// <param name="toleranceRel">  the relative tolerance </param>
	  /// <param name="stepMaximum">  the maximum steps </param>
	  /// <param name="measures">  the calibration measures, used to compute the function for which the root is found </param>
	  /// <returns> the curve calibrator </returns>
	  public static RatesCurveCalibrator of(double toleranceAbs, double toleranceRel, int stepMaximum, CalibrationMeasures measures)
	  {

		return of(toleranceAbs, toleranceRel, stepMaximum, measures, CalibrationMeasures.PRESENT_VALUE);
	  }

	  /// <summary>
	  /// Obtains an instance specifying tolerances and measures to use.
	  /// <para>
	  /// This uses a Broyden root finder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="toleranceAbs">  the absolute tolerance </param>
	  /// <param name="toleranceRel">  the relative tolerance </param>
	  /// <param name="stepMaximum">  the maximum steps </param>
	  /// <param name="measures">  the calibration measures, used to compute the function for which the root is found </param>
	  /// <param name="pvMeasures">  the present value measures, used to compute the present value sensitivity to market quotes 
	  ///   stored in the metadata </param>
	  /// <returns> the curve calibrator </returns>
	  public static RatesCurveCalibrator of(double toleranceAbs, double toleranceRel, int stepMaximum, CalibrationMeasures measures, CalibrationMeasures pvMeasures)
	  {

		NewtonVectorRootFinder rootFinder = NewtonVectorRootFinder.broyden(toleranceAbs, toleranceRel, stepMaximum);
		return new RatesCurveCalibrator(rootFinder, measures, pvMeasures);
	  }

	  /// <summary>
	  /// Obtains an instance specifying the measures to use.
	  /// </summary>
	  /// <param name="rootFinder">  the root finder to use </param>
	  /// <param name="measures">  the calibration measures, used to compute the function for which the root is found </param>
	  /// <param name="pvMeasures">  the present value measures, used to compute the present value sensitivity to market quotes 
	  ///   stored in the metadata </param>
	  /// <returns> the curve calibrator </returns>
	  public static RatesCurveCalibrator of(NewtonVectorRootFinder rootFinder, CalibrationMeasures measures, CalibrationMeasures pvMeasures)
	  {

		return new RatesCurveCalibrator(rootFinder, measures, pvMeasures);
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  private RatesCurveCalibrator(NewtonVectorRootFinder rootFinder, CalibrationMeasures measures, CalibrationMeasures pvMeasures)
	  {

		this.rootFinder = ArgChecker.notNull(rootFinder, "rootFinder");
		this.measures = ArgChecker.notNull(measures, "measures");
		this.pvMeasures = ArgChecker.notNull(pvMeasures, "pvMeasures");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the measures.
	  /// </summary>
	  /// <returns> the measures </returns>
	  public CalibrationMeasures Measures
	  {
		  get
		  {
			return measures;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrates a single curve group, containing one or more curves.
	  /// <para>
	  /// The calibration is defined using <seealso cref="RatesCurveGroupDefinition"/>.
	  /// Observable market data, time-series and FX are also needed to complete the calibration.
	  /// The valuation date is defined by the market data.
	  /// </para>
	  /// <para>
	  /// The Jacobian matrices are computed and stored in curve metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveGroupDefn">  the curve group definition </param>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including time-series </param>
	  /// <param name="refData">  the reference data, used to resolve the trades </param>
	  /// <returns> the rates provider resulting from the calibration </returns>
	  public ImmutableRatesProvider calibrate(RatesCurveGroupDefinition curveGroupDefn, MarketData marketData, ReferenceData refData)
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<Index, LocalDateDoubleTimeSeries> timeSeries = marketData.TimeSeriesIds.Where(typeof(IndexQuoteId).isInstance).Select(typeof(IndexQuoteId).cast).collect(toImmutableMap(id => id.Index, id => marketData.getTimeSeries(id)));
		ImmutableRatesProvider knownData = ImmutableRatesProvider.builder(marketData.ValuationDate).fxRateProvider(MarketDataFxRateProvider.of(marketData)).timeSeries(timeSeries).build();
		return calibrate(ImmutableList.of(curveGroupDefn), knownData, marketData, refData);
	  }

	  /// <summary>
	  /// Calibrates a list of curve groups, each containing one or more curves.
	  /// <para>
	  /// The calibration is defined using a list of <seealso cref="RatesCurveGroupDefinition"/>.
	  /// Observable market data and existing known data are also needed to complete the calibration.
	  /// </para>
	  /// <para>
	  /// A curve must only exist in one group.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="allGroupsDefn">  the curve group definitions </param>
	  /// <param name="knownData">  the starting data for the calibration </param>
	  /// <param name="marketData">  the market data required to build a trade for the instrument </param>
	  /// <param name="refData">  the reference data, used to resolve the trades </param>
	  /// <returns> the rates provider resulting from the calibration </returns>
	  public ImmutableRatesProvider calibrate(IList<RatesCurveGroupDefinition> allGroupsDefn, ImmutableRatesProvider knownData, MarketData marketData, ReferenceData refData)
	  {
		// this method effectively takes one CurveGroupDefinition
		// the list is a split of the definition, not multiple independent definitions

		if (!knownData.ValuationDate.Equals(marketData.ValuationDate))
		{
		  throw new System.ArgumentException(Messages.format("Valuation dates do not match: {} and {}", knownData.ValuationDate, marketData.ValuationDate));
		}
		// perform calibration one group at a time, building up the result by mutating these variables
		ImmutableRatesProvider providerCombined = knownData;
		ImmutableList<CurveParameterSize> orderPrev = ImmutableList.of();
		ImmutableMap<CurveName, JacobianCalibrationMatrix> jacobians = ImmutableMap.of();
		foreach (RatesCurveGroupDefinition groupDefn in allGroupsDefn)
		{
		  if (groupDefn.Entries.Empty)
		  {
			continue;
		  }
		  RatesCurveGroupDefinition groupDefnBound = groupDefn.bindTimeSeries(knownData.ValuationDate, knownData.TimeSeries);
		  // combine all data in the group into flat lists
		  ImmutableList<ResolvedTrade> trades = groupDefnBound.resolvedTrades(marketData, refData);
		  ImmutableList<double> initialGuesses = groupDefnBound.initialGuesses(marketData);
		  ImmutableList<CurveParameterSize> orderGroup = toOrder(groupDefnBound);
		  ImmutableList<CurveParameterSize> orderPrevAndGroup = ImmutableList.builder<CurveParameterSize>().addAll(orderPrev).addAll(orderGroup).build();

		  // calibrate
		  RatesProviderGenerator providerGenerator = ImmutableRatesProviderGenerator.of(providerCombined, groupDefnBound, refData);
		  DoubleArray calibratedGroupParams = calibrateGroup(providerGenerator, trades, initialGuesses, orderGroup);
		  ImmutableRatesProvider calibratedProvider = providerGenerator.generate(calibratedGroupParams);

		  // use calibration to build Jacobian matrices
		  if (groupDefnBound.ComputeJacobian)
		  {
			jacobians = updateJacobiansForGroup(calibratedProvider, trades, orderGroup, orderPrev, orderPrevAndGroup, jacobians);
		  }
		  ImmutableMap<CurveName, DoubleArray> sensitivityToMarketQuote = ImmutableMap.of();
		  if (groupDefnBound.ComputePvSensitivityToMarketQuote)
		  {
			ImmutableRatesProvider providerWithJacobian = providerGenerator.generate(calibratedGroupParams, jacobians);
			sensitivityToMarketQuote = sensitivityToMarketQuoteForGroup(providerWithJacobian, trades, orderGroup);
		  }
		  orderPrev = orderPrevAndGroup;

		  // use Jacobians to build output curves
		  providerCombined = providerGenerator.generate(calibratedGroupParams, jacobians, sensitivityToMarketQuote);
		}
		// return the calibrated provider
		return providerCombined;
	  }

	  // converts a definition to the curve order list
	  private static ImmutableList<CurveParameterSize> toOrder(RatesCurveGroupDefinition groupDefn)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return groupDefn.CurveDefinitions.Select(def => def.toCurveParameterSize()).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  // calibrates a single group
	  private DoubleArray calibrateGroup(RatesProviderGenerator providerGenerator, ImmutableList<ResolvedTrade> trades, ImmutableList<double> initialGuesses, ImmutableList<CurveParameterSize> curveOrder)
	  {

		// setup for calibration
		System.Func<DoubleArray, DoubleArray> valueCalculator = new CalibrationValue(trades, measures, providerGenerator);
		System.Func<DoubleArray, DoubleMatrix> derivativeCalculator = new CalibrationDerivative(trades, measures, providerGenerator, curveOrder);

		// calibrate
		DoubleArray initGuessMatrix = DoubleArray.copyOf(initialGuesses);
		return rootFinder.findRoot(valueCalculator, derivativeCalculator, initGuessMatrix);
	  }

	  //-------------------------------------------------------------------------
	  // calculates the Jacobian and builds the result, called once per group
	  // this uses, but does not alter, data from previous groups
	  private ImmutableMap<CurveName, JacobianCalibrationMatrix> updateJacobiansForGroup(ImmutableRatesProvider provider, ImmutableList<ResolvedTrade> trades, ImmutableList<CurveParameterSize> orderGroup, ImmutableList<CurveParameterSize> orderPrev, ImmutableList<CurveParameterSize> orderAll, ImmutableMap<CurveName, JacobianCalibrationMatrix> jacobians)
	  {

		// sensitivity to all parameters in the stated order
		int totalParamsAll = orderAll.Select(e => e.ParameterCount).Sum();
		DoubleMatrix res = derivatives(trades, provider, orderAll, totalParamsAll);

		// jacobian direct
		int nbTrades = trades.size();
		int totalParamsGroup = orderGroup.Select(e => e.ParameterCount).Sum();
		int totalParamsPrevious = totalParamsAll - totalParamsGroup;
		DoubleMatrix pDmCurrentMatrix = jacobianDirect(res, nbTrades, totalParamsGroup, totalParamsPrevious);

		// jacobian indirect: when totalParamsPrevious > 0
		DoubleMatrix pDmPrevious = jacobianIndirect(res, pDmCurrentMatrix, nbTrades, totalParamsGroup, totalParamsPrevious, orderPrev, jacobians);

		// add to the map of jacobians, one entry for each curve in this group
		ImmutableMap.Builder<CurveName, JacobianCalibrationMatrix> jacobianBuilder = ImmutableMap.builder();
		jacobianBuilder.putAll(jacobians);
		int startIndex = 0;
		foreach (CurveParameterSize order in orderGroup)
		{
		  int paramCount = order.ParameterCount;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] pDmCurveArray = new double[paramCount][totalParamsAll];
		  double[][] pDmCurveArray = RectangularArrays.ReturnRectangularDoubleArray(paramCount, totalParamsAll);
		  // copy data for previous groups
		  if (totalParamsPrevious > 0)
		  {
			for (int p = 0; p < paramCount; p++)
			{
			  Array.Copy(pDmPrevious.rowArray(startIndex + p), 0, pDmCurveArray[p], 0, totalParamsPrevious);
			}
		  }
		  // copy data for this group
		  for (int p = 0; p < paramCount; p++)
		  {
			Array.Copy(pDmCurrentMatrix.rowArray(startIndex + p), 0, pDmCurveArray[p], totalParamsPrevious, totalParamsGroup);
		  }
		  // build final Jacobian matrix
		  DoubleMatrix pDmCurveMatrix = DoubleMatrix.ofUnsafe(pDmCurveArray);
		  jacobianBuilder.put(order.Name, JacobianCalibrationMatrix.of(orderAll, pDmCurveMatrix));
		  startIndex += paramCount;
		}
		return jacobianBuilder.build();
	  }

	  private ImmutableMap<CurveName, DoubleArray> sensitivityToMarketQuoteForGroup(ImmutableRatesProvider provider, ImmutableList<ResolvedTrade> trades, ImmutableList<CurveParameterSize> orderGroup)
	  {

		ImmutableMap.Builder<CurveName, DoubleArray> mqsGroup = new ImmutableMap.Builder<CurveName, DoubleArray>();
		int nodeIndex = 0;
		foreach (CurveParameterSize cps in orderGroup)
		{
		  int nbParameters = cps.ParameterCount;
		  double[] mqsCurve = new double[nbParameters];
		  for (int looptrade = 0; looptrade < nbParameters; looptrade++)
		  {
			DoubleArray mqsNode = pvMeasures.derivative(trades.get(nodeIndex), provider, orderGroup);
			mqsCurve[looptrade] = mqsNode.get(nodeIndex);
			nodeIndex++;
		  }
		  mqsGroup.put(cps.Name, DoubleArray.ofUnsafe(mqsCurve));
		}
		return mqsGroup.build();
	  }

	  // calculate the derivatives
	  private DoubleMatrix derivatives(ImmutableList<ResolvedTrade> trades, ImmutableRatesProvider provider, ImmutableList<CurveParameterSize> orderAll, int totalParamsAll)
	  {

		return DoubleMatrix.ofArrayObjects(trades.size(), totalParamsAll, i => measures.derivative(trades.get(i), provider, orderAll));
	  }

	  // jacobian direct, for the current group
	  private static DoubleMatrix jacobianDirect(DoubleMatrix res, int nbTrades, int totalParamsGroup, int totalParamsPrevious)
	  {

//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] direct = new double[totalParamsGroup][totalParamsGroup];
		double[][] direct = RectangularArrays.ReturnRectangularDoubleArray(totalParamsGroup, totalParamsGroup);
		for (int i = 0; i < nbTrades; i++)
		{
		  Array.Copy(res.rowArray(i), totalParamsPrevious, direct[i], 0, totalParamsGroup);
		}
		return MATRIX_ALGEBRA.getInverse(DoubleMatrix.copyOf(direct));
	  }

	  // jacobian indirect, merging groups
	  private static DoubleMatrix jacobianIndirect(DoubleMatrix res, DoubleMatrix pDmCurrentMatrix, int nbTrades, int totalParamsGroup, int totalParamsPrevious, ImmutableList<CurveParameterSize> orderPrevious, ImmutableMap<CurveName, JacobianCalibrationMatrix> jacobiansPrevious)
	  {

		if (totalParamsPrevious == 0)
		{
		  return DoubleMatrix.EMPTY;
		}
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] nonDirect = new double[totalParamsGroup][totalParamsPrevious];
		double[][] nonDirect = RectangularArrays.ReturnRectangularDoubleArray(totalParamsGroup, totalParamsPrevious);
		for (int i = 0; i < nbTrades; i++)
		{
		  Array.Copy(res.rowArray(i), 0, nonDirect[i], 0, totalParamsPrevious);
		}
		DoubleMatrix pDpPreviousMatrix = (DoubleMatrix) MATRIX_ALGEBRA.scale(MATRIX_ALGEBRA.multiply(pDmCurrentMatrix, DoubleMatrix.copyOf(nonDirect)), -1d);
		// all curves: order and size
		int[] startIndexBefore = new int[orderPrevious.size()];
		for (int i = 1; i < orderPrevious.size(); i++)
		{
		  startIndexBefore[i] = startIndexBefore[i - 1] + orderPrevious.get(i - 1).ParameterCount;
		}
		// transition Matrix: all curves from previous groups
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] transition = new double[totalParamsPrevious][totalParamsPrevious];
		double[][] transition = RectangularArrays.ReturnRectangularDoubleArray(totalParamsPrevious, totalParamsPrevious);
		for (int i = 0; i < orderPrevious.size(); i++)
		{
		  int paramCountOuter = orderPrevious.get(i).ParameterCount;
		  JacobianCalibrationMatrix thisInfo = jacobiansPrevious.get(orderPrevious.get(i).Name);
		  DoubleMatrix thisMatrix = thisInfo.JacobianMatrix;
		  int startIndexInner = 0;
		  for (int j = 0; j < orderPrevious.size(); j++)
		  {
			int paramCountInner = orderPrevious.get(j).ParameterCount;
			if (thisInfo.containsCurve(orderPrevious.get(j).Name))
			{ // If not, the matrix stay with 0
			  for (int k = 0; k < paramCountOuter; k++)
			  {
				Array.Copy(thisMatrix.rowArray(k), startIndexInner, transition[startIndexBefore[i] + k], startIndexBefore[j], paramCountInner);
			  }
			}
			startIndexInner += paramCountInner;
		  }
		}
		DoubleMatrix transitionMatrix = DoubleMatrix.copyOf(transition);
		return (DoubleMatrix) MATRIX_ALGEBRA.multiply(pDpPreviousMatrix, transitionMatrix);
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Messages.format("CurveCalibrator[{}]", measures);
	  }

	}

}