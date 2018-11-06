/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.mock;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.mockito.Mockito.when;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using MockRatesProvider = com.opengamma.strata.pricer.impl.MockRatesProvider;
	using IborIndexRates = com.opengamma.strata.pricer.rate.IborIndexRates;
	using IborRateSensitivity = com.opengamma.strata.pricer.rate.IborRateSensitivity;
	using SimpleRatesProvider = com.opengamma.strata.pricer.rate.SimpleRatesProvider;
	using ResolvedIborFuture = com.opengamma.strata.product.index.ResolvedIborFuture;

	/// <summary>
	/// Test <seealso cref="DiscountingIborFutureTradePricer"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiscountingIborFutureProductPricerTest
	public class DiscountingIborFutureProductPricerTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly DiscountingIborFutureProductPricer PRICER = DiscountingIborFutureProductPricer.DEFAULT;
	  private static readonly ResolvedIborFuture FUTURE = IborFutureDummyData.IBOR_FUTURE.resolve(REF_DATA);

	  private const double RATE = 0.045;
	  private const double TOLERANCE_PRICE = 1.0e-9;
	  private const double TOLERANCE_PRICE_DELTA = 1.0e-9;

	  //------------------------------------------------------------------------- 
	  public virtual void test_marginIndex()
	  {
		double notional = FUTURE.Notional;
		double accrualFactor = FUTURE.AccrualFactor;
		double price = 0.99;
		double marginIndexExpected = price * notional * accrualFactor;
		double marginIndexComputed = PRICER.marginIndex(FUTURE, price);
		assertEquals(marginIndexComputed, marginIndexExpected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_marginIndexSensitivity()
	  {
		double notional = FUTURE.Notional;
		double accrualFactor = FUTURE.AccrualFactor;
		PointSensitivities sensiExpected = PointSensitivities.of(IborRateSensitivity.of(FUTURE.IborRate.Observation, -notional * accrualFactor));
		PointSensitivities priceSensitivity = PRICER.priceSensitivity(FUTURE, new MockRatesProvider());
		PointSensitivities sensiComputed = PRICER.marginIndexSensitivity(FUTURE, priceSensitivity).normalized();
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, 1e-5));
	  }

	  //------------------------------------------------------------------------- 
	  public virtual void test_price()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;
		when(mockIbor.rate(FUTURE.IborRate.Observation)).thenReturn(RATE);

		assertEquals(PRICER.price(FUTURE, prov), 1.0 - RATE, TOLERANCE_PRICE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_priceSensitivity()
	  {
		IborIndexRates mockIbor = mock(typeof(IborIndexRates));
		SimpleRatesProvider prov = new SimpleRatesProvider();
		prov.IborRates = mockIbor;

		PointSensitivities sensiExpected = PointSensitivities.of(IborRateSensitivity.of(FUTURE.IborRate.Observation, -1d));
		PointSensitivities sensiComputed = PRICER.priceSensitivity(FUTURE, prov);
		assertTrue(sensiComputed.equalWithTolerance(sensiExpected, TOLERANCE_PRICE_DELTA));
	  }

	}

}