using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Reference price index calculation method.
	/// <para>
	/// This defines how the reference index calculation occurs.
	/// </para>
	/// <para>
	/// References: "Bond Pricing", OpenGamma Documentation 5, Version 2.0, May 2013, 
	/// "Inflation Instruments: Swap Zero-coupon, Year-on-year and Bonds."
	/// </para>
	/// </summary>
	public sealed class PriceIndexCalculationMethod : NamedEnum
	{

	  /// <summary>
	  /// The reference index is the price index of a month.
	  /// The reference month is linked to the payment date.
	  /// </summary>
	  public static readonly PriceIndexCalculationMethod MONTHLY = new PriceIndexCalculationMethod("MONTHLY", InnerEnum.MONTHLY);
	  /// <summary>
	  /// The reference index is linearly interpolated between two months.
	  /// The interpolation is done with the number of days of the payment month.
	  /// The number of days is counted from the beginning of the month.
	  /// </summary>
	  public static readonly PriceIndexCalculationMethod INTERPOLATED = new PriceIndexCalculationMethod("INTERPOLATED", InnerEnum.INTERPOLATED);
	  /// <summary>
	  /// The reference index is linearly interpolated between two months.
	  /// The interpolation is done with the number of days of the payment month.
	  /// The number of days is counted from the 10th day of the month.
	  /// </summary>
	  public static readonly PriceIndexCalculationMethod INTERPOLATED_JAPAN = new PriceIndexCalculationMethod("INTERPOLATED_JAPAN", InnerEnum.INTERPOLATED_JAPAN);

	  private static readonly IList<PriceIndexCalculationMethod> valueList = new List<PriceIndexCalculationMethod>();

	  static PriceIndexCalculationMethod()
	  {
		  valueList.Add(MONTHLY);
		  valueList.Add(INTERPOLATED);
		  valueList.Add(INTERPOLATED_JAPAN);
	  }

	  public enum InnerEnum
	  {
		  MONTHLY,
		  INTERPOLATED,
		  INTERPOLATED_JAPAN
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PriceIndexCalculationMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PriceIndexCalculationMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PriceIndexCalculationMethod.class);

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
//ORIGINAL LINE: @FromString public static PriceIndexCalculationMethod of(String name)
	  public static PriceIndexCalculationMethod of(string name)
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


		public static IList<PriceIndexCalculationMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PriceIndexCalculationMethod valueOf(string name)
		{
			foreach (PriceIndexCalculationMethod enumInstance in PriceIndexCalculationMethod.valueList)
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