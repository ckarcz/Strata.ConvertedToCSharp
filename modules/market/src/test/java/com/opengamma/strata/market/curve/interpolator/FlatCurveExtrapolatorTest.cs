using System;

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
	/// Test <seealso cref="FlatCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FlatCurveExtrapolatorTest
	public class FlatCurveExtrapolatorTest
	{

	  private static readonly Random RANDOM = new Random(0L);
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = FlatCurveExtrapolator.INSTANCE;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);

	  public virtual void test_basics()
	  {
		assertEquals(FLAT_EXTRAPOLATOR.Name, FlatCurveExtrapolator.NAME);
		assertEquals(FLAT_EXTRAPOLATOR.ToString(), FlatCurveExtrapolator.NAME);
	  }

	  public virtual void test_extrapolation()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);

		for (int i = 0; i < 100; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = RANDOM.nextDouble() * 20.0 - 10;
		  double x = RANDOM.NextDouble() * 20.0 - 10;
		  if (x < 0)
		  {
			assertEquals(bci.interpolate(x), 3.0, 1e-12);
			assertEquals(bci.firstDerivative(x), 0.0, 1e-12);
			assertEquals(bci.parameterSensitivity(x).get(0), 1.0, 1e-12);
		  }
		  else if (x > 5.0)
		  {
			assertEquals(bci.interpolate(x), 2.0, 1e-12);
			assertEquals(bci.firstDerivative(x), 0.0, 1e-12);
			assertEquals(bci.parameterSensitivity(x).get(X_DATA.size() - 1), 1.0, 1e-12);
		  }
		}
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(FLAT_EXTRAPOLATOR);
	  }

	}

}