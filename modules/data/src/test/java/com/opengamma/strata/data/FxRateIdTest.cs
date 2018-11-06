/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
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
//	import static org.testng.Assert.assertEquals;

	using Bean = org.joda.beans.Bean;
	using MetaBean = org.joda.beans.MetaBean;
	using Test = org.testng.annotations.Test;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;

	/// <summary>
	/// Test <seealso cref="FxRateId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateIdTest
	public class FxRateIdTest
	{

	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Test");
	  private static readonly CurrencyPair PAIR = CurrencyPair.of(GBP, USD);
	  private static readonly CurrencyPair INVERSE = PAIR.inverse();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_pair()
	  {
		FxRateId test = FxRateId.of(PAIR);
		FxRateId inverse = FxRateId.of(INVERSE);
		assertEquals(test.Pair, PAIR);
		assertEquals(inverse.Pair, PAIR);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(FxRate));
		assertEquals(test.ToString(), "FxRateId:GBP/USD");
	  }

	  public virtual void test_of_currencies()
	  {
		FxRateId test = FxRateId.of(GBP, USD);
		FxRateId inverse = FxRateId.of(USD, GBP);
		assertEquals(test.Pair, PAIR);
		assertEquals(inverse.Pair, PAIR);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(FxRate));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_pairAndSource()
	  {
		FxRateId test = FxRateId.of(PAIR, OBS_SOURCE);
		FxRateId inverse = FxRateId.of(INVERSE);
		assertEquals(test.Pair, PAIR);
		assertEquals(inverse.Pair, PAIR);
		assertEquals(test.ObservableSource, OBS_SOURCE);
		assertEquals(test.MarketDataType, typeof(FxRate));
	  }

	  public virtual void test_of_currenciesAndSource()
	  {
		FxRateId test = FxRateId.of(GBP, USD, OBS_SOURCE);
		FxRateId inverse = FxRateId.of(USD, GBP);
		assertEquals(test.Pair, PAIR);
		assertEquals(inverse.Pair, PAIR);
		assertEquals(test.ObservableSource, OBS_SOURCE);
		assertEquals(test.MarketDataType, typeof(FxRate));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxRateId test = FxRateId.of(GBP, USD);
		coverImmutableBean(test);
		FxRateId test2 = FxRateId.of(EUR, CHF, OBS_SOURCE);
		coverBeanEquals(test, test2);
	  }

	  public virtual void coverage_builder()
	  {
		MetaBean meta = MetaBean.of(typeof(FxRateId));
		Bean test1 = meta.builder().set("pair", CurrencyPair.parse("EUR/GBP")).set("observableSource", OBS_SOURCE).build();
		Bean test2 = meta.builder().set("pair", CurrencyPair.parse("EUR/GBP")).set("observableSource", OBS_SOURCE).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxRateId test = FxRateId.of(GBP, USD);
		assertSerialization(test);
	  }

	}

}