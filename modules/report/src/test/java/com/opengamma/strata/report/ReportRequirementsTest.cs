/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Column = com.opengamma.strata.calc.Column;
	using Measures = com.opengamma.strata.measure.Measures;

	/// <summary>
	/// Test <seealso cref="ReportRequirements"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ReportRequirementsTest
	public class ReportRequirementsTest
	{

	  private static readonly Column COLUMN = Column.of(Measures.PRESENT_VALUE);
	  private static readonly Column COLUMN2 = Column.of(Measures.PAR_RATE);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		ReportRequirements test = sut();
		assertEquals(test.TradeMeasureRequirements, ImmutableList.of(COLUMN));
	  }

	  public virtual void test_of_array()
	  {
		ReportRequirements test = ReportRequirements.of(COLUMN, COLUMN2);
		assertEquals(test.TradeMeasureRequirements, ImmutableList.of(COLUMN, COLUMN2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  //-------------------------------------------------------------------------
	  internal static ReportRequirements sut()
	  {
		return ReportRequirements.of(ImmutableList.of(COLUMN));
	  }

	  internal static ReportRequirements sut2()
	  {
		return ReportRequirements.of(ImmutableList.of(COLUMN2));
	  }

	}

}