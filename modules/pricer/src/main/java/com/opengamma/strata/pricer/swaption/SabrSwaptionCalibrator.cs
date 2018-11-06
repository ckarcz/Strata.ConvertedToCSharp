using System;
using System.Collections;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using MathException = com.opengamma.strata.math.MathException;
	using NewtonRaphsonSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.NewtonRaphsonSingleRootFinder;
	using LeastSquareResultsWithTransform = com.opengamma.strata.math.impl.statistics.leastsquare.LeastSquareResultsWithTransform;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using SabrModelFitter = com.opengamma.strata.pricer.impl.volatility.smile.SabrModelFitter;
	using SabrInterestRateParameters = com.opengamma.strata.pricer.model.SabrInterestRateParameters;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;

	/// <summary>
	/// Swaption SABR calibrator.
	/// <para>
	/// This calibrator takes raw data and produces calibrated SABR parameters.
	/// </para>
	/// </summary>
	public sealed class SabrSwaptionCalibrator
	{

	  /// <summary>
	  /// The SABR implied volatility formula.
	  /// </summary>
	  private readonly SabrVolatilityFormula sabrVolatilityFormula;
	  /// <summary>
	  /// The swap pricer.
	  /// Required for forward rate computation.
	  /// </summary>
	  private readonly DiscountingSwapProductPricer swapPricer;
	  /// <summary>
	  /// The reference data.
	  /// </summary>
	  private readonly ReferenceData refData;

	  /// <summary>
	  /// The root-finder used in the Alpha calibration to ATM volatility. </summary>
	  private static readonly NewtonRaphsonSingleRootFinder ROOT_FINDER = new NewtonRaphsonSingleRootFinder();

	  /// <summary>
	  /// The default instance of the class.
	  /// </summary>
	  public static readonly SabrSwaptionCalibrator DEFAULT = new SabrSwaptionCalibrator(SabrVolatilityFormula.hagan(), DiscountingSwapProductPricer.DEFAULT, ReferenceData.standard());

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a SABR volatility function provider and a swap pricer.
	  /// <para>
	  /// The swap pricer is used to compute the forward rate required for calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sabrVolatilityFormula">  the SABR implied volatility formula </param>
	  /// <param name="swapPricer">  the swap pricer </param>
	  /// <returns> the calibrator </returns>
	  public static SabrSwaptionCalibrator of(SabrVolatilityFormula sabrVolatilityFormula, DiscountingSwapProductPricer swapPricer)
	  {

		return new SabrSwaptionCalibrator(sabrVolatilityFormula, swapPricer, ReferenceData.standard());
	  }

	  /// <summary>
	  /// Obtains an instance from a SABR volatility function provider and a swap pricer.
	  /// <para>
	  /// The swap pricer is used to compute the forward rate required for calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sabrVolatilityFormula">  the SABR implied volatility formula </param>
	  /// <param name="swapPricer">  the swap pricer </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the calibrator </returns>
	  public static SabrSwaptionCalibrator of(SabrVolatilityFormula sabrVolatilityFormula, DiscountingSwapProductPricer swapPricer, ReferenceData refData)
	  {

		return new SabrSwaptionCalibrator(sabrVolatilityFormula, swapPricer, refData);
	  }

	  private SabrSwaptionCalibrator(SabrVolatilityFormula sabrVolatilityFormula, DiscountingSwapProductPricer swapPricer, ReferenceData refData)
	  {

		this.sabrVolatilityFormula = ArgChecker.notNull(sabrVolatilityFormula, "sabrVolatilityFormula");
		this.swapPricer = ArgChecker.notNull(swapPricer, "swapPricer");
		this.refData = ArgChecker.notNull(refData, "refData");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate SABR parameters to a set of raw swaption data.
	  /// <para>
	  /// The SABR parameters are calibrated with fixed beta and fixed shift surfaces.
	  /// The raw data can be (shifted) log-normal volatilities, normal volatilities or option prices
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="definition">  the definition of the calibration to be performed </param>
	  /// <param name="calibrationDateTime">  the data and time of the calibration </param>
	  /// <param name="data">  the map of raw option data, keyed by tenor </param>
	  /// <param name="ratesProvider">  the rate provider used to compute the swap forward rates </param>
	  /// <param name="betaSurface">  the beta surface </param>
	  /// <param name="shiftSurface">  the shift surface </param>
	  /// <returns> the SABR volatility object </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("null") public SabrParametersSwaptionVolatilities calibrateWithFixedBetaAndShift(SabrSwaptionDefinition definition, java.time.ZonedDateTime calibrationDateTime, com.opengamma.strata.pricer.option.TenorRawOptionData data, com.opengamma.strata.pricer.rate.RatesProvider ratesProvider, com.opengamma.strata.market.surface.Surface betaSurface, com.opengamma.strata.market.surface.Surface shiftSurface)
	  public SabrParametersSwaptionVolatilities calibrateWithFixedBetaAndShift(SabrSwaptionDefinition definition, ZonedDateTime calibrationDateTime, TenorRawOptionData data, RatesProvider ratesProvider, Surface betaSurface, Surface shiftSurface)
	  {

		// If a MathException is thrown by a calibration for a specific expiry/tenor, an exception is thrown by the method
		return calibrateWithFixedBetaAndShift(definition, calibrationDateTime, data, ratesProvider, betaSurface, shiftSurface, true);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate SABR parameters to a set of raw swaption data.
	  /// <para>
	  /// The SABR parameters are calibrated with fixed beta and fixed shift surfaces.
	  /// The raw data can be (shifted) log-normal volatilities, normal volatilities or option prices
	  /// </para>
	  /// <para>
	  /// This method offers the flexibility to skip the data sets that throw a MathException (stopOnMathException = false).
	  /// The option to skip those data sets should be use with care, as part of the input data may be unused in the output.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="definition">  the definition of the calibration to be performed </param>
	  /// <param name="calibrationDateTime">  the data and time of the calibration </param>
	  /// <param name="data">  the map of raw option data, keyed by tenor </param>
	  /// <param name="ratesProvider">  the rate provider used to compute the swap forward rates </param>
	  /// <param name="betaSurface">  the beta surface </param>
	  /// <param name="shiftSurface">  the shift surface </param>
	  /// <param name="stopOnMathException">  flag indicating if the calibration should stop on math exceptions or skip the 
	  ///   expiries/tenors which throw MathException </param>
	  /// <returns> the SABR volatility object </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("null") public SabrParametersSwaptionVolatilities calibrateWithFixedBetaAndShift(SabrSwaptionDefinition definition, java.time.ZonedDateTime calibrationDateTime, com.opengamma.strata.pricer.option.TenorRawOptionData data, com.opengamma.strata.pricer.rate.RatesProvider ratesProvider, com.opengamma.strata.market.surface.Surface betaSurface, com.opengamma.strata.market.surface.Surface shiftSurface, boolean stopOnMathException)
	  public SabrParametersSwaptionVolatilities calibrateWithFixedBetaAndShift(SabrSwaptionDefinition definition, ZonedDateTime calibrationDateTime, TenorRawOptionData data, RatesProvider ratesProvider, Surface betaSurface, Surface shiftSurface, bool stopOnMathException)
	  {

		SwaptionVolatilitiesName name = definition.Name;
		FixedIborSwapConvention convention = definition.Convention;
		DayCount dayCount = definition.DayCount;
		SurfaceInterpolator interpolator = definition.Interpolator;

		BitArray @fixed = new BitArray();
		@fixed.Set(1, true); // Beta fixed
		BusinessDayAdjustment bda = convention.FloatingLeg.StartDateBusinessDayAdjustment;
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		// Sorted maps to obtain the surfaces nodes in standard order
		SortedDictionary<double, SortedDictionary<double, ParameterMetadata>> parameterMetadataTmp = new SortedDictionary<double, SortedDictionary<double, ParameterMetadata>>();
		SortedDictionary<double, SortedDictionary<double, DoubleArray>> dataSensitivityAlphaTmp = new SortedDictionary<double, SortedDictionary<double, DoubleArray>>(); // Sensitivity to the calibrating data
		SortedDictionary<double, SortedDictionary<double, DoubleArray>> dataSensitivityRhoTmp = new SortedDictionary<double, SortedDictionary<double, DoubleArray>>();
		SortedDictionary<double, SortedDictionary<double, DoubleArray>> dataSensitivityNuTmp = new SortedDictionary<double, SortedDictionary<double, DoubleArray>>();
		SortedDictionary<double, SortedDictionary<double, SabrFormulaData>> sabrPointTmp = new SortedDictionary<double, SortedDictionary<double, SabrFormulaData>>();
		foreach (Tenor tenor in data.Tenors)
		{
		  RawOptionData tenorData = data.getData(tenor);
		  double timeTenor = tenor.Period.Years + tenor.Period.Months / 12;
		  IList<Period> expiries = tenorData.Expiries;
		  int nbExpiries = expiries.Count;
		  for (int loopexpiry = 0; loopexpiry < nbExpiries; loopexpiry++)
		  {
			Pair<DoubleArray, DoubleArray> availableSmile = tenorData.availableSmileAtExpiry(expiries[loopexpiry]);
			if (availableSmile.First.size() == 0)
			{ // If not data is available, no calibration possible
			  continue;
			}
			LocalDate exerciseDate = expirationDate(bda, calibrationDate, expiries[loopexpiry]);
			LocalDate effectiveDate = convention.calculateSpotDateFromTradeDate(exerciseDate, refData);
			double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
			double beta = betaSurface.zValue(timeToExpiry, timeTenor);
			double shift = shiftSurface.zValue(timeToExpiry, timeTenor);
			LocalDate endDate = effectiveDate.plus(tenor);
			SwapTrade swap0 = convention.toTrade(calibrationDate, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			double forward = swapPricer.parRate(swap0.Product.resolve(refData), ratesProvider);
			SabrFormulaData sabrPoint = null;
			DoubleMatrix inverseJacobian = null;
			bool error = false;
			try
			{
			  Pair<SabrFormulaData, DoubleMatrix> calibrationResult = calibration(forward, shift, beta, @fixed, bda, calibrationDateTime, dayCount, availableSmile.First, availableSmile.Second, expiries[loopexpiry], tenorData);
			  sabrPoint = calibrationResult.First;
			  inverseJacobian = calibrationResult.Second;
			}
			catch (MathException e)
			{
			  error = true;
			  if (stopOnMathException)
			  {
				string message = Messages.format("{} at expiry {} and tenor {}", e.Message, expiries[loopexpiry], tenor);
				throw new MathException(message, e);
			  }
			}
			if (!error)
			{
			  if (!parameterMetadataTmp.ContainsKey(timeToExpiry))
			  {
				parameterMetadataTmp[timeToExpiry] = new SortedDictionary<>();
				dataSensitivityAlphaTmp[timeToExpiry] = new SortedDictionary<>();
				dataSensitivityRhoTmp[timeToExpiry] = new SortedDictionary<>();
				dataSensitivityNuTmp[timeToExpiry] = new SortedDictionary<>();
				sabrPointTmp[timeToExpiry] = new SortedDictionary<>();
			  }
			  SortedDictionary<double, ParameterMetadata> parameterMetadataExpiryMap = parameterMetadataTmp[timeToExpiry];
			  SortedDictionary<double, DoubleArray> dataSensitivityAlphaExpiryMap = dataSensitivityAlphaTmp[timeToExpiry];
			  SortedDictionary<double, DoubleArray> dataSensitivityRhoExpiryMap = dataSensitivityRhoTmp[timeToExpiry];
			  SortedDictionary<double, DoubleArray> dataSensitivityNuExpiryMap = dataSensitivityNuTmp[timeToExpiry];
			  SortedDictionary<double, SabrFormulaData> sabrPointExpiryMap = sabrPointTmp[timeToExpiry];
			  parameterMetadataExpiryMap[timeTenor] = SwaptionSurfaceExpiryTenorParameterMetadata.of(timeToExpiry, timeTenor, expiries[loopexpiry].ToString() + "x" + tenor.ToString());
			  dataSensitivityAlphaExpiryMap[timeTenor] = inverseJacobian.row(0);
			  dataSensitivityRhoExpiryMap[timeTenor] = inverseJacobian.row(2);
			  dataSensitivityNuExpiryMap[timeTenor] = inverseJacobian.row(3);
			  sabrPointExpiryMap[timeTenor] = sabrPoint;
			}
		  }
		}
		DoubleArray timeToExpiryArray = DoubleArray.EMPTY;
		DoubleArray timeTenorArray = DoubleArray.EMPTY;
		DoubleArray alphaArray = DoubleArray.EMPTY;
		DoubleArray rhoArray = DoubleArray.EMPTY;
		DoubleArray nuArray = DoubleArray.EMPTY;
		IList<ParameterMetadata> parameterMetadata = new List<ParameterMetadata>();
		IList<DoubleArray> dataSensitivityAlpha = new List<DoubleArray>(); // Sensitivity to the calibrating data
		IList<DoubleArray> dataSensitivityRho = new List<DoubleArray>();
		IList<DoubleArray> dataSensitivityNu = new List<DoubleArray>();
		foreach (double? timeToExpiry in parameterMetadataTmp.Keys)
		{
		  SortedDictionary<double, ParameterMetadata> parameterMetadataExpiryMap = parameterMetadataTmp[timeToExpiry];
		  SortedDictionary<double, DoubleArray> dataSensitivityAlphaExpiryMap = dataSensitivityAlphaTmp[timeToExpiry];
		  SortedDictionary<double, DoubleArray> dataSensitivityRhoExpiryMap = dataSensitivityRhoTmp[timeToExpiry];
		  SortedDictionary<double, DoubleArray> dataSensitivityNuExpiryMap = dataSensitivityNuTmp[timeToExpiry];
		  SortedDictionary<double, SabrFormulaData> sabrPointExpiryMap = sabrPointTmp[timeToExpiry];
		  foreach (double? timeTenor in parameterMetadataExpiryMap.Keys)
		  {
			parameterMetadata.Add(parameterMetadataExpiryMap[timeTenor]);
			dataSensitivityAlpha.Add(dataSensitivityAlphaExpiryMap[timeTenor]);
			dataSensitivityRho.Add(dataSensitivityRhoExpiryMap[timeTenor]);
			dataSensitivityNu.Add(dataSensitivityNuExpiryMap[timeTenor]);
			timeToExpiryArray = timeToExpiryArray.concat(timeToExpiry);
			timeTenorArray = timeTenorArray.concat(timeTenor);
			SabrFormulaData sabrPt = sabrPointExpiryMap[timeTenor];
			alphaArray = alphaArray.concat(sabrPt.Alpha);
			rhoArray = rhoArray.concat(sabrPt.Rho);
			nuArray = nuArray.concat(sabrPt.Nu);
		  }
		}
		SurfaceMetadata metadataAlpha = Surfaces.sabrParameterByExpiryTenor(name.Name + "-Alpha", dayCount, ValueType.SABR_ALPHA).withParameterMetadata(parameterMetadata);
		SurfaceMetadata metadataRho = Surfaces.sabrParameterByExpiryTenor(name.Name + "-Rho", dayCount, ValueType.SABR_RHO).withParameterMetadata(parameterMetadata);
		SurfaceMetadata metadataNu = Surfaces.sabrParameterByExpiryTenor(name.Name + "-Nu", dayCount, ValueType.SABR_NU).withParameterMetadata(parameterMetadata);
		InterpolatedNodalSurface alphaSurface = InterpolatedNodalSurface.of(metadataAlpha, timeToExpiryArray, timeTenorArray, alphaArray, interpolator);
		InterpolatedNodalSurface rhoSurface = InterpolatedNodalSurface.of(metadataRho, timeToExpiryArray, timeTenorArray, rhoArray, interpolator);
		InterpolatedNodalSurface nuSurface = InterpolatedNodalSurface.of(metadataNu, timeToExpiryArray, timeTenorArray, nuArray, interpolator);
		SabrInterestRateParameters @params = SabrInterestRateParameters.of(alphaSurface, betaSurface, rhoSurface, nuSurface, shiftSurface, sabrVolatilityFormula);
		return SabrParametersSwaptionVolatilities.builder().name(name).convention(convention).valuationDateTime(calibrationDateTime).parameters(@params).dataSensitivityAlpha(dataSensitivityAlpha).dataSensitivityRho(dataSensitivityRho).dataSensitivityNu(dataSensitivityNu).build();
	  }

	  // The main part of the calibration. The calibration is done 4 times with different starting points: low and high
	  // volatilities and high and low vol of vol. The best result (in term of chi^2) is returned.
	  private Pair<SabrFormulaData, DoubleMatrix> calibration(double forward, double shift, double beta, BitArray @fixed, BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, DoubleArray strike, DoubleArray data, Period expiry, RawOptionData rawData)
	  {

		double rhoStart = -0.50 * beta + 0.50 * (1 - beta);
		// Correlation is usually positive for normal and negative for log-normal;.
		double[] alphaStart = new double[4];
		alphaStart[0] = 0.0025 / Math.Pow(forward + shift, beta); // Low vol
		alphaStart[1] = alphaStart[0];
		alphaStart[2] = 4 * alphaStart[0]; // High vol
		alphaStart[3] = alphaStart[2];
		double[] nuStart = new double[4];
		nuStart[0] = 0.10; // Low vol of vol
		nuStart[1] = 0.50; // High vol of vol
		nuStart[2] = 0.10;
		nuStart[3] = 0.50;
		double chi2 = 1.0E+12; // Large number
		Pair<LeastSquareResultsWithTransform, DoubleArray> sabrCalibrationResult = null;
		for (int i = 0; i < 4; i++)
		{ // Try different starting points and take the best
		  DoubleArray startParameters = DoubleArray.of(alphaStart[i], beta, rhoStart, nuStart[i]);
		  Pair<LeastSquareResultsWithTransform, DoubleArray> r = null;
		  if (rawData.DataType.Equals(ValueType.NORMAL_VOLATILITY))
		  {
			r = calibrateLsShiftedFromNormalVolatilities(bda, calibrationDateTime, dayCount, expiry, forward, strike, rawData.StrikeType, data, startParameters, @fixed, shift);
		  }
		  else
		  {
			if (rawData.DataType.Equals(ValueType.PRICE))
			{
			  r = calibrateLsShiftedFromPrices(bda, calibrationDateTime, dayCount, expiry, forward, strike, rawData.StrikeType, data, startParameters, @fixed, shift);
			}
			else
			{
			  if (rawData.DataType.Equals(ValueType.BLACK_VOLATILITY))
			  {
				r = calibrateLsShiftedFromBlackVolatilities(bda, calibrationDateTime, dayCount, expiry, forward, strike, rawData.StrikeType, data, rawData.Shift.GetValueOrDefault(0d), startParameters, @fixed, shift);
			  }
			  else
			  {
				throw new System.ArgumentException("Data type not supported");
			  }
			}
		  }
		  if (r.First.ChiSq < chi2)
		  { // Keep best calibration
			sabrCalibrationResult = r;
			chi2 = r.First.ChiSq;
		  }
		}
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("null") com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData sabrParameters = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData.of(sabrCalibrationResult.getFirst().getModelParameters().toArrayUnsafe());
		SabrFormulaData sabrParameters = SabrFormulaData.of(sabrCalibrationResult.First.ModelParameters.toArrayUnsafe());
		DoubleMatrix parameterSensitivityToBlackShifted = sabrCalibrationResult.First.ModelParameterSensitivityToData;
		DoubleArray blackVolSensitivitytoRawData = sabrCalibrationResult.Second;
		// Multiply the sensitivity to the intermediary (shifted) log-normal vol by its sensitivity to the raw data
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] parameterSensitivityToDataArray = new double[4][blackVolSensitivitytoRawData.size()];
		double[][] parameterSensitivityToDataArray = RectangularArrays.ReturnRectangularDoubleArray(4, blackVolSensitivitytoRawData.size());
		for (int loopsabr = 0; loopsabr < 4; loopsabr++)
		{
		  for (int loopdata = 0; loopdata < blackVolSensitivitytoRawData.size(); loopdata++)
		  {
			parameterSensitivityToDataArray[loopsabr][loopdata] = parameterSensitivityToBlackShifted.get(loopsabr, loopdata) * blackVolSensitivitytoRawData.get(loopdata);
		  }
		}
		DoubleMatrix parameterSensitivityToData = DoubleMatrix.ofUnsafe(parameterSensitivityToDataArray);
		return Pair.of(sabrParameters, parameterSensitivityToData);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate SABR alpha parameters to a set of ATM swaption volatilities.
	  /// <para>
	  /// The SABR parameters are calibrated with all the parameters other than alpha (beta, rhu, nu, shift) fixed.
	  /// The at-the-money volatilities can be log-normal or normal volatilities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <param name="sabr">  the SABR parameters from which the beta, rho, nu and shift are extracted </param>
	  /// <param name="ratesProvider">  the rate provider used to compute the swap forward rates </param>
	  /// <param name="atmVolatilities">  the swaption volatilities containing the ATM volatilities to be calibrated </param>
	  /// <param name="tenors">  the tenors for which the alpha parameter should be calibrated </param>
	  /// <param name="expiries">  the expiries for which the alpha parameter should be calibrated </param>
	  /// <param name="interpolator">  the interpolator for the alpha surface </param>
	  /// <returns> the SABR volatility object </returns>
	  public SabrParametersSwaptionVolatilities calibrateAlphaWithAtm(SwaptionVolatilitiesName name, SabrParametersSwaptionVolatilities sabr, RatesProvider ratesProvider, SwaptionVolatilities atmVolatilities, IList<Tenor> tenors, IList<Period> expiries, SurfaceInterpolator interpolator)
	  {

		int nbTenors = tenors.Count;
		FixedIborSwapConvention convention = sabr.Convention;
		DayCount dayCount = sabr.DayCount;
		BusinessDayAdjustment bda = convention.FloatingLeg.StartDateBusinessDayAdjustment;
		LocalDate calibrationDate = sabr.ValuationDate;
		DoubleArray timeToExpiryArray = DoubleArray.EMPTY;
		DoubleArray timeTenorArray = DoubleArray.EMPTY;
		DoubleArray alphaArray = DoubleArray.EMPTY;
		IList<ParameterMetadata> parameterMetadata = new List<ParameterMetadata>();
		IList<DoubleArray> dataSensitivityAlpha = new List<DoubleArray>(); // Sensitivity to the calibrating data
		int nbExpiries = expiries.Count;
		for (int loopexpiry = 0; loopexpiry < nbExpiries; loopexpiry++)
		{
		  for (int looptenor = 0; looptenor < nbTenors; looptenor++)
		  {
			double timeTenor = tenors[looptenor].Period.Years + tenors[looptenor].Period.Months / 12;
			LocalDate exerciseDate = expirationDate(bda, calibrationDate, expiries[loopexpiry]);
			LocalDate effectiveDate = convention.calculateSpotDateFromTradeDate(exerciseDate, refData);
			double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
			LocalDate endDate = effectiveDate.plus(tenors[looptenor]);
			SwapTrade swap0 = convention.toTrade(calibrationDate, effectiveDate, endDate, BuySell.BUY, 1.0, 0.0);
			double forward = swapPricer.parRate(swap0.Product.resolve(refData), ratesProvider);
			double atmVolatility = atmVolatilities.volatility(timeToExpiry, timeTenor, forward, forward);
			ValueType volatilityType = atmVolatilities.VolatilityType;
			// Currently there is no 'SwaptionVolatilities' with Black shifted.
			double beta = sabr.Parameters.beta(timeToExpiry, timeTenor);
			double rho = sabr.Parameters.rho(timeToExpiry, timeTenor);
			double nu = sabr.Parameters.nu(timeToExpiry, timeTenor);
			double shift = sabr.Parameters.shift(timeToExpiry, timeTenor);
			Pair<double, double> calibrationResult = calibrationAtm(forward, shift, beta, rho, nu, bda, sabr.ValuationDateTime, dayCount, expiries[loopexpiry], atmVolatility, volatilityType);
			timeToExpiryArray = timeToExpiryArray.concat(timeToExpiry);
			timeTenorArray = timeTenorArray.concat(timeTenor);
			alphaArray = alphaArray.concat(calibrationResult.First);
			parameterMetadata.Add(SwaptionSurfaceExpiryTenorParameterMetadata.of(timeToExpiry, timeTenor, expiries[loopexpiry].ToString() + "x" + tenors[looptenor].ToString()));
			dataSensitivityAlpha.Add(DoubleArray.of(calibrationResult.Second));
		  }
		}
		SurfaceMetadata metadataAlpha = Surfaces.sabrParameterByExpiryTenor(name.Name + "-Alpha", dayCount, ValueType.SABR_ALPHA).withParameterMetadata(parameterMetadata);
		InterpolatedNodalSurface alphaSurface = InterpolatedNodalSurface.of(metadataAlpha, timeToExpiryArray, timeTenorArray, alphaArray, interpolator);
		SabrInterestRateParameters @params = SabrInterestRateParameters.of(alphaSurface, sabr.Parameters.BetaSurface, sabr.Parameters.RhoSurface, sabr.Parameters.NuSurface, sabr.Parameters.ShiftSurface, sabrVolatilityFormula);
		return SabrParametersSwaptionVolatilities.builder().name(name).convention(convention).valuationDateTime(sabr.ValuationDateTime).parameters(@params).dataSensitivityAlpha(dataSensitivityAlpha).build();
	  }

	  // Calibration for one option. Distribute the calculation according to the type of volatility (Black/Normal)
	  private Pair<double, double> calibrationAtm(double forward, double shift, double beta, double rho, double nu, BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period expiry, double volatility, ValueType volatilityType)
	  {
		double alphaStart = volatility / Math.Pow(forward + shift, beta);
		DoubleArray startParameters = DoubleArray.of(alphaStart, beta, rho, nu);
		Pair<double, double> r = null;
		if (volatilityType.Equals(ValueType.NORMAL_VOLATILITY))
		{
		  r = calibrateAtmShiftedFromNormalVolatilities(bda, calibrationDateTime, dayCount, expiry, forward, volatility, startParameters, shift);
		}
		else
		{
		  if (volatilityType.Equals(ValueType.BLACK_VOLATILITY))
		  {
			r = calibrateAtmShiftedFromBlackVolatilities(bda, calibrationDateTime, dayCount, expiry, forward, volatility, 0.0, startParameters, shift);
		  }
		  else
		  {
			throw new System.ArgumentException("Data type not supported");
		  }
		}
		return r;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate the SABR parameters to a set of Black volatilities at given moneyness by least square.
	  /// <para>
	  /// All the associated swaptions have the same expiration date, given by a period
	  /// from calibration time, and the same tenor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bda">  the business day adjustment for the exercise date adjustment </param>
	  /// <param name="calibrationDateTime">  the calibration date and time </param>
	  /// <param name="dayCount">  the day count for the computation of the time to exercise </param>
	  /// <param name="periodToExpiry">  the period to expiry </param>
	  /// <param name="forward">  the forward price/rate </param>
	  /// <param name="strikesLike">  the options strike-like dimension </param>
	  /// <param name="strikeType">  the strike type </param>
	  /// <param name="blackVolatilitiesInput">  the option (call/payer) implied volatilities in shifted Black model </param>
	  /// <param name="shiftInput">  the shift used to computed the input implied shifted Black volatilities </param>
	  /// <param name="startParameters">  the starting parameters for the calibration. If one or more of the parameters are fixed,
	  ///   the starting value will be used as the fixed parameter. </param>
	  /// <param name="fixedParameters">  the flag for the fixed parameters that are not calibrated </param>
	  /// <param name="shiftOutput">  the shift to calibrate the shifted SABR </param>
	  /// <returns> the least square results and the derivative of the shifted log-normal used for calibration with respect 
	  ///   to the raw data </returns>
	  public Pair<LeastSquareResultsWithTransform, DoubleArray> calibrateLsShiftedFromBlackVolatilities(BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period periodToExpiry, double forward, DoubleArray strikesLike, ValueType strikeType, DoubleArray blackVolatilitiesInput, double shiftInput, DoubleArray startParameters, BitArray fixedParameters, double shiftOutput)
	  {

		int nbStrikes = strikesLike.size();
		ArgChecker.isTrue(nbStrikes == blackVolatilitiesInput.size(), "size of strikes must be the same as size of volatilities");
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate exerciseDate = expirationDate(bda, calibrationDate, periodToExpiry);
		double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
		DoubleArray errors = DoubleArray.filled(nbStrikes, 1e-4);
		DoubleArray strikes = strikesShifted(forward, 0.0, strikesLike, strikeType);
		Pair<DoubleArray, DoubleArray> volAndDerivatives = blackVolatilitiesShiftedFromBlackVolatilitiesShifted(forward, shiftOutput, timeToExpiry, strikes, blackVolatilitiesInput, shiftInput);
		DoubleArray blackVolatilitiesTransformed = volAndDerivatives.First;
		DoubleArray strikesShifted = this.strikesShifted(forward, shiftOutput, strikesLike, strikeType);
		SabrModelFitter fitter = new SabrModelFitter(forward + shiftOutput, strikesShifted, timeToExpiry, blackVolatilitiesTransformed, errors, sabrVolatilityFormula);
		LeastSquareResultsWithTransform result = fitter.solve(startParameters, fixedParameters);
		return Pair.of(result, volAndDerivatives.Second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate the SABR alpha parameter to an ATM Black volatility and compute the derivative of the result with 
	  /// respect to the input volatility.
	  /// </summary>
	  /// <param name="bda">  the business day adjustment for the exercise date adjustment </param>
	  /// <param name="calibrationDateTime">  the calibration date and time </param>
	  /// <param name="dayCount">  the day count for the computation of the time to exercise </param>
	  /// <param name="periodToExpiry">  the period to expiry </param>
	  /// <param name="forward">  the forward price/rate </param>
	  /// <param name="blackVolatility">  the option (call/payer) Black implied volatility </param>
	  /// <param name="shiftInput">  the shift used to computed the input implied shifted Black volatilities </param>
	  /// <param name="startParameters">  the starting parameters for the calibration. The alpha parameter is used as a starting
	  ///   point for the root-finding, the other parameters are fixed. </param>
	  /// <param name="shiftOutput">  the shift to calibrate the shifted SABR </param>
	  /// <returns> the alpha calibrated and its derivative with respect to the volatility </returns>
	  public Pair<double, double> calibrateAtmShiftedFromBlackVolatilities(BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period periodToExpiry, double forward, double blackVolatility, double shiftInput, DoubleArray startParameters, double shiftOutput)
	  {

		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate exerciseDate = expirationDate(bda, calibrationDate, periodToExpiry);
		double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
		Pair<DoubleArray, DoubleArray> volAndDerivatives = blackVolatilitiesShiftedFromBlackVolatilitiesShifted(forward, shiftOutput, timeToExpiry, DoubleArray.of(forward), DoubleArray.of(blackVolatility), shiftInput);
		DoubleArray blackVolatilitiesTransformed = volAndDerivatives.First;
		System.Func<double, double> volFunction = (a) => sabrVolatilityFormula.volatility(forward + shiftOutput, forward + shiftOutput, timeToExpiry, a, startParameters.get(1), startParameters.get(2), startParameters.get(3)) - blackVolatilitiesTransformed.get(0);
		double alphaCalibrated = ROOT_FINDER.getRoot(volFunction, startParameters.get(0)).Value;
		double dAlphadBlack = 1.0d / sabrVolatilityFormula.volatilityAdjoint(forward + shiftOutput, forward + shiftOutput, timeToExpiry, alphaCalibrated, startParameters.get(1), startParameters.get(2), startParameters.get(3)).getDerivative(2);
		return Pair.of(alphaCalibrated, dAlphadBlack * volAndDerivatives.Second.get(0));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an array of shifted Black volatilities from shifted Black volatilities with a different shift and 
	  /// the sensitivities of the Black volatilities outputs with respect to the normal volatilities inputs.
	  /// </summary>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="shiftOutput">  the shift required in the output </param>
	  /// <param name="timeToExpiry">  the time to expiration </param>
	  /// <param name="strikes">  the option strikes </param>
	  /// <param name="blackVolatilities">  the shifted implied Black volatilities </param>
	  /// <param name="shiftInput">  the shift used in the input Black implied volatilities </param>
	  /// <returns> the shifted black volatilities and their derivatives </returns>
	  public Pair<DoubleArray, DoubleArray> blackVolatilitiesShiftedFromBlackVolatilitiesShifted(double forward, double shiftOutput, double timeToExpiry, DoubleArray strikes, DoubleArray blackVolatilities, double shiftInput)
	  {

		if (shiftInput == shiftOutput)
		{
		  return Pair.of(blackVolatilities, DoubleArray.filled(blackVolatilities.size(), 1.0d));
		  // No change required if shifts are the same
		}
		int nbStrikes = strikes.size();
		double[] impliedVolatility = new double[nbStrikes];
		double[] impliedVolatilityDerivatives = new double[nbStrikes];
		for (int i = 0; i < nbStrikes; i++)
		{
		  ValueDerivatives price = BlackFormulaRepository.priceAdjoint(forward + shiftInput, strikes.get(i) + shiftInput, timeToExpiry, blackVolatilities.get(i), true); // vega-[3]
		  ValueDerivatives iv = BlackFormulaRepository.impliedVolatilityAdjoint(price.Value, forward + shiftOutput, strikes.get(i) + shiftOutput, timeToExpiry, true);
		  impliedVolatility[i] = iv.Value;
		  impliedVolatilityDerivatives[i] = iv.getDerivative(0) * price.getDerivative(3);
		}
		return Pair.of(DoubleArray.ofUnsafe(impliedVolatility), DoubleArray.ofUnsafe(impliedVolatilityDerivatives));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate the SABR parameters to a set of option prices at given moneyness.
	  /// <para>
	  /// All the associated swaptions have the same expiration date, given by a period
	  /// from calibration time, and the same tenor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bda">  the business day adjustment for the exercise date adjustment </param>
	  /// <param name="calibrationDateTime">  the calibration date and time </param>
	  /// <param name="dayCount">  the day count for the computation of the time to exercise </param>
	  /// <param name="periodToExpiry">  the period to expiry </param>
	  /// <param name="forward">  the forward price/rate </param>
	  /// <param name="strikesLike">  the options strike-like dimension </param>
	  /// <param name="strikeType">  the strike type </param>
	  /// <param name="prices">  the option (call/payer) prices </param>
	  /// <param name="startParameters">  the starting parameters for the calibration. If one or more of the parameters are fixed,
	  ///   the starting value will be used as the fixed parameter. </param>
	  /// <param name="fixedParameters">  the flag for the fixed parameters that are not calibrated </param>
	  /// <param name="shiftOutput">  the shift to calibrate the shifted SABR </param>
	  /// <returns> SABR parameters </returns>
	  public Pair<LeastSquareResultsWithTransform, DoubleArray> calibrateLsShiftedFromPrices(BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period periodToExpiry, double forward, DoubleArray strikesLike, ValueType strikeType, DoubleArray prices, DoubleArray startParameters, BitArray fixedParameters, double shiftOutput)
	  {

		int nbStrikes = strikesLike.size();
		ArgChecker.isTrue(nbStrikes == prices.size(), "size of strikes must be the same as size of prices");
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate exerciseDate = expirationDate(bda, calibrationDate, periodToExpiry);
		double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
		DoubleArray errors = DoubleArray.filled(nbStrikes, 1e-4);
		DoubleArray strikes = strikesShifted(forward, 0.0, strikesLike, strikeType);
		Pair<DoubleArray, DoubleArray> volAndDerivatives = blackVolatilitiesShiftedFromPrices(forward, shiftOutput, timeToExpiry, strikes, prices);
		DoubleArray blackVolatilitiesTransformed = volAndDerivatives.First;
		DoubleArray strikesShifted = this.strikesShifted(forward, shiftOutput, strikesLike, strikeType);
		SabrModelFitter fitter = new SabrModelFitter(forward + shiftOutput, strikesShifted, timeToExpiry, blackVolatilitiesTransformed, errors, sabrVolatilityFormula);
		return Pair.of(fitter.solve(startParameters, fixedParameters), volAndDerivatives.Second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an array of shifted Black volatilities from option prices and the sensitivities of the 
	  /// Black volatilities with respect to the price inputs.
	  /// </summary>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="shiftOutput">  the shift required in the output </param>
	  /// <param name="timeToExpiry">  the time to expiration </param>
	  /// <param name="strikes">  the option strikes </param>
	  /// <param name="prices">  the option prices </param>
	  /// <returns> the shifted black volatilities and their derivatives </returns>
	  public Pair<DoubleArray, DoubleArray> blackVolatilitiesShiftedFromPrices(double forward, double shiftOutput, double timeToExpiry, DoubleArray strikes, DoubleArray prices)
	  {

		int nbStrikes = strikes.size();
		double[] impliedVolatility = new double[nbStrikes];
		double[] impliedVolatilityDerivatives = new double[nbStrikes];
		for (int i = 0; i < nbStrikes; i++)
		{
		  ValueDerivatives iv = BlackFormulaRepository.impliedVolatilityAdjoint(prices.get(i), forward + shiftOutput, strikes.get(i) + shiftOutput, timeToExpiry, true);
		  impliedVolatility[i] = iv.Value;
		  impliedVolatilityDerivatives[i] = iv.getDerivative(0);
		}
		return Pair.of(DoubleArray.ofUnsafe(impliedVolatility), DoubleArray.ofUnsafe(impliedVolatilityDerivatives));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate the SABR parameters to a set of normal volatilities at given moneyness.
	  /// <para>
	  /// All the associated swaptions have the same expiration date, given by a period
	  /// from calibration time, and the same tenor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bda">  the business day adjustment for the exercise date adjustment </param>
	  /// <param name="calibrationDateTime">  the calibration date and time </param>
	  /// <param name="dayCount">  the day count for the computation of the time to exercise </param>
	  /// <param name="periodToExpiry">  the period to expiry </param>
	  /// <param name="forward">  the forward price/rate </param>
	  /// <param name="strikesLike">  the options strike-like dimension </param>
	  /// <param name="strikeType">  the strike type </param>
	  /// <param name="normalVolatilities">  the option (call/payer) normal model implied volatilities </param>
	  /// <param name="startParameters">  the starting parameters for the calibration. If one or more of the parameters are fixed,
	  ///   the starting value will be used as the fixed parameter. </param>
	  /// <param name="fixedParameters">  the flag for the fixed parameters that are not calibrated </param>
	  /// <param name="shiftOutput">  the shift to calibrate the shifted SABR </param>
	  /// <returns> the least square results and the derivative of the shifted log-normal used for calibration with respect 
	  ///   to the raw data </returns>
	  public Pair<LeastSquareResultsWithTransform, DoubleArray> calibrateLsShiftedFromNormalVolatilities(BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period periodToExpiry, double forward, DoubleArray strikesLike, ValueType strikeType, DoubleArray normalVolatilities, DoubleArray startParameters, BitArray fixedParameters, double shiftOutput)
	  {

		int nbStrikes = strikesLike.size();
		ArgChecker.isTrue(nbStrikes == normalVolatilities.size(), "size of strikes must be the same as size of prices");
		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate exerciseDate = expirationDate(bda, calibrationDate, periodToExpiry);
		double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
		DoubleArray errors = DoubleArray.filled(nbStrikes, 1e-4);
		DoubleArray strikes = strikesShifted(forward, 0.0, strikesLike, strikeType);
		Pair<DoubleArray, DoubleArray> volAndDerivatives = blackVolatilitiesShiftedFromNormalVolatilities(forward, shiftOutput, timeToExpiry, strikes, normalVolatilities);
		DoubleArray blackVolatilitiesTransformed = volAndDerivatives.First;
		DoubleArray strikesShifted = this.strikesShifted(forward, shiftOutput, strikesLike, strikeType);
		SabrModelFitter fitter = new SabrModelFitter(forward + shiftOutput, strikesShifted, timeToExpiry, blackVolatilitiesTransformed, errors, sabrVolatilityFormula);
		LeastSquareResultsWithTransform result = fitter.solve(startParameters, fixedParameters);
		return Pair.of(result, volAndDerivatives.Second);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate the SABR alpha parameter to an ATM normal volatility and compute the derivative of the result 
	  /// with respect to the input volatility.
	  /// </summary>
	  /// <param name="bda">  the business day adjustment for the exercise date adjustment </param>
	  /// <param name="calibrationDateTime">  the calibration date and time </param>
	  /// <param name="dayCount">  the day count for the computation of the time to exercise </param>
	  /// <param name="periodToExpiry">  the period to expiry </param>
	  /// <param name="forward">  the forward price/rate </param>
	  /// <param name="normalVolatility">  the option (call/payer) normal model implied volatility </param>
	  /// <param name="startParameters">  the starting parameters for the calibration. The alpha parameter is used as a starting
	  ///   point for the root-finding, the other parameters are fixed. </param>
	  /// <param name="shiftOutput">  the shift to calibrate the shifted SABR </param>
	  /// <returns> the alpha calibrated and its derivative with respect to the volatility </returns>
	  public Pair<double, double> calibrateAtmShiftedFromNormalVolatilities(BusinessDayAdjustment bda, ZonedDateTime calibrationDateTime, DayCount dayCount, Period periodToExpiry, double forward, double normalVolatility, DoubleArray startParameters, double shiftOutput)
	  {

		LocalDate calibrationDate = calibrationDateTime.toLocalDate();
		LocalDate exerciseDate = expirationDate(bda, calibrationDate, periodToExpiry);
		double timeToExpiry = dayCount.relativeYearFraction(calibrationDate, exerciseDate);
		Pair<DoubleArray, DoubleArray> volAndDerivatives = blackVolatilitiesShiftedFromNormalVolatilities(forward, shiftOutput, timeToExpiry, DoubleArray.of(forward), DoubleArray.of(normalVolatility));
		DoubleArray blackVolatilitiesTransformed = volAndDerivatives.First;
		System.Func<double, double> volFunction = (a) => sabrVolatilityFormula.volatility(forward + shiftOutput, forward + shiftOutput, timeToExpiry, a, startParameters.get(1), startParameters.get(2), startParameters.get(3)) - blackVolatilitiesTransformed.get(0);
		double alphaCalibrated = ROOT_FINDER.getRoot(volFunction, startParameters.get(0)).Value;
		double dAlphadBlack = 1.0d / sabrVolatilityFormula.volatilityAdjoint(forward + shiftOutput, forward + shiftOutput, timeToExpiry, alphaCalibrated, startParameters.get(1), startParameters.get(2), startParameters.get(3)).getDerivative(2);
		return Pair.of(alphaCalibrated, dAlphadBlack * volAndDerivatives.Second.get(0));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an array of shifted Black volatilities from Normal volatilities and the sensitivities of the 
	  /// Black volatilities with respect to the normal volatilities inputs.
	  /// <para>
	  /// The transformation between normal and Black volatility is done using 
	  /// <seealso cref="BlackFormulaRepository#impliedVolatilityFromNormalApproximated"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="shiftOutput">  the shift required in the output </param>
	  /// <param name="timeToExpiry">  the time to expiration </param>
	  /// <param name="strikes">  the option strikes </param>
	  /// <param name="normalVolatilities">  the normal volatilities </param>
	  /// <returns> the shifted black volatilities and their derivatives </returns>
	  public Pair<DoubleArray, DoubleArray> blackVolatilitiesShiftedFromNormalVolatilities(double forward, double shiftOutput, double timeToExpiry, DoubleArray strikes, DoubleArray normalVolatilities)
	  {

		int nbStrikes = strikes.size();
		double[] impliedVolatility = new double[nbStrikes];
		double[] impliedVolatilityDerivatives = new double[nbStrikes];
		for (int i = 0; i < nbStrikes; i++)
		{
		  ValueDerivatives iv = BlackFormulaRepository.impliedVolatilityFromNormalApproximatedAdjoint(forward + shiftOutput, strikes.get(i) + shiftOutput, timeToExpiry, normalVolatilities.get(i));
		  impliedVolatility[i] = iv.Value;
		  impliedVolatilityDerivatives[i] = iv.getDerivative(0);
		}
		return Pair.of(DoubleArray.ofUnsafe(impliedVolatility), DoubleArray.ofUnsafe(impliedVolatilityDerivatives));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compute shifted strikes from forward and strike-like value type.
	  /// </summary>
	  /// <param name="forward">  the forward rate </param>
	  /// <param name="shiftOutput">  the shift for the output </param>
	  /// <param name="strikesLike">  the strike-like values </param>
	  /// <param name="strikeType">  the strike type </param>
	  /// <returns> the strikes </returns>
	  private DoubleArray strikesShifted(double forward, double shiftOutput, DoubleArray strikesLike, ValueType strikeType)
	  {
		int nbStrikes = strikesLike.size();
		if (strikeType.Equals(ValueType.STRIKE))
		{
		  return DoubleArray.of(nbStrikes, i => strikesLike.get(i) + shiftOutput);
		}
		if (strikeType.Equals(ValueType.SIMPLE_MONEYNESS))
		{
		  return DoubleArray.of(nbStrikes, i => forward + strikesLike.get(i) + shiftOutput);
		}
		if (strikeType.Equals(ValueType.LOG_MONEYNESS))
		{
		  return DoubleArray.of(nbStrikes, i => forward * Math.Exp(strikesLike.get(i)) + shiftOutput);
		}
		throw new System.ArgumentException("Strike type not supported");
	  }

	  /// <summary>
	  /// Calculates the expiration date of a swaption from the calibration date and the underlying swap convention.
	  /// </summary>
	  /// <param name="bda">  the business day convention </param>
	  /// <param name="calibrationDate">  the calibration date </param>
	  /// <param name="expiry">  the period to expiry </param>
	  /// <returns> the date </returns>
	  private LocalDate expirationDate(BusinessDayAdjustment bda, LocalDate calibrationDate, Period expiry)
	  {
		return bda.adjust(calibrationDate.plus(expiry), refData);
	  }

	}

}