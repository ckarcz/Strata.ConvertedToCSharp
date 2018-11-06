/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.payment
{
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using Payment = com.opengamma.strata.basics.currency.Payment;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using CashFlows = com.opengamma.strata.market.amount.CashFlows;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using ResolvedBulletPaymentTrade = com.opengamma.strata.product.payment.ResolvedBulletPaymentTrade;

	/// <summary>
	/// Pricer for for bullet payment trades.
	/// <para>
	/// This provides the ability to price <seealso cref="ResolvedBulletPaymentTrade"/>.
	/// The trade is priced by discounting the underlying payment.
	/// </para>
	/// </summary>
	public class DiscountingBulletPaymentTradePricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingBulletPaymentTradePricer DEFAULT = new DiscountingBulletPaymentTradePricer(DiscountingPaymentPricer.DEFAULT);

	  /// <summary>
	  /// Pricer for <seealso cref="Payment"/>.
	  /// </summary>
	  private readonly DiscountingPaymentPricer paymentPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="paymentPricer">  the pricer for <seealso cref="Payment"/> </param>
	  public DiscountingBulletPaymentTradePricer(DiscountingPaymentPricer paymentPricer)
	  {
		this.paymentPricer = ArgChecker.notNull(paymentPricer, "paymentPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying payment pricer.
	  /// </summary>
	  /// <returns> the payment pricer </returns>
	  public virtual DiscountingPaymentPricer PaymentPricer
	  {
		  get
		  {
			return paymentPricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bullet payment trade.
	  /// <para>
	  /// The present value of the trade is the value on the valuation date.
	  /// This is the discounted forecast value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the present value of the trade </returns>
	  public virtual CurrencyAmount presentValue(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.presentValue(trade.Product.Payment, provider);
	  }

	  /// <summary>
	  /// Explains the present value of the bullet payment product.
	  /// <para>
	  /// This returns explanatory information about the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the explanatory information </returns>
	  public virtual ExplainMap explainPresentValue(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.explainPresentValue(trade.Product.Payment, provider);
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the bullet payment trade.
	  /// <para>
	  /// The present value sensitivity of the trade is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the point sensitivity of the present value </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.presentValueSensitivity(trade.Product.Payment, provider).build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future cash flow of the bullet payment trade.
	  /// <para>
	  /// There is only one cash flow on the payment date for the bullet payment trade.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the cash flows </returns>
	  public virtual CashFlows cashFlows(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.cashFlows(trade.Product.Payment, provider);
	  }

	  /// <summary>
	  /// Calculates the currency exposure of the bullet payment trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the currency exposure </returns>
	  public virtual CurrencyAmount currencyExposure(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.presentValue(trade.Product.Payment, provider);
	  }

	  /// <summary>
	  /// Calculates the current cash of the bullet payment trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="provider">  the provider </param>
	  /// <returns> the current cash </returns>
	  public virtual CurrencyAmount currentCash(ResolvedBulletPaymentTrade trade, BaseProvider provider)
	  {
		return paymentPricer.currentCash(trade.Product.Payment, provider);
	  }

	}

}