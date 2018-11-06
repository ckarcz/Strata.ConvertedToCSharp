using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.random
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;

	using Test = org.testng.annotations.Test;

	using MersenneTwister64 = com.opengamma.strata.math.impl.cern.MersenneTwister64;

	/// <summary>
	/// Test <seealso cref="NormalRandomNumberGenerator"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NormalRandomNumberGeneratorTest
	public class NormalRandomNumberGeneratorTest
	{

	  private static readonly NormalRandomNumberGenerator GENERATOR = new NormalRandomNumberGenerator(0, 1);

	  public virtual void test_array()
	  {
		double[] result = GENERATOR.getVector(10);
		assertEquals(result.Length, 10);
	  }

	  public virtual void test_list()
	  {
		IList<double[]> result = GENERATOR.getVectors(10, 50);
		assertEquals(result.Count, 50);
		foreach (double[] d in result)
		{
		  assertEquals(d.Length, 10);
		}
	  }

	  public virtual void test_invalid()
	  {
		assertThrowsIllegalArg(() => new NormalRandomNumberGenerator(0, -1));
		assertThrowsIllegalArg(() => new NormalRandomNumberGenerator(0, -1, new MersenneTwister64()));
		assertThrowsIllegalArg(() => new NormalRandomNumberGenerator(0, 1, null));
		assertThrowsIllegalArg(() => GENERATOR.getVectors(-1, 4));
		assertThrowsIllegalArg(() => GENERATOR.getVectors(1, -5));
	  }

	}

}