/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Pricer for caplet/floorlet in a normal or Bachelier model.
	/// <para>
	/// The value of the caplet/floorlet after expiry is a fixed payoff amount. The value is zero if valuation date is 
	/// after payment date of the caplet/floorlet.
	/// </para>
	/// </summary>
	public class NormalIborCapletFloorletPeriodPricer : VolatilityIborCapletFloorletPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly NormalIborCapletFloorletPeriodPricer DEFAULT = new NormalIborCapletFloorletPeriodPricer();

	  protected internal override void validate(IborCapletFloorletVolatilities volatilities)
	  {
		ArgChecker.isTrue(volatilities is NormalIborCapletFloorletVolatilities, "volatilities must be normal volatilities");
	  }

	}

}