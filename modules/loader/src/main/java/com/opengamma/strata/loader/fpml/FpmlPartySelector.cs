using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.fpml
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ListMultimap = com.google.common.collect.ListMultimap;

	/// <summary>
	/// Finds the party representing "us" in FpML.
	/// <para>
	/// The FpML data structure is neutral as to the direction of a trade, choosing to
	/// represent the two parties throughout the structure. The Strata data model takes
	/// the opposite view, with each trade stored with Pay/Receive or Buy/Sell concepts
	/// expressed from "our" point of view. This selector is used to bridge the gap,
	/// picking the party that represents "us" in the FpML data.
	/// </para>
	/// </summary>
	public interface FpmlPartySelector
	{

	  /// <summary>
	  /// Returns a selector that will choose any party from the trade.
	  /// <para>
	  /// The party chosen varies by trade type and will be consistent for a given input file.
	  /// For example, in a FRA the 'Buy' party will be chosen, whereas in a swap
	  /// the 'Pay' party of the first leg will be chosen.
	  /// In general, it is not recommended to rely on this implementation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the selector that will choose the party from the first leg </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FpmlPartySelector any()
	//  {
	//	return FpmlDocument.ANY_SELECTOR;
	//  }

	  /// <summary>
	  /// Returns a selector that matches the specified party ID.
	  /// <para>
	  /// This examines the party IDs included in the FpML document and returns the
	  /// href ID for the party that exactly matches the specified party ID.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="partyId">  the party ID to match </param>
	  /// <returns> the selector that will choose the party based on the specified party ID </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FpmlPartySelector matching(String partyId)
	//  {
	//	return allParties -> allParties.entries().stream().filter(e -> e.getValue().equals(partyId)).map(e -> e.getKey()).distinct().collect(toImmutableList());
	//  }

	  /// <summary>
	  /// Returns a selector that matches the specified party ID regular expression.
	  /// <para>
	  /// This examines the party IDs included in the FpML document and returns the
	  /// href ID for the party that matches the specified party ID regular expression.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="partyIdRegex">  the party ID regular expression to match </param>
	  /// <returns> the selector that will choose the party based on the specified party ID </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FpmlPartySelector matchingRegex(java.util.regex.Pattern partyIdRegex)
	//  {
	//	return allParties -> allParties.entries().stream().filter(e -> partyIdRegex.matcher(e.getValue()).matches()).map(e -> e.getKey()).distinct().collect(toImmutableList());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Given a map of all parties in the FpML document, extract those that
	  /// represent "our" side of the trade.
	  /// </summary>
	  /// <param name="allParties">  the multimap of party href id to associated partyId </param>
	  /// <returns> the party href ids to use, empty if unable to find "our" party </returns>
	  IList<string> selectParties(ListMultimap<string, string> allParties);

	}

}