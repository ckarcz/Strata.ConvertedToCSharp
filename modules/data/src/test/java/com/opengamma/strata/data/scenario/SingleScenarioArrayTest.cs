using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="SingleScenarioArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SingleScenarioArrayTest
	public class SingleScenarioArrayTest
	{

	  public virtual void create()
	  {
		SingleScenarioArray<string> test = SingleScenarioArray.of(3, "A");
		assertEquals(test.ScenarioCount, 3);
		assertEquals(test.Value, "A");
		assertEquals(test.get(0), "A");
		assertEquals(test.get(1), "A");
		assertEquals(test.get(2), "A");
		assertEquals(test.ToList(), ImmutableList.of("A", "A", "A"));
	  }

	  public virtual void convertCurrencyAmount()
	  {
		FxRateScenarioArray rates = FxRateScenarioArray.of(GBP, USD, DoubleArray.of(1.61, 1.62, 1.63));
		ScenarioFxRateProvider fxProvider = new TestScenarioFxRateProvider(rates);
		SingleScenarioArray<CurrencyAmount> test = SingleScenarioArray.of(3, CurrencyAmount.of(GBP, 2));

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: ScenarioArray<?> convertedList = test.convertedTo(USD, fxProvider);
		ScenarioArray<object> convertedList = test.convertedTo(USD, fxProvider);
		IList<CurrencyAmount> expectedValues = ImmutableList.of(CurrencyAmount.of(USD, 2 * 1.61), CurrencyAmount.of(USD, 2 * 1.62), CurrencyAmount.of(USD, 2 * 1.63));
		DefaultScenarioArray<CurrencyAmount> expectedList = DefaultScenarioArray.of(expectedValues);
		assertThat(convertedList).isEqualTo(expectedList);
	  }

	  public virtual void coverage()
	  {
		SingleScenarioArray<string> test = SingleScenarioArray.of(3, "A");
		coverImmutableBean(test);
		SingleScenarioArray<string> test2 = SingleScenarioArray.of(2, "B");
		coverBeanEquals(test, test2);
	  }

	}

}