/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
	using Array2DRowRealMatrix = org.apache.commons.math3.linear.Array2DRowRealMatrix;
	using EigenDecomposition = org.apache.commons.math3.linear.EigenDecomposition;
	using RealMatrix = org.apache.commons.math3.linear.RealMatrix;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RealPolynomialFunction1D = com.opengamma.strata.math.impl.function.RealPolynomialFunction1D;

	/// <summary>
	/// The eigenvalues of a matrix $\mathbf{A}$ are the roots of the characteristic
	/// polynomial $P(x) = \mathrm{det}[\mathbf{A} - x\mathbb{1}]$. For a 
	/// polynomial 
	/// $$
	/// \begin{align*}
	/// P(x) = \sum_{i=0}^n a_i x^i
	/// \end{align*} 
	/// $$
	/// an equivalent polynomial can be constructed from the characteristic polynomial of the matrix
	/// $$
	/// \begin{align*}
	/// A = 
	/// \begin{pmatrix}
	/// -\frac{a_{m-1}}{a_m}  & -\frac{a_{m-2}}{a_m} & \cdots & -\frac{a_{1}}{a_m} & -\frac{a_{0}}{a_m} \\
	/// 1                      & 0                     & \cdots & 0                   & 0                   \\
	/// 0                      & 1                     & \cdots & 0                   & 0                   \\
	/// \vdots                &                       & \cdots &                     & \vdots             \\
	/// 0                      & 0                     & \cdots & 1                   & 0                   
	/// \end{pmatrix}
	/// \end{align*}
	/// $$
	/// and so the roots are found by calculating the eigenvalues of this matrix.
	/// </summary>
	public class EigenvaluePolynomialRootFinder : Polynomial1DRootFinder<double>
	{

	  public virtual double?[] getRoots(RealPolynomialFunction1D function)
	  {
		ArgChecker.notNull(function, "function");
		double[] coeffs = function.Coefficients;
		int l = coeffs.Length - 1;
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] hessianDeref = new double[l][l];
		double[][] hessianDeref = RectangularArrays.ReturnRectangularDoubleArray(l, l);
		for (int i = 0; i < l; i++)
		{
		  hessianDeref[0][i] = -coeffs[l - i - 1] / coeffs[l];
		  for (int j = 1; j < l; j++)
		  {
			hessianDeref[j][i] = 0;
			if (i != l - 1)
			{
			  hessianDeref[i + 1][i] = 1;
			}
		  }
		}
		RealMatrix hessian = new Array2DRowRealMatrix(hessianDeref);
		double[] d = (new EigenDecomposition(hessian)).RealEigenvalues;
		double?[] result = new double?[d.Length];
		for (int i = 0; i < d.Length; i++)
		{
		  result[i] = d[i];
		}
		return result;
	  }

	}

}