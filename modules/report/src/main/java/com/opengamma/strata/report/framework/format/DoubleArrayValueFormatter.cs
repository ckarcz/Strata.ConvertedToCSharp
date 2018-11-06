/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{

	/// <summary>
	/// Formats primitive double arrays.
	/// </summary>
	internal sealed class DoubleArrayValueFormatter : ValueFormatter<double[]>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly DoubleArrayValueFormatter INSTANCE = new DoubleArrayValueFormatter();

	  // restricted constructor
	  private DoubleArrayValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the array delimited by spaces and surrounded with square brackets.
	  /// <pre>
	  ///   new double[]{1, 2, 3} -> "[1.0 2.0 3.0]"
	  /// </pre>
	  /// </summary>
	  /// <param name="array">  an array </param>
	  /// <returns> the array formatted for inclusion in a CSV file - space delimited and surrounded with square brackets </returns>
	  public string formatForCsv(double[] array)
	  {
		return Arrays.stream(array).mapToObj(double?.toString).collect(joining(" ", "[", "]"));
	  }

	  /// <summary>
	  /// Returns the array delimited by commas and spaces and surrounded with square brackets.
	  /// <pre>
	  ///   new double[]{1, 2, 3} -> "[1.0, 2.0, 3.0]"
	  /// </pre>
	  /// </summary>
	  /// <param name="array">  an array </param>
	  /// <returns> the array formatted for display - comma delimited and surrounded with square brackets </returns>
	  public string formatForDisplay(double[] array)
	  {
		return Arrays.ToString(array);
	  }
	}

}