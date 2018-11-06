using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	using ToString = org.joda.convert.ToString;

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A name for an item of market data.
	/// <para>
	/// The name is used to locate an item in market data.
	/// While a <seealso cref="MarketDataId"/> is unique within a system, a <seealso cref="MarketDataName"/> is not.
	/// However, it is intended to be unique within any single coherent data set.
	/// </para>
	/// <para>
	/// For example, a curve group contains a set of curves, and within the group the name is unique.
	/// But the market data system may contain many curve groups where the same name appears in each group.
	/// The {@code MarketDataId} includes both the group name and curve name in order to ensure uniqueness.
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the market data this identifier refers to </param>
	public abstract class MarketDataName<T> : Named, IComparable<MarketDataName<JavaToDotNetGenericWildcard>>
	{

	  /// <summary>
	  /// Gets the market data name.
	  /// <para>
	  /// The name must be unique within any single coherent data set.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
	  public override abstract string Name {get;}

	  /// <summary>
	  /// Gets the type of data this name refers to.
	  /// </summary>
	  /// <returns> the type of the market data this name refers to </returns>
	  public abstract Type<T> MarketDataType {get;}

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Compares this name to another.
	  /// <para>
	  /// Instances are compared in alphabetical order based on the name, taking into account the implementation type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the object to compare to </param>
	  /// <returns> the comparison </returns>
	  public virtual int compareTo<T1>(MarketDataName<T1> other)
	  {
		if (this.GetType() == other.GetType())
		{
		  return Name.CompareTo(other.Name);
		}
		return compareSlow(other);
	  }

	  // compare when classes differ, broken out for inlining
	  private int compareSlow<T1>(MarketDataName<T1> other)
	  {
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		return ComparisonChain.start().compare(this.GetType().FullName, other.GetType().FullName).compare(Name, other.Name).result();
	  }

	  /// <summary>
	  /// Checks if this instance equals another.
	  /// <para>
	  /// Instances are compared based on the name and market data type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the object to compare to, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override sealed bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: MarketDataName<?> other = (MarketDataName<?>) obj;
		  MarketDataName<object> other = (MarketDataName<object>) obj;
		  return Name.Equals(other.Name);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code.
	  /// </summary>
	  /// <returns> a suitable hash code </returns>
	  public override sealed int GetHashCode()
	  {
		return Name.GetHashCode() ^ this.GetType().GetHashCode();
	  }

	  /// <summary>
	  /// Returns the name.
	  /// </summary>
	  /// <returns> the name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @ToString public final String toString()
	  public override sealed string ToString()
	  {
		return Name;
	  }

	}

}