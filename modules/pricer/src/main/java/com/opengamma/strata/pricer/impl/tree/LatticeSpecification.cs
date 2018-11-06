/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Lattice specification interface.
	/// <para>
	/// An implementation of the lattice specification defines construction of binomial and trinomial trees, and computes
	/// transition probabilities and state steps.
	/// </para>
	/// <para>
	/// Reference: Y. Iwashita, "Tree Option Pricing Models" OpenGamma Quantitative Research 23.
	/// </para>
	/// </summary>
	public interface LatticeSpecification
	{

	  /// <summary>
	  /// Computes parameters for uniform trinomial tree.
	  /// <para>
	  /// The interest rate must be zero-coupon continuously compounded rate.
	  /// </para>
	  /// <para>
	  /// The trinomial tree parameters are represented as {@code DoubleArray} containing [0] up factor, [1] middle factor, 
	  /// [2] down factor, [3] up probability, [4] middle probability, [5] down probability.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatility">  the volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="dt">  the time step </param>
	  /// <returns> the trinomial tree parameters </returns>
	  DoubleArray getParametersTrinomial(double volatility, double interestRate, double dt);

	}

}