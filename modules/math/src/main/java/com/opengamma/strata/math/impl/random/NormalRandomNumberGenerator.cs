using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.random
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;
	using NormalDistribution = com.opengamma.strata.math.impl.statistics.distribution.NormalDistribution;
	using ProbabilityDistribution = com.opengamma.strata.math.impl.statistics.distribution.ProbabilityDistribution;

	/// <summary>
	/// Random number generator based on {@code ProbabilityDistribution}. 
	/// </summary>
	public class NormalRandomNumberGenerator : RandomNumberGenerator
	{

	  /// <summary>
	  /// The underlying distribution.
	  /// </summary>
	  private readonly ProbabilityDistribution<double> normal;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mean">  the mean </param>
	  /// <param name="sigma">  the sigma </param>
	  public NormalRandomNumberGenerator(double mean, double sigma)
	  {
		ArgChecker.notNegativeOrZero(sigma, "standard deviation");
		this.normal = new NormalDistribution(mean, sigma);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="mean">  the mean </param>
	  /// <param name="sigma">  the sigma </param>
	  /// <param name="engine">  the random number engine </param>
	  public NormalRandomNumberGenerator(double mean, double sigma, RandomEngine engine)
	  {
		ArgChecker.notNegativeOrZero(sigma, "standard deviation");
		ArgChecker.notNull(engine, "engine");
		this.normal = new NormalDistribution(mean, sigma, engine);
	  }

	  //-------------------------------------------------------------------------
	  public virtual double[] getVector(int size)
	  {
		ArgChecker.notNegative(size, "size");
		double[] result = new double[size];
		for (int i = 0; i < size; i++)
		{
		  result[i] = normal.nextRandom();
		}
		return result;
	  }

	  public virtual IList<double[]> getVectors(int arraySize, int listSize)
	  {
		ArgChecker.notNegative(arraySize, "arraySize");
		ArgChecker.notNegative(listSize, "listSize");
		IList<double[]> result = new List<double[]>(listSize);
		double[] x;
		for (int i = 0; i < listSize; i++)
		{
		  x = new double[arraySize];
		  for (int j = 0; j < arraySize; j++)
		  {
			x[j] = normal.nextRandom();
		  }
		  result.Add(x);
		}
		return result;
	  }

	}

}