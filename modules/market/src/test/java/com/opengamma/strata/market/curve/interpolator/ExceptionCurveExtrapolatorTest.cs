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
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="ExceptionCurveExtrapolator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExceptionCurveExtrapolatorTest
	public class ExceptionCurveExtrapolatorTest
	{

	  private static readonly CurveExtrapolator EXCEPTION_EXTRAPOLATOR = ExceptionCurveExtrapolator.INSTANCE;

	  private static readonly DoubleArray X_DATA = DoubleArray.of(0.0, 0.4, 1.0, 1.8, 2.8, 5.0);
	  private static readonly DoubleArray Y_DATA = DoubleArray.of(3.0, 4.0, 3.1, 2.0, 7.0, 2.0);

	  public virtual void test_basics()
	  {
		assertEquals(EXCEPTION_EXTRAPOLATOR.Name, ExceptionCurveExtrapolator.NAME);
		assertEquals(EXCEPTION_EXTRAPOLATOR.ToString(), ExceptionCurveExtrapolator.NAME);
	  }

	  public virtual void test_exceptionThrown()
	  {
		BoundCurveInterpolator bci = CurveInterpolators.LINEAR.bind(X_DATA, Y_DATA, EXCEPTION_EXTRAPOLATOR, EXCEPTION_EXTRAPOLATOR);
		assertThrows(() => bci.interpolate(-1d), typeof(System.NotSupportedException));
		assertThrows(() => bci.firstDerivative(-1d), typeof(System.NotSupportedException));
		assertThrows(() => bci.parameterSensitivity(-1d), typeof(System.NotSupportedException));
		assertThrows(() => bci.interpolate(10d), typeof(System.NotSupportedException));
		assertThrows(() => bci.firstDerivative(10d), typeof(System.NotSupportedException));
		assertThrows(() => bci.parameterSensitivity(10d), typeof(System.NotSupportedException));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(EXCEPTION_EXTRAPOLATOR);
	  }

	}

}