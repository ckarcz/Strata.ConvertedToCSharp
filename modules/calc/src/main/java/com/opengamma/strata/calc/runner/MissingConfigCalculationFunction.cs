using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Function used when there is no function registered that can calculate a requested value.
	/// </summary>
	internal sealed class MissingConfigCalculationFunction : CalculationFunction<CalculationTarget>
	{

	  /// <summary>
	  /// Shared instance.
	  /// </summary>
	  internal static readonly CalculationFunction<CalculationTarget> INSTANCE = new MissingConfigCalculationFunction();

	  // restricted constructor
	  private MissingConfigCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Type<CalculationTarget> targetType()
	  {
		return typeof(CalculationTarget);
	  }

	  public ISet<Measure> supportedMeasures()
	  {
		// pass all measures here so that the calculation is run to get the correct error message
		return ImmutableSet.copyOf(Measure.extendedEnum().lookupAllNormalized().values());
	  }

	  public Currency naturalCurrency(CalculationTarget trade, ReferenceData refData)
	  {
		throw new System.InvalidOperationException("Function has no currency-convertible measures");
	  }

	  public FunctionRequirements requirements(CalculationTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		return FunctionRequirements.empty();
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(com.opengamma.strata.basics.CalculationTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public IDictionary<Measure, Result<object>> calculate(CalculationTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		throw new System.InvalidOperationException(Messages.format("No function configured for measures {} on '{}'", measures, target.GetType().Name));
	  }

	}

}