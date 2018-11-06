/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndexObservation = com.opengamma.strata.basics.index.IborIndexObservation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborAveragedFixingTest
	public class IborAveragedFixingTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly IborIndexObservation GBP_LIBOR_3M_OBS = IborIndexObservation.of(GBP_LIBOR_3M, date(2014, 6, 30), REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_date()
	  {
		IborAveragedFixing test = IborAveragedFixing.of(GBP_LIBOR_3M_OBS);
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(null).weight(1).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_of_date_fixedRate()
	  {
		IborAveragedFixing test = IborAveragedFixing.of(GBP_LIBOR_3M_OBS, 0.05);
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(0.05).weight(1).build();
		assertEquals(test, expected);
		assertEquals(test.FixedRate, double?.of(0.05));
	  }

	  public virtual void test_of_date_fixedRate_null()
	  {
		IborAveragedFixing test = IborAveragedFixing.of(GBP_LIBOR_3M_OBS, null);
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(null).weight(1).build();
		assertEquals(test, expected);
		assertEquals(test.FixedRate, double?.empty());
	  }

	  public virtual void test_of_date_null()
	  {
		assertThrowsIllegalArg(() => IborAveragedFixing.of(null));
		assertThrowsIllegalArg(() => IborAveragedFixing.of(null, 0.05));
		assertThrowsIllegalArg(() => IborAveragedFixing.of(null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_ofDaysInResetPeriod()
	  {
		IborAveragedFixing test = IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, date(2014, 7, 2), date(2014, 8, 2));
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(null).weight(31).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_ofDaysInResetPeriod_fixedRate()
	  {
		IborAveragedFixing test = IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, date(2014, 7, 2), date(2014, 9, 2), 0.06);
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(0.06).weight(62).build();
		assertEquals(test, expected);
		assertEquals(test.FixedRate, double?.of(0.06));
	  }

	  public virtual void test_ofDaysInResetPeriod_fixedRate_null()
	  {
		IborAveragedFixing test = IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, date(2014, 7, 2), date(2014, 9, 2), null);
		IborAveragedFixing expected = IborAveragedFixing.builder().observation(GBP_LIBOR_3M_OBS).fixedRate(null).weight(62).build();
		assertEquals(test, expected);
		assertEquals(test.FixedRate, double?.empty());
	  }

	  public virtual void test_ofDaysInResetPeriod_null()
	  {
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(null, date(2014, 7, 2), date(2014, 8, 2)));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, null, date(2014, 8, 2)));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, date(2014, 7, 2), null));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(null, null, null));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(null, date(2014, 7, 2), date(2014, 8, 2), 0.05));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, null, date(2014, 8, 2), 0.05));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(GBP_LIBOR_3M_OBS, date(2014, 7, 2), null, 0.05));
		assertThrowsIllegalArg(() => IborAveragedFixing.ofDaysInResetPeriod(null, null, null, null));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		IborAveragedFixing test = IborAveragedFixing.of(GBP_LIBOR_3M_OBS);
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		IborAveragedFixing test = IborAveragedFixing.of(GBP_LIBOR_3M_OBS);
		assertSerialization(test);
	  }

	}

}