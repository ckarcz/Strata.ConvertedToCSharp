/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// 
	//CSOFF: JavadocMethod
	public abstract class MatrixValidate
	{

	  public static void notNaNOrInfinite(DoubleMatrix x)
	  {
		int rows = x.rowCount();
		int cols = x.columnCount();
		for (int i = 0; i < rows; i++)
		{
		  for (int j = 0; j < cols; j++)
		  {
			double temp = x.get(i, j);
			if (double.IsNaN(temp))
			{
			  throw new MathException("Matrix contains a NaN");
			}
			if (double.IsInfinity(temp))
			{
			  throw new MathException("Matrix contains an infinite");
			}
		  }
		}
	  }

	}

}