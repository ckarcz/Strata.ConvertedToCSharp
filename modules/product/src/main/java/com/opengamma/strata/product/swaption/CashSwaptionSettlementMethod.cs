using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Cash settlement method of cash settled swaptions.
	/// <para>
	/// Reference: "Swaption Pricing", OpenGamma Documentation 10, Version 1.2, April 2011.
	/// </para>
	/// </summary>
	public sealed class CashSwaptionSettlementMethod : NamedEnum
	{

	  /// <summary>
	  /// The cash price method
	  /// <para>
	  /// If exercised, the value of the underlying swap is exchanged at the cash settlement date.
	  /// </para>
	  /// </summary>
	  public static readonly CashSwaptionSettlementMethod CASH_PRICE = new CashSwaptionSettlementMethod("CASH_PRICE", InnerEnum.CASH_PRICE);
	  /// <summary>
	  /// The par yield curve method.
	  /// <para>
	  /// The settlement amount is computed with cash-settled annuity using the pre-agreed strike swap rate.
	  /// </para>
	  /// </summary>
	  public static readonly CashSwaptionSettlementMethod PAR_YIELD = new CashSwaptionSettlementMethod("PAR_YIELD", InnerEnum.PAR_YIELD);
	  /// <summary>
	  /// The zero coupon yield method.
	  /// <para>
	  /// The settlement amount is computed with the discount factor based on the agreed zero coupon curve.
	  /// </para>
	  /// </summary>
	  public static readonly CashSwaptionSettlementMethod ZERO_COUPON_YIELD = new CashSwaptionSettlementMethod("ZERO_COUPON_YIELD", InnerEnum.ZERO_COUPON_YIELD);

	  private static readonly IList<CashSwaptionSettlementMethod> valueList = new List<CashSwaptionSettlementMethod>();

	  static CashSwaptionSettlementMethod()
	  {
		  valueList.Add(CASH_PRICE);
		  valueList.Add(PAR_YIELD);
		  valueList.Add(ZERO_COUPON_YIELD);
	  }

	  public enum InnerEnum
	  {
		  CASH_PRICE,
		  PAR_YIELD,
		  ZERO_COUPON_YIELD
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CashSwaptionSettlementMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CashSwaptionSettlementMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CashSwaptionSettlementMethod.class);

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
//ORIGINAL LINE: @FromString public static CashSwaptionSettlementMethod of(String name)
	  public static CashSwaptionSettlementMethod of(string name)
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


		public static IList<CashSwaptionSettlementMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CashSwaptionSettlementMethod valueOf(string name)
		{
			foreach (CashSwaptionSettlementMethod enumInstance in CashSwaptionSettlementMethod.valueList)
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