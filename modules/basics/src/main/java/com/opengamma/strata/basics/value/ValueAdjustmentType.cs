using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// The type of value adjustment.
	/// <para>
	/// A {@code double} value can be transformed into another value in various different ways.
	/// Each type is a function of two values, the base value and the modifying value.
	/// </para>
	/// <para>
	/// Each type represents a different way to express the same concept.
	/// For example, here is how an increase from 200 to 220 could be represented:
	/// </para>
	/// <para>
	/// <table class="border 1px solid black;border-collapse:collapse">
	/// <tr>
	/// <th>Type</th><th>baseValue</th><th>modifyingValue</th><th>Calculation</th>
	/// </tr><tr>
	/// <td>Replace</td><td>200</td><td>220</td><td>{@code result = modifyingValue = 220}</td>
	/// </tr><tr>
	/// <td>DeltaAmount</td><td>200</td><td>20</td><td>{@code result = baseValue + modifyingValue = (200 + 20) = 220}</td>
	/// </tr><tr>
	/// <td>DeltaMultiplier</td><td>200</td><td>0.1</td>
	/// <td>{@code result = baseValue + baseValue * modifyingValue = (200 + 200 * 0.1) = 220}</td>
	/// </tr><tr>
	/// <td>Multiplier</td><td>200</td><td>1.1</td><td>{@code result = baseValue * modifyingValue = (200 * 1.1) = 220}</td>
	/// </tr>
	/// </table>
	/// </para>
	/// </summary>
	public abstract class ValueAdjustmentType : NamedEnum
	{

	  /// <summary>
	  /// The modifying value replaces the base value.
	  /// The input base value is ignored.
	  /// <para>
	  /// The result is {@code modifyingValue}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ValueAdjustmentType REPLACE = new ValueAdjustmentType()
	  {
		  public double adjust(double baseValue, double modifyingValue)
		  {
			  return modifyingValue;
		  }
	  },
	  /// <summary>
	  /// Calculates the result by treating the modifying value as a delta, adding it to the base value.
	  /// <para>
	  /// The result is {@code (baseValue + modifyingValue)}.
	  /// </para>
	  /// <para>
	  /// This adjustment type can be referred to as an <i>absolute shift</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ValueAdjustmentType DELTA_AMOUNT = new ValueAdjustmentType()
	  {
		  public double adjust(double baseValue, double modifyingValue)
		  {
			  return (baseValue + modifyingValue);
		  }
	  },
	  /// <summary>
	  /// Calculates the result by treating the modifying value as a multiplication factor, adding it to the base value.
	  /// <para>
	  /// The result is {@code (baseValue + baseValue * modifyingValue)}.
	  /// </para>
	  /// <para>
	  /// This adjustment type can be referred to as a <i>relative shift</i>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ValueAdjustmentType DELTA_MULTIPLIER = new ValueAdjustmentType()
	  {
		  public double adjust(double baseValue, double modifyingValue)
		  {
			  return (baseValue + baseValue * modifyingValue);
		  }
	  },
	  /// <summary>
	  /// Calculates the result by treating the modifying value as a multiplication factor to apply to the base value.
	  /// <para>
	  /// The result is {@code (baseValue * modifyingValue)}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ValueAdjustmentType MULTIPLIER = new ValueAdjustmentType()
	  {
		  public double adjust(double baseValue, double modifyingValue)
		  {
			  return (baseValue * modifyingValue);
		  }
	  };

	  private static readonly IList<ValueAdjustmentType> valueList = new List<ValueAdjustmentType>();

	  static ValueAdjustmentType()
	  {
		  valueList.Add(REPLACE);
		  valueList.Add(DELTA_AMOUNT);
		  valueList.Add(DELTA_MULTIPLIER);
		  valueList.Add(MULTIPLIER);
	  }

	  public enum InnerEnum
	  {
		  REPLACE,
		  DELTA_AMOUNT,
		  DELTA_MULTIPLIER,
		  MULTIPLIER
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private ValueAdjustmentType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<ValueAdjustmentType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(ValueAdjustmentType.class);

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
//ORIGINAL LINE: @FromString public static ValueAdjustmentType of(String name) { return NAMES.parse(name); } public abstract double adjust(double baseValue, double modifyingValue);
	  public static ValueAdjustmentType of(string name) {return NAMES.parse(name);} public abstract double adjust(double baseValue, double modifyingValue);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ValueAdjustmentType public = new ValueAdjustmentType()
	  {
		  return NAMES.format(this);
	  }


		public static IList<ValueAdjustmentType> values()
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

		public static ValueAdjustmentType valueOf(string name)
		{
			foreach (ValueAdjustmentType enumInstance in ValueAdjustmentType.valueList)
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