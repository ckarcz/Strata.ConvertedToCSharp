/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using ValueType = com.opengamma.strata.market.ValueType;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// Volatility for Ibor caplet/floorlet in the normal or Bachelier model.
	/// </summary>
	public interface NormalIborCapletFloorletVolatilities : IborCapletFloorletVolatilities
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.market.ValueType getVolatilityType()
	//  {
	//	return ValueType.NORMAL_VOLATILITY;
	//  }

	  NormalIborCapletFloorletVolatilities withParameter(int parameterIndex, double newValue);

	  NormalIborCapletFloorletVolatilities withPerturbation(ParameterPerturbation perturbation);

	}

}