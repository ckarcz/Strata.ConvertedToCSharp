using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Mutable builder for sensitivity to a group of curves.
	/// <para>
	/// Contains a mutable list of <seealso cref="PointSensitivity point sensitivity"/> objects, each
	/// referring to a specific point on a curve that was queried.
	/// The order of the list has no specific meaning, but does allow duplicates.
	/// </para>
	/// <para>
	/// This is a mutable builder that is not intended for use in multiple threads.
	/// It is intended to be used to create an immutable <seealso cref="PointSensitivities"/> instance.
	/// Note that each individual point sensitivity implementation is immutable.
	/// </para>
	/// </summary>
	public sealed class MutablePointSensitivities : PointSensitivityBuilder
	{

	  /// <summary>
	  /// The point sensitivities.
	  /// <para>
	  /// Each entry includes details of the curve it relates to.
	  /// </para>
	  /// </summary>
	  private readonly IList<PointSensitivity> sensitivities = new List<PointSensitivity>();

	  /// <summary>
	  /// Creates an empty instance.
	  /// </summary>
	  public MutablePointSensitivities()
	  {
	  }

	  /// <summary>
	  /// Creates an instance with the specified sensitivity.
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity to add </param>
	  public MutablePointSensitivities(PointSensitivity sensitivity)
	  {
		ArgChecker.notNull(sensitivity, "sensitivity");
		this.sensitivities.Add(sensitivity);
	  }

	  /// <summary>
	  /// Creates an instance with the specified sensitivities.
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivities, which is copied </param>
	  public MutablePointSensitivities<T1>(IList<T1> sensitivities) where T1 : PointSensitivity
	  {
		ArgChecker.notNull(sensitivities, "sensitivities");
		((IList<PointSensitivity>)this.sensitivities).AddRange(sensitivities);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of sensitivity entries.
	  /// </summary>
	  /// <returns> the size of the internal list of point sensitivities </returns>
	  public int size()
	  {
		return sensitivities.Count;
	  }

	  /// <summary>
	  /// Gets the immutable list of point sensitivities.
	  /// <para>
	  /// Each entry includes details of the curve it relates to.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the immutable list of sensitivities </returns>
	  public ImmutableList<PointSensitivity> Sensitivities
	  {
		  get
		  {
			return ImmutableList.copyOf(sensitivities);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a point sensitivity, mutating the internal list.
	  /// <para>
	  /// This instance will be mutated, with the new sensitivity added at the end of the list.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivity">  the sensitivity to add </param>
	  /// <returns> {@code this}, for method chaining </returns>
	  public MutablePointSensitivities add(PointSensitivity sensitivity)
	  {
		ArgChecker.notNull(sensitivity, "sensitivity");
		this.sensitivities.Add(sensitivity);
		return this;
	  }

	  /// <summary>
	  /// Adds a list of point sensitivities, mutating the internal list.
	  /// <para>
	  /// This instance will be mutated, with the new sensitivities added at the end of the list.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="sensitivities">  the sensitivities to add </param>
	  /// <returns> {@code this}, for method chaining </returns>
	  public MutablePointSensitivities addAll(IList<PointSensitivity> sensitivities)
	  {
		ArgChecker.notNull(sensitivities, "sensitivities");
		((IList<PointSensitivity>)this.sensitivities).AddRange(sensitivities);
		return this;
	  }

	  /// <summary>
	  /// Merges the list of point sensitivities from another instance, mutating the internal list.
	  /// <para>
	  /// This instance will be mutated, with the new sensitivities added at the end of the list.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other sensitivity to add </param>
	  /// <returns> {@code this}, for method chaining </returns>
	  public MutablePointSensitivities addAll(MutablePointSensitivities other)
	  {
		((IList<PointSensitivity>)this.sensitivities).AddRange(other.sensitivities);
		return this;
	  }

	  //-------------------------------------------------------------------------
	  public MutablePointSensitivities withCurrency(Currency currency)
	  {
		sensitivities.replaceAll(ps => ps.withCurrency(currency));
		return this;
	  }

	  public override MutablePointSensitivities multipliedBy(double factor)
	  {
		return mapSensitivity(s => s * factor);
	  }

	  public MutablePointSensitivities mapSensitivity(System.Func<double, double> @operator)
	  {
		sensitivities.replaceAll(cs => cs.withSensitivity(@operator(cs.Sensitivity)));
		return this;
	  }

	  //-------------------------------------------------------------------------
	  public override MutablePointSensitivities combinedWith(PointSensitivityBuilder other)
	  {
		return other.buildInto(this);
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return (combination == this ? combination : combination.addAll(this));
	  }

	  public override PointSensitivities build()
	  {
		return toImmutable();
	  }

	  public MutablePointSensitivities cloned()
	  {
		return new MutablePointSensitivities(new List<>(sensitivities));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sorts the mutable list of point sensitivities.
	  /// <para>
	  /// Sorts the point sensitivities in this instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> {@code this}, for method chaining </returns>
	  public MutablePointSensitivities sort()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		sensitivities.sort(PointSensitivity::compareKey);
		return this;
	  }

	  /// <summary>
	  /// Normalizes the point sensitivities by sorting and merging, mutating the internal list.
	  /// <para>
	  /// The list of sensitivities is sorted and then merged.
	  /// Any two entries that represent the same curve query are merged.
	  /// For example, if there are two point sensitivities that were created based on the same curve,
	  /// currency and fixing date, then the entries are combined, summing the sensitivity value.
	  /// </para>
	  /// <para>
	  /// The intention is that normalization occurs after gathering all the point sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> {@code this}, for method chaining </returns>
	  public MutablePointSensitivities normalize()
	  {
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		sensitivities.sort(PointSensitivity::compareKey);
		PointSensitivity previous = sensitivities[0];
		for (int i = 1; i < sensitivities.Count; i++)
		{
		  PointSensitivity current = sensitivities[i];
		  if (current.compareKey(previous) == 0)
		  {
			sensitivities[i - 1] = previous.withSensitivity(previous.Sensitivity + current.Sensitivity);
			sensitivities.RemoveAt(i);
			i--;
		  }
		  previous = current;
		}
		return this;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns an immutable version of this object.
	  /// <para>
	  /// The result is an instance of the immutable <seealso cref="PointSensitivities"/>.
	  /// It will contain the same individual point sensitivities.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the immutable sensitivity instance, not null </returns>
	  public PointSensitivities toImmutable()
	  {
		return PointSensitivities.of(sensitivities);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is MutablePointSensitivities)
		{
		  MutablePointSensitivities other = (MutablePointSensitivities) obj;
//JAVA TO C# CONVERTER WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: return sensitivities.equals(other.sensitivities);
		  return sensitivities.SequenceEqual(other.sensitivities);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return sensitivities.GetHashCode();
	  }

	  public override string ToString()
	  {
		return (new StringBuilder(64)).Append("MutablePointSensitivities{sensitivities=").Append(sensitivities).Append('}').ToString();
	  }

	}

}