using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using LegalEntityDiscountingProvider = com.opengamma.strata.pricer.bond.LegalEntityDiscountingProvider;
	using CreditRatesProvider = com.opengamma.strata.pricer.credit.CreditRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Calculator to obtain the Market Quote sensitivities.
	/// <para>
	/// This needs the <seealso cref="JacobianCalibrationMatrix"/> obtained during curve calibration.
	/// The Market Quote sensitivities are also called Par Rate when the instruments used
	/// in the curve calibration are quoted in rate, e.g. IRS, FRA or OIS.
	/// </para>
	/// </summary>
	public class MarketQuoteSensitivityCalculator
	{

	  /// <summary>
	  /// The default instance.
	  /// </summary>
	  public static readonly MarketQuoteSensitivityCalculator DEFAULT = new MarketQuoteSensitivityCalculator();
	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new OGMatrixAlgebra();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the market quote sensitivities from parameter sensitivity.
	  /// </summary>
	  /// <param name="paramSensitivities">  the curve parameter sensitivities </param>
	  /// <param name="provider">  the rates provider, containing Jacobian calibration information </param>
	  /// <returns> the market quote sensitivities </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(CurrencyParameterSensitivities paramSensitivities, RatesProvider provider)
	  {

		ArgChecker.notNull(paramSensitivities, "paramSensitivities");
		ArgChecker.notNull(provider, "provider");

		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (CurrencyParameterSensitivity paramSens in paramSensitivities.Sensitivities)
		{
		  // find the matching calibration info
		  Curve curve = provider.findData(paramSens.MarketDataName).filter(v => v is Curve).map(v => (Curve) v).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires curve: " + paramSens.MarketDataName));
		  JacobianCalibrationMatrix info = curve.Metadata.findInfo(CurveInfoType.JACOBIAN).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires Jacobian calibration information"));

		  // calculate the market quote sensitivity using the Jacobian
		  DoubleMatrix jacobian = info.JacobianMatrix;
		  DoubleArray paramSensMatrix = paramSens.Sensitivity;
		  DoubleArray marketQuoteSensMatrix = (DoubleArray) MATRIX_ALGEBRA.multiply(paramSensMatrix, jacobian);
		  DoubleArray marketQuoteSens = marketQuoteSensMatrix;

		  // split between different curves
		  IDictionary<CurveName, DoubleArray> split = info.splitValues(marketQuoteSens);
		  foreach (KeyValuePair<CurveName, DoubleArray> entry in split.SetOfKeyValuePairs())
		  {
			CurveName curveName = entry.Key;
			CurrencyParameterSensitivity maketQuoteSens = provider.findData(curveName).map(c => c.createParameterSensitivity(paramSens.Currency, entry.Value)).orElse(CurrencyParameterSensitivity.of(curveName, paramSens.Currency, entry.Value));
			result = result.combinedWith(maketQuoteSens);
		  }
		}
		return result;
	  }

	  /// <summary>
	  /// Calculates the market quote sensitivities from parameter sensitivity.
	  /// <para>
	  /// This calculates the market quote sensitivities of fixed incomes.
	  /// The input parameter sensitivities must be computed based on the legal entity discounting provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="paramSensitivities">  the curve parameter sensitivities </param>
	  /// <param name="provider">  the legal entity discounting provider, containing Jacobian calibration information </param>
	  /// <returns> the market quote sensitivities </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(CurrencyParameterSensitivities paramSensitivities, LegalEntityDiscountingProvider provider)
	  {

		ArgChecker.notNull(paramSensitivities, "paramSensitivities");
		ArgChecker.notNull(provider, "provider");

		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (CurrencyParameterSensitivity paramSens in paramSensitivities.Sensitivities)
		{
		  // find the matching calibration info
		  Curve curve = provider.findData(paramSens.MarketDataName).filter(v => v is Curve).map(v => (Curve) v).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires curve: " + paramSens.MarketDataName));
		  JacobianCalibrationMatrix info = curve.Metadata.findInfo(CurveInfoType.JACOBIAN).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires Jacobian calibration information"));

		  // calculate the market quote sensitivity using the Jacobian
		  DoubleMatrix jacobian = info.JacobianMatrix;
		  DoubleArray paramSensMatrix = paramSens.Sensitivity;
		  DoubleArray marketQuoteSensMatrix = (DoubleArray) MATRIX_ALGEBRA.multiply(paramSensMatrix, jacobian);
		  DoubleArray marketQuoteSens = marketQuoteSensMatrix;

		  // split between different curves
		  IDictionary<CurveName, DoubleArray> split = info.splitValues(marketQuoteSens);
		  foreach (KeyValuePair<CurveName, DoubleArray> entry in split.SetOfKeyValuePairs())
		  {
			CurveName curveName = entry.Key;
			CurrencyParameterSensitivity maketQuoteSens = provider.findData(curveName).map(c => c.createParameterSensitivity(paramSens.Currency, entry.Value)).orElse(CurrencyParameterSensitivity.of(curveName, paramSens.Currency, entry.Value));
			result = result.combinedWith(maketQuoteSens);
		  }
		}
		return result;
	  }

	  /// <summary>
	  /// Calculates the market quote sensitivities from parameter sensitivity.
	  /// <para>
	  /// This calculates the market quote sensitivities of credit derivatives.
	  /// The input parameter sensitivities must be computed based on the credit rates provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="paramSensitivities">  the curve parameter sensitivities </param>
	  /// <param name="provider">  the credit rates provider, containing Jacobian calibration information </param>
	  /// <returns> the market quote sensitivities </returns>
	  public virtual CurrencyParameterSensitivities sensitivity(CurrencyParameterSensitivities paramSensitivities, CreditRatesProvider provider)
	  {

		CurrencyParameterSensitivities result = CurrencyParameterSensitivities.empty();
		foreach (CurrencyParameterSensitivity paramSens in paramSensitivities.Sensitivities)
		{
		  // find the matching calibration info
		  Curve curve = provider.findData(paramSens.MarketDataName).filter(v => v is Curve).map(v => (Curve) v).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires curve: " + paramSens.MarketDataName));
		  JacobianCalibrationMatrix info = curve.Metadata.findInfo(CurveInfoType.JACOBIAN).orElseThrow(() => new System.ArgumentException("Market Quote sensitivity requires Jacobian calibration information"));

		  // calculate the market quote sensitivity using the Jacobian
		  DoubleMatrix jacobian = info.JacobianMatrix;
		  DoubleArray paramSensMatrix = paramSens.Sensitivity;
		  DoubleArray marketQuoteSensMatrix = (DoubleArray) MATRIX_ALGEBRA.multiply(paramSensMatrix, jacobian);
		  DoubleArray marketQuoteSens = marketQuoteSensMatrix;

		  // split between different curves
		  IDictionary<CurveName, DoubleArray> split = info.splitValues(marketQuoteSens);
		  foreach (KeyValuePair<CurveName, DoubleArray> entry in split.SetOfKeyValuePairs())
		  {
			CurveName curveName = entry.Key;
			CurrencyParameterSensitivity maketQuoteSens = provider.findData(curveName).map(c => c.createParameterSensitivity(paramSens.Currency, entry.Value)).orElse(CurrencyParameterSensitivity.of(curveName, paramSens.Currency, entry.Value));
			result = result.combinedWith(maketQuoteSens);
		  }
		}
		return result;
	  }

	}

}