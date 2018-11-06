/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
{
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard FRA conventions.
	/// <para>
	/// FRA conventions are based on the details held within the <seealso cref="IborIndex"/>.
	/// As such, there is a factory method rather than constants for the conventions.
	/// </para>
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class FraConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<FraConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(FraConvention));

	  /// <summary>
	  /// Obtains a convention based on the specified index.
	  /// <para>
	  /// This uses the index name to find the matching convention.
	  /// By default, this will always return a convention, however configuration may be added
	  /// to restrict the conventions that are registered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, from which the index name is used to find the matching convention </param>
	  /// <returns> the convention </returns>
	  /// <exception cref="IllegalArgumentException"> if no convention is registered for the index </exception>
	  public static FraConvention of(IborIndex index)
	  {
		return FraConvention.of(index);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private FraConventions()
	  {
	  }

	}

}