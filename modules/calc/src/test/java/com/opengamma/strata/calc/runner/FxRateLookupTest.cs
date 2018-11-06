/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using FxMatrixId = com.opengamma.strata.data.FxMatrixId;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using ImmutableMarketData = com.opengamma.strata.data.ImmutableMarketData;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// Test <seealso cref="FxRateLookup"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateLookupTest
	public class FxRateLookupTest
	{

	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");
	  private static readonly LocalDate VAL_DATE = date(2016, 6, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_ofRates()
	  {
		FxRateLookup test = FxRateLookup.ofRates();
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxRateId.of(GBP, USD), FxRate.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  public virtual void test_ofRates_source()
	  {
		FxRateLookup test = FxRateLookup.ofRates(OBS_SOURCE);
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxRateId.of(GBP, USD, OBS_SOURCE), FxRate.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  public virtual void test_ofRates_currency()
	  {
		FxRateLookup test = FxRateLookup.ofRates(EUR);
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxRateId.of(GBP, USD), FxRate.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  public virtual void test_ofRates_currency_source()
	  {
		FxRateLookup test = FxRateLookup.ofRates(EUR, OBS_SOURCE);
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxRateId.of(GBP, USD, OBS_SOURCE), FxRate.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  public virtual void test_ofMatrix()
	  {
		FxRateLookup test = FxRateLookup.ofMatrix();
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxMatrixId.standard(), FxMatrix.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  public virtual void test_ofMatrix_source()
	  {
		FxRateLookup test = FxRateLookup.ofMatrix(FxMatrixId.of(OBS_SOURCE));
		MarketData marketData = ImmutableMarketData.builder(VAL_DATE).addValue(FxMatrixId.of(OBS_SOURCE), FxMatrix.of(GBP, USD, 1.5d)).build();

		assertEquals(test.fxRateProvider(marketData).fxRate(GBP, USD), 1.5d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage_rates()
	  {
		FxRateLookup test = FxRateLookup.ofRates();
		coverImmutableBean((ImmutableBean) test);
		FxRateLookup test2 = FxRateLookup.ofRates(EUR);
		coverBeanEquals((ImmutableBean) test, (ImmutableBean) test2);
	  }

	  public virtual void coverage_matrix()
	  {
		FxRateLookup test = FxRateLookup.ofMatrix();
		coverImmutableBean((ImmutableBean) test);
		FxRateLookup test2 = FxRateLookup.ofMatrix(FxMatrixId.of(OBS_SOURCE));
		coverBeanEquals((ImmutableBean) test, (ImmutableBean) test2);
	  }

	  public virtual void test_serialization()
	  {
		FxRateLookup test1 = FxRateLookup.ofRates();
		assertSerialization((ImmutableBean) test1);
		FxRateLookup test2 = FxRateLookup.ofMatrix();
		assertSerialization((ImmutableBean) test2);
	  }

	}

}