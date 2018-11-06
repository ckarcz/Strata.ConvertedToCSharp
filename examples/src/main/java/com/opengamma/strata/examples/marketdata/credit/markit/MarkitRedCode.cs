using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.examples.marketdata.credit.markit
{
	using FromString = org.joda.convert.FromString;

	using Preconditions = com.google.common.@base.Preconditions;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// A simple string type to contain a 6 or 9 character Markit RED Code.
	/// <para>
	/// static utilities to convert from or to StandardIds with a fixed schema
	/// </para>
	/// <para>
	/// http://www.markit.com/product/reference-data-cds
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class MarkitRedCode : TypedString<MarkitRedCode>
	{

	  /// <summary>
	  /// Serialization version.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Scheme used in an OpenGamma <seealso cref="StandardId"/> where the value is a Markit RED code.
	  /// </summary>
	  public const string MARKIT_REDCODE_SCHEME = "MarkitRedCode";

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// RED codes must be 6 or 9 characters long.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> a RED code </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static MarkitRedCode of(String name)
	  public static MarkitRedCode of(string name)
	  {
		ArgChecker.isTrue(name.Length == 6 || name.Length == 9, "RED Code must be exactly 6 or 9 characters");
		return new MarkitRedCode(name);
	  }

	  /// <summary>
	  /// Converts from a standard identifier ensuring the scheme is correct.
	  /// </summary>
	  /// <param name="id"> standard id identifying a RED code </param>
	  /// <returns> the equivalent RED code </returns>
	  public static MarkitRedCode from(StandardId id)
	  {
		Preconditions.checkArgument(id.Scheme.Equals(MARKIT_REDCODE_SCHEME));
		return MarkitRedCode.of(id.Value);
	  }

	  /// <summary>
	  /// Creates a standard identifier using the correct Markit RED code scheme.
	  /// </summary>
	  /// <param name="name">  the Markit RED code, 6 or 9 characters long </param>
	  /// <returns> the equivalent standard identifier </returns>
	  public static StandardId id(string name)
	  {
		ArgChecker.isTrue(name.Length == 6 || name.Length == 9, "RED Code must be exactly 6 or 9 characters");
		return StandardId.of(MARKIT_REDCODE_SCHEME, name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the RED code </param>
	  private MarkitRedCode(string name) : base(name)
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Converts this RED code to a standard identifier.
	  /// </summary>
	  /// <returns> the standard identifier </returns>
	  public StandardId toStandardId()
	  {
		return StandardId.of(MARKIT_REDCODE_SCHEME, Name);
	  }

	}

}