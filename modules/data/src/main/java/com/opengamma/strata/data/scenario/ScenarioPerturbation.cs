using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// A perturbation that can be applied to a market data box to create market data
	/// for use in one or more scenarios.
	/// <para>
	/// A perturbation is used to change market data in some way.
	/// It applies to a single piece of data, such as a discount curve or volatility surface.
	/// For example, a 5 basis point parallel shift of a curve, or a 10% increase in the quoted price of a security.
	/// </para>
	/// <para>
	/// Implementations must be immutable and thread-safe beans.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data handled by the perturbation </param>
	public interface ScenarioPerturbation<T>
	{

	  /// <summary>
	  /// Returns an instance that does not perturb the input.
	  /// <para>
	  /// This is useful for creating base scenarios where none of the market data is perturbed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the market data handled by the perturbation </param>
	  /// <returns> a perturbation that returns its input unchanged </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <T> ScenarioPerturbation<T> none()
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> ScenarioPerturbation<T> none()
	//  {
	//	// It is safe to cast this to any type because it returns it input with no changes
	//	return (ScenarioPerturbation<T>) NoOpScenarioPerturbation.INSTANCE;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Applies this perturbation to the market data in a box, returning a box containing new, modified data.
	  /// <para>
	  /// The original market data must not be altered.
	  /// Instead a perturbed copy must be returned.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the market data to perturb </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> new market data derived by applying the perturbation to the input data </returns>
	  /// <exception cref="RuntimeException"> if unable to perform the perturbation </exception>
	  MarketDataBox<T> applyTo(MarketDataBox<T> marketData, ReferenceData refData);

	  /// <summary>
	  /// Returns the number of scenarios for which this perturbation generates data.
	  /// </summary>
	  /// <returns> the number of scenarios for which this perturbation generates data </returns>
	  int ScenarioCount {get;}

	  /// <summary>
	  /// Returns the market data type that the perturbation changes.
	  /// </summary>
	  /// <returns> the data type </returns>
	  Type<T> MarketDataType {get;}

	}

}