using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.array
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.DoubleArrayMath.EMPTY_DOUBLE_ARRAY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test <seealso cref="DoubleArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoubleArrayTest
	public class DoubleArrayTest
	{

	  private const object ANOTHER_TYPE = "";

	  private const double DELTA = 1e-14;

	  public virtual void test_EMPTY()
	  {
		assertContent(DoubleArray.EMPTY);
	  }

	  public virtual void test_of()
	  {
		assertContent(DoubleArray.of());
		assertContent(DoubleArray.of(1d), 1d);
		assertContent(DoubleArray.of(1d, 2d), 1d, 2d);
		assertContent(DoubleArray.of(1d, 2d, 3d), 1d, 2d, 3d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d), 1d, 2d, 3d, 4d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d, 5d), 1d, 2d, 3d, 4d, 5d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d, 5d, 6d), 1d, 2d, 3d, 4d, 5d, 6d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d, 5d, 6d, 7d), 1d, 2d, 3d, 4d, 5d, 6d, 7d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d), 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d);
		assertContent(DoubleArray.of(1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d), 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d);
	  }

	  public virtual void test_of_lambda()
	  {
		assertContent(DoubleArray.of(0, i =>
		{
		throw new AssertionError();
		}));
		AtomicInteger counter = new AtomicInteger(2);
		assertContent(DoubleArray.of(1, i => counter.AndIncrement), 2d);
		assertContent(DoubleArray.of(2, i => counter.AndIncrement), 3d, 4d);
	  }

	  public virtual void test_of_stream()
	  {
		assertContent(DoubleArray.of(DoubleStream.empty()));
		assertContent(DoubleArray.of(DoubleStream.of(1d, 2d, 3d)), 1d, 2d, 3d);
	  }

	  public virtual void test_ofUnsafe()
	  {
		double[] @base = new double[] {1d, 2d, 3d};
		DoubleArray test = DoubleArray.ofUnsafe(@base);
		assertContent(test, 1d, 2d, 3d);
		@base[0] = 4d;
		// internal state of object mutated - don't do this in application code!
		assertContent(test, 4d, 2d, 3d);
		// empty
		assertContent(DoubleArray.ofUnsafe(EMPTY_DOUBLE_ARRAY));
	  }

	  public virtual void test_copyOf_List()
	  {
		assertContent(DoubleArray.copyOf(ImmutableList.of(1d, 2d, 3d)), 1d, 2d, 3d);
		assertContent(DoubleArray.copyOf(ImmutableList.of()));
	  }

	  public virtual void test_copyOf_array()
	  {
		double[] @base = new double[] {1d, 2d, 3d};
		DoubleArray test = DoubleArray.copyOf(@base);
		assertContent(test, 1d, 2d, 3d);
		@base[0] = 4d;
		// internal state of object is not mutated
		assertContent(test, 1d, 2d, 3d);
		// empty
		assertContent(DoubleArray.copyOf(EMPTY_DOUBLE_ARRAY));
	  }

	  public virtual void test_copyOf_array_fromIndex()
	  {
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 0), 1d, 2d, 3d);
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 1), 2d, 3d);
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 3));
		assertThrows(() => DoubleArray.copyOf(new double[] {1d, 2d, 3d}, -1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 4), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_copyOf_array_fromToIndex()
	  {
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 0, 3), 1d, 2d, 3d);
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 1, 2), 2d);
		assertContent(DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 1, 1));
		assertThrows(() => DoubleArray.copyOf(new double[] {1d, 2d, 3d}, -1, 3), typeof(System.IndexOutOfRangeException));
		assertThrows(() => DoubleArray.copyOf(new double[] {1d, 2d, 3d}, 0, 5), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_filled()
	  {
		assertContent(DoubleArray.filled(0));
		assertContent(DoubleArray.filled(3), 0d, 0d, 0d);
	  }

	  public virtual void test_filled_withValue()
	  {
		assertContent(DoubleArray.filled(0, 1.5));
		assertContent(DoubleArray.filled(3, 1.5), 1.5, 1.5, 1.5);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_get()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d, 3d, 4d);
		assertEquals(test.get(0), 1d);
		assertEquals(test.get(4), 4d);
		assertThrows(() => test.get(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.get(5), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_contains()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d, 3d, 4d);
		assertEquals(test.contains(1d), true);
		assertEquals(test.contains(3d), true);
		assertEquals(test.contains(5d), false);
		assertEquals(DoubleArray.EMPTY.contains(5d), false);
	  }

	  public virtual void test_indexOf()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d, 3d, 4d);
		assertEquals(test.indexOf(2d), 1);
		assertEquals(test.indexOf(3d), 2);
		assertEquals(test.indexOf(5d), -1);
		assertEquals(DoubleArray.EMPTY.indexOf(5d), -1);
	  }

	  public virtual void test_lastIndexOf()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d, 3d, 4d);
		assertEquals(test.lastIndexOf(2d), 1);
		assertEquals(test.lastIndexOf(3d), 3);
		assertEquals(test.lastIndexOf(5d), -1);
		assertEquals(DoubleArray.EMPTY.lastIndexOf(5d), -1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_copyInto()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		double[] dest = new double[4];
		test.copyInto(dest, 0);
		assertTrue(Arrays.Equals(dest, new double[] {1d, 2d, 3d, 0d}));

		double[] dest2 = new double[4];
		test.copyInto(dest2, 1);
		assertTrue(Arrays.Equals(dest2, new double[] {0d, 1d, 2d, 3d}));

		double[] dest3 = new double[4];
		assertThrows(() => test.copyInto(dest3, 2), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.copyInto(dest3, -1), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_subArray_from()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.subArray(0), 1d, 2d, 3d);
		assertContent(test.subArray(1), 2d, 3d);
		assertContent(test.subArray(2), 3d);
		assertContent(test.subArray(3));
		assertThrows(() => test.subArray(4), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.subArray(-1), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_subArray_fromTo()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.subArray(0, 3), 1d, 2d, 3d);
		assertContent(test.subArray(1, 3), 2d, 3d);
		assertContent(test.subArray(2, 3), 3d);
		assertContent(test.subArray(3, 3));
		assertContent(test.subArray(1, 2), 2d);
		assertThrows(() => test.subArray(0, 4), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.subArray(-1, 3), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unlikely-arg-type") public void test_toList()
	  public virtual void test_toList()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		IList<double> list = test.toList();
		assertContent(DoubleArray.copyOf(list), 1d, 2d, 3d);
		assertEquals(list.Count, 3);
		assertEquals(list.Count == 0, false);
		assertEquals(list[0], 1d);
		assertEquals(list[2], 3d);
		assertEquals(list.Contains(2d), true);
		assertEquals(list.Contains(5d), false);
		assertEquals(list.Contains(""), false);
		assertEquals(list.IndexOf(2d), 1);
		assertEquals(list.IndexOf(5d), -1);
		assertEquals(list.IndexOf(""), -1);
		assertEquals(list.LastIndexOf(3d), 2);
		assertEquals(list.LastIndexOf(5d), -1);
		assertEquals(list.LastIndexOf(""), -1);

		assertThrows(() => list.Clear(), typeof(System.NotSupportedException));
		assertThrows(() => list.set(0, 3d), typeof(System.NotSupportedException));
	  }

	  public virtual void test_toList_iterator()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		IList<double> list = test.toList();
		IEnumerator<double> it = list.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next(), 1d);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next(), 2d);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next(), 3d);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), false);

//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
		assertThrows(() => it.remove(), typeof(System.NotSupportedException));
	  }

	  public virtual void test_toList_listIterator()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		IList<double> list = test.toList();
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
		IEnumerator<double> lit = list.GetEnumerator();
		assertEquals(lit.nextIndex(), 0);
		assertEquals(lit.previousIndex(), -1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), false);
		assertThrows(() => lit.previous(), typeof(NoSuchElementException));

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next(), 1d);
		assertEquals(lit.nextIndex(), 1);
		assertEquals(lit.previousIndex(), 0);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next(), 2d);
		assertEquals(lit.nextIndex(), 2);
		assertEquals(lit.previousIndex(), 1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next(), 3d);
		assertEquals(lit.nextIndex(), 3);
		assertEquals(lit.previousIndex(), 2);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), false);
		assertEquals(lit.hasPrevious(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertThrows(() => lit.next(), typeof(NoSuchElementException));

		assertEquals(lit.previous(), 3d);
		assertEquals(lit.nextIndex(), 2);
		assertEquals(lit.previousIndex(), 1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
		assertThrows(() => lit.remove(), typeof(System.NotSupportedException));
		assertThrows(() => lit.set(2d), typeof(System.NotSupportedException));
		assertThrows(() => lit.add(2d), typeof(System.NotSupportedException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		double[] streamed = test.ToArray();
		assertTrue(Arrays.Equals(streamed, new double[] {1d, 2d, 3d}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forEach()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		double[] extracted = new double[3];
		test.forEach((i, v) => extracted[i] = v);
		assertTrue(Arrays.Equals(extracted, new double[] {1d, 2d, 3d}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_with()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.with(0, 2.6d), 2.6d, 2d, 3d);
		assertContent(test.with(0, 1d), 1d, 2d, 3d);
		assertThrows(() => test.with(-1, 2d), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.with(3, 2d), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.plus(5), 6d, 7d, 8d);
		assertContent(test.plus(0), 1d, 2d, 3d);
		assertContent(test.plus(-5), -4d, -3d, -2d);
	  }

	  public virtual void test_minus()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.minus(5), -4d, -3d, -2d);
		assertContent(test.minus(0), 1d, 2d, 3d);
		assertContent(test.minus(-5), 6d, 7d, 8d);
	  }

	  public virtual void test_multipliedBy()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.multipliedBy(5), 5d, 10d, 15d);
		assertContent(test.multipliedBy(1), 1d, 2d, 3d);
	  }

	  public virtual void test_dividedBy()
	  {
		DoubleArray test = DoubleArray.of(10d, 20d, 30d);
		assertContent(test.dividedBy(5), 2d, 4d, 6d);
		assertContent(test.dividedBy(1), 10d, 20d, 30d);
	  }

	  public virtual void test_map()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.map(v => 1 / v), 1d, 1d / 2d, 1d / 3d);
	  }

	  public virtual void test_mapWithIndex()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d, 3d);
		assertContent(test.mapWithIndex((i, v) => i * v), 0d, 2d, 6d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_array()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertContent(test1.plus(test2), 1.5d, 2.6d, 3.7d);
		assertThrows(() => test1.plus(DoubleArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_minus_array()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertContent(test1.minus(test2), 0.5d, 1.4d, 2.3d);
		assertThrows(() => test1.minus(DoubleArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_multipliedBy_array()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertContent(test1.multipliedBy(test2), 0.5d, 1.2d, 2.1d);
		assertThrows(() => test1.multipliedBy(DoubleArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_dividedBy_array()
	  {
		DoubleArray test1 = DoubleArray.of(10d, 20d, 30d);
		DoubleArray test2 = DoubleArray.of(2d, 5d, 10d);
		assertContent(test1.dividedBy(test2), 5d, 4d, 3d);
		assertThrows(() => test1.dividedBy(DoubleArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_combine()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertContent(test1.combine(test2, (a, b) => a * b), 0.5d, 2d * 0.6d, 3d * 0.7d);
		assertThrows(() => test1.combine(DoubleArray.EMPTY, (a, b) => a * b), typeof(System.ArgumentException));
	  }

	  public virtual void test_combineReduce()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertEquals(test1.combineReduce(test2, (r, a, b) => r + a * b), 0.5d + 2d * 0.6d + 3d * 0.7d);
		assertThrows(() => test1.combineReduce(DoubleArray.EMPTY, (r, a, b) => r + a * b), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_sorted()
	  {
		assertContent(DoubleArray.of().sorted());
		assertContent(DoubleArray.of(2d).sorted(), 2d);
		assertContent(DoubleArray.of(2d, 1d, 3d, 0d).sorted(), 0d, 1d, 2d, 3d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_min()
	  {
		assertEquals(DoubleArray.of(2d).min(), 2d);
		assertEquals(DoubleArray.of(2d, 1d, 3d).min(), 1d);
		assertThrows(() => DoubleArray.EMPTY.min(), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_max()
	  {
		assertEquals(DoubleArray.of(2d).max(), 2d);
		assertEquals(DoubleArray.of(2d, 1d, 3d).max(), 3d);
		assertThrows(() => DoubleArray.EMPTY.max(), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_sum()
	  {
		assertEquals(DoubleArray.EMPTY.sum(), 0d);
		assertEquals(DoubleArray.of(2d).sum(), 2d);
		assertEquals(DoubleArray.of(2d, 1d, 3d).sum(), 6d);
	  }

	  public virtual void test_reduce()
	  {
		assertEquals(DoubleArray.EMPTY.reduce(2d, (r, v) =>
		{
		throw new AssertionError();
		}), 2d);
		assertEquals(DoubleArray.of(2d).reduce(1d, (r, v) => r * v), 2d);
		assertEquals(DoubleArray.of(2d, 1d, 3d).reduce(1d, (r, v) => r * v), 6d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_concat_varargs()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		assertContent(test1.concat(0.5d, 0.6d, 0.7d), 1d, 2d, 3d, 0.5d, 0.6d, 0.7d);
		assertContent(test1.concat(new double[] {0.5d, 0.6d, 0.7d}), 1d, 2d, 3d, 0.5d, 0.6d, 0.7d);
		assertContent(test1.concat(EMPTY_DOUBLE_ARRAY), 1d, 2d, 3d);
		assertContent(DoubleArray.EMPTY.concat(new double[] {1d, 2d, 3d}), 1d, 2d, 3d);
	  }

	  public virtual void test_concat_object()
	  {
		DoubleArray test1 = DoubleArray.of(1d, 2d, 3d);
		DoubleArray test2 = DoubleArray.of(0.5d, 0.6d, 0.7d);
		assertContent(test1.concat(test2), 1d, 2d, 3d, 0.5d, 0.6d, 0.7d);
		assertContent(test1.concat(DoubleArray.EMPTY), 1d, 2d, 3d);
		assertContent(DoubleArray.EMPTY.concat(test1), 1d, 2d, 3d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalWithTolerance()
	  {
		DoubleArray a1 = DoubleArray.of(1d, 2d);
		DoubleArray a2 = DoubleArray.of(1d, 2.02d);
		DoubleArray a3 = DoubleArray.of(1d, 2.009d);
		DoubleArray b = DoubleArray.of(1d, 2d, 3d);
		assertEquals(a1.equalWithTolerance(a2, 0.01d), false);
		assertEquals(a1.equalWithTolerance(a3, 0.01d), true);
		assertEquals(a1.equalWithTolerance(b, 0.01d), false);
	  }

	  public virtual void test_equalZeroWithTolerance()
	  {
		DoubleArray a1 = DoubleArray.of(0d, 0d);
		DoubleArray a2 = DoubleArray.of(0d, 0.02d);
		DoubleArray a3 = DoubleArray.of(0d, 0.009d);
		DoubleArray b = DoubleArray.of(1d, 2d, 3d);
		assertEquals(a1.equalZeroWithTolerance(0.01d), true);
		assertEquals(a2.equalZeroWithTolerance(0.01d), false);
		assertEquals(a3.equalZeroWithTolerance(0.01d), true);
		assertEquals(b.equalZeroWithTolerance(0.01d), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		DoubleArray a1 = DoubleArray.of(1d, 2d);
		DoubleArray a2 = DoubleArray.of(1d, 2d);
		DoubleArray b = DoubleArray.of(1d, 2d, 3d);
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_toString()
	  {
		DoubleArray test = DoubleArray.of(1d, 2d);
		assertEquals(test.ToString(), "[1.0, 2.0]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(DoubleArray.of(1d, 2d, 3d));
		DoubleArray.of(1d, 2d, 3d).metaBean().metaProperty("array").metaBean();
		DoubleArray.of(1d, 2d, 3d).metaBean().metaProperty("array").propertyGenericType();
		DoubleArray.of(1d, 2d, 3d).metaBean().metaProperty("array").annotations();
	  }

	  //-------------------------------------------------------------------------
	  private void assertContent(DoubleArray array, params double[] expected)
	  {
		if (expected.Length == 0)
		{
		  assertSame(array, DoubleArray.EMPTY);
		  assertEquals(array.Empty, true);
		}
		else
		{
		  assertEquals(array.size(), expected.Length);
		  assertArray(array.toArray(), expected);
		  assertArray(array.toArrayUnsafe(), expected);
		  assertEquals(array.dimensions(), 1);
		  assertEquals(array.Empty, false);
		}
	  }

	  private void assertArray(double[] array, double[] expected)
	  {
		assertEquals(array.Length, expected.Length);

		for (int i = 0; i < array.Length; i++)
		{
		  assertEquals(array[i], expected[i], DELTA, "Unexpected value at index " + i + ",");
		}
	  }
	}

}