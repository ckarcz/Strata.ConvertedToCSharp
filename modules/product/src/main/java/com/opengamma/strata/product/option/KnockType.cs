using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// The knock type of barrier event.
	/// <para>
	/// This defines the knock type of <seealso cref="Barrier"/>.
	/// </para>
	/// </summary>
	public sealed class KnockType : NamedEnum
	{

	  /// <summary>
	  /// Knock-in 
	  /// </summary>
	  public static readonly KnockType KNOCK_IN = new KnockType("KNOCK_IN", InnerEnum.KNOCK_IN);
	  /// <summary>
	  /// Knock-out 
	  /// </summary>
	  public static readonly KnockType KNOCK_OUT = new KnockType("KNOCK_OUT", InnerEnum.KNOCK_OUT);

	  private static readonly IList<KnockType> valueList = new List<KnockType>();

	  static KnockType()
	  {
		  valueList.Add(KNOCK_IN);
		  valueList.Add(KNOCK_OUT);
	  }

	  public enum InnerEnum
	  {
		  KNOCK_IN,
		  KNOCK_OUT
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private KnockType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<KnockType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(KnockType.class);

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
//ORIGINAL LINE: @FromString public static KnockType of(String name)
	  public static KnockType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Knock-in'.
	  /// </summary>
	  /// <returns> true if knock-in, false if knock-out </returns>
	  public bool KnockIn
	  {
		  get
		  {
			return this == KNOCK_IN;
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


		public static IList<KnockType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static KnockType valueOf(string name)
		{
			foreach (KnockType enumInstance in KnockType.valueList)
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