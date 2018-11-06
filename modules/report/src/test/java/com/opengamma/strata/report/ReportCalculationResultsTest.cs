using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.BUY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.BuySell.SELL;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Column = com.opengamma.strata.calc.Column;
	using Results = com.opengamma.strata.calc.Results;
	using Result = com.opengamma.strata.collect.result.Result;
	using Measures = com.opengamma.strata.measure.Measures;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;
	using Fra = com.opengamma.strata.product.fra.Fra;
	using FraTrade = com.opengamma.strata.product.fra.FraTrade;

	/// <summary>
	/// Test <seealso cref="ReportCalculationResults"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ReportCalculationResultsTest
	public class ReportCalculationResultsTest
	{

	  private static readonly LocalDate VAL_DATE = date(2016, 6, 30);
	  private static readonly LocalDate VAL_DATE2 = date(2016, 7, 1);
	  private static readonly FraTrade TRADE = FraTrade.of(TradeInfo.empty(), Fra.builder().buySell(BUY).notional(1_000_000).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(date(2015, 8, 7))).fixedRate(0.25d).index(GBP_LIBOR_3M).build());
	  private static readonly FraTrade TRADE2 = FraTrade.of(TradeInfo.empty(), Fra.builder().buySell(SELL).notional(1_000_000).startDate(date(2015, 8, 5)).endDate(date(2015, 11, 5)).paymentDate(AdjustableDate.of(date(2015, 8, 7))).fixedRate(0.25d).index(GBP_LIBOR_3M).build());
	  private static readonly Column COLUMN = Column.of(Measures.PRESENT_VALUE);
	  private static readonly Column COLUMN2 = Column.of(Measures.PAR_RATE);
	  private static readonly CurrencyAmount PV = CurrencyAmount.of(GBP, 12);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ReportCalculationResults test = sut();
		assertEquals(test.ValuationDate, VAL_DATE);
		assertEquals(test.Targets, ImmutableList.of(TRADE));
		assertEquals(test.Columns, ImmutableList.of(COLUMN));
		assertEquals(test.CalculationResults.get(0, 0).Value, PV);
		assertEquals(test.ReferenceData, ReferenceData.standard());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  //-------------------------------------------------------------------------
	  internal static ReportCalculationResults sut()
	  {
		Results results = Results.of(ImmutableList.of(COLUMN.toHeader()), ImmutableList.of(Result.success(PV)));
		return ReportCalculationResults.of(VAL_DATE, ImmutableList.of(TRADE), ImmutableList.of(COLUMN), results);
	  }

	  internal static ReportCalculationResults sut2()
	  {
		Results results = Results.of(ImmutableList.of(COLUMN.toHeader()), ImmutableList.of(Result.success(Convert.ToDouble(25))));
		return ReportCalculationResults.of(VAL_DATE2, ImmutableList.of(TRADE2), ImmutableList.of(COLUMN2), results);
	  }

	}

}