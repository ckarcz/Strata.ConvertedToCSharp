/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DecompositionFactoryTest
	public class DecompositionFactoryTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadName()
		public virtual void testBadName()
		{
		DecompositionFactory.getDecomposition("X");
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNullDecomposition()
	  public virtual void testNullDecomposition()
	  {
		DecompositionFactory.getDecompositionName(null);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(DecompositionFactory.LU_COMMONS_NAME, DecompositionFactory.getDecompositionName(DecompositionFactory.getDecomposition(DecompositionFactory.LU_COMMONS_NAME)));
		assertEquals(DecompositionFactory.QR_COMMONS_NAME, DecompositionFactory.getDecompositionName(DecompositionFactory.getDecomposition(DecompositionFactory.QR_COMMONS_NAME)));
		assertEquals(DecompositionFactory.SV_COMMONS_NAME, DecompositionFactory.getDecompositionName(DecompositionFactory.getDecomposition(DecompositionFactory.SV_COMMONS_NAME)));
	  }

	}

}