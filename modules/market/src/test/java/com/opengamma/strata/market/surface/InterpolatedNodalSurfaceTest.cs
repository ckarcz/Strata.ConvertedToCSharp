/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
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
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using BoundSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.BoundSurfaceInterpolator;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;

	/// <summary>
	/// Test <seealso cref="InterpolatedNodalSurface"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class InterpolatedNodalSurfaceTest
	public class InterpolatedNodalSurfaceTest
	{

	  private const int SIZE = 9;
	  private const string NAME = "TestSurface";
	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of(NAME);
	  private static readonly SurfaceMetadata METADATA = DefaultSurfaceMetadata.of(SURFACE_NAME);
	  private static readonly SurfaceMetadata METADATA_ENTRIES = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).dayCount(ACT_365F).parameterMetadata(ParameterMetadata.listOfEmpty(SIZE)).build();
	  private static readonly SurfaceMetadata METADATA_ENTRIES2 = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).dayCount(ACT_365F).parameterMetadata(ParameterMetadata.listOfEmpty(SIZE + 2)).build();
	  private static readonly DoubleArray XVALUES = DoubleArray.of(0d, 0d, 0d, 2d, 2d, 2d, 4d, 4d, 4d);
	  private static readonly DoubleArray XVALUES2 = DoubleArray.of(1d, 1d, 1d, 2d, 2d, 2d, 3d, 3d, 3d);
	  private static readonly DoubleArray YVALUES = DoubleArray.of(0d, 3d, 4d, 0d, 3d, 4d, 0d, 3d, 4d);
	  private static readonly DoubleArray YVALUES2 = DoubleArray.of(3d, 4d, 5d, 3d, 4d, 5d, 3d, 4d, 5d);
	  private static readonly DoubleArray ZVALUES = DoubleArray.of(5d, 7d, 8d, 6d, 7d, 8d, 8d, 7d, 8d);
	  private static readonly DoubleArray ZVALUES_BUMPED = DoubleArray.of(3d, 5d, 6d, 4d, 5d, 6d, 6d, 5d, 6d);
	  private static readonly GridSurfaceInterpolator INTERPOLATOR = GridSurfaceInterpolator.of(LINEAR, LINEAR);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_SurfaceMetadata()
	  {
		InterpolatedNodalSurface test = InterpolatedNodalSurface.of(METADATA_ENTRIES, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.getParameter(0)).isEqualTo(ZVALUES.get(0));
		assertThat(test.getParameter(1)).isEqualTo(ZVALUES.get(1));
		assertThat(test.getParameterMetadata(0)).isSameAs(METADATA_ENTRIES.ParameterMetadata.get().get(0));
		assertThat(test.getParameterMetadata(1)).isSameAs(METADATA_ENTRIES.ParameterMetadata.get().get(1));
		assertThat(test.withParameter(0, 2d)).isEqualTo(InterpolatedNodalSurface.of(METADATA_ENTRIES, XVALUES, YVALUES, ZVALUES.with(0, 2d), INTERPOLATOR));
		assertThat(test.withPerturbation((i, v, m) => v - 2d)).isEqualTo(InterpolatedNodalSurface.of(METADATA_ENTRIES, XVALUES, YVALUES, ZVALUES_BUMPED, INTERPOLATOR));
		assertThat(test.Interpolator).isEqualTo(INTERPOLATOR);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES);
		assertThat(test.ZValues).isEqualTo(ZVALUES);
	  }

	  public virtual void test_of_invalid()
	  {
		// not enough nodes
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA, DoubleArray.of(1d), DoubleArray.of(2d), DoubleArray.of(3d), INTERPOLATOR));
		// x node size != y node size
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA, XVALUES, DoubleArray.of(1d, 3d), ZVALUES, INTERPOLATOR));
		// x node size != z node size
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, DoubleArray.of(1d, 3d), INTERPOLATOR));
		// parameter metadata size != node size
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA_ENTRIES, DoubleArray.of(1d, 3d), DoubleArray.of(1d, 3d), DoubleArray.of(1d, 3d), INTERPOLATOR));
		// x not in order
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA, DoubleArray.of(2d, 1d), DoubleArray.of(1d, 1d), DoubleArray.of(2d, 3d), INTERPOLATOR));
		// y not in order
		assertThrowsIllegalArg(() => InterpolatedNodalSurface.of(METADATA, DoubleArray.of(1d, 1d), DoubleArray.of(2d, 1d), DoubleArray.of(2d, 3d), INTERPOLATOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_lookup()
	  {
		InterpolatedNodalSurface test = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		assertThat(test.zValue(XVALUES.get(0), YVALUES.get(0))).isEqualTo(ZVALUES.get(0));
		assertThat(test.zValue(XVALUES.get(1), YVALUES.get(1))).isEqualTo(ZVALUES.get(1));
		assertThat(test.zValue(XVALUES.get(2), YVALUES.get(2))).isEqualTo(ZVALUES.get(2));
		assertThat(test.zValue(0d, 1.5d)).isEqualTo(6d);
		assertThat(test.zValue(1d, 3d)).isEqualTo(7d);

		BoundSurfaceInterpolator bound = INTERPOLATOR.bind(XVALUES, YVALUES, ZVALUES);
		assertThat(test.zValue(1.5d, 3.7d)).isEqualTo(bound.interpolate(1.5d, 3.7d));
		DoubleArray sensiValues = test.zValueParameterSensitivity(1.5d, 1.5d).Sensitivity;
		DoubleArray sensiValuesInterp = bound.parameterSensitivity(1.5d, 1.5d);
		assertTrue(sensiValues.equalWithTolerance(sensiValuesInterp, 1e-8));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withMetadata()
	  {
		InterpolatedNodalSurface @base = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		InterpolatedNodalSurface test = @base.withMetadata(METADATA_ENTRIES);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA_ENTRIES);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES);
		assertThat(test.ZValues).isEqualTo(ZVALUES);
	  }

	  public virtual void test_withMetadata_badSize()
	  {
		InterpolatedNodalSurface @base = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		assertThrowsIllegalArg(() => @base.withMetadata(METADATA_ENTRIES2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withZValues()
	  {
		InterpolatedNodalSurface @base = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		InterpolatedNodalSurface test = @base.withZValues(ZVALUES_BUMPED);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ParameterCount).isEqualTo(SIZE);
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.XValues).isEqualTo(XVALUES);
		assertThat(test.YValues).isEqualTo(YVALUES);
		assertThat(test.ZValues).isEqualTo(ZVALUES_BUMPED);
	  }

	  public virtual void test_withZValues_badSize()
	  {
		InterpolatedNodalSurface @base = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		assertThrowsIllegalArg(() => @base.withZValues(DoubleArray.EMPTY));
		assertThrowsIllegalArg(() => @base.withZValues(DoubleArray.of(4d, 6d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		InterpolatedNodalSurface test = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		coverImmutableBean(test);
		InterpolatedNodalSurface test2 = InterpolatedNodalSurface.builder().metadata(METADATA_ENTRIES).xValues(XVALUES2).yValues(YVALUES2).zValues(ZVALUES_BUMPED).interpolator(GridSurfaceInterpolator.of(DOUBLE_QUADRATIC, FLAT, FLAT, LINEAR, FLAT, FLAT)).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		InterpolatedNodalSurface test = InterpolatedNodalSurface.of(METADATA, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		assertSerialization(test);
	  }

	}

}