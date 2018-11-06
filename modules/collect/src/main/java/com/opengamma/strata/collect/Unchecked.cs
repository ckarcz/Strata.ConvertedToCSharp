using System;
using System.Threading;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using Throwables = com.google.common.@base.Throwables;
	using CheckedBiConsumer = com.opengamma.strata.collect.function.CheckedBiConsumer;
	using CheckedBiFunction = com.opengamma.strata.collect.function.CheckedBiFunction;
	using CheckedBiPredicate = com.opengamma.strata.collect.function.CheckedBiPredicate;
	using CheckedBinaryOperator = com.opengamma.strata.collect.function.CheckedBinaryOperator;
	using CheckedConsumer = com.opengamma.strata.collect.function.CheckedConsumer;
	using CheckedFunction = com.opengamma.strata.collect.function.CheckedFunction;
	using CheckedPredicate = com.opengamma.strata.collect.function.CheckedPredicate;
	using CheckedRunnable = com.opengamma.strata.collect.function.CheckedRunnable;
	using CheckedSupplier = com.opengamma.strata.collect.function.CheckedSupplier;
	using CheckedUnaryOperator = com.opengamma.strata.collect.function.CheckedUnaryOperator;

	/// <summary>
	/// Static utility methods that convert checked exceptions to unchecked.
	/// <para>
	/// Two {@code wrap()} methods are provided that can wrap an arbitrary piece of logic
	/// and convert checked exceptions to unchecked.
	/// </para>
	/// <para>
	/// A number of other methods are provided that allow a lambda block to be decorated
	/// to avoid handling checked exceptions.
	/// For example, the method <seealso cref="File#getCanonicalFile()"/> throws an <seealso cref="IOException"/>
	/// which can be handled as follows:
	/// <pre>
	///  stream.map(Unchecked.function(file -&gt; file.getCanonicalFile())
	/// </pre>
	/// </para>
	/// <para>
	/// Each method accepts a functional interface that is defined to throw <seealso cref="Throwable"/>.
	/// Catching {@code Throwable} means that any method can be wrapped.
	/// Any {@code InvocationTargetException} is extracted and processed recursively.
	/// Any <seealso cref="IOException"/> is converted to an <seealso cref="UncheckedIOException"/>.
	/// Any <seealso cref="ReflectiveOperationException"/> is converted to an <seealso cref="UncheckedReflectiveOperationException"/>.
	/// Any <seealso cref="Error"/> or <seealso cref="RuntimeException"/> is re-thrown without alteration.
	/// Any other exception is wrapped in a <seealso cref="RuntimeException"/>.
	/// </para>
	/// </summary>
	public sealed class Unchecked
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Unchecked()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Wraps a block of code, converting checked exceptions to unchecked.
	  /// <pre>
	  ///   Unchecked.wrap(() -&gt; {
	  ///     // any code that throws a checked exception
	  ///   }
	  /// </pre>
	  /// <para>
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="block">  the code block to wrap </param>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="RuntimeException"> if an exception occurs </exception>
	  public static void wrap(CheckedRunnable block)
	  {
		try
		{
		  block();
		}
		catch (Exception ex)
		{
		  throw propagate(ex);
		}
	  }

	  /// <summary>
	  /// Wraps a block of code, converting checked exceptions to unchecked.
	  /// <pre>
	  ///   Unchecked.wrap(() -&gt; {
	  ///     // any code that throws a checked exception
	  ///   }
	  /// </pre>
	  /// <para>
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the result </param>
	  /// <param name="block">  the code block to wrap </param>
	  /// <returns> the result of invoking the block </returns>
	  /// <exception cref="UncheckedIOException"> if an IO exception occurs </exception>
	  /// <exception cref="RuntimeException"> if an exception occurs </exception>
	  public static T wrap<T>(CheckedSupplier<T> block)
	  {
		try
		{
		  return block();
		}
		catch (Exception ex)
		{
		  throw propagate(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code Runnable} interface.
	  /// <para>
	  /// This wraps the specified runnable returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="runnable">  the runnable to be decorated </param>
	  /// <returns> the runnable instance that handles checked exceptions </returns>
	  public static ThreadStart runnable(CheckedRunnable runnable)
	  {
		return () =>
		{
	  try
	  {
		runnable();
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code Function} interface.
	  /// <para>
	  /// This wraps the specified function returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the input type of the function </param>
	  /// @param <R>  the return type of the function </param>
	  /// <param name="function">  the function to be decorated </param>
	  /// <returns> the function instance that handles checked exceptions </returns>
	  public static System.Func<T, R> function<T, R>(CheckedFunction<T, R> function)
	  {
		return (t) =>
		{
	  try
	  {
		return Unchecked.function(t);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code BiFunction} interface.
	  /// <para>
	  /// This wraps the specified function returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the first input type of the function </param>
	  /// @param <U>  the second input type of the function </param>
	  /// @param <R>  the return type of the function </param>
	  /// <param name="function">  the function to be decorated </param>
	  /// <returns> the function instance that handles checked exceptions </returns>
	  public static System.Func<T, U, R> biFunction<T, U, R>(CheckedBiFunction<T, U, R> function)
	  {
		return (t, u) =>
		{
	  try
	  {
		return function(t, u);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code UnaryOperator} interface.
	  /// <para>
	  /// This wraps the specified operator returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the operator </param>
	  /// <param name="function">  the function to be decorated </param>
	  /// <returns> the function instance that handles checked exceptions </returns>
	  public static System.Func<T, T> unaryOperator<T>(CheckedUnaryOperator<T> function)
	  {
		return (t) =>
		{
	  try
	  {
		return function.apply(t);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code BinaryOperator} interface.
	  /// <para>
	  /// This wraps the specified operator returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the operator </param>
	  /// <param name="function">  the function to be decorated </param>
	  /// <returns> the function instance that handles checked exceptions </returns>
	  public static System.Func<T, T, T> binaryOperator<T>(CheckedBinaryOperator<T> function)
	  {
		return (t, u) =>
		{
	  try
	  {
		return function.apply(t, u);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code Predicate} interface.
	  /// <para>
	  /// This wraps the specified predicate returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the predicate </param>
	  /// <param name="predicate">  the predicate to be decorated </param>
	  /// <returns> the predicate instance that handles checked exceptions </returns>
	  public static System.Predicate<T> predicate<T>(CheckedPredicate<T> predicate)
	  {
		return (t) =>
		{
	  try
	  {
		return Unchecked.predicate(t);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code BiPredicate} interface.
	  /// <para>
	  /// This wraps the specified predicate returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the first type of the predicate </param>
	  /// @param <U>  the second type of the predicate </param>
	  /// <param name="predicate">  the predicate to be decorated </param>
	  /// <returns> the predicate instance that handles checked exceptions </returns>
	  public static System.Func<T, U, bool> biPredicate<T, U>(CheckedBiPredicate<T, U> predicate)
	  {
		return (t, u) =>
		{
	  try
	  {
		return predicate(t, u);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code Consumer} interface.
	  /// <para>
	  /// This wraps the specified consumer returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the consumer </param>
	  /// <param name="consumer">  the consumer to be decorated </param>
	  /// <returns> the consumer instance that handles checked exceptions </returns>
	  public static System.Action<T> consumer<T>(CheckedConsumer<T> consumer)
	  {
		return (t) =>
		{
	  try
	  {
		Unchecked.consumer(t);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code BiConsumer} interface.
	  /// <para>
	  /// This wraps the specified consumer returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the first type of the consumer </param>
	  /// @param <U>  the second type of the consumer </param>
	  /// <param name="consumer">  the consumer to be decorated </param>
	  /// <returns> the consumer instance that handles checked exceptions </returns>
	  public static System.Action<T, U> biConsumer<T, U>(CheckedBiConsumer<T, U> consumer)
	  {
		return (t, u) =>
		{
	  try
	  {
		consumer(t, u);
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts checked exceptions to unchecked based on the {@code Supplier} interface.
	  /// <para>
	  /// This wraps the specified supplier returning an instance that handles checked exceptions.
	  /// If a checked exception is thrown it is converted to an <seealso cref="UncheckedIOException"/>
	  /// or <seealso cref="RuntimeException"/> as appropriate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the result type of the supplier </param>
	  /// <param name="supplier">  the supplier to be decorated </param>
	  /// <returns> the supplier instance that handles checked exceptions </returns>
	  public static System.Func<R> supplier<R>(CheckedSupplier<R> supplier)
	  {
		return () =>
		{
	  try
	  {
		return supplier();
	  }
	  catch (Exception ex)
	  {
		throw propagate(ex);
	  }
		};
	  }

	  /// <summary>
	  /// Propagates {@code throwable} as-is if possible, or by wrapping in a {@code RuntimeException} if not.
	  /// <ul>
	  ///   <li>If {@code throwable} is an {@code InvocationTargetException} the cause is extracted and processed recursively.</li>
	  ///   <li>If {@code throwable} is an {@code Error} or {@code RuntimeException} it is propagated as-is.</li>
	  ///   <li>If {@code throwable} is an {@code IOException} it is wrapped in {@code UncheckedIOException} and thrown.</li>
	  ///   <li>If {@code throwable} is an {@code ReflectiveOperationException} it is wrapped in
	  ///     {@code UncheckedReflectiveOperationException} and thrown.</li>
	  ///   <li>Otherwise {@code throwable} is wrapped in a {@code RuntimeException} and thrown.</li>
	  /// </ul>
	  /// This method always throws an exception. The return type is a convenience to satisfy the type system
	  /// when the enclosing method returns a value. For example:
	  /// <pre>
	  ///   T foo() {
	  ///     try {
	  ///       return methodWithCheckedException();
	  ///     } catch (Exception e) {
	  ///       throw Unchecked.propagate(e);
	  ///     }
	  ///   }
	  /// </pre>
	  /// </summary>
	  /// <param name="throwable"> the {@code Throwable} to propagate </param>
	  /// <returns> nothing; this method always throws an exception </returns>
	  public static Exception propagate(Exception throwable)
	  {
		if (throwable is InvocationTargetException)
		{
		  throw propagate(((InvocationTargetException) throwable).InnerException);
		}
		else if (throwable is IOException)
		{
		  throw new UncheckedIOException((IOException) throwable);
		}
		else if (throwable is ReflectiveOperationException)
		{
		  throw new UncheckedReflectiveOperationException((ReflectiveOperationException) throwable);
		}
		else
		{
		  Throwables.throwIfUnchecked(throwable);
		  throw new Exception(throwable);
		}
	  }

	}

}