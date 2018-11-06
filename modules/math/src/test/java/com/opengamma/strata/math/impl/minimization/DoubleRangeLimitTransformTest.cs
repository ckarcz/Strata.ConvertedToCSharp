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
//ORIGINAL LINE: @Test public class DoubleRangeLimitTransformTest extends ParameterLimitsTransformTestCase
	public class DoubleRangeLimitTransformTest : ParameterLimitsTransformTestCase
	{
	  private const double A = -2.5;
	  private const double B = 1.0;
	  private static readonly ParameterLimitsTransform RANGE_LIMITS = new DoubleRangeLimitTransform(A, B);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange1()
	  public virtual void testOutOfRange1()
	  {
		RANGE_LIMITS.transform(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange2()
	  public virtual void testOutOfRange2()
	  {
		RANGE_LIMITS.transform(1.01);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange3()
	  public virtual void testOutOfRange3()
	  {
		RANGE_LIMITS.transformGradient(-3);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfRange4()
	  public virtual void testOutOfRange4()
	  {
		RANGE_LIMITS.transformGradient(1.01);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		for (int i = 0; i < 10; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = A + (B - A) * RANDOM.nextDouble();
		  double x = A + (B - A) * RANDOM.NextDouble();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = 5 * NORMAL.nextRandom();
		  double y = 5 * NORMAL.nextRandom();
		  assertRoundTrip(RANGE_LIMITS, x);
		  assertReverseRoundTrip(RANGE_LIMITS, y);

		  assertGradient(RANGE_LIMITS, x);
		  assertInverseGradient(RANGE_LIMITS, y);
		  assertGradientRoundTrip(RANGE_LIMITS, x);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCodeAndEquals()
	  public virtual void testHashCodeAndEquals()
	  {
		ParameterLimitsTransform other = new DoubleRangeLimitTransform(A, B);
		assertEquals(other, RANGE_LIMITS);
		assertEquals(other.GetHashCode(), RANGE_LIMITS.GetHashCode());
		other = new DoubleRangeLimitTransform(A - 1, B);
		assertFalse(other.Equals(RANGE_LIMITS));
		other = new DoubleRangeLimitTransform(A, B + 1);
		assertFalse(other.Equals(RANGE_LIMITS));
	  }
	}

}