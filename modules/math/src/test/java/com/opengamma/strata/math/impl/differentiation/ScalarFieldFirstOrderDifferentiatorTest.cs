using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.differentiation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ScalarFieldFirstOrderDifferentiatorTest
	public class ScalarFieldFirstOrderDifferentiatorTest
	{

	  private static readonly System.Func<DoubleArray, double> F = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
  return x1 * x1 + 2 * x2 * x2 - x1 * x2 + x1 * Math.Cos(x2) - x2 * Math.Sin(x1);
	  };

	  private static readonly System.Func<DoubleArray, bool> DOMAIN = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  return x1 >= 0.0 && x1 <= Math.PI;
	  };

	  private static readonly System.Func<DoubleArray, DoubleArray> G = (final DoubleArray x) =>
	  {

  double x1 = x.get(0);
  double x2 = x.get(1);
  return DoubleArray.of(2 * x1 - x2 + Math.Cos(x2) - x2 * Math.Cos(x1), 4 * x2 - x1 - x1 * Math.Sin(x2) - Math.Sin(x1));

	  };
	  private const double EPS = 1e-4;
	  private static readonly ScalarFieldFirstOrderDifferentiator FORWARD = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.FORWARD, EPS);
	  private static readonly ScalarFieldFirstOrderDifferentiator CENTRAL = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.CENTRAL, EPS);
	  private static readonly ScalarFieldFirstOrderDifferentiator BACKWARD = new ScalarFieldFirstOrderDifferentiator(FiniteDifferenceType.BACKWARD, EPS);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDifferenceType()
	  public virtual void testNullDifferenceType()
	  {
		new ScalarFirstOrderDifferentiator(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullFunction()
	  public virtual void testNullFunction()
	  {
		CENTRAL.differentiate((System.Func<DoubleArray, double>) null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray x = com.opengamma.strata.collect.array.DoubleArray.of(.2245, -1.2344);
		DoubleArray x = DoubleArray.of(.2245, -1.2344);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray anGrad = G.apply(x);
		DoubleArray anGrad = G.apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray fdFwdGrad = FORWARD.differentiate(F).apply(x);
		DoubleArray fdFwdGrad = FORWARD.differentiate(F).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray fdCentGrad = CENTRAL.differentiate(F).apply(x);
		DoubleArray fdCentGrad = CENTRAL.differentiate(F).apply(x);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray fdBackGrad = BACKWARD.differentiate(F).apply(x);
		DoubleArray fdBackGrad = BACKWARD.differentiate(F).apply(x);

		for (int i = 0; i < 2; i++)
		{
		  assertEquals(fdFwdGrad.get(i), anGrad.get(i), 10 * EPS);
		  assertEquals(fdCentGrad.get(i), anGrad.get(i), EPS * EPS);
		  assertEquals(fdBackGrad.get(i), anGrad.get(i), 10 * EPS);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void domainTest()
	  public virtual void domainTest()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] x = new com.opengamma.strata.collect.array.DoubleArray[3];
		DoubleArray[] x = new DoubleArray[3];
		x[0] = DoubleArray.of(0.2245, -1.2344);
		x[1] = DoubleArray.of(0.0, 12.6);
		x[2] = DoubleArray.of(Math.PI, 0.0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.function.Function<com.opengamma.strata.collect.array.DoubleArray, com.opengamma.strata.collect.array.DoubleArray> fdGradFunc = CENTRAL.differentiate(F, DOMAIN);
		System.Func<DoubleArray, DoubleArray> fdGradFunc = CENTRAL.differentiate(F, DOMAIN);

		for (int k = 0; k < 3; k++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray fdRes = fdGradFunc.apply(x[k]);
		  DoubleArray fdRes = fdGradFunc(x[k]);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray alRes = G.apply(x[k]);
		  DoubleArray alRes = G.apply(x[k]);
		  for (int i = 0; i < 2; i++)
		  {
			assertEquals(fdRes.get(i), alRes.get(i), 1e-7);
		  }
		}
	  }

	}

}