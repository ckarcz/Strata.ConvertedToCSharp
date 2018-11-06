/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
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
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using BeanBuilder = org.joda.beans.BeanBuilder;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="YearMonthDateParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class YearMonthDateParameterMetadataTest
	public class YearMonthDateParameterMetadataTest
	{

	  private static readonly LocalDate DATE = date(2015, 7, 30);
	  private static readonly YearMonth JAN2015 = YearMonth.of(2015, 1);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noLabel()
	  {
		YearMonthDateParameterMetadata test = YearMonthDateParameterMetadata.of(DATE, JAN2015);
		assertEquals(test.Date, DATE);
		assertEquals(test.YearMonth, JAN2015);
		assertEquals(test.Label, "Jan15");
		assertEquals(test.Identifier, JAN2015);
	  }

	  public virtual void test_of_label()
	  {
		YearMonthDateParameterMetadata test = YearMonthDateParameterMetadata.of(DATE, JAN2015, "Jan 2015");
		assertEquals(test.Date, DATE);
		assertEquals(test.YearMonth, JAN2015);
		assertEquals(test.Label, "Jan 2015");
		assertEquals(test.Identifier, JAN2015);
	  }

	  public virtual void test_builder_defaultLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		BeanBuilder<YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		builder.set(YearMonthDateParameterMetadata.meta().date(), DATE);
		builder.set(YearMonthDateParameterMetadata.meta().yearMonth(), JAN2015);
		YearMonthDateParameterMetadata test = builder.build();
		assertEquals(test.Date, DATE);
		assertEquals(test.YearMonth, JAN2015);
		assertEquals(test.Label, "Jan15");
		assertEquals(test.Identifier, JAN2015);
	  }

	  public virtual void test_builder_specifyLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		BeanBuilder<YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		builder.set(YearMonthDateParameterMetadata.meta().date(), DATE);
		builder.set(YearMonthDateParameterMetadata.meta().yearMonth(), JAN2015);
		builder.set(YearMonthDateParameterMetadata.meta().label(), "Jan 2015");
		YearMonthDateParameterMetadata test = builder.build();
		assertEquals(test.Date, DATE);
		assertEquals(test.YearMonth, JAN2015);
		assertEquals(test.Label, "Jan 2015");
		assertEquals(test.Identifier, JAN2015);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		BeanBuilder<YearMonthDateParameterMetadata> builder = YearMonthDateParameterMetadata.meta().builder();
		builder.set(YearMonthDateParameterMetadata.meta().date(), DATE);
		assertThrowsIllegalArg(() => builder.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		YearMonthDateParameterMetadata test = YearMonthDateParameterMetadata.of(DATE, JAN2015);
		coverImmutableBean(test);
		YearMonthDateParameterMetadata test2 = YearMonthDateParameterMetadata.of(date(2014, 1, 1), YearMonth.of(2016, 2));
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		YearMonthDateParameterMetadata test = YearMonthDateParameterMetadata.of(DATE, JAN2015);
		assertSerialization(test);
	  }

	}

}