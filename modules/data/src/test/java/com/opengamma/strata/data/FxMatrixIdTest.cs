/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using FxMatrix = com.opengamma.strata.basics.currency.FxMatrix;

	/// <summary>
	/// Test <seealso cref="FxMatrixId"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FxMatrixIdTest
	public class FxMatrixIdTest
	{

	  private static readonly ObservableSource OBS_SOURCE = ObservableSource.of("Vendor");

	  //-------------------------------------------------------------------------
	  public virtual void test_standard()
	  {
		FxMatrixId test = FxMatrixId.standard();
		assertEquals(test.ObservableSource, ObservableSource.NONE);
		assertEquals(test.MarketDataType, typeof(FxMatrix));
		assertEquals(test.ToString(), "FxMatrixId");
	  }

	  public virtual void test_of()
	  {
		FxMatrixId test = FxMatrixId.of(OBS_SOURCE);
		assertEquals(test.ObservableSource, OBS_SOURCE);
		assertEquals(test.MarketDataType, typeof(FxMatrix));
		assertEquals(test.ToString(), "FxMatrixId:Vendor");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		FxMatrixId test = FxMatrixId.standard();
		coverImmutableBean(test);
		FxMatrixId test2 = FxMatrixId.of(OBS_SOURCE);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		FxMatrixId test = FxMatrixId.standard();
		assertSerialization(test);
	  }

	}

}