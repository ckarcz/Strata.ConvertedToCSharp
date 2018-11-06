/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Helper for measures.
	/// </summary>
	internal sealed class MeasureHelper
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<Measure> ENUM_LOOKUP = ExtendedEnum.of(typeof(Measure));

	  //-------------------------------------------------------------------------
	  private MeasureHelper()
	  {
	  }

	}

}