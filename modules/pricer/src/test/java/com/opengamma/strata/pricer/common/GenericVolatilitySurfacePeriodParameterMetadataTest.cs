/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	/// Test <seealso cref="GenericVolatilitySurfacePeriodParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class GenericVolatilitySurfacePeriodParameterMetadataTest
	public class GenericVolatilitySurfacePeriodParameterMetadataTest
	{

	  private static readonly Period TIME_TO_EXPIRY = Period.ofYears(2);
	  private static readonly LogMoneynessStrike STRIKE1 = LogMoneynessStrike.of(0.98d);
	  private static readonly SimpleStrike STRIKE2 = SimpleStrike.of(1.05);

	  public virtual void test_of_withStrikeType()
	  {
		GenericVolatilitySurfacePeriodParameterMetadata test = GenericVolatilitySurfacePeriodParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, STRIKE1));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE1.Label).ToString());
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.Period, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		Pair<Period, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE2);
		string label = "(2, 1.35)";
		GenericVolatilitySurfacePeriodParameterMetadata test = GenericVolatilitySurfacePeriodParameterMetadata.of(TIME_TO_EXPIRY, STRIKE2, label);
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE2);
		assertEquals(test.Period, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfacePeriodParameterMetadata> builder = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata> builder = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		Pair<Period, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE1);
		builder.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().period(), TIME_TO_EXPIRY);
		builder.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().strike(), STRIKE1);
		GenericVolatilitySurfacePeriodParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE1.Label).ToString());
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.Period, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfacePeriodParameterMetadata> builder = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata> builder = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		Pair<Period, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE1);
		string label = "(2, 0.75)";
		builder.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().period(), TIME_TO_EXPIRY);
		builder.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().strike(), STRIKE1);
		builder.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().label(), label);
		GenericVolatilitySurfacePeriodParameterMetadata test = builder.build();
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.Period, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfacePeriodParameterMetadata> builder1 = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata> builder1 = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		assertThrowsIllegalArg(() => builder1.build());
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends GenericVolatilitySurfacePeriodParameterMetadata> builder2 = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		BeanBuilder<GenericVolatilitySurfacePeriodParameterMetadata> builder2 = GenericVolatilitySurfacePeriodParameterMetadata.meta().builder();
		builder2.set(GenericVolatilitySurfacePeriodParameterMetadata.meta().period(), TIME_TO_EXPIRY);
		assertThrowsIllegalArg(() => builder2.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		GenericVolatilitySurfacePeriodParameterMetadata test1 = GenericVolatilitySurfacePeriodParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		coverImmutableBean(test1);
		GenericVolatilitySurfacePeriodParameterMetadata test2 = GenericVolatilitySurfacePeriodParameterMetadata.of(Period.ofMonths(3), MoneynessStrike.of(1.1d));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		GenericVolatilitySurfacePeriodParameterMetadata test = GenericVolatilitySurfacePeriodParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1);
		assertSerialization(test);
	  }

	}

}