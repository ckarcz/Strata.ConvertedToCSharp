using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.BLACK_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using PenaltyMatrixGenerator = com.opengamma.strata.math.impl.interpolation.PenaltyMatrixGenerator;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;

	/// <summary>
	/// Definition of caplet volatilities calibration.
	/// <para>
	/// This definition is used with <seealso cref="DirectIborCapletFloorletVolatilityCalibrator"/>. 
	/// The volatilities of the constituent caplets in the market caps are "model parameters" 
	/// and calibrated to the market data under a certain penalty constraint.
	/// The resulting volatilities object will be a set of caplet volatilities interpolated by <seealso cref="GridSurfaceInterpolator"/>.
	/// </para>
	/// <para>
	/// The penalty defined in this class is based on the finite difference approximation of the second order derivatives 
	/// along time and strike directions. See <seealso cref="PenaltyMatrixGenerator"/> for detail.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class DirectIborCapletFloorletVolatilityDefinition implements IborCapletFloorletVolatilityDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DirectIborCapletFloorletVolatilityDefinition : IborCapletFloorletVolatilityDefinition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborCapletFloorletVolatilitiesName name;
		private readonly IborCapletFloorletVolatilitiesName name;
	  /// <summary>
	  /// The Ibor index for which the data is valid.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
	  private readonly IborIndex index;
	  /// <summary>
	  /// The day count to measure the time in the expiry dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// Penalty intensity parameter for expiry dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double lambdaExpiry;
	  private readonly double lambdaExpiry;
	  /// <summary>
	  /// Penalty intensity parameter for strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double lambdaStrike;
	  private readonly double lambdaStrike;
	  /// <summary>
	  /// The interpolator for the caplet volatilities.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator interpolator;
	  private readonly GridSurfaceInterpolator interpolator;
	  /// <summary>
	  /// The shift parameter of shifted Black model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// The market volatilities are calibrated to shifted Black model if this field is not null.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.market.curve.Curve shiftCurve;
	  private readonly Curve shiftCurve;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with zero shift.
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="lambdaExpiry">  the penalty intensity parameter for time dimension </param>
	  /// <param name="lambdaStrike">  the penalty intensity parameter for strike dimension </param>
	  /// <param name="interpolator">  the surface interpolator </param>
	  /// <returns> the instance </returns>
	  public static DirectIborCapletFloorletVolatilityDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double lambdaExpiry, double lambdaStrike, GridSurfaceInterpolator interpolator)
	  {

		return new DirectIborCapletFloorletVolatilityDefinition(name, index, dayCount, lambdaExpiry, lambdaStrike, interpolator, null);
	  }

	  /// <summary>
	  /// Obtains an instance with shift curve.
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="lambdaExpiry">  the penalty intensity parameter for time dimension </param>
	  /// <param name="lambdaStrike">  the penalty intensity parameter for strike dimension </param>
	  /// <param name="interpolator">  the surface interpolator </param>
	  /// <param name="shiftCurve">  the shift surface </param>
	  /// <returns> the instance </returns>
	  public static DirectIborCapletFloorletVolatilityDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double lambdaExpiry, double lambdaStrike, GridSurfaceInterpolator interpolator, Curve shiftCurve)
	  {

		return new DirectIborCapletFloorletVolatilityDefinition(name, index, dayCount, lambdaExpiry, lambdaStrike, interpolator, shiftCurve);
	  }

	  //-------------------------------------------------------------------------
	  public SurfaceMetadata createMetadata(RawOptionData capFloorData)
	  {
		SurfaceMetadata metadata;
		if (capFloorData.DataType.Equals(BLACK_VOLATILITY))
		{
		  metadata = Surfaces.blackVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else if (capFloorData.DataType.Equals(NORMAL_VOLATILITY))
		{
		  metadata = Surfaces.normalVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else
		{
		  throw new System.ArgumentException("Data type not supported");
		}
		return metadata;
	  }

	  /// <summary>
	  /// Computes penalty matrix. 
	  /// <para>
	  /// The penalty matrix is based on the second order finite difference differentiation in <seealso cref="PenaltyMatrixGenerator"/>.
	  /// The number of node points in each direction must be greater than 2 in order to compute the second order derivative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="strikes">  the strikes </param>
	  /// <param name="expiries">  the expiries </param>
	  /// <returns> the penalty matrix </returns>
	  public DoubleMatrix computePenaltyMatrix(DoubleArray strikes, DoubleArray expiries)
	  {
		ArgChecker.isTrue(strikes.size() > 2, "Need at least 3 points for a curvature estimate");
		ArgChecker.isTrue(expiries.size() > 2, "Need at least 3 points for a curvature estimate");
		return PenaltyMatrixGenerator.getPenaltyMatrix(new double[][] {expiries.toArray(), strikes.toArray()}, new int[] {2, 2}, new double[] {lambdaExpiry, lambdaStrike});
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DirectIborCapletFloorletVolatilityDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static DirectIborCapletFloorletVolatilityDefinition.Meta meta()
	  {
		return DirectIborCapletFloorletVolatilityDefinition.Meta.INSTANCE;
	  }

	  static DirectIborCapletFloorletVolatilityDefinition()
	  {
		MetaBean.register(DirectIborCapletFloorletVolatilityDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static DirectIborCapletFloorletVolatilityDefinition.Builder builder()
	  {
		return new DirectIborCapletFloorletVolatilityDefinition.Builder();
	  }

	  private DirectIborCapletFloorletVolatilityDefinition(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double lambdaExpiry, double lambdaStrike, GridSurfaceInterpolator interpolator, Curve shiftCurve)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		ArgChecker.notNegative(lambdaExpiry, "lambdaExpiry");
		ArgChecker.notNegative(lambdaStrike, "lambdaStrike");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		this.name = name;
		this.index = index;
		this.dayCount = dayCount;
		this.lambdaExpiry = lambdaExpiry;
		this.lambdaStrike = lambdaStrike;
		this.interpolator = interpolator;
		this.shiftCurve = shiftCurve;
	  }

	  public override DirectIborCapletFloorletVolatilityDefinition.Meta metaBean()
	  {
		return DirectIborCapletFloorletVolatilityDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborCapletFloorletVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index for which the data is valid. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count to measure the time in the expiry dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets penalty intensity parameter for expiry dimension. </summary>
	  /// <returns> the value of the property </returns>
	  public double LambdaExpiry
	  {
		  get
		  {
			return lambdaExpiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets penalty intensity parameter for strike dimension. </summary>
	  /// <returns> the value of the property </returns>
	  public double LambdaStrike
	  {
		  get
		  {
			return lambdaStrike;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator for the caplet volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public GridSurfaceInterpolator Interpolator
	  {
		  get
		  {
			return interpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift parameter of shifted Black model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// The market volatilities are calibrated to shifted Black model if this field is not null.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<Curve> ShiftCurve
	  {
		  get
		  {
			return Optional.ofNullable(shiftCurve);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  DirectIborCapletFloorletVolatilityDefinition other = (DirectIborCapletFloorletVolatilityDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(lambdaExpiry, other.lambdaExpiry) && JodaBeanUtils.equal(lambdaStrike, other.lambdaStrike) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(shiftCurve, other.shiftCurve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lambdaExpiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lambdaStrike);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftCurve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("DirectIborCapletFloorletVolatilityDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("lambdaExpiry").Append('=').Append(lambdaExpiry).Append(',').Append(' ');
		buf.Append("lambdaStrike").Append('=').Append(lambdaStrike).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code DirectIborCapletFloorletVolatilityDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(DirectIborCapletFloorletVolatilityDefinition), typeof(IborCapletFloorletVolatilitiesName));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(DirectIborCapletFloorletVolatilityDefinition), typeof(IborIndex));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(DirectIborCapletFloorletVolatilityDefinition), typeof(DayCount));
			  lambdaExpiry_Renamed = DirectMetaProperty.ofImmutable(this, "lambdaExpiry", typeof(DirectIborCapletFloorletVolatilityDefinition), Double.TYPE);
			  lambdaStrike_Renamed = DirectMetaProperty.ofImmutable(this, "lambdaStrike", typeof(DirectIborCapletFloorletVolatilityDefinition), Double.TYPE);
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(DirectIborCapletFloorletVolatilityDefinition), typeof(GridSurfaceInterpolator));
			  shiftCurve_Renamed = DirectMetaProperty.ofImmutable(this, "shiftCurve", typeof(DirectIborCapletFloorletVolatilityDefinition), typeof(Curve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "lambdaExpiry", "lambdaStrike", "interpolator", "shiftCurve");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborCapletFloorletVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code lambdaExpiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> lambdaExpiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code lambdaStrike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> lambdaStrike_Renamed;
		/// <summary>
		/// The meta-property for the {@code interpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<GridSurfaceInterpolator> interpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code shiftCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> shiftCurve_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "lambdaExpiry", "lambdaStrike", "interpolator", "shiftCurve");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1966011430: // lambdaExpiry
			  return lambdaExpiry_Renamed;
			case -1568838055: // lambdaStrike
			  return lambdaStrike_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override DirectIborCapletFloorletVolatilityDefinition.Builder builder()
		{
		  return new DirectIborCapletFloorletVolatilityDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(DirectIborCapletFloorletVolatilityDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborCapletFloorletVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lambdaExpiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> lambdaExpiry()
		{
		  return lambdaExpiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lambdaStrike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> lambdaStrike()
		{
		  return lambdaStrike_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code interpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<GridSurfaceInterpolator> interpolator()
		{
		  return interpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shiftCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> shiftCurve()
		{
		  return shiftCurve_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).Name;
			case 100346066: // index
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).Index;
			case 1905311443: // dayCount
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).DayCount;
			case -1966011430: // lambdaExpiry
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).LambdaExpiry;
			case -1568838055: // lambdaStrike
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).LambdaStrike;
			case 2096253127: // interpolator
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).Interpolator;
			case 1908090253: // shiftCurve
			  return ((DirectIborCapletFloorletVolatilityDefinition) bean).shiftCurve;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code DirectIborCapletFloorletVolatilityDefinition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<DirectIborCapletFloorletVolatilityDefinition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborCapletFloorletVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double lambdaExpiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double lambdaStrike_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal GridSurfaceInterpolator interpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Curve shiftCurve_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(DirectIborCapletFloorletVolatilityDefinition beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.index_Renamed = beanToCopy.Index;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.lambdaExpiry_Renamed = beanToCopy.LambdaExpiry;
		  this.lambdaStrike_Renamed = beanToCopy.LambdaStrike;
		  this.interpolator_Renamed = beanToCopy.Interpolator;
		  this.shiftCurve_Renamed = beanToCopy.shiftCurve;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -1966011430: // lambdaExpiry
			  return lambdaExpiry_Renamed;
			case -1568838055: // lambdaStrike
			  return lambdaStrike_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (IborCapletFloorletVolatilitiesName) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case -1966011430: // lambdaExpiry
			  this.lambdaExpiry_Renamed = (double?) newValue.Value;
			  break;
			case -1568838055: // lambdaStrike
			  this.lambdaStrike_Renamed = (double?) newValue.Value;
			  break;
			case 2096253127: // interpolator
			  this.interpolator_Renamed = (GridSurfaceInterpolator) newValue;
			  break;
			case 1908090253: // shiftCurve
			  this.shiftCurve_Renamed = (Curve) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override DirectIborCapletFloorletVolatilityDefinition build()
		{
		  return new DirectIborCapletFloorletVolatilityDefinition(name_Renamed, index_Renamed, dayCount_Renamed, lambdaExpiry_Renamed, lambdaStrike_Renamed, interpolator_Renamed, shiftCurve_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the volatilities. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(IborCapletFloorletVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the Ibor index for which the data is valid. </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the day count to measure the time in the expiry dimension. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets penalty intensity parameter for expiry dimension. </summary>
		/// <param name="lambdaExpiry">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lambdaExpiry(double lambdaExpiry)
		{
		  ArgChecker.notNegative(lambdaExpiry, "lambdaExpiry");
		  this.lambdaExpiry_Renamed = lambdaExpiry;
		  return this;
		}

		/// <summary>
		/// Sets penalty intensity parameter for strike dimension. </summary>
		/// <param name="lambdaStrike">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lambdaStrike(double lambdaStrike)
		{
		  ArgChecker.notNegative(lambdaStrike, "lambdaStrike");
		  this.lambdaStrike_Renamed = lambdaStrike;
		  return this;
		}

		/// <summary>
		/// Sets the interpolator for the caplet volatilities. </summary>
		/// <param name="interpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder interpolator(GridSurfaceInterpolator interpolator)
		{
		  JodaBeanUtils.notNull(interpolator, "interpolator");
		  this.interpolator_Renamed = interpolator;
		  return this;
		}

		/// <summary>
		/// Sets the shift parameter of shifted Black model.
		/// <para>
		/// The x value of the curve is the expiry.
		/// The market volatilities are calibrated to shifted Black model if this field is not null.
		/// </para>
		/// </summary>
		/// <param name="shiftCurve">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder shiftCurve(Curve shiftCurve)
		{
		  this.shiftCurve_Renamed = shiftCurve;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("DirectIborCapletFloorletVolatilityDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("lambdaExpiry").Append('=').Append(JodaBeanUtils.ToString(lambdaExpiry_Renamed)).Append(',').Append(' ');
		  buf.Append("lambdaStrike").Append('=').Append(JodaBeanUtils.ToString(lambdaStrike_Renamed)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}