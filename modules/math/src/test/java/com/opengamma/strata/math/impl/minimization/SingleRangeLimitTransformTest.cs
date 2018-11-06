using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SingleRangeLimitTransformTest extends ParameterLimitsTransformTestCase
	public class SingleRangeLimitTransformTest : ParameterLimitsTransformTestCase
	{
	  private const double A = -2.5;
	  private const double B = 1.0;
	  private static readonly ParameterLimitsTransform LOWER_LIMIT = new SingleRangeLimitTransform(B, ParameterLimitsTransform_LimitType.GREATER_THAN);
	  private static readonly ParameterLimitsTransform UPPER_LIMIT = new SingleRangeLimitTransform(A, ParameterLimitsTransform_LimitType.LESS_THAN);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange1()
	  public virtual void testOutOfRange1()
	  {
		LOWER_LIMIT.transform(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange2()
	  public virtual void testOutOfRange2()
	  {
		UPPER_LIMIT.transform(1.01);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange3()
	  public virtual void testOutOfRange3()
	  {
		LOWER_LIMIT.transformGradient(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange4()
	  public virtual void testOutOfRange4()
	  {
		UPPER_LIMIT.transformGradient(1.01);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLower()
	  public virtual void testLower()
	  {
		for (int i = 0; i < 10; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = B - 5 * Math.log(RANDOM.nextDouble());
		  double x = B - 5 * Math.Log(RANDOM.NextDouble());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = 5 * NORMAL.nextRandom();
		  double y = 5 * NORMAL.nextRandom();
		  assertRoundTrip(LOWER_LIMIT, x);
		  assertReverseRoundTrip(LOWER_LIMIT, y);
		  assertGradient(LOWER_LIMIT, x);
		  assertInverseGradient(LOWER_LIMIT, y);
		  assertGradientRoundTrip(LOWER_LIMIT, x);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testUpper()
	  public virtual void testUpper()
	  {
		for (int i = 0; i < 10; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = A + 5 * Math.log(RANDOM.nextDouble());
		  double x = A + 5 * Math.Log(RANDOM.NextDouble());
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = 5 * NORMAL.nextRandom();
		  double y = 5 * NORMAL.nextRandom();
		  assertRoundTrip(UPPER_LIMIT, x);
		  assertReverseRoundTrip(UPPER_LIMIT, y);
		  assertGradient(UPPER_LIMIT, x);
		  assertInverseGradient(UPPER_LIMIT, y);
		  assertGradientRoundTrip(UPPER_LIMIT, x);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCodeAndEquals()
	  public virtual void testHashCodeAndEquals()
	  {
		ParameterLimitsTransform other = new SingleRangeLimitTransform(B, ParameterLimitsTransform_LimitType.GREATER_THAN);
		assertEquals(other, LOWER_LIMIT);
		assertEquals(other.GetHashCode(), LOWER_LIMIT.GetHashCode());
		other = new SingleRangeLimitTransform(A, ParameterLimitsTransform_LimitType.GREATER_THAN);
		assertFalse(other.Equals(LOWER_LIMIT));
		other = new SingleRangeLimitTransform(B, ParameterLimitsTransform_LimitType.LESS_THAN);
		assertFalse(other.Equals(LOWER_LIMIT));
	  }
	}

}