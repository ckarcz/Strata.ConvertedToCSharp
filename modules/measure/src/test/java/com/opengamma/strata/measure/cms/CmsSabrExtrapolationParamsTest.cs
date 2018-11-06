/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.cms
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

	/// <summary>
	/// Test <seealso cref="CmsSabrExtrapolationParams"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CmsSabrExtrapolationParamsTest
	public class CmsSabrExtrapolationParamsTest
	{

	  public virtual void test_of()
	  {
		CmsSabrExtrapolationParams test = CmsSabrExtrapolationParams.of(1d, 2d);
		assertEquals(test.CutOffStrike, 1d);
		assertEquals(test.Mu, 2d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		CmsSabrExtrapolationParams test = CmsSabrExtrapolationParams.of(1d, 2d);
		coverImmutableBean(test);
		CmsSabrExtrapolationParams test2 = CmsSabrExtrapolationParams.of(3d, 4d);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		CmsSabrExtrapolationParams test = CmsSabrExtrapolationParams.of(1d, 2d);
		assertSerialization(test);
	  }

	}

}