/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.statistics.distribution
{
	/// <summary>
	/// Provides double precision machine constants
	/// </summary>
	// CSOFF: AbbreviationAsWordInName
	internal sealed class D1MACH
	{

	  /// <summary>
	  /// Smallest normalised number representable by a double according to IEEE
	  /// It's about 2.225E-308 </summary>
	  /// <returns> Smallest normalised number representable by a double according to IEEE  </returns>
	  internal static double one()
	  {
		return Double.longBitsToDouble(0x0010000000000000L);
	  }

	  /// <summary>
	  /// Largest normalised number representable by a double according to IEEE
	  /// It's about 1.7975E+308 </summary>
	  /// <returns> Largest normalised number representable by a double according to IEEE  </returns>
	  internal static double two()
	  {
		return Double.longBitsToDouble(0x7fefffffffffffffL);
	  }

	  /// <summary>
	  /// Machine precision/machine radix according to IEEE
	  /// Approximately 1.11E-16 === Math.pow(2,-53)/2 (assuming radix 2) </summary>
	  /// <returns> Machine precision/machine radix according to IEEE </returns>

	  internal static double three()
	  {
		return Double.longBitsToDouble(4368491638549381120L);
	  }

	  /// <summary>
	  /// Machine precision according to IEEE
	  /// Approximately 2.22E-16 === Math.pow(2,-53) </summary>
	  /// <returns> Machine precision according to IEEE </returns>
	  internal static double four()
	  {
		return Double.longBitsToDouble(4372995238176751616L);
	  }

	  /// <summary>
	  /// Log10(Machine radix) </summary>
	  /// <returns> Log10(Machine radix) </returns>
	  internal static double five()
	  {
		return Double.longBitsToDouble(4599094494223104511L);
	  }

	}

}