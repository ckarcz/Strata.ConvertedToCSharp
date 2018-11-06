/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;

	/// <summary>
	/// The specification of how to build FX option volatilities.
	/// <para>
	/// This is the specification for a single volatility object, <seealso cref="FxOptionVolatilities"/>. 
	/// Each implementation of this interface must have the ability to create an instance of the respective implementation 
	/// of <seealso cref="FxOptionVolatilities"/>.
	/// </para>
	/// </summary>
	public interface FxOptionVolatilitiesSpecification
	{

	  /// <summary>
	  /// Gets the name of a set of FX option volatilities.
	  /// </summary>
	  /// <returns> the name </returns>
	  FxOptionVolatilitiesName Name {get;}

	  /// <summary>
	  /// Gets the currency pair.
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  CurrencyPair CurrencyPair {get;}

	  /// <summary>
	  /// Gets the volatilities nodes.
	  /// </summary>
	  /// <returns> the nodes </returns>
	  ImmutableList<FxOptionVolatilitiesNode> Nodes {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates FX option volatilities.
	  /// <para>
	  /// The number and ordering of {@code parameters} must be coherent to those of nodes, {@code #getNodes()}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="valuationDateTime">  the valuation date time </param>
	  /// <param name="parameters">  the parameters </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the volatilities </returns>
	  FxOptionVolatilities volatilities(ZonedDateTime valuationDateTime, DoubleArray parameters, ReferenceData refData);

	  /// <summary>
	  /// Obtains the inputs required to create the FX option volatilities.
	  /// </summary>
	  /// <returns> the inputs </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableList<com.opengamma.strata.market.observable.QuoteId> volatilitiesInputs()
	//  {
	//	return getNodes().stream().map(FxOptionVolatilitiesNode::getQuoteId).collect(toImmutableList());
	//  }

	  /// <summary>
	  /// Gets the number of parameters.
	  /// </summary>
	  /// <returns> the number of parameters </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getParameterCount()
	//  {
	//	return getNodes().size();
	//  }

	}

}