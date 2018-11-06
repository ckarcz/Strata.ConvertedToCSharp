/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;

	/// <summary>
	/// A template for creating an Ibor Future trade.
	/// </summary>
	public interface IborFutureTemplate : TradeTemplate
	{

	  /// <summary>
	  /// Obtains a template based on the specified convention using a relative definition of time.
	  /// <para>
	  /// The specific future is defined by two date-related inputs, the minimum period and the 1-based future number.
	  /// For example, the 2nd future of the series where the 1st future is at least 1 week after the value date
	  /// would be represented by a minimum period of 1 week and future number 2.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="minimumPeriod">  the minimum period between the base date and the first future </param>
	  /// <param name="sequenceNumber">  the 1-based index of the future after the minimum period, must be 1 or greater </param>
	  /// <param name="convention">  the future convention </param>
	  /// <returns> the template </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFutureTemplate of(java.time.Period minimumPeriod, int sequenceNumber, IborFutureConvention convention)
	//  {
	//	return RelativeIborFutureTemplate.of(minimumPeriod, sequenceNumber, convention);
	//  }

	  /// <summary>
	  /// Obtains a template based on the specified convention using an absolute definition of time.
	  /// <para>
	  /// The future is selected from a sequence of futures based on a year-month.
	  /// In most cases, the date of the future will be in the same month as the specified month,
	  /// but this is not guaranteed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearMonth">  the year-month to use to select the future </param>
	  /// <param name="convention">  the future convention </param>
	  /// <returns> the template </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static IborFutureTemplate of(java.time.YearMonth yearMonth, IborFutureConvention convention)
	//  {
	//	return AbsoluteIborFutureTemplate.of(yearMonth, convention);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying index.
	  /// </summary>
	  /// <returns> the index </returns>
	  IborIndex Index {get;}

	  /// <summary>
	  /// Gets the market convention of the Ibor future.
	  /// </summary>
	  /// <returns> the convention </returns>
	  IborFutureConvention Convention {get;}

	  /// <summary>
	  /// Creates a trade based on this template.
	  /// <para>
	  /// This returns a trade based on the specified date.
	  /// The notional is unsigned, with the quantity determining the direction of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="securityId">  the identifier of the security </param>
	  /// <param name="quantity">  the number of contracts traded, positive if buying, negative if selling </param>
	  /// <param name="notional">  the notional amount of one future contract </param>
	  /// <param name="price">  the trade price </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> the trade </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  IborFutureTrade createTrade(LocalDate tradeDate, SecurityId securityId, double quantity, double notional, double price, ReferenceData refData);

	  /// <summary>
	  /// Calculates the reference date of the trade.
	  /// </summary>
	  /// <param name="tradeDate">  the date of the trade </param>
	  /// <param name="refData">  the reference data, used to resolve the date </param>
	  /// <returns> the future reference date </returns>
	  LocalDate calculateReferenceDateFromTradeDate(LocalDate tradeDate, ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the approximate maturity from the trade date.
	  /// <para>
	  /// This returns a year fraction that estimates the time to maturity.
	  /// For example, this might take the number of months between the trade date
	  /// and the date of the end of the future and divide it by 12.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <returns> the approximate time to maturity </returns>
	  double approximateMaturity(LocalDate tradeDate);

	}

}