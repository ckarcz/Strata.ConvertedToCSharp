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
//ORIGINAL LINE: @Test public class RidderSingleRootFinderTest extends RealSingleRootFinderTestCase
	public class RidderSingleRootFinderTest : RealSingleRootFinderTestCase
	{
	  private static readonly RealSingleRootFinder FINDER = new RidderSingleRootFinder();

	  protected internal override RealSingleRootFinder RootFinder
	  {
		  get
		  {
			return FINDER;
		  }
	  }
	}

}