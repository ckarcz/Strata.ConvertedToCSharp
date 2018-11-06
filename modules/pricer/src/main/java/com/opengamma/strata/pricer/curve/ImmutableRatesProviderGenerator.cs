using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.curve
{

	using HashMultimap = com.google.common.collect.HashMultimap;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSetMultimap = com.google.common.collect.ImmutableSetMultimap;
	using SetMultimap = com.google.common.collect.SetMultimap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveDefinition = com.opengamma.strata.market.curve.CurveDefinition;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using JacobianCalibrationMatrix = com.opengamma.strata.market.curve.JacobianCalibrationMatrix;
	using RatesCurveGroupDefinition = com.opengamma.strata.market.curve.RatesCurveGroupDefinition;
	using RatesCurveGroupEntry = com.opengamma.strata.market.curve.RatesCurveGroupEntry;
	using ImmutableRatesProvider = com.opengamma.strata.pricer.rate.ImmutableRatesProvider;

	/// <summary>
	/// Generates a rates provider based on an existing provider.
	/// <para>
	/// This takes a base <seealso cref="ImmutableRatesProvider"/> and list of curve definitions
	/// to generate a child provider.
	/// </para>
	/// </summary>
	public sealed class ImmutableRatesProviderGenerator : RatesProviderGenerator
	{

	  /// <summary>
	  /// The underlying known data.
	  /// This includes curves and FX matrix.
	  /// </summary>
	  private readonly ImmutableRatesProvider knownProvider;
	  /// <summary>
	  /// The curve definitions for the new curves to be generated.
	  /// </summary>
	  private readonly ImmutableList<CurveDefinition> curveDefinitions;
	  /// <summary>
	  /// The list of curve metadata associated with each definition.
	  /// The size of this list must match the size of the definition list.
	  /// </summary>
	  private readonly ImmutableList<CurveMetadata> curveMetadata;
	  /// <summary>
	  /// The map between curve name and currencies for discounting.
	  /// The map should contains all the curve in the definition list but may have more names
	  /// than the curve definition list. Only the curves in the definitions list are created.
	  /// </summary>
	  private readonly ImmutableSetMultimap<CurveName, Currency> discountCurveNames;
	  /// <summary>
	  /// The map between curve name and indices for forward rates and prices.
	  /// The map should contains all the curve in the definition list but may have more names
	  /// than the curve definition list. Only the curves in the definitions list are created
	  /// </summary>
	  private readonly ImmutableSetMultimap<CurveName, Index> forwardCurveNames;

	  /// <summary>
	  /// Obtains a generator from an existing provider and definition.
	  /// </summary>
	  /// <param name="knownProvider">  the underlying known provider </param>
	  /// <param name="groupDefn">  the curve group definition </param>
	  /// <param name="refData">  the reference data to use </param>
	  /// <returns> the generator </returns>
	  public static ImmutableRatesProviderGenerator of(ImmutableRatesProvider knownProvider, RatesCurveGroupDefinition groupDefn, ReferenceData refData)
	  {

		IList<CurveDefinition> curveDefns = new List<CurveDefinition>();
		IList<CurveMetadata> curveMetadata = new List<CurveMetadata>();
		SetMultimap<CurveName, Currency> discountNames = HashMultimap.create();
		SetMultimap<CurveName, Index> indexNames = HashMultimap.create();

		foreach (CurveDefinition curveDefn in groupDefn.CurveDefinitions)
		{
		  curveDefns.Add(curveDefn);
		  curveMetadata.Add(curveDefn.metadata(knownProvider.ValuationDate, refData));
		  CurveName curveName = curveDefn.Name;
		  // A curve group is guaranteed to include an entry for every definition
		  RatesCurveGroupEntry entry = groupDefn.findEntry(curveName).get();
		  ISet<Currency> ccy = entry.DiscountCurrencies;
		  discountNames.putAll(curveName, ccy);
		  indexNames.putAll(curveName, entry.Indices);
		}
		return new ImmutableRatesProviderGenerator(knownProvider, curveDefns, curveMetadata, discountNames, indexNames);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="knownProvider">  the underlying known provider </param>
	  /// <param name="curveDefinitions">  the curve definitions </param>
	  /// <param name="curveMetadata">  the curve metadata </param>
	  /// <param name="discountCurveNames">  the map of discount curves </param>
	  /// <param name="forwardCurveNames">  the map of index forward curves </param>
	  private ImmutableRatesProviderGenerator(ImmutableRatesProvider knownProvider, IList<CurveDefinition> curveDefinitions, IList<CurveMetadata> curveMetadata, SetMultimap<CurveName, Currency> discountCurveNames, SetMultimap<CurveName, Index> forwardCurveNames)
	  {

		this.knownProvider = ArgChecker.notNull(knownProvider, "knownProvider");
		this.curveDefinitions = ImmutableList.copyOf(ArgChecker.notNull(curveDefinitions, "curveDefinitions"));
		this.curveMetadata = ImmutableList.copyOf(ArgChecker.notNull(curveMetadata, "curveMetadata"));
		this.discountCurveNames = ImmutableSetMultimap.copyOf(ArgChecker.notNull(discountCurveNames, "discountCurveNames"));
		this.forwardCurveNames = ImmutableSetMultimap.copyOf(ArgChecker.notNull(forwardCurveNames, "forwardCurveNames"));
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableRatesProvider generate(DoubleArray parameters, IDictionary<CurveName, JacobianCalibrationMatrix> jacobians, IDictionary<CurveName, DoubleArray> sensitivitiesMarketQuote)
	  {

		// collect curves for child provider based on existing provider
		IDictionary<Currency, Curve> discountCurves = new Dictionary<Currency, Curve>();
		IDictionary<Index, Curve> indexCurves = new Dictionary<Index, Curve>();
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		discountCurves.putAll(knownProvider.DiscountCurves);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		indexCurves.putAll(knownProvider.IndexCurves);

		// generate curves from combined parameter array
		int startIndex = 0;
		for (int i = 0; i < curveDefinitions.size(); i++)
		{
		  CurveDefinition curveDefn = curveDefinitions.get(i);
		  CurveMetadata metadata = curveMetadata.get(i);
		  CurveName name = curveDefn.Name;
		  // extract parameters for the child curve
		  int paramCount = curveDefn.ParameterCount;
		  DoubleArray curveParams = parameters.subArray(startIndex, startIndex + paramCount);
		  startIndex += paramCount;
		  // create the child curve
		  CurveMetadata childMetadata = this.childMetadata(metadata, curveDefn, jacobians, sensitivitiesMarketQuote);
		  Curve curve = curveDefn.curve(knownProvider.ValuationDate, childMetadata, curveParams);
		  // put child curve into maps
		  ISet<Currency> currencies = discountCurveNames.get(name);
		  foreach (Currency currency in currencies)
		  {
			discountCurves[currency] = curve;
		  }
		  ISet<Index> indices = forwardCurveNames.get(name);
		  foreach (Index index in indices)
		  {
			indexCurves[index] = curve;
		  }
		}
		return knownProvider.toBuilder().discountCurves(discountCurves).indexCurves(indexCurves).build();
	  }

	  // build the map of additional info
	  private CurveMetadata childMetadata(CurveMetadata metadata, CurveDefinition curveDefn, IDictionary<CurveName, JacobianCalibrationMatrix> jacobians, IDictionary<CurveName, DoubleArray> sensitivitiesMarketQuote)
	  {

		JacobianCalibrationMatrix jacobian = jacobians[curveDefn.Name];
		CurveMetadata metadataResult = metadata;
		if (jacobian != null)
		{
		  metadataResult = metadata.withInfo(CurveInfoType.JACOBIAN, jacobian);
		}
		DoubleArray sensitivity = sensitivitiesMarketQuote[curveDefn.Name];
		if (sensitivity != null)
		{
		  metadataResult = metadataResult.withInfo(CurveInfoType.PV_SENSITIVITY_TO_MARKET_QUOTE, sensitivity);
		}
		return metadataResult;
	  }

	}

}