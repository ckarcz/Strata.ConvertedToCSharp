/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding
{
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BisectionSingleRootFinderTest extends RealSingleRootFinderTestCase
	public class BisectionSingleRootFinderTest : RealSingleRootFinderTestCase
	{
	  private static readonly RealSingleRootFinder FINDER = new BisectionSingleRootFinder();

	  protected internal override RealSingleRootFinder RootFinder
	  {
		  get
		  {
			return FINDER;
		  }
	  }

	}

}