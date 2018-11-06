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
	/// Flag indicating whether a trade is "put" or "call".
	/// <para>
	/// The concepts of put and call apply to options trading.
	/// A call gives the owner the right, but not obligation, to buy the underlying at
	/// an agreed price in the future. A put gives a similar option to sell.
	/// </para>
	/// </summary>
	public sealed class PutCall : NamedEnum
	{

	  /// <summary>
	  /// Put.
	  /// </summary>
	  public static readonly PutCall PUT = new PutCall("PUT", InnerEnum.PUT);
	  /// <summary>
	  /// Call.
	  /// </summary>
	  public static readonly PutCall CALL = new PutCall("CALL", InnerEnum.CALL);

	  private static readonly IList<PutCall> valueList = new List<PutCall>();

	  static PutCall()
	  {
		  valueList.Add(PUT);
		  valueList.Add(CALL);
	  }

	  public enum InnerEnum
	  {
		  PUT,
		  CALL
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private PutCall(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<PutCall> NAMES = com.opengamma.strata.collect.named.EnumNames.of(PutCall.class);

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
//ORIGINAL LINE: @FromString public static PutCall of(String name)
	  public static PutCall of(string name)
	  {
		return NAMES.parse(name);
	  }

	  /// <summary>
	  /// Converts a boolean "is put" flag to the enum value.
	  /// </summary>
	  /// <param name="isPut">  the put flag, true for put, false for call </param>
	  /// <returns> the equivalent enum </returns>
	  public static PutCall ofPut(bool isPut)
	  {
		return isPut ? PUT : CALL;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the type is 'Put'.
	  /// </summary>
	  /// <returns> true if put, false if call </returns>
	  public bool Put
	  {
		  get
		  {
			return this == PUT;
		  }
	  }

	  /// <summary>
	  /// Checks if the type is 'Call'.
	  /// </summary>
	  /// <returns> true if call, false if put </returns>
	  public bool Call
	  {
		  get
		  {
			return this == CALL;
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


		public static IList<PutCall> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static PutCall valueOf(string name)
		{
			foreach (PutCall enumInstance in PutCall.valueList)
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