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
	/// Test <seealso cref="TrigeorgisLatticeSpecification"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TrigeorgisLatticeSpecificationTest
	public class TrigeorgisLatticeSpecificationTest
	{

	  private const int NUM = 35;
	  private const double VOL = 0.13;
	  private const double RATE = 0.03;
	  private static readonly double DT = 2d / NUM;

	  public virtual void test_formula()
	  {
		TrigeorgisLatticeSpecification test = new TrigeorgisLatticeSpecification();
		DoubleArray computed = test.getParametersTrinomial(VOL, RATE, DT);
		double dx = VOL * Math.Sqrt(3d * DT);
		double nu = RATE - 0.5 * VOL * VOL;
		double u = Math.Exp(dx);
		double d = Math.Exp(-dx);
		double up = 0.5 * ((VOL * VOL * DT + nu * nu * DT * DT) / (dx * dx) + nu * DT / dx);
		double dm = 1d - (VOL * VOL * DT + nu * nu * DT * DT) / (dx * dx);
		double dp = 0.5 * ((VOL * VOL * DT + nu * nu * DT * DT) / (dx * dx) - nu * DT / dx);
		DoubleArray expected = DoubleArray.of(u, 1d, d, up, dm, dp);
		assertTrue(DoubleArrayMath.fuzzyEquals(computed.toArray(), expected.toArray(), 1.0e-14));
	  }

	}

}