/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
	/// <summary>
	/// Measures for testing.
	/// </summary>
	public sealed class TestingMeasures
	{

	  /// <summary>
	  /// Measure representing the cash flows of the calculation target.
	  /// </summary>
	  public static readonly Measure CASH_FLOWS = ImmutableMeasure.of("CashFlows");
	  /// <summary>
	  /// Measure representing the par rate of the calculation target.
	  /// </summary>
	  public static readonly Measure PAR_RATE = ImmutableMeasure.of("ParRate", false);
	  /// <summary>
	  /// Measure representing the present value of the calculation target.
	  /// </summary>
	  public static readonly Measure PRESENT_VALUE = ImmutableMeasure.of("PresentValue");
	  /// <summary>
	  /// Measure representing the present value of the calculation target.
	  /// <para>
	  /// Calculated values are not converted to the reporting currency and may contain values in multiple currencies
	  /// if the target contains multiple currencies.
	  /// </para>
	  /// </summary>
	  public static readonly Measure PRESENT_VALUE_MULTI_CCY = ImmutableMeasure.of("PresentValueMultiCurrency", false);
	  /// <summary>
	  /// Measure representing the Bucketed PV01 of the calculation target.
	  /// </summary>
	  public static readonly Measure BUCKETED_PV01 = ImmutableMeasure.of("BucketedPV01");

	  //-------------------------------------------------------------------------
	  private TestingMeasures()
	  {
	  }

	}

}