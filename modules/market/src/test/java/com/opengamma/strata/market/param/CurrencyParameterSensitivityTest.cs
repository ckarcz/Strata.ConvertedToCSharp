using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using MapStream = com.opengamma.strata.collect.MapStream;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;

	/// <summary>
	/// Test <seealso cref="CurrencyParameterSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CurrencyParameterSensitivityTest
	public class CurrencyParameterSensitivityTest
	{

	  private const double FACTOR1 = 3.14;
	  private static readonly DoubleArray VECTOR_USD1 = DoubleArray.of(100, 200, 300, 123);
	  private static readonly DoubleArray VECTOR_USD2 = DoubleArray.of(150, 250, 350, 153, 550);
	  private static readonly DoubleArray VECTOR_USD_COMBINED = VECTOR_USD1.concat(VECTOR_USD2);
	  private static readonly DoubleArray VECTOR_USD_FACTOR = DoubleArray.of(100 * FACTOR1, 200 * FACTOR1, 300 * FACTOR1, 123 * FACTOR1);
	  private static readonly DoubleArray VECTOR_EUR1 = DoubleArray.of(1000, 250, 321, 123, 321);
	  private static readonly DoubleArray VECTOR_EUR1_IN_USD = DoubleArray.of(1000 * 1.5, 250 * 1.5, 321 * 1.5, 123 * 1.5, 321 * 1.5);
	  private static readonly Currency USD = Currency.USD;
	  private static readonly Currency EUR = Currency.EUR;
	  private static readonly FxRate FX_RATE = FxRate.of(EUR, USD, 1.5d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME_COMBINED = com.opengamma.strata.market.curve.CurveName.of("NAME_COMBINED");
	  private static readonly MarketDataName<object> NAME_COMBINED = CurveName.of("NAME_COMBINED");
	  private static readonly IList<ParameterMetadata> METADATA_USD1 = ParameterMetadata.listOfEmpty(4);
	  private static readonly IList<ParameterMetadata> METADATA_USD2 = ParameterMetadata.listOfEmpty(5);
	  private static readonly IList<ParameterMetadata> METADATA_EUR1 = ParameterMetadata.listOfEmpty(5);
	  private static readonly IList<ParameterMetadata> METADATA_BAD = ParameterMetadata.listOfEmpty(1);
	  private static readonly ImmutableList<ParameterMetadata> METADATA_COMBINED = ImmutableList.builder<ParameterMetadata>().addAll(METADATA_USD1).addAll(METADATA_USD2).build();
	  private static readonly IList<ParameterSize> PARAM_SPLIT = ImmutableList.of(ParameterSize.of(NAME1, 4), ParameterSize.of(NAME2, 5));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_metadata()
	  {
		CurrencyParameterSensitivity test = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		assertEquals(test.MarketDataName, NAME1);
		assertEquals(test.ParameterCount, VECTOR_USD1.size());
		assertEquals(test.ParameterMetadata, METADATA_USD1);
		assertEquals(test.getParameterMetadata(0), METADATA_USD1[0]);
		assertEquals(test.Currency, USD);
		assertEquals(test.Sensitivity, VECTOR_USD1);
	  }

	  public virtual void test_of_metadata_badMetadata()
	  {
		assertThrowsIllegalArg(() => CurrencyParameterSensitivity.of(NAME1, METADATA_BAD, USD, VECTOR_USD1));
	  }

	  public virtual void test_of_metadataParamSplit()
	  {
		CurrencyParameterSensitivity test = CurrencyParameterSensitivity.of(NAME_COMBINED, METADATA_COMBINED, USD, VECTOR_USD_COMBINED, PARAM_SPLIT);
		assertEquals(test.MarketDataName, NAME_COMBINED);
		assertEquals(test.ParameterCount, VECTOR_USD_COMBINED.size());
		assertEquals(test.ParameterMetadata, METADATA_COMBINED);
		assertEquals(test.getParameterMetadata(0), METADATA_COMBINED.get(0));
		assertEquals(test.Sensitivity, VECTOR_USD_COMBINED);
		assertEquals(test.ParameterSplit, PARAM_SPLIT);
	  }

	  public virtual void test_of_metadataParamSplit_badSplit()
	  {
		assertThrowsIllegalArg(() => CurrencyParameterSensitivity.of(NAME_COMBINED, METADATA_USD1, USD, VECTOR_USD1, PARAM_SPLIT));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_map()
	  {
		ImmutableMap<ParameterMetadata, double> map = ImmutableMap.of(TenorParameterMetadata.of(Tenor.TENOR_1Y), 12d, TenorParameterMetadata.of(Tenor.TENOR_2Y), -32d, TenorParameterMetadata.of(Tenor.TENOR_5Y), 5d);
		CurrencyParameterSensitivity test = CurrencyParameterSensitivity.of(NAME1, USD, map);
		assertEquals(test.MarketDataName, NAME1);
		assertEquals(test.ParameterCount, 3);
		assertEquals(test.ParameterMetadata, map.Keys.asList());
		assertEquals(test.Currency, USD);
		assertEquals(test.Sensitivity, DoubleArray.copyOf(map.values()));
		assertEquals(test.sensitivities().toMap(), map);
		assertEquals(test.toSensitivityMap(typeof(Tenor)), MapStream.of(map).mapKeys(pm => pm.Identifier).toMap());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combine()
	  {
		CurrencyParameterSensitivity base1 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity base2 = CurrencyParameterSensitivity.of(NAME2, METADATA_USD2, USD, VECTOR_USD2);
		CurrencyParameterSensitivity test = CurrencyParameterSensitivity.combine(NAME_COMBINED, base1, base2);
		assertEquals(test.MarketDataName, NAME_COMBINED);
		assertEquals(test.ParameterCount, VECTOR_USD_COMBINED.size());
		assertEquals(test.ParameterMetadata, METADATA_COMBINED);
		assertEquals(test.getParameterMetadata(0), METADATA_COMBINED.get(0));
		assertEquals(test.Sensitivity, VECTOR_USD_COMBINED);
		assertEquals(test.ParameterSplit, PARAM_SPLIT);
	  }

	  public virtual void test_combine_arraySize0()
	  {
		assertThrowsIllegalArg(() => CurrencyParameterSensitivity.combine(NAME_COMBINED));
	  }

	  public virtual void test_combine_arraySize1()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		assertThrowsIllegalArg(() => CurrencyParameterSensitivity.combine(NAME_COMBINED, @base));
	  }

	  public virtual void test_combine_duplicateNames()
	  {
		CurrencyParameterSensitivity base1 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity base2 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD2, USD, VECTOR_USD2);
		assertThrowsIllegalArg(() => CurrencyParameterSensitivity.combine(NAME_COMBINED, base1, base2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_EUR1, EUR, VECTOR_EUR1);
		CurrencyParameterSensitivity test = @base.convertedTo(USD, FX_RATE);
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA_EUR1, USD, VECTOR_EUR1_IN_USD));
	  }

	  public virtual void test_convertedTo_sameCurrency()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_EUR1, EUR, VECTOR_EUR1);
		CurrencyParameterSensitivity test = @base.convertedTo(EUR, FX_RATE);
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity test = @base.multipliedBy(FACTOR1);
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD_FACTOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity test = @base.withSensitivity(VECTOR_USD_FACTOR);
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD_FACTOR));
		assertThrowsIllegalArg(() => @base.withSensitivity(DoubleArray.of(1d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_array()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity test = @base.plus(VECTOR_USD1);
		assertEquals(test, @base.multipliedBy(2));
	  }

	  public virtual void test_plus_array_wrongSize()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		assertThrowsIllegalArg(() => @base.plus(VECTOR_USD2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_sensitivity()
	  {
		CurrencyParameterSensitivity base1 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity test = base1.plus(base1);
		assertEquals(test, base1.multipliedBy(2));
	  }

	  public virtual void test_plus_sensitivity_wrongName()
	  {
		CurrencyParameterSensitivity base1 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity base2 = CurrencyParameterSensitivity.of(NAME2, METADATA_USD1, USD, VECTOR_USD1);
		assertThrowsIllegalArg(() => base1.plus(base2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_split1()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		ImmutableList<CurrencyParameterSensitivity> test = @base.split();
		assertEquals(test.size(), 1);
		assertEquals(test.get(0), @base);
	  }

	  public virtual void test_split2()
	  {
		CurrencyParameterSensitivity base1 = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyParameterSensitivity base2 = CurrencyParameterSensitivity.of(NAME2, METADATA_USD2, USD, VECTOR_USD2);
		CurrencyParameterSensitivity combined = CurrencyParameterSensitivity.combine(NAME_COMBINED, base1, base2);
		ImmutableList<CurrencyParameterSensitivity> test = combined.split();
		assertEquals(test.size(), 2);
		assertEquals(test.get(0), base1);
		assertEquals(test.get(1), base2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_total()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		CurrencyAmount test = @base.total();
		assertEquals(test.Currency, USD);
		double expected = VECTOR_USD1.get(0) + VECTOR_USD1.get(1) + VECTOR_USD1.get(2) + VECTOR_USD1.get(3);
		assertEquals(test.Amount, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toSensitivityMap_badType()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		assertThrows(typeof(System.InvalidCastException), () => @base.toSensitivityMap(typeof(Tenor)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toUnitParameterSensitivity()
	  {
		CurrencyParameterSensitivity @base = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		UnitParameterSensitivity test = @base.toUnitParameterSensitivity();
		assertEquals(test, UnitParameterSensitivity.of(NAME1, METADATA_USD1, VECTOR_USD1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CurrencyParameterSensitivity test = CurrencyParameterSensitivity.of(NAME1, METADATA_USD1, USD, VECTOR_USD1);
		coverImmutableBean(test);
		CurrencyParameterSensitivity test2 = CurrencyParameterSensitivity.of(NAME2, METADATA_EUR1, EUR, VECTOR_EUR1);
		coverBeanEquals(test, test2);
	  }

	}

}