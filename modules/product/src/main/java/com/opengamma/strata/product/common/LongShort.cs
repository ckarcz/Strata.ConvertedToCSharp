using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Flag indicating whether a trade is "long" or "short".
	/// <para>
	/// A long position is one where a financial instrument is bought with the expectation
	/// that its value will rise. A short position is the opposite where the expectation
	/// is that its value will fall, usually applied to the sale of a borrowed asset.
	/// </para>
	/// </summary>
	public sealed class LongShort : NamedEnum
	{

	  /// <summary>
	  /// Long.
	  /// </summary>
	  public static readonly LongShort LONG = new LongShort("LONG", InnerEnum.LONG, 1);
	  /// <summary>
	  /// Short.
	  /// </summary>
	  public static readonly LongShort SHORT = new LongShort("SHORT", InnerEnum.SHORT, -1);

	  private static readonly IList<LongShort> valueList = new List<LongShort>();

	  static LongShort()
	  {
		  valueList.Add(LONG);
		  valueList.Add(SHORT);
	  }

	  public enum InnerEnum
	  {
		  LONG,
		  SHORT
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<LongShort> NAMES = com.opengamma.strata.collect.named.EnumNames.of(LongShort.class);

	  /// <summary>
	  /// True if long, used to avoid a branch.
	  /// </summary>
	  private readonly bool isLong;
	  /// <summary>
	  /// The sign, used to avoid a branch.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly int sign_Renamed;

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
//ORIGINAL LINE: @FromString public static LongShort of(String name)
	  public static LongShort of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a boolean "is long" flag to the enum value.
	  /// </summary>
	  /// <param name="isLong">  the long flag, true for long, false for short </param>
	  /// <returns> the equivalent enum </returns>
	  public static LongShort ofLong(bool isLong)
	  {
		return isLong ? LONG : SHORT;
	  }

	  // Restricted constructor
	  private LongShort(string name, InnerEnum innerEnum, int sign)
	  {
		this.isLong = (sign == 1);
		this.sign_Renamed = sign;

		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Long'.
	  /// </summary>
	  /// <returns> true if long, false if short </returns>
	  public bool Long
	  {
		  get
		  {
			return isLong;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Short'.
	  /// </summary>
	  /// <returns> true if short, false if long </returns>
	  public bool Short
	  {
		  get
		  {
			return !isLong;
		  }
	  }

	  /// <summary>
	  /// Returns the sign, where 'Long' returns 1 and 'Short' returns -1.
	  /// </summary>
	  /// <returns> 1 if long, -1 if short </returns>
	  public int sign()
	  {
		return sign_Renamed;
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


		public static IList<LongShort> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static LongShort valueOf(string name)
		{
			foreach (LongShort enumInstance in LongShort.valueList)
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