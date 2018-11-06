using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.STEP_UPPER;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using GenericVolatilitySurfacePeriodParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfacePeriodParameterMetadata;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Test <seealso cref="SurfaceIborCapletFloorletVolatilityBootstrapDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SurfaceIborCapletFloorletVolatilityBootstrapDefinitionTest
	public class SurfaceIborCapletFloorletVolatilityBootstrapDefinitionTest
	{

	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("TestName");
	  private static readonly ConstantCurve SHIFT = ConstantCurve.of("Black shift", 0.02);

	  public virtual void test_of()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, STEP_UPPER, DOUBLE_QUADRATIC);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, GridSurfaceInterpolator.of(STEP_UPPER, DOUBLE_QUADRATIC));
		assertEquals(test.Name, NAME);
		assertFalse(test.ShiftCurve.Present);
	  }

	  public virtual void test_of_surface()
	  {
		GridSurfaceInterpolator interp = GridSurfaceInterpolator.of(LINEAR, LINEAR);
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, interp);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, interp);
		assertEquals(test.Name, NAME);
		assertFalse(test.ShiftCurve.Present);
	  }

	  public virtual void test_of_shift()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, STEP_UPPER, DOUBLE_QUADRATIC, SHIFT);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, GridSurfaceInterpolator.of(STEP_UPPER, DOUBLE_QUADRATIC));
		assertEquals(test.Name, NAME);
		assertEquals(test.ShiftCurve.get(), SHIFT);
	  }

	  public virtual void test_of_surface_shift()
	  {
		GridSurfaceInterpolator interp = GridSurfaceInterpolator.of(LINEAR, LINEAR);
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, interp, SHIFT);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, interp);
		assertEquals(test.Name, NAME);
		assertEquals(test.ShiftCurve.get(), SHIFT);
	  }

	  public virtual void test_createCap()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition @base = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, TIME_SQUARE, DOUBLE_QUADRATIC);
		LocalDate startDate = LocalDate.of(2012, 4, 20);
		LocalDate endDate = LocalDate.of(2017, 4, 20);
		double strike = 0.01;
		IborCapFloorLeg expected = IborCapFloorLeg.builder().calculation(IborRateCalculation.of(USD_LIBOR_3M)).capSchedule(ValueSchedule.of(strike)).currency(USD_LIBOR_3M.Currency).notional(ValueSchedule.ALWAYS_1).paymentDateOffset(DaysAdjustment.NONE).paymentSchedule(PeriodicSchedule.of(startDate, endDate, Frequency.of(USD_LIBOR_3M.Tenor.Period), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, USD_LIBOR_3M.FixingCalendar), StubConvention.NONE, RollConventions.NONE)).payReceive(PayReceive.RECEIVE).build();
		IborCapFloorLeg computed = @base.createCap(startDate, endDate, strike);
		assertEquals(computed, expected);
	  }

	  public virtual void test_createMetadata_normal()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition @base = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		RawOptionData capData = RawOptionData.of(ImmutableList.of(Period.ofYears(1), Period.ofYears(5)), DoubleArray.of(0.005, 0.01, 0.015), ValueType.STRIKE, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.15, 0.12, 0.13},
			new double[] {0.1, Double.NaN, 0.09}
		}), ValueType.NORMAL_VOLATILITY);
		IList<GenericVolatilitySurfacePeriodParameterMetadata> list = new List<GenericVolatilitySurfacePeriodParameterMetadata>();
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.005)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.01)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.015)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(5), SimpleStrike.of(0.005)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(5), SimpleStrike.of(0.015)));
		SurfaceMetadata expected = Surfaces.normalVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA).withParameterMetadata(list);
		SurfaceMetadata computed = @base.createMetadata(capData);
		assertEquals(computed, expected);
	  }

	  public virtual void test_createMetadata_black()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition @base = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		RawOptionData capData = RawOptionData.of(ImmutableList.of(Period.ofYears(1), Period.ofYears(5)), DoubleArray.of(0.005, 0.01, 0.015), ValueType.STRIKE, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.15, 0.12, 0.13},
			new double[] {0.1, 0.08, 0.09}
		}), ValueType.BLACK_VOLATILITY);
		IList<GenericVolatilitySurfacePeriodParameterMetadata> list = new List<GenericVolatilitySurfacePeriodParameterMetadata>();
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.005)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.01)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(1), SimpleStrike.of(0.015)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(5), SimpleStrike.of(0.005)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(5), SimpleStrike.of(0.01)));
		list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofYears(5), SimpleStrike.of(0.015)));
		SurfaceMetadata expected = Surfaces.blackVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA).withParameterMetadata(list);
		SurfaceMetadata computed = @base.createMetadata(capData);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_wrongInterpolator()
	  {
		assertThrowsIllegalArg(() => SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, DOUBLE_QUADRATIC, DOUBLE_QUADRATIC));

	  }
	  public virtual void test_createMetadata_wrongValueType()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition @base = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		RawOptionData capData = RawOptionData.of(ImmutableList.of(Period.ofYears(1), Period.ofYears(5)), DoubleArray.of(0.005, 0.01, 0.015), ValueType.STRIKE, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.15, 0.12, 0.13},
			new double[] {0.1, 0.08, 0.09}
		}), ValueType.PRICE);
		assertThrowsIllegalArg(() => @base.createMetadata(capData));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test1 = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		coverImmutableBean(test1);
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test2 = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(IborCapletFloorletVolatilitiesName.of("other"), GBP_LIBOR_3M, ACT_365F, LINEAR, LINEAR, SHIFT);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SurfaceIborCapletFloorletVolatilityBootstrapDefinition test = SurfaceIborCapletFloorletVolatilityBootstrapDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LINEAR, DOUBLE_QUADRATIC);
		assertSerialization(test);
	  }

	}

}