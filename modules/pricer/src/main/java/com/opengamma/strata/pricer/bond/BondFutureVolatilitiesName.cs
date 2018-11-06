using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.bond
{

	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// The name of a set of bond future volatilities.
	/// </summary>
	[Serializable]
	public sealed class BondFutureVolatilitiesName : MarketDataName<BondFutureVolatilities>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The name.
	  /// </summary>
	  private readonly string name;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name </param>
	  /// <returns> the name instance </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static BondFutureVolatilitiesName of(String name)
	  public static BondFutureVolatilitiesName of(string name)
	  {
		return new BondFutureVolatilitiesName(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private BondFutureVolatilitiesName(string name)
	  {
		this.name = ArgChecker.notEmpty(name, "name");
	  }

	  //-------------------------------------------------------------------------
	  public override Type<BondFutureVolatilities> MarketDataType
	  {
		  get
		  {
			return typeof(BondFutureVolatilities);
		  }
	  }

	  public override string Name
	  {
		  get
		  {
			return name;
		  }
	  }

	}

}