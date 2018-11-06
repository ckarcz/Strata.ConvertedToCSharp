using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.datasets
{

	/// <summary>
	/// List of central bank meeting dates used for testing.
	/// </summary>
	public class MeetingDatesDataSets
	{

	  public static readonly IList<LocalDate> FOMC_MEETINGS_2015 = new List<LocalDate>();
	  static MeetingDatesDataSets()
	  {
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 1, 28));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 3, 18));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 4, 29));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 6, 17));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 7, 29));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 9, 17));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 10, 28));
		FOMC_MEETINGS_2015.Add(LocalDate.of(2015, 12, 16));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 1, 27));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 3, 16));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 4, 27));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 6, 15));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 7, 27));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 9, 21));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 11, 2));
		FOMC_MEETINGS_2016.Add(LocalDate.of(2016, 12, 14));
		((IList<LocalDate>)FOMC_MEETINGS).AddRange(FOMC_MEETINGS_2015);
		((IList<LocalDate>)FOMC_MEETINGS).AddRange(FOMC_MEETINGS_2016);
	  };
	  public static readonly IList<LocalDate> FOMC_MEETINGS_2016 = new List<LocalDate>();

	  public static readonly IList<LocalDate> FOMC_MEETINGS = new List<LocalDate>();

	}

}