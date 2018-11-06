/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
	/// <summary>
	/// Enum representing the various differencing types that can be used to estimate the gradient of a function.
	/// <para>
	/// Forward: $\frac{f(x + \epsilon) - f(x)}{\epsilon}$
	/// </para>
	/// <para>
	/// Central: $\frac{f(x + \epsilon) - f(x - \epsilon)}{2 * \epsilon}$
	/// </para>
	/// <para>
	/// Backward: $\frac{f(x) - f(x - \epsilon)}{\epsilon}$
	/// </para>
	/// </summary>
	public enum FiniteDifferenceType
	{

	  /// <summary>
	  /// Forward differencing
	  /// </summary>
	  FORWARD,
	  /// <summary>
	  /// Central differencing
	  /// </summary>
	  CENTRAL,
	  /// <summary>
	  /// Backward differencing
	  /// </summary>
	  BACKWARD

	}

}