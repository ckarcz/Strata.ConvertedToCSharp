using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The type of a portfolio item.
	/// <para>
	/// This allows trades, positions and sensitivities to be separated.
	/// </para>
	/// </summary>
	public sealed class PortfolioItemType : NamedEnum
	{

	  /// <summary>
	  /// A trade.
	  /// <para>
	  /// A trade is a transaction that occurred on a specific date between two counterparties.
	  /// See <seealso cref="Trade"/>.
	  /// </para>
	  /// </summary>
	  public static readonly PortfolioItemType TRADE = new PortfolioItemType("TRADE", InnerEnum.TRADE);
	  /// <summary>
	  /// A position.
	  /// <para>
	  /// A position is effectively the sum of one or more trades in a <seealso cref="Security"/>.
	  /// See <seealso cref="Position"/>.
	  /// </para>
	  /// </summary>
	  public static readonly PortfolioItemType POSITION = new PortfolioItemType("POSITION", InnerEnum.POSITION);
	  /// <summary>
	  /// Risk expressed as sensitivities.
	  /// </summary>
	  public static readonly PortfolioItemType SENSITIVITIES = new PortfolioItemType("SENSITIVITIES", InnerEnum.SENSITIVITIES);
	  /// <summary>
	  /// Any other kind of portfolio item.
	  /// </summary>
	  public static readonly PortfolioItemType OTHER = new PortfolioItemType("OTHER", InnerEnum.OTHER);

	  private static readonly IList<PortfolioItemType> valueList = new List<PortfolioItemType>();

	  static PortfolioItemType()
	  {
		  valueList.Add(TRADE);
		  valueList.Add(POSITION);
		  valueList.Add(SENSITIVITIES);
		  valueList.Add(OTHER);
	  }

	  public enum InnerEnum
	  {
		  TRADE,
		  POSITION,
		  SENSITIVITIES,
		  OTHER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PortfolioItemType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PortfolioItemType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PortfolioItemType.class);

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
//ORIGINAL LINE: @FromString public static PortfolioItemType of(String name)
	  public static PortfolioItemType of(string name)
	  {
		return NAMES.parse(name);
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


		public static IList<PortfolioItemType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PortfolioItemType valueOf(string name)
		{
			foreach (PortfolioItemType enumInstance in PortfolioItemType.valueList)
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