/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.common
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
//	import static org.testng.Assert.assertEquals;

	using BeanBuilder = org.joda.beans.BeanBuilder;
	using Test = org.testng.annotations.Test;

	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using LogMoneynessStrike = com.opengamma.strata.market.option.LogMoneynessStrike;
	using MoneynessStrike = com.opengamma.strata.market.option.MoneynessStrike;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using Strike = com.opengamma.strata.market.option.Strike;

	/// <summary>
	/// Test <seealso cref="GenericVolatilitySurfaceYearFractionParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericVolatilitySurfaceYearFractionParameterMetadataTest
	public class GenericVolatilitySurfaceYearFractionParameterMetadataTest
	{

	  private const double TIME_TO_EXPIRY = 1.5d;
	  private static readonly LogMoneynessStrike STRIKE1 = LogMoneynessStrike.of(0.98d);
	  private static readonly SimpleStrike STRIKE2 = SimpleStrike.of(1.05);

	  public virtual void test_of_withStrikeType()
	  {
		GenericVolatilitySurfaceYearFractionParameterMetadata test = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, STRIKE1));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE1.Label).ToString());
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE2);
		string label = "(1.5, 1.35)";
		GenericVolatilitySurfaceYearFractionParameterMetadata test = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE2, label);
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE2);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfaceYearFractionParameterMetadata> builder = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfaceYearFractionParameterMetadata> builder = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE1);
		builder.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().strike(), STRIKE1);
		GenericVolatilitySurfaceYearFractionParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE1.Label).ToString());
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfaceYearFractionParameterMetadata> builder = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfaceYearFractionParameterMetadata> builder = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE1);
		string label = "(1.5, 0.75)";
		builder.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().strike(), STRIKE1);
		builder.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().label(), label);
		GenericVolatilitySurfaceYearFractionParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfaceYearFractionParameterMetadata> builder1 = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfaceYearFractionParameterMetadata> builder1 = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		assertThrowsIllegalArg(() => builder1.build());
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfaceYearFractionParameterMetadata> builder2 = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfaceYearFractionParameterMetadata> builder2 = GenericVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		builder2.set(GenericVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		assertThrowsIllegalArg(() => builder2.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		GenericVolatilitySurfaceYearFractionParameterMetadata test1 = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		coverImmutableBean(test1);
		GenericVolatilitySurfaceYearFractionParameterMetadata test2 = GenericVolatilitySurfaceYearFractionParameterMetadata.of(3d, MoneynessStrike.of(1.1d));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		GenericVolatilitySurfaceYearFractionParameterMetadata test = GenericVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		assertSerialization(test);
	  }

	}

}