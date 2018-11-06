using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
/*
 * This code is copied from the original library from the `cern.jet.random` package.
 * Changes:
 * - package name
 * - missing Javadoc param tags
 * - reformat
 */
/*
Copyright � 1999 CERN - European Organization for Nuclear Research.
Permission to use, copy, modify, distribute and sell this software and its documentation for any purpose
is hereby granted without fee, provided that the above copyright notice appear in all copies and
that both that copyright notice and this permission notice appear in supporting documentation.
CERN makes no representations about the suitability of this software for any purpose.
It is provided "as is" without expressed or implied warranty.
*/
namespace com.opengamma.strata.math.impl.cern
{
	//CSOFF: ALL
	/// <summary>
	/// StudentT distribution (aka T-distribution); See the <A HREF="http://www.cern.ch/RD11/rkb/AN16pp/node279.html#SECTION0002790000000000000000"> math definition</A>
	/// and <A HREF="http://www.statsoft.com/textbook/gloss.html#Student's t Distribution"> animated definition</A>.
	/// <para>
	/// <tt>p(x) = k  *  (1+x^2/f) ^ -(f+1)/2</tt> where <tt>k = g((f+1)/2) / (sqrt(pi*f) * g(f/2))</tt> and <tt>g(a)</tt> being the gamma function and <tt>f</tt> being the degrees of freedom.
	/// </para>
	/// <para>
	/// Valid parameter ranges: <tt>freedom &gt; 0</tt>.
	/// </para>
	/// <para>
	/// Instance methods operate on a user supplied uniform random number generator; they are unsynchronized.
	/// <dt>
	/// Static methods operate on a default uniform random number generator; they are synchronized.
	/// </para>
	/// <para>
	/// <b>Implementation:</b>
	/// <dt>
	/// Method: Adapted Polar Box-Muller transformation.
	/// <dt>
	/// This is a port of <A HREF="http://wwwinfo.cern.ch/asd/lhc++/clhep/manual/RefGuide/Random/RandStudentT.html">RandStudentT</A> used in <A HREF="http://wwwinfo.cern.ch/asd/lhc++/clhep">CLHEP 1.4.0</A> (C++).
	/// CLHEP's implementation, in turn, is based on <tt>tpol.c</tt> from the <A HREF="http://www.cis.tu-graz.ac.at/stat/stadl/random.html">C-RAND / WIN-RAND</A> library.
	/// C-RAND's implementation, in turn, is based upon
	/// </para>
	/// <para>R.W. Bailey (1994): Polar generation of random variates with the t-distribution, Mathematics of Computation 62, 779-781.
	/// 
	/// @author wolfgang.hoschek@cern.ch
	/// @version 1.0, 09/24/99
	/// </para>
	/// </summary>
	[Serializable]
	public class StudentT : AbstractContinousDistribution
	{

	  private new const long serialVersionUID = 1L;

	  protected internal double freedom;

	  protected internal double TERM; // performance cache for pdf()
	  // The uniform random number generated shared by all <b>static</b> methods. 
	  protected internal static StudentT shared = new StudentT(1.0, makeDefaultGenerator());

	  /// <summary>
	  /// Constructs a StudentT distribution.
	  /// Example: freedom=1.0. </summary>
	  /// <param name="freedom"> degrees of freedom. </param>
	  /// <param name="randomGenerator"> input </param>
	  /// <exception cref="IllegalArgumentException"> if <tt>freedom &lt;= 0.0</tt>. </exception>
	  public StudentT(double freedom, RandomEngine randomGenerator)
	  {
		RandomGenerator = randomGenerator;
		State = freedom;
	  }

	  /// <summary>
	  /// Returns the cumulative distribution function. </summary>
	  /// <param name="x"> input </param>
	  /// <returns> result </returns>
	  public virtual double cdf(double x)
	  {
		return Probability.studentT(freedom, x);
	  }

	  /// <summary>
	  /// Returns a random number from the distribution.
	  /// </summary>
	  public override double nextDouble()
	  {
		return nextDouble(this.freedom);
	  }

	  /// <summary>
	  /// Returns a random number from the distribution; bypasses the internal state. </summary>
	  /// <param name="degreesOfFreedom"> degrees of freedom. </param>
	  /// <returns> result </returns>
	  /// <exception cref="IllegalArgumentException"> if <tt>a &lt;= 0.0</tt>. </exception>
	  public virtual double nextDouble(double degreesOfFreedom)
	  {
		/*
		 * The polar method of Box/Muller for generating Normal variates 
		 * is adapted to the Student-t distribution. The two generated   
		 * variates are not independent and the expected no. of uniforms 
		 * per variate is 2.5464.
		 *
		 * REFERENCE :  - R.W. Bailey (1994): Polar generation of random  
		 *                variates with the t-distribution, Mathematics   
		 *                of Computation 62, 779-781.
		 */
		if (degreesOfFreedom <= 0.0)
		{
		  throw new System.ArgumentException();
		}
		double u, v, w;

		do
		{
		  u = 2.0 * randomGenerator.raw() - 1.0;
		  v = 2.0 * randomGenerator.raw() - 1.0;
		} while ((w = u * u + v * v) > 1.0);

		return (u * Math.Sqrt(degreesOfFreedom * (Math.Exp(-2.0 / degreesOfFreedom * Math.Log(w)) - 1.0) / w));
	  }

	  /// <summary>
	  /// Returns the probability distribution function. </summary>
	  /// <param name="x"> input </param>
	  /// <returns> result </returns>
	  public virtual double pdf(double x)
	  {
		return this.TERM * Math.Pow((1 + x * x / freedom), -(freedom + 1) * 0.5);
	  }

	  /// <summary>
	  /// Sets the distribution parameter. </summary>
	  /// <param name="freedom"> degrees of freedom. </param>
	  /// <exception cref="IllegalArgumentException"> if <tt>freedom &lt;= 0.0</tt>. </exception>
	  public virtual double State
	  {
		  set
		  {
			if (value <= 0.0)
			{
			  throw new System.ArgumentException();
			}
			this.freedom = value;
    
			double val = Fun.logGamma((value + 1) / 2) - Fun.logGamma(value / 2);
			this.TERM = Math.Exp(val) / Math.Sqrt(Math.PI * value);
		  }
	  }

	  /// <summary>
	  /// Returns a random number from the distribution. </summary>
	  /// <param name="freedom"> degrees of freedom. </param>
	  /// <returns> result </returns>
	  /// <exception cref="IllegalArgumentException"> if <tt>freedom &lt;= 0.0</tt>. </exception>
	  public static double staticNextDouble(double freedom)
	  {
		lock (shared)
		{
		  return shared.nextDouble(freedom);
		}
	  }

	  /// <summary>
	  /// Returns a String representation of the receiver.
	  /// </summary>
	  public override string ToString()
	  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		return this.GetType().FullName + "(" + freedom + ")";
	  }

	}

}