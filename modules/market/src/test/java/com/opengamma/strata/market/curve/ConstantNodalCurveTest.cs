/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
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

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="ConstantNodalCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConstantNodalCurveTest
	public class ConstantNodalCurveTest
	{

	  private const int SIZE = 1;
	  private const string NAME = "TestCurve";
	  private static readonly CurveName CURVE_NAME = CurveName.of(NAME);
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(CURVE_NAME, ACT_365F);
	  private static readonly CurveMetadata METADATA_ENTRIES = Curves.zeroRates(CURVE_NAME, ACT_365F, ParameterMetadata.listOfEmpty(SIZE));
	  private static readonly CurveMetadata METADATA_ENTRIES2 = Curves.zeroRates(CURVE_NAME, ACT_365F, ParameterMetadata.listOfEmpty(SIZE + 2));
	  private static readonly CurveMetadata METADATA_NOPARAM = Curves.zeroRates(CURVE_NAME, ACT_365F);
	  private const double XVALUE = 2d;
	  private static readonly DoubleArray XVALUE_ARRAY = DoubleArray.of(XVALUE);
	  private const double YVALUE = 7d;
	  private static readonly DoubleArray YVALUE_ARRAY = DoubleArray.of(YVALUE);
	  private static readonly DoubleArray XVALUE_ARRAY_NEW = DoubleArray.of(3d);
	  private const double YVALUE_BUMPED = 5d;
	  private static readonly DoubleArray YVALUE_BUMPED_ARRAY = DoubleArray.of(YVALUE_BUMPED);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurveMetadata()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, YVALUE);
		ConstantNodalCurve testRe = ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, YVALUE);
		assertThat(test).isEqualTo(testRe);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.getParameter(0)).isEqualTo(YVALUE);
		assertThrowsIllegalArg(() => test.getParameter(1));
		assertThat(test.getParameterMetadata(0)).isSameAs(METADATA_ENTRIES.ParameterMetadata.get().get(0));
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, 2d));
		assertThrowsIllegalArg(() => test.withParameter(1, 2d));
		assertThat(test.withPerturbation((i, v, m) => v - 2d)).isEqualTo(ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, YVALUE_BUMPED));
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUE_ARRAY);
		assertThat(test.YValues).isEqualTo(YVALUE_ARRAY);
	  }

	  public virtual void test_of_noCurveMetadata()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA_NOPARAM, XVALUE, YVALUE);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.getParameter(0)).isEqualTo(YVALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, XVALUE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withNode()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, YVALUE);
		SimpleCurveParameterMetadata param = SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, XVALUE);
		ConstantNodalCurve test = @base.withNode(XVALUE, 2d, param);
		assertThat(test.XValue).isEqualTo(XVALUE);
		assertThat(test.YValue).isEqualTo(2d);
		assertThat(test.getParameterMetadata(0)).isEqualTo(param);
	  }

	  public virtual void test_withNode_invalid()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA_ENTRIES, XVALUE, YVALUE);
		assertThrowsIllegalArg(() => test.withNode(1, 2, ParameterMetadata.empty()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_values()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		assertThat(test.yValue(10.2421)).isEqualTo(YVALUE);
		assertThat(test.yValueParameterSensitivity(10.2421).MarketDataName).isEqualTo(CURVE_NAME);
		assertThat(test.yValueParameterSensitivity(10.2421).Sensitivity).isEqualTo(DoubleArray.of(1d));
		assertThat(test.firstDerivative(10.2421)).isEqualTo(0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withMetadata()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		ConstantNodalCurve test = @base.withMetadata(METADATA_ENTRIES);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUE_ARRAY);
		assertThat(test.YValues).isEqualTo(YVALUE_ARRAY);
	  }

	  public virtual void test_withMetadata_badSize()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		assertThrowsIllegalArg(() => @base.withMetadata(METADATA_ENTRIES2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withValues()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		ConstantNodalCurve test = @base.withYValues(YVALUE_BUMPED_ARRAY);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(XVALUE_ARRAY);
		assertThat(test.YValues).isEqualTo(YVALUE_BUMPED_ARRAY);
	  }

	  public virtual void test_withValues_badSize()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		assertThrowsIllegalArg(() => @base.withYValues(DoubleArray.EMPTY));
		assertThrowsIllegalArg(() => @base.withYValues(DoubleArray.of(4d, 6d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withValuesXy()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		ConstantNodalCurve test = @base.withValues(XVALUE_ARRAY_NEW, YVALUE_BUMPED_ARRAY);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(XVALUE_ARRAY_NEW);
		assertThat(test.YValues).isEqualTo(YVALUE_BUMPED_ARRAY);
	  }

	  public virtual void test_withValuesXy_badSize()
	  {
		ConstantNodalCurve @base = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		assertThrowsIllegalArg(() => @base.withValues(DoubleArray.EMPTY, DoubleArray.EMPTY));
		assertThrowsIllegalArg(() => @base.withValues(DoubleArray.of(4d), DoubleArray.of(6d, 0d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		coverImmutableBean(test);
		ConstantNodalCurve test2 = ConstantNodalCurve.of(METADATA_ENTRIES, 55d, 23d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ConstantNodalCurve test = ConstantNodalCurve.of(METADATA, XVALUE, YVALUE);
		assertSerialization(test);
	  }

	}

}