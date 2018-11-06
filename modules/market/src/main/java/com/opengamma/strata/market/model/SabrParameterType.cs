using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.model
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The type of the SABR parameter - Alpha, Beta, Rho, Nu or shift.
	/// </summary>
	public sealed class SabrParameterType : NamedEnum
	{

	  /// <summary>
	  /// SABR alpha.
	  /// </summary>
	  public static readonly SabrParameterType ALPHA = new SabrParameterType("ALPHA", InnerEnum.ALPHA);
	  /// <summary>
	  /// SABR beta.
	  /// </summary>
	  public static readonly SabrParameterType BETA = new SabrParameterType("BETA", InnerEnum.BETA);
	  /// <summary>
	  /// SABR rho.
	  /// </summary>
	  public static readonly SabrParameterType RHO = new SabrParameterType("RHO", InnerEnum.RHO);
	  /// <summary>
	  /// SABR nu.
	  /// </summary>
	  public static readonly SabrParameterType NU = new SabrParameterType("NU", InnerEnum.NU);
	  /// <summary>
	  /// SABR shift.
	  /// </summary>
	  public static readonly SabrParameterType SHIFT = new SabrParameterType("SHIFT", InnerEnum.SHIFT);

	  private static readonly IList<SabrParameterType> valueList = new List<SabrParameterType>();

	  static SabrParameterType()
	  {
		  valueList.Add(ALPHA);
		  valueList.Add(BETA);
		  valueList.Add(RHO);
		  valueList.Add(NU);
		  valueList.Add(SHIFT);
	  }

	  public enum InnerEnum
	  {
		  ALPHA,
		  BETA,
		  RHO,
		  NU,
		  SHIFT
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private SabrParameterType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<SabrParameterType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(SabrParameterType.class);

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
//ORIGINAL LINE: @FromString public static SabrParameterType of(String name)
	  public static SabrParameterType of(string name)
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


		public static IList<SabrParameterType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static SabrParameterType valueOf(string name)
		{
			foreach (SabrParameterType enumInstance in SabrParameterType.valueList)
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