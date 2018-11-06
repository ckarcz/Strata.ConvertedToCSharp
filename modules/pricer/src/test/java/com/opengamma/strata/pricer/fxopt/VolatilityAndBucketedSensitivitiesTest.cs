/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test <seealso cref="VolatilityAndBucketedSensitivities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class VolatilityAndBucketedSensitivitiesTest
	public class VolatilityAndBucketedSensitivitiesTest
	{

	  private const double VOL = 0.34;
	  private static readonly DoubleMatrix SENSITIVITIES = DoubleMatrix.of(2, 3, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6);
	  private static readonly DoubleMatrix SENSITIVITIES2 = DoubleMatrix.of(1, 3, 0.1, 0.2, 0.3);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullSensitivities()
	  public virtual void testNullSensitivities()
	  {
		VolatilityAndBucketedSensitivities.of(VOL, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		VolatilityAndBucketedSensitivities @object = VolatilityAndBucketedSensitivities.of(VOL, SENSITIVITIES);
		assertEquals(VOL, @object.Volatility);
		assertEquals(SENSITIVITIES, @object.Sensitivities);
		VolatilityAndBucketedSensitivities other = VolatilityAndBucketedSensitivities.of(VOL, DoubleMatrix.of(2, 3, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6));
		assertEquals(@object, other);
		assertEquals(@object.GetHashCode(), other.GetHashCode());
		other = VolatilityAndBucketedSensitivities.of(VOL + 0.01, SENSITIVITIES);
		assertFalse(other.Equals(@object));
		other = VolatilityAndBucketedSensitivities.of(VOL, SENSITIVITIES2);
		assertFalse(other.Equals(@object));
	  }

	}

}