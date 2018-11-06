/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class Integrator1DTest
	public class Integrator1DTest
	{
	  private static readonly Integrator1D<double, double> INTEGRATOR = new Integrator1DAnonymousInnerClass();

	  private class Integrator1DAnonymousInnerClass : Integrator1D<double, double>
	  {
		  public Integrator1DAnonymousInnerClass()
		  {
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: @Override public System.Nullable<double> integrate(final System.Func<double, double> f, final System.Nullable<double> lower, final System.Nullable<double> upper)
		  public override double? integrate(System.Func<double, double> f, double? lower, double? upper)
		  {
			return 0.0;
		  }

	  }
	  private static readonly System.Func<double, double> F = (final double? x) =>
	  {

  return 0.0;

	  };
	  private static readonly double?[] L = new double?[] {1.3};
	  private static readonly double?[] U = new double?[] {3.4};

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		INTEGRATOR.integrate(null, L, U);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLowerBound()
	  public virtual void testNullLowerBound()
	  {
		INTEGRATOR.integrate(F, null, U);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullUpperBound()
	  public virtual void testNullUpperBound()
	  {
		INTEGRATOR.integrate(F, L, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyLowerBound()
	  public virtual void testEmptyLowerBound()
	  {
		INTEGRATOR.integrate(F, new double?[0], U);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyUpperBound()
	  public virtual void testEmptyUpperBound()
	  {
		INTEGRATOR.integrate(F, L, new double?[0]);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullLowerBoundValue()
	  public virtual void testNullLowerBoundValue()
	  {
		INTEGRATOR.integrate(F, new double?[] {null}, U);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullUpperBoundValue()
	  public virtual void testNullUpperBoundValue()
	  {
		INTEGRATOR.integrate(F, L, new double?[] {null});
	  }
	}

}