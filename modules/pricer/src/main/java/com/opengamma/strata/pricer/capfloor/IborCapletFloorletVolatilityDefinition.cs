/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{

	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;

	/// <summary>
	/// Definition of caplet volatilities calibration.
	/// </summary>
	public interface IborCapletFloorletVolatilityDefinition
	{

	  /// <summary>
	  /// Gets the name of these volatilities.
	  /// </summary>
	  /// <returns> the name </returns>
	  IborCapletFloorletVolatilitiesName Name {get;}

	  /// <summary>
	  /// Gets the Ibor index for which the data is valid.
	  /// </summary>
	  /// <returns> the Ibor index </returns>
	  IborIndex Index {get;}

	  /// <summary>
	  /// Gets the day count to use.
	  /// </summary>
	  /// <returns> the day count </returns>
	  DayCount DayCount {get;}

	  /// <summary>
	  /// Creates surface metadata.
	  /// </summary>
	  /// <param name="capFloorData">  the cap/floor data </param>
	  /// <returns> the surface metadata </returns>
	  SurfaceMetadata createMetadata(RawOptionData capFloorData);

	  /// <summary>
	  /// Creates a standard cap from start date, end date and strike. 
	  /// </summary>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="strike">  the strike </param>
	  /// <returns> the cap </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.capfloor.IborCapFloorLeg createCap(java.time.LocalDate startDate, java.time.LocalDate endDate, double strike)
	//  {
	//	IborIndex index = getIndex();
	//	return IborCapFloorLeg.builder().calculation(IborRateCalculation.of(index)).capSchedule(ValueSchedule.of(strike)).currency(index.getCurrency()).notional(ValueSchedule.ALWAYS_1).paymentSchedule(PeriodicSchedule.of(startDate, endDate, Frequency.of(index.getTenor().getPeriod()), BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, index.getFixingCalendar()), StubConvention.NONE, RollConventions.NONE)).payReceive(PayReceive.RECEIVE).build();
	//  }

	}

}