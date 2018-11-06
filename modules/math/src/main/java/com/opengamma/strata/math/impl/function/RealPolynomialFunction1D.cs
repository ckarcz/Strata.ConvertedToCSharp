using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Class representing a polynomial that has real coefficients and takes a real
	/// argument. The function is defined as:
	/// $$
	/// \begin{align*}
	/// p(x) = a_0 + a_1 x + a_2 x^2 + \ldots + a_{n-1} x^{n-1}
	/// \end{align*}
	/// $$
	/// </summary>
	public class RealPolynomialFunction1D : DoubleFunction1D
	{

	  private readonly double[] _coefficients;
	  private readonly int _n;

	  /// <summary>
	  /// Creates an instance.
	  /// 
	  /// The array of coefficients for a polynomial
	  /// $p(x) = a_0 + a_1 x + a_2 x^2 + ... + a_{n-1} x^{n-1}$
	  /// is $\\{a_0, a_1, a_2, ..., a_{n-1}\\}$.
	  /// </summary>
	  /// <param name="coefficients">  the array of coefficients, not null or empty </param>
	  public RealPolynomialFunction1D(params double[] coefficients)
	  {
		ArgChecker.notNull(coefficients, "coefficients");
		ArgChecker.isTrue(coefficients.Length > 0, "coefficients length must be greater than zero");
		_coefficients = coefficients;
		_n = _coefficients.Length;
	  }

	  //-------------------------------------------------------------------------
	  public override double applyAsDouble(double x)
	  {
		ArgChecker.notNull(x, "x");
		double y = _coefficients[_n - 1];
		for (int i = _n - 2; i >= 0; i--)
		{
		  y = x * y + _coefficients[i];
		}
		return y;
	  }

	  /// <summary>
	  /// Gets the coefficients of this polynomial.
	  /// </summary>
	  /// <returns> the coefficients of this polynomial </returns>
	  public virtual double[] Coefficients
	  {
		  get
		  {
			return _coefficients;
		  }
	  }

	  /// <summary>
	  /// Adds a function to the polynomial.
	  /// If the function is not a <seealso cref="RealPolynomialFunction1D"/> then the addition takes
	  /// place as in <seealso cref="DoubleFunction1D"/>, otherwise the result will also be a polynomial.
	  /// </summary>
	  /// <param name="f">  the function to add </param>
	  /// <returns> $P+f$ </returns>
	  public override DoubleFunction1D add(DoubleFunction1D f)
	  {
		ArgChecker.notNull(f, "function");
		if (f is RealPolynomialFunction1D)
		{
		  RealPolynomialFunction1D p1 = (RealPolynomialFunction1D) f;
		  double[] c1 = p1.Coefficients;
		  double[] c = _coefficients;
		  int n = c1.Length;
		  bool longestIsNew = n > _n;
		  double[] c3 = longestIsNew ? Arrays.copyOf(c1, n) : Arrays.copyOf(c, _n);
		  for (int i = 0; i < (longestIsNew ? _n : n); i++)
		  {
			c3[i] += longestIsNew ? c[i] : c1[i];
		  }
		  return new RealPolynomialFunction1D(c3);
		}
		return DoubleFunction1D.this.add(f);
	  }

	  /// <summary>
	  /// Adds a constant to the polynomial (equivalent to adding the value to the constant
	  /// term of the polynomial). The result is also a polynomial.
	  /// </summary>
	  /// <param name="a">  the value to add </param>
	  /// <returns> $P+a$  </returns>
	  public override RealPolynomialFunction1D add(double a)
	  {
		double[] c = Arrays.copyOf(Coefficients, _n);
		c[0] += a;
		return new RealPolynomialFunction1D(c);
	  }

	  /// <summary>
	  /// Returns the derivative of this polynomial (also a polynomial), where
	  /// $$
	  /// \begin{align*}
	  /// P'(x) = a_1 + 2 a_2 x + 3 a_3 x^2 + 4 a_4 x^3 + \dots + n a_n x^{n-1}
	  /// \end{align*}
	  /// $$.
	  /// </summary>
	  /// <returns> the derivative polynomial </returns>
	  public override RealPolynomialFunction1D derivative()
	  {
		int n = _coefficients.Length - 1;
		double[] coefficients = new double[n];
		for (int i = 1; i <= n; i++)
		{
		  coefficients[i - 1] = i * _coefficients[i];
		}
		return new RealPolynomialFunction1D(coefficients);
	  }

	  /// <summary>
	  /// Divides the polynomial by a constant value (equivalent to dividing each coefficient by this value).
	  /// The result is also a polynomial.
	  /// </summary>
	  /// <param name="a">  the divisor </param>
	  /// <returns> the polynomial  </returns>
	  public override RealPolynomialFunction1D divide(double a)
	  {
		double[] c = Arrays.copyOf(Coefficients, _n);
		for (int i = 0; i < _n; i++)
		{
		  c[i] /= a;
		}
		return new RealPolynomialFunction1D(c);
	  }

	  /// <summary>
	  /// Multiplies the polynomial by a function.
	  /// If the function is not a <seealso cref="RealPolynomialFunction1D"/> then the multiplication takes
	  /// place as in <seealso cref="DoubleFunction1D"/>, otherwise the result will also be a polynomial.
	  /// </summary>
	  /// <param name="f">  the function by which to multiply </param>
	  /// <returns> $P \dot f$ </returns>
	  public override DoubleFunction1D multiply(DoubleFunction1D f)
	  {
		ArgChecker.notNull(f, "function");
		if (f is RealPolynomialFunction1D)
		{
		  RealPolynomialFunction1D p1 = (RealPolynomialFunction1D) f;
		  double[] c = _coefficients;
		  double[] c1 = p1.Coefficients;
		  int m = c1.Length;
		  double[] newC = new double[_n + m - 1];
		  for (int i = 0; i < newC.Length; i++)
		  {
			newC[i] = 0;
			for (int j = Math.Max(0, i + 1 - m); j < Math.Min(_n, i + 1); j++)
			{
			  newC[i] += c[j] * c1[i - j];
			}
		  }
		  return new RealPolynomialFunction1D(newC);
		}
		return DoubleFunction1D.this.multiply(f);
	  }

	  /// <summary>
	  /// Multiplies the polynomial by a constant value (equivalent to multiplying each
	  /// coefficient by this value). The result is also a polynomial.
	  /// </summary>
	  /// <param name="a">  the multiplicator </param>
	  /// <returns> the polynomial  </returns>
	  public override RealPolynomialFunction1D multiply(double a)
	  {
		double[] c = Arrays.copyOf(Coefficients, _n);
		for (int i = 0; i < _n; i++)
		{
		  c[i] *= a;
		}
		return new RealPolynomialFunction1D(c);
	  }

	  /// <summary>
	  /// Subtracts a function from the polynomial.
	  /// <para>
	  /// If the function is not a <seealso cref="RealPolynomialFunction1D"/> then the subtract takes place
	  /// as in <seealso cref="DoubleFunction1D"/>, otherwise the result will also be a polynomial.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="f">  the function to subtract </param>
	  /// <returns> $P-f$ </returns>
	  public override DoubleFunction1D subtract(DoubleFunction1D f)
	  {
		ArgChecker.notNull(f, "function");
		if (f is RealPolynomialFunction1D)
		{
		  RealPolynomialFunction1D p1 = (RealPolynomialFunction1D) f;
		  double[] c = _coefficients;
		  double[] c1 = p1.Coefficients;
		  int m = c.Length;
		  int n = c1.Length;
		  int min = Math.Min(m, n);
		  int max = Math.Max(m, n);
		  double[] c3 = new double[max];
		  for (int i = 0; i < min; i++)
		  {
			c3[i] = c[i] - c1[i];
		  }
		  for (int i = min; i < max; i++)
		  {
			if (m == max)
			{
			  c3[i] = c[i];
			}
			else
			{
			  c3[i] = -c1[i];
			}
		  }
		  return new RealPolynomialFunction1D(c3);
		}
		return DoubleFunction1D.this.subtract(f);
	  }

	  /// <summary>
	  /// Subtracts a constant from the polynomial (equivalent to subtracting the value from the
	  /// constant term of the polynomial). The result is also a polynomial.
	  /// </summary>
	  /// <param name="a">  the value to add </param>
	  /// <returns> $P-a$  </returns>
	  public override RealPolynomialFunction1D subtract(double a)
	  {
		double[] c = Arrays.copyOf(Coefficients, _n);
		c[0] -= a;
		return new RealPolynomialFunction1D(c);
	  }

	  /// <summary>
	  /// Converts the polynomial to its monic form. If 
	  /// $$
	  /// \begin{align*}
	  /// P(x) = a_0 + a_1 x + a_2 x^2 + a_3 x^3 \dots + a_n x^n
	  /// \end{align*}
	  /// $$
	  /// then the monic form is
	  /// $$
	  /// \begin{align*}
	  /// P(x) = \lambda_0 + \lambda_1 x + \lambda_2 x^2 + \lambda_3 x^3 \dots + x^n
	  /// \end{align*}
	  /// $$
	  /// where 
	  /// $$
	  /// \begin{align*}
	  /// \lambda_i = \frac{a_i}{a_n}
	  /// \end{align*}
	  /// $$
	  /// </summary>
	  /// <returns> the polynomial in monic form </returns>
	  public virtual RealPolynomialFunction1D toMonic()
	  {
		double an = _coefficients[_n - 1];
		if (DoubleMath.fuzzyEquals(an, (double) 1, 1e-15))
		{
		  return new RealPolynomialFunction1D(Arrays.copyOf(_coefficients, _n));
		}
		double[] rescaled = new double[_n];
		for (int i = 0; i < _n; i++)
		{
		  rescaled[i] = _coefficients[i] / an;
		}
		return new RealPolynomialFunction1D(rescaled);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		RealPolynomialFunction1D other = (RealPolynomialFunction1D) obj;
		if (!Arrays.Equals(_coefficients, other._coefficients))
		{
		  return false;
		}
		return true;
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		result = prime * result + Arrays.GetHashCode(_coefficients);
		return result;
	  }

	}

}