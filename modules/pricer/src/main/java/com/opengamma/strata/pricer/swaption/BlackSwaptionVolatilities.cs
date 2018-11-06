/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// Volatility for swaptions in the log-normal or Black model.
	/// </summary>
	public interface BlackSwaptionVolatilities : SwaptionVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.BLACK_VOLATILITY;
	//  }

	  BlackSwaptionVolatilities withParameter(int parameterIndex, double newValue);

	  BlackSwaptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	}

}