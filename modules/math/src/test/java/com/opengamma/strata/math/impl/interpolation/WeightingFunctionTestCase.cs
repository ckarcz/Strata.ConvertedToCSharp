/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class WeightingFunctionTestCase
	public abstract class WeightingFunctionTestCase
	{

	  internal static readonly double[] STRIKES = new double[] {1, 1.1, 1.2, 1.3, 1.4, 1.5};
	  internal const double STRIKE = 1.345;
	  internal const int INDEX = 3;
	  internal const double EPS = 1e-15;

	  protected internal abstract WeightingFunction Instance {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullStrikes2()
	  public virtual void testNullStrikes2()
	  {
		Instance.getWeight(null, INDEX, STRIKE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNegativeIndex()
	  public virtual void testNegativeIndex()
	  {
		Instance.getWeight(STRIKES, -INDEX, STRIKE);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testHighIndex()
	  public virtual void testHighIndex()
	  {
		Instance.getWeight(STRIKES, STRIKES.Length, STRIKE);
	  }

	}

}