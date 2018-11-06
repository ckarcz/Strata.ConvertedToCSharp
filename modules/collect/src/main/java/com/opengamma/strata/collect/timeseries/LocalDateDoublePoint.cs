using System;
using System.Text;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.timeseries
{

	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;

	using ComparisonChain = com.google.common.collect.ComparisonChain;

	/// <summary>
	/// Immutable representation of a single point in a {@code LocalDateDoubleTimeSeries}.
	/// <para>
	/// This implementation uses arrays internally.
	/// </para>
	/// </summary>
	public sealed class LocalDateDoublePoint : IComparable<LocalDateDoublePoint>
	{

	  /// <summary>
	  /// The date.
	  /// </summary>
	  private readonly LocalDate date;
	  /// <summary>
	  /// The value.
	  /// </summary>
	  private readonly double value;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a point from date and value.
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <param name="value">  the value </param>
	  /// <returns> the point </returns>
	  public static LocalDateDoublePoint of(LocalDate date, double value)
	  {
		return new LocalDateDoublePoint(date, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="date">  the date </param>
	  /// <param name="value">  the value </param>
	  private LocalDateDoublePoint(LocalDate date, double value)
	  {
		this.date = ArgChecker.notNull(date, "date");
		this.value = value;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date.
	  /// </summary>
	  /// <returns> the date </returns>
	  public LocalDate Date
	  {
		  get
		  {
			return date;
		  }
	  }

	  /// <summary>
	  /// Gets the value.
	  /// </summary>
	  /// <returns> the value </returns>
	  public double Value
	  {
		  get
		  {
			return value;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this point with another date.
	  /// </summary>
	  /// <param name="date">  the date to change the point to </param>
	  /// <returns> a point based on this point with the date changed </returns>
	  public LocalDateDoublePoint withDate(LocalDate date)
	  {
		return LocalDateDoublePoint.of(date, value);
	  }

	  /// <summary>
	  /// Returns a copy of this point with another value.
	  /// </summary>
	  /// <param name="value">  the value to change the point to </param>
	  /// <returns> a point based on this point with the value changed </returns>
	  public LocalDateDoublePoint withValue(double value)
	  {
		return LocalDateDoublePoint.of(date, value);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this point to another.
	  /// <para>
	  /// The sort order is by date, then by double.
	  /// This is compatible with equals.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other point </param>
	  /// <returns> negative if this is less than, zero if equal, positive if greater than </returns>
	  public int CompareTo(LocalDateDoublePoint other)
	  {
		return ComparisonChain.start().compare(date, other.date).compare(value, other.value).result();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this point is equal to another point.
	  /// </summary>
	  /// <param name="obj">  the object to check, null returns false </param>
	  /// <returns> true if this is equal to the other point </returns>
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj is LocalDateDoublePoint)
		{
		  LocalDateDoublePoint other = (LocalDateDoublePoint) obj;
		  return date.Equals(other.date) && JodaBeanUtils.equal(value, other.value);
		}
		return false;
	  }

	  /// <summary>
	  /// A hash code for this point.
	  /// </summary>
	  /// <returns> a suitable hash code </returns>
	  public override int GetHashCode()
	  {
		return date.GetHashCode() ^ JodaBeanUtils.GetHashCode(value);
	  }

	  /// <summary>
	  /// Returns a string representation of the point.
	  /// </summary>
	  /// <returns> the string </returns>
	  public override string ToString()
	  {
		return (new StringBuilder(24)).Append('(').Append(date).Append('=').Append(value).Append(')').ToString();
	  }

	}

}