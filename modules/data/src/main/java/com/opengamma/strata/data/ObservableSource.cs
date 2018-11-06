using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data
{
	using FromString = org.joda.convert.FromString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// Identifies the source of observable market data, for example Bloomberg or Reuters.
	/// <para>
	/// The meaning of a source is deliberately abstract, identified only by name.
	/// While it may refer to a major system, such as Bloomberg, it might refer to any
	/// other system or sub-system, such as data from a specific broker.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class ObservableSource : TypedString<ObservableSource>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Matcher for checking the name.
	  /// It must only contains the characters A-Z, a-z, 0-9 and -.
	  /// </summary>
	  private static readonly CharMatcher NAME_MATCHER = CharMatcher.inRange('A', 'Z').or(CharMatcher.inRange('a', 'z')).or(CharMatcher.inRange('0', '9')).or(CharMatcher.@is('-')).precomputed();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// A market data source used when the application does not care about the source.
	  /// </summary>
	  public static readonly ObservableSource NONE = of("None");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Source names must only contains the characters A-Z, a-z, 0-9 and -.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the source </param>
	  /// <returns> a source with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ObservableSource of(String name)
	  public static ObservableSource of(string name)
	  {
		return new ObservableSource(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the source </param>
	  private ObservableSource(string name) : base(name, NAME_MATCHER, "Source name must only contain the characters A-Z, a-z, 0-9 and -")
	  {
	  }

	}

}