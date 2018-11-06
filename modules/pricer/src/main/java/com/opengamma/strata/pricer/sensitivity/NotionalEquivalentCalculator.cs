using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Calculator to obtain the notional equivalent.
	/// <para>
	/// This needs the <seealso cref="DoubleArray"/> with present value sensitivity to
	/// market quotes obtained during curve calibration to be available.
	/// </para>
	/// </summary>
	public class NotionalEquivalentCalculator
	{

	  /// <summary>
	  /// The default instance.
	  /// </summary>
	  public static readonly NotionalEquivalentCalculator DEFAULT = new NotionalEquivalentCalculator();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the notional equivalent from the present value market quote sensitivities.
	  /// <para>
	  /// The notional equivalent is the notional in each instrument used to calibrate the curves to have the same
	  /// sensitivity as the one of the portfolio described by the market quote sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketQuoteSensitivities">  the market quote sensitivities </param>
	  /// <param name="provider">  the rates provider, containing sensitivity information </param>
	  /// <returns> the notionals </returns>
	  public virtual CurrencyParameterSensitivities notionalEquivalent(CurrencyParameterSensitivities marketQuoteSensitivities, RatesProvider provider)
	  {

		IList<CurrencyParameterSensitivity> equivalentList = new List<CurrencyParameterSensitivity>();
		foreach (CurrencyParameterSensitivity s in marketQuoteSensitivities.Sensitivities)
		{
		  ArgChecker.isTrue(s.MarketDataName is CurveName, "curve name");
		  CurveName name = (CurveName) s.MarketDataName;
		  Optional<Curve> curveOpt = provider.findData(name);
		  ArgChecker.isTrue(curveOpt.Present, "Curve {} in the sensitiivty is not present in the provider", name);
		  Curve curve = curveOpt.get();
		  Optional<DoubleArray> pvSensiOpt = curve.Metadata.findInfo(CurveInfoType.PV_SENSITIVITY_TO_MARKET_QUOTE);
		  ArgChecker.isTrue(pvSensiOpt.Present, "Present value sensitivity curve info is required");
		  DoubleArray pvSensi = pvSensiOpt.get();
		  double[] notionalArray = new double[pvSensi.size()];
		  for (int i = 0; i < pvSensi.size(); i++)
		  {
			notionalArray[i] = s.Sensitivity.get(i) / pvSensi.get(i);
		  }
		  DoubleArray notional = DoubleArray.ofUnsafe(notionalArray);
		  equivalentList.Add(CurrencyParameterSensitivity.of(name, s.ParameterMetadata, s.Currency, notional));
		}
		return CurrencyParameterSensitivities.of(equivalentList);
	  }

	}

}