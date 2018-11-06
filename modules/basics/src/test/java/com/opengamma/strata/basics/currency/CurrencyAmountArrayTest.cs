using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
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
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test <seealso cref="CurrencyAmountArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyAmountArrayTest
	public class CurrencyAmountArrayTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyDoubleArray()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, values);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Values).isEqualTo(values);
		assertThat(test.size()).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void test_of_List()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
		CurrencyAmountArray test = CurrencyAmountArray.of(values);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.size()).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void test_of_CurrencyList_mixedCurrency()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(USD, 2), CurrencyAmount.of(GBP, 3));
		assertThrowsIllegalArg(() => CurrencyAmountArray.of(values));
	  }

	  public virtual void test_of_function()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
		CurrencyAmountArray test = CurrencyAmountArray.of(3, i => values[i]);
		assertThat(test.Currency).isEqualTo(GBP);
		assertThat(test.Values).isEqualTo(DoubleArray.of(1d, 2d, 3d));
		assertThat(test.size()).isEqualTo(3);
		assertThat(test.get(0)).isEqualTo(CurrencyAmount.of(GBP, 1));
		assertThat(test.get(1)).isEqualTo(CurrencyAmount.of(GBP, 2));
		assertThat(test.get(2)).isEqualTo(CurrencyAmount.of(GBP, 3));
		assertThat(test.ToList()).containsExactly(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(GBP, 2), CurrencyAmount.of(GBP, 3));
	  }

	  public virtual void test_of_function_mixedCurrency()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(USD, 2), CurrencyAmount.of(GBP, 3));
		assertThrowsIllegalArg(() => CurrencyAmountArray.of(3, i => values[i]));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus()
	  {
		IList<CurrencyAmount> values = ImmutableList.of(CurrencyAmount.of(GBP, 1), CurrencyAmount.of(USD, 2), CurrencyAmount.of(GBP, 3));
		assertThrowsIllegalArg(() => CurrencyAmountArray.of(3, i => values[i]));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, values);

		FxRate fxRate = FxRate.of(GBP, USD, 1.61);
		CurrencyAmountArray convertedList = test.convertedTo(USD, fxRate);
		DoubleArray expectedValues = DoubleArray.of(1 * 1.61, 2 * 1.61, 3 * 1.61);
		CurrencyAmountArray expectedList = CurrencyAmountArray.of(USD, expectedValues);
		assertThat(convertedList).isEqualTo(expectedList);
	  }

	  public virtual void test_convertedTo_noConversionNecessary()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, values);

		FxRate fxRate = FxRate.of(GBP, USD, 1.61);
		CurrencyAmountArray convertedList = test.convertedTo(GBP, fxRate);
		assertThat(convertedList).isEqualTo(test);
	  }

	  public virtual void test_convertedTo_missingFxRate()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, values);

		FxRate fxRate = FxRate.of(EUR, USD, 1.61);
		assertThrows(() => test.convertedTo(USD, fxRate), typeof(System.ArgumentException));
	  }

	  public virtual void test_minus_currencyAmount()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray array = CurrencyAmountArray.of(GBP, values);

		CurrencyAmountArray result = array.minus(CurrencyAmount.of(GBP, 0.5));
		assertThat(result).isEqualTo(CurrencyAmountArray.of(GBP, DoubleArray.of(0.5, 1.5, 2.5)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		DoubleArray values = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, values);
		coverImmutableBean(test);
		DoubleArray values2 = DoubleArray.of(1, 2, 3);
		CurrencyAmountArray test2 = CurrencyAmountArray.of(USD, values2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CurrencyAmountArray test = CurrencyAmountArray.of(GBP, DoubleArray.of(1, 2, 3));
		assertSerialization(test);
	  }

	}

}