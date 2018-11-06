/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;

	/// <summary>
	/// Formatter for adjustable dates.
	/// </summary>
	internal sealed class AdjustableDateValueFormatter : ValueFormatter<AdjustableDate>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly AdjustableDateValueFormatter INSTANCE = new AdjustableDateValueFormatter();

	  // restricted constructor
	  private AdjustableDateValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(AdjustableDate amount)
	  {
		return amount.Unadjusted.ToString();
	  }

	  public string formatForDisplay(AdjustableDate amount)
	  {
		return amount.Unadjusted.ToString();
	  }

	}

}