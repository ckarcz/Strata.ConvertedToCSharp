using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using QuoteId = com.opengamma.strata.market.observable.QuoteId;

	/// <summary>
	/// Test <seealso cref="RatesCurveInputs"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RatesCurveInputsTest
	public class RatesCurveInputsTest
	{

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> DATA_MAP = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG", "Ticker")), 6d);
	  private static readonly IDictionary<MarketDataId<object>, object> DATA_MAP = ImmutableMap.of(QuoteId.of(StandardId.of("OG", "Ticker")), 6d);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Map<com.opengamma.strata.data.MarketDataId<?>, Object> DATA_MAP2 = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.market.observable.QuoteId.of(com.opengamma.strata.basics.StandardId.of("OG", "Ticker")), 7d);
	  private static readonly IDictionary<MarketDataId<object>, object> DATA_MAP2 = ImmutableMap.of(QuoteId.of(StandardId.of("OG", "Ticker")), 7d);
	  private static readonly CurveMetadata METADATA = DefaultCurveMetadata.of("Test");
	  private static readonly CurveMetadata METADATA2 = DefaultCurveMetadata.of("Test2");

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		RatesCurveInputs test = RatesCurveInputs.of(DATA_MAP, METADATA);
		assertThat(test.MarketData).isEqualTo(DATA_MAP);
		assertThat(test.CurveMetadata).isEqualTo(METADATA);
	  }

	  public virtual void test_builder()
	  {
		RatesCurveInputs test = RatesCurveInputs.builder().marketData(DATA_MAP).curveMetadata(METADATA).build();
		assertThat(test.MarketData).isEqualTo(DATA_MAP);
		assertThat(test.CurveMetadata).isEqualTo(METADATA);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RatesCurveInputs test = RatesCurveInputs.of(DATA_MAP, METADATA);
		coverImmutableBean(test);
		RatesCurveInputs test2 = RatesCurveInputs.of(DATA_MAP2, METADATA2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RatesCurveInputs test = RatesCurveInputs.of(DATA_MAP, METADATA);
		assertSerialization(test);
	  }

	}

}