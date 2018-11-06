/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
	/// <summary>
	/// Formats a value into a string.
	/// <para>
	/// See <seealso cref="ValueFormatters"/> for common implementations.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the value </param>
	public interface ValueFormatter<T>
	{

	  /// <summary>
	  /// Formats a value for use in a CSV file.
	  /// <para>
	  /// Typically this retains all information from the object and keeps the representation compact.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="object">  the object to format. </param>
	  /// <returns> the object formatted into a string </returns>
	  string formatForCsv(T @object);

	  /// <summary>
	  /// Formats a value for display.
	  /// <para>
	  /// Typically this may add characters intended to make the value easier to read, or perform
	  /// rounding on numeric values.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="object">  the object to format </param>
	  /// <returns> the object formatted into a string </returns>
	  string formatForDisplay(T @object);

	}

}