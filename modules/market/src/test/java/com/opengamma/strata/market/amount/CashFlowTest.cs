/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.amount
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.BasicProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Assertions = org.assertj.core.api.Assertions;
	using Offset = org.assertj.core.data.Offset;
	using Test = org.testng.annotations.Test;

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;
	using FxRate = com.opengamma.strata.basics.currency.FxRate;

	/// <summary>
	/// Test <seealso cref="CashFlow"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CashFlowTest
	public class CashFlowTest
	{

	  private static readonly Offset<double> TOLERANCE = Assertions.offset(1e-8);
	  private static readonly LocalDate PAYMENT_DATE = LocalDate.of(2015, 5, 21);
	  private const double FORECAST_VALUE = 31245.65;
	  private const double DISCOUNT_FACTOR = 0.95;
	  private static readonly double PRESENT_VALUE = FORECAST_VALUE * DISCOUNT_FACTOR;
	  private static readonly CurrencyAmount FUTURE_AMOUNT = CurrencyAmount.of(GBP, FORECAST_VALUE);
	  private static readonly CurrencyAmount PRESENT_AMOUNT = CurrencyAmount.of(GBP, PRESENT_VALUE);

	  //-------------------------------------------------------------------------
	  public virtual void test_ofPresentValue_CurrencyAmount()
	  {
		CashFlow test = CashFlow.ofPresentValue(PAYMENT_DATE, PRESENT_AMOUNT, DISCOUNT_FACTOR);
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(GBP);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(GBP);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_ofPresentValue_Currency()
	  {
		CashFlow test = CashFlow.ofPresentValue(PAYMENT_DATE, GBP, PRESENT_VALUE, DISCOUNT_FACTOR);
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(GBP);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(GBP);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_ofForecastValue_CurrencyAmount()
	  {
		CashFlow test = CashFlow.ofForecastValue(PAYMENT_DATE, FUTURE_AMOUNT, DISCOUNT_FACTOR);
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(GBP);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(GBP);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_ofForecastValue_Currency()
	  {
		CashFlow test = CashFlow.ofForecastValue(PAYMENT_DATE, GBP, FORECAST_VALUE, DISCOUNT_FACTOR);
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(GBP);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(GBP);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_convertedTo()
	  {
		CashFlow @base = CashFlow.ofForecastValue(PAYMENT_DATE, GBP, FORECAST_VALUE, DISCOUNT_FACTOR);
		CashFlow test = @base.convertedTo(USD, FxRate.of(GBP, USD, 1.5));
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(USD);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE * 1.5, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(USD);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE * 1.5, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  public virtual void test_convertedTo_noConversion()
	  {
		CashFlow @base = CashFlow.ofForecastValue(PAYMENT_DATE, GBP, FORECAST_VALUE, DISCOUNT_FACTOR);
		CashFlow test = @base.convertedTo(GBP, FxMatrix.empty());
		assertThat(test.PaymentDate).isEqualTo(PAYMENT_DATE);
		assertThat(test.PresentValue).hasCurrency(GBP);
		assertThat(test.PresentValue).hasAmount(PRESENT_VALUE, TOLERANCE);
		assertThat(test.ForecastValue).hasCurrency(GBP);
		assertThat(test.ForecastValue).hasAmount(FORECAST_VALUE, TOLERANCE);
		assertThat(test.DiscountFactor).isCloseTo(DISCOUNT_FACTOR, TOLERANCE);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CashFlow test1 = CashFlow.ofForecastValue(PAYMENT_DATE, USD, FORECAST_VALUE, DISCOUNT_FACTOR);
		coverImmutableBean(test1);
		CashFlow test2 = CashFlow.ofForecastValue(LocalDate.of(2015, 7, 11), GBP, 0.24, 0.987);
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		CashFlow test = CashFlow.ofForecastValue(PAYMENT_DATE, USD, FORECAST_VALUE, DISCOUNT_FACTOR);
		assertSerialization(test);
	  }

	}

}