using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using MarketData = com.opengamma.strata.data.MarketData;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using IsdaCreditCurveDefinition = com.opengamma.strata.market.curve.IsdaCreditCurveDefinition;
	using NodalCurve = com.opengamma.strata.market.curve.NodalCurve;
	using CdsIndexIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIndexIsdaCreditCurveNode;
	using CdsIsdaCreditCurveNode = com.opengamma.strata.market.curve.node.CdsIsdaCreditCurveNode;
	using LegalEntityInformation = com.opengamma.strata.market.observable.LegalEntityInformation;
	using LegalEntityInformationId = com.opengamma.strata.market.observable.LegalEntityInformationId;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ResolvedTradeParameterMetadata = com.opengamma.strata.market.param.ResolvedTradeParameterMetadata;

	/// <summary>
	/// ISDA compliant index curve calibrator.
	/// <para>
	/// A single credit curve (index curve) is calibrated for CDS index trades.
	/// </para>
	/// <para>
	/// The curve is defined using one or more <seealso cref="CdsIndexIsdaCreditCurveNode nodes"/>.
	/// Each node primarily defines enough information to produce a reference CDS index trade.
	/// All of the curve nodes must be based on a common CDS index ID and currency.
	/// </para>
	/// <para>
	/// Calibration involves pricing, and re-pricing, these trades to find the best fit using a root finder, 
	/// where the pricing is based on <seealso cref="IsdaHomogenousCdsIndexTradePricer"/>, thus the calibration is 
	/// completed by using a calibrator for single name CDS trades, <seealso cref="IsdaCompliantCreditCurveCalibrator"/>.
	/// </para>
	/// <para>
	/// Relevant discount curve and recovery rate curve are required to complete the calibration.
	/// </para>
	/// </summary>
	public class IsdaCompliantIndexCurveCalibrator
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  private static readonly IsdaCompliantIndexCurveCalibrator STANDARD = new IsdaCompliantIndexCurveCalibrator(FastCreditCurveCalibrator.standard());

	  /// <summary>
	  /// The underlying credit curve calibrator.
	  /// </summary>
	  private readonly IsdaCompliantCreditCurveCalibrator creditCurveCalibrator;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the standard curve calibrator.
	  /// <para>
	  /// The accuracy of the root finder is set to be its default, 1.0e-12;
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard curve calibrator </returns>
	  public static IsdaCompliantIndexCurveCalibrator standard()
	  {
		return IsdaCompliantIndexCurveCalibrator.STANDARD;
	  }

	  /// <summary>
	  /// Constructor with the underlying credit curve calibrator specified. 
	  /// </summary>
	  /// <param name="creditCurveCalibrator">  the credit curve calibrator </param>
	  public IsdaCompliantIndexCurveCalibrator(IsdaCompliantCreditCurveCalibrator creditCurveCalibrator)
	  {
		this.creditCurveCalibrator = ArgChecker.notNull(creditCurveCalibrator, "creditCurveCalibrator");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrates the index curve to the market data.
	  /// <para>
	  /// This creates the single credit curve for CDS index trades.
	  /// The curve nodes in {@code IsdaCreditCurveDefinition} must be CDS index.
	  /// </para>
	  /// <para>
	  /// The relevant discount curve and recovery rate curve must be stored in {@code ratesProvider}. 
	  /// The day count convention for the resulting credit curve is the same as that of the discount curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveDefinition">  the curve definition </param>
	  /// <param name="marketData">  the market data </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the index curve </returns>
	  public virtual LegalEntitySurvivalProbabilities calibrate(IsdaCreditCurveDefinition curveDefinition, MarketData marketData, ImmutableCreditRatesProvider ratesProvider, ReferenceData refData)
	  {

		ArgChecker.isTrue(curveDefinition.CurveValuationDate.Equals(ratesProvider.ValuationDate), "ratesProvider and curveDefinition must be based on the same valuation date");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<CdsIndexIsdaCreditCurveNode> curveNodes = curveDefinition.CurveNodes.Where(n => n is CdsIndexIsdaCreditCurveNode).Select(n => (CdsIndexIsdaCreditCurveNode) n).collect(Guavate.toImmutableList());
		// Homogeneity of curveNode will be checked within IsdaCompliantCreditCurveCalibrator
		double indexFactor = computeIndexFactor(curveNodes.get(0), marketData);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<CdsIsdaCreditCurveNode> cdsNodes = curveNodes.Select(i => toCdsNode(i)).collect(Guavate.toImmutableList());
		LegalEntitySurvivalProbabilities creditCurve = creditCurveCalibrator.calibrate(cdsNodes, curveDefinition.Name, marketData, ratesProvider, curveDefinition.DayCount, curveDefinition.Currency, curveDefinition.ComputeJacobian, false, refData);
		NodalCurve underlyingCurve = ((IsdaCreditDiscountFactors) creditCurve.SurvivalProbabilities).Curve;
		CurveMetadata metadata = underlyingCurve.Metadata.withInfo(CurveInfoType.CDS_INDEX_FACTOR, indexFactor);
		if (curveDefinition.StoreNodeTrade)
		{
		  int nNodes = curveDefinition.CurveNodes.size();
		  ImmutableList<ParameterMetadata> parameterMetadata = IntStream.range(0, nNodes).mapToObj(n => ResolvedTradeParameterMetadata.of(curveNodes.get(n).trade(1d, marketData, refData).UnderlyingTrade.resolve(refData), curveNodes.get(n).Label)).collect(Guavate.toImmutableList());
		  metadata = metadata.withParameterMetadata(parameterMetadata);
		}
		NodalCurve curveWithFactor = underlyingCurve.withMetadata(metadata);

		return LegalEntitySurvivalProbabilities.of(creditCurve.LegalEntityId, IsdaCreditDiscountFactors.of(creditCurve.Currency, creditCurve.ValuationDate, curveWithFactor));
	  }

	  //-------------------------------------------------------------------------
	  private CdsIsdaCreditCurveNode toCdsNode(CdsIndexIsdaCreditCurveNode index)
	  {
		return CdsIsdaCreditCurveNode.builder().label(index.Label).legalEntityId(index.CdsIndexId).observableId(index.ObservableId).quoteConvention(index.QuoteConvention).template(index.Template).fixedRate(index.FixedRate.HasValue ? index.FixedRate.Value : null).build();
	  }

	  private double computeIndexFactor(CdsIndexIsdaCreditCurveNode node, MarketData marketData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		double numDefaulted = node.LegalEntityIds.Select(s => marketData.getValue(LegalEntityInformationId.of(s))).Select(typeof(LegalEntityInformation).cast).Where(LegalEntityInformation::isDefaulted).ToList().size();
		double numTotal = node.LegalEntityIds.size();
		return (numTotal - numDefaulted) / numTotal;
	  }

	}

}