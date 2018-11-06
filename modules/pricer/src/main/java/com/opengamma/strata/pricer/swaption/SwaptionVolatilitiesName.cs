using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.swaption
{

	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// The name of a set of swaption volatilities.
	/// </summary>
	[Serializable]
	public sealed class SwaptionVolatilitiesName : MarketDataName<SwaptionVolatilities>
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
//ORIGINAL LINE: @FromString public static SwaptionVolatilitiesName of(String name)
	  public static SwaptionVolatilitiesName of(string name)
	  {
		return new SwaptionVolatilitiesName(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private SwaptionVolatilitiesName(string name)
	  {
		this.name = ArgChecker.notEmpty(name, "name");
	  }

	  //-------------------------------------------------------------------------
	  public override Type<SwaptionVolatilities> MarketDataType
	  {
		  get
		  {
			return typeof(SwaptionVolatilities);
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