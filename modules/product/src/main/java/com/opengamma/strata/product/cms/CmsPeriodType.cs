using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// A CMS payment period type.
	/// <para>
	/// A CMS payment period is a CMS coupon, CMS caplet or CMS floorlet.
	/// All of these payments are defined in a unified manner by <seealso cref="CmsPeriod"/>.
	/// </para>
	/// </summary>
	public sealed class CmsPeriodType : NamedEnum
	{

	  /// <summary>
	  /// CMS coupon.
	  /// </summary>
	  public static readonly CmsPeriodType COUPON = new CmsPeriodType("COUPON", InnerEnum.COUPON);
	  /// <summary>
	  /// CMS caplet.
	  /// </summary>
	  public static readonly CmsPeriodType CAPLET = new CmsPeriodType("CAPLET", InnerEnum.CAPLET);
	  /// <summary>
	  /// CMS floorlet.
	  /// </summary>
	  public static readonly CmsPeriodType FLOORLET = new CmsPeriodType("FLOORLET", InnerEnum.FLOORLET);

	  private static readonly IList<CmsPeriodType> valueList = new List<CmsPeriodType>();

	  static CmsPeriodType()
	  {
		  valueList.Add(COUPON);
		  valueList.Add(CAPLET);
		  valueList.Add(FLOORLET);
	  }

	  public enum InnerEnum
	  {
		  COUPON,
		  CAPLET,
		  FLOORLET
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CmsPeriodType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CmsPeriodType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CmsPeriodType.class);

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
//ORIGINAL LINE: @FromString public static CmsPeriodType of(String name)
	  public static CmsPeriodType of(string name)
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


		public static IList<CmsPeriodType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CmsPeriodType valueOf(string name)
		{
			foreach (CmsPeriodType enumInstance in CmsPeriodType.valueList)
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