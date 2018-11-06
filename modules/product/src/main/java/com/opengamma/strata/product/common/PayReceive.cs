using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Flag indicating whether a financial instrument is "pay" or "receive".
	/// <para>
	/// Specifies the direction of payments.
	/// For example, a swap typically has two legs, a pay leg, where payments are made
	/// to the counterparty, and a receive leg, where payments are received.
	/// </para>
	/// </summary>
	public sealed class PayReceive : NamedEnum
	{

	  /// <summary>
	  /// Pay.
	  /// </summary>
	  public static readonly PayReceive PAY = new PayReceive("PAY", InnerEnum.PAY);
	  /// <summary>
	  /// Receive.
	  /// </summary>
	  public static readonly PayReceive RECEIVE = new PayReceive("RECEIVE", InnerEnum.RECEIVE);

	  private static readonly IList<PayReceive> valueList = new List<PayReceive>();

	  static PayReceive()
	  {
		  valueList.Add(PAY);
		  valueList.Add(RECEIVE);
	  }

	  public enum InnerEnum
	  {
		  PAY,
		  RECEIVE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PayReceive(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PayReceive> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PayReceive.class);

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
//ORIGINAL LINE: @FromString public static PayReceive of(String name)
	  public static PayReceive of(string name)
	  {
		return NAMES.parse(name);
	  }

	  /// <summary>
	  /// Converts a boolean "is pay" flag to the enum value.
	  /// </summary>
	  /// <param name="isPay">  the pay flag, true for pay, false for receive </param>
	  /// <returns> the equivalent enum </returns>
	  public static PayReceive ofPay(bool isPay)
	  {
		return isPay ? PAY : RECEIVE;
	  }

	  /// <summary>
	  /// Converts a signed amount to the enum value.
	  /// <para>
	  /// A negative value will return 'Pay'.
	  /// A positive value will return 'Receive'.
	  /// This effectively parses the result of <seealso cref="#normalize(double)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to check </param>
	  /// <returns> the equivalent enum </returns>
	  public static PayReceive ofSignedAmount(double amount)
	  {
		return amount.CompareTo(0d) < 0 ? PAY : RECEIVE;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Normalizes the specified notional amount using this pay/receive rule.
	  /// <para>
	  /// This returns a negative signed amount if this is 'Pay', and a positive
	  /// signed amount if this is 'Receive'. This effectively normalizes the input
	  /// notional to the pay/receive sign conventions of this library.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="amount">  the amount to adjust </param>
	  /// <returns> the adjusted amount </returns>
	  public double normalize(double amount)
	  {
		double normalized = Math.Abs(amount);
		return Pay ? -normalized : normalized;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Pay'.
	  /// </summary>
	  /// <returns> true if pay, false if receive </returns>
	  public bool Pay
	  {
		  get
		  {
			return this == PAY;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Receive'.
	  /// </summary>
	  /// <returns> true if receive, false if pay </returns>
	  public bool Receive
	  {
		  get
		  {
			return this == RECEIVE;
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


		public static IList<PayReceive> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PayReceive valueOf(string name)
		{
			foreach (PayReceive enumInstance in PayReceive.valueList)
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