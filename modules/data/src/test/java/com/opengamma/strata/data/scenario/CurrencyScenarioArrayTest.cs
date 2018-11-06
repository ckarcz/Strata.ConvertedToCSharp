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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
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
	/// Test <seealso cref="CurrencyScenarioArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyScenarioArrayTest
	public class CurrencyScenarioArrayTest
	{

	  public virtual void create()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Amounts.Values).isEqualTo(values);
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void create_fromList()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
		CurrencyScenarioArray test = CurrencyScenarioArray.of(values);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Amounts.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void create_fromList_mixedCurrency()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(USD, 2), CurrencyAmount.of(GBP, 3));
		assertThrowsIllegalArg(() => CurrencyScenarioArray.of(values));
	  }

	  public virtual void create_fromFunction()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
		CurrencyScenarioArray test = CurrencyScenarioArray.of(3, i => values[i]);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Amounts.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.ScenarioCount).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void create_fromFunction_mixedCurrency()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(USD, 2), CurrencyAmount.of(GBP, 3));
		assertThrowsIllegalArg(() => CurrencyScenarioArray.of(3, i => values[i]));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Test that values are converted to the reporting currency using the rates in the market data.
	  /// </summary>
	  public virtual void convert()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);

		CurrencyScenarioArray convertedList = test.convertedTo(USD, fxProvider);
		DoubleArray expectedValues = DoubleArray.of(1 * 1.61, 2 * 1.62, 3 * 1.63);
		CurrencyScenarioArray expectedList = CurrencyScenarioArray.of(USD, expectedValues);
		assertThat(convertedList).isEqualTo(expectedList);
	  }

	  /// <summary>
	  /// Test that no conversion is done and no rates are used if the values are already in the reporting currency.
	  /// </summary>
	  public virtual void noConversionNecessary()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);

		CurrencyScenarioArray convertedList = test.convertedTo(GBP, fxProvider);
		assertThat(convertedList).isEqualTo(test);
	  }

	  /// <summary>
	  /// Test the expected exception is thrown when there are no FX rates available to convert the values.
	  /// </summary>
	  public virtual void missingFxRates()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		FxRateScenarioArray rates = FxRateScenarioArray.of(EUR, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);

		assertThrows(() => test.convertedTo(USD, fxProvider), typeof(System.ArgumentException));
	  }

	  /// <summary>
	  /// Test the expected exception is thrown if there are not the same number of rates as there are values.
	  /// </summary>
	  public virtual void wrongNumberOfFxRates()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);

		assertThrows(() => test.convertedTo(USD, fxProvider), typeof(System.ArgumentException), "Expected 3 FX rates but received 2");
	  }

	  /// <summary>
	  /// Test the plus() methods work as expected.
	  /// </summary>
	  public virtual void plus()
	  {
		CurrencyScenarioArray currencyScenarioArray = CurrencyScenarioArray.of(GBP, DoubleArray.of(1, 2, 3));

		CurrencyScenarioArray arrayToAdd = CurrencyScenarioArray.of(GBP, DoubleArray.of(4, 5, 6));
		CurrencyScenarioArray plusArraysResult = currencyScenarioArray.plus(arrayToAdd);
		assertThat(plusArraysResult).isEqualTo(CurrencyScenarioArray.of(GBP, DoubleArray.of(5, 7, 9)));

		CurrencyAmount amountToAdd = CurrencyAmount.of(Currency.GBP, 10);
		CurrencyScenarioArray plusAmountResult = currencyScenarioArray.plus(amountToAdd);
		assertThat(plusAmountResult).isEqualTo(CurrencyScenarioArray.of(GBP, DoubleArray.of(11, 12, 13)));
	  }

	  /// <summary>
	  /// Test the minus() methods work as expected.
	  /// </summary>
	  public virtual void minus()
	  {
		CurrencyScenarioArray currencyScenarioArray = CurrencyScenarioArray.of(GBP, DoubleArray.of(1, 2, 3));

		CurrencyScenarioArray arrayToSubtract = CurrencyScenarioArray.of(GBP, DoubleArray.of(3, 2, 1));
		CurrencyScenarioArray minusArrayResult = currencyScenarioArray.minus(arrayToSubtract);
		assertThat(minusArrayResult).isEqualTo(CurrencyScenarioArray.of(GBP, DoubleArray.of(-2, 0, 2)));

		CurrencyAmount amountToSubtract = CurrencyAmount.of(Currency.GBP, 2);
		CurrencyScenarioArray minusAmountResult = currencyScenarioArray.minus(amountToSubtract);
		assertThat(minusAmountResult).isEqualTo(CurrencyScenarioArray.of(GBP, DoubleArray.of(-1, 0, 1)));
	  }


	  public virtual void coverage()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyScenarioArray test = CurrencyScenarioArray.of(GBP, values);
		coverImmutableBean(test);
		DoubleArray values2 = DoubleArray.of(1, 2, 3);
		CurrencyScenarioArray test2 = CurrencyScenarioArray.of(USD, values2);
		coverBeanEquals(test, test2);
	  }

	}

}