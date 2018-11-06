using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// Abstract derived calculation function with fields for the target type, measure and required measures.
	/// <para>
	/// Empty requirements are returned from <seealso cref="#requirements"/>.
	/// Subtypes only need to provide an implementation of the <seealso cref="#calculate"/> method.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of calculation target handled by the function </param>
	/// @param <R> the type of the measure calculated by the function </param>
	public abstract class AbstractDerivedCalculationFunction<T, R> : DerivedCalculationFunction<T, R> where T : com.opengamma.strata.basics.CalculationTarget
	{
		public abstract R calculate(T target, IDictionary<Measure, object> requiredMeasures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, ReferenceData refData);

	  /// <summary>
	  /// The target type handled by the function, often a trade. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly Type<T> targetType_Renamed;

	  /// <summary>
	  /// The measure calculated by the function. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly Measure measure_Renamed;

	  /// <summary>
	  /// The measures required as inputs to the calculation. </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ISet<Measure> requiredMeasures_Renamed;

	  /// <summary>
	  /// Creates a new function which calculates one measure for targets of one type.
	  /// </summary>
	  /// <param name="targetType">  the target type handled by the function, often a trade </param>
	  /// <param name="measure">  the measure calculated by the function </param>
	  /// <param name="requiredMeasures">  the measures required as inputs to the calculation </param>
	  protected internal AbstractDerivedCalculationFunction(Type<T> targetType, Measure measure, params Measure[] requiredMeasures) : this(targetType, measure, ImmutableSet.copyOf(requiredMeasures))
	  {

	  }

	  /// <summary>
	  /// Creates a new function which calculates one measure for targets of one type.
	  /// </summary>
	  /// <param name="targetType">  the target type handled by the function, often a trade </param>
	  /// <param name="measure">  the measure calculated by the function </param>
	  /// <param name="requiredMeasures">  the measures required as inputs to the calculation </param>
	  protected internal AbstractDerivedCalculationFunction(Type<T> targetType, Measure measure, ISet<Measure> requiredMeasures)
	  {

		this.measure_Renamed = measure;
		this.requiredMeasures_Renamed = ImmutableSet.copyOf(requiredMeasures);
		this.targetType_Renamed = targetType;
	  }

	  public virtual Type<T> targetType()
	  {
		return targetType_Renamed;
	  }

	  public virtual Measure measure()
	  {
		return measure_Renamed;
	  }

	  public virtual ISet<Measure> requiredMeasures()
	  {
		return requiredMeasures_Renamed;
	  }

	  public virtual FunctionRequirements requirements(T target, CalculationParameters parameters, ReferenceData refData)
	  {
		return FunctionRequirements.empty();
	  }

	}

}