using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.entry;


	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Multimap = com.google.common.collect.Multimap;
	using Streams = com.google.common.collect.Streams;

	/// <summary>
	/// A stream implementation based on {@code Map.Entry}.
	/// <para>
	/// This stream wraps a {@code Stream&lt;Map.Entry&gt;}, providing convenient methods for
	/// manipulating the keys and values. Unlike a {@code Map}, the keys in a {@code MapStream}
	/// do not have to be unique, although certain methods will fail if they are not unique.
	/// 
	/// </para>
	/// </summary>
	/// @param <K>  the key type </param>
	/// @param <V>  the value type </param>
	public sealed class MapStream<K, V> : Stream<KeyValuePair<K, V>>
	{

	  /// <summary>
	  /// The stream of map entries. </summary>
	  private readonly Stream<KeyValuePair<K, V>> underlying;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a stream over the entries in the map.
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="map">  the map to wrap </param>
	  /// <returns> a stream over the entries in the map </returns>
	  public static MapStream<K, V> of<K, V>(IDictionary<K, V> map)
	  {
		return new MapStream<K, V>(map.SetOfKeyValuePairs().stream());
	  }

	  /// <summary>
	  /// Returns a stream over all the entries in the multimap.
	  /// <para>
	  /// This will typically create a stream with duplicate keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="multimap">  the multimap to wrap </param>
	  /// <returns> a stream over the entries in the multimap </returns>
	  public static MapStream<K, V> of<K, V>(Multimap<K, V> multimap)
	  {
		return new MapStream<K, V>(multimap.entries().stream());
	  }

	  /// <summary>
	  /// Returns a stream of map entries where the values are taken from a collection
	  /// and the keys are created by applying a function to each value.
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="collection">  the collection of values </param>
	  /// <param name="keyFunction">  a function which returns the key for a value </param>
	  /// <returns> a stream of map entries derived from the values in the collection </returns>
	  public static MapStream<K, V> of<K, V>(ICollection<V> collection, System.Func<V, K> keyFunction)
	  {
		return of(collection.stream(), keyFunction);
	  }

	  /// <summary>
	  /// Returns a stream of map entries where the keys and values are extracted from a
	  /// collection by applying a function to each item in the collection.
	  /// </summary>
	  /// @param <T>  the collection type </param>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="collection">  the collection of values </param>
	  /// <param name="keyFunction">  a function which returns the key </param>
	  /// <param name="valueFunction">  a function which returns the value </param>
	  /// <returns> a stream of map entries derived from the collection </returns>
	  public static MapStream<K, V> of<T, K, V>(ICollection<T> collection, System.Func<T, K> keyFunction, System.Func<T, V> valueFunction)
	  {

		return of(collection.stream(), keyFunction, valueFunction);
	  }

	  /// <summary>
	  /// Returns a stream of map entries where the values are taken from a stream
	  /// and the keys are created by applying a function to each value.
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="stream">  the stream of values </param>
	  /// <param name="keyFunction">  a function which returns the key for a value </param>
	  /// <returns> a stream of map entries derived from the values in the stream </returns>
	  public static MapStream<K, V> of<K, V>(Stream<V> stream, System.Func<V, K> keyFunction)
	  {
		return new MapStream<K, V>(stream.map(v => entry(keyFunction(v), v)));
	  }

	  /// <summary>
	  /// Returns a stream of map entries where the keys and values are extracted from a
	  /// stream by applying a function to each item in the stream.
	  /// </summary>
	  /// @param <T>  the collection type </param>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="stream">  the stream of values </param>
	  /// <param name="keyFunction">  a function which returns the key for a value </param>
	  /// <param name="valueFunction">  a function which returns the value </param>
	  /// <returns> a stream of map entries derived from the stream </returns>
	  public static MapStream<K, V> of<T, K, V>(Stream<T> stream, System.Func<T, K> keyFunction, System.Func<T, V> valueFunction)
	  {

		return new MapStream<K, V>(stream.map(item => entry(keyFunction(item), valueFunction(item))));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a map stream that combines two other streams, continuing until either stream ends.
	  /// <para>
	  /// Note that this can produce a stream with non-unique keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <param name="keyStream">  the stream of keys </param>
	  /// <param name="valueStream">  the stream of values </param>
	  /// <returns> a stream of map entries derived from the stream </returns>
	  public static MapStream<K, V> zip<K, V>(Stream<K> keyStream, Stream<V> valueStream)
	  {
		return new MapStream<K, V>(Guavate.zip(keyStream, valueStream, Guavate.entry));
	  }

	  /// <summary>
	  /// Returns a stream of map entries where each key is the index of the value in the original stream.
	  /// </summary>
	  /// @param <V>  the value type </param>
	  /// <param name="stream">  the stream of values </param>
	  /// <returns> a stream of map entries derived from the stream </returns>
	  public static MapStream<int, V> zipWithIndex<V>(Stream<V> stream)
	  {
		Stream<KeyValuePair<int, V>> zipped = Streams.mapWithIndex(stream, (value, index) => entry(Math.toIntExact(index), value));
		return new MapStream<int, V>(zipped);
	  }

	  /// <summary>
	  /// Returns an empty map stream.
	  /// </summary>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <returns> an empty map stream </returns>
	  public static MapStream<K, V> empty<K, V>()
	  {
		return new MapStream<K, V>(Stream.empty());
	  }

	  /// <summary>
	  /// Creates a stream of map entries whose elements are those of the first stream followed by those of the second
	  /// stream.
	  /// </summary>
	  /// <param name="a">  the first stream of entries </param>
	  /// <param name="b">  the second stream of entries </param>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <returns> the concatenation of the two input streams </returns>
	  public static MapStream<K, V> concat<K, V, T1, T2>(MapStream<T1> a, MapStream<T2> b) where T1 : K where T2 : V
	  {

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") MapStream<K, V> kvMapStream = new MapStream<>(com.google.common.collect.Streams.concat((java.util.stream.Stream<? extends java.util.Map.Entry<K, V>>) a, (java.util.stream.Stream<? extends java.util.Map.Entry<K, V>>) b));
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		MapStream<K, V> kvMapStream = new MapStream<K, V>(Streams.concat((Stream<KeyValuePair<K, V>>) a, (Stream<KeyValuePair<K, V>>) b));
		return kvMapStream;
	  }

	  //-------------------------------------------------------------------------
	  // creates an instance
	  private MapStream(Stream<KeyValuePair<K, V>> underlying)
	  {
		this.underlying = underlying;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the keys as a stream, dropping the values.
	  /// <para>
	  /// A <seealso cref="MapStream"/> may contain the same key more than once, so callers
	  /// may need to call <seealso cref="Stream#distinct()"/> on the result.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a stream of the keys </returns>
	  public Stream<K> keys()
	  {
		return underlying.map(DictionaryEntry.getKey);
	  }

	  /// <summary>
	  /// Returns the values as a stream, dropping the keys.
	  /// </summary>
	  /// <returns> a stream of the values </returns>
	  public Stream<V> values()
	  {
		return underlying.map(DictionaryEntry.getValue);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Filters the stream by applying the predicate function to each key and value.
	  /// <para>
	  /// Entries are included in the returned stream if the predicate function returns true.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  a predicate function applied to each key and value in the stream </param>
	  /// <returns> a stream including the entries for which the predicate function returned true </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public MapStream<K, V> filter(java.util.function.BiFunction<? super K, ? super V, bool> predicate)
	  public MapStream<K, V> filter<T1>(System.Func<T1> predicate)
	  {
		return wrap(underlying.filter(e => predicate(e.Key, e.Value)));
	  }

	  /// <summary>
	  /// Filters the stream by applying the predicate function to each key.
	  /// <para>
	  /// Entries are included in the returned stream if the predicate function returns true.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  a predicate function applied to each key in the stream </param>
	  /// <returns> a stream including the entries for which the predicate function returned true </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public MapStream<K, V> filterKeys(java.util.function.Predicate<? super K> predicate)
	  public MapStream<K, V> filterKeys<T1>(System.Predicate<T1> predicate)
	  {
		return wrap(underlying.filter(e => predicate(e.Key)));
	  }

	  /// <summary>
	  /// Filters the stream checking the type of each key.
	  /// <para>
	  /// Entries are included in the returned stream if the key is an instance of the specified type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type to filter to </param>
	  /// <param name="castToClass">  the class to filter the keys to </param>
	  /// <returns> a stream including only those entries where the key is an instance of the specified type </returns>
	  public MapStream<R, V> filterKeys<R>(Type<R> castToClass)
	  {
		return wrap(underlying.filter(e => castToClass.IsInstanceOfType(e.Key)).map(e => entry(castToClass.cast(e.Key), e.Value)));
	  }

	  /// <summary>
	  /// Filters the stream by applying the predicate function to each value.
	  /// <para>
	  /// Entries are included in the returned stream if the predicate function returns true.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  a predicate function applied to each value in the stream </param>
	  /// <returns> a stream including the entries for which the predicate function returned true </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public MapStream<K, V> filterValues(java.util.function.Predicate<? super V> predicate)
	  public MapStream<K, V> filterValues<T1>(System.Predicate<T1> predicate)
	  {
		return wrap(underlying.filter(e => predicate(e.Value)));
	  }

	  /// <summary>
	  /// Filters the stream checking the type of each value.
	  /// <para>
	  /// Entries are included in the returned stream if the value is an instance of the specified type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type to filter to </param>
	  /// <param name="castToClass">  the class to filter the values to </param>
	  /// <returns> a stream including only those entries where the value is an instance of the specified type </returns>
	  public MapStream<K, R> filterValues<R>(Type<R> castToClass)
	  {
		return wrap(underlying.filter(e => castToClass.IsInstanceOfType(e.Value)).map(e => entry(e.Key, castToClass.cast(e.Value))));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Transforms the keys in the stream by applying a mapper function to each key.
	  /// <para>
	  /// The values are unchanged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return value is used as the new key </param>
	  /// @param <R>  the type of the new keys </param>
	  /// <returns> a stream of entries with the keys transformed and the values unchanged </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<R, V> mapKeys(java.util.function.Function<? super K, ? extends R> mapper)
	  public MapStream<R, V> mapKeys<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return wrap(underlying.map(e => entry(mapper(e.Key), e.Value)));
	  }

	  /// <summary>
	  /// Transforms the keys in the stream by applying a mapper function to each key and value.
	  /// <para>
	  /// The values are unchanged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return value is used as the new key </param>
	  /// @param <R>  the type of the new keys </param>
	  /// <returns> a stream of entries with the keys transformed and the values unchanged </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<R, V> mapKeys(java.util.function.BiFunction<? super K, ? super V, ? extends R> mapper)
	  public MapStream<R, V> mapKeys<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return wrap(underlying.map(e => entry(mapper(e.Key, e.Value), e.Value)));
	  }

	  /// <summary>
	  /// Transforms the values in the stream by applying a mapper function to each value.
	  /// <para>
	  /// The keys are unchanged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return value is used as the new value </param>
	  /// @param <R>  the type of the new values </param>
	  /// <returns> a stream of entries with the values transformed and the keys unchanged </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<K, R> mapValues(java.util.function.Function<? super V, ? extends R> mapper)
	  public MapStream<K, R> mapValues<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return wrap(underlying.map(e => entry(e.Key, mapper(e.Value))));
	  }

	  /// <summary>
	  /// Transforms the values in the stream by applying a mapper function to each key and value.
	  /// <para>
	  /// The keys are unchanged.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return value is used as the new value </param>
	  /// @param <R>  the type of the new values </param>
	  /// <returns> a stream of entries with the values transformed and the keys unchanged </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<K, R> mapValues(java.util.function.BiFunction<? super K, ? super V, ? extends R> mapper)
	  public MapStream<K, R> mapValues<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return wrap(underlying.map(e => entry(e.Key, mapper(e.Key, e.Value))));
	  }

	  /// <summary>
	  /// Transforms the entries in the stream by applying a mapper function to each key and value.
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are included in the new stream </param>
	  /// @param <R>  the type of elements in the new stream </param>
	  /// <returns> a stream containing the values returned from the mapper function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> java.util.stream.Stream<R> map(java.util.function.BiFunction<? super K, ? super V, ? extends R> mapper)
	  public Stream<R> map<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return underlying.map(e => mapper(e.Key, e.Value));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Transforms the keys in the stream by applying a mapper function to each key.
	  /// <para>
	  /// The new keys produced will be associated with the original value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are the keys in the new stream </param>
	  /// @param <R>  the type of the new keys </param>
	  /// <returns> a stream of entries with new keys from the mapper function assigned to the values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<R, V> flatMapKeys(java.util.function.Function<? super K, java.util.stream.Stream<R>> mapper)
	  public MapStream<R, V> flatMapKeys<R, T1>(System.Func<T1> mapper)
	  {
		return wrap(underlying.flatMap(e => mapper(e.Key).map(newKey => entry(newKey, e.Value))));
	  }

	  /// <summary>
	  /// Transforms the keys in the stream by applying a mapper function to each key and value.
	  /// <para>
	  /// The new keys produced will be associated with the original value.
	  /// </para>
	  /// <para>
	  /// For example this could turn a {@code MapStream<List<String>, LocalDate>} into a
	  /// {@code MapStream<String, LocalDate>}
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are the keys in the new stream </param>
	  /// @param <R>  the type of the new keys </param>
	  /// <returns> a stream of entries with new keys from the mapper function assigned to the values </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<R, V> flatMapKeys(java.util.function.BiFunction<? super K, ? super V, java.util.stream.Stream<R>> mapper)
	  public MapStream<R, V> flatMapKeys<R, T1>(System.Func<T1> mapper)
	  {
		return wrap(underlying.flatMap(e => mapper(e.Key, e.Value).map(newKey => entry(newKey, e.Value))));
	  }

	  /// <summary>
	  /// Transforms the values in the stream by applying a mapper function to each value.
	  /// <para>
	  /// The new values produced will be associated with the original key.
	  /// </para>
	  /// <para>
	  /// For example this could turn a {@code MapStream<LocalDate, List<String>>} into a
	  /// {@code MapStream<LocalDate, String>}
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are the values in the new stream </param>
	  /// @param <R>  the type of the new values </param>
	  /// <returns> a stream of entries with new values from the mapper function assigned to the keys </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<K, R> flatMapValues(java.util.function.Function<? super V, java.util.stream.Stream<R>> mapper)
	  public MapStream<K, R> flatMapValues<R, T1>(System.Func<T1> mapper)
	  {
		return wrap(underlying.flatMap(e => mapper(e.Value).map(newValue => entry(e.Key, newValue))));
	  }

	  /// <summary>
	  /// Transforms the values in the stream by applying a mapper function to each key and value.
	  /// <para>
	  /// The new values produced will be associated with the original key.
	  /// </para>
	  /// <para>
	  /// For example this could turn a {@code MapStream<LocalDate, List<String>>} into a
	  /// {@code MapStream<LocalDate, String>}
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are the values in the new stream </param>
	  /// @param <R>  the type of the new values </param>
	  /// <returns> a stream of entries with new values from the mapper function assigned to the keys </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> MapStream<K, R> flatMapValues(java.util.function.BiFunction<? super K, ? super V, java.util.stream.Stream<R>> mapper)
	  public MapStream<K, R> flatMapValues<R, T1>(System.Func<T1> mapper)
	  {
		return wrap(underlying.flatMap(e => mapper(e.Key, e.Value).map(newValue => entry(e.Key, newValue))));
	  }

	  /// <summary>
	  /// Transforms the entries in the stream by applying a mapper function to each key and value to produce a stream of
	  /// elements, and then flattening the resulting stream of streams.
	  /// </summary>
	  /// <param name="mapper">  a mapper function whose return values are included in the new stream </param>
	  /// @param <R>  the type of the elements in the new stream </param>
	  /// <returns> a stream containing the values returned from the mapper function </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <R> java.util.stream.Stream<R> flatMap(java.util.function.BiFunction<? super K, ? super V, java.util.stream.Stream<R>> mapper)
	  public Stream<R> flatMap<R, T1>(System.Func<T1> mapper)
	  {
		return underlying.flatMap(e => mapper(e.Key, e.Value));
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sorts the entries in the stream by comparing the keys using their natural ordering.
	  /// <para>
	  /// If the keys in this map stream are not {@code Comparable} a {@code java.lang.ClassCastException} may be thrown.
	  /// In this case use <seealso cref="#sortedKeys(Comparator)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the sorted stream </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public MapStream<K, V> sortedKeys()
	  public MapStream<K, V> sortedKeys()
	  {
		IComparer<K> comparator = (IComparer<K>) System.Collections.IComparer.naturalOrder();
		return wrap(underlying.sorted((e1, e2) => comparator.Compare(e1.Key, e2.Key)));
	  }

	  /// <summary>
	  /// Sorts the entries in the stream by comparing the keys using the supplied comparator.
	  /// </summary>
	  /// <param name="comparator">  a comparator of keys </param>
	  /// <returns> the sorted stream </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public MapStream<K, V> sortedKeys(java.util.Comparator<? super K> comparator)
	  public MapStream<K, V> sortedKeys<T1>(IComparer<T1> comparator)
	  {
		return wrap(underlying.sorted((e1, e2) => comparator.Compare(e1.Key, e2.Key)));
	  }

	  /// <summary>
	  /// Sorts the entries in the stream by comparing the values using their natural ordering.
	  /// <para>
	  /// If the values in this map stream are not {@code Comparable} a {@code java.lang.ClassCastException} may be thrown.
	  /// In this case use <seealso cref="#sortedValues(Comparator)"/> instead.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the sorted stream </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public MapStream<K, V> sortedValues()
	  public MapStream<K, V> sortedValues()
	  {
		IComparer<V> comparator = (IComparer<V>) System.Collections.IComparer.naturalOrder();
		return wrap(underlying.sorted((e1, e2) => comparator.Compare(e1.Value, e2.Value)));
	  }

	  /// <summary>
	  /// Sorts the entries in the stream by comparing the values using the supplied comparator.
	  /// </summary>
	  /// <param name="comparator">  a comparator of values </param>
	  /// <returns> the sorted stream </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public MapStream<K, V> sortedValues(java.util.Comparator<? super V> comparator)
	  public MapStream<K, V> sortedValues<T1>(IComparer<T1> comparator)
	  {
		return wrap(underlying.sorted((e1, e2) => comparator.Compare(e1.Value, e2.Value)));
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Finds the minimum entry in the stream by comparing the keys using the supplied comparator.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="comparator">  a comparator of keys </param>
	  /// <returns> the minimum entry </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public java.util.Optional<java.util.Map.Entry<K, V>> minKeys(java.util.Comparator<? super K> comparator)
	  public Optional<KeyValuePair<K, V>> minKeys<T1>(IComparer<T1> comparator)
	  {
		return underlying.min((e1, e2) => comparator.Compare(e1.Key, e2.Key));
	  }

	  /// <summary>
	  /// Finds the minimum entry in the stream by comparing the values using the supplied comparator.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="comparator">  a comparator of values </param>
	  /// <returns> the minimum entry </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public java.util.Optional<java.util.Map.Entry<K, V>> minValues(java.util.Comparator<? super V> comparator)
	  public Optional<KeyValuePair<K, V>> minValues<T1>(IComparer<T1> comparator)
	  {
		return underlying.min((e1, e2) => comparator.Compare(e1.Value, e2.Value));
	  }

	  /// <summary>
	  /// Finds the maximum entry in the stream by comparing the keys using the supplied comparator.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="comparator">  a comparator of keys </param>
	  /// <returns> the maximum entry </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public java.util.Optional<java.util.Map.Entry<K, V>> maxKeys(java.util.Comparator<? super K> comparator)
	  public Optional<KeyValuePair<K, V>> maxKeys<T1>(IComparer<T1> comparator)
	  {
		return underlying.max((e1, e2) => comparator.Compare(e1.Key, e2.Key));
	  }

	  /// <summary>
	  /// Finds the maximum entry in the stream by comparing the values using the supplied comparator.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="comparator">  a comparator of values </param>
	  /// <returns> the maximum entry </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public java.util.Optional<java.util.Map.Entry<K, V>> maxValues(java.util.Comparator<? super V> comparator)
	  public Optional<KeyValuePair<K, V>> maxValues<T1>(IComparer<T1> comparator)
	  {
		return underlying.max((e1, e2) => comparator.Compare(e1.Value, e2.Value));
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns whether any elements of this stream match the provided predicate.
	  /// <para>
	  /// This is a short-circuiting terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  the predicate to apply to the entries </param>
	  /// <returns> whether any of the entries matched the predicate </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public boolean anyMatch(java.util.function.BiPredicate<? super K, ? super V> predicate)
	  public bool anyMatch<T1>(System.Func<T1> predicate)
	  {
		return underlying.anyMatch(e => predicate(e.Key, e.Value));
	  }

	  /// <summary>
	  /// Returns whether all elements of this stream match the provided predicate.
	  /// <para>
	  /// This is a short-circuiting terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  the predicate to apply to the entries </param>
	  /// <returns> whether all of the entries matched the predicate </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public boolean allMatch(java.util.function.BiPredicate<? super K, ? super V> predicate)
	  public bool allMatch<T1>(System.Func<T1> predicate)
	  {
		return underlying.allMatch(e => predicate(e.Key, e.Value));
	  }

	  /// <summary>
	  /// Returns whether no elements of this stream match the provided predicate.
	  /// <para>
	  /// This is a short-circuiting terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="predicate">  the predicate to apply to the entries </param>
	  /// <returns> whether none of the entries matched the predicate </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public boolean noneMatch(java.util.function.BiPredicate<? super K, ? super V> predicate)
	  public bool noneMatch<T1>(System.Func<T1> predicate)
	  {
		return underlying.noneMatch(e => predicate(e.Key, e.Value));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an immutable map built from the entries in the stream.
	  /// <para>
	  /// The keys must be unique or an exception will be thrown.
	  /// Duplicate keys can be handled using <seealso cref="#toMap(BiFunction)"/>.
	  /// </para>
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable map built from the entries in the stream </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key occurs more than once </exception>
	  public ImmutableMap<K, V> toMap()
	  {
		return underlying.collect(Guavate.toImmutableMap(DictionaryEntry.getKey, DictionaryEntry.getValue));
	  }

	  /// <summary>
	  /// Returns an immutable map built from the entries in the stream.
	  /// <para>
	  /// If the same key maps to multiple values the merge function is invoked with both values and the return
	  /// value is used in the map.
	  /// </para>
	  /// <para>
	  /// Can be used with <seealso cref="#concat(MapStream, MapStream)"/> to merge immutable
	  /// maps with duplicate keys.
	  /// </para>
	  /// <para>
	  /// For example, to merge immutable maps with duplicate keys preferring values in the first map:
	  /// <pre>
	  ///   MapStream.concat(mapStreamA, mapStreamB).toMap((a,b) -> a);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mergeFn">  function used to merge values when the same key appears multiple times in the stream </param>
	  /// <returns> an immutable map built from the entries in the stream </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<K, V> toMap(java.util.function.BiFunction<? super V, ? super V, ? extends V> mergeFn)
	  public ImmutableMap<K, V> toMap<T1>(System.Func<T1> mergeFn) where T1 : V
	  {
		return underlying.collect(Guavate.toImmutableMap(DictionaryEntry.getKey, DictionaryEntry.getValue, mergeFn));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an immutable map built from the entries in the stream, grouping by key.
	  /// <para>
	  /// Entries are grouped based on the equality of the key.
	  /// </para>
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable map built from the entries in the stream </returns>
	  public ImmutableMap<K, IList<V>> toMapGrouping()
	  {
		return toMapGrouping(toList());
	  }

	  /// <summary>
	  /// Returns an immutable map built from the entries in the stream, grouping by key.
	  /// <para>
	  /// Entries are grouped based on the equality of the key.
	  /// The collector allows the values to be flexibly combined.
	  /// </para>
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <A>  the internal collector type </param>
	  /// @param <R>  the type of the combined values </param>
	  /// <param name="valueCollector">  the collector used to combined the values </param>
	  /// <returns> a stream where the values have been grouped </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public <A, R> com.google.common.collect.ImmutableMap<K, R> toMapGrouping(java.util.stream.Collector<? super V, A, R> valueCollector)
	  public ImmutableMap<K, R> toMapGrouping<A, R, T1>(Collector<T1> valueCollector)
	  {
		return underlying.collect(collectingAndThen(groupingBy(DictionaryEntry.getKey, mapping(DictionaryEntry.getValue, valueCollector)), ImmutableMap.copyOf));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an immutable list multimap built from the entries in the stream.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an immutable list multimap built from the entries in the stream </returns>
	  public ImmutableListMultimap<K, V> toListMultimap()
	  {
		return underlying.collect(Guavate.toImmutableListMultimap(DictionaryEntry.getKey, DictionaryEntry.getValue));
	  }

	  /// <summary>
	  /// Performs an action for each entry in the stream, passing the key and value to the action.
	  /// <para>
	  /// This is a terminal operation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="action">  an action performed for each entry in the stream </param>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public void forEach(java.util.function.BiConsumer<? super K, ? super V> action)
	  public void forEach<T1>(System.Action<T1> action)
	  {
		underlying.forEach(e => action(e.Key, e.Value));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public MapStream<K, V> filter(System.Predicate<? super java.util.Map.Entry<K, V>> predicate)
	  public override MapStream<K, V> filter<T1>(System.Predicate<T1> predicate)
	  {
		return wrap(underlying.filter(predicate));
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <R> java.util.stream.Stream<R> map(System.Func<? super java.util.Map.Entry<K, V>, ? extends R> mapper)
	  public override Stream<R> map<R, T1>(System.Func<T1> mapper) where T1 : R
	  {
		return underlying.map(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.IntStream mapToInt(System.Func<? super java.util.Map.Entry<K, V>, int> mapper)
	  public override IntStream mapToInt<T1>(System.Func<T1> mapper)
	  {
		return underlying.mapToInt(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.LongStream mapToLong(System.Func<? super java.util.Map.Entry<K, V>, long> mapper)
	  public override LongStream mapToLong<T1>(System.Func<T1> mapper)
	  {
		return underlying.mapToLong(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.DoubleStream mapToDouble(System.Func<? super java.util.Map.Entry<K, V>, double> mapper)
	  public override DoubleStream mapToDouble<T1>(System.Func<T1> mapper)
	  {
		return underlying.mapToDouble(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <R> java.util.stream.Stream<R> flatMap(System.Func<? super java.util.Map.Entry<K, V>, ? extends java.util.stream.Stream<? extends R>> mapper)
	  public override Stream<R> flatMap<R, T1>(System.Func<T1> mapper) where T1 : java.util.stream.Stream<T1 extends R>
	  {
		return underlying.flatMap(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.IntStream flatMapToInt(System.Func<? super java.util.Map.Entry<K, V>, ? extends java.util.stream.IntStream> mapper)
	  public override IntStream flatMapToInt<T1>(System.Func<T1> mapper) where T1 : java.util.stream.IntStream
	  {
		return underlying.flatMapToInt(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.LongStream flatMapToLong(System.Func<? super java.util.Map.Entry<K, V>, ? extends java.util.stream.LongStream> mapper)
	  public override LongStream flatMapToLong<T1>(System.Func<T1> mapper) where T1 : java.util.stream.LongStream
	  {
		return underlying.flatMapToLong(mapper);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.stream.DoubleStream flatMapToDouble(System.Func<? super java.util.Map.Entry<K, V>, ? extends java.util.stream.DoubleStream> mapper)
	  public override DoubleStream flatMapToDouble<T1>(System.Func<T1> mapper) where T1 : java.util.stream.DoubleStream
	  {
		return underlying.flatMapToDouble(mapper);
	  }

	  public override MapStream<K, V> distinct()
	  {
		return wrap(underlying.distinct());
	  }

	  public override MapStream<K, V> sorted()
	  {
		return wrap(underlying.sorted());
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public MapStream<K, V> sorted(java.util.Comparator<? super java.util.Map.Entry<K, V>> comparator)
	  public override MapStream<K, V> sorted<T1>(IComparer<T1> comparator)
	  {
		return wrap(underlying.sorted(comparator));
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public MapStream<K, V> peek(System.Action<? super java.util.Map.Entry<K, V>> action)
	  public override MapStream<K, V> peek<T1>(System.Action<T1> action)
	  {
		return wrap(underlying.peek(action));
	  }

	  public override MapStream<K, V> limit(long maxSize)
	  {
		return wrap(underlying.limit(maxSize));
	  }

	  public override MapStream<K, V> skip(long n)
	  {
		return wrap(underlying.skip(n));
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public void forEach(System.Action<? super java.util.Map.Entry<K, V>> action)
	  public override void forEach<T1>(System.Action<T1> action)
	  {
		underlying.forEach(action);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public void forEachOrdered(System.Action<? super java.util.Map.Entry<K, V>> action)
	  public override void forEachOrdered<T1>(System.Action<T1> action)
	  {
		underlying.forEachOrdered(action);
	  }

	  public override object[] toArray()
	  {
		return underlying.toArray();
	  }

	  public override A[] toArray<A>(System.Func<int, A[]> generator)
	  {
		return underlying.toArray(generator);
	  }

	  public override KeyValuePair<K, V> reduce(KeyValuePair<K, V> identity, System.Func<KeyValuePair<K, V>, KeyValuePair<K, V>, KeyValuePair<K, V>> accumulator)
	  {
		return underlying.reduce(identity, accumulator);
	  }

	  public override Optional<KeyValuePair<K, V>> reduce(System.Func<KeyValuePair<K, V>, KeyValuePair<K, V>, KeyValuePair<K, V>> accumulator)
	  {
		return underlying.reduce(accumulator);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <U> U reduce(U identity, System.Func<U, ? super java.util.Map.Entry<K, V>, U> accumulator, System.Func<U, U, U> combiner)
	  public override U reduce<U, T1>(U identity, System.Func<T1> accumulator, System.Func<U, U, U> combiner)
	  {
		return underlying.reduce(identity, accumulator, combiner);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <R> R collect(System.Func<R> supplier, System.Action<R, ? super java.util.Map.Entry<K, V>> accumulator, System.Action<R, R> combiner)
	  public override R collect<R, T1>(System.Func<R> supplier, System.Action<T1> accumulator, System.Action<R, R> combiner)
	  {

		return underlying.collect(supplier, accumulator, combiner);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <R, A> R collect(java.util.stream.Collector<? super java.util.Map.Entry<K, V>, A, R> collector)
	  public override R collect<R, A, T1>(Collector<T1> collector)
	  {
		return underlying.collect(collector);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.Optional<java.util.Map.Entry<K, V>> min(java.util.Comparator<? super java.util.Map.Entry<K, V>> comparator)
	  public override Optional<KeyValuePair<K, V>> min<T1>(IComparer<T1> comparator)
	  {
		return underlying.min(comparator);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public java.util.Optional<java.util.Map.Entry<K, V>> max(java.util.Comparator<? super java.util.Map.Entry<K, V>> comparator)
	  public override Optional<KeyValuePair<K, V>> max<T1>(IComparer<T1> comparator)
	  {
		return underlying.max(comparator);
	  }

	  public override long count()
	  {
		return underlying.count();
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public boolean anyMatch(System.Predicate<? super java.util.Map.Entry<K, V>> predicate)
	  public override bool anyMatch<T1>(System.Predicate<T1> predicate)
	  {
		return underlying.anyMatch(predicate);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public boolean allMatch(System.Predicate<? super java.util.Map.Entry<K, V>> predicate)
	  public override bool allMatch<T1>(System.Predicate<T1> predicate)
	  {
		return underlying.allMatch(predicate);
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public boolean noneMatch(System.Predicate<? super java.util.Map.Entry<K, V>> predicate)
	  public override bool noneMatch<T1>(System.Predicate<T1> predicate)
	  {
		return underlying.noneMatch(predicate);
	  }

	  public override Optional<KeyValuePair<K, V>> findFirst()
	  {
		return underlying.findFirst();
	  }

	  public override Optional<KeyValuePair<K, V>> findAny()
	  {
		return underlying.findAny();
	  }

	  public override IEnumerator<KeyValuePair<K, V>> iterator()
	  {
		return underlying.GetEnumerator();
	  }

	  public override Spliterator<KeyValuePair<K, V>> spliterator()
	  {
		return underlying.spliterator();
	  }

	  public override bool Parallel
	  {
		  get
		  {
			return underlying.Parallel;
		  }
	  }

	  public override MapStream<K, V> sequential()
	  {
		return wrap(underlying.sequential());
	  }

	  public override MapStream<K, V> parallel()
	  {
		return wrap(underlying.parallel());
	  }

	  public override MapStream<K, V> unordered()
	  {
		return wrap(underlying.unordered());
	  }

	  public override MapStream<K, V> onClose(ThreadStart closeHandler)
	  {
		return wrap(underlying.onClose(closeHandler));
	  }

	  public override void close()
	  {
		underlying.close();
	  }

	  //-------------------------------------------------------------------------
	  private static MapStream<K, V> wrap<K, V>(Stream<KeyValuePair<K, V>> underlying)
	  {
		return new MapStream<K, V>(underlying);
	  }

	}

}