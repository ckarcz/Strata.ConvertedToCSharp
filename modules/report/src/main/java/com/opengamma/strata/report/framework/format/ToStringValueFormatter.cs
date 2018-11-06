/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
	/// <summary>
	/// Default formatter which returns the value of {@code toString()} on the object.
	/// </summary>
	internal sealed class ToStringValueFormatter : ValueFormatter<object>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly ToStringValueFormatter INSTANCE = new ToStringValueFormatter();

	  // restricted constructor
	  private ToStringValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(object @object)
	  {
		return @object.ToString();
	  }

	  public string formatForDisplay(object @object)
	  {
		return @object.ToString();
	  }

	}

}