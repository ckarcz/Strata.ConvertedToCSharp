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
//	import static com.opengamma.strata.market.ValueType.SABR_ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_RHO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.STEP_UPPER;
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
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Test <seealso cref="SabrIborCapletFloorletVolatilityBootstrapDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrIborCapletFloorletVolatilityBootstrapDefinitionTest
	public class SabrIborCapletFloorletVolatilityBootstrapDefinitionTest
	{

	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("TestName");

	  public virtual void test_ofFixedBeta()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, LINEAR);
		assertEquals(test.ExtrapolatorLeft, FLAT);
		assertEquals(test.ExtrapolatorRight, FLAT);
		assertEquals(test.Name, NAME);
		assertEquals(test.BetaCurve.get(), ConstantCurve.of(Curves.sabrParameterByExpiry(NAME.Name + "-Beta", ACT_ACT_ISDA, SABR_BETA), 0.5));
		assertFalse(test.RhoCurve.Present);
		assertEquals(test.SabrVolatilityFormula, SabrVolatilityFormula.hagan());
		assertEquals(test.ShiftCurve, ConstantCurve.of("Zero shift", 0d));
	  }

	  public virtual void test_ofFixedBeta_shift()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, 0.01, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, LINEAR);
		assertEquals(test.ExtrapolatorLeft, FLAT);
		assertEquals(test.ExtrapolatorRight, FLAT);
		assertEquals(test.Name, NAME);
		assertEquals(test.BetaCurve.get(), ConstantCurve.of(Curves.sabrParameterByExpiry(NAME.Name + "-Beta", ACT_ACT_ISDA, SABR_BETA), 0.5));
		assertFalse(test.RhoCurve.Present);
		assertEquals(test.SabrVolatilityFormula, SabrVolatilityFormula.hagan());
		assertEquals(test.ShiftCurve, ConstantCurve.of("Shift curve", 0.01));
	  }

	  public virtual void test_ofFixedRho()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, LINEAR);
		assertEquals(test.ExtrapolatorLeft, FLAT);
		assertEquals(test.ExtrapolatorRight, FLAT);
		assertEquals(test.Name, NAME);
		assertEquals(test.RhoCurve.get(), ConstantCurve.of(Curves.sabrParameterByExpiry(NAME.Name + "-Rho", ACT_ACT_ISDA, SABR_RHO), 0.5));
		assertFalse(test.BetaCurve.Present);
		assertEquals(test.SabrVolatilityFormula, SabrVolatilityFormula.hagan());
		assertEquals(test.ShiftCurve, ConstantCurve.of("Zero shift", 0d));
	  }

	  public virtual void test_ofFixedRho_shift()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedRho(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, 0.01, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, LINEAR);
		assertEquals(test.ExtrapolatorLeft, FLAT);
		assertEquals(test.ExtrapolatorRight, FLAT);
		assertEquals(test.Name, NAME);
		assertEquals(test.RhoCurve.get(), ConstantCurve.of(Curves.sabrParameterByExpiry(NAME.Name + "-Rho", ACT_ACT_ISDA, SABR_RHO), 0.5));
		assertFalse(test.BetaCurve.Present);
		assertEquals(test.SabrVolatilityFormula, SabrVolatilityFormula.hagan());
		assertEquals(test.ShiftCurve, ConstantCurve.of("Shift curve", 0.01));
	  }

	  public virtual void test_builder()
	  {
		Curve betaCurve = ConstantCurve.of(Curves.sabrParameterByExpiry(NAME.Name + "-Beta", ACT_ACT_ISDA, SABR_BETA), 0.65);
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.builder().index(USD_LIBOR_3M).name(NAME).interpolator(LINEAR).extrapolatorLeft(FLAT).extrapolatorRight(CurveExtrapolators.LINEAR).dayCount(ACT_ACT_ISDA).sabrVolatilityFormula(SabrVolatilityFormula.hagan()).betaCurve(betaCurve).build();
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, LINEAR);
		assertEquals(test.ExtrapolatorLeft, FLAT);
		assertEquals(test.ExtrapolatorRight, CurveExtrapolators.LINEAR);
		assertEquals(test.Name, NAME);
		assertEquals(test.BetaCurve.get(), betaCurve);
		assertFalse(test.RhoCurve.Present);
		assertEquals(test.SabrVolatilityFormula, SabrVolatilityFormula.hagan());
		assertEquals(test.ShiftCurve, ConstantCurve.of("Zero shift", 0d));
	  }

	  public virtual void test_createCap()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition @base = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, STEP_UPPER, FLAT, FLAT, SabrVolatilityFormula.hagan());
		LocalDate startDate = LocalDate.of(2012, 4, 20);
		LocalDate endDate = LocalDate.of(2017, 4, 20);
		double strike = 0.01;
		IborCapFloorLeg expected = IborCapFloorLeg.builder().calculation(IborRateCalculation.of(USD_LIBOR_3M)).capSchedule(ValueSchedule.of(strike)).currency(USD_LIBOR_3M.Currency).notional(ValueSchedule.ALWAYS_1).paymentDateOffset(DaysAdjustment.NONE).paymentSchedule(PeriodicSchedule.of(startDate, endDate, Frequency.of(USD_LIBOR_3M.Tenor.Period), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, USD_LIBOR_3M.FixingCalendar), StubConvention.NONE, RollConventions.NONE)).payReceive(PayReceive.RECEIVE).build();
		IborCapFloorLeg computed = @base.createCap(startDate, endDate, strike);
		assertEquals(computed, expected);
	  }

	  public virtual void test_createSabrParameterMetadata()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition @base = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		ImmutableList<CurveMetadata> expected = ImmutableList.of(Curves.sabrParameterByExpiry(NAME.Name + "-Alpha", ACT_ACT_ISDA, SABR_ALPHA), Curves.sabrParameterByExpiry(NAME.Name + "-Beta", ACT_ACT_ISDA, SABR_BETA), Curves.sabrParameterByExpiry(NAME.Name + "-Rho", ACT_ACT_ISDA, SABR_RHO), Curves.sabrParameterByExpiry(NAME.Name + "-Nu", ACT_ACT_ISDA, SABR_NU));
		ImmutableList<CurveMetadata> computed = @base.createSabrParameterMetadata();
		assertEquals(computed, expected);
	  }

	  public virtual void test_createMetadata_normal()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition @base = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		RawOptionData capData = RawOptionData.of(ImmutableList.of(Period.ofYears(1), Period.ofYears(5)), DoubleArray.of(0.005, 0.01, 0.015), ValueType.STRIKE, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.15, 0.12, 0.13},
			new double[] {0.1, Double.NaN, 0.09}
		}), ValueType.NORMAL_VOLATILITY);
		SurfaceMetadata expected = Surfaces.normalVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA);
		SurfaceMetadata computed = @base.createMetadata(capData);
		assertEquals(computed, expected);
	  }

	  public virtual void test_createMetadata_black()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition @base = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		RawOptionData capData = RawOptionData.of(ImmutableList.of(Period.ofYears(1), Period.ofYears(5)), DoubleArray.of(0.005, 0.01, 0.015), ValueType.STRIKE, DoubleMatrix.copyOf(new double[][]
		{
			new double[] {0.15, 0.12, 0.13},
			new double[] {0.1, 0.08, 0.09}
		}), ValueType.BLACK_VOLATILITY);
		SurfaceMetadata expected = Surfaces.blackVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA);
		SurfaceMetadata computed = @base.createMetadata(capData);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_wrongInterpolator()
	  {
		assertThrowsIllegalArg(() => SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, DOUBLE_QUADRATIC, FLAT, FLAT, SabrVolatilityFormula.hagan()));

	  }

	  public virtual void test_createMetadata_wrongValueType()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition @base = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
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
		SabrIborCapletFloorletVolatilityBootstrapDefinition test1 = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		coverImmutableBean(test1);
		SabrIborCapletFloorletVolatilityBootstrapDefinition test2 = SabrIborCapletFloorletVolatilityBootstrapDefinition.builder().index(GBP_LIBOR_3M).name(IborCapletFloorletVolatilitiesName.of("other")).interpolator(STEP_UPPER).extrapolatorLeft(FLAT).extrapolatorRight(CurveExtrapolators.LINEAR).rhoCurve(ConstantCurve.of("rho", 0.1d)).shiftCurve(ConstantCurve.of("shift", 0.01d)).dayCount(ACT_365F).sabrVolatilityFormula(SabrVolatilityFormula.hagan()).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SabrIborCapletFloorletVolatilityBootstrapDefinition test = SabrIborCapletFloorletVolatilityBootstrapDefinition.ofFixedBeta(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, 0.5, LINEAR, FLAT, FLAT, SabrVolatilityFormula.hagan());
		assertSerialization(test);
	  }

	}

}