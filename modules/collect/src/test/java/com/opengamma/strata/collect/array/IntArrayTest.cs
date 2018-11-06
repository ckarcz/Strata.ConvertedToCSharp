using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.array
{
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
	/// Test <seealso cref="IntArray"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IntArrayTest
	public class IntArrayTest
	{

	  private static readonly int[] EMPTY_INT_ARRAY = new int[0];
	  private const object ANOTHER_TYPE = "";

	  public virtual void test_EMPTY()
	  {
		assertContent(IntArray.EMPTY);
	  }

	  public virtual void test_of()
	  {
		assertContent(IntArray.of());
		assertContent(IntArray.of(1), 1);
		assertContent(IntArray.of(1, 2), 1, 2);
		assertContent(IntArray.of(1, 2, 3), 1, 2, 3);
		assertContent(IntArray.of(1, 2, 3, 4), 1, 2, 3, 4);
		assertContent(IntArray.of(1, 2, 3, 4, 5), 1, 2, 3, 4, 5);
		assertContent(IntArray.of(1, 2, 3, 4, 5, 6), 1, 2, 3, 4, 5, 6);
		assertContent(IntArray.of(1, 2, 3, 4, 5, 6, 7), 1, 2, 3, 4, 5, 6, 7);
		assertContent(IntArray.of(1, 2, 3, 4, 5, 6, 7, 8), 1, 2, 3, 4, 5, 6, 7, 8);
		assertContent(IntArray.of(1, 2, 3, 4, 5, 6, 7, 8, 9), 1, 2, 3, 4, 5, 6, 7, 8, 9);
	  }

	  public virtual void test_of_lambda()
	  {
		assertContent(IntArray.of(0, i =>
		{
		throw new AssertionError();
		}));
		AtomicInteger counter = new AtomicInteger(2);
		assertContent(IntArray.of(1, i => counter.AndIncrement), 2);
		assertContent(IntArray.of(2, i => counter.AndIncrement), 3, 4);
	  }

	  public virtual void test_of_stream()
	  {
		assertContent(IntArray.of(IntStream.empty()));
		assertContent(IntArray.of(IntStream.of(1, 2, 3)), 1, 2, 3);
	  }

	  public virtual void test_ofUnsafe()
	  {
		int[] @base = new int[] {1, 2, 3};
		IntArray test = IntArray.ofUnsafe(@base);
		assertContent(test, 1, 2, 3);
		@base[0] = 4;
		// internal state of object mutated - don't do this in application code!
		assertContent(test, 4, 2, 3);
		// empty
		assertContent(IntArray.ofUnsafe(EMPTY_INT_ARRAY));
	  }

	  public virtual void test_copyOf_List()
	  {
		assertContent(IntArray.copyOf(ImmutableList.of(1, 2, 3)), 1, 2, 3);
		assertContent(IntArray.copyOf(ImmutableList.of()));
	  }

	  public virtual void test_copyOf_array()
	  {
		int[] @base = new int[] {1, 2, 3};
		IntArray test = IntArray.copyOf(@base);
		assertContent(test, 1, 2, 3);
		@base[0] = 4;
		// internal state of object is not mutated
		assertContent(test, 1, 2, 3);
		// empty
		assertContent(IntArray.copyOf(EMPTY_INT_ARRAY));
	  }

	  public virtual void test_copyOf_array_fromIndex()
	  {
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 0), 1, 2, 3);
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 1), 2, 3);
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 3));
		assertThrows(() => IntArray.copyOf(new int[] {1, 2, 3}, -1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => IntArray.copyOf(new int[] {1, 2, 3}, 4), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_copyOf_array_fromToIndex()
	  {
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 0, 3), 1, 2, 3);
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 1, 2), 2);
		assertContent(IntArray.copyOf(new int[] {1, 2, 3}, 1, 1));
		assertThrows(() => IntArray.copyOf(new int[] {1, 2, 3}, -1, 3), typeof(System.IndexOutOfRangeException));
		assertThrows(() => IntArray.copyOf(new int[] {1, 2, 3}, 0, 5), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_filled()
	  {
		assertContent(IntArray.filled(0));
		assertContent(IntArray.filled(3), 0, 0, 0);
	  }

	  public virtual void test_filled_withValue()
	  {
		assertContent(IntArray.filled(0, 1));
		assertContent(IntArray.filled(3, 1), 1, 1, 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_get()
	  {
		IntArray test = IntArray.of(1, 2, 3, 3, 4);
		assertEquals(test.get(0), 1);
		assertEquals(test.get(4), 4);
		assertThrows(() => test.get(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.get(5), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_contains()
	  {
		IntArray test = IntArray.of(1, 2, 3, 3, 4);
		assertEquals(test.contains(1), true);
		assertEquals(test.contains(3), true);
		assertEquals(test.contains(5), false);
		assertEquals(IntArray.EMPTY.contains(5), false);
	  }

	  public virtual void test_indexOf()
	  {
		IntArray test = IntArray.of(1, 2, 3, 3, 4);
		assertEquals(test.indexOf(2), 1);
		assertEquals(test.indexOf(3), 2);
		assertEquals(test.indexOf(5), -1);
		assertEquals(IntArray.EMPTY.indexOf(5), -1);
	  }

	  public virtual void test_lastIndexOf()
	  {
		IntArray test = IntArray.of(1, 2, 3, 3, 4);
		assertEquals(test.lastIndexOf(2), 1);
		assertEquals(test.lastIndexOf(3), 3);
		assertEquals(test.lastIndexOf(5), -1);
		assertEquals(IntArray.EMPTY.lastIndexOf(5), -1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_copyInto()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		int[] dest = new int[4];
		test.copyInto(dest, 0);
		assertTrue(Arrays.Equals(dest, new int[] {1, 2, 3, 0}));

		int[] dest2 = new int[4];
		test.copyInto(dest2, 1);
		assertTrue(Arrays.Equals(dest2, new int[] {0, 1, 2, 3}));

		int[] dest3 = new int[4];
		assertThrows(() => test.copyInto(dest3, 2), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.copyInto(dest3, -1), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_subArray_from()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.subArray(0), 1, 2, 3);
		assertContent(test.subArray(1), 2, 3);
		assertContent(test.subArray(2), 3);
		assertContent(test.subArray(3));
		assertThrows(() => test.subArray(4), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.subArray(-1), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void test_subArray_fromTo()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.subArray(0, 3), 1, 2, 3);
		assertContent(test.subArray(1, 3), 2, 3);
		assertContent(test.subArray(2, 3), 3);
		assertContent(test.subArray(3, 3));
		assertContent(test.subArray(1, 2), 2);
		assertThrows(() => test.subArray(0, 4), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.subArray(-1, 3), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toList()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		IList<int> list = test.toList();
		assertContent(IntArray.copyOf(list), 1, 2, 3);
		assertEquals(list.Count, 3);
		assertEquals(list.Count == 0, false);
		assertEquals(list[0], 1);
		assertEquals(list[2], 3);
		assertEquals(list.Contains(2), true);
		assertEquals(list.Contains(5), false);
		assertEquals(list.Contains(ANOTHER_TYPE), false);
		assertEquals(list.IndexOf(2), 1);
		assertEquals(list.IndexOf(5), -1);
		assertEquals(list.IndexOf(ANOTHER_TYPE), -1);
		assertEquals(list.LastIndexOf(3), 2);
		assertEquals(list.LastIndexOf(5), -1);
		assertEquals(list.LastIndexOf(ANOTHER_TYPE), -1);

		assertThrows(() => list.Clear(), typeof(System.NotSupportedException));
		assertThrows(() => list.set(0, 3), typeof(System.NotSupportedException));
	  }

	  public virtual void test_toList_iterator()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		IList<int> list = test.toList();
		IEnumerator<int> it = list.GetEnumerator();
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next().intValue(), 1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next().intValue(), 2);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.next().intValue(), 3);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(it.hasNext(), false);

//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
		assertThrows(() => it.remove(), typeof(System.NotSupportedException));
	  }

	  public virtual void test_toList_listIterator()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		IList<int> list = test.toList();
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
		IEnumerator<int> lit = list.GetEnumerator();
		assertEquals(lit.nextIndex(), 0);
		assertEquals(lit.previousIndex(), -1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), false);
		assertThrows(() => lit.previous(), typeof(NoSuchElementException));

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next().intValue(), 1);
		assertEquals(lit.nextIndex(), 1);
		assertEquals(lit.previousIndex(), 0);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next().intValue(), 2);
		assertEquals(lit.nextIndex(), 2);
		assertEquals(lit.previousIndex(), 1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.next().intValue(), 3);
		assertEquals(lit.nextIndex(), 3);
		assertEquals(lit.previousIndex(), 2);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), false);
		assertEquals(lit.hasPrevious(), true);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertThrows(() => lit.next(), typeof(NoSuchElementException));

		assertEquals(lit.previous().intValue(), 3);
		assertEquals(lit.nextIndex(), 2);
		assertEquals(lit.previousIndex(), 1);
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
		assertEquals(lit.hasNext(), true);
		assertEquals(lit.hasPrevious(), true);

//JAVA TO C# CONVERTER TODO TASK: .NET enumerators are read-only:
		assertThrows(() => lit.remove(), typeof(System.NotSupportedException));
		assertThrows(() => lit.set(2), typeof(System.NotSupportedException));
		assertThrows(() => lit.add(2), typeof(System.NotSupportedException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_stream()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		int[] streamed = test.ToArray();
		assertTrue(Arrays.Equals(streamed, new int[] {1, 2, 3}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_forEach()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		int[] extracted = new int[3];
		test.forEach((i, v) => extracted[i] = v);
		assertTrue(Arrays.Equals(extracted, new int[] {1, 2, 3}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_with()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.with(0, 4), 4, 2, 3);
		assertContent(test.with(0, 1), 1, 2, 3);
		assertThrows(() => test.with(-1, 2), typeof(System.IndexOutOfRangeException));
		assertThrows(() => test.with(3, 2), typeof(System.IndexOutOfRangeException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.plus(5), 6, 7, 8);
		assertContent(test.plus(0), 1, 2, 3);
		assertContent(test.plus(-5), -4, -3, -2);
	  }

	  public virtual void test_minus()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.minus(5), -4, -3, -2);
		assertContent(test.minus(0), 1, 2, 3);
		assertContent(test.minus(-5), 6, 7, 8);
	  }

	  public virtual void test_multipliedBy()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.multipliedBy(5), 5, 10, 15);
		assertContent(test.multipliedBy(1), 1, 2, 3);
	  }

	  public virtual void test_dividedBy()
	  {
		IntArray test = IntArray.of(10, 20, 30);
		assertContent(test.dividedBy(5), 2, 4, 6);
		assertContent(test.dividedBy(1), 10, 20, 30);
	  }

	  public virtual void test_map()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.map(v => 1 / v), 1, 1 / 2, 1 / 3);
	  }

	  public virtual void test_mapWithIndex()
	  {
		IntArray test = IntArray.of(1, 2, 3);
		assertContent(test.mapWithIndex((i, v) => i * v), 0, 2, 6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_array()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertContent(test1.plus(test2), 6, 8, 10);
		assertThrows(() => test1.plus(IntArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_minus_array()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertContent(test1.minus(test2), -4, -4, -4);
		assertThrows(() => test1.minus(IntArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_multipliedBy_array()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertContent(test1.multipliedBy(test2), 5, 12, 21);
		assertThrows(() => test1.multipliedBy(IntArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_dividedBy_array()
	  {
		IntArray test1 = IntArray.of(10, 20, 30);
		IntArray test2 = IntArray.of(2, 5, 10);
		assertContent(test1.dividedBy(test2), 5, 4, 3);
		assertThrows(() => test1.dividedBy(IntArray.EMPTY), typeof(System.ArgumentException));
	  }

	  public virtual void test_combine()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertContent(test1.combine(test2, (a, b) => a * b), 5, 12, 21);
		assertThrows(() => test1.combine(IntArray.EMPTY, (a, b) => a * b), typeof(System.ArgumentException));
	  }

	  public virtual void test_combineReduce()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertEquals(test1.combineReduce(test2, (r, a, b) => r + a * b), 5 + 12 + 21);
		assertThrows(() => test1.combineReduce(IntArray.EMPTY, (r, a, b) => r + a * b), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_sorted()
	  {
		assertContent(IntArray.of().sorted());
		assertContent(IntArray.of(2).sorted(), 2);
		assertContent(IntArray.of(2, 1, 3, 0).sorted(), 0, 1, 2, 3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_min()
	  {
		assertEquals(IntArray.of(2).min(), 2);
		assertEquals(IntArray.of(2, 1, 3).min(), 1);
		assertThrows(() => IntArray.EMPTY.min(), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_max()
	  {
		assertEquals(IntArray.of(2).max(), 2);
		assertEquals(IntArray.of(2, 1, 3).max(), 3);
		assertThrows(() => IntArray.EMPTY.max(), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_sum()
	  {
		assertEquals(IntArray.EMPTY.sum(), 0);
		assertEquals(IntArray.of(2).sum(), 2);
		assertEquals(IntArray.of(2, 1, 3).sum(), 6);
	  }

	  public virtual void test_reduce()
	  {
		assertEquals(IntArray.EMPTY.reduce(2, (r, v) =>
		{
		throw new AssertionError();
		}), 2);
		assertEquals(IntArray.of(2).reduce(1, (r, v) => r * v), 2);
		assertEquals(IntArray.of(2, 1, 3).reduce(1, (r, v) => r * v), 6);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_concat_varargs()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		assertContent(test1.concat(5, 6, 7), 1, 2, 3, 5, 6, 7);
		assertContent(test1.concat(new int[] {5, 6, 7}), 1, 2, 3, 5, 6, 7);
		assertContent(test1.concat(EMPTY_INT_ARRAY), 1, 2, 3);
		assertContent(IntArray.EMPTY.concat(new int[] {1, 2, 3}), 1, 2, 3);
	  }

	  public virtual void test_concat_object()
	  {
		IntArray test1 = IntArray.of(1, 2, 3);
		IntArray test2 = IntArray.of(5, 6, 7);
		assertContent(test1.concat(test2), 1, 2, 3, 5, 6, 7);
		assertContent(test1.concat(IntArray.EMPTY), 1, 2, 3);
		assertContent(IntArray.EMPTY.concat(test1), 1, 2, 3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalsHashCode()
	  {
		IntArray a1 = IntArray.of(1, 2);
		IntArray a2 = IntArray.of(1, 2);
		IntArray b = IntArray.of(1, 2, 3);
		assertEquals(a1.Equals(a1), true);
		assertEquals(a1.Equals(a2), true);
		assertEquals(a1.Equals(b), false);
		assertEquals(a1.Equals(ANOTHER_TYPE), false);
		assertEquals(a1.Equals(null), false);
		assertEquals(a1.GetHashCode(), a2.GetHashCode());
	  }

	  public virtual void test_toString()
	  {
		IntArray test = IntArray.of(1, 2);
		assertEquals(test.ToString(), "[1, 2]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(IntArray.of(1, 2, 3));
		IntArray.of(1, 2, 3).metaBean().metaProperty("array").metaBean();
		IntArray.of(1, 2, 3).metaBean().metaProperty("array").propertyGenericType();
		IntArray.of(1, 2, 3).metaBean().metaProperty("array").annotations();
	  }

	  //-------------------------------------------------------------------------
	  private void assertContent(IntArray array, params int[] expected)
	  {
		if (expected.Length == 0)
		{
		  assertSame(array, IntArray.EMPTY);
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

	  private void assertArray(int[] array, int[] expected)
	  {
		assertEquals(array.Length, expected.Length);

		for (int i = 0; i < array.Length; i++)
		{
		  assertEquals(array[i], expected[i], "Unexpected value at index " + i + ",");
		}
	  }
	}

}