using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.leastsquare
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using BasisFunctionAggregation = com.opengamma.strata.math.impl.interpolation.BasisFunctionAggregation;

	/// <summary>
	/// Generalized least square calculator.
	/// </summary>
	/// @param <T> The type of the inputs to the basis functions </param>
	public class GeneralizedLeastSquareResults<T> : LeastSquareResults
	{

	  private readonly System.Func<T, double> _function;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="basisFunctions">  the basis functions </param>
	  /// <param name="chiSq">  the chi-squared of the fit </param>
	  /// <param name="parameters">  the parameters that were fit </param>
	  /// <param name="covariance">  the covariance matrix of the result </param>
	  public GeneralizedLeastSquareResults(IList<System.Func<T, double>> basisFunctions, double chiSq, DoubleArray parameters, DoubleMatrix covariance) : base(chiSq, parameters, covariance, null)
	  {


		_function = new BasisFunctionAggregation<>(basisFunctions, parameters.toArray());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the functions field. </summary>
	  /// <returns> the functions </returns>
	  public virtual System.Func<T, double> Function
	  {
		  get
		  {
			return _function;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = base.GetHashCode();
		result = prime * result + _function.GetHashCode();
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (!base.Equals(obj))
		{
		  return false;
		}
		if (!(obj is GeneralizedLeastSquareResults))
		{
		  return false;
		}
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: GeneralizedLeastSquareResults<?> other = (GeneralizedLeastSquareResults<?>) obj;
		GeneralizedLeastSquareResults<object> other = (GeneralizedLeastSquareResults<object>) obj;
		if (!Objects.Equals(_function, other._function))
		{
		  return false;
		}
		return true;
	  }

	}

}