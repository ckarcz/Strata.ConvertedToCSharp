/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */

/// <summary>
/// Entity objects describing trades and products in financial markets.
/// <para>
/// The trade model has three basic concepts, trades, securities and products.
/// </para>
/// <para>
/// A <seealso cref="com.opengamma.strata.product.Trade Trade"/> is the basic element of finance,
/// a transaction between two organizations, known as counterparties.
/// Most trades represented in the system will be contracts that have been agreed on a date in the past.
/// The trade model also allows trades with a date in the future, or without any date.
/// </para>
/// <para>
/// A <seealso cref="com.opengamma.strata.product.Security Security"/> is a standard contract that is traded,
/// such as an equity share or futures contract. Securities are typically created once and shared
/// using an identifier, represented by a <seealso cref="com.opengamma.strata.product.SecurityId SecurityId"/>.
/// They are often referred to as <i>reference data</i>.
/// Securities may also be stored in a <seealso cref="com.opengamma.strata.product.Position Position"/> instead
/// of in a {@code Trade}.
/// </para>
/// <para>
/// A <seealso cref="com.opengamma.strata.product.Product Product"/> is the financial details of the trade or security.
/// A product typically contains enough information to be priced, such as the dates, holidays, indices,
/// currencies and amounts. There is an implementation of {@code Product} for each distinct type
/// of financial instrument.
/// </para>
/// <para>
/// Trades are typically classified as Over-The-Counter (OTC) and listed.
/// </para>
/// <para>
/// An OTC trade directly embeds the product it refers to.
/// As such, OTC trades implement <seealso cref="com.opengamma.strata.product.ProductTrade ProductTrade"/>.
/// </para>
/// <para>
/// For example, consider an OTC instrument such as an interest rate swap.
/// The object model consists of a <seealso cref="com.opengamma.strata.product.swap.SwapTrade SwapTrade"/>
/// that directly contains a <seealso cref="com.opengamma.strata.product.swap.Swap Swap"/>,
/// where {@code SwapTrade} implements {@code ProductTrade}.
/// </para>
/// <para>
/// The key to understanding the model is appreciating the separation of products from trades.
/// In many cases, it is possible to price the product without knowing any trade details.
/// This allows a product to be an underlying of another product, such as a swap within a swaption.
/// </para>
/// <para>
/// A listed trade can be defined in two ways.
/// </para>
/// <para>
/// The first approach is to use <seealso cref="com.opengamma.strata.product.SecurityTrade SecurityTrade"/>.
/// A {@code SecurityTrade} stores just the security identifier, quantity and trade price.
/// When the trade needs to be priced, the identifier can be resolved to a {@code Security} using
/// <seealso cref="com.opengamma.strata.basics.ReferenceData ReferenceData"/>.
/// The reference data could be backed by an in-memory store or a database.
/// </para>
/// <para>
/// The second approach is to use a more specific trade type, such as
/// <seealso cref="com.opengamma.strata.product.bond.BondFutureTrade BondFutureTrade"/>.
/// These types include the product details directly so that no reference data is needed.
/// As such, this approach avoids the need to use the Strata {@code Security} classes.
/// </para>
/// <para>
/// For example, consider a bond future.
/// In the first approach, the application would create a {@code SecurityTrade} using the identifier of the future.
/// The reference data would be populated, mapping the identifier to an instance of {@code BondFutureSecurity}
/// and additional identifiers for each of the underlying {@code FixedCouponBondSecurity} instances.
/// </para>
/// <para>
/// In the second approach, the trade would be defined using {@code BondFutureTrade}. In this case,
/// the trade directly holds the product model of the {@code BondFuture} and each underlying {@code FixedCouponBond}.
/// There is thus no need to populate the reference data with securities.
/// </para>
/// <para>
/// The key to understanding the model is appreciating the separation of products from trades and securities.
/// It is often possible to price either against the market or against a model.
/// Details for pricing against the market are held in the security.
/// Details for pricing against the model are held in the product.
/// </para>
/// </summary>
namespace com.opengamma.strata.product
{
}