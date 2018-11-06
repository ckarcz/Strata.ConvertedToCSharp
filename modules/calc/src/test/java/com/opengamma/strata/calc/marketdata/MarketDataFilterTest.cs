/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using NamedMarketDataId = com.opengamma.strata.data.NamedMarketDataId;

	/// <summary>
	/// Test <seealso cref="MarketDataFilter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class MarketDataFilterTest
	public class MarketDataFilterTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_ofIdType()
	  {
		MarketDataFilter<string, MarketDataId<string>> test = MarketDataFilter.ofIdType(typeof(TestId));
		assertEquals(test.MarketDataIdType, typeof(TestId));
		assertTrue(test.matches(new TestId("a"), null, REF_DATA));
	  }

	  public virtual void test_ofId()
	  {
		MarketDataFilter<string, MarketDataId<string>> test = MarketDataFilter.ofId(new TestId("a"));
		assertEquals(test.MarketDataIdType, typeof(TestId));
		assertTrue(test.matches(new TestId("a"), null, REF_DATA));
		assertFalse(test.matches(new TestId("b"), null, REF_DATA));
	  }

	  public virtual void test_ofName()
	  {
		MarketDataFilter<string, NamedMarketDataId<string>> test = MarketDataFilter.ofName(new TestingName("a"));
		assertEquals(test.MarketDataIdType, typeof(NamedMarketDataId));
		assertTrue(test.matches(new TestingNamedId("a"), null, REF_DATA));
		assertFalse(test.matches(new TestingNamedId("b"), null, REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		MarketDataFilter<string, MarketDataId<string>> test1 = MarketDataFilter.ofIdType(typeof(TestId));
		coverImmutableBean((ImmutableBean) test1);
		MarketDataFilter<string, MarketDataId<string>> test2 = MarketDataFilter.ofId(new TestId("a"));
		coverImmutableBean((ImmutableBean) test2);
		MarketDataFilter<string, NamedMarketDataId<string>> test3 = MarketDataFilter.ofName(new TestingName("a"));
		coverImmutableBean((ImmutableBean) test3);
	  }

	}

}