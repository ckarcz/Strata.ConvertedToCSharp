/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
	using SettlementType = com.opengamma.strata.product.common.SettlementType;

	/// <summary>
	/// Defines how the swaption will be settled.
	/// <para>
	/// Settlement can be physical, where an interest rate swap is created, or cash,
	/// where a monetary amount is exchanged.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= PhysicalSwaptionSettlement </seealso>
	/// <seealso cref= CashSwaptionSettlement </seealso>
	public interface SwaptionSettlement
	{

	  /// <summary>
	  /// Gets the settlement type of swaption.
	  /// <para>
	  /// The settlement type is cash settlement or physical settlement, defined in <seealso cref="SettlementType"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the settlement type </returns>
	  SettlementType SettlementType {get;}

	}

}