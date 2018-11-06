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
//	import static org.testng.Assert.assertEquals;

	using BeanBuilder = org.joda.beans.BeanBuilder;
	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="TenorParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorParameterMetadataTest
	public class TenorParameterMetadataTest
	{

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noLabel()
	  {
		TenorParameterMetadata test = TenorParameterMetadata.of(TENOR_10Y);
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10Y");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_of_label()
	  {
		TenorParameterMetadata test = TenorParameterMetadata.of(TENOR_10Y, "10 year");
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10 year");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_defaultLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		BeanBuilder<TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		builder.set(TenorParameterMetadata.meta().tenor(), TENOR_10Y);
		TenorParameterMetadata test = builder.build();
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10Y");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_specifyLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		BeanBuilder<TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		builder.set(TenorParameterMetadata.meta().tenor(), TENOR_10Y);
		builder.set(TenorParameterMetadata.meta().label(), "10 year");
		TenorParameterMetadata test = builder.build();
		assertEquals(test.Tenor, TENOR_10Y);
		assertEquals(test.Label, "10 year");
		assertEquals(test.Identifier, TENOR_10Y);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		BeanBuilder<TenorParameterMetadata> builder = TenorParameterMetadata.meta().builder();
		assertThrowsIllegalArg(() => builder.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TenorParameterMetadata test = TenorParameterMetadata.of(TENOR_10Y);
		coverImmutableBean(test);
		TenorParameterMetadata test2 = TenorParameterMetadata.of(TENOR_12M);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		TenorParameterMetadata test = TenorParameterMetadata.of(TENOR_10Y);
		assertSerialization(test);
	  }

	}

}