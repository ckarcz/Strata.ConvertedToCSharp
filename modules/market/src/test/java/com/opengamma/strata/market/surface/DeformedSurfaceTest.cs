/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArrayMath = com.opengamma.strata.collect.DoubleArrayMath;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;

	/// <summary>
	/// Test <seealso cref="DeformedSurface"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DeformedSurfaceTest
	public class DeformedSurfaceTest
	{

	  private const int SIZE = 9;
	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of("TestSurface");
	  private static readonly SurfaceMetadata METADATA_ORG = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).dayCount(ACT_365F).parameterMetadata(ParameterMetadata.listOfEmpty(SIZE)).build();
	  private static readonly DoubleArray XVALUES = DoubleArray.of(0d, 0d, 0d, 2d, 2d, 2d, 4d, 4d, 4d);
	  private static readonly DoubleArray YVALUES = DoubleArray.of(0d, 3d, 4d, 0d, 3d, 4d, 0d, 3d, 4d);
	  private static readonly DoubleArray ZVALUES = DoubleArray.of(5d, 7d, 8d, 6d, 7d, 8d, 8d, 7d, 8d);
	  private static readonly GridSurfaceInterpolator INTERPOLATOR = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly InterpolatedNodalSurface SURFACE_ORG = InterpolatedNodalSurface.of(METADATA_ORG, XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
	  private static readonly System.Func<DoublesPair, ValueDerivatives> FUNCTION = (DoublesPair x) =>
	  {
  double value = 1.5 * SURFACE_ORG.zValue(x) * x.First * x.Second;
  DoubleArray derivatives = SURFACE_ORG.zValueParameterSensitivity(x).multipliedBy(1.5 * x.First * x.Second).Sensitivity;
  return ValueDerivatives.of(value, derivatives);
	  };
	  private static readonly SurfaceMetadata METADATA = DefaultSurfaceMetadata.of("DeformedTestSurface");

	  public virtual void test_of()
	  {
		DeformedSurface test = DeformedSurface.of(METADATA, SURFACE_ORG, FUNCTION);
		assertEquals(test.DeformationFunction, FUNCTION);
		assertEquals(test.Metadata, METADATA);
		assertEquals(test.Name, METADATA.SurfaceName);
		assertEquals(test.OriginalSurface, SURFACE_ORG);
		assertEquals(test.ParameterCount, SIZE);
		assertEquals(test.getParameter(2), SURFACE_ORG.getParameter(2));
		assertEquals(test.getParameterMetadata(2), SURFACE_ORG.getParameterMetadata(2));
	  }

	  public virtual void test_zValue()
	  {
		double tol = 1.0e-14;
		double x = 2.5;
		double y = 1.44;
		DeformedSurface test = DeformedSurface.of(METADATA, SURFACE_ORG, FUNCTION);
		double computedValue1 = test.zValue(x, y);
		double computedValue2 = test.zValue(DoublesPair.of(x, y));
		UnitParameterSensitivity computedSensi1 = test.zValueParameterSensitivity(x, y);
		UnitParameterSensitivity computedSensi2 = test.zValueParameterSensitivity(DoublesPair.of(x, y));
		ValueDerivatives expected = FUNCTION.apply(DoublesPair.of(x, y));
		assertEquals(computedValue1, expected.Value);
		assertEquals(computedValue2, expected.Value);
		assertTrue(DoubleArrayMath.fuzzyEquals(computedSensi1.Sensitivity.toArray(), expected.Derivatives.toArray(), tol));
		assertTrue(DoubleArrayMath.fuzzyEquals(computedSensi2.Sensitivity.toArray(), expected.Derivatives.toArray(), tol));
	  }

	  public virtual void test_withParameter()
	  {
		assertThrowsIllegalArg(() => DeformedSurface.of(METADATA, SURFACE_ORG, FUNCTION).withParameter(1, 1.2d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DeformedSurface test1 = DeformedSurface.of(METADATA, SURFACE_ORG, FUNCTION);
		coverImmutableBean(test1);
		Surface surface1 = InterpolatedNodalSurface.of(DefaultSurfaceMetadata.of("TestSurface1"), XVALUES, YVALUES, ZVALUES, INTERPOLATOR);
		DeformedSurface test2 = DeformedSurface.of(DefaultSurfaceMetadata.of("DeformedTestSurface1"), surface1, (DoublesPair x) =>
		{
		return ValueDerivatives.of(surface1.zValue(x), surface1.zValueParameterSensitivity(x).Sensitivity);
		});
		coverBeanEquals(test1, test2);
	  }

	}

}