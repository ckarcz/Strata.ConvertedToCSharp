using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A convention defining accrued interest calculation type for inflation bond securities.
	/// <para>
	/// Yield of a bond security is a conventional number representing the internal rate of
	/// return of standardized cash flows.
	/// When calculating accrued interest, it is necessary to use a formula specific to each 
	/// yield convention. Accordingly, the computation of price, convexity and duration from 
	/// the yield should be based on this yield convention.
	/// </para>
	/// <para>
	/// "Inflation Instruments: Swap Zero-coupon, Year-on-year and Bonds."
	/// </para>
	/// </summary>
	public sealed class CapitalIndexedBondYieldConvention : NamedEnum
	{

	  /// <summary>
	  /// The US real yield convention. Used for TIPS (see Federal Register Vol. 69, N0. 170, p 53623).
	  /// </summary>
	  public static readonly CapitalIndexedBondYieldConvention US_IL_REAL = new CapitalIndexedBondYieldConvention("US_IL_REAL", InnerEnum.US_IL_REAL, "US-I/L-Real");

	  /// <summary>
	  /// The UK real yield convention. Used for inflation linked GILTS.
	  /// </summary>
	  public static readonly CapitalIndexedBondYieldConvention GB_IL_FLOAT = new CapitalIndexedBondYieldConvention("GB_IL_FLOAT", InnerEnum.GB_IL_FLOAT, "GB-I/L-Float");

	  /// <summary>
	  /// The UK real yield convention. Used for UK inflation linked corporate bond.
	  /// </summary>
	  public static readonly CapitalIndexedBondYieldConvention GB_IL_BOND = new CapitalIndexedBondYieldConvention("GB_IL_BOND", InnerEnum.GB_IL_BOND, "GB-I/L-Bond");

	  /// <summary>
	  /// The Japan simple yield convention for inflation index bond.
	  /// </summary>
	  public static readonly CapitalIndexedBondYieldConvention JP_IL_SIMPLE = new CapitalIndexedBondYieldConvention("JP_IL_SIMPLE", InnerEnum.JP_IL_SIMPLE, "JP-I/L-Simple");

	  /// <summary>
	  /// The Japan compound yield convention for inflation index bond.
	  /// </summary>
	  public static readonly CapitalIndexedBondYieldConvention JP_IL_COMPOUND = new CapitalIndexedBondYieldConvention("JP_IL_COMPOUND", InnerEnum.JP_IL_COMPOUND, "JP-I/L-Compound");

	  private static readonly IList<CapitalIndexedBondYieldConvention> valueList = new List<CapitalIndexedBondYieldConvention>();

	  static CapitalIndexedBondYieldConvention()
	  {
		  valueList.Add(US_IL_REAL);
		  valueList.Add(GB_IL_FLOAT);
		  valueList.Add(GB_IL_BOND);
		  valueList.Add(JP_IL_SIMPLE);
		  valueList.Add(JP_IL_COMPOUND);
	  }

	  public enum InnerEnum
	  {
		  US_IL_REAL,
		  GB_IL_FLOAT,
		  GB_IL_BOND,
		  JP_IL_SIMPLE,
		  JP_IL_COMPOUND
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CapitalIndexedBondYieldConvention> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(CapitalIndexedBondYieldConvention.class);

	  // name
	  private readonly string name;

	  // create
	  private CapitalIndexedBondYieldConvention(string name, InnerEnum innerEnum, string name)
	  {
		this.name = name;

		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

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
//ORIGINAL LINE: @FromString public static CapitalIndexedBondYieldConvention of(String name)
	  public static CapitalIndexedBondYieldConvention of(string name)
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
		return name;
	  }


		public static IList<CapitalIndexedBondYieldConvention> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CapitalIndexedBondYieldConvention valueOf(string name)
		{
			foreach (CapitalIndexedBondYieldConvention enumInstance in CapitalIndexedBondYieldConvention.valueList)
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