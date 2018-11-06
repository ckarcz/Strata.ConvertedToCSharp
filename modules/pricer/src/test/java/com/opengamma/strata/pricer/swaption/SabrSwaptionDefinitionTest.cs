/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LOG_LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using SurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.SurfaceInterpolator;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapConventions = com.opengamma.strata.product.swap.type.FixedIborSwapConventions;

	/// <summary>
	/// Tests <seealso cref="SabrSwaptionDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrSwaptionDefinitionTest
	public class SabrSwaptionDefinitionTest
	{

	  private static readonly SwaptionVolatilitiesName NAME = SwaptionVolatilitiesName.of("Test");
	  private static readonly SwaptionVolatilitiesName NAME2 = SwaptionVolatilitiesName.of("Test2");
	  private static readonly FixedIborSwapConvention CONVENTION = FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_3M;
	  private static readonly FixedIborSwapConvention CONVENTION2 = FixedIborSwapConventions.EUR_FIXED_1Y_EURIBOR_6M;
	  private static readonly DayCount DAY_COUNT = DayCounts.ACT_360;
	  private static readonly DayCount DAY_COUNT2 = DayCounts.ACT_365F;
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D = GridSurfaceInterpolator.of(LINEAR, LINEAR);
	  private static readonly SurfaceInterpolator INTERPOLATOR_2D2 = GridSurfaceInterpolator.of(LINEAR, LOG_LINEAR);

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		SabrSwaptionDefinition test = SabrSwaptionDefinition.of(NAME, CONVENTION, DAY_COUNT, INTERPOLATOR_2D);
		assertEquals(test.Name, NAME);
		assertEquals(test.Convention, CONVENTION);
		assertEquals(test.DayCount, DAY_COUNT);
		assertEquals(test.Interpolator, INTERPOLATOR_2D);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SabrSwaptionDefinition test = SabrSwaptionDefinition.of(NAME, CONVENTION, DAY_COUNT, INTERPOLATOR_2D);
		coverImmutableBean(test);
		SabrSwaptionDefinition test2 = SabrSwaptionDefinition.of(NAME2, CONVENTION2, DAY_COUNT2, INTERPOLATOR_2D2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		SabrSwaptionDefinition test = SabrSwaptionDefinition.of(NAME, CONVENTION, DAY_COUNT, INTERPOLATOR_2D);
		assertSerialization(test);
	  }

	}

}