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
	/// The method of accruing interest based on an Overnight index.
	/// <para>
	/// Two methods of accrual are supported - compounded and averaged.
	/// Averaging is primarily related to the 'USD-FED-FUND' index.
	/// </para>
	/// </summary>
	public sealed class OvernightAccrualMethod : NamedEnum
	{

	  /// <summary>
	  /// The compounded method.
	  /// <para>
	  /// Interest is accrued by simple compounding of each rate published during the accrual period.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2a(3C).
	  /// </para>
	  /// <para>
	  /// This is the most common formula for OIS swaps.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightAccrualMethod COMPOUNDED = new OvernightAccrualMethod("COMPOUNDED", InnerEnum.COMPOUNDED);
	  /// <summary>
	  /// The averaged method.
	  /// <para>
	  /// Interest is accrued by taking the average of all the rates published on the
	  /// index during the accrual period.
	  /// </para>
	  /// <para>
	  /// This is intended for Fed Fund OIS swaps.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightAccrualMethod AVERAGED = new OvernightAccrualMethod("AVERAGED", InnerEnum.AVERAGED);
	  /// <summary>
	  /// The averaged daily method.
	  /// <para>
	  /// Interest is accrued by taking the average of all the daily rates during the observation period.
	  /// </para>
	  /// <para>
	  /// This is intended for Fed Fund futures, not swaps.
	  /// </para>
	  /// </summary>
	  public static readonly OvernightAccrualMethod AVERAGED_DAILY = new OvernightAccrualMethod("AVERAGED_DAILY", InnerEnum.AVERAGED_DAILY);

	  private static readonly IList<OvernightAccrualMethod> valueList = new List<OvernightAccrualMethod>();

	  static OvernightAccrualMethod()
	  {
		  valueList.Add(COMPOUNDED);
		  valueList.Add(AVERAGED);
		  valueList.Add(AVERAGED_DAILY);
	  }

	  public enum InnerEnum
	  {
		  COMPOUNDED,
		  AVERAGED,
		  AVERAGED_DAILY
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private OvernightAccrualMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<OvernightAccrualMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(OvernightAccrualMethod.class);

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
//ORIGINAL LINE: @FromString public static OvernightAccrualMethod of(String name)
	  public static OvernightAccrualMethod of(string name)
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


		public static IList<OvernightAccrualMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static OvernightAccrualMethod valueOf(string name)
		{
			foreach (OvernightAccrualMethod enumInstance in OvernightAccrualMethod.valueList)
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