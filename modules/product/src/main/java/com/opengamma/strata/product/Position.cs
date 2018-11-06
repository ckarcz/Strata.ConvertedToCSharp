/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A position in a security.
	/// <para>
	/// This is used to represent the total quantity of a <seealso cref="Security"/>.
	/// A position is effectively the sum of one or more trades.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// </para>
	/// </summary>
	public interface Position : PortfolioItem, SecurityQuantity
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PortfolioItemSummary summarize()
	//  {
	//	// AAPL x 200
	//	String description = getSecurityId().getStandardId().getValue() + " x " + SummarizerUtils.value(getQuantity());
	//	return SummarizerUtils.summary(this, ProductType.SECURITY, description);
	//  }

	  /// <summary>
	  /// Gets the standard position information.
	  /// <para>
	  /// All positions contain this standard set of information.
	  /// It includes the identifier and an extensible data map.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the position information </returns>
	  PositionInfo Info {get;}

	  /// <summary>
	  /// Gets the identifier of the underlying security.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the security identifier </returns>
	  SecurityId SecurityId {get;}

	  /// <summary>
	  /// Gets the net quantity of the security.
	  /// <para>
	  /// This returns the <i>net</i> quantity of the underlying security.
	  /// The result is positive if the net position is <i>long</i> and negative
	  /// if the net position is <i>short</i>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the net quantity of the underlying security </returns>
	  double Quantity {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  Position withInfo(PositionInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  Position withQuantity(double quantity);

	}

}