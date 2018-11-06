/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// Parameter metadata that specifies a date.
	/// </summary>
	public interface TenoredParameterMetadata : ParameterMetadata
	{

	  /// <summary>
	  /// Gets the tenor associated with the parameter.
	  /// <para>
	  /// This is the tenor of the parameter.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the tenor </returns>
	  Tenor Tenor {get;}

	  /// <summary>
	  /// Returns an instance with the tenor updated.
	  /// </summary>
	  /// <param name="tenor">  the tenor to update to </param>
	  /// <returns> the updated metadata </returns>
	  TenoredParameterMetadata withTenor(Tenor tenor);

	}

}