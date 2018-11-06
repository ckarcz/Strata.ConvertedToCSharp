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

	using SchedulePeriod = com.opengamma.strata.basics.schedule.SchedulePeriod;
	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The base date that each payment is made relative to.
	/// <para>
	/// When calculating the payment date for a swap leg, the date is calculated relative to another date.
	/// The other date is specified by this enum.
	/// </para>
	/// </summary>
	public sealed class PaymentRelativeTo : NamedEnum
	{

	  /// <summary>
	  /// The payment is made relative to the start of each payment period.
	  /// <para>
	  /// The payment date is relative to the start date of the first accrual period
	  /// within the payment period, as adjusted by business day conventions.
	  /// </para>
	  /// <para>
	  /// This can be referred to as "payment in advance".
	  /// </para>
	  /// </summary>
	  public static readonly PaymentRelativeTo PERIOD_START = new PaymentRelativeTo("PERIOD_START", InnerEnum.PERIOD_START);
	  /// <summary>
	  /// The payment is made relative to the end of each payment period.
	  /// <para>
	  /// The payment date is relative to the end date of the last accrual period
	  /// within the payment period, as adjusted by business day conventions.
	  /// </para>
	  /// <para>
	  /// This can be referred to as "payment in arrears".
	  /// </para>
	  /// </summary>
	  public static readonly PaymentRelativeTo PERIOD_END = new PaymentRelativeTo("PERIOD_END", InnerEnum.PERIOD_END);

	  private static readonly IList<PaymentRelativeTo> valueList = new List<PaymentRelativeTo>();

	  static PaymentRelativeTo()
	  {
		  valueList.Add(PERIOD_START);
		  valueList.Add(PERIOD_END);
	  }

	  public enum InnerEnum
	  {
		  PERIOD_START,
		  PERIOD_END
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PaymentRelativeTo(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PaymentRelativeTo> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PaymentRelativeTo.class);

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
//ORIGINAL LINE: @FromString public static PaymentRelativeTo of(String name)
	  public static PaymentRelativeTo of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  // selects the base date for payment
	  internal java.time.LocalDate selectBaseDate(com.opengamma.strata.basics.schedule.SchedulePeriod period)
	  {
		return (this == PERIOD_END ? period.EndDate : period.StartDate);
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


		public static IList<PaymentRelativeTo> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PaymentRelativeTo valueOf(string name)
		{
			foreach (PaymentRelativeTo enumInstance in PaymentRelativeTo.valueList)
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