using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="CoxRossRubinsteinLatticeSpecification"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CoxRossRubinsteinLatticeSpecificationTest
	public class CoxRossRubinsteinLatticeSpecificationTest
	{

	  private const int NUM = 35;
	  private const double VOL = 0.12;
	  private const double RATE = 0.03;
	  private static readonly double DT = 2d / NUM;

	  public virtual void test_formula()
	  {
		CoxRossRubinsteinLatticeSpecification test = new CoxRossRubinsteinLatticeSpecification();
		DoubleArray computed = test.getParametersTrinomial(VOL, RATE, DT);
		double u = Math.Exp(VOL * Math.Sqrt(2.0 * DT));
		double d = Math.Exp(-VOL * Math.Sqrt(2.0 * DT));
		double up = Math.Pow((Math.Exp(0.5 * RATE * DT) - Math.Exp(-VOL * Math.Sqrt(0.5 * DT))) / (Math.Exp(VOL * Math.Sqrt(0.5 * DT)) - Math.Exp(-VOL * Math.Sqrt(0.5 * DT))), 2);
		double dp = Math.Pow((Math.Exp(VOL * Math.Sqrt(0.5 * DT)) - Math.Exp(0.5 * RATE * DT)) / (Math.Exp(VOL * Math.Sqrt(0.5 * DT)) - Math.Exp(-VOL * Math.Sqrt(0.5 * DT))), 2);
		DoubleArray expected = DoubleArray.of(u, 1d, d, up, 1d - up - dp, dp);
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected.toArray(), 1.0e-14));
	  }

	}

}