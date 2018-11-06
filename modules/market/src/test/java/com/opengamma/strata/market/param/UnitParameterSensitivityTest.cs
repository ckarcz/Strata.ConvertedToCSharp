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
	/// Test <seealso cref="UnitParameterSensitivity"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class UnitParameterSensitivityTest
	public class UnitParameterSensitivityTest
	{

	  private const double FACTOR1 = 3.14;
	  private static readonly DoubleArray VECTOR1 = DoubleArray.of(100, 200, 300, 123);
	  private static readonly DoubleArray VECTOR1_FACTOR = DoubleArray.of(100 * FACTOR1, 200 * FACTOR1, 300 * FACTOR1, 123 * FACTOR1);
	  private static readonly DoubleArray VECTOR2 = DoubleArray.of(1000, 250, 321, 123, 321);
	  private static readonly DoubleArray VECTOR_COMBINED = VECTOR1.concat(VECTOR2);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME1 = com.opengamma.strata.market.curve.CurveName.of("NAME-1");
	  private static readonly MarketDataName<object> NAME1 = CurveName.of("NAME-1");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME2 = com.opengamma.strata.market.curve.CurveName.of("NAME-2");
	  private static readonly MarketDataName<object> NAME2 = CurveName.of("NAME-2");
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.data.MarketDataName<?> NAME_COMBINED = com.opengamma.strata.market.curve.CurveName.of("NAME-COMBINED");
	  private static readonly MarketDataName<object> NAME_COMBINED = CurveName.of("NAME-COMBINED");
	  private static readonly IList<ParameterMetadata> METADATA1 = ParameterMetadata.listOfEmpty(4);
	  private static readonly IList<ParameterMetadata> METADATA2 = ParameterMetadata.listOfEmpty(5);
	  private static readonly ImmutableList<ParameterMetadata> METADATA_COMBINED = ImmutableList.builder<ParameterMetadata>().addAll(METADATA1).addAll(METADATA2).build();
	  private static readonly IList<ParameterMetadata> METADATA_BAD = ParameterMetadata.listOfEmpty(1);
	  private static readonly IList<ParameterSize> PARAM_SPLIT = ImmutableList.of(ParameterSize.of(NAME1, 4), ParameterSize.of(NAME2, 5));

	  //-------------------------------------------------------------------------
	  public virtual void test_of_metadata()
	  {
		UnitParameterSensitivity test = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		assertEquals(test.MarketDataName, NAME1);
		assertEquals(test.ParameterCount, VECTOR1.size());
		assertEquals(test.ParameterMetadata, METADATA1);
		assertEquals(test.getParameterMetadata(0), METADATA1[0]);
		assertEquals(test.Sensitivity, VECTOR1);
		assertEquals(test.ParameterSplit, null);
	  }

	  public virtual void test_of_metadata_badMetadata()
	  {
		assertThrowsIllegalArg(() => UnitParameterSensitivity.of(NAME1, METADATA_BAD, VECTOR1));
	  }

	  public virtual void test_of_metadataParamSplit()
	  {
		UnitParameterSensitivity test = UnitParameterSensitivity.of(NAME_COMBINED, METADATA_COMBINED, VECTOR_COMBINED, PARAM_SPLIT);
		assertEquals(test.MarketDataName, NAME_COMBINED);
		assertEquals(test.ParameterCount, VECTOR_COMBINED.size());
		assertEquals(test.ParameterMetadata, METADATA_COMBINED);
		assertEquals(test.getParameterMetadata(0), METADATA_COMBINED.get(0));
		assertEquals(test.Sensitivity, VECTOR_COMBINED);
		assertEquals(test.ParameterSplit, PARAM_SPLIT);
	  }

	  public virtual void test_of_metadataParamSplit_badSplit()
	  {
		assertThrowsIllegalArg(() => UnitParameterSensitivity.of(NAME_COMBINED, METADATA1, VECTOR1, PARAM_SPLIT));
	  }

	  public virtual void test_combine()
	  {
		UnitParameterSensitivity base1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity base2 = UnitParameterSensitivity.of(NAME2, METADATA2, VECTOR2);
		UnitParameterSensitivity test = UnitParameterSensitivity.combine(NAME_COMBINED, base1, base2);
		assertEquals(test.MarketDataName, NAME_COMBINED);
		assertEquals(test.ParameterCount, VECTOR_COMBINED.size());
		assertEquals(test.ParameterMetadata, METADATA_COMBINED);
		assertEquals(test.getParameterMetadata(0), METADATA_COMBINED.get(0));
		assertEquals(test.Sensitivity, VECTOR_COMBINED);
		assertEquals(test.ParameterSplit, PARAM_SPLIT);
	  }

	  public virtual void test_combine_arraySize0()
	  {
		assertThrowsIllegalArg(() => UnitParameterSensitivity.combine(NAME_COMBINED));
	  }

	  public virtual void test_combine_arraySize1()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		assertThrowsIllegalArg(() => UnitParameterSensitivity.combine(NAME_COMBINED, @base));
	  }

	  public virtual void test_combine_duplicateNames()
	  {
		UnitParameterSensitivity base1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity base2 = UnitParameterSensitivity.of(NAME1, METADATA2, VECTOR2);
		assertThrowsIllegalArg(() => UnitParameterSensitivity.combine(NAME_COMBINED, base1, base2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy_currency()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		CurrencyParameterSensitivity test = @base.multipliedBy(USD, FACTOR1);
		assertEquals(test, CurrencyParameterSensitivity.of(NAME1, METADATA1, USD, VECTOR1_FACTOR));
	  }

	  public virtual void test_multipliedBy()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity test = @base.multipliedBy(FACTOR1);
		assertEquals(test, UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1_FACTOR));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_withSensitivity()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity test = @base.withSensitivity(VECTOR1_FACTOR);
		assertEquals(test, UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1_FACTOR));
		assertThrowsIllegalArg(() => @base.withSensitivity(DoubleArray.of(1d)));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_array()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity test = @base.plus(VECTOR1);
		assertEquals(test, @base.multipliedBy(2));
	  }

	  public virtual void test_plus_array_wrongSize()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		assertThrowsIllegalArg(() => @base.plus(VECTOR2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_plus_sensitivity()
	  {
		UnitParameterSensitivity base1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity test = base1.plus(base1);
		assertEquals(test, base1.multipliedBy(2));
	  }

	  public virtual void test_plus_sensitivity_wrongName()
	  {
		UnitParameterSensitivity base1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity base2 = UnitParameterSensitivity.of(NAME2, METADATA1, VECTOR1);
		assertThrowsIllegalArg(() => base1.plus(base2));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_split1()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		ImmutableList<UnitParameterSensitivity> test = @base.split();
		assertEquals(test.size(), 1);
		assertEquals(test.get(0), @base);
	  }

	  public virtual void test_split2()
	  {
		UnitParameterSensitivity base1 = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		UnitParameterSensitivity base2 = UnitParameterSensitivity.of(NAME2, METADATA2, VECTOR2);
		UnitParameterSensitivity combined = UnitParameterSensitivity.combine(NAME_COMBINED, base1, base2);
		ImmutableList<UnitParameterSensitivity> test = combined.split();
		assertEquals(test.size(), 2);
		assertEquals(test.get(0), base1);
		assertEquals(test.get(1), base2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_total()
	  {
		UnitParameterSensitivity @base = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		double test = @base.total();
		assertEquals(test, VECTOR1.get(0) + VECTOR1.get(1) + VECTOR1.get(2) + VECTOR1.get(3));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		UnitParameterSensitivity test = UnitParameterSensitivity.of(NAME1, METADATA1, VECTOR1);
		coverImmutableBean(test);
		UnitParameterSensitivity test2 = UnitParameterSensitivity.of(NAME2, METADATA2, VECTOR2);
		coverBeanEquals(test, test2);
	  }

	}

}