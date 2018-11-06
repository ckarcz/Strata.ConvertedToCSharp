/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{

	/// <summary>
	/// An instrument representing a security associated with a legal entity. 
	/// <para>
	/// Examples include fixed coupon bonds and capital index bonds. 
	/// </para>
	/// </summary>
	public interface LegalEntitySecurity : Security
	{

	  /// <summary>
	  /// Get the legal entity identifier.
	  /// <para>
	  /// The identifier is used for the legal entity that issues the security.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the legal entity identifier </returns>
	  LegalEntityId LegalEntityId {get;}

	  //-------------------------------------------------------------------------
	  LegalEntitySecurity withInfo(SecurityInfo info);

	}

}