using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The option expiry type, 'American' or 'European'.
	/// </summary>
	public sealed class EtdOptionType : NamedEnum
	{

	  /// <summary>
	  /// American option.
	  /// Can be exercised on any date during its life.
	  /// </summary>
	  public static readonly EtdOptionType AMERICAN = new EtdOptionType("AMERICAN", InnerEnum.AMERICAN, "A");
	  /// <summary>
	  /// European option.
	  /// Can be exercised only on a single date.
	  /// </summary>
	  public static readonly EtdOptionType EUROPEAN = new EtdOptionType("EUROPEAN", InnerEnum.EUROPEAN, "E");

	  private static readonly IList<EtdOptionType> valueList = new List<EtdOptionType>();

	  static EtdOptionType()
	  {
		  valueList.Add(AMERICAN);
		  valueList.Add(EUROPEAN);
	  }

	  public enum InnerEnum
	  {
		  AMERICAN,
		  EUROPEAN
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<EtdOptionType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(EtdOptionType.class);

	  /// <summary>
	  /// The single letter code used for the settlement type.
	  /// </summary>
	  private readonly string code;

	  private EtdOptionType(string name, InnerEnum innerEnum, string code)
	  {
		this.code = code;

		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

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
//ORIGINAL LINE: @FromString public static EtdOptionType of(String name)
	  public static EtdOptionType of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the short code for the type.
	  /// </summary>
	  /// <returns> the short code </returns>
	  public string Code
	  {
		  get
		  {
			return code;
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


		public static IList<EtdOptionType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static EtdOptionType valueOf(string name)
		{
			foreach (EtdOptionType enumInstance in EtdOptionType.valueList)
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