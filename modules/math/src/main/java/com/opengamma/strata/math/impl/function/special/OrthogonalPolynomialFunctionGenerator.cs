/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// 
	//CSOFF: JavadocMethod
	public abstract class OrthogonalPolynomialFunctionGenerator
	{

	  private static readonly RealPolynomialFunction1D ZERO = new RealPolynomialFunction1D(new double[] {0});
	  private static readonly RealPolynomialFunction1D ONE = new RealPolynomialFunction1D(new double[] {1});
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private static readonly RealPolynomialFunction1D X_Renamed = new RealPolynomialFunction1D(new double[] {0, 1});

	  public abstract DoubleFunction1D[] getPolynomials(int n);

	  public abstract Pair<DoubleFunction1D, DoubleFunction1D>[] getPolynomialsAndFirstDerivative(int n);

	  protected internal virtual DoubleFunction1D Zero
	  {
		  get
		  {
			return ZERO;
		  }
	  }

	  protected internal virtual DoubleFunction1D One
	  {
		  get
		  {
			return ONE;
		  }
	  }

	  protected internal virtual DoubleFunction1D X
	  {
		  get
		  {
			return X_Renamed;
		  }
	  }

	}

}