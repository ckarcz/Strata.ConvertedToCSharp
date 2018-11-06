/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
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

	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="ConstantSurface"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConstantSurfaceTest
	public class ConstantSurfaceTest
	{

	  private const string NAME = "TestSurface";
	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of(NAME);
	  private static readonly SurfaceMetadata METADATA = DefaultSurfaceMetadata.of(SURFACE_NAME);
	  private static readonly SurfaceMetadata METADATA2 = DefaultSurfaceMetadata.of("Test2");
	  private const double VALUE = 6d;

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String()
	  {
		ConstantSurface test = ConstantSurface.of(NAME, VALUE);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ZValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantSurface.of(NAME, 2d));
		assertThat(test.withPerturbation((i, v, m) => v + 1d)).isEqualTo(ConstantSurface.of(NAME, VALUE + 1d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantSurface.of(METADATA2, VALUE));
	  }

	  public virtual void test_of_SurfaceName()
	  {
		ConstantSurface test = ConstantSurface.of(SURFACE_NAME, VALUE);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ZValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantSurface.of(NAME, 2d));
		assertThat(test.withPerturbation((i, v, m) => v + 1d)).isEqualTo(ConstantSurface.of(NAME, VALUE + 1d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantSurface.of(METADATA2, VALUE));
	  }

	  public virtual void test_of_SurfaceMetadata()
	  {
		ConstantSurface test = ConstantSurface.of(METADATA, VALUE);
		assertThat(test.Name).isEqualTo(SURFACE_NAME);
		assertThat(test.ZValue).isEqualTo(VALUE);
		assertThat(test.ParameterCount).isEqualTo(1);
		assertThat(test.getParameter(0)).isEqualTo(VALUE);
		assertThat(test.getParameterMetadata(0)).isEqualTo(ParameterMetadata.empty());
		assertThat(test.withParameter(0, 2d)).isEqualTo(ConstantSurface.of(NAME, 2d));
		assertThat(test.withPerturbation((i, v, m) => v + 1d)).isEqualTo(ConstantSurface.of(NAME, VALUE + 1d));
		assertThat(test.Metadata).isEqualTo(METADATA);
		assertThat(test.withMetadata(METADATA2)).isEqualTo(ConstantSurface.of(METADATA2, VALUE));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_lookup()
	  {
		ConstantSurface test = ConstantSurface.of(SURFACE_NAME, VALUE);
		assertThat(test.zValue(0d, 0d)).isEqualTo(VALUE);
		assertThat(test.zValue(-10d, 10d)).isEqualTo(VALUE);
		assertThat(test.zValue(100d, -100d)).isEqualTo(VALUE);

		assertThat(test.zValueParameterSensitivity(0d, 0d).Sensitivity.get(0)).isEqualTo(1d);
		assertThat(test.zValueParameterSensitivity(-10d, 10d).Sensitivity.get(0)).isEqualTo(1d);
		assertThat(test.zValueParameterSensitivity(100d, -100d).Sensitivity.get(0)).isEqualTo(1d);
	  }

	  public virtual void test_lookup_byPair()
	  {
		ConstantSurface test = ConstantSurface.of(SURFACE_NAME, VALUE);
		assertThat(test.zValue(DoublesPair.of(0d, 0d))).isEqualTo(VALUE);
		assertThat(test.zValue(DoublesPair.of(-10d, 10d))).isEqualTo(VALUE);
		assertThat(test.zValue(DoublesPair.of(100d, -100d))).isEqualTo(VALUE);

		assertThat(test.zValueParameterSensitivity(DoublesPair.of(0d, 0d)).Sensitivity.get(0)).isEqualTo(1d);
		assertThat(test.zValueParameterSensitivity(DoublesPair.of(-10d, 10d)).Sensitivity.get(0)).isEqualTo(1d);
		assertThat(test.zValueParameterSensitivity(DoublesPair.of(100d, -100d)).Sensitivity.get(0)).isEqualTo(1d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ConstantSurface test = ConstantSurface.of(SURFACE_NAME, VALUE);
		coverImmutableBean(test);
		ConstantSurface test2 = ConstantSurface.of("Coverage", 9d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		ConstantSurface test = ConstantSurface.of(SURFACE_NAME, VALUE);
		assertSerialization(test);
	  }

	}

}