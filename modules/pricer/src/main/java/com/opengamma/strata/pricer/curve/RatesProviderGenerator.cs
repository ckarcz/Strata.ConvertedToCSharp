using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;

	/// <summary>
	/// Generates a <seealso cref="RatesProvider"/> from a set of parameters.
	/// <para>
	/// This creates a new provider based on the specified parameters.
	/// </para>
	/// </summary>
	public interface RatesProviderGenerator
	{

	  /// <summary>
	  /// Generates a rates provider from a set of parameters.
	  /// <para>
	  /// The number of parameters passed has to match the total number of parameters in all the curves generated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters describing the provider </param>
	  /// <returns> the provider </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.pricer.rate.ImmutableRatesProvider generate(com.opengamma.strata.collect.array.DoubleArray parameters)
	//  {
	//	return generate(parameters, ImmutableMap.of());
	//  }

	  /// <summary>
	  /// Generates a rates provider from a set of parameters and calibration information.
	  /// <para>
	  /// The number of parameters passed has to match the total number of parameters in all the curves generated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters describing the provider </param>
	  /// <param name="jacobians">  the curve calibration info </param>
	  /// <returns> the provider </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.pricer.rate.ImmutableRatesProvider generate(com.opengamma.strata.collect.array.DoubleArray parameters, java.util.Map<com.opengamma.strata.market.curve.CurveName, com.opengamma.strata.market.curve.JacobianCalibrationMatrix> jacobians)
	//  {
	//	return generate(parameters, jacobians, ImmutableMap.of());
	//  }

	  /// <summary>
	  /// Generates a rates provider from a set of parameters and calibration information.
	  /// <para>
	  /// The number of parameters passed has to match the total number of parameters in all the curves generated.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters describing the provider </param>
	  /// <param name="jacobians">  the curve calibration info </param>
	  /// <param name="sensitivitiesMarketQuote">  the PV sensitivities </param>
	  /// <returns> the provider </returns>
	  ImmutableRatesProvider generate(DoubleArray parameters, IDictionary<CurveName, JacobianCalibrationMatrix> jacobians, IDictionary<CurveName, DoubleArray> sensitivitiesMarketQuote);

	}

}