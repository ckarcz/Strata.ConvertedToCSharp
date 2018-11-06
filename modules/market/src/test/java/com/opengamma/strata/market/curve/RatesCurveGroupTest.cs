using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.CHF_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_1M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_2M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.EUR_EONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;

	/// <summary>
	/// Test <seealso cref="RatesCurveGroup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveGroupTest
	public class RatesCurveGroupTest
	{

	  private static readonly CurveGroupName NAME = CurveGroupName.of("TestGroup");
	  private static readonly CurveGroupName NAME2 = CurveGroupName.of("TestGroup2");
	  private static readonly CurveName DISCOUNT_NAME = CurveName.of("Discount");
	  private static readonly CurveName IBOR_NAME = CurveName.of("Ibor");
	  private static readonly CurveName OVERNIGHT_NAME = CurveName.of("Overnight");
	  private static readonly Curve DISCOUNT_CURVE = ConstantCurve.of("Discount", 0.99);
	  private static readonly IDictionary<Currency, Curve> DISCOUNT_CURVES = ImmutableMap.of(GBP, DISCOUNT_CURVE);
	  private static readonly Curve IBOR_CURVE = ConstantCurve.of("Ibor", 0.5);
	  private static readonly Curve OVERNIGHT_CURVE = ConstantCurve.of("Overnight", 0.6);
	  private static readonly IDictionary<Index, Curve> IBOR_CURVES = ImmutableMap.of(GBP_LIBOR_3M, IBOR_CURVE);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		RatesCurveGroup test = RatesCurveGroup.of(NAME, DISCOUNT_CURVES, IBOR_CURVES);
		assertThat(test.Name).isEqualTo(NAME);
		assertThat(test.DiscountCurves).isEqualTo(DISCOUNT_CURVES);
		assertThat(test.ForwardCurves).isEqualTo(IBOR_CURVES);
		assertThat(test.findCurve(DISCOUNT_NAME)).hasValue(DISCOUNT_CURVE);
		assertThat(test.findCurve(IBOR_NAME)).hasValue(IBOR_CURVE);
		assertThat(test.findCurve(OVERNIGHT_NAME)).Empty;
		assertThat(test.findDiscountCurve(GBP)).hasValue(DISCOUNT_CURVE);
		assertThat(test.findDiscountCurve(USD)).Empty;
		assertThat(test.findForwardCurve(GBP_LIBOR_3M)).hasValue(IBOR_CURVE);
		assertThat(test.findForwardCurve(CHF_LIBOR_3M)).Empty;
	  }

	  public virtual void test_builder()
	  {
		RatesCurveGroup test = RatesCurveGroup.builder().name(NAME).discountCurves(DISCOUNT_CURVES).forwardCurves(IBOR_CURVES).build();
		assertThat(test.Name).isEqualTo(NAME);
		assertThat(test.DiscountCurves).isEqualTo(DISCOUNT_CURVES);
		assertThat(test.ForwardCurves).isEqualTo(IBOR_CURVES);
		assertThat(test.findDiscountCurve(GBP)).hasValue(DISCOUNT_CURVE);
		assertThat(test.findDiscountCurve(USD)).Empty;
		assertThat(test.findForwardCurve(GBP_LIBOR_3M)).hasValue(IBOR_CURVE);
		assertThat(test.findForwardCurve(CHF_LIBOR_3M)).Empty;
	  }

	  public virtual void test_ofCurves()
	  {
		RatesCurveGroupDefinition definition = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("group")).addCurve(DISCOUNT_NAME, GBP, GBP_LIBOR_1M).addForwardCurve(IBOR_NAME, USD_LIBOR_1M, USD_LIBOR_2M).addForwardCurve(OVERNIGHT_NAME, EUR_EONIA).build();
		RatesCurveGroup group = RatesCurveGroup.ofCurves(definition, DISCOUNT_CURVE, OVERNIGHT_CURVE, IBOR_CURVE);
		assertThat(group.findDiscountCurve(GBP)).hasValue(DISCOUNT_CURVE);
		assertThat(group.findForwardCurve(USD_LIBOR_1M)).hasValue(IBOR_CURVE);
		assertThat(group.findForwardCurve(USD_LIBOR_2M)).hasValue(IBOR_CURVE);
		assertThat(group.findForwardCurve(EUR_EONIA)).hasValue(OVERNIGHT_CURVE);
	  }

	  public virtual void test_ofCurves_duplicateCurveName()
	  {
		RatesCurveGroupDefinition definition = RatesCurveGroupDefinition.builder().name(CurveGroupName.of("group")).addForwardCurve(IBOR_NAME, USD_LIBOR_1M, USD_LIBOR_2M).build();
		RatesCurveGroup group = RatesCurveGroup.ofCurves(definition, IBOR_CURVE, IBOR_CURVE);
		assertThat(group.findForwardCurve(USD_LIBOR_1M)).hasValue(IBOR_CURVE);
		assertThat(group.findForwardCurve(USD_LIBOR_2M)).hasValue(IBOR_CURVE);
	  }

	  public virtual void stream()
	  {
		RatesCurveGroup test = RatesCurveGroup.of(NAME, DISCOUNT_CURVES, IBOR_CURVES);
		IList<Curve> expected = ImmutableList.builder<Curve>().addAll(DISCOUNT_CURVES.Values).addAll(IBOR_CURVES.Values).build();
		assertThat(test.ToList()).containsOnlyElementsOf(expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatesCurveGroup test = RatesCurveGroup.of(NAME, DISCOUNT_CURVES, IBOR_CURVES);
		coverImmutableBean(test);
		RatesCurveGroup test2 = RatesCurveGroup.of(NAME2, ImmutableMap.of(), ImmutableMap.of());
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatesCurveGroup test = RatesCurveGroup.of(NAME, DISCOUNT_CURVES, IBOR_CURVES);
		assertSerialization(test);
	  }

	}

}