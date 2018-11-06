using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
	using FromString = org.joda.convert.FromString;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using TypedString = com.opengamma.strata.collect.TypedString;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;

	/// <summary>
	/// The type that provides meaning to additional surface information.
	/// <para>
	/// Additional surface information is stored in <seealso cref="SurfaceMetadata"/>.
	/// It provides the ability to associate arbitrary information with a surface in a key-value map.
	/// For example, it might be used to provide information about one of the axes.
	/// </para>
	/// <para>
	/// Applications that wish to use surface information should declare a static
	/// constant declaring the {@code SurfaceInfoType} instance, the type parameter
	/// and an UpperCamelCase name. For example:
	/// <pre>
	///  public static final SurfaceInfoType&lt;String&gt; OWNER = SurfaceInfoType.of("Owner");
	/// </pre>
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the associated value </param>
	[Serializable]
	public sealed class SurfaceInfoType<T> : TypedString<SurfaceInfoType<T>>
	{

	  /// <summary>
	  /// Key used to access information about the <seealso cref="DayCount"/>.
	  /// </summary>
	  public static readonly SurfaceInfoType<DayCount> DAY_COUNT = SurfaceInfoType.of("DayCount");
	  /// <summary>
	  /// Key used to access information about the type of moneyness.
	  /// </summary>
	  public static readonly SurfaceInfoType<MoneynessType> MONEYNESS_TYPE = SurfaceInfoType.of("MoneynessType");

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
	  /// @param <T>  the type associated with the info </param>
	  /// <param name="name">  the name </param>
	  /// <returns> a type instance with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static <T> SurfaceInfoType<T> of(String name)
	  public static SurfaceInfoType<T> of<T>(string name)
	  {
		return new SurfaceInfoType<T>(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private SurfaceInfoType(string name) : base(name)
	  {
	  }

	}

}