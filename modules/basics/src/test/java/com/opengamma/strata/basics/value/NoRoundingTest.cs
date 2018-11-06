/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.value
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="NoRounding"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NoRoundingTest
	public class NoRoundingTest
	{

	  public virtual void test_none()
	  {
		Rounding test = Rounding.none();
		assertEquals(test.ToString(), "No rounding");
		assertEquals(Rounding.none(), test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void round_double_NONE()
	  {
		assertEquals(Rounding.none().round(1.23d), 1.23d);
	  }

	  public virtual void round_BigDecimal_NONE()
	  {
		assertEquals(Rounding.none().round(decimal.valueOf(1.23d)), decimal.valueOf(1.23d));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(NoRounding.INSTANCE);
	  }

	  public virtual void test_serialization()
	  {
		Rounding test = Rounding.none();
		assertSerialization(test);
	  }

	}

}