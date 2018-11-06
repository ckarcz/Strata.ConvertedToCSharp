/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.fra.FraDummyData.FRA_TRADE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using RatesProviderDataSets = com.opengamma.strata.pricer.datasets.RatesProviderDataSets;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;
	using ResolvedFra = com.opengamma.strata.product.fra.ResolvedFra;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="DiscountingFraTradePricer"/>.
	/// <para>
	/// Some of the methods in the trade pricer are comparable to the product pricer methods, thus tested in  
	/// <seealso cref="DiscountingFraProductPricerTest"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingFraTradePricerTest
	public class DiscountingFraTradePricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2014, 1, 22);
	  private const double DISCOUNT_FACTOR = 0.98d;
	  private const double FORWARD_RATE = 0.02;
	  private static readonly DiscountingFraProductPricer PRICER_PRODUCT = DiscountingFraProductPricer.DEFAULT;
	  private static readonly DiscountingFraTradePricer PRICER_TRADE = new DiscountingFraTradePricer(PRICER_PRODUCT);

	  private static readonly ResolvedFraTrade RFRA_TRADE = FRA_TRADE.resolve(REF_DATA);
	  private static readonly ResolvedFra RFRA = FRA.resolve(REF_DATA);
	  private static readonly SimpleRatesProvider RATES_PROVIDER;
	  static DiscountingFraTradePricerTest()
	  {
		DiscountFactors mockDf = mock(typeof(DiscountFactors));
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		RATES_PROVIDER = new SimpleRatesProvider(VAL_DATE, mockDf);
		RATES_PROVIDER.IborRates = mockIbor;
		IborIndexObservation obs = ((IborRateComputation) RFRA.FloatingRate).Observation;
		IborRateSensitivity sens = IborRateSensitivity.of(obs, 1d);
		when(mockIbor.ratePointSensitivity(obs)).thenReturn(sens);
		when(mockIbor.rate(obs)).thenReturn(FORWARD_RATE);
		when(mockDf.discountFactor(RFRA.PaymentDate)).thenReturn(DISCOUNT_FACTOR);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getters()
	  {
		assertEquals(DiscountingFraTradePricer.DEFAULT.ProductPricer, DiscountingFraProductPricer.DEFAULT);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_currencyExposure()
	  {
		assertEquals(PRICER_TRADE.currencyExposure(RFRA_TRADE, RATES_PROVIDER), MultiCurrencyAmount.of(PRICER_TRADE.presentValue(RFRA_TRADE, RATES_PROVIDER)));
	  }

	  public virtual void test_currentCash_zero()
	  {
		assertEquals(PRICER_TRADE.currentCash(RFRA_TRADE, RATES_PROVIDER), CurrencyAmount.zero(FRA.Currency));
	  }

	  public virtual void test_currentCash_onPaymentDate()
	  {
		LocalDate paymentDate = RFRA.PaymentDate;
		double publishedRate = 0.025;
		ResolvedFraTrade trade = FraTrade.builder().info(TradeInfo.builder().tradeDate(paymentDate).build()).product(FRA).build().resolve(REF_DATA);
		ImmutableRatesProvider ratesProvider = RatesProviderDataSets.multiGbp(paymentDate).toBuilder().timeSeries(GBP_LIBOR_3M, LocalDateDoubleTimeSeries.of(paymentDate, publishedRate)).build();
		assertEquals(PRICER_TRADE.currentCash(trade, ratesProvider), CurrencyAmount.of(FRA.Currency, (publishedRate - FRA.FixedRate) / (1d + publishedRate * RFRA.YearFraction) * RFRA.YearFraction * RFRA.Notional));
	  }

	}

}