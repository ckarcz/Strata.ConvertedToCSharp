/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Abstract test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public abstract class MinimumBracketerTestCase
	public abstract class MinimumBracketerTestCase
	{
	  private static readonly System.Func<double, double> F = (final double? x) =>
	  {

  return null;

	  };

	  protected internal abstract MinimumBracketer Bracketer {get;}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		Bracketer.checkInputs(null, 1.0, 2.0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInputs()
	  public virtual void testInputs()
	  {
		Bracketer.checkInputs(F, 1.0, 1.0);
	  }
	}

}