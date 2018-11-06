using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{

	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;

	/// <summary>
	/// The name of a surface.
	/// </summary>
	[Serializable]
	public sealed class SurfaceName : MarketDataName<Surface>
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
	  /// Surface names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the surface </param>
	  /// <returns> a surface with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static SurfaceName of(String name)
	  public static SurfaceName of(string name)
	  {
		return new SurfaceName(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the surface </param>
	  private SurfaceName(string name)
	  {
		this.name = ArgChecker.notEmpty(name, "name");
	  }

	  //-------------------------------------------------------------------------
	  public override Type<Surface> MarketDataType
	  {
		  get
		  {
			return typeof(Surface);
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