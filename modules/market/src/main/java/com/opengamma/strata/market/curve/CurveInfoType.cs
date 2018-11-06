using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using FromString = org.joda.convert.FromString;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using TypedString = com.opengamma.strata.collect.TypedString;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// The type that provides meaning to additional curve information.
	/// <para>
	/// Additional curve information is stored in <seealso cref="CurveMetadata"/>.
	/// It provides the ability to associate arbitrary information with a curve in a key-value map.
	/// For example, it might be used to provide information about one of the axes.
	/// </para>
	/// <para>
	/// Applications that wish to use curve information should declare a static
	/// constant declaring the {@code CurveInfoType} instance, the type parameter
	/// and an UpperCamelCase name. For example:
	/// <pre>
	///  public static final CurveInfoType&lt;String&gt; OWNER = CurveInfoType.of("Owner");
	/// </pre>
	/// 
	/// </para>
	/// </summary>
	/// @param <T>  the type of the associated value </param>
	[Serializable]
	public sealed class CurveInfoType<T> : TypedString<CurveInfoType<T>>
	{

	  /// <summary>
	  /// Key used to access information about the <seealso cref="DayCount"/>.
	  /// </summary>
	  public static readonly CurveInfoType<DayCount> DAY_COUNT = CurveInfoType.of("DayCount");
	  /// <summary>
	  /// Key used to access information about the <seealso cref="JacobianCalibrationMatrix"/>.
	  /// </summary>
	  public static readonly CurveInfoType<JacobianCalibrationMatrix> JACOBIAN = CurveInfoType.of("Jacobian");
	  /// <summary>
	  /// Key used to access information about the number of compounding per year, as an <seealso cref="Integer"/>.
	  /// </summary>
	  public static readonly CurveInfoType<int> COMPOUNDING_PER_YEAR = CurveInfoType.of("CompoundingPerYear");
	  /// <summary>
	  /// Key used to access information about the present value sensitivity to market quote, 
	  /// represented by a <seealso cref="DoubleArray"/>.
	  /// </summary>
	  public static readonly CurveInfoType<DoubleArray> PV_SENSITIVITY_TO_MARKET_QUOTE = CurveInfoType.of("PVSensitivityToMarketQuote");
	  /// <summary>
	  /// Key used to access information about the index factor.
	  /// </summary>
	  public static readonly CurveInfoType<double> CDS_INDEX_FACTOR = CurveInfoType.of("CdsIndexFactor");

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
//ORIGINAL LINE: @FromString public static <T> CurveInfoType<T> of(String name)
	  public static CurveInfoType<T> of<T>(string name)
	  {
		return new CurveInfoType<T>(name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name </param>
	  private CurveInfoType(string name) : base(name)
	  {
	  }

	}

}