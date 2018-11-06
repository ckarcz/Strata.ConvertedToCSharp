using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="ParameterizedDataCombiner"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterizedDataCombinerTest
	public class ParameterizedDataCombinerTest
	{

	  private static readonly TestingParameterizedData2 DATA1 = new TestingParameterizedData2(1d, 2d);
	  private static readonly TestingParameterizedData DATA2 = new TestingParameterizedData(3d);
	  private static readonly TestingParameterizedData2 DATA3 = new TestingParameterizedData2(4d, 5d);

	  //-------------------------------------------------------------------------
	  public virtual void test_basics()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.ParameterCount, 5);
		assertEquals(test.getParameter(0), 1d);
		assertEquals(test.getParameter(1), 2d);
		assertEquals(test.getParameter(2), 3d);
		assertEquals(test.getParameter(3), 4d);
		assertEquals(test.getParameter(4), 5d);
		assertEquals(test.getParameterMetadata(0), ParameterMetadata.empty());
		assertThrows(() => test.getParameter(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.getParameter(5), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.getParameterMetadata(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.getParameterMetadata(5), typeof(System.IndexOutOfRangeException));
		assertThrowsIllegalArg(() => ParameterizedDataCombiner.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_underlyingWithParameter0()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 0, -1d).getParameter(0), -1d);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 0, -1d).getParameter(1), 2d);
		assertEquals(test.underlyingWithParameter(1, typeof(TestingParameterizedData), 0, -1d).getParameter(0), 3d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 0, -1d).getParameter(0), 4d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 0, -1d).getParameter(1), 5d);
	  }

	  public virtual void test_underlyingWithParameter1()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 1, -1d).getParameter(0), 1d);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 1, -1d).getParameter(1), -1d);
		assertEquals(test.underlyingWithParameter(1, typeof(TestingParameterizedData), 1, -1d).getParameter(0), 3d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 1, -1d).getParameter(0), 4d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 1, -1d).getParameter(1), 5d);
	  }

	  public virtual void test_underlyingWithParameter2()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 2, -1d).getParameter(0), 1d);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 2, -1d).getParameter(1), 2d);
		assertEquals(test.underlyingWithParameter(1, typeof(TestingParameterizedData), 2, -1d).getParameter(0), -1d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 2, -1d).getParameter(0), 4d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 2, -1d).getParameter(1), 5d);
	  }

	  public virtual void test_underlyingWithParameter3()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 3, -1d).getParameter(0), 1d);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 3, -1d).getParameter(1), 2d);
		assertEquals(test.underlyingWithParameter(1, typeof(TestingParameterizedData), 3, -1d).getParameter(0), 3d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 3, -1d).getParameter(0), -1d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 3, -1d).getParameter(1), 5d);
	  }

	  public virtual void test_underlyingWithParameter4()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 4, -1d).getParameter(0), 1d);
		assertEquals(test.underlyingWithParameter(0, typeof(TestingParameterizedData2), 4, -1d).getParameter(1), 2d);
		assertEquals(test.underlyingWithParameter(1, typeof(TestingParameterizedData), 4, -1d).getParameter(0), 3d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 4, -1d).getParameter(0), 4d);
		assertEquals(test.underlyingWithParameter(2, typeof(TestingParameterizedData2), 4, -1d).getParameter(1), -1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_underlyingWithPerturbation()
	  {
		ParameterPerturbation perturbation = (i, v, m) => v + i + 0.5d;
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.underlyingWithPerturbation(0, typeof(TestingParameterizedData2), perturbation).getParameter(0), 1.5d);
		assertEquals(test.underlyingWithPerturbation(0, typeof(TestingParameterizedData2), perturbation).getParameter(1), 3.5d);
		assertEquals(test.underlyingWithPerturbation(1, typeof(TestingParameterizedData), perturbation).getParameter(0), 5.5d);
		assertEquals(test.underlyingWithPerturbation(2, typeof(TestingParameterizedData2), perturbation).getParameter(0), 7.5d);
		assertEquals(test.underlyingWithPerturbation(2, typeof(TestingParameterizedData2), perturbation).getParameter(1), 9.5d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withParameter()
	  {
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		assertEquals(test.withParameter(typeof(ParameterizedData), 0, -1d), ImmutableList.of(DATA1.withParameter(0, -1d), DATA2, DATA3));
		assertEquals(test.withParameter(typeof(ParameterizedData), 1, -1d), ImmutableList.of(DATA1.withParameter(1, -1d), DATA2, DATA3));
		assertEquals(test.withParameter(typeof(ParameterizedData), 2, -1d), ImmutableList.of(DATA1, DATA2.withParameter(0, -1d), DATA3));
		assertEquals(test.withParameter(typeof(ParameterizedData), 3, -1d), ImmutableList.of(DATA1, DATA2, DATA3.withParameter(0, -1d)));
		assertEquals(test.withParameter(typeof(ParameterizedData), 4, -1d), ImmutableList.of(DATA1, DATA2, DATA3.withParameter(1, -1d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withPerturbation()
	  {
		ParameterPerturbation perturbation = (i, v, m) => v + i + 0.5d;
		ParameterizedDataCombiner test = ParameterizedDataCombiner.of(DATA1, DATA2, DATA3);
		IList<ParameterizedData> perturbed = test.withPerturbation(typeof(ParameterizedData), perturbation);
		assertEquals(perturbed[0], new TestingParameterizedData2(1.5d, 3.5d));
		assertEquals(perturbed[1], new TestingParameterizedData(5.5d));
		assertEquals(perturbed[2], new TestingParameterizedData2(7.5d, 9.5d));
	  }

	}

}