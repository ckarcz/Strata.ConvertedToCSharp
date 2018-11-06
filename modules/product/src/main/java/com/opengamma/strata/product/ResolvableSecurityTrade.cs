/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using ResolvableCalculationTarget = com.opengamma.strata.basics.ResolvableCalculationTarget;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A trade that has a security identifier that can be resolved using reference data.
	/// <para>
	/// This represents those trades that hold a security identifier. It allows the trade
	/// to be resolved, returning an alternate representation of the same trade with complete
	/// security information.
	/// </para>
	/// </summary>
	public interface ResolvableSecurityTrade : SecurityQuantityTrade, ResolvableCalculationTarget
	{

	  /// <summary>
	  /// Resolves the security identifier using the specified reference data.
	  /// <para>
	  /// This takes the security identifier of this trade, looks it up in reference data,
	  /// and returns the equivalent trade with full security information.
	  /// If the security has underlying securities, they will also have been resolved in the result.
	  /// </para>
	  /// <para>
	  /// The resulting trade is bound to data from reference data.
	  /// If the data changes, the resulting trade form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default SecuritizedProductTrade<JavaToDotNetGenericWildcard> resolveTarget(com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//	SecurityId securityId = getSecurityId();
	//	Security security = refData.getValue(securityId);
	//	SecurityQuantityTrade trade = security.createTrade(getInfo(), getQuantity(), getPrice(), refData);
	//	if (trade instanceof SecuritizedProductTrade)
	//	{
	//	  return (SecuritizedProductTrade<?>) trade;
	//	}
	//	throw new ClassCastException(Messages.format("Reference data for security '{}' did not implement SecuritizedProductTrade: ", securityId, trade.getClass().getName()));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  ResolvableSecurityTrade withInfo(TradeInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  ResolvableSecurityTrade withQuantity(double quantity);

	  /// <summary>
	  /// Returns an instance with the specified price.
	  /// </summary>
	  /// <param name="price">  the new price </param>
	  /// <returns> the instance with the specified price </returns>
	  ResolvableSecurityTrade withPrice(double price);

	}

}