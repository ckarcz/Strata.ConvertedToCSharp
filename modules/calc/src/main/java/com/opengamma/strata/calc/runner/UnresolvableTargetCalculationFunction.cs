using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

	/// <summary>
	/// Function used when the target cannot be resolved.
	/// </summary>
	internal sealed class UnresolvableTargetCalculationFunction : CalculationFunction<UnresolvableTarget>
	{

	  /// <summary>
	  /// Shared instance.
	  /// </summary>
	  internal static readonly CalculationFunction<UnresolvableTarget> INSTANCE = new UnresolvableTargetCalculationFunction();

	  // restricted constructor
	  private UnresolvableTargetCalculationFunction()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public Type<UnresolvableTarget> targetType()
	  {
		return typeof(UnresolvableTarget);
	  }

	  public ISet<Measure> supportedMeasures()
	  {
		// pass all measures here so that the calculation is run to get the correct error message
		return ImmutableSet.copyOf(Measure.extendedEnum().lookupAllNormalized().values());
	  }

	  public Currency naturalCurrency(UnresolvableTarget target, ReferenceData refData)
	  {
		throw new System.InvalidOperationException("Function has no currency-convertible measures");
	  }

	  public FunctionRequirements requirements(UnresolvableTarget target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
	  {

		return FunctionRequirements.empty();
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(UnresolvableTarget target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
	  public IDictionary<Measure, Result<object>> calculate(UnresolvableTarget target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
	  {

		throw new System.InvalidOperationException(Messages.format("Target '{}' cannot be resolved: {}", target.Target.GetType(), target.Message));
	  }

	}

}