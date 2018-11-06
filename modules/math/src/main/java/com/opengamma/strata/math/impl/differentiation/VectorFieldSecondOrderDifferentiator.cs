/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// The Vector field second order differentiator.
	/// </summary>
	public class VectorFieldSecondOrderDifferentiator : Differentiator<DoubleArray, DoubleArray, DoubleMatrix[]>
	{

	  private const double DEFAULT_EPS = 1e-4;

	  private readonly double eps;
	  private readonly double epsSqr;
	  private readonly VectorFieldFirstOrderDifferentiator vectorFieldDiff;
	  private readonly MatrixFieldFirstOrderDifferentiator maxtrixFieldDiff;

	  /// <summary>
	  /// Creates an instance using the default values.
	  /// </summary>
	  public VectorFieldSecondOrderDifferentiator()
	  {
		this.eps = DEFAULT_EPS;
		this.epsSqr = eps * eps;
		this.vectorFieldDiff = new VectorFieldFirstOrderDifferentiator(eps);
		this.maxtrixFieldDiff = new MatrixFieldFirstOrderDifferentiator(eps);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// This computes the second derivative of a vector field, which is a rank 3 tensor field.
	  /// The tensor is represented as an array of DoubleMatrix, where each matrix is
	  /// a Hessian (for the dependent variable y_i), so the indexing is
	  /// H^i_{j,k} =\partial^2y_i/\partial x_j \partial x_k
	  /// </summary>
	  /// <param name="function">  the function representing the vector field </param>
	  /// <returns> a function representing the second derivative of the vector field (i.e. a rank 3 tensor field) </returns>
	  public virtual System.Func<DoubleArray, DoubleMatrix[]> differentiate(System.Func<DoubleArray, DoubleArray> function)
	  {

		ArgChecker.notNull(function, "function");
		System.Func<DoubleArray, DoubleMatrix> jacFunc = vectorFieldDiff.differentiate(function);
		System.Func<DoubleArray, DoubleMatrix[]> hFunc = maxtrixFieldDiff.differentiate(jacFunc);
		return new FuncAnonymousInnerClass(this, hFunc);
	  }

	  private class FuncAnonymousInnerClass : System.Func<DoubleArray, DoubleMatrix[]>
	  {
		  private readonly VectorFieldSecondOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleMatrix[]> hFunc;

		  public FuncAnonymousInnerClass(VectorFieldSecondOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleMatrix[]> hFunc)
		  {
			  this.outerInstance = outerInstance;
			  this.hFunc = hFunc;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix[] apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix[] apply(DoubleArray x)
		  {
			DoubleMatrix[] gamma = hFunc(x);
			return outerInstance.reshapeTensor(gamma);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual System.Func<DoubleArray, DoubleMatrix[]> differentiate(System.Func<DoubleArray, DoubleArray> function, System.Func<DoubleArray, bool> domain)
	  {

		ArgChecker.notNull(function, "function");
		System.Func<DoubleArray, DoubleMatrix> jacFunc = vectorFieldDiff.differentiate(function, domain);
		System.Func<DoubleArray, DoubleMatrix[]> hFunc = maxtrixFieldDiff.differentiate(jacFunc, domain);
		return new FuncAnonymousInnerClass2(this, hFunc);
	  }

	  private class FuncAnonymousInnerClass2 : System.Func<DoubleArray, DoubleMatrix[]>
	  {
		  private readonly VectorFieldSecondOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleMatrix[]> hFunc;

		  public FuncAnonymousInnerClass2(VectorFieldSecondOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleMatrix[]> hFunc)
		  {
			  this.outerInstance = outerInstance;
			  this.hFunc = hFunc;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix[] apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix[] apply(DoubleArray x)
		  {
			DoubleMatrix[] gamma = hFunc(x);
			return outerInstance.reshapeTensor(gamma);
		  }
	  }

	  /// <summary>
	  /// Gamma is in the  form gamma^i_{j,k} =\partial^2y_j/\partial x_i \partial x_k, where i is the
	  /// index of the matrix in the stack (3rd index of the tensor), and j,k are the individual
	  /// matrix indices. We would like it in the form H^i_{j,k} =\partial^2y_i/\partial x_j \partial x_k,
	  /// so that each matrix is a Hessian (for the dependent variable y_i), hence the reshaping below.
	  /// </summary>
	  /// <param name="gamma">  the rank 3 tensor </param>
	  /// <returns> the reshaped rank 3 tensor </returns>
	  private DoubleMatrix[] reshapeTensor(DoubleMatrix[] gamma)
	  {
		int m = gamma.Length;
		int n = gamma[0].rowCount();
		ArgChecker.isTrue(gamma[0].columnCount() == m, "tenor wrong size. Seond index is {}, should be {}", gamma[0].columnCount(), m);
		DoubleMatrix[] res = new DoubleMatrix[n];
		for (int i = 0; i < n; i++)
		{
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] temp = new double[m][m];
		  double[][] temp = RectangularArrays.ReturnRectangularDoubleArray(m, m);
		  for (int j = 0; j < m; j++)
		  {
			DoubleMatrix gammaJ = gamma[j];
			for (int k = j; k < m; k++)
			{
			  temp[j][k] = gammaJ.get(i, k);
			}
		  }
		  for (int j = 0; j < m; j++)
		  {
			for (int k = 0; k < j; k++)
			{
			  temp[j][k] = temp[k][j];
			}
		  }
		  res[i] = DoubleMatrix.copyOf(temp);
		}
		return res;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Differentiate.
	  /// </summary>
	  /// <param name="function">  the function </param>
	  /// <returns> the result </returns>
	  public virtual System.Func<DoubleArray, DoubleMatrix[]> differentiateFull(System.Func<DoubleArray, DoubleArray> function)
	  {

		return new FuncAnonymousInnerClass3(this, function);
	  }

	  private class FuncAnonymousInnerClass3 : System.Func<DoubleArray, DoubleMatrix[]>
	  {
		  private readonly VectorFieldSecondOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;

		  public FuncAnonymousInnerClass3(VectorFieldSecondOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix[] apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix[] apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			DoubleArray y = function(x);
			int n = x.size();
			int m = y.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][][] res = new double[m][n][n];
			double[][][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n, n);

			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray xPlusOneEps = x.with(j, xj + outerInstance.eps);
			  DoubleArray xMinusOneEps = x.with(j, xj - outerInstance.eps);
			  DoubleArray up = function(x.with(j, xj + outerInstance.eps));
			  DoubleArray down = function(xMinusOneEps);
			  for (int i = 0; i < m; i++)
			  {
				res[i][j][j] = (up.get(i) + down.get(i) - 2 * y.get(i)) / outerInstance.epsSqr;
			  }
			  for (int k = j + 1; k < n; k++)
			  {
				double xk = x.get(k);
				DoubleArray downup = function(xMinusOneEps.with(k, xk + outerInstance.eps));
				DoubleArray downdown = function(xMinusOneEps.with(k, xk - outerInstance.eps));
				DoubleArray updown = function(xPlusOneEps.with(k, xk - outerInstance.eps));
				DoubleArray upup = function(xPlusOneEps.with(k, xk + outerInstance.eps));
				for (int i = 0; i < m; i++)
				{
				  res[i][j][k] = (upup.get(i) + downdown.get(i) - updown.get(i) - downup.get(i)) / 4 / outerInstance.epsSqr;
				}
			  }
			}
			DoubleMatrix[] mres = new DoubleMatrix[m];
			for (int i = 0; i < m; i++)
			{
			  for (int j = 0; j < n; j++)
			  {
				for (int k = 0; k < j; k++)
				{
				  res[i][j][k] = res[i][k][j];
				}
			  }
			  mres[i] = DoubleMatrix.copyOf(res[i]);
			}
			return mres;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the second derivative of a vector field, without cross derivatives. 
	  /// 
	  /// This creates a function returning a matrix whose {i,j} element is 
	  /// $H^i_{j} = \partial^2y_i/\partial x_j \partial x_j$.
	  /// </summary>
	  /// <param name="function">  the function representing the vector field </param>
	  /// <returns> a function representing the second derivative of the vector field (i.e. a rank 3 tensor field) </returns>
	  public virtual System.Func<DoubleArray, DoubleMatrix> differentiateNoCross(System.Func<DoubleArray, DoubleArray> function)
	  {

		return new FuncAnonymousInnerClass4(this, function);
	  }

	  private class FuncAnonymousInnerClass4 : System.Func<DoubleArray, DoubleMatrix>
	  {
		  private readonly VectorFieldSecondOrderDifferentiator outerInstance;

		  private System.Func<DoubleArray, DoubleArray> function;

		  public FuncAnonymousInnerClass4(VectorFieldSecondOrderDifferentiator outerInstance, System.Func<DoubleArray, DoubleArray> function)
		  {
			  this.outerInstance = outerInstance;
			  this.function = function;
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public com.opengamma.strata.collect.array.DoubleMatrix apply(com.opengamma.strata.collect.array.DoubleArray x)
		  public override DoubleMatrix apply(DoubleArray x)
		  {
			ArgChecker.notNull(x, "x");
			DoubleArray y = function(x);
			int n = x.size();
			int m = y.size();
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[m][n];
			double[][] res = RectangularArrays.ReturnRectangularDoubleArray(m, n);
			for (int j = 0; j < n; j++)
			{
			  double xj = x.get(j);
			  DoubleArray up = function(x.with(j, xj + outerInstance.eps));
			  DoubleArray down = function(x.with(j, xj - outerInstance.eps));
			  for (int i = 0; i < m; i++)
			  {
				res[i][j] = (up.get(i) + down.get(i) - 2d * y.get(i)) / outerInstance.epsSqr;
			  }
			}
			return DoubleMatrix.copyOf(res);
		  }
	  }

	}

}