using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.sensitivity.CurveSensitivitiesType.ZERO_RATE_DELTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.sensitivity.CurveSensitivitiesType.ZERO_RATE_GAMMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.AttributeType.DESCRIPTION;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.AttributeType.NAME;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using TenorParameterMetadata = com.opengamma.strata.market.param.TenorParameterMetadata;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;
	using PortfolioItemSummary = com.opengamma.strata.product.PortfolioItemSummary;
	using PortfolioItemType = com.opengamma.strata.product.PortfolioItemType;
	using ProductType = com.opengamma.strata.product.ProductType;

	/// <summary>
	/// Test <seealso cref="CurveSensitivities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurveSensitivitiesTest
	public class CurveSensitivitiesTest
	{

	  private static readonly TenorParameterMetadata TENOR_MD_1M = TenorParameterMetadata.of(Tenor.TENOR_1M);
	  private static readonly TenorParameterMetadata TENOR_MD_1W = TenorParameterMetadata.of(Tenor.TENOR_1W);
	  private static readonly TenorParameterMetadata TENOR_MD_1Y = TenorParameterMetadata.of(Tenor.TENOR_1Y);
	  private static readonly TenorParameterMetadata TENOR_MD_2Y = TenorParameterMetadata.of(Tenor.TENOR_2Y);
	  private static readonly TenorParameterMetadata TENOR_MD_3Y = TenorParameterMetadata.of(Tenor.TENOR_3Y);
	  private static readonly TenorParameterMetadata TENOR_MD_4Y = TenorParameterMetadata.of(Tenor.TENOR_4Y);
	  private static readonly TenorParameterMetadata TENOR_MD_5Y = TenorParameterMetadata.of(Tenor.TENOR_5Y);

	  private static readonly DoubleArray VECTOR_USD1 = DoubleArray.of(100, 200, 300, 123);
	  private static readonly DoubleArray VECTOR_USD2 = DoubleArray.of(1000, 250, 321, 123);
	  private static readonly DoubleArray VECTOR_EUR1 = DoubleArray.of(1000, 250, 321, 123, 321);
	  private static readonly DoubleArray VECTOR_EUR1_IN_USD = DoubleArray.of(1000 * 1.6, 250 * 1.6, 321 * 1.6, 123 * 1.6, 321 * 1.6);
	  private static readonly Currency USD = Currency.USD;
	  private static readonly Currency EUR = Currency.EUR;
	  private static readonly FxRate FX_RATE = FxRate.of(EUR, USD, 1.6d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
	  private static readonly IList<ParameterMetadata> METADATA1 = ImmutableList.of(TENOR_MD_1Y, TENOR_MD_2Y, TENOR_MD_3Y, TENOR_MD_4Y);
	  private static readonly IList<ParameterMetadata> METADATA2 = ImmutableList.of(TENOR_MD_1Y, TENOR_MD_2Y, TENOR_MD_3Y, TENOR_MD_4Y, TENOR_MD_5Y);

	  private static readonly CurrencyParameterSensitivity ENTRY_USD = CurrencyParameterSensitivity.of(NAME1, METADATA1, USD, VECTOR_USD1);
	  private static readonly CurrencyParameterSensitivity ENTRY_USD2 = CurrencyParameterSensitivity.of(NAME1, METADATA1, USD, VECTOR_USD2);
	  private static readonly CurrencyParameterSensitivity ENTRY_EUR = CurrencyParameterSensitivity.of(NAME2, METADATA2, EUR, VECTOR_EUR1);
	  private static readonly CurrencyParameterSensitivity ENTRY_EUR_IN_USD = CurrencyParameterSensitivity.of(NAME2, METADATA2, USD, VECTOR_EUR1_IN_USD);

	  private static readonly StandardId ID2 = StandardId.of("A", "B");
	  private static readonly CurrencyParameterSensitivities SENSI1 = CurrencyParameterSensitivities.of(ENTRY_USD);
	  private static readonly CurrencyParameterSensitivities SENSI2 = CurrencyParameterSensitivities.of(ENTRY_USD2, ENTRY_EUR);
	  private static readonly PortfolioItemInfo INFO1 = PortfolioItemInfo.empty().withAttribute(DESCRIPTION, "1");
	  private static readonly PortfolioItemInfo INFO2 = PortfolioItemInfo.empty().withId(ID2).withAttribute(NAME, "2").withAttribute(DESCRIPTION, "2");

	  //-------------------------------------------------------------------------
	  public virtual void test_empty()
	  {
		CurveSensitivities test = CurveSensitivities.empty();
		assertEquals(test.Info, PortfolioItemInfo.empty());
		assertEquals(test.TypedSensitivities, ImmutableMap.of());
	  }

	  public virtual void test_of_single()
	  {
		CurveSensitivities test = sut();
		assertEquals(test.Id, null);
		assertEquals(test.Info, INFO1);
		assertEquals(test.TypedSensitivities, ImmutableMap.of(ZERO_RATE_DELTA, SENSI1));
		assertEquals(test.getTypedSensitivity(ZERO_RATE_DELTA), SENSI1);
		assertThrows(typeof(System.ArgumentException), () => test.getTypedSensitivity(ZERO_RATE_GAMMA));
		assertEquals(test.findTypedSensitivity(ZERO_RATE_DELTA), SENSI1);
		assertEquals(test.findTypedSensitivity(ZERO_RATE_GAMMA), null);
	  }

	  public virtual void test_of_map()
	  {
		CurveSensitivities test = sut2();
		assertEquals(test.Id, ID2);
		assertEquals(test.Info, INFO2);
		assertEquals(test.TypedSensitivities, ImmutableMap.of(ZERO_RATE_DELTA, SENSI1, ZERO_RATE_GAMMA, SENSI2));
		assertEquals(test.getTypedSensitivity(ZERO_RATE_DELTA), SENSI1);
		assertEquals(test.getTypedSensitivity(ZERO_RATE_GAMMA), SENSI2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_empty()
	  {
		CurveSensitivities test = CurveSensitivities.builder(PortfolioItemInfo.empty()).build();
		assertEquals(test.Info, PortfolioItemInfo.empty());
		assertEquals(test.TypedSensitivities, ImmutableMap.of());
	  }

	  public virtual void test_builder_tenors()
	  {
		CurveName curveName = CurveName.of("GBP");
		CurrencyParameterSensitivity sens1Y = CurrencyParameterSensitivity.of(curveName, ImmutableList.of(TENOR_MD_1Y), GBP, DoubleArray.of(3));
		CurveSensitivities test = CurveSensitivities.builder(PortfolioItemInfo.empty()).add(ZERO_RATE_DELTA, curveName, GBP, TENOR_MD_1M, 4).add(ZERO_RATE_DELTA, curveName, GBP, TENOR_MD_1W, 1).add(ZERO_RATE_DELTA, curveName, GBP, TENOR_MD_1Y, 2).add(ZERO_RATE_DELTA, curveName, GBP, TENOR_MD_1W, 2).add(ZERO_RATE_DELTA, sens1Y).build();
		assertEquals(test.Info, PortfolioItemInfo.empty());
		assertEquals(test.TypedSensitivities.size(), 1);
		CurrencyParameterSensitivities sens = test.getTypedSensitivity(ZERO_RATE_DELTA);
		assertEquals(sens.Sensitivities.size(), 1);
		CurrencyParameterSensitivity singleSens = sens.getSensitivity(curveName, GBP);
		assertEquals(singleSens.Sensitivity, DoubleArray.of(3, 4, 5));
		assertEquals(singleSens.getParameterMetadata(0), TENOR_MD_1W);
		assertEquals(singleSens.getParameterMetadata(1), TENOR_MD_1M);
		assertEquals(singleSens.getParameterMetadata(2), TENOR_MD_1Y);
	  }

	  public virtual void test_builder_mixCurrency()
	  {
		CurveName curveName = CurveName.of("WEIRD");
		CurveSensitivities test = CurveSensitivities.builder(PortfolioItemInfo.empty()).add(ZERO_RATE_DELTA, curveName, GBP, TENOR_MD_1Y, 1).add(ZERO_RATE_DELTA, curveName, USD, TENOR_MD_1Y, 2).build();
		assertEquals(test.Info, PortfolioItemInfo.empty());
		assertEquals(test.TypedSensitivities.size(), 1);
		CurrencyParameterSensitivities sens = test.getTypedSensitivity(ZERO_RATE_DELTA);
		assertEquals(sens.Sensitivities.size(), 2);
		CurrencyParameterSensitivity sensGbp = sens.getSensitivity(curveName, GBP);
		assertEquals(sensGbp.Sensitivity, DoubleArray.of(1));
		assertEquals(sensGbp.getParameterMetadata(0), TENOR_MD_1Y);
		CurrencyParameterSensitivity sensUsd = sens.getSensitivity(curveName, USD);
		assertEquals(sensUsd.Sensitivity, DoubleArray.of(2));
		assertEquals(sensUsd.getParameterMetadata(0), TENOR_MD_1Y);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mergedWith_map_empty()
	  {
		CurveSensitivities @base = sut();
		IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivities> additional = ImmutableMap.of();
		CurveSensitivities test = @base.mergedWith(additional);
		assertEquals(test, @base);
	  }

	  public virtual void test_mergedWith_map_mergeAndAdd()
	  {
		CurveSensitivities base1 = sut();
		CurveSensitivities base2 = sut2();
		CurveSensitivities test = base1.mergedWith(base2.TypedSensitivities);
		assertEquals(test.Info, base1.Info);
		assertEquals(test.TypedSensitivities.Keys, ImmutableSet.of(ZERO_RATE_DELTA, ZERO_RATE_GAMMA));
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_DELTA), SENSI1.multipliedBy(2));
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_GAMMA), SENSI2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mergedWith_sens_empty()
	  {
		CurveSensitivities @base = sut();
		CurveSensitivities test = @base.mergedWith(CurveSensitivities.empty());
		assertEquals(test, @base);
	  }

	  public virtual void test_mergedWith_sens_mergeAndAdd()
	  {
		CurveSensitivities base1 = sut();
		CurveSensitivities base2 = sut2();
		CurveSensitivities test = base1.mergedWith(base2);
		assertEquals(test.Info, PortfolioItemInfo.empty().withId(ID2).withAttribute(NAME, "2").withAttribute(DESCRIPTION, "1"));
		assertEquals(test.TypedSensitivities.Keys, ImmutableSet.of(ZERO_RATE_DELTA, ZERO_RATE_GAMMA));
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_DELTA), SENSI1.multipliedBy(2));
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_GAMMA), SENSI2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withMarketDataNames()
	  {
		CurveSensitivities @base = sut();
		CurveSensitivities test = @base.withMarketDataNames(name => NAME2);
		assertEquals(@base.TypedSensitivities.get(ZERO_RATE_DELTA).Sensitivities.get(0).MarketDataName, NAME1);
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_DELTA).Sensitivities.get(0).MarketDataName, NAME2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withParameterMetadatas()
	  {
		CurveSensitivities @base = sut();
		CurveSensitivities test = @base.withParameterMetadatas(md => TENOR_MD_1Y);
		CurrencyParameterSensitivity testSens = test.TypedSensitivities.get(ZERO_RATE_DELTA).Sensitivities.get(0);
		assertEquals(testSens.ParameterMetadata, ImmutableList.of(TENOR_MD_1Y));
		assertEquals(testSens.Sensitivity, DoubleArray.of(723));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo_singleCurrency()
	  {
		CurveSensitivities @base = sut();
		CurveSensitivities test = @base.convertedTo(USD, FxMatrix.empty());
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_DELTA).Sensitivities, ImmutableList.of(ENTRY_USD));
	  }

	  public virtual void test_convertedTo_multipleCurrency()
	  {
		CurveSensitivities @base = sut2();
		CurveSensitivities test = @base.convertedTo(USD, FX_RATE);
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_DELTA).Sensitivities, ImmutableList.of(ENTRY_USD));
		assertEquals(test.TypedSensitivities.get(ZERO_RATE_GAMMA).Sensitivities, ImmutableList.of(ENTRY_USD2, ENTRY_EUR_IN_USD));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_summarize()
	  {
		CurveSensitivities @base = sut2();
		PortfolioItemSummary test = @base.summarize();
		assertEquals(test.Id, ID2);
		assertEquals(test.PortfolioItemType, PortfolioItemType.SENSITIVITIES);
		assertEquals(test.ProductType, ProductType.SENSITIVITIES);
		assertEquals(test.Currencies, ImmutableSet.of(EUR, USD));
		assertEquals(test.Description, "CurveSensitivities[ZeroRateDelta, ZeroRateGamma]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  private CurveSensitivities sut()
	  {
		return CurveSensitivities.of(INFO1, ZERO_RATE_DELTA, SENSI1);
	  }

	  private CurveSensitivities sut2()
	  {
		return CurveSensitivities.of(INFO2, ImmutableMap.of(ZERO_RATE_DELTA, SENSI1, ZERO_RATE_GAMMA, SENSI2));
	  }

	}

}