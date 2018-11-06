/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.PAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.common.PayReceive.RECEIVE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="ResolvedIborCapFloorLeg"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ResolvedIborCapFloorLegTest
	public class ResolvedIborCapFloorLegTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private const double STRIKE = 0.0125;
	  private const double NOTIONAL = 1.0e6;
	  private static readonly IborCapletFloorletPeriod PERIOD_1 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 3, 17)).endDate(LocalDate.of(2011, 6, 17)).unadjustedStartDate(LocalDate.of(2011, 3, 17)).unadjustedEndDate(LocalDate.of(2011, 6, 17)).paymentDate(LocalDate.of(2011, 6, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 6, 15), REF_DATA)).yearFraction(0.2556).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_2 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 6, 17)).endDate(LocalDate.of(2011, 9, 19)).unadjustedStartDate(LocalDate.of(2011, 6, 17)).unadjustedEndDate(LocalDate.of(2011, 9, 17)).paymentDate(LocalDate.of(2011, 9, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 9, 15), REF_DATA)).yearFraction(0.2611).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_3 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 9, 19)).endDate(LocalDate.of(2011, 12, 19)).unadjustedStartDate(LocalDate.of(2011, 9, 17)).unadjustedEndDate(LocalDate.of(2011, 12, 17)).paymentDate(LocalDate.of(2011, 12, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 12, 15), REF_DATA)).yearFraction(0.2528).build();
	  private static readonly IborCapletFloorletPeriod PERIOD_4 = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 12, 19)).endDate(LocalDate.of(2012, 3, 19)).unadjustedStartDate(LocalDate.of(2011, 12, 17)).unadjustedEndDate(LocalDate.of(2012, 3, 17)).paymentDate(LocalDate.of(2012, 3, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2012, 3, 15), REF_DATA)).yearFraction(0.2528).build();

	  public virtual void test_builder()
	  {
		ResolvedIborCapFloorLeg test = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, PERIOD_2, PERIOD_3, PERIOD_4).payReceive(RECEIVE).build();
		assertEquals(test.CapletFloorletPeriods, ImmutableList.of(PERIOD_1, PERIOD_2, PERIOD_3, PERIOD_4));
		assertEquals(test.PayReceive, RECEIVE);
		assertEquals(test.StartDate, PERIOD_1.StartDate);
		assertEquals(test.EndDate, PERIOD_4.EndDate);
		assertEquals(test.FinalPeriod, PERIOD_4);
		assertEquals(test.FinalFixingDateTime, PERIOD_4.FixingDateTime);
		assertEquals(test.Currency, EUR);
		assertEquals(test.Index, EUR_EURIBOR_3M);
	  }

	  public virtual void test_builder_fail()
	  {
		// two currencies
		IborCapletFloorletPeriod periodGbp = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(GBP).startDate(LocalDate.of(2011, 6, 17)).endDate(LocalDate.of(2011, 9, 19)).unadjustedStartDate(LocalDate.of(2011, 6, 17)).unadjustedEndDate(LocalDate.of(2011, 9, 17)).paymentDate(LocalDate.of(2011, 9, 21)).iborRate(IborRateComputation.of(EUR_EURIBOR_3M, LocalDate.of(2011, 9, 15), REF_DATA)).yearFraction(0.2611).build();
		assertThrowsIllegalArg(() => ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, periodGbp).payReceive(RECEIVE).build());
		// two indices
		IborCapletFloorletPeriod periodLibor = IborCapletFloorletPeriod.builder().caplet(STRIKE).notional(NOTIONAL).currency(EUR).startDate(LocalDate.of(2011, 6, 17)).endDate(LocalDate.of(2011, 9, 19)).unadjustedStartDate(LocalDate.of(2011, 6, 17)).unadjustedEndDate(LocalDate.of(2011, 9, 17)).paymentDate(LocalDate.of(2011, 9, 21)).iborRate(IborRateComputation.of(GBP_LIBOR_3M, LocalDate.of(2011, 9, 15), REF_DATA)).yearFraction(0.2611).build();
		assertThrowsIllegalArg(() => ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, periodLibor).payReceive(RECEIVE).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ResolvedIborCapFloorLeg test1 = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, PERIOD_2, PERIOD_3, PERIOD_4).payReceive(RECEIVE).build();
		coverImmutableBean(test1);
		ResolvedIborCapFloorLeg test2 = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_2, PERIOD_3).payReceive(PAY).build();
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		ResolvedIborCapFloorLeg test = ResolvedIborCapFloorLeg.builder().capletFloorletPeriods(PERIOD_1, PERIOD_2, PERIOD_3, PERIOD_4).payReceive(RECEIVE).build();
		assertSerialization(test);
	  }

	}

}