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
	/// Volatility for swaptions in the normal or Bachelier model.
	/// </summary>
	public interface NormalSwaptionVolatilities : SwaptionVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.NORMAL_VOLATILITY;
	//  }

	  NormalSwaptionVolatilities withParameter(int parameterIndex, double newValue);

	  NormalSwaptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	}

}