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
	/// Trigeorgis lattice specification.
	/// </summary>
	public sealed class TrigeorgisLatticeSpecification : LatticeSpecification
	{

	  public DoubleArray getParametersTrinomial(double volatility, double interestRate, double dt)
	  {
		double volSq = volatility * volatility;
		double mu = interestRate - 0.5 * volSq;
		double mudt = mu * dt;
		double mudtSq = mudt * mudt;
		double dx = volatility * Math.Sqrt(3d * dt);
		double upFactor = Math.Exp(dx);
		double downFactor = Math.Exp(-dx);
		double part = (volSq * dt + mudtSq) / dx / dx;
		double upProbability = 0.5 * (part + mudt / dx);
		double middleProbability = 1d - part;
		double downProbability = 0.5 * (part - mudt / dx);
		return DoubleArray.of(upFactor, 1d, downFactor, upProbability, middleProbability, downProbability);
	  }
	}

}