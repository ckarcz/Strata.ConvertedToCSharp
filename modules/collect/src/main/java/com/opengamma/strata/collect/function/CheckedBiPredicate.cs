/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{

	/// <summary>
	/// A checked version of {@code BiPredicate}.
	/// <para>
	/// This is intended to be used with <seealso cref="Unchecked"/>.
	/// 
	/// </para>
	/// </summary>
	/// @param <T> the type of the first object parameter </param>
	/// @param <U> the type of the second object parameter </param>
	public delegate bool CheckedBiPredicate<T, U>(T t, U u);

}