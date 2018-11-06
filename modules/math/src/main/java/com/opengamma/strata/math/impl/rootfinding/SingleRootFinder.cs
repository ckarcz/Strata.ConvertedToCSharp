/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{

	/// <summary>
	/// Interface for classes that attempt to find a root for a one-dimensional function
	/// (see <seealso cref="Function"/>) $f(x)$ bounded by user-supplied values,
	/// $x_1$ and $x_2$. If there is not a single root between these  bounds, an exception is thrown.
	/// </summary>
	/// @param <S> The input type of the function </param>
	/// @param <T> The output type of the function </param>
	public interface SingleRootFinder<S, T>
	{

	  /// <summary>
	  /// Finds the root.
	  /// </summary>
	  /// <param name="function"> the function, not null </param>
	  /// <param name="roots">  the roots, not null </param>
	  /// <returns> a root lying between x1 and x2 </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public abstract S getRoot(System.Func<S, T> function, S... roots);
	  S getRoot(System.Func<S, T> function, params S[] roots);

	}

}