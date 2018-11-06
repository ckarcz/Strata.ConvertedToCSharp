/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;

	/// <summary>
	/// Test <seealso cref="Quote"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuoteTest
	public class QuoteTest
	{
	  private static readonly QuoteId QUOTE_ID_1 = QuoteId.of(StandardId.of("og", "id1"));

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_QuoteId() throws Exception
	  public virtual void test_of_QuoteId()
	  {
		Quote test = Quote.of(QUOTE_ID_1, 1.234);
		assertEquals(test.QuoteId, QUOTE_ID_1);
		assertEquals(test.Value, 1.234);
	  }

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void test_of_nullQuoteId() throws Exception
	  public virtual void test_of_nullQuoteId()
	  {
		assertThrowsIllegalArg(() => Quote.of(null, 1.2345));
	  }

	  public virtual void coverage()
	  {
		Quote test = Quote.of(QUOTE_ID_1, 1.234);
		coverImmutableBean(test);
		Quote test2 = Quote.of(QuoteId.of(StandardId.of("a", "b")), 4.321);
		coverBeanEquals(test, test2);
	  }

	}

}