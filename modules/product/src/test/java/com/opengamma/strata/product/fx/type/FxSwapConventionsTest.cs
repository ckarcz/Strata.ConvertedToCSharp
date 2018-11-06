namespace com.opengamma.strata.product.fx.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.EUTA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.GBLO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.JPTO;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.HolidayCalendarIds.USNY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using DataProvider = org.testng.annotations.DataProvider;
	using Test = org.testng.annotations.Test;

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using HolidayCalendarId = com.opengamma.strata.basics.date.HolidayCalendarId;

	/// <summary>
	/// Test <seealso cref="FxSwapConventions"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxSwapConventionsTest
	public class FxSwapConventionsTest
	{

	  private static readonly HolidayCalendarId EUTA_USNY = EUTA.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_EUTA = GBLO.combinedWith(EUTA);
	  private static readonly HolidayCalendarId GBLO_USNY = GBLO.combinedWith(USNY);
	  private static readonly HolidayCalendarId GBLO_JPTO = GBLO.combinedWith(JPTO);

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "spotLag") public static Object[][] data_spot_lag()
	  public static object[][] data_spot_lag()
	  {
		return new object[][]
		{
			new object[] {FxSwapConventions.EUR_USD, 2},
			new object[] {FxSwapConventions.EUR_GBP, 2},
			new object[] {FxSwapConventions.GBP_USD, 2},
			new object[] {FxSwapConventions.GBP_JPY, 2}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "spotLag") public void test_spot_lag(ImmutableFxSwapConvention convention, int lag)
	  public virtual void test_spot_lag(ImmutableFxSwapConvention convention, int lag)
	  {
		assertEquals(convention.SpotDateOffset.Days, lag);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "currencyPair") public static Object[][] data_currency_pair()
	  public static object[][] data_currency_pair()
	  {
		return new object[][]
		{
			new object[] {FxSwapConventions.EUR_USD, CurrencyPair.of(EUR, USD)},
			new object[] {FxSwapConventions.EUR_GBP, CurrencyPair.of(EUR, GBP)},
			new object[] {FxSwapConventions.GBP_USD, CurrencyPair.of(GBP, USD)},
			new object[] {FxSwapConventions.GBP_JPY, CurrencyPair.of(GBP, JPY)}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "currencyPair") public void test_currency_pair(ImmutableFxSwapConvention convention, com.opengamma.strata.basics.currency.CurrencyPair ccys)
	  public virtual void test_currency_pair(ImmutableFxSwapConvention convention, CurrencyPair ccys)
	  {
		assertEquals(convention.CurrencyPair, ccys);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DataProvider(name = "calendar") public static Object[][] data_calendar()
	  public static object[][] data_calendar()
	  {
		return new object[][]
		{
			new object[] {FxSwapConventions.EUR_USD, EUTA_USNY},
			new object[] {FxSwapConventions.EUR_GBP, GBLO_EUTA},
			new object[] {FxSwapConventions.GBP_USD, GBLO_USNY},
			new object[] {FxSwapConventions.GBP_JPY, GBLO_JPTO}
		};
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test(dataProvider = "calendar") public void test_calendar(ImmutableFxSwapConvention convention, com.opengamma.strata.basics.date.HolidayCalendarId cal)
	  public virtual void test_calendar(ImmutableFxSwapConvention convention, HolidayCalendarId cal)
	  {
		assertEquals(convention.SpotDateOffset.Calendar, cal);
		assertEquals(convention.BusinessDayAdjustment.Calendar, cal);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(FxSwapConventions));
		coverPrivateConstructor(typeof(StandardFxSwapConventions));
	  }

	}

}