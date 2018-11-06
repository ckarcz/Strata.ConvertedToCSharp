/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// An operation consuming three arguments - {@code int}, {@code int} and {@code double}.
	/// <para>
	/// Implementations of this interface will operate using side-effects.
	/// </para>
	/// </summary>
	public delegate void IntIntDoubleConsumer(int intValue1, int intValue2, double doubleValue);

}