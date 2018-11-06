/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="SsviFormulaData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SsviFormulaDataTest
	public class SsviFormulaDataTest
	{

	  private const double SIGMA = 0.20;
	  private const double RHO = -0.20;
	  private const double ETA = 0.50;
	  private static readonly SsviFormulaData DATA = SsviFormulaData.of(SIGMA, RHO, ETA);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void test()
	  public virtual void test()
	  {
		assertEquals(DATA.Sigma, SIGMA, 0);
		assertEquals(DATA.Rho, RHO, 0);
		assertEquals(DATA.Eta, ETA, 0);
		assertEquals(DATA.getParameter(0), SIGMA, 0);
		assertEquals(DATA.getParameter(1), RHO, 0);
		assertEquals(DATA.getParameter(2), ETA, 0);
		assertEquals(DATA.NumberOfParameters, 3);
		SsviFormulaData other = SsviFormulaData.of(new double[] {SIGMA, RHO, ETA});
		assertEquals(other, DATA);
		assertEquals(other.GetHashCode(), DATA.GetHashCode());

		other = other.with(0, SIGMA - 0.01);
		assertFalse(other.Equals(DATA));
		other = SsviFormulaData.of(SIGMA * 0.5, RHO, ETA);
		assertFalse(other.Equals(DATA));
		other = SsviFormulaData.of(SIGMA, RHO * 0.5, ETA);
		assertFalse(other.Equals(DATA));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNegativeEta()
	  public virtual void testNegativeEta()
	  {
		assertThrowsIllegalArg(() => SsviFormulaData.of(SIGMA, RHO, -ETA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testNegativeSigma()
	  public virtual void testNegativeSigma()
	  {
		assertThrowsIllegalArg(() => SsviFormulaData.of(-SIGMA, RHO, ETA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testLowRho()
	  public virtual void testLowRho()
	  {
		assertThrowsIllegalArg(() => SsviFormulaData.of(SIGMA, RHO - 10, ETA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testHighRho()
	  public virtual void testHighRho()
	  {
		assertThrowsIllegalArg(() => SsviFormulaData.of(SIGMA, RHO + 10, ETA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testWrongIndex()
	  public virtual void testWrongIndex()
	  {
		assertThrowsIllegalArg(() => DATA.isAllowed(-1, ETA));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public void testWrongParameterLength()
	  public virtual void testWrongParameterLength()
	  {
		assertThrowsIllegalArg(() => SsviFormulaData.of(new double[] {ETA, RHO, SIGMA, 0.1}));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(DATA);
		SsviFormulaData another = SsviFormulaData.of(1.2, 0.4, 0.2);
		coverBeanEquals(DATA, another);
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(DATA);
	  }

	}

}