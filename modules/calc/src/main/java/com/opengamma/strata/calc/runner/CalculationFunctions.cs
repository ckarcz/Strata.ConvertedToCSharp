/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;

	/// <summary>
	/// The calculation functions.
	/// <para>
	/// This provides the complete set of functions that will be used in a calculation.
	/// </para>
	/// <para>
	/// The default implementation is accessed by the static factory methods.
	/// It matches the <seealso cref="CalculationFunction"/> by the type of the <seealso cref="CalculationTarget"/>.
	/// As such, the default implementation is essentially a {@code Map} where the keys are the
	/// target type {@code Class} that the function operates on.
	/// </para>
	/// </summary>
	public interface CalculationFunctions
	{

	  /// <summary>
	  /// Obtains an empty instance with no functions.
	  /// </summary>
	  /// <returns> the empty instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationFunctions empty()
	//  {
	//	return DefaultCalculationFunctions.EMPTY;
	//  }

	  /// <summary>
	  /// Obtains an instance from the specified functions.
	  /// <para>
	  /// This returns an implementation that matches the function by the type of the
	  /// target, as returned by <seealso cref="CalculationFunction#targetType()"/>.
	  /// The list will be converted to a {@code Map} keyed by the target type.
	  /// Each function must refer to a different target type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <returns> the calculation functions </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationFunctions of(CalculationFunction<JavaToDotNetGenericWildcard>... functions)
	//  {
	//	return DefaultCalculationFunctions.of(Stream.of(functions).collect(toImmutableMap(fn -> fn.targetType())));
	//  }

	  /// <summary>
	  /// Obtains an instance from the specified functions.
	  /// <para>
	  /// This returns an implementation that matches the function by the type of the
	  /// target, as returned by <seealso cref="CalculationFunction#targetType()"/>.
	  /// The list will be converted to a {@code Map} keyed by the target type.
	  /// Each function must refer to a different target type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <returns> the calculation functions </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationFunctions of(java.util.List<JavaToDotNetGenericWildcard extends CalculationFunction<JavaToDotNetGenericWildcard>> functions)
	//  {
	//	return DefaultCalculationFunctions.of(functions.stream().collect(toImmutableMap(fn -> fn.targetType())));
	//  }

	  /// <summary>
	  /// Obtains an instance from the specified functions.
	  /// <para>
	  /// This returns an implementation that matches the function by the type of the target.
	  /// When finding the matching function, the target type is looked up in the specified map.
	  /// The map will be validated to ensure the {@code Class} is consistent with
	  /// <seealso cref="CalculationFunction#targetType()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <returns> the calculation functions </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CalculationFunctions of(java.util.Map<Class, JavaToDotNetGenericWildcard extends CalculationFunction<JavaToDotNetGenericWildcard>> functions)
	//  {
	//	return DefaultCalculationFunctions.of(functions);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the function that handles the specified target.
	  /// <para>
	  /// If no function is found, a suitable default that can perform no calculations is provided.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the target type </param>
	  /// <param name="target">  the calculation target, such as a trade </param>
	  /// <returns> the function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public default <T extends com.opengamma.strata.basics.CalculationTarget> CalculationFunction<? super T> getFunction(T target)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <T> CalculationFunction<JavaToDotNetGenericWildcard> getFunction(T target)
	//  {
	//	return findFunction(target).orElse(MissingConfigCalculationFunction.INSTANCE);
	//  }

	  /// <summary>
	  /// Finds the function that handles the specified target.
	  /// <para>
	  /// If no function is found the result is empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the target type </param>
	  /// <param name="target">  the calculation target, such as a trade </param>
	  /// <returns> the function, empty if not found </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public abstract <T extends com.opengamma.strata.basics.CalculationTarget> java.util.Optional<CalculationFunction<? super T>> findFunction(T target);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  Optional<CalculationFunction> findFunction<T>(T target);

	  /// <summary>
	  /// Returns a set of calculation functions which combines the functions in this set with the functions in another.
	  /// <para>
	  /// If both sets of functions contain a function for a target then the function from this set is returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  another set of calculation functions </param>
	  /// <returns> a set of calculation functions which combines the functions in this set with the functions in the other </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CalculationFunctions composedWith(CalculationFunctions other)
	//  {
	//	return CompositeCalculationFunctions.of(this, other);
	//  }

	  /// <summary>
	  /// Returns a set of calculation functions which combines the functions in this set with some
	  /// derived calculation functions.
	  /// <para>
	  /// Each derived function calculates one measure for one type of target, possibly using other calculated measures
	  /// as inputs.
	  /// </para>
	  /// <para>
	  /// If any of the derived functions depend on each other they must be passed to this method in the correct
	  /// order to ensure their dependencies can be satisfied. For example, if there is a derived function
	  /// {@code fnA} which depends on the measure calculated by function {@code fnB} they must be passed to
	  /// this method in the order {@code fnB, fnA}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <returns> a set of calculation functions which combines the functions in this set with some
	  ///   derived calculation functions </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default CalculationFunctions composedWith(DerivedCalculationFunction<JavaToDotNetGenericWildcard, JavaToDotNetGenericWildcard>... functions)
	//  {
	//	return new DerivedCalculationFunctions(this, functions);
	//  }
	}

}