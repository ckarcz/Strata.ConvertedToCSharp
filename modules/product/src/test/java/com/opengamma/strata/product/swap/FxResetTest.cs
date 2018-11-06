/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_GBP_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.FxIndices.EUR_USD_ECB;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using FxIndexObservation = com.opengamma.strata.basics.index.FxIndexObservation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxResetTest
	public class FxResetTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate DATE_2014_06_30 = date(2014, 6, 30);

	  public virtual void test_of()
	  {
		FxReset test = FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, DATE_2014_06_30, REF_DATA), GBP);
		assertEquals(test.Index, EUR_GBP_ECB);
		assertEquals(test.ReferenceCurrency, GBP);
	  }

	  public virtual void test_invalidCurrency()
	  {
		assertThrowsIllegalArg(() => FxReset.meta().builder().set(FxReset.meta().observation(), FxIndexObservation.of(EUR_USD_ECB, DATE_2014_06_30, REF_DATA)).set(FxReset.meta().referenceCurrency(), GBP).build());
		assertThrowsIllegalArg(() => FxReset.of(FxIndexObservation.of(EUR_USD_ECB, DATE_2014_06_30, REF_DATA), GBP));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxReset test = FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, DATE_2014_06_30, REF_DATA), GBP);
		coverImmutableBean(test);
		FxReset test2 = FxReset.of(FxIndexObservation.of(EUR_USD_ECB, date(2014, 1, 15), REF_DATA), USD);
		coverBeanEquals(test, test2);
		FxReset test3 = FxReset.of(FxIndexObservation.of(EUR_USD_ECB, date(2014, 1, 15), REF_DATA), EUR);
		coverBeanEquals(test2, test3);
	  }

	  public virtual void test_serialization()
	  {
		FxReset test = FxReset.of(FxIndexObservation.of(EUR_GBP_ECB, DATE_2014_06_30, REF_DATA), GBP);
		assertSerialization(test);
	  }

	}

}