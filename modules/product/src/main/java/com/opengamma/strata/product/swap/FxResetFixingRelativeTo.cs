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
	/// The base date that each FX reset fixing is made relative to.
	/// <para>
	/// When calculating the FX reset fixing dates for a swap leg, the date is calculated relative to another date.
	/// The other date is specified by this enum.
	/// </para>
	/// </summary>
	public sealed class FxResetFixingRelativeTo : NamedEnum
	{

	  /// <summary>
	  /// The FX reset fixing is made relative to the start of the first accrual period.
	  /// <para>
	  /// The fixing date is relative to the start date of the first accrual period
	  /// within the payment period, as adjusted by business day conventions.
	  /// </para>
	  /// </summary>
	  public static readonly FxResetFixingRelativeTo PERIOD_START = new FxResetFixingRelativeTo("PERIOD_START", InnerEnum.PERIOD_START);
	  /// <summary>
	  /// The FX reset fixing is made relative to the end of the last accrual period.
	  /// <para>
	  /// The fixing date is relative to the end date of the last accrual period
	  /// within the payment period, as adjusted by business day conventions.
	  /// </para>
	  /// </summary>
	  public static readonly FxResetFixingRelativeTo PERIOD_END = new FxResetFixingRelativeTo("PERIOD_END", InnerEnum.PERIOD_END);

	  private static readonly IList<FxResetFixingRelativeTo> valueList = new List<FxResetFixingRelativeTo>();

	  static FxResetFixingRelativeTo()
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

	  private FxResetFixingRelativeTo(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FxResetFixingRelativeTo> NAMES = com.opengamma.strata.collect.named.EnumNames.of(FxResetFixingRelativeTo.class);

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
//ORIGINAL LINE: @FromString public static FxResetFixingRelativeTo of(String name)
	  public static FxResetFixingRelativeTo of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  // selects the base date for fixing
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


		public static IList<FxResetFixingRelativeTo> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FxResetFixingRelativeTo valueOf(string name)
		{
			foreach (FxResetFixingRelativeTo enumInstance in FxResetFixingRelativeTo.valueList)
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