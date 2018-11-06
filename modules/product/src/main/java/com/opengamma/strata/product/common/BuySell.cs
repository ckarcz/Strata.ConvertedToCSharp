using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Flag indicating whether a trade is "buy" or "sell".
	/// <para>
	/// Specifies whether the financial instrument is buy-side or sell-side.
	/// For example, in a Forward Rate Agreement the buyer receives the floating rate
	/// of interest in exchange for a fixed rate, whereas the seller pays the floating rate.
	/// This flag is stored on the instrument to indicate whether it was bought or sold.
	/// </para>
	/// </summary>
	public sealed class BuySell : NamedEnum
	{

	  /// <summary>
	  /// Buy.
	  /// </summary>
	  public static readonly BuySell BUY = new BuySell("BUY", InnerEnum.BUY);
	  /// <summary>
	  /// Sell.
	  /// </summary>
	  public static readonly BuySell SELL = new BuySell("SELL", InnerEnum.SELL);

	  private static readonly IList<BuySell> valueList = new List<BuySell>();

	  static BuySell()
	  {
		  valueList.Add(BUY);
		  valueList.Add(SELL);
	  }

	  public enum InnerEnum
	  {
		  BUY,
		  SELL
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private BuySell(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<BuySell> NAMES = com.opengamma.strata.collect.named.EnumNames.of(BuySell.class);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Parsing handles the mixed case form produced by <seealso cref="#toString()"/> and
	  /// the upper and lower case variants of the enum constant name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name to parse </param>
	  /// <returns> the type </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static BuySell of(String name)
	  public static BuySell of(string name)
	  {
		return NAMES.parse(name);
	  }

	  /// <summary>
	  /// Converts a boolean "is buy" flag to the enum value.
	  /// </summary>
	  /// <param name="isBuy">  the buy flag, true for buy, false for sell </param>
	  /// <returns> the equivalent enum </returns>
	  public static BuySell ofBuy(bool isBuy)
	  {
		return isBuy ? BUY : SELL;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Normalizes the specified notional amount using this buy/sell rule.
	  /// <para>
	  /// This returns a positive signed amount if this is 'buy', and a negative signed amount 
	  /// if this is 'sell'. This effectively normalizes the input notional
	  /// to the buy/sell sign conventions of this library.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to adjust </param>
	  /// <returns> the adjusted amount </returns>
	  public double normalize(double amount)
	  {
		double normalized = Math.Abs(amount);
		return Buy ? normalized : -normalized;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Buy'.
	  /// </summary>
	  /// <returns> true if buy, false if sell </returns>
	  public bool Buy
	  {
		  get
		  {
			return this == BUY;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Sell'.
	  /// </summary>
	  /// <returns> true if sell, false if buy </returns>
	  public bool Sell
	  {
		  get
		  {
			return this == SELL;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return NAMES.format(this);
	  }


		public static IList<BuySell> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static BuySell valueOf(string name)
		{
			foreach (BuySell enumInstance in BuySell.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}