/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LinearWeightingFunctionTest extends WeightingFunctionTestCase
	public class LinearWeightingFunctionTest : WeightingFunctionTestCase
	{
		protected internal override WeightingFunction Instance
		{
			get
			{
			return LinearWeightingFunction.INSTANCE;
			}
		}

	  public virtual void testWeighting()
	  {
		assertEquals(Instance.getWeight(STRIKES, INDEX, STRIKE), 0.55, EPS);
		assertEquals(Instance.getWeight(STRIKES, INDEX, STRIKES[3]), 1, EPS);
	  }

	  public virtual void testName()
	  {
		assertEquals(Instance.Name, "Linear");
	  }

	}

}