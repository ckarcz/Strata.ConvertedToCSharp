/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.rate
{

	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using DispatchingRateComputationFn = com.opengamma.strata.pricer.impl.rate.DispatchingRateComputationFn;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Computes a rate.
	/// <para>
	/// This function provides the ability to compute a rate defined by <seealso cref="RateComputation"/>.
	/// The rate will be based on known historic data and forward curves.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe functions.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of rate to be observed </param>
	public interface RateComputationFn<T> where T : com.opengamma.strata.product.rate.RateComputation
	{

	  /// <summary>
	  /// Returns the standard instance of the function.
	  /// <para>
	  /// Use this method to avoid a direct dependency on the implementation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the rate computation function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RateComputationFn<com.opengamma.strata.product.rate.RateComputation> standard()
	//  {
	//	return DispatchingRateComputationFn.DEFAULT;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Determines the applicable rate for the computation.
	  /// <para>
	  /// Each type of rate has specific rules, encapsulated in {@code RateComputation}.
	  /// </para>
	  /// <para>
	  /// The start date and end date refer to the accrual period.
	  /// In many cases, this information is not necessary, however it does enable some
	  /// implementations that would not otherwise be possible.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="computation">  the computation definition </param>
	  /// <param name="startDate">  the start date of the accrual period </param>
	  /// <param name="endDate">  the end date of the accrual period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the applicable rate </returns>
	  double rate(T computation, LocalDate startDate, LocalDate endDate, RatesProvider provider);

	  /// <summary>
	  /// Determines the point sensitivity for the rate computation.
	  /// <para>
	  /// This returns a sensitivity instance referring to the curves used to determine
	  /// each forward rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="computation">  the computation definition </param>
	  /// <param name="startDate">  the start date of the accrual period </param>
	  /// <param name="endDate">  the end date of the accrual period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity </returns>
	  PointSensitivityBuilder rateSensitivity(T computation, LocalDate startDate, LocalDate endDate, RatesProvider provider);

	  /// <summary>
	  /// Explains the calculation of the applicable rate.
	  /// <para>
	  /// This adds information to the <seealso cref="ExplainMapBuilder"/> to aid understanding of the computation.
	  /// The actual rate is also returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="computation">  the computation definition </param>
	  /// <param name="startDate">  the start date of the accrual period </param>
	  /// <param name="endDate">  the end date of the accrual period </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <param name="builder">  the builder to populate </param>
	  /// <returns> the applicable rate </returns>
	  double explainRate(T computation, LocalDate startDate, LocalDate endDate, RatesProvider provider, ExplainMapBuilder builder);

	}

}