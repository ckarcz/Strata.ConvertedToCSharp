using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Provides the calibration value.
	/// <para>
	/// This provides the value from the specified <seealso cref="CalibrationMeasures"/> instance
	/// in matrix form suitable for use in curve calibration root finding.
	/// The value will typically be par spread or converted present value.
	/// </para>
	/// </summary>
	internal class CalibrationValue : System.Func<DoubleArray, DoubleArray>
	{

	  /// <summary>
	  /// The trades.
	  /// </summary>
	  private readonly IList<ResolvedTrade> trades;
	  /// <summary>
	  /// The calibration measures.
	  /// </summary>
	  private readonly CalibrationMeasures measures;
	  /// <summary>
	  /// The provider generator, used to create child providers.
	  /// </summary>
	  private readonly RatesProviderGenerator providerGenerator;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="trades">  the trades </param>
	  /// <param name="measures">  the calibration measures </param>
	  /// <param name="providerGenerator">  the provider generator, used to create child providers </param>
	  internal CalibrationValue(IList<ResolvedTrade> trades, CalibrationMeasures measures, RatesProviderGenerator providerGenerator)
	  {

		this.trades = trades;
		this.measures = measures;
		this.providerGenerator = providerGenerator;
	  }

	  //-------------------------------------------------------------------------
	  public override DoubleArray apply(DoubleArray x)
	  {
		// create child provider from matrix
		ImmutableRatesProvider childProvider = providerGenerator.generate(x);
		// calculate value for each trade using the child provider
		return DoubleArray.of(trades.Count, i => measures.value(trades[i], childProvider));
	  }

	}

}