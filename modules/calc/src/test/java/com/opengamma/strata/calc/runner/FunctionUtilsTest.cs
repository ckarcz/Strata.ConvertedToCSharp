using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using MultiCurrencyAmount = com.opengamma.strata.basics.currency.MultiCurrencyAmount;
	using MultiCurrencyScenarioArray = com.opengamma.strata.data.scenario.MultiCurrencyScenarioArray;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FunctionUtilsTest
	public class FunctionUtilsTest
	{

	  public virtual void toScenarioArray()
	  {
		IList<CurrencyAmount> amounts = ImmutableList.of(CurrencyAmount.of(Currency.GBP, 1), CurrencyAmount.of(Currency.USD, 2));

		ScenarioArray<CurrencyAmount> expectedResult = ScenarioArray.of(amounts);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ScenarioArray<CurrencyAmount> result = amounts.collect(FunctionUtils.toScenarioArray());
		assertThat(result).isEqualTo(expectedResult);
	  }

	  public virtual void toScenarioArray2()
	  {
		IList<MultiCurrencyAmount> amounts = ImmutableList.of(MultiCurrencyAmount.of(Currency.GBP, 1), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.USD, 2), CurrencyAmount.of(Currency.GBP, 3)));

		ScenarioArray<MultiCurrencyAmount> expectedResult = ScenarioArray.of(amounts);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ScenarioArray<MultiCurrencyAmount> result = amounts.collect(FunctionUtils.toScenarioArray());
		assertThat(result).isEqualTo(expectedResult);
	  }

	  public virtual void toMultiCurrencyArray()
	  {
		IList<MultiCurrencyAmount> amounts = ImmutableList.of(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 20), CurrencyAmount.of(Currency.USD, 30), CurrencyAmount.of(Currency.EUR, 40)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 21), CurrencyAmount.of(Currency.USD, 32), CurrencyAmount.of(Currency.EUR, 43)), MultiCurrencyAmount.of(CurrencyAmount.of(Currency.GBP, 22), CurrencyAmount.of(Currency.USD, 33), CurrencyAmount.of(Currency.EUR, 44)));

		MultiCurrencyScenarioArray expected = MultiCurrencyScenarioArray.of(amounts);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		MultiCurrencyScenarioArray array = amounts.collect(FunctionUtils.toMultiCurrencyValuesArray());
		assertThat(array).isEqualTo(expected);
	  }
	}

}