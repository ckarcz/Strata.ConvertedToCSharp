/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.AssertJUnit.assertNull;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RealFunctionIntegrator1DFactoryTest
	public class RealFunctionIntegrator1DFactoryTest
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(expectedExceptions = IllegalArgumentException.class) public void testBadName()
		public virtual void testBadName()
		{
		RealFunctionIntegrator1DFactory.getIntegrator("a");
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNullCalculator()
	  public virtual void testNullCalculator()
	  {
		assertNull(RealFunctionIntegrator1DFactory.getIntegratorName(null));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(RealFunctionIntegrator1DFactory.EXTENDED_TRAPEZOID, RealFunctionIntegrator1DFactory.getIntegratorName(RealFunctionIntegrator1DFactory.getIntegrator(RealFunctionIntegrator1DFactory.EXTENDED_TRAPEZOID)));
		assertEquals(RealFunctionIntegrator1DFactory.ROMBERG, RealFunctionIntegrator1DFactory.getIntegratorName(RealFunctionIntegrator1DFactory.getIntegrator(RealFunctionIntegrator1DFactory.ROMBERG)));
		assertEquals(RealFunctionIntegrator1DFactory.SIMPSON, RealFunctionIntegrator1DFactory.getIntegratorName(RealFunctionIntegrator1DFactory.getIntegrator(RealFunctionIntegrator1DFactory.SIMPSON)));
	  }
	}

}