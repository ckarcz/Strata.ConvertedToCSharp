/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;

	/// <summary>
	/// Helper for creating common types of surfaces.
	/// </summary>
	public sealed class Surfaces
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Surfaces()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-tenor volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryTenor(string name, DayCount dayCount)
	  {
		return blackVolatilityByExpiryTenor(SurfaceName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-tenor volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryTenor(SurfaceName name, DayCount dayCount)
	  {
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.BLACK_VOLATILITY).dayCount(dayCount).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-strike volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent strike
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryStrike(string name, DayCount dayCount)
	  {
		return blackVolatilityByExpiryStrike(SurfaceName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-strike volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent strike
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryStrike(SurfaceName name, DayCount dayCount)
	  {
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.BLACK_VOLATILITY).dayCount(dayCount).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-log moneyness volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent log-moneyness
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryLogMoneyness(string name, DayCount dayCount)
	  {
		return blackVolatilityByExpiryLogMoneyness(SurfaceName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Black expiry-log moneyness volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent log-moneyness
	  /// The z-values represent Black volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata blackVolatilityByExpiryLogMoneyness(SurfaceName name, DayCount dayCount)
	  {
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).dayCount(dayCount).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-tenor volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpiryTenor(string name, DayCount dayCount)
	  {
		return normalVolatilityByExpiryTenor(SurfaceName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-tenor volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpiryTenor(SurfaceName name, DayCount dayCount)
	  {
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(dayCount).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-strike volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent strike
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpiryStrike(string name, DayCount dayCount)
	  {
		return normalVolatilityByExpiryStrike(SurfaceName.of(name), dayCount);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-strike volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent strike
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpiryStrike(SurfaceName name, DayCount dayCount)
	  {
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(dayCount).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-simple moneyness volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent simple moneyness.
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="moneynessType">  the moneyness type, prices or rates </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpirySimpleMoneyness(string name, DayCount dayCount, MoneynessType moneynessType)
	  {

		return normalVolatilityByExpirySimpleMoneyness(SurfaceName.of(name), dayCount, moneynessType);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing Normal expiry-simple moneyness volatility.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent simple moneyness.
	  /// The z-values represent Normal volatility.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="moneynessType">  the moneyness type, prices or rates </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata normalVolatilityByExpirySimpleMoneyness(SurfaceName name, DayCount dayCount, MoneynessType moneynessType)
	  {

		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SIMPLE_MONEYNESS).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(dayCount).addInfo(SurfaceInfoType.MONEYNESS_TYPE, moneynessType).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates metadata for a surface providing a SABR expiry-tenor parameter.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="zType">  the z-value type, which must be one of the four SABR values </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata sabrParameterByExpiryTenor(string name, DayCount dayCount, ValueType zType)
	  {

		return sabrParameterByExpiryTenor(SurfaceName.of(name), dayCount, zType);
	  }

	  /// <summary>
	  /// Creates metadata for a surface providing a SABR expiry-tenor parameter.
	  /// <para>
	  /// The x-values represent time to expiry year fractions as defined by the specified day count.
	  /// The y-values represent tenor year fractions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the surface name </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="zType">  the z-value type, which must be one of the four SABR values </param>
	  /// <returns> the surface metadata </returns>
	  public static SurfaceMetadata sabrParameterByExpiryTenor(SurfaceName name, DayCount dayCount, ValueType zType)
	  {

		if (!zType.Equals(ValueType.SABR_ALPHA) && !zType.Equals(ValueType.SABR_BETA) && !zType.Equals(ValueType.SABR_RHO) && !zType.Equals(ValueType.SABR_NU))
		{
		  throw new System.ArgumentException("SABR z-value type must be SabrAlpha, SabrBeta, SabrRho or SabrNu");
		}
		return DefaultSurfaceMetadata.builder().surfaceName(name).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(zType).dayCount(dayCount).build();
	  }

	}

}