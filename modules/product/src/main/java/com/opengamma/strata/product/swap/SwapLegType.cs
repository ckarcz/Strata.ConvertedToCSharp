using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The type of a swap leg.
	/// <para>
	/// This provides a high-level categorization of a swap leg.
	/// This is useful when it is necessary to find a specific leg.
	/// </para>
	/// </summary>
	public sealed class SwapLegType : NamedEnum
	{

	  /// <summary>
	  /// A fixed rate swap leg.
	  /// All periods in this leg must have a fixed rate.
	  /// </summary>
	  public static readonly SwapLegType FIXED = new SwapLegType("FIXED", InnerEnum.FIXED);
	  /// <summary>
	  /// A floating rate swap leg based on an Ibor index.
	  /// <para>
	  /// This kind of leg may include some fixed periods, such as in a stub or
	  /// where the first rate is specified in the contract.
	  /// </para>
	  /// </summary>
	  public static readonly SwapLegType IBOR = new SwapLegType("IBOR", InnerEnum.IBOR);
	  /// <summary>
	  /// A floating rate swap leg based on an Overnight index.
	  /// <para>
	  /// This kind of leg may include some fixed periods, such as in a stub or
	  /// where the first rate is specified in the contract.
	  /// </para>
	  /// </summary>
	  public static readonly SwapLegType OVERNIGHT = new SwapLegType("OVERNIGHT", InnerEnum.OVERNIGHT);
	  /// <summary>
	  /// A floating rate swap leg based on an price index.
	  /// <para>
	  /// This kind of leg may include some reference dates 
	  /// where the index rate is specified.
	  /// </para>
	  /// </summary>
	  public static readonly SwapLegType INFLATION = new SwapLegType("INFLATION", InnerEnum.INFLATION);
	  /// <summary>
	  /// A swap leg that is not based on a Fixed, Ibor, Overnight or Inflation rate.
	  /// </summary>
	  public static readonly SwapLegType OTHER = new SwapLegType("OTHER", InnerEnum.OTHER);

	  private static readonly IList<SwapLegType> valueList = new List<SwapLegType>();

	  static SwapLegType()
	  {
		  valueList.Add(FIXED);
		  valueList.Add(IBOR);
		  valueList.Add(OVERNIGHT);
		  valueList.Add(INFLATION);
		  valueList.Add(OTHER);
	  }

	  public enum InnerEnum
	  {
		  FIXED,
		  IBOR,
		  OVERNIGHT,
		  INFLATION,
		  OTHER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private SwapLegType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<SwapLegType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(SwapLegType.class);

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
//ORIGINAL LINE: @FromString public static SwapLegType of(String name)
	  public static SwapLegType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Fixed'.
	  /// </summary>
	  /// <returns> true if fixed, false otherwise </returns>
	  public bool Fixed
	  {
		  get
		  {
			return this == FIXED;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is floating, defined as 'Ibor', 'Overnight' or 'Inflation'.
	  /// </summary>
	  /// <returns> true if floating, false otherwise </returns>
	  public bool Float
	  {
		  get
		  {
			return this == IBOR || this == OVERNIGHT || this == INFLATION;
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


		public static IList<SwapLegType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static SwapLegType valueOf(string name)
		{
			foreach (SwapLegType enumInstance in SwapLegType.valueList)
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