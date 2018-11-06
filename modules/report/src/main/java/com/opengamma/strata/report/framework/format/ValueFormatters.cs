/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Provides standard formatters.
	/// <para>
	/// Each formatter implements <seealso cref="ValueFormatter"/>.
	/// </para>
	/// </summary>
	public sealed class ValueFormatters
	{

	  /// <summary>
	  /// The default formatter that returns the value of the {@code toString()} method.
	  /// </summary>
	  public static readonly ValueFormatter<object> TO_STRING = ToStringValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used for {@code AdjustableDate}, printing the unadjusted date.
	  /// </summary>
	  public static readonly ValueFormatter<AdjustableDate> ADJUSTABLE_DATE = AdjustableDateValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used for {@code CurrencyAmount}.
	  /// </summary>
	  public static readonly ValueFormatter<CurrencyAmount> CURRENCY_AMOUNT = CurrencyAmountValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used for {@code CurrencyParameterSensitivity}.
	  /// </summary>
	  public static readonly ValueFormatter<CurrencyParameterSensitivity> CURRENCY_PARAMETER_SENSITIVITY = CurrencyParameterSensitivityValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used for {@code double[]}.
	  /// </summary>
	  public static readonly ValueFormatter<double[]> DOUBLE_ARRAY = DoubleArrayValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used for {@code double}.
	  /// </summary>
	  public static readonly ValueFormatter<double> DOUBLE = DoubleValueFormatter.INSTANCE;
	  /// <summary>
	  /// The formatter to be used when no specific formatter exists for the object.
	  /// </summary>
	  public static readonly ValueFormatter<object> UNSUPPORTED = UnsupportedValueFormatter.INSTANCE;

	  // restricted constructor
	  private ValueFormatters()
	  {

	  }

	}

}