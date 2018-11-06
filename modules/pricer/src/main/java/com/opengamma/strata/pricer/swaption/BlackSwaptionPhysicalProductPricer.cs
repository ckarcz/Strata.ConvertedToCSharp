/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using DiscountingSwapProductPricer = com.opengamma.strata.pricer.swap.DiscountingSwapProductPricer;
	using Swap = com.opengamma.strata.product.swap.Swap;

	/// <summary>
	/// Pricer for swaption with physical settlement in a log-normal or Black model on the swap rate.
	/// <para>
	/// The swap underlying the swaption must have a fixed leg on which the forward rate is computed.
	/// The underlying swap must be single currency.
	/// </para>
	/// <para>
	/// The volatility parameters are not adjusted for the underlying swap convention.
	/// </para>
	/// <para>
	/// The value of the swaption after expiry is 0.
	/// For a swaption which has already expired, a negative number is returned by
	/// <seealso cref="SwaptionVolatilities#relativeTime(ZonedDateTime)"/>.
	/// </para>
	/// </summary>
	public class BlackSwaptionPhysicalProductPricer : VolatilitySwaptionPhysicalProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly BlackSwaptionPhysicalProductPricer DEFAULT = new BlackSwaptionPhysicalProductPricer(DiscountingSwapProductPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="swapPricer">  the pricer for <seealso cref="Swap"/> </param>
	  public BlackSwaptionPhysicalProductPricer(DiscountingSwapProductPricer swapPricer) : base(swapPricer)
	  {
	  }

	}

}