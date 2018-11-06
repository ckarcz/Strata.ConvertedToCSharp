using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The expiry type of an Exchange Traded Derivative (ETD) product.
	/// <para>
	/// Most ETDs expire monthly, on a date calculated via a formula.
	/// Some ETDs expire weekly, or on a specific date, see <seealso cref="EtdVariant"/> for more details.
	/// </para>
	/// </summary>
	public sealed class EtdExpiryType : NamedEnum
	{

	  /// <summary>
	  /// The ETD expires once a month on a standardized day.
	  /// </summary>
	  public static readonly EtdExpiryType MONTHLY = new EtdExpiryType("MONTHLY", InnerEnum.MONTHLY);
	  /// <summary>
	  /// The ETD expires in a specific week of the month.
	  /// The week is specified by the date code in <seealso cref="EtdVariant"/>.
	  /// </summary>
	  public static readonly EtdExpiryType WEEKLY = new EtdExpiryType("WEEKLY", InnerEnum.WEEKLY);
	  /// <summary>
	  /// The ETD expires on a specified day-of-month.
	  /// The day-of-month is specified by the date code in <seealso cref="EtdVariant"/>.
	  /// </summary>
	  public static readonly EtdExpiryType DAILY = new EtdExpiryType("DAILY", InnerEnum.DAILY);

	  private static readonly IList<EtdExpiryType> valueList = new List<EtdExpiryType>();

	  static EtdExpiryType()
	  {
		  valueList.Add(MONTHLY);
		  valueList.Add(WEEKLY);
		  valueList.Add(DAILY);
	  }

	  public enum InnerEnum
	  {
		  MONTHLY,
		  WEEKLY,
		  DAILY
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private EtdExpiryType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<EtdExpiryType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(EtdExpiryType.class);

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
//ORIGINAL LINE: @FromString public static EtdExpiryType of(String name)
	  public static EtdExpiryType of(string name)
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


		public static IList<EtdExpiryType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static EtdExpiryType valueOf(string name)
		{
			foreach (EtdExpiryType enumInstance in EtdExpiryType.valueList)
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