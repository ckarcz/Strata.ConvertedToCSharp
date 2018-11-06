/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */

/// <summary>
/// Representations of currency and money.
/// <para>
/// The representation of <seealso cref="com.opengamma.strata.basics.currency.Currency Currency"/> is
/// separate from that in the JDK to provide more control. A pair of currencies is
/// represented by <seealso cref="com.opengamma.strata.basics.currency.CurrencyPair CurrencyPair"/>,
/// which provides a mechanism of determining whether the pair is in standard FX market order.
/// </para>
/// <para>
/// <seealso cref="com.opengamma.strata.basics.currency.CurrencyAmount CurrencyAmount"/> provides
/// the primary monetary representation, while
/// <seealso cref="com.opengamma.strata.basics.currency.MultiCurrencyAmount MultiCurrencyAmount"/>
/// provide a representation where the amount is in multiple currencies.
/// </para>
/// <para>
/// Basic support for FX conversions is also provided. A single FX rate can be represented
/// using <seealso cref="com.opengamma.strata.basics.currency.FxRate FxRate"/>, while a matrix of
/// FX rates is represented using <seealso cref="com.opengamma.strata.basics.currency.FxMatrix FxMatrix"/>.
/// The <seealso cref="com.opengamma.strata.basics.currency.FxConvertible FxConvertible"/> and
/// <seealso cref="com.opengamma.strata.basics.currency.FxRateProvider FxRateProvider"/> interfaces
/// provide the glue to make currency conversion easy.
/// </para>
/// </summary>
namespace com.opengamma.strata.basics.currency
{
}