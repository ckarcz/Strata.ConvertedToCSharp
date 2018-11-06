/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolators = com.opengamma.strata.market.curve.interpolator.CurveExtrapolators;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;

	/// <summary>
	/// Provides access to discount factors for a single currency.
	/// <para>
	/// The discount factor represents the time value of money for the specified currency
	/// when comparing the valuation date to the specified date.
	/// </para>
	/// <para>
	/// This is also used for representing survival probabilities of a legal entity for a single currency.
	/// </para>
	/// </summary>
	public interface CreditDiscountFactors : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Obtains an instance from a curve.
	  /// <para>
	  /// If the curve satisfies the conditions for ISDA compliant curve, 
	  /// {@code IsdaCompliantZeroRateDiscountFactors} is always instantiated. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="curve">  the underlying curve </param>
	  /// <returns> the discount factors view </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CreditDiscountFactors of(com.opengamma.strata.basics.currency.Currency currency, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve curve)
	//  {
	//	CurveMetadata metadata = curve.getMetadata();
	//	if (metadata.getXValueType().equals(ValueType.YEAR_FRACTION) && metadata.getYValueType().equals(ValueType.ZERO_RATE))
	//	{
	//	  if (curve instanceof ConstantNodalCurve)
	//	  {
	//		ConstantNodalCurve constantCurve = (ConstantNodalCurve) curve;
	//		return IsdaCreditDiscountFactors.of(currency, valuationDate, constantCurve);
	//	  }
	//	  if (curve instanceof InterpolatedNodalCurve)
	//	  {
	//		InterpolatedNodalCurve interpolatedCurve = (InterpolatedNodalCurve) curve;
	//		ArgChecker.isTrue(interpolatedCurve.getInterpolator().equals(CurveInterpolators.PRODUCT_LINEAR), "Interpolator must be PRODUCT_LINEAR");
	//		ArgChecker.isTrue(interpolatedCurve.getExtrapolatorLeft().equals(CurveExtrapolators.FLAT), "Left extrapolator must be FLAT");
	//		ArgChecker.isTrue(interpolatedCurve.getExtrapolatorRight().equals(CurveExtrapolators.PRODUCT_LINEAR), "Right extrapolator must be PRODUCT_LINEAR");
	//		return IsdaCreditDiscountFactors.of(currency, valuationDate, interpolatedCurve);
	//	  }
	//	}
	//	throw new IllegalArgumentException("Unknown curve type");
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency.
	  /// <para>
	  /// The currency that discount factors are provided for.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  Currency Currency {get;}

	  /// <summary>
	  /// Obtains day count convention.
	  /// <para>
	  /// This is typically the day count convention of the underlying curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day count </returns>
	  DayCount DayCount {get;}

	  /// <summary>
	  /// Creates an instance of <seealso cref="DiscountFactors"/>.
	  /// </summary>
	  /// <returns> the instance </returns>
	  DiscountFactors toDiscountFactors();

	  /// <summary>
	  /// Obtains the parameter keys of the underlying curve.
	  /// </summary>
	  /// <returns> the parameter keys </returns>
	  DoubleArray ParameterKeys {get;}

	  //-------------------------------------------------------------------------
	  CreditDiscountFactors withParameter(int parameterIndex, double newValue);

	  CreditDiscountFactors withPerturbation(ParameterPerturbation perturbation);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the instance is based on an ISDA compliant curve.
	  /// <para>
	  /// This returns 'false' by default, and should be overridden when needed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> true if this is an ISDA compliant curve, false otherwise </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default boolean isIsdaCompliant()
	//  {
	//	return false;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the relative time between the valuation date and the specified date.
	  /// <para>
	  /// The {@code double} value returned from this method is used as the input to other methods.
	  /// It is typically calculated from a <seealso cref="DayCount"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <returns>  the year fraction </returns>
	  /// <exception cref="RuntimeException"> if it is not possible to convert dates to relative times </exception>
	  double relativeYearFraction(LocalDate date);

	  /// <summary>
	  /// Gets the discount factor for the specified date.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the discount factor </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double discountFactor(java.time.LocalDate date)
	//  {
	//	double yearFraction = relativeYearFraction(date);
	//	return discountFactor(yearFraction);
	//  }

	  /// <summary>
	  /// Gets the discount factor for specified year fraction.
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the discount factor </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double discountFactor(double yearFraction);

	  /// <summary>
	  /// Gets the continuously compounded zero rate for the specified date.
	  /// <para>
	  /// The continuously compounded zero rate is coherent to <seealso cref="#discountFactor(LocalDate)"/> along with 
	  /// year fraction which is computed internally in each implementation. 
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double zeroRate(java.time.LocalDate date)
	//  {
	//	double yearFraction = relativeYearFraction(date);
	//	return zeroRate(yearFraction);
	//  }

	  /// <summary>
	  /// Gets the continuously compounded zero rate for specified year fraction.
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double zeroRate(double yearFraction);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-discountFactor * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.pricer.ZeroRateSensitivity zeroRatePointSensitivity(java.time.LocalDate date)
	//  {
	//	double yearFraction = relativeYearFraction(date);
	//	return zeroRatePointSensitivity(yearFraction);
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified year fraction.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-discountFactor * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.pricer.ZeroRateSensitivity zeroRatePointSensitivity(double yearFraction)
	//  {
	//	return zeroRatePointSensitivity(yearFraction, getCurrency());
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified date specifying the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-discountFactor * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.pricer.ZeroRateSensitivity zeroRatePointSensitivity(java.time.LocalDate date, com.opengamma.strata.basics.currency.Currency sensitivityCurrency)
	//  {
	//	double yearFraction = relativeYearFraction(date);
	//	return zeroRatePointSensitivity(yearFraction, sensitivityCurrency);
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity at the specified year fraction specifying the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity typically has the value {@code (-discountFactor * yearFraction)}.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactor(LocalDate)"/>.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the market data.
	  /// </para>
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  ZeroRateSensitivity zeroRatePointSensitivity(double yearFraction, Currency sensitivityCurrency);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the parameter sensitivity from the point sensitivity.
	  /// <para>
	  /// This is used to convert a single point sensitivity to parameter sensitivity.
	  /// The calculation typically involves multiplying the point and unit sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="pointSensitivity">  the point sensitivity to convert </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities parameterSensitivity(ZeroRateSensitivity pointSensitivity);

	  /// <summary>
	  /// Creates the parameter sensitivity when the sensitivity values are known.
	  /// <para>
	  /// In most cases, <seealso cref="#parameterSensitivity(ZeroRateSensitivity)"/> should be used and manipulated.
	  /// However, it can be useful to create parameter sensitivity from pre-computed sensitivity values.
	  /// </para>
	  /// <para>
	  /// There will typically be one <seealso cref="CurrencyParameterSensitivity"/> for each underlying data
	  /// structure, such as a curve. For example, if the discount factors are based on a single discount
	  /// curve, then there will be one {@code CurrencyParameterSensitivity} in the result.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="sensitivities">  the sensitivity values, which must match the parameter count </param>
	  /// <returns> the parameter sensitivity </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
	  CurrencyParameterSensitivities createParameterSensitivity(Currency currency, DoubleArray sensitivities);

	}

}