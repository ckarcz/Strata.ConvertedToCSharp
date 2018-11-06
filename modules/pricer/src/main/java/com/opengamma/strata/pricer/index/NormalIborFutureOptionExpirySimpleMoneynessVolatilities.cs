using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.index
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using InterpolatedNodalSurface = com.opengamma.strata.market.surface.InterpolatedNodalSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceInfoType = com.opengamma.strata.market.surface.SurfaceInfoType;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;

	/// <summary>
	/// Data provider of volatility for Ibor future options in the normal or Bachelier model.
	/// <para>
	/// The volatility is represented by a surface on the expiry and simple moneyness.
	/// The expiry is measured in number of days (not time) according to a day-count convention.
	/// The simple moneyness can be on the price or on the rate (1-price).
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class NormalIborFutureOptionExpirySimpleMoneynessVolatilities implements NormalIborFutureOptionVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class NormalIborFutureOptionExpirySimpleMoneynessVolatilities : NormalIborFutureOptionVolatilities, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
		private readonly IborIndex index;
	  /// <summary>
	  /// The valuation date-time.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.ZonedDateTime valuationDateTime;
	  private readonly ZonedDateTime valuationDateTime;
	  /// <summary>
	  /// The normal volatility surface.
	  /// <para>
	  /// The x-value of the surface is the expiry, as a year fraction.
	  /// The y-value of the surface is the simple moneyness.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface surface;
	  private readonly Surface surface;
	  /// <summary>
	  /// Whether the moneyness is on the price (true) or on the rate (false).
	  /// </summary>
	  [NonSerialized]
	  private readonly bool moneynessOnPrice; // cached, not a property
	  /// <summary>
	  /// The day count convention of the surface.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the volatility surface and the date-time for which it is valid.
	  /// <para>
	  /// The surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surface must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#SIMPLE_MONEYNESS"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#NORMAL_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="SurfaceInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#normalVolatilityByExpirySimpleMoneyness(String, DayCount, MoneynessType)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="surface">  the implied volatility surface </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <returns> the volatilities </returns>
	  public static NormalIborFutureOptionExpirySimpleMoneynessVolatilities of(IborIndex index, ZonedDateTime valuationDateTime, Surface surface)
	  {

		return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities(index, valuationDateTime, surface);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private NormalIborFutureOptionExpirySimpleMoneynessVolatilities(com.opengamma.strata.basics.index.IborIndex index, java.time.ZonedDateTime valuationDateTime, com.opengamma.strata.market.surface.Surface surface)
	  private NormalIborFutureOptionExpirySimpleMoneynessVolatilities(IborIndex index, ZonedDateTime valuationDateTime, Surface surface)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(valuationDateTime, "valuationDateTime");
		ArgChecker.notNull(surface, "surface");
		surface.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for Normal volatilities");
		surface.Metadata.YValueType.checkEquals(ValueType.SIMPLE_MONEYNESS, "Incorrect y-value type for Normal volatilities");
		surface.Metadata.ZValueType.checkEquals(ValueType.NORMAL_VOLATILITY, "Incorrect z-value type for Normal volatilities");
		DayCount dayCount = surface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing DayCount"));
		MoneynessType moneynessType = surface.Metadata.findInfo(SurfaceInfoType.MONEYNESS_TYPE).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing MoneynessType"));

		this.index = index;
		this.valuationDateTime = valuationDateTime;
		this.surface = surface;
		this.moneynessOnPrice = moneynessType == MoneynessType.PRICE;
		this.dayCount = dayCount;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities(index, valuationDateTime, surface);
	  }

	  //-------------------------------------------------------------------------
	  public IborFutureOptionVolatilitiesName Name
	  {
		  get
		  {
			return IborFutureOptionVolatilitiesName.of(surface.Name.Name);
		  }
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (surface.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(surface));
		}
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return surface.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return surface.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return surface.getParameterMetadata(parameterIndex);
	  }

	  public NormalIborFutureOptionExpirySimpleMoneynessVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities(index, valuationDateTime, surface.withParameter(parameterIndex, newValue));
	  }

	  public NormalIborFutureOptionExpirySimpleMoneynessVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities(index, valuationDateTime, surface.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(double expiry, LocalDate fixingDate, double strikePrice, double futurePrice)
	  {
		double simpleMoneyness = moneynessOnPrice ? strikePrice - futurePrice : futurePrice - strikePrice;
		return surface.zValue(expiry, simpleMoneyness);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is IborFutureOptionSensitivity)
		  {
			IborFutureOptionSensitivity pt = (IborFutureOptionSensitivity) point;
			if (pt.VolatilitiesName.Equals(Name))
			{
			  sens = sens.combinedWith(parameterSensitivity(pt));
			}
		  }
		}
		return sens;
	  }

	  private CurrencyParameterSensitivity parameterSensitivity(IborFutureOptionSensitivity point)
	  {
		double simpleMoneyness = moneynessOnPrice ? point.StrikePrice - point.FuturePrice : point.FuturePrice - point.StrikePrice;
		UnitParameterSensitivity unitSens = surface.zValueParameterSensitivity(point.Expiry, simpleMoneyness);
		return unitSens.multipliedBy(point.Currency, point.Sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public double relativeTime(ZonedDateTime zonedDateTime)
	  {
		ArgChecker.notNull(zonedDateTime, "date");
		return dayCount.relativeYearFraction(valuationDateTime.toLocalDate(), zonedDateTime.toLocalDate());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code NormalIborFutureOptionExpirySimpleMoneynessVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Meta meta()
	  {
		return NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Meta.INSTANCE;
	  }

	  static NormalIborFutureOptionExpirySimpleMoneynessVolatilities()
	  {
		MetaBean.register(NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Builder builder()
	  {
		return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Builder();
	  }

	  public override NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Meta metaBean()
	  {
		return NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index of the underlying future. </summary>
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
	  /// Gets the valuation date-time.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZonedDateTime ValuationDateTime
	  {
		  get
		  {
			return valuationDateTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the normal volatility surface.
	  /// <para>
	  /// The x-value of the surface is the expiry, as a year fraction.
	  /// The y-value of the surface is the simple moneyness.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Surface Surface
	  {
		  get
		  {
			return surface;
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
		  NormalIborFutureOptionExpirySimpleMoneynessVolatilities other = (NormalIborFutureOptionExpirySimpleMoneynessVolatilities) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(surface, other.surface);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(surface);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("NormalIborFutureOptionExpirySimpleMoneynessVolatilities{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code NormalIborFutureOptionExpirySimpleMoneynessVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(NormalIborFutureOptionExpirySimpleMoneynessVolatilities), typeof(IborIndex));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(NormalIborFutureOptionExpirySimpleMoneynessVolatilities), typeof(ZonedDateTime));
			  surface_Renamed = DirectMetaProperty.ofImmutable(this, "surface", typeof(NormalIborFutureOptionExpirySimpleMoneynessVolatilities), typeof(Surface));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "valuationDateTime", "surface");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> valuationDateTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code surface} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Surface> surface_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "valuationDateTime", "surface");
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
			case 100346066: // index
			  return index_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case -1853231955: // surface
			  return surface_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Builder builder()
		{
		  return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(NormalIborFutureOptionExpirySimpleMoneynessVolatilities);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> valuationDateTime()
		{
		  return valuationDateTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code surface} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Surface> surface()
		{
		  return surface_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((NormalIborFutureOptionExpirySimpleMoneynessVolatilities) bean).Index;
			case -949589828: // valuationDateTime
			  return ((NormalIborFutureOptionExpirySimpleMoneynessVolatilities) bean).ValuationDateTime;
			case -1853231955: // surface
			  return ((NormalIborFutureOptionExpirySimpleMoneynessVolatilities) bean).Surface;
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
	  /// The bean-builder for {@code NormalIborFutureOptionExpirySimpleMoneynessVolatilities}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<NormalIborFutureOptionExpirySimpleMoneynessVolatilities>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime valuationDateTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Surface surface_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(NormalIborFutureOptionExpirySimpleMoneynessVolatilities beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.valuationDateTime_Renamed = beanToCopy.ValuationDateTime;
		  this.surface_Renamed = beanToCopy.Surface;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case -1853231955: // surface
			  return surface_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime_Renamed = (ZonedDateTime) newValue;
			  break;
			case -1853231955: // surface
			  this.surface_Renamed = (Surface) newValue;
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

		public override NormalIborFutureOptionExpirySimpleMoneynessVolatilities build()
		{
		  return new NormalIborFutureOptionExpirySimpleMoneynessVolatilities(index_Renamed, valuationDateTime_Renamed, surface_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the index of the underlying future. </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the valuation date-time.
		/// <para>
		/// The volatilities are calibrated for this date-time.
		/// </para>
		/// </summary>
		/// <param name="valuationDateTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDateTime(ZonedDateTime valuationDateTime)
		{
		  JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		  this.valuationDateTime_Renamed = valuationDateTime;
		  return this;
		}

		/// <summary>
		/// Sets the normal volatility surface.
		/// <para>
		/// The x-value of the surface is the expiry, as a year fraction.
		/// The y-value of the surface is the simple moneyness.
		/// </para>
		/// </summary>
		/// <param name="surface">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder surface(Surface surface)
		{
		  JodaBeanUtils.notNull(surface, "surface");
		  this.surface_Renamed = surface;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("NormalIborFutureOptionExpirySimpleMoneynessVolatilities.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime_Renamed)).Append(',').Append(' ');
		  buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}