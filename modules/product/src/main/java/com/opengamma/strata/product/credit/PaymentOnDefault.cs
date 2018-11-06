using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The payment on default.
	/// <para>
	/// Whether the accrued premium is paid in the event of a default.
	/// </para>
	/// </summary>
	public sealed class PaymentOnDefault : NamedEnum
	{

	  /// <summary>
	  /// The accrued premium.
	  /// <para>
	  /// If the credit event happens between coupon dates, the accrued premium is paid. 
	  /// </para>
	  /// </summary>
	  public static readonly PaymentOnDefault ACCRUED_PREMIUM = new PaymentOnDefault("ACCRUED_PREMIUM", InnerEnum.ACCRUED_PREMIUM);
	  /// <summary>
	  /// None. 
	  /// <para>
	  /// Even if the credit event happens between coupon dates, the accrued premium is not paid.
	  /// </para>
	  /// </summary>
	  public static readonly PaymentOnDefault NONE = new PaymentOnDefault("NONE", InnerEnum.NONE);

	  private static readonly IList<PaymentOnDefault> valueList = new List<PaymentOnDefault>();

	  static PaymentOnDefault()
	  {
		  valueList.Add(ACCRUED_PREMIUM);
		  valueList.Add(NONE);
	  }

	  public enum InnerEnum
	  {
		  ACCRUED_PREMIUM,
		  NONE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PaymentOnDefault(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PaymentOnDefault> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PaymentOnDefault.class);

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
//ORIGINAL LINE: @FromString public static PaymentOnDefault of(String name)
	  public static PaymentOnDefault of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Check if the accrued premium is paid.
	  /// </summary>
	  /// <returns> true if the accrued premium is paid, false otherwise </returns>
	  public bool AccruedInterest
	  {
		  get
		  {
			return this == ACCRUED_PREMIUM;
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


		public static IList<PaymentOnDefault> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PaymentOnDefault valueOf(string name)
		{
			foreach (PaymentOnDefault enumInstance in PaymentOnDefault.valueList)
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