using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using HullWhiteOneFactorPiecewiseConstantInterestRateModel = com.opengamma.strata.pricer.impl.rate.model.HullWhiteOneFactorPiecewiseConstantInterestRateModel;

	/// <summary>
	/// Hull-White one factor model with piecewise constant volatility.
	/// <para>
	/// Reference: Henrard, M. "The Irony in the derivatives discounting Part II: the crisis", Wilmott Journal, 2010, 2, 301-316
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class HullWhiteOneFactorPiecewiseConstantParametersProvider implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class HullWhiteOneFactorPiecewiseConstantParametersProvider : ImmutableBean
	{

	  /// <summary>
	  /// Hull-White one factor model with piecewise constant volatility.
	  /// </summary>
	  private static readonly HullWhiteOneFactorPiecewiseConstantInterestRateModel MODEL = HullWhiteOneFactorPiecewiseConstantInterestRateModel.DEFAULT;

	  /// <summary>
	  /// The Hull-White model parameters.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final HullWhiteOneFactorPiecewiseConstantParameters parameters;
	  private readonly HullWhiteOneFactorPiecewiseConstantParameters parameters;
	  /// <summary>
	  /// The day count applicable to the model.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The valuation date.
	  /// <para>
	  /// The volatilities are calibrated for this date-time.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZonedDateTime valuationDateTime;
	  private readonly ZonedDateTime valuationDateTime;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from Hull-White model parameters and the date-time for which it is valid.
	  /// </summary>
	  /// <param name="parameters">  the Hull-White model parameters </param>
	  /// <param name="dayCount">  the day count applicable to the model </param>
	  /// <param name="valuationDateTime">  the valuation date-time </param>
	  /// <returns> the provider </returns>
	  public static HullWhiteOneFactorPiecewiseConstantParametersProvider of(HullWhiteOneFactorPiecewiseConstantParameters parameters, DayCount dayCount, ZonedDateTime valuationDateTime)
	  {

		return new HullWhiteOneFactorPiecewiseConstantParametersProvider(parameters, dayCount, valuationDateTime);
	  }

	  /// <summary>
	  /// Obtains an instance from Hull-White model parameters and the date, time and zone for which it is valid.
	  /// </summary>
	  /// <param name="parameters">  the Hull-White model parameters </param>
	  /// <param name="dayCount">  the day count applicable to the model </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="valuationTime">  the valuation time </param>
	  /// <param name="valuationZone">  the valuation time zone </param>
	  /// <returns> the provider </returns>
	  public static HullWhiteOneFactorPiecewiseConstantParametersProvider of(HullWhiteOneFactorPiecewiseConstantParameters parameters, DayCount dayCount, LocalDate valuationDate, LocalTime valuationTime, ZoneId valuationZone)
	  {

		return of(parameters, dayCount, valuationDate.atTime(valuationTime).atZone(valuationZone));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the future convexity factor for the specified period at the future reference date.
	  /// </summary>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="startDate">  the start date of the period </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <returns> the convexity factor </returns>
	  public double futuresConvexityFactor(LocalDate referenceDate, LocalDate startDate, LocalDate endDate)
	  {
		double referenceTime = relativeTime(referenceDate);
		double startTime = relativeTime(startDate);
		double endTime = relativeTime(endDate);
		return MODEL.futuresConvexityFactor(parameters, referenceTime, startTime, endTime);
	  }

	  /// <summary>
	  /// Calculates the future convexity factor and its derivative for the specified period at the future reference date.
	  /// </summary>
	  /// <param name="referenceDate">  the reference date </param>
	  /// <param name="startDate">  the start date of the period </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <returns> the convexity factor </returns>
	  public ValueDerivatives futuresConvexityFactorAdjoint(LocalDate referenceDate, LocalDate startDate, LocalDate endDate)
	  {
		double referenceTime = relativeTime(referenceDate);
		double startTime = relativeTime(startDate);
		double endTime = relativeTime(endDate);
		return MODEL.futuresConvexityFactorAdjoint(parameters, referenceTime, startTime, endTime);
	  }

	  /// <summary>
	  /// Converts a date to a relative year fraction.
	  /// <para>
	  /// When the date is after the valuation date, the returned number is negative.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="date">  the date to find the relative year fraction of </param>
	  /// <returns> the relative year fraction </returns>
	  public double relativeTime(LocalDate date)
	  {
		ArgChecker.notNull(date, "date");
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		bool timeIsNegative = valuationDate.isAfter(date);
		if (timeIsNegative)
		{
		  return -dayCount.yearFraction(date, valuationDate);
		}
		return dayCount.yearFraction(valuationDate, date);
	  }

	  /// <summary>
	  /// Calculates the alpha value for the specified period with respect to the maturity date.
	  /// <para>
	  /// The alpha is computed with a bond numeraire of {@code numeraireDate}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date of the period </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <param name="numeraireDate">  the numeraire date </param>
	  /// <param name="maturityDate">  the maturity date </param>
	  /// <returns>  the alpha </returns>
	  public double alpha(LocalDate startDate, LocalDate endDate, LocalDate numeraireDate, LocalDate maturityDate)
	  {
		double startTime = relativeTime(startDate);
		double endTime = relativeTime(endDate);
		double numeraireTime = relativeTime(numeraireDate);
		double maturityTime = relativeTime(maturityDate);
		return MODEL.alpha(parameters, startTime, endTime, numeraireTime, maturityTime);
	  }

	  /// <summary>
	  /// Calculates the alpha and its derivative values for the specified period with respect to the maturity date.
	  /// <para>
	  /// The alpha is computed with a bond numeraire of {@code numeraireDate}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="startDate">  the start date of the period </param>
	  /// <param name="endDate">  the end date of the period </param>
	  /// <param name="numeraireDate">  the numeraire date </param>
	  /// <param name="maturityDate">  the maturity date </param>
	  /// <returns> the alpha adjoint </returns>
	  public ValueDerivatives alphaAdjoint(LocalDate startDate, LocalDate endDate, LocalDate numeraireDate, LocalDate maturityDate)
	  {

		double startTime = relativeTime(startDate);
		double endTime = relativeTime(endDate);
		double numeraireTime = relativeTime(numeraireDate);
		double maturityTime = relativeTime(maturityDate);
		return MODEL.alphaAdjoint(parameters, startTime, endTime, numeraireTime, maturityTime);
	  }

	  /// <summary>
	  /// Returns a Hull-White one-factor model.
	  /// </summary>
	  /// <returns> the model </returns>
	  public HullWhiteOneFactorPiecewiseConstantInterestRateModel Model
	  {
		  get
		  {
			return MODEL;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantParametersProvider}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static HullWhiteOneFactorPiecewiseConstantParametersProvider.Meta meta()
	  {
		return HullWhiteOneFactorPiecewiseConstantParametersProvider.Meta.INSTANCE;
	  }

	  static HullWhiteOneFactorPiecewiseConstantParametersProvider()
	  {
		MetaBean.register(HullWhiteOneFactorPiecewiseConstantParametersProvider.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private HullWhiteOneFactorPiecewiseConstantParametersProvider(HullWhiteOneFactorPiecewiseConstantParameters parameters, DayCount dayCount, ZonedDateTime valuationDateTime)
	  {
		JodaBeanUtils.notNull(parameters, "parameters");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		this.parameters = parameters;
		this.dayCount = dayCount;
		this.valuationDateTime = valuationDateTime;
	  }

	  public override HullWhiteOneFactorPiecewiseConstantParametersProvider.Meta metaBean()
	  {
		return HullWhiteOneFactorPiecewiseConstantParametersProvider.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Hull-White model parameters. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public HullWhiteOneFactorPiecewiseConstantParameters Parameters
	  {
		  get
		  {
			return parameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count applicable to the model. </summary>
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
	  /// Gets the valuation date.
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  HullWhiteOneFactorPiecewiseConstantParametersProvider other = (HullWhiteOneFactorPiecewiseConstantParametersProvider) obj;
		  return JodaBeanUtils.equal(parameters, other.parameters) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("HullWhiteOneFactorPiecewiseConstantParametersProvider{");
		buf.Append("parameters").Append('=').Append(parameters).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantParametersProvider}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  parameters_Renamed = DirectMetaProperty.ofImmutable(this, "parameters", typeof(HullWhiteOneFactorPiecewiseConstantParametersProvider), typeof(HullWhiteOneFactorPiecewiseConstantParameters));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(HullWhiteOneFactorPiecewiseConstantParametersProvider), typeof(DayCount));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(HullWhiteOneFactorPiecewiseConstantParametersProvider), typeof(ZonedDateTime));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "parameters", "dayCount", "valuationDateTime");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code parameters} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<HullWhiteOneFactorPiecewiseConstantParameters> parameters_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> valuationDateTime_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "parameters", "dayCount", "valuationDateTime");
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
			case 458736106: // parameters
			  return parameters_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends HullWhiteOneFactorPiecewiseConstantParametersProvider> builder()
		public override BeanBuilder<HullWhiteOneFactorPiecewiseConstantParametersProvider> builder()
		{
		  return new HullWhiteOneFactorPiecewiseConstantParametersProvider.Builder();
		}

		public override Type beanType()
		{
		  return typeof(HullWhiteOneFactorPiecewiseConstantParametersProvider);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code parameters} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<HullWhiteOneFactorPiecewiseConstantParameters> parameters()
		{
		  return parameters_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> valuationDateTime()
		{
		  return valuationDateTime_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 458736106: // parameters
			  return ((HullWhiteOneFactorPiecewiseConstantParametersProvider) bean).Parameters;
			case 1905311443: // dayCount
			  return ((HullWhiteOneFactorPiecewiseConstantParametersProvider) bean).DayCount;
			case -949589828: // valuationDateTime
			  return ((HullWhiteOneFactorPiecewiseConstantParametersProvider) bean).ValuationDateTime;
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
	  /// The bean-builder for {@code HullWhiteOneFactorPiecewiseConstantParametersProvider}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<HullWhiteOneFactorPiecewiseConstantParametersProvider>
	  {

		internal HullWhiteOneFactorPiecewiseConstantParameters parameters;
		internal DayCount dayCount;
		internal ZonedDateTime valuationDateTime;

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
			case 458736106: // parameters
			  return parameters;
			case 1905311443: // dayCount
			  return dayCount;
			case -949589828: // valuationDateTime
			  return valuationDateTime;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 458736106: // parameters
			  this.parameters = (HullWhiteOneFactorPiecewiseConstantParameters) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount = (DayCount) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime = (ZonedDateTime) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override HullWhiteOneFactorPiecewiseConstantParametersProvider build()
		{
		  return new HullWhiteOneFactorPiecewiseConstantParametersProvider(parameters, dayCount, valuationDateTime);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("HullWhiteOneFactorPiecewiseConstantParametersProvider.Builder{");
		  buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}