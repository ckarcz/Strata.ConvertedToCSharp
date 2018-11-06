using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.sensitivity
{


	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using CommonsMatrixAlgebra = com.opengamma.strata.math.impl.matrix.CommonsMatrixAlgebra;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Utilities to transform sensitivities.
	/// </summary>
	public class CurveSensitivityUtils
	{
	  /// <summary>
	  /// The matrix algebra used for matrix inversion.
	  /// </summary>
	  private static readonly MatrixAlgebra MATRIX_ALGEBRA = new CommonsMatrixAlgebra();

	  /// <summary>
	  /// Construct the inverse Jacobian matrix from the sensitivities of the trades market quotes to the curve parameters.
	  /// <para>
	  /// All the trades and sensitivities must be in the same currency. The data should be coherent with the
	  /// market quote sensitivities passed in an order coherent with the list of curves.
	  /// </para>
	  /// <para>
	  /// For each trade describing the market quotes, the sensitivity provided should be the sensitivity of that
	  /// market quote to the curve parameters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveOrder">  the order in which the curves should be represented in the jacobian </param>
	  /// <param name="marketQuoteSensitivities">  the market quotes sensitivity to the curve parameters </param>
	  /// <returns> inverse jacobian matrix, which correspond to the sensitivity of the parameters to the market quotes </returns>
	  public static DoubleMatrix jacobianFromMarketQuoteSensitivities(IList<CurveParameterSize> curveOrder, IList<CurrencyParameterSensitivities> marketQuoteSensitivities)
	  {

		Currency ccy = marketQuoteSensitivities[0].Sensitivities.get(0).Currency;
		DoubleMatrix jacobianMatrix = DoubleMatrix.ofArrayObjects(marketQuoteSensitivities.Count, marketQuoteSensitivities.Count, i => row(curveOrder, marketQuoteSensitivities[i], ccy));
		return MATRIX_ALGEBRA.getInverse(jacobianMatrix);
	  }

	  /// <summary>
	  /// Computes the row corresponding to a trade for the Jacobian matrix.
	  /// </summary>
	  /// <param name="curveOrder">  the curve order </param>
	  /// <param name="parameterSensitivities">  the sensitivities </param>
	  /// <param name="ccy">  the currency common to all sensitivities </param>
	  /// <returns> the row </returns>
	  private static DoubleArray row(IList<CurveParameterSize> curveOrder, CurrencyParameterSensitivities parameterSensitivities, Currency ccy)
	  {

		DoubleArray row = DoubleArray.EMPTY;
		foreach (CurveParameterSize curveNameAndSize in curveOrder)
		{
		  Optional<CurrencyParameterSensitivity> sensitivityOneCurve = parameterSensitivities.findSensitivity(curveNameAndSize.Name, ccy);
		  if (sensitivityOneCurve.Present)
		  {
			row = row.concat(sensitivityOneCurve.get().Sensitivity);
		  }
		  else
		  {
			row = row.concat(DoubleArray.filled(curveNameAndSize.ParameterCount));
		  }
		}
		return row;
	  }

	  /// <summary>
	  /// Construct the inverse Jacobian matrix from the trades and a function used to compute the sensitivities of the 
	  /// market quotes to the curve parameters.
	  /// <para>
	  /// All the trades must be in the same currency. The trades should be coherent with the curves order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveOrder">  the order in which the curves should be represented in the jacobian </param>
	  /// <param name="trades">  the list of trades </param>
	  /// <param name="sensitivityFunction">  the function from a trade to the market quote sensitivity to curve parameters </param>
	  /// <returns> inverse jacobian matrix, which correspond to the sensitivity of the parameters to the market quotes </returns>
	  public static DoubleMatrix jacobianFromMarketQuoteSensitivities(IList<CurveParameterSize> curveOrder, IList<ResolvedTrade> trades, System.Func<ResolvedTrade, CurrencyParameterSensitivities> sensitivityFunction)
	  {

		IList<CurrencyParameterSensitivities> marketQuoteSensitivities = new List<CurrencyParameterSensitivities>();
		foreach (ResolvedTrade t in trades)
		{
		  marketQuoteSensitivities.Add(sensitivityFunction(t));
		}
		return jacobianFromMarketQuoteSensitivities(curveOrder, marketQuoteSensitivities);
	  }

	  /// <summary>
	  /// Re-buckets a <seealso cref="CurrencyParameterSensitivities"/> to a given set of dates.
	  /// <para>
	  /// The list of dates must be sorted in chronological order. All sensitivities are re-bucketed to the same date list.
	  /// The re-bucketing is done by linear weighting on the number of days, i.e. the sensitivities for dates outside the 
	  /// extremes are fully bucketed to the extremes and for date between two re-bucketing dates, the weight on the start 
	  /// date is the number days between end date and the date re-bucketed divided by the number of days between the 
	  /// start and the end.
	  /// The input sensitivity should have a <seealso cref="DatedParameterMetadata"/> for each sensitivity.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the input sensitivities </param>
	  /// <param name="targetDates">  the list of dates for the re-bucketing </param>
	  /// <returns> the sensitivity after the re-bucketing </returns>
	  public static CurrencyParameterSensitivities linearRebucketing(CurrencyParameterSensitivities sensitivities, IList<LocalDate> targetDates)
	  {

		checkSortedDates(targetDates);
		int nbBuckets = targetDates.Count;
		IList<ParameterMetadata> pmdTarget = targetDates.Select(date => LabelDateParameterMetadata.of(date, date.ToString())).ToList();
		ImmutableList<CurrencyParameterSensitivity> sensitivitiesList = sensitivities.Sensitivities;
		IList<CurrencyParameterSensitivity> sensitivityTarget = new List<CurrencyParameterSensitivity>();
		foreach (CurrencyParameterSensitivity sensitivity in sensitivitiesList)
		{
		  double[] rebucketedSensitivityAmounts = new double[nbBuckets];
		  DoubleArray sensitivityAmounts = sensitivity.Sensitivity;
		  IList<ParameterMetadata> parameterMetadataList = sensitivity.ParameterMetadata;
		  for (int loopnode = 0; loopnode < sensitivityAmounts.size(); loopnode++)
		  {
			ParameterMetadata nodeMetadata = parameterMetadataList[loopnode];
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			ArgChecker.isTrue(nodeMetadata is DatedParameterMetadata, "re-bucketing requires sensitivity date for node {} which is of type {} while 'DatedParameterMetadata' is expected", nodeMetadata.Label, nodeMetadata.GetType().FullName);
			DatedParameterMetadata datedParameterMetadata = (DatedParameterMetadata) nodeMetadata;
			LocalDate nodeDate = datedParameterMetadata.Date;
			rebucketingArray(targetDates, rebucketedSensitivityAmounts, sensitivityAmounts.get(loopnode), nodeDate);
		  }
		  CurrencyParameterSensitivity rebucketedSensitivity = CurrencyParameterSensitivity.of(sensitivity.MarketDataName, pmdTarget, sensitivity.Currency, DoubleArray.ofUnsafe(rebucketedSensitivityAmounts));
		  sensitivityTarget.Add(rebucketedSensitivity);
		}
		return CurrencyParameterSensitivities.of(sensitivityTarget);
	  }

	  /// <summary>
	  /// Re-buckets a <seealso cref="CurrencyParameterSensitivities"/> to a given set of dates.
	  /// <para>
	  /// The list of dates must be sorted in chronological order. All sensitivities are re-bucketed to the same date list.
	  /// The re-bucketing is done by linear weighting on the number of days, i.e. the sensitivities for dates outside the 
	  /// extremes are fully bucketed to the extremes and for date between two re-bucketing dates, the weight on the start 
	  /// date is the number days between end date and the date re-bucketed divided by the number of days between the 
	  /// start and the end. The date of the nodes can be directly in the parameter metadata - when the metadata is of the 
	  /// type <seealso cref="DatedParameterMetadata"/> - or inferred from the sensitivity date and the tenor when the
	  /// metadata is of the type <seealso cref="TenorParameterMetadata"/>. Only those types of metadata are accepted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the input sensitivities </param>
	  /// <param name="targetDates">  the list of dates for the re-bucketing </param>
	  /// <param name="sensitivityDate">  the date for which the sensitivities are valid </param>
	  /// <returns> the sensitivity after the re-bucketing </returns>
	  public static CurrencyParameterSensitivities linearRebucketing(CurrencyParameterSensitivities sensitivities, IList<LocalDate> targetDates, LocalDate sensitivityDate)
	  {

		checkSortedDates(targetDates);
		int nbBuckets = targetDates.Count;
		IList<ParameterMetadata> pmdTarget = targetDates.Select(date => LabelDateParameterMetadata.of(date, date.ToString())).ToList();
		ImmutableList<CurrencyParameterSensitivity> sensitivitiesList = sensitivities.Sensitivities;
		IList<CurrencyParameterSensitivity> sensitivityTarget = new List<CurrencyParameterSensitivity>();
		foreach (CurrencyParameterSensitivity sensitivity in sensitivitiesList)
		{
		  double[] rebucketedSensitivityAmounts = new double[nbBuckets];
		  DoubleArray sensitivityAmounts = sensitivity.Sensitivity;
		  IList<ParameterMetadata> parameterMetadataList = sensitivity.ParameterMetadata;
		  for (int loopnode = 0; loopnode < sensitivityAmounts.size(); loopnode++)
		  {
			ParameterMetadata nodeMetadata = parameterMetadataList[loopnode];
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
			ArgChecker.isTrue((nodeMetadata is DatedParameterMetadata) || (nodeMetadata is TenorParameterMetadata), "re-bucketing requires sensitivity date or node for node {} which is of type {}", nodeMetadata.Label, nodeMetadata.GetType().FullName);
			LocalDate nodeDate;
			if (nodeMetadata is DatedParameterMetadata)
			{
			  DatedParameterMetadata datedParameterMetadata = (DatedParameterMetadata) nodeMetadata;
			  nodeDate = datedParameterMetadata.Date;
			}
			else
			{
			  TenorParameterMetadata tenorParameterMetadata = (TenorParameterMetadata) nodeMetadata;
			  nodeDate = sensitivityDate.plus(tenorParameterMetadata.Tenor);
			}
			rebucketingArray(targetDates, rebucketedSensitivityAmounts, sensitivityAmounts.get(loopnode), nodeDate);
		  }
		  CurrencyParameterSensitivity rebucketedSensitivity = CurrencyParameterSensitivity.of(sensitivity.MarketDataName, pmdTarget, sensitivity.Currency, DoubleArray.ofUnsafe(rebucketedSensitivityAmounts));
		  sensitivityTarget.Add(rebucketedSensitivity);
		}
		return CurrencyParameterSensitivities.of(sensitivityTarget);
	  }

	  /// <summary>
	  /// Re-bucket one sensitivity at a specific date and add it to an existing array.
	  /// </summary>
	  /// <param name="targetDates">  the list of dates for the re-bucketing </param>
	  /// <param name="rebucketedSensitivityAmounts">  the array of sensitivities; the array is modified by the method </param>
	  /// <param name="sensitivityAmount">  the value of the sensitivity at the given data </param>
	  /// <param name="sensitivityDate">  the date associated to the amount to re-bucket </param>
	  private static void rebucketingArray(IList<LocalDate> targetDates, double[] rebucketedSensitivityAmounts, double sensitivityAmount, LocalDate sensitivityDate)
	  {

		int nbBuckets = targetDates.Count;
		if (!sensitivityDate.isAfter(targetDates[0]))
		{
		  rebucketedSensitivityAmounts[0] += sensitivityAmount;
		}
		else if (!sensitivityDate.isBefore(targetDates[nbBuckets - 1]))
		{
		  rebucketedSensitivityAmounts[nbBuckets - 1] += sensitivityAmount;
		}
		else
		{
		  int indexSensitivityDate = 0;
		  while (sensitivityDate.isAfter(targetDates[indexSensitivityDate]))
		  {
			indexSensitivityDate++;
		  } // 'indexSensitivityDate' contains the index of the node after the sensitivity date
		  long intervalLength = targetDates[indexSensitivityDate].toEpochDay() - targetDates[indexSensitivityDate - 1].toEpochDay();
		  double weight = ((double)(targetDates[indexSensitivityDate].toEpochDay() - sensitivityDate.toEpochDay())) / intervalLength;
		  rebucketedSensitivityAmounts[indexSensitivityDate - 1] += weight * sensitivityAmount;
		  rebucketedSensitivityAmounts[indexSensitivityDate] += (1.0d - weight) * sensitivityAmount;
		}
	  }

	  // Check that the dates in the list are sorted in chronological order.
	  private static void checkSortedDates(IList<LocalDate> dates)
	  {
		for (int loopdate = 0; loopdate < dates.Count - 1; loopdate++)
		{
		  ArgChecker.inOrderNotEqual(dates[loopdate], dates[loopdate + 1], "first date", "following date");
		}
	  }

	}

}