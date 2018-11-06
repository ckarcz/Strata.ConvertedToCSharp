/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverEnum;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ReportingCurrency"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ReportingCurrencyTest
	public class ReportingCurrencyTest
	{

	  public virtual void test_NATURAL()
	  {
		ReportingCurrency test = ReportingCurrency.NATURAL;
		assertEquals(test.Type, ReportingCurrencyType.NATURAL);
		assertEquals(test.Specific, false);
		assertEquals(test.Natural, true);
		assertEquals(test.None, false);
		assertEquals(test.ToString(), "Natural");
		assertThrows(() => test.Currency, typeof(System.InvalidOperationException));
	  }

	  public virtual void test_NONE()
	  {
		ReportingCurrency test = ReportingCurrency.NONE;
		assertEquals(test.Type, ReportingCurrencyType.NONE);
		assertEquals(test.Specific, false);
		assertEquals(test.Natural, false);
		assertEquals(test.None, true);
		assertEquals(test.ToString(), "None");
		assertThrows(() => test.Currency, typeof(System.InvalidOperationException));
	  }

	  public virtual void test_of_specific()
	  {
		ReportingCurrency test = ReportingCurrency.of(USD);
		assertEquals(test.Type, ReportingCurrencyType.SPECIFIC);
		assertEquals(test.Specific, true);
		assertEquals(test.Natural, false);
		assertEquals(test.None, false);
		assertEquals(test.Currency, USD);
		assertEquals(test.ToString(), "Specific:USD");
	  }

	  public virtual void test_type()
	  {
		assertEquals(ReportingCurrencyType.of("Specific").ToString(), "Specific");
		assertEquals(ReportingCurrencyType.of("Natural").ToString(), "Natural");
		assertEquals(ReportingCurrencyType.of("None").ToString(), "None");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ReportingCurrency test = ReportingCurrency.NATURAL;
		coverImmutableBean(test);
		ReportingCurrency test2 = ReportingCurrency.of(USD);
		coverBeanEquals(test, test2);
		coverEnum(typeof(ReportingCurrencyType));
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(ReportingCurrency.NATURAL);
		assertSerialization(ReportingCurrency.of(USD));
	  }

	}

}