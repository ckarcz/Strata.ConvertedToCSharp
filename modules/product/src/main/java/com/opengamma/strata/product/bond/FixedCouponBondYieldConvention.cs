using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// A convention defining accrued interest calculation type for a bond security.
	/// <para>
	/// Yield of a bond security is a conventional number representing the internal rate of
	/// return of standardized cash flows.
	/// When calculating accrued interest, it is necessary to use a formula specific to each
	/// yield convention. Accordingly, the computation of price, convexity and duration from
	/// the yield should be based on this yield convention.
	/// </para>
	/// <para>
	/// References: "Bond Pricing", OpenGamma Documentation 5, Version 2.0, May 2013
	/// </para>
	/// </summary>
	public sealed class FixedCouponBondYieldConvention : NamedEnum
	{

	  /// <summary>
	  /// UK BUMP/DMO method.
	  /// </summary>
	  public static readonly FixedCouponBondYieldConvention GB_BUMP_DMO = new FixedCouponBondYieldConvention("GB_BUMP_DMO", InnerEnum.GB_BUMP_DMO, "GB-Bump-DMO");

	  /// <summary>
	  /// US street.
	  /// </summary>
	  public static readonly FixedCouponBondYieldConvention US_STREET = new FixedCouponBondYieldConvention("US_STREET", InnerEnum.US_STREET, "US-Street");

	  /// <summary>
	  /// German bonds.
	  /// </summary>
	  public static readonly FixedCouponBondYieldConvention DE_BONDS = new FixedCouponBondYieldConvention("DE_BONDS", InnerEnum.DE_BONDS, "DE-Bonds");

	  /// <summary>
	  /// Japan simple yield.
	  /// </summary>
	  public static readonly FixedCouponBondYieldConvention JP_SIMPLE = new FixedCouponBondYieldConvention("JP_SIMPLE", InnerEnum.JP_SIMPLE, "JP-Simple");

	  private static readonly IList<FixedCouponBondYieldConvention> valueList = new List<FixedCouponBondYieldConvention>();

	  static FixedCouponBondYieldConvention()
	  {
		  valueList.Add(GB_BUMP_DMO);
		  valueList.Add(US_STREET);
		  valueList.Add(DE_BONDS);
		  valueList.Add(JP_SIMPLE);
	  }

	  public enum InnerEnum
	  {
		  GB_BUMP_DMO,
		  US_STREET,
		  DE_BONDS,
		  JP_SIMPLE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FixedCouponBondYieldConvention> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(FixedCouponBondYieldConvention.class);

	  // name
	  private readonly string name;

	  // create
	  private FixedCouponBondYieldConvention(string name, InnerEnum innerEnum, string name)
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
//ORIGINAL LINE: @FromString public static FixedCouponBondYieldConvention of(String name)
	  public static FixedCouponBondYieldConvention of(string name)
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


		public static IList<FixedCouponBondYieldConvention> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FixedCouponBondYieldConvention valueOf(string name)
		{
			foreach (FixedCouponBondYieldConvention enumInstance in FixedCouponBondYieldConvention.valueList)
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