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
	/// Group used to identify a related set of repo curves when pricing bonds.
	/// <para>
	/// This class was previously called {@code BondGroup}.
	/// It was renamed in version 1.1 of Strata to allow {@code LegalEntityDiscountingProvider}
	/// to be used for pricing bills as well as bonds.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class RepoGroup : TypedString<RepoGroup>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Group names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the group name </param>
	  /// <returns> a group with the specified String </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static RepoGroup of(String name)
	  public static RepoGroup of(string name)
	  {
		return new RepoGroup(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the group name </param>
	  internal RepoGroup(string name) : base(name)
	  {
	  }

	}

}