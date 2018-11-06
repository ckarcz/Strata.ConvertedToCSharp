/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//ORIGINAL LINE: @Test public class SineWeightingFunctionTest extends WeightingFunctionTestCase
	public class SineWeightingFunctionTest : WeightingFunctionTestCase
	{
		protected internal override WeightingFunction Instance
		{
			get
			{
			return SineWeightingFunction.INSTANCE;
			}
		}

	  public virtual void testName()
	  {
		assertEquals(Instance.Name, "Sine");
	  }

	}

}