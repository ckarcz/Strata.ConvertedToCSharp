/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;

	/// <summary>
	/// A term structure of smile as used in Forex market.
	/// <para>
	/// The term structure defined here is composed of smile descriptions at different times.
	/// The data of each smile contains delta and volatility in <seealso cref="SmileDeltaParameters"/>. 
	/// The delta values must be common to all of the smiles.
	/// </para>
	/// <para>
	/// The volatility and its sensitivities to data points are represented as a function of time, strike and forward.
	/// </para>
	/// </summary>
	public interface SmileDeltaTermStructure : ParameterizedData
	{

	  /// <summary>
	  /// Gets the day count convention used for the expiry.
	  /// </summary>
	  /// <returns> the day count </returns>
	  DayCount DayCount {get;}

	  /// <summary>
	  /// Gets the number of smiles.
	  /// </summary>
	  /// <returns> the number of smiles </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getSmileCount()
	//  {
	//	return getVolatilityTerm().size();
	//  }

	  /// <summary>
	  /// Gets the number of strikes.
	  /// </summary>
	  /// <returns> the number of strikes </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getStrikeCount()
	//  {
	//	return getVolatilityTerm().get(0).getVolatility().size();
	//  }

	  /// <summary>
	  /// Gets delta values.
	  /// </summary>
	  /// <returns> the delta values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.array.DoubleArray getDelta()
	//  {
	//	return getVolatilityTerm().get(0).getDelta();
	//  }

	  /// <summary>
	  /// Gets the volatility smiles from delta.
	  /// </summary>
	  /// <returns> the volatility smiles </returns>
	  ImmutableList<SmileDeltaParameters> VolatilityTerm {get;}

	  /// <summary>
	  /// Gets the expiries associated with the volatility term.
	  /// </summary>
	  /// <returns> the set of expiry </returns>
	  DoubleArray Expiries {get;}

	  /// <summary>
	  /// Computes full delta for all strikes including put delta absolute value.
	  /// <para>
	  /// The ATM is 0.50 delta and the x call are transformed in 1-x put.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the delta </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.array.DoubleArray getDeltaFull()
	//  {
	//	int nbDelta = getDelta().size();
	//	double[] result = new double[2 * nbDelta + 1];
	//	for (int loopdelta = 0; loopdelta < nbDelta; loopdelta++)
	//	{
	//	  result[loopdelta] = getDelta().get(loopdelta);
	//	  result[nbDelta + 1 + loopdelta] = 1.0 - getDelta().get(nbDelta - 1 - loopdelta);
	//	}
	//	result[nbDelta] = 0.50;
	//	return DoubleArray.ofUnsafe(result);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the volatility at a given time/strike/forward from the term structure.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility </returns>
	  double volatility(double expiry, double strike, double forward);

	  /// <summary>
	  /// Calculates the volatility and the volatility sensitivity with respect to the volatility data points.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry </param>
	  /// <param name="strike">  the strike </param>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the volatility </returns>
	  VolatilityAndBucketedSensitivities volatilityAndSensitivities(double expiry, double strike, double forward);

	  /// <summary>
	  /// Calculates the smile at a given time.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry </param>
	  /// <returns> the smile </returns>
	  SmileDeltaParameters smileForExpiry(double expiry);

	  /// <summary>
	  /// Calculates the smile at a given time and the sensitivities with respect to the volatility data points.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry </param>
	  /// <param name="volatilityAtTimeSensitivity">  the sensitivity to the volatilities of the smile at the given time </param>
	  /// <returns> the smile and sensitivities </returns>
	  SmileAndBucketedSensitivities smileAndSensitivitiesForExpiry(double expiry, DoubleArray volatilityAtTimeSensitivity);

	  //-------------------------------------------------------------------------
	  SmileDeltaTermStructure withParameter(int parameterIndex, double newValue);

	  SmileDeltaTermStructure withPerturbation(ParameterPerturbation perturbation);

	}

}