using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.io
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Alignment of the data within an ASCII table.
	/// <para>
	/// See <seealso cref="AsciiTable"/> for more details.
	/// </para>
	/// </summary>
	public sealed class AsciiTableAlignment : NamedEnum
	{

	  /// <summary>
	  /// Align left.
	  /// </summary>
	  public static readonly AsciiTableAlignment LEFT = new AsciiTableAlignment("LEFT", InnerEnum.LEFT);
	  /// <summary>
	  /// Align right.
	  /// </summary>
	  public static readonly AsciiTableAlignment RIGHT = new AsciiTableAlignment("RIGHT", InnerEnum.RIGHT);

	  private static readonly IList<AsciiTableAlignment> valueList = new List<AsciiTableAlignment>();

	  static AsciiTableAlignment()
	  {
		  valueList.Add(LEFT);
		  valueList.Add(RIGHT);
	  }

	  public enum InnerEnum
	  {
		  LEFT,
		  RIGHT
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private AsciiTableAlignment(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<AsciiTableAlignment> NAMES = com.opengamma.strata.collect.named.EnumNames.of(AsciiTableAlignment.class);

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
//ORIGINAL LINE: @FromString public static AsciiTableAlignment of(String name)
	  public static AsciiTableAlignment of(string name)
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


		public static IList<AsciiTableAlignment> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static AsciiTableAlignment valueOf(string name)
		{
			foreach (AsciiTableAlignment enumInstance in AsciiTableAlignment.valueList)
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