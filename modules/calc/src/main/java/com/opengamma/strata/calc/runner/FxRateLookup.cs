/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using FxMatrixId = com.opengamma.strata.data.FxMatrixId;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// The lookup that provides access to FX rates in market data.
	/// <para>
	/// An instance of <seealso cref="MarketData"/> can contain many different FX rates.
	/// This lookup allows a specific set of rates to be obtained.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public interface FxRateLookup : CalculationParameter
	{

	  /// <summary>
	  /// Obtains the standard instance.
	  /// <para>
	  /// This expects the market data to contain instances of <seealso cref="FxRateId"/>
	  /// based on the default <seealso cref="ObservableSource"/>.
	  /// Triangulation will use the default of the currency, typically USD.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofRates()
	//  {
	//	return DefaultFxRateLookup.DEFAULT;
	//  }

	  /// <summary>
	  /// Obtains the standard instance.
	  /// <para>
	  /// This expects the market data to contain instances of <seealso cref="FxRateId"/>
	  /// based on the specified <seealso cref="ObservableSource"/>.
	  /// Triangulation will use the default of the currency, typically USD.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="observableSource">  the source of observable market data </param>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofRates(com.opengamma.strata.data.ObservableSource observableSource)
	//  {
	//	return new DefaultFxRateLookup(observableSource);
	//  }

	  /// <summary>
	  /// Obtains an instance that uses triangulation on the specified currency.
	  /// <para>
	  /// This expects the market data to contain instances of <seealso cref="FxRateId"/>
	  /// based on the default <seealso cref="ObservableSource"/>.
	  /// Triangulation will use the specified currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="triangulationCurrency">  the triangulation currency </param>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofRates(com.opengamma.strata.basics.currency.Currency triangulationCurrency)
	//  {
	//	return new DefaultFxRateLookup(triangulationCurrency, ObservableSource.NONE);
	//  }

	  /// <summary>
	  /// Obtains an instance that uses triangulation on the specified currency.
	  /// <para>
	  /// This expects the market data to contain instances of <seealso cref="FxRateId"/>
	  /// based on the specified <seealso cref="ObservableSource"/>.
	  /// Triangulation will use the specified currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="triangulationCurrency">  the triangulation currency </param>
	  /// <param name="observableSource">  the source of observable market data </param>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofRates(com.opengamma.strata.basics.currency.Currency triangulationCurrency, com.opengamma.strata.data.ObservableSource observableSource)
	//  {
	//	return new DefaultFxRateLookup(triangulationCurrency, observableSource);
	//  }

	  /// <summary>
	  /// Obtains an instance that uses an FX matrix.
	  /// <para>
	  /// This expects the market data to contain an instance of <seealso cref="FxMatrix"/>
	  /// accessed by the standard <seealso cref="FxMatrixId"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofMatrix()
	//  {
	//	return MatrixFxRateLookup.DEFAULT;
	//  }

	  /// <summary>
	  /// Obtains an instance that uses an FX matrix.
	  /// <para>
	  /// This expects the market data to contain an instance of <seealso cref="FxMatrix"/>
	  /// accessed by the specified <seealso cref="FxMatrixId"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="matrixId">  the FX matrix identifier </param>
	  /// <returns> the FX rate lookup </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static FxRateLookup ofMatrix(com.opengamma.strata.data.FxMatrixId matrixId)
	//  {
	//	return new MatrixFxRateLookup(matrixId);
	//  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default Class queryType()
	//  {
	//	return FxRateLookup.class;
	//  }

	  /// <summary>
	  /// Obtains an FX rate provider based on the specified market data.
	  /// <para>
	  /// This provides an <seealso cref="FxRateProvider"/> suitable for obtaining FX rates.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="marketData">  the complete set of market data for one scenario </param>
	  /// <returns> the FX rate provider </returns>
	  FxRateProvider fxRateProvider(MarketData marketData);

	}

}