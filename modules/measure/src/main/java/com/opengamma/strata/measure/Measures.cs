/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Measure = com.opengamma.strata.calc.Measure;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

	/// <summary>
	/// The standard set of measures that can be calculated by Strata.
	/// <para>
	/// A measure identifies the calculation result that is required.
	/// For example present value, par rate or spread.
	/// </para>
	/// <para>
	/// Note that not all measures will be available for all targets.
	/// </para>
	/// </summary>
	public sealed class Measures
	{

	  /// <summary>
	  /// Measure representing the present value of the calculation target.
	  /// <para>
	  /// The result is a single currency monetary amount in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PRESENT_VALUE = Measure.of(StandardMeasures.PRESENT_VALUE.Name);
	  /// <summary>
	  /// Measure representing a break-down of the present value calculation on the target.
	  /// <para>
	  /// No currency conversion is performed on the monetary amounts.
	  /// </para>
	  /// </summary>
	  public static readonly Measure EXPLAIN_PRESENT_VALUE = Measure.of(StandardMeasures.EXPLAIN_PRESENT_VALUE.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the calibrated sum PV01 on the calculation target.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated data structure.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// It is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PV01_CALIBRATED_SUM = Measure.of(StandardMeasures.PV01_CALIBRATED_SUM.Name);
	  /// <summary>
	  /// Measure representing the calibrated bucketed PV01 on the calculation target.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the calibrated data structure.
	  /// The result is provided for each affected curve and currency, bucketed by parameter.
	  /// It is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PV01_CALIBRATED_BUCKETED = Measure.of(StandardMeasures.PV01_CALIBRATED_BUCKETED.Name);
	  /// <summary>
	  /// Measure representing the market quote sum PV01 on the calculation target.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the
	  /// market quotes used to calibrate the data structure.
	  /// The result is the sum of the sensitivities of all affected curves.
	  /// It is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PV01_MARKET_QUOTE_SUM = Measure.of(StandardMeasures.PV01_MARKET_QUOTE_SUM.Name);
	  /// <summary>
	  /// Measure representing the market quote bucketed PV01 on the calculation target.
	  /// <para>
	  /// This is the sensitivity of present value to a one basis point shift in the
	  /// market quotes used to calibrate the data structure.
	  /// The result is provided for each affected curve and currency, bucketed by parameter.
	  /// It is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PV01_MARKET_QUOTE_BUCKETED = Measure.of(StandardMeasures.PV01_MARKET_QUOTE_BUCKETED.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the par rate of the calculation target.
	  /// </summary>
	  public static readonly Measure PAR_RATE = Measure.of(StandardMeasures.PAR_RATE.Name);
	  /// <summary>
	  /// Measure representing the par spread of the calculation target.
	  /// </summary>
	  public static readonly Measure PAR_SPREAD = Measure.of(StandardMeasures.PAR_SPREAD.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the present value of each leg of the calculation target.
	  /// <para>
	  /// The result is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure LEG_PRESENT_VALUE = Measure.of(StandardMeasures.LEG_PRESENT_VALUE.Name);
	  /// <summary>
	  /// Measure representing the initial notional amount of each leg of the calculation target.
	  /// <para>
	  /// The result is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure LEG_INITIAL_NOTIONAL = Measure.of(StandardMeasures.LEG_INITIAL_NOTIONAL.Name);
	  /// <summary>
	  /// Measure representing the accrued interest of the calculation target.
	  /// </summary>
	  public static readonly Measure ACCRUED_INTEREST = Measure.of(StandardMeasures.ACCRUED_INTEREST.Name);
	  /// <summary>
	  /// Measure representing the cash flows of the calculation target.
	  /// <para>
	  /// Cash flows provide details about the payments of the target.
	  /// The result is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure CASH_FLOWS = Measure.of(StandardMeasures.CASH_FLOWS.Name);
	  /// <summary>
	  /// Measure representing the currency exposure of the calculation target.
	  /// <para>
	  /// Currency exposure is the currency risk, expressed as the equivalent amount in each currency.
	  /// Calculated values are not converted to the reporting currency and may contain values in multiple currencies
	  /// if the target contains multiple currencies.
	  /// </para>
	  /// </summary>
	  public static readonly Measure CURRENCY_EXPOSURE = Measure.of(StandardMeasures.CURRENCY_EXPOSURE.Name);
	  /// <summary>
	  /// Measure representing the current cash of the calculation target.
	  /// <para>
	  /// Current cash is the sum of all cash flows paid on the valuation date.
	  /// The result is expressed in the reporting currency.
	  /// </para>
	  /// </summary>
	  public static readonly Measure CURRENT_CASH = Measure.of(StandardMeasures.CURRENT_CASH.Name);
	  /// <summary>
	  /// Measure representing the forward FX rate of the calculation target.
	  /// </summary>
	  public static readonly Measure FORWARD_FX_RATE = Measure.of(StandardMeasures.FORWARD_FX_RATE.Name);
	  /// <summary>
	  /// Measure representing the unit price of the instrument.
	  /// <para>
	  /// This is the price of a single unit of a security using Strata market conventions.
	  /// The price is represented as a {@code double}, even if it is actually a currency amount.
	  /// </para>
	  /// </summary>
	  public static readonly Measure UNIT_PRICE = Measure.of(StandardMeasures.UNIT_PRICE.Name);
	  /// <summary>
	  /// Measure representing the resolved form of the calculation target.
	  /// <para>
	  /// Many calculation targets have a <seealso cref="Resolvable resolved"/> form that is optimized for pricing.
	  /// This measure allows the resolved form to be obtained.
	  /// Since the target is the same for all scenarios, the result is not wrapped in <seealso cref="ScenarioArray"/>.
	  /// </para>
	  /// </summary>
	  public static readonly Measure RESOLVED_TARGET = Measure.of(StandardMeasures.RESOLVED_TARGET.Name);

	  //-------------------------------------------------------------------------
	  private Measures()
	  {
	  }

	}

}