using System.Collections.Generic;

/*
 * Copyright (C) 2013 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.result
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Represents the reason why failure occurred.
	/// <para>
	/// Each failure is categorized as one of the following reasons.
	/// </para>
	/// </summary>
	public sealed class FailureReason : NamedEnum
	{

	  /// <summary>
	  /// There were multiple failures of different types.
	  /// <para>
	  /// An operation may produce zero to many errors.
	  /// If there is one error then that reason is used.
	  /// If there are many errors then the overall reason is "multiple".
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason MULTIPLE = new FailureReason("MULTIPLE", InnerEnum.MULTIPLE);
	  /// <summary>
	  /// An error occurred.
	  /// <para>
	  /// Where possible, a more specific reason code should be used.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason ERROR = new FailureReason("ERROR", InnerEnum.ERROR);
	  /// <summary>
	  /// The input was invalid.
	  /// <para>
	  /// One or more input parameters was invalid.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason INVALID = new FailureReason("INVALID", InnerEnum.INVALID);
	  /// <summary>
	  /// A parsing error occurred.
	  /// <para>
	  /// This is used when an error occurred during parsing.
	  /// Typically, this refers to parsing a file, such as CSV or XML.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason PARSING = new FailureReason("PARSING", InnerEnum.PARSING);
	  /// <summary>
	  /// The operation requested was not applicable.
	  /// <para>
	  /// This is used when the particular combination of inputs is not applicable,
	  /// but given a different combination a result could have been calculated.
	  /// For example, this might occur in a grid of results where the calculation
	  /// requested for a column is not applicable for every row.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason NOT_APPLICABLE = new FailureReason("NOT_APPLICABLE", InnerEnum.NOT_APPLICABLE);
	  /// <summary>
	  /// The operation requested is unsupported.
	  /// <para>
	  /// The operation failed because it is not supported.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason UNSUPPORTED = new FailureReason("UNSUPPORTED", InnerEnum.UNSUPPORTED);
	  /// <summary>
	  /// The operation failed because data was missing.
	  /// <para>
	  /// One or more pieces of data that the operation required were missing.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason MISSING_DATA = new FailureReason("MISSING_DATA", InnerEnum.MISSING_DATA);
	  /// <summary>
	  /// Currency conversion failed.
	  /// <para>
	  /// This is used to indicate that the operation failed during currency conversion, perhaps due to missing FX rates.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason CURRENCY_CONVERSION = new FailureReason("CURRENCY_CONVERSION", InnerEnum.CURRENCY_CONVERSION);
	  /// <summary>
	  /// The operation could not be performed.
	  /// <para>
	  /// This is used to indicate that a calculation failed.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason CALCULATION_FAILED = new FailureReason("CALCULATION_FAILED", InnerEnum.CALCULATION_FAILED);
	  /// <summary>
	  /// Failure occurred for some other reason.
	  /// <para>
	  /// This reason should only be used when no other type is applicable.
	  /// If using this reason, please consider raising an issue to get another
	  /// more descriptive reason added.
	  /// </para>
	  /// </summary>
	  public static readonly FailureReason OTHER = new FailureReason("OTHER", InnerEnum.OTHER);

	  private static readonly IList<FailureReason> valueList = new List<FailureReason>();

	  static FailureReason()
	  {
		  valueList.Add(MULTIPLE);
		  valueList.Add(ERROR);
		  valueList.Add(INVALID);
		  valueList.Add(PARSING);
		  valueList.Add(NOT_APPLICABLE);
		  valueList.Add(UNSUPPORTED);
		  valueList.Add(MISSING_DATA);
		  valueList.Add(CURRENCY_CONVERSION);
		  valueList.Add(CALCULATION_FAILED);
		  valueList.Add(OTHER);
	  }

	  public enum InnerEnum
	  {
		  MULTIPLE,
		  ERROR,
		  INVALID,
		  PARSING,
		  NOT_APPLICABLE,
		  UNSUPPORTED,
		  MISSING_DATA,
		  CURRENCY_CONVERSION,
		  CALCULATION_FAILED,
		  OTHER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private FailureReason(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  // this enum is unusual in that the names are just the standard enum names
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FailureReason> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(FailureReason.class);

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
//ORIGINAL LINE: @FromString public static FailureReason of(String name)
	  public static FailureReason of(string name)
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
		return name();
	  }


		public static IList<FailureReason> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FailureReason valueOf(string name)
		{
			foreach (FailureReason enumInstance in FailureReason.valueList)
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