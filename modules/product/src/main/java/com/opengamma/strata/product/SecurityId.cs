using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataId = com.opengamma.strata.basics.ReferenceDataId;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An identifier for a security.
	/// <para>
	/// This identifier is used to obtain a <seealso cref="Security"/> from <seealso cref="ReferenceData"/>.
	/// </para>
	/// <para>
	/// A security identifier uniquely identifies a security within the system.
	/// A real-world security will typically have multiple identifiers.
	/// The only restriction placed on the identifier is that it is sufficiently
	/// unique for the reference data lookup. As such, it is acceptable to use
	/// an identifier from a well-known global or vendor symbology.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class SecurityId : ReferenceDataId<Security>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The identifier, expressed as a standard two-part identifier.
	  /// </summary>
	  private readonly StandardId standardId;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a scheme and value.
	  /// <para>
	  /// The scheme and value are used to produce a <seealso cref="StandardId"/>, where more
	  /// information is available on how schemes and values relate to industry identifiers.
	  /// </para>
	  /// <para>
	  /// The scheme must be non-empty and match the regular expression '{@code [A-Za-z0-9:/+.=_-]*}'.
	  /// This permits letters, numbers, colon, forward-slash, plus, dot, equals, underscore and dash.
	  /// If necessary, the scheme can be encoded using <seealso cref="StandardId#encodeScheme(String)"/>.
	  /// </para>
	  /// <para>
	  /// The value must be non-empty and match the regular expression '{@code [!-z][ -z]*}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="scheme">  the scheme of the identifier, not empty </param>
	  /// <param name="value">  the value of the identifier, not empty </param>
	  /// <returns> the security identifier </returns>
	  /// <exception cref="IllegalArgumentException"> if the scheme or value is invalid </exception>
	  public static SecurityId of(string scheme, string value)
	  {
		return of(StandardId.of(scheme, value));
	  }

	  /// <summary>
	  /// Creates an instance from a standard two-part identifier.
	  /// </summary>
	  /// <param name="standardId">  the underlying standard two-part identifier </param>
	  /// <returns> the security identifier </returns>
	  public static SecurityId of(StandardId standardId)
	  {
		return new SecurityId(standardId);
	  }

	  /// <summary>
	  /// Parses an {@code StandardId} from a formatted scheme and value.
	  /// <para>
	  /// This parses the identifier from the form produced by {@code toString()}
	  /// which is '{@code $scheme~$value}'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="str">  the identifier to parse </param>
	  /// <returns> the security identifier </returns>
	  /// <exception cref="IllegalArgumentException"> if the identifier cannot be parsed </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static SecurityId parse(String str)
	  public static SecurityId parse(string str)
	  {
		return new SecurityId(StandardId.parse(str));
	  }

	  // creates an identifier
	  private SecurityId(StandardId standardId)
	  {
		this.standardId = ArgChecker.notNull(standardId, "standardId");
	  }

	  // resolve after deserialization
	  private object readResolve()
	  {
		return of(standardId);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the standard two-part identifier.
	  /// </summary>
	  /// <returns> the standard two-part identifier </returns>
	  public StandardId StandardId
	  {
		  get
		  {
			return standardId;
		  }
	  }

	  /// <summary>
	  /// Gets the type of data this identifier refers to.
	  /// <para>
	  /// A {@code SecurityId} refers to a {@code Security}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the type of the reference data this identifier refers to </returns>
	  public Type<Security> ReferenceDataType
	  {
		  get
		  {
			return typeof(Security);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if this identifier equals another identifier.
	  /// <para>
	  /// The comparison checks the name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="obj">  the other identifier, null returns false </param>
	  /// <returns> true if equal </returns>
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj is SecurityId)
		{
		  return standardId.Equals(((SecurityId) obj).standardId);
		}
		return false;
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the identifier.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return standardId.GetHashCode() + 7;
	  }

	  /// <summary>
	  /// Returns the identifier in a standard string format.
	  /// <para>
	  /// The returned string is in the form '{@code $scheme~$value}'.
	  /// This is suitable for use with <seealso cref="#parse(String)"/>.
	  /// For example, if the scheme is 'OG-Future' and the value is 'Eurex-FGBL-Mar14'
	  /// then the result is 'OG-Future~Eurex-FGBL-Mar14'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> a parsable representation of the identifier </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return standardId.ToString();
	  }

	}

}