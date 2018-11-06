using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using Cds = com.opengamma.strata.product.credit.Cds;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using ResolvedCdsTrade = com.opengamma.strata.product.credit.ResolvedCdsTrade;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;

	/// <summary>
	/// Test <seealso cref="CdsMarketQuoteConverter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsMarketQuoteConverterTest
	public class CdsMarketQuoteConverterTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly CdsMarketQuoteConverter CONV = CdsMarketQuoteConverter.DEFAULT;
	  private static readonly CdsMarketQuoteConverter CONV_MARKIT_FIX = new CdsMarketQuoteConverter(AccrualOnDefaultFormula.MARKIT_FIX);
	  private static readonly IsdaCompliantCreditCurveCalibrator CALIB = FastCreditCurveCalibrator.standard();

	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "AAB");
	  private static readonly HolidayCalendarId DEFAULT_CALENDAR = HolidayCalendarIds.SAT_SUN;
	  private static readonly LocalDate TODAY = LocalDate.of(2008, 9, 19);
	  private static readonly LocalDate START_DATE = LocalDate.of(2007, 3, 20);
	  private static readonly LocalDate END_DATE = LocalDate.of(2015, 12, 20);
	  private static readonly LocalDate[] MATURITIES = new LocalDate[] {LocalDate.of(2008, 12, 20), LocalDate.of(2009, 6, 20), LocalDate.of(2010, 6, 20), LocalDate.of(2011, 6, 20), LocalDate.of(2012, 6, 20), LocalDate.of(2014, 6, 20), LocalDate.of(2017, 6, 20)};
	  // yield curve
	  private static readonly DoubleArray DSC_TIME = DoubleArray.ofUnsafe(new double[] {0.09315068493150684, 0.18082191780821918, 0.2602739726027397, 0.5068493150684932, 0.7589041095890411, 1.010958904109589, 2.010958904109589, 3.010958904109589, 4.016438356164384, 5.013698630136987, 6.013698630136987, 7.013698630136987, 8.016438356164384, 9.021917808219179, 10.01917808219178, 11.016438356164384, 12.01917808219178, 15.024657534246575, 20.030136986301372, 25.027397260273972, 30.030136986301372});
	  private static readonly DoubleArray DSC_RATE = DoubleArray.ofUnsafe(new double[] {0.004510969198370304, 0.00930277781406035, 0.012152971715618414, 0.017638643770220588, 0.019260098011444397, 0.02072958904811958, 0.01658424716087226, 0.02035074046575936, 0.023313764334801694, 0.025640888682876155, 0.027453756419591822, 0.028832553111413566, 0.029976760913966324, 0.030912599984222154, 0.03173930709211652, 0.03249979503727117, 0.033314372450170285, 0.034875344837724434, 0.03532470846114178, 0.03501411934224827, 0.03490957722439039});
	  private static readonly CurveName DSC_NAME = CurveName.of("gbp_dsc");
	  private static readonly IsdaCreditDiscountFactors DSC_CURVE = IsdaCreditDiscountFactors.of(GBP, TODAY, DSC_NAME, DSC_TIME, DSC_RATE, ACT_365F);
	  // recovery rate
	  private const double RECOVERY_RATE = 0.4;
	  private static readonly ConstantRecoveryRates REC_RATES = ConstantRecoveryRates.of(LEGAL_ENTITY, TODAY, RECOVERY_RATE);
	  // rates provider without credit curve
	  private static readonly CreditRatesProvider RATES_PROVIDER = ImmutableCreditRatesProvider.builder().discountCurves(ImmutableMap.of(GBP, DSC_CURVE)).recoveryRateCurves(ImmutableMap.of(LEGAL_ENTITY, REC_RATES)).valuationDate(TODAY).build();

	  private const double ONE_BP = 1.0e-4;
	  private const double TOL = 1.e-15;

	  public virtual void standardQuoteTest()
	  {
		double pointsUpFront = 0.007;
		double expectedParSpread = 0.011112592882846; // taken from Excel-ISDA 1.8.2
		double premium = 100d * ONE_BP;
		Cds product = Cds.of(BUY, LEGAL_ENTITY, GBP, 1.0e6, START_DATE, END_DATE, Frequency.P3M, DEFAULT_CALENDAR, premium);
		TradeInfo info = TradeInfo.builder().tradeDate(TODAY).settlementDate(product.SettlementDateOffset.adjust(TODAY, REF_DATA)).build();
		ResolvedCdsTrade trade = CdsTrade.builder().product(product).info(info).build().resolve(REF_DATA);
		CdsQuote pufQuote = CdsQuote.of(CdsQuoteConvention.POINTS_UPFRONT, pointsUpFront);
		CdsQuote quotedSpread = CONV.quotedSpreadFromPointsUpfront(trade, pufQuote, RATES_PROVIDER, REF_DATA);
		assertEquals(quotedSpread.QuotedValue, expectedParSpread, 1e-14);
		assertTrue(quotedSpread.QuoteConvention.Equals(CdsQuoteConvention.QUOTED_SPREAD));
		CdsQuote derivedPuf = CONV.pointsUpFrontFromQuotedSpread(trade, quotedSpread, RATES_PROVIDER, REF_DATA);
		assertEquals(derivedPuf.QuotedValue, pointsUpFront, 1e-15);
		assertTrue(derivedPuf.QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT));
	  }

	  public virtual void standardQuoteTest2()
	  {
		double quotedSpread = 143.4 * ONE_BP;
		double expectedPuf = -0.2195134271137960; // taken from Excel-ISDA 1.8.2
		double premium = 500d * ONE_BP;
		Cds product = Cds.of(SELL, LEGAL_ENTITY, GBP, 1.0e8, START_DATE, END_DATE, Frequency.P6M, DEFAULT_CALENDAR, premium);
		TradeInfo info = TradeInfo.builder().tradeDate(TODAY).settlementDate(product.SettlementDateOffset.adjust(TODAY, REF_DATA)).build();
		ResolvedCdsTrade trade = CdsTrade.builder().product(product).info(info).build().resolve(REF_DATA);
		CdsQuote quotedSpreadQuoted = CdsQuote.of(CdsQuoteConvention.QUOTED_SPREAD, quotedSpread);
		CdsQuote derivedPuf = CONV.pointsUpFrontFromQuotedSpread(trade, quotedSpreadQuoted, RATES_PROVIDER, REF_DATA);
		assertEquals(derivedPuf.QuotedValue, expectedPuf, 5e-13);
		assertTrue(derivedPuf.QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT));
		CdsQuote derivedQuotedSpread = CONV.quotedSpreadFromPointsUpfront(trade, derivedPuf, RATES_PROVIDER, REF_DATA);
		assertEquals(derivedQuotedSpread.QuotedValue, quotedSpread, 1e-15);
		assertTrue(derivedQuotedSpread.QuoteConvention.Equals(CdsQuoteConvention.QUOTED_SPREAD));
	  }

	  public virtual void parSpreadQuoteTest()
	  {
		int nPillars = MATURITIES.Length;
		IList<Cds> products = new List<Cds>(nPillars);
		IList<CdsQuote> quotes = new List<CdsQuote>(nPillars);
		double[] parSpreads = new double[] {0.00769041167742121, 0.010780108645654813, 0.014587245777777417, 0.017417253343028126, 0.01933997409465104, 0.022289540511698912, 0.025190509434219924};
		for (int i = 0; i < nPillars; ++i)
		{
		  products.Add(Cds.of(BUY, LEGAL_ENTITY, GBP, 1.0e6, START_DATE, MATURITIES[i], Frequency.P3M, DEFAULT_CALENDAR, parSpreads[i]));
		  quotes.Add(CdsQuote.of(CdsQuoteConvention.PAR_SPREAD, parSpreads[i]));
		}
		TradeInfo info = TradeInfo.builder().tradeDate(TODAY).settlementDate(products[0].SettlementDateOffset.adjust(TODAY, REF_DATA)).build();
		IList<ResolvedCdsTrade> trades = products.Select(p => CdsTrade.builder().product(p).info(info).build().resolve(REF_DATA)).ToList();
		IList<CdsQuote> pufsComp = CONV.quotesFromParSpread(trades, quotes, RATES_PROVIDER, CdsQuoteConvention.POINTS_UPFRONT, REF_DATA);
		IList<CdsQuote> pufsMfComp = CONV_MARKIT_FIX.quotesFromParSpread(trades, quotes, RATES_PROVIDER, CdsQuoteConvention.POINTS_UPFRONT, REF_DATA);
		IList<CdsQuote> qssComp = CONV.quotesFromParSpread(trades, quotes, RATES_PROVIDER, CdsQuoteConvention.QUOTED_SPREAD, REF_DATA);
		IList<CdsQuote> qssMfComp = CONV_MARKIT_FIX.quotesFromParSpread(trades, quotes, RATES_PROVIDER, CdsQuoteConvention.QUOTED_SPREAD, REF_DATA);
		for (int i = 0; i < nPillars; ++i)
		{
		  assertEquals(pufsComp[i].QuotedValue, 0d, TOL);
		  assertTrue(pufsComp[i].QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT));
		  assertEquals(pufsMfComp[i].QuotedValue, 0d, TOL);
		  assertTrue(pufsMfComp[i].QuoteConvention.Equals(CdsQuoteConvention.POINTS_UPFRONT));
		}
		for (int i = 0; i < nPillars; ++i)
		{
		  CdsQuote qsRe = CONV.quotedSpreadFromPointsUpfront(trades[i], pufsComp[i], RATES_PROVIDER, REF_DATA);
		  CdsQuote qsMfRe = CONV_MARKIT_FIX.quotedSpreadFromPointsUpfront(trades[i], pufsMfComp[i], RATES_PROVIDER, REF_DATA);
		  assertEquals(qsRe.QuotedValue, qssComp[i].QuotedValue, TOL);
		  assertEquals(qsMfRe.QuotedValue, qssMfComp[i].QuotedValue, TOL);
		}
	  }

	  public virtual void pricePufTest()
	  {
		double premium = 150d * ONE_BP;
		Cds product = Cds.of(BUY, LEGAL_ENTITY, GBP, 1.0e6, START_DATE, END_DATE, Frequency.P3M, DEFAULT_CALENDAR, premium);
		TradeInfo info = TradeInfo.builder().tradeDate(TODAY).settlementDate(product.SettlementDateOffset.adjust(TODAY, REF_DATA)).build();
		ResolvedCdsTrade trade = CdsTrade.builder().product(product).info(info).build().resolve(REF_DATA);
		NodalCurve cc = CALIB.calibrate(ImmutableList.of(trade), DoubleArray.of(0.0123), DoubleArray.of(0.0), CurveName.of("test"), TODAY, DSC_CURVE, REC_RATES, REF_DATA);
		CreditRatesProvider rates = RATES_PROVIDER.toImmutableCreditRatesProvider().toBuilder().creditCurves(ImmutableMap.of(Pair.of(LEGAL_ENTITY, GBP), LegalEntitySurvivalProbabilities.of(LEGAL_ENTITY, IsdaCreditDiscountFactors.of(GBP, TODAY, cc)))).build();
		double pointsUpFront = CONV.pointsUpfront(trade, rates, REF_DATA);
		double cleanPrice = CONV.cleanPrice(trade, rates, REF_DATA);
		double cleanPriceRe = CONV.cleanPriceFromPointsUpfront(pointsUpFront);
		assertEquals(cleanPrice, cleanPriceRe, TOL);
	  }

	}

}