using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The protection start of the day.
	/// <para>
	/// When the protection starts on the start date.
	/// </para>
	/// </summary>
	public sealed class ProtectionStartOfDay : NamedEnum
	{

	  /// <summary>
	  /// Beginning of the start day. 
	  /// <para>
	  /// The protection starts at the beginning of the day. 
	  /// </para>
	  /// </summary>
	  public static readonly ProtectionStartOfDay BEGINNING = new ProtectionStartOfDay("BEGINNING", InnerEnum.BEGINNING);
	  /// <summary>
	  /// None.
	  /// <para>
	  /// The protection start is not specified. 
	  /// The CDS is priced based on the default date logic in respective model implementation.
	  /// </para>
	  /// </summary>
	  public static readonly ProtectionStartOfDay NONE = new ProtectionStartOfDay("NONE", InnerEnum.NONE);

	  private static readonly IList<ProtectionStartOfDay> valueList = new List<ProtectionStartOfDay>();

	  static ProtectionStartOfDay()
	  {
		  valueList.Add(BEGINNING);
		  valueList.Add(NONE);
	  }

	  public enum InnerEnum
	  {
		  BEGINNING,
		  NONE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private ProtectionStartOfDay(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<ProtectionStartOfDay> NAMES = com.opengamma.strata.collect.named.EnumNames.of(ProtectionStartOfDay.class);

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
//ORIGINAL LINE: @FromString public static ProtectionStartOfDay of(String name)
	  public static ProtectionStartOfDay of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Check if the type is 'Beginning'.
	  /// </summary>
	  /// <returns> true if beginning, false otherwise </returns>
	  public bool Beginning
	  {
		  get
		  {
			return this == BEGINNING;
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


		public static IList<ProtectionStartOfDay> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static ProtectionStartOfDay valueOf(string name)
		{
			foreach (ProtectionStartOfDay enumInstance in ProtectionStartOfDay.valueList)
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