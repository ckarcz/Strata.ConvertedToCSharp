/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Direct inversion of a tridiagonal matrix using the method from
	/// "R. Usmani, Inversion of a tridiagonal Jacobi matrix, Linear Algebra Appl. 212/213 (1994) 413-414."
	/// </summary>
	public class InverseTridiagonalMatrixCalculator : System.Func<TridiagonalMatrix, DoubleMatrix>
	{

	  public override DoubleMatrix apply(TridiagonalMatrix x)
	  {
		ArgChecker.notNull(x, "x");
		double[] a = x.DiagonalData;
		double[] b = x.UpperSubDiagonalData;
		double[] c = x.LowerSubDiagonalData;
		int n = a.Length;
		int i, j, k;
		double[] theta = new double[n + 1];
		double[] phi = new double[n];

		theta[0] = 1.0;
		theta[1] = a[0];
		for (i = 2; i <= n; i++)
		{
		  theta[i] = a[i - 1] * theta[i - 1] - b[i - 2] * c[i - 2] * theta[i - 2];
		}

		if (theta[n] == 0.0)
		{
		  throw new MathException("Zero determinant. Cannot invert the matrix");
		}

		phi[n - 1] = 1.0;
		phi[n - 2] = a[n - 1];
		for (i = n - 3; i >= 0; i--)
		{
		  phi[i] = a[i + 1] * phi[i + 1] - b[i + 1] * c[i + 1] * phi[i + 2];
		}

		double product;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] res = new double[n][n];
		double[][] res = RectangularArrays.ReturnRectangularDoubleArray(n, n);

		for (j = 0; j < n; j++)
		{
		  for (i = 0; i <= j; i++)
		  {
			product = 1.0;
			for (k = i; k < j; k++)
			{
			  product *= b[k];
			}
			res[i][j] = ((i + j) % 2 == 0 ? 1 : -1) * product * theta[i] * phi[j] / theta[n];
		  }
		  for (i = j + 1; i < n; i++)
		  {
			product = 1.0;
			for (k = j; k < i; k++)
			{
			  product *= c[k];
			}
			res[i][j] = ((i + j) % 2 == 0 ? 1 : -1) * product * theta[j] * phi[i] / theta[n];
		  }
		}
		return DoubleMatrix.copyOf(res);
	  }

	}

}