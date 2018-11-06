/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// A predicate of three arguments - {@code int}, {@code int} and {@code double}.
	/// <para>
	/// This takes three arguments and returns a {@code boolean} result.
	/// </para>
	/// </summary>
	public delegate bool IntIntDoublePredicate(int intValue1, int intValue2, double doubleValue);

}