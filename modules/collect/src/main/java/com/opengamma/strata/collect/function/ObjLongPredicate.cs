/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{

	/// <summary>
	/// A predicate of two arguments - one object and one {@code long}.
	/// <para>
	/// This takes two arguments and returns a {@code boolean} result.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the object parameter </param>
	/// <seealso cref= BiPredicate </seealso>
	public interface ObjLongPredicate<T>
	{

	  /// <summary>
	  /// Evaluates the predicate.
	  /// </summary>
	  /// <param name="obj">  the first argument </param>
	  /// <param name="value">  the second argument </param>
	  /// <returns> true if the arguments match the predicate </returns>
	  bool test(T obj, long value);

	  /// <summary>
	  /// Returns a new predicate that returns true if both predicates return true.
	  /// <para>
	  /// The second predicate is only invoked if the first returns true.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the second predicate </param>
	  /// <returns> the combined predicate, "this AND that" </returns>
	  /// <exception cref="NullPointerException"> if the other predicate is null </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public default ObjLongPredicate<T> and(ObjLongPredicate<? super T> other)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ObjLongPredicate<T> and(ObjLongPredicate<JavaToDotNetGenericWildcard> other)
	//  {
	//	Objects.requireNonNull(other);
	//	return (obj, value) -> test(obj, value) && other.test(obj, value);
	//  }

	  /// <summary>
	  /// Returns a new predicate that returns true if either predicates returns true.
	  /// <para>
	  /// The second predicate is only invoked if the first returns false.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the second predicate </param>
	  /// <returns> the combined predicate, "this OR that" </returns>
	  /// <exception cref="NullPointerException"> if the other predicate is null </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public default ObjLongPredicate<T> or(ObjLongPredicate<? super T> other)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ObjLongPredicate<T> or(ObjLongPredicate<JavaToDotNetGenericWildcard> other)
	//  {
	//	Objects.requireNonNull(other);
	//	return (obj, value) -> test(obj, value) || other.test(obj, value);
	//  }

	  /// <summary>
	  /// Returns a new predicate that negates the result of this predicate.
	  /// </summary>
	  /// <returns> the predicate, "NOT this" </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ObjLongPredicate<T> negate()
	//  {
	//	return (obj, value) -> !test(obj, value);
	//  }

	}

}