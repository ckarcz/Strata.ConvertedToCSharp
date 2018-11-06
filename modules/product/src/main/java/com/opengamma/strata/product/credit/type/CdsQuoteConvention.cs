using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Market quote conventions for credit default swaps.
	/// </summary>
	public sealed class CdsQuoteConvention : NamedEnum
	{

	  /// <summary>
	  /// Par spread.
	  /// <para>
	  /// Par spread is the old (i.e. pre-April 2009) way of quoting CDSs. 
	  /// A CDS would be constructed to have an initial fair value of zero; 
	  /// the par-spread is the value of the coupon (premium) on the premium leg that makes this so. 
	  /// </para>
	  /// <para>
	  /// A zero hazard curve (or equivalent, e.g. the survival probability curve) can be implied from a set of par spread quotes 
	  /// (on the same name at different maturities) by finding the curve that gives all the CDSs a PV of zero  
	  /// (the curve is not unique and will depend on other modeling choices). 
	  /// </para>
	  /// </summary>
	  public static readonly CdsQuoteConvention PAR_SPREAD = new CdsQuoteConvention("PAR_SPREAD", InnerEnum.PAR_SPREAD);

	  /// <summary>
	  /// Points upfront.
	  /// <para>
	  /// Points upfront (PUF) is the current (as of April 2009) way of quoting CDSs. A CDS has a fixed coupon (premium). 
	  /// </para>
	  /// <para>
	  /// An up front fee is payable by the buyer of protection (i.e. the payer of the premiums) - this fee can be negative 
	  /// (i.e. an amount is received by the protection buyer). PUF is quoted as a percentage of the notional. 
	  /// </para>
	  /// <para>
	  /// A zero hazard curve (or equivalent, e.g. the survival probability curve) can be implied from a set of PUF quotes
	  /// (on the same name at different maturities) by finding the curve that gives all the CDSs a clean present value 
	  /// equal to their {@code PUF * notional}  (the curve is not unique and will depend on other modeling choices). 
	  /// </para>
	  /// </summary>
	  public static readonly CdsQuoteConvention POINTS_UPFRONT = new CdsQuoteConvention("POINTS_UPFRONT", InnerEnum.POINTS_UPFRONT);

	  /// <summary>
	  /// Quoted spread.
	  /// <para>
	  /// Quoted spread (sometimes misleadingly called flat spread) is an alternative to quoting PUF 
	  /// where people wish to see a spread like number. 
	  /// It is numerically close in value to the equivalent par spread but is not exactly the same.
	  /// </para>
	  /// <para>
	  /// To find the quoted spread of a CDS from its PUF (and premium) one first finds the unique flat hazard rate 
	  /// that will give the CDS a clean present value equal to its {@code PUF * notional}; one then finds 
	  /// the par spread (the coupon that makes the CDS have zero clean PV) of the CDS from this flat hazard curve - 
	  /// this is the quoted spread (and the reason for the confusing name, flat spread).
	  /// To go from a quoted spread to PUF, one does the reverse of the above.
	  /// </para>
	  /// <para>
	  /// A zero hazard curve (or equivalent, e.g. the survival probability curve) cannot be directly implied from 
	  /// a set of quoted spreads - one must first convert to PUF.
	  /// </para>
	  /// </summary>
	  public static readonly CdsQuoteConvention QUOTED_SPREAD = new CdsQuoteConvention("QUOTED_SPREAD", InnerEnum.QUOTED_SPREAD);

	  private static readonly IList<CdsQuoteConvention> valueList = new List<CdsQuoteConvention>();

	  static CdsQuoteConvention()
	  {
		  valueList.Add(PAR_SPREAD);
		  valueList.Add(POINTS_UPFRONT);
		  valueList.Add(QUOTED_SPREAD);
	  }

	  public enum InnerEnum
	  {
		  PAR_SPREAD,
		  POINTS_UPFRONT,
		  QUOTED_SPREAD
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private CdsQuoteConvention(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<CdsQuoteConvention> NAMES = com.opengamma.strata.collect.named.EnumNames.of(CdsQuoteConvention.class);

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
//ORIGINAL LINE: @FromString public static CdsQuoteConvention of(String name)
	  public static CdsQuoteConvention of(string name)
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


		public static IList<CdsQuoteConvention> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static CdsQuoteConvention valueOf(string name)
		{
			foreach (CdsQuoteConvention enumInstance in CdsQuoteConvention.valueList)
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