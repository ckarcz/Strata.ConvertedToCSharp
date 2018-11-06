/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Abstraction for the vector function $f: \mathbb{R}^m \to \mathbb{R}^n \quad x \mapsto f(x)$ where the 
	/// Jacobian $j : \mathbb{R}^m \to \mathbb{R}^{n\times m} \quad x \mapsto j(x)$ is also provided.
	/// </summary>
	public abstract class VectorFunction : System.Func<DoubleArray, DoubleArray>
	{

	  /// <summary>
	  /// Calculate the Jacobian at a point $\mathbf{x}$. For a function 
	  /// $f: \mathbb{R}^m \to \mathbb{R}^n \quad x \mapsto f(x)$, the Jacobian is a n by m matrix.
	  /// </summary>
	  /// <param name="x">  the input vector $\mathbf{x}$ </param>
	  /// <returns> the Jacobian $\mathbf{J}$ </returns>
	  public abstract DoubleMatrix calculateJacobian(DoubleArray x);

	  /// <summary>
	  /// The length of the input vector $\mathbf{x}$.
	  /// </summary>
	  /// <returns> length of input vector (domain)  </returns>
	  public abstract int LengthOfDomain {get;}

	  /// <summary>
	  /// The length of the output vector $\mathbf{y}$.
	  /// </summary>
	  /// <returns> length of output vector (range)  </returns>
	  public abstract int LengthOfRange {get;}

	}

}