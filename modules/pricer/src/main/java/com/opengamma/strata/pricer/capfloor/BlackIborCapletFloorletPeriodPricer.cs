/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Pricer for caplet/floorlet in a log-normal or Black model.
	/// <para>
	/// The value of the caplet/floorlet after expiry is a fixed payoff amount. The value is zero if valuation date is 
	/// after payment date of the caplet/floorlet.
	/// </para>
	/// </summary>
	public class BlackIborCapletFloorletPeriodPricer : VolatilityIborCapletFloorletPeriodPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly BlackIborCapletFloorletPeriodPricer DEFAULT = new BlackIborCapletFloorletPeriodPricer();

	  protected internal override void validate(IborCapletFloorletVolatilities volatilities)
	  {
		ArgChecker.isTrue(volatilities is BlackIborCapletFloorletVolatilities, "volatilities must be Black volatilities");
	  }

	}

}