using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.entry;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.pairsToImmutableMap;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.list;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableMultimap = com.google.common.collect.ImmutableMultimap;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MapStreamTest
	public class MapStreamTest
	{

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly IDictionary<string, int> map_Renamed = ImmutableMap.of("one", 1, "two", 2, "three", 3, "four", 4);

	  //-------------------------------------------------------------------------
	  public virtual void keys()
	  {
		IList<string> result = MapStream.of(map_Renamed).keys().collect(toImmutableList());
		assertThat(result).isEqualTo(ImmutableList.of("one", "two", "three", "four"));
	  }

	  public virtual void values()
	  {
		IList<int> result = MapStream.of(map_Renamed).values().collect(toImmutableList());
		assertThat(result).isEqualTo(ImmutableList.of(1, 2, 3, 4));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void filter()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("one", 1, "two", 2);
		IDictionary<string, int> result = MapStream.of(map_Renamed).filter((k, v) => k.Equals("one") || v == 2).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void filterKeys()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("one", 1, "two", 2);
		IDictionary<string, int> result = MapStream.of(map_Renamed).filterKeys(k => k.length() == 3).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void filterKeys_byClass()
	  {
		IDictionary<Number, Number> map = ImmutableMap.of(1, 11, 2d, 22d, 3, 33d);
		IDictionary<int, Number> result = MapStream.of(map).filterKeys(typeof(Integer)).toMap();
		assertThat(result).isEqualTo(ImmutableMap.of(1, 11, 3, 33d));
	  }

	  public virtual void filterValues()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("one", 1, "two", 2);
		IDictionary<string, int> result = MapStream.of(map_Renamed).filterValues(v => v < 3).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void filterValues_byClass()
	  {
		IDictionary<Number, Number> map = ImmutableMap.of(1, 11, 2d, 22, 3, 33d);
		IDictionary<Number, int> result = MapStream.of(map).filterValues(typeof(Integer)).toMap();
		assertThat(result).isEqualTo(ImmutableMap.of(1, 11, 2d, 22));
	  }

	  public virtual void mapKeysToKeys()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("ONE", 1, "TWO", 2, "THREE", 3, "FOUR", 4);
		IDictionary<string, int> result = MapStream.of(map_Renamed).mapKeys(k => k.ToUpper(Locale.ENGLISH)).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void mapKeysAndValuesToKeys()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("one1", 1, "two2", 2, "three3", 3, "four4", 4);
		IDictionary<string, int> result = MapStream.of(map_Renamed).mapKeys((k, v) => k + v).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void mapValuesToValues()
	  {
		IDictionary<string, int> expected = ImmutableMap.of("one", 2, "two", 4, "three", 6, "four", 8);
		IDictionary<string, int> result = MapStream.of(map_Renamed).mapValues(v => v * 2).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void mapKeysAndValuesToValues()
	  {
		IDictionary<string, string> expected = ImmutableMap.of("one", "one1", "two", "two2", "three", "three3", "four", "four4");
		IDictionary<string, string> result = MapStream.of(map_Renamed).mapValues((k, v) => k + v).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void map()
	  {
		IList<string> expected = ImmutableList.of("one1", "two2", "three3", "four4");
		IList<string> result = MapStream.of(map_Renamed).map((k, v) => k + v).collect(toList());
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void flatMapKeysToKeys()
	  {
		IDictionary<string, int> expected = ImmutableMap.builder<string, int>().put("one", 1).put("ONE", 1).put("two", 2).put("TWO", 2).put("three", 3).put("THREE", 3).put("four", 4).put("FOUR", 4).build();

		ImmutableMap<string, int> result = MapStream.of(map_Renamed).flatMapKeys(key => Stream.of(key.ToLower(Locale.ENGLISH), key.ToUpper(Locale.ENGLISH))).toMap();

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void flatMapKeysAndValuesToKeys()
	  {
		IDictionary<string, int> expected = ImmutableMap.builder<string, int>().put("one", 1).put("1", 1).put("two", 2).put("2", 2).put("three", 3).put("3", 3).put("four", 4).put("4", 4).build();

		ImmutableMap<string, int> result = MapStream.of(map_Renamed).flatMapKeys((key, value) => Stream.of(key, Convert.ToString(value))).toMap();

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void flatMapValuesToValues()
	  {
		IList<Pair<string, int>> expected = ImmutableList.of(Pair.of("one", 1), Pair.of("one", 1), Pair.of("two", 2), Pair.of("two", 4), Pair.of("three", 3), Pair.of("three", 9), Pair.of("four", 4), Pair.of("four", 16));

		IList<Pair<string, int>> result = MapStream.of(map_Renamed).flatMapValues(value => Stream.of(value, value * value)).map((k, v) => Pair.of(k, v)).collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void flatMapKeysAndValuesToValues()
	  {
		IList<Pair<string, string>> expected = ImmutableList.of(Pair.of("one", "one"), Pair.of("one", "1"), Pair.of("two", "two"), Pair.of("two", "2"), Pair.of("three", "three"), Pair.of("three", "3"), Pair.of("four", "four"), Pair.of("four", "4"));

		IList<Pair<string, string>> result = MapStream.of(map_Renamed).flatMapValues((key, value) => Stream.of(key, Convert.ToString(value))).map((k, v) => Pair.of(k, v)).collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void flatMap()
	  {
		IDictionary<string, string> expected = ImmutableMap.builder<string, string>().put("one", "1").put("1", "one").put("two", "2").put("2", "two").put("three", "3").put("3", "three").put("four", "4").put("4", "four").build();

		IDictionary<string, string> result = MapStream.of(map_Renamed).flatMap((k, v) => Stream.of(Pair.of(k, Convert.ToString(v)), Pair.of(Convert.ToString(v), k))).collect(pairsToImmutableMap());

		assertThat(result).isEqualTo(expected);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void sortedKeys()
	  {
		IList<KeyValuePair<string, int>> expected = ImmutableList.of(entry("four", 4), entry("one", 1), entry("three", 3), entry("two", 2));

		IList<KeyValuePair<string, int>> result = MapStream.of(map_Renamed).sortedKeys().collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void sortedKeys_comparator()
	  {
		IList<KeyValuePair<string, int>> expected = ImmutableList.of(entry("two", 2), entry("three", 3), entry("one", 1), entry("four", 4));

		IList<KeyValuePair<string, int>> result = MapStream.of(map_Renamed).sortedKeys(System.Collections.IComparer.reverseOrder()).collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void sortedValues()
	  {
		ImmutableMap<string, int> invertedValuesMap = ImmutableMap.of("one", 4, "two", 3, "three", 2, "four", 1);

		IList<KeyValuePair<string, int>> expected = ImmutableList.of(entry("four", 1), entry("three", 2), entry("two", 3), entry("one", 4));

		IList<KeyValuePair<string, int>> result = MapStream.of(invertedValuesMap).sortedValues().collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void sortedValues_comparator()
	  {
		IList<KeyValuePair<string, int>> expected = ImmutableList.of(entry("four", 4), entry("three", 3), entry("two", 2), entry("one", 1));

		IList<KeyValuePair<string, int>> result = MapStream.of(map_Renamed).sortedValues(System.Collections.IComparer.reverseOrder()).collect(toList());

		assertThat(result).isEqualTo(expected);
	  }

	  //-----------------------------------------------------------------------
	  public virtual void minKeys()
	  {
		KeyValuePair<string, int> result = MapStream.of(map_Renamed).minKeys(System.Collections.IComparer.naturalOrder()).get();
		assertThat(result).isEqualTo(entry("four", 4));
	  }

	  public virtual void minValues()
	  {
		KeyValuePair<string, int> result = MapStream.of(map_Renamed).minValues(System.Collections.IComparer.naturalOrder()).get();
		assertThat(result).isEqualTo(entry("one", 1));
	  }

	  public virtual void maxKeys()
	  {
		KeyValuePair<string, int> result = MapStream.of(map_Renamed).maxKeys(System.Collections.IComparer.naturalOrder()).get();
		assertThat(result).isEqualTo(entry("two", 2));
	  }

	  public virtual void maxValues()
	  {
		KeyValuePair<string, int> result = MapStream.of(map_Renamed).maxValues(System.Collections.IComparer.naturalOrder()).get();
		assertThat(result).isEqualTo(entry("four", 4));
	  }

	  //-----------------------------------------------------------------------
	  public virtual void anyMatch()
	  {
		assertThat(MapStream.of(map_Renamed).anyMatch((key, value) => key.length() + value < 10)).True;
		assertThat(MapStream.of(map_Renamed).anyMatch((key, value) => key.length() + value < 8)).True;
		assertThat(MapStream.of(map_Renamed).anyMatch((key, value) => key.length() + value < 4)).False;
	  }

	  public virtual void allMatch()
	  {
		assertThat(MapStream.of(map_Renamed).allMatch((key, value) => key.length() + value < 10)).True;
		assertThat(MapStream.of(map_Renamed).allMatch((key, value) => key.length() + value < 8)).False;
		assertThat(MapStream.of(map_Renamed).allMatch((key, value) => key.length() + value < 4)).False;
	  }

	  public virtual void noneMatch()
	  {
		assertThat(MapStream.of(map_Renamed).noneMatch((key, value) => key.length() + value < 10)).False;
		assertThat(MapStream.of(map_Renamed).noneMatch((key, value) => key.length() + value < 8)).False;
		assertThat(MapStream.of(map_Renamed).noneMatch((key, value) => key.length() + value < 4)).True;
	  }

	  //-----------------------------------------------------------------------
	  public virtual void forEach()
	  {
		Dictionary<object, object> mutableMap = new Dictionary<object, object>();
		MapStream.of(map_Renamed).forEach((k, v) => mutableMap.put(k, v));
		assertThat(mutableMap).isEqualTo(map_Renamed);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void ofMultimap()
	  {
		ImmutableMultimap<string, int> input = ImmutableMultimap.of("one", 1, "two", 2, "one", 3);
		assertThat(MapStream.of(input)).containsExactlyInAnyOrder(entry("one", 1), entry("two", 2), entry("one", 3));
		assertThat(MapStream.of(input).toMap(int?.sum)).containsOnly(entry("one", 4), entry("two", 2));
	  }

	  public virtual void ofCollection()
	  {
		IList<string> letters = ImmutableList.of("a", "b", "c");
		IDictionary<string, string> expected = ImmutableMap.of("A", "a", "B", "b", "C", "c");
		IDictionary<string, string> result = MapStream.of(letters, letter => letter.ToUpper(Locale.ENGLISH)).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void ofCollection_2arg()
	  {
		IList<string> letters = ImmutableList.of("a", "b", "c");
		IDictionary<string, string> expected = ImmutableMap.of("A", "aa", "B", "bb", "C", "cc");
		IDictionary<string, string> result = MapStream.of(letters, letter => letter.ToUpper(Locale.ENGLISH), letter => letter + letter).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void ofStream()
	  {
		Stream<string> letters = Stream.of("a", "b", "c");
		IDictionary<string, string> expected = ImmutableMap.of("A", "a", "B", "b", "C", "c");
		IDictionary<string, string> result = MapStream.of(letters, letter => letter.ToUpper(Locale.ENGLISH)).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void ofStream_2arg()
	  {
		Stream<string> letters = Stream.of("a", "b", "c");
		IDictionary<string, string> expected = ImmutableMap.of("A", "aa", "B", "bb", "C", "cc");
		IDictionary<string, string> result = MapStream.of(letters, letter => letter.ToUpper(Locale.ENGLISH), letter => letter + letter).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void zip()
	  {
		Stream<int> numbers = Stream.of(0, 1, 2);
		Stream<string> letters = Stream.of("a", "b", "c");
		IDictionary<int, string> expected = ImmutableMap.of(0, "a", 1, "b", 2, "c");
		IDictionary<int, string> result = MapStream.zip(numbers, letters).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void zip_longerFirst()
	  {
		Stream<int> numbers = Stream.of(0, 1, 2, 3);
		Stream<string> letters = Stream.of("a", "b", "c");
		IDictionary<int, string> expected = ImmutableMap.of(0, "a", 1, "b", 2, "c");
		IDictionary<int, string> result = MapStream.zip(numbers, letters).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void zip_longerSecond()
	  {
		Stream<int> numbers = Stream.of(0, 1, 2);
		Stream<string> letters = Stream.of("a", "b", "c", "d");
		IDictionary<int, string> expected = ImmutableMap.of(0, "a", 1, "b", 2, "c");
		IDictionary<int, string> result = MapStream.zip(numbers, letters).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void zipWithIndex()
	  {
		Stream<string> letters = Stream.of("a", "b", "c");
		IDictionary<int, string> expected = ImmutableMap.of(0, "a", 1, "b", 2, "c");
		IDictionary<int, string> result = MapStream.zipWithIndex(letters).toMap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void concat()
	  {
		ImmutableMap<string, int> map1 = ImmutableMap.of("one", 1, "two", 2, "three", 3);
		ImmutableMap<string, int> map2 = ImmutableMap.of("three", 7, "four", 4);
		ImmutableMap<string, int> result = MapStream.concat(MapStream.of(map1), MapStream.of(map2)).toMap((a, b) => a);
		assertThat(result).isEqualTo(map_Renamed);
	  }

	  public virtual void concatGeneric()
	  {
		ImmutableMap<string, object> map1 = ImmutableMap.of("one", 1, "two", 2, "three", 3);
		ImmutableMap<object, int> map2 = ImmutableMap.of("three", 7, "four", 4);
		ImmutableMap<object, object> result = MapStream.concat(MapStream.of(map1), MapStream.of(map2)).toMap((a, b) => a);
		assertThat(result).isEqualTo(map_Renamed);
	  }

	  public virtual void concatNumberValues()
	  {
		ImmutableMap<string, double> map1 = ImmutableMap.of("one", 1D, "two", 2D, "three", 3D);
		ImmutableMap<object, int> map2 = ImmutableMap.of("three", 7, "four", 4);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.google.common.collect.ImmutableMap<Object, ? extends Number> result = MapStream.concat(MapStream.of(map1), MapStream.of(map2)).toMap((a, b) -> a);
		ImmutableMap<object, ? extends Number> result = MapStream.concat(MapStream.of(map1), MapStream.of(map2)).toMap((a, b) => a);
		assertThat(result).isEqualTo(ImmutableMap.of("one", 1D, "two", 2D, "three", 3D, "four", 4));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void toMapDuplicateKeys()
	  {
		assertThrowsIllegalArg(() => MapStream.of(map_Renamed).mapKeys(k => "key").toMap());
	  }

	  public virtual void toMapWithMerge()
	  {
		IDictionary<string, int> map = ImmutableMap.of("a", 1, "aa", 2, "b", 10, "bb", 20, "c", 1);
		IDictionary<string, int> expected = ImmutableMap.of("a", 3, "b", 30, "c", 1);
		IDictionary<string, int> result = MapStream.of(map).mapKeys(s => s.substring(0, 1)).toMap((v1, v2) => v1 + v2);
		assertThat(result).isEqualTo(expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void toMapGrouping()
	  {
		IDictionary<string, int> map = ImmutableMap.of("a", 1, "aa", 2, "b", 10, "bb", 20, "c", 1);
		IDictionary<string, IList<int>> expected = ImmutableMap.of("a", list(1, 2), "b", list(10, 20), "c", list(1));
		IDictionary<string, IList<int>> result = MapStream.of(map).mapKeys(s => s.substring(0, 1)).toMapGrouping();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void toMapGroupingWithCollector()
	  {
		IDictionary<string, int> map = ImmutableMap.of("a", 1, "aa", 2, "b", 10, "bb", 20, "c", 1);
		IDictionary<string, int> expected = ImmutableMap.of("a", 3, "b", 30, "c", 1);
		IDictionary<string, int> result = MapStream.of(map).mapKeys(s => s.substring(0, 1)).toMapGrouping(reducing(0, int?.sum));
		assertThat(result).isEqualTo(expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void toListMultimap()
	  {
		IDictionary<string, int> map = ImmutableMap.of("a", 1, "aa", 2, "b", 10, "bb", 20, "c", 1);
		ListMultimap<string, int> expected = ImmutableListMultimap.of("a", 1, "a", 2, "b", 10, "b", 20, "c", 1);
		ListMultimap<string, int> result = MapStream.of(map).mapKeys(s => s.substring(0, 1)).toListMultimap();
		assertThat(result).isEqualTo(expected);
	  }

	  public virtual void coverage()
	  {
		MapStream.empty().filter(e => false).distinct().sorted().sorted((e1, e2) => 0).peek(e => e.ToString()).limit(0).skip(0).sequential().parallel().unordered().onClose(() => Console.WriteLine()).close();
		MapStream.empty().anyMatch(e => true);
		MapStream.empty().allMatch(e => true);
		MapStream.empty().noneMatch(e => true);
		MapStream.empty().count();
		MapStream.empty().findAny();
		MapStream.empty().findFirst();
		MapStream.empty().max((e1, e2) => 0);
		MapStream.empty().min((e1, e2) => 0);
		MapStream.empty().GetEnumerator();
		MapStream.empty().spliterator();
		MapStream.empty().Parallel;
		MapStream.empty().map(e => e);
		MapStream.empty().mapToInt(e => 0);
		MapStream.empty().mapToLong(e => 0);
		MapStream.empty().mapToDouble(e => 0);
		MapStream.empty().flatMap(e => Stream.empty());
		MapStream.empty().flatMapToDouble(e => DoubleStream.empty());
		MapStream.empty().flatMapToInt(e => IntStream.empty());
		MapStream.empty().flatMapToLong(e => LongStream.empty());
		MapStream.empty().collect(toList());
		MapStream.empty().collect(() => null, (o, e) => Console.WriteLine(), (o1, o2) => Console.WriteLine());
		MapStream.empty().toArray();
		MapStream.empty().toArray(i => new object[0]);
		MapStream.empty().forEach(e => Console.WriteLine());
		MapStream.empty().forEachOrdered(e => Console.WriteLine());
		MapStream.empty().reduce(new AbstractMap.SimpleEntry<>(null, null), (o1, o2) => null);
		MapStream.empty().reduce((o1, o2) => null);
		MapStream.empty().reduce(null, (o, e) => null, (o1, o2) => null);
	  }
	}

}