/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
	/// <summary>
	/// A function interface that allows a single parameter to be perturbed.
	/// <para>
	/// This interface is used by <seealso cref="ParameterizedData"/> to allow parameters to be
	/// efficiently perturbed (altered). The method is invoked with the parameter index,
	/// value and metadata, and must return the new value.
	/// </para>
	/// </summary>
	public delegate double ParameterPerturbation(int index, double value, ParameterMetadata metadata);

}