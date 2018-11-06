using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
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

	using ComparisonChain = com.google.common.collect.ComparisonChain;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;
	using MutablePointSensitivities = com.opengamma.strata.market.sensitivity.MutablePointSensitivities;
	using PointSensitivity = com.opengamma.strata.market.sensitivity.PointSensitivity;
	using PointSensitivityBuilder = com.opengamma.strata.market.sensitivity.PointSensitivityBuilder;

	/// <summary>
	/// Point sensitivity to the zero hazard rate curve.
	/// <para>
	/// Holds the sensitivity to the zero hazard rate curve at a specific date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class CreditCurveZeroRateSensitivity implements com.opengamma.strata.market.sensitivity.PointSensitivity, com.opengamma.strata.market.sensitivity.PointSensitivityBuilder, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CreditCurveZeroRateSensitivity : PointSensitivity, PointSensitivityBuilder, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.StandardId legalEntityId;
		private readonly StandardId legalEntityId;
	  /// <summary>
	  /// The zero rate sensitivity.
	  /// <para>
	  /// This stores curve currency, year fraction, sensitivity currency and sensitivity value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.ZeroRateSensitivity zeroRateSensitivity;
	  private readonly ZeroRateSensitivity zeroRateSensitivity;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity identifier </param>
	  /// <param name="currency">  the currency of the curve and sensitivity </param>
	  /// <param name="yearFraction">  the year fraction that was looked up on the curve </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static CreditCurveZeroRateSensitivity of(StandardId legalEntityId, Currency currency, double yearFraction, double sensitivity)
	  {

		ZeroRateSensitivity zeroRateSensitivity = ZeroRateSensitivity.of(currency, yearFraction, sensitivity);
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance with sensitivity currency specified.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity identifier </param>
	  /// <param name="curveCurrency">  the currency of the curve </param>
	  /// <param name="yearFraction">  the year fraction that was looked up on the curve </param>
	  /// <param name="sensitivityCurrency">  the currency of the sensitivity </param>
	  /// <param name="sensitivity">  the value of the sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static CreditCurveZeroRateSensitivity of(StandardId legalEntityId, Currency curveCurrency, double yearFraction, Currency sensitivityCurrency, double sensitivity)
	  {

		ZeroRateSensitivity zeroRateSensitivity = ZeroRateSensitivity.of(curveCurrency, yearFraction, sensitivityCurrency, sensitivity);
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity);
	  }

	  /// <summary>
	  /// Obtains an instance from {@code ZeroRateSensitivity} and {@code StandardId}.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity identifier </param>
	  /// <param name="zeroRateSensitivity">  the zero rate sensitivity </param>
	  /// <returns> the point sensitivity object </returns>
	  public static CreditCurveZeroRateSensitivity of(StandardId legalEntityId, ZeroRateSensitivity zeroRateSensitivity)
	  {

		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity);
	  }

	  //-------------------------------------------------------------------------
	  public Currency Currency
	  {
		  get
		  {
			return zeroRateSensitivity.Currency;
		  }
	  }

	  public double Sensitivity
	  {
		  get
		  {
			return zeroRateSensitivity.Sensitivity;
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the curve for which the sensitivity is computed.
	  /// </summary>
	  /// <returns> the curve currency </returns>
	  public Currency CurveCurrency
	  {
		  get
		  {
			return zeroRateSensitivity.CurveCurrency;
		  }
	  }

	  /// <summary>
	  /// Gets the time that was queried, expressed as a year fraction.
	  /// </summary>
	  /// <returns> the year fraction </returns>
	  public double YearFraction
	  {
		  get
		  {
			return zeroRateSensitivity.YearFraction;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public CreditCurveZeroRateSensitivity withCurrency(Currency currency)
	  {
		if (this.zeroRateSensitivity.Currency.Equals(currency))
		{
		  return this;
		}
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity.withCurrency(currency));
	  }

	  public CreditCurveZeroRateSensitivity withSensitivity(double sensitivity)
	  {
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity.withSensitivity(sensitivity));
	  }

	  public int compareKey(PointSensitivity other)
	  {
		if (other is CreditCurveZeroRateSensitivity)
		{
		  CreditCurveZeroRateSensitivity otherZero = (CreditCurveZeroRateSensitivity) other;
		  return ComparisonChain.start().compare(zeroRateSensitivity.YearFraction, otherZero.zeroRateSensitivity.YearFraction).compare(zeroRateSensitivity.Currency, otherZero.zeroRateSensitivity.Currency).compare(zeroRateSensitivity.CurveCurrency, otherZero.zeroRateSensitivity.CurveCurrency).compare(legalEntityId, otherZero.legalEntityId).result();
		}
		return this.GetType().Name.CompareTo(other.GetType().Name);
	  }

	  public override CreditCurveZeroRateSensitivity convertedTo(Currency resultCurrency, FxRateProvider rateProvider)
	  {
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity.convertedTo(resultCurrency, rateProvider));
	  }

	  //-------------------------------------------------------------------------
	  public override CreditCurveZeroRateSensitivity multipliedBy(double factor)
	  {
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity.multipliedBy(factor));
	  }

	  public CreditCurveZeroRateSensitivity mapSensitivity(System.Func<double, double> @operator)
	  {
		return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity.mapSensitivity(@operator));
	  }

	  public CreditCurveZeroRateSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination.add(this);
	  }

	  public CreditCurveZeroRateSensitivity cloned()
	  {
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the underlying {@code ZeroRateSensitivity}. 
	  /// <para>
	  /// This creates the zero rate sensitivity object by omitting the legal entity identifier.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the point sensitivity object </returns>
	  public ZeroRateSensitivity toZeroRateSensitivity()
	  {
		return zeroRateSensitivity;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CreditCurveZeroRateSensitivity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CreditCurveZeroRateSensitivity.Meta meta()
	  {
		return CreditCurveZeroRateSensitivity.Meta.INSTANCE;
	  }

	  static CreditCurveZeroRateSensitivity()
	  {
		MetaBean.register(CreditCurveZeroRateSensitivity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private CreditCurveZeroRateSensitivity(StandardId legalEntityId, ZeroRateSensitivity zeroRateSensitivity)
	  {
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(zeroRateSensitivity, "zeroRateSensitivity");
		this.legalEntityId = legalEntityId;
		this.zeroRateSensitivity = zeroRateSensitivity;
	  }

	  public override CreditCurveZeroRateSensitivity.Meta metaBean()
	  {
		return CreditCurveZeroRateSensitivity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of a credit derivative.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the zero rate sensitivity.
	  /// <para>
	  /// This stores curve currency, year fraction, sensitivity currency and sensitivity value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZeroRateSensitivity ZeroRateSensitivity
	  {
		  get
		  {
			return zeroRateSensitivity;
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
		  CreditCurveZeroRateSensitivity other = (CreditCurveZeroRateSensitivity) obj;
		  return JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(zeroRateSensitivity, other.zeroRateSensitivity);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(zeroRateSensitivity);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("CreditCurveZeroRateSensitivity{");
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("zeroRateSensitivity").Append('=').Append(JodaBeanUtils.ToString(zeroRateSensitivity));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CreditCurveZeroRateSensitivity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(CreditCurveZeroRateSensitivity), typeof(StandardId));
			  zeroRateSensitivity_Renamed = DirectMetaProperty.ofImmutable(this, "zeroRateSensitivity", typeof(CreditCurveZeroRateSensitivity), typeof(ZeroRateSensitivity));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "legalEntityId", "zeroRateSensitivity");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code zeroRateSensitivity} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZeroRateSensitivity> zeroRateSensitivity_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "legalEntityId", "zeroRateSensitivity");
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
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 1232683479: // zeroRateSensitivity
			  return zeroRateSensitivity_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends CreditCurveZeroRateSensitivity> builder()
		public override BeanBuilder<CreditCurveZeroRateSensitivity> builder()
		{
		  return new CreditCurveZeroRateSensitivity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CreditCurveZeroRateSensitivity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code zeroRateSensitivity} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZeroRateSensitivity> zeroRateSensitivity()
		{
		  return zeroRateSensitivity_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 866287159: // legalEntityId
			  return ((CreditCurveZeroRateSensitivity) bean).LegalEntityId;
			case 1232683479: // zeroRateSensitivity
			  return ((CreditCurveZeroRateSensitivity) bean).ZeroRateSensitivity;
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
	  /// The bean-builder for {@code CreditCurveZeroRateSensitivity}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<CreditCurveZeroRateSensitivity>
	  {

		internal StandardId legalEntityId;
		internal ZeroRateSensitivity zeroRateSensitivity;

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
			case 866287159: // legalEntityId
			  return legalEntityId;
			case 1232683479: // zeroRateSensitivity
			  return zeroRateSensitivity;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 866287159: // legalEntityId
			  this.legalEntityId = (StandardId) newValue;
			  break;
			case 1232683479: // zeroRateSensitivity
			  this.zeroRateSensitivity = (ZeroRateSensitivity) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override CreditCurveZeroRateSensitivity build()
		{
		  return new CreditCurveZeroRateSensitivity(legalEntityId, zeroRateSensitivity);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("CreditCurveZeroRateSensitivity.Builder{");
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId)).Append(',').Append(' ');
		  buf.Append("zeroRateSensitivity").Append('=').Append(JodaBeanUtils.ToString(zeroRateSensitivity));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}