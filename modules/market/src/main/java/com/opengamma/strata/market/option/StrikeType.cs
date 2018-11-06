using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.option
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The type of a strike.
	/// <para>
	/// The strike of option instruments is represented in different ways.
	/// For example, the strike types include delta, moneyness, log-moneyness, and strike itself.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class StrikeType : TypedString<StrikeType>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The type of a simple strike. </summary>
	  /// <seealso cref= SimpleStrike </seealso>
	  public static readonly StrikeType STRIKE = of("Strike");
	  /// <summary>
	  /// The type of a strike based on absolute delta. </summary>
	  /// <seealso cref= DeltaStrike </seealso>
	  public static readonly StrikeType DELTA = of("Delta");
	  /// <summary>
	  /// The type of a strike based on moneyness, defined as {@code strike/forward}. </summary>
	  /// <seealso cref= MoneynessStrike </seealso>
	  public static readonly StrikeType MONEYNESS = of("Moneyness");
	  /// <summary>
	  /// The type of a strike based on log-moneyness, defined as the {@code ln(strike/forward)}. </summary>
	  /// <seealso cref= LogMoneynessStrike </seealso>
	  public static readonly StrikeType LOG_MONEYNESS = of("LogMoneyness");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Strike types may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> the type with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static StrikeType of(String name)
	  public static StrikeType of(string name)
	  {
		return new StrikeType(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  private StrikeType(string name) : base(name)
	  {
	  }

	}

}