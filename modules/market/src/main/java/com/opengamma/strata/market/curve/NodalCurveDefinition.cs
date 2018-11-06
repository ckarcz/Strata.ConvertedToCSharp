/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketData = com.opengamma.strata.data.MarketData;

	/// <summary>
	/// Provides the definition of how to calibrate a nodal curve.
	/// <para>
	/// A nodal curve is built from a number of parameters and described by metadata.
	/// Calibration is based on a list of <seealso cref="CurveNode"/> instances, one for each parameter,
	/// that specify the underlying instruments.
	/// </para>
	/// </summary>
	public interface NodalCurveDefinition : CurveDefinition
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default int getParameterCount()
	//  {
	//	return getNodes().size();
	//  }

	  NodalCurveDefinition filtered(LocalDate valuationDate, ReferenceData refData);

	  NodalCurve curve(LocalDate valuationDate, CurveMetadata metadata, DoubleArray parameters);

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableList<double> initialGuess(com.opengamma.strata.data.MarketData marketData)
	//  {
	//	ImmutableList.Builder<double> result = ImmutableList.builder();
	//	for (CurveNode node : getNodes())
	//	{
	//	  result.add(node.initialGuess(marketData, getYValueType()));
	//	}
	//	return result.build();
	//  }

	}

}