using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.etd
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The contract code for an Exchange Traded Derivative (ETD).
	/// <para>
	/// This is the code supplied by the exchange for use in clearing and margining, such as in SPAN.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class EtdContractCode : TypedString<EtdContractCode>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// The name may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> a type instance with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static EtdContractCode of(String name)
	  public static EtdContractCode of(string name)
	  {
		return new EtdContractCode(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private EtdContractCode(string name) : base(name)
	  {
	  }

	}

}