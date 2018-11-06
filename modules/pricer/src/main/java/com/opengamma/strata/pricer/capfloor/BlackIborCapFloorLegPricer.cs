/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;

	/// <summary>
	/// Pricer for cap/floor legs in log-normal or Black model.
	/// </summary>
	public class BlackIborCapFloorLegPricer : VolatilityIborCapFloorLegPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly BlackIborCapFloorLegPricer DEFAULT = new BlackIborCapFloorLegPricer(BlackIborCapletFloorletPeriodPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="IborCapletFloorletPeriod"/>. </param>
	  public BlackIborCapFloorLegPricer(BlackIborCapletFloorletPeriodPricer periodPricer) : base(periodPricer)
	  {
	  }

	}

}