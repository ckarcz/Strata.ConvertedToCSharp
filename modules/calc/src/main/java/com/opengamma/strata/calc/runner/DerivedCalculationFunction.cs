using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// A derived calculation function calculates one measure using the measures calculated by another function.
	/// <para>
	/// Strata executes the other function and checks that all required measures are available before calling
	/// this function.
	/// </para>
	/// <para>
	/// A derived calculation function can be added to an existing set of calculation functions using
	/// <seealso cref="CalculationFunctions#composedWith(DerivedCalculationFunction[])"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the target handled by this function, often a trade </param>
	/// @param <R> the type of value calculated by this function </param>
	public interface DerivedCalculationFunction<T, R> where T : com.opengamma.strata.basics.CalculationTarget
	{

	  /// <summary>
	  /// Returns the type of calculation target handled by the function.
	  /// </summary>
	  /// <returns> the type of calculation target handled by the function </returns>
	  Type<T> targetType();

	  /// <summary>
	  /// Returns the measure calculated by the function.
	  /// </summary>
	  /// <returns> the measure calculated by the function </returns>
	  Measure measure();

	  /// <summary>
	  /// Returns the measures required by this function to calculate its measure.
	  /// </summary>
	  /// <returns> the measures required by this function to calculate its measure </returns>
	  ISet<Measure> requiredMeasures();

	  /// <summary>
	  /// Returns requirements for the market data required by this function to calculate its measure.
	  /// </summary>
	  /// <param name="target">  the target of the calculation, often a trade </param>
	  /// <param name="parameters">  the calculation parameters specifying how the calculations should be performed </param>
	  /// <param name="refData">  the reference data used in the calculations </param>
	  /// <returns> requirements for the market data required by this function to calculate its measure </returns>
	  FunctionRequirements requirements(T target, CalculationParameters parameters, ReferenceData refData);

	  /// <summary>
	  /// Calculates the measure.
	  /// <para>
	  /// This method is only invoked if all of the required measures are available.
	  /// Therefore implementation can safely assume that {@code requiredMeasures} contains all the
	  /// required data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target of the calculation, often a trade </param>
	  /// <param name="requiredMeasures">  the calculated measure values required by this function to calculate its measure </param>
	  /// <param name="parameters">  the calculation parameters specifying how the calculations should be performed </param>
	  /// <param name="marketData">  the market data used in the calculations </param>
	  /// <param name="refData">  the reference data used in the calculations </param>
	  /// <returns> the calculated measure value. </returns>
	  R calculate(T target, IDictionary<Measure, object> requiredMeasures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData);
	}

}