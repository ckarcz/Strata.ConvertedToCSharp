using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	/// <summary>
	/// Computes the number of terms required to achieve a given accuracy in an orthogonal polynomial series.
	/// This code is an approximate translation of the equivalent function in the "Public Domain" code from SLATEC, see:
	/// http://www.netlib.org/slatec/fnlib/initds.f
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	internal sealed class INITDS
	{

	  /// <summary>
	  /// Computes an orthogonal series based on coefficients "os" including sufficient terms to insure that the error is no larger than 'eta'. </summary>
	  /// <param name="os"> array of coefficients of an orthogonal series </param>
	  /// <param name="nos"> number of coefficients in "os" </param>
	  /// <param name="eta"> usually 10% of machine precision, arbitrary! </param>
	  /// <returns> the number of terms needed in the series to achieve the desired accuracy </returns>
	  internal static int getInitds(double[] os, int nos, double eta)
	  {
		if (os == null)
		{
		  throw new MathException("INITDS: os is null");
		}
		if (nos != os.Length)
		{
		  throw new MathException("INITDS: nos and os.length are not equal, they should be!");
		}
		if (nos < 1)
		{
		  throw new MathException("INITDS: Number of coeffs is less than 1");
		}
		double err = 0;
		int i = 0;
		bool error = true;
		for (int ii = 0; ii < nos; ii++)
		{
		  i = nos - 1 - ii;
		  err += Math.Abs(os[i]); // Not quite what F77 things, no cast to float.
		  if (err > eta)
		  {
			error = false;
			break;
		  }
		}
		if (error)
		{
		  throw new MathException("INITDS: Chebyshev series too short for specified accuracy");
		}
		return i;
	  }

	}

}