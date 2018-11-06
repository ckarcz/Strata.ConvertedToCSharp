/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using AdjustablePayment = com.opengamma.strata.basics.currency.AdjustablePayment;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for credit default swap trades.
	/// </summary>
	public interface CdsConvention : TradeConvention, Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CdsConvention of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CdsConvention of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the convention to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<CdsConvention> extendedEnum()
	//  {
	//	return CdsConventions.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Get the number of days between valuation date and settlement date.
	  /// <para>
	  /// It is usually 3 business days for standardised CDS contracts.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> days adjustment </returns>
	  DaysAdjustment SettlementDateOffset {get;}

	  /// <summary>
	  /// Get the currency of the CDS.
	  /// <para>
	  /// The amounts of the notional are expressed in terms of this currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  Currency Currency {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a CDS trade based on the trade date and the IMM date logic. 
	  /// <para>
	  /// The start date and end date are computed from trade date with the standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate startDate = CdsImmDateLogic.getPreviousImmDate(tradeDate);
	//	LocalDate roll = CdsImmDateLogic.getNextSemiAnnualRollDate(tradeDate);
	//	LocalDate endDate = roll.plus(tenor).minusMonths(3);
	//	return createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, refData);
	//  }

	  /// <summary>
	  /// Creates a CDS trade based on the trade date, start date and the IMM date logic. 
	  /// <para>
	  /// The end date is computed from the start date with the standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, java.time.LocalDate startDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate roll = CdsImmDateLogic.getNextSemiAnnualRollDate(startDate);
	//	LocalDate endDate = roll.plus(tenor).minusMonths(3);
	//	return createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, refData);
	//  }

	  /// <summary>
	  /// Creates a CDS trade from trade date, start date and end date.
	  /// <para>
	  /// The settlement date is computed from the trade date using {@code settlementDateOffset} defined in the convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate settlementDate = getSettlementDateOffset().adjust(tradeDate, refData);
	//	TradeInfo tradeInfo = TradeInfo.builder().tradeDate(tradeDate).settlementDate(settlementDate).build();
	//	return toTrade(legalEntityId, tradeInfo, startDate, endDate, buySell, notional, fixedRate);
	//  }

	  /// <summary>
	  /// Creates a CDS trade with {@code TradeInfo}.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeInfo">  the trade info </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <returns> the CDS trade </returns>
	  CdsTrade toTrade(StandardId legalEntityId, TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a CDS trade with upfront fee based on the trade date and the IMM date logic. 
	  /// <para>
	  /// The start date and end date are computed from trade date with the standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="upFrontFee">  the upFront fee </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.currency.AdjustablePayment upFrontFee, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate startDate = CdsImmDateLogic.getPreviousImmDate(tradeDate);
	//	LocalDate roll = CdsImmDateLogic.getNextSemiAnnualRollDate(tradeDate);
	//	LocalDate endDate = roll.plus(tenor).minusMonths(3);
	//	return createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, upFrontFee, refData);
	//  }

	  /// <summary>
	  /// Creates a CDS trade with upfront fee based on the trade date, start date and the IMM date logic. 
	  /// <para>
	  /// The end date is computed from the start date with the standard semi-annual roll convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="tenor">  the tenor </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="upFrontFee">  the upFront fee </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, java.time.LocalDate startDate, com.opengamma.strata.basics.date.Tenor tenor, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.currency.AdjustablePayment upFrontFee, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate roll = CdsImmDateLogic.getNextSemiAnnualRollDate(startDate);
	//	LocalDate endDate = roll.plus(tenor).minusMonths(3);
	//	return createTrade(legalEntityId, tradeDate, startDate, endDate, buySell, notional, fixedRate, upFrontFee, refData);
	//  }

	  /// <summary>
	  /// Creates a CDS trade with upfront fee from trade date, start date and end date.
	  /// <para>
	  /// The settlement date is computed from the trade date using {@code settlementDateOffset} defined in the convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="upFrontFee">  the upFront fee </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the CDS trade </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.credit.CdsTrade createTrade(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate tradeDate, java.time.LocalDate startDate, java.time.LocalDate endDate, com.opengamma.strata.product.common.BuySell buySell, double notional, double fixedRate, com.opengamma.strata.basics.currency.AdjustablePayment upFrontFee, com.opengamma.strata.basics.ReferenceData refData)
	//  {
	//
	//	LocalDate settlementDate = getSettlementDateOffset().adjust(tradeDate, refData);
	//	TradeInfo tradeInfo = TradeInfo.builder().tradeDate(tradeDate).settlementDate(settlementDate).build();
	//	return toTrade(legalEntityId, tradeInfo, startDate, endDate, buySell, notional, fixedRate, upFrontFee);
	//  }

	  /// <summary>
	  /// Creates a CDS trade with upfront fee and {@code TradeInfo}.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="tradeInfo">  the trade info </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="buySell">  buy or sell </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="upFrontFee">  the upFront fee </param>
	  /// <returns> the CDS trade </returns>
	  CdsTrade toTrade(StandardId legalEntityId, TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, BuySell buySell, double notional, double fixedRate, AdjustablePayment upFrontFee);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this convention.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	}

}