using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{


	using FluentIterable = com.google.common.collect.FluentIterable;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableMultiset = com.google.common.collect.ImmutableMultiset;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ImmutableSetMultimap = com.google.common.collect.ImmutableSetMultimap;
	using ImmutableSortedMap = com.google.common.collect.ImmutableSortedMap;
	using ImmutableSortedSet = com.google.common.collect.ImmutableSortedSet;
	using Iterables = com.google.common.collect.Iterables;
	using Lists = com.google.common.collect.Lists;
	using ThreadFactoryBuilder = com.google.common.util.concurrent.ThreadFactoryBuilder;
	using ObjIntPair = com.opengamma.strata.collect.tuple.ObjIntPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Utilities that help bridge the gap between Java 8 and Google Guava.
	/// <para>
	/// Guava has the <seealso cref="FluentIterable"/> concept which is similar to streams.
	/// In many ways, fluent iterable is nicer, because it directly binds to the
	/// immutable collection classes. However, on balance it seems wise to use
	/// the stream API rather than {@code FluentIterable} in Java 8.
	/// </para>
	/// </summary>
	public sealed class Guavate
	{

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private Guavate()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Concatenates a number of iterables into a single list.
	  /// <para>
	  /// This is harder than it should be, a method {@code Stream.of(Iterable)}
	  /// would have been appropriate, but cannot be added now.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the iterable </param>
	  /// <param name="iterables">  the iterables to combine </param>
	  /// <returns> the list that combines the inputs </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public static <T> com.google.common.collect.ImmutableList<T> concatToList(Iterable<? extends T>... iterables)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static ImmutableList<T> concatToList<T>(params IEnumerable<T>[] iterables)
	  {
		return ImmutableList.copyOf(Iterables.concat(iterables));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Uses a number of suppliers to create a single optional result.
	  /// <para>
	  /// This invokes each supplier in turn until a non empty optional is returned.
	  /// As such, not all suppliers are necessarily invoked.
	  /// </para>
	  /// <para>
	  /// The Java 8 <seealso cref="Optional"/> class does not have an {@code or} method,
	  /// so this provides an alternative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the optional </param>
	  /// <param name="suppliers">  the suppliers to combine </param>
	  /// <returns> the first non empty optional </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @SafeVarargs public static <T> java.util.Optional<T> firstNonEmpty(System.Func<java.util.Optional<? extends T>>... suppliers)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Optional<T> firstNonEmpty<T>(params System.Func<Optional<T>>[] suppliers)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.function.Supplier<java.util.Optional<? extends T>> supplier : suppliers)
		foreach (System.Func<Optional<T>> supplier in suppliers)
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Optional<? extends T> result = supplier.get();
		  Optional<T> result = supplier();
		  if (result.Present)
		  {
			// safe, because Optional is read-only
			return (Optional<T>) result;
		  }
		}
		return null;
	  }

	  /// <summary>
	  /// Chooses the first optional that is not empty.
	  /// <para>
	  /// The Java 8 <seealso cref="Optional"/> class does not have an {@code or} method,
	  /// so this provides an alternative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the optional </param>
	  /// <param name="optionals">  the optionals to combine </param>
	  /// <returns> the first non empty optional </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @SafeVarargs public static <T> java.util.Optional<T> firstNonEmpty(java.util.Optional<? extends T>... optionals)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Optional<T> firstNonEmpty<T>(params Optional<T>[] optionals)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Optional<? extends T> optional : optionals)
		foreach (Optional<T> optional in optionals)
		{
		  if (optional.Present)
		  {
			// safe, because Optional is read-only
			return (Optional<T>) optional;
		  }
		}
		return null;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a single {@code Map.Entry}.
	  /// <para>
	  /// The entry is immutable.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K>  the type of the key </param>
	  /// @param <V>  the type of the value </param>
	  /// <param name="key">  the key </param>
	  /// <param name="value">  the value </param>
	  /// <returns> the map entry </returns>
	  public static KeyValuePair<K, V> entry<K, V>(K key, V value)
	  {
		return new AbstractMap.SimpleImmutableEntry<>(key, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts an iterable to a serial stream.
	  /// <para>
	  /// This is harder than it should be, a method {@code Stream.of(Iterable)}
	  /// would have been appropriate, but cannot be added now.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the iterable </param>
	  /// <param name="iterable">  the iterable to convert </param>
	  /// <returns> a stream of the elements in the iterable </returns>
	  public static Stream<T> stream<T>(IEnumerable<T> iterable)
	  {
		return StreamSupport.stream(iterable.spliterator(), false);
	  }

	  /// <summary>
	  /// Converts an optional to a stream with zero or one elements.
	  /// </summary>
	  /// @param <T>  the type of optional element </param>
	  /// <param name="optional">  the optional </param>
	  /// <returns> a stream containing a single value if the optional has a value, else a stream with no values. </returns>
	  public static Stream<T> stream<T>(Optional<T> optional)
	  {
		return optional.Present ? Stream.of(optional.get()) : Stream.empty();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a stream that wraps a stream with the index.
	  /// <para>
	  /// Each input object is decorated with an <seealso cref="ObjIntPair"/>.
	  /// The {@code int} is the index of the element in the stream.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the stream </param>
	  /// <param name="stream">  the stream to index </param>
	  /// <returns> a stream of pairs, containing the element and index </returns>
	  public static Stream<ObjIntPair<T>> zipWithIndex<T>(Stream<T> stream)
	  {
		Spliterator<T> split1 = stream.spliterator();
		IEnumerator<T> it1 = Spliterators.iterator(split1);
		IEnumerator<ObjIntPair<T>> it = new IteratorAnonymousInnerClass(it1);
		Spliterator<ObjIntPair<T>> split = Spliterators.spliterator(it, split1.ExactSizeIfKnown, split1.characteristics());
		return StreamSupport.stream(split, false);
	  }

	  private class IteratorAnonymousInnerClass : IEnumerator<ObjIntPair<T>>
	  {
		  private IEnumerator<T> it1;

		  public IteratorAnonymousInnerClass(IEnumerator<T> it1)
		  {
			  this.it1 = it1;
		  }

		  private int index;

		  public bool hasNext()
		  {
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			return it1.hasNext();
		  }

		  public ObjIntPair<T> next()
		  {
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			return ObjIntPair.of(it1.next(), index++);
		  }
	  }

	  /// <summary>
	  /// Creates a stream that combines two other streams, continuing until either stream ends.
	  /// <para>
	  /// Each pair of input objects is combined into a <seealso cref="Pair"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <A>  the type of the first stream </param>
	  /// @param <B>  the type of the second stream </param>
	  /// <param name="stream1">  the first stream </param>
	  /// <param name="stream2">  the first stream </param>
	  /// <returns> a stream of pairs, one from each stream </returns>
	  public static Stream<Pair<A, B>> zip<A, B>(Stream<A> stream1, Stream<B> stream2)
	  {
		return zip(stream1, stream2, (a, b) => Pair.of(a, b));
	  }

	  /// <summary>
	  /// Creates a stream that combines two other streams, continuing until either stream ends.
	  /// <para>
	  /// The combiner function is called once for each pair of objects found in the input streams.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <A>  the type of the first stream </param>
	  /// @param <B>  the type of the second stream </param>
	  /// @param <R>  the type of the resulting stream </param>
	  /// <param name="stream1">  the first stream </param>
	  /// <param name="stream2">  the first stream </param>
	  /// <param name="zipper">  the function used to combine the pair of objects </param>
	  /// <returns> a stream of pairs, one from each stream </returns>
	  internal static Stream<R> zip<A, B, R>(Stream<A> stream1, Stream<B> stream2, System.Func<A, B, R> zipper)
	  {
		// this is private for now, to see if it is really needed on the API
		// it suffers from generics problems at the call site with common zipper functions
		// as such, it is less useful than it might seem
		Spliterator<A> split1 = stream1.spliterator();
		Spliterator<B> split2 = stream2.spliterator();
		// merged stream lacks some characteristics
		int characteristics = split1.characteristics() & split2.characteristics() & ~(Spliterator.DISTINCT | Spliterator.SORTED);
		long size = Math.Min(split1.ExactSizeIfKnown, split2.ExactSizeIfKnown);

		IEnumerator<A> it1 = Spliterators.iterator(split1);
		IEnumerator<B> it2 = Spliterators.iterator(split2);
		IEnumerator<R> it = new IteratorAnonymousInnerClass2(zipper, it1, it2);
		Spliterator<R> split = Spliterators.spliterator(it, size, characteristics);
		return StreamSupport.stream(split, false);
	  }

	  private class IteratorAnonymousInnerClass2 : IEnumerator<R>
	  {
		  private System.Func<A, B, R> zipper;
		  private IEnumerator<A> it1;
		  private IEnumerator<B> it2;

		  public IteratorAnonymousInnerClass2(System.Func<A, B, R> zipper, IEnumerator<A> it1, IEnumerator<B> it2)
		  {
			  this.zipper = zipper;
			  this.it1 = it1;
			  this.it2 = it2;
		  }

		  public bool hasNext()
		  {
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			return it1.hasNext() && it2.hasNext();
		  }

		  public R next()
		  {
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			return zipper(it1.next(), it2.next());
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a predicate that negates the original.
	  /// <para>
	  /// The JDK provides <seealso cref="Predicate#negate()"/> however this requires a predicate.
	  /// Sometimes, it can be useful to have a static method to achieve this.
	  /// <pre>
	  ///  stream.filter(not(String::isEmpty))
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the object the predicate works on </param>
	  /// <param name="predicate">  the predicate to negate </param>
	  /// <returns> the negated predicate </returns>
	  public static System.Predicate<R> not<R>(System.Predicate<R> predicate)
	  {
		return predicate.negate();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Reducer used in a stream to ensure there is no more than one matching element.
	  /// <para>
	  /// This method returns an operator that can be used with <seealso cref="Stream#reduce(BinaryOperator)"/>
	  /// that returns either zero or one elements from the stream. Unlike <seealso cref="Stream#findFirst()"/>
	  /// or <seealso cref="Stream#findAny()"/>, this approach ensures an exception is thrown if there
	  /// is more than one element in the stream.
	  /// </para>
	  /// <para>
	  /// This would be used as follows:
	  /// <pre>
	  ///   stream.filter(...).reduce(Guavate.ensureOnlyOne()).get();
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the stream </param>
	  /// <returns> the operator </returns>
	  public static System.Func<T, T, T> ensureOnlyOne<T>()
	  {
		return (a, b) =>
		{
	  throw new System.ArgumentException(Messages.format("Multiple values found where only one was expected: {} and {}", a, b));
		};
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable list.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableList"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the list </param>
	  /// <returns> the immutable list collector </returns>
	  public static Collector<T, ImmutableList.Builder<T>, ImmutableList<T>> toImmutableList<T>()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableList.Builder<T>::new, ImmutableList.Builder<T>.add, (l, r) => l.addAll(r.build()), ImmutableList.Builder<T>.build);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable list of
	  /// immutable lists of size equal to or less than size.
	  /// For example, the following list [a, b, c, d, e] with a partition
	  /// size of 2 will give [[a, b], [c, d], [e]].
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableList"/> of <seealso cref="ImmutableList"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the size of the partitions of the original list </param>
	  /// @param <T>  the type of element in the list </param>
	  /// <returns> the immutable list of lists collector </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <T> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableList<com.google.common.collect.ImmutableList<T>>> splittingBySize(int size)
	  public static Collector<T, ?, ImmutableList<ImmutableList<T>>> splittingBySize<T>(int size)
	  {
		return Collectors.collectingAndThen(Collectors.collectingAndThen(Guavate.toImmutableList(), objects => Lists.partition(objects, size)), Guavate.toImmutables);
	  }

	  /// <summary>
	  /// Helper method to transform a list of lists into their immutable counterparts.
	  /// </summary>
	  /// <param name="lists">  the list of lists </param>
	  /// @param <T>  the type of element in the list </param>
	  /// <returns> the immutable lists </returns>
	  private static ImmutableList<ImmutableList<T>> toImmutables<T>(IList<IList<T>> lists)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return lists.Select(ImmutableList.copyOf).collect(Guavate.toImmutableList());
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable set.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSet"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the set </param>
	  /// <returns> the immutable set collector </returns>
	  public static Collector<T, ImmutableSet.Builder<T>, ImmutableSet<T>> toImmutableSet<T>()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableSet.Builder<T>::new, ImmutableSet.Builder<T>.add, (l, r) => l.addAll(r.build()), ImmutableSet.Builder<T>.build, Collector.Characteristics.UNORDERED);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable sorted set.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSet"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the sorted set </param>
	  /// <returns> the immutable sorted set collector </returns>
	  public static Collector<T, ImmutableSortedSet.Builder<T>, ImmutableSortedSet<T>> toImmutableSortedSet<T>()
	  {
		return Collector.of((System.Func<ImmutableSortedSet.Builder<T>>) ImmutableSortedSet.naturalOrder, ImmutableSortedSet.Builder<T>.add, (l, r) => l.addAll(r.build()), ImmutableSortedSet.Builder<T>.build, Collector.Characteristics.UNORDERED);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable sorted set.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSet"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the sorted set </param>
	  /// <param name="comparator">  the comparator </param>
	  /// <returns> the immutable sorted set collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T> java.util.stream.Collector<T, com.google.common.collect.ImmutableSortedSet.Builder<T>, com.google.common.collect.ImmutableSortedSet<T>> toImmutableSortedSet(java.util.Comparator<? super T> comparator)
	  public static Collector<T, ImmutableSortedSet.Builder<T>, ImmutableSortedSet<T>> toImmutableSortedSet<T, T1>(IComparer<T1> comparator)
	  {
		return Collector.of((System.Func<ImmutableSortedSet.Builder<T>>)() => new ImmutableSortedSet.Builder<>(comparator), ImmutableSortedSet.Builder<T>.add, (l, r) => l.addAll(r.build()), ImmutableSortedSet.Builder<T>.build, Collector.Characteristics.UNORDERED);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable multiset.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMultiset"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of element in the multiset </param>
	  /// <returns> the immutable multiset collector </returns>
	  public static Collector<T, ImmutableMultiset.Builder<T>, ImmutableMultiset<T>> toImmutableMultiset<T>()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableMultiset.Builder<T>::new, ImmutableMultiset.Builder<T>.add, (l, r) => l.addAll(r.build()), ImmutableMultiset.Builder<T>.build, Collector.Characteristics.UNORDERED);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMap"/>, retaining insertion order.
	  /// </para>
	  /// <para>
	  /// This returns a map by extracting a key from each element.
	  /// The input stream must resolve to unique keys.
	  /// The value associated with each key is the stream element.
	  /// See <seealso cref="Collectors#toMap(Function, Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result map </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <returns> the immutable map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableMap<K, T>> toImmutableMap(java.util.function.Function<? super T, ? extends K> keyExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableMap<K, T>> toImmutableMap<T, K, T1>(System.Func<T1> keyExtractor) where T1 : K
	  {

		return toImmutableMap(keyExtractor, System.Func.identity());
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMap"/>, retaining insertion order.
	  /// </para>
	  /// <para>
	  /// This returns a map by converting each stream element to a key and value.
	  /// The input stream must resolve to unique keys.
	  /// See <seealso cref="Collectors#toMap(Function, Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result map </param>
	  /// @param <V> the type of the values in the result map </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <param name="valueExtractor">  function to produce values from stream elements </param>
	  /// <returns> the immutable map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K, V> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableMap<K, V>> toImmutableMap(java.util.function.Function<? super T, ? extends K> keyExtractor, java.util.function.Function<? super T, ? extends V> valueExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableMap<K, V>> toImmutableMap<T, K, V, T1, T2>(System.Func<T1> keyExtractor, System.Func<T2> valueExtractor) where T1 : K where T2 : V
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableMap.Builder<K, V>::new, (builder, val) => builder.put(keyExtractor(val), valueExtractor(val)), (l, r) => l.putAll(r.build()), ImmutableMap.Builder<K, V>.build, Collector.Characteristics.UNORDERED);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMap"/>, retaining insertion order.
	  /// </para>
	  /// <para>
	  /// This returns a map by converting each stream element to a key and value.
	  /// If the same key is generated more than once the merge function is applied to the
	  /// values and the return value of the function is used as the value in the map.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result map </param>
	  /// @param <V> the type of the values in the result map </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <param name="valueExtractor">  function to produce values from stream elements </param>
	  /// <param name="mergeFn">  function to merge values with the same key </param>
	  /// <returns> the immutable map collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K, V> java.util.stream.Collector<T, java.util.Map<K, V>, com.google.common.collect.ImmutableMap<K, V>> toImmutableMap(java.util.function.Function<? super T, ? extends K> keyExtractor, java.util.function.Function<? super T, ? extends V> valueExtractor, java.util.function.BiFunction<? super V, ? super V, ? extends V> mergeFn)
	  public static Collector<T, IDictionary<K, V>, ImmutableMap<K, V>> toImmutableMap<T, K, V, T1, T2, T3>(System.Func<T1> keyExtractor, System.Func<T2> valueExtractor, System.Func<T3> mergeFn) where T1 : K where T2 : V where T3 : V
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(LinkedHashMap<K, V>::new, (map, val) => map.merge(keyExtractor(val), valueExtractor(val), mergeFn), (m1, m2) => mergeMaps(m1, m2, mergeFn), map => ImmutableMap.copyOf(map), Collector.Characteristics.UNORDERED);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable sorted map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSortedMap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a map by extracting a key from each element.
	  /// The input stream must resolve to unique keys.
	  /// The value associated with each key is the stream element.
	  /// See <seealso cref="Collectors#toMap(Function, Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result map </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <returns> the immutable sorted map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K extends Comparable<?>> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableSortedMap<K, T>> toImmutableSortedMap(java.util.function.Function<? super T, ? extends K> keyExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableSortedMap<K, T>> toImmutableSortedMap<T, K, T1>(System.Func<T1> keyExtractor) where T1 : K
	  {

		return toImmutableSortedMap(keyExtractor, System.Func.identity());
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable sorted map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSortedMap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a map by converting each stream element to a key and value.
	  /// The input stream must resolve to unique keys.
	  /// See <seealso cref="Collectors#toMap(Function, Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result map </param>
	  /// @param <V> the type of the values in the result map </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <param name="valueExtractor">  function to produce values from stream elements </param>
	  /// <returns> the immutable sorted map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K extends Comparable<?>, V> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableSortedMap<K, V>> toImmutableSortedMap(java.util.function.Function<? super T, ? extends K> keyExtractor, java.util.function.Function<? super T, ? extends V> valueExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableSortedMap<K, V>> toImmutableSortedMap<T, K, V, T1, T2>(System.Func<T1> keyExtractor, System.Func<T2> valueExtractor) where T1 : K where T2 : V
	  {

		return Collector.of((System.Func<ImmutableSortedMap.Builder<K, V>>) ImmutableSortedMap.naturalOrder, (builder, val) => builder.put(keyExtractor(val), valueExtractor(val)), (l, r) => l.putAll(r.build()), ImmutableSortedMap.Builder<K, V>.build, Collector.Characteristics.UNORDERED);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable multimap.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableListMultimap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a multimap by extracting a key from each element.
	  /// The value associated with each key is the stream element.
	  /// Stream elements may be converted to the same key, with the values forming a multimap list.
	  /// See <seealso cref="Collectors#groupingBy(Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result multimap </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <returns> the immutable multimap collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableListMultimap<K, T>> toImmutableListMultimap(java.util.function.Function<? super T, ? extends K> keyExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableListMultimap<K, T>> toImmutableListMultimap<T, K, T1>(System.Func<T1> keyExtractor) where T1 : K
	  {

		return toImmutableListMultimap(keyExtractor, System.Func.identity());
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable multimap.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableListMultimap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a multimap by converting each stream element to a key and value.
	  /// Stream elements may be converted to the same key, with the values forming a multimap list.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result multimap </param>
	  /// @param <V> the type of the values in the result multimap </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <param name="valueExtractor">  function to produce values from stream elements </param>
	  /// <returns> the immutable multimap collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K, V> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableListMultimap<K, V>> toImmutableListMultimap(java.util.function.Function<? super T, ? extends K> keyExtractor, java.util.function.Function<? super T, ? extends V> valueExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableListMultimap<K, V>> toImmutableListMultimap<T, K, V, T1, T2>(System.Func<T1> keyExtractor, System.Func<T2> valueExtractor) where T1 : K where T2 : V
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableListMultimap.Builder<K, V>::new, (builder, val) => builder.put(keyExtractor(val), valueExtractor(val)), (l, r) => l.putAll(r.build()), ImmutableListMultimap.Builder<K, V>.build, Collector.Characteristics.UNORDERED);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable multimap.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSetMultimap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a multimap by extracting a key from each element.
	  /// The value associated with each key is the stream element.
	  /// Stream elements may be converted to the same key, with the values forming a multimap set.
	  /// See <seealso cref="Collectors#groupingBy(Function)"/> for more details.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result multimap </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <returns> the immutable multimap collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableSetMultimap<K, T>> toImmutableSetMultimap(java.util.function.Function<? super T, ? extends K> keyExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableSetMultimap<K, T>> toImmutableSetMultimap<T, K, T1>(System.Func<T1> keyExtractor) where T1 : K
	  {

		return toImmutableSetMultimap(keyExtractor, System.Func.identity());
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable multimap.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableSetMultimap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a multimap by converting each stream element to a key and value.
	  /// Stream elements may be converted to the same key, with the values forming a multimap set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the stream elements </param>
	  /// @param <K> the type of the keys in the result multimap </param>
	  /// @param <V> the type of the values in the result multimap </param>
	  /// <param name="keyExtractor">  function to produce keys from stream elements </param>
	  /// <param name="valueExtractor">  function to produce values from stream elements </param>
	  /// <returns> the immutable multimap collector </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: public static <T, K, V> java.util.stream.Collector<T, ?, com.google.common.collect.ImmutableSetMultimap<K, V>> toImmutableSetMultimap(java.util.function.Function<? super T, ? extends K> keyExtractor, java.util.function.Function<? super T, ? extends V> valueExtractor)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public static Collector<T, ?, ImmutableSetMultimap<K, V>> toImmutableSetMultimap<T, K, V, T1, T2>(System.Func<T1> keyExtractor, System.Func<T2> valueExtractor) where T1 : K where T2 : V
	  {

//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
		return Collector.of(ImmutableSetMultimap.Builder<K, V>::new, (builder, val) => builder.put(keyExtractor(val), valueExtractor(val)), (l, r) => l.putAll(r.build()), ImmutableSetMultimap.Builder<K, V>.build, Collector.Characteristics.UNORDERED);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable map
	  /// from a stream containing map entries. This is a common case if a map's
	  /// {@code entrySet} has undergone a {@code filter} operation. For example:
	  /// <pre>
	  ///   {@code
	  ///       Map<String, Integer> input = ImmutableMap.of("a", 1, "b", 2, "c", 3, "d", 4);
	  ///       ImmutableMap<String, Integer> output =
	  ///         input.entrySet()
	  ///           .stream()
	  ///           .filter(e -> e.getValue() % 2 == 1)
	  ///           .collect(entriesToImmutableMap());
	  /// 
	  ///       // Produces map with "a" -> 1, "c" -> 3, "e" -> 5
	  ///   }
	  /// </pre>
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a map by converting each {@code Map.Entry} to a key and value.
	  /// The input stream must resolve to unique keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K> the type of the keys in the result map </param>
	  /// @param <V> the type of the values in the result map </param>
	  /// <returns> the immutable map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <K, V> java.util.stream.Collector<java.util.Map.Entry<K, V>, ?, com.google.common.collect.ImmutableMap<K, V>> entriesToImmutableMap()
	  public static Collector<KeyValuePair<K, V>, ?, ImmutableMap<K, V>> entriesToImmutableMap<K, V>()
	  {
		return toImmutableMap(DictionaryEntry.getKey, DictionaryEntry.getValue);
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to build an immutable map
	  /// from a stream containing pairs. This is a common case if a map's
	  /// {@code entrySet} has undergone a {@code map} operation with the
	  /// {@code Map.Entry} converted to a {@code Pair}. For example:
	  /// <pre>
	  ///   {@code
	  ///       Map<String, Integer> input = ImmutableMap.of("a", 1, "b", 2, "c", 3, "d", 4);
	  ///       ImmutableMap<String, Double> output =
	  ///         input.entrySet()
	  ///           .stream()
	  ///           .map(e -> Pair.of(e.getKey().toUpperCase(), Math.pow(e.getValue(), 2)))
	  ///           .collect(pairsToImmutableMap());
	  /// 
	  ///       // Produces map with "A" -> 1.0, "B" -> 4.0, "C" -> 9.0, "D" -> 16.0
	  ///   }
	  /// </pre>
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing streams to be gathered into
	  /// an <seealso cref="ImmutableMap"/>.
	  /// </para>
	  /// <para>
	  /// This returns a map by converting each stream element to a key and value.
	  /// The input stream must resolve to unique keys.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K> the type of the keys in the result map </param>
	  /// @param <V> the type of the values in the result map </param>
	  /// <returns> the immutable map collector </returns>
	  /// <exception cref="IllegalArgumentException"> if the same key is generated twice </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <K, V> java.util.stream.Collector<com.opengamma.strata.collect.tuple.Pair<K, V>, ?, com.google.common.collect.ImmutableMap<K, V>> pairsToImmutableMap()
	  public static Collector<Pair<K, V>, ?, ImmutableMap<K, V>> pairsToImmutableMap<K, V>()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		return toImmutableMap(Pair::getFirst, Pair::getSecond);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Helper method to merge two mutable maps by inserting all values from {@code map2} into {@code map1}.
	  /// <para>
	  /// If {@code map1} already contains a mapping for a key the merge function is applied to the existing value and
	  /// the new value, and the return value is inserted.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="map1">  the map into which values are copied </param>
	  /// <param name="map2">  the map from which values are copied </param>
	  /// <param name="mergeFn">  function applied to the existing and new values if the map contains the key </param>
	  /// @param <K>  the key type </param>
	  /// @param <V>  the value type </param>
	  /// <returns> {@code map1} with the values from {@code map2} inserted </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: private static <K, V> java.util.Map<K, V> mergeMaps(java.util.Map<K, V> map1, java.util.Map<K, V> map2, java.util.function.BiFunction<? super V, ? super V, ? extends V> mergeFn)
	  private static IDictionary<K, V> mergeMaps<K, V, T1>(IDictionary<K, V> map1, IDictionary<K, V> map2, System.Func<T1> mergeFn) where T1 : V
	  {

		foreach (KeyValuePair<K, V> entry in map2.SetOfKeyValuePairs())
		{
		  V existingValue = map1[entry.Key];

		  if (existingValue == default(V))
		  {
			map1[entry.Key] = entry.Value;
		  }
		  else
		  {
			map1[entry.Key] = mergeFn(existingValue, entry.Value);
		  }
		}
		return map1;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a list of futures to a single future, combining the values into a list.
	  /// <para>
	  /// The <seealso cref="CompletableFuture#allOf(CompletableFuture...)"/> method is useful
	  /// but it returns {@code Void}. This method combines the futures but also
	  /// returns the resulting value as a list.
	  /// Effectively, this converts {@code List<CompletableFuture<T>>} to {@code CompletableFuture<List<T>>}.
	  /// </para>
	  /// <para>
	  /// If any input future completes exceptionally, the result will also complete exceptionally.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the type of the values in the list </param>
	  /// <param name="futures"> the futures to convert, may be empty </param>
	  /// <returns> a future that combines the input futures as a list </returns>
	  public static CompletableFuture<IList<T>> combineFuturesAsList<T, T1>(IList<T1> futures) where T1 : java.util.concurrent.CompletableFuture<T1 extends T>
	  {

		int size = futures.Count;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.concurrent.CompletableFuture<? extends T>[] futuresArray = futures.toArray(new java.util.concurrent.CompletableFuture[size]);
		CompletableFuture<T>[] futuresArray = futures.ToArray();
		return CompletableFuture.allOf(futuresArray).thenApply(unused =>
		{
		ImmutableList.Builder<T> builder = ImmutableList.builderWithExpectedSize(size);
		for (int i = 0; i < size; i++)
		{
			builder.add(futuresArray[i].join());
		}
		return builder.build();
		});
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to convert a list of futures to a single future,
	  /// combining the values into a list.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing a stream of futures to be combined into a single future.
	  /// This converts {@code List<CompletableFuture<T>>} to {@code CompletableFuture<List<T>>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <S> the type of the input futures </param>
	  /// @param <T> the type of the values </param>
	  /// <returns> a collector that combines the input futures as a list </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <T, S extends java.util.concurrent.CompletableFuture<? extends T>> java.util.stream.Collector<S, ?, java.util.concurrent.CompletableFuture<java.util.List<T>>> toCombinedFuture()
	  public static Collector<S, ?, CompletableFuture<IList<T>>> toCombinedFuture<T, S>()
	  {

		return collectingAndThen(toImmutableList(), Guavate.combineFuturesAsList);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts a map of futures to a single future.
	  /// <para>
	  /// This is similar to <seealso cref="#combineFuturesAsList(List)"/> but for maps.
	  /// Effectively, this converts {@code Map<K, CompletableFuture<V>>} to {@code CompletableFuture<Map<K, V>>}.
	  /// </para>
	  /// <para>
	  /// If any input future completes exceptionally, the result will also complete exceptionally.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K> the type of the keys in the map </param>
	  /// @param <V> the type of the values in the map </param>
	  /// @param <F> the type of the futures </param>
	  /// <param name="futures"> the futures to convert, may be empty </param>
	  /// <returns> a future that combines the input futures as a map </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public static <K, V, F extends java.util.concurrent.CompletableFuture<? extends V>> java.util.concurrent.CompletableFuture<java.util.Map<K, V>> combineFuturesAsMap(java.util.Map<K, F> futures)
	  public static CompletableFuture<IDictionary<K, V>> combineFuturesAsMap<K, V, F>(IDictionary<K, F> futures)
	  {

		int size = futures.Count;
		K[] keyArray = (K[]) new object[size];
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.concurrent.CompletableFuture<? extends V>[] futuresArray = new java.util.concurrent.CompletableFuture[size];
		CompletableFuture<V>[] futuresArray = new CompletableFuture[size];
		int index = 0;
		foreach (KeyValuePair<K, F> entry in futures.SetOfKeyValuePairs())
		{
		  keyArray[index] = entry.Key;
		  futuresArray[index] = entry.Value;
		  index++;
		}
		return CompletableFuture.allOf(futuresArray).thenApply(unused =>
		{
		ImmutableMap.Builder<K, V> builder = ImmutableMap.builderWithExpectedSize(size);
		for (int i = 0; i < size; i++)
		{
			builder.put(keyArray[i], futuresArray[i].join());
		}
		return builder.build();
		});
	  }

	  /// <summary>
	  /// Collector used at the end of a stream to convert a map of futures to a single future,
	  /// combining the values into a map.
	  /// <para>
	  /// A collector is used to gather data at the end of a stream operation.
	  /// This method returns a collector allowing a stream of futures to be combined into a single future.
	  /// This converts {@code Map<K, CompletableFuture<V>>} to {@code CompletableFuture<Map<K, V>>}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <K> the type of the keys in the map </param>
	  /// @param <V> the type of the values in the map </param>
	  /// @param <F> the type of the input futures </param>
	  /// <returns> a collector that combines the input futures as a map </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static <K, V, F extends java.util.concurrent.CompletableFuture<? extends V>> java.util.stream.Collector<java.util.Map.Entry<K, F>, ?, java.util.concurrent.CompletableFuture<java.util.Map<K, V>>> toCombinedFutureMap()
	  public static Collector<KeyValuePair<K, F>, ?, CompletableFuture<IDictionary<K, V>>> toCombinedFutureMap<K, V, F>()
	  {

		return collectingAndThen(entriesToImmutableMap(), Guavate.combineFuturesAsMap);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Polls on a regular frequency until a result is found.
	  /// <para>
	  /// Polling is performed via the specified supplier, which must return null until the result is available.
	  /// If the supplier throws an exception, polling will stop and the future will complete exceptionally.
	  /// </para>
	  /// <para>
	  /// If the future is cancelled, the polling will also be cancelled.
	  /// It is advisable to consider using a timeout when querying the future.
	  /// </para>
	  /// <para>
	  /// In most cases, there needs to be an initial request, which might return an identifier to query.
	  /// This pattern may be useful for that case:
	  /// <pre>
	  ///  return CompletableFuture.supplyAsync(initPollingReturningId(), executorService)
	  ///      .thenCompose(id -&gt; poll(executorService, delay, freq, performPolling(id)));
	  ///  });
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T> the result type </param>
	  /// <param name="executorService">  the executor service to use for polling </param>
	  /// <param name="initialDelay">  the initial delay before starting to poll </param>
	  /// <param name="frequency">  the frequency to poll at </param>
	  /// <param name="pollingTask">  the task used to poll, returning null when not yet complete </param>
	  /// <returns> the future representing the asynchronous operation </returns>
	  public static CompletableFuture<T> poll<T>(ScheduledExecutorService executorService, Duration initialDelay, Duration frequency, System.Func<T> pollingTask)
	  {

		CompletableFuture<T> result = new CompletableFuture<T>();
		ThreadStart decoratedPollingTask = () => pollTask(pollingTask, result);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.concurrent.ScheduledFuture<?> scheduledTask = executorService.scheduleAtFixedRate(decoratedPollingTask, initialDelay.toMillis(), frequency.toMillis(), java.util.concurrent.TimeUnit.MILLISECONDS);
		ScheduledFuture<object> scheduledTask = executorService.scheduleAtFixedRate(decoratedPollingTask, initialDelay.toMillis(), frequency.toMillis(), TimeUnit.MILLISECONDS);
		return result.whenComplete((r, ex) => scheduledTask.cancel(true));
	  }

	  // the task the executor calls
	  private static void pollTask<T>(System.Func<T> pollingTask, CompletableFuture<T> resultFuture)
	  {

		try
		{
		  T result = pollingTask();
		  if (result != default(T))
		  {
			resultFuture.complete(result);
		  }
		}
		catch (Exception ex)
		{
		  resultFuture.completeExceptionally(ex);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a ThreadFactoryBuilder which names new threads with the name of the calling class plus a unique integer.
	  /// </summary>
	  /// <returns> the thread factory builder </returns>
	  public static ThreadFactoryBuilder namedThreadFactory()
	  {
		return namedThreadFactory(callerClass(3).Name);
	  }

	  /// <summary>
	  /// Creates a ThreadFactoryBuilder which names new threads with the given name prefix plus a unique integer.
	  /// </summary>
	  /// <param name="threadNamePrefix">  the name which new thread names should be prefixed by </param>
	  /// <returns> the thread factory builder </returns>
	  public static ThreadFactoryBuilder namedThreadFactory(string threadNamePrefix)
	  {
		return (new ThreadFactoryBuilder()).setNameFormat(threadNamePrefix + "-%d");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the caller class.
	  /// <para>
	  /// This takes an argument which is the number of stack levels to look back.
	  /// This will be 2 to return the caller of this method, 3 to return the caller of the caller, and so on.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="callStackDepth">  the depth of the stack to look back </param>
	  /// <returns> the caller class </returns>
	  public static Type callerClass(int callStackDepth)
	  {
		return CallerClassSecurityManager.INSTANCE.callerClass(callStackDepth);
	  }

	  // on Java 9 or later could use StackWalker, but this is a good choice for Java 8
	  internal class CallerClassSecurityManager : SecurityManager
	  {
		internal static readonly CallerClassSecurityManager INSTANCE = new CallerClassSecurityManager();

		internal virtual Type callerClass(int callStackDepth)
		{
		  return ClassContext[callStackDepth];
		}
	  }

	}

}