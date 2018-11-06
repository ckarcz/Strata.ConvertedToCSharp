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
	/// The type of an Exchange Traded Derivative (ETD) settlement.
	/// <para>
	/// This is used for Flex options.
	/// </para>
	/// </summary>
	public sealed class EtdSettlementType : NamedEnum
	{

	  /// <summary>
	  /// Cash settlement. </summary>
	  public static readonly EtdSettlementType CASH = new EtdSettlementType("CASH", InnerEnum.CASH, "C");
	  /// <summary>
	  /// Physical settlement. </summary>
	  public static readonly EtdSettlementType PHYSICAL = new EtdSettlementType("PHYSICAL", InnerEnum.PHYSICAL, "E");
	  /// <summary>
	  /// Derivative. </summary>
	  public static readonly EtdSettlementType DERIVATIVE = new EtdSettlementType("DERIVATIVE", InnerEnum.DERIVATIVE, "D");
	  /// <summary>
	  /// Notional Settlement. </summary>
	  public static readonly EtdSettlementType NOTIONAL = new EtdSettlementType("NOTIONAL", InnerEnum.NOTIONAL, "N");
	  /// <summary>
	  /// Payment-versus-Payment. </summary>
	  public static readonly EtdSettlementType PAYMENT_VS_PAYMENT = new EtdSettlementType("PAYMENT_VS_PAYMENT", InnerEnum.PAYMENT_VS_PAYMENT, "P");
	  /// <summary>
	  /// Stock. </summary>
	  public static readonly EtdSettlementType STOCK = new EtdSettlementType("STOCK", InnerEnum.STOCK, "S");
	  /// <summary>
	  /// Cascade. </summary>
	  public static readonly EtdSettlementType CASCADE = new EtdSettlementType("CASCADE", InnerEnum.CASCADE, "T");
	  /// <summary>
	  /// Alternate. </summary>
	  public static readonly EtdSettlementType ALTERNATE = new EtdSettlementType("ALTERNATE", InnerEnum.ALTERNATE, "A");

	  private static readonly IList<EtdSettlementType> valueList = new List<EtdSettlementType>();

	  static EtdSettlementType()
	  {
		  valueList.Add(CASH);
		  valueList.Add(PHYSICAL);
		  valueList.Add(DERIVATIVE);
		  valueList.Add(NOTIONAL);
		  valueList.Add(PAYMENT_VS_PAYMENT);
		  valueList.Add(STOCK);
		  valueList.Add(CASCADE);
		  valueList.Add(ALTERNATE);
	  }

	  public enum InnerEnum
	  {
		  CASH,
		  PHYSICAL,
		  DERIVATIVE,
		  NOTIONAL,
		  PAYMENT_VS_PAYMENT,
		  STOCK,
		  CASCADE,
		  ALTERNATE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<EtdSettlementType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(EtdSettlementType.class);

	  /// <summary>
	  /// The single letter code used for the settlement type.
	  /// </summary>
	  private readonly string code;

	  private EtdSettlementType(string name, InnerEnum innerEnum, string code)
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
//ORIGINAL LINE: @FromString public static EtdSettlementType of(String name)
	  public static EtdSettlementType of(string name)
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


		public static IList<EtdSettlementType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static EtdSettlementType valueOf(string name)
		{
			foreach (EtdSettlementType enumInstance in EtdSettlementType.valueList)
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