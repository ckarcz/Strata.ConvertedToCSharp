/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using HullWhiteOneFactorPiecewiseConstantInterestRateModel = com.opengamma.strata.pricer.impl.rate.model.HullWhiteOneFactorPiecewiseConstantInterestRateModel;

	/// <summary>
	/// Test <seealso cref="HullWhiteOneFactorPiecewiseConstantParametersProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class HullWhiteOneFactorPiecewiseConstantParametersProviderTest
	public class HullWhiteOneFactorPiecewiseConstantParametersProviderTest
	{

	  private const double MEAN_REVERSION = 0.01;
	  private static readonly DoubleArray VOLATILITY = DoubleArray.of(0.01, 0.011, 0.012, 0.013, 0.014);
	  private static readonly DoubleArray VOLATILITY_TIME = DoubleArray.of(0.5, 1.0, 2.0, 5.0);
	  private static readonly HullWhiteOneFactorPiecewiseConstantParameters PARAMETERS = HullWhiteOneFactorPiecewiseConstantParameters.of(MEAN_REVERSION, VOLATILITY, VOLATILITY_TIME);
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2015, 2, 14);
	  private static readonly LocalTime TIME = LocalTime.of(14, 0x0);
	  private static readonly ZoneId ZONE = ZoneId.of("GMT+05");
	  private static readonly ZonedDateTime DATE_TIME = VAL_DATE.atTime(TIME).atZone(ZONE);

	  public virtual void test_of_ZonedDateTime()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider test = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.Parameters, PARAMETERS);
		assertEquals(test.ValuationDateTime, DATE_TIME);
	  }

	  public virtual void test_of_LocalDateAndTime()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider test = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, VAL_DATE, TIME, ZONE);
		assertEquals(test.DayCount, ACT_360);
		assertEquals(test.Parameters, PARAMETERS);
		assertEquals(test.ValuationDateTime, VAL_DATE.atTime(TIME).atZone(ZONE));
	  }

	  public virtual void test_futuresConvexityFactor()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider provider = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		LocalDate data1 = LocalDate.of(2015, 5, 14);
		LocalDate data2 = LocalDate.of(2015, 5, 20);
		LocalDate data3 = LocalDate.of(2015, 8, 20);
		double computed = provider.futuresConvexityFactor(data1, data2, data3);
		double expected = HullWhiteOneFactorPiecewiseConstantInterestRateModel.DEFAULT.futuresConvexityFactor(PARAMETERS, ACT_360.relativeYearFraction(VAL_DATE, data1), ACT_360.relativeYearFraction(VAL_DATE, data2), ACT_360.relativeYearFraction(VAL_DATE, data3));
		assertEquals(computed, expected);
	  }

	  public virtual void test_futuresConvexityFactorAdjoint()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider provider = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		LocalDate data1 = LocalDate.of(2015, 5, 14);
		LocalDate data2 = LocalDate.of(2015, 5, 20);
		LocalDate data3 = LocalDate.of(2015, 8, 20);
		ValueDerivatives computed = provider.futuresConvexityFactorAdjoint(data1, data2, data3);
		ValueDerivatives expected = HullWhiteOneFactorPiecewiseConstantInterestRateModel.DEFAULT.futuresConvexityFactorAdjoint(PARAMETERS, ACT_360.relativeYearFraction(VAL_DATE, data1), ACT_360.relativeYearFraction(VAL_DATE, data2), ACT_360.relativeYearFraction(VAL_DATE, data3));
		assertEquals(computed, expected);
	  }

	  public virtual void test_alpha()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider provider = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		LocalDate data1 = LocalDate.of(2015, 5, 20);
		LocalDate data2 = LocalDate.of(2015, 8, 20);
		LocalDate data3 = LocalDate.of(2015, 8, 20);
		LocalDate data4 = LocalDate.of(2015, 8, 27);
		double computed = provider.alpha(data1, data2, data3, data4);
		double expected = HullWhiteOneFactorPiecewiseConstantInterestRateModel.DEFAULT.alpha(PARAMETERS, ACT_360.relativeYearFraction(VAL_DATE, data1), ACT_360.relativeYearFraction(VAL_DATE, data2), ACT_360.relativeYearFraction(VAL_DATE, data3), ACT_360.relativeYearFraction(VAL_DATE, data4));
		assertEquals(computed, expected);
	  }

	  public virtual void test_alphaAdjoint()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider provider = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		LocalDate data1 = LocalDate.of(2015, 5, 20);
		LocalDate data2 = LocalDate.of(2015, 8, 20);
		LocalDate data3 = LocalDate.of(2015, 8, 20);
		LocalDate data4 = LocalDate.of(2015, 8, 27);
		ValueDerivatives computed = provider.alphaAdjoint(data1, data2, data3, data4);
		ValueDerivatives expected = HullWhiteOneFactorPiecewiseConstantInterestRateModel.DEFAULT.alphaAdjoint(PARAMETERS, ACT_360.relativeYearFraction(VAL_DATE, data1), ACT_360.relativeYearFraction(VAL_DATE, data2), ACT_360.relativeYearFraction(VAL_DATE, data3), ACT_360.relativeYearFraction(VAL_DATE, data4));
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider test1 = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		coverImmutableBean(test1);
		HullWhiteOneFactorPiecewiseConstantParameters @params = HullWhiteOneFactorPiecewiseConstantParameters.of(0.02, DoubleArray.of(0.01, 0.011, 0.014), DoubleArray.of(0.5, 5.0));
		HullWhiteOneFactorPiecewiseConstantParametersProvider test2 = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(@params, ACT_ACT_ISDA, DATE_TIME.plusDays(1));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		HullWhiteOneFactorPiecewiseConstantParametersProvider test = HullWhiteOneFactorPiecewiseConstantParametersProvider.of(PARAMETERS, ACT_360, DATE_TIME);
		assertSerialization(test);
	  }
	}

}