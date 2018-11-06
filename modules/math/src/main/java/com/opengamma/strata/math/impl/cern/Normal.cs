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
 * - remove unused method
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
	/// Normal (aka Gaussian) distribution; See the <A HREF="http://www.cern.ch/RD11/rkb/AN16pp/node188.html#SECTION0001880000000000000000"> math definition</A>
	/// and <A HREF="http://www.statsoft.com/textbook/glosn.html#Normal Distribution"> animated definition</A>.
	/// <pre>                       
	///				   1                       2
	///	  pdf(x) = ---------    exp( - (x-mean) / 2v ) 
	///			   sqrt(2pi*v)
	/// 
	///							x
	///							 -
	///				   1        | |                 2
	///	  cdf(x) = ---------    |    exp( - (t-mean) / 2v ) dt
	///			   sqrt(2pi*v)| |
	///						   -
	///						  -inf.
	/// </pre>
	/// where <tt>v = variance = standardDeviation^2</tt>.
	/// <para>
	/// Instance methods operate on a user supplied uniform random number generator; they are unsynchronized.
	/// <dt>
	/// Static methods operate on a default uniform random number generator; they are synchronized.
	/// </para>
	/// <para>
	/// <b>Implementation:</b> Polar Box-Muller transformation. See 
	/// G.E.P. Box, M.E. Muller (1958): A note on the generation of random normal deviates, Annals Math. Statist. 29, 610-611.
	/// </para>
	/// <para>
	/// @author wolfgang.hoschek@cern.ch
	/// @version 1.0, 09/24/99
	/// </para>
	/// </summary>
	[Serializable]
	public class Normal : AbstractContinousDistribution
	{

	  private new const long serialVersionUID = 1L;

	  protected internal double mean;
	  protected internal double variance;
	  protected internal double standardDeviation;

	  protected internal double cache; // cache for Box-Mueller algorithm
	  protected internal bool cacheFilled; // Box-Mueller

	  protected internal double SQRT_INV; // performance cache

	  // The uniform random number generated shared by all <b>static</b> methods.
	  protected internal static Normal shared = new Normal(0.0, 1.0, makeDefaultGenerator());

	  /// <summary>
	  /// Constructs a normal (gauss) distribution.
	  /// Example: mean=0.0, standardDeviation=1.0. </summary>
	  /// <param name="mean"> mean </param>
	  /// <param name="standardDeviation"> standard deviation </param>
	  /// <param name="randomGenerator"> generator </param>
	  public Normal(double mean, double standardDeviation, RandomEngine randomGenerator)
	  {
		RandomGenerator = randomGenerator;
		setState(mean, standardDeviation);
	  }

	  /// <summary>
	  /// Returns the cumulative distribution function. </summary>
	  /// <param name="x"> x </param>
	  /// <returns> result </returns>
	  public virtual double cdf(double x)
	  {
		return Probability.normal(mean, variance, x);
	  }

	  /// <summary>
	  /// Returns a random number from the distribution.
	  /// </summary>
	  public override double nextDouble()
	  {
		return nextDouble(this.mean, this.standardDeviation);
	  }

	  /// <summary>
	  /// Returns a random number from the distribution; bypasses the internal state. </summary>
	  /// <param name="mean"> mean </param>
	  /// <param name="standardDeviation"> standard deviation </param>
	  /// <returns> result </returns>
	  public virtual double nextDouble(double mean, double standardDeviation)
	  {
		// Uses polar Box-Muller transformation.
		if (cacheFilled && this.mean == mean && this.standardDeviation == standardDeviation)
		{
		  cacheFilled = false;
		  return cache;
		}
		;

		double x, y, r, z;
		do
		{
		  x = 2.0 * randomGenerator.raw() - 1.0;
		  y = 2.0 * randomGenerator.raw() - 1.0;
		  r = x * x + y * y;
		} while (r >= 1.0);

		z = Math.Sqrt(-2.0 * Math.Log(r) / r);
		cache = mean + standardDeviation * x * z;
		cacheFilled = true;
		return mean + standardDeviation * y * z;
	  }

	  /// <summary>
	  /// Returns the probability distribution function. </summary>
	  /// <param name="x"> x </param>
	  /// <returns> result </returns>
	  public virtual double pdf(double x)
	  {
		double diff = x - mean;
		return SQRT_INV * Math.Exp(-(diff * diff) / (2.0 * variance));
	  }

	  /// <summary>
	  /// Sets the uniform random generator internally used.
	  /// </summary>
	  protected internal override RandomEngine RandomGenerator
	  {
		  set
		  {
			base.RandomGenerator = value;
			this.cacheFilled = false;
		  }
	  }

	  /// <summary>
	  /// Sets the mean and variance. </summary>
	  /// <param name="mean"> mean </param>
	  /// <param name="standardDeviation"> standard deviation </param>
	  public virtual void setState(double mean, double standardDeviation)
	  {
		if (mean != this.mean || standardDeviation != this.standardDeviation)
		{
		  this.mean = mean;
		  this.standardDeviation = standardDeviation;
		  this.variance = standardDeviation * standardDeviation;
		  this.cacheFilled = false;

		  this.SQRT_INV = 1.0 / Math.Sqrt(2.0 * Math.PI * variance);
		}
	  }

	  /// <summary>
	  /// Returns a random number from the distribution with the given mean and standard deviation. </summary>
	  /// <param name="mean"> mean </param>
	  /// <param name="standardDeviation"> standard deviation </param>
	  /// <returns> result </returns>
	  public static double staticNextDouble(double mean, double standardDeviation)
	  {
		lock (shared)
		{
		  return shared.nextDouble(mean, standardDeviation);
		}
	  }

	  /// <summary>
	  /// Returns a String representation of the receiver.
	  /// </summary>
	  public override string ToString()
	  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		return this.GetType().FullName + "(" + mean + "," + standardDeviation + ")";
	  }

	}

}