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
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using LabelParameterMetadata = com.opengamma.strata.market.param.LabelParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParallelShiftedCurveTest
	public class ParallelShiftedCurveTest
	{

	  private static readonly CurveMetadata METADATA = DefaultCurveMetadata.of("Test");
	  private static readonly Curve CONSTANT_CURVE = ConstantCurve.of(METADATA, 3d);
	  private static readonly Curve CONSTANT_CURVE2 = ConstantCurve.of(METADATA, 5d);

	  //-------------------------------------------------------------------------
	  public virtual void absolute()
	  {
		ParallelShiftedCurve test = ParallelShiftedCurve.absolute(CONSTANT_CURVE, 1d);
		assertThat(test.UnderlyingCurve).isEqualTo(CONSTANT_CURVE);
		assertThat(test.ShiftType).isEqualTo(ShiftType.ABSOLUTE);
		assertThat(test.ShiftAmount).isEqualTo(1d);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.Name).isEqualTo(METADATA.CurveName);
		assertThat(test.ParameterCount).isEqualTo(2);
		assertThat(test.getParameter(0)).isEqualTo(3d);
		assertThat(test.getParameter(1)).isEqualTo(1d);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.getParameterMetadata(1)).isEqualTo(LabelParameterMetadata.of("AbsoluteShift"));
		assertThat(test.withParameter(0, 5d)).isEqualTo(ParallelShiftedCurve.absolute(CONSTANT_CURVE2, 1d));
		assertThat(test.withParameter(1, 0.5d)).isEqualTo(ParallelShiftedCurve.absolute(CONSTANT_CURVE, 0.5d));
		assertThat(test.withPerturbation((i, v, m) => v + 2d)).isEqualTo(ParallelShiftedCurve.absolute(CONSTANT_CURVE2, 3d));
		assertThat(test.yValue(0)).isEqualTo(4d);
		assertThat(test.yValue(1)).isEqualTo(4d);
	  }

	  public virtual void relative()
	  {
		ParallelShiftedCurve test = ParallelShiftedCurve.relative(CONSTANT_CURVE, 0.1d);
		assertThat(test.UnderlyingCurve).isEqualTo(CONSTANT_CURVE);
		assertThat(test.ShiftType).isEqualTo(ShiftType.RELATIVE);
		assertThat(test.ShiftAmount).isEqualTo(0.1d);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.Name).isEqualTo(METADATA.CurveName);
		assertThat(test.ParameterCount).isEqualTo(2);
		assertThat(test.getParameter(0)).isEqualTo(3d);
		assertThat(test.getParameter(1)).isEqualTo(0.1d);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.getParameterMetadata(1)).isEqualTo(LabelParameterMetadata.of("RelativeShift"));
		assertThat(test.withParameter(0, 5d)).isEqualTo(ParallelShiftedCurve.relative(CONSTANT_CURVE2, 0.1d));
		assertThat(test.withParameter(1, 0.5d)).isEqualTo(ParallelShiftedCurve.relative(CONSTANT_CURVE, 0.5d));
		assertThat(test.withPerturbation((i, v, m) => v + 2d)).isEqualTo(ParallelShiftedCurve.relative(CONSTANT_CURVE2, 2.1d));
		assertThat(test.yValue(0)).isEqualTo(3.3d, offset(1e-10d));
		assertThat(test.yValue(1)).isEqualTo(3.3d, offset(1e-10d));
	  }

	  public virtual void test_of()
	  {
		Curve test = ParallelShiftedCurve.of(CONSTANT_CURVE, ShiftType.RELATIVE, 0.1d);
		assertThat(test.yValue(0)).isEqualTo(3.3d, offset(1e-10));
		assertThat(test.yValue(1)).isEqualTo(3.3d, offset(1e-10));
		assertThat(test.Name).isEqualTo(METADATA.CurveName);
		assertThat(test.ParameterCount).isEqualTo(2);
		assertThat(test.getParameter(0)).isEqualTo(3d);
		assertThat(test.getParameter(1)).isEqualTo(0.1d);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.getParameterMetadata(1)).isEqualTo(LabelParameterMetadata.of("RelativeShift"));
		assertThat(test.Metadata).isEqualTo(METADATA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_yValueParameterSensitivity()
	  {
		InterpolatedNodalCurve curve = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 1), DoubleArray.of(2, 2.5), CurveInterpolators.LINEAR);

		Curve absoluteShiftedCurve = ParallelShiftedCurve.absolute(curve, 1);
		Curve relativeShiftedCurve = ParallelShiftedCurve.relative(curve, 0.2);

		UnitParameterSensitivity expected = curve.yValueParameterSensitivity(0.1);
		assertThat(absoluteShiftedCurve.yValueParameterSensitivity(0.1)).isEqualTo(expected);
		assertThat(relativeShiftedCurve.yValueParameterSensitivity(0.1)).isEqualTo(expected);
	  }

	  public virtual void test_firstDerivative()
	  {
		InterpolatedNodalCurve curve = InterpolatedNodalCurve.of(METADATA, DoubleArray.of(0, 1), DoubleArray.of(2, 2.5), CurveInterpolators.LINEAR);

		Curve absoluteShiftedCurve = ParallelShiftedCurve.absolute(curve, 1);
		Curve relativeShiftedCurve = ParallelShiftedCurve.relative(curve, 0.2);

		assertThat(curve.firstDerivative(0.1)).isEqualTo(0.5);
		assertThat(absoluteShiftedCurve.firstDerivative(0.1)).isEqualTo(0.5);
		assertThat(relativeShiftedCurve.firstDerivative(0.1)).isEqualTo(0.5 * 1.2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ParallelShiftedCurve test = ParallelShiftedCurve.absolute(CONSTANT_CURVE, 1);
		coverImmutableBean(test);
		ParallelShiftedCurve test2 = ParallelShiftedCurve.relative(CONSTANT_CURVE2, 0.2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ParallelShiftedCurve test = ParallelShiftedCurve.absolute(CONSTANT_CURVE, 1);
		assertSerialization(test);
	  }

	}

}