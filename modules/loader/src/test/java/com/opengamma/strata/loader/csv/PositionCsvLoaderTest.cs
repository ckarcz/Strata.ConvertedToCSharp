using System.Collections.Generic;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.joda.beans.test.BeanAssert.assertBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CharSource = com.google.common.io.CharSource;
	using ImmutableReferenceData = com.opengamma.strata.basics.ImmutableReferenceData;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ResourceLocator = com.opengamma.strata.collect.io.ResourceLocator;
	using FailureItem = com.opengamma.strata.collect.result.FailureItem;
	using FailureReason = com.opengamma.strata.collect.result.FailureReason;
	using ValueWithFailures = com.opengamma.strata.collect.result.ValueWithFailures;
	using GenericSecurity = com.opengamma.strata.product.GenericSecurity;
	using GenericSecurityPosition = com.opengamma.strata.product.GenericSecurityPosition;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;
	using ResolvableSecurityPosition = com.opengamma.strata.product.ResolvableSecurityPosition;
	using SecurityId = com.opengamma.strata.product.SecurityId;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using SecurityPosition = com.opengamma.strata.product.SecurityPosition;
	using SecurityPriceInfo = com.opengamma.strata.product.SecurityPriceInfo;
	using ExchangeIds = com.opengamma.strata.product.common.ExchangeIds;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using EtdContractCode = com.opengamma.strata.product.etd.EtdContractCode;
	using EtdContractSpec = com.opengamma.strata.product.etd.EtdContractSpec;
	using EtdContractSpecId = com.opengamma.strata.product.etd.EtdContractSpecId;
	using EtdFuturePosition = com.opengamma.strata.product.etd.EtdFuturePosition;
	using EtdFutureSecurity = com.opengamma.strata.product.etd.EtdFutureSecurity;
	using EtdIdUtils = com.opengamma.strata.product.etd.EtdIdUtils;
	using EtdOptionPosition = com.opengamma.strata.product.etd.EtdOptionPosition;
	using EtdOptionSecurity = com.opengamma.strata.product.etd.EtdOptionSecurity;
	using EtdOptionType = com.opengamma.strata.product.etd.EtdOptionType;
	using EtdSettlementType = com.opengamma.strata.product.etd.EtdSettlementType;
	using EtdType = com.opengamma.strata.product.etd.EtdType;
	using EtdVariant = com.opengamma.strata.product.etd.EtdVariant;

	/// <summary>
	/// Test <seealso cref="PositionCsvLoader"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class PositionCsvLoaderTest
	public class PositionCsvLoaderTest
	{

	  private static readonly EtdContractCode FGBL = EtdContractCode.of("FGBL");
	  private static readonly EtdContractCode OGBL = EtdContractCode.of("OGBL");

	  private static readonly SecurityPosition SECURITY1 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123431")).build()).securityId(SecurityId.of("OG-Security", "AAPL")).longQuantity(12d).shortQuantity(14.5d).build();
	  private static readonly SecurityPosition SECURITY2 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123432")).build()).securityId(SecurityId.of("BBG", "MSFT")).longQuantity(20d).shortQuantity(0d).build();
	  private static readonly SecurityPosition SECURITY3 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123433")).build()).securityId(SecurityId.of("OG-Security", "AAPL")).longQuantity(12d).shortQuantity(14.5d).build();
	  private static readonly GenericSecurityPosition SECURITY3FULL = GenericSecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123433")).build()).security(GenericSecurity.of(SecurityInfo.of(SecurityId.of("OG-Security", "AAPL"), SecurityPriceInfo.of(5, CurrencyAmount.of(USD, 0.01), 10)))).longQuantity(12d).shortQuantity(14.5d).build();

	  private static readonly ResourceLocator FILE = ResourceLocator.of("classpath:com/opengamma/strata/loader/csv/positions.csv");

	  //-------------------------------------------------------------------------
	  public virtual void test_isKnownFormat()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		assertEquals(test.isKnownFormat(FILE.CharSource), true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_load_security()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<Position>> trades = test.load(FILE);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<SecurityPosition> filtered = trades.Value.Where(typeof(SecurityPosition).isInstance).Select(typeof(SecurityPosition).cast).collect(toImmutableList());
		assertEquals(filtered.Count, 2);

		assertBeanEquals(SECURITY1, filtered[0]);
		assertBeanEquals(SECURITY2, filtered[1]);
	  }

	  public virtual void test_load_genericSecurity()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<Position>> trades = test.load(FILE);

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<GenericSecurityPosition> filtered = trades.Value.Where(typeof(GenericSecurityPosition).isInstance).Select(typeof(GenericSecurityPosition).cast).collect(toImmutableList());
		assertEquals(filtered.Count, 1);

		assertBeanEquals(SECURITY3FULL, filtered[0]);
	  }

	  public virtual void test_parseFiltering()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		assertEquals(test.parse(ImmutableList.of(FILE.CharSource)).Value.Count, 3); // 7 errors
		assertEquals(test.parse(ImmutableList.of(FILE.CharSource), typeof(SecurityPosition)).Value.Count, 10);
		assertEquals(test.parse(ImmutableList.of(FILE.CharSource), typeof(ResolvableSecurityPosition)).Value.Count, 3);
		assertEquals(test.parse(ImmutableList.of(FILE.CharSource), typeof(GenericSecurityPosition)).Value.Count, 1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parse_future()
	  {
		EtdContractSpecId specId = EtdContractSpecId.of("OG-ETD", "F-ECAG-FGBL");
		EtdContractSpec contract = EtdContractSpec.builder().id(specId).type(EtdType.FUTURE).exchangeId(ExchangeIds.ECAG).contractCode(FGBL).description("Dummy").priceInfo(SecurityPriceInfo.of(Currency.GBP, 100)).build();
		ReferenceData refData = ImmutableReferenceData.of(specId, contract);
		PositionCsvLoader test = PositionCsvLoader.of(refData);
		ValueWithFailures<IList<EtdFuturePosition>> trades = test.parse(ImmutableList.of(FILE.CharSource), typeof(EtdFuturePosition));
		IList<EtdFuturePosition> filtered = trades.Value;
		assertEquals(filtered.Count, 4);

		EtdFuturePosition expected1 = EtdFuturePosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123421")).build()).security(EtdFutureSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofMonthly())).longQuantity(15d).shortQuantity(2d).build();
		assertBeanEquals(expected1, filtered[0]);

		EtdFuturePosition expected2 = EtdFuturePosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123422")).build()).security(EtdFutureSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofFlexFuture(13, EtdSettlementType.CASH))).longQuantity(0d).shortQuantity(13d).build();
		assertBeanEquals(expected2, filtered[1]);

		EtdFuturePosition expected3 = EtdFuturePosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123423")).build()).security(EtdFutureSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofWeekly(2))).longQuantity(0d).shortQuantity(20d).build();
		assertBeanEquals(expected3, filtered[2]);

		EtdFuturePosition expected4 = EtdFuturePosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123424")).build()).security(EtdFutureSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofDaily(3))).longQuantity(30d).shortQuantity(0d).build();
		assertBeanEquals(expected4, filtered[3]);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parse_option()
	  {
		EtdContractSpecId specId = EtdContractSpecId.of("OG-ETD", "O-ECAG-OGBL");
		EtdContractSpec contract = EtdContractSpec.builder().id(specId).type(EtdType.OPTION).exchangeId(ExchangeIds.ECAG).contractCode(OGBL).description("Dummy").priceInfo(SecurityPriceInfo.of(Currency.GBP, 100)).build();
		ReferenceData refData = ImmutableReferenceData.of(specId, contract);
		PositionCsvLoader test = PositionCsvLoader.of(refData);
		ValueWithFailures<IList<EtdOptionPosition>> trades = test.parse(ImmutableList.of(FILE.CharSource), typeof(EtdOptionPosition));

		IList<EtdOptionPosition> filtered = trades.Value;
		assertEquals(filtered.Count, 3);

		EtdOptionPosition expected1 = EtdOptionPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123431")).build()).security(EtdOptionSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofMonthly(), 0, PutCall.PUT, 3d, YearMonth.of(2017, 9))).longQuantity(15d).shortQuantity(2d).build();
		assertBeanEquals(expected1, filtered[0]);

		EtdOptionPosition expected2 = EtdOptionPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123432")).build()).security(EtdOptionSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofFlexOption(13, EtdSettlementType.CASH, EtdOptionType.AMERICAN), 0, PutCall.CALL, 4d)).longQuantity(0d).shortQuantity(13d).build();
		assertBeanEquals(expected2, filtered[1]);

		EtdOptionPosition expected3 = EtdOptionPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123433")).build()).security(EtdOptionSecurity.of(contract, YearMonth.of(2017, 6), EtdVariant.ofWeekly(2), 0, PutCall.PUT, 5.1d)).longQuantity(0d).shortQuantity(20d).build();
		assertBeanEquals(expected3, filtered[2]);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_parseLightweight()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<SecurityPosition>> trades = test.parseLightweight(ImmutableList.of(FILE.CharSource));
		IList<SecurityPosition> filtered = trades.Value;
		assertEquals(filtered.Count, 10);

		assertBeanEquals(SECURITY1, filtered[0]);
		assertBeanEquals(SECURITY2, filtered[1]);
		assertBeanEquals(SECURITY3, filtered[2]);

		SecurityPosition expected3 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123421")).build()).securityId(EtdIdUtils.futureId(ExchangeIds.ECAG, FGBL, YearMonth.of(2017, 6), EtdVariant.ofMonthly())).longQuantity(15d).shortQuantity(2d).build();
		assertBeanEquals(expected3, filtered[3]);

		SecurityPosition expected4 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123422")).build()).securityId(EtdIdUtils.futureId(ExchangeIds.ECAG, FGBL, YearMonth.of(2017, 6), EtdVariant.ofFlexFuture(13, EtdSettlementType.CASH))).longQuantity(0d).shortQuantity(13d).build();
		assertBeanEquals(expected4, filtered[4]);

		SecurityPosition expected5 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123423")).build()).securityId(EtdIdUtils.futureId(ExchangeIds.ECAG, FGBL, YearMonth.of(2017, 6), EtdVariant.ofWeekly(2))).longQuantity(0d).shortQuantity(20d).build();
		assertBeanEquals(expected5, filtered[5]);

		SecurityPosition expected6 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123424")).build()).securityId(EtdIdUtils.futureId(ExchangeIds.ECAG, FGBL, YearMonth.of(2017, 6), EtdVariant.ofDaily(3))).longQuantity(30d).shortQuantity(0d).build();
		assertBeanEquals(expected6, filtered[6]);

		SecurityPosition expected7 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123431")).build()).securityId(EtdIdUtils.optionId(ExchangeIds.ECAG, OGBL, YearMonth.of(2017, 6), EtdVariant.ofMonthly(), 0, PutCall.PUT, 3d, YearMonth.of(2017, 9))).longQuantity(15d).shortQuantity(2d).build();
		assertBeanEquals(expected7, filtered[7]);

		SecurityPosition expected8 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123432")).build()).securityId(EtdIdUtils.optionId(ExchangeIds.ECAG, OGBL, YearMonth.of(2017, 6), EtdVariant.ofFlexOption(13, EtdSettlementType.CASH, EtdOptionType.AMERICAN), 0, PutCall.CALL, 4d)).longQuantity(0d).shortQuantity(13d).build();
		assertBeanEquals(expected8, filtered[8]);

		SecurityPosition expected9 = SecurityPosition.builder().info(PositionInfo.builder().id(StandardId.of("OG", "123433")).build()).securityId(EtdIdUtils.optionId(ExchangeIds.ECAG, OGBL, YearMonth.of(2017, 6), EtdVariant.ofWeekly(2), 0, PutCall.PUT, 5.1d)).longQuantity(0d).shortQuantity(20d).build();
		assertBeanEquals(expected9, filtered[9]);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_load_invalidNoHeader()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<Position>> trades = test.parse(ImmutableList.of(CharSource.wrap("")));

		assertEquals(trades.Failures.size(), 1);
		FailureItem failure = trades.Failures.get(0);
		assertEquals(failure.Reason, FailureReason.PARSING);
		assertEquals(failure.Message.Contains("CSV file could not be parsed"), true);
	  }

	  public virtual void test_load_invalidNoType()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<Position>> trades = test.parse(ImmutableList.of(CharSource.wrap("Id")));

		assertEquals(trades.Failures.size(), 1);
		FailureItem failure = trades.Failures.get(0);
		assertEquals(failure.Reason, FailureReason.PARSING);
		assertEquals(failure.Message.Contains("CSV file does not contain 'Strata Position Type' header"), true);
	  }

	  public virtual void test_load_invalidUnknownType()
	  {
		PositionCsvLoader test = PositionCsvLoader.standard();
		ValueWithFailures<IList<Position>> trades = test.parse(ImmutableList.of(CharSource.wrap("Strata Position Type\nFoo")));

		assertEquals(trades.Failures.size(), 1);
		FailureItem failure = trades.Failures.get(0);
		assertEquals(failure.Reason, FailureReason.PARSING);
		assertEquals(failure.Message, "CSV file position type 'Foo' is not known at line 2");
	  }

	  public virtual void test_load_invalidNoQuantity()
	  {
		EtdContractSpecId specId = EtdContractSpecId.of("OG-ETD", "F-ECAG-FGBL");
		EtdContractSpec contract = EtdContractSpec.builder().id(specId).type(EtdType.FUTURE).exchangeId(ExchangeIds.ECAG).contractCode(FGBL).description("Dummy").priceInfo(SecurityPriceInfo.of(Currency.GBP, 100)).build();
		ReferenceData refData = ImmutableReferenceData.of(specId, contract);
		PositionCsvLoader test = PositionCsvLoader.of(refData);
		ValueWithFailures<IList<Position>> trades = test.parse(ImmutableList.of(CharSource.wrap("Strata Position Type,Exchange,Contract Code,Expiry\nFUT,ECAG,FGBL,2017-06")));

		assertEquals(trades.Failures.size(), 1);
		FailureItem failure = trades.Failures.get(0);
		assertEquals(failure.Reason, FailureReason.PARSING);
		assertEquals(failure.Message, "CSV file position could not be parsed at line 2: " + "Security must contain a quantity column, either 'Quantity' or 'Long Quantity' and 'Short Quantity'");
	  }

	}

}