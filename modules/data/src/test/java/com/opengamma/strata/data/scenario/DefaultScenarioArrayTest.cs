using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="DefaultScenarioArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultScenarioArrayTest
	public class DefaultScenarioArrayTest
	{

	  public virtual void create()
	  {
		DefaultScenarioArray<int> test = DefaultScenarioArray.of(1, 2, 3);
		assertThat(test.Values).isEqualTo(ImmutableList.of(1, 2, 3));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(1);
		assertThat(test.get(1)).isEqualTo(2);
		assertThat(test.get(2)).isEqualTo(3);
		assertThat(test.ToList()).isEqualTo(ImmutableList.of(1, 2, 3));
	  }

	  public virtual void create_withFunction()
	  {
		DefaultScenarioArray<int> test = DefaultScenarioArray.of(3, i => (i + 1));
		assertThat(test.Values).isEqualTo(ImmutableList.of(1, 2, 3));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(1);
		assertThat(test.get(1)).isEqualTo(2);
		assertThat(test.get(2)).isEqualTo(3);
		assertThat(test.ToList()).isEqualTo(ImmutableList.of(1, 2, 3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void convertCurrencyAmount()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);

		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(Currency.GBP, 1), CurrencyAmount.of(Currency.GBP, 2), CurrencyAmount.of(Currency.GBP, 3));
		DefaultScenarioArray<CurrencyAmount> test = DefaultScenarioArray.of(values);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioArray<?> convertedList = test.convertedTo(com.opengamma.strata.basics.currency.Currency.USD, fxProvider);
		ScenarioArray<object> convertedList = test.convertedTo(Currency.USD, fxProvider);
		IList<CurrencyAmount> expectedValues = ImmutableList.of(CurrencyAmount.of(Currency.USD, 1 * 1.61), CurrencyAmount.of(Currency.USD, 2 * 1.62), CurrencyAmount.of(Currency.USD, 3 * 1.63));
		DefaultScenarioArray<CurrencyAmount> expectedList = DefaultScenarioArray.of(expectedValues);
		assertThat(convertedList).isEqualTo(expectedList);
	  }

	  public virtual void noConversionNecessary()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);

		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(Currency.GBP, 1), CurrencyAmount.of(Currency.GBP, 2), CurrencyAmount.of(Currency.GBP, 3));
		DefaultScenarioArray<CurrencyAmount> test = DefaultScenarioArray.of(values);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioArray<?> convertedList = test.convertedTo(com.opengamma.strata.basics.currency.Currency.GBP, fxProvider);
		ScenarioArray<object> convertedList = test.convertedTo(Currency.GBP, fxProvider);
		ScenarioArray<CurrencyAmount> expectedList = DefaultScenarioArray.of(values);
		assertThat(convertedList).isEqualTo(expectedList);
	  }

	  public virtual void notConvertible()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);

		IList<string> values = ImmutableList.of("a", "b", "c");
		DefaultScenarioArray<string> test = DefaultScenarioArray.of(values);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioArray<?> convertedList = test.convertedTo(com.opengamma.strata.basics.currency.Currency.GBP, fxProvider);
		ScenarioArray<object> convertedList = test.convertedTo(Currency.GBP, fxProvider);
		assertThat(convertedList).isEqualTo(test);
	  }

	  public virtual void missingFxRates()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(EUR, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);

		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(Currency.GBP, 1), CurrencyAmount.of(Currency.GBP, 2), CurrencyAmount.of(Currency.GBP, 3));
		DefaultScenarioArray<CurrencyAmount> test = DefaultScenarioArray.of(values);

		assertThrows(() => test.convertedTo(Currency.USD, fxProvider), typeof(System.ArgumentException));
	  }

	  public virtual void wrongNumberOfFxRates()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);

		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(Currency.GBP, 1), CurrencyAmount.of(Currency.GBP, 2));
		DefaultScenarioArray<CurrencyAmount> test = DefaultScenarioArray.of(values);

		assertThrows(() => test.convertedTo(Currency.USD, fxProvider), typeof(System.ArgumentException), "Expected 2 FX rates but received 3");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DefaultScenarioArray<int> test = DefaultScenarioArray.of(1, 2, 3);
		coverImmutableBean(test);
		DefaultScenarioArray<string> test2 = DefaultScenarioArray.of("2", "3");
		coverBeanEquals(test, test2);
	  }

	}

}