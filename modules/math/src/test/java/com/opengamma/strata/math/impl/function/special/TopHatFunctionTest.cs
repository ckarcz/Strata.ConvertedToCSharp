/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TopHatFunctionTest
	public class TopHatFunctionTest
	{
	  private const double X1 = 2;
	  private const double X2 = 2.5;
	  private const double Y = 10;
	  private static readonly System.Func<double, double> F = new TopHatFunction(X1, X2, Y);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongOrder()
	  public virtual void testWrongOrder()
	  {
		new TopHatFunction(X2, X1, Y);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNull()
	  public virtual void testNull()
	  {
		F.apply((double?) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testX1()
	  public virtual void testX1()
	  {
		F.apply(X1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testX2()
	  public virtual void testX2()
	  {
		F.apply(X2);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(F.apply(X1 - 1e-15), 0, 0);
		assertEquals(F.apply(X2 + 1e-15), 0, 0);
		assertEquals(F.apply((X1 + X2) / 2), Y, 0);
	  }
	}

}