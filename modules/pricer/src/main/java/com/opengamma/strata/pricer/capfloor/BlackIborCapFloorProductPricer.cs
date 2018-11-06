/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using DiscountingSwapLegPricer = com.opengamma.strata.pricer.swap.DiscountingSwapLegPricer;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Pricer for cap/floor products in log-normal or Black model.
	/// </summary>
	public class BlackIborCapFloorProductPricer : VolatilityIborCapFloorProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly BlackIborCapFloorProductPricer DEFAULT = new BlackIborCapFloorProductPricer(BlackIborCapFloorLegPricer.DEFAULT, DiscountingSwapLegPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="capFloorLegPricer">  the pricer for <seealso cref="IborCapFloorLeg"/> </param>
	  /// <param name="payLegPricer">  the pricer for <seealso cref="SwapLeg"/> </param>
	  public BlackIborCapFloorProductPricer(BlackIborCapFloorLegPricer capFloorLegPricer, DiscountingSwapLegPricer payLegPricer) : base(capFloorLegPricer, payLegPricer)
	  {

	  }

	}

}