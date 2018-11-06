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

	using Test = org.testng.annotations.Test;

	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="ConstantCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConstantCurveTest
	public class ConstantCurveTest
	{

	  private const string NAME = "TestCurve";
	  private static readonly CurveName CURVE_NAME = CurveName.of(NAME);
	  private static readonly CurveMetadata METADATA = DefaultCurveMetadata.of(CURVE_NAME);
	  private static readonly CurveMetadata METADATA2 = DefaultCurveMetadata.of("Test2");
	  private const double VALUE = 6d;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		ConstantCurve test = ConstantCurve.of(NAME, VALUE);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.YValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantCurve.of(NAME, 2d));
		assertThat(test.withPerturbation((i, v, m) => v + 1d)).isEqualTo(ConstantCurve.of(NAME, VALUE + 1d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantCurve.of(METADATA2, VALUE));
	  }

	  public virtual void test_of_CurveName()
	  {
		ConstantCurve test = ConstantCurve.of(CURVE_NAME, VALUE);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.YValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantCurve.of(NAME, 2d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantCurve.of(METADATA2, VALUE));
	  }

	  public virtual void test_of_CurveMetadata()
	  {
		ConstantCurve test = ConstantCurve.of(METADATA, VALUE);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.YValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantCurve.of(NAME, 2d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantCurve.of(METADATA2, VALUE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_lookup()
	  {
		ConstantCurve test = ConstantCurve.of(CURVE_NAME, VALUE);
		assertThat(test.yValue(0d)).isEqualTo(VALUE);
		assertThat(test.yValue(-10d)).isEqualTo(VALUE);
		assertThat(test.yValue(100d)).isEqualTo(VALUE);

		assertThat(test.yValueParameterSensitivity(0d).Sensitivity.toArray()).containsExactly(1d);
		assertThat(test.yValueParameterSensitivity(-10d).Sensitivity.toArray()).containsExactly(1d);
		assertThat(test.yValueParameterSensitivity(100d).Sensitivity.toArray()).containsExactly(1d);

		assertThat(test.firstDerivative(0d)).isEqualTo(0d);
		assertThat(test.firstDerivative(-10d)).isEqualTo(0d);
		assertThat(test.firstDerivative(100d)).isEqualTo(0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ConstantCurve test = ConstantCurve.of(CURVE_NAME, VALUE);
		coverImmutableBean(test);
		ConstantCurve test2 = ConstantCurve.of("Coverage", 9d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ConstantCurve test = ConstantCurve.of(CURVE_NAME, VALUE);
		assertSerialization(test);
	  }

	}

}