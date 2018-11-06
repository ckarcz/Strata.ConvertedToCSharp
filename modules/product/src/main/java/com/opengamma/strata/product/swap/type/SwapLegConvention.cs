/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{

	/// <summary>
	/// A market convention for swap legs.
	/// <para>
	/// A convention contains key information that is commonly used in the market.
	/// Two legs are often combined to form a <seealso cref="TradeConvention"/> for a swap,
	/// such as <seealso cref="FixedIborSwapConvention"/> or <seealso cref="FixedOvernightSwapConvention"/>.
	/// </para>
	/// <para>
	/// Each implementation should provide a method with the name {@code toLeg} with
	/// whatever arguments are necessary to complete the leg.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface SwapLegConvention
	{

	}

}