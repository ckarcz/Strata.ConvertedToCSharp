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
	/// Test <seealso cref="SwaptionSurfaceExpirySimpleMoneynessParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SwaptionSurfaceExpirySimpleMoneynessParameterMetadataTest
	public class SwaptionSurfaceExpirySimpleMoneynessParameterMetadataTest
	{

	  private const double TIME_TO_EXPIRY = 1.5d;
	  private const double SIMPLE_MONEYNESS = 0.25d;

	  public virtual void test_of_noLabel()
	  {
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS).ToString());
		assertEquals(test.SimpleMoneyness, SIMPLE_MONEYNESS);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		string label = "(1.5Y, 0.25)";
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS, label);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS));
		assertEquals(test.Label, label);
		assertEquals(test.SimpleMoneyness, SIMPLE_MONEYNESS);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpirySimpleMoneynessParameterMetadata> builder = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpirySimpleMoneynessParameterMetadata> builder = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS);
		builder.set(SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().simpleMoneyness(), SIMPLE_MONEYNESS);
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, pair.ToString());
		assertEquals(test.SimpleMoneyness, SIMPLE_MONEYNESS);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends SwaptionSurfaceExpirySimpleMoneynessParameterMetadata> builder = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().builder();
		BeanBuilder<SwaptionSurfaceExpirySimpleMoneynessParameterMetadata> builder = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().builder();
		Pair<double, double> pair = Pair.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS);
		string label = "(1.5Y, 0.25)";
		builder.set(SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().simpleMoneyness(), SIMPLE_MONEYNESS);
		builder.set(SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.meta().label(), label);
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.SimpleMoneyness, SIMPLE_MONEYNESS);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test1 = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS);
		coverImmutableBean(test1);
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test2 = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.of(2.5d, 60d, "(2.5, 60)");
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		SwaptionSurfaceExpirySimpleMoneynessParameterMetadata test = SwaptionSurfaceExpirySimpleMoneynessParameterMetadata.of(TIME_TO_EXPIRY, SIMPLE_MONEYNESS);
		assertSerialization(test);
	  }

	}

}