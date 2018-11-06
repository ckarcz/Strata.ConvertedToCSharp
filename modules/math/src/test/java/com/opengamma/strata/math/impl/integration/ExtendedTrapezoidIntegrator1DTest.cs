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
//ORIGINAL LINE: @Test public class ExtendedTrapezoidIntegrator1DTest extends Integrator1DTestCase
	public class ExtendedTrapezoidIntegrator1DTest : Integrator1DTestCase
	{
	  private static readonly Integrator1D<double, double> INTEGRATOR = new ExtendedTrapezoidIntegrator1D();

	  protected internal override Integrator1D<double, double> Integrator
	  {
		  get
		  {
			return INTEGRATOR;
		  }
	  }

	}

}