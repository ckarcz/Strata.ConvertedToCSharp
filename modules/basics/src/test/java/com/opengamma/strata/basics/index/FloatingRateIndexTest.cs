/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using Tenor = com.opengamma.strata.basics.date.Tenor;

	/// <summary>
	/// Test <seealso cref="FloatingRateIndex"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FloatingRateIndexTest
	public class FloatingRateIndexTest
	{

	  public virtual void test_parse_noTenor()
	  {
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR"), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR-1M"), IborIndices.GBP_LIBOR_1M);
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR-3M"), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.parse("GBP-SONIA"), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateIndex.parse("GB-RPI"), PriceIndices.GB_RPI);
		assertThrowsIllegalArg(() => FloatingRateIndex.parse(null));
		assertThrowsIllegalArg(() => FloatingRateIndex.parse("NotAnIndex"));
	  }

	  public virtual void test_parse_withTenor()
	  {
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_6M);
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR-1M", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_1M);
		assertEquals(FloatingRateIndex.parse("GBP-LIBOR-3M", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.parse("GBP-SONIA", Tenor.TENOR_6M), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateIndex.parse("GB-RPI", Tenor.TENOR_6M), PriceIndices.GB_RPI);
		assertThrowsIllegalArg(() => FloatingRateIndex.parse(null, Tenor.TENOR_6M));
		assertThrowsIllegalArg(() => FloatingRateIndex.parse("NotAnIndex", Tenor.TENOR_6M));
	  }

	  public virtual void test_tryParse_noTenor()
	  {
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR"), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR-1M"), IborIndices.GBP_LIBOR_1M);
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR-3M"), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.tryParse("GBP-SONIA"), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateIndex.tryParse("GB-RPI"), PriceIndices.GB_RPI);
		assertEquals(FloatingRateIndex.tryParse(null), null);
		assertEquals(FloatingRateIndex.tryParse("NotAnIndex"), null);
	  }

	  public virtual void test_tryParse_withTenor()
	  {
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_6M);
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR-1M", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_1M);
		assertEquals(FloatingRateIndex.tryParse("GBP-LIBOR-3M", Tenor.TENOR_6M), IborIndices.GBP_LIBOR_3M);
		assertEquals(FloatingRateIndex.tryParse("GBP-SONIA", Tenor.TENOR_6M), OvernightIndices.GBP_SONIA);
		assertEquals(FloatingRateIndex.tryParse("GB-RPI", Tenor.TENOR_6M), PriceIndices.GB_RPI);
		assertEquals(FloatingRateIndex.tryParse(null, Tenor.TENOR_6M), null);
		assertEquals(FloatingRateIndex.tryParse("NotAnIndex", Tenor.TENOR_6M), null);
	  }

	}

}