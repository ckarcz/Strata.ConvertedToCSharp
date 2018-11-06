/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FunctionUtilsTest
	public class FunctionUtilsTest
	{
	  private const double EPS = 1e-15;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testSquare()
	  public virtual void testSquare()
	  {
		for (int i = 0; i < 100; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = Math.random();
		  double x = GlobalRandom.NextDouble;
		  assertEquals(FunctionUtils.square(x), x * x, EPS);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testCube()
	  public virtual void testCube()
	  {
		for (int i = 0; i < 100; i++)
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double x = Math.random();
		  double x = GlobalRandom.NextDouble;
		  assertEquals(FunctionUtils.cube(x), x * x * x, EPS);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullIndices()
	  public virtual void testNullIndices()
	  {
		FunctionUtils.toTensorIndex(null, new int[] {1, 2, 3});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDimensions1()
	  public virtual void testNullDimensions1()
	  {
		FunctionUtils.toTensorIndex(new int[] {1, 2, 3}, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testWrongLength()
	  public virtual void testWrongLength()
	  {
		FunctionUtils.toTensorIndex(new int[] {1, 2}, new int[] {1, 2, 3});
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullDimensions2()
	  public virtual void testNullDimensions2()
	  {
		FunctionUtils.fromTensorIndex(2, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTensorIndexTest1()
	  public virtual void testTensorIndexTest1()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] indices = new int[] {2 };
		int[] indices = new int[] {2};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] dimensions = new int[] {5 };
		int[] dimensions = new int[] {5};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int index = FunctionUtils.toTensorIndex(indices, dimensions);
		int index = FunctionUtils.toTensorIndex(indices, dimensions);
		assertEquals(indices[0], index, 0);

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		assertEquals(indices[0], res[0], 0);

	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTensorIndexTest2()
	  public virtual void testTensorIndexTest2()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] indices = new int[] {2, 3 };
		int[] indices = new int[] {2, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] dimensions = new int[] {5, 7 };
		int[] dimensions = new int[] {5, 7};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int index = FunctionUtils.toTensorIndex(indices, dimensions);
		int index = FunctionUtils.toTensorIndex(indices, dimensions);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		assertEquals(indices[0], res[0], 0);
		assertEquals(indices[1], res[1], 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTensorIndexTest3()
	  public virtual void testTensorIndexTest3()
	  {

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] indices = new int[] {2, 3, 1 };
		int[] indices = new int[] {2, 3, 1};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] dimensions = new int[] {5, 7, 3 };
		int[] dimensions = new int[] {5, 7, 3};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int index = FunctionUtils.toTensorIndex(indices, dimensions);
		int index = FunctionUtils.toTensorIndex(indices, dimensions);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		int[] res = FunctionUtils.fromTensorIndex(index, dimensions);
		assertEquals(indices[0], res[0], 0);
		assertEquals(indices[1], res[1], 0);
		assertEquals(indices[2], res[2], 0);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testOutOfBounds()
	  public virtual void testOutOfBounds()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] indices = new int[] {2, 7, 1 };
		int[] indices = new int[] {2, 7, 1};
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int[] dimensions = new int[] {5, 7, 3 };
		int[] dimensions = new int[] {5, 7, 3};
		FunctionUtils.toTensorIndex(indices, dimensions);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void getLowerBoundIndexTest()
	  public virtual void getLowerBoundIndexTest()
	  {
		int i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-2.0, -1.0), -0.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(1.0, 2.0), -0.0);
		assertEquals(i, 0);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(1.0, 2.0, 3.0), 2.5);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(1.0, 2.0, 3.0), 2.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(1.0, 2.0, 3.0), -2.0);
		assertEquals(i, 0);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-2.0, -1.0, 0.0), -0.0);
		assertEquals(i, 2);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-2.0, -1.0, 0.0), 0.0);
		assertEquals(i, 2);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-2.0, -1.0, 0.0), -0.0);
		assertEquals(i, 2);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-2.0, -1.0, -0.0), -0.0);
		assertEquals(i, 2);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-1.0, 0.0, 1.0), -0.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-1.0, 0.0, 1.0), 0.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-1.0, -0.0, 1.0), 0.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-1.0, -0.0, 1.0), -0.0);
		assertEquals(i, 1);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(0.0, 1.0, 2.0), -0.0);
		assertEquals(i, 0);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(0.0, 1.0, 2.0), 0.0);
		assertEquals(i, 0);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-0.0, 1.0, 2.0), 0.0);
		assertEquals(i, 0);
		i = FunctionUtils.getLowerBoundIndex(DoubleArray.of(-0.0, 1.0, 2.0), -0.0);
		assertEquals(i, 0);
	  }
	}

}