/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using RandomEngine = com.opengamma.strata.math.impl.cern.RandomEngine;

	/// <summary>
	/// StudentT calculator.
	/// </summary>
	// CSOFF: JavadocMethod
	public class StudentTTwoTailedCriticalValueCalculator : System.Func<double, double>
	{

	  private readonly System.Func<double, double> _calc;

	  public StudentTTwoTailedCriticalValueCalculator(double nu)
	  {
		ArgChecker.notNegative(nu, "nu");
		_calc = new StudentTOneTailedCriticalValueCalculator(nu);
	  }

	  public StudentTTwoTailedCriticalValueCalculator(double nu, RandomEngine engine)
	  {
		ArgChecker.notNegative(nu, "nu");
		ArgChecker.notNull(engine, "engine");
		_calc = new StudentTOneTailedCriticalValueCalculator(nu, engine);
	  }

	  public override double? apply(double? x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.notNegative(x, "x");
		return _calc.apply(0.5 + 0.5 * x);
	  }

	}

}