using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.common
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Enumerates the types of price that can be returned. 
	/// <para>
	/// The price is usually clean or dirty.
	/// Financial instruments are often quoted in terms of clean price rather than dirty price.
	/// </para>
	/// <para>
	/// The dirty price is the full price, which is typically the mark-to-market value of an instrument.
	/// </para>
	/// <para>
	/// The clean price is computed from the dirty price by subtracting/adding accrued interest.
	/// Subtraction/addition is determined by the direction of accrued interest payment.  
	/// </para>
	/// </summary>
	public sealed class PriceType : NamedEnum
	{

	  /// <summary>
	  /// Clean price.
	  /// <para>
	  /// The accrued interest is removed from the full price.
	  /// </para>
	  /// </summary>
	  public static readonly PriceType CLEAN = new PriceType("CLEAN", InnerEnum.CLEAN);
	  /// <summary>
	  /// Dirty price.
	  /// <para>
	  /// The dirty price is the full price of an instrument.
	  /// </para>
	  /// </summary>
	  public static readonly PriceType DIRTY = new PriceType("DIRTY", InnerEnum.DIRTY);

	  private static readonly IList<PriceType> valueList = new List<PriceType>();

	  static PriceType()
	  {
		  valueList.Add(CLEAN);
		  valueList.Add(DIRTY);
	  }

	  public enum InnerEnum
	  {
		  CLEAN,
		  DIRTY
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PriceType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PriceType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PriceType.class);

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
//ORIGINAL LINE: @FromString public static PriceType of(String name)
	  public static PriceType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Check if the price type is 'Clean'.
	  /// </summary>
	  /// <returns> true if clean, false if dirty </returns>
	  public bool CleanPrice
	  {
		  get
		  {
			return this == CLEAN;
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


		public static IList<PriceType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PriceType valueOf(string name)
		{
			foreach (PriceType enumInstance in PriceType.valueList)
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