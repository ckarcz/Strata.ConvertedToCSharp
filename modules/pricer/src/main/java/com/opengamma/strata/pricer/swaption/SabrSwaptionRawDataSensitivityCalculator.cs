using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using SurfaceName = com.opengamma.strata.market.surface.SurfaceName;

	/// <summary>
	/// Calculator to obtain the raw data sensitivities for swaption related products using calibrated SABR data.
	/// <para>
	/// This needs data sensitivity info obtained during curve calibration.
	/// </para>
	/// </summary>
	public class SabrSwaptionRawDataSensitivityCalculator
	{

	  /// <summary>
	  /// The default instance.
	  /// </summary>
	  public static readonly SabrSwaptionRawDataSensitivityCalculator DEFAULT = new SabrSwaptionRawDataSensitivityCalculator();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the raw data sensitivities from SABR parameter sensitivity.
	  /// <para>
	  /// The SABR parameter sensitivities to data are stored in some optional data in the 
	  /// <seealso cref="SabrParametersSwaptionVolatilities"/>.
	  /// The sensitivities to the SABR parameters passed in should be compatible with the SABR parameters in term of data order.
	  /// </para>
	  /// <para>
	  /// Only the sensitivity to the SABR parameters for which there is a data sensitivity are taken into account.
	  /// At least one of the four parameter must have such sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="paramSensitivities">  the curve SABR parameter sensitivities </param>
	  /// <param name="volatilities">  the SABR parameters, including the data sensitivity metadata </param>
	  /// <returns> the raw data sensitivities </returns>
	  public virtual CurrencyParameterSensitivity parallelSensitivity(CurrencyParameterSensitivities paramSensitivities, SabrParametersSwaptionVolatilities volatilities)
	  {

		IList<IList<DoubleArray>> sensitivityToRawData = new List<IList<DoubleArray>>(4);
		Optional<ImmutableList<DoubleArray>> alphaInfo = volatilities.DataSensitivityAlpha;
		sensitivityToRawData.Add(alphaInfo.orElse(null));
		Optional<ImmutableList<DoubleArray>> betaInfo = volatilities.DataSensitivityBeta;
		sensitivityToRawData.Add(betaInfo.orElse(null));
		Optional<ImmutableList<DoubleArray>> rhoInfo = volatilities.DataSensitivityRho;
		sensitivityToRawData.Add(rhoInfo.orElse(null));
		Optional<ImmutableList<DoubleArray>> nuInfo = volatilities.DataSensitivityNu;
		sensitivityToRawData.Add(nuInfo.orElse(null));
		ArgChecker.isTrue(alphaInfo.Present || betaInfo.Present || rhoInfo.Present || nuInfo.Present, "at least one sensitivity to raw data must be available");
		checkCurrency(paramSensitivities);
		int nbSurfaceNode = sensitivityToRawData[0].Count;
		double[] sensitivityRawArray = new double[nbSurfaceNode];
		Currency ccy = null;
		IList<ParameterMetadata> metadataResult = null;
		foreach (CurrencyParameterSensitivity s in paramSensitivities.Sensitivities)
		{
		  ccy = s.Currency;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataName<?> name = s.getMarketDataName();
		  MarketDataName<object> name = s.MarketDataName;
		  if (name is SurfaceName)
		  {
			if (volatilities.Parameters.AlphaSurface.Name.Equals(name) && alphaInfo.Present)
			{
			  updateSensitivity(s, sensitivityToRawData[0], sensitivityRawArray);
			  metadataResult = s.ParameterMetadata;
			}
			if (volatilities.Parameters.BetaSurface.Name.Equals(name) && betaInfo.Present)
			{
			  updateSensitivity(s, sensitivityToRawData[1], sensitivityRawArray);
			  metadataResult = s.ParameterMetadata;
			}
			if (volatilities.Parameters.RhoSurface.Name.Equals(name) && rhoInfo.Present)
			{
			  updateSensitivity(s, sensitivityToRawData[2], sensitivityRawArray);
			  metadataResult = s.ParameterMetadata;
			}
			if (volatilities.Parameters.NuSurface.Name.Equals(name) && nuInfo.Present)
			{
			  updateSensitivity(s, sensitivityToRawData[3], sensitivityRawArray);
			  metadataResult = s.ParameterMetadata;
			}
		  }
		}
		DoubleArray sensitivityRaw = DoubleArray.ofUnsafe(sensitivityRawArray);
		return CurrencyParameterSensitivity.of(SurfaceName.of("RawDataParallelSensitivity"), metadataResult, ccy, sensitivityRaw);
	  }

	  // Update the parallel sensitivity for one of the SABR parameters
	  private static void updateSensitivity(CurrencyParameterSensitivity s, IList<DoubleArray> sensitivityInfoParam, double[] sensitivityRawArray)
	  {

		int nbSurfaceNode = sensitivityInfoParam.Count;
		ArgChecker.isTrue(s.Sensitivity.size() == nbSurfaceNode, "sensitivity and surface info are not of the same size");
		for (int loopnode = 0; loopnode < nbSurfaceNode; loopnode++)
		{
		  double sum = sensitivityInfoParam[loopnode].sum();
		  sensitivityRawArray[loopnode] += s.Sensitivity.get(loopnode) * sum;
		}
	  }

	  // Check that all the sensitivities are in the same currency
	  private static void checkCurrency(CurrencyParameterSensitivities paramSensitivities)
	  {
		IList<CurrencyParameterSensitivity> sensitivitiesList = paramSensitivities.Sensitivities;
		if (sensitivitiesList.Count > 0)
		{ // When no sensitivity, no check required.
		  Currency ccy = sensitivitiesList[0].Currency;
		  for (int i = 1; i < sensitivitiesList.Count; i++)
		  {
			ArgChecker.isTrue(ccy.Equals(sensitivitiesList[i].Currency), "sensitivities must be in the same currency for aggregation");
		  }
		}
	  }

	}

}