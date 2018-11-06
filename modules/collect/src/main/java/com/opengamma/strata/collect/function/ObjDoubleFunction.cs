/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{

	/// <summary>
	/// A function of two arguments - one object and one {@code double}.
	/// <para>
	/// This takes two arguments and returns an object result.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the object parameter </param>
	/// @param <R> the type of the result </param>
	/// <seealso cref= BiFunction </seealso>
	public interface ObjDoubleFunction<T, R>
	{

	  /// <summary>
	  /// Applies the function.
	  /// </summary>
	  /// <param name="obj">  the first argument </param>
	  /// <param name="value">  the second argument </param>
	  /// <returns> the result of the function </returns>
	  R apply(T obj, double value);

	  /// <summary>
	  /// Returns a new function that composes this function and the specified function.
	  /// <para>
	  /// This returns a composed function that applies the input to this function
	  /// and then converts the result using the specified function.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <V> the result type of second function </param>
	  /// <param name="other">  the second function </param>
	  /// <returns> the combined function, "this AND_THEN that" </returns>
	  /// <exception cref="NullPointerException"> if the other function is null </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public default <V> ObjDoubleFunction<T, V> andThen(java.util.function.Function<? super R, ? extends V> other)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default <V> ObjDoubleFunction<T, V> andThen(System.Func<JavaToDotNetGenericWildcard, JavaToDotNetGenericWildcard extends V> other)
	//  {
	//	Objects.requireNonNull(other);
	//	return (obj, value) -> other.apply(apply(obj, value));
	//  }

	}

}