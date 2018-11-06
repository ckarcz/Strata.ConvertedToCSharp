using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
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
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

	/// <summary>
	/// Test <seealso cref="CombinedReferenceData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CombinedReferenceDataTest
	public class CombinedReferenceDataTest
	{

	  private static readonly TestingReferenceDataId ID1 = new TestingReferenceDataId("1");
	  private static readonly TestingReferenceDataId ID2 = new TestingReferenceDataId("2");
	  private static readonly TestingReferenceDataId ID3 = new TestingReferenceDataId("3");
	  private static readonly TestingReferenceDataId ID4 = new TestingReferenceDataId("4");
	  private const double? VAL1 = 123d;
	  private const double? VAL2 = 234d;
	  private const double? VAL3 = 999d;
	  private static readonly ImmutableReferenceData BASE_DATA1 = baseData1();
	  private static readonly ImmutableReferenceData BASE_DATA2 = baseData2();

	  //-------------------------------------------------------------------------
	  public virtual void test_combination()
	  {
		CombinedReferenceData test = new CombinedReferenceData(BASE_DATA1, BASE_DATA2);
		assertEquals(test.containsValue(ID1), true);
		assertEquals(test.containsValue(ID2), true);
		assertEquals(test.containsValue(ID3), true);
		assertEquals(test.containsValue(ID4), false);
		assertEquals(test.getValue(ID1), VAL1);
		assertEquals(test.getValue(ID2), VAL2);
		assertEquals(test.getValue(ID3), VAL3);
		assertThrows(() => test.getValue(ID4), typeof(ReferenceDataNotFoundException));
		assertEquals(test.findValue(ID1), VAL1);
		assertEquals(test.findValue(ID2), VAL2);
		assertEquals(test.findValue(ID3), VAL3);
		assertEquals(test.findValue(ID4), null);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CombinedReferenceData test = new CombinedReferenceData(BASE_DATA1, BASE_DATA2);
		coverImmutableBean(test);
		CombinedReferenceData test2 = new CombinedReferenceData(BASE_DATA2, BASE_DATA1);
		coverBeanEquals(test, test2);
	  }

	  public virtual void serialization()
	  {
		CombinedReferenceData test = new CombinedReferenceData(BASE_DATA1, BASE_DATA2);
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  private static ImmutableReferenceData baseData1()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL1, ID2, VAL2);
		return ImmutableReferenceData.of(dataMap);
	  }

	  private static ImmutableReferenceData baseData2()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<ReferenceDataId<?>, Object> dataMap = com.google.common.collect.ImmutableMap.of(ID1, VAL3, ID3, VAL3);
		IDictionary<ReferenceDataId<object>, object> dataMap = ImmutableMap.of(ID1, VAL3, ID3, VAL3);
		return ImmutableReferenceData.of(dataMap);
	  }

	}

}