using System;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.common
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// An identifier for an exchange based on the ISO Market Identifier Code (MIC).
	/// <para>
	/// Identifiers for common exchanges are provided in <seealso cref="ExchangeIds"/>.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class ExchangeId : Named
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The Market Identifier Code (MIC) identifying the exchange.
	  /// </summary>
	  private readonly string name;

	  private ExchangeId(string name)
	  {
		this.name = ArgChecker.notBlank(name, "name");
	  }

	  /// <summary>
	  /// Returns the Market Identifier Code (MIC) identifying the exchange.
	  /// </summary>
	  /// <returns> the Market Identifier Code (MIC) identifying the exchange </returns>
	  public string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  /// <summary>
	  /// Returns an identifier for an exchange.
	  /// </summary>
	  /// <param name="name"> the Market Identifier Code (MIC) identifying the exchange </param>
	  /// <returns> an identifier for an exchange </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ExchangeId of(String name)
	  public static ExchangeId of(string name)
	  {
		return new ExchangeId(name);
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
		if (this == obj)
		{
		  return true;
		}
		if (obj == null || this.GetType() != obj.GetType())
		{
		  return false;
		}
		ExchangeId that = (ExchangeId) obj;
		return name.Equals(that.name);
	  }

	  /// <summary>
	  /// Returns a suitable hash code for the identifier.
	  /// </summary>
	  /// <returns> the hash code </returns>
	  public override int GetHashCode()
	  {
		return name.GetHashCode();
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return name;
	  }

	}

}