using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The formula for accrual on default.
	/// <para>
	/// This specifies which formula is used in {@code IsdaCdsProductPricer} for computing the accrued payment on default. 
	/// The formula is 'original ISDA', 'Markit fix' or 'correct'.
	/// </para>
	/// </summary>
	public sealed class ArbitrageHandling : NamedEnum
	{

	  /// <summary>
	  /// Ignore.
	  /// <para>
	  /// If the market data has arbitrage, the curve will still build. 
	  /// The survival probability will not be monotonically decreasing 
	  /// (equivalently, some forward hazard rates will be negative). 
	  /// </para>
	  /// </summary>
	  public static readonly ArbitrageHandling IGNORE = new ArbitrageHandling("IGNORE", InnerEnum.IGNORE);
	  /// <summary>
	  /// Fail.
	  /// <para>
	  /// An exception is thrown if an arbitrage is found. 
	  /// </para>
	  /// </summary>
	  public static readonly ArbitrageHandling FAIL = new ArbitrageHandling("FAIL", InnerEnum.FAIL);
	  /// <summary>
	  /// Zero hazard rate.
	  /// <para>
	  /// If a particular spread implies a negative forward hazard rate, 
	  /// the hazard rate is set to zero, and the calibration continues. 
	  /// The resultant curve will not exactly reprice the input CDSs, but will find new spreads that just avoid arbitrage.   
	  /// </para>
	  /// </summary>
	  public static readonly ArbitrageHandling ZERO_HAZARD_RATE = new ArbitrageHandling("ZERO_HAZARD_RATE", InnerEnum.ZERO_HAZARD_RATE);

	  private static readonly IList<ArbitrageHandling> valueList = new List<ArbitrageHandling>();

	  static ArbitrageHandling()
	  {
		  valueList.Add(IGNORE);
		  valueList.Add(FAIL);
		  valueList.Add(ZERO_HAZARD_RATE);
	  }

	  public enum InnerEnum
	  {
		  IGNORE,
		  FAIL,
		  ZERO_HAZARD_RATE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private ArbitrageHandling(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<ArbitrageHandling> NAMES = com.opengamma.strata.collect.named.EnumNames.of(ArbitrageHandling.class);

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
//ORIGINAL LINE: @FromString public static ArbitrageHandling of(String name)
	  public static ArbitrageHandling of(string name)
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


		public static IList<ArbitrageHandling> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static ArbitrageHandling valueOf(string name)
		{
			foreach (ArbitrageHandling enumInstance in ArbitrageHandling.valueList)
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