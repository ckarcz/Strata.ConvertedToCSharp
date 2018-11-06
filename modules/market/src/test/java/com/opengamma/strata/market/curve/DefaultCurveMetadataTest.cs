/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="CurveMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultCurveMetadataTest
	public class DefaultCurveMetadataTest
	{

	  private const string NAME = "TestCurve";
	  private static readonly CurveName CURVE_NAME = CurveName.of(NAME);
	  private static readonly JacobianCalibrationMatrix JACOBIAN_DATA = JacobianCalibrationMatrix.of(ImmutableList.of(CurveParameterSize.of(CURVE_NAME, 1)), DoubleMatrix.filled(2, 2));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String_noMetadata()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.of(NAME);
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.Info).isEqualTo(ImmutableMap.of());
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_of_CurveName_noMetadata()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.of(CURVE_NAME);
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.Info).isEqualTo(ImmutableMap.of());
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_builder1()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.builder().curveName(CURVE_NAME.ToString()).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(ACT_360).jacobian(JACOBIAN_DATA).addInfo(CurveInfoType.DAY_COUNT, null).parameterMetadata(ImmutableList.of(ParameterMetadata.empty())).build();
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.YEAR_FRACTION);
		assertThat(test.YValueType).isEqualTo(ValueType.DISCOUNT_FACTOR);
		assertThat(test.findInfo(CurveInfoType.DAY_COUNT)).Empty;
		assertThat(test.getInfo(CurveInfoType.JACOBIAN)).isEqualTo(JACOBIAN_DATA);
		assertThat(test.findInfo(CurveInfoType.JACOBIAN)).isEqualTo(JACOBIAN_DATA);
		assertThat(test.findInfo(CurveInfoType.of("Rubbish"))).isEqualTo(null);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  public virtual void test_builder2()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).addInfo(CurveInfoType.DAY_COUNT, ACT_360).jacobian(JACOBIAN_DATA).parameterMetadata(ParameterMetadata.empty()).build();
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.YEAR_FRACTION);
		assertThat(test.YValueType).isEqualTo(ValueType.DISCOUNT_FACTOR);
		assertThat(test.getInfo(CurveInfoType.DAY_COUNT)).isEqualTo(ACT_360);
		assertThat(test.findInfo(CurveInfoType.DAY_COUNT)).isEqualTo(ACT_360);
		assertThat(test.getInfo(CurveInfoType.JACOBIAN)).isEqualTo(JACOBIAN_DATA);
		assertThat(test.findInfo(CurveInfoType.JACOBIAN)).isEqualTo(JACOBIAN_DATA);
		assertThat(test.findInfo(CurveInfoType.of("Rubbish"))).isEqualTo(null);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  public virtual void test_builder3()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.builder().curveName(CURVE_NAME).parameterMetadata(ImmutableList.of(ParameterMetadata.empty())).clearParameterMetadata().build();
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_builder4()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).parameterMetadata(ParameterMetadata.empty()).parameterMetadata(ParameterMetadata.empty()).build();
		assertThat(test.CurveName).isEqualTo(CURVE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.YEAR_FRACTION);
		assertThat(test.YValueType).isEqualTo(ValueType.DISCOUNT_FACTOR);
		assertThrowsIllegalArg(() => test.getInfo(CurveInfoType.DAY_COUNT));
		assertThat(test.findInfo(CurveInfoType.DAY_COUNT)).isEqualTo(null);
		assertThat(test.findInfo(CurveInfoType.JACOBIAN)).isEqualTo(null);
		assertThat(test.findInfo(CurveInfoType.of("Rubbish"))).isEqualTo(null);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withInfo()
	  {
		DefaultCurveMetadata @base = DefaultCurveMetadata.of(CURVE_NAME);
		assertThat(@base.findInfo(CurveInfoType.DAY_COUNT).Present).False;
		DefaultCurveMetadata test = @base.withInfo(CurveInfoType.DAY_COUNT, ACT_360);
		assertThat(@base.findInfo(CurveInfoType.DAY_COUNT).Present).False;
		assertThat(test.findInfo(CurveInfoType.DAY_COUNT).Present).True;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withParameterMetadata()
	  {
		DefaultCurveMetadata @base = DefaultCurveMetadata.of(CURVE_NAME);
		DefaultCurveMetadata test = @base.withParameterMetadata(ParameterMetadata.listOfEmpty(2));
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsAll(ParameterMetadata.listOfEmpty(2));
		// redo for test coverage
		DefaultCurveMetadata test2 = test.withParameterMetadata(ParameterMetadata.listOfEmpty(3));
		assertThat(test2.ParameterMetadata.Present).True;
		assertThat(test2.ParameterMetadata.get()).containsAll(ParameterMetadata.listOfEmpty(3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultCurveMetadata test = DefaultCurveMetadata.of(CURVE_NAME);
		coverImmutableBean(test);
		DefaultCurveMetadata test2 = DefaultCurveMetadata.builder().curveName(CURVE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(ACT_360).jacobian(JACOBIAN_DATA).parameterMetadata(ParameterMetadata.empty()).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CurveMetadata test = DefaultCurveMetadata.of(CURVE_NAME);
		assertSerialization(test);
	  }

	}

}