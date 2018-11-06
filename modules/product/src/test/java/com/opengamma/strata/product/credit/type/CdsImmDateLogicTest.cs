/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.credit.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertTrue;


	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="CdsImmDateLogic"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CdsImmDateLogicTest
	public class CdsImmDateLogicTest
	{

	  public virtual void onImmDateTest()
	  {
		LocalDate today = LocalDate.of(2013, 3, 20);
		LocalDate prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(prevIMM, LocalDate.of(2012, 12, 20));

		today = LocalDate.of(2017, 6, 20);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2017, 3, 20), prevIMM);

		today = LocalDate.of(2011, 9, 20);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2011, 6, 20), prevIMM);

		today = LocalDate.of(2015, 12, 20);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2015, 9, 20), prevIMM);
	  }

	  public virtual void PreviousImmTest()
	  {
		LocalDate today = LocalDate.of(2011, 6, 21);
		LocalDate prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2011, 6, 20), prevIMM);

		prevIMM = CdsImmDateLogic.getPreviousImmDate(CdsImmDateLogic.getPreviousImmDate(prevIMM));
		assertEquals(LocalDate.of(2010, 12, 20), prevIMM);

		today = LocalDate.of(2011, 6, 18);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2011, 3, 20), prevIMM);

		today = LocalDate.of(1976, 7, 30);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(1976, 6, 20), prevIMM);

		today = LocalDate.of(1977, 2, 13);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(1976, 12, 20), prevIMM);

		today = LocalDate.of(2013, 3, 1);
		prevIMM = CdsImmDateLogic.getPreviousImmDate(today);
		assertEquals(LocalDate.of(2012, 12, 20), prevIMM);
	  }

	  public virtual void isSemiAnnualRollDateTest()
	  {
		LocalDate date0 = LocalDate.of(2013, 3, 14);
		LocalDate date1 = LocalDate.of(2013, 6, 20);
		LocalDate date2 = LocalDate.of(2013, 3, 20);
		LocalDate date3 = LocalDate.of(2013, 9, 20);
		assertFalse(CdsImmDateLogic.isSemiAnnualRollDate(date0));
		assertFalse(CdsImmDateLogic.isSemiAnnualRollDate(date1));
		assertTrue(CdsImmDateLogic.isSemiAnnualRollDate(date2));
		assertTrue(CdsImmDateLogic.isSemiAnnualRollDate(date3));
	  }

	  public virtual void getNextSemiAnnualRollDateTest()
	  {
		LocalDate[] dates = new LocalDate[] {LocalDate.of(2013, 3, 14), LocalDate.of(2013, 6, 20), LocalDate.of(2013, 3, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2013, 1, 21), LocalDate.of(2013, 3, 21), LocalDate.of(2013, 9, 19), LocalDate.of(2013, 9, 21), LocalDate.of(2013, 11, 21)};
		LocalDate[] datesExp = new LocalDate[] {LocalDate.of(2013, 3, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2014, 3, 20), LocalDate.of(2013, 3, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2013, 9, 20), LocalDate.of(2014, 3, 20), LocalDate.of(2014, 3, 20)};
		for (int i = 0; i < dates.Length; ++i)
		{
		  assertEquals(CdsImmDateLogic.getNextSemiAnnualRollDate(dates[i]), datesExp[i]);
		}
	  }

	}

}