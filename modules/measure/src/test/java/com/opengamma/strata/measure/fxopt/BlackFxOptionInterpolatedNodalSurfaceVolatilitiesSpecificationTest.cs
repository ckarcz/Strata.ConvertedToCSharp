using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.DOUBLE_QUADRATIC;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.PCHIP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using BlackFxOptionSurfaceVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSurfaceVolatilities;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using FxVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.fxopt.FxVolatilitySurfaceYearFractionParameterMetadata;

	/// <summary>
	/// Test <seealso cref="BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecificationTest
	public class BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecificationTest
	{

	  private static readonly FxOptionVolatilitiesName VOL_NAME = FxOptionVolatilitiesName.of("test");
	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CurrencyPair GBP_USD = CurrencyPair.of(GBP, USD);
	  private static readonly HolidayCalendarId NY_LO = USNY.combinedWith(GBLO);
	  private static readonly DaysAdjustment SPOT_OFFSET = DaysAdjustment.ofBusinessDays(2, NY_LO);
	  private static readonly BusinessDayAdjustment BDA = BusinessDayAdjustment.of(FOLLOWING, NY_LO);
	  private static readonly IList<Tenor> TENORS = ImmutableList.of(Tenor.TENOR_3M, Tenor.TENOR_6M, Tenor.TENOR_1Y);
	  private static readonly IList<double> STRIKES = ImmutableList.of(1.35, 1.5, 1.65, 1.7);
	  private static readonly ImmutableList<FxOptionVolatilitiesNode> NODES;
	  private static readonly ImmutableList<QuoteId> QUOTE_IDS;
	  static BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecificationTest()
	  {
		ImmutableList.Builder<FxOptionVolatilitiesNode> nodeBuilder = ImmutableList.builder();
		ImmutableList.Builder<QuoteId> quoteIdBuilder = ImmutableList.builder();
		for (int i = 0; i < TENORS.Count; ++i)
		{
		  for (int j = 0; j < STRIKES.Count; ++j)
		  {
			QuoteId quoteId = QuoteId.of(StandardId.of("OG", GBP_USD.ToString() + "_" + TENORS[i].ToString() + "_" + STRIKES[j]));
			nodeBuilder.add(FxOptionVolatilitiesNode.of(GBP_USD, SPOT_OFFSET, BDA, ValueType.BLACK_VOLATILITY, quoteId, TENORS[i], SimpleStrike.of(STRIKES[j])));
			quoteIdBuilder.add(quoteId);
		  }
		}
		NODES = nodeBuilder.build();
		QUOTE_IDS = quoteIdBuilder.build();
	  }

	  public virtual void test_builder()
	  {
		BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification test = BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(FLAT).strikeInterpolator(DOUBLE_QUADRATIC).strikeExtrapolatorLeft(FLAT).strikeExtrapolatorRight(LINEAR).build();
		assertEquals(test.CurrencyPair, GBP_USD);
		assertEquals(test.DayCount, ACT_365F);
		assertEquals(test.Name, VOL_NAME);
		assertEquals(test.Nodes, NODES);
		assertEquals(test.ParameterCount, NODES.size());
		assertEquals(test.StrikeInterpolator, DOUBLE_QUADRATIC);
		assertEquals(test.StrikeExtrapolatorLeft, FLAT);
		assertEquals(test.StrikeExtrapolatorRight, LINEAR);
		assertEquals(test.TimeInterpolator, PCHIP);
		assertEquals(test.TimeExtrapolatorLeft, LINEAR);
		assertEquals(test.TimeExtrapolatorRight, FLAT);
		assertEquals(test.volatilitiesInputs(), QUOTE_IDS);
	  }

	  public virtual void test_volatilities()
	  {
		BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification @base = BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(NODES).timeInterpolator(PCHIP).strikeInterpolator(DOUBLE_QUADRATIC).build();
		LocalDate date = LocalDate.of(2017, 9, 25);
		ZonedDateTime dateTime = date.atStartOfDay().atZone(ZoneId.of("Europe/London"));
		DoubleArray parameters = DoubleArray.of(0.19, 0.15, 0.13, 0.14, 0.14, 0.11, 0.09, 0.09, 0.11, 0.09, 0.07, 0.07);
		BlackFxOptionSurfaceVolatilities computed = @base.volatilities(dateTime, parameters, REF_DATA);
		DaysAdjustment expOffset = DaysAdjustment.ofBusinessDays(-2, NY_LO);
		double[] expiries = new double[STRIKES.Count * TENORS.Count];
		double[] strikes = new double[STRIKES.Count * TENORS.Count];
		ImmutableList.Builder<ParameterMetadata> paramMetadata = ImmutableList.builder();
		for (int i = 0; i < TENORS.Count; ++i)
		{
		  double expiry = ACT_365F.relativeYearFraction(date, expOffset.adjust(BDA.adjust(SPOT_OFFSET.adjust(date, REF_DATA).plus(TENORS[i]), REF_DATA), REF_DATA));
		  for (int j = 0; j < STRIKES.Count; ++j)
		  {
			paramMetadata.add(FxVolatilitySurfaceYearFractionParameterMetadata.of(expiry, SimpleStrike.of(STRIKES[j]), GBP_USD));
			expiries[STRIKES.Count * i + j] = expiry;
			strikes[STRIKES.Count * i + j] = STRIKES[j];
		  }
		}
		InterpolatedNodalSurface surface = InterpolatedNodalSurface.ofUnsorted(Surfaces.blackVolatilityByExpiryStrike(VOL_NAME.Name, ACT_365F).withParameterMetadata(paramMetadata.build()), DoubleArray.ofUnsafe(expiries), DoubleArray.ofUnsafe(strikes), parameters, GridSurfaceInterpolator.of(PCHIP, DOUBLE_QUADRATIC));
		BlackFxOptionSurfaceVolatilities expected = BlackFxOptionSurfaceVolatilities.of(VOL_NAME, GBP_USD, dateTime, surface);
		assertEquals(computed, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification test1 = BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(LINEAR).strikeInterpolator(PCHIP).strikeExtrapolatorLeft(LINEAR).strikeExtrapolatorRight(LINEAR).build();
		coverImmutableBean(test1);
		CurrencyPair eurUsd = CurrencyPair.of(EUR, USD);
		ImmutableList.Builder<FxOptionVolatilitiesNode> nodeBuilder = ImmutableList.builder();
		for (int i = 0; i < TENORS.Count; ++i)
		{
		  for (int j = 0; j < STRIKES.Count; ++j)
		  {
			QuoteId quoteId = QuoteId.of(StandardId.of("OG", eurUsd.ToString() + "_" + TENORS[i].ToString() + "_" + STRIKES[j]));
			nodeBuilder.add(FxOptionVolatilitiesNode.of(eurUsd, SPOT_OFFSET, BDA, ValueType.BLACK_VOLATILITY, quoteId, TENORS[i], SimpleStrike.of(STRIKES[j])));
		  }
		}
		BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification test2 = BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(FxOptionVolatilitiesName.of("other")).currencyPair(eurUsd).dayCount(ACT_360).nodes(nodeBuilder.build()).timeInterpolator(DOUBLE_QUADRATIC).strikeInterpolator(DOUBLE_QUADRATIC).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void serialization()
	  {
		BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification test = BlackFxOptionInterpolatedNodalSurfaceVolatilitiesSpecification.builder().name(VOL_NAME).currencyPair(GBP_USD).dayCount(ACT_365F).nodes(NODES).timeInterpolator(PCHIP).timeExtrapolatorLeft(LINEAR).timeExtrapolatorRight(FLAT).strikeInterpolator(DOUBLE_QUADRATIC).strikeExtrapolatorLeft(FLAT).strikeExtrapolatorRight(LINEAR).build();
		assertSerialization(test);
	  }

	}

}