/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using BeanBuilder = org.joda.beans.BeanBuilder;
	using Test = org.testng.annotations.Test;

	using Pair = com.opengamma.strata.collect.tuple.Pair;

	/// <summary>
	/// Test <seealso cref="SwaptionSurfaceExpiryTenorParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionSurfaceExpiryTenorParameterMetadataTest
	public class SwaptionSurfaceExpiryTenorParameterMetadataTest
	{

	  private const double TIME_TO_EXPIRY = 1.5d;
	  private const double TENOR = 36d;

	  public virtual void test_of_noLabel()
	  {
		SwaptionSurfaceExpiryTenorParameterMetadata test = SwaptionSurfaceExpiryTenorParameterMetadata.of(TIME_TO_EXPIRY, TENOR);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, TENOR));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, TENOR).ToString());
		assertEquals(test.Tenor, TENOR);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		string label = "(1.5Y, 36M)";
		SwaptionSurfaceExpiryTenorParameterMetadata test = SwaptionSurfaceExpiryTenorParameterMetadata.of(TIME_TO_EXPIRY, TENOR, label);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, TENOR));
		assertEquals(test.Label, label);
		assertEquals(test.Tenor, TENOR);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpiryTenorParameterMetadata> builder = SwaptionSurfaceExpiryTenorParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpiryTenorParameterMetadata> builder = SwaptionSurfaceExpiryTenorParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, TENOR);
		builder.set(SwaptionSurfaceExpiryTenorParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpiryTenorParameterMetadata.meta().tenor(), TENOR);
		SwaptionSurfaceExpiryTenorParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, pair.ToString());
		assertEquals(test.Tenor, TENOR);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpiryTenorParameterMetadata> builder = SwaptionSurfaceExpiryTenorParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpiryTenorParameterMetadata> builder = SwaptionSurfaceExpiryTenorParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, TENOR);
		string label = "(1.5Y, 36M)";
		builder.set(SwaptionSurfaceExpiryTenorParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpiryTenorParameterMetadata.meta().tenor(), TENOR);
		builder.set(SwaptionSurfaceExpiryTenorParameterMetadata.meta().label(), label);
		SwaptionSurfaceExpiryTenorParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Tenor, TENOR);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwaptionSurfaceExpiryTenorParameterMetadata test1 = SwaptionSurfaceExpiryTenorParameterMetadata.of(TIME_TO_EXPIRY, TENOR);
		coverImmutableBean(test1);
		SwaptionSurfaceExpiryTenorParameterMetadata test2 = SwaptionSurfaceExpiryTenorParameterMetadata.of(2.5d, 60d, "(2.5, 60)");
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwaptionSurfaceExpiryTenorParameterMetadata test = SwaptionSurfaceExpiryTenorParameterMetadata.of(TIME_TO_EXPIRY, TENOR);
		assertSerialization(test);
	  }

	}

}