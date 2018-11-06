using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The accrual start for credit default swaps.
	/// <para>
	/// The accrual start is the next day or the previous IMM date.
	/// </para>
	/// </summary>
	public sealed class AccrualStart : NamedEnum
	{

	  /// <summary>
	  /// The accrual starts on T+1, i.e., the next day.
	  /// </summary>
	  public static readonly AccrualStart NEXT_DAY = new AccrualStart("NEXT_DAY", InnerEnum.NEXT_DAY);

	  /// <summary>
	  /// The accrual starts on the previous IMM date.
	  /// <para>
	  /// The IMM date must be computed based on <seealso cref="CdsImmDateLogic"/>.
	  /// </para>
	  /// </summary>
	  public static readonly AccrualStart IMM_DATE = new AccrualStart("IMM_DATE", InnerEnum.IMM_DATE);

	  private static readonly IList<AccrualStart> valueList = new List<AccrualStart>();

	  static AccrualStart()
	  {
		  valueList.Add(NEXT_DAY);
		  valueList.Add(IMM_DATE);
	  }

	  public enum InnerEnum
	  {
		  NEXT_DAY,
		  IMM_DATE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private AccrualStart(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<AccrualStart> NAMES = com.opengamma.strata.collect.named.EnumNames.of(AccrualStart.class);

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
//ORIGINAL LINE: @FromString public static AccrualStart of(String name)
	  public static AccrualStart of(string name)
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


		public static IList<AccrualStart> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static AccrualStart valueOf(string name)
		{
			foreach (AccrualStart enumInstance in AccrualStart.valueList)
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