/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Country = com.opengamma.strata.basics.location.Country;

	/// <summary>
	/// A legal entity.
	/// <para>
	/// A legal entity is one of the building blocks of finance, representing an organization.
	/// It is used to capture details for credit worthiness.
	/// The legal entity can be looked up in <seealso cref="ReferenceData"/> using the identifier.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable beans.
	/// 
	/// </para>
	/// </summary>
	/// <seealso cref= SimpleLegalEntity </seealso>
	public interface LegalEntity
	{

	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier uniquely identifies the legal entity within the system.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the legal entity identifier </returns>
	  LegalEntityId LegalEntityId {get;}

	  /// <summary>
	  /// Gets the name of the legal entity.
	  /// <para>
	  /// This is intended for humans.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the name </returns>
	  string Name {get;}

	  /// <summary>
	  /// Gets the country that the legal entity is based in.
	  /// </summary>
	  /// <returns> the country </returns>
	  Country Country {get;}

	}

}