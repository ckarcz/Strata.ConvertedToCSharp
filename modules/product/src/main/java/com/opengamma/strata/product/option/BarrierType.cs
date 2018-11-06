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
	/// The barrier type of barrier event.
	/// <para>
	/// This defines the barrier type of <seealso cref="Barrier"/>.
	/// </para>
	/// </summary>
	public sealed class BarrierType : NamedEnum
	{

	  /// <summary>
	  /// Down 
	  /// </summary>
	  public static readonly BarrierType DOWN = new BarrierType("DOWN", InnerEnum.DOWN);
	  /// <summary>
	  /// Up 
	  /// </summary>
	  public static readonly BarrierType UP = new BarrierType("UP", InnerEnum.UP);

	  private static readonly IList<BarrierType> valueList = new List<BarrierType>();

	  static BarrierType()
	  {
		  valueList.Add(DOWN);
		  valueList.Add(UP);
	  }

	  public enum InnerEnum
	  {
		  DOWN,
		  UP
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private BarrierType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<BarrierType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(BarrierType.class);

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
//ORIGINAL LINE: @FromString public static BarrierType of(String name)
	  public static BarrierType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Down'.
	  /// </summary>
	  /// <returns> true if down, false if up </returns>
	  public bool Down
	  {
		  get
		  {
			return this == DOWN;
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


		public static IList<BarrierType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static BarrierType valueOf(string name)
		{
			foreach (BarrierType enumInstance in BarrierType.valueList)
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