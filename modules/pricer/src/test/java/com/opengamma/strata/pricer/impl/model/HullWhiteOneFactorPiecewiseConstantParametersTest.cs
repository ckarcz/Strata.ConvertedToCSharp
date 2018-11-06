/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using HullWhiteOneFactorPiecewiseConstantParameters = com.opengamma.strata.pricer.model.HullWhiteOneFactorPiecewiseConstantParameters;

	/// <summary>
	/// Test <seealso cref="HullWhiteOneFactorPiecewiseConstantParameters"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteOneFactorPiecewiseConstantParametersTest
	public class HullWhiteOneFactorPiecewiseConstantParametersTest
	{

	  private const double MEAN_REVERSION = 0.01;
	  private static readonly DoubleArray VOLATILITY = DoubleArray.of(0.01, 0.011, 0.012, 0.013, 0.014);
	  private static readonly DoubleArray VOLATILITY_TIME = DoubleArray.of(0.5, 1.0, 2.0, 5.0);

	  public virtual void test_of()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters test = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		assertEquals(test.LastVolatility, VOLATILITY.get(VOLATILITY.size() - 1));
		assertEquals(test.MeanReversion, MEAN_REVERSION);
		assertEquals(test.Volatility, VOLATILITY);
		assertEquals(test.VolatilityTime, DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 1000d));
	  }

	  public virtual void test_of_noTime()
	  {
		double eta = 0.02;
		HullWhiteOneFactorPiecewiseConstantParameters test = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, DoubleArray.of(eta), DoubleArray.of());
		assertEquals(test.LastVolatility, eta);
		assertEquals(test.MeanReversion, MEAN_REVERSION);
		assertEquals(test.Volatility, DoubleArray.of(eta));
		assertEquals(test.VolatilityTime, DoubleArray.of(0d, 1000d));
	  }

	  public virtual void test_setVolatility()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters @base = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		DoubleArray newVol = DoubleArray.of(0.04, 0.012, 0.016, 0.019, 0.024);
		HullWhiteOneFactorPiecewiseConstantParameters test = @base.withVolatility(newVol);
		assertEquals(test.LastVolatility, newVol.get(newVol.size() - 1));
		assertEquals(test.MeanReversion, @base.MeanReversion);
		assertEquals(test.Volatility, newVol);
		assertEquals(test.VolatilityTime, @base.VolatilityTime);
	  }

	  public virtual void test_setLastVolatility()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters @base = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		double lastVol = 0.092;
		HullWhiteOneFactorPiecewiseConstantParameters test = @base.withLastVolatility(lastVol);
		assertEquals(test.LastVolatility, lastVol);
		assertEquals(test.MeanReversion, @base.MeanReversion);
		assertEquals(test.Volatility, DoubleArray.of(0.01, 0.011, 0.012, 0.013, lastVol));
		assertEquals(test.VolatilityTime, @base.VolatilityTime);
	  }

	  public virtual void test_addVolatility()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters @base = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		double time = 7.0;
		double vol = 0.015;
		HullWhiteOneFactorPiecewiseConstantParameters test = @base.withVolatilityAdded(vol, time);
		assertEquals(test.LastVolatility, vol);
		assertEquals(test.MeanReversion, MEAN_REVERSION);
		assertEquals(test.Volatility, DoubleArray.of(0.01, 0.011, 0.012, 0.013, 0.014, vol));
		assertEquals(test.VolatilityTime, DoubleArray.of(0.0, 0.5, 1.0, 2.0, 5.0, 7.0, 1000d));
	  }

	  public virtual void test_of_notAscendingTime()
	  {
		DoubleArray time = DoubleArray.of(0.5, 1.0, 4.0, 2.0);
		assertThrowsIllegalArg(() => HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, time));
	  }

	  public virtual void test_of_notAscendingTime1()
	  {
		DoubleArray time = DoubleArray.of(0.5, 1.0, 4.0);
		assertThrowsIllegalArg(() => HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, time));
	  }

	  public virtual void test_addVolatility_notAscendingTime()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters @base = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		double time = 3.0;
		double vol = 0.015;
		assertThrowsIllegalArg(() => @base.withVolatilityAdded(vol, time));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters test1 = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		coverImmutableBean(test1);
		HullWhiteOneFactorPiecewiseConstantParameters test2 = HullWhiteOneFactorPiecewiseConstantParameters.of(0.02, DoubleArray.of(0.015), DoubleArray.of());
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		HullWhiteOneFactorPiecewiseConstantParameters test = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
		assertSerialization(test);
	  }
	}

}