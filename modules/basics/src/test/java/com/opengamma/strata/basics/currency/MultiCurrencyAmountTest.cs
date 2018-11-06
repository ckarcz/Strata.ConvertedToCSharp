using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.MultiCurrencyAmount.toMultiCurrencyAmount;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using BeanBuilder = org.joda.beans.BeanBuilder;
	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;

	/// <summary>
	/// Test <seealso cref="MultiCurrencyAmount"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MultiCurrencyAmountTest
	public class MultiCurrencyAmountTest
	{

	  private static readonly Currency CCY1 = Currency.AUD;
	  private static readonly Currency CCY2 = Currency.CAD;
	  private static readonly Currency CCY3 = Currency.CHF;
	  private const double AMT1 = 101;
	  private const double AMT2 = 103;
	  private const double AMT3 = 107;
	  private static readonly CurrencyAmount CA1 = CurrencyAmount.of(CCY1, AMT1);
	  private static readonly CurrencyAmount CA2 = CurrencyAmount.of(CCY2, AMT2);
	  private static readonly CurrencyAmount CA3 = CurrencyAmount.of(CCY3, AMT3);

	  //-------------------------------------------------------------------------
	  public virtual void test_empty()
	  {
		assertMCA(MultiCurrencyAmount.empty());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_CurrencyDouble()
	  {
		assertMCA(MultiCurrencyAmount.of(CCY1, AMT1), CA1);
	  }

	  public virtual void test_of_CurrencyDouble_null()
	  {
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(null, AMT1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_VarArgs_empty()
	  {
		assertMCA(MultiCurrencyAmount.of());
	  }

	  public virtual void test_of_VarArgs()
	  {
		assertMCA(MultiCurrencyAmount.of(CA1, CA3), CA1, CA3);
	  }

	  public virtual void test_of_VarArgs_duplicate()
	  {
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(CA1, CurrencyAmount.of(CCY1, AMT2)));
	  }

	  public virtual void test_of_VarArgs_null()
	  {
		CurrencyAmount[] array = null;
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(array));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_Iterable()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, CA3);
		assertMCA(MultiCurrencyAmount.of(iterable), CA1, CA3);
	  }

	  public virtual void test_of_Iterable_duplicate()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, CurrencyAmount.of(CCY1, AMT2));
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(iterable));
	  }

	  public virtual void test_of_Iterable_null()
	  {
		IEnumerable<CurrencyAmount> iterable = null;
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(iterable));
	  }

	  public virtual void test_of_Iterable_containsNull()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, null, CA2);
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(iterable));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_Map()
	  {
		IDictionary<Currency, double> map = ImmutableMap.builder<Currency, double>().put(CCY1, AMT1).put(CCY3, AMT3).build();
		assertMCA(MultiCurrencyAmount.of(map), CA1, CA3);
	  }

	  public virtual void test_of_Map_null()
	  {
		IDictionary<Currency, double> map = null;
		assertThrowsIllegalArg(() => MultiCurrencyAmount.of(map));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_total_Iterable()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, CA3);
		assertMCA(MultiCurrencyAmount.total(iterable), CA1, CA3);
	  }

	  public virtual void test_total_Iterable_duplicate()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, CurrencyAmount.of(CCY1, AMT2), CA2);
		assertMCA(MultiCurrencyAmount.total(iterable), CurrencyAmount.of(CCY1, AMT1 + AMT2), CA2);
	  }

	  public virtual void test_total_Iterable_null()
	  {
		IEnumerable<CurrencyAmount> iterable = null;
		assertThrowsIllegalArg(() => MultiCurrencyAmount.total(iterable));
	  }

	  public virtual void test_total_Iterable_containsNull()
	  {
		IEnumerable<CurrencyAmount> iterable = Arrays.asList(CA1, null, CA2);
		assertThrowsIllegalArg(() => MultiCurrencyAmount.total(iterable));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collector()
	  {
		IList<CurrencyAmount> amount = ImmutableList.of(CurrencyAmount.of(CCY1, 100), CurrencyAmount.of(CCY1, 150), CurrencyAmount.of(CCY2, 100));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		MultiCurrencyAmount test = amount.collect(toMultiCurrencyAmount());
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(CurrencyAmount.of(CCY1, 250), CurrencyAmount.of(CCY2, 100));
		assertEquals(test, expected);
	  }

	  public virtual void test_collector_parallel()
	  {
		IList<CurrencyAmount> amount = ImmutableList.of(CurrencyAmount.of(CCY1, 100), CurrencyAmount.of(CCY1, 150), CurrencyAmount.of(CCY2, 100));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		MultiCurrencyAmount test = amount.collect(toMultiCurrencyAmount());
		MultiCurrencyAmount expected = MultiCurrencyAmount.of(CurrencyAmount.of(CCY1, 250), CurrencyAmount.of(CCY2, 100));
		assertEquals(test, expected);
	  }

	  public virtual void test_collector_null()
	  {
		IList<CurrencyAmount> amount = Arrays.asList(CurrencyAmount.of(CCY1, 100), null, CurrencyAmount.of(CCY2, 100));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		assertThrowsIllegalArg(() => amount.collect(toMultiCurrencyAmount()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_beanBuilder()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.meta().builder().set(MultiCurrencyAmount.meta().amounts(), ImmutableSortedSet.of(CA1, CA2, CA3)).build();
		assertMCA(test, CA1, CA2, CA3);
	  }

	  public virtual void test_beanBuilder_invalid()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends MultiCurrencyAmount> test = MultiCurrencyAmount.meta().builder().set(MultiCurrencyAmount.meta().amounts(), com.google.common.collect.ImmutableSortedSet.of(CA1, CA2, CurrencyAmount.of(CA1.getCurrency(), AMT3)));
		BeanBuilder<MultiCurrencyAmount> test = MultiCurrencyAmount.meta().builder().set(MultiCurrencyAmount.meta().amounts(), ImmutableSortedSet.of(CA1, CA2, CurrencyAmount.of(CA1.Currency, AMT3)));
		assertThrowsIllegalArg(() => test.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_contains_null()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => @base.contains(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_CurrencyDouble_merge()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.plus(Currency.AUD, 3);
		assertMCA(test, cb, CurrencyAmount.of(Currency.AUD, 120));
	  }

	  public virtual void test_plus_CurrencyDouble_add()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.plus(Currency.NZD, 3);
		assertMCA(test, ca, cb, CurrencyAmount.of(Currency.NZD, 3));
	  }

	  public virtual void test_plus_CurrencyDouble_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.plus((Currency) null, 1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_CurrencyAmount_merge()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.AUD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.plus(cc);
		assertMCA(test, cb, CurrencyAmount.of(Currency.AUD, 120));
	  }

	  public virtual void test_plus_CurrencyAmount_add()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.NZD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.plus(cc);
		assertMCA(test, ca, cb, cc);
	  }

	  public virtual void test_plus_CurrencyAmount_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.plus((CurrencyAmount) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_MultiCurrencyAmount_mergeAndAdd()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.AUD, 3);
		CurrencyAmount cd = CurrencyAmount.of(Currency.NZD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount mc2 = MultiCurrencyAmount.of(cc, cd);
		MultiCurrencyAmount test = mc1.plus(mc2);
		assertMCA(test, cb, cd, CurrencyAmount.of(Currency.AUD, 120));
	  }

	  public virtual void test_plus_MultiCurrencyAmount_empty()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount mc2 = MultiCurrencyAmount.of();
		MultiCurrencyAmount test = mc1.plus(mc2);
		assertMCA(test, ca, cb);
	  }

	  public virtual void test_plus_MultiCurrencyAmount_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.plus((MultiCurrencyAmount) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_minus_CurrencyDouble_merge()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.minus(Currency.AUD, 3);
		assertMCA(test, cb, CurrencyAmount.of(Currency.AUD, 114));
	  }

	  public virtual void test_minus_CurrencyDouble_add()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.minus(Currency.NZD, 3);
		assertMCA(test, ca, cb, CurrencyAmount.of(Currency.NZD, -3));
	  }

	  public virtual void test_minus_CurrencyDouble_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.minus((Currency) null, 1));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_minus_CurrencyAmount_merge()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.AUD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.minus(cc);
		assertMCA(test, cb, CurrencyAmount.of(Currency.AUD, 114));
	  }

	  public virtual void test_minus_CurrencyAmount_add()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.NZD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount test = mc1.minus(cc);
		assertMCA(test, ca, cb, cc.negated());
	  }

	  public virtual void test_minus_CurrencyAmount_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.minus((CurrencyAmount) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_minus_MultiCurrencyAmount_mergeAndAdd()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		CurrencyAmount cc = CurrencyAmount.of(Currency.AUD, 3);
		CurrencyAmount cd = CurrencyAmount.of(Currency.NZD, 3);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount mc2 = MultiCurrencyAmount.of(cc, cd);
		MultiCurrencyAmount test = mc1.minus(mc2);
		assertMCA(test, cb, cd.negated(), CurrencyAmount.of(Currency.AUD, 114));
	  }

	  public virtual void test_minus_MultiCurrencyAmount_empty()
	  {
		CurrencyAmount ca = CurrencyAmount.of(Currency.AUD, 117);
		CurrencyAmount cb = CurrencyAmount.of(Currency.USD, 12);
		MultiCurrencyAmount mc1 = MultiCurrencyAmount.of(ca, cb);
		MultiCurrencyAmount mc2 = MultiCurrencyAmount.of();
		MultiCurrencyAmount test = mc1.minus(mc2);
		assertMCA(test, ca, cb);
	  }

	  public virtual void test_minus_MultiCurrencyAmount_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.minus((MultiCurrencyAmount) null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
		MultiCurrencyAmount test = @base.multipliedBy(2.5);
		assertMCA(test, CA1.multipliedBy(2.5), CA2.multipliedBy(2.5));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_negated()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
		MultiCurrencyAmount test = @base.negated();
		assertMCA(test, CA1.negated(), CA2.negated());
		assertEquals(MultiCurrencyAmount.of(CurrencyAmount.zero(Currency.USD), CurrencyAmount.zero(Currency.EUR)).negated(), MultiCurrencyAmount.of(CurrencyAmount.zero(Currency.USD), CurrencyAmount.zero(Currency.EUR)));
		assertEquals(MultiCurrencyAmount.of(CurrencyAmount.of(Currency.USD, -0d), CurrencyAmount.of(Currency.EUR, -0d)).negated(), MultiCurrencyAmount.of(CurrencyAmount.zero(Currency.USD), CurrencyAmount.zero(Currency.EUR)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapAmounts()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
		MultiCurrencyAmount test = @base.mapAmounts(a => a * 2.5 + 1);
		assertMCA(test, CA1.mapAmount(a => a * 2.5 + 1), CA2.mapAmount(a => a * 2.5 + 1));
	  }

	  public virtual void test_mapAmounts_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.mapAmounts(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapCurrencyAmounts()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
		MultiCurrencyAmount test = @base.mapCurrencyAmounts(a => CurrencyAmount.of(CCY3, 1));
		assertMCA(test, CurrencyAmount.of(CCY3, 2));
	  }

	  public virtual void test_mapCurrencyAmounts_null()
	  {
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertThrowsIllegalArg(() => test.mapCurrencyAmounts(null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream()
	  {
		MultiCurrencyAmount @base = MultiCurrencyAmount.of(CA1, CA2);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		MultiCurrencyAmount test = @base.Select(ca => ca.mapAmount(a => a * 3)).collect(toMultiCurrencyAmount());
		assertMCA(test, CA1.mapAmount(a => a * 3), CA2.mapAmount(a => a * 3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toMap()
	  {
		ImmutableSortedMap<Currency, double> test = MultiCurrencyAmount.of(CA1, CA2).toMap();
		assertEquals(test.size(), 2);
		assertEquals(test.containsKey(CA1.Currency), true);
		assertEquals(test.containsKey(CA2.Currency), true);
		assertEquals(test.get(CA1.Currency), Convert.ToDouble(CA1.Amount));
		assertEquals(test.get(CA2.Currency), Convert.ToDouble(CA2.Amount));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo_rateProvider_noConversionSize1()
	  {
		FxRateProvider provider = (ccy1, ccy2) =>
		{
	  throw new System.ArgumentException();
		};
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA2);
		assertEquals(test.convertedTo(CCY2, provider), CA2);
	  }

	  public virtual void test_convertedTo_rateProvider_conversionSize1()
	  {
		FxRateProvider provider = (ccy1, ccy2) =>
		{
	  if (ccy1.Equals(CCY1) && ccy2.Equals(CCY2))
	  {
		return 2.5d;
	  }
	  throw new System.ArgumentException();
		};
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1);
		assertEquals(test.convertedTo(CCY2, provider), CurrencyAmount.of(CCY2, AMT1 * 2.5d));
	  }

	  public virtual void test_convertedTo_rateProvider_conversionSize2()
	  {
		FxRateProvider provider = (ccy1, ccy2) =>
		{
	  if (ccy1.Equals(ccy2))
	  {
		return 1d;
	  }
	  if (ccy1.Equals(CCY1) && ccy2.Equals(CCY2))
	  {
		return 2.5d;
	  }
	  throw new System.ArgumentException();
		};
		MultiCurrencyAmount test = MultiCurrencyAmount.of(CA1, CA2);
		assertEquals(test.convertedTo(CCY2, provider), CA2.plus(CurrencyAmount.of(CCY2, AMT1 * 2.5d)));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void test_serialization()
	  {
		assertSerialization(MultiCurrencyAmount.of(CA1, CA2, CA3));
	  }

	  public virtual void coverage()
	  {
		coverImmutableBean(MultiCurrencyAmount.of(CA1, CA2, CA3));
	  }

	  //-------------------------------------------------------------------------
	  private void assertMCA(MultiCurrencyAmount actual, params CurrencyAmount[] expected)
	  {
		assertEquals(actual.size(), expected.Length);
		assertEquals(actual.Amounts.size(), expected.Length);
		assertEquals(actual.Amounts, ImmutableSet.copyOf(expected));
		ISet<Currency> currencies = new HashSet<Currency>();
		foreach (CurrencyAmount expectedAmount in expected)
		{
		  currencies.Add(expectedAmount.Currency);
		  assertEquals(actual.contains(expectedAmount.Currency), true);
		  assertEquals(actual.getAmount(expectedAmount.Currency), expectedAmount);
		  assertEquals(actual.getAmountOrZero(expectedAmount.Currency), expectedAmount);
		}
		assertEquals(actual.Currencies, currencies);
		Currency nonExisting = Currency.of("FRZ");
		assertEquals(actual.contains(nonExisting), false);
		assertThrowsIllegalArg(() => actual.getAmount(nonExisting));
		assertEquals(actual.getAmountOrZero(nonExisting), CurrencyAmount.zero(nonExisting));
	  }

	}

}