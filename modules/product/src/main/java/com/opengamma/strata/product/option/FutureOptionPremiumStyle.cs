using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.option
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The style of premium for an option on a futures contract.
	/// <para>
	/// There are two styles of future options, one with daily margining, and one
	/// with an up-front premium. This class specifies the types.
	/// </para>
	/// </summary>
	public sealed class FutureOptionPremiumStyle : NamedEnum
	{

	  /// <summary>
	  /// The "DailyMargin" style, used where the option has daily margining.
	  /// This is also known as <i>future-style margining</i>.
	  /// </summary>
	  public static readonly FutureOptionPremiumStyle DAILY_MARGIN = new FutureOptionPremiumStyle("DAILY_MARGIN", InnerEnum.DAILY_MARGIN);
	  /// <summary>
	  /// The "UpfrontPremium" style, used where the option has an upfront premium.
	  /// This is also known as <i>equity-style margining</i>.
	  /// </summary>
	  public static readonly FutureOptionPremiumStyle UPFRONT_PREMIUM = new FutureOptionPremiumStyle("UPFRONT_PREMIUM", InnerEnum.UPFRONT_PREMIUM);

	  private static readonly IList<FutureOptionPremiumStyle> valueList = new List<FutureOptionPremiumStyle>();

	  static FutureOptionPremiumStyle()
	  {
		  valueList.Add(DAILY_MARGIN);
		  valueList.Add(UPFRONT_PREMIUM);
	  }

	  public enum InnerEnum
	  {
		  DAILY_MARGIN,
		  UPFRONT_PREMIUM
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private FutureOptionPremiumStyle(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FutureOptionPremiumStyle> NAMES = com.opengamma.strata.collect.named.EnumNames.of(FutureOptionPremiumStyle.class);

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
//ORIGINAL LINE: @FromString public static FutureOptionPremiumStyle of(String name)
	  public static FutureOptionPremiumStyle of(string name)
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


		public static IList<FutureOptionPremiumStyle> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FutureOptionPremiumStyle valueOf(string name)
		{
			foreach (FutureOptionPremiumStyle enumInstance in FutureOptionPremiumStyle.valueList)
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