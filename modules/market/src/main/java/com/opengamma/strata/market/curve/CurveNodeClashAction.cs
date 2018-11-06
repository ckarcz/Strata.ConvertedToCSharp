using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The action to perform when the dates of two curve nodes clash.
	/// <para>
	/// See <seealso cref="CurveNodeDateOrder"/> for more details.
	/// </para>
	/// </summary>
	public sealed class CurveNodeClashAction : NamedEnum
	{

	  /// <summary>
	  /// When a clash occurs, an exception is thrown.
	  /// </summary>
	  public static readonly CurveNodeClashAction EXCEPTION = new CurveNodeClashAction("EXCEPTION", InnerEnum.EXCEPTION);
	  /// <summary>
	  /// When a clash occurs, this node is dropped.
	  /// </summary>
	  public static readonly CurveNodeClashAction DROP_THIS = new CurveNodeClashAction("DROP_THIS", InnerEnum.DROP_THIS);
	  /// <summary>
	  /// When a clash occurs, the other node is dropped.
	  /// </summary>
	  public static readonly CurveNodeClashAction DROP_OTHER = new CurveNodeClashAction("DROP_OTHER", InnerEnum.DROP_OTHER);

	  private static readonly IList<CurveNodeClashAction> valueList = new List<CurveNodeClashAction>();

	  static CurveNodeClashAction()
	  {
		  valueList.Add(EXCEPTION);
		  valueList.Add(DROP_THIS);
		  valueList.Add(DROP_OTHER);
	  }

	  public enum InnerEnum
	  {
		  EXCEPTION,
		  DROP_THIS,
		  DROP_OTHER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CurveNodeClashAction(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CurveNodeClashAction> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CurveNodeClashAction.class);

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
//ORIGINAL LINE: @FromString public static CurveNodeClashAction of(String name)
	  public static CurveNodeClashAction of(string name)
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


		public static IList<CurveNodeClashAction> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CurveNodeClashAction valueOf(string name)
		{
			foreach (CurveNodeClashAction enumInstance in CurveNodeClashAction.valueList)
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