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
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using CalculationParameters = com.opengamma.strata.calc.runner.CalculationParameters;
	using TestParameter = com.opengamma.strata.calc.runner.TestParameter;

	/// <summary>
	/// Test <seealso cref="Column"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ColumnTest
	public class ColumnTest
	{

	  private static readonly CalculationParameter PARAM = new TestParameter();

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_columnNameFromMeasure()
	  {
		Column test = Column.builder().measure(TestingMeasures.PRESENT_VALUE).build();
		assertEquals(test.Name, ColumnName.of(TestingMeasures.PRESENT_VALUE.Name));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, null);
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_builder_columnNameSpecified()
	  {
		Column test = Column.builder().measure(TestingMeasures.PRESENT_VALUE).name(ColumnName.of("NPV")).reportingCurrency(ReportingCurrency.NATURAL).build();
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.NATURAL);
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_builder_missingData()
	  {
		assertThrowsIllegalArg(() => Column.builder().build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_Measure()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE);
		assertEquals(test.Name, ColumnName.of(TestingMeasures.PRESENT_VALUE.Name));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, null);
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_of_MeasureCurrency()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, USD);
		assertEquals(test.Name, ColumnName.of(TestingMeasures.PRESENT_VALUE.Name));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_of_MeasureCalculationParameters()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, PARAM);
		assertEquals(test.Name, ColumnName.of(TestingMeasures.PRESENT_VALUE.Name));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, null);
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_MeasureCurrencyCalculationParameters()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, USD, PARAM);
		assertEquals(test.Name, ColumnName.of(TestingMeasures.PRESENT_VALUE.Name));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_MeasureString()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV");
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, null);
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_of_MeasureStringCurrency()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV", USD);
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.empty());
	  }

	  public virtual void test_of_MeasureStringCalculationParameters()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV", PARAM);
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, null);
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  public virtual void test_of_MeasureStringCurrencyCalculationParameters()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV", USD, PARAM);
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.ReportingCurrency, ReportingCurrency.of(USD));
		assertEquals(test.Parameters, CalculationParameters.of(PARAM));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toHeader_withCurrency()
	  {
		ColumnHeader test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV", USD).toHeader();
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.Currency, USD);
	  }

	  public virtual void test_toHeader_withoutCurrency()
	  {
		ColumnHeader test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV").toHeader();
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PRESENT_VALUE);
		assertEquals(test.Currency, null);
	  }

	  public virtual void test_toHeader_withNonConvertibleMeasure()
	  {
		ColumnHeader test = Column.of(TestingMeasures.PAR_RATE, "NPV", USD).toHeader();
		assertEquals(test.Name, ColumnName.of("NPV"));
		assertEquals(test.Measure, TestingMeasures.PAR_RATE);
		assertEquals(test.Currency, null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		Column test = Column.of(TestingMeasures.PRESENT_VALUE, "NPV", USD, PARAM);
		coverImmutableBean(test);
		Column test2 = Column.of(TestingMeasures.CASH_FLOWS);
		coverBeanEquals(test, test2);
	  }

	}

}