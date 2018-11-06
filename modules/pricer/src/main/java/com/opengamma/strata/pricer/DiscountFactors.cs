/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.pricer.SimpleDiscountFactors.EFFECTIVE_ZERO;


	using Currency = com.opengamma.strata.basics.currency.Currency;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataView = com.opengamma.strata.market.MarketDataView;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
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
	/// </summary>
	public interface DiscountFactors : MarketDataView, ParameterizedData
	{

	  /// <summary>
	  /// Obtains an instance from a curve.
	  /// <para>
	  /// The curve is specified by an instance of <seealso cref="Curve"/>, such as <seealso cref="InterpolatedNodalCurve"/>.
	  /// The curve must have x-values of <seealso cref="ValueType#YEAR_FRACTION year fractions"/> with
	  /// the day count specified. The y-values must be <seealso cref="ValueType#ZERO_RATE zero rates"/>
	  /// or <seealso cref="ValueType#DISCOUNT_FACTOR discount factors"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="valuationDate">  the valuation date for which the curve is valid </param>
	  /// <param name="curve">  the underlying curve </param>
	  /// <returns> the discount factors view </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static DiscountFactors of(com.opengamma.strata.basics.currency.Currency currency, java.time.LocalDate valuationDate, com.opengamma.strata.market.curve.Curve curve)
	//  {
	//	if (curve.getMetadata().getYValueType().equals(ValueType.DISCOUNT_FACTOR))
	//	{
	//	  return SimpleDiscountFactors.of(currency, valuationDate, curve);
	//	}
	//	if (curve.getMetadata().getYValueType().equals(ValueType.ZERO_RATE))
	//	{
	//	  Optional<int> frequencyOpt = curve.getMetadata().findInfo(CurveInfoType.COMPOUNDING_PER_YEAR);
	//	  if (frequencyOpt.isPresent())
	//	  {
	//		return ZeroRatePeriodicDiscountFactors.of(currency, valuationDate, curve);
	//	  }
	//	  return ZeroRateDiscountFactors.of(currency, valuationDate, curve);
	//	}
	//	throw new IllegalArgumentException(Messages.format("Unknown value type in discount curve, must be 'DiscountFactor' or 'ZeroRate' but was '{}'", curve.getMetadata().getYValueType()));
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

	  //-------------------------------------------------------------------------
	  DiscountFactors withParameter(int parameterIndex, double newValue);

	  DiscountFactors withPerturbation(ParameterPerturbation perturbation);

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
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
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
	  /// Returns the discount factor derivative with respect to the year fraction or time.
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <returns> the discount factor derivative </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
	  double discountFactorTimeDerivative(double yearFraction);

	  /// <summary>
	  /// Gets the discount factor for the specified date with z-spread.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the discount factor </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double discountFactorWithSpread(java.time.LocalDate date, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	double yearFraction = relativeYearFraction(date);
	//	return discountFactorWithSpread(yearFraction, zSpread, compoundedRateType, periodsPerYear);
	//  }

	  /// <summary>
	  /// Gets the discount factor for the specified year fraction with z-spread.
	  /// <para>
	  /// The discount factor represents the time value of money for the specified currency
	  /// when comparing the valuation date to the specified date.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// If the valuation date is on or after the specified date, the discount factor is 1.
	  /// </para>
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the discount factor </returns>
	  /// <exception cref="RuntimeException"> if the value cannot be obtained </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double discountFactorWithSpread(double yearFraction, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	if (Math.abs(yearFraction) < EFFECTIVE_ZERO)
	//	{
	//	  return 1d;
	//	}
	//	double df = discountFactor(yearFraction);
	//	if (compoundedRateType.equals(CompoundedRateType.PERIODIC))
	//	{
	//	  ArgChecker.notNegativeOrZero(periodsPerYear, "periodPerYear");
	//	  double ratePeriodicAnnualPlusOne = Math.pow(df, -1.0 / periodsPerYear / yearFraction) + zSpread / periodsPerYear;
	//	  return Math.pow(ratePeriodicAnnualPlusOne, -periodsPerYear * yearFraction);
	//	}
	//	else
	//	{
	//	  return df * Math.exp(-zSpread * yearFraction);
	//	}
	//  }

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
//	  public default ZeroRateSensitivity zeroRatePointSensitivity(java.time.LocalDate date)
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
//	  public default ZeroRateSensitivity zeroRatePointSensitivity(double yearFraction)
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
//	  public default ZeroRateSensitivity zeroRatePointSensitivity(java.time.LocalDate date, com.opengamma.strata.basics.currency.Currency sensitivityCurrency)
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
	  /// Calculates the zero rate point sensitivity with z-spread at the specified date.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactorWithSpread(LocalDate, double, CompoundedRateType, int)"/>.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ZeroRateSensitivity zeroRatePointSensitivityWithSpread(java.time.LocalDate date, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	double yearFraction = relativeYearFraction(date);
	//	return zeroRatePointSensitivityWithSpread(yearFraction, zSpread, compoundedRateType, periodsPerYear);
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity with z-spread at the specified year fraction.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactorWithSpread(LocalDate, double, CompoundedRateType, int)"/>.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// The year fraction must be based on {@code #relativeYearFraction(LocalDate)}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yearFraction">  the year fraction </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ZeroRateSensitivity zeroRatePointSensitivityWithSpread(double yearFraction, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	return zeroRatePointSensitivityWithSpread(yearFraction, getCurrency(), zSpread, compoundedRateType, periodsPerYear);
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity with z-spread at the specified date specifying
	  /// the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactorWithSpread(LocalDate, double, CompoundedRateType, int)"/>.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
	  /// </para>
	  /// <para>
	  /// This method allows the currency of the sensitivity to differ from the currency of the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to discount to </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ZeroRateSensitivity zeroRatePointSensitivityWithSpread(java.time.LocalDate date, com.opengamma.strata.basics.currency.Currency sensitivityCurrency, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	double yearFraction = relativeYearFraction(date);
	//	return zeroRatePointSensitivityWithSpread(yearFraction, sensitivityCurrency, zSpread, compoundedRateType, periodsPerYear);
	//  }

	  /// <summary>
	  /// Calculates the zero rate point sensitivity with z-spread at the specified year fraction specifying
	  /// the currency of the sensitivity.
	  /// <para>
	  /// This returns a sensitivity instance referring to the zero rate sensitivity of the
	  /// points that were queried in the market data.
	  /// The sensitivity refers to the result of <seealso cref="#discountFactorWithSpread(LocalDate, double, CompoundedRateType, int)"/>.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic
	  /// compounded rates of the discounting curve.
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
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the point sensitivity of the zero rate </returns>
	  /// <exception cref="RuntimeException"> if the result cannot be calculated </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default ZeroRateSensitivity zeroRatePointSensitivityWithSpread(double yearFraction, com.opengamma.strata.basics.currency.Currency sensitivityCurrency, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	//  {
	//
	//	ZeroRateSensitivity sensi = zeroRatePointSensitivity(yearFraction, sensitivityCurrency);
	//	if (Math.abs(yearFraction) < EFFECTIVE_ZERO)
	//	{
	//	  return sensi;
	//	}
	//	double factor;
	//	if (compoundedRateType.equals(CompoundedRateType.PERIODIC))
	//	{
	//	  double df = discountFactor(yearFraction);
	//	  double dfRoot = Math.pow(df, -1d / periodsPerYear / yearFraction);
	//	  factor = dfRoot / df / Math.pow(dfRoot + zSpread / periodsPerYear, periodsPerYear * yearFraction + 1d);
	//	}
	//	else
	//	{
	//	  factor = Math.exp(-zSpread * yearFraction);
	//	}
	//	return sensi.multipliedBy(factor);
	//  }

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