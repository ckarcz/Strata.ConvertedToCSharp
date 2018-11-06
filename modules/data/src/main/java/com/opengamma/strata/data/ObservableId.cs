/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// A market data identifier that identifies observable data.
	/// <para>
	/// Observable data can be requested from an external data provider, for example Bloomberg or Reuters.
	/// </para>
	/// <para>
	/// An observable ID contains three pieces of information:
	/// <ul>
	///   <li>A <seealso cref="StandardId"/> identifying the market data. This ID can come from any system. It might be
	///   an OpenGamma ID, for example {@code OpenGammaIndex~GBP_LIBOR_3M}, or it can be an ID from a market
	///   data source, for example {@code BloombergTicker~AAPL US Equity}.</li>
	/// 
	///   <li>A <seealso cref="FieldName"/> indicating the field in the market data record containing the data. See
	///   the {@code FieldName} documentation for more details.</li>
	/// 
	///   <li>A <seealso cref="ObservableSource"/> indicating where the data should come from.
	///   It is important to note that the standard ID is not necessarily related to the source.
	///   There is typically a mapping step in the market data system that maps the standard ID
	///   into an ID that can be used to look up the data in the source.</li>
	/// </ul>
	/// </para>
	/// <para>
	/// Observable data is always represented by a {@code double}.
	/// </para>
	/// </summary>
	public interface ObservableId : MarketDataId<double>
	{

	  /// <summary>
	  /// Gets the type of data this identifier refers to, which is a {@code double}.
	  /// </summary>
	  /// <returns> the type of the market data this identifier refers to, {@code Double.class} </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class<double> getMarketDataType()
	//  {
	//	return Double.class;
	//  }

	  /// <summary>
	  /// Gets the standard identifier identifying the data.
	  /// <para>
	  /// The identifier may be the identifier used to identify the item in an underlying data provider,
	  /// for example a Bloomberg ticker. It also may be any arbitrary unique identifier that can be resolved
	  /// to one or more data provider identifiers which are used to request the data from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a standard identifier, such as a ticker, to identify the desired data </returns>
	  StandardId StandardId {get;}

	  /// <summary>
	  /// Gets the field name in the market data record that contains the market data item.
	  /// <para>
	  /// Each ticker typically exposes many different fields. The field name specifies the desired field.
	  /// For example, the <seealso cref="FieldName#MARKET_VALUE market value"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the field name in the market data record that contains the market data item </returns>
	  FieldName FieldName {get;}

	  /// <summary>
	  /// Gets the source of market data from which the market data should be retrieved.
	  /// <para>
	  /// The source identifies the source of data, such as Bloomberg or Reuters.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the source from which the market data should be retrieved </returns>
	  ObservableSource ObservableSource {get;}

	  /// <summary>
	  /// Returns an identifier equivalent to this with the specified source.
	  /// </summary>
	  /// <param name="obsSource">  the source of market data </param>
	  /// <returns> the observable identifier </returns>
	  ObservableId withObservableSource(ObservableSource obsSource);

	}

}