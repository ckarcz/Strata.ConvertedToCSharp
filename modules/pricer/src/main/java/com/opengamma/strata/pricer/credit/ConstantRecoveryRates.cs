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
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketDataName = com.opengamma.strata.data.MarketDataName;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;

	/// <summary>
	/// The constant recovery rate.
	/// <para>
	/// The recovery rate is constant for any given date.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ConstantRecoveryRates implements RecoveryRates, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ConstantRecoveryRates : RecoveryRates, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.StandardId legalEntityId;
		private readonly StandardId legalEntityId;
	  /// <summary>
	  /// The valuation date.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final java.time.LocalDate valuationDate;
	  private readonly LocalDate valuationDate;
	  /// <summary>
	  /// The recovery rate.
	  /// <para>
	  /// The recovery rate is represented in decimal form, and must be between 0 and 1 inclusive.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double recoveryRate;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly double recoveryRate_Renamed;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="legalEntityId">  the legal entity identifier </param>
	  /// <param name="valuationDate">  the valuation date </param>
	  /// <param name="recoveryRate">  the recovery rate </param>
	  /// <returns> the instance </returns>
	  public static ConstantRecoveryRates of(StandardId legalEntityId, LocalDate valuationDate, double recoveryRate)
	  {
		return new ConstantRecoveryRates(legalEntityId, valuationDate, recoveryRate);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inRangeInclusive(recoveryRate_Renamed, 0d, 1d, "recoveryRate");
	  }

	  //-------------------------------------------------------------------------
	  public double recoveryRate(LocalDate date)
	  {
		return recoveryRate_Renamed;
	  }

	  public Optional<T> findData<T>(MarketDataName<T> name)
	  {
		return null;
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return 1;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		ArgChecker.isTrue(parameterIndex == 0, "Only one parameter for ConstantRecoveryRates");
		return recoveryRate_Renamed;
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return ParameterMetadata.empty();
	  }

	  public ConstantRecoveryRates withParameter(int parameterIndex, double newValue)
	  {
		ArgChecker.isTrue(parameterIndex == 0, "Only one parameter for ConstantRecoveryRates");
		return new ConstantRecoveryRates(legalEntityId, valuationDate, newValue);
	  }

	  public ConstantRecoveryRates withPerturbation(ParameterPerturbation perturbation)
	  {
		double perturbedValue = perturbation(0, recoveryRate_Renamed, getParameterMetadata(0));
		return new ConstantRecoveryRates(legalEntityId, valuationDate, perturbedValue);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantRecoveryRates}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ConstantRecoveryRates.Meta meta()
	  {
		return ConstantRecoveryRates.Meta.INSTANCE;
	  }

	  static ConstantRecoveryRates()
	  {
		MetaBean.register(ConstantRecoveryRates.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ConstantRecoveryRates(StandardId legalEntityId, LocalDate valuationDate, double recoveryRate)
	  {
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(valuationDate, "valuationDate");
		this.legalEntityId = legalEntityId;
		this.valuationDate = valuationDate;
		this.recoveryRate_Renamed = recoveryRate;
		validate();
	  }

	  public override ConstantRecoveryRates.Meta metaBean()
	  {
		return ConstantRecoveryRates.Meta.INSTANCE;
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
	  /// Gets the valuation date. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate ValuationDate
	  {
		  get
		  {
			return valuationDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the recovery rate.
	  /// <para>
	  /// The recovery rate is represented in decimal form, and must be between 0 and 1 inclusive.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double RecoveryRate
	  {
		  get
		  {
			return recoveryRate_Renamed;
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
		  ConstantRecoveryRates other = (ConstantRecoveryRates) obj;
		  return JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(valuationDate, other.valuationDate) && JodaBeanUtils.equal(recoveryRate_Renamed, other.recoveryRate_Renamed);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valuationDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(recoveryRate_Renamed);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("ConstantRecoveryRates{");
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("valuationDate").Append('=').Append(valuationDate).Append(',').Append(' ');
		buf.Append("recoveryRate").Append('=').Append(JodaBeanUtils.ToString(recoveryRate_Renamed));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantRecoveryRates}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(ConstantRecoveryRates), typeof(StandardId));
			  valuationDate_Renamed = DirectMetaProperty.ofImmutable(this, "valuationDate", typeof(ConstantRecoveryRates), typeof(LocalDate));
			  recoveryRate_Renamed = DirectMetaProperty.ofImmutable(this, "recoveryRate", typeof(ConstantRecoveryRates), Double.TYPE);
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "legalEntityId", "valuationDate", "recoveryRate");
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
		/// The meta-property for the {@code valuationDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> valuationDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code recoveryRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> recoveryRate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "legalEntityId", "valuationDate", "recoveryRate");
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
			case 113107279: // valuationDate
			  return valuationDate_Renamed;
			case 2002873877: // recoveryRate
			  return recoveryRate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ConstantRecoveryRates> builder()
		public override BeanBuilder<ConstantRecoveryRates> builder()
		{
		  return new ConstantRecoveryRates.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ConstantRecoveryRates);
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
		/// The meta-property for the {@code valuationDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> valuationDate()
		{
		  return valuationDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code recoveryRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> recoveryRate()
		{
		  return recoveryRate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 866287159: // legalEntityId
			  return ((ConstantRecoveryRates) bean).LegalEntityId;
			case 113107279: // valuationDate
			  return ((ConstantRecoveryRates) bean).ValuationDate;
			case 2002873877: // recoveryRate
			  return ((ConstantRecoveryRates) bean).RecoveryRate;
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
	  /// The bean-builder for {@code ConstantRecoveryRates}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ConstantRecoveryRates>
	  {

		internal StandardId legalEntityId;
		internal LocalDate valuationDate;
		internal double recoveryRate;

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
			case 113107279: // valuationDate
			  return valuationDate;
			case 2002873877: // recoveryRate
			  return recoveryRate;
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
			case 113107279: // valuationDate
			  this.valuationDate = (LocalDate) newValue;
			  break;
			case 2002873877: // recoveryRate
			  this.recoveryRate = (double?) newValue.Value;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ConstantRecoveryRates build()
		{
		  return new ConstantRecoveryRates(legalEntityId, valuationDate, recoveryRate);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("ConstantRecoveryRates.Builder{");
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId)).Append(',').Append(' ');
		  buf.Append("valuationDate").Append('=').Append(JodaBeanUtils.ToString(valuationDate)).Append(',').Append(' ');
		  buf.Append("recoveryRate").Append('=').Append(JodaBeanUtils.ToString(recoveryRate));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}