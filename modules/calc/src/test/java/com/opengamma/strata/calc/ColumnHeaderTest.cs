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
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ColumnHeader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ColumnHeaderTest
	public class ColumnHeaderTest
	{

	  public virtual void test_of_NameMeasure()
	  {
		ColumnHeader test = ColumnHeader.of(ColumnName.of("ParRate"), TestingMeasures.PAR_RATE);
		assertEquals(test.Name, ColumnName.of("ParRate"));
		assertEquals(test.Measure, TestingMeasures.PAR_RATE);
		assertEquals(test.Currency, null);
	  }

	  public virtual void test_of_NameMeasureCurrency()
	  {
		ColumnHeader test = ColumnHeader.of(ColumnName.of("NPV"), TestingMeasures.PRESENT_VALUE, USD);
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.Currency, USD);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ColumnHeader test = ColumnHeader.of(ColumnName.of("NPV"), TestingMeasures.PRESENT_VALUE, USD);
		coverImmutableBean(test);
		ColumnHeader test2 = ColumnHeader.of(ColumnName.of("ParRate"), TestingMeasures.PAR_RATE);
		coverBeanEquals(test, test2);
	  }

	}

}