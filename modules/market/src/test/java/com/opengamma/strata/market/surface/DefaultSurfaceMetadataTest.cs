/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
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

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="DefaultSurfaceMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultSurfaceMetadataTest
	public class DefaultSurfaceMetadataTest
	{

	  private const string NAME = "TestSurface";
	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of(NAME);
	  private static readonly SurfaceInfoType<string> DESCRIPTION = SurfaceInfoType.of("Description");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_String_noMetadata()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(NAME);
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.Info).isEqualTo(ImmutableMap.of());
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_of_SurfaceName_noMetadata()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME);
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.Info).isEqualTo(ImmutableMap.of());
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_builder1()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME.ToString()).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).zValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).addInfo(DESCRIPTION, "Hello").parameterMetadata(ImmutableList.of(ParameterMetadata.empty())).build();
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.YEAR_FRACTION);
		assertThat(test.YValueType).isEqualTo(ValueType.DISCOUNT_FACTOR);
		assertThat(test.ZValueType).isEqualTo(ValueType.ZERO_RATE);
		assertThat(test.getInfo(SurfaceInfoType.DAY_COUNT)).isEqualTo(ACT_365F);
		assertThat(test.findInfo(SurfaceInfoType.DAY_COUNT)).isEqualTo(ACT_365F);
		assertThat(test.getInfo(DESCRIPTION)).isEqualTo("Hello");
		assertThat(test.findInfo(DESCRIPTION)).isEqualTo(("Hello"));
		assertThat(test.findInfo(SurfaceInfoType.of("Rubbish"))).isEqualTo(null);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  public virtual void test_builder2()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).zValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).addInfo(SurfaceInfoType.DAY_COUNT, null).addInfo(DESCRIPTION, "Hello").parameterMetadata(ImmutableList.of(ParameterMetadata.empty())).build();
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.YEAR_FRACTION);
		assertThat(test.YValueType).isEqualTo(ValueType.DISCOUNT_FACTOR);
		assertThat(test.ZValueType).isEqualTo(ValueType.ZERO_RATE);
		assertThrowsIllegalArg(() => test.getInfo(SurfaceInfoType.DAY_COUNT));
		assertThat(test.findInfo(SurfaceInfoType.DAY_COUNT)).Empty;
		assertThat(test.getInfo(DESCRIPTION)).isEqualTo("Hello");
		assertThat(test.findInfo(DESCRIPTION)).isEqualTo(("Hello"));
		assertThat(test.findInfo(SurfaceInfoType.of("Rubbish"))).isEqualTo(null);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  public virtual void test_builder3()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).parameterMetadata(ImmutableList.of(ParameterMetadata.empty())).clearParameterMetadata().build();
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withInfo()
	  {
		DefaultSurfaceMetadata @base = DefaultSurfaceMetadata.of(SURFACE_NAME);
		assertThat(@base.findInfo(SurfaceInfoType.DAY_COUNT).Present).False;
		DefaultSurfaceMetadata test = @base.withInfo(SurfaceInfoType.DAY_COUNT, ACT_360);
		assertThat(@base.findInfo(SurfaceInfoType.DAY_COUNT).Present).False;
		assertThat(test.findInfo(SurfaceInfoType.DAY_COUNT).Present).True;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withParameterMetadata()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME).withParameterMetadata(ImmutableList.of(ParameterMetadata.empty()));
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ParameterMetadata.Present).True;
		assertThat(test.ParameterMetadata.get()).containsExactly(ParameterMetadata.empty());
	  }

	  public virtual void test_withParameterMetadata_clearWhenEmpty()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME).withParameterMetadata(null);
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  public virtual void test_withParameterMetadata_clearWhenNonEmpty()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME).withParameterMetadata(ImmutableList.of(ParameterMetadata.empty())).withParameterMetadata(null);
		assertThat(test.SurfaceName).isEqualTo(SURFACE_NAME);
		assertThat(test.XValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.YValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ZValueType).isEqualTo(ValueType.UNKNOWN);
		assertThat(test.ParameterMetadata.Present).False;
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME);
		coverImmutableBean(test);
		DefaultSurfaceMetadata test2 = DefaultSurfaceMetadata.builder().surfaceName(SurfaceName.of("Test")).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).zValueType(ValueType.ZERO_RATE).dayCount(ACT_365F).parameterMetadata(ParameterMetadata.empty()).build();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		DefaultSurfaceMetadata test = DefaultSurfaceMetadata.of(SURFACE_NAME);
		assertSerialization(test);
	  }

	}

}