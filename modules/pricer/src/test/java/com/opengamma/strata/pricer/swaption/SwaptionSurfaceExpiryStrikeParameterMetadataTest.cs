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
	/// Test <seealso cref="SwaptionSurfaceExpiryStrikeParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionSurfaceExpiryStrikeParameterMetadataTest
	public class SwaptionSurfaceExpiryStrikeParameterMetadataTest
	{

	  private const double TIME_TO_EXPIRY = 1.5d;
	  private const double STRIKE = 0.25d;

	  public virtual void test_of_noLabel()
	  {
		SwaptionSurfaceExpiryStrikeParameterMetadata test = SwaptionSurfaceExpiryStrikeParameterMetadata.of(TIME_TO_EXPIRY, STRIKE);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, STRIKE));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE).ToString());
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		string label = "(1.5Y, 0.25)";
		SwaptionSurfaceExpiryStrikeParameterMetadata test = SwaptionSurfaceExpiryStrikeParameterMetadata.of(TIME_TO_EXPIRY, STRIKE, label);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, STRIKE));
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpiryStrikeParameterMetadata> builder = SwaptionSurfaceExpiryStrikeParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpiryStrikeParameterMetadata> builder = SwaptionSurfaceExpiryStrikeParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, STRIKE);
		builder.set(SwaptionSurfaceExpiryStrikeParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpiryStrikeParameterMetadata.meta().strike(), STRIKE);
		SwaptionSurfaceExpiryStrikeParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, pair.ToString());
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpiryStrikeParameterMetadata> builder = SwaptionSurfaceExpiryStrikeParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpiryStrikeParameterMetadata> builder = SwaptionSurfaceExpiryStrikeParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, STRIKE);
		string label = "(1.5Y, 0.25)";
		builder.set(SwaptionSurfaceExpiryStrikeParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpiryStrikeParameterMetadata.meta().strike(), STRIKE);
		builder.set(SwaptionSurfaceExpiryStrikeParameterMetadata.meta().label(), label);
		SwaptionSurfaceExpiryStrikeParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwaptionSurfaceExpiryStrikeParameterMetadata test1 = SwaptionSurfaceExpiryStrikeParameterMetadata.of(TIME_TO_EXPIRY, STRIKE);
		coverImmutableBean(test1);
		SwaptionSurfaceExpiryStrikeParameterMetadata test2 = SwaptionSurfaceExpiryStrikeParameterMetadata.of(2.5d, 60d, "(2.5, 60)");
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwaptionSurfaceExpiryStrikeParameterMetadata test = SwaptionSurfaceExpiryStrikeParameterMetadata.of(TIME_TO_EXPIRY, STRIKE);
		assertSerialization(test);
	  }

	}

}