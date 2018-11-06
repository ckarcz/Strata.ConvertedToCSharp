using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Builder to create {@code TradeInfo}.
	/// <para>
	/// This builder allows a <seealso cref="TradeInfo"/> to be created.
	/// </para>
	/// </summary>
	public sealed class TradeInfoBuilder
	{

	  /// <summary>
	  /// The primary identifier for the trade.
	  /// <para>
	  /// The identifier is used to identify the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private StandardId id_Renamed;
	  /// <summary>
	  /// The counterparty identifier.
	  /// <para>
	  /// An identifier used to specify the counterparty of the trade.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private StandardId counterparty_Renamed;
	  /// <summary>
	  /// The trade date.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private LocalDate tradeDate_Renamed;
	  /// <summary>
	  /// The trade time.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private LocalTime tradeTime_Renamed;
	  /// <summary>
	  /// The trade time-zone.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ZoneId zone_Renamed;
	  /// <summary>
	  /// The settlement date.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private LocalDate settlementDate_Renamed;
	  /// <summary>
	  /// The trade attributes.
	  /// <para>
	  /// Trade attributes, provide the ability to associate arbitrary information
	  /// with a trade in a key-value map.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<AttributeType<?>, Object> attributes = new java.util.HashMap<>();
	  private readonly IDictionary<AttributeType<object>, object> attributes = new Dictionary<AttributeType<object>, object>();

	  // creates an empty instance
	  internal TradeInfoBuilder()
	  {
	  }

	  // creates a populated instance
	  internal TradeInfoBuilder<T1>(StandardId id, StandardId counterparty, LocalDate tradeDate, LocalTime tradeTime, ZoneId zone, LocalDate settlementDate, IDictionary<T1> attributes)
	  {

		this.id_Renamed = id;
		this.counterparty_Renamed = counterparty;
		this.tradeDate_Renamed = tradeDate;
		this.tradeTime_Renamed = tradeTime;
		this.zone_Renamed = zone;
		this.settlementDate_Renamed = settlementDate;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.attributes.putAll(attributes);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sets the primary identifier for the trade, optional.
	  /// <para>
	  /// The identifier is used to identify the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="id">  the identifier </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder id(StandardId id)
	  {
		this.id_Renamed = id;
		return this;
	  }

	  /// <summary>
	  /// Sets the counterparty identifier, optional.
	  /// <para>
	  /// An identifier used to specify the counterparty of the trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="counterparty">  the counterparty </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder counterparty(StandardId counterparty)
	  {
		this.counterparty_Renamed = counterparty;
		return this;
	  }

	  /// <summary>
	  /// Sets the trade date, optional.
	  /// </summary>
	  /// <param name="tradeDate">  the trade date </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder tradeDate(LocalDate tradeDate)
	  {
		this.tradeDate_Renamed = tradeDate;
		return this;
	  }

	  /// <summary>
	  /// Sets the trade time, optional.
	  /// </summary>
	  /// <param name="tradeTime">  the trade time </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder tradeTime(LocalTime tradeTime)
	  {
		this.tradeTime_Renamed = tradeTime;
		return this;
	  }

	  /// <summary>
	  /// Sets the trade time-zone, optional.
	  /// </summary>
	  /// <param name="zone">  the trade zone </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder zone(ZoneId zone)
	  {
		this.zone_Renamed = zone;
		return this;
	  }

	  /// <summary>
	  /// Sets the settlement date, optional.
	  /// </summary>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> this, for chaining </returns>
	  public TradeInfoBuilder settlementDate(LocalDate settlementDate)
	  {
		this.settlementDate_Renamed = settlementDate;
		return this;
	  }

	  /// <summary>
	  /// Adds a trade attribute to the map of attributes.
	  /// <para>
	  /// The attribute is added using {@code Map.put(type, value)} semantics.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the value </param>
	  /// <param name="type">  the type providing meaning to the value </param>
	  /// <param name="value">  the value </param>
	  /// <returns> this, for chaining </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> TradeInfoBuilder addAttribute(AttributeType<T> type, T value)
	  public TradeInfoBuilder addAttribute<T>(AttributeType<T> type, T value)
	  {
		ArgChecker.notNull(type, "type");
		ArgChecker.notNull(value, "value");
		// ImmutableMap.Builder would not provide Map.put semantics
		attributes[type] = value;
		return this;
	  }

	  /// <summary>
	  /// Builds the trade information.
	  /// </summary>
	  /// <returns> the trade information </returns>
	  public TradeInfo build()
	  {
		return new TradeInfo(id_Renamed, counterparty_Renamed, tradeDate_Renamed, tradeTime_Renamed, zone_Renamed, settlementDate_Renamed, attributes);
	  }

	}

}