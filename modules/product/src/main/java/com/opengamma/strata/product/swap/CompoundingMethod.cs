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
	/// A convention defining how to compound interest.
	/// <para>
	/// When calculating interest, it may be necessary to apply compounding.
	/// Compound interest occurs where the basic interest is collected over one period but paid over a longer period.
	/// For example, interest may be collected every three months but only paid every year.
	/// </para>
	/// <para>
	/// For more information see this <a href="http://www.isda.org/c_and_a/pdf/ISDA-Compounding-memo.pdf">ISDA note</a>.
	/// </para>
	/// </summary>
	public sealed class CompoundingMethod : NamedEnum
	{

	  /// <summary>
	  /// No compounding applies.
	  /// <para>
	  /// This is typically used when the payment periods align with the accrual periods
	  /// thus no compounding is necessary. It may also be used when there are multiple
	  /// accrual periods, but they are summed rather than compounded.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundingMethod NONE = new CompoundingMethod("NONE", InnerEnum.NONE);
	  /// <summary>
	  /// Straight compounding applies, which is inclusive of the spread.
	  /// <para>
	  /// Compounding is based on the total of the observed rate and the spread.
	  /// </para>
	  /// <para>
	  /// Defined as "Compounding" in the ISDA 2006 definitions.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundingMethod STRAIGHT = new CompoundingMethod("STRAIGHT", InnerEnum.STRAIGHT);
	  /// <summary>
	  /// Flat compounding applies.
	  /// <para>
	  /// For interest on the notional, known as the <i>Basic Compounding Period Amount</i>,
	  /// compounding is based on the total of the observed rate and the spread.
	  /// For interest on previously accrued interest, known as the <i>Additional Compounding Period Amount</i>,
	  /// compounding is based only on the observed rate, excluding the spread.
	  /// </para>
	  /// <para>
	  /// Defined as "Flat Compounding" in the ISDA 2006 definitions.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundingMethod FLAT = new CompoundingMethod("FLAT", InnerEnum.FLAT);
	  /// <summary>
	  /// Spread exclusive compounding applies.
	  /// <para>
	  /// Compounding is based only on the observed rate, with the spread treated as simple interest.
	  /// </para>
	  /// <para>
	  /// Defined as "Compounding treating Spread as simple interest" in the ISDA definitions.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundingMethod SPREAD_EXCLUSIVE = new CompoundingMethod("SPREAD_EXCLUSIVE", InnerEnum.SPREAD_EXCLUSIVE);

	  private static readonly IList<CompoundingMethod> valueList = new List<CompoundingMethod>();

	  static CompoundingMethod()
	  {
		  valueList.Add(NONE);
		  valueList.Add(STRAIGHT);
		  valueList.Add(FLAT);
		  valueList.Add(SPREAD_EXCLUSIVE);
	  }

	  public enum InnerEnum
	  {
		  NONE,
		  STRAIGHT,
		  FLAT,
		  SPREAD_EXCLUSIVE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CompoundingMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CompoundingMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CompoundingMethod.class);

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
//ORIGINAL LINE: @FromString public static CompoundingMethod of(String name)
	  public static CompoundingMethod of(string name)
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


		public static IList<CompoundingMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CompoundingMethod valueOf(string name)
		{
			foreach (CompoundingMethod enumInstance in CompoundingMethod.valueList)
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