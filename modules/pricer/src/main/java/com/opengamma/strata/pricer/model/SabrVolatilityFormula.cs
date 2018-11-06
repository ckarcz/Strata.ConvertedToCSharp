/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using SabrHaganVolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.SabrHaganVolatilityFunctionProvider;

	/// <summary>
	/// Provides volatility and sensitivity in the SABR model.
	/// </summary>
	public interface SabrVolatilityFormula
	{

	  /// <summary>
	  /// The Hagan SABR volatility formula.
	  /// <para>
	  /// This provides the functions of volatility and its sensitivity to the SABR model
	  /// parameters based on the original Hagan SABR formula.
	  /// </para>
	  /// <para>
	  /// Reference: Hagan, P.; Kumar, D.; Lesniewski, A. & Woodward, D. "Managing smile risk", Wilmott Magazine, 2002, September, 84-108
	  /// </para>
	  /// <para>
	  /// OpenGamma documentation: SABR Implementation, OpenGamma documentation n. 33, April 2016.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the SABR Hagan formula </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static SabrVolatilityFormula hagan()
	//  {
	//	return SabrHaganVolatilityFunctionProvider.DEFAULT;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="alpha">  the SABR alpha value </param>
	  /// <param name="beta">  the SABR beta value </param>
	  /// <param name="rho">  the SABR rho value </param>
	  /// <param name="nu">  the SABR nu value </param>
	  /// <returns> the volatility </returns>
	  double volatility(double forward, double strike, double timeToExpiry, double alpha, double beta, double rho, double nu);

	  /// <summary>
	  /// Calculates volatility and the adjoint (volatility sensitivity to forward, strike and model parameters). 
	  /// <para>
	  /// By default the derivatives are computed by central finite difference approximation.
	  /// This should be overridden in each subclass.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="alpha">  the SABR alpha value </param>
	  /// <param name="beta">  the SABR beta value </param>
	  /// <param name="rho">  the SABR rho value </param>
	  /// <param name="nu">  the SABR nu value </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  ValueDerivatives volatilityAdjoint(double forward, double strike, double timeToExpiry, double alpha, double beta, double rho, double nu);

	}

}