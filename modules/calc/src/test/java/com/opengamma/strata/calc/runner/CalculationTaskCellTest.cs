/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertNotNull;

	using Test = org.testng.annotations.Test;


	/// <summary>
	/// Test <seealso cref="CalculationTaskCell"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CalculationTaskCellTest
	public class CalculationTaskCellTest
	{

	  public virtual void of()
	  {
		CalculationTaskCell test = CalculationTaskCell.of(1, 2, TestingMeasures.PRESENT_VALUE, ReportingCurrency.of(USD));
		assertEquals(test.RowIndex, 1);
		assertEquals(test.ColumnIndex, 2);
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.ToString(), "CalculationTaskCell[(1, 2), measure=PresentValue, currency=Specific:USD]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CalculationTaskCell test = CalculationTaskCell.of(1, 2, TestingMeasures.PRESENT_VALUE, ReportingCurrency.of(USD));
		coverImmutableBean(test);
		CalculationTaskCell test2 = CalculationTaskCell.of(1, 2, TestingMeasures.PAR_RATE, ReportingCurrency.NATURAL);
		coverBeanEquals(test, test2);
		assertNotNull(CalculationTaskCell.meta());
	  }

	}

}