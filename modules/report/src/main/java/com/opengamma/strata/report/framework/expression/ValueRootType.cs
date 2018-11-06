using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// Enumerates the possible value path roots.
	/// </summary>
	public sealed class ValueRootType
	{

	  /// <summary>
	  /// Refers to the set of possible calculated measures.
	  /// </summary>
	  public static readonly ValueRootType MEASURES = new ValueRootType("MEASURES", InnerEnum.MEASURES, "Measures");
	  /// <summary>
	  /// Refers to the product on the trade.
	  /// </summary>
	  public static readonly ValueRootType PRODUCT = new ValueRootType("PRODUCT", InnerEnum.PRODUCT, "Product");
	  /// <summary>
	  /// Refers to the security on the trade.
	  /// </summary>
	  public static readonly ValueRootType SECURITY = new ValueRootType("SECURITY", InnerEnum.SECURITY, "Security");
	  /// <summary>
	  /// Refers to the trade.
	  /// </summary>
	  public static readonly ValueRootType TRADE = new ValueRootType("TRADE", InnerEnum.TRADE, "Trade");
	  /// <summary>
	  /// Refers to the position.
	  /// </summary>
	  public static readonly ValueRootType POSITION = new ValueRootType("POSITION", InnerEnum.POSITION, "Position");
	  /// <summary>
	  /// Refers to the target (trade or position).
	  /// </summary>
	  public static readonly ValueRootType TARGET = new ValueRootType("TARGET", InnerEnum.TARGET, "Target");

	  private static readonly IList<ValueRootType> valueList = new List<ValueRootType>();

	  static ValueRootType()
	  {
		  valueList.Add(MEASURES);
		  valueList.Add(PRODUCT);
		  valueList.Add(SECURITY);
		  valueList.Add(TRADE);
		  valueList.Add(POSITION);
		  valueList.Add(TARGET);
	  }

	  public enum InnerEnum
	  {
		  MEASURES,
		  PRODUCT,
		  SECURITY,
		  TRADE,
		  POSITION,
		  TARGET
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The name of the token.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly string token_Renamed;

	  /// <summary>
	  /// The complete set of valid roots.
	  /// </summary>
	  private static readonly IList<string> VALID_ROOTS = java.util.Arrays.stream(values()).map(r => r.token).collect(toImmutableList());

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="token">  the root token name </param>
	  internal ValueRootType(string name, InnerEnum innerEnum, string token)
	  {
		this.token_Renamed = token;

		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the token that the root type corresponds to.
	  /// </summary>
	  /// <returns> the token </returns>
	  public string token()
	  {
		return token_Renamed;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Parses a string into the corresponding root type.
	  /// </summary>
	  /// <param name="rootString">  the token </param>
	  /// <returns> the root type corresponding to the given string </returns>
	  public static ValueRootType parseToken(string rootString)
	  {
		return java.util.values().Where(val => val.token.equalsIgnoreCase(rootString)).First().orElseThrow(() => new System.ArgumentException(Messages.format("Invalid root: {}. Value path must start with one of: {}", rootString, VALID_ROOTS)));
	  }


		public static IList<ValueRootType> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static ValueRootType valueOf(string name)
		{
			foreach (ValueRootType enumInstance in ValueRootType.valueList)
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