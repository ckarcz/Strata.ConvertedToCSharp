/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ResolvedIborCapFloor = com.opengamma.strata.product.capfloor.ResolvedIborCapFloor;

	/// <summary>
	/// Pricer for cap/floor trades in log-normal or Black model.
	/// </summary>
	public class BlackIborCapFloorTradePricer : VolatilityIborCapFloorTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly BlackIborCapFloorTradePricer DEFAULT = new BlackIborCapFloorTradePricer(BlackIborCapFloorProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedIborCapFloor"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public BlackIborCapFloorTradePricer(BlackIborCapFloorProductPricer productPricer, DiscountingPaymentPricer paymentPricer) : base(productPricer, paymentPricer)
	  {

	  }

	}

}