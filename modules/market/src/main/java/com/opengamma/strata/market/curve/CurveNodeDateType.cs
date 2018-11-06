using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The types of curve node date.
	/// <para>
	/// This is used to identify how the date of a node should be calculated.
	/// </para>
	/// </summary>
	public sealed class CurveNodeDateType : NamedEnum
	{

	  /// <summary>
	  /// Defines a fixed date that is externally provided.
	  /// </summary>
	  public static readonly CurveNodeDateType FIXED = new CurveNodeDateType("FIXED", InnerEnum.FIXED);
	  /// <summary>
	  /// Defines the end date of the trade.
	  /// This will typically be the last accrual date, but may be any suitable
	  /// date at the end of the trade.
	  /// </summary>
	  public static readonly CurveNodeDateType END = new CurveNodeDateType("END", InnerEnum.END);
	  /// <summary>
	  /// Defines the last fixing date referenced in the trade.
	  /// Used only for instruments referencing an Ibor index.
	  /// </summary>
	  public static readonly CurveNodeDateType LAST_FIXING = new CurveNodeDateType("LAST_FIXING", InnerEnum.LAST_FIXING);

	  private static readonly IList<CurveNodeDateType> valueList = new List<CurveNodeDateType>();

	  static CurveNodeDateType()
	  {
		  valueList.Add(FIXED);
		  valueList.Add(END);
		  valueList.Add(LAST_FIXING);
	  }

	  public enum InnerEnum
	  {
		  FIXED,
		  END,
		  LAST_FIXING
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CurveNodeDateType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CurveNodeDateType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CurveNodeDateType.class);

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
//ORIGINAL LINE: @FromString public static CurveNodeDateType of(String name)
	  public static CurveNodeDateType of(string name)
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


		public static IList<CurveNodeDateType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CurveNodeDateType valueOf(string name)
		{
			foreach (CurveNodeDateType enumInstance in CurveNodeDateType.valueList)
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