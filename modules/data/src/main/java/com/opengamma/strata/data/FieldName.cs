using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The name of a field in a market data record.
	/// <para>
	/// Market data is typically provided as a record containing multiple fields. Each field contains an item
	/// of data. The record is identified by a unique ID and the fields are identified by name.
	/// Therefore an item of market data is uniquely identified by the combination its ID and field name.
	/// </para>
	/// <para>
	/// Different market data providers use different sets of field names. The names in this class are
	/// not specific to any provider, and are mapped to the provider field names by the market data
	/// system. This allows calculations to request an item of data using its field name, such as
	/// "closing price", without having to know which data provider it is coming from.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class FieldName : TypedString<FieldName>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The field name for the market value - 'MarketValue'.
	  /// <para>
	  /// This is used to refer to the standard market quote for the identifier.
	  /// It is typically used as the default when no field name is specified.
	  /// </para>
	  /// </summary>
	  public static readonly FieldName MARKET_VALUE = of("MarketValue");
	  /// <summary>
	  /// The field name for the settlement price - 'SettlementPrice'.
	  /// <para>
	  /// This is used to refer to the daily settlement price used in margining.
	  /// </para>
	  /// </summary>
	  public static readonly FieldName SETTLEMENT_PRICE = of("SettlementPrice");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Field names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> a field with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FieldName of(String name)
	  public static FieldName of(string name)
	  {
		return new FieldName(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  private FieldName(string name) : base(name)
	  {
	  }

	}

}