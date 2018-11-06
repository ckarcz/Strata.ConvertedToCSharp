using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Column = com.opengamma.strata.calc.Column;
	using Measure = com.opengamma.strata.calc.Measure;
	using Results = com.opengamma.strata.calc.Results;
	using Result = com.opengamma.strata.collect.result.Result;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;

	/// <summary>
	/// Test <seealso cref="ValuePathEvaluator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ValuePathEvaluatorTest
	public class ValuePathEvaluatorTest
	{

	  public virtual void measurePath()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> currencyResults = ValuePathEvaluator.evaluate("Measures.PresentValue.Currency", reportResults);
		IList<Result<object>> currencyResults = ValuePathEvaluator.evaluate("Measures.PresentValue.Currency", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> expectedCurrencies = com.google.common.collect.ImmutableList.of(com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.Currency.CAD), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.Currency.AUD), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.Currency.CHF));
		IList<Result<object>> expectedCurrencies = ImmutableList.of(Result.success(Currency.CAD), Result.success(Currency.AUD), Result.success(Currency.CHF));
		assertThat(currencyResults).isEqualTo(expectedCurrencies);

		// Amount returns the CurrencyAmount which is slightly unexpected
		// It's required in order to be able to format the amount to the correct number of decimal places
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> amountResults = ValuePathEvaluator.evaluate("Measures.PresentValue.Amount", reportResults);
		IList<Result<object>> amountResults = ValuePathEvaluator.evaluate("Measures.PresentValue.Amount", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> expectedAmounts = com.google.common.collect.ImmutableList.of(com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.CAD, 2d)), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.AUD, 3d)), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.CHF, 4d)));
		IList<Result<object>> expectedAmounts = ImmutableList.of(Result.success(CurrencyAmount.of(Currency.CAD, 2d)), Result.success(CurrencyAmount.of(Currency.AUD, 3d)), Result.success(CurrencyAmount.of(Currency.CHF, 4d)));
		assertThat(amountResults).isEqualTo(expectedAmounts);
	  }

	  public virtual void measurePath_failure_noDot()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> results = ValuePathEvaluator.evaluate("Measures", reportResults);
		IList<Result<object>> results = ValuePathEvaluator.evaluate("Measures", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.get(0);
		Result<object> result = results[0];
		assertThat(result.Failure).True;
		assertThat(result.Failure.Message).contains("PresentValue");
		assertThat(result.Failure.Message).contains("ParRate");
	  }

	  public virtual void measurePath_failure_noMeasureName()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> results = ValuePathEvaluator.evaluate("Measures.", reportResults);
		IList<Result<object>> results = ValuePathEvaluator.evaluate("Measures.", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.get(0);
		Result<object> result = results[0];
		assertThat(result.Failure).True;
		assertThat(result.Failure.Message).contains("PresentValue");
		assertThat(result.Failure.Message).contains("ParRate");
	  }

	  public virtual void measurePath_failure_unknownMeasure()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> results = ValuePathEvaluator.evaluate("Measures.Wibble", reportResults);
		IList<Result<object>> results = ValuePathEvaluator.evaluate("Measures.Wibble", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.get(0);
		Result<object> result = results[0];
		assertThat(result.Failure).True;
		assertThat(result.Failure.Message).contains("Wibble");
		assertThat(result.Failure.Message).contains("PresentValue");
		assertThat(result.Failure.Message).contains("ParRate");
	  }

	  public virtual void measurePath_failure_nonQueriedMeasure()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> results = ValuePathEvaluator.evaluate("Measures.ParSpread", reportResults);
		IList<Result<object>> results = ValuePathEvaluator.evaluate("Measures.ParSpread", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = results.get(0);
		Result<object> result = results[0];
		assertThat(result.Failure).True;
		assertThat(result.Failure.Message).contains("PresentValue");
		assertThat(result.Failure.Message).contains("ParRate");
	  }

	  public virtual void tradePath()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> counterpartyResults = ValuePathEvaluator.evaluate("Trade.Counterparty.Value", reportResults);
		IList<Result<object>> counterpartyResults = ValuePathEvaluator.evaluate("Trade.Counterparty.Value", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> expectedCounterparties = com.google.common.collect.ImmutableList.of(com.opengamma.strata.collect.result.Result.success("cpty1"), com.opengamma.strata.collect.result.Result.success("cpty2"), com.opengamma.strata.collect.result.Result.success("cpty3"));
		IList<Result<object>> expectedCounterparties = ImmutableList.of(Result.success("cpty1"), Result.success("cpty2"), Result.success("cpty3"));
		assertThat(counterpartyResults).isEqualTo(expectedCounterparties);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> counterpartyResults2 = ValuePathEvaluator.evaluate("Target.Counterparty.Value", reportResults);
		IList<Result<object>> counterpartyResults2 = ValuePathEvaluator.evaluate("Target.Counterparty.Value", reportResults);
		assertThat(counterpartyResults2).isEqualTo(expectedCounterparties);
	  }

	  public virtual void productPath()
	  {
		ReportCalculationResults reportResults = ValuePathEvaluatorTest.reportResults();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> counterpartyResults = ValuePathEvaluator.evaluate("Trade.Product.Notional", reportResults);
		IList<Result<object>> counterpartyResults = ValuePathEvaluator.evaluate("Trade.Product.Notional", reportResults);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.collect.result.Result<?>> expectedCounterparties = com.google.common.collect.ImmutableList.of(com.opengamma.strata.collect.result.Result.success(1_000_000d), com.opengamma.strata.collect.result.Result.success(10_000_000d), com.opengamma.strata.collect.result.Result.success(100_000_000d));
		IList<Result<object>> expectedCounterparties = ImmutableList.of(Result.success(1_000_000d), Result.success(10_000_000d), Result.success(100_000_000d));
		assertThat(counterpartyResults).isEqualTo(expectedCounterparties);
	  }

	  //--------------------------------------------------------------------------------------------------

	  private static ReportCalculationResults reportResults()
	  {
		Measure measure = Measure.of("PresentValue");
		Column column = Column.of(measure);
		IList<Column> columns = ImmutableList.of(column);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<? extends com.opengamma.strata.collect.result.Result<?>> resultValues = com.google.common.collect.ImmutableList.of(com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.CAD, 2d)), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.AUD, 3d)), com.opengamma.strata.collect.result.Result.success(com.opengamma.strata.basics.currency.CurrencyAmount.of(com.opengamma.strata.basics.currency.Currency.CHF, 4d)));
		IList<Result<object>> resultValues = ImmutableList.of(Result.success(CurrencyAmount.of(Currency.CAD, 2d)), Result.success(CurrencyAmount.of(Currency.AUD, 3d)), Result.success(CurrencyAmount.of(Currency.CHF, 4d)));
		IList<Trade> trades = ImmutableList.of(trade("cpty1", 1_000_000), trade("cpty2", 10_000_000), trade("cpty3", 100_000_000));
		Results results = Results.of(ImmutableList.of(column.toHeader()), resultValues);
		return ReportCalculationResults.of(LocalDate.now(ZoneOffset.UTC), trades, columns, results);
	  }

	  private static Trade trade(string counterparty, double notional)
	  {
		TradeInfo tradeInfo = TradeInfo.builder().counterparty(StandardId.of("cpty", counterparty)).build();
		Fra fra = Fra.builder().buySell(BUY).notional(notional).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(date(2015, 8, 7))).fixedRate(0.25d).index(GBP_LIBOR_3M).build();
		return FraTrade.builder().info(tradeInfo).product(fra).build();
	  }
	}

}