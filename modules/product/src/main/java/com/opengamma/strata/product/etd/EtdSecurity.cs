/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// An instrument representing an exchange traded derivative (ETD).
	/// </summary>
	public interface EtdSecurity : Security, SecuritizedProduct
	{

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.product.SecurityId getSecurityId()
	//  {
	//	return Security.this.getSecurityId();
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.basics.currency.Currency getCurrency()
	//  {
	//	return Security.this.getCurrency();
	//  }

//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.google.common.collect.ImmutableSet<com.opengamma.strata.product.SecurityId> getUnderlyingIds()
	//  {
	//	return ImmutableSet.of();
	//  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the ID of the contract specification from which this security is derived.
	  /// </summary>
	  /// <returns> the ID </returns>
	  EtdContractSpecId ContractSpecId {get;}

	  /// <summary>
	  /// Gets the type of the contract - future or option.
	  /// </summary>
	  /// <returns> the type, future or option </returns>
	  EtdType Type {get;}

	  /// <summary>
	  /// Gets the year-month of the expiry.
	  /// <para>
	  /// Expiry will occur on a date implied by the variant of the ETD.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the year-month </returns>
	  YearMonth Expiry {get;}

	  /// <summary>
	  /// Gets the variant of ETD.
	  /// <para>
	  /// This captures the variant of the ETD. The most common variant is 'Monthly'.
	  /// Other variants are 'Weekly', 'Daily' and 'Flex'.
	  /// </para>
	  /// <para>
	  /// When building, this defaults to 'Monthly'.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the variant </returns>
	  EtdVariant Variant {get;}

	  //-------------------------------------------------------------------------
	  EtdSecurity withInfo(SecurityInfo info);

	}

}