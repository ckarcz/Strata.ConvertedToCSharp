/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// A convention defining how to round a number.
	/// <para>
	/// This defines a standard mechanism for rounding a {@code double} or <seealso cref="BigDecimal"/>.
	/// Since financial instruments have different and complex conventions, rounding is extensible.
	/// </para>
	/// <para>
	/// Note that rounding a {@code double} is not straightforward as floating point
	/// numbers are based on a binary representation, not a decimal one.
	/// For example, the value 0.1 cannot be exactly represented in a {@code double}.
	/// </para>
	/// <para>
	/// The standard implementation is <seealso cref="HalfUpRounding"/>.
	/// Additional implementations may be added by implementing this interface.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface Rounding
	{

	  /// <summary>
	  /// Obtains an instance that performs no rounding.
	  /// </summary>
	  /// <returns> the rounding convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Rounding none()
	//  {
	//	return NoRounding.INSTANCE;
	//  }

	  /// <summary>
	  /// Obtains an instance that rounds to the number of minor units in the currency.
	  /// <para>
	  /// This returns a convention that rounds for the specified currency.
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <returns> the rounding convention </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Rounding of(com.opengamma.strata.basics.currency.Currency currency)
	//  {
	//	return HalfUpRounding.ofDecimalPlaces(currency.getMinorUnitDigits());
	//  }

	  /// <summary>
	  /// Obtains an instance that rounds to the specified number of decimal places.
	  /// <para>
	  /// This returns a convention that rounds to the specified number of decimal places.
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="decimalPlaces">  the number of decimal places to round to, from 0 to 255 inclusive </param>
	  /// <returns> the rounding convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Rounding ofDecimalPlaces(int decimalPlaces)
	//  {
	//	return HalfUpRounding.ofDecimalPlaces(decimalPlaces);
	//  }

	  /// <summary>
	  /// Obtains an instance from the number of decimal places and fraction.
	  /// <para>
	  /// This returns a convention that rounds to a fraction of the specified number of decimal places.
	  /// Rounding follows the normal <seealso cref="RoundingMode#HALF_UP"/> convention.
	  /// </para>
	  /// <para>
	  /// For example, to round to the nearest 1/32nd of the 4th decimal place, call
	  /// this method with the arguments 4 and 32.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="decimalPlaces">  the number of decimal places to round to, from 0 to 255 inclusive </param>
	  /// <param name="fraction">  the fraction of the last decimal place, such as 32 for 1/32, from 0 to 255 inclusive </param>
	  /// <returns> the rounding convention </returns>
	  /// <exception cref="IllegalArgumentException"> if the decimal places or fraction is invalid </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static Rounding ofFractionalDecimalPlaces(int decimalPlaces, int fraction)
	//  {
	//	return HalfUpRounding.ofFractionalDecimalPlaces(decimalPlaces, fraction);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Rounds the specified value according to the rules of the convention.
	  /// </summary>
	  /// <param name="value">  the value to be rounded </param>
	  /// <returns> the rounded value </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double round(double value)
	//  {
	//	return round(BigDecimal.valueOf(value)).doubleValue();
	//  }

	  /// <summary>
	  /// Rounds the specified value according to the rules of the convention.
	  /// </summary>
	  /// <param name="value">  the value to be rounded </param>
	  /// <returns> the rounded value </returns>
	  decimal round(decimal value);

	}

}