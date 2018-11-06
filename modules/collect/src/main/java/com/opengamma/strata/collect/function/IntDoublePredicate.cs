/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// A predicate of two arguments - {@code int} and {@code double}.
	/// <para>
	/// This takes two arguments and returns a {@code boolean} result.
	/// </para>
	/// </summary>
	public delegate bool IntDoublePredicate(int intValue, double doubleValue);

}