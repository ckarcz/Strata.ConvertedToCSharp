using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Builder for surface metadata.
	/// <para>
	/// This is created using <seealso cref="DefaultSurfaceMetadata#builder()"/>.
	/// </para>
	/// </summary>
	public sealed class DefaultSurfaceMetadataBuilder
	{

	  /// <summary>
	  /// The surface name.
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private SurfaceName surfaceName_Renamed;
	  /// <summary>
	  /// The x-value type, providing meaning to the x-values of the surface.
	  /// <para>
	  /// This type provides meaning to the x-values.
	  /// It defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ValueType xValueType_Renamed = ValueType.UNKNOWN;
	  /// <summary>
	  /// The y-value type, providing meaning to the y-values of the surface.
	  /// <para>
	  /// This type provides meaning to the y-values.
	  /// It defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ValueType yValueType_Renamed = ValueType.UNKNOWN;
	  /// <summary>
	  /// The y-value type, providing meaning to the z-values of the surface.
	  /// <para>
	  /// This type provides meaning to the z-values.
	  /// It defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private ValueType zValueType_Renamed = ValueType.UNKNOWN;
	  /// <summary>
	  /// The additional surface information.
	  /// <para>
	  /// This stores additional information for the surface.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<SurfaceInfoType<?>, Object> info = new java.util.HashMap<>();
	  private readonly IDictionary<SurfaceInfoType<object>, object> info = new Dictionary<SurfaceInfoType<object>, object>();
	  /// <summary>
	  /// The metadata about the parameters.
	  /// <para>
	  /// If present, the parameter metadata will match the number of parameters on the surface.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private IList<ParameterMetadata> parameterMetadata_Renamed;

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  internal DefaultSurfaceMetadataBuilder()
	  {
	  }

	  /// <summary>
	  /// Restricted copy constructor.
	  /// </summary>
	  /// <param name="beanToCopy">  the bean to copy from </param>
	  internal DefaultSurfaceMetadataBuilder(DefaultSurfaceMetadata beanToCopy)
	  {
		this.surfaceName_Renamed = beanToCopy.SurfaceName;
		this.xValueType_Renamed = beanToCopy.XValueType;
		this.yValueType_Renamed = beanToCopy.YValueType;
		this.zValueType_Renamed = beanToCopy.ZValueType;
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		this.info.putAll(beanToCopy.Info);
		this.parameterMetadata_Renamed = beanToCopy.ParameterMetadata.orElse(null);
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Sets the surface name.
	  /// </summary>
	  /// <param name="surfaceName">  the surface name </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder surfaceName(string surfaceName)
	  {
		this.surfaceName_Renamed = SurfaceName.of(surfaceName);
		return this;
	  }

	  /// <summary>
	  /// Sets the surface name.
	  /// </summary>
	  /// <param name="surfaceName">  the surface name </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder surfaceName(SurfaceName surfaceName)
	  {
		this.surfaceName_Renamed = ArgChecker.notNull(surfaceName, "surfaceName");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the x-value type, providing meaning to the x-values of the surface.
	  /// <para>
	  /// This type provides meaning to the x-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValueType">  the x-value type </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder xValueType(ValueType xValueType)
	  {
		this.xValueType_Renamed = ArgChecker.notNull(xValueType, "xValueType");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the y-value type, providing meaning to the y-values of the surface.
	  /// <para>
	  /// This type provides meaning to the y-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="yValueType">  the y-value type </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder yValueType(ValueType yValueType)
	  {
		this.yValueType_Renamed = ArgChecker.notNull(yValueType, "yValueType");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the z-value type, providing meaning to the z-values of the surface.
	  /// <para>
	  /// This type provides meaning to the z-values.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="zValueType">  the z-value type </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder zValueType(ValueType zValueType)
	  {
		this.zValueType_Renamed = ArgChecker.notNull(zValueType, "zValueType");
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Sets the day count.
	  /// <para>
	  /// This stores the day count in the additional information map using the
	  /// key <seealso cref="SurfaceInfoType#DAY_COUNT"/>.
	  /// </para>
	  /// <para>
	  /// This is stored in the additional information map using {@code Map.put} semantics,
	  /// removing the key if the day count is null.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="dayCount">  the day count, may be null </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder dayCount(DayCount dayCount)
	  {
		return addInfo(SurfaceInfoType.DAY_COUNT, dayCount);
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
	  public DefaultSurfaceMetadataBuilder addInfo<T>(SurfaceInfoType<T> type, T value)
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
	  /// The parameter metadata must match the number of parameters on the surface.
	  /// This will replace the existing parameter-level metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder parameterMetadata<T1>(IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		this.parameterMetadata_Renamed = ImmutableList.copyOf(parameterMetadata);
		return this;
	  }

	  /// <summary>
	  /// Sets the parameter-level metadata.
	  /// <para>
	  /// The parameter metadata must match the number of parameters on the surface.
	  /// This will replace the existing parameter-level metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> this, for chaining </returns>
	  public DefaultSurfaceMetadataBuilder parameterMetadata(params ParameterMetadata[] parameterMetadata)
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
	  public DefaultSurfaceMetadataBuilder clearParameterMetadata()
	  {
		this.parameterMetadata_Renamed = null;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the metadata instance.
	  /// </summary>
	  /// <returns> the instance </returns>
	  public DefaultSurfaceMetadata build()
	  {
		return new DefaultSurfaceMetadata(surfaceName_Renamed, xValueType_Renamed, yValueType_Renamed, zValueType_Renamed, info, parameterMetadata_Renamed);
	  }

	}

}