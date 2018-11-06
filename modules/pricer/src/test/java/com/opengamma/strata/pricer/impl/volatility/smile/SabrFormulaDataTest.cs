/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="SabrFormulaData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SabrFormulaDataTest
	public class SabrFormulaDataTest
	{

	  private const double NU = 0.8;
	  private const double RHO = -0.65;
	  private const double BETA = 0.76;
	  private const double ALPHA = 1.4;
	  private static readonly SabrFormulaData DATA = SabrFormulaData.of(ALPHA, BETA, RHO, NU);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(DATA.Alpha, ALPHA, 0);
		assertEquals(DATA.Beta, BETA, 0);
		assertEquals(DATA.Nu, NU, 0);
		assertEquals(DATA.Rho, RHO, 0);
		assertEquals(DATA.getParameter(0), ALPHA, 0);
		assertEquals(DATA.getParameter(1), BETA, 0);
		assertEquals(DATA.getParameter(2), RHO, 0);
		assertEquals(DATA.getParameter(3), NU, 0);
		assertEquals(DATA.NumberOfParameters, 4);
		SabrFormulaData other = SabrFormulaData.of(new double[] {ALPHA, BETA, RHO, NU});
		assertEquals(other, DATA);
		assertEquals(other.GetHashCode(), DATA.GetHashCode());

		other = other.with(0, ALPHA - 0.01);
		assertFalse(other.Equals(DATA));
		other = SabrFormulaData.of(ALPHA, BETA * 0.5, RHO, NU);
		assertFalse(other.Equals(DATA));
		other = SabrFormulaData.of(ALPHA, BETA, RHO, NU * 0.5);
		assertFalse(other.Equals(DATA));
		other = SabrFormulaData.of(ALPHA, BETA, RHO * 0.5, NU);
		assertFalse(other.Equals(DATA));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNegativeBETA()
	  public virtual void testNegativeBETA()
	  {
		assertThrowsIllegalArg(() => SabrFormulaData.of(ALPHA, -BETA, RHO, NU));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNegativeNu()
	  public virtual void testNegativeNu()
	  {
		assertThrowsIllegalArg(() => SabrFormulaData.of(ALPHA, BETA, RHO, -NU));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLowRho()
	  public virtual void testLowRho()
	  {
		assertThrowsIllegalArg(() => SabrFormulaData.of(ALPHA, BETA, RHO - 10, NU));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHighRho()
	  public virtual void testHighRho()
	  {
		assertThrowsIllegalArg(() => SabrFormulaData.of(ALPHA, BETA, RHO + 10, NU));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testWrongIndex()
	  public virtual void testWrongIndex()
	  {
		assertThrowsIllegalArg(() => DATA.isAllowed(-1, ALPHA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testWrongParameterLength()
	  public virtual void testWrongParameterLength()
	  {
		assertThrowsIllegalArg(() => SabrFormulaData.of(new double[] {ALPHA, BETA, RHO, NU, 0.1}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(DATA);
		SabrFormulaData another = SabrFormulaData.of(1.2, 0.4, 0.0, 0.2);
		coverBeanEquals(DATA, another);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(DATA);
	  }

	}

}