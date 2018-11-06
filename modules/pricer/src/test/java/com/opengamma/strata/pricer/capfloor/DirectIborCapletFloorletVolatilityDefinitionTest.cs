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
//	import static com.opengamma.strata.market.ValueType.BLACK_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.STRIKE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using PenaltyMatrixGenerator = com.opengamma.strata.math.impl.interpolation.PenaltyMatrixGenerator;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;

	/// <summary>
	/// Test <seealso cref="DirectIborCapletFloorletVolatilityDefinition"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DirectIborCapletFloorletVolatilityDefinitionTest
	public class DirectIborCapletFloorletVolatilityDefinitionTest
	{

	  private static readonly IborCapletFloorletVolatilitiesName NAME = IborCapletFloorletVolatilitiesName.of("test");
	  private static readonly GridSurfaceInterpolator INTERPOLATOR = GridSurfaceInterpolator.of(CurveInterpolators.DOUBLE_QUADRATIC, CurveInterpolators.DOUBLE_QUADRATIC);
	  private static readonly Curve SHIFT_CURVE = ConstantCurve.of("Black Shift", 0.05);
	  private const double LAMBDA_EXPIRY = 0.07;
	  private const double LAMBDA_STRIKE = 0.05;
	  private static readonly ImmutableList<Period> EXPIRIES = ImmutableList.of(Period.ofYears(1), Period.ofYears(3));
	  private static readonly DoubleArray STRIKES = DoubleArray.of(0.01, 0.02, 0.03);
	  private static readonly DoubleMatrix DATA = DoubleMatrix.copyOf(new double[][]
	  {
		  new double[] {0.22, 0.18, 0.18},
		  new double[] {0.17, 0.15, 0.165}
	  });
	  private static readonly RawOptionData SAMPLE_BLACK = RawOptionData.of(EXPIRIES, STRIKES, STRIKE, DATA, BLACK_VOLATILITY);
	  private static readonly RawOptionData SAMPLE_NORMAL = RawOptionData.of(EXPIRIES, STRIKES, STRIKE, DATA, NORMAL_VOLATILITY);

	  public virtual void test_of()
	  {
		DirectIborCapletFloorletVolatilityDefinition test = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, INTERPOLATOR);
		assertEquals(test.LambdaExpiry, LAMBDA_EXPIRY);
		assertEquals(test.LambdaStrike, LAMBDA_STRIKE);
		assertEquals(test.Name, NAME);
		assertFalse(test.ShiftCurve.Present);
	  }

	  public virtual void test_of_shift()
	  {
		DirectIborCapletFloorletVolatilityDefinition test = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR, SHIFT_CURVE);
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, INTERPOLATOR);
		assertEquals(test.LambdaExpiry, LAMBDA_EXPIRY);
		assertEquals(test.LambdaStrike, LAMBDA_STRIKE);
		assertEquals(test.Name, NAME);
		assertEquals(test.ShiftCurve.get(), SHIFT_CURVE);
	  }

	  public virtual void test_builder()
	  {
		DirectIborCapletFloorletVolatilityDefinition test = DirectIborCapletFloorletVolatilityDefinition.builder().name(NAME).index(USD_LIBOR_3M).dayCount(ACT_ACT_ISDA).lambdaExpiry(LAMBDA_EXPIRY).lambdaStrike(LAMBDA_STRIKE).interpolator(INTERPOLATOR).shiftCurve(SHIFT_CURVE).build();
		assertEquals(test.DayCount, ACT_ACT_ISDA);
		assertEquals(test.Index, USD_LIBOR_3M);
		assertEquals(test.Interpolator, INTERPOLATOR);
		assertEquals(test.LambdaExpiry, LAMBDA_EXPIRY);
		assertEquals(test.LambdaStrike, LAMBDA_STRIKE);
		assertEquals(test.Name, NAME);
		assertEquals(test.ShiftCurve.get(), SHIFT_CURVE);
	  }

	  public virtual void test_createMetadata()
	  {
		DirectIborCapletFloorletVolatilityDefinition @base = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR);
		assertEquals(@base.createMetadata(SAMPLE_BLACK), Surfaces.blackVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA));
		assertEquals(@base.createMetadata(SAMPLE_NORMAL), Surfaces.normalVolatilityByExpiryStrike(NAME.Name, ACT_ACT_ISDA));
		assertThrowsIllegalArg(() => @base.createMetadata(RawOptionData.of(EXPIRIES, STRIKES, STRIKE, DATA, ValueType.PRICE)));
	  }

	  public virtual void test_computePenaltyMatrix()
	  {
		DirectIborCapletFloorletVolatilityDefinition @base = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR);
		DoubleArray strikes1 = DoubleArray.of(0.1);
		DoubleArray expiries1 = DoubleArray.of(1d, 2d, 5d);
		assertThrowsIllegalArg(() => @base.computePenaltyMatrix(strikes1, expiries1));
		DoubleArray strikes2 = DoubleArray.of(0.01, 0.05, 0.1);
		DoubleArray expiries2 = DoubleArray.of(2d);
		assertThrowsIllegalArg(() => @base.computePenaltyMatrix(strikes2, expiries2));
		DoubleArray strikes3 = DoubleArray.of(0.05, 0.1, 0.15);
		DoubleArray expiries3 = DoubleArray.of(1d, 2d, 5d);
		DoubleMatrix computed = @base.computePenaltyMatrix(strikes3, expiries3);
		DoubleMatrix expected = PenaltyMatrixGenerator.getPenaltyMatrix(new double[][] {expiries3.toArray(), strikes3.toArray()}, new int[] {2, 2}, new double[] {LAMBDA_EXPIRY, LAMBDA_STRIKE});
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DirectIborCapletFloorletVolatilityDefinition test1 = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR, SHIFT_CURVE);
		coverImmutableBean(test1);
		DirectIborCapletFloorletVolatilityDefinition test2 = DirectIborCapletFloorletVolatilityDefinition.of(IborCapletFloorletVolatilitiesName.of("other"), GBP_LIBOR_3M, ACT_365F, 0.01, 0.02, GridSurfaceInterpolator.of(CurveInterpolators.LINEAR, CurveInterpolators.LINEAR));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		DirectIborCapletFloorletVolatilityDefinition test = DirectIborCapletFloorletVolatilityDefinition.of(NAME, USD_LIBOR_3M, ACT_ACT_ISDA, LAMBDA_EXPIRY, LAMBDA_STRIKE, INTERPOLATOR, SHIFT_CURVE);
		assertSerialization(test);
	  }

	}

}