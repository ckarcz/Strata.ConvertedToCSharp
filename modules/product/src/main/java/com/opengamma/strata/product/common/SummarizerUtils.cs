using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{


	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// Utilities to support summarizing portfolio items.
	/// <para>
	/// This class provides a central place for description logic.
	/// </para>
	/// </summary>
	public sealed class SummarizerUtils
	{

	  /// <summary>
	  /// Date format. </summary>
	  private static readonly DateTimeFormatter DATE_FORMAT = DateTimeFormatter.ofPattern("dMMMuu", Locale.UK);

	  // restricted constructor
	  private SummarizerUtils()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a date to a string.
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <returns> the string form </returns>
	  public static string date(LocalDate date)
	  {
		return date.format(DATE_FORMAT);
	  }

	  /// <summary>
	  /// Converts a date range to a string.
	  /// </summary>
	  /// <param name="start">  the start date </param>
	  /// <param name="end">  the end date </param>
	  /// <returns> the string form </returns>
	  public static string dateRange(LocalDate start, LocalDate end)
	  {
		return date(start) + "-" + date(end);
	  }

	  /// <summary>
	  /// Converts a date range to a period string.
	  /// </summary>
	  /// <param name="start">  the start date </param>
	  /// <param name="end">  the end date </param>
	  /// <returns> the string form </returns>
	  public static string datePeriod(LocalDate start, LocalDate end)
	  {
		int months = Math.toIntExact(MONTHS.between(start, end.plusDays(3)));
		if (months > 0)
		{
		  return Tenor.of(Period.ofMonths((int) months)).normalized().ToString();
		}
		else
		{
		  return Tenor.of(Period.ofDays((int) start.until(end, ChronoUnit.DAYS))).ToString();
		}
	  }

	  /// <summary>
	  /// Converts an amount to a string.
	  /// </summary>
	  /// <param name="currencyAmount">  the amount </param>
	  /// <returns> the string form </returns>
	  public static string amount(CurrencyAmount currencyAmount)
	  {
		return amount(currencyAmount.Currency, currencyAmount.Amount);
	  }

	  /// <summary>
	  /// Converts an amount to a string.
	  /// </summary>
	  /// <param name="currency">  the currency </param>
	  /// <param name="value">  the value </param>
	  /// <returns> the string form </returns>
	  public static string amount(Currency currency, double value)
	  {
		decimal dec = decimal.valueOf(value).stripTrailingZeros();
		string symbol = currency.Code + " ";
		if (dec.scale() <= -3)
		{
		  if (Math.Abs(dec.longValue()) >= 1_000_000L)
		  {
			dec = dec.movePointLeft(6);
			return symbol + dec.toPlainString() + "mm";
		  }
		  else
		  {
			dec = dec.movePointLeft(3);
			return symbol + dec.toPlainString() + "k";
		  }
		}
		if (dec.scale() > currency.MinorUnitDigits)
		{
		  dec = dec.setScale(currency.MinorUnitDigits, RoundingMode.HALF_UP);
		}
		DecimalFormat formatter = new DecimalFormat("###,###.###", new DecimalFormatSymbols(Locale.UK));
		return symbol + formatter.format(dec);
	  }

	  /// <summary>
	  /// Converts a value to a string.
	  /// </summary>
	  /// <param name="value">  the value </param>
	  /// <returns> the string form </returns>
	  public static string value(double value)
	  {
		decimal dec = decimal.valueOf(value).stripTrailingZeros();
		if (dec.scale() > 6)
		{
		  dec = dec.setScale(6, RoundingMode.HALF_UP);
		}
		return dec.toPlainString();
	  }

	  /// <summary>
	  /// Converts a value to a percentage string.
	  /// </summary>
	  /// <param name="value">  the value </param>
	  /// <returns> the string form </returns>
	  public static string percent(double value)
	  {
		decimal dec = decimal.valueOf(value);
		dec = dec * decimal.valueOf(100).stripTrailingZeros();
		if (dec.scale() > 4)
		{
		  dec = dec.setScale(4, RoundingMode.HALF_UP);
		}
		return dec.toPlainString() + "%";
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts pay/receive to a string.
	  /// </summary>
	  /// <param name="payReceive">  the value </param>
	  /// <returns> the string form </returns>
	  public static string payReceive(PayReceive payReceive)
	  {
		return payReceive.ToString().Substring(0, 3);
	  }

	  /// <summary>
	  /// Converts an FX exchange to a string.
	  /// </summary>
	  /// <param name="base">  the base currency amount </param>
	  /// <param name="counter">  the counter currency amount </param>
	  /// <returns> the string form </returns>
	  public static string fx(CurrencyAmount @base, CurrencyAmount counter)
	  {
		decimal rateDec = decimal.valueOf(counter.Amount / @base.Amount).setScale(@base.Currency.MinorUnitDigits + 2, RoundingMode.HALF_UP).abs();
		FxRate rate = FxRate.of(@base.Currency, counter.Currency, rateDec.doubleValue());
		decimal baseDec = decimal.valueOf(@base.Amount).stripTrailingZeros();
		decimal counterDec = decimal.valueOf(counter.Amount).stripTrailingZeros();
		bool roundBase = baseDec.scale() < counterDec.scale();
		CurrencyAmount round = roundBase ? @base : counter;
		return (round.Amount < 0 ? "Pay " : "Rec ") + SummarizerUtils.amount(round.mapAmount(a => Math.Abs(a))) + " " + "@ " + rate;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a summary instance for a position.
	  /// </summary>
	  /// <param name="position">  the position </param>
	  /// <param name="type">  the type </param>
	  /// <param name="description">  the description </param>
	  /// <param name="currencies">  the currencies, may be empty </param>
	  /// <returns> the string form </returns>
	  public static PortfolioItemSummary summary(Position position, ProductType type, string description, params Currency[] currencies)
	  {
		return PortfolioItemSummary.of(position.Id.orElse(null), PortfolioItemType.POSITION, type, ImmutableSet.copyOf(currencies), description);
	  }

	  /// <summary>
	  /// Creates a summary instance for a trade.
	  /// </summary>
	  /// <param name="trade">  the trade </param>
	  /// <param name="type">  the type </param>
	  /// <param name="description">  the description </param>
	  /// <param name="currencies">  the currencies, may be empty </param>
	  /// <returns> the string form </returns>
	  public static PortfolioItemSummary summary(Trade trade, ProductType type, string description, params Currency[] currencies)
	  {
		return PortfolioItemSummary.of(trade.Id.orElse(null), PortfolioItemType.TRADE, type, ImmutableSet.copyOf(currencies), description);
	  }

	}

}