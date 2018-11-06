/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.Tenor.TENOR_6M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// Tests <seealso cref="TenorRawOptionData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class TenorRawOptionDataTest
	public class TenorRawOptionDataTest
	{

	  private static readonly RawOptionData DATA1 = RawOptionDataTest.sut();
	  private static readonly RawOptionData DATA2 = RawOptionDataTest.sut2();

	  private static readonly ImmutableMap<Tenor, RawOptionData> DATA_MAP = ImmutableMap.of(TENOR_3M, DATA1, TENOR_6M, DATA2);

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		TenorRawOptionData test = TenorRawOptionData.of(DATA_MAP);
		assertEquals(test.Data, DATA_MAP);
		assertEquals(test.getData(TENOR_3M), DATA1);
		assertEquals(test.Tenors, ImmutableList.of(TENOR_3M, TENOR_6M));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		TenorRawOptionData test = TenorRawOptionData.of(DATA_MAP);
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		TenorRawOptionData test = TenorRawOptionData.of(DATA_MAP);
		assertSerialization(test);
	  }

	}

}