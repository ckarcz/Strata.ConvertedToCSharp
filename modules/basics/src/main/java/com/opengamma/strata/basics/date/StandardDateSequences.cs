using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Standard date sequence implementations.
	/// <para>
	/// See <seealso cref="DateSequences"/> for the description of each.
	/// </para>
	/// </summary>
	internal sealed class StandardDateSequences : DateSequence
	{

	  // IMM in Mar/Jun/Sep/Dec
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDateSequences QUARTERLY_IMM = new StandardDateSequences()
	  {
		  public java.time.LocalDate next(java.time.LocalDate date)
		  {
			  return nth(date, 1);
		  }
		  public java.time.LocalDate nextOrSame(java.time.LocalDate date)
		  {
			  return nthOrSame(date, 1);
		  }
		  public java.time.LocalDate nth(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.with(THIRD_WEDNESDAY);
			  if (!base.isAfter(date))
			  {
				  base = base.plusMonths(1);
			  }
			  return shift(base, sequenceNumber);
		  }
		  public java.time.LocalDate nthOrSame(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.with(THIRD_WEDNESDAY);
			  if (base.isBefore(date))
			  {
				  base = base.plusMonths(1);
			  }
			  return shift(base, sequenceNumber);
		  }
		  private java.time.LocalDate shift(java.time.LocalDate base, int sequenceNumber)
		  {
			  int month = base.getMonthValue();
			  int offset = (month % 3 == 0 ? 0 : 3 - month % 3) + (sequenceNumber - 1) * 3;
			  return base.plusMonths(offset).with(THIRD_WEDNESDAY);
		  }
		  public java.time.LocalDate dateMatching(java.time.YearMonth yearMonth)
		  {
			  return nextOrSame(yearMonth.atDay(1));
		  }
	  },

	  // Third Wednesday
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDateSequences MONTHLY_IMM = new StandardDateSequences()
	  {
		  public java.time.LocalDate next(java.time.LocalDate date)
		  {
			  return nth(date, 1);
		  }
		  public java.time.LocalDate nextOrSame(java.time.LocalDate date)
		  {
			  return nthOrSame(date, 1);
		  }
		  public java.time.LocalDate nth(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.with(THIRD_WEDNESDAY);
			  if (!base.isAfter(date))
			  {
				  return base.plusMonths(sequenceNumber).with(THIRD_WEDNESDAY);
			  }
			  return base.plusMonths(sequenceNumber - 1).with(THIRD_WEDNESDAY);
		  }
		  public java.time.LocalDate nthOrSame(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.with(THIRD_WEDNESDAY);
			  if (base.isBefore(date))
			  {
				  return base.plusMonths(sequenceNumber).with(THIRD_WEDNESDAY);
			  }
			  return base.plusMonths(sequenceNumber - 1).with(THIRD_WEDNESDAY);
		  }
		  public java.time.LocalDate dateMatching(java.time.YearMonth yearMonth)
		  {
			  return yearMonth.atDay(1).with(THIRD_WEDNESDAY);
		  }
	  },

	  // 10th in Mar/Jun/Sep/Dec
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDateSequences QUARTERLY_10TH = new StandardDateSequences()
	  {
		  public java.time.LocalDate next(java.time.LocalDate date)
		  {
			  return nth(date, 1);
		  }
		  public java.time.LocalDate nextOrSame(java.time.LocalDate date)
		  {
			  return nthOrSame(date, 1);
		  }
		  public java.time.LocalDate nth(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.withDayOfMonth(10);
			  if (!base.isAfter(date))
			  {
				  base = base.plusMonths(1);
			  }
			  return shift(base, sequenceNumber);
		  }
		  public java.time.LocalDate nthOrSame(java.time.LocalDate date, int sequenceNumber)
		  {
			  com.opengamma.strata.collect.ArgChecker.notNegativeOrZero(sequenceNumber, "sequenceNumber");
			  java.time.LocalDate base = date.withDayOfMonth(10);
			  if (base.isBefore(date))
			  {
				  base = base.plusMonths(1);
			  }
			  return shift(base, sequenceNumber);
		  }
		  private java.time.LocalDate shift(java.time.LocalDate base, int sequenceNumber)
		  {
			  int month = base.getMonthValue();
			  int offset = (month % 3 == 0 ? 0 : 3 - month % 3) + (sequenceNumber - 1) * 3;
			  return base.plusMonths(offset).withDayOfMonth(10);
		  }
		  public java.time.LocalDate dateMatching(java.time.YearMonth yearMonth)
		  {
			  return nextOrSame(yearMonth.atDay(1));
		  }
	  };

	  private static readonly IList<StandardDateSequences> valueList = new List<StandardDateSequences>();

	  static StandardDateSequences()
	  {
		  valueList.Add(QUARTERLY_IMM);
		  valueList.Add(MONTHLY_IMM);
		  valueList.Add(QUARTERLY_10TH);
	  }

	  public enum InnerEnum
	  {
		  QUARTERLY_IMM,
		  MONTHLY_IMM,
		  QUARTERLY_10TH
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private StandardDateSequences(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // Third Wednesday
	  private static readonly java.time.temporal.TemporalAdjuster THIRD_WEDNESDAY = java.time.temporal.TemporalAdjusters.dayOfWeekInMonth(3, java.time.DayOfWeek.WEDNESDAY);

	  // name
	  private readonly string name;

	  // create
//JAVA TO C# CONVERTER TODO TASK: Enum value-specific class bodies are not converted by Java to C# Converter:
	  public static readonly StandardDateSequences private = new StandardDateSequences()
	  {
		  this.name = name;
	  }
	  public String getName()
	  {
		  return name;
	  }
	  public String toString()
	  {
		  return name;
	  }


		public static IList<StandardDateSequences> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static StandardDateSequences valueOf(string name)
		{
			foreach (StandardDateSequences enumInstance in StandardDateSequences.valueList)
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