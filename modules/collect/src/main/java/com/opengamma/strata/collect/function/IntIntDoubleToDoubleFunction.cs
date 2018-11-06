/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// A function of three arguments - {@code int}, {@code int} and {@code double}.
	/// <para>
	/// This takes three arguments and returns a {@code double} result.
	/// </para>
	/// </summary>
	public delegate double IntIntDoubleToDoubleFunction(int intValue1, int intValue2, double doubleValue);

}