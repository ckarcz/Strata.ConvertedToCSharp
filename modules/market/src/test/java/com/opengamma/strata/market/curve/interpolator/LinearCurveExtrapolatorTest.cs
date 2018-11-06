/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="LinearCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LinearCurveExtrapolatorTest
	public class LinearCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator LINEAR_EXTRAPOLATOR = LinearCurveExtrapolator.INSTANCE;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);
	  private static readonly DoubleArray X_TEST = DoubleArray.of(-1.0, 6.0);
	  private static readonly DoubleArray Y_TEST = DoubleArray.of(-1.1, -5.272727273);

	  public virtual void test_basics()
	  {
		assertEquals(LINEAR_EXTRAPOLATOR.Name, LinearCurveExtrapolator.NAME);
		assertEquals(LINEAR_EXTRAPOLATOR.ToString(), LinearCurveExtrapolator.NAME);
	  }

	  public virtual void test_extrapolation()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.DOUBLE_QUADRATIC.bind(X_DATA, Y_DATA, LINEAR_EXTRAPOLATOR, LINEAR_EXTRAPOLATOR);
		for (int i = 0; i < X_TEST.size(); i++)
		{
		  assertEquals(bci.interpolate(X_TEST.get(i)), Y_TEST.get(i), 1e-6);
		}
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(LINEAR_EXTRAPOLATOR);
	  }

	}

}