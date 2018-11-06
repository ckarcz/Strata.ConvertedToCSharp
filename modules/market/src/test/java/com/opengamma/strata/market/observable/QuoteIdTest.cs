/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{
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
	/// Test <seealso cref="QuoteId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuoteIdTest
	public class QuoteIdTest
	{

	  private static readonly StandardId ID1 = StandardId.of("OG-Ticker", "1");
	  private static readonly StandardId ID2 = StandardId.of("OG-Ticker", "2");
	  private static readonly ObservableSource OBS_SOURCE2 = ObservableSource.of("Vendor2");
	  private static readonly FieldName FIELD2 = FieldName.of("Field2");

	  //-------------------------------------------------------------------------
	  public virtual void test_of_1arg()
	  {
		QuoteId test = QuoteId.of(ID1);
		assertEquals(test.StandardId, ID1);
		assertEquals(test.FieldName, FieldName.MARKET_VALUE);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "QuoteId:OG-Ticker~1/MarketValue");
	  }

	  public virtual void test_of_2args()
	  {
		QuoteId test = QuoteId.of(ID1, FIELD2);
		assertEquals(test.StandardId, ID1);
		assertEquals(test.FieldName, FIELD2);
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "QuoteId:OG-Ticker~1/Field2");
	  }

	  public virtual void test_of_3args()
	  {
		QuoteId test = QuoteId.of(ID1, FIELD2, OBS_SOURCE2);
		assertEquals(test.StandardId, ID1);
		assertEquals(test.FieldName, FIELD2);
		assertEquals(test.ObservableSource, OBS_SOURCE2);
		assertEquals(test.MarketDataType, typeof(Double));
		assertEquals(test.ToString(), "QuoteId:OG-Ticker~1/Field2/Vendor2");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		QuoteId test = QuoteId.of(ID1);
		coverImmutableBean(test);
		QuoteId test2 = QuoteId.of(ID2, FIELD2, OBS_SOURCE2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		QuoteId test = QuoteId.of(ID1);
		assertSerialization(test);
	  }

	}

}