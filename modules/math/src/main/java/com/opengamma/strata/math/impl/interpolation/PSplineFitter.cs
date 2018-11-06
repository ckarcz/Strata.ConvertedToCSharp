using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using GeneralizedLeastSquare = com.opengamma.strata.math.impl.statistics.leastsquare.GeneralizedLeastSquare;
	using GeneralizedLeastSquareResults = com.opengamma.strata.math.impl.statistics.leastsquare.GeneralizedLeastSquareResults;

	/// <summary>
	/// P-Spline fitter.
	/// </summary>
	public class PSplineFitter
	{

	  private readonly BasisFunctionGenerator _generator = new BasisFunctionGenerator();
	  private readonly GeneralizedLeastSquare _gls = new GeneralizedLeastSquare();

	  /// <summary>
	  /// Fits a curve to x-y data. </summary>
	  /// <param name="x"> The independent variables </param>
	  /// <param name="y"> The dependent variables </param>
	  /// <param name="sigma"> The error (or tolerance) on the y variables </param>
	  /// <param name="xa"> The lowest value of x </param>
	  /// <param name="xb"> The highest value of x </param>
	  /// <param name="nKnots"> Number of knots (note, the actual number of basis splines and thus fitted weights, equals nKnots + degree-1) </param>
	  /// <param name="degree"> The degree of the basis function - 0 is piecewise constant, 1 is a sawtooth function (i.e. two straight lines joined in the middle), 2 gives three 
	  ///   quadratic sections joined together, etc. For a large value of degree, the basis function tends to a gaussian </param>
	  /// <param name="lambda"> The weight given to the penalty function </param>
	  /// <param name="differenceOrder"> applies the penalty the nth order difference in the weights, so a differenceOrder of 2 will penalise large 2nd derivatives etc </param>
	  /// <returns> The results of the fit </returns>
	  public virtual GeneralizedLeastSquareResults<double> solve(IList<double> x, IList<double> y, IList<double> sigma, double xa, double xb, int nKnots, int degree, double lambda, int differenceOrder)
	  {
		IList<System.Func<double, double>> bSplines = _generator.generateSet(BasisFunctionKnots.fromUniform(xa, xb, nKnots, degree));
		return _gls.solve(x, y, sigma, bSplines, lambda, differenceOrder);
	  }

	  /// <summary>
	  /// Given a set of data {x_i ,y_i} where each x_i is a vector and the y_i are scalars, we wish to find a function (represented
	  /// by B-splines) that fits the data while maintaining smoothness in each direction. </summary>
	  /// <param name="x"> The independent (vector) variables, as List&lt;double[]> </param>
	  /// <param name="y"> The dependent variables, as List&lt;Double> y </param>
	  /// <param name="sigma"> The error (or tolerance) on the y variables </param>
	  /// <param name="xa">  The lowest value of x in each dimension </param>
	  /// <param name="xb"> The highest value of x in each dimension </param>
	  /// <param name="nKnots"> Number of knots in each dimension (note, the actual number of basis splines and thus fitted weights,
	  ///   equals nKnots + degree-1) </param>
	  /// <param name="degree"> The degree of the basis function in each dimension - 0 is piecewise constant, 1 is a sawtooth function
	  ///   (i.e. two straight lines joined in the middle), 2 gives three quadratic sections joined together, etc. For a large
	  ///   value of degree, the basis function tends to a gaussian </param>
	  /// <param name="lambda"> The weight given to the penalty function in each dimension </param>
	  /// <param name="differenceOrder"> applies the penalty the nth order difference in the weights, so a differenceOrder of 2
	  ///   will penalize large 2nd derivatives etc. A difference differenceOrder can be used in each dimension </param>
	  /// <returns> The results of the fit </returns>
	  public virtual GeneralizedLeastSquareResults<double[]> solve(IList<double[]> x, IList<double> y, IList<double> sigma, double[] xa, double[] xb, int[] nKnots, int[] degree, double[] lambda, int[] differenceOrder)
	  {
		BasisFunctionKnots[] knots = new BasisFunctionKnots[xa.Length];
		for (int i = 0; i < xa.Length; i++)
		{
		  knots[i] = BasisFunctionKnots.fromUniform(xa[i], xb[i], nKnots[i], degree[i]);
		}
		IList<System.Func<double[], double>> bSplines = _generator.generateSet(knots);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = xa.length;
		int dim = xa.Length;
		int[] sizes = new int[dim];
		for (int i = 0; i < dim; i++)
		{
		  sizes[i] = nKnots[i] + degree[i] - 1;
		}
		return _gls.solve(x, y, sigma, bSplines, sizes, lambda, differenceOrder);
	  }

	}

}