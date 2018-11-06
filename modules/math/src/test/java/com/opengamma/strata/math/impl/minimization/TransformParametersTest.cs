using System.Collections;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertFalse;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TransformParametersTest
	public class TransformParametersTest
	{
	  private static readonly DoubleArray INIT = DoubleArray.of(1, 2, 3, 4);
	  private static readonly ParameterLimitsTransform[] NULLS = new ParameterLimitsTransform[]
	  {
		  new NullTransform(),
		  new NullTransform(),
		  new NullTransform(),
		  new NullTransform()
	  };
	  private static readonly BitArray FIXED = new BitArray(4);
	  private static readonly UncoupledParameterTransforms PARAMS;

	  static TransformParametersTest()
	  {
		FIXED.Set(0, true);
		PARAMS = new UncoupledParameterTransforms(INIT, NULLS, FIXED);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullStartValues()
	  public virtual void testNullStartValues()
	  {
		new UncoupledParameterTransforms(null, NULLS, FIXED);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullTransforms()
	  public virtual void testNullTransforms()
	  {
		new UncoupledParameterTransforms(INIT, null, FIXED);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testEmptyTransforms()
	  public virtual void testEmptyTransforms()
	  {
		new UncoupledParameterTransforms(INIT, new ParameterLimitsTransform[0], FIXED);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testNullBitSet()
	  public virtual void testNullBitSet()
	  {
		new UncoupledParameterTransforms(INIT, NULLS, null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testAllFixed()
	  public virtual void testAllFixed()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.BitSet allFixed = new java.util.BitSet();
		BitArray allFixed = new BitArray();
		allFixed.Set(0, true);
		allFixed.Set(1, true);
		allFixed.Set(2, true);
		allFixed.Set(3, true);
		new UncoupledParameterTransforms(INIT, NULLS, allFixed);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testTransformNullParameters()
	  public virtual void testTransformNullParameters()
	  {
		PARAMS.transform(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testTransformWrongParameters()
	  public virtual void testTransformWrongParameters()
	  {
		PARAMS.transform(DoubleArray.of(1, 2));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseTransformNullParameters()
	  public virtual void testInverseTransformNullParameters()
	  {
		PARAMS.inverseTransform(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseTransformWrongParameters()
	  public virtual void testInverseTransformWrongParameters()
	  {
		PARAMS.inverseTransform(DoubleArray.of(1, 2));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testJacobianNullParameters()
	  public virtual void testJacobianNullParameters()
	  {
		PARAMS.jacobian(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testJacobianWrongParameters()
	  public virtual void testJacobianWrongParameters()
	  {
		PARAMS.jacobian(DoubleArray.of(1, 2));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseJacobianNullParameters()
	  public virtual void testInverseJacobianNullParameters()
	  {
		PARAMS.inverseJacobian(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testInverseJacobianWrongParameters()
	  public virtual void testInverseJacobianWrongParameters()
	  {
		PARAMS.inverseJacobian(DoubleArray.of(1, 2));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(PARAMS.NumberOfModelParameters, 4);
		assertEquals(PARAMS.NumberOfFittingParameters, 3);
		UncoupledParameterTransforms other = new UncoupledParameterTransforms(INIT, NULLS, FIXED);
		assertEquals(PARAMS, other);
		assertEquals(PARAMS.GetHashCode(), other.GetHashCode());
		other = new UncoupledParameterTransforms(DoubleArray.of(1, 2, 4, 5), NULLS, FIXED);
		assertFalse(other.Equals(PARAMS));
		other = new UncoupledParameterTransforms(INIT, new ParameterLimitsTransform[]
		{
			new DoubleRangeLimitTransform(1, 2),
			new NullTransform(),
			new NullTransform(),
			new NullTransform()
		}, FIXED);
		assertFalse(other.Equals(PARAMS));
		other = new UncoupledParameterTransforms(INIT, NULLS, new BitArray(4));
		assertFalse(other.Equals(PARAMS));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testTransformAndInverse()
	  public virtual void testTransformAndInverse()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray functionParameters = com.opengamma.strata.collect.array.DoubleArray.of(1, 2, 6, 4);
		DoubleArray functionParameters = DoubleArray.of(1, 2, 6, 4);
		assertEquals(PARAMS.inverseTransform(PARAMS.transform(functionParameters)), functionParameters);
	  }
	}

}