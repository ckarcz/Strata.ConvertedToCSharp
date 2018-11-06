using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using HolidayCalendarIds = com.opengamma.strata.basics.date.HolidayCalendarIds;

	/// <summary>
	/// Test <seealso cref="ReferenceData"/> and <seealso cref="ImmutableReferenceData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ReferenceDataTest
	public class ReferenceDataTest
	{

	  private static readonly TestingReferenceDataId ID1 = new TestingReferenceDataId("1");
	  private static readonly TestingReferenceDataId ID2 = new TestingReferenceDataId("2");
	  private static readonly TestingReferenceDataId ID3 = new TestingReferenceDataId("3");
	  private const Number VAL1 = 1;
	  private const Number VAL2 = 2;
	  private const Number VAL3 = 3;
	  private static readonly ReferenceData REF_DATA1 = new ReferenceDataAnonymousInnerClass();

	  private class ReferenceDataAnonymousInnerClass : ReferenceData
	  {
		  public ReferenceDataAnonymousInnerClass()
		  {
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T queryValueOrNull(ReferenceDataId<T> id)
		  public override T queryValueOrNull<T>(ReferenceDataId<T> id)
		  {
			return id.Equals(ID1) ? (T) VAL1 : null;
		  }
	  }
	  private static readonly ReferenceData REF_DATA2 = new ReferenceDataAnonymousInnerClass2();

	  private class ReferenceDataAnonymousInnerClass2 : ReferenceData
	  {
		  public ReferenceDataAnonymousInnerClass2()
		  {
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T queryValueOrNull(ReferenceDataId<T> id)
		  public override T queryValueOrNull<T>(ReferenceDataId<T> id)
		  {
			return id.Equals(ID2) ? (T) VAL2 : null;
		  }
	  }
	  private static readonly ReferenceData REF_DATA3 = new ReferenceDataAnonymousInnerClass3();

	  private class ReferenceDataAnonymousInnerClass3 : ReferenceData
	  {
		  public ReferenceDataAnonymousInnerClass3()
		  {
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T queryValueOrNull(ReferenceDataId<T> id)
		  public override T queryValueOrNull<T>(ReferenceDataId<T> id)
		  {
			return id.Equals(ID1) ? (T) VAL3 : null;
		  }
	  }
	  private static readonly ReferenceData REF_DATA12 = new ReferenceDataAnonymousInnerClass4();

	  private class ReferenceDataAnonymousInnerClass4 : ReferenceData
	  {
		  public ReferenceDataAnonymousInnerClass4()
		  {
		  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public <T> T queryValueOrNull(ReferenceDataId<T> id)
		  public override T queryValueOrNull<T>(ReferenceDataId<T> id)
		  {
			return id.Equals(ID2) ? (T) VAL2 : (id.Equals(ID1) ? (T) VAL1 : null);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_standard()
	  {
		ReferenceData test = ReferenceData.standard();
		assertEquals(test.containsValue(HolidayCalendarIds.NO_HOLIDAYS), true);
		assertEquals(test.containsValue(HolidayCalendarIds.SAT_SUN), true);
		assertEquals(test.containsValue(HolidayCalendarIds.FRI_SAT), true);
		assertEquals(test.containsValue(HolidayCalendarIds.THU_FRI), true);
		assertEquals(test.containsValue(HolidayCalendarIds.GBLO), true);
	  }

	  public virtual void test_minimal()
	  {
		ReferenceData test = ReferenceData.minimal();
		assertEquals(test.containsValue(HolidayCalendarIds.NO_HOLIDAYS), true);
		assertEquals(test.containsValue(HolidayCalendarIds.SAT_SUN), true);
		assertEquals(test.containsValue(HolidayCalendarIds.FRI_SAT), true);
		assertEquals(test.containsValue(HolidayCalendarIds.THU_FRI), true);
		assertEquals(test.containsValue(HolidayCalendarIds.GBLO), false);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_of_RD()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		ReferenceData test = ReferenceData.of(dataMap);

		assertEquals(test.containsValue(HolidayCalendarIds.NO_HOLIDAYS), true);
		assertEquals(test.containsValue(HolidayCalendarIds.SAT_SUN), true);
		assertEquals(test.containsValue(HolidayCalendarIds.FRI_SAT), true);
		assertEquals(test.containsValue(HolidayCalendarIds.THU_FRI), true);

		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.findValue(ID1), VAL1);

		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.findValue(ID2), VAL2);

		assertEquals(test.containsValue(ID3), false);
		assertThrows(() => test.getValue(ID3), typeof(ReferenceDataNotFoundException));
		assertEquals(test.findValue(ID3), null);
	  }

	  public virtual void test_of_IRD()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		ImmutableReferenceData test = ImmutableReferenceData.of(dataMap);

		assertEquals(test.containsValue(HolidayCalendarIds.NO_HOLIDAYS), false);
		assertEquals(test.containsValue(HolidayCalendarIds.SAT_SUN), false);
		assertEquals(test.containsValue(HolidayCalendarIds.FRI_SAT), false);
		assertEquals(test.containsValue(HolidayCalendarIds.THU_FRI), false);

		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.findValue(ID1), VAL1);

		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.findValue(ID2), VAL2);

		assertEquals(test.containsValue(ID3), false);
		assertThrows(() => test.getValue(ID3), typeof(ReferenceDataNotFoundException));
		assertEquals(test.findValue(ID3), null);
	  }

	  public virtual void test_of_single()
	  {
		ReferenceData test = ImmutableReferenceData.of(ID1, VAL1);

		assertEquals(test.containsValue(HolidayCalendarIds.NO_HOLIDAYS), false);
		assertEquals(test.containsValue(HolidayCalendarIds.SAT_SUN), false);
		assertEquals(test.containsValue(HolidayCalendarIds.FRI_SAT), false);
		assertEquals(test.containsValue(HolidayCalendarIds.THU_FRI), false);

		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.findValue(ID1), VAL1);

		assertEquals(test.containsValue(ID2), false);
		assertThrows(() => test.getValue(ID2), typeof(ReferenceDataNotFoundException));
		assertEquals(test.findValue(ID2), null);
	  }

	  public virtual void test_empty()
	  {
		ReferenceData test = ReferenceData.empty();

		assertEquals(test.containsValue(ID1), false);
		assertThrows(() => test.getValue(ID1), typeof(ReferenceDataNotFoundException));
		assertEquals(test.findValue(ID1), null);
	  }

	  public virtual void test_of_badType()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, "67");
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, "67"); // not a Number
		assertThrows(() => ReferenceData.of(dataMap), typeof(System.InvalidCastException));
	  }

	  public virtual void test_of_null()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = new java.util.HashMap<>();
		IDictionary<ReferenceDataId<object>, object> dataMap = new Dictionary<ReferenceDataId<object>, object>();
		dataMap[ID1] = null;
		assertThrows(() => ReferenceData.of(dataMap), typeof(System.ArgumentException));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_defaultMethods()
	  {
		assertEquals(REF_DATA1.containsValue(ID1), true);
		assertEquals(REF_DATA1.containsValue(ID2), false);

		assertEquals(REF_DATA1.getValue(ID1), VAL1);
		assertThrows(() => REF_DATA1.getValue(ID2), typeof(ReferenceDataNotFoundException));

		assertEquals(REF_DATA1.findValue(ID1), VAL1);
		assertEquals(REF_DATA1.findValue(ID2), null);

		assertEquals(REF_DATA1.queryValueOrNull(ID1), VAL1);
		assertEquals(REF_DATA1.queryValueOrNull(ID2), null);

		assertEquals(ID1.queryValueOrNull(REF_DATA1), VAL1);
		assertEquals(ID2.queryValueOrNull(REF_DATA1), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_other_other_noClash()
	  {
		ReferenceData test = REF_DATA1.combinedWith(REF_DATA2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
	  }

	  public virtual void test_combinedWith_other_other_noClashSame()
	  {
		ReferenceData test = REF_DATA1.combinedWith(REF_DATA12);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
	  }

	  public virtual void test_combinedWith_other_other_clash()
	  {
		ReferenceData combined = REF_DATA1.combinedWith(REF_DATA3);
		assertEquals(combined.getValue(ID1), VAL1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_IRD_IRD_noClash()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		ImmutableReferenceData test1 = ImmutableReferenceData.of(dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap2 = ImmutableMap.of(ID2, VAL2);
		ImmutableReferenceData test2 = ImmutableReferenceData.of(dataMap2);

		ReferenceData test = test1.combinedWith(test2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
	  }

	  public virtual void test_combinedWith_IRD_IRD_noClashSame()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		ImmutableReferenceData test1 = ImmutableReferenceData.of(dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		ImmutableReferenceData test2 = ImmutableReferenceData.of(dataMap2);

		ReferenceData test = test1.combinedWith(test2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
	  }

	  public virtual void test_combinedWith_IRD_IRD_clash()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		ImmutableReferenceData test1 = ImmutableReferenceData.of(dataMap1);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID1, VAL3);
		IDictionary<ReferenceDataId<object>, object> dataMap2 = ImmutableMap.of(ID1, VAL3);
		ImmutableReferenceData test2 = ImmutableReferenceData.of(dataMap2);
		ReferenceData combined = test1.combinedWith(test2);
		assertEquals(combined.getValue(ID1), VAL1);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith_IRD_other_noClash()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap1 = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap1 = ImmutableMap.of(ID1, VAL1);
		ImmutableReferenceData test1 = ImmutableReferenceData.of(dataMap1);

		ReferenceData test = test1.combinedWith(REF_DATA2);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		ImmutableReferenceData test = ImmutableReferenceData.of(dataMap);
		coverImmutableBean(test);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap2 = com.google.common.collect.ImmutableMap.of(ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap2 = ImmutableMap.of(ID2, VAL2);
		ImmutableReferenceData test2 = ImmutableReferenceData.of(dataMap2);
		coverBeanEquals(test, test2);

		coverPrivateConstructor(typeof(StandardReferenceData));
	  }

	  public virtual void test_serialization()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1);
		ReferenceData test = ImmutableReferenceData.of(dataMap);
		assertSerialization(test);
	  }

	}

}