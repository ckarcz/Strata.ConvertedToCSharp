using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ValueType = com.opengamma.strata.market.ValueType;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using TenorRawOptionData = com.opengamma.strata.pricer.option.TenorRawOptionData;

	/// <summary>
	/// Utilities for the different tests related to <seealso cref="SabrSwaptionCalibrator"/>
	/// </summary>
	public class SabrSwaptionCalibratorSmileTestUtils
	{

	  /// <summary>
	  /// Create a <seealso cref="RawOptionData"/> object for calibration from data.
	  /// </summary>
	  /// <param name="tenors">  the list of tenors </param>
	  /// <param name="expiries">  the list of expiries </param>
	  /// <param name="strikeLikeType">  the type of the strike-like dimension </param>
	  /// <param name="strikeLikeData">  the data related to the strike-like dimension </param>
	  /// <param name="dataType">  the type of the data </param>
	  /// <param name="dataArray">  the array with the raw data, including potential Double.NaN for missing data. </param>
	  /// <returns> the raw option data object </returns>
	  public static TenorRawOptionData rawData(IList<Tenor> tenors, IList<Period> expiries, ValueType strikeLikeType, DoubleArray strikeLikeData, ValueType dataType, double[][][] dataArray)
	  {

		IDictionary<Tenor, RawOptionData> raw = new SortedDictionary<Tenor, RawOptionData>();
		for (int looptenor = 0; looptenor < dataArray.Length; looptenor++)
		{
		  DoubleMatrix matrix = DoubleMatrix.ofUnsafe(dataArray[looptenor]);
		  raw[tenors[looptenor]] = RawOptionData.of(expiries, strikeLikeData, strikeLikeType, matrix, dataType);
		}
		return TenorRawOptionData.of(raw);
	  }

	  /// <summary>
	  /// Create a <seealso cref="RawOptionData"/> object for calibration from data and shift one point.
	  /// </summary>
	  /// <param name="tenors">  the list of tenors </param>
	  /// <param name="expiries">  the list of expiries </param>
	  /// <param name="strikeLikeType">  the type of the strike-like dimension </param>
	  /// <param name="strikeLikeData">  the data related to the strike-like dimension </param>
	  /// <param name="dataType">  the type of the data </param>
	  /// <param name="dataArray">  the array with the raw data, including potential Double.NaN for missing data. </param>
	  /// <param name="i">  the index of the tenor to shift </param>
	  /// <param name="j">  the index of the expiry to shift </param>
	  /// <param name="k">  the index of the strike-like dimension to shift </param>
	  /// <param name="shift">  the size of the shift </param>
	  /// <returns> the raw option data object </returns>
	  public static TenorRawOptionData rawDataShiftPoint(IList<Tenor> tenors, IList<Period> expiries, ValueType strikeLikeType, DoubleArray strikeLikeData, ValueType dataType, double[][][] dataArray, int i, int j, int k, double shift)
	  {

		IDictionary<Tenor, RawOptionData> raw = new SortedDictionary<Tenor, RawOptionData>();
		for (int looptenor = 0; looptenor < dataArray.Length; looptenor++)
		{
		  double[][] shiftedData = java.util.dataArray[looptenor].Select(row => row.clone()).ToArray(l => new double[l][]); // deep copy of 2d array
		  if (looptenor == i)
		  {
			shiftedData[j][k] += shift;
		  }
		  DoubleMatrix matrix = DoubleMatrix.ofUnsafe(shiftedData);
		  raw[tenors[looptenor]] = RawOptionData.of(expiries, strikeLikeData, strikeLikeType, matrix, dataType);
		}
		return TenorRawOptionData.of(raw);
	  }

	  /// <summary>
	  /// Create a <seealso cref="RawOptionData"/> object for calibration from data and shift one smile.
	  /// </summary>
	  /// <param name="tenors">  the list of tenors </param>
	  /// <param name="expiries">  the list of expiries </param>
	  /// <param name="strikeLikeType">  the type of the strike-like dimension </param>
	  /// <param name="strikeLikeData">  the data related to the strike-like dimension </param>
	  /// <param name="dataType">  the type of the data </param>
	  /// <param name="dataArray">  the array with the raw data, including potential Double.NaN for missing data. </param>
	  /// <param name="i">  the index of the tenor to shift </param>
	  /// <param name="j">  the index of the expiry to shift </param>
	  /// <param name="shift">  the size of the shift </param>
	  /// <returns> the raw option data object </returns>
	  public static TenorRawOptionData rawDataShiftSmile(IList<Tenor> tenors, IList<Period> expiries, ValueType strikeLikeType, DoubleArray strikeLikeData, ValueType dataType, double[][][] dataArray, int i, int j, double shift)
	  {

		IDictionary<Tenor, RawOptionData> raw = new SortedDictionary<Tenor, RawOptionData>();
		for (int looptenor = 0; looptenor < dataArray.Length; looptenor++)
		{
		  double[][] shiftedData = java.util.dataArray[looptenor].Select(row => row.clone()).ToArray(l => new double[l][]); // deep copy of 2d array
		  if (looptenor == i)
		  {
			for (int k = 0; k < shiftedData[j].Length; k++)
			{
			  shiftedData[j][k] += shift;
			}
		  }
		  DoubleMatrix matrix = DoubleMatrix.ofUnsafe(shiftedData);
		  raw[tenors[looptenor]] = RawOptionData.of(expiries, strikeLikeData, strikeLikeType, matrix, dataType);
		}
		return TenorRawOptionData.of(raw);
	  }

	  /// <summary>
	  /// Check that the results are acceptable by checking that the absolute difference or the relative difference
	  /// is below a given threshold.
	  /// </summary>
	  /// <param name="value1">  the first value </param>
	  /// <param name="value2">  the second value to compare </param>
	  /// <param name="tolerance">  the tolerance </param>
	  /// <param name="msg">  the message to return in case of failure </param>
	  public static void checkAcceptable(double value1, double value2, double tolerance, string msg)
	  {
		assertTrue((Math.Abs(value1 - value2) < tolerance) || (Math.Abs((value1 - value2) / value2) < tolerance), msg);
	  }

	}

}