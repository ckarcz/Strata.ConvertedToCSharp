using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ValueAdjustment = com.opengamma.strata.basics.value.ValueAdjustment;
	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;

	/// <summary>
	/// Enum representing alternative ways to apply a shift which modifies the value of a piece of market data.
	/// </summary>
	public abstract class ShiftType : NamedEnum
	{

	  /// <summary>
	  /// A relative shift where the value is scaled by the shift amount.
	  /// <para>
	  /// The shift amount is interpreted as a decimal percentage. For example, a shift amount of 0.1 is a
	  /// shift of +10% which multiplies the value by 1.1. A shift amount of -0.2 is a shift of -20%
	  /// which multiplies the value by 0.8.
	  /// </para>
	  /// <para>
	  /// {@code shiftedValue = (value + value * shiftAmount)}
	  /// </para>
	  /// <para>
	  /// {@code shiftAmount} is well-defined for nonzero {@code value}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ShiftType RELATIVE = new ShiftType()
	  {
		  public double applyShift(double value, double shiftAmount)
		  {
			  return value + value * shiftAmount;
		  }
		  public com.opengamma.strata.basics.value.ValueAdjustment toValueAdjustment(double shiftAmount)
		  {
			  return com.opengamma.strata.basics.value.ValueAdjustment.ofDeltaMultiplier(shiftAmount);
		  }
		  public double computeShift(double baseValue, double shiftedValue)
		  {
			  return shiftedValue / baseValue - 1d;
		  }
	  },

	  /// <summary>
	  /// An absolute shift where the shift amount is added to the value.
	  /// <para>
	  /// {@code shiftedValue = (value + shiftAmount)}
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ShiftType ABSOLUTE = new ShiftType()
	  {
		  public double applyShift(double value, double shiftAmount)
		  {
			  return value + shiftAmount;
		  }
		  public com.opengamma.strata.basics.value.ValueAdjustment toValueAdjustment(double shiftAmount)
		  {
			  return com.opengamma.strata.basics.value.ValueAdjustment.ofDeltaAmount(shiftAmount);
		  }
		  public double computeShift(double baseValue, double shiftedValue)
		  {
			  return shiftedValue - baseValue;
		  }
	  },

	  /// <summary>
	  /// A scaled shift where the value is multiplied by the shift.
	  /// <para>
	  /// {@code shiftedValue = (value * shiftAmount)}
	  /// </para>
	  /// <para>
	  /// {@code shiftAmount} is well-defined for nonzero {@code value}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ShiftType SCALED = new ShiftType()
	  {
		  public double applyShift(double value, double shiftAmount)
		  {
			  return value * shiftAmount;
		  }
		  public com.opengamma.strata.basics.value.ValueAdjustment toValueAdjustment(double shiftAmount)
		  {
			  return com.opengamma.strata.basics.value.ValueAdjustment.ofMultiplier(shiftAmount);
		  }
		  public double computeShift(double baseValue, double shiftedValue)
		  {
			  return shiftedValue / baseValue;
		  }
	  };

	  private static readonly IList<ShiftType> valueList = new List<ShiftType>();

	  static ShiftType()
	  {
		  valueList.Add(RELATIVE);
		  valueList.Add(ABSOLUTE);
		  valueList.Add(SCALED);
	  }

	  public enum InnerEnum
	  {
		  RELATIVE,
		  ABSOLUTE,
		  SCALED
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private ShiftType(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<ShiftType> NAMES = com.opengamma.strata.collect.named.EnumNames.of(ShiftType.class);

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
//ORIGINAL LINE: @FromString public static ShiftType of(String name) { return NAMES.parse(name); } public abstract double applyShift(double value, double shiftAmount);
	  public static ShiftType of(string name) {return NAMES.parse(name);} public abstract double applyShift(double value, double shiftAmount);

	  /// <summary>
	  /// Returns a value adjustment that applies the shift amount using appropriate logic for the shift type.
	  /// </summary>
	  /// <param name="shiftAmount">  the shift to apply </param>
	  /// <returns> a value adjustment that applies the shift amount using appropriate logic for the shift type </returns>
	  public abstract com.opengamma.strata.basics.value.ValueAdjustment toValueAdjustment(double shiftAmount);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the shift amount using appropriate logic for the shift type.
	  /// </summary>
	  /// <param name="baseValue">  the base value </param>
	  /// <param name="shiftedValue">  the shifted value </param>
	  /// <returns> the shift amount </returns>
	  public abstract double computeShift(double baseValue, double shiftedValue);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly ShiftType public = new ShiftType()
	  {
		  return NAMES.format(this);
	  }


		public static IList<ShiftType> values()
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

		public static ShiftType valueOf(string name)
		{
			foreach (ShiftType enumInstance in ShiftType.valueList)
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