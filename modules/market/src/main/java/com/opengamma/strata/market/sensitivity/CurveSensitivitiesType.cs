using System;

/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{
	using FromString = org.joda.convert.FromString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The type of curve sensitivities.
	/// <para>
	/// There are many possible types of curve sensitivity, and this type can be used to identify them.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class CurveSensitivitiesType : TypedString<CurveSensitivitiesType>
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
	  /// Type used when each sensitivity is a zero rate delta - 'ZeroRateDelta'.
	  /// This is the first order derivative of <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </summary>
	  public static readonly CurveSensitivitiesType ZERO_RATE_DELTA = of("ZeroRateDelta");
	  /// <summary>
	  /// Type used when each sensitivity is a zero rate gamma - 'ZeroRateGamma'.
	  /// This is the second order derivative of <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </summary>
	  public static readonly CurveSensitivitiesType ZERO_RATE_GAMMA = of("ZeroRateGamma");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Value types must only contains the characters A-Z, a-z, 0-9 and -.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> a field with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CurveSensitivitiesType of(String name)
	  public static CurveSensitivitiesType of(string name)
	  {
		return new CurveSensitivitiesType(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  private CurveSensitivitiesType(string name) : base(name, NAME_MATCHER, "Sensitivity type must only contain the characters A-Z, a-z, 0-9 and -")
	  {
	  }

	}

}