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
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveInfoType = com.opengamma.strata.market.curve.CurveInfoType;
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
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Volatility for Ibor caplet/floorlet in the shifted log-normal or shifted Black model based on a surface.
	/// <para> 
	/// The volatility is represented by a surface on the expiry and strike dimensions.
	/// The shift parameter is represented by a curve defined by expiry. 
	/// </para>
	/// <para>
	/// Although this implementation is able to handle zero shift, it is recommended to use 
	/// <seealso cref="BlackIborCapletFloorletExpiryStrikeVolatilities"/> instead.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities implements BlackIborCapletFloorletVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities : BlackIborCapletFloorletVolatilities, ImmutableBean
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
	  /// The Black volatility surface.
	  /// <para>
	  /// The x-value of the surface is the expiry, as a year fraction.
	  /// The y-value of the surface is the strike.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.surface.Surface surface;
	  private readonly Surface surface;
	  /// <summary>
	  /// The shift parameter of shifted Black model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// Use <seealso cref="BlackIborCapletFloorletExpiryStrikeVolatilities"/> for zero shift.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve shiftCurve;
	  private readonly Curve shiftCurve;
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
	  /// <li>The z-value type must be <seealso cref="ValueType#BLACK_VOLATILITY"/>
	  /// <li>The day count must be set in the additional information using <seealso cref="SurfaceInfoType#DAY_COUNT"/>
	  /// </ul>
	  /// Suitable surface metadata can be created using
	  /// <seealso cref="Surfaces#blackVolatilityByExpiryStrike(String, DayCount)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the Ibor index for which the data is valid </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <param name="surface">  the implied volatility surface </param>
	  /// <param name="shiftCurve">  the shift surface </param>
	  /// <returns> the volatilities </returns>
	  public static ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities of(IborIndex index, ZonedDateTime valuationDateTime, Surface surface, Curve shiftCurve)
	  {

		return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface, shiftCurve);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(com.opengamma.strata.basics.index.IborIndex index, java.time.ZonedDateTime valuationDateTime, com.opengamma.strata.market.surface.Surface surface, com.opengamma.strata.market.curve.Curve shiftCurve)
	  private ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(IborIndex index, ZonedDateTime valuationDateTime, Surface surface, Curve shiftCurve)
	  {

		ArgChecker.notNull(index, "index");
		ArgChecker.notNull(valuationDateTime, "valuationDateTime");
		ArgChecker.notNull(surface, "surface");
		ArgChecker.notNull(shiftCurve, "shiftCurve");
		surface.Metadata.XValueType.checkEquals(ValueType.YEAR_FRACTION, "Incorrect x-value type for Black volatilities");
		surface.Metadata.YValueType.checkEquals(ValueType.STRIKE, "Incorrect y-value type for Black volatilities");
		surface.Metadata.ZValueType.checkEquals(ValueType.BLACK_VOLATILITY, "Incorrect z-value type for Black volatilities");
		DayCount dayCount = surface.Metadata.findInfo(SurfaceInfoType.DAY_COUNT).orElseThrow(() => new System.ArgumentException("Incorrect surface metadata, missing DayCount"));
		validate(shiftCurve, dayCount);

		this.index = index;
		this.valuationDateTime = valuationDateTime;
		this.surface = surface;
		this.dayCount = dayCount;
		this.shiftCurve = shiftCurve;
	  }

	  private static void validate(Curve curve, DayCount dayCount)
	  {
		if (!curve.Metadata.findInfo(CurveInfoType.DAY_COUNT).orElse(dayCount).Equals(dayCount))
		{
		  throw new System.ArgumentException("shift curve must have the same day count as surface");
		}
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface, shiftCurve);
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
		if (shiftCurve.Name.Equals(name))
		{
		  return (name.MarketDataType.cast(shiftCurve));
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

	  public ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface.withParameter(parameterIndex, newValue), shiftCurve);
	  }

	  public ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface.withPerturbation(perturbation), shiftCurve);
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(double expiry, double strike, double forward)
	  {
		double shift = shiftCurve.yValue(expiry);
		return surface.zValue(expiry, strike + shift);
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
		double shift = shiftCurve.yValue(expiry);
		UnitParameterSensitivity unitSens = surface.zValueParameterSensitivity(expiry, strike + shift);
		return unitSens.multipliedBy(point.Currency, point.Sensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public double price(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = shiftCurve.yValue(expiry);
		return BlackFormulaRepository.price(forward + shift, strike + shift, expiry, volatility, putCall.Call);
	  }

	  public double priceDelta(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = shiftCurve.yValue(expiry);
		return BlackFormulaRepository.delta(forward + shift, strike + shift, expiry, volatility, putCall.Call);
	  }

	  public double priceGamma(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = shiftCurve.yValue(expiry);
		return BlackFormulaRepository.gamma(forward + shift, strike + shift, expiry, volatility);
	  }

	  public double priceTheta(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = shiftCurve.yValue(expiry);
		return BlackFormulaRepository.driftlessTheta(forward + shift, strike + shift, expiry, volatility);
	  }

	  public double priceVega(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		double shift = shiftCurve.yValue(expiry);
		return BlackFormulaRepository.vega(forward + shift, strike + shift, expiry, volatility);
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
	  /// The meta-bean for {@code ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Meta meta()
	  {
		return ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE;
	  }

	  static ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities()
	  {
		MetaBean.register(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Meta metaBean()
	  {
		return ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Meta.INSTANCE;
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
	  /// Gets the Black volatility surface.
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
	  /// <summary>
	  /// Gets the shift parameter of shifted Black model.
	  /// <para>
	  /// The x value of the curve is the expiry.
	  /// Use <seealso cref="BlackIborCapletFloorletExpiryStrikeVolatilities"/> for zero shift.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve ShiftCurve
	  {
		  get
		  {
			return shiftCurve;
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
		  ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities other = (ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(surface, other.surface) && JodaBeanUtils.equal(shiftCurve, other.shiftCurve);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(surface);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftCurve);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities{");
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("surface").Append('=').Append(surface).Append(',').Append(' ');
		buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities), typeof(IborIndex));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities), typeof(ZonedDateTime));
			  surface_Renamed = DirectMetaProperty.ofImmutable(this, "surface", typeof(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities), typeof(Surface));
			  shiftCurve_Renamed = DirectMetaProperty.ofImmutable(this, "shiftCurve", typeof(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities), typeof(Curve));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "valuationDateTime", "surface", "shiftCurve");
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
		/// The meta-property for the {@code shiftCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> shiftCurve_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "valuationDateTime", "surface", "shiftCurve");
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
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities> builder()
		public override BeanBuilder<ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities> builder()
		{
		  return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities);
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
			case 100346066: // index
			  return ((ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) bean).Index;
			case -949589828: // valuationDateTime
			  return ((ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) bean).ValuationDateTime;
			case -1853231955: // surface
			  return ((ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) bean).Surface;
			case 1908090253: // shiftCurve
			  return ((ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities) bean).ShiftCurve;
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
	  /// The bean-builder for {@code ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities>
	  {

		internal IborIndex index;
		internal ZonedDateTime valuationDateTime;
		internal Surface surface;
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
			case 100346066: // index
			  return index;
			case -949589828: // valuationDateTime
			  return valuationDateTime;
			case -1853231955: // surface
			  return surface;
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
			case 100346066: // index
			  this.index = (IborIndex) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime = (ZonedDateTime) newValue;
			  break;
			case -1853231955: // surface
			  this.surface = (Surface) newValue;
			  break;
			case 1908090253: // shiftCurve
			  this.shiftCurve = (Curve) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities build()
		{
		  return new ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities(index, valuationDateTime, surface, shiftCurve);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ShiftedBlackIborCapletFloorletExpiryStrikeVolatilities.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime)).Append(',').Append(' ');
		  buf.Append("surface").Append('=').Append(JodaBeanUtils.ToString(surface)).Append(',').Append(' ');
		  buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}