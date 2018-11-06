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
	/// Flag indicating how a financial instrument is to be settled.
	/// <para>
	/// Some financial instruments have a choice of settlement, either by cash or by
	/// delivering the underlying instrument that was tracked.
	/// For example, a swaption might be cash settled or produce an actual interest rate swap.
	/// </para>
	/// </summary>
	public sealed class SettlementType : NamedEnum
	{

	  /// <summary>
	  /// Cash settlement.
	  /// <para>
	  /// Cash amount is paid (by the short party to the long party) at expiry.
	  /// </para>
	  /// </summary>
	  public static readonly SettlementType CASH = new SettlementType("CASH", InnerEnum.CASH);
	  /// <summary>
	  /// Physical delivery.
	  /// <para>
	  /// The two parties enter into a new financial instrument at expiry.
	  /// </para>
	  /// </summary>
	  public static readonly SettlementType PHYSICAL = new SettlementType("PHYSICAL", InnerEnum.PHYSICAL);

	  private static readonly IList<SettlementType> valueList = new List<SettlementType>();

	  static SettlementType()
	  {
		  valueList.Add(CASH);
		  valueList.Add(PHYSICAL);
	  }

	  public enum InnerEnum
	  {
		  CASH,
		  PHYSICAL
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private SettlementType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<SettlementType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(SettlementType.class);

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
//ORIGINAL LINE: @FromString public static SettlementType of(String name)
	  public static SettlementType of(string name)
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


		public static IList<SettlementType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static SettlementType valueOf(string name)
		{
			foreach (SettlementType enumInstance in SettlementType.valueList)
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