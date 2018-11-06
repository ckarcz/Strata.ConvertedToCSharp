using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// The name of a set of Ibor future option volatilities.
	/// </summary>
	[Serializable]
	public sealed class IborFutureOptionVolatilitiesName : MarketDataName<IborFutureOptionVolatilities>
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
//ORIGINAL LINE: @FromString public static IborFutureOptionVolatilitiesName of(String name)
	  public static IborFutureOptionVolatilitiesName of(string name)
	  {
		return new IborFutureOptionVolatilitiesName(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private IborFutureOptionVolatilitiesName(string name)
	  {
		this.name = ArgChecker.notEmpty(name, "name");
	  }

	  //-------------------------------------------------------------------------
	  public override Type<IborFutureOptionVolatilities> MarketDataType
	  {
		  get
		  {
			return typeof(IborFutureOptionVolatilities);
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