/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableRatesProviderSimpleData = com.opengamma.strata.pricer.datasets.ImmutableRatesProviderSimpleData;
	using SwapDummyData = com.opengamma.strata.pricer.swap.SwapDummyData;
	using ResolvedIborFixingDepositTrade = com.opengamma.strata.product.deposit.ResolvedIborFixingDepositTrade;
	using ResolvedTermDepositTrade = com.opengamma.strata.product.deposit.ResolvedTermDepositTrade;
	using ResolvedFraTrade = com.opengamma.strata.product.fra.ResolvedFraTrade;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;
	using ResolvedIborFutureTrade = com.opengamma.strata.product.index.ResolvedIborFutureTrade;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;

	/// <summary>
	/// Test <seealso cref="CalibrationMeasures"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalibrationMeasuresTest
	public class CalibrationMeasuresTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_PAR_SPREAD()
	  {
		assertThat(CalibrationMeasures.PAR_SPREAD.Name).isEqualTo("ParSpread");
		assertThat(CalibrationMeasures.PAR_SPREAD.TradeTypes).contains(typeof(ResolvedFraTrade), typeof(ResolvedFxSwapTrade), typeof(ResolvedIborFixingDepositTrade), typeof(ResolvedIborFutureTrade), typeof(ResolvedSwapTrade), typeof(ResolvedTermDepositTrade));
	  }

	  public virtual void test_MARKET_QUOTE()
	  {
		assertThat(CalibrationMeasures.MARKET_QUOTE.Name).isEqualTo("MarketQuote");
		assertThat(CalibrationMeasures.MARKET_QUOTE.TradeTypes).contains(typeof(ResolvedFraTrade), typeof(ResolvedIborFixingDepositTrade), typeof(ResolvedIborFutureTrade), typeof(ResolvedSwapTrade), typeof(ResolvedTermDepositTrade));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_array()
	  {
		CalibrationMeasures test = CalibrationMeasures.of("Test", TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.SWAP_PAR_SPREAD);
		assertThat(test.Name).isEqualTo("Test");
		assertThat(test.TradeTypes).containsOnly(typeof(ResolvedFraTrade), typeof(ResolvedSwapTrade));
		assertThat(test.ToString()).isEqualTo("Test");
	  }

	  public virtual void test_of_list()
	  {
		CalibrationMeasures test = CalibrationMeasures.of("Test", ImmutableList.of(TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.SWAP_PAR_SPREAD));
		assertThat(test.Name).isEqualTo("Test");
		assertThat(test.TradeTypes).containsOnly(typeof(ResolvedFraTrade), typeof(ResolvedSwapTrade));
		assertThat(test.ToString()).isEqualTo("Test");
	  }

	  public virtual void test_of_duplicate()
	  {
		assertThrowsIllegalArg(() => CalibrationMeasures.of("Test", TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.FRA_PAR_SPREAD));
		assertThrowsIllegalArg(() => CalibrationMeasures.of("Test", ImmutableList.of(TradeCalibrationMeasure.FRA_PAR_SPREAD, TradeCalibrationMeasure.FRA_PAR_SPREAD)));
	  }

	  public virtual void test_measureNotKnown()
	  {
		CalibrationMeasures test = CalibrationMeasures.of("Test", TradeCalibrationMeasure.FRA_PAR_SPREAD);
		assertThrowsIllegalArg(() => test.value(SwapDummyData.SWAP_TRADE, ImmutableRatesProviderSimpleData.IMM_PROV_EUR_FIX), "Trade type 'ResolvedSwapTrade' is not supported for calibration");
	  }

	}

}