/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// An operation consuming two arguments - {@code int} and {@code int}.
	/// <para>
	/// Implementations of this interface will operate using side-effects.
	/// </para>
	/// </summary>
	public delegate void IntIntConsumer(int intValue1, int intValue2);

}