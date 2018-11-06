using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.entriesToImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.pairsToImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertUtilityClass;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableMultiset = com.google.common.collect.ImmutableMultiset;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSetMultimap = com.google.common.collect.ImmutableSetMultimap;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;
	using Ordering = com.google.common.collect.Ordering;
	using ObjIntPair = com.opengamma.strata.collect.tuple.ObjIntPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test Guavate.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GuavateTest
	public class GuavateTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_concatToList()
	  {
		IEnumerable<string> iterable1 = Arrays.asList("a", "b", "c");
		IEnumerable<string> iterable2 = Arrays.asList("d", "e", "f");
		IList<string> test = Guavate.concatToList(iterable1, iterable2);
		assertEquals(test, ImmutableList.of("a", "b", "c", "d", "e", "f"));
	  }

	  public virtual void test_concatToList_differentTypes()
	  {
		IEnumerable<int> iterable1 = Arrays.asList(1, 2, 3);
		IEnumerable<double> iterable2 = Arrays.asList(10d, 20d, 30d);
		ImmutableList<Number> test = Guavate.concatToList(iterable1, iterable2);
		assertEquals(test, ImmutableList.of(1, 2, 3, 10d, 20d, 30d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_firstNonEmpty_supplierMatch1()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(() => Convert.ToInt32(1), () => Convert.ToDouble(2d));
		assertEquals(test, Convert.ToInt32(1));
	  }

	  public virtual void test_firstNonEmpty_supplierMatch2()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(() => null, () => Convert.ToDouble(2d));
		assertEquals(test, Convert.ToDouble(2d));
	  }

	  public virtual void test_firstNonEmpty_supplierMatchNone()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(() => null, () => null);
		assertEquals(test, null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_firstNonEmpty_optionalMatch1()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(Convert.ToInt32(1), Convert.ToDouble(2d));
		assertEquals(test, Convert.ToInt32(1));
	  }

	  public virtual void test_firstNonEmpty_optionalMatch2()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(null, Convert.ToDouble(2d));
		assertEquals(test, Convert.ToDouble(2d));
	  }

	  public virtual void test_firstNonEmpty_optionalMatchNone()
	  {
		Optional<Number> test = Guavate.firstNonEmpty(null, null);
		assertEquals(test, null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream_Iterable()
	  {
		IEnumerable<string> iterable = Arrays.asList("a", "b", "c");
		IList<string> test = Guavate.stream(iterable).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of("a", "b", "c"));
	  }

	  public virtual void test_stream_Optional()
	  {
		Optional<string> optional = "foo";
		IList<string> test1 = Guavate.stream(optional).collect(Collectors.toList());
		assertEquals(test1, ImmutableList.of("foo"));

		Optional<string> empty = null;
		IList<string> test2 = Guavate.stream(empty).collect(Collectors.toList());
		assertEquals(test2, ImmutableList.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zipWithIndex()
	  {
		Stream<string> @base = Stream.of("a", "b", "c");
		IList<ObjIntPair<string>> test = Guavate.zipWithIndex(@base).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of(ObjIntPair.of("a", 0), ObjIntPair.of("b", 1), ObjIntPair.of("c", 2)));
	  }

	  public virtual void test_zipWithIndex_empty()
	  {
		Stream<string> @base = Stream.of();
		IList<ObjIntPair<string>> test = Guavate.zipWithIndex(@base).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_zip()
	  {
		Stream<string> base1 = Stream.of("a", "b", "c");
		Stream<int> base2 = Stream.of(1, 2, 3);
		IList<Pair<string, int>> test = Guavate.zip(base1, base2).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of(Pair.of("a", 1), Pair.of("b", 2), Pair.of("c", 3)));
	  }

	  public virtual void test_zip_firstLonger()
	  {
		Stream<string> base1 = Stream.of("a", "b", "c");
		Stream<int> base2 = Stream.of(1, 2);
		IList<Pair<string, int>> test = Guavate.zip(base1, base2).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of(Pair.of("a", 1), Pair.of("b", 2)));
	  }

	  public virtual void test_zip_secondLonger()
	  {
		Stream<string> base1 = Stream.of("a", "b");
		Stream<int> base2 = Stream.of(1, 2, 3);
		IList<Pair<string, int>> test = Guavate.zip(base1, base2).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of(Pair.of("a", 1), Pair.of("b", 2)));
	  }

	  public virtual void test_zip_empty()
	  {
		Stream<string> base1 = Stream.of();
		Stream<int> base2 = Stream.of();
		IList<Pair<string, int>> test = Guavate.zip(base1, base2).collect(Collectors.toList());
		assertEquals(test, ImmutableList.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_not_Predicate()
	  {
		IList<string> data = Arrays.asList("a", "", "c");
		IList<string> test = data.Where(Guavate.not(string.isEmpty)).ToList();
		assertEquals(test, ImmutableList.of("a", "c"));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ensureOnlyOne()
	  {
		assertEquals(Stream.empty().reduce(Guavate.ensureOnlyOne()), null);
		assertEquals(Stream.of("a").reduce(Guavate.ensureOnlyOne()), ("a"));
		assertThrowsIllegalArg(() => Stream.of("a", "b").reduce(Guavate.ensureOnlyOne()));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toImmutableList()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<string> test = list.Where(s => s.length() == 1).collect(Guavate.toImmutableList());
		assertEquals(test, ImmutableList.of("a", "b", "c", "a"));
	  }

	  public virtual void test_splittingBySize()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<ImmutableList<string>> test = list.collect(Guavate.splittingBySize(4));
		assertEquals(test, ImmutableList.of(ImmutableList.of("a", "ab", "b", "bb"), ImmutableList.of("c", "a")));
	  }

	  public virtual void test_toImmutableSet()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSet<string> test = list.Where(s => s.length() == 1).collect(Guavate.toImmutableSet());
		assertEquals(test, ImmutableSet.of("a", "b", "c"));
	  }

	  public virtual void test_toImmutableSortedSet()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSortedSet<string> test = list.Where(s => s.length() == 1).collect(Guavate.toImmutableSortedSet());
		assertEquals(test, ImmutableSortedSet.of("a", "b", "c"));
	  }

	  public virtual void test_toImmutableSortedSet_comparator()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSortedSet<string> test = list.Where(s => s.length() == 1).collect(Guavate.toImmutableSortedSet(Ordering.natural().reverse()));
		assertEquals(test, ImmutableSortedSet.reverseOrder().add("a").add("b").add("c").build());
	  }

	  public virtual void test_toImmutableMultiset()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableMultiset<string> test = list.Where(s => s.length() == 1).collect(Guavate.toImmutableMultiset());
		assertEquals(test, ImmutableMultiset.of("a", "a", "b", "c"));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toImmutableMap_key()
	  {
		IList<string> list = Arrays.asList("a", "ab", "bob");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableMap<int, string> test = list.collect(Guavate.toImmutableMap(s => s.length()));
		assertEquals(test, ImmutableMap.builder().put(1, "a").put(2, "ab").put(3, "bob").build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_toImmutableMap_key_duplicateKeys()
	  public virtual void test_toImmutableMap_key_duplicateKeys()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		list.collect(Guavate.toImmutableMap(s => s.length()));
	  }

	  public virtual void test_toImmutableMap_mergeFn()
	  {
		IList<string> list = Arrays.asList("b", "a", "b", "b", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<string, int> result = list.collect(Guavate.toImmutableMap(s => s, s => 1, (s1, s2) => s1 + s2));
		IDictionary<string, int> expected = ImmutableMap.of("a", 2, "b", 3, "c", 1);
		assertEquals(result, expected);
		IEnumerator<string> iterator = result.Keys.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(iterator.next(), "b");
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(iterator.next(), "a");
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(iterator.next(), "c");
	  }

	  public virtual void test_toImmutableMap_keyValue()
	  {
		IList<string> list = Arrays.asList("a", "ab", "bob");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableMap<int, string> test = list.collect(Guavate.toImmutableMap(s => s.length(), s => "!" + s));
		assertEquals(test, ImmutableMap.builder().put(1, "!a").put(2, "!ab").put(3, "!bob").build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_toImmutableMap_keyValue_duplicateKeys()
	  public virtual void test_toImmutableMap_keyValue_duplicateKeys()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		list.collect(Guavate.toImmutableMap(s => s.length(), s => "!" + s));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toImmutableSortedMap_key()
	  {
		IList<string> list = Arrays.asList("bob", "a", "ab");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSortedMap<int, string> test = list.collect(Guavate.toImmutableSortedMap(s => s.length()));
		assertEquals(test, ImmutableSortedMap.naturalOrder().put(1, "a").put(2, "ab").put(3, "bob").build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_toImmutableSortedMap_key_duplicateKeys()
	  public virtual void test_toImmutableSortedMap_key_duplicateKeys()
	  {
		IList<string> list = Arrays.asList("a", "ab", "c", "bb", "b", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		list.collect(Guavate.toImmutableSortedMap(s => s.length()));
	  }

	  public virtual void test_toImmutableSortedMap_keyValue()
	  {
		IList<string> list = Arrays.asList("bob", "a", "ab");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSortedMap<int, string> test = list.collect(Guavate.toImmutableSortedMap(s => s.length(), s => "!" + s));
		assertEquals(test, ImmutableSortedMap.naturalOrder().put(1, "!a").put(2, "!ab").put(3, "!bob").build());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void test_toImmutableSortedMap_keyValue_duplicateKeys()
	  public virtual void test_toImmutableSortedMap_keyValue_duplicateKeys()
	  {
		IList<string> list = Arrays.asList("a", "ab", "c", "bb", "b", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		list.collect(Guavate.toImmutableSortedMap(s => s.length(), s => "!" + s));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toImmutableListMultimap_key()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableListMultimap<int, string> test = list.collect(Guavate.toImmutableListMultimap(s => s.length()));
		ImmutableListMultimap<object, object> expected = ImmutableListMultimap.builder().put(1, "a").put(2, "ab").put(1, "b").put(2, "bb").put(1, "c").put(1, "a").build();
		assertEquals(test, expected);
	  }

	  public virtual void test_toImmutableListMultimap_keyValue()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableListMultimap<int, string> test = list.collect(Guavate.toImmutableListMultimap(s => s.length(), s => "!" + s));
		ImmutableListMultimap<object, object> expected = ImmutableListMultimap.builder().put(1, "!a").put(2, "!ab").put(1, "!b").put(2, "!bb").put(1, "!c").put(1, "!a").build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toImmutableSetMultimap_key()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSetMultimap<int, string> test = list.collect(Guavate.toImmutableSetMultimap(s => s.length()));
		ImmutableSetMultimap<object, object> expected = ImmutableSetMultimap.builder().put(1, "a").put(2, "ab").put(1, "b").put(2, "bb").put(1, "c").put(1, "a").build();
		assertEquals(test, expected);
	  }

	  public virtual void test_toImmutableSetMultimap_keyValue()
	  {
		IList<string> list = Arrays.asList("a", "ab", "b", "bb", "c", "a");
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableSetMultimap<int, string> test = list.collect(Guavate.toImmutableSetMultimap(s => s.length(), s => "!" + s));
		ImmutableSetMultimap<object, object> expected = ImmutableSetMultimap.builder().put(1, "!a").put(2, "!ab").put(1, "!b").put(2, "!bb").put(1, "!c").build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapEntriesToImmutableMap()
	  {
		IDictionary<string, int> input = ImmutableMap.of("a", 1, "b", 2, "c", 3, "d", 4, "e", 5);
		IDictionary<string, int> expected = ImmutableMap.of("a", 1, "c", 3, "e", 5);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableMap<string, int> output = input.SetOfKeyValuePairs().Where(e => e.Value % 2 == 1).collect(entriesToImmutableMap());
		assertEquals(output, expected);
	  }

	  public virtual void test_pairsToImmutableMap()
	  {
		IDictionary<string, int> input = ImmutableMap.of("a", 1, "b", 2, "c", 3, "d", 4);
		IDictionary<string, double> expected = ImmutableMap.of("A", 1.0, "B", 4.0, "C", 9.0, "D", 16.0);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableMap<string, double> output = input.SetOfKeyValuePairs().Select(e => Pair.of(e.Key.ToUpper(Locale.ENGLISH), Math.Pow(e.Value, 2))).collect(pairsToImmutableMap());
		assertEquals(output, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_entry()
	  {
		KeyValuePair<string, int> test = Guavate.entry("A", 1);
		assertEquals(test.Key, "A");
		assertEquals(test.Value, (int?) 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combineFuturesAsList()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		return "B";
		});
		IList<CompletableFuture<string>> input = ImmutableList.of(future1, future2);

		CompletableFuture<IList<string>> test = Guavate.combineFuturesAsList(input);

		assertEquals(test.Done, false);
		latch.Signal();
		IList<string> combined = test.join();
		assertEquals(test.Done, true);
		assertEquals(combined.Count, 2);
		assertEquals(combined[0], "A");
		assertEquals(combined[1], "B");
	  }

	  public virtual void test_combineFuturesAsList_exception()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		throw new System.InvalidOperationException("Oops");
		});
		IList<CompletableFuture<string>> input = ImmutableList.of(future1, future2);

		CompletableFuture<IList<string>> test = Guavate.combineFuturesAsList(input);

		assertEquals(test.Done, false);
		latch.Signal();
		assertThrows(typeof(CompletionException), () => test.join());
		assertEquals(test.Done, true);
		assertEquals(test.CompletedExceptionally, true);
	  }

	  public virtual void test_toCombinedFuture()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		return "B";
		});
		IList<CompletableFuture<string>> input = ImmutableList.of(future1, future2);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		CompletableFuture<IList<string>> test = input.collect(Guavate.toCombinedFuture());

		assertEquals(test.Done, false);
		latch.Signal();
		IList<string> combined = test.join();
		assertEquals(test.Done, true);
		assertEquals(combined.Count, 2);
		assertEquals(combined[0], "A");
		assertEquals(combined[1], "B");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combineFuturesAsMap()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		return "B";
		});
		IDictionary<string, CompletableFuture<string>> input = ImmutableMap.of("a", future1, "b", future2);

		CompletableFuture<IDictionary<string, string>> test = Guavate.combineFuturesAsMap(input);

		assertEquals(test.Done, false);
		latch.Signal();
		IDictionary<string, string> combined = test.join();
		assertEquals(test.Done, true);
		assertEquals(combined.Count, 2);
		assertEquals(combined["a"], "A");
		assertEquals(combined["b"], "B");
	  }

	  public virtual void test_combineFuturesAsMap_exception()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		throw new System.InvalidOperationException("Oops");
		});
		IDictionary<string, CompletableFuture<string>> input = ImmutableMap.of("a", future1, "b", future2);

		CompletableFuture<IDictionary<string, string>> test = Guavate.combineFuturesAsMap(input);

		assertEquals(test.Done, false);
		latch.Signal();
		assertThrows(typeof(CompletionException), () => test.join());
		assertEquals(test.Done, true);
		assertEquals(test.CompletedExceptionally, true);
	  }

	  public virtual void test_toCombinedFutureMap()
	  {
		CompletableFuture<string> future1 = new CompletableFuture<string>();
		future1.complete("A");
		System.Threading.CountdownEvent latch = new System.Threading.CountdownEvent(1);
		CompletableFuture<string> future2 = CompletableFuture.supplyAsync(() =>
		{
		try
		{
			latch.await();
		}
		catch (InterruptedException)
		{
		}
		return "B";
		});
		IDictionary<string, CompletableFuture<string>> input = ImmutableMap.of("a", future1, "b", future2);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		CompletableFuture<IDictionary<string, string>> test = input.SetOfKeyValuePairs().collect(Guavate.toCombinedFutureMap());

		assertEquals(test.Done, false);
		latch.Signal();
		IDictionary<string, string> combined = test.join();
		assertEquals(test.Done, true);
		assertEquals(combined.Count, 2);
		assertEquals(combined["a"], "A");
		assertEquals(combined["b"], "B");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_poll()
	  {
		AtomicInteger counter = new AtomicInteger();
		System.Func<string> pollingFn = () =>
		{
	  switch (counter.incrementAndGet())
	  {
		case 1:
		  return null;
		case 2:
		  return "Yes";
		default:
		  throw new AssertionError("Test failed");
	  }
		};

		ScheduledExecutorService executor = Executors.newSingleThreadScheduledExecutor();

		CompletableFuture<string> future = Guavate.poll(executor, Duration.ofMillis(100), Duration.ofMillis(100), pollingFn);
		assertEquals(future.join(), "Yes");
	  }

	  public virtual void test_poll_exception()
	  {
		AtomicInteger counter = new AtomicInteger();
		System.Func<string> pollingFn = () =>
		{
	  switch (counter.incrementAndGet())
	  {
		case 1:
		  return null;
		case 2:
		  throw new System.InvalidOperationException("Expected");
		default:
		  throw new AssertionError("Test failed");
	  }
		};

		ScheduledExecutorService executor = Executors.newSingleThreadScheduledExecutor();
		try
		{
		  CompletableFuture<string> future = Guavate.poll(executor, Duration.ofMillis(100), Duration.ofMillis(100), pollingFn);
		  assertThrows(() => future.join(), typeof(CompletionException), "java.lang.IllegalStateException: Expected");
		}
		finally
		{
		  executor.shutdown();
		}
	  }

	  //-------------------------------------------------------------------------
	  private static void doNothing()
	  {
	  }

	  public virtual void test_namedThreadFactory()
	  {
		ThreadFactory threadFactory = Guavate.namedThreadFactory().build();
		assertEquals(threadFactory.newThread(() => doNothing()).Name, "GuavateTest-0");
	  }

	  public virtual void test_namedThreadFactory_prefix()
	  {
		ThreadFactory threadFactory = Guavate.namedThreadFactory("ThreadMaker").build();
		assertEquals(threadFactory.newThread(() => doNothing()).Name, "ThreadMaker-0");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_callerClass()
	  {
		assertEquals(Guavate.callerClass(0), typeof(Guavate.CallerClassSecurityManager));
		assertEquals(Guavate.callerClass(1), typeof(Guavate));
		assertEquals(Guavate.callerClass(2), typeof(GuavateTest));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_validUtilityClass()
	  {
		assertUtilityClass(typeof(Guavate));
	  }

	}

}