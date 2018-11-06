/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertSame;

	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class NoPointSensitivityTest
	public class NoPointSensitivityTest
	{

	  public virtual void test_withCurrency()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		assertSame(@base.withCurrency(GBP), @base); // no effect
		assertSame(@base.withCurrency(USD), @base); // no effect
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_multipliedBy()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		assertSame(@base.multipliedBy(2.0), @base); // no effect
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_mapSensitivity()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		assertSame(@base.mapSensitivity(s => 2.0), @base); // no effect
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_normalize()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		PointSensitivityBuilder test = @base.normalize();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_combinedWith()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		PointSensitivityBuilder ibor = DummyPointSensitivity.of(GBP, date(2015, 6, 30), 2.0d);
		assertSame(@base.combinedWith(ibor), ibor); // returns other
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_buildInto()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		MutablePointSensitivities combo = new MutablePointSensitivities();
		MutablePointSensitivities test = @base.buildInto(combo);
		assertSame(test, combo);
		assertEquals(test.Sensitivities, ImmutableList.of());
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_cloned()
	  {
		PointSensitivityBuilder @base = PointSensitivityBuilder.none();
		PointSensitivityBuilder test = @base.cloned();
		assertSame(test, @base);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_toString()
	  {
		PointSensitivityBuilder test = PointSensitivityBuilder.none();
		assertEquals(test.ToString(), "NoPointSensitivity");
	  }

	}

}