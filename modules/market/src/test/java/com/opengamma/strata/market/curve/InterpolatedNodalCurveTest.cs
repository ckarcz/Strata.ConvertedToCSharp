using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BoundCurveInterpolator = com.opengamma.strata.market.curve.interpolator.BoundCurveInterpolator;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Test <seealso cref="InterpolatedNodalCurve"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InterpolatedNodalCurveTest
	public class InterpolatedNodalCurveTest
	{

	  private const int SIZE = 3;
	  private const string TNR_1Y = "1Y";

	  private const string NAME = "TestCurve";
	  private static readonly CurveName CURVE_NAME = CurveName.of(NAME);
	  private static readonly CurveMetadata METADATA = Curves.zeroRates(CURVE_NAME, ACT_365F);
	  private static readonly CurveMetadata METADATA_ENTRIES = Curves.zeroRates(CURVE_NAME, ACT_365F, ParameterMetadata.listOfEmpty(SIZE));
	  private static readonly CurveMetadata METADATA_ENTRIES2 = Curves.zeroRates(CURVE_NAME, ACT_365F, ParameterMetadata.listOfEmpty(SIZE + 2));
	  private static readonly CurveMetadata METADATA_NOPARAM = Curves.zeroRates(CURVE_NAME, ACT_365F);
	  private static readonly DoubleArray XVALUES = DoubleArray.of(1d, 2d, 3d);
	  private static readonly DoubleArray XVALUES2 = DoubleArray.of(0d, 2d, 3d);
	  private static readonly DoubleArray YVALUES = DoubleArray.of(5d, 7d, 8d);
	  private static readonly DoubleArray YVALUES_BUMPED = DoubleArray.of(3d, 5d, 6d);
	  private static readonly CurveInterpolator INTERPOLATOR = CurveInterpolators.LOG_LINEAR;
	  private static readonly CurveExtrapolator FLAT_EXTRAPOLATOR = CurveExtrapolators.FLAT;
	  private static readonly CurveExtrapolator LINEAR_EXTRAPOLATOR = CurveExtrapolators.LINEAR;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurveMetadata()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.getParameter(0)).isEqualTo(YVALUES.get(0));
		assertThat(test.getParameter(1)).isEqualTo(YVALUES.get(1));
		assertThat(test.getParameterMetadata(0)).isSameAs(METADATA_ENTRIES.ParameterMetadata.get().get(0));
		assertThat(test.getParameterMetadata(1)).isSameAs(METADATA_ENTRIES.ParameterMetadata.get().get(1));
		assertThat(test.withParameter(0, 2d)).isEqualTo(InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES.with(0, 2d), INTERPOLATOR));
		assertThat(test.withPerturbation((i, v, m) => v - 2d)).isEqualTo(InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES_BUMPED, INTERPOLATOR));
		assertThat(test.ExtrapolatorLeft.Name).isEqualTo(FLAT_EXTRAPOLATOR.Name);
		assertThat(test.Interpolator.Name).isEqualTo(INTERPOLATOR.Name);
		assertThat(test.ExtrapolatorRight.Name).isEqualTo(FLAT_EXTRAPOLATOR.Name);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES);
	  }

	  public virtual void test_of_extrapolators()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR, LINEAR_EXTRAPOLATOR, LINEAR_EXTRAPOLATOR);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ExtrapolatorLeft.Name).isEqualTo(LINEAR_EXTRAPOLATOR.Name);
		assertThat(test.Interpolator.Name).isEqualTo(INTERPOLATOR.Name);
		assertThat(test.ExtrapolatorRight.Name).isEqualTo(LINEAR_EXTRAPOLATOR.Name);
	  }

	  public virtual void test_of_noCurveMetadata()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA_NOPARAM, XVALUES, YVALUES, INTERPOLATOR);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.getParameter(0)).isEqualTo(YVALUES.get(0));
		assertThat(test.getParameter(1)).isEqualTo(YVALUES.get(1));
		assertThat(test.getParameterMetadata(0)).isEqualTo(SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, XVALUES.get(0)));
		assertThat(test.getParameterMetadata(1)).isEqualTo(SimpleCurveParameterMetadata.of(ValueType.YEAR_FRACTION, XVALUES.get(1)));
	  }

	  public virtual void test_of_invalid()
	  {
		// not enough nodes
		assertThrowsIllegalArg(() => InterpolatedNodalCurve.of(METADATA, DoubleArray.of(1d), DoubleArray.of(1d), INTERPOLATOR));
		// x node size != y node size
		assertThrowsIllegalArg(() => InterpolatedNodalCurve.of(METADATA, XVALUES, DoubleArray.of(1d, 3d), INTERPOLATOR));
		// parameter metadata size != node size
		assertThrowsIllegalArg(() => InterpolatedNodalCurve.of(METADATA_ENTRIES, DoubleArray.of(1d, 3d), DoubleArray.of(1d, 3d), INTERPOLATOR));
		// x not in order
		assertThrowsIllegalArg(() => InterpolatedNodalCurve.of(METADATA, DoubleArray.of(2d, 1d), DoubleArray.of(2d, 3d), INTERPOLATOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_lookup()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		BoundCurveInterpolator interp = INTERPOLATOR.bind(XVALUES, YVALUES, FLAT_EXTRAPOLATOR, FLAT_EXTRAPOLATOR);
		assertThat(test.yValue(XVALUES.get(0))).isEqualTo(YVALUES.get(0));
		assertThat(test.yValue(XVALUES.get(1))).isEqualTo(YVALUES.get(1));
		assertThat(test.yValue(XVALUES.get(2))).isEqualTo(YVALUES.get(2));
		assertThat(test.yValue(10d)).isEqualTo(interp.interpolate(10d));

		assertThat(test.yValueParameterSensitivity(10d).MarketDataName).isEqualTo(CURVE_NAME);
		assertThat(test.yValueParameterSensitivity(10d).Sensitivity).isEqualTo(interp.parameterSensitivity(10d));
		assertThat(test.firstDerivative(10d)).isEqualTo(interp.firstDerivative(10d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withMetadata()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		InterpolatedNodalCurve test = @base.withMetadata(METADATA_ENTRIES);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES);
	  }

	  public virtual void test_withMetadata_badSize()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		assertThrowsIllegalArg(() => @base.withMetadata(METADATA_ENTRIES2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withValues()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		InterpolatedNodalCurve test = @base.withYValues(YVALUES_BUMPED);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES_BUMPED);
	  }

	  public virtual void test_withValues_badSize()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		assertThrowsIllegalArg(() => @base.withYValues(DoubleArray.EMPTY));
		assertThrowsIllegalArg(() => @base.withYValues(DoubleArray.of(4d, 6d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withValuesXy()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		InterpolatedNodalCurve test = @base.withValues(XVALUES2, YVALUES_BUMPED);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(XVALUES2);
		assertThat(test.YValues).isEqualTo(YVALUES_BUMPED);
	  }

	  public virtual void test_withValuesXy_badSize()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		assertThrowsIllegalArg(() => @base.withValues(DoubleArray.EMPTY, DoubleArray.EMPTY));
		assertThrowsIllegalArg(() => @base.withValues(DoubleArray.of(1d, 3d, 5d), DoubleArray.of(4d, 6d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withNode_atStart_withMetadata()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR);
		LabelDateParameterMetadata item = LabelDateParameterMetadata.of(date(2015, 6, 30), TNR_1Y);
		InterpolatedNodalCurve test = @base.withNode(0.5d, 4d, item);
		IList<ParameterMetadata> list = new List<ParameterMetadata>();
		list.Add(item);
		((IList<ParameterMetadata>)list).AddRange(ParameterMetadata.listOfEmpty(SIZE));
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE + 1);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES.withParameterMetadata(list));
		assertThat(test.XValues).isEqualTo(DoubleArray.of(0.5d, 1d, 2d, 3d));
		assertThat(test.YValues).isEqualTo(DoubleArray.of(4d, 5d, 7d, 8d));
	  }

	  public virtual void test_withNode_inMiddle_withMetadata()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR);
		LabelDateParameterMetadata item = LabelDateParameterMetadata.of(date(2015, 6, 30), TNR_1Y);
		InterpolatedNodalCurve test = @base.withNode(2.5d, 4d, item);
		IList<ParameterMetadata> list = new List<ParameterMetadata>(METADATA_ENTRIES.ParameterMetadata.get());
		list.Insert(2, item);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE + 1);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES.withParameterMetadata(list));
		assertThat(test.XValues).isEqualTo(DoubleArray.of(1d, 2d, 2.5d, 3d));
		assertThat(test.YValues).isEqualTo(DoubleArray.of(5d, 7d, 4d, 8d));
	  }

	  public virtual void test_withNode_atEnd_withoutMetadata()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		LabelDateParameterMetadata item = LabelDateParameterMetadata.of(date(2015, 6, 30), TNR_1Y);
		InterpolatedNodalCurve test = @base.withNode(0.5d, 4d, item);
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE + 1);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(DoubleArray.of(0.5d, 1d, 2d, 3d));
		assertThat(test.YValues).isEqualTo(DoubleArray.of(4d, 5d, 7d, 8d));
	  }

	  public virtual void test_withNode_replace_withMetadata()
	  {
		InterpolatedNodalCurve @base = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR);
		LabelDateParameterMetadata item = LabelDateParameterMetadata.of(date(2015, 6, 30), TNR_1Y);
		InterpolatedNodalCurve test = @base.withNode(2d, 4d, item);
		IList<ParameterMetadata> list = new List<ParameterMetadata>(METADATA_ENTRIES.ParameterMetadata.get());
		list[1] = item;
		assertThat(test.Name).isEqualTo(CURVE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA.withParameterMetadata(list));
		assertThat(test.XValues).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.YValues).isEqualTo(DoubleArray.of(5d, 4d, 8d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_createParameterSensitivity()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA_ENTRIES, XVALUES, YVALUES, INTERPOLATOR);
		assertThat(test.createParameterSensitivity(DoubleArray.of(2d, 3d, 4d))).isEqualTo(UnitParameterSensitivity.of(CURVE_NAME, DoubleArray.of(2d, 3d, 4d)));
		assertThat(test.createParameterSensitivity(Currency.GBP, DoubleArray.of(2d, 3d, 4d))).isEqualTo(CurrencyParameterSensitivity.of(CURVE_NAME, Currency.GBP, DoubleArray.of(2d, 3d, 4d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		coverImmutableBean(test);
		InterpolatedNodalCurve test2 = InterpolatedNodalCurve.builder().metadata(METADATA_ENTRIES).xValues(XVALUES2).yValues(YVALUES_BUMPED).extrapolatorLeft(CurveExtrapolators.LOG_LINEAR).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorRight(CurveExtrapolators.LOG_LINEAR).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InterpolatedNodalCurve test = InterpolatedNodalCurve.of(METADATA, XVALUES, YVALUES, INTERPOLATOR);
		assertSerialization(test);
	  }

	}

}