using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect
{

	using FromString = org.joda.convert.FromString;

	/// <summary>
	/// The sample type.
	/// </summary>
	[Serializable]
	public sealed class SampleValidatedType : TypedString<SampleValidatedType>
	{

	  /// <summary>
	  /// Validation of name. </summary>
	  private static readonly Pattern PATTERN = Pattern.compile("[A-Z]+");
	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// </summary>
	  /// <param name="name">  the name to lookup, not null </param>
	  /// <returns> the type matching the name, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static SampleValidatedType of(String name)
	  public static SampleValidatedType of(string name)
	  {
		return new SampleValidatedType(name);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name, not null </param>
	  private SampleValidatedType(string name) : base(name, PATTERN, "Name must be letters")
	  {
	  }

	}

}