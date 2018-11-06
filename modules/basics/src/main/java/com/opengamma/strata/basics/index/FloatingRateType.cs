using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The type of a floating rate index.
	/// <para>
	/// This provides a high-level categorization of the floating rate index.
	/// This is used to classify the index and create the right kind of pricing
	/// index, <seealso cref="IborIndex"/> or <seealso cref="OvernightIndex"/>.
	/// </para>
	/// </summary>
	public sealed class FloatingRateType : NamedEnum
	{

	  /// <summary>
	  /// A floating rate index that is based on an Ibor index.
	  /// <para>
	  /// This kind of rate translates to an <seealso cref="IborIndex"/>.
	  /// </para>
	  /// </summary>
	  public static readonly FloatingRateType IBOR = new FloatingRateType("IBOR", InnerEnum.IBOR);
	  /// <summary>
	  /// A floating rate index that is based on an Overnight index with compounding.
	  /// <para>
	  /// This kind of rate translates to an <seealso cref="OvernightIndex"/>.
	  /// </para>
	  /// </summary>
	  public static readonly FloatingRateType OVERNIGHT_COMPOUNDED = new FloatingRateType("OVERNIGHT_COMPOUNDED", InnerEnum.OVERNIGHT_COMPOUNDED);
	  /// <summary>
	  /// A floating rate index that is based on an Overnight index with averaging.
	  /// <para>
	  /// This kind of rate translates to an <seealso cref="OvernightIndex"/>.
	  /// This is typically used only for US Fed Fund swaps.
	  /// </para>
	  /// </summary>
	  public static readonly FloatingRateType OVERNIGHT_AVERAGED = new FloatingRateType("OVERNIGHT_AVERAGED", InnerEnum.OVERNIGHT_AVERAGED);
	  /// <summary>
	  /// A floating rate index that is based on a price index.
	  /// <para>
	  /// This kind of rate translates to an <seealso cref="PriceIndex"/>.
	  /// </para>
	  /// </summary>
	  public static readonly FloatingRateType PRICE = new FloatingRateType("PRICE", InnerEnum.PRICE);
	  /// <summary>
	  /// A floating rate index of another type.
	  /// </summary>
	  public static readonly FloatingRateType OTHER = new FloatingRateType("OTHER", InnerEnum.OTHER);

	  private static readonly IList<FloatingRateType> valueList = new List<FloatingRateType>();

	  static FloatingRateType()
	  {
		  valueList.Add(IBOR);
		  valueList.Add(OVERNIGHT_COMPOUNDED);
		  valueList.Add(OVERNIGHT_AVERAGED);
		  valueList.Add(PRICE);
		  valueList.Add(OTHER);
	  }

	  public enum InnerEnum
	  {
		  IBOR,
		  OVERNIGHT_COMPOUNDED,
		  OVERNIGHT_AVERAGED,
		  PRICE,
		  OTHER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private FloatingRateType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FloatingRateType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(FloatingRateType.class);

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
//ORIGINAL LINE: @FromString public static FloatingRateType of(String name)
	  public static FloatingRateType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Ibor'.
	  /// </summary>
	  /// <returns> true if Ibor, false otherwise </returns>
	  public bool Ibor
	  {
		  get
		  {
			return this == IBOR;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'OvernightCompounded' or 'OvernightAveraged'.
	  /// </summary>
	  /// <returns> true if Overnight, false otherwise </returns>
	  public bool Overnight
	  {
		  get
		  {
			return this == OVERNIGHT_COMPOUNDED || this == OVERNIGHT_AVERAGED;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Price'.
	  /// </summary>
	  /// <returns> true if Price, false otherwise </returns>
	  public bool Price
	  {
		  get
		  {
			return this == PRICE;
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


		public static IList<FloatingRateType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FloatingRateType valueOf(string name)
		{
			foreach (FloatingRateType enumInstance in FloatingRateType.valueList)
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