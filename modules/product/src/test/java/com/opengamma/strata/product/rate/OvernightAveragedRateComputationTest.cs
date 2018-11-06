/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.rate
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.CHF_TOIS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.USD_FED_FUND;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Index = com.opengamma.strata.basics.index.Index;
	using OvernightIndexObservation = com.opengamma.strata.basics.index.OvernightIndexObservation;

	/// <summary>
	/// Test.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightAveragedRateComputationTest
	public class OvernightAveragedRateComputationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();

	  //-------------------------------------------------------------------------
	  public virtual void test_of_noRateCutoff()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		OvernightAveragedRateComputation expected = OvernightAveragedRateComputation.builder().index(USD_FED_FUND).fixingCalendar(USD_FED_FUND.FixingCalendar.resolve(REF_DATA)).startDate(date(2016, 2, 24)).endDate(date(2016, 3, 24)).rateCutOffDays(0).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_of_noRateCutoff_tomNext()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(CHF_TOIS, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		OvernightAveragedRateComputation expected = OvernightAveragedRateComputation.builder().index(CHF_TOIS).fixingCalendar(CHF_TOIS.FixingCalendar.resolve(REF_DATA)).startDate(date(2016, 2, 23)).endDate(date(2016, 3, 23)).rateCutOffDays(0).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_of_rateCutoff_0()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), 0, REF_DATA);
		OvernightAveragedRateComputation expected = OvernightAveragedRateComputation.builder().index(USD_FED_FUND).fixingCalendar(USD_FED_FUND.FixingCalendar.resolve(REF_DATA)).startDate(date(2016, 2, 24)).endDate(date(2016, 3, 24)).rateCutOffDays(0).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_of_rateCutoff_2()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), 2, REF_DATA);
		OvernightAveragedRateComputation expected = OvernightAveragedRateComputation.builder().index(USD_FED_FUND).fixingCalendar(USD_FED_FUND.FixingCalendar.resolve(REF_DATA)).startDate(date(2016, 2, 24)).endDate(date(2016, 3, 24)).rateCutOffDays(2).build();
		assertEquals(test, expected);
	  }

	  public virtual void test_of_badDateOrder()
	  {
		assertThrowsIllegalArg(() => OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 2, 24), REF_DATA));
		assertThrowsIllegalArg(() => OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 25), date(2016, 2, 24), REF_DATA));
	  }

	  public virtual void test_of_rateCutoff_negative()
	  {
		assertThrowsIllegalArg(() => OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), -1, REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_calculate()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		assertEquals(test.calculateEffectiveFromFixing(date(2016, 2, 24)), USD_FED_FUND.calculateEffectiveFromFixing(date(2016, 2, 24), REF_DATA));
		assertEquals(test.calculateFixingFromEffective(date(2016, 2, 24)), USD_FED_FUND.calculateFixingFromEffective(date(2016, 2, 24), REF_DATA));
		assertEquals(test.calculatePublicationFromFixing(date(2016, 2, 24)), USD_FED_FUND.calculatePublicationFromFixing(date(2016, 2, 24), REF_DATA));
		assertEquals(test.calculateMaturityFromFixing(date(2016, 2, 24)), USD_FED_FUND.calculateMaturityFromFixing(date(2016, 2, 24), REF_DATA));
		assertEquals(test.calculateMaturityFromEffective(date(2016, 2, 24)), USD_FED_FUND.calculateMaturityFromEffective(date(2016, 2, 24), REF_DATA));
	  }

	  public virtual void test_observeOn()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		assertEquals(test.observeOn(date(2016, 2, 24)), OvernightIndexObservation.of(USD_FED_FUND, date(2016, 2, 24), REF_DATA));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_collectIndices()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		test.collectIndices(builder);
		assertEquals(builder.build(), ImmutableSet.of(USD_FED_FUND));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		coverImmutableBean(test);
		OvernightAveragedRateComputation test2 = OvernightAveragedRateComputation.of(GBP_SONIA, date(2014, 6, 3), date(2014, 7, 3), 3, REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightAveragedRateComputation test = OvernightAveragedRateComputation.of(USD_FED_FUND, date(2016, 2, 24), date(2016, 3, 24), REF_DATA);
		assertSerialization(test);
	  }

	}

}