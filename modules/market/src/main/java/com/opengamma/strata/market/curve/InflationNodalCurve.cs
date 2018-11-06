using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
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

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using UnitParameterSensitivity = com.opengamma.strata.market.param.UnitParameterSensitivity;

	/// <summary>
	/// Curve specifically designed for inflation, with features for seasonality and initial point.
	/// <para>
	/// The curve details with the initial point and the total seasonal adjustment for each month can be directly provided.
	/// Alternatively, the curve without the initial point and the seasonality as a month-on-month can be provided and the 
	/// final curve computed from there.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class InflationNodalCurve implements NodalCurve, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InflationNodalCurve : NodalCurve, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final NodalCurve underlying;
		private readonly NodalCurve underlying;
	  /// <summary>
	  /// Describes the monthly seasonal adjustments.
	  /// The array has a dimension of 12, one element for each month.
	  /// The adjustments are described as a perturbation to the existing values.
	  /// No adjustment to the fixing value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray seasonality;
	  private readonly DoubleArray seasonality;
	  /// <summary>
	  /// The shift type applied to the unadjusted value and the adjustment.
	  /// (value, seasonality) -> adjustmentType.applyShift(value, seasonality).
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ShiftType adjustmentType;
	  private readonly ShiftType adjustmentType;
	  /// <summary>
	  /// The first x-value, from the curve.
	  /// </summary>
	  [NonSerialized]
	  private readonly double xFixing; // cached, not a property
	  /// <summary>
	  /// The first y-value from the curve.
	  /// </summary>
	  [NonSerialized]
	  private readonly double yFixing; // cached, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the curve.
	  /// <para>
	  /// The seasonal adjustment is the total adjustment for each month, starting with January. 
	  /// </para>
	  /// <para>
	  /// See <seealso cref="#of(NodalCurve, LocalDate, YearMonth, double, SeasonalityDefinition)"/> for
	  /// month-on-month adjustments and the adjustment starting point, including locking the last fixing.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curve">  the curve with initial fixing </param>
	  /// <param name="seasonality">  the total seasonal adjustment for each month, with the first element the January adjustment </param>
	  /// <param name="adjustmentType">  the adjustment type </param>
	  /// <returns> the seasonal curve instance </returns>
	  public static InflationNodalCurve of(NodalCurve curve, DoubleArray seasonality, ShiftType adjustmentType)
	  {

		return new InflationNodalCurve(curve, seasonality, adjustmentType);
	  }

	  /// <summary>
	  /// Obtains an instance from a curve without initial fixing point and month-on-month seasonal adjustment.
	  /// <para>
	  /// The total adjustment is computed by accumulation of the monthly adjustment, starting with no adjustment for the 
	  /// last fixing month.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="curveWithoutFixing">  the curve without the fixing </param>
	  /// <param name="valuationDate">  the valuation date of the curve </param>
	  /// <param name="lastMonth">  the last month for which the fixing is known </param>
	  /// <param name="lastFixingValue">  the value of the last fixing </param>
	  /// <param name="seasonalityDefinition">  the seasonality definition, which is made of month-on-month adjustment 
	  ///   and the adjustment type </param>
	  /// <returns> the seasonal curve instance </returns>
	  public static InflationNodalCurve of(NodalCurve curveWithoutFixing, LocalDate valuationDate, YearMonth lastMonth, double lastFixingValue, SeasonalityDefinition seasonalityDefinition)
	  {

		YearMonth valuationMonth = YearMonth.from(valuationDate);
		ArgChecker.isTrue(lastMonth.isBefore(valuationMonth), "Last fixing month must be before valuation date");
		double nbMonth = valuationMonth.until(lastMonth, MONTHS);
		DoubleArray x = curveWithoutFixing.XValues;
		ArgChecker.isTrue(nbMonth < x.get(0), "The first estimation month should be after the last known index fixing");
		NodalCurve extendedCurve = curveWithoutFixing.withNode(nbMonth, lastFixingValue, ParameterMetadata.empty());
		double[] seasonalityCompoundedArray = new double[12];
		int lastMonthIndex = lastMonth.Month.Value - 1;
		seasonalityCompoundedArray[(int)((nbMonth + 12 + 1) % 12)] = seasonalityDefinition.SeasonalityMonthOnMonth.get(lastMonthIndex % 12);
		for (int i = 1; i < 12; i++)
		{
		  int j = (int)((nbMonth + 12 + 1 + i) % 12);
		  seasonalityCompoundedArray[j] = seasonalityDefinition.AdjustmentType.applyShift(seasonalityCompoundedArray[(j - 1 + 12) % 12], seasonalityDefinition.SeasonalityMonthOnMonth.get((lastMonthIndex + i) % 12));
		}
		return new InflationNodalCurve(extendedCurve, DoubleArray.ofUnsafe(seasonalityCompoundedArray), seasonalityDefinition.AdjustmentType);
	  }

	  //-------------------------------------------------------------------------
	  // restricted constructor
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private InflationNodalCurve(NodalCurve curve, com.opengamma.strata.collect.array.DoubleArray seasonality, com.opengamma.strata.market.ShiftType adjustmentType)
	  private InflationNodalCurve(NodalCurve curve, DoubleArray seasonality, ShiftType adjustmentType)
	  {

		this.underlying = curve;
		this.seasonality = seasonality;
		this.xFixing = curve.XValues.get(0);
		this.yFixing = curve.YValues.get(0);
		int i = seasonalityIndex(xFixing);
		ArgChecker.isTrue(adjustmentType.applyShift(yFixing, seasonality.get(i)) - yFixing < 1.0E-10, "Fixing value should be unadjusted");
		this.adjustmentType = adjustmentType;
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new InflationNodalCurve(underlying, seasonality, adjustmentType);
	  }

	  //-------------------------------------------------------------------------
	  public CurveMetadata Metadata
	  {
		  get
		  {
			return underlying.Metadata;
		  }
	  }

	  public double yValue(double x)
	  {
		int i = seasonalityIndex(x);
		double adjustment = seasonality.get(i);
		return adjustmentType.applyShift(underlying.yValue(x), adjustment);
	  }

	  // The index on the seasonality vector associated to a time (nb months).
	  private int seasonalityIndex(double x)
	  {
		long xLong = (long)Math.Round(x, MidpointRounding.AwayFromZero);
		return (int)(((xLong % 12) + 12) % 12); // Shift by 12 has java compute the remainder of negative numbers as negative
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return underlying.ParameterCount - 1;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return underlying.getParameter(parameterIndex + 1);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return underlying.getParameterMetadata(parameterIndex + 1);
	  }

	  public DoubleArray XValues
	  {
		  get
		  {
			return underlying.XValues.subArray(1);
		  }
	  }

	  public DoubleArray YValues
	  {
		  get
		  {
			return underlying.YValues.subArray(1);
		  }
	  }

	  public UnitParameterSensitivity yValueParameterSensitivity(double x)
	  {
		int i = seasonalityIndex(x);
		double adjustment = seasonality.get(i);
		double derivativeFactor = 0d;
		if (adjustmentType.Equals(ShiftType.ABSOLUTE))
		{
		  derivativeFactor = 1d;
		}
		else if (adjustmentType.Equals(ShiftType.SCALED))
		{
		  derivativeFactor = adjustment;
		}
		else
		{
		  throw new System.ArgumentException("ShiftType " + adjustmentType + " is not supported for sensitivities");
		}
		// remove the first point from the underlying sensitivity
		UnitParameterSensitivity u = underlying.yValueParameterSensitivity(x);
		UnitParameterSensitivity u2 = UnitParameterSensitivity.of(u.MarketDataName, u.ParameterMetadata.subList(1, u.ParameterMetadata.size()), u.Sensitivity.subArray(1));
		return u2.multipliedBy(derivativeFactor);
	  }

	  public double firstDerivative(double x)
	  {
		throw new System.NotSupportedException("Value implemented only at discrete (monthly) values; no derivative available");
	  }

	  public InflationNodalCurve withMetadata(CurveMetadata metadata)
	  {
		return new InflationNodalCurve(underlying.withMetadata(metadata), seasonality, adjustmentType);
	  }

	  public InflationNodalCurve withYValues(DoubleArray values)
	  {
		DoubleArray yExtended = DoubleArray.of(yFixing).concat(values);
		return new InflationNodalCurve(underlying.withYValues(yExtended), seasonality, adjustmentType);
	  }

	  public InflationNodalCurve withValues(DoubleArray xValues, DoubleArray yValues)
	  {
		DoubleArray xExtended = DoubleArray.of(xFixing).concat(xValues);
		DoubleArray yExtended = DoubleArray.of(yFixing).concat(yValues);
		return new InflationNodalCurve(underlying.withValues(xExtended, yExtended), seasonality, adjustmentType);
	  }

	  public InflationNodalCurve withParameter(int parameterIndex, double newValue)
	  {
		return new InflationNodalCurve(underlying.withParameter(parameterIndex + 1, newValue), seasonality, adjustmentType);
	  }

	  public InflationNodalCurve withNode(double x, double y, ParameterMetadata paramMetadata)
	  {
		ArgChecker.isTrue(xFixing < x, "node can be added only after the fixing anchor");
		return new InflationNodalCurve(underlying.withNode(x, y, paramMetadata), seasonality, adjustmentType);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationNodalCurve}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InflationNodalCurve.Meta meta()
	  {
		return InflationNodalCurve.Meta.INSTANCE;
	  }

	  static InflationNodalCurve()
	  {
		MetaBean.register(InflationNodalCurve.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override InflationNodalCurve.Meta metaBean()
	  {
		return InflationNodalCurve.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying curve, before the seasonality adjustment.
	  /// This includes the fixed initial value, which is not treated as a parameter. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public NodalCurve Underlying
	  {
		  get
		  {
			return underlying;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets describes the monthly seasonal adjustments.
	  /// The array has a dimension of 12, one element for each month.
	  /// The adjustments are described as a perturbation to the existing values.
	  /// No adjustment to the fixing value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Seasonality
	  {
		  get
		  {
			return seasonality;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift type applied to the unadjusted value and the adjustment.
	  /// (value, seasonality) -> adjustmentType.applyShift(value, seasonality). </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ShiftType AdjustmentType
	  {
		  get
		  {
			return adjustmentType;
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
		  InflationNodalCurve other = (InflationNodalCurve) obj;
		  return JodaBeanUtils.equal(underlying, other.underlying) && JodaBeanUtils.equal(seasonality, other.seasonality) && JodaBeanUtils.equal(adjustmentType, other.adjustmentType);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(seasonality);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(adjustmentType);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("InflationNodalCurve{");
		buf.Append("underlying").Append('=').Append(underlying).Append(',').Append(' ');
		buf.Append("seasonality").Append('=').Append(seasonality).Append(',').Append(' ');
		buf.Append("adjustmentType").Append('=').Append(JodaBeanUtils.ToString(adjustmentType));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InflationNodalCurve}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(InflationNodalCurve), typeof(NodalCurve));
			  seasonality_Renamed = DirectMetaProperty.ofImmutable(this, "seasonality", typeof(InflationNodalCurve), typeof(DoubleArray));
			  adjustmentType_Renamed = DirectMetaProperty.ofImmutable(this, "adjustmentType", typeof(InflationNodalCurve), typeof(ShiftType));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlying", "seasonality", "adjustmentType");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<NodalCurve> underlying_Renamed;
		/// <summary>
		/// The meta-property for the {@code seasonality} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> seasonality_Renamed;
		/// <summary>
		/// The meta-property for the {@code adjustmentType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ShiftType> adjustmentType_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlying", "seasonality", "adjustmentType");
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
			case -1770633379: // underlying
			  return underlying_Renamed;
			case -857898080: // seasonality
			  return seasonality_Renamed;
			case -1002343865: // adjustmentType
			  return adjustmentType_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends InflationNodalCurve> builder()
		public override BeanBuilder<InflationNodalCurve> builder()
		{
		  return new InflationNodalCurve.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InflationNodalCurve);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<NodalCurve> underlying()
		{
		  return underlying_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code seasonality} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> seasonality()
		{
		  return seasonality_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code adjustmentType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ShiftType> adjustmentType()
		{
		  return adjustmentType_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1770633379: // underlying
			  return ((InflationNodalCurve) bean).Underlying;
			case -857898080: // seasonality
			  return ((InflationNodalCurve) bean).Seasonality;
			case -1002343865: // adjustmentType
			  return ((InflationNodalCurve) bean).AdjustmentType;
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
	  /// The bean-builder for {@code InflationNodalCurve}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<InflationNodalCurve>
	  {

		internal NodalCurve underlying;
		internal DoubleArray seasonality;
		internal ShiftType adjustmentType;

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
			case -1770633379: // underlying
			  return underlying;
			case -857898080: // seasonality
			  return seasonality;
			case -1002343865: // adjustmentType
			  return adjustmentType;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1770633379: // underlying
			  this.underlying = (NodalCurve) newValue;
			  break;
			case -857898080: // seasonality
			  this.seasonality = (DoubleArray) newValue;
			  break;
			case -1002343865: // adjustmentType
			  this.adjustmentType = (ShiftType) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override InflationNodalCurve build()
		{
		  return new InflationNodalCurve(underlying, seasonality, adjustmentType);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("InflationNodalCurve.Builder{");
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying)).Append(',').Append(' ');
		  buf.Append("seasonality").Append('=').Append(JodaBeanUtils.ToString(seasonality)).Append(',').Append(' ');
		  buf.Append("adjustmentType").Append('=').Append(JodaBeanUtils.ToString(adjustmentType));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}