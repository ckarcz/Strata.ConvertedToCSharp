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
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using CurveInterpolators = com.opengamma.strata.market.curve.interpolator.CurveInterpolators;
	using SimpleStrike = com.opengamma.strata.market.option.SimpleStrike;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using GridSurfaceInterpolator = com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator;
	using GenericVolatilitySurfacePeriodParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfacePeriodParameterMetadata;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;

	/// <summary>
	/// Definition of caplet volatilities calibration.
	/// <para>
	/// This definition is used with <seealso cref="SurfaceIborCapletFloorletVolatilityBootstrapper"/>. 
	/// The caplet volatilities are computed by bootstrap along the time direction, 
	/// thus the interpolation and left extrapolation for the time dimension must be local. 
	/// The resulting volatilities object will be a set of caplet volatilities interpolated by <seealso cref="GridSurfaceInterpolator"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SurfaceIborCapletFloorletVolatilityBootstrapDefinition implements IborCapletFloorletVolatilityDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SurfaceIborCapletFloorletVolatilityBootstrapDefinition : IborCapletFloorletVolatilityDefinition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborCapletFloorletVolatilitiesName name;
		private readonly IborCapletFloorletVolatilitiesName name;
	  /// <summary>
	  /// The Ibor index.
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
	  /// The interpolator for the caplet volatilities.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.interpolator.GridSurfaceInterpolator interpolator;
	  private readonly GridSurfaceInterpolator interpolator;
	  /// <summary>
	  /// The shift parameter of shifted Black model.
	  /// <para>
	  /// The market volatilities are calibrated to shifted Black model if this field is not null.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.market.curve.Curve shiftCurve;
	  private readonly Curve shiftCurve;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with gird surface interpolator.
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="interpolator">  the surface interpolator </param>
	  /// <returns> the instance </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, GridSurfaceInterpolator interpolator)
	  {

		return of(name, index, dayCount, interpolator, null);
	  }

	  /// <summary>
	  /// Obtains an instance with gird surface interpolator and shift curve.
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="interpolator">  the surface interpolator </param>
	  /// <param name="shiftCurve">  the shift curve </param>
	  /// <returns> the instance </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, GridSurfaceInterpolator interpolator, Curve shiftCurve)
	  {

		return new SurfaceIborCapletFloorletVolatilityBootstrapDefinition(name, index, dayCount, interpolator, shiftCurve);
	  }

	  /// <summary>
	  /// Obtains an instance with time interpolator and strike interpolator. 
	  /// <para>
	  /// The extrapolation is completed by default extrapolators in {@code GridSurfaceInterpolator}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="timeInterpolator">  the time interpolator </param>
	  /// <param name="strikeInterpolator">  the strike interpolator </param>
	  /// <returns> the instance </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, CurveInterpolator timeInterpolator, CurveInterpolator strikeInterpolator)
	  {

		return of(name, index, dayCount, timeInterpolator, strikeInterpolator, null);
	  }

	  /// <summary>
	  /// Obtains an instance with time interpolator, strike interpolator and shift curve.
	  /// <para>
	  /// The extrapolation is completed by default extrapolators in {@code GridSurfaceInterpolator}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count to use </param>
	  /// <param name="timeInterpolator">  the time interpolator </param>
	  /// <param name="strikeInterpolator">  the strike interpolator </param>
	  /// <param name="shiftCurve">  the shift curve </param>
	  /// <returns> the instance </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapDefinition of(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, CurveInterpolator timeInterpolator, CurveInterpolator strikeInterpolator, Curve shiftCurve)
	  {

		GridSurfaceInterpolator gridInterpolator = GridSurfaceInterpolator.of(timeInterpolator, strikeInterpolator);
		return of(name, index, dayCount, gridInterpolator, shiftCurve);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(interpolator.XExtrapolatorLeft.Equals(FLAT), "x extrapolator left must be flat extrapolator");
		ArgChecker.isTrue(interpolator.XInterpolator.Equals(CurveInterpolators.LINEAR) || interpolator.XInterpolator.Equals(CurveInterpolators.STEP_UPPER) || interpolator.XInterpolator.Equals(CurveInterpolators.TIME_SQUARE), "x interpolator must be local interpolator");
	  }

	  //-------------------------------------------------------------------------
	  public SurfaceMetadata createMetadata(RawOptionData capFloorData)
	  {
		IList<GenericVolatilitySurfacePeriodParameterMetadata> list = new List<GenericVolatilitySurfacePeriodParameterMetadata>();
		ImmutableList<Period> expiries = capFloorData.Expiries;
		int nExpiries = expiries.size();
		DoubleArray strikes = capFloorData.Strikes;
		int nStrikes = strikes.size();
		for (int i = 0; i < nExpiries; ++i)
		{
		  for (int j = 0; j < nStrikes; ++j)
		  {
			if (Double.isFinite(capFloorData.Data.get(i, j)))
			{
			  list.Add(GenericVolatilitySurfacePeriodParameterMetadata.of(expiries.get(i), SimpleStrike.of(strikes.get(j))));
			}
		  }
		}
		SurfaceMetadata metadata;
		if (capFloorData.DataType.Equals(ValueType.BLACK_VOLATILITY))
		{
		  metadata = Surfaces.blackVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else if (capFloorData.DataType.Equals(ValueType.NORMAL_VOLATILITY))
		{
		  metadata = Surfaces.normalVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else
		{
		  throw new System.ArgumentException("Data type not supported");
		}
		return metadata.withParameterMetadata(list);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SurfaceIborCapletFloorletVolatilityBootstrapDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Meta meta()
	  {
		return SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Meta.INSTANCE;
	  }

	  static SurfaceIborCapletFloorletVolatilityBootstrapDefinition()
	  {
		MetaBean.register(SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SurfaceIborCapletFloorletVolatilityBootstrapDefinition(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, GridSurfaceInterpolator interpolator, Curve shiftCurve)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		this.name = name;
		this.index = index;
		this.dayCount = dayCount;
		this.interpolator = interpolator;
		this.shiftCurve = shiftCurve;
		validate();
	  }

	  public override SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Meta metaBean()
	  {
		return SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Meta.INSTANCE;
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
	  /// Gets the Ibor index. </summary>
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SurfaceIborCapletFloorletVolatilityBootstrapDefinition other = (SurfaceIborCapletFloorletVolatilityBootstrapDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(shiftCurve, other.shiftCurve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftCurve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(192);
		buf.Append("SurfaceIborCapletFloorletVolatilityBootstrapDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SurfaceIborCapletFloorletVolatilityBootstrapDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition), typeof(IborCapletFloorletVolatilitiesName));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition), typeof(IborIndex));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition), typeof(DayCount));
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition), typeof(GridSurfaceInterpolator));
			  shiftCurve_Renamed = DirectMetaProperty.ofImmutable(this, "shiftCurve", typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition), typeof(Curve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "interpolator", "shiftCurve");
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "interpolator", "shiftCurve");
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
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SurfaceIborCapletFloorletVolatilityBootstrapDefinition> builder()
		public override BeanBuilder<SurfaceIborCapletFloorletVolatilityBootstrapDefinition> builder()
		{
		  return new SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SurfaceIborCapletFloorletVolatilityBootstrapDefinition);
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
			  return ((SurfaceIborCapletFloorletVolatilityBootstrapDefinition) bean).Name;
			case 100346066: // index
			  return ((SurfaceIborCapletFloorletVolatilityBootstrapDefinition) bean).Index;
			case 1905311443: // dayCount
			  return ((SurfaceIborCapletFloorletVolatilityBootstrapDefinition) bean).DayCount;
			case 2096253127: // interpolator
			  return ((SurfaceIborCapletFloorletVolatilityBootstrapDefinition) bean).Interpolator;
			case 1908090253: // shiftCurve
			  return ((SurfaceIborCapletFloorletVolatilityBootstrapDefinition) bean).shiftCurve;
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
	  /// The bean-builder for {@code SurfaceIborCapletFloorletVolatilityBootstrapDefinition}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SurfaceIborCapletFloorletVolatilityBootstrapDefinition>
	  {

		internal IborCapletFloorletVolatilitiesName name;
		internal IborIndex index;
		internal DayCount dayCount;
		internal GridSurfaceInterpolator interpolator;
		internal Curve shiftCurve;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name;
			case 100346066: // index
			  return index;
			case 1905311443: // dayCount
			  return dayCount;
			case 2096253127: // interpolator
			  return interpolator;
			case 1908090253: // shiftCurve
			  return shiftCurve;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name = (IborCapletFloorletVolatilitiesName) newValue;
			  break;
			case 100346066: // index
			  this.index = (IborIndex) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount = (DayCount) newValue;
			  break;
			case 2096253127: // interpolator
			  this.interpolator = (GridSurfaceInterpolator) newValue;
			  break;
			case 1908090253: // shiftCurve
			  this.shiftCurve = (Curve) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SurfaceIborCapletFloorletVolatilityBootstrapDefinition build()
		{
		  return new SurfaceIborCapletFloorletVolatilityBootstrapDefinition(name, index, dayCount, interpolator, shiftCurve);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(192);
		  buf.Append("SurfaceIborCapletFloorletVolatilityBootstrapDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator)).Append(',').Append(' ');
		  buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}