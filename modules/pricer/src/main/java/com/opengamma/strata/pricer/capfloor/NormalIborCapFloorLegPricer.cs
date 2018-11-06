/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
	using IborCapletFloorletPeriod = com.opengamma.strata.product.capfloor.IborCapletFloorletPeriod;

	/// <summary>
	/// Pricer for cap/floor legs in normal or Bachelier model.
	/// </summary>
	public class NormalIborCapFloorLegPricer : VolatilityIborCapFloorLegPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public new static readonly NormalIborCapFloorLegPricer DEFAULT = new NormalIborCapFloorLegPricer(NormalIborCapletFloorletPeriodPricer.DEFAULT);

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="periodPricer">  the pricer for <seealso cref="IborCapletFloorletPeriod"/>. </param>
	  public NormalIborCapFloorLegPricer(NormalIborCapletFloorletPeriodPricer periodPricer) : base(periodPricer)
	  {
	  }

	}

}