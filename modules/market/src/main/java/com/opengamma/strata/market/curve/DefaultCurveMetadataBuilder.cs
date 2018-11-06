using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Builder for curve metadata.
	/// <para>
	/// This is created using <seealso cref="DefaultCurveMetadata#builder()"/>.
	/// </para>
	/// </summary>
	public sealed class DefaultCurveMetadataBuilder
	{

	  /// <summary>
	  /// The curve name.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private CurveName curveName_Renamed;
	  /// <summary>
	  /// The x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// It defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ValueType xValueType_Renamed = ValueType.UNKNOWN;
	  /// <summary>
	  /// The y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// It defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ValueType yValueType_Renamed = ValueType.UNKNOWN;
	  /// <summary>
	  /// The additional curve information.
	  /// <para>
	  /// This stores additional information for the curve.
	  /// </para>
	  /// <para>
	  /// The most common information is the <seealso cref="CurveInfoType#DAY_COUNT day count"/>
	  /// and <seealso cref="CurveInfoType#JACOBIAN curve calibration Jacobian"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<CurveInfoType<?>, Object> info = new java.util.HashMap<>();
	  private readonly IDictionary<CurveInfoType<object>, object> info = new Dictionary<CurveInfoType<object>, object>();
	  /// <summary>
	  /// The metadata about the parameters.
	  /// <para>
	  /// If present, the parameter metadata will match the number of parameters on the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private IList<ParameterMetadata> parameterMetadata_Renamed;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  internal DefaultCurveMetadataBuilder()
	  {
	  }

	  /// <summary>
	  /// Restricted copy constructor.
	  /// </summary>
	  /// <param name="beanToCopy">  the bean to copy from </param>
	  internal DefaultCurveMetadataBuilder(DefaultCurveMetadata beanToCopy)
	  {
		this.curveName_Renamed = beanToCopy.CurveName;
		this.xValueType_Renamed = beanToCopy.XValueType;
		this.yValueType_Renamed = beanToCopy.YValueType;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.info.putAll(beanToCopy.Info);
		this.parameterMetadata_Renamed = beanToCopy.ParameterMetadata.orElse(null);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sets the curve name.
	  /// </summary>
	  /// <param name="curveName">  the curve name </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder curveName(string curveName)
	  {
		this.curveName_Renamed = CurveName.of(curveName);
		return this;
	  }

	  /// <summary>
	  /// Sets the curve name.
	  /// </summary>
	  /// <param name="curveName">  the curve name </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder curveName(CurveName curveName)
	  {
		this.curveName_Renamed = ArgChecker.notNull(curveName, "curveName");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValueType">  the x-value type </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder xValueType(ValueType xValueType)
	  {
		this.xValueType_Renamed = ArgChecker.notNull(xValueType, "xValueType");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yValueType">  the y-value type </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder yValueType(ValueType yValueType)
	  {
		this.yValueType_Renamed = ArgChecker.notNull(yValueType, "yValueType");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the day count.
	  /// <para>
	  /// This stores the day count in the additional information map using the
	  /// key <seealso cref="CurveInfoType#DAY_COUNT"/>.
	  /// </para>
	  /// <para>
	  /// This is stored in the additional information map using {@code Map.put} semantics,
	  /// removing the key if the day count is null.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dayCount">  the day count, may be null </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder dayCount(DayCount dayCount)
	  {
		return addInfo(CurveInfoType.DAY_COUNT, dayCount);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the calibration information.
	  /// <para>
	  /// This stores the calibration information in the additional information map
	  /// using the key <seealso cref="CurveInfoType#JACOBIAN"/>.
	  /// </para>
	  /// <para>
	  /// This is stored in the additional information map using {@code Map.put} semantics,
	  /// removing the key if the jacobian is null.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="jacobian">  the calibration information, may be null </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder jacobian(JacobianCalibrationMatrix jacobian)
	  {
		return addInfo(CurveInfoType.JACOBIAN, jacobian);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a single piece of additional information.
	  /// <para>
	  /// This is stored in the additional information map using {@code Map.put} semantics,
	  /// removing the key if the instance is null.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the info </param>
	  /// <param name="type">  the type to store under </param>
	  /// <param name="value">  the value to store, may be null </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder addInfo<T>(CurveInfoType<T> type, T value)
	  {
		ArgChecker.notNull(type, "type");
		if (value != default(T))
		{
		  this.info[type] = value;
		}
		else
		{
		  this.info.Remove(type);
		}
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the parameter-level metadata.
	  /// <para>
	  /// The parameter metadata must match the number of parameters on the curve.
	  /// This will replace the existing parameter-level metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder parameterMetadata<T1>(IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		this.parameterMetadata_Renamed = ImmutableList.copyOf(parameterMetadata);
		return this;
	  }

	  /// <summary>
	  /// Sets the parameter-level metadata.
	  /// <para>
	  /// The parameter metadata must match the number of parameters on the curve.
	  /// This will replace the existing parameter-level metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder parameterMetadata(params ParameterMetadata[] parameterMetadata)
	  {
		this.parameterMetadata_Renamed = ImmutableList.copyOf(parameterMetadata);
		return this;
	  }

	  /// <summary>
	  /// Clears the parameter-level metadata.
	  /// <para>
	  /// The existing parameter-level metadata will be removed.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> this, for chaining </returns>
	  public DefaultCurveMetadataBuilder clearParameterMetadata()
	  {
		this.parameterMetadata_Renamed = null;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the metadata instance.
	  /// </summary>
	  /// <returns> the instance </returns>
	  public DefaultCurveMetadata build()
	  {
		return new DefaultCurveMetadata(curveName_Renamed, xValueType_Renamed, yValueType_Renamed, info, parameterMetadata_Renamed);
	  }

	}

}