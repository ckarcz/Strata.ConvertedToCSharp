/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using Gamma = org.apache.commons.math3.special.Gamma;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// The natural logarithm of the Gamma function <seealso cref="GammaFunction"/>.
	/// <para>
	/// This class is a wrapper for the
	/// <a href="http://commons.apache.org/proper/commons-math/javadocs/api-3.5/org/apache/commons/math3/special/Gamma.html">Commons Math library implementation</a> 
	/// of the log-Gamma function
	/// </para>
	/// </summary>
	public class NaturalLogGammaFunction : System.Func<double, double>
	{

	  public override double? apply(double? x)
	  {
		ArgChecker.isTrue(x > 0, "x must be greater than zero");
		return Gamma.logGamma(x);
	  }

	}

}