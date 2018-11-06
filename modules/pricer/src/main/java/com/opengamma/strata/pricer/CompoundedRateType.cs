using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A compounded rate type.
	/// <para>
	/// Compounded rate is continuously compounded rate or periodically compounded rate.
	/// The main application of this is z-spread computation under a specific way of compounding.
	/// See, for example, <seealso cref="DiscountFactors"/>.
	/// </para>
	/// </summary>
	public sealed class CompoundedRateType : NamedEnum
	{

	  /// <summary>
	  /// Periodic compounding.
	  /// <para>
	  /// The rate is periodically compounded.
	  /// In this case the number of periods par year should be specified in addition.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundedRateType PERIODIC = new CompoundedRateType("PERIODIC", InnerEnum.PERIODIC);
	  /// <summary>
	  /// Continuous compounding.
	  /// <para>
	  /// The rate is continuously compounded.
	  /// </para>
	  /// </summary>
	  public static readonly CompoundedRateType CONTINUOUS = new CompoundedRateType("CONTINUOUS", InnerEnum.CONTINUOUS);

	  private static readonly IList<CompoundedRateType> valueList = new List<CompoundedRateType>();

	  static CompoundedRateType()
	  {
		  valueList.Add(PERIODIC);
		  valueList.Add(CONTINUOUS);
	  }

	  public enum InnerEnum
	  {
		  PERIODIC,
		  CONTINUOUS
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CompoundedRateType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CompoundedRateType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CompoundedRateType.class);

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
//ORIGINAL LINE: @FromString public static CompoundedRateType of(String name)
	  public static CompoundedRateType of(string name)
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


		public static IList<CompoundedRateType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CompoundedRateType valueOf(string name)
		{
			foreach (CompoundedRateType enumInstance in CompoundedRateType.valueList)
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