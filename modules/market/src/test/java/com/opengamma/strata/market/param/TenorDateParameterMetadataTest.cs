/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_10Y;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_12M;
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
	/// Test <seealso cref="TenorDateParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorDateParameterMetadataTest
	public class TenorDateParameterMetadataTest
	{

	  private static readonly LocalDate DATE = date(2015, 7, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noLabel()
	  {
		TenorDateParameterMetadata test = TenorDateParameterMetadata.of(DATE, TENOR_10Y);
		assertEquals(test.Date, DATE);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10Y");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_of_label()
	  {
		TenorDateParameterMetadata test = TenorDateParameterMetadata.of(DATE, TENOR_10Y, "10 year");
		assertEquals(test.Date, DATE);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10 year");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_defaultLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		BeanBuilder<TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		builder.set(TenorDateParameterMetadata.meta().date(), DATE);
		builder.set(TenorDateParameterMetadata.meta().tenor(), TENOR_10Y);
		TenorDateParameterMetadata test = builder.build();
		assertEquals(test.Date, DATE);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10Y");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_specifyLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		BeanBuilder<TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		builder.set(TenorDateParameterMetadata.meta().date(), DATE);
		builder.set(TenorDateParameterMetadata.meta().tenor(), TENOR_10Y);
		builder.set(TenorDateParameterMetadata.meta().label(), "10 year");
		TenorDateParameterMetadata test = builder.build();
		assertEquals(test.Date, DATE);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10 year");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		BeanBuilder<TenorDateParameterMetadata> builder = TenorDateParameterMetadata.meta().builder();
		builder.set(TenorDateParameterMetadata.meta().date(), DATE);
		assertThrowsIllegalArg(() => builder.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TenorDateParameterMetadata test = TenorDateParameterMetadata.of(DATE, TENOR_10Y);
		coverImmutableBean(test);
		TenorDateParameterMetadata test2 = TenorDateParameterMetadata.of(date(2014, 1, 1), TENOR_12M);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		TenorDateParameterMetadata test = TenorDateParameterMetadata.of(DATE, TENOR_10Y);
		assertSerialization(test);
	  }

	}

}