using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCounts = com.opengamma.strata.basics.date.DayCounts;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using IborIndices = com.opengamma.strata.basics.index.IborIndices;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using FixedIborSwapCurveNode = com.opengamma.strata.market.curve.node.FixedIborSwapCurveNode;
	using FraCurveNode = com.opengamma.strata.market.curve.node.FraCurveNode;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using FraTemplate = com.opengamma.strata.product.fra.type.FraTemplate;
	using FixedIborSwapConvention = com.opengamma.strata.product.swap.type.FixedIborSwapConvention;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;
	using FixedRateSwapLegConvention = com.opengamma.strata.product.swap.type.FixedRateSwapLegConvention;
	using IborRateSwapLegConvention = com.opengamma.strata.product.swap.type.IborRateSwapLegConvention;
	using ImmutableFixedIborSwapConvention = com.opengamma.strata.product.swap.type.ImmutableFixedIborSwapConvention;

	/// <summary>
	/// Helper methods for testing curves.
	/// </summary>
	internal sealed class CurveTestUtils
	{

	  private const string TEST_SCHEME = "test";

	  private static readonly BusinessDayAdjustment BDA_FOLLOW = BusinessDayAdjustment.of(FOLLOWING, GBLO);

	  private static readonly IborRateSwapLegConvention FLOATING_CONVENTION = IborRateSwapLegConvention.of(IborIndices.USD_LIBOR_3M);

	  private static readonly FixedRateSwapLegConvention FIXED_CONVENTION = FixedRateSwapLegConvention.of(Currency.USD, DayCounts.ACT_360, Frequency.P6M, BDA_FOLLOW);

	  private static readonly FixedIborSwapConvention SWAP_CONVENTION = ImmutableFixedIborSwapConvention.of("USD-Swap", FIXED_CONVENTION, FLOATING_CONVENTION);

	  private CurveTestUtils()
	  {
	  }

	  internal static InterpolatedNodalCurveDefinition fraCurveDefinition()
	  {
		string fra1x4 = "fra1x4";
		string fra2x5 = "fra2x5";
		string fra3x6 = "fra3x6";
		string fra6x9 = "fra6x9";
		string fra9x12 = "fra9x12";
		string fra12x15 = "fra12x15";
		string fra18x21 = "fra18x21";

		FraCurveNode fra1x4Node = fraNode(1, fra1x4);
		FraCurveNode fra2x5Node = fraNode(2, fra2x5);
		FraCurveNode fra3x6Node = fraNode(3, fra3x6);
		FraCurveNode fra6x9Node = fraNode(6, fra6x9);
		FraCurveNode fra9x12Node = fraNode(9, fra9x12);
		FraCurveNode fra12x15Node = fraNode(12, fra12x15);
		FraCurveNode fra18x21Node = fraNode(18, fra18x21);

		CurveName curveName = CurveName.of("FRA Curve");

		IList<CurveNode> nodes = ImmutableList.of(fra1x4Node, fra2x5Node, fra3x6Node, fra6x9Node, fra9x12Node, fra12x15Node, fra18x21Node);

		return InterpolatedNodalCurveDefinition.builder().name(curveName).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(DayCounts.ACT_ACT_ISDA).nodes(nodes).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
	  }

	  internal static InterpolatedNodalCurveDefinition fraSwapCurveDefinition()
	  {
		string fra3x6 = "fra3x6";
		string fra6x9 = "fra6x9";
		string swap1y = "swap1y";
		string swap2y = "swap2y";
		string swap3y = "swap3y";

		FraCurveNode fra3x6Node = CurveTestUtils.fraNode(3, fra3x6);
		FraCurveNode fra6x9Node = CurveTestUtils.fraNode(6, fra6x9);
		FixedIborSwapCurveNode swap1yNode = fixedIborSwapNode(Tenor.TENOR_1Y, swap1y);
		FixedIborSwapCurveNode swap2yNode = fixedIborSwapNode(Tenor.TENOR_2Y, swap2y);
		FixedIborSwapCurveNode swap3yNode = fixedIborSwapNode(Tenor.TENOR_3Y, swap3y);

		CurveName curveName = CurveName.of("FRA and Fixed-Float Swap Curve");
		IList<CurveNode> nodes = ImmutableList.of(fra3x6Node, fra6x9Node, swap1yNode, swap2yNode, swap3yNode);

		return InterpolatedNodalCurveDefinition.builder().name(curveName).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(DayCounts.ACT_ACT_ISDA).nodes(nodes).interpolator(CurveInterpolators.DOUBLE_QUADRATIC).extrapolatorLeft(CurveExtrapolators.FLAT).extrapolatorRight(CurveExtrapolators.FLAT).build();
	  }

	  internal static FraCurveNode fraNode(int startMonths, string id)
	  {
		Period periodToStart = Period.ofMonths(startMonths);
		QuoteId quoteId = QuoteId.of(StandardId.of(TEST_SCHEME, id));
		return FraCurveNode.of(FraTemplate.of(periodToStart, IborIndices.USD_LIBOR_3M), quoteId);
	  }

	  internal static FixedIborSwapCurveNode fixedIborSwapNode(Tenor tenor, string id)
	  {
		QuoteId quoteId = QuoteId.of(StandardId.of(TEST_SCHEME, id));
		FixedIborSwapTemplate template = FixedIborSwapTemplate.of(Period.ZERO, tenor, SWAP_CONVENTION);
		return FixedIborSwapCurveNode.of(template, quoteId);
	  }

	  internal static ObservableId id(string nodeName)
	  {
		return QuoteId.of(StandardId.of(TEST_SCHEME, nodeName));
	  }

	  internal static ObservableId key(CurveNode node)
	  {
		if (node is FraCurveNode)
		{
		  return ((FraCurveNode) node).RateId;
		}
		else if (node is FixedIborSwapCurveNode)
		{
		  return ((FixedIborSwapCurveNode) node).RateId;
		}
		else
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("Unsupported node type " + node.GetType().FullName);
		}
	  }
	}

}