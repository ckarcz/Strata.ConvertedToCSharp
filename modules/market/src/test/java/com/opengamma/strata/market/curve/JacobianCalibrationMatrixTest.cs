using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Test <seealso cref="JacobianCalibrationMatrix"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class JacobianCalibrationMatrixTest
	public class JacobianCalibrationMatrixTest
	{

	  private static readonly CurveName NAME1 = CurveName.of("Test1");
	  private static readonly CurveName NAME2 = CurveName.of("Test2");
	  private static readonly CurveName NAME3 = CurveName.of("Test3");
	  private static readonly CurveParameterSize CPS1 = CurveParameterSize.of(NAME1, 3);
	  private static readonly CurveParameterSize CPS2 = CurveParameterSize.of(NAME2, 2);
	  private static readonly IList<CurveParameterSize> CPS = ImmutableList.of(CPS1, CPS2);
	  private static readonly DoubleMatrix MATRIX = DoubleMatrix.of(2, 2, 1d, 2d, 2d, 3d);
	  private static readonly DoubleMatrix MATRIX2 = DoubleMatrix.of(2, 2, 2d, 2d, 3d, 3d);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		JacobianCalibrationMatrix test = JacobianCalibrationMatrix.of(CPS, MATRIX);
		assertEquals(test.Order, CPS);
		assertEquals(test.JacobianMatrix, MATRIX);
		assertEquals(test.CurveCount, 2);
		assertEquals(test.TotalParameterCount, 5);
		assertEquals(test.containsCurve(NAME1), true);
		assertEquals(test.containsCurve(NAME2), true);
		assertEquals(test.containsCurve(NAME3), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_split()
	  {
		JacobianCalibrationMatrix test = JacobianCalibrationMatrix.of(CPS, MATRIX);
		DoubleArray array = DoubleArray.of(1, 2, 3, 4, 5);
		DoubleArray array1 = DoubleArray.of(1, 2, 3);
		DoubleArray array2 = DoubleArray.of(4, 5);
		assertEquals(test.splitValues(array), ImmutableMap.of(NAME1, array1, NAME2, array2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		JacobianCalibrationMatrix test = JacobianCalibrationMatrix.of(CPS, MATRIX);
		coverImmutableBean(test);
		JacobianCalibrationMatrix test2 = JacobianCalibrationMatrix.of(ImmutableList.of(CPS1), MATRIX2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		JacobianCalibrationMatrix test = JacobianCalibrationMatrix.of(CPS, MATRIX);
		assertSerialization(test);
	  }

	}

}