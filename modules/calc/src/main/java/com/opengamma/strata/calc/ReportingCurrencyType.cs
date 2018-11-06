using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The available types of reporting currency.
	/// <para>
	/// There are three options - 'Specific', 'Natural' and 'None'.
	/// </para>
	/// </summary>
	public sealed class ReportingCurrencyType : NamedEnum
	{

	  /// <summary>
	  /// The specific reporting currency.
	  /// See <seealso cref="ReportingCurrency#of(Currency)"/>.
	  /// </summary>
	  public static readonly ReportingCurrencyType SPECIFIC = new ReportingCurrencyType("SPECIFIC", InnerEnum.SPECIFIC);
	  /// <summary>
	  /// The "natural" reporting currency.
	  /// See <seealso cref="ReportingCurrency#NATURAL"/>.
	  /// </summary>
	  public static readonly ReportingCurrencyType NATURAL = new ReportingCurrencyType("NATURAL", InnerEnum.NATURAL);
	  /// <summary>
	  /// No currency conversion is to be performed.
	  /// See <seealso cref="ReportingCurrency#NONE"/>.
	  /// </summary>
	  public static readonly ReportingCurrencyType NONE = new ReportingCurrencyType("NONE", InnerEnum.NONE);

	  private static readonly IList<ReportingCurrencyType> valueList = new List<ReportingCurrencyType>();

	  static ReportingCurrencyType()
	  {
		  valueList.Add(SPECIFIC);
		  valueList.Add(NATURAL);
		  valueList.Add(NONE);
	  }

	  public enum InnerEnum
	  {
		  SPECIFIC,
		  NATURAL,
		  NONE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private ReportingCurrencyType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<ReportingCurrencyType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(ReportingCurrencyType.class);

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
//ORIGINAL LINE: @FromString public static ReportingCurrencyType of(String name)
	  public static ReportingCurrencyType of(string name)
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


		public static IList<ReportingCurrencyType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static ReportingCurrencyType valueOf(string name)
		{
			foreach (ReportingCurrencyType enumInstance in ReportingCurrencyType.valueList)
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