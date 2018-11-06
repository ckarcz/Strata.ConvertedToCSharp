/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// Volatility for FX option in the log-normal or Black model.
	/// </summary>
	public interface BlackFxOptionVolatilities : FxOptionVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.BLACK_VOLATILITY;
	//  }

	  BlackFxOptionVolatilities withParameter(int parameterIndex, double newValue);

	  BlackFxOptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	}

}