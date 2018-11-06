using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using FxOptionVolatilities = com.opengamma.strata.pricer.fxopt.FxOptionVolatilities;
	using FxOptionVolatilitiesId = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesId;

	/// <summary>
	/// Market data function that builds FX option volatilities.
	/// <para>
	/// This function creates FX option volatilities, turning {@code FxOptionVolatilitiesId} into {@code FxOptionVolatilities}.
	/// </para>
	/// </summary>
	public class FxOptionVolatilitiesMarketDataFunction : MarketDataFunction<FxOptionVolatilities, FxOptionVolatilitiesId>
	{

	  public virtual MarketDataRequirements requirements(FxOptionVolatilitiesId id, MarketDataConfig marketDataConfig)
	  {

		FxOptionVolatilitiesDefinition volatilitiesDefinition = marketDataConfig.get(typeof(FxOptionVolatilitiesDefinition), id.Name.Name);
		return MarketDataRequirements.builder().addValues(volatilitiesDefinition.volatilitiesInputs()).build();
	  }

	  public virtual MarketDataBox<FxOptionVolatilities> build(FxOptionVolatilitiesId id, MarketDataConfig marketDataConfig, ScenarioMarketData marketData, ReferenceData refData)
	  {

		FxOptionVolatilitiesDefinition volatilitiesDefinition = marketDataConfig.get(typeof(FxOptionVolatilitiesDefinition), id.Name.Name);
		ValuationZoneTimeDefinition zoneTimeDefinition = marketDataConfig.get(typeof(ValuationZoneTimeDefinition));
		int nScenarios = marketData.ScenarioCount;
		MarketDataBox<LocalDate> valuationDates = marketData.ValuationDate;
		MarketDataBox<ZonedDateTime> valuationDateTimes = zoneTimeDefinition.toZonedDateTime(valuationDates);

		int nParameters = volatilitiesDefinition.ParameterCount;
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<MarketDataBox<double>> inputs = volatilitiesDefinition.volatilitiesInputs().Select(q => marketData.getValue(q)).collect(toImmutableList());
		ImmutableList<FxOptionVolatilities> vols = IntStream.range(0, nScenarios).mapToObj(scenarioIndex => volatilitiesDefinition.volatilities(valuationDateTimes.getValue(scenarioIndex), DoubleArray.of(nParameters, paramIndex => inputs.get(paramIndex).getValue(scenarioIndex)), refData)).collect(toImmutableList());

		return nScenarios > 1 ? MarketDataBox.ofScenarioValues(vols) : MarketDataBox.ofSingleValue(vols.get(0));
	  }

	  public virtual Type<FxOptionVolatilitiesId> MarketDataIdType
	  {
		  get
		  {
			return typeof(FxOptionVolatilitiesId);
		  }
	  }

	}

}