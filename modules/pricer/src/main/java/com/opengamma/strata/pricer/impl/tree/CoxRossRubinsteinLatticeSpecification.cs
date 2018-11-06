using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Cox-Ross-Rubinstein lattice specification.
	/// </summary>
	public sealed class CoxRossRubinsteinLatticeSpecification : LatticeSpecification
	{

	  public DoubleArray getParametersTrinomial(double volatility, double interestRate, double dt)
	  {
		double dx = volatility * Math.Sqrt(2d * dt);
		double upFactor = Math.Exp(dx);
		double downFactor = Math.Exp(-dx);
		double factor1 = Math.Exp(0.5 * interestRate * dt);
		double factor2 = Math.Exp(0.5 * dx);
		double factor3 = Math.Exp(-0.5 * dx);
		double upProbability = Math.Pow((factor1 - factor3) / (factor3 - factor2), 2);
		double downProbability = Math.Pow((factor2 - factor1) / (factor3 - factor2), 2);
		double middleProbability = 1d - upProbability - downProbability;
		return DoubleArray.of(upFactor, 1d, downFactor, upProbability, middleProbability, downProbability);
	  }

	}

}