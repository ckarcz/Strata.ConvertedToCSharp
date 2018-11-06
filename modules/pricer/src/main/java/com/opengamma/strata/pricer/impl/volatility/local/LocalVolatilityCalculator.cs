/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.local
{

	using Surface = com.opengamma.strata.market.surface.Surface;

	/// <summary>
	/// Local volatility calculation.
	/// </summary>
	public interface LocalVolatilityCalculator
	{

	  /// <summary>
	  /// Computes local volatility surface from call price surface.
	  /// <para>
	  /// The interest rate and dividend rate must be zero-coupon continuously compounded rates based on respective day 
	  /// count convention.
	  /// Thus {@code interestRate} and {@code dividendRate} are functions from year fraction to zero rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="callPriceSurface">  the price surface </param>
	  /// <param name="spot">  the spot </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="dividendRate">  the dividend rate </param>
	  /// <returns> the local volatility surface </returns>
	  Surface localVolatilityFromPrice(Surface callPriceSurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate);

	  /// <summary>
	  /// Computes local volatility surface from implied volatility surface.
	  /// <para>
	  /// The implied volatility surface must be spanned by time to expiry and strike.
	  /// </para>
	  /// <para>
	  /// The interest rate and dividend rate must be zero-coupon continuously compounded rates based on 
	  /// respective day count convention.
	  /// Thus {@code interestRate} and {@code dividendRate} are functions from year fraction to zero rate.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="impliedVolatilitySurface">  the implied volatility surface </param>
	  /// <param name="spot">  the spot </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="dividendRate">  the dividend </param>
	  /// <returns> the local volatility surface </returns>
	  Surface localVolatilityFromImpliedVolatility(Surface impliedVolatilitySurface, double spot, System.Func<double, double> interestRate, System.Func<double, double> dividendRate);

	}

}