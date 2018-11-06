/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{

	/// <summary>
	/// A checked version of {@code Function}.
	/// <para>
	/// This is intended to be used with <seealso cref="Unchecked"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the object parameter </param>
	/// @param <R> the type of the result </param>
	public delegate R CheckedFunction<T, R>(T t);

}