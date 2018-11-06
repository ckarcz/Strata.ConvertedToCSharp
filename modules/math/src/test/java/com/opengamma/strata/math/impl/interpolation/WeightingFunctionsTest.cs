/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class WeightingFunctionsTest
	public class WeightingFunctionsTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadName()
		public virtual void testBadName()
		{
		WeightingFunction.of("Random");
		}

	  public virtual void test()
	  {
		assertEquals(WeightingFunction.of("Linear"), WeightingFunctions.LINEAR);
		assertEquals(WeightingFunction.of("Sine"), WeightingFunctions.SINE);
	  }

	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(WeightingFunctions));
	  }

	}

}