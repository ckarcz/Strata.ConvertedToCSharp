using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using Result = com.opengamma.strata.collect.result.Result;
	using ObservableId = com.opengamma.strata.data.ObservableId;

	/// <summary>
	/// A provider of observable market data.
	/// <para>
	/// This plugin point allows a market data supplier to be provided.
	/// Implementations might request data from an external data provider, such as Bloomberg or Reuters.
	/// </para>
	/// </summary>
	public interface ObservableDataProvider
	{

	  /// <summary>
	  /// Obtains an instance that provides no market data.
	  /// <para>
	  /// When invoked, the provider will return a map where every requested identifier is a failure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a provider that returns failures if invoked </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ObservableDataProvider none()
	//  {
	//	return identifiers -> identifiers.stream().collect(toImmutableMap(id -> id, id -> Result.failure(FailureReason.MISSING_DATA, "No observable market data provider configured, unable to provide data for '{}'", id)));
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Provides market data for the specified identifiers.
	  /// <para>
	  /// The implementation will provide market data for each identifier.
	  /// If market data cannot be obtained for an identifier, a failure will be returned.
	  /// The returned map must contain one entry for each identifier that was requested.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="identifiers">  the market data identifiers to find </param>
	  /// <returns> the map of market data values, keyed by identifier </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract java.util.Map<com.opengamma.strata.data.ObservableId, com.opengamma.strata.collect.result.Result<double>> provideObservableData(java.util.Set<? extends com.opengamma.strata.data.ObservableId> identifiers);
	  IDictionary<ObservableId, Result<double>> provideObservableData<T1>(ISet<T1> identifiers);

	}

}