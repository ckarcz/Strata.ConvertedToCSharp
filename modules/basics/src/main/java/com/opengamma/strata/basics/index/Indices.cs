/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
	using CombinedExtendedEnum = com.opengamma.strata.collect.named.CombinedExtendedEnum;

	/// <summary>
	/// Helper for {@code Index}
	/// </summary>
	internal sealed class Indices
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly CombinedExtendedEnum<Index> ENUM_LOOKUP = CombinedExtendedEnum.of(typeof(Index));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Indices()
	  {
	  }

	}

}