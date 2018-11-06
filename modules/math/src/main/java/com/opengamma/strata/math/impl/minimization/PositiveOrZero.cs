/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// A function from a vector x (<seealso cref="DoubleArray "/> to Boolean that returns true
	/// iff all the elements of x are positive or zero.
	/// </summary>
	public class PositiveOrZero : System.Func<DoubleArray, bool>
	{

	  public override bool? apply(DoubleArray x)
	  {
		double[] data = x.toArray();
		foreach (double value in data)
		{
		  if (value < 0.0)
		  {
			return false;
		  }
		}
		return true;
	  }

	}

}