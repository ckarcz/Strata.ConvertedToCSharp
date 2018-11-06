/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using DoubleMath = com.google.common.math.DoubleMath;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// StudentT calculator.
	/// </summary>
	// CSOFF: JavadocMethod
	public class StudentTOneTailedCriticalValueCalculator : System.Func<double, double>
	{

	  private readonly ProbabilityDistribution<double> _dist;

	  public StudentTOneTailedCriticalValueCalculator(double nu)
	  {
		ArgChecker.notNegative(nu, "nu");
		_dist = new StudentTDistribution(nu);
	  }

	  public StudentTOneTailedCriticalValueCalculator(double nu, RandomEngine engine)
	  {
		ArgChecker.notNegative(nu, "nu");
		ArgChecker.notNull(engine, "engine");
		_dist = new StudentTDistribution(nu, engine);
	  }

	  public override double? apply(double? x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.notNegative(x, "x");
		if (DoubleMath.fuzzyEquals(x, 0.5, 1e-14))
		{
		  return 0.5;
		}
		return _dist.getInverseCDF(x.Value);
	  }

	}

}