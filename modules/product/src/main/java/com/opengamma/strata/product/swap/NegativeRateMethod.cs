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
	/// A convention defining how to handle a negative interest rate.
	/// <para>
	/// When calculating a floating rate, the result may be negative.
	/// This convention defines whether to allow the negative value or round to zero.
	/// </para>
	/// </summary>
	public abstract class NegativeRateMethod : NamedEnum
	{

	  /// <summary>
	  /// The "Negative Interest Rate Method", that allows the rate to be negative.
	  /// <para>
	  /// When calculating a payment, negative rates are allowed and result in a payment
	  /// in the opposite direction to that normally expected.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4b and 6.4c.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly NegativeRateMethod ALLOW_NEGATIVE = new NegativeRateMethod()
	  {
		  public double adjust(double rate)
		  {
			  return rate;
		  }
	  },
	  /// <summary>
	  /// The "Zero Rate Method", that prevents the rate from going below zero.
	  /// <para>
	  /// When calculating a payment, or other amount during compounding, the rate is
	  /// not allowed to go below zero.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.4d and 6.4e.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly NegativeRateMethod NOT_NEGATIVE = new NegativeRateMethod()
	  {
		  public double adjust(double rate)
		  {
			  return Math.max(rate, 0);
		  }
	  };

	  private static readonly IList<NegativeRateMethod> valueList = new List<NegativeRateMethod>();

	  static NegativeRateMethod()
	  {
		  valueList.Add(ALLOW_NEGATIVE);
		  valueList.Add(NOT_NEGATIVE);
	  }

	  public enum InnerEnum
	  {
		  ALLOW_NEGATIVE,
		  NOT_NEGATIVE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private NegativeRateMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<NegativeRateMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(NegativeRateMethod.class);

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
//ORIGINAL LINE: @FromString public static NegativeRateMethod of(String name) { return NAMES.parse(name); } public abstract double adjust(double rate);
	  public static NegativeRateMethod of(string name) {return NAMES.parse(name);} public abstract double adjust(double rate);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly NegativeRateMethod public = new NegativeRateMethod()
	  {
		  return NAMES.format(this);
	  }


		public static IList<NegativeRateMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static NegativeRateMethod valueOf(string name)
		{
			foreach (NegativeRateMethod enumInstance in NegativeRateMethod.valueList)
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