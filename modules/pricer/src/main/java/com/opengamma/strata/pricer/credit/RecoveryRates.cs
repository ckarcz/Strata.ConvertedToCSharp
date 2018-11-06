/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using StandardId = com.opengamma.strata.basics.StandardId;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;

	/// <summary>
	/// Recovery rates.
	/// <para>
	/// This represents the recovery rates of a legal entity.
	/// </para>
	/// </summary>
	public interface RecoveryRates : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Obtains an instance from a curve.
	  /// <para>
	  /// If the curve is {@code ConstantCurve}, {@code ConstantRecoveryRates} is always instantiated. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity identifier </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="curve">  the underlying curve </param>
	  /// <returns> the instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static RecoveryRates of(com.opengamma.strata.basics.StandardId legalEntityId, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve curve)
	//  {
	//	if (curve.getMetadata().getYValueType().equals(ValueType.RECOVERY_RATE))
	//	{
	//	  ConstantCurve constantCurve = (ConstantCurve) curve;
	//	  return ConstantRecoveryRates.of(legalEntityId, valuationDate, constantCurve.getYValue());
	//	}
	//	throw new IllegalArgumentException("Unknown curve type");
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date. 
	  /// </summary>
	  /// <returns> the valuation date </returns>
	  LocalDate ValuationDate {get;}

	  /// <summary>
	  /// Gets the standard identifier of a legal entity.
	  /// </summary>
	  /// <returns> the legal entity ID </returns>
	  StandardId LegalEntityId {get;}

	  /// <summary>
	  /// Gets the recovery rate for the specified date. 
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <returns> the recovery rate </returns>
	  double recoveryRate(LocalDate date);

	  RecoveryRates withParameter(int parameterIndex, double newValue);

	  RecoveryRates withPerturbation(ParameterPerturbation perturbation);

	}

}