/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{

	/// <summary>
	/// Parameter metadata that specifies a date.
	/// </summary>
	public interface DatedParameterMetadata : ParameterMetadata
	{

	  /// <summary>
	  /// Gets the date associated with the parameter.
	  /// <para>
	  /// This is the date that is most closely associated with the parameter.
	  /// The actual parameter is typically a year fraction based on a day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the date </returns>
	  LocalDate Date {get;}

	}

}