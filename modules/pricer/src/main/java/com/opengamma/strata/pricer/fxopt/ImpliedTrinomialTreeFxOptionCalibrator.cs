/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoublesPair = com.opengamma.strata.collect.tuple.DoublesPair;
	using ImpliedTrinomialTreeLocalVolatilityCalculator = com.opengamma.strata.pricer.impl.volatility.local.ImpliedTrinomialTreeLocalVolatilityCalculator;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFxVanillaOption = com.opengamma.strata.product.fxopt.ResolvedFxVanillaOption;

	/// <summary>
	/// Utilities to calibrate implied trinomial tree to Black volatilities of FX options.
	/// </summary>
	public class ImpliedTrinomialTreeFxOptionCalibrator
	{

	  /// <summary>
	  /// Number of time steps.
	  /// </summary>
	  private readonly int nSteps;

	  /// <summary>
	  /// Calibrator with the specified number of time steps.
	  /// </summary>
	  /// <param name="nSteps">  number of time steps </param>
	  public ImpliedTrinomialTreeFxOptionCalibrator(int nSteps)
	  {
		ArgChecker.isTrue(nSteps > 1, "the number of steps should be greater than 1");
		this.nSteps = nSteps;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains number of time steps.
	  /// </summary>
	  /// <returns> number of time steps </returns>
	  public virtual int NumberOfSteps
	  {
		  get
		  {
			return nSteps;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calibrate trinomial tree to Black volatilities by using a vanilla option.
	  /// <para>
	  /// {@code ResolvedFxVanillaOption} is typically the underlying option of an exotic instrument to price using the 
	  /// calibrated tree, and is used to ensure that the grid points properly cover the lifetime of the target option.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="option">  the vanilla option </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the trinomial tree data </returns>
	  public virtual RecombiningTrinomialTreeData calibrateTrinomialTree(ResolvedFxVanillaOption option, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		double timeToExpiry = volatilities.relativeTime(option.Expiry);
		CurrencyPair currencyPair = option.Underlying.CurrencyPair;
		return calibrateTrinomialTree(timeToExpiry, currencyPair, ratesProvider, volatilities);
	  }

	  /// <summary>
	  /// Calibrate trinomial tree to Black volatilities.
	  /// <para>
	  /// {@code timeToExpiry} determines the coverage of the resulting trinomial tree.
	  /// Thus this should match the time to expiry of the target instrument to price using the calibrated tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <param name="volatilities">  the Black volatility provider </param>
	  /// <returns> the trinomial tree data </returns>
	  public virtual RecombiningTrinomialTreeData calibrateTrinomialTree(double timeToExpiry, CurrencyPair currencyPair, RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		validate(ratesProvider, volatilities);
		if (timeToExpiry <= 0d)
		{
		  throw new System.ArgumentException("option expired");
		}
		Currency ccyBase = currencyPair.Base;
		Currency ccyCounter = currencyPair.Counter;
		double todayFx = ratesProvider.fxRate(currencyPair);
		DiscountFactors baseDiscountFactors = ratesProvider.discountFactors(ccyBase);
		DiscountFactors counterDiscountFactors = ratesProvider.discountFactors(ccyCounter);
		System.Func<double, double> interestRate = (double? t) =>
		{
	return counterDiscountFactors.zeroRate(t.Value);
		};
		System.Func<double, double> dividendRate = (double? t) =>
		{
	return baseDiscountFactors.zeroRate(t.Value);
		};
		System.Func<DoublesPair, double> impliedVolSurface = (DoublesPair tk) =>
		{
	double dfBase = baseDiscountFactors.discountFactor(tk.First);
	double dfCounter = counterDiscountFactors.discountFactor(tk.First);
	double forward = todayFx * dfBase / dfCounter;
	return volatilities.volatility(currencyPair, tk.First, tk.Second, forward);
		};
		ImpliedTrinomialTreeLocalVolatilityCalculator localVol = new ImpliedTrinomialTreeLocalVolatilityCalculator(nSteps, timeToExpiry);
		return localVol.calibrateImpliedVolatility(impliedVolSurface, todayFx, interestRate, dividendRate);
	  }

	  //-------------------------------------------------------------------------
	  private void validate(RatesProvider ratesProvider, BlackFxOptionVolatilities volatilities)
	  {

		ArgChecker.isTrue(ratesProvider.ValuationDate.isEqual(volatilities.ValuationDateTime.toLocalDate()), "Volatility and rate data must be for the same date");
	  }

	}

}