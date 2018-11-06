/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThatThrownBy;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertThrows;

	using Test = org.testng.annotations.Test;

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ExchangeIds = com.opengamma.strata.product.common.ExchangeIds;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Test <seealso cref="EtdContractSpec"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class EtdContractSpecTest
	public class EtdContractSpecTest
	{

	  private static readonly EtdContractSpec FUTURE_CONTRACT = sut();
	  private static readonly EtdContractSpec OPTION_CONTRACT = sut2();

	  //-------------------------------------------------------------------------
	  public virtual void test_attributes()
	  {
		assertEquals(sut2().getAttribute(AttributeType.NAME), "NAME");
		assertEquals(sut2().findAttribute(AttributeType.NAME).get(), "NAME");
		assertThrows(typeof(System.ArgumentException), () => sut2().getAttribute(AttributeType.of("Foo")));
		EtdContractSpec updated = sut2().withAttribute(AttributeType.NAME, "FOO");
		assertEquals(updated.getAttribute(AttributeType.NAME), "FOO");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void createFutureAutoId()
	  {
		EtdFutureSecurity security = FUTURE_CONTRACT.createFuture(YearMonth.of(2015, 6), EtdVariant.MONTHLY);

		assertThat(security.SecurityId).isEqualTo(SecurityId.of(EtdIdUtils.ETD_SCHEME, "F-ECAG-FOO-201506"));
		assertThat(security.Expiry).isEqualTo(YearMonth.of(2015, 6));
		assertThat(security.ContractSpecId).isEqualTo(FUTURE_CONTRACT.Id);
		assertThat(security.Variant).isEqualTo(EtdVariant.MONTHLY);
		assertThat(security.Info.PriceInfo).isEqualTo(FUTURE_CONTRACT.PriceInfo);
	  }

	  public virtual void createFutureFromOptionContractSpec()
	  {
		assertThatThrownBy(() => OPTION_CONTRACT.createFuture(YearMonth.of(2015, 6), EtdVariant.MONTHLY)).isInstanceOf(typeof(System.InvalidOperationException)).hasMessage("Cannot create an EtdFutureSecurity from a contract specification of type 'Option'");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void createOptionAutoId()
	  {
		EtdOptionSecurity security = OPTION_CONTRACT.createOption(YearMonth.of(2015, 6), EtdVariant.MONTHLY, 0, PutCall.CALL, 123.45);

		assertThat(security.SecurityId).isEqualTo(SecurityId.of(EtdIdUtils.ETD_SCHEME, "O-IFEN-BAR-201506-C123.45"));
		assertThat(security.Expiry).isEqualTo(YearMonth.of(2015, 6));
		assertThat(security.ContractSpecId).isEqualTo(OPTION_CONTRACT.Id);
		assertThat(security.Variant).isEqualTo(EtdVariant.MONTHLY);
		assertThat(security.PutCall).isEqualTo(PutCall.CALL);
		assertThat(security.StrikePrice).isEqualTo(123.45);
		assertThat(security.UnderlyingExpiryMonth).Empty;
		assertThat(security.Info.PriceInfo).isEqualTo(OPTION_CONTRACT.PriceInfo);
	  }

	  public virtual void createOptionFromFutureContractSpec()
	  {
		assertThatThrownBy(() => FUTURE_CONTRACT.createOption(YearMonth.of(2015, 6), EtdVariant.MONTHLY, 0, PutCall.CALL, 123.45)).isInstanceOf(typeof(System.InvalidOperationException)).hasMessage("Cannot create an EtdOptionSecurity from a contract specification of type 'Future'");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void createOptionWithUnderlyingAutoId()
	  {
		EtdOptionSecurity security = OPTION_CONTRACT.createOption(YearMonth.of(2015, 6), EtdVariant.MONTHLY, 0, PutCall.CALL, 123.45, YearMonth.of(2015, 9));

		assertThat(security.SecurityId).isEqualTo(SecurityId.of(EtdIdUtils.ETD_SCHEME, "O-IFEN-BAR-201506-C123.45-U201509"));
		assertThat(security.Expiry).isEqualTo(YearMonth.of(2015, 6));
		assertThat(security.ContractSpecId).isEqualTo(OPTION_CONTRACT.Id);
		assertThat(security.Variant).isEqualTo(EtdVariant.MONTHLY);
		assertThat(security.PutCall).isEqualTo(PutCall.CALL);
		assertThat(security.StrikePrice).isEqualTo(123.45);
		assertThat(security.UnderlyingExpiryMonth).hasValue(YearMonth.of(2015, 9));
		assertThat(security.Info.PriceInfo).isEqualTo(OPTION_CONTRACT.PriceInfo);
	  }

	  public virtual void createOptionWithUnderlyingFromFutureContractSpec()
	  {
		assertThatThrownBy(() => FUTURE_CONTRACT.createOption(YearMonth.of(2015, 6), EtdVariant.MONTHLY, 0, PutCall.CALL, 123.45, YearMonth.of(2015, 9))).isInstanceOf(typeof(System.InvalidOperationException)).hasMessage("Cannot create an EtdOptionSecurity from a contract specification of type 'Future'");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(sut());
		coverBeanEquals(sut(), sut2());
	  }

	  public virtual void test_serialization()
	  {
		assertSerialization(sut());
	  }

	  //-------------------------------------------------------------------------
	  internal static EtdContractSpec sut()
	  {
		return EtdContractSpec.builder().id(EtdContractSpecId.of("test", "123")).type(EtdType.FUTURE).exchangeId(ExchangeIds.ECAG).contractCode(EtdContractCode.of("FOO")).description("A test future template").priceInfo(SecurityPriceInfo.of(Currency.GBP, 100)).build();
	  }

	  internal static EtdContractSpec sut2()
	  {
		return EtdContractSpec.builder().type(EtdType.OPTION).exchangeId(ExchangeIds.IFEN).contractCode(EtdContractCode.of("BAR")).description("A test option template").priceInfo(SecurityPriceInfo.of(Currency.EUR, 10)).addAttribute(AttributeType.NAME, "NAME").build();
	  }

	}

}