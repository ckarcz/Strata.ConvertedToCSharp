/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// Test <seealso cref="IndexQuoteId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IndexQuoteIdTest
	public class IndexQuoteIdTest
	{

	  private static readonly FieldName FIELD = FieldName.of("Field");
	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_1arg()
	  {
		IndexQuoteId test = IndexQuoteId.of(GBP_SONIA);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.FieldName, FieldName.MARKET_VALUE);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.StandardId, StandardId.of("OG-Index", GBP_SONIA.Name));
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "IndexQuoteId:GBP-SONIA/MarketValue");
	  }

	  public virtual void test_of_2args()
	  {
		IndexQuoteId test = IndexQuoteId.of(GBP_SONIA, FIELD);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.FieldName, FIELD);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.StandardId, StandardId.of("OG-Index", GBP_SONIA.Name));
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "IndexQuoteId:GBP-SONIA/Field");
	  }

	  public virtual void test_of_3args()
	  {
		IndexQuoteId test = IndexQuoteId.of(GBP_SONIA, FIELD, OBS_SOURCE);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.FieldName, FIELD);
		assertEquals(test.ObservableSource, OBS_SOURCE);
		assertEquals(test.StandardId, StandardId.of("OG-Index", GBP_SONIA.Name));
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "IndexQuoteId:GBP-SONIA/Field/Vendor");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IndexQuoteId test = IndexQuoteId.of(GBP_SONIA);
		coverImmutableBean(test);
		IndexQuoteId test2 = IndexQuoteId.of(USD_FED_FUND, FIELD, OBS_SOURCE);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		IndexQuoteId test = IndexQuoteId.of(GBP_SONIA);
		assertSerialization(test);
	  }

	}

}