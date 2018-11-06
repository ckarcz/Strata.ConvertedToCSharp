/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.index
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.EUR_EONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.OvernightIndices.GBP_SONIA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="OvernightIndexObservation"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class OvernightIndexObservationTest
	public class OvernightIndexObservationTest
	{

	  private static readonly ReferenceData REF_DATA = ReferenceData.standard();
	  private static readonly LocalDate FIXING_DATE = date(2016, 2, 22);
	  private static readonly LocalDate PUBLICATION_DATE = GBP_SONIA.calculatePublicationFromFixing(FIXING_DATE, REF_DATA);
	  private static readonly LocalDate EFFECTIVE_DATE = GBP_SONIA.calculateEffectiveFromFixing(FIXING_DATE, REF_DATA);
	  private static readonly LocalDate MATURITY_DATE = GBP_SONIA.calculateMaturityFromEffective(EFFECTIVE_DATE, REF_DATA);

	  //-------------------------------------------------------------------------
	  public virtual void test_of()
	  {
		OvernightIndexObservation test = OvernightIndexObservation.of(GBP_SONIA, FIXING_DATE, REF_DATA);
		assertEquals(test.Index, GBP_SONIA);
		assertEquals(test.FixingDate, FIXING_DATE);
		assertEquals(test.PublicationDate, PUBLICATION_DATE);
		assertEquals(test.EffectiveDate, EFFECTIVE_DATE);
		assertEquals(test.MaturityDate, MATURITY_DATE);
		assertEquals(test.Currency, GBP_SONIA.Currency);
		assertEquals(test.ToString(), "OvernightIndexObservation[GBP-SONIA on 2016-02-22]");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		OvernightIndexObservation test = OvernightIndexObservation.of(GBP_SONIA, FIXING_DATE, REF_DATA);
		coverImmutableBean(test);
		OvernightIndexObservation test2 = OvernightIndexObservation.of(EUR_EONIA, FIXING_DATE.plusDays(1), REF_DATA);
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		OvernightIndexObservation test = OvernightIndexObservation.of(GBP_SONIA, FIXING_DATE, REF_DATA);
		assertSerialization(test);
	  }

	}

}