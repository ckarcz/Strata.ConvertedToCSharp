/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{
	using ImmutableList = com.google.common.collect.ImmutableList;
	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;
	using FixedCouponBond = com.opengamma.strata.product.bond.FixedCouponBond;
	using ResolvedBondFuture = com.opengamma.strata.product.bond.ResolvedBondFuture;
	using ResolvedFixedCouponBond = com.opengamma.strata.product.bond.ResolvedFixedCouponBond;

	/// <summary>
	/// Pricer for for bond future products.
	/// <para>
	/// This function provides the ability to price a <seealso cref="ResolvedBondFuture"/>.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="FixedCouponBond"/>. The bond futures delivery is a bond
	/// for an amount computed from the bond future price, a conversion factor and the accrued interest.
	/// </para>
	/// </summary>
	public sealed class DiscountingBondFutureProductPricer
	{

	  /// <summary>
	  /// Default implementation.
	  /// </summary>
	  public static readonly DiscountingBondFutureProductPricer DEFAULT = new DiscountingBondFutureProductPricer(DiscountingFixedCouponBondProductPricer.DEFAULT);

	  /// <summary>
	  /// Underlying pricer.
	  /// </summary>
	  private readonly DiscountingFixedCouponBondProductPricer bondPricer;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="bondPricer">  the pricer for <seealso cref="ResolvedFixedCouponBond"/>. </param>
	  public DiscountingBondFutureProductPricer(DiscountingFixedCouponBondProductPricer bondPricer)
	  {
		this.bondPricer = ArgChecker.notNull(bondPricer, "bondPricer");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the number related to bond futures product on which the daily margin is computed.
	  /// <para>
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="price">  the price of the product, in decimal form </param>
	  /// <returns> the index </returns>
	  internal double marginIndex(ResolvedBondFuture future, double price)
	  {
		return price * future.Notional;
	  }

	  /// <summary>
	  /// Calculates the margin index sensitivity of the bond future product.
	  /// <para>
	  /// The margin index sensitivity is the sensitivity of the margin index to the underlying curves.
	  /// For two consecutive settlement prices C1 and C2, the daily margin is computed as 
	  ///    {@code (marginIndex(future, C2) - marginIndex(future, C1))}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="priceSensitivity">  the price sensitivity of the product </param>
	  /// <returns> the index sensitivity </returns>
	  internal PointSensitivities marginIndexSensitivity(ResolvedBondFuture future, PointSensitivities priceSensitivity)
	  {
		return priceSensitivity.multipliedBy(future.Notional);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price of the bond future product.
	  /// <para>
	  /// The price of the product is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bond futures. This is coherent with the pricing of <seealso cref="FixedCouponBond"/>.
	  /// For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double price(ResolvedBondFuture future, LegalEntityDiscountingProvider discountingProvider)
	  {
		ImmutableList<ResolvedFixedCouponBond> basket = future.DeliveryBasket;
		int size = basket.size();
		double[] priceBonds = new double[size];
		for (int i = 0; i < size; ++i)
		{
		  ResolvedFixedCouponBond bond = basket.get(i);
		  double dirtyPrice = bondPricer.dirtyPriceFromCurves(bond, discountingProvider, future.LastDeliveryDate);
		  priceBonds[i] = bondPricer.cleanPriceFromDirtyPrice(bond, future.LastDeliveryDate, dirtyPrice) / future.ConversionFactors.get(i);
		}
		return Doubles.min(priceBonds);
	  }

	  /// <summary>
	  /// Calculates the price of the bond future product with z-spread.
	  /// <para>
	  /// The price of the product is the price on the valuation date.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// Strata uses <i>decimal prices</i> for bond futures. This is coherent with the pricing of <seealso cref="FixedCouponBond"/>.
	  /// For example, a price of 99.32% is represented in Strata by 0.9932.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the price of the product, in decimal form </returns>
	  public double priceWithZSpread(ResolvedBondFuture future, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		ImmutableList<ResolvedFixedCouponBond> basket = future.DeliveryBasket;
		int size = basket.size();
		double[] priceBonds = new double[size];
		for (int i = 0; i < size; ++i)
		{
		  ResolvedFixedCouponBond bond = basket.get(i);
		  double dirtyPrice = bondPricer.dirtyPriceFromCurvesWithZSpread(bond, discountingProvider, zSpread, compoundedRateType, periodPerYear, future.LastDeliveryDate);
		  priceBonds[i] = bondPricer.cleanPriceFromDirtyPrice(bond, future.LastDeliveryDate, dirtyPrice) / future.ConversionFactors.get(i);
		}
		return Doubles.min(priceBonds);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the price sensitivity of the bond future product.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// </para>
	  /// <para>
	  /// Note that the price sensitivity should be no currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public PointSensitivities priceSensitivity(ResolvedBondFuture future, LegalEntityDiscountingProvider discountingProvider)
	  {
		ImmutableList<ResolvedFixedCouponBond> basket = future.DeliveryBasket;
		int size = basket.size();
		double[] priceBonds = new double[size];
		int indexCTD = 0;
		double priceMin = 2d;
		for (int i = 0; i < size; i++)
		{
		  ResolvedFixedCouponBond bond = basket.get(i);
		  double dirtyPrice = bondPricer.dirtyPriceFromCurves(bond, discountingProvider, future.LastDeliveryDate);
		  priceBonds[i] = bondPricer.cleanPriceFromDirtyPrice(bond, future.LastDeliveryDate, dirtyPrice) / future.ConversionFactors.get(i);
		  if (priceBonds[i] < priceMin)
		  {
			priceMin = priceBonds[i];
			indexCTD = i;
		  }
		}
		ResolvedFixedCouponBond bond = basket.get(indexCTD);
		PointSensitivityBuilder pointSensi = bondPricer.dirtyPriceSensitivity(bond, discountingProvider, future.LastDeliveryDate);
		return pointSensi.multipliedBy(1d / future.ConversionFactors.get(indexCTD)).build();
	  }

	  /// <summary>
	  /// Calculates the price sensitivity of the bond future product with z-spread.
	  /// <para>
	  /// The price sensitivity of the product is the sensitivity of the price to the underlying curves.
	  /// </para>
	  /// <para>
	  /// The z-spread is a parallel shift applied to continuously compounded rates or periodic compounded rates 
	  /// of the issuer discounting curve.
	  /// </para>
	  /// <para>
	  /// Note that the price sensitivity should be no currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="future">  the future </param>
	  /// <param name="discountingProvider">  the discounting provider </param>
	  /// <param name="zSpread">  the z-spread </param>
	  /// <param name="compoundedRateType">  the compounded rate type </param>
	  /// <param name="periodPerYear">  the number of periods per year </param>
	  /// <returns> the price curve sensitivity of the product </returns>
	  public PointSensitivities priceSensitivityWithZSpread(ResolvedBondFuture future, LegalEntityDiscountingProvider discountingProvider, double zSpread, CompoundedRateType compoundedRateType, int periodPerYear)
	  {

		ImmutableList<ResolvedFixedCouponBond> basket = future.DeliveryBasket;
		int size = basket.size();
		double[] priceBonds = new double[size];
		int indexCTD = 0;
		double priceMin = 2d;
		for (int i = 0; i < size; i++)
		{
		  ResolvedFixedCouponBond bond = basket.get(i);
		  double dirtyPrice = bondPricer.dirtyPriceFromCurvesWithZSpread(bond, discountingProvider, zSpread, compoundedRateType, periodPerYear, future.LastDeliveryDate);
		  priceBonds[i] = bondPricer.cleanPriceFromDirtyPrice(bond, future.LastDeliveryDate, dirtyPrice) / future.ConversionFactors.get(i);
		  if (priceBonds[i] < priceMin)
		  {
			priceMin = priceBonds[i];
			indexCTD = i;
		  }
		}
		ResolvedFixedCouponBond bond = basket.get(indexCTD);
		PointSensitivityBuilder pointSensi = bondPricer.dirtyPriceSensitivityWithZspread(bond, discountingProvider, zSpread, compoundedRateType, periodPerYear, future.LastDeliveryDate);
		return pointSensi.multipliedBy(1d / future.ConversionFactors.get(indexCTD)).build();
	  }

	}

}