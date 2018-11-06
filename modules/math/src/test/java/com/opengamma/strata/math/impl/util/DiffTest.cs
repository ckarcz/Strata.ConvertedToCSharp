/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.util
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertTrue;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Tests <seealso cref="Diff"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DiffTest
	public class DiffTest
	{

	  /* double data */
	  internal double[] _dataDouble = new double[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal double[] _dataDoubleAnswerDiff0times = new double[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal double[] _dataDoubleAnswerDiff1times = new double[] {4, -3, 6, 1, 13, -12, 2};
	  internal double[] _dataDoubleAnswerDiff2times = new double[] {-7, 9, -5, 12, -25, 14};
	  internal double[] _dataDoubleAnswerDiff3times = new double[] {16, -14, 17, -37, 39};
	  internal double[] _dataDoubleAnswerDiff4times = new double[] {-30, 31, -54, 76};
	  internal double[] _dataDoubleAnswerDiff5times = new double[] {61, -85, 130};
	  internal double[] _dataDoubleAnswerDiff6times = new double[] {-146, 215};
	  internal double[] _dataDoubleAnswerDiff7times = new double[] {361};
	  internal double[] _dataNullDouble = null;

	  internal float[] _dataFloat = new float[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal float[] _dataFloatAnswerDiff0times = new float[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal float[] _dataFloatAnswerDiff1times = new float[] {4, -3, 6, 1, 13, -12, 2};
	  internal float[] _dataFloatAnswerDiff2times = new float[] {-7, 9, -5, 12, -25, 14};
	  internal float[] _dataFloatAnswerDiff3times = new float[] {16, -14, 17, -37, 39};
	  internal float[] _dataFloatAnswerDiff4times = new float[] {-30, 31, -54, 76};
	  internal float[] _dataFloatAnswerDiff5times = new float[] {61, -85, 130};
	  internal float[] _dataFloatAnswerDiff6times = new float[] {-146, 215};
	  internal float[] _dataFloatAnswerDiff7times = new float[] {361};
	  internal float[] _dataNullFloat = null;

	  /* long data */
	  internal long[] _dataLong = new long[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal long[] _dataLongAnswerDiff0times = new long[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal long[] _dataLongAnswerDiff1times = new long[] {4, -3, 6, 1, 13, -12, 2};
	  internal long[] _dataLongAnswerDiff2times = new long[] {-7, 9, -5, 12, -25, 14};
	  internal long[] _dataLongAnswerDiff3times = new long[] {16, -14, 17, -37, 39};
	  internal long[] _dataLongAnswerDiff4times = new long[] {-30, 31, -54, 76};
	  internal long[] _dataLongAnswerDiff5times = new long[] {61, -85, 130};
	  internal long[] _dataLongAnswerDiff6times = new long[] {-146, 215};
	  internal long[] _dataLongAnswerDiff7times = new long[] {361};
	  internal long[] _dataNullLong = null;

	  /* int data */
	  internal int[] _dataInteger = new int[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal int[] _dataIntegerAnswerDiff0times = new int[] {-7, -3, -6, 0, 1, 14, 2, 4};
	  internal int[] _dataIntegerAnswerDiff1times = new int[] {4, -3, 6, 1, 13, -12, 2};
	  internal int[] _dataIntegerAnswerDiff2times = new int[] {-7, 9, -5, 12, -25, 14};
	  internal int[] _dataIntegerAnswerDiff3times = new int[] {16, -14, 17, -37, 39};
	  internal int[] _dataIntegerAnswerDiff4times = new int[] {-30, 31, -54, 76};
	  internal int[] _dataIntegerAnswerDiff5times = new int[] {61, -85, 130};
	  internal int[] _dataIntegerAnswerDiff6times = new int[] {-146, 215};
	  internal int[] _dataIntegerAnswerDiff7times = new int[] {361};
	  internal int[] _dataNullInteger = null;

	  /* test doubles */
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffDoubleNull()
	  public virtual void testDiffDoubleNull()
	  {
		Diff.values(_dataNullDouble);
	  }

	  public virtual void testDiffDouble()
	  {
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff1times, Diff.values(_dataDouble)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffDoubleNtimesDoubleNull()
	  public virtual void testDiffDoubleNtimesDoubleNull()
	  {
		Diff.values(_dataNullDouble, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffDoubleNtimesTtooLarge()
	  public virtual void testDiffDoubleNtimesTtooLarge()
	  {
		Diff.values(_dataDouble, 8);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffDoubleNtimesTtooSmall()
	  public virtual void testDiffDoubleNtimesTtooSmall()
	  {
		Diff.values(_dataDouble, -1);
	  }

	  public virtual void testDiffDoubleNtimes()
	  {
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff0times, Diff.values(_dataDouble, 0)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff1times, Diff.values(_dataDouble, 1)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff2times, Diff.values(_dataDouble, 2)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff3times, Diff.values(_dataDouble, 3)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff4times, Diff.values(_dataDouble, 4)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff5times, Diff.values(_dataDouble, 5)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff6times, Diff.values(_dataDouble, 6)));
		assertTrue(Arrays.Equals(_dataDoubleAnswerDiff7times, Diff.values(_dataDouble, 7)));
	  }

	  /* test floats */
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffFloatNull()
	  public virtual void testDiffFloatNull()
	  {
		Diff.values(_dataNullFloat);
	  }

	  public virtual void testDiffFloat()
	  {
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff1times, Diff.values(_dataFloat)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffFloatNtimesFloatNull()
	  public virtual void testDiffFloatNtimesFloatNull()
	  {
		Diff.values(_dataNullFloat, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffFloatNtimesTtooLarge()
	  public virtual void testDiffFloatNtimesTtooLarge()
	  {
		Diff.values(_dataFloat, 8);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffFloatNtimesTtooSmall()
	  public virtual void testDiffFloatNtimesTtooSmall()
	  {
		Diff.values(_dataFloat, -1);
	  }

	  public virtual void testDiffFloatNtimes()
	  {
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff0times, Diff.values(_dataFloat, 0)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff1times, Diff.values(_dataFloat, 1)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff2times, Diff.values(_dataFloat, 2)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff3times, Diff.values(_dataFloat, 3)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff4times, Diff.values(_dataFloat, 4)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff5times, Diff.values(_dataFloat, 5)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff6times, Diff.values(_dataFloat, 6)));
		assertTrue(Arrays.Equals(_dataFloatAnswerDiff7times, Diff.values(_dataFloat, 7)));
	  }

	  /* test integers */
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffIntegerNull()
	  public virtual void testDiffIntegerNull()
	  {
		Diff.values(_dataNullInteger);
	  }

	  public virtual void testDiffInteger()
	  {
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff1times, Diff.values(_dataInteger)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffIntegerNtimesIntegerNull()
	  public virtual void testDiffIntegerNtimesIntegerNull()
	  {
		Diff.values(_dataNullInteger, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffIntegerNtimesTtooLarge()
	  public virtual void testDiffIntegerNtimesTtooLarge()
	  {
		Diff.values(_dataInteger, 8);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffIntegerNtimesTtooSmall()
	  public virtual void testDiffIntegerNtimesTtooSmall()
	  {
		Diff.values(_dataInteger, -1);
	  }

	  public virtual void testDiffIntegerNtimes()
	  {
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff0times, Diff.values(_dataInteger, 0)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff1times, Diff.values(_dataInteger, 1)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff2times, Diff.values(_dataInteger, 2)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff3times, Diff.values(_dataInteger, 3)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff4times, Diff.values(_dataInteger, 4)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff5times, Diff.values(_dataInteger, 5)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff6times, Diff.values(_dataInteger, 6)));
		assertTrue(Arrays.Equals(_dataIntegerAnswerDiff7times, Diff.values(_dataInteger, 7)));
	  }

	  /* test longs */
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffLongNull()
	  public virtual void testDiffLongNull()
	  {
		Diff.values(_dataNullLong);
	  }

	  public virtual void testDiffLong()
	  {
		assertTrue(Arrays.Equals(_dataLongAnswerDiff1times, Diff.values(_dataLong)));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffLongNtimesLongNull()
	  public virtual void testDiffLongNtimesLongNull()
	  {
		Diff.values(_dataNullLong, 1);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffLongNtimesTtooLarge()
	  public virtual void testDiffLongNtimesTtooLarge()
	  {
		Diff.values(_dataLong, 8);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testDiffLongNtimesTtooSmall()
	  public virtual void testDiffLongNtimesTtooSmall()
	  {
		Diff.values(_dataLong, -1);
	  }

	  public virtual void testDiffLongNtimes()
	  {
		assertTrue(Arrays.Equals(_dataLongAnswerDiff0times, Diff.values(_dataLong, 0)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff1times, Diff.values(_dataLong, 1)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff2times, Diff.values(_dataLong, 2)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff3times, Diff.values(_dataLong, 3)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff4times, Diff.values(_dataLong, 4)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff5times, Diff.values(_dataLong, 5)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff6times, Diff.values(_dataLong, 6)));
		assertTrue(Arrays.Equals(_dataLongAnswerDiff7times, Diff.values(_dataLong, 7)));
	  }

	}

}