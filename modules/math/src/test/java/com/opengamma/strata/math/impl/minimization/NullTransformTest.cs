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
//ORIGINAL LINE: @Test public class NullTransformTest extends ParameterLimitsTransformTestCase
	public class NullTransformTest : ParameterLimitsTransformTestCase
	{
	  private static readonly ParameterLimitsTransform NULL_TRANSFORM = new NullTransform();

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		for (int i = 0; i < 10; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double y = 5 * NORMAL.nextRandom();
		  double y = 5 * NORMAL.nextRandom();
		  assertRoundTrip(NULL_TRANSFORM, y);
		  assertReverseRoundTrip(NULL_TRANSFORM, y);
		  assertGradient(NULL_TRANSFORM, y);
		  assertInverseGradient(NULL_TRANSFORM, y);
		  assertGradientRoundTrip(NULL_TRANSFORM, y);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHashCodeAndEquals()
	  public virtual void testHashCodeAndEquals()
	  {
		ParameterLimitsTransform other = new NullTransform();
		assertEquals(other, NULL_TRANSFORM);
		assertEquals(other.GetHashCode(), NULL_TRANSFORM.GetHashCode());
		other = new ParameterLimitsTransformAnonymousInnerClass(this);
		assertFalse(other.Equals(NULL_TRANSFORM));
	  }

	  private class ParameterLimitsTransformAnonymousInnerClass : ParameterLimitsTransform
	  {
		  private readonly NullTransformTest outerInstance;

		  public ParameterLimitsTransformAnonymousInnerClass(NullTransformTest outerInstance)
		  {
			  this.outerInstance = outerInstance;
		  }


//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double transformGradient(final double x)
		  public double transformGradient(double x)
		  {
			return 0;
		  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double transform(final double x)
		  public double transform(double x)
		  {
			return 0;
		  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double inverseTransformGradient(final double y)
		  public double inverseTransformGradient(double y)
		  {
			return 0;
		  }

//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public double inverseTransform(final double y)
		  public double inverseTransform(double y)
		  {
			return 0;
		  }
	  }
	}

}