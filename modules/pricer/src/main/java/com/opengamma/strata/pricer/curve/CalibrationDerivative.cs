using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CurveParameterSize = com.opengamma.strata.market.curve.CurveParameterSize;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using ResolvedTrade = com.opengamma.strata.product.ResolvedTrade;

	/// <summary>
	/// Provides the calibration derivative.
	/// <para>
	/// This provides the value sensitivity from the specified <seealso cref="CalibrationMeasures"/>
	/// instance in matrix form suitable for use in curve calibration root finding.
	/// The value will typically be par spread or converted present value.
	/// </para>
	/// </summary>
	internal class CalibrationDerivative : System.Func<DoubleArray, DoubleMatrix>
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
	  /// Provide the order in which the curves appear in the long vector result.
	  /// The expected number of parameters for each curve is also provided.
	  /// </summary>
	  private readonly IList<CurveParameterSize> curveOrder;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="trades">  the trades </param>
	  /// <param name="measures">  the calibration measures </param>
	  /// <param name="providerGenerator">  the provider generator, used to create child providers </param>
	  /// <param name="curveOrder">  the curve order </param>
	  public CalibrationDerivative(IList<ResolvedTrade> trades, CalibrationMeasures measures, RatesProviderGenerator providerGenerator, IList<CurveParameterSize> curveOrder)
	  {

		this.measures = measures;
		this.trades = trades;
		this.providerGenerator = providerGenerator;
		this.curveOrder = curveOrder;
	  }

	  //-------------------------------------------------------------------------
	  public override DoubleMatrix apply(DoubleArray x)
	  {
		// create child provider from matrix
		ImmutableRatesProvider provider = providerGenerator.generate(x);
		// calculate derivative for each trade using the child provider
		int size = trades.Count;
		return DoubleMatrix.ofArrayObjects(size, size, i => measures.derivative(trades[i], provider, curveOrder));
	  }

	}

}