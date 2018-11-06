using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Generator for a set of basis functions.
	/// </summary>
	public class BasisFunctionGenerator
	{

	  /// <summary>
	  /// Generate a set of b-splines with a given polynomial degree on the specified knots. </summary>
	  /// <param name="knots"> holder for the knots and degree </param>
	  /// <returns> a List of functions </returns>
	  public virtual IList<System.Func<double, double>> generateSet(BasisFunctionKnots knots)
	  {
		ArgChecker.notNull(knots, "knots");

		double[] k = knots.Knots;
		IList<System.Func<double, double>> set = null;
		for (int d = 0; d <= knots.Degree; d++)
		{
		  set = generateSet(k, d, set);
		}
		return set;
	  }

	  /// <summary>
	  /// Generate a set of N-dimensional b-splines as the produce of 1-dimensional b-splines with a given polynomial degree.
	  /// on the specified knots </summary>
	  /// <param name="knots"> holder for the knots and degree in each dimension </param>
	  /// <returns> a List of functions </returns>
	  public virtual IList<System.Func<double[], double>> generateSet(BasisFunctionKnots[] knots)
	  {
		ArgChecker.noNulls(knots, "knots");
		int dim = knots.Length;
		int[] nSplines = new int[dim];
		int product = 1;
		IList<IList<System.Func<double, double>>> oneDSets = new List<IList<System.Func<double, double>>>(dim);
		for (int i = 0; i < dim; i++)
		{
		  oneDSets.Add(generateSet(knots[i]));
		  nSplines[i] = knots[i].NumSplines;
		  product *= nSplines[i];
		}
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<java.util.function.Function<double[], double>> functions = new java.util.ArrayList<>(product);
		IList<System.Func<double[], double>> functions = new List<System.Func<double[], double>>(product);

		for (int i = 0; i < product; i++)
		{
		  int[] indices = FunctionUtils.fromTensorIndex(i, nSplines);
		  functions.Add(generateMultiDim(oneDSets, indices));
		}
		return functions;
	  }

	  /// <summary>
	  /// Generate the i^th basis function </summary>
	  /// <param name="data"> Container for the knots and degree of the basis function </param>
	  /// <param name="index"> The index (from zero) of the function. Must be in range 0 to data.getNumSplines() (exclusive)
	  ///   For example if the degree is 1, and index is 0, this will cover the first three knots. </param>
	  /// <returns> The i^th basis function </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: protected java.util.function.Function<double, double> generate(BasisFunctionKnots data, final int index)
	  protected internal virtual System.Func<double, double> generate(BasisFunctionKnots data, int index)
	  {
		ArgChecker.notNull(data, "data");
		ArgChecker.isTrue(index >= 0 && index < data.NumSplines, "index must be in range {} to {} (exclusive)", 0, data.NumSplines);
		return generate(data.Knots, data.Degree, index);
	  }

	  /// <summary>
	  /// Generate the n-dimensional basis function for the given index position. This is formed as the product of 1-d basis
	  /// functions. </summary>
	  /// <param name="oneDSets"> The sets of basis functions in each dimension </param>
	  /// <param name="index"> index (from zero) of the basis function in each dimension. </param>
	  /// <returns> A n-dimensional basis function </returns>
	  private System.Func<double[], double> generateMultiDim(IList<IList<System.Func<double, double>>> oneDSets, int[] index)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int dim = index.length;
		int dim = index.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<java.util.function.Function<double, double>> funcs = new java.util.ArrayList<>(dim);
		IList<System.Func<double, double>> funcs = new List<System.Func<double, double>>(dim);
		for (int i = 0; i < dim; i++)
		{
		  funcs.Add(oneDSets[i][index[i]]);
		}

		return (double[] x) =>
		{
	double product = 1.0;
	ArgChecker.isTrue(dim == x.Length, "length of x {} was not equal to dimension {}", x.Length, dim);
	for (int i = 0; i < dim; i++)
	{
	  product *= funcs[i](x[i]);
	}
	return product;
		};
	  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private java.util.List<java.util.function.Function<double, double>> generateSet(final double[] knots, final int degree, final java.util.List<java.util.function.Function<double, double>> degreeM1Set)
	  private IList<System.Func<double, double>> generateSet(double[] knots, int degree, IList<System.Func<double, double>> degreeM1Set)
	  {

		int nSplines = knots.Length - degree - 1;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<java.util.function.Function<double, double>> functions = new java.util.ArrayList<>(nSplines);
		IList<System.Func<double, double>> functions = new List<System.Func<double, double>>(nSplines);
		for (int i = 0; i < nSplines; i++)
		{
		  functions.Add(generate(knots, degree, i, degreeM1Set));
		}
		return functions;
	  }

	  /// <summary>
	  /// Generate a basis function of the required degree either by using the set of functions one degree lower, or by recursion </summary>
	  /// <param name="knots"> The knots that support the basis functions </param>
	  /// <param name="degree"> The required degree </param>
	  /// <param name="index"> The index of the required function </param>
	  /// <param name="degreeM1Set"> Set of basis functions one degree lower than required (can be null) </param>
	  /// <returns> The basis function </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private java.util.function.Function<double, double> generate(final double[] knots, final int degree, final int index, final java.util.List<java.util.function.Function<double, double>> degreeM1Set)
	  private System.Func<double, double> generate(double[] knots, int degree, int index, IList<System.Func<double, double>> degreeM1Set)
	  {

		if (degree == 0)
		{
		  return new FuncAnonymousInnerClass(this, knots, index);
		}

		if (degree == 1)
		{
		  return new FuncAnonymousInnerClass2(this, knots, index);
		}

		// if degreeM1Set is unavailable, use the recursion
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> fa = degreeM1Set == null ? generate(knots, degree - 1, index) : degreeM1Set.get(index);
		System.Func<double, double> fa = degreeM1Set == null ? generate(knots, degree - 1, index) : degreeM1Set[index];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<double, double> fb = degreeM1Set == null ? generate(knots, degree - 1, index + 1) : degreeM1Set.get(index + 1);
		System.Func<double, double> fb = degreeM1Set == null ? generate(knots, degree - 1, index + 1) : degreeM1Set[index + 1];

		return new FuncAnonymousInnerClass3(this, knots, degree, index, fa, fb);

	  }

	  private class FuncAnonymousInnerClass : System.Func<double, double>
	  {
		  private readonly BasisFunctionGenerator outerInstance;

		  private double[] knots;
		  private int index;

		  public FuncAnonymousInnerClass(BasisFunctionGenerator outerInstance, double[] knots, int index)
		  {
			  this.outerInstance = outerInstance;
			  this.knots = knots;
			  this.index = index;
			  _l = knots[index];
			  _h = knots[index + 1];
		  }

		  private readonly double _l;
		  private readonly double _h;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> apply(final System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			return (x >= _l && x < _h) ? 1.0 : 0.0;
		  }
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<double, double>
	  {
		  private readonly BasisFunctionGenerator outerInstance;

		  private double[] knots;
		  private int index;

		  public FuncAnonymousInnerClass2(BasisFunctionGenerator outerInstance, double[] knots, int index)
		  {
			  this.outerInstance = outerInstance;
			  this.knots = knots;
			  this.index = index;
			  _l = knots[index];
			  _m = knots[index + 1];
			  _h = knots[index + 2];
			  _inv1 = 1.0 / (knots[index + 1] - knots[index]);
			  _inv2 = 1.0 / (knots[index + 2] - knots[index + 1]);
		  }

		  private readonly double _l;
		  private readonly double _m;
		  private readonly double _h;
		  private readonly double _inv1;
		  private readonly double _inv2;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> apply(final System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			return (x <= _l || x >= _h) ? 0.0 : (x <= _m ? (x - _l) * _inv1 : (_h - x) * _inv2);
		  }
	  }

	  private class FuncAnonymousInnerClass3 : System.Func<double, double>
	  {
		  private readonly BasisFunctionGenerator outerInstance;

		  private double[] knots;
		  private int degree;
		  private int index;
		  private System.Func<double, double> fa;
		  private System.Func<double, double> fb;

		  public FuncAnonymousInnerClass3(BasisFunctionGenerator outerInstance, double[] knots, int degree, int index, System.Func<double, double> fa, System.Func<double, double> fb)
		  {
			  this.outerInstance = outerInstance;
			  this.knots = knots;
			  this.degree = degree;
			  this.index = index;
			  this.fa = fa;
			  this.fb = fb;
			  _inv1 = 1.0 / (knots[index + degree] - knots[index]);
			  _inv2 = 1.0 / (knots[index + degree + 1] - knots[index + 1]);
			  _xa = knots[index];
			  _xb = knots[index + degree + 1];
		  }

		  private readonly double _inv1;
		  private readonly double _inv2;
		  private readonly double _xa;
		  private readonly double _xb;

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> apply(final System.Nullable<double> x)
		  public override double? apply(double? x)
		  {
			return (x - _xa) * _inv1 * fa(x) + (_xb - x) * _inv2 * fb(x);
		  }
	  }

	  /// <summary>
	  /// Generate a basis function of the required degree by recursion </summary>
	  /// <param name="knots"> The knots that support the basis functions </param>
	  /// <param name="degree"> The required degree </param>
	  /// <param name="index"> The index of the required function </param>
	  /// <returns> The basis function </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private java.util.function.Function<double, double> generate(final double[] knots, final int degree, final int index)
	  private System.Func<double, double> generate(double[] knots, int degree, int index)
	  {
		return generate(knots, degree, index, null);
	  }

	}

}