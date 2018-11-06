using System.Collections.Generic;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivitiesBuilder = com.opengamma.strata.market.param.CurrencyParameterSensitivitiesBuilder;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using PortfolioItemInfo = com.opengamma.strata.product.PortfolioItemInfo;

	/// <summary>
	/// Builder for {@code CurveSensitivities}.
	/// </summary>
	public sealed class CurveSensitivitiesBuilder
	{

	  /// <summary>
	  /// The info.
	  /// </summary>
	  private readonly PortfolioItemInfo info;
	  /// <summary>
	  /// The map of sensitivity data.
	  /// </summary>
	  private readonly IDictionary<CurveSensitivitiesType, CurrencyParameterSensitivitiesBuilder> data = new SortedDictionary<CurveSensitivitiesType, CurrencyParameterSensitivitiesBuilder>();

	  //-------------------------------------------------------------------------
	  // restricted constructor
	  internal CurveSensitivitiesBuilder(PortfolioItemInfo info)
	  {
		this.info = info;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a sensitivity to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type">  the sensitivity type </param>
	  /// <param name="sensitivity">  the sensitivity to ad </param>
	  /// <returns> this, for chaining </returns>
	  public CurveSensitivitiesBuilder add(CurveSensitivitiesType type, CurrencyParameterSensitivity sensitivity)
	  {

		data.computeIfAbsent(type, t => CurrencyParameterSensitivities.builder()).add(sensitivity);
		return this;
	  }

	  /// <summary>
	  /// Adds a single sensitivity to the builder.
	  /// <para>
	  /// Values with the same market data name and currency will be merged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type">  the sensitivity type </param>
	  /// <param name="curveName">  the curve name </param>
	  /// <param name="currency">  the currency of the sensitivity </param>
	  /// <param name="metadata">  the sensitivity metadata, not empty </param>
	  /// <param name="sensitivityValue">  the sensitivity value </param>
	  /// <returns> this, for chaining </returns>
	  public CurveSensitivitiesBuilder add(CurveSensitivitiesType type, CurveName curveName, Currency currency, ParameterMetadata metadata, double sensitivityValue)
	  {

		data.computeIfAbsent(type, t => CurrencyParameterSensitivities.builder()).add(curveName, currency, metadata, sensitivityValue);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the sensitivity from the provided data.
	  /// <para>
	  /// If all the values for a single sensitivity are tenor-based, or all are date-based,
	  /// then the resulting sensitivity will have the tenors sorted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the sensitivities instance </returns>
	  public CurveSensitivities build()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return CurveSensitivities.of(info, MapStream.of(data).mapValues(CurrencyParameterSensitivitiesBuilder::build).toMap());
	  }

	}

}