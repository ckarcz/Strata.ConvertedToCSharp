/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="DoublesScheduleGenerator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoublesScheduleGeneratorTest
	public class DoublesScheduleGeneratorTest
	{

	  public virtual void getIntegrationsPointsTest()
	  {
		double start = 0.1;
		double end = 2.0;

		DoubleArray setA0 = DoubleArray.of(0.5, 0.9, 1.4);
		DoubleArray setB0 = DoubleArray.of(0.3, 0.4, 1.5, 1.6);
		DoubleArray exp0 = DoubleArray.of(0.1, 0.3, 0.4, 0.5, 0.9, 1.4, 1.5, 1.6, 2.0);
		DoubleArray res0 = DoublesScheduleGenerator.getIntegrationsPoints(start, end, setA0, setB0);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp0.toArray(), res0.toArray(), 0d));

		DoubleArray setA1 = DoubleArray.of(0.5, 0.9, 1.4);
		DoubleArray setB1 = DoubleArray.of(0.3, 0.4, 0.5, 1.5, 1.6);
		DoubleArray exp1 = DoubleArray.of(0.1, 0.3, 0.4, 0.5, 0.9, 1.4, 1.5, 1.6, 2.0);
		DoubleArray res1 = DoublesScheduleGenerator.getIntegrationsPoints(start, end, setA1, setB1);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp1.toArray(), res1.toArray(), 0d));

		/*
		 * different(temp[pos], end) == false
		 */
		DoubleArray setA2 = DoubleArray.of(0.2, 0.9, 1.4);
		DoubleArray setB2 = DoubleArray.of(0.3, 0.4, 0.5, 1.5, 2.0 - 1.e-3);
		DoubleArray exp2 = DoubleArray.of(0.1, 0.2, 0.3, 0.4, 0.5, 0.9, 1.4, 1.5, 2.0);
		DoubleArray res2 = DoublesScheduleGenerator.getIntegrationsPoints(start, end, setA2, setB2);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp2.toArray(), res2.toArray(), 0d));

		DoubleArray setA3 = DoubleArray.of(0.05, 0.07);
		DoubleArray setB3 = DoubleArray.of(0.03, 0.04);
		DoubleArray exp3 = DoubleArray.of(0.1, 2.0);
		DoubleArray res3 = DoublesScheduleGenerator.getIntegrationsPoints(start, end, setA3, setB3);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp3.toArray(), res3.toArray(), 0d));

		DoubleArray setA4 = DoubleArray.of(2.2, 2.7);
		DoubleArray setB4 = DoubleArray.of(2.3, 2.4);
		DoubleArray exp4 = DoubleArray.of(0.1, 2.0);
		DoubleArray res4 = DoublesScheduleGenerator.getIntegrationsPoints(start, end, setA4, setB4);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp4.toArray(), res4.toArray(), 0d));

		DoubleArray setA5 = DoubleArray.of(-0.5, 0.0, 1.2);
		DoubleArray setB5 = DoubleArray.of(-0.2, -0.0, 1.2);
		DoubleArray exp5 = DoubleArray.of(-0.3, -0.2, 0.0, 1.2, 2.0);
		DoubleArray res5 = DoublesScheduleGenerator.getIntegrationsPoints(-0.3, end, setA5, setB5);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp5.toArray(), res5.toArray(), 0d));
	  }

	  public virtual void truncateSetInclusiveTest()
	  {
		double lower = 0.1;
		double upper = 2.5;

		DoubleArray set0 = DoubleArray.of(-0.2, 1.5, 2.9);
		DoubleArray exp0 = DoubleArray.of(0.1, 1.5, 2.5);
		DoubleArray res0 = DoublesScheduleGenerator.truncateSetInclusive(lower, upper, set0);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp0.toArray(), res0.toArray(), 0d));

		DoubleArray set1 = DoubleArray.of(-0.2, -0.1);
		DoubleArray exp1 = DoubleArray.of(0.1, 2.5);
		DoubleArray res1 = DoublesScheduleGenerator.truncateSetInclusive(lower, upper, set1);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp1.toArray(), res1.toArray(), 0d));

		DoubleArray set2 = DoubleArray.of(0.1 + 1.e-3, 1.5, 2.8);
		DoubleArray exp2 = DoubleArray.of(0.1, 1.5, 2.5);
		DoubleArray res2 = DoublesScheduleGenerator.truncateSetInclusive(lower, upper, set2);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp2.toArray(), res2.toArray(), 0d));

		DoubleArray set3 = DoubleArray.of(0.0, 1.5, 2.5 + 1.e-3);
		DoubleArray exp3 = DoubleArray.of(0.1, 1.5, 2.5);
		DoubleArray res3 = DoublesScheduleGenerator.truncateSetInclusive(lower, upper, set3);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp3.toArray(), res3.toArray(), 0d));

		DoubleArray set4 = DoubleArray.of(0.1 - 1.e-4, 1.5, 2.5 - 1.e-4);
		DoubleArray exp4 = DoubleArray.of(0.1, 1.5, 2.5);
		DoubleArray res4 = DoublesScheduleGenerator.truncateSetInclusive(lower, upper, set4);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp4.toArray(), res4.toArray(), 0d));

		DoubleArray set5 = DoubleArray.of(lower + 1.e-4, lower + 2.e-4);
		DoubleArray exp5 = DoubleArray.of(lower, lower + 1.e-3);
		DoubleArray res5 = DoublesScheduleGenerator.truncateSetInclusive(lower, lower + 1.e-3, set5);
		assertTrue(DoubleArrayMath.fuzzyEquals(exp5.toArray(), res5.toArray(), 0d));
	  }
	}

}