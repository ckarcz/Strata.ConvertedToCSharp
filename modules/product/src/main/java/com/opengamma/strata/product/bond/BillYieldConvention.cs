using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A convention defining how yield is computed for a bill.
	/// </summary>
	public abstract class BillYieldConvention : NamedEnum
	{

	  /// <summary>
	  /// Discount.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly BillYieldConvention DISCOUNT = new BillYieldConvention()
	  {
		  public double priceFromYield(double yield, double accrualFactor)
		  {
			  return 1.0d - accrualFactor * yield;
		  }
		  public double yieldFromPrice(double price, double accrualFactor)
		  {
			  return (1.0d - price) / accrualFactor;
		  }
	  },

	  /// <summary>
	  /// France CD: interest at maturity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly BillYieldConvention FRANCE_CD = new BillYieldConvention()
	  {
		  public double priceFromYield(double yield, double accrualFactor)
		  {
			  return 1.0d / (1.0d + accrualFactor * yield);
		  }
		  public double yieldFromPrice(double price, double accrualFactor)
		  {
			  return (1.0d / price - 1) / accrualFactor;
		  }
	  },

	  /// <summary>
	  /// Interest at maturity.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly BillYieldConvention INTEREST_AT_MATURITY = new BillYieldConvention()
	  {
		  public double priceFromYield(double yield, double accrualFactor)
		  {
			  return 1.0d / (1.0d + accrualFactor * yield);
		  }
		  public double yieldFromPrice(double price, double accrualFactor)
		  {
			  return (1.0d / price - 1) / accrualFactor;
		  }
	  },

	  /// <summary>
	  /// Japanese T-Bills.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly BillYieldConvention JAPAN_BILLS = new BillYieldConvention()
	  {
		  public double priceFromYield(double yield, double accrualFactor)
		  {
			  return 1.0d / (1.0d + accrualFactor * yield);
		  }
		  public double yieldFromPrice(double price, double accrualFactor)
		  {
			  return (1.0d / price - 1) / accrualFactor;
		  }
	  };

	  private static readonly IList<BillYieldConvention> valueList = new List<BillYieldConvention>();

	  static BillYieldConvention()
	  {
		  valueList.Add(DISCOUNT);
		  valueList.Add(FRANCE_CD);
		  valueList.Add(INTEREST_AT_MATURITY);
		  valueList.Add(JAPAN_BILLS);
	  }

	  public enum InnerEnum
	  {
		  DISCOUNT,
		  FRANCE_CD,
		  INTEREST_AT_MATURITY,
		  JAPAN_BILLS
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private BillYieldConvention(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<BillYieldConvention> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(BillYieldConvention.class);

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private BillYieldConvention(String name) { this.name = name; } @FromString public static BillYieldConvention of(String name) { return NAMES.parse(name); } @ToString @Override public String toString() { return name; } public abstract double priceFromYield(double yield, double accrualFactor);
	  private BillYieldConvention(string name) {this.name = name;} public static BillYieldConvention of(string name) {return NAMES.parse(name);} @Override public string toString() {return name;} public abstract double priceFromYield(double yield, double accrualFactor);

	  /// <summary>
	  /// Computes the yield from a price and a accrual factor.
	  /// </summary>
	  /// <param name="price">  the price </param>
	  /// <param name="accrualFactor">  the accrual factor </param>
	  /// <returns> the yield </returns>
	  public abstract double yieldFromPrice(double price, double accrualFactor);


		public static IList<BillYieldConvention> values()
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

		public static BillYieldConvention valueOf(string name)
		{
			foreach (BillYieldConvention enumInstance in BillYieldConvention.valueList)
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