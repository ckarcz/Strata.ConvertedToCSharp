using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Helper for creating common types of curves.
	/// </summary>
	public sealed class Curves
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Curves()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing zero rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata zeroRates(string name, DayCount dayCount)
	  {
		return zeroRates(CurveName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing zero rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata zeroRates(CurveName name, DayCount dayCount)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing zero rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata zeroRates<T1>(CurveName name, DayCount dayCount, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.ZERO_RATE).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing forward rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata forwardRates(string name, DayCount dayCount)
	  {
		return forwardRates(CurveName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing forward rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata forwardRates(CurveName name, DayCount dayCount)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing forward rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata forwardRates<T1>(CurveName name, DayCount dayCount, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.FORWARD_RATE).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing discount factors.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata discountFactors(string name, DayCount dayCount)
	  {
		return discountFactors(CurveName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing discount factors.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata discountFactors(CurveName name, DayCount dayCount)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing discount factors.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata discountFactors<T1>(CurveName name, DayCount dayCount, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.DISCOUNT_FACTOR).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing monthly prices, typically used in inflation.
	  /// <para>
	  /// The x-values represent months relative to an unspecified base month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata prices(string name)
	  {
		return prices(CurveName.of(name));
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing monthly prices, typically used in inflation.
	  /// <para>
	  /// The x-values represent months relative to an unspecified base month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata prices(CurveName name)
	  {
		ArgChecker.notNull(name, "name");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing monthly prices, typically used in inflation.
	  /// <para>
	  /// The x-values represent months relative to an unspecified base month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata prices<T1>(CurveName name, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		ArgChecker.notNull(name, "name");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.MONTHS).yValueType(ValueType.PRICE_INDEX).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing Black volatility by expiry.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata blackVolatilityByExpiry(string name, DayCount dayCount)
	  {
		return blackVolatilityByExpiry(CurveName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing Black volatility by expiry.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata blackVolatilityByExpiry(CurveName name, DayCount dayCount)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing Black volatility by expiry.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata blackVolatilityByExpiry<T1>(CurveName name, DayCount dayCount, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.BLACK_VOLATILITY).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates curve metadata for a curve providing recovery rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata recoveryRates(string name, DayCount dayCount)
	  {
		return recoveryRates(CurveName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing recovery rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata recoveryRates(CurveName name, DayCount dayCount)
	  {
		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.RECOVERY_RATE).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates curve metadata for a curve providing recovery rates.
	  /// <para>
	  /// The x-values represent year fractions relative to an unspecified base date
	  /// as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata recoveryRates<T1>(CurveName name, DayCount dayCount, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		ArgChecker.notNull(name, "name");
		ArgChecker.notNull(dayCount, "dayCount");
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.RECOVERY_RATE).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a curve providing a SABR parameter.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="yType">  the y-value type, which must be one of the four SABR values </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata sabrParameterByExpiry(string name, DayCount dayCount, ValueType yType)
	  {

		return sabrParameterByExpiry(CurveName.of(name), dayCount, yType);
	  }

	  /// <summary>
	  /// Creates metadata for a curve providing a SABR parameter.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="yType">  the y-value type, which must be one of the four SABR values </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata sabrParameterByExpiry(CurveName name, DayCount dayCount, ValueType yType)
	  {

		if (!yType.Equals(ValueType.SABR_ALPHA) && !yType.Equals(ValueType.SABR_BETA) && !yType.Equals(ValueType.SABR_RHO) && !yType.Equals(ValueType.SABR_NU))
		{
		  throw new System.ArgumentException("SABR y-value type must be SabrAlpha, SabrBeta, SabrRho or SabrNu");
		}
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(yType).dayCount(dayCount).build();
	  }

	  /// <summary>
	  /// Creates metadata for a curve providing a SABR parameter.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the curve name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="yType">  the y-value type, which must be one of the four SABR values </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the curve metadata </returns>
	  public static CurveMetadata sabrParameterByExpiry<T1>(CurveName name, DayCount dayCount, ValueType yType, IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {

		if (!yType.Equals(ValueType.SABR_ALPHA) && !yType.Equals(ValueType.SABR_BETA) && !yType.Equals(ValueType.SABR_RHO) && !yType.Equals(ValueType.SABR_NU))
		{
		  throw new System.ArgumentException("SABR y-value type must be SabrAlpha, SabrBeta, SabrRho or SabrNu");
		}
		return DefaultCurveMetadata.builder().curveName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(yType).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	}

}