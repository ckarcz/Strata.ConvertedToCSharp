/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// An item in a portfolio.
	/// <para>
	/// This represents a single item in a portfolio.
	/// Typically a portfolio will consist of <seealso cref="Trade trades"/> and <seealso cref="Position positions"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface PortfolioItem : CalculationTarget
	{

	  /// <summary>
	  /// Gets the additional information about the portfolio item.
	  /// </summary>
	  /// <returns> the additional information </returns>
	  PortfolioItemInfo Info {get;}

	  /// <summary>
	  /// Gets the primary identifier for the portfolio item, optional.
	  /// <para>
	  /// The identifier is used to identify the portfolio item.
	  /// It will typically be an identifier in an external data system.
	  /// </para>
	  /// <para>
	  /// A portfolio item may have multiple active identifiers. Any identifier may be chosen here.
	  /// Certain uses of the identifier, such as storage in a database, require that the
	  /// identifier does not change over time, and this should be considered best practice.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the identifier, optional </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default java.util.Optional<com.opengamma.strata.basics.StandardId> getId()
	//  {
	//	return getInfo().getId();
	//  }

	  /// <summary>
	  /// Summarizes the portfolio item.
	  /// <para>
	  /// This provides a summary, including a human readable description.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the summary of the item </returns>
	  PortfolioItemSummary summarize();

	}

}