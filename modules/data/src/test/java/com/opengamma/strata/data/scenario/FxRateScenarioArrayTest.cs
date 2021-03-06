﻿/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateScenarioArrayTest
	public class FxRateScenarioArrayTest
	{

	  private const double TOLERANCE = 1e-10;

	  public virtual void getValues()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1.07, 1.08, 1.09));
		assertThat(rates.Pair).isEqualTo(CurrencyPair.of(Currency.EUR, Currency.USD));
		assertThat(rates.ScenarioCount).isEqualTo(3);
		assertThat(rates.get(0)).isEqualTo(FxRate.of(Currency.EUR, Currency.USD, 1.07));
		assertThat(rates.get(1)).isEqualTo(FxRate.of(Currency.EUR, Currency.USD, 1.08));
		assertThat(rates.get(2)).isEqualTo(FxRate.of(Currency.EUR, Currency.USD, 1.09));
		assertThrows(typeof(System.IndexOutOfRangeException), () => rates.get(3));
	  }

	  public virtual void fxRate()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(CurrencyPair.of(Currency.EUR, Currency.USD), DoubleArray.of(1.07, 1.08, 1.09));
		assertThat(rates.fxRate(Currency.EUR, Currency.USD, 0)).isEqualTo(1.07);
		assertThat(rates.fxRate(Currency.EUR, Currency.USD, 1)).isEqualTo(1.08);
		assertThat(rates.fxRate(Currency.EUR, Currency.USD, 2)).isEqualTo(1.09);

		assertThat(rates.fxRate(Currency.USD, Currency.EUR, 0)).isEqualTo(1 / 1.07);
		assertThat(rates.fxRate(Currency.USD, Currency.EUR, 1)).isEqualTo(1 / 1.08);
		assertThat(rates.fxRate(Currency.USD, Currency.EUR, 2)).isEqualTo(1 / 1.09);
	  }

	  public virtual void identicalCurrenciesHaveRateOfOne()
	  {
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.EUR, Currency.EUR, DoubleArray.of(1.07, 1.08, 1.09)), "Conversion rate between identical currencies must be one");
	  }

	  public virtual void unknownCurrencyPair()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1.07, 1.08, 1.09));
		assertThrowsIllegalArg(() => rates.fxRate(Currency.AED, Currency.ARS, 0));
	  }

	  public virtual void convert()
	  {
		FxRateScenarioArray eurGbp = FxRateScenarioArray.of(Currency.EUR, Currency.GBP, DoubleArray.of(0.76, 0.75));
		DoubleArray input = DoubleArray.of(1.11, 1.12);
		DoubleArray expected = DoubleArray.of(1.11 * 0.76, 1.12 * 0.75);
		DoubleArray converted = eurGbp.convert(input, Currency.EUR, Currency.GBP);
		for (int i = 0; i < converted.size(); i++)
		{
		  assertThat(converted.get(i)).isEqualTo(expected.get(i), offset(TOLERANCE));
		}
	  }

	  public virtual void convert_inverse()
	  {
		FxRateScenarioArray eurGbp = FxRateScenarioArray.of(Currency.EUR, Currency.GBP, DoubleArray.of(0.76, 0.75));
		DoubleArray input = DoubleArray.of(1.11, 1.12);
		DoubleArray expected = DoubleArray.of(1.11 * 1 / 0.76, 1.12 * 1 / 0.75);
		DoubleArray converted = eurGbp.convert(input, Currency.GBP, Currency.EUR);
		for (int i = 0; i < converted.size(); i++)
		{
		  assertThat(converted.get(i)).isEqualTo(expected.get(i), offset(TOLERANCE));
		}
	  }

	  public virtual void convert_unknown()
	  {
		FxRateScenarioArray eurGbp = FxRateScenarioArray.of(Currency.EUR, Currency.GBP, DoubleArray.of(0.76, 0.75));
		assertThrowsIllegalArg(() => eurGbp.convert(DoubleArray.of(1.07, 1.08), Currency.EUR, Currency.USD));
	  }

	  public virtual void crossRates()
	  {
		FxRateScenarioArray eurGbp = FxRateScenarioArray.of(Currency.EUR, Currency.GBP, DoubleArray.of(0.76, 0.75));
		FxRateScenarioArray eurUsd = FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1.11, 1.12));
		FxRateScenarioArray gbpEur = FxRateScenarioArray.of(Currency.GBP, Currency.EUR, DoubleArray.of(1 / 0.76, 1 / 0.75));
		FxRateScenarioArray usdEur = FxRateScenarioArray.of(Currency.USD, Currency.EUR, DoubleArray.of(1 / 1.11, 1 / 1.12));
		FxRateScenarioArray expectedGbpUsd = FxRateScenarioArray.of(Currency.GBP, Currency.USD, DoubleArray.of(1.460526315789474, 1.4933333333333334));

		assertArraysEqual(eurGbp.crossRates(eurUsd), expectedGbpUsd);
		assertArraysEqual(eurGbp.crossRates(usdEur), expectedGbpUsd);
		assertArraysEqual(gbpEur.crossRates(eurUsd), expectedGbpUsd);
		assertArraysEqual(gbpEur.crossRates(usdEur), expectedGbpUsd);

		assertArraysEqual(eurUsd.crossRates(eurGbp), expectedGbpUsd);
		assertArraysEqual(usdEur.crossRates(eurGbp), expectedGbpUsd);
		assertArraysEqual(eurUsd.crossRates(gbpEur), expectedGbpUsd);
		assertArraysEqual(usdEur.crossRates(gbpEur), expectedGbpUsd);
	  }

	  public virtual void crossRatesInvalidInputs()
	  {
		// Argument has both currencies the same
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.GBP, Currency.USD, DoubleArray.of(1)).crossRates(FxRateScenarioArray.of(Currency.EUR, Currency.EUR, DoubleArray.of(1))));

		// Receiver has both currencies the same
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.GBP, Currency.GBP, DoubleArray.of(1)).crossRates(FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1))));

		// No currency in common
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.GBP, Currency.CHF, DoubleArray.of(1)).crossRates(FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1))));

		// Both pairs the same
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.GBP, Currency.CHF, DoubleArray.of(1)).crossRates(FxRateScenarioArray.of(Currency.GBP, Currency.CHF, DoubleArray.of(1))));

		// Different length arrays
		assertThrowsIllegalArg(() => FxRateScenarioArray.of(Currency.GBP, Currency.CHF, DoubleArray.of(1)).crossRates(FxRateScenarioArray.of(Currency.EUR, Currency.CHF, DoubleArray.of(1, 2))));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxRateScenarioArray rates1 = FxRateScenarioArray.of(Currency.EUR, Currency.USD, DoubleArray.of(1.07, 1.08, 1.09));
		FxRateScenarioArray rates2 = FxRateScenarioArray.of(Currency.GBP, Currency.USD, DoubleArray.of(1.46, 1.47, 1.48));
		coverImmutableBean(rates1);
		coverBeanEquals(rates1, rates2);
	  }

	  //-------------------------------------------------------------------------
	  private static void assertArraysEqual(FxRateScenarioArray a1, FxRateScenarioArray a2)
	  {
		assertThat(a1.ScenarioCount).isEqualTo(a2.ScenarioCount);
		assertThat(a1.Pair).isEqualTo(a2.Pair);

		for (int i = 0; i < a1.ScenarioCount; i++)
		{
		  assertThat(a1.fxRate(Currency.GBP, Currency.USD, i)).isEqualTo(a2.fxRate(Currency.GBP, Currency.USD, i), offset(TOLERANCE));
		}
	  }
	}

}