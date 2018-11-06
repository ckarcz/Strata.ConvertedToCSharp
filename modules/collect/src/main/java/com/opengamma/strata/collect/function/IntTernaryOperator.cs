/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.function
{
	/// <summary>
	/// A function of three arguments that returns a value.
	/// <para>
	/// All the inputs and outputs are of type {@code int}.
	/// </para>
	/// </summary>
	public delegate int IntTernaryOperator(int a, int b, int c);

}