using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.curve
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataConfig = com.opengamma.strata.calc.marketdata.MarketDataConfig;
	using MarketDataFunction = com.opengamma.strata.calc.marketdata.MarketDataFunction;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using Messages = com.opengamma.strata.collect.Messages;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveGroup = com.opengamma.strata.market.curve.CurveGroup;
	using CurveGroupDefinition = com.opengamma.strata.market.curve.CurveGroupDefinition;
	using CurveId = com.opengamma.strata.market.curve.CurveId;
	using RatesCurveGroup = com.opengamma.strata.market.curve.RatesCurveGroup;

	/// <summary>
	/// Market data function that locates a curve by name.
	/// <para>
	/// This function finds an instance of <seealso cref="Curve"/> using the name held in <seealso cref="CurveId"/>.
	/// </para>
	/// <para>
	/// The curve is not actually built in this class, it is extracted from an existing <seealso cref="RatesCurveGroup"/>.
	/// The curve group must be available in the {@code MarketDataLookup} passed to the <seealso cref="#build"/> method.
	/// </para>
	/// </summary>
	public class CurveMarketDataFunction : MarketDataFunction<Curve, CurveId>
	{

	  public virtual MarketDataRequirements requirements(CurveId id, MarketDataConfig config)
	  {
		CurveGroupDefinition groupDefn = config.get(typeof(CurveGroupDefinition), id.CurveGroupName);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<? extends com.opengamma.strata.market.curve.CurveGroup> groupId = groupDefn.createGroupId(id.getObservableSource());
		MarketDataId<CurveGroup> groupId = groupDefn.createGroupId(id.ObservableSource);
		return MarketDataRequirements.of(groupId);
	  }

	  public virtual MarketDataBox<Curve> build(CurveId id, MarketDataConfig config, ScenarioMarketData marketData, ReferenceData refData)
	  {

		// find curve
		CurveGroupDefinition groupDefn = config.get(typeof(CurveGroupDefinition), id.CurveGroupName);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.MarketDataId<? extends com.opengamma.strata.market.curve.CurveGroup> groupId = groupDefn.createGroupId(id.getObservableSource());
		MarketDataId<CurveGroup> groupId = groupDefn.createGroupId(id.ObservableSource);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.data.scenario.MarketDataBox<? extends com.opengamma.strata.market.curve.CurveGroup> curveGroupBox = marketData.getValue(groupId);
		MarketDataBox<CurveGroup> curveGroupBox = marketData.getValue(groupId);
		return curveGroupBox.map(curveGroup => findCurve(id, curveGroup));
	  }

	  // finds the curve
	  private Curve findCurve(CurveId id, CurveGroup curveGroup)
	  {
		return curveGroup.findCurve(id.CurveName).orElseThrow(() => new System.ArgumentException(Messages.format("No curve found: {}", id.CurveName)));
	  }

	  public virtual Type<CurveId> MarketDataIdType
	  {
		  get
		  {
			return typeof(CurveId);
		  }
	  }

	}

}