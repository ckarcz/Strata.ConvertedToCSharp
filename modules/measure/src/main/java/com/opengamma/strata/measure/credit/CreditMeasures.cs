/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.credit
{
	using Measure = com.opengamma.strata.calc.Measure;

	/// <summary>
	/// The standard set of credit measures that can be calculated by Strata.
	/// <para>
	/// A measure identifies the calculation result that is required.
	/// </para>
	/// </summary>
	public sealed class CreditMeasures
	{

	  /// <summary>
	  /// Measure representing the principal.
	  /// </summary>
	  public static readonly Measure PRINCIPAL = Measure.of(StandardCreditMeasures.PRINCIPAL.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the PV change under a 1 bps shift in calibrated curve.
	  /// </summary>
	  public static readonly Measure IR01_CALIBRATED_PARALLEL = Measure.of(StandardCreditMeasures.IR01_CALIBRATED_PARALLEL.Name);
	  /// <summary>
	  /// Measure representing the PV change under a series of 1 bps shifts in calibrated curve at each curve node.
	  /// </summary>
	  public static readonly Measure IR01_CALIBRATED_BUCKETED = Measure.of(StandardCreditMeasures.IR01_CALIBRATED_BUCKETED.Name);
	  /// <summary>
	  /// Measure representing the PV change under a 1 bps shift to market quotes.
	  /// </summary>
	  public static readonly Measure IR01_MARKET_QUOTE_PARALLEL = Measure.of(StandardCreditMeasures.IR01_MARKET_QUOTE_PARALLEL.Name);
	  /// <summary>
	  /// Measure representing the PV change under a series of 1 bps shifts in market quotes at each curve node.
	  /// </summary>
	  public static readonly Measure IR01_MARKET_QUOTE_BUCKETED = Measure.of(StandardCreditMeasures.IR01_MARKET_QUOTE_BUCKETED.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the PV change under a 1 bps shift in credit spread.
	  /// </summary>
	  public static readonly Measure CS01_PARALLEL = Measure.of(StandardCreditMeasures.CS01_PARALLEL.Name);
	  /// <summary>
	  /// Measure representing the PV change under a series of 1 bps shifts in credit spread at each curve node.
	  /// </summary>
	  public static readonly Measure CS01_BUCKETED = Measure.of(StandardCreditMeasures.CS01_BUCKETED.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Measure representing the PV change under a 1 bps shift in recovery rate.
	  /// </summary>
	  public static readonly Measure RECOVERY01 = Measure.of(StandardCreditMeasures.RECOVERY01.Name);
	  /// <summary>
	  /// Measure representing the PV change in case of immediate default.
	  /// </summary>
	  public static readonly Measure JUMP_TO_DEFAULT = Measure.of(StandardCreditMeasures.JUMP_TO_DEFAULT.Name);
	  /// <summary>
	  /// Measure representing the expected value of protection settlement.
	  /// </summary>
	  public static readonly Measure EXPECTED_LOSS = Measure.of(StandardCreditMeasures.EXPECTED_LOSS.Name);

	  //-------------------------------------------------------------------------
	  private CreditMeasures()
	  {
	  }

	}

}