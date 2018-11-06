/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Formatter for currency amounts.
	/// </summary>
	internal sealed class CurrencyAmountValueFormatter : ValueFormatter<CurrencyAmount>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly CurrencyAmountValueFormatter INSTANCE = new CurrencyAmountValueFormatter();

	  /// <summary>
	  /// The underlying formatter.
	  /// </summary>
	  private readonly DoubleValueFormatter doubleFormatter = DoubleValueFormatter.INSTANCE;

	  // restricted constructor
	  private CurrencyAmountValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(CurrencyAmount amount)
	  {
		return doubleFormatter.formatForCsv(amount.Amount);
	  }

	  public string formatForDisplay(CurrencyAmount amount)
	  {
		return doubleFormatter.formatForDisplay(amount.Amount, amount.Currency.MinorUnitDigits);
	  }

	}

}