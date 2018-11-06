using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.currency
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.BasicProjectAssertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CAD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.CHF;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.JPY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.SEK;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.FxMatrix.entriesToFxMatrix;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.FxMatrix.pairsToFxMatrix;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.offset;


	using Offset = org.assertj.core.data.Offset;
	using Bean = org.joda.beans.Bean;
	using JodaBeanSer = org.joda.beans.ser.JodaBeanSer;
	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Pair = com.opengamma.strata.collect.tuple.Pair;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxMatrixTest
	public class FxMatrixTest
	{

	  private const double TOLERANCE = 1e-6;
	  public static readonly Offset<double> TOL = offset(TOLERANCE);
	  private const object ANOTHER_TYPE = "";

	  public virtual void emptyMatrixCanHandleTrivialRate()
	  {
		FxMatrix matrix = FxMatrix.empty();
		assertThat(matrix.Currencies).Empty;
		assertThat(matrix.fxRate(USD, USD)).isEqualTo(1.0);
		assertThat(matrix.ToString()).isEqualTo("FxMatrix[ : ]");
	  }

	  public virtual void emptyMatrixCannotDoConversion()
	  {
		FxMatrix matrix = FxMatrix.builder().build();
		assertThat(matrix.Currencies).Empty;
		assertThrowsIllegalArg(() => matrix.fxRate(USD, EUR));
	  }

	  public virtual void singleRateMatrixByOfCurrencyPairFactory()
	  {
		FxMatrix matrix = FxMatrix.of(CurrencyPair.of(GBP, USD), 1.6);
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(0.625);
		assertThat(matrix.ToString()).isEqualTo("FxMatrix[GBP, USD : [1.0, 1.6],[0.625, 1.0]]");
	  }

	  public virtual void singleRateMatrixByOfCurrenciesFactory()
	  {
		FxMatrix matrix = FxMatrix.of(GBP, USD, 1.6);
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(0.625);
	  }

	  public virtual void singleRateMatrixByBuilder()
	  {
		FxMatrix matrix = FxMatrix.builder().addRate(GBP, USD, 1.6).build();
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(0.625);
	  }

	  public virtual void canAddRateUsingCurrencyPair()
	  {
		FxMatrix matrix = FxMatrix.builder().addRate(CurrencyPair.of(GBP, USD), 1.6).build();
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(0.625);
	  }

	  public virtual void singleRateMatrixCannotDoConversionForUnknownCurrency()
	  {
		FxMatrix matrix = FxMatrix.builder().addRate(GBP, USD, 1.6).build();
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThrowsIllegalArg(() => matrix.fxRate(USD, EUR));
	  }

	  public virtual void matrixCalculatesCrossRates()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).addRate(EUR, CHF, 1.2).build();

		assertThat(matrix.Currencies).containsOnly(GBP, USD, EUR, CHF);

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(1 / 1.6);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.4);
		assertThat(matrix.fxRate(USD, EUR)).isEqualTo(1 / 1.4);
		assertThat(matrix.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.6, TOL);
		assertThat(matrix.fxRate(GBP, EUR)).isEqualTo(1.6 / 1.4, TOL);
		assertThat(matrix.fxRate(EUR, CHF)).isEqualTo(1.2);
	  }

	  public virtual void cannotAddEntryWithNoCommonCurrencyAndBuild()
	  {
		assertThrows(() => FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(CHF, AUD, 1.6).build(), typeof(System.InvalidOperationException));
	  }

	  public virtual void canAddEntryWithNoCommonCurrencyIfSuppliedBySubsequentEntries()
	  {
		FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(CHF, AUD, 1.6).addRate(EUR, CHF, 1.2).addRate(EUR, USD, 1.4).build();
	  }

	  public virtual void rateCanBeUpdatedInBuilder()
	  {
		FxMatrix matrix = FxMatrix.builder().addRate(GBP, USD, 1.5).addRate(GBP, USD, 1.6).build();
		assertThat(matrix.Currencies).containsOnly(GBP, USD);
		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(0.625);
	  }

	  public virtual void ratedCanBeUpdatedAndAddedViaBuilder()
	  {
		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.5).build();

		assertThat(matrix1.Currencies).containsOnly(GBP, USD);
		assertThat(matrix1.fxRate(GBP, USD)).isEqualTo(1.5);

		FxMatrix matrix2 = matrix1.toBuilder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).build();

		assertThat(matrix2.Currencies).containsOnly(GBP, USD, EUR);
		assertThat(matrix2.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix2.fxRate(EUR, USD)).isEqualTo(1.4);
	  }

	  public virtual void updatingRateIsNotSymmetric()
	  {

		/*
		Expected data as produced from old analytics FxMatrix
		
		[USD, GBP,    EUR] - {
		USD {1.0 ,0.666, 0.714283},
		GBP {1.5, 1.0,   1.071428},
		EUR {1.4, 0.933, 1.0}}
		
		[USD,     GBP,    EUR] - {
		{1.0,     0.625,  0.66964},
		{1.6,     1.0,    1.071428},
		{1.49333, 0.9333, 1.0}}
		
		 [USD,    GBP,    EUR] - {
		 {1.0,    0.625,  0.71428},
		 {1.6,    1.0,    1.14285},
		 {1.4,    0.875,  1.0}}
		 */

		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.5).addRate(EUR, USD, 1.4).build();

		FxMatrix matrix2 = matrix1.toBuilder().addRate(GBP, USD, 1.6).build();

		// Switching the currency order for the update gives a
		// different matrix and has a different effect on the
		// the rates
		FxMatrix matrix3 = matrix1.toBuilder().addRate(USD, GBP, 1 / 1.6).build();

		assertThat(matrix2).isNotEqualTo(matrix3);

		assertThat(matrix1.Currencies).hasSize(3);
		assertThat(matrix1.fxRate(GBP, USD)).isEqualTo(1.5);
		assertThat(matrix1.fxRate(EUR, USD)).isEqualTo(1.4);
		assertThat(matrix1.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.5, TOL);

		// The rate we updated
		assertThat(matrix2.fxRate(GBP, USD)).isEqualTo(1.6);

		// Matrix2 update was restating USD wrt GBP so
		// EUR/USD is affected
		assertThat(matrix2.fxRate(EUR, USD)).isEqualTo(1.4 * (1.6 / 1.5), TOL); // = 1.49333
		// but EUR/GBP is not
		assertThat(matrix2.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.5, TOL); // = 0.9333

		// The rate we updated
		assertThat(matrix3.fxRate(GBP, USD)).isEqualTo(1.6);

		// As matrix3 update was restating GBP wrt USD, there is
		// no effect on EUR/USD
		assertThat(matrix3.fxRate(EUR, USD)).isEqualTo(1.4);
		// but there is an effect on EUR/GBP
		assertThat(matrix3.fxRate(EUR, GBP)).isEqualTo((1.4 / 1.5) * (1.5 / 1.6), TOL); // = 0.875
	  }

	  public virtual void rateCanBeUpdatedWithDirectionSwitched()
	  {
		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.6).build();

		assertThat(matrix1.Currencies).hasSize(2);
		assertThat(matrix1.fxRate(GBP, USD)).isEqualTo(1.6);

		FxMatrix matrix2 = matrix1.toBuilder().addRate(USD, GBP, 0.625).build();

		assertThat(matrix2.Currencies).hasSize(2);
		assertThat(matrix2.fxRate(GBP, USD)).isEqualTo(1.6);
	  }

	  public virtual void addSimpleMultipleRates()
	  {

		// Use linked to force the order of evaluation
		// want to see that builder recovers when
		// encountering a currency pair for 2 unknown
		// currencies but which will appear later
		LinkedHashMap<CurrencyPair, double> rates = new LinkedHashMap<CurrencyPair, double>();
		rates.put(CurrencyPair.of(GBP, USD), 1.6);
		rates.put(CurrencyPair.of(EUR, USD), 1.4);

		FxMatrix matrix = FxMatrix.builder().addRates(rates).build();

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(1 / 1.6);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.4);
		assertThat(matrix.fxRate(USD, EUR)).isEqualTo(1 / 1.4);
		assertThat(matrix.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.6, TOL);
		assertThat(matrix.fxRate(GBP, EUR)).isEqualTo(1.6 / 1.4, TOL);
	  }

	  public virtual void addMultipleRatesContainingEntryWithNoCommonCurrency()
	  {

		LinkedHashMap<CurrencyPair, double> rates = new LinkedHashMap<CurrencyPair, double>();
		rates.put(CurrencyPair.of(GBP, USD), 1.6);
		rates.put(CurrencyPair.of(EUR, USD), 1.4);
		rates.put(CurrencyPair.of(JPY, CAD), 0.01); // Neither currency linked to one of the others

		assertThrows(() => FxMatrix.builder().addRates(rates).build(), typeof(System.InvalidOperationException));
	  }

	  public virtual void addMultipleRates()
	  {

		// Use linked map to force the order of evaluation
		// want to see that builder recovers when
		// encountering a currency pair for 2 unknown
		// currencies but which will appear later
		LinkedHashMap<CurrencyPair, double> rates = new LinkedHashMap<CurrencyPair, double>();
		rates.put(CurrencyPair.of(GBP, USD), 1.6);
		rates.put(CurrencyPair.of(EUR, USD), 1.4);
		rates.put(CurrencyPair.of(CHF, AUD), 1.2); // Neither currency seen before
		rates.put(CurrencyPair.of(SEK, AUD), 0.16); // AUD seen before but not added yet
		rates.put(CurrencyPair.of(JPY, CAD), 0.01); // Neither currency seen before
		rates.put(CurrencyPair.of(EUR, CHF), 1.2);
		rates.put(CurrencyPair.of(JPY, USD), 0.0084);

		FxMatrix matrix = FxMatrix.builder().addRates(rates).build();

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(1 / 1.6);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.4);
		assertThat(matrix.fxRate(USD, EUR)).isEqualTo(1 / 1.4);
		assertThat(matrix.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.6, TOL);
		assertThat(matrix.fxRate(GBP, EUR)).isEqualTo(1.6 / 1.4, TOL);
		assertThat(matrix.fxRate(EUR, CHF)).isEqualTo(1.2);
	  }

	  public virtual void streamEntriesToMatrix()
	  {

		// If we obtain a stream of rates we can collect to an fx matrix
		IDictionary<CurrencyPair, double> rates = ImmutableMap.builder<CurrencyPair, double>().put(CurrencyPair.of(GBP, USD), 1.6).put(CurrencyPair.of(EUR, USD), 1.4).put(CurrencyPair.of(CHF, AUD), 1.2).put(CurrencyPair.of(SEK, AUD), 0.1).put(CurrencyPair.of(JPY, CAD), 0.0).put(CurrencyPair.of(EUR, CHF), 1.2).put(CurrencyPair.of(JPY, USD), 0.008).build();

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		FxMatrix matrix = rates.SetOfKeyValuePairs().collect(entriesToFxMatrix());

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.4);
	  }

	  public virtual void streamPairsToMatrix()
	  {

		// If we obtain a stream of pairs with rates we can stream them
		// This could happen if an entry set undergoes a map operation

		IDictionary<CurrencyPair, double> rates = ImmutableMap.builder<CurrencyPair, double>().put(CurrencyPair.of(GBP, USD), 1.6).put(CurrencyPair.of(EUR, USD), 1.4).put(CurrencyPair.of(CHF, AUD), 1.2).put(CurrencyPair.of(SEK, AUD), 0.1).put(CurrencyPair.of(JPY, CAD), 0.0).put(CurrencyPair.of(EUR, CHF), 1.2).put(CurrencyPair.of(JPY, USD), 0.008).build();

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		FxMatrix matrix = rates.SetOfKeyValuePairs().Select(e => Pair.of(e.Key, e.Value * 1.01)).collect(pairsToFxMatrix());

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.616);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.414);
	  }

	  // By adding more than 8 currencies we force a resizing
	  // operation - ensure it causes no issues
	  public virtual void addMultipleRatesSingle()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).addRate(EUR, CHF, 1.2).addRate(EUR, CHF, 1.2).addRate(CHF, AUD, 1.2).addRate(SEK, AUD, 0.16).addRate(JPY, USD, 0.0084).addRate(JPY, CAD, 0.01).addRate(USD, NZD, 1.3).build();

		assertThat(matrix.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(matrix.fxRate(USD, GBP)).isEqualTo(1 / 1.6);
		assertThat(matrix.fxRate(EUR, USD)).isEqualTo(1.4, TOL);
		assertThat(matrix.fxRate(USD, EUR)).isEqualTo(1 / 1.4, TOL);
		assertThat(matrix.fxRate(EUR, GBP)).isEqualTo(1.4 / 1.6, TOL);
		assertThat(matrix.fxRate(GBP, EUR)).isEqualTo(1.6 / 1.4, TOL);
		assertThat(matrix.fxRate(EUR, CHF)).isEqualTo(1.2);
	  }

	  public virtual void convertCurrencyAmount()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, EUR, 1.4).addRate(GBP, USD, 1.6).build();

		CurrencyAmount amount = CurrencyAmount.of(GBP, 1600);

		assertThat(matrix.convert(amount, GBP)).isEqualTo(amount);

		assertThat(matrix.convert(amount, USD)).hasCurrency(USD).hasAmount(2560);

		assertThat(matrix.convert(amount, EUR)).hasCurrency(EUR).hasAmount(2240);
	  }

	  public virtual void convertMultipleCurrencyAmountWithNoEntries()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, EUR, 1.4).addRate(GBP, USD, 1.6).build();

		MultiCurrencyAmount amount = MultiCurrencyAmount.of();

		assertThat(matrix.convert(amount, GBP)).hasCurrency(GBP).hasAmount(0);

		assertThat(matrix.convert(amount, USD)).hasCurrency(USD).hasAmount(0);

		assertThat(matrix.convert(amount, EUR)).hasCurrency(EUR).hasAmount(0);
	  }

	  public virtual void convertMultipleCurrencyAmountWithSingleEntry()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, EUR, 1.4).addRate(GBP, USD, 1.6).build();

		MultiCurrencyAmount amount = MultiCurrencyAmount.of(CurrencyAmount.of(GBP, 1600));

		assertThat(matrix.convert(amount, GBP)).hasCurrency(GBP).hasAmount(1600);

		assertThat(matrix.convert(amount, USD)).hasCurrency(USD).hasAmount(2560);

		assertThat(matrix.convert(amount, EUR)).hasCurrency(EUR).hasAmount(2240);
	  }

	  public virtual void convertMultipleCurrencyAmountWithMultipleEntries()
	  {

		FxMatrix matrix = FxMatrix.builder().addRate(GBP, EUR, 1.4).addRate(GBP, USD, 1.6).build();

		MultiCurrencyAmount amount = MultiCurrencyAmount.of(CurrencyAmount.of(GBP, 1600), CurrencyAmount.of(EUR, 1200), CurrencyAmount.of(USD, 1500));

		assertThat(matrix.convert(amount, GBP)).hasCurrency(GBP).hasAmount(1600d + (1200 / 1.4) + (1500 / 1.6), TOL);

		assertThat(matrix.convert(amount, USD)).hasCurrency(USD).hasAmount((1600d * 1.6) + ((1200 / 1.4) * 1.6) + 1500);

		assertThat(matrix.convert(amount, EUR)).hasCurrency(EUR).hasAmount((1600d * 1.4) + 1200 + ((1500 / 1.6) * 1.4));
	  }

	  public virtual void cannotMergeDisjointMatrices()
	  {

		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).build();

		FxMatrix matrix2 = FxMatrix.builder().addRate(CHF, AUD, 1.2).addRate(SEK, AUD, 0.16).build();

		assertThrowsIllegalArg(() => matrix1.merge(matrix2));
	  }

	  public virtual void mergeIgnoresDuplicateCurrencies()
	  {

		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).addRate(EUR, CHF, 1.2).build();

		FxMatrix matrix2 = FxMatrix.builder().addRate(GBP, USD, 1.7).addRate(EUR, USD, 1.5).addRate(EUR, CHF, 1.3).build();

		FxMatrix result = matrix1.merge(matrix2);
		assertThat(result).isEqualTo(matrix1);
	  }

	  public virtual void mergeAddsInAdditionalCurrencies()
	  {
		FxMatrix matrix1 = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).build();

		FxMatrix matrix2 = FxMatrix.builder().addRate(EUR, CHF, 1.2).addRate(CHF, AUD, 1.2).build();

		FxMatrix result = matrix1.merge(matrix2);
		assertThat(result.Currencies).contains(USD, GBP, EUR, CHF, AUD);

		assertThat(result.fxRate(GBP, USD)).isEqualTo(1.6);
		assertThat(result.fxRate(GBP, EUR)).isEqualTo(1.6 / 1.4, TOL);

		assertThat(result.fxRate(EUR, CHF)).isEqualTo(1.2);
		assertThat(result.fxRate(CHF, AUD)).isEqualTo(1.2);

		assertThat(result.fxRate(GBP, CHF)).isEqualTo((1.6 / 1.4) * 1.2, TOL);
		assertThat(result.fxRate(GBP, AUD)).isEqualTo((1.6 / 1.4) * 1.2 * 1.2, TOL);
	  }

	  public virtual void equalsGood()
	  {
		FxMatrix m1 = FxMatrix.builder().addRate(GBP, USD, 1.4).build();
		FxMatrix m2 = FxMatrix.builder().addRate(GBP, USD, 1.39).build();
		FxMatrix m3 = FxMatrix.builder().addRate(GBP, USD, 1.39).build();
		FxMatrix m4 = FxMatrix.builder().addRate(GBP, EUR, 1.2).build();

		assertThat(m1.Equals(m1)).True;
		assertThat(m2.Equals(m2)).True;
		assertThat(m3.Equals(m3)).True;
		assertThat(m4.Equals(m4)).True;

		assertThat(m1.Equals(m2)).False;
		assertThat(m1.Equals(m4)).False;

		assertThat(m2.Equals(m3)).True;
	  }

	  public virtual void equalsBad()
	  {
		FxMatrix test = FxMatrix.builder().addRate(USD, GBP, 1.4).build();
		assertThat(test.Equals(ANOTHER_TYPE)).False;
		assertThat(test.Equals(null)).False;
	  }

	  public virtual void hashCodeCoverage()
	  {
		FxMatrix m1 = FxMatrix.builder().addRate(GBP, USD, 1.4).build();
		FxMatrix m2 = FxMatrix.builder().addRate(GBP, USD, 1.39).build();
		FxMatrix m3 = FxMatrix.builder().addRate(GBP, USD, 1.39).build();

		assertThat(m1.GetHashCode()).isNotEqualTo(m2.GetHashCode());
		assertThat(m2.GetHashCode()).isEqualTo(m3.GetHashCode());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(FxMatrix.empty());
		coverImmutableBean(FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).addRate(EUR, CHF, 1.2).build());
	  }

	  public virtual void testSerializeDeserialize()
	  {
		FxMatrix test1 = FxMatrix.builder().addRate(GBP, USD, 1.6).addRate(EUR, USD, 1.4).addRate(EUR, CHF, 1.2).build();
		FxMatrix test2 = FxMatrix.builder().addRate(GBP, USD, 1.7).addRate(EUR, USD, 1.5).addRate(EUR, CHF, 1.3).build();
		cycleBean(FxMatrix.empty());
		cycleBean(test1);
		cycleBean(test2);
		assertSerialization(FxMatrix.empty());
		assertSerialization(test1);
		assertSerialization(test2);
	  }

	  private void cycleBean(Bean bean)
	  {
		JodaBeanSer ser = JodaBeanSer.COMPACT;
		string result = ser.xmlWriter().write(bean);
		Bean cycled = ser.xmlReader().read(result);
		assertThat(cycled).isEqualTo(bean);
	  }

	}

}