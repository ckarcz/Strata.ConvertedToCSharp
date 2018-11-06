/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// Volatility for Ibor future options in the normal or Bachelier model.
	/// </summary>
	public interface NormalIborFutureOptionVolatilities : IborFutureOptionVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.NORMAL_VOLATILITY;
	//  }

	  NormalIborFutureOptionVolatilities withParameter(int parameterIndex, double newValue);

	  NormalIborFutureOptionVolatilities withPerturbation(ParameterPerturbation perturbation);

	}

}