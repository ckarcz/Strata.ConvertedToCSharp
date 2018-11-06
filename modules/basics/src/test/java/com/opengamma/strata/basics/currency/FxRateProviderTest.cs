/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="FxRateProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxRateProviderTest
	public class FxRateProviderTest
	{

	  public virtual void emptyMatrixCanHandleTrivialRate()
	  {
		FxRateProvider test = (ccy1, ccy2) =>
		{
	  return 2.5d;
		};
		assertThat(test.fxRate(CurrencyPair.of(GBP, USD))).isEqualTo(2.5d);
	  }

	}

}