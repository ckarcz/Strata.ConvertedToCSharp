/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using RatesProviderFxDataSets = com.opengamma.strata.pricer.fx.RatesProviderFxDataSets;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using EuropeanVanillaOptionFunction = com.opengamma.strata.pricer.impl.tree.EuropeanVanillaOptionFunction;
	using OptionFunction = com.opengamma.strata.pricer.impl.tree.OptionFunction;
	using TrinomialTree = com.opengamma.strata.pricer.impl.tree.TrinomialTree;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using ResolvedFxSingle = com.opengamma.strata.product.fx.ResolvedFxSingle;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;

	/// <summary>
	/// Test <seealso cref="ImpliedTrinomialTreeFxOptionCalibrator"/>.
	/// <para>
	/// Further tests with barrier options are in <seealso cref="ImpliedTrinomialTreeFxSingleBarrierOptionProductPricerTest"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ImpliedTrinomialTreeFxOptionCalibratorTest
	public class ImpliedTrinomialTreeFxOptionCalibratorTest
	{

	  private static readonly ZoneId ZONE = ZoneId.of("Z");
	  private static readonly LocalDate VAL_DATE = LocalDate.of(2011, 6, 13);
	  private static readonly ZonedDateTime VAL_DATETIME = VAL_DATE.atStartOfDay(ZONE);
	  private static readonly LocalDate PAY_DATE = LocalDate.of(2012, 9, 15);
	  private static readonly LocalDate EXPIRY_DATE = LocalDate.of(2012, 9, 15);
	  private static readonly ZonedDateTime EXPIRY_DATETIME = EXPIRY_DATE.atStartOfDay(ZONE);
	  // providers
	  private static readonly BlackFxOptionSmileVolatilities VOLS = FxVolatilitySmileDataSet.createVolatilitySmileProvider5(VAL_DATETIME);
	  private static readonly BlackFxOptionSmileVolatilities VOLS_MRKT = FxVolatilitySmileDataSet.createVolatilitySmileProvider5Market(VAL_DATETIME);
	  private static readonly ImmutableRatesProvider RATE_PROVIDER = RatesProviderFxDataSets.createProviderEurUsdFlat(VAL_DATE);
	  // call - for calibration
	  private const double NOTIONAL = 100_000_000d;
	  private const double STRIKE_RATE = 1.35;
	  private static readonly CurrencyAmount EUR_AMOUNT_REC = CurrencyAmount.of(EUR, NOTIONAL);
	  private static readonly CurrencyAmount USD_AMOUNT_PAY = CurrencyAmount.of(USD, -NOTIONAL * STRIKE_RATE);
	  private static readonly ResolvedFxSingle FX_PRODUCT = ResolvedFxSingle.of(EUR_AMOUNT_REC, USD_AMOUNT_PAY, PAY_DATE);
	  private static readonly ResolvedFxVanillaOption CALL = ResolvedFxVanillaOption.builder().longShort(LongShort.LONG).expiry(EXPIRY_DATETIME).underlying(FX_PRODUCT).build();
	  private static readonly ImpliedTrinomialTreeFxOptionCalibrator CALIB = new ImpliedTrinomialTreeFxOptionCalibrator(39);
	  private static readonly RecombiningTrinomialTreeData TREE_DATA = CALIB.calibrateTrinomialTree(CALL, RATE_PROVIDER, VOLS);
	  private static readonly RecombiningTrinomialTreeData TREE_DATA_MRKT = CALIB.calibrateTrinomialTree(CALL, RATE_PROVIDER, VOLS_MRKT);
	  private static readonly TrinomialTree TREE = new TrinomialTree();

	  public virtual void test_recoverVolatility()
	  {
		int nSteps = TREE_DATA.NumberOfSteps;
		double spot = TREE_DATA.Spot;
		double timeToExpiry = TREE_DATA.getTime(nSteps);
		double dfDom = RATE_PROVIDER.discountFactors(USD).discountFactor(timeToExpiry);
		double dfFor = RATE_PROVIDER.discountFactors(EUR).discountFactor(timeToExpiry);
		double forward = spot * dfFor / dfDom;
		for (int i = 0; i < 100; ++i)
		{
		  double strike = spot * (0.8 + 0.004 * i);
		  OptionFunction func = EuropeanVanillaOptionFunction.of(strike, timeToExpiry, PutCall.CALL, nSteps);
		  double price = TREE.optionPrice(func, TREE_DATA);
		  double impliedVol = BlackFormulaRepository.impliedVolatility(price / dfDom, forward, strike, timeToExpiry, true);
		  double orgVol = VOLS.volatility(FX_PRODUCT.CurrencyPair, timeToExpiry, strike, forward);
		  assertEquals(impliedVol, orgVol, orgVol * 0.1); // large tol
		  double priceMrkt = TREE.optionPrice(func, TREE_DATA_MRKT);
		  double impliedVolMrkt = BlackFormulaRepository.impliedVolatility(priceMrkt / dfDom, forward, strike, timeToExpiry, true);
		  double orgVolMrkt = VOLS_MRKT.volatility(FX_PRODUCT.CurrencyPair, timeToExpiry, strike, forward);
		  assertEquals(impliedVolMrkt, orgVolMrkt, orgVolMrkt * 0.1); // large tol
		}
	  }

	}

}