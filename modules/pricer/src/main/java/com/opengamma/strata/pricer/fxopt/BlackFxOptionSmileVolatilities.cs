using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

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

	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using CurrencyParameterSensitivities = com.opengamma.strata.market.param.CurrencyParameterSensitivities;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using PointSensitivities = com.opengamma.strata.market.sensitivity.PointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Data provider of volatility for FX options in the log-normal or Black-Scholes model.
	/// <para>
	/// The volatility is represented by a term structure of interpolated smile, 
	/// <seealso cref="SmileDeltaTermStructure"/>, which represents expiry dependent smile formed of
	/// ATM, risk reversal and strangle as used in FX market.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BlackFxOptionSmileVolatilities implements BlackFxOptionVolatilities, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BlackFxOptionSmileVolatilities : BlackFxOptionVolatilities, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final FxOptionVolatilitiesName name;
		private readonly FxOptionVolatilitiesName name;
	  /// <summary>
	  /// The currency pair that the volatilities are for.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The valuation date-time.
	  /// All data items in this provider is calibrated for this date-time.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.ZonedDateTime valuationDateTime;
	  private readonly ZonedDateTime valuationDateTime;
	  /// <summary>
	  /// The volatility model.
	  /// <para>
	  /// This represents expiry dependent smile which consists of ATM, risk reversal
	  /// and strangle as used in FX market.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SmileDeltaTermStructure smile;
	  private readonly SmileDeltaTermStructure smile;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance based on a smile.
	  /// </summary>
	  /// <param name="name">  the name of the volatilities </param>
	  /// <param name="currencyPair">  the currency pair </param>
	  /// <param name="valuationTime">  the valuation date-time </param>
	  /// <param name="smile">  the term structure of smile </param>
	  /// <returns> the provider </returns>
	  public static BlackFxOptionSmileVolatilities of(FxOptionVolatilitiesName name, CurrencyPair currencyPair, ZonedDateTime valuationTime, SmileDeltaTermStructure smile)
	  {

		return new BlackFxOptionSmileVolatilities(name, currencyPair, valuationTime, smile);
	  }

	  //-------------------------------------------------------------------------
	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		if (this.name.Equals(name))
		{
		  return (name.MarketDataType.cast(this));
		}
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return smile.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return smile.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return smile.getParameterMetadata(parameterIndex);
	  }

	  public BlackFxOptionSmileVolatilities withParameter(int parameterIndex, double newValue)
	  {
		return new BlackFxOptionSmileVolatilities(name, currencyPair, valuationDateTime, smile.withParameter(parameterIndex, newValue));
	  }

	  public BlackFxOptionSmileVolatilities withPerturbation(ParameterPerturbation perturbation)
	  {
		return new BlackFxOptionSmileVolatilities(name, currencyPair, valuationDateTime, smile.withPerturbation(perturbation));
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(CurrencyPair currencyPair, double expiryTime, double strike, double forward)
	  {
		if (currencyPair.isInverse(this.currencyPair))
		{
		  return smile.volatility(expiryTime, 1d / strike, 1d / forward);
		}
		return smile.volatility(expiryTime, strike, forward);
	  }

	  public CurrencyParameterSensitivities parameterSensitivity(PointSensitivities pointSensitivities)
	  {
		CurrencyParameterSensitivities sens = CurrencyParameterSensitivities.empty();
		foreach (PointSensitivity point in pointSensitivities.Sensitivities)
		{
		  if (point is FxOptionSensitivity)
		  {
			FxOptionSensitivity pt = (FxOptionSensitivity) point;
			if (pt.VolatilitiesName.Equals(Name))
			{
			  sens = sens.combinedWith(parameterSensitivity(pt));
			}
		  }
		}
		return sens;
	  }

	  private CurrencyParameterSensitivity parameterSensitivity(FxOptionSensitivity point)
	  {
		double expiryTime = point.Expiry;
		double strike = currencyPair.isInverse(point.CurrencyPair) ? 1d / point.Strike : point.Strike;
		double forward = currencyPair.isInverse(point.CurrencyPair) ? 1d / point.Forward : point.Forward;
		double pointValue = point.Sensitivity;
		DoubleMatrix bucketedSensi = smile.volatilityAndSensitivities(expiryTime, strike, forward).Sensitivities;
		double[] times = smile.Expiries.toArray();
		int nTimes = times.Length;
		IList<double> sensiList = new List<double>();
		IList<ParameterMetadata> paramList = new List<ParameterMetadata>();
		for (int i = 0; i < nTimes; ++i)
		{
		  DoubleArray deltas = smile.VolatilityTerm.get(i).Delta;
		  int nDeltas = deltas.size();
		  int nDeltasTotal = 2 * nDeltas + 1;
		  double[] deltasTotal = new double[nDeltasTotal]; // absolute delta
		  deltasTotal[nDeltas] = 0.5d;
		  for (int j = 0; j < nDeltas; ++j)
		  {
			deltasTotal[j] = 1d - deltas.get(j);
			deltasTotal[2 * nDeltas - j] = deltas.get(j);
		  }
		  for (int j = 0; j < nDeltasTotal; ++j)
		  {
			sensiList.Add(bucketedSensi.get(i, j) * pointValue);
			DeltaStrike absoluteDelta = DeltaStrike.of(deltasTotal[j]);
			ParameterMetadata parameterMetadata = FxVolatilitySurfaceYearFractionParameterMetadata.of(times[i], absoluteDelta, currencyPair);
			paramList.Add(parameterMetadata);
		  }
		}
		return CurrencyParameterSensitivity.of(name, paramList, point.Currency, DoubleArray.copyOf(sensiList));
	  }

	  //-------------------------------------------------------------------------
	  public double price(double expiry, PutCall putCall, double strike, double forward, double volatility)
	  {
		return BlackFormulaRepository.price(forward, strike, expiry, volatility, putCall.Call);
	  }

	  //-------------------------------------------------------------------------
	  public double relativeTime(ZonedDateTime dateTime)
	  {
		ArgChecker.notNull(dateTime, "dateTime");
		LocalDate valuationDate = valuationDateTime.toLocalDate();
		LocalDate date = dateTime.toLocalDate();
		return smile.DayCount.relativeYearFraction(valuationDate, date);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSmileVolatilities}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BlackFxOptionSmileVolatilities.Meta meta()
	  {
		return BlackFxOptionSmileVolatilities.Meta.INSTANCE;
	  }

	  static BlackFxOptionSmileVolatilities()
	  {
		MetaBean.register(BlackFxOptionSmileVolatilities.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BlackFxOptionSmileVolatilities.Builder builder()
	  {
		return new BlackFxOptionSmileVolatilities.Builder();
	  }

	  private BlackFxOptionSmileVolatilities(FxOptionVolatilitiesName name, CurrencyPair currencyPair, ZonedDateTime valuationDateTime, SmileDeltaTermStructure smile)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		JodaBeanUtils.notNull(smile, "smile");
		this.name = name;
		this.currencyPair = currencyPair;
		this.valuationDateTime = valuationDateTime;
		this.smile = smile;
	  }

	  public override BlackFxOptionSmileVolatilities.Meta metaBean()
	  {
		return BlackFxOptionSmileVolatilities.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxOptionVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that the volatilities are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the valuation date-time.
	  /// All data items in this provider is calibrated for this date-time. </summary>
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
	  /// Gets the volatility model.
	  /// <para>
	  /// This represents expiry dependent smile which consists of ATM, risk reversal
	  /// and strangle as used in FX market.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SmileDeltaTermStructure Smile
	  {
		  get
		  {
			return smile;
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
		  BlackFxOptionSmileVolatilities other = (BlackFxOptionSmileVolatilities) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(valuationDateTime, other.valuationDateTime) && JodaBeanUtils.equal(smile, other.smile);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDateTime);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(smile);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("BlackFxOptionSmileVolatilities{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("valuationDateTime").Append('=').Append(valuationDateTime).Append(',').Append(' ');
		buf.Append("smile").Append('=').Append(JodaBeanUtils.ToString(smile));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSmileVolatilities}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(BlackFxOptionSmileVolatilities), typeof(FxOptionVolatilitiesName));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(BlackFxOptionSmileVolatilities), typeof(CurrencyPair));
			  valuationDateTime_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDateTime", typeof(BlackFxOptionSmileVolatilities), typeof(ZonedDateTime));
			  smile_Renamed = DirectMetaProperty.ofImmutable(this, "smile", typeof(BlackFxOptionSmileVolatilities), typeof(SmileDeltaTermStructure));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currencyPair", "valuationDateTime", "smile");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxOptionVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> valuationDateTime_Renamed;
		/// <summary>
		/// The meta-property for the {@code smile} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SmileDeltaTermStructure> smile_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currencyPair", "valuationDateTime", "smile");
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
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 109556488: // smile
			  return smile_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BlackFxOptionSmileVolatilities.Builder builder()
		{
		  return new BlackFxOptionSmileVolatilities.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BlackFxOptionSmileVolatilities);
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
		public MetaProperty<FxOptionVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valuationDateTime} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> valuationDateTime()
		{
		  return valuationDateTime_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code smile} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SmileDeltaTermStructure> smile()
		{
		  return smile_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((BlackFxOptionSmileVolatilities) bean).Name;
			case 1005147787: // currencyPair
			  return ((BlackFxOptionSmileVolatilities) bean).CurrencyPair;
			case -949589828: // valuationDateTime
			  return ((BlackFxOptionSmileVolatilities) bean).ValuationDateTime;
			case 109556488: // smile
			  return ((BlackFxOptionSmileVolatilities) bean).Smile;
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
	  /// The bean-builder for {@code BlackFxOptionSmileVolatilities}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BlackFxOptionSmileVolatilities>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxOptionVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime valuationDateTime_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SmileDeltaTermStructure smile_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(BlackFxOptionSmileVolatilities beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.valuationDateTime_Renamed = beanToCopy.ValuationDateTime;
		  this.smile_Renamed = beanToCopy.Smile;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case -949589828: // valuationDateTime
			  return valuationDateTime_Renamed;
			case 109556488: // smile
			  return smile_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (FxOptionVolatilitiesName) newValue;
			  break;
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case -949589828: // valuationDateTime
			  this.valuationDateTime_Renamed = (ZonedDateTime) newValue;
			  break;
			case 109556488: // smile
			  this.smile_Renamed = (SmileDeltaTermStructure) newValue;
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

		public override BlackFxOptionSmileVolatilities build()
		{
		  return new BlackFxOptionSmileVolatilities(name_Renamed, currencyPair_Renamed, valuationDateTime_Renamed, smile_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the volatilities. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(FxOptionVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency pair that the volatilities are for. </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the valuation date-time.
		/// All data items in this provider is calibrated for this date-time. </summary>
		/// <param name="valuationDateTime">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valuationDateTime(ZonedDateTime valuationDateTime)
		{
		  JodaBeanUtils.notNull(valuationDateTime, "valuationDateTime");
		  this.valuationDateTime_Renamed = valuationDateTime;
		  return this;
		}

		/// <summary>
		/// Sets the volatility model.
		/// <para>
		/// This represents expiry dependent smile which consists of ATM, risk reversal
		/// and strangle as used in FX market.
		/// </para>
		/// </summary>
		/// <param name="smile">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder smile(SmileDeltaTermStructure smile)
		{
		  JodaBeanUtils.notNull(smile, "smile");
		  this.smile_Renamed = smile;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("BlackFxOptionSmileVolatilities.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("valuationDateTime").Append('=').Append(JodaBeanUtils.ToString(valuationDateTime_Renamed)).Append(',').Append(' ');
		  buf.Append("smile").Append('=').Append(JodaBeanUtils.ToString(smile_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}