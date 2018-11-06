using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The formula for accrual on default.
	/// <para>
	/// This specifies which formula is used in {@code IsdaCdsProductPricer} for computing the accrued payment on default. 
	/// The formula is 'original ISDA', 'Markit fix' or 'correct'.
	/// </para>
	/// </summary>
	public sealed class AccrualOnDefaultFormula : NamedEnum
	{

	  /// <summary>
	  /// The formula in v1.8.1 and below.
	  /// </summary>
	  public static readonly AccrualOnDefaultFormula ORIGINAL_ISDA = new AccrualOnDefaultFormula("ORIGINAL_ISDA", InnerEnum.ORIGINAL_ISDA, "OriginalISDA");

	  /// <summary>
	  /// The correction proposed by Markit (v 1.8.2).
	  /// </summary>
	  public static readonly AccrualOnDefaultFormula MARKIT_FIX = new AccrualOnDefaultFormula("MARKIT_FIX", InnerEnum.MARKIT_FIX, "MarkitFix");

	  /// <summary>
	  /// The mathematically correct formula.
	  /// </summary>
	  public static readonly AccrualOnDefaultFormula CORRECT = new AccrualOnDefaultFormula("CORRECT", InnerEnum.CORRECT, "Correct");

	  private static readonly IList<AccrualOnDefaultFormula> valueList = new List<AccrualOnDefaultFormula>();

	  static AccrualOnDefaultFormula()
	  {
		  valueList.Add(ORIGINAL_ISDA);
		  valueList.Add(MARKIT_FIX);
		  valueList.Add(CORRECT);
	  }

	  public enum InnerEnum
	  {
		  ORIGINAL_ISDA,
		  MARKIT_FIX,
		  CORRECT
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<AccrualOnDefaultFormula> NAMES = com.opengamma.strata.collect.named.EnumNames.ofManualToString(AccrualOnDefaultFormula.class);

	  // name
	  private readonly string name;

	  // create
	  private AccrualOnDefaultFormula(string name, InnerEnum innerEnum, string name)
	  {
		this.name = name;

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
//ORIGINAL LINE: @FromString public static AccrualOnDefaultFormula of(String name)
	  public static AccrualOnDefaultFormula of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the omega value. 
	  /// <para>
	  /// The omega value is used in <seealso cref="IsdaCdsProductPricer"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the omega value </returns>
	  public double Omega
	  {
		  get
		  {
			if (this == ORIGINAL_ISDA)
			{
			  return 1d / 730d;
			}
			else
			{
			  return 0d;
			}
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
		return name;
	  }


		public static IList<AccrualOnDefaultFormula> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static AccrualOnDefaultFormula valueOf(string name)
		{
			foreach (AccrualOnDefaultFormula enumInstance in AccrualOnDefaultFormula.valueList)
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