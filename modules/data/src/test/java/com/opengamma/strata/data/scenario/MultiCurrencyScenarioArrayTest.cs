using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CAD;
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
//	import static com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray.toMultiCurrencyScenarioArray;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.data.Offset.offset;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MultiCurrencyScenarioArrayTest
	public class MultiCurrencyScenarioArrayTest
	{

	  private static readonly MultiCurrencyScenarioArray VALUES_ARRAY = MultiCurrencyScenarioArray.of(ImmutableList.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 20), CurrencyAmount.of(Currency.USD, 30), CurrencyAmount.of(Currency.EUR, 40)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 32), CurrencyAmount.of(Currency.EUR, 43)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44))));

	  public virtual void createAndGetValues()
	  {
		assertThat(VALUES_ARRAY.getValues(Currency.GBP)).isEqualTo(DoubleArray.of(20, 21, 22));
		assertThat(VALUES_ARRAY.getValues(Currency.USD)).isEqualTo(DoubleArray.of(30, 32, 33));
		assertThat(VALUES_ARRAY.getValues(Currency.EUR)).isEqualTo(DoubleArray.of(40, 43, 44));

		MultiCurrencyScenarioArray raggedArray = MultiCurrencyScenarioArray.of(ImmutableList.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.EUR, 4)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 32), CurrencyAmount.of(Currency.EUR, 43)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.EUR, 44))));

		assertThat(raggedArray.ScenarioCount).isEqualTo(3);
		assertThat(raggedArray.getValues(Currency.GBP)).isEqualTo(DoubleArray.of(0, 21, 0));
		assertThat(raggedArray.getValues(Currency.USD)).isEqualTo(DoubleArray.of(0, 32, 0));
		assertThat(raggedArray.getValues(Currency.EUR)).isEqualTo(DoubleArray.of(4, 43, 44));
		assertThrowsIllegalArg(() => raggedArray.getValues(Currency.AUD));
	  }

	  public virtual void emptyAmounts()
	  {
		MultiCurrencyScenarioArray array = MultiCurrencyScenarioArray.of(MultiCurrencyAmount.empty(), MultiCurrencyAmount.empty());
		assertThat(array.ScenarioCount).isEqualTo(2);
		assertThat(array.get(0)).isEqualTo(MultiCurrencyAmount.empty());
		assertThat(array.get(1)).isEqualTo(MultiCurrencyAmount.empty());
	  }

	  public virtual void createByFunction()
	  {
		MultiCurrencyAmount mca1 = MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 10), CurrencyAmount.of(Currency.USD, 20));
		MultiCurrencyAmount mca2 = MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 10), CurrencyAmount.of(Currency.EUR, 30));
		MultiCurrencyAmount mca3 = MultiCurrencyAmount.of(CurrencyAmount.of(Currency.USD, 40));
		IList<MultiCurrencyAmount> amounts = ImmutableList.of(mca1, mca2, mca3);

		MultiCurrencyScenarioArray test = MultiCurrencyScenarioArray.of(3, i => amounts[i]);
		assertThat(test.get(0)).isEqualTo(mca1.plus(Currency.EUR, 0));
		assertThat(test.get(1)).isEqualTo(mca2.plus(Currency.USD, 0));
		assertThat(test.get(2)).isEqualTo(mca3.plus(Currency.GBP, 0).plus(Currency.EUR, 0));
	  }

	  public virtual void createByFunctionEmptyAmounts()
	  {
		MultiCurrencyScenarioArray test = MultiCurrencyScenarioArray.of(3, i => MultiCurrencyAmount.empty());
		assertThat(test.ScenarioCount).isEqualTo(3);
	  }

	  public virtual void mapFactoryMethod()
	  {
		MultiCurrencyScenarioArray array = MultiCurrencyScenarioArray.of(ImmutableMap.of(Currency.GBP, DoubleArray.of(20, 21, 22), Currency.USD, DoubleArray.of(30, 32, 33), Currency.EUR, DoubleArray.of(40, 43, 44)));

		assertThat(array).isEqualTo(VALUES_ARRAY);

		assertThrowsIllegalArg(() => MultiCurrencyScenarioArray.of(ImmutableMap.of(Currency.GBP, DoubleArray.of(20, 21), Currency.EUR, DoubleArray.of(40, 43, 44))), "Arrays must have the same size.*");
	  }

	  public virtual void getAllAmountsUnsafe()
	  {
		IDictionary<Currency, DoubleArray> expected = ImmutableMap.of(Currency.GBP, DoubleArray.of(20, 21, 22), Currency.USD, DoubleArray.of(30, 32, 33), Currency.EUR, DoubleArray.of(40, 43, 44));
		assertThat(VALUES_ARRAY.Amounts.Values).isEqualTo(expected);
	  }

	  public virtual void get()
	  {
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44));
		assertThat(VALUES_ARRAY.get(2)).isEqualTo(expected);
		assertThrows(() => VALUES_ARRAY.get(3), typeof(System.IndexOutOfRangeException));
		assertThrows(() => VALUES_ARRAY.get(-1), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void stream()
	  {
		IList<MultiCurrencyAmount> expected = ImmutableList.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 20), CurrencyAmount.of(Currency.USD, 30), CurrencyAmount.of(Currency.EUR, 40)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 32), CurrencyAmount.of(Currency.EUR, 43)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44)));

		assertThat(VALUES_ARRAY.ToList()).isEqualTo(expected);
	  }

	  public virtual void convert()
	  {
		FxRateScenarioArray rates1 = FxRateScenarioArray.of(GBP, CAD, DoubleArray.of(2.00, 2.01, 2.02));
		FxRateScenarioArray rates2 = FxRateScenarioArray.of(USD, CAD, DoubleArray.of(1.30, 1.31, 1.32));
		FxRateScenarioArray rates3 = FxRateScenarioArray.of(EUR, CAD, DoubleArray.of(1.4, 1.4, 1.4));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates1, rates2, rates3);
		CurrencyScenarioArray convertedArray = VALUES_ARRAY.convertedTo(Currency.CAD, fxProvider);
		DoubleArray expected = DoubleArray.of(20 * 2.00 + 30 * 1.30 + 40 * 1.4, 21 * 2.01 + 32 * 1.31 + 43 * 1.4, 22 * 2.02 + 33 * 1.32 + 44 * 1.4);
		assertThat(convertedArray.Amounts.Values).isEqualTo(expected);
	  }

	  public virtual void convertIntoAnExistingCurrency()
	  {
		FxRateScenarioArray rates1 = FxRateScenarioArray.of(USD, GBP, DoubleArray.of(1 / 1.50, 1 / 1.51, 1 / 1.52));
		FxRateScenarioArray rates2 = FxRateScenarioArray.of(EUR, GBP, DoubleArray.of(0.7, 0.7, 0.7));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates1, rates2);
		CurrencyScenarioArray convertedArray = VALUES_ARRAY.convertedTo(Currency.GBP, fxProvider);
		assertThat(convertedArray.Currency).isEqualTo(Currency.GBP);
		double[] expected = new double[] {20 + 30 / 1.50 + 40 * 0.7, 21 + 32 / 1.51 + 43 * 0.7, 22 + 33 / 1.52 + 44 * 0.7};

		for (int i = 0; i < 3; i++)
		{
		  assertThat(convertedArray.get(i).Amount).isEqualTo(expected[i], offset(1e-6));
		}
	  }

	  /// <summary>
	  /// Test the hand-written equals and hashCode methods which correctly handle maps with array values
	  /// </summary>
	  public virtual void equalsHashCode()
	  {
		MultiCurrencyScenarioArray array = MultiCurrencyScenarioArray.of(ImmutableList.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 20), CurrencyAmount.of(Currency.USD, 30), CurrencyAmount.of(Currency.EUR, 40)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 32), CurrencyAmount.of(Currency.EUR, 43)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44))));
		assertThat(array).isEqualTo(VALUES_ARRAY);
		assertThat(array.GetHashCode()).isEqualTo(VALUES_ARRAY.GetHashCode());
	  }

	  public virtual void getCurrencies()
	  {
		assertThat(VALUES_ARRAY.Currencies).containsExactlyInAnyOrder(Currency.GBP, Currency.USD, Currency.EUR);
	  }

	  public virtual void collector()
	  {
		IList<CurrencyScenarioArray> arrays = ImmutableList.of(CurrencyScenarioArray.of(USD, DoubleArray.of(10, 20, 30)), CurrencyScenarioArray.of(USD, DoubleArray.of(5, 6, 7)), CurrencyScenarioArray.of(EUR, DoubleArray.of(2, 4, 6)), CurrencyScenarioArray.of(GBP, DoubleArray.of(11, 12, 13)), CurrencyScenarioArray.of(GBP, DoubleArray.of(1, 2, 3)));

		IDictionary<Currency, DoubleArray> expectedMap = ImmutableMap.of(USD, DoubleArray.of(15, 26, 37), EUR, DoubleArray.of(2, 4, 6), GBP, DoubleArray.of(12, 14, 16));

		MultiCurrencyScenarioArray expected = MultiCurrencyScenarioArray.of(expectedMap);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		assertThat(arrays.collect(toMultiCurrencyScenarioArray())).isEqualTo(expected);
	  }

	  public virtual void total()
	  {
		IList<CurrencyScenarioArray> arrays = ImmutableList.of(CurrencyScenarioArray.of(USD, DoubleArray.of(10, 20, 30)), CurrencyScenarioArray.of(USD, DoubleArray.of(5, 6, 7)), CurrencyScenarioArray.of(EUR, DoubleArray.of(2, 4, 6)), CurrencyScenarioArray.of(GBP, DoubleArray.of(11, 12, 13)), CurrencyScenarioArray.of(GBP, DoubleArray.of(1, 2, 3)));

		IDictionary<Currency, DoubleArray> expectedMap = ImmutableMap.of(USD, DoubleArray.of(15, 26, 37), EUR, DoubleArray.of(2, 4, 6), GBP, DoubleArray.of(12, 14, 16));

		MultiCurrencyScenarioArray expected = MultiCurrencyScenarioArray.of(expectedMap);
		assertThat(MultiCurrencyScenarioArray.total(arrays)).isEqualTo(expected);
	  }

	  public virtual void collectorDifferentArrayLengths()
	  {
		IList<CurrencyScenarioArray> arrays = ImmutableList.of(CurrencyScenarioArray.of(USD, DoubleArray.of(10, 20, 30)), CurrencyScenarioArray.of(GBP, DoubleArray.of(1, 2)));

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		assertThrowsIllegalArg(() => arrays.collect(toMultiCurrencyScenarioArray()));
	  }

	  public virtual void coverage()
	  {
		coverImmutableBean(VALUES_ARRAY);
		MultiCurrencyScenarioArray test2 = MultiCurrencyScenarioArray.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 31), CurrencyAmount.of(Currency.EUR, 41)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44)));
		coverBeanEquals(VALUES_ARRAY, test2);
	  }

	}

}