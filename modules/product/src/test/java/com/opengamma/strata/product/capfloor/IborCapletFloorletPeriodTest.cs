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
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;

	/// <summary>
	/// Test <seealso cref="IborCapletFloorletPeriod"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapletFloorletPeriodTest
	public class IborCapletFloorletPeriodTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING = LocalDate.of(2011, 1, 4);
	  private static readonly ZonedDateTime FIXING_TIME_ZONE = EUR_EURIBOR_3M.calculateFixingDateTime(FIXING);
	  private const double STRIKE = 0.04;
	  private static readonly LocalDate START_UNADJ = LocalDate.of(2010, 10, 8);
	  private static readonly LocalDate END_UNADJ = LocalDate.of(2011, 1, 8);
	  private static readonly LocalDate START = LocalDate.of(2010, 10, 8);
	  private static readonly LocalDate END = LocalDate.of(2011, 1, 10);
	  private static readonly LocalDate PAYMENT = LocalDate.of(2011, 1, 13);
	  private const double NOTIONAL = 1.e6;
	  private static readonly IborRateComputation RATE_COMP = IborRateComputation.of(EUR_EURIBOR_3M, FIXING, REF_DATA);
	  private const double YEAR_FRACTION = 0.251d;

	  public virtual void test_builder_min()
	  {
		IborCapletFloorletPeriod test = IborCapletFloorletPeriod.builder().notional(NOTIONAL).startDate(START).endDate(END).yearFraction(YEAR_FRACTION).caplet(STRIKE).iborRate(RATE_COMP).build();
		assertEquals(test.Caplet.Value, STRIKE);
		assertEquals(test.Floorlet.HasValue, false);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.StartDate, START);
		assertEquals(test.EndDate, END);
		assertEquals(test.PaymentDate, test.EndDate);
		assertEquals(test.Currency, EUR);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.IborRate, RATE_COMP);
		assertEquals(test.Index, EUR_EURIBOR_3M);
		assertEquals(test.FixingDate, FIXING_TIME_ZONE.toLocalDate());
		assertEquals(test.FixingDateTime, FIXING_TIME_ZONE);
		assertEquals(test.PutCall, PutCall.CALL);
		assertEquals(test.UnadjustedStartDate, START);
		assertEquals(test.UnadjustedEndDate, END);
		assertEquals(test.YearFraction, YEAR_FRACTION);
	  }

	  public virtual void test_builder_full()
	  {
		IborCapletFloorletPeriod test = IborCapletFloorletPeriod.builder().notional(NOTIONAL).startDate(START).endDate(END).unadjustedStartDate(START_UNADJ).unadjustedEndDate(END_UNADJ).paymentDate(PAYMENT).yearFraction(YEAR_FRACTION).currency(GBP).floorlet(STRIKE).iborRate(RATE_COMP).build();
		assertEquals(test.Floorlet.Value, STRIKE);
		assertEquals(test.Caplet.HasValue, false);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.StartDate, START);
		assertEquals(test.EndDate, END);
		assertEquals(test.UnadjustedStartDate, START_UNADJ);
		assertEquals(test.UnadjustedEndDate, END_UNADJ);
		assertEquals(test.PaymentDate, PAYMENT);
		assertEquals(test.Currency, GBP);
		assertEquals(test.Notional, NOTIONAL);
		assertEquals(test.IborRate, RATE_COMP);
		assertEquals(test.Index, EUR_EURIBOR_3M);
		assertEquals(test.FixingDateTime, FIXING_TIME_ZONE);
		assertEquals(test.PutCall, PutCall.PUT);
		assertEquals(test.YearFraction, YEAR_FRACTION);
	  }

	  public virtual void test_builder_fail()
	  {
		// rate observation missing
		assertThrowsIllegalArg(() => IborCapletFloorletPeriod.builder().notional(NOTIONAL).caplet(STRIKE).build());
		// cap and floor missing
		assertThrowsIllegalArg(() => IborCapletFloorletPeriod.builder().notional(NOTIONAL).iborRate(RATE_COMP).build());
		// cap and floor present
		assertThrowsIllegalArg(() => IborCapletFloorletPeriod.builder().notional(NOTIONAL).caplet(STRIKE).floorlet(STRIKE).iborRate(RATE_COMP).build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static IborCapletFloorletPeriod sut()
	  {
		return IborCapletFloorletPeriod.builder().notional(NOTIONAL).startDate(START).endDate(END).caplet(STRIKE).iborRate(RATE_COMP).build();
	  }

	  internal static IborCapletFloorletPeriod sut2()
	  {
		return IborCapletFloorletPeriod.builder().notional(-NOTIONAL).startDate(START.plusDays(1)).endDate(END.plusDays(1)).floorlet(STRIKE).iborRate(IborRateComputation.of(USD_LIBOR_6M, LocalDate.of(2013, 2, 15), REF_DATA)).build();
	  }

	}

}