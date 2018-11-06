using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.explain
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;

	/// <summary>
	/// Test <seealso cref="ExplainMap"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ExplainMapTest
	public class ExplainMapTest
	{

	  private static readonly string EOL = Environment.NewLine;
	  private static readonly LocalDate DATE1 = date(2015, 6, 30);
	  private static readonly LocalDate DATE2 = date(2015, 9, 30);
	  private static readonly CurrencyAmount AMOUNT1 = CurrencyAmount.of(GBP, 1000);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> map = new java.util.HashMap<>();
		IDictionary<ExplainKey<object>, object> map = new Dictionary<ExplainKey<object>, object>();
		map[ExplainKey.START_DATE] = DATE1;
		map[ExplainKey.END_DATE] = DATE2;
		map[ExplainKey.PRESENT_VALUE] = AMOUNT1;
		ExplainMap test = ExplainMap.of(map);
		assertEquals(test.Map, map);
		assertEquals(test.get(ExplainKey.START_DATE), DATE1);
		assertEquals(test.get(ExplainKey.END_DATE), DATE2);
		assertEquals(test.get(ExplainKey.ACCRUAL_DAY_COUNT), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_builder_simple()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		builder.put(ExplainKey.ACCRUAL_DAYS, 2);
		ExplainMap test = builder.build();
		assertEquals(test.Map.size(), 1);
		assertEquals(test.get(ExplainKey.ACCRUAL_DAYS), 2);
		assertEquals(test.get(ExplainKey.ACCRUAL_DAY_COUNT), null);
	  }

	  public virtual void test_builder_openClose()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		ExplainMapBuilder child = builder.openListEntry(ExplainKey.LEGS);
		child.put(ExplainKey.ACCRUAL_DAYS, 2);
		ExplainMapBuilder result = child.closeListEntry(ExplainKey.LEGS);
		ExplainMap test = result.build();
		assertEquals(test.Map.size(), 1);
		assertEquals(test.get(ExplainKey.LEGS).Present, true);
		assertEquals(test.get(ExplainKey.LEGS).get().size(), 1);
		assertEquals(test.get(ExplainKey.LEGS).get().get(0).get(ExplainKey.ACCRUAL_DAYS), 2);
	  }

	  public virtual void test_builder_openClose_wrongCloseKey()
	  {
		ExplainMapBuilder builder = ExplainMap.builder();
		ExplainMapBuilder child = builder.openListEntry(ExplainKey.LEGS);
		child.put(ExplainKey.ACCRUAL_DAYS, 2);
		assertThrows(() => child.closeListEntry(ExplainKey.PAYMENT_PERIODS), typeof(System.InvalidOperationException));
	  }

	  public virtual void test_builder_addListEntry()
	  {
		ExplainMapBuilder @base = ExplainMap.builder();
		ExplainMapBuilder result1 = @base.addListEntry(ExplainKey.LEGS, child => child.put(ExplainKey.ACCRUAL_DAYS, 2));
		ExplainMapBuilder result2 = result1.addListEntry(ExplainKey.LEGS, child => child.put(ExplainKey.ACCRUAL_DAYS, 3));
		ExplainMap test = result2.build();
		assertEquals(test.Map.size(), 1);
		assertEquals(test.get(ExplainKey.LEGS).Present, true);
		assertEquals(test.get(ExplainKey.LEGS).get().size(), 2);
		assertEquals(test.get(ExplainKey.LEGS).get().get(0).get(ExplainKey.ACCRUAL_DAYS), 2);
		assertEquals(test.get(ExplainKey.LEGS).get().get(1).get(ExplainKey.ACCRUAL_DAYS), 3);
	  }

	  public virtual void test_builder_addListEntryWithIndex()
	  {
		ExplainMapBuilder @base = ExplainMap.builder();
		ExplainMapBuilder result1 = @base.addListEntryWithIndex(ExplainKey.LEGS, child => child.put(ExplainKey.ACCRUAL_DAYS, 2));
		ExplainMapBuilder result2 = result1.addListEntryWithIndex(ExplainKey.LEGS, child => child.put(ExplainKey.ACCRUAL_DAYS, 3));
		ExplainMap test = result2.build();
		assertEquals(test.Map.size(), 1);
		assertEquals(test.get(ExplainKey.LEGS).Present, true);
		assertEquals(test.get(ExplainKey.LEGS).get().size(), 2);
		assertEquals(test.get(ExplainKey.LEGS).get().get(0).get(ExplainKey.ENTRY_INDEX), 0);
		assertEquals(test.get(ExplainKey.LEGS).get().get(0).get(ExplainKey.ACCRUAL_DAYS), 2);
		assertEquals(test.get(ExplainKey.LEGS).get().get(1).get(ExplainKey.ENTRY_INDEX), 1);
		assertEquals(test.get(ExplainKey.LEGS).get().get(1).get(ExplainKey.ACCRUAL_DAYS), 3);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_explanationString()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> child1map = new java.util.LinkedHashMap<>();
		IDictionary<ExplainKey<object>, object> child1map = new LinkedHashMap<ExplainKey<object>, object>();
		child1map[ExplainKey.PAYMENT_PERIODS] = ImmutableList.of(ExplainMap.of(ImmutableMap.of()));
		child1map[ExplainKey.INDEX_VALUE] = 1.2d;
		child1map[ExplainKey.COMBINED_RATE] = 1.4d;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> child2map = new java.util.LinkedHashMap<>();
		IDictionary<ExplainKey<object>, object> child2map = new LinkedHashMap<ExplainKey<object>, object>();
		child2map[ExplainKey.INDEX_VALUE] = 2.3d;

		IList<ExplainMap> list1 = new List<ExplainMap>();
		IList<ExplainMap> list2 = new List<ExplainMap>();
		list2.Add(ExplainMap.of(child1map));
		list2.Add(ExplainMap.of(child2map));

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> map = new java.util.LinkedHashMap<>();
		IDictionary<ExplainKey<object>, object> map = new LinkedHashMap<ExplainKey<object>, object>();
		map[ExplainKey.LEGS] = list1;
		map[ExplainKey.START_DATE] = DATE1;
		map[ExplainKey.END_DATE] = DATE2;
		map[ExplainKey.OBSERVATIONS] = list2;
		map[ExplainKey.PRESENT_VALUE] = AMOUNT1;

		ExplainMap test = ExplainMap.of(map);
		assertEquals(test.explanationString(), "" + "ExplainMap {" + EOL + "  Legs = []," + EOL + "  StartDate = 2015-06-30," + EOL + "  EndDate = 2015-09-30," + EOL + "  Observations = [{" + EOL + "    PaymentPeriods = [{" + EOL + "    }]," + EOL + "    IndexValue = 1.2," + EOL + "    CombinedRate = 1.4" + EOL + "  },{" + EOL + "    IndexValue = 2.3" + EOL + "  }]," + EOL + "  PresentValue = GBP 1000" + EOL + "}" + EOL);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> map = new java.util.HashMap<>();
		IDictionary<ExplainKey<object>, object> map = new Dictionary<ExplainKey<object>, object>();
		map[ExplainKey.START_DATE] = DATE1;
		map[ExplainKey.END_DATE] = DATE2;
		ExplainMap test = ExplainMap.of(map);
		coverImmutableBean(test);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> map2 = new java.util.HashMap<>();
		IDictionary<ExplainKey<object>, object> map2 = new Dictionary<ExplainKey<object>, object>();
		map[ExplainKey.START_DATE] = DATE2;
		ExplainMap test2 = ExplainMap.of(map2);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ExplainKey<?>, Object> map = new java.util.HashMap<>();
		IDictionary<ExplainKey<object>, object> map = new Dictionary<ExplainKey<object>, object>();
		map[ExplainKey.START_DATE] = DATE1;
		map[ExplainKey.END_DATE] = DATE2;
		ExplainMap test = ExplainMap.of(map);
		assertSerialization(test);
	  }

	}

}