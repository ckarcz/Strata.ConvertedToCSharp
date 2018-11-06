/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GoldenSectionMinimizer1DTest extends Minimizer1DTestCase
	public class GoldenSectionMinimizer1DTest : Minimizer1DTestCase
	{
	  private static readonly ScalarMinimizer MINIMIZER = new GoldenSectionMinimizer1D();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		base.assertInputs(MINIMIZER);
		base.assertMinimizer(MINIMIZER);
	  }
	}

}