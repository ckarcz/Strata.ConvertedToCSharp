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
	/// Pricer for cap/floor trades in normal or Bachelier model.
	/// </summary>
	public class NormalIborCapFloorTradePricer : VolatilityIborCapFloorTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly NormalIborCapFloorTradePricer DEFAULT = new NormalIborCapFloorTradePricer(NormalIborCapFloorProductPricer.DEFAULT, DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="productPricer">  the pricer for <seealso cref="ResolvedIborCapFloor"/> </param>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public NormalIborCapFloorTradePricer(NormalIborCapFloorProductPricer productPricer, DiscountingPaymentPricer paymentPricer) : base(productPricer, paymentPricer)
	  {

	  }

	}

}