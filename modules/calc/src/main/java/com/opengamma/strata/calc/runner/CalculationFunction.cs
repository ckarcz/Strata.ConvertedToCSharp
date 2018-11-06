using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;
	using ScenarioFxConvertible = com.opengamma.strata.data.scenario.ScenarioFxConvertible;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Primary interface for all calculation functions that calculate measures.
	/// <para>
	/// Implementations of this interface provide the ability to calculate one or more measures
	/// for a target (trade) using one or more sets of market data (scenarios).
	/// The methods of the function allow the <seealso cref="CalculationRunner"/> to correctly invoke the function:
	/// <ul>
	/// <li><seealso cref="#targetType()"/>
	///  - the target type that the function applies to
	/// <li><seealso cref="#supportedMeasures()"/>
	///  - the set of measures that can be calculated
	/// <li><seealso cref="#naturalCurrency(CalculationTarget, ReferenceData)"/>
	///  - the "natural" currency of the target
	/// <li><seealso cref="#requirements(CalculationTarget, Set, CalculationParameters, ReferenceData)"/>
	///  - the market data requirements for performing the calculation
	/// <li><seealso cref="#calculate(CalculationTarget, Set, CalculationParameters, ScenarioMarketData, ReferenceData)"/>
	///  - perform the calculation
	/// </ul>
	/// </para>
	/// <para>
	/// If any of the calculated values contain any currency amounts and implement <seealso cref="ScenarioFxConvertible"/>
	/// the calculation runner will automatically convert the amounts into the reporting currency.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of target handled by this function </param>
	public interface CalculationFunction<T> where T : com.opengamma.strata.basics.CalculationTarget
	{

	  /// <summary>
	  /// Gets the target type that this function applies to.
	  /// <para>
	  /// The target type will typically be a concrete class.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the target type </returns>
	  Type<T> targetType();

	  /// <summary>
	  /// Returns the set of measures that the function can calculate.
	  /// </summary>
	  /// <returns> the read-only set of measures that the function can calculate </returns>
	  ISet<Measure> supportedMeasures();

	  /// <summary>
	  /// Returns an identifier that should uniquely identify the specified target.
	  /// <para>
	  /// This identifier is used in error messages to identify the target.
	  /// This should normally be overridden to provide a suitable identifier.
	  /// For example, if the target is a trade, there will typically be a trade identifier available.
	  /// </para>
	  /// <para>
	  /// This method must not throw an exception.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target of the calculation </param>
	  /// <returns> the identifier of the target, empty if no suitable identifier available </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.Optional<String> identifier(T target)
	//  {
	//	return Optional.empty();
	//  }

	  /// <summary>
	  /// Returns the "natural" currency for the specified target.
	  /// <para>
	  /// This is the currency to which currency amounts are converted if the "natural"
	  /// reporting currency is requested using <seealso cref="ReportingCurrency#NATURAL"/>.
	  /// Most targets have a "natural" currency, for example the currency of a FRA or
	  /// the base currency of an FX forward.
	  /// </para>
	  /// <para>
	  /// It is required that all functions that return a currency-convertible measure
	  /// must choose a "natural" currency for each trade. The choice must be consistent
	  /// not random, given the same trade the same currency must be returned.
	  /// This might involve picking, the first leg or base currency from a currency pair.
	  /// An exception must only be thrown if the function handles no currency-convertible measures.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target of the calculation </param>
	  /// <param name="refData">  the reference data to be used in the calculation </param>
	  /// <returns> the "natural" currency of the target </returns>
	  /// <exception cref="IllegalStateException"> if the function calculates no currency-convertible measures </exception>
	  Currency naturalCurrency(T target, ReferenceData refData);

	  /// <summary>
	  /// Determines the market data required by this function to perform its calculations.
	  /// <para>
	  /// Any market data needed by the {@code calculate} method should be specified.
	  /// </para>
	  /// <para>
	  /// The set of measures may include measures that are not supported by this function.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target of the calculation </param>
	  /// <param name="measures">  the set of measures to be calculated </param>
	  /// <param name="parameters">  the parameters that affect how the calculation is performed </param>
	  /// <param name="refData">  the reference data to be used in the calculation </param>
	  /// <returns> the requirements specifying the market data the function needs to perform calculations </returns>
	  FunctionRequirements requirements(T target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData);

	  /// <summary>
	  /// Calculates values of multiple measures for the target using multiple sets of market data.
	  /// <para>
	  /// The set of measures must only contain measures that the function supports,
	  /// as returned by <seealso cref="#supportedMeasures()"/>. The market data must provide at least the
	  /// set of data requested by <seealso cref="#requirements(CalculationTarget, Set, CalculationParameters, ReferenceData)"/>.
	  /// </para>
	  /// <para>
	  /// The result of this method will often be an instance of <seealso cref="ScenarioArray"/>, which
	  /// handles the common case where there is one calculated value for each scenario.
	  /// However, it is also possible for the function to calculate an aggregated result, such
	  /// as the maximum or minimum value across all scenarios, in which case the result would
	  /// not implement {@code ScenarioArray}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the target of the calculation </param>
	  /// <param name="measures">  the set of measures to calculate </param>
	  /// <param name="parameters">  the parameters that affect how the calculation is performed </param>
	  /// <param name="marketData">  the multi-scenario market data to be used in the calculation </param>
	  /// <param name="refData">  the reference data to be used in the calculation </param>
	  /// <returns> the read-only map of calculated values, keyed by their measure </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(T target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData);
	  IDictionary<Measure, Result<object>> calculate(T target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData);

	}

}