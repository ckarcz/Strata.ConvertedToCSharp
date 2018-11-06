using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using CurveName = com.opengamma.strata.market.curve.CurveName;

	/// <summary>
	/// Test <seealso cref="UnitParameterSensitivities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class UnitParameterSensitivitiesTest
	public class UnitParameterSensitivitiesTest
	{

	  private const double FACTOR1 = 3.14;
	  private static readonly DoubleArray VECTOR1 = DoubleArray.of(100, 200, 300, 123);
	  private static readonly DoubleArray VECTOR2 = DoubleArray.of(1000, 250, 321, 123);
	  private static readonly DoubleArray VECTOR_ZERO = DoubleArray.of(0, 0, 0, 0);
	  private static readonly DoubleArray TOTAL_USD = DoubleArray.of(1100, 450, 621, 246);
	  private static readonly DoubleArray VECTOR3 = DoubleArray.of(1000, 250, 321, 123, 321);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME0 = com.opengamma.strata.market.curve.CurveName.of("NAME-0");
	  private static readonly MarketDataName<object> NAME0 = CurveName.of("NAME-0");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME3 = com.opengamma.strata.market.curve.CurveName.of("NAME-3");
	  private static readonly MarketDataName<object> NAME3 = CurveName.of("NAME-3");
	  private static readonly IList<ParameterMetadata> METADATA0 = ParameterMetadata.listOfEmpty(4);
	  private static readonly IList<ParameterMetadata> METADATA1 = ParameterMetadata.listOfEmpty(4);
	  private static readonly IList<ParameterMetadata> METADATA2 = ParameterMetadata.listOfEmpty(5);
	  private static readonly IList<ParameterMetadata> METADATA3 = ParameterMetadata.listOfEmpty(4);

	  private static readonly UnitParameterSensitivity ENTRY1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
	  private static readonly UnitParameterSensitivity ENTRY2 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR2);
	  private static readonly UnitParameterSensitivity ENTRY_TOTAL_1_2 = UnitParameterSensitivity.of(NAME1, METADATA1, TOTAL_USD);
	  private static readonly UnitParameterSensitivity ENTRY_SMALL = UnitParameterSensitivity.of(NAME1, ParameterMetadata.listOfEmpty(1), DoubleArray.of(100d));
	  private static readonly UnitParameterSensitivity ENTRY3 = UnitParameterSensitivity.of(NAME2, METADATA2, VECTOR3);
	  private static readonly UnitParameterSensitivity ENTRY_ZERO0 = UnitParameterSensitivity.of(NAME0, METADATA0, VECTOR_ZERO);
	  private static readonly UnitParameterSensitivity ENTRY_ZERO3 = UnitParameterSensitivity.of(NAME3, METADATA3, VECTOR_ZERO);
	  private static readonly UnitParameterSensitivity ENTRY_COMBINED = UnitParameterSensitivity.combine(NAME3, ENTRY1, ENTRY3);

	  private static readonly UnitParameterSensitivities SENSI_1 = UnitParameterSensitivities.of(ENTRY1);
	  private static readonly UnitParameterSensitivities SENSI_2 = UnitParameterSensitivities.of(ImmutableList.of(ENTRY2, ENTRY3));

	  private const double TOLERENCE_CMP = 1.0E-8;

	  //-------------------------------------------------------------------------
	  public virtual void test_empty()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.empty();
		assertEquals(test.size(), 0);
		assertEquals(test.Sensitivities.size(), 0);
	  }

	  public virtual void test_of_single()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.of(ENTRY1);
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY1));
	  }

	  public virtual void test_of_array_none()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.of();
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_list_none()
	  {
		ImmutableList<UnitParameterSensitivity> list = ImmutableList.of();
		UnitParameterSensitivities test = UnitParameterSensitivities.of(list);
		assertEquals(test.size(), 0);
	  }

	  public virtual void test_of_list_notNormalized()
	  {
		ImmutableList<UnitParameterSensitivity> list = ImmutableList.of(ENTRY1, ENTRY3);
		UnitParameterSensitivities test = UnitParameterSensitivities.of(list);
		assertEquals(test.size(), 2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY1, ENTRY3));
	  }

	  public virtual void test_of_list_normalized()
	  {
		ImmutableList<UnitParameterSensitivity> list = ImmutableList.of(ENTRY1, ENTRY2);
		UnitParameterSensitivities test = UnitParameterSensitivities.of(list);
		assertEquals(test.size(), 1);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_TOTAL_1_2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_getSensitivity()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.of(ENTRY1);
		assertEquals(test.getSensitivity(NAME1), ENTRY1);
		assertThrowsIllegalArg(() => test.getSensitivity(NAME0));
	  }

	  public virtual void test_findSensitivity()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.of(ENTRY1);
		assertEquals(test.findSensitivity(NAME1), ENTRY1);
		assertEquals(test.findSensitivity(NAME0), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_one_notNormalized()
	  {
		UnitParameterSensitivities test = SENSI_1.combinedWith(ENTRY3);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY1, ENTRY3));
	  }

	  public virtual void test_combinedWith_one_normalized()
	  {
		UnitParameterSensitivities test = SENSI_1.combinedWith(ENTRY2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_TOTAL_1_2));
	  }

	  public virtual void test_combinedWith_one_sizeMismatch()
	  {
		assertThrowsIllegalArg(() => SENSI_1.combinedWith(ENTRY_SMALL));
	  }

	  public virtual void test_combinedWith_other()
	  {
		UnitParameterSensitivities test = SENSI_1.combinedWith(SENSI_2);
		assertEquals(test.Sensitivities, ImmutableList.of(ENTRY_TOTAL_1_2, ENTRY3));
	  }

	  public virtual void test_combinedWith_otherEmpty()
	  {
		UnitParameterSensitivities test = SENSI_1.combinedWith(UnitParameterSensitivities.empty());
		assertEquals(test, SENSI_1);
	  }

	  public virtual void test_combinedWith_empty()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.empty().combinedWith(SENSI_1);
		assertEquals(test, SENSI_1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy_currency()
	  {
		CurrencyParameterSensitivities multiplied = SENSI_2.multipliedBy(USD, FACTOR1);
		assertEquals(multiplied.size(), 2);
		DoubleArray test1 = multiplied.getSensitivity(NAME1, USD).Sensitivity;
		for (int i = 0; i < VECTOR1.size(); i++)
		{
		  assertEquals(test1.get(i), VECTOR2.get(i) * FACTOR1);
		}
		DoubleArray test2 = multiplied.getSensitivity(NAME2, USD).Sensitivity;
		for (int i = 0; i < VECTOR1.size(); i++)
		{
		  assertEquals(test2.get(i), VECTOR3.get(i) * FACTOR1);
		}
	  }

	  public virtual void test_multipliedBy()
	  {
		UnitParameterSensitivities multiplied = SENSI_1.multipliedBy(FACTOR1);
		DoubleArray test = multiplied.Sensitivities.get(0).Sensitivity;
		for (int i = 0; i < VECTOR1.size(); i++)
		{
		  assertEquals(test.get(i), VECTOR1.get(i) * FACTOR1);
		}
	  }

	  public virtual void test_mapSensitivities()
	  {
		UnitParameterSensitivities multiplied = SENSI_1.mapSensitivities(a => 1 / a);
		DoubleArray test = multiplied.Sensitivities.get(0).Sensitivity;
		for (int i = 0; i < VECTOR1.size(); i++)
		{
		  assertEquals(test.get(i), 1 / VECTOR1.get(i));
		}
	  }

	  public virtual void test_multipliedBy_vs_combinedWith()
	  {
		UnitParameterSensitivities multiplied = SENSI_2.multipliedBy(2d);
		UnitParameterSensitivities added = SENSI_2.combinedWith(SENSI_2);
		assertEquals(multiplied, added);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_split()
	  {
		UnitParameterSensitivities test = UnitParameterSensitivities.of(ENTRY_COMBINED).split();
		assertEquals(test, UnitParameterSensitivities.of(ENTRY1, ENTRY3));
	  }

	  public virtual void test_split_noSplit()
	  {
		UnitParameterSensitivities test = SENSI_1.split();
		assertEquals(test, SENSI_1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_equalWithTolerance()
	  {
		UnitParameterSensitivities sensUsdTotal = UnitParameterSensitivities.of(ENTRY_TOTAL_1_2);
		UnitParameterSensitivities sensEur = UnitParameterSensitivities.of(ENTRY3);
		UnitParameterSensitivities sens1plus2 = SENSI_1.combinedWith(ENTRY2);
		UnitParameterSensitivities sensZeroA = UnitParameterSensitivities.of(ENTRY_ZERO3);
		UnitParameterSensitivities sensZeroB = UnitParameterSensitivities.of(ENTRY_ZERO0);
		UnitParameterSensitivities sens1plus2plus0a = SENSI_1.combinedWith(ENTRY2).combinedWith(ENTRY_ZERO0);
		UnitParameterSensitivities sens1plus2plus0b = SENSI_1.combinedWith(ENTRY2).combinedWith(ENTRY_ZERO3);
		UnitParameterSensitivities sens1plus2plus0 = SENSI_1.combinedWith(ENTRY2).combinedWith(ENTRY_ZERO0).combinedWith(ENTRY_ZERO3);
		UnitParameterSensitivities sens2plus0 = SENSI_2.combinedWith(sensZeroA);
		assertEquals(SENSI_1.equalWithTolerance(sensZeroA, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(SENSI_1, TOLERENCE_CMP), true);
		assertEquals(SENSI_1.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(SENSI_1.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);

		assertEquals(SENSI_2.equalWithTolerance(sensZeroA, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(SENSI_2, TOLERENCE_CMP), true);
		assertEquals(SENSI_2.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(SENSI_2.equalWithTolerance(sens2plus0, TOLERENCE_CMP), true);

		assertEquals(sensZeroA.equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);
		assertEquals(sensZeroA.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);
		assertEquals(sensZeroA.equalWithTolerance(sensZeroB, TOLERENCE_CMP), true);

		assertEquals(sensZeroB.equalWithTolerance(sensZeroB, TOLERENCE_CMP), true);
		assertEquals(sensZeroB.equalWithTolerance(SENSI_1, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(SENSI_2, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensUsdTotal, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensEur, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sens1plus2, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sens2plus0, TOLERENCE_CMP), false);
		assertEquals(sensZeroB.equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);

		assertEquals(sens1plus2.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0a.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0a, TOLERENCE_CMP), true);
		assertEquals(sens1plus2plus0b.equalWithTolerance(sens1plus2plus0b, TOLERENCE_CMP), true);
		assertEquals(sens2plus0.equalWithTolerance(sens2plus0, TOLERENCE_CMP), true);

		assertEquals(sensZeroA.equalWithTolerance(UnitParameterSensitivities.empty(), TOLERENCE_CMP), true);
		assertEquals(UnitParameterSensitivities.empty().equalWithTolerance(sensZeroA, TOLERENCE_CMP), true);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverImmutableBean(UnitParameterSensitivities.empty());
		coverImmutableBean(SENSI_1);
		coverBeanEquals(SENSI_1, SENSI_2);
	  }

	}

}