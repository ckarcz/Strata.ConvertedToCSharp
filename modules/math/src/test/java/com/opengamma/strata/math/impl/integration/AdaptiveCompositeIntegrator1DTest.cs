using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	/// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class AdaptiveCompositeIntegrator1DTest extends Integrator1DTestCase
	public class AdaptiveCompositeIntegrator1DTest : Integrator1DTestCase
	{
	  private static readonly Integrator1D<double, double> INTEGRATOR = new AdaptiveCompositeIntegrator1D(new SimpsonIntegrator1D());

	  protected internal override Integrator1D<double, double> Integrator
	  {
		  get
		  {
			return INTEGRATOR;
		  }
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void sampleDataTest()
	  public virtual void sampleDataTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> localInt = new AdaptiveCompositeIntegrator1D(new SimpsonIntegrator1D(), 10.0, 1.e-4);
		Integrator1D<double, double> localInt = new AdaptiveCompositeIntegrator1D(new SimpsonIntegrator1D(), 10.0, 1.e-4);
		assertEquals(-0.368924186060527, localInt.integrate(sampleFunc(), 1.1, 3.0), 1.e-6); // answer from quadpack
	  }

	  private System.Func<double, double> sampleFunc()
	  {
		return (final double? x) =>
		{
	return 100.0 * Math.Sin(10.0 / x) / x / x;
		};
	  }

	  /// 
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void equalsHashCodetest()
	  public virtual void equalsHashCodetest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integBase = new SimpsonIntegrator1D();
		Integrator1D<double, double> integBase = new SimpsonIntegrator1D();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ0 = new AdaptiveCompositeIntegrator1D(integBase);
		Integrator1D<double, double> integ0 = new AdaptiveCompositeIntegrator1D(integBase);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ1 = new AdaptiveCompositeIntegrator1D(integBase);
		Integrator1D<double, double> integ1 = new AdaptiveCompositeIntegrator1D(integBase);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ2 = new AdaptiveCompositeIntegrator1D(new RungeKuttaIntegrator1D());
		Integrator1D<double, double> integ2 = new AdaptiveCompositeIntegrator1D(new RungeKuttaIntegrator1D());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ3 = new AdaptiveCompositeIntegrator1D(integBase, 1.0, 1.e-5);
		Integrator1D<double, double> integ3 = new AdaptiveCompositeIntegrator1D(integBase, 1.0, 1.e-5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ4 = new AdaptiveCompositeIntegrator1D(integBase, 2.0, 1.e-5);
		Integrator1D<double, double> integ4 = new AdaptiveCompositeIntegrator1D(integBase, 2.0, 1.e-5);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Integrator1D<double, double> integ5 = new AdaptiveCompositeIntegrator1D(integBase, 1.0, 1.e-6);
		Integrator1D<double, double> integ5 = new AdaptiveCompositeIntegrator1D(integBase, 1.0, 1.e-6);

		assertTrue(integ0.Equals(integ0));

		assertTrue(integ0.Equals(integ1));
		assertTrue(integ1.Equals(integ0));
		assertTrue(integ1.GetHashCode() == integ0.GetHashCode());

		assertTrue(!(integ0.Equals(integ2)));
		assertTrue(!(integ0.Equals(integ3)));
		assertTrue(!(integ0.Equals(integ4)));
		assertTrue(!(integ0.Equals(integ5)));
		assertTrue(!(integ0.Equals(integBase)));
		assertTrue(!(integ0.Equals(null)));
		assertTrue(!(integ3.Equals(integ5)));

		assertTrue(!(integ1.GetHashCode() == INTEGRATOR.GetHashCode()));
	  }
	}

}