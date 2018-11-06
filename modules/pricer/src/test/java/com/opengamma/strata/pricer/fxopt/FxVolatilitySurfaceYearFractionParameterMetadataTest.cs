/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
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

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using MoneynessStrike = com.opengamma.strata.market.option.MoneynessStrike;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using Strike = com.opengamma.strata.market.option.Strike;

	/// <summary>
	/// Test <seealso cref="FxVolatilitySurfaceYearFractionParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxVolatilitySurfaceYearFractionParameterMetadataTest
	public class FxVolatilitySurfaceYearFractionParameterMetadataTest
	{

	  private const double TIME_TO_EXPIRY = 1.5d;
	  private static readonly DeltaStrike STRIKE = DeltaStrike.of(0.75d);
	  private static readonly SimpleStrike STRIKE1 = SimpleStrike.of(1.35);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(GBP, USD);

	  public virtual void test_of_withStrikeType()
	  {
		FxVolatilitySurfaceYearFractionParameterMetadata test = FxVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE, CURRENCY_PAIR);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Identifier, Pair.of(TIME_TO_EXPIRY, STRIKE));
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE.Label).ToString());
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_of_withLabel()
	  {
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE1);
		string label = "(1.5, 1.35)";
		FxVolatilitySurfaceYearFractionParameterMetadata test = FxVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE1, label, CURRENCY_PAIR);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE1);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_noLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends FxVolatilitySurfaceYearFractionParameterMetadata> builder = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata> builder = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().currencyPair(), CURRENCY_PAIR);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().strike(), STRIKE);
		FxVolatilitySurfaceYearFractionParameterMetadata test = builder.build();
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, Pair.of(TIME_TO_EXPIRY, STRIKE.Label).ToString());
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_withLabel()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends FxVolatilitySurfaceYearFractionParameterMetadata> builder = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata> builder = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		Pair<double, Strike> pair = Pair.of(TIME_TO_EXPIRY, STRIKE);
		string label = "(1.5, 0.75)";
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().currencyPair(), CURRENCY_PAIR);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().strike(), STRIKE);
		builder.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().label(), label);
		FxVolatilitySurfaceYearFractionParameterMetadata test = builder.build();
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Identifier, pair);
		assertEquals(test.Label, label);
		assertEquals(test.Strike, STRIKE);
		assertEquals(test.YearFraction, TIME_TO_EXPIRY);
	  }

	  public virtual void test_builder_incomplete()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends FxVolatilitySurfaceYearFractionParameterMetadata> builder1 = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata> builder1 = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		assertThrowsIllegalArg(() => builder1.build());
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: org.joda.beans.BeanBuilder<? extends FxVolatilitySurfaceYearFractionParameterMetadata> builder2 = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		BeanBuilder<FxVolatilitySurfaceYearFractionParameterMetadata> builder2 = FxVolatilitySurfaceYearFractionParameterMetadata.meta().builder();
		builder2.set(FxVolatilitySurfaceYearFractionParameterMetadata.meta().yearFraction(), TIME_TO_EXPIRY);
		assertThrowsIllegalArg(() => builder2.build());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxVolatilitySurfaceYearFractionParameterMetadata test1 = FxVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE, CURRENCY_PAIR);
		coverImmutableBean(test1);
		FxVolatilitySurfaceYearFractionParameterMetadata test2 = FxVolatilitySurfaceYearFractionParameterMetadata.of(3d, MoneynessStrike.of(1.1d), CurrencyPair.of(EUR, AUD));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxVolatilitySurfaceYearFractionParameterMetadata test = FxVolatilitySurfaceYearFractionParameterMetadata.of(TIME_TO_EXPIRY, STRIKE, CURRENCY_PAIR);
		assertSerialization(test);
	  }

	}

}