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

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ValueType = com.opengamma.strata.market.ValueType;
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
	using NormalFormulaRepository = com.opengamma.strata.pricer.impl.option.NormalFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Volatility for Ibor caplet/floorlet in the normal or Bachelier model based on a surface.
	/// <para>
	/// The volatility is represented by a surface on the expiry and strike dimensions.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class NormalIborCapletFloorletExpiryStrikeVolatilities implements NormalIborCapletFloorletVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class NormalIborCapletFloorletExpiryStrikeVolatilities : NormalIborCapletFloorletVolatilities, ImmutableBean
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
	  /// The y-value of the surface is the strike.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface surface;
	  private readonly Surface surface;
	  /// <summary>
	  /// The day count convention of the surface.
	  /// </summary>
	  [NonSerialized]
	  private readonly DayCount dayCount; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the implied volatility surface and the date-time for which it is valid.
	  /// <para>
	  /// The surface is specified by an instance of <seealso cref="Surface"/>, such as <seealso cref="InterpolatedNodalSurface"/>.
	  /// The surface must contain the correct metadata:
	  /// <ul>
	  /// <li>The x-value type must be <seealso cref="ValueType#YEAR_FRACTION"/>
	  /// <li>The y-value type must be <seealso cref="ValueType#STRIKE"/>
	  /// <li>The z-value type must be <seealso cref="ValueType#NORMAL_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="SurfaceInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#normalVolatilityByExpiryStrike(String, DayCount)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index for which the data is valid </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="surface">  the implied volatility surface </param>
	  /// <returns> the volatilities </returns>
	  public static NormalIborCapletFloorletExpiryStrikeVolatilities of(IborIndex index, ZonedDateTime valuationDateTime, Surface surface)
	  {

		return new NormalIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private NormalIborCapletFloorletExpiryStrikeVolatilities(com.opengamma.strata.basics.index.IborIndex index, java.time.ZonedDateTime valuationDateTime, com.opengamma.strata.market.surface.Surface surface)
	  private NormalIborCapletFloorletExpiryStrikeVolatilities(IborIndex index, ZonedDateTime valuationDateTime, Surface surface)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(valuationDateTime, "valuationDateTime");
		ArgChecker.notNull(surface, "surface");
		surface.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for Normal volatilities");
		surface.Metadata.YValueType.checkEquals(ValueType.STRIKE, "Incorrect y-value type for Normal volatilities");
		surface.Metadata.ZValueType.checkEquals(ValueType.NORMAL_VOLATILITY, "Incorrect z-value type for Normal volatilities");
		DayCount dayCount = surface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing DayCount"));

		this.index = index;
		this.valuationDateTime = valuationDateTime;
		this.surface = surface;
		this.dayCount = dayCount;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new NormalIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface);
	  }

	  //-------------------------------------------------------------------------
	  public IborCapletFloorletVolatilitiesName Name
	  {
		  get
		  {
			return IborCapletFloorletVolatilitiesName.of(surface.Name.Name);
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

	  public NormalIborCapletFloorletExpiryStrikeVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new NormalIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface.withParameter(parameterIndex, newValue));
	  }

	  public NormalIborCapletFloorletExpiryStrikeVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new NormalIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(double expiry, double strike, double forward)
	  {
		return surface.zValue(expiry, strike);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is IborCapletFloorletSensitivity)
		  {
			IborCapletFloorletSensitivity pt = (IborCapletFloorletSensitivity) point;
			if (pt.VolatilitiesName.Equals(Name))
			{
			  sens = sens.combinedWith(parameterSensitivity(pt));
			}
		  }
		}
		return sens;
	  }

	  private CurrencyParameterSensitivity parameterSensitivity(IborCapletFloorletSensitivity point)
	  {
		double expiry = point.Expiry;
		double strike = point.Strike;
		UnitParameterSensitivity unitSens = surface.zValueParameterSensitivity(expiry, strike);
		return unitSens.multipliedBy(point.Currency, point.Sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public double price(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return NormalFormulaRepository.price(forward, strike, expiry, volatility, putCall);
	  }

	  public double priceDelta(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return NormalFormulaRepository.delta(forward, strike, expiry, volatility, putCall);
	  }

	  public double priceGamma(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return NormalFormulaRepository.gamma(forward, strike, expiry, volatility, putCall);
	  }

	  public double priceTheta(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return NormalFormulaRepository.theta(forward, strike, expiry, volatility, putCall);
	  }

	  public double priceVega(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return NormalFormulaRepository.vega(forward, strike, expiry, volatility, putCall);
	  }

	  //-------------------------------------------------------------------------
	  public double relativeTime(ZonedDateTime dateTime)
	  {
		ArgChecker.notNull(dateTime, "dateTime");
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		LocalDate date = dateTime.toLocalDate();
		return dayCount.relativeYearFraction(valuationDate, date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code NormalIborCapletFloorletExpiryStrikeVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static NormalIborCapletFloorletExpiryStrikeVolatilities.Meta meta()
	  {
		return NormalIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE;
	  }

	  static NormalIborCapletFloorletExpiryStrikeVolatilities()
	  {
		MetaBean.register(NormalIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override NormalIborCapletFloorletExpiryStrikeVolatilities.Meta metaBean()
	  {
		return NormalIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The data must valid in terms of this Ibor index.
	  /// </para>
	  /// </summary>
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
	  /// The y-value of the surface is the strike.
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  NormalIborCapletFloorletExpiryStrikeVolatilities other = (NormalIborCapletFloorletExpiryStrikeVolatilities) obj;
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
		buf.Append("NormalIborCapletFloorletExpiryStrikeVolatilities{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code NormalIborCapletFloorletExpiryStrikeVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(NormalIborCapletFloorletExpiryStrikeVolatilities), typeof(IborIndex));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(NormalIborCapletFloorletExpiryStrikeVolatilities), typeof(ZonedDateTime));
			  surface_Renamed = DirectMetaProperty.ofImmutable(this, "surface", typeof(NormalIborCapletFloorletExpiryStrikeVolatilities), typeof(Surface));
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

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends NormalIborCapletFloorletExpiryStrikeVolatilities> builder()
		public override BeanBuilder<NormalIborCapletFloorletExpiryStrikeVolatilities> builder()
		{
		  return new NormalIborCapletFloorletExpiryStrikeVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(NormalIborCapletFloorletExpiryStrikeVolatilities);
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
			  return ((NormalIborCapletFloorletExpiryStrikeVolatilities) bean).Index;
			case -949589828: // valuationDateTime
			  return ((NormalIborCapletFloorletExpiryStrikeVolatilities) bean).ValuationDateTime;
			case -1853231955: // surface
			  return ((NormalIborCapletFloorletExpiryStrikeVolatilities) bean).Surface;
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
	  /// The bean-builder for {@code NormalIborCapletFloorletExpiryStrikeVolatilities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<NormalIborCapletFloorletExpiryStrikeVolatilities>
	  {

		internal IborIndex index;
		internal ZonedDateTime valuationDateTime;
		internal Surface surface;

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
			case 100346066: // index
			  return index;
			case -949589828: // valuationDateTime
			  return valuationDateTime;
			case -1853231955: // surface
			  return surface;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index = (IborIndex) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime = (ZonedDateTime) newValue;
			  break;
			case -1853231955: // surface
			  this.surface = (Surface) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override NormalIborCapletFloorletExpiryStrikeVolatilities build()
		{
		  return new NormalIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("NormalIborCapletFloorletExpiryStrikeVolatilities.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime)).Append(',').Append(' ');
		  buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}