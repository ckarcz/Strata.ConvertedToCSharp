/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{
	using Test = org.testng.annotations.Test;

	using SVDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.SVDecompositionCommons;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BroydenVectorRootFinderTest extends VectorRootFinderTest
	public class BroydenVectorRootFinderTest : VectorRootFinderTest
	{
	  private static readonly BaseNewtonVectorRootFinder DEFAULT = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS);
	  private static readonly BaseNewtonVectorRootFinder SV = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS, new SVDecompositionCommons());
	  private static readonly BaseNewtonVectorRootFinder DEFAULT_JACOBIAN_2D = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS);
	  private static readonly BaseNewtonVectorRootFinder SV_JACOBIAN_2D = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS, new SVDecompositionCommons());
	  private static readonly BaseNewtonVectorRootFinder DEFAULT_JACOBIAN_3D = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS);
	  private static readonly BaseNewtonVectorRootFinder SV_JACOBIAN_3D = new BroydenVectorRootFinder(TOLERANCE, TOLERANCE, MAXSTEPS, new SVDecompositionCommons());

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSingular1()
	  public virtual void testSingular1()
	  {
		assertFunction2D(DEFAULT, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testSingular2()
	  public virtual void testSingular2()
	  {
		assertFunction2D(DEFAULT_JACOBIAN_2D, EPS);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertLinear(DEFAULT, EPS);
		assertLinear(SV, EPS);
		assertFunction2D(SV, EPS);
		assertFunction2D(SV_JACOBIAN_2D, EPS);
		assertFunction3D(DEFAULT, EPS);
		assertFunction3D(DEFAULT_JACOBIAN_3D, EPS);
		assertFunction3D(SV, EPS);
		assertFunction3D(SV_JACOBIAN_3D, EPS);
		assertYieldCurveBootstrap(DEFAULT, EPS);
	  }
	}

}