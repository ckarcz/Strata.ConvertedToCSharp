using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fx
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateConfigTest
	public class FxRateConfigTest
	{

	  private static readonly QuoteId QUOTE_KEY = QuoteId.of(StandardId.of("test", "EUR/USD"));
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(Currency.EUR, Currency.USD);

	  public virtual void containsPair()
	  {
		assertThat(config().getObservableRateKey(CURRENCY_PAIR)).hasValue(QUOTE_KEY);
	  }

	  public virtual void containsInversePair()
	  {
		assertThat(config().getObservableRateKey(CURRENCY_PAIR.inverse())).hasValue(QUOTE_KEY);
	  }

	  public virtual void missingPair()
	  {
		assertThat(config().getObservableRateKey(CurrencyPair.of(Currency.GBP, Currency.USD))).Empty;
	  }

	  public virtual void nonConventionPair()
	  {
		IDictionary<CurrencyPair, QuoteId> ratesMap = ImmutableMap.of(CurrencyPair.of(Currency.USD, Currency.EUR), QUOTE_KEY);
		string regex = "Currency pairs must be quoted using market conventions but USD/EUR is not";
		assertThrowsIllegalArg(() => FxRateConfig.builder().observableRates(ratesMap).build(), regex);
		assertThrowsIllegalArg(() => FxRateConfig.of(ratesMap), regex);
	  }

	  private static FxRateConfig config()
	  {
		IDictionary<CurrencyPair, QuoteId> ratesMap = ImmutableMap.of(CURRENCY_PAIR, QUOTE_KEY);
		return FxRateConfig.of(ratesMap);
	  }
	}

}