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
	/// A convention defining how to process a floating rate reset schedule.
	/// <para>
	/// When calculating interest, there may be multiple reset dates for a given accrual period.
	/// This typically involves an average of a number of different rate fixings.
	/// </para>
	/// </summary>
	public sealed class IborRateResetMethod : NamedEnum
	{

	  /// <summary>
	  /// The unweighted average method.
	  /// <para>
	  /// The result is a simple average of the applicable rates.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2a(3C).
	  /// </para>
	  /// </summary>
	  public static readonly IborRateResetMethod UNWEIGHTED = new IborRateResetMethod("UNWEIGHTED", InnerEnum.UNWEIGHTED);
	  /// <summary>
	  /// The weighted average method.
	  /// <para>
	  /// The result is a weighted average of the applicable rates based on the
	  /// number of days each rate is applicable.
	  /// </para>
	  /// <para>
	  /// Defined by the 2006 ISDA definitions article 6.2a(3D).
	  /// </para>
	  /// </summary>
	  public static readonly IborRateResetMethod WEIGHTED = new IborRateResetMethod("WEIGHTED", InnerEnum.WEIGHTED);

	  private static readonly IList<IborRateResetMethod> valueList = new List<IborRateResetMethod>();

	  static IborRateResetMethod()
	  {
		  valueList.Add(UNWEIGHTED);
		  valueList.Add(WEIGHTED);
	  }

	  public enum InnerEnum
	  {
		  UNWEIGHTED,
		  WEIGHTED
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private IborRateResetMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<IborRateResetMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(IborRateResetMethod.class);

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
//ORIGINAL LINE: @FromString public static IborRateResetMethod of(String name)
	  public static IborRateResetMethod of(string name)
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


		public static IList<IborRateResetMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static IborRateResetMethod valueOf(string name)
		{
			foreach (IborRateResetMethod enumInstance in IborRateResetMethod.valueList)
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