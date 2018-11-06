using System.Collections.Generic;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.LocalDateUtils.daysBetween;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.LocalDateUtils.doy;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static Math.toIntExact;

	using Frequency = com.opengamma.strata.basics.schedule.Frequency;

	/// <summary>
	/// Standard day count convention implementations.
	/// <para>
	/// See <seealso cref="DayCounts"/> for the description of each.
	/// </para>
	/// </summary>
	internal abstract class StandardDayCounts : DayCount
	{

	  // always one
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ONE_ONE = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  return 1;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  return 1;
		  }
	  },

	  // actual days / actual days in year
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_ACT_ISDA = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int y1 = firstDate.getYear();
			  int y2 = secondDate.getYear();
			  double firstYearLength = firstDate.lengthOfYear();
			  if (y1 == y2)
			  {
				  double actualDays = doy(secondDate) - doy(firstDate);
				  return actualDays / firstYearLength;
			  }
			  double firstRemainderOfYear = firstYearLength - doy(firstDate) + 1;
			  double secondRemainderOfYear = doy(secondDate) - 1;
			  double secondYearLength = secondDate.lengthOfYear();
			  return firstRemainderOfYear / firstYearLength + secondRemainderOfYear / secondYearLength + (y2 - y1 - 1);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // complex ICMA calculation
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_ACT_ICMA = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  if (firstDate.equals(secondDate))
			  {
				  return 0d;
			  }
			  java.time.LocalDate scheduleStartDate = scheduleInfo.getStartDate();
			  java.time.LocalDate scheduleEndDate = scheduleInfo.getEndDate();
			  java.time.LocalDate nextCouponDate = scheduleInfo.getPeriodEndDate(firstDate);
			  com.opengamma.strata.basics.schedule.Frequency freq = scheduleInfo.getFrequency();
			  boolean eom = scheduleInfo.isEndOfMonthConvention();
			  if (nextCouponDate.equals(scheduleEndDate))
			  {
				  return finalPeriod(firstDate, secondDate, freq, eom);
			  }
			  if (firstDate.equals(scheduleStartDate))
			  {
				  return initPeriod(firstDate, secondDate, nextCouponDate, freq, eom);
			  }
			  double actualDays = daysBetween(firstDate, secondDate);
			  double periodDays = daysBetween(firstDate, nextCouponDate);
			  return actualDays / (freq.eventsPerYear() * periodDays);
		  }
		  private double initPeriod(java.time.LocalDate startDate, java.time.LocalDate endDate, java.time.LocalDate couponDate, com.opengamma.strata.basics.schedule.Frequency freq, boolean eom)
		  {
			  java.time.LocalDate currentNominal = couponDate;
			  java.time.LocalDate prevNominal = eom(couponDate, currentNominal.minus(freq), eom);
			  double result = 0;
			  while (prevNominal.isAfter(startDate))
			  {
				  result += calc(prevNominal, currentNominal, startDate, endDate, freq);
				  currentNominal = prevNominal;
				  prevNominal = eom(couponDate, currentNominal.minus(freq), eom);
			  }
			  return result + calc(prevNominal, currentNominal, startDate, endDate, freq);
		  }
		  private double finalPeriod(java.time.LocalDate couponDate, java.time.LocalDate endDate, com.opengamma.strata.basics.schedule.Frequency freq, boolean eom)
		  {
			  java.time.LocalDate curNominal = couponDate;
			  java.time.LocalDate nextNominal = eom(couponDate, curNominal.plus(freq), eom);
			  double result = 0;
			  while (nextNominal.isBefore(endDate))
			  {
				  result += calc(curNominal, nextNominal, curNominal, endDate, freq);
				  curNominal = nextNominal;
				  nextNominal = eom(couponDate, curNominal.plus(freq), eom);
			  }
			  return result + calc(curNominal, nextNominal, curNominal, endDate, freq);
		  }
		  private java.time.LocalDate eom(java.time.LocalDate base, java.time.LocalDate calc, boolean eom)
		  {
			  return (eom && base.getDayOfMonth() == base.lengthOfMonth() ? calc.withDayOfMonth(calc.lengthOfMonth()) : calc);
		  }
		  private double calc(java.time.LocalDate prevNominal, java.time.LocalDate curNominal, java.time.LocalDate start, java.time.LocalDate end, com.opengamma.strata.basics.schedule.Frequency freq)
		  {
			  if (end.isAfter(prevNominal))
			  {
				  long curNominalEpochDay = curNominal.toEpochDay();
				  long prevNominalEpochDay = prevNominal.toEpochDay();
				  long startEpochDay = start.toEpochDay();
				  long endEpochDay = end.toEpochDay();
				  double periodDays = curNominalEpochDay - prevNominalEpochDay;
				  double actualDays = Math.min(endEpochDay, curNominalEpochDay) - Math.max(startEpochDay, prevNominalEpochDay);
				  return actualDays / (freq.eventsPerYear() * periodDays);
			  }
			  return 0;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // AFB year-based calculation
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_ACT_AFB = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  java.time.LocalDate end = secondDate;
			  java.time.LocalDate start = secondDate.minusYears(1);
			  int years = 0;
			  while (!start.isBefore(firstDate))
			  {
				  years++;
				  end = start;
				  start = secondDate.minusYears(years + 1);
			  }
			  long actualDays = daysBetween(firstDate, end);
			  java.time.LocalDate nextLeap = DateAdjusters.nextOrSameLeapDay(firstDate);
			  return years + (actualDays / (nextLeap.isBefore(end) ? 366d : 365d));
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // actual days / actual days in year from start date
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_ACT_YEAR = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  java.time.LocalDate startDate = firstDate;
			  int yearsAdded = 0;
			  while (secondDate.compareTo(startDate.plusYears(1)) > 0)
			  {
				  startDate = firstDate.plusYears(++yearsAdded);
			  }
			  double actualDays = daysBetween(startDate, secondDate);
			  double actualDaysInYear = daysBetween(startDate, startDate.plusYears(1));
			  return yearsAdded + (actualDays / actualDaysInYear);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // actual days / 365 or 366
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_365_ACTUAL = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  java.time.LocalDate nextLeap = DateAdjusters.nextLeapDay(firstDate);
			  return actualDays / (nextLeap.isAfter(secondDate) ? 365d : 366d);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // actual days / 365 or 366
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_365L = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  if (firstDate.equals(secondDate))
			  {
				  return 0d;
			  }
			  java.time.LocalDate nextCouponDate = scheduleInfo.getPeriodEndDate(firstDate);
			  if (scheduleInfo.getFrequency().isAnnual())
			  {
				  java.time.LocalDate nextLeap = DateAdjusters.nextLeapDay(firstDate);
				  return actualDays / (nextLeap.isAfter(nextCouponDate) ? 365d : 366d);
			  }
			  else
			  {
				  return actualDays / (nextCouponDate.isLeapYear() ? 366d : 365d);
			  }
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // simple actual days / 360
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_360 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  return daysBetween(firstDate, secondDate) / 360d;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // simple actual days / 364
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_364 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  return daysBetween(firstDate, secondDate) / 364d;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // simple actual days / 365
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_365F = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  return daysBetween(firstDate, secondDate) / 365d;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // simple actual days / 365.25
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts ACT_365_25 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  return daysBetween(firstDate, secondDate) / 365.25d;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  return toIntExact(actualDays);
		  }
	  },

	  // no leaps / 365
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts NL_365 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  int numberOfLeapDays = 0;
			  java.time.LocalDate temp = DateAdjusters.nextLeapDay(firstDate);
			  while (temp.isAfter(secondDate) == false)
			  {
				  numberOfLeapDays++;
				  temp = DateAdjusters.nextLeapDay(temp);
			  }
			  return (actualDays - numberOfLeapDays) / 365d;
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  long actualDays = daysBetween(firstDate, secondDate);
			  int numberOfLeapDays = 0;
			  java.time.LocalDate temp = DateAdjusters.nextLeapDay(firstDate);
			  while (temp.isAfter(secondDate) == false)
			  {
				  numberOfLeapDays++;
				  temp = DateAdjusters.nextLeapDay(temp);
			  }
			  return toIntExact(actualDays) - numberOfLeapDays;
		  }
	  },

	  // ISDA thirty day months / 360
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_360_ISDA = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360Days(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
	  },

	  // US thirty day months / 360 with dynamic EOM rule
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_U_360 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  if (scheduleInfo.isEndOfMonthConvention())
			  {
				  return THIRTY_U_360_EOM.calculateYearFraction(firstDate, secondDate, scheduleInfo);
			  }
			  else
			  {
				  return THIRTY_360_ISDA.calculateYearFraction(firstDate, secondDate, scheduleInfo);
			  }
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  return THIRTY_360_ISDA.days(firstDate, secondDate);
		  }
	  },

	  // US thirty day months / 360 with fixed EOM rule
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_U_360_EOM = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (lastDayOfFebruary(firstDate))
			  {
				  if (lastDayOfFebruary(secondDate))
				  {
					  d2 = 30;
				  }
				  d1 = 30;
			  }
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (lastDayOfFebruary(firstDate))
			  {
				  if (lastDayOfFebruary(secondDate))
				  {
					  d2 = 30;
				  }
				  d1 = 30;
			  }
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360Days(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
	  },

//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_360_PSA = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31 || lastDayOfFebruary(firstDate))
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31 || lastDayOfFebruary(firstDate))
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 && d1 == 30)
			  {
				  d2 = 30;
			  }
			  return thirty360Days(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
	  },

	  // ISDA EU thirty day months / 360
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_E_360_ISDA = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31 || lastDayOfFebruary(firstDate))
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 || (lastDayOfFebruary(secondDate) && !secondDate.equals(scheduleInfo.getEndDate())))
			  {
				  d2 = 30;
			  }
			  return thirty360(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31 || lastDayOfFebruary(firstDate))
			  {
				  d1 = 30;
			  }
			  if (d2 == 31 || (lastDayOfFebruary(secondDate)))
			  {
				  d2 = 30;
			  }
			  return thirty360Days(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
	  },

	  // E thirty day months / 360
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_E_360 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31)
			  {
				  d2 = 30;
			  }
			  return thirty360(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31)
			  {
				  d2 = 30;
			  }
			  return thirty360Days(firstDate.getYear(), firstDate.getMonthValue(), d1, secondDate.getYear(), secondDate.getMonthValue(), d2);
		  }
	  },

	  // E+ thirty day months / 360
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDayCounts THIRTY_EPLUS_360 = new StandardDayCounts()
	  {
		  public double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  int m1 = firstDate.getMonthValue();
			  int m2 = secondDate.getMonthValue();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31)
			  {
				  d2 = 1;
				  m2 = m2 + 1;
			  }
			  return thirty360(firstDate.getYear(), m1, d1, secondDate.getYear(), m2, d2);
		  }
		  public int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate)
		  {
			  int d1 = firstDate.getDayOfMonth();
			  int d2 = secondDate.getDayOfMonth();
			  int m1 = firstDate.getMonthValue();
			  int m2 = secondDate.getMonthValue();
			  if (d1 == 31)
			  {
				  d1 = 30;
			  }
			  if (d2 == 31)
			  {
				  d2 = 1;
				  m2 = m2 + 1;
			  }
			  return thirty360Days(firstDate.getYear(), m1, d1, secondDate.getYear(), m2, d2);
		  }
	  };

	  private static readonly IList<StandardDayCounts> valueList = new List<StandardDayCounts>();

	  static StandardDayCounts()
	  {
		  valueList.Add(ONE_ONE);
		  valueList.Add(ACT_ACT_ISDA);
		  valueList.Add(ACT_ACT_ICMA);
		  valueList.Add(ACT_ACT_AFB);
		  valueList.Add(ACT_ACT_YEAR);
		  valueList.Add(ACT_365_ACTUAL);
		  valueList.Add(ACT_365L);
		  valueList.Add(ACT_360);
		  valueList.Add(ACT_364);
		  valueList.Add(ACT_365F);
		  valueList.Add(ACT_365_25);
		  valueList.Add(NL_365);
		  valueList.Add(THIRTY_360_ISDA);
		  valueList.Add(THIRTY_U_360);
		  valueList.Add(THIRTY_U_360_EOM);
		  valueList.Add(THIRTY_360_PSA);
		  valueList.Add(THIRTY_E_360_ISDA);
		  valueList.Add(THIRTY_E_360);
		  valueList.Add(THIRTY_EPLUS_360);
	  }

	  public enum InnerEnum
	  {
		  ONE_ONE,
		  ACT_ACT_ISDA,
		  ACT_ACT_ICMA,
		  ACT_ACT_AFB,
		  ACT_ACT_YEAR,
		  ACT_365_ACTUAL,
		  ACT_365L,
		  ACT_360,
		  ACT_364,
		  ACT_365F,
		  ACT_365_25,
		  NL_365,
		  THIRTY_360_ISDA,
		  THIRTY_U_360,
		  THIRTY_U_360_EOM,
		  THIRTY_360_PSA,
		  THIRTY_E_360_ISDA,
		  THIRTY_E_360,
		  THIRTY_EPLUS_360
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StandardDayCounts(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: private StandardDayCounts(String name) { this.name = name; } private static double thirty360(int y1, int m1, int d1, int y2, int m2, int d2) { return(360 * (y2 - y1) + 30 * (m2 - m1) + (d2 - d1)) / 360d; } private static int thirty360Days(int y1, int m1, int d1, int y2, int m2, int d2) { return 360 * (y2 - y1) + 30 * (m2 - m1) + (d2 - d1); } private static boolean lastDayOfFebruary(java.time.LocalDate date) { return date.getMonthValue() == 2 && date.getDayOfMonth() == date.lengthOfMonth(); } @Override public double yearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo) { if(secondDate.isBefore(firstDate)) { throw new IllegalArgumentException("Dates must be in time-line order"); } return calculateYearFraction(firstDate, secondDate, scheduleInfo); } @Override public int days(java.time.LocalDate firstDate, java.time.LocalDate secondDate) { if(secondDate.isBefore(firstDate)) { throw new IllegalArgumentException("Dates must be in time-line order"); } return calculateDays(firstDate, secondDate); } @Override public double relativeYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo) { if(secondDate.isBefore(firstDate)) { return -calculateYearFraction(secondDate, firstDate, scheduleInfo); } return calculateYearFraction(firstDate, secondDate, scheduleInfo); } abstract double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo);
	  private StandardDayCounts(string name) {this.name = name;} private static double thirty360(int y1, int m1, int d1, int y2, int m2, int d2) {return(360 * (y2 - y1) + 30 * (m2 - m1) + (d2 - d1)) / 360d;} private static int thirty360Days(int y1, int m1, int d1, int y2, int m2, int d2) {return 360 * (y2 - y1) + 30 * (m2 - m1) + (d2 - d1);} private static bool lastDayOfFebruary(java.time.LocalDate date) {return date.getMonthValue() == 2 && date.getDayOfMonth() == date.lengthOfMonth();} public double yearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo) {if (secondDate.isBefore(firstDate)) {throw new System.ArgumentException("Dates must be in time-line order");} return calculateYearFraction(firstDate, secondDate, scheduleInfo);} public int days(java.time.LocalDate firstDate, java.time.LocalDate secondDate) {if (secondDate.isBefore(firstDate)) {throw new System.ArgumentException("Dates must be in time-line order");} return calculateDays(firstDate, secondDate);} public double relativeYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo) {if (secondDate.isBefore(firstDate)) {return -calculateYearFraction(secondDate, firstDate, scheduleInfo);} return calculateYearFraction(firstDate, secondDate, scheduleInfo);} abstract double calculateYearFraction(java.time.LocalDate firstDate, java.time.LocalDate secondDate, ScheduleInfo scheduleInfo);

	  //calculate the number of days between the specified dates, using validated inputs
	  internal abstract int calculateDays(java.time.LocalDate firstDate, java.time.LocalDate secondDate);

	  //-------------------------------------------------------------------------
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  public override string ToString()
	  {
		return name;
	  }


		public static IList<StandardDayCounts> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static StandardDayCounts valueOf(string name)
		{
			foreach (StandardDayCounts enumInstance in StandardDayCounts.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}