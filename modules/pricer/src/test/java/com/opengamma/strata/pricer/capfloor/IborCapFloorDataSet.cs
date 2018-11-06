/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using BusinessDayConventions = com.opengamma.strata.basics.date.BusinessDayConventions;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Frequency = com.opengamma.strata.basics.schedule.Frequency;
	using PeriodicSchedule = com.opengamma.strata.basics.schedule.PeriodicSchedule;
	using RollConventions = com.opengamma.strata.basics.schedule.RollConventions;
	using StubConvention = com.opengamma.strata.basics.schedule.StubConvention;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using IborCapFloorLeg = com.opengamma.strata.product.capfloor.IborCapFloorLeg;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using FixedRateCalculation = com.opengamma.strata.product.swap.FixedRateCalculation;
	using IborRateCalculation = com.opengamma.strata.product.swap.IborRateCalculation;
	using NotionalSchedule = com.opengamma.strata.product.swap.NotionalSchedule;
	using PaymentSchedule = com.opengamma.strata.product.swap.PaymentSchedule;
	using RateCalculationSwapLeg = com.opengamma.strata.product.swap.RateCalculationSwapLeg;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// Data set of Ibor cap/floor securities.
	/// </summary>
	public class IborCapFloorDataSet
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly BusinessDayAdjustment BUSINESS_ADJ = BusinessDayAdjustment.of(BusinessDayConventions.MODIFIED_FOLLOWING, EUTA);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an Ibor cap/floor leg.
	  /// <para>
	  /// The Ibor index should be {@code EUR_EURIBOR_3M} or {@code EUR_EURIBOR_6M} to match the availability of the curve 
	  /// data in <seealso cref="IborCapletFloorletDataSet"/>. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="strikeSchedule">  the strike </param>
	  /// <param name="notionalSchedule">  the notional </param>
	  /// <param name="putCall">  cap or floor </param>
	  /// <param name="payRec">  pay or receive </param>
	  /// <returns> the instance </returns>
	  public static ResolvedIborCapFloorLeg createCapFloorLeg(IborIndex index, LocalDate startDate, LocalDate endDate, ValueSchedule strikeSchedule, ValueSchedule notionalSchedule, PutCall putCall, PayReceive payRec)
	  {

		IborCapFloorLeg leg = createCapFloorLegUnresolved(index, startDate, endDate, strikeSchedule, notionalSchedule, putCall, payRec);
		return leg.resolve(REF_DATA);
	  }

	  /// <summary>
	  /// Creates an Ibor cap/floor leg.
	  /// <para>
	  /// The Ibor index should be {@code EUR_EURIBOR_3M} or {@code EUR_EURIBOR_6M} to match the availability of the curve 
	  /// data in <seealso cref="IborCapletFloorletDataSet"/>. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="strikeSchedule">  the strike </param>
	  /// <param name="notionalSchedule">  the notional </param>
	  /// <param name="putCall">  cap or floor </param>
	  /// <param name="payRec">  pay or receive </param>
	  /// <returns> the instance </returns>
	  public static IborCapFloorLeg createCapFloorLegUnresolved(IborIndex index, LocalDate startDate, LocalDate endDate, ValueSchedule strikeSchedule, ValueSchedule notionalSchedule, PutCall putCall, PayReceive payRec)
	  {

		Frequency frequency = Frequency.of(index.Tenor.Period);
		PeriodicSchedule paySchedule = PeriodicSchedule.of(startDate, endDate, frequency, BUSINESS_ADJ, StubConvention.NONE, RollConventions.NONE);
		IborRateCalculation rateCalculation = IborRateCalculation.of(index);
		if (putCall.Call)
		{
		  return IborCapFloorLeg.builder().calculation(rateCalculation).capSchedule(strikeSchedule).notional(notionalSchedule).paymentSchedule(paySchedule).payReceive(payRec).build();
		}
		return IborCapFloorLeg.builder().calculation(rateCalculation).floorSchedule(strikeSchedule).notional(notionalSchedule).paymentSchedule(paySchedule).payReceive(payRec).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Create a pay leg.
	  /// <para>
	  /// The pay leg created is periodic fixed rate payments without compounding.
	  /// The Ibor index is used to specify the payment frequency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="payRec">  pay or receive </param>
	  /// <returns> the instance </returns>
	  public static ResolvedSwapLeg createFixedPayLeg(IborIndex index, LocalDate startDate, LocalDate endDate, double fixedRate, double notional, PayReceive payRec)
	  {

		SwapLeg leg = createFixedPayLegUnresolved(index, startDate, endDate, fixedRate, notional, payRec);
		return leg.resolve(REF_DATA);
	  }

	  /// <summary>
	  /// Create a pay leg.
	  /// <para>
	  /// The pay leg created is periodic fixed rate payments without compounding.
	  /// The Ibor index is used to specify the payment frequency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="startDate">  the start date </param>
	  /// <param name="endDate">  the end date </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <param name="notional">  the notional </param>
	  /// <param name="payRec">  pay or receive </param>
	  /// <returns> the instance </returns>
	  public static SwapLeg createFixedPayLegUnresolved(IborIndex index, LocalDate startDate, LocalDate endDate, double fixedRate, double notional, PayReceive payRec)
	  {

		Frequency frequency = Frequency.of(index.Tenor.Period);
		PeriodicSchedule accSchedule = PeriodicSchedule.of(startDate, endDate, frequency, BUSINESS_ADJ, StubConvention.NONE, RollConventions.NONE);
		return RateCalculationSwapLeg.builder().payReceive(payRec).accrualSchedule(accSchedule).calculation(FixedRateCalculation.of(fixedRate, ACT_360)).paymentSchedule(PaymentSchedule.builder().paymentFrequency(frequency).paymentDateOffset(DaysAdjustment.NONE).build()).notionalSchedule(NotionalSchedule.of(CurrencyAmount.of(EUR, notional))).build();
	  }

	}

}