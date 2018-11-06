using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// Legal entity group.
	/// </summary>
	[Serializable]
	public sealed class LegalEntityGroup : TypedString<LegalEntityGroup>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Legal entity group names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the legal entity group name </param>
	  /// <returns> a legal entity group with the specified String </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static LegalEntityGroup of(String name)
	  public static LegalEntityGroup of(string name)
	  {
		return new LegalEntityGroup(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the legal entity group name </param>
	  private LegalEntityGroup(string name) : base(name)
	  {
	  }

	}

}