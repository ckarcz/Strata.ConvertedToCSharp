using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A convention defining how to discount Forward Rate Agreements (FRAs).
	/// <para>
	/// When calculating the price of a FRA, there are different approaches to pricing in different markets.
	/// This method captures the approach to discounting.
	/// </para>
	/// <para>
	/// Defined by the 2006 ISDA definitions article 8.4.
	/// </para>
	/// </summary>
	public sealed class FraDiscountingMethod : NamedEnum
	{

	  /// <summary>
	  /// No discounting applies.
	  /// </summary>
	  public static readonly FraDiscountingMethod NONE = new FraDiscountingMethod("NONE", InnerEnum.NONE, "None");
	  /// <summary>
	  /// FRA discounting as defined by ISDA.
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 8.4b.
	  /// </para>
	  /// </summary>
	  public static readonly FraDiscountingMethod ISDA = new FraDiscountingMethod("ISDA", InnerEnum.ISDA, "ISDA");
	  /// <summary>
	  /// FRA discounting as defined by the Australian Financial Markets Association (AFMA).
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 8.4e.
	  /// </para>
	  /// </summary>
	  public static readonly FraDiscountingMethod AFMA = new FraDiscountingMethod("AFMA", InnerEnum.AFMA, "AFMA");

	  private static readonly IList<FraDiscountingMethod> valueList = new List<FraDiscountingMethod>();

	  static FraDiscountingMethod()
	  {
		  valueList.Add(NONE);
		  valueList.Add(ISDA);
		  valueList.Add(AFMA);
	  }

	  public enum InnerEnum
	  {
		  NONE,
		  ISDA,
		  AFMA
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FraDiscountingMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(FraDiscountingMethod.class);

	  // name
	  private readonly string name;

	  // create
	  private FraDiscountingMethod(string name, InnerEnum innerEnum, string name)
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
//ORIGINAL LINE: @FromString public static FraDiscountingMethod of(String name)
	  public static FraDiscountingMethod of(string name)
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


		public static IList<FraDiscountingMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FraDiscountingMethod valueOf(string name)
		{
			foreach (FraDiscountingMethod enumInstance in FraDiscountingMethod.valueList)
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