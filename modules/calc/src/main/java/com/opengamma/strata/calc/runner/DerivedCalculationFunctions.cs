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

	/// <summary>
	/// A set of <seealso cref="DerivedCalculationFunction"/> instances which decorates another <seealso cref="CalculationFunctions"/>
	/// instance, creating a combined set of functions.
	/// </summary>
	internal class DerivedCalculationFunctions : CalculationFunctions
	{

	  /// <summary>
	  /// The underlying set of calculation functions. </summary>
	  private readonly CalculationFunctions delegateFunctions;

	  /// <summary>
	  /// Derived calculation functions keyed by the type of target they handle. </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<Class, java.util.List<DerivedCalculationFunction<?, ?>>> functionsByTargetType;
	  private readonly IDictionary<Type, IList<DerivedCalculationFunction<object, ?>>> functionsByTargetType;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="delegateFunctions">  the underlying set of calculation functions </param>
	  /// <param name="functions">  the derived calculation functions </param>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: DerivedCalculationFunctions(CalculationFunctions delegateFunctions, DerivedCalculationFunction<?, ?>... functions)
	  internal DerivedCalculationFunctions(CalculationFunctions delegateFunctions, params DerivedCalculationFunction<object, ?>[] functions)
	  {
		this.delegateFunctions = delegateFunctions;
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		this.functionsByTargetType = java.util.functions.collect(groupingBy(fn => fn.targetType()));
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <T extends com.opengamma.strata.basics.CalculationTarget> java.util.Optional<CalculationFunction<? super T>> findFunction(T target)
	  public virtual Optional<CalculationFunction> findFunction<T>(T target) where T : com.opengamma.strata.basics.CalculationTarget
	  {
		return delegateFunctions.findFunction(target).map(fn => wrap(fn, target));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private <T extends com.opengamma.strata.basics.CalculationTarget, R> CalculationFunction<? super T> wrap(CalculationFunction<? super T> fn, T target)
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private CalculationFunction<object> wrap<T, R, T1>(CalculationFunction<T1> fn, T target) where T : com.opengamma.strata.basics.CalculationTarget
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<DerivedCalculationFunction<?, ?>> derivedFunctions = functionsByTargetType.get(target.getClass());
		IList<DerivedCalculationFunction<object, ?>> derivedFunctions = functionsByTargetType[target.GetType()];
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super T> wrappedFn = fn;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> wrappedFn = fn;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (DerivedCalculationFunction<?, ?> derivedFn : derivedFunctions)
		foreach (DerivedCalculationFunction<object, ?> derivedFn in derivedFunctions)
		{
		  // These casts are necessary because the type information is lost when the functions are stored in the map.
		  // They are safe because T is the target type which is is the map key and R isn't actually used
		  CalculationFunction<T> wrappedFnCast = (CalculationFunction<T>) wrappedFn;
		  DerivedCalculationFunction<T, R> derivedFnCast = (DerivedCalculationFunction<T, R>) derivedFn;
		  wrappedFn = new DerivedCalculationFunctionWrapper<>(derivedFnCast, wrappedFnCast);
		}
		return wrappedFn;
	  }
	}

}