/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// Volatility for swaptions in SABR model.
	/// <para>
	/// The volatility is represented in terms of SABR model parameters.
	/// </para>
	/// <para>
	/// The prices are calculated using the SABR implied volatility with respect to the Black formula.
	/// </para>
	/// </summary>
	public interface SabrSwaptionVolatilities : SwaptionVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.BLACK_VOLATILITY; // SABR implemented with Black implied volatility
	//  }

	  SabrSwaptionVolatilities withParameter(int parameterIndex, double newValue);

	  SabrSwaptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the alpha parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the alpha parameter </returns>
	  double alpha(double expiry, double tenor);

	  /// <summary>
	  /// Calculates the beta parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the beta parameter </returns>
	  double beta(double expiry, double tenor);

	  /// <summary>
	  /// Calculates the rho parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the rho parameter </returns>
	  double rho(double expiry, double tenor);

	  /// <summary>
	  /// Calculates the nu parameter for a pair of time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the nu parameter </returns>
	  double nu(double expiry, double tenor);

	  /// <summary>
	  /// Calculates the shift parameter for the specified time to expiry and instrument tenor.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <returns> the shift parameter </returns>
	  double shift(double expiry, double tenor);

	  /// <summary>
	  /// Calculates the volatility and associated sensitivities.
	  /// <para>
	  /// The derivatives are stored in an array with:
	  /// <ul>
	  /// <li>[0] derivative with respect to the forward
	  /// <li>[1] derivative with respect to the forward strike
	  /// <li>[2] derivative with respect to the alpha
	  /// <li>[3] derivative with respect to the beta
	  /// <li>[4] derivative with respect to the rho
	  /// <li>[5] derivative with respect to the nu
	  /// </ul>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry as a year fraction </param>
	  /// <param name="tenor">  the tenor of the instrument as a year fraction </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility and associated sensitivities </returns>
	  ValueDerivatives volatilityAdjoint(double expiry, double tenor, double strike, double forward);

	}

}