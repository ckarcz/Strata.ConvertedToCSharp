/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fra
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CashFlow = com.opengamma.strata.market.amount.CashFlow;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using ExplainMapBuilder = com.opengamma.strata.market.explain.ExplainMapBuilder;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using RateComputationFn = com.opengamma.strata.pricer.rate.RateComputationFn;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedFra = com.opengamma.strata.product.fra.ResolvedFra;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Pricer for for forward rate agreement (FRA) products.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedFra"/>.
	/// The product is priced using a forward curve for the index.
	/// </para>
	/// </summary>
	public class DiscountingFraProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingFraProductPricer DEFAULT = new DiscountingFraProductPricer(RateComputationFn.standard());

	  /// <summary>
	  /// Rate computation.
	  /// </summary>
	  private readonly RateComputationFn<RateComputation> rateComputationFn;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="rateComputationFn">  the rate computation function </param>
	  public DiscountingFraProductPricer(RateComputationFn<RateComputation> rateComputationFn)
	  {
		this.rateComputationFn = ArgChecker.notNull(rateComputationFn, "rateComputationFn");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the FRA product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the present value of the product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedFra fra, RatesProvider provider)
	  {
		// forecastValue * discountFactor
		double df = provider.discountFactor(fra.Currency, fra.PaymentDate);
		double pv = forecastValue0(fra, provider) * df;
		return CurrencyAmount.of(fra.Currency, pv);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the FRA product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedFra fra, RatesProvider provider)
	  {
		DiscountFactors discountFactors = provider.discountFactors(fra.Currency);
		double df = discountFactors.discountFactor(fra.PaymentDate);
		double notional = fra.Notional;
		double unitAmount = this.unitAmount(fra, provider);
		double derivative = this.derivative(fra, provider);
		PointSensitivityBuilder iborSens = forwardRateSensitivity(fra, provider).multipliedBy(derivative * df * notional);
		PointSensitivityBuilder discSens = discountFactors.zeroRatePointSensitivity(fra.PaymentDate).multipliedBy(unitAmount * notional);
		return iborSens.withCurrency(fra.Currency).combinedWith(discSens).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the forecast value of the FRA product.
	  /// <para>
	  /// The forecast value of the product is the value on the valuation date without present value discounting.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the forecast value of the product </returns>
	  public virtual CurrencyAmount forecastValue(ResolvedFra fra, RatesProvider provider)
	  {
		double fv = forecastValue0(fra, provider);
		return CurrencyAmount.of(fra.Currency, fv);
	  }

	  /// <summary>
	  /// Calculates the forecast value sensitivity of the FRA product.
	  /// <para>
	  /// The forecast value sensitivity of the product is the sensitivity of the forecast value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the point sensitivity of the forecast value </returns>
	  public virtual PointSensitivities forecastValueSensitivity(ResolvedFra fra, RatesProvider provider)
	  {
		double notional = fra.Notional;
		double derivative = this.derivative(fra, provider);
		PointSensitivityBuilder iborSens = forwardRateSensitivity(fra, provider).multipliedBy(derivative * notional);
		return iborSens.withCurrency(fra.Currency).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the par rate of the FRA product.
	  /// <para>
	  /// The par rate is the rate for which the FRA present value is 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate </returns>
	  public virtual double parRate(ResolvedFra fra, RatesProvider provider)
	  {
		return forwardRate(fra, provider);
	  }

	  /// <summary>
	  /// Calculates the par rate curve sensitivity of the FRA product.
	  /// <para>
	  /// The par rate curve sensitivity of the product is the sensitivity of the par rate to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par rate sensitivity </returns>
	  public virtual PointSensitivities parRateSensitivity(ResolvedFra fra, RatesProvider provider)
	  {
		return forwardRateSensitivity(fra, provider).build();
	  }

	  /// <summary>
	  /// Calculates the par spread of the FRA product.
	  /// <para>
	  /// This is spread to be added to the fixed rate to have a present value of 0.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread </returns>
	  public virtual double parSpread(ResolvedFra fra, RatesProvider provider)
	  {
		double forward = forwardRate(fra, provider);
		return forward - fra.FixedRate;
	  }

	  /// <summary>
	  /// Calculates the par spread curve sensitivity of the FRA product.
	  /// <para>
	  /// The par spread curve sensitivity of the product is the sensitivity of the par spread to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the par spread sensitivity </returns>
	  public virtual PointSensitivities parSpreadSensitivity(ResolvedFra fra, RatesProvider provider)
	  {
		return forwardRateSensitivity(fra, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flow of the FRA product.
	  /// <para>
	  /// There is only one cash flow on the payment date for the FRA product.
	  /// The expected currency amount of the cash flow is the same as <seealso cref="#forecastValue(ResolvedFra, RatesProvider)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the product </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the cash flows </returns>
	  public virtual CashFlows cashFlows(ResolvedFra fra, RatesProvider provider)
	  {
		LocalDate paymentDate = fra.PaymentDate;
		double forecastValue = forecastValue0(fra, provider);
		double df = provider.discountFactor(fra.Currency, paymentDate);
		CashFlow cashFlow = CashFlow.ofForecastValue(paymentDate, fra.Currency, forecastValue, df);
		return CashFlows.of(cashFlow);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Explains the present value of the FRA product.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="fra">  the FRA product for which present value should be computed </param>
	  /// <param name="provider">  the rates provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedFra fra, RatesProvider provider)
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		Currency currency = fra.Currency;
		builder.put(ExplainKey.ENTRY_TYPE, "FRA");
		builder.put(ExplainKey.PAYMENT_DATE, fra.PaymentDate);
		builder.put(ExplainKey.START_DATE, fra.StartDate);
		builder.put(ExplainKey.END_DATE, fra.EndDate);
		builder.put(ExplainKey.ACCRUAL_YEAR_FRACTION, fra.YearFraction);
		builder.put(ExplainKey.DAYS, (int) DAYS.between(fra.StartDate, fra.EndDate));
		builder.put(ExplainKey.PAYMENT_CURRENCY, currency);
		builder.put(ExplainKey.NOTIONAL, CurrencyAmount.of(currency, fra.Notional));
		builder.put(ExplainKey.TRADE_NOTIONAL, CurrencyAmount.of(currency, fra.Notional));
		if (fra.PaymentDate.isBefore(provider.ValuationDate))
		{
		  builder.put(ExplainKey.COMPLETED, true);
		  builder.put(ExplainKey.FORECAST_VALUE, CurrencyAmount.zero(currency));
		  builder.put(ExplainKey.PRESENT_VALUE, CurrencyAmount.zero(currency));
		}
		else
		{
		  double rate = rateComputationFn.explainRate(fra.FloatingRate, fra.StartDate, fra.EndDate, provider, builder);
		  builder.put(ExplainKey.FIXED_RATE, fra.FixedRate);
		  builder.put(ExplainKey.DISCOUNT_FACTOR, provider.discountFactor(currency, fra.PaymentDate));
		  builder.put(ExplainKey.PAY_OFF_RATE, rate);
		  builder.put(ExplainKey.UNIT_AMOUNT, unitAmount(fra, provider));
		  builder.put(ExplainKey.FORECAST_VALUE, forecastValue(fra, provider));
		  builder.put(ExplainKey.PRESENT_VALUE, presentValue(fra, provider));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  // calculates the forecast value
	  private double forecastValue0(ResolvedFra fra, RatesProvider provider)
	  {
		if (fra.PaymentDate.isBefore(provider.ValuationDate))
		{
		  return 0d;
		}
		// notional * unitAmount
		return fra.Notional * unitAmount(fra, provider);
	  }

	  // unit amount in various discounting methods
	  private double unitAmount(ResolvedFra fra, RatesProvider provider)
	  {
		switch (fra.Discounting)
		{
		  case NONE:
			return unitAmountNone(fra, provider);
		  case ISDA:
			return unitAmountIsda(fra, provider);
		  case AFMA:
			return unitAmountAfma(fra, provider);
		  default:
			throw new System.ArgumentException("Unknown FraDiscounting value: " + fra.Discounting);
		}
	  }

	  // NONE discounting method
	  private double unitAmountNone(ResolvedFra fra, RatesProvider provider)
	  {
		double fixedRate = fra.FixedRate;
		double forwardRate = this.forwardRate(fra, provider);
		double yearFraction = fra.YearFraction;
		return (forwardRate - fixedRate) * yearFraction;
	  }

	  // ISDA discounting method
	  private double unitAmountIsda(ResolvedFra fra, RatesProvider provider)
	  {
		double fixedRate = fra.FixedRate;
		double forwardRate = this.forwardRate(fra, provider);
		double yearFraction = fra.YearFraction;
		return ((forwardRate - fixedRate) / (1.0 + forwardRate * yearFraction)) * yearFraction;
	  }

	  // AFMA discounting method
	  private double unitAmountAfma(ResolvedFra fra, RatesProvider provider)
	  {
		double fixedRate = fra.FixedRate;
		double forwardRate = this.forwardRate(fra, provider);
		double yearFraction = fra.YearFraction;
		return (1.0 / (1.0 + fixedRate * yearFraction)) - (1.0 / (1.0 + forwardRate * yearFraction));
	  }

	  //-------------------------------------------------------------------------
	  // determine the derivative
	  private double derivative(ResolvedFra fra, RatesProvider provider)
	  {
		switch (fra.Discounting)
		{
		  case NONE:
			return derivativeNone(fra, provider);
		  case ISDA:
			return derivativeIsda(fra, provider);
		  case AFMA:
			return derivativeAfma(fra, provider);
		  default:
			throw new System.ArgumentException("Unknown FraDiscounting value: " + fra.Discounting);
		}
	  }

	  // NONE discounting method
	  private double derivativeNone(ResolvedFra fra, RatesProvider provider)
	  {
		return fra.YearFraction;
	  }

	  // ISDA discounting method
	  private double derivativeIsda(ResolvedFra fra, RatesProvider provider)
	  {
		double fixedRate = fra.FixedRate;
		double forwardRate = this.forwardRate(fra, provider);
		double yearFraction = fra.YearFraction;
		double dsc = 1.0 / (1.0 + forwardRate * yearFraction);
		return (1.0 + fixedRate * yearFraction) * yearFraction * dsc * dsc;
	  }

	  // AFMA discounting method
	  private double derivativeAfma(ResolvedFra fra, RatesProvider provider)
	  {
		double forwardRate = this.forwardRate(fra, provider);
		double yearFraction = fra.YearFraction;
		double dsc = 1.0 / (1.0 + forwardRate * yearFraction);
		return yearFraction * dsc * dsc;
	  }

	  //-------------------------------------------------------------------------
	  // query the forward rate
	  private double forwardRate(ResolvedFra fra, RatesProvider provider)
	  {
		return rateComputationFn.rate(fra.FloatingRate, fra.StartDate, fra.EndDate, provider);
	  }

	  // query the sensitivity
	  private PointSensitivityBuilder forwardRateSensitivity(ResolvedFra fra, RatesProvider provider)
	  {
		return rateComputationFn.rateSensitivity(fra.FloatingRate, fra.StartDate, fra.EndDate, provider);
	  }

	}

}