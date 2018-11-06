/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxConvertible = com.opengamma.strata.basics.currency.FxConvertible;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// Point sensitivity.
	/// <para>
	/// The sensitivity to a single point on a curve, surface or similar.
	/// This is used within <seealso cref="PointSensitivities"/>.
	/// </para>
	/// <para>
	/// Each implementation of this interface will consist of two distinct parts.
	/// The first is a set of information that identifies the point.
	/// The second is the sensitivity value.
	/// </para>
	/// <para>
	/// For example, when an Ibor index is queried, the implementation would typically contain
	/// the Ibor index, fixing date and the sensitivity value.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// </para>
	/// </summary>
	public interface PointSensitivity : FxConvertible<PointSensitivity>
	{

	  /// <summary>
	  /// Gets the currency of the point sensitivity.
	  /// </summary>
	  /// <returns> the currency </returns>
	  Currency Currency {get;}

	  /// <summary>
	  /// Gets the point sensitivity value.
	  /// </summary>
	  /// <returns> the sensitivity </returns>
	  double Sensitivity {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified sensitivity currency set.
	  /// <para>
	  /// The result will consists of the same points, but with the sensitivity currency altered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the new currency </param>
	  /// <returns> an instance based on this sensitivity with the specified currency </returns>
	  PointSensitivity withCurrency(Currency currency);

	  /// <summary>
	  /// Returns an instance with the new point sensitivity value.
	  /// </summary>
	  /// <param name="sensitivity">  the new sensitivity </param>
	  /// <returns> an instance based on this sensitivity with the specified sensitivity </returns>
	  PointSensitivity withSensitivity(double sensitivity);

	  /// <summary>
	  /// Compares the key of two sensitivities, excluding the point sensitivity value.
	  /// <para>
	  /// If the other point sensitivity is of a different type, the comparison
	  /// is based solely on the simple class name.
	  /// If the point sensitivity is of the same type, the comparison must
	  /// check the key, then the currency, then the date, then any other state.
	  /// </para>
	  /// <para>
	  /// The comparison by simple class name ensures that all instances of the same
	  /// type are ordered together.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other sensitivity </param>
	  /// <returns> positive if greater, zero if equal, negative if less </returns>
	  int compareKey(PointSensitivity other);

	  /// <summary>
	  /// Converts this instance to an equivalent amount in the specified currency.
	  /// <para>
	  /// The result will be expressed in terms of the given currency.
	  /// Any FX conversion that is required will use rates from the provider.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="resultCurrency">  the currency of the result </param>
	  /// <param name="rateProvider">  the provider of FX rates </param>
	  /// <returns> the converted instance, which should be expressed in the specified currency </returns>
	  /// <exception cref="RuntimeException"> if no FX rate could be found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PointSensitivity convertedTo(com.opengamma.strata.basics.currency.Currency resultCurrency, com.opengamma.strata.basics.currency.FxRateProvider rateProvider)
	//  {
	//	if (getCurrency().equals(resultCurrency))
	//	{
	//	  return this;
	//	}
	//	double fxRate = rateProvider.fxRate(getCurrency(), resultCurrency);
	//	return withCurrency(resultCurrency).withSensitivity(fxRate * getSensitivity());
	//  }

	}

}