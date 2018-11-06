/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using ResolvedBill = com.opengamma.strata.product.bond.ResolvedBill;

	/// <summary>
	/// Pricer for bill products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedBill"/>.
	/// </para>
	/// </summary>
	public class DiscountingBillProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingBillProductPricer DEFAULT = new DiscountingBillProductPricer();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value of the bill product.
	  /// <para>
	  /// The present value of the product is the value on the valuation date.
	  /// The result is expressed using the payment currency of the bill.
	  /// </para>
	  /// <para>
	  /// Coupon payments of the product are considered based on the valuation date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value of the bill product </returns>
	  public virtual CurrencyAmount presentValue(ResolvedBill bill, LegalEntityDiscountingProvider provider)
	  {
		if (provider.ValuationDate.isAfter(bill.Notional.Date))
		{
		  return CurrencyAmount.of(bill.Currency, 0.0d);
		}
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfMaturity = issuerDf.discountFactor(bill.Notional.Date);
		return bill.Notional.Value.multipliedBy(dfMaturity);
	  }

	  /// <summary>
	  /// Calculates the present value of a bill product with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value of the bill product </returns>
	  public virtual CurrencyAmount presentValueWithZSpread(ResolvedBill bill, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (provider.ValuationDate.isAfter(bill.Notional.Date))
		{
		  return CurrencyAmount.of(bill.Currency, 0.0d);
		}
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfMaturity = issuerDf.DiscountFactors.discountFactorWithSpread(bill.Notional.Date, zSpread, compoundedRateType, periodsPerYear);
		return bill.Notional.Value.multipliedBy(dfMaturity);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the present value sensitivity of the bill product.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivities presentValueSensitivity(ResolvedBill bill, LegalEntityDiscountingProvider provider)
	  {
		if (provider.ValuationDate.isAfter(bill.Notional.Date))
		{
		  return PointSensitivities.empty();
		}
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfEndBar = bill.Notional.Amount;
		PointSensitivityBuilder sensMaturity = issuerDf.zeroRatePointSensitivity(bill.Notional.Date).multipliedBy(dfEndBar);
		return sensMaturity.build();
	  }

	  /// <summary>
	  /// Calculates the present value sensitivity of the bill product with z-spread.
	  /// <para>
	  /// The present value sensitivity of the product is the sensitivity of the present value to
	  /// the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the product </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the present value curve sensitivity of the product </returns>
	  public virtual PointSensitivities presentValueSensitivityWithZSpread(ResolvedBill bill, LegalEntityDiscountingProvider provider, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		if (provider.ValuationDate.isAfter(bill.Notional.Date))
		{
		  return PointSensitivities.empty();
		}
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfEndBar = bill.Notional.Amount;
		ZeroRateSensitivity zeroSensMaturity = issuerDf.DiscountFactors.zeroRatePointSensitivityWithSpread(bill.Notional.Date, zSpread, compoundedRateType, periodsPerYear);
		IssuerCurveZeroRateSensitivity dscSensMaturity = IssuerCurveZeroRateSensitivity.of(zeroSensMaturity, issuerDf.LegalEntityGroup).multipliedBy(dfEndBar);

		return dscSensMaturity.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price for settlement at a given settlement date using curves.
	  /// </summary>
	  /// <param name="bill">  the bill </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the price </returns>
	  public virtual double priceFromCurves(ResolvedBill bill, LegalEntityDiscountingProvider provider, LocalDate settlementDate)
	  {
		ArgChecker.inOrderNotEqual(settlementDate, bill.Notional.Date, "settlementDate", "endDate");
		ArgChecker.inOrderOrEqual(provider.ValuationDate, settlementDate, "valuationDate", "settlementDate");
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfMaturity = issuerDf.discountFactor(bill.Notional.Date);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bill, provider);
		double dfRepoSettle = repoDf.discountFactor(settlementDate);
		return dfMaturity / dfRepoSettle;
	  }

	  /// <summary>
	  /// Calculates the price for settlement at a given settlement date using curves with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// The z-spread is applied only on the legal entity curve, not on the repo curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the bill </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the price </returns>
	  public virtual double priceFromCurvesWithZSpread(ResolvedBill bill, LegalEntityDiscountingProvider provider, LocalDate settlementDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		ArgChecker.inOrderNotEqual(settlementDate, bill.Notional.Date, "settlementDate", "endDate");
		ArgChecker.inOrderOrEqual(provider.ValuationDate, settlementDate, "valuationDate", "settlementDate");
		IssuerCurveDiscountFactors issuerDf = issuerCurveDf(bill, provider);
		double dfMaturity = issuerDf.DiscountFactors.discountFactorWithSpread(bill.Notional.Date, zSpread, compoundedRateType, periodsPerYear);
		RepoCurveDiscountFactors repoDf = repoCurveDf(bill, provider);
		double dfRepoSettle = repoDf.discountFactor(settlementDate);
		return dfMaturity / dfRepoSettle;
	  }

	  /// <summary>
	  /// Calculates the yield for settlement at a given settlement date using curves.
	  /// </summary>
	  /// <param name="bill">  the bill </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <returns> the yield </returns>
	  public virtual double yieldFromCurves(ResolvedBill bill, LegalEntityDiscountingProvider provider, LocalDate settlementDate)
	  {
		double price = priceFromCurves(bill, provider, settlementDate);
		return bill.yieldFromPrice(price, settlementDate);
	  }

	  /// <summary>
	  /// Calculates the yield for settlement at a given settlement date using curves with z-spread.
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or
	  /// periodic compounded rates of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// The z-spread is applied only on the legal entity curve, not on the repo curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="bill">  the bill </param>
	  /// <param name="provider">  the discounting provider </param>
	  /// <param name="settlementDate">  the settlement date </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodsPerYear">  the number of periods per year </param>
	  /// <returns> the yield </returns>
	  public virtual double yieldFromCurvesWithZSpread(ResolvedBill bill, LegalEntityDiscountingProvider provider, LocalDate settlementDate, double zSpread, CompoundedRateType compoundedRateType, int periodsPerYear)
	  {

		double price = priceFromCurvesWithZSpread(bill, provider, settlementDate, zSpread, compoundedRateType, periodsPerYear);
		return bill.yieldFromPrice(price, settlementDate);
	  }

	  //-------------------------------------------------------------------------
	  // extracts the repo curve discount factors for the bond
	  internal static RepoCurveDiscountFactors repoCurveDf(ResolvedBill bill, LegalEntityDiscountingProvider provider)
	  {
		return provider.repoCurveDiscountFactors(bill.SecurityId, bill.LegalEntityId, bill.Currency);
	  }

	  // extracts the issuer curve discount factors for the bond
	  internal static IssuerCurveDiscountFactors issuerCurveDf(ResolvedBill bill, LegalEntityDiscountingProvider provider)
	  {
		return provider.issuerCurveDiscountFactors(bill.LegalEntityId, bill.Currency);
	  }

	}

}