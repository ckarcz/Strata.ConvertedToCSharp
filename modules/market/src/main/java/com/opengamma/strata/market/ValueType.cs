using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market
{
	using FromString = org.joda.convert.FromString;

	using CharMatcher = com.google.common.@base.CharMatcher;
	using Messages = com.opengamma.strata.collect.Messages;
	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The type of a value.
	/// <para>
	/// The market data system contains many different kinds of value, and this type can be used to identify them.
	/// </para>
	/// <para>
	/// For example, constants are provided for common financial concepts, such as discount factors,
	/// zero rates and year fractions. The set of types is fully extensible.
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class ValueType : TypedString<ValueType>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;
	  /// <summary>
	  /// Matcher for checking the name.
	  /// It must only contains the characters A-Z, a-z, 0-9 and -.
	  /// </summary>
	  private static readonly CharMatcher NAME_MATCHER = CharMatcher.inRange('A', 'Z').or(CharMatcher.inRange('a', 'z')).or(CharMatcher.inRange('0', '9')).or(CharMatcher.@is('-')).precomputed();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Type used when the meaning of each value is not known - 'Unknown'.
	  /// </summary>
	  public static readonly ValueType UNKNOWN = of("Unknown");

	  /// <summary>
	  /// Type used when each value is a year fraction relative to a base date - 'YearFraction'.
	  /// </summary>
	  public static readonly ValueType YEAR_FRACTION = of("YearFraction");
	  /// <summary>
	  /// Type used when each value is the number of months relative to a base month - 'Months'.
	  /// </summary>
	  public static readonly ValueType MONTHS = of("Months");
	  /// <summary>
	  /// Type used when each value is a zero rate - 'ZeroRate'.
	  /// </summary>
	  public static readonly ValueType ZERO_RATE = of("ZeroRate");
	  /// <summary>
	  /// Type used when each value is a forward rate - 'ForwardRate'.
	  /// </summary>
	  public static readonly ValueType FORWARD_RATE = of("ForwardRate");
	  /// <summary>
	  /// Type used when each value is a discount factor - 'DiscountFactor'.
	  /// </summary>
	  public static readonly ValueType DISCOUNT_FACTOR = of("DiscountFactor");
	  /// <summary>
	  /// Type used when each value is a price index, as used for inflation products - 'PriceIndex'.
	  /// </summary>
	  public static readonly ValueType PRICE_INDEX = of("PriceIndex");
	  /// <summary>
	  /// Type used when each value is a recovery rate - 'RecoveryRate'.
	  /// </summary>
	  public static readonly ValueType RECOVERY_RATE = of("RecoveryRate");

	  /// <summary>
	  /// Type used when each value is a Black model implied volatility - 'BlackVolatility'.
	  /// </summary>
	  public static readonly ValueType BLACK_VOLATILITY = of("BlackVolatility");
	  /// <summary>
	  /// Type used when each value is a Normal (Bachelier) model implied volatility - 'NormalVolatility'.
	  /// </summary>
	  public static readonly ValueType NORMAL_VOLATILITY = of("NormalVolatility");
	  /// <summary>
	  /// Type used when each value is a local volatility - 'LocalVolatility'.
	  /// </summary>
	  public static readonly ValueType LOCAL_VOLATILITY = of("LocalVolatility");
	  /// <summary>
	  /// Type used when each value is a Price - 'Price'.
	  /// </summary>
	  public static readonly ValueType PRICE = of("Price");
	  /// <summary>
	  /// Type used when each value is a strike - 'Strike'.
	  /// </summary>
	  public static readonly ValueType STRIKE = of("Strike");
	  /// <summary>
	  /// Type used when each value is simple-moneyness, i.e. the value refers to strike minus forward - 'SimpleMoneyness'.
	  /// </summary>
	  public static readonly ValueType SIMPLE_MONEYNESS = of("SimpleMoneyness");
	  /// <summary>
	  /// Type used when each value is log-moneyness, i.e. the value refers to log of strike divided by forward - 'LogMoneyness'.
	  /// </summary>
	  public static readonly ValueType LOG_MONEYNESS = of("LogMoneyness");
	  /// <summary>
	  /// Type used when each value is the SABR alpha parameter - 'SabrAlpha'.
	  /// </summary>
	  public static readonly ValueType SABR_ALPHA = of("SabrAlpha");
	  /// <summary>
	  /// Type used when each value is the SABR beta parameter - 'SabrBeta'.
	  /// </summary>
	  public static readonly ValueType SABR_BETA = of("SabrBeta");
	  /// <summary>
	  /// Type used when each value is the SABR rho parameter - 'SabrRho'.
	  /// </summary>
	  public static readonly ValueType SABR_RHO = of("SabrRho");
	  /// <summary>
	  /// Type used when each value is the SABR nu parameter - 'SabrNu'.
	  /// </summary>
	  public static readonly ValueType SABR_NU = of("SabrNu");
	  /// <summary>
	  /// Type used when each value is a risk reversal - 'RiskReversal'.
	  /// </summary>
	  public static readonly ValueType RISK_REVERSAL = of("RiskReversal");
	  /// <summary>
	  /// Type used when each value is a strangle - 'Strangle'.
	  /// </summary>
	  public static readonly ValueType STRANGLE = of("Strangle");

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Value types must only contains the characters A-Z, a-z, 0-9 and -.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  /// <returns> a field with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ValueType of(String name)
	  public static ValueType of(string name)
	  {
		return new ValueType(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the field </param>
	  private ValueType(string name) : base(name, NAME_MATCHER, "Value type must only contain the characters A-Z, a-z, 0-9 and -")
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks that this instance equals the specified instance.
	  /// <para>
	  /// This returns normally if they are equal.
	  /// Otherwise, an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the instance to check against </param>
	  /// <param name="exceptionPrefix">  the exception prefix </param>
	  public void checkEquals(ValueType other, string exceptionPrefix)
	  {
		if (!this.Equals(other))
		{
		  throw new System.ArgumentException(Messages.format("{}, expected {} but was {}", exceptionPrefix, other, this));
		}
	  }

	}

}