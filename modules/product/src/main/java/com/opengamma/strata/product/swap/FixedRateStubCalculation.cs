using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
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

	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FixedRateComputation = com.opengamma.strata.product.rate.FixedRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// Defines the rate applicable in the initial or final stub of a fixed swap leg.
	/// <para>
	/// A standard swap leg consists of a regular periodic schedule and one or two stub periods at each end.
	/// This class defines what fixed rate to use during a stub.
	/// </para>
	/// <para>
	/// The calculation may be specified in two ways.
	/// <ul>
	/// <li>A fixed rate, applicable for the whole stub
	/// <li>A fixed amount for the whole stub
	/// </ul>
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class FixedRateStubCalculation implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedRateStubCalculation : ImmutableBean
	{

	  /// <summary>
	  /// An instance that has no special rate handling.
	  /// </summary>
	  public static readonly FixedRateStubCalculation NONE = new FixedRateStubCalculation(null, null);

	  /// <summary>
	  /// The fixed rate to use in the stub.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// If the fixed rate is present, then {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> fixedRate;
	  private readonly double? fixedRate;
	  /// <summary>
	  /// The known amount to pay/receive for the stub.
	  /// <para>
	  /// If the known amount is present, then {@code fixedRate} must not be present.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.currency.CurrencyAmount knownAmount;
	  private readonly CurrencyAmount knownAmount;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with a single fixed rate.
	  /// </summary>
	  /// <param name="fixedRate">  the fixed rate for the stub </param>
	  /// <returns> the stub </returns>
	  public static FixedRateStubCalculation ofFixedRate(double fixedRate)
	  {
		return new FixedRateStubCalculation(fixedRate, null);
	  }

	  /// <summary>
	  /// Obtains an instance with a known amount of interest.
	  /// </summary>
	  /// <param name="knownAmount">  the known amount of interest </param>
	  /// <returns> the stub </returns>
	  public static FixedRateStubCalculation ofKnownAmount(CurrencyAmount knownAmount)
	  {
		ArgChecker.notNull(knownAmount, "knownAmount");
		return new FixedRateStubCalculation(null, knownAmount);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (fixedRate != null && knownAmount != null)
		{
		  throw new System.ArgumentException("Either rate or amount may be specified, not both");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates the {@code RateComputation} for the stub.
	  /// </summary>
	  /// <param name="defaultRate">  the default fixed rate </param>
	  /// <returns> the rate computation </returns>
	  internal RateComputation createRateComputation(double defaultRate)
	  {
		if (FixedRate)
		{
		  return FixedRateComputation.of(fixedRate.Value);
		}
		else if (KnownAmount)
		{
		  return KnownAmountRateComputation.of(knownAmount);
		}
		else
		{
		  return FixedRateComputation.of(defaultRate);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Checks if the stub has a fixed rate.
	  /// </summary>
	  /// <returns> true if a fixed rate applies </returns>
	  public bool FixedRate
	  {
		  get
		  {
			return fixedRate != null;
		  }
	  }

	  /// <summary>
	  /// Checks if the stub has a known amount.
	  /// </summary>
	  /// <returns> true if the stub has a known amount </returns>
	  public bool KnownAmount
	  {
		  get
		  {
			return knownAmount != null;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateStubCalculation}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedRateStubCalculation.Meta meta()
	  {
		return FixedRateStubCalculation.Meta.INSTANCE;
	  }

	  static FixedRateStubCalculation()
	  {
		MetaBean.register(FixedRateStubCalculation.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private FixedRateStubCalculation(double? fixedRate, CurrencyAmount knownAmount)
	  {
		this.fixedRate = fixedRate;
		this.knownAmount = knownAmount;
		validate();
	  }

	  public override FixedRateStubCalculation.Meta metaBean()
	  {
		return FixedRateStubCalculation.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate to use in the stub.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// If the fixed rate is present, then {@code knownAmount} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FixedRate
	  {
		  get
		  {
			return fixedRate != null ? double?.of(fixedRate) : double?.empty();
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the known amount to pay/receive for the stub.
	  /// <para>
	  /// If the known amount is present, then {@code fixedRate} must not be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<CurrencyAmount> KnownAmount
	  {
		  get
		  {
			return Optional.ofNullable(knownAmount);
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
		  FixedRateStubCalculation other = (FixedRateStubCalculation) obj;
		  return JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(knownAmount, other.knownAmount);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(knownAmount);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("FixedRateStubCalculation{");
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("knownAmount").Append('=').Append(JodaBeanUtils.ToString(knownAmount));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedRateStubCalculation}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(FixedRateStubCalculation), typeof(Double));
			  knownAmount_Renamed = DirectMetaProperty.ofImmutable(this, "knownAmount", typeof(FixedRateStubCalculation), typeof(CurrencyAmount));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "fixedRate", "knownAmount");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code knownAmount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> knownAmount_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "fixedRate", "knownAmount");
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
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case -158727813: // knownAmount
			  return knownAmount_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends FixedRateStubCalculation> builder()
		public override BeanBuilder<FixedRateStubCalculation> builder()
		{
		  return new FixedRateStubCalculation.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedRateStubCalculation);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code knownAmount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> knownAmount()
		{
		  return knownAmount_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 747425396: // fixedRate
			  return ((FixedRateStubCalculation) bean).fixedRate;
			case -158727813: // knownAmount
			  return ((FixedRateStubCalculation) bean).knownAmount;
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
	  /// The bean-builder for {@code FixedRateStubCalculation}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<FixedRateStubCalculation>
	  {

		internal double? fixedRate;
		internal CurrencyAmount knownAmount;

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
			case 747425396: // fixedRate
			  return fixedRate;
			case -158727813: // knownAmount
			  return knownAmount;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 747425396: // fixedRate
			  this.fixedRate = (double?) newValue;
			  break;
			case -158727813: // knownAmount
			  this.knownAmount = (CurrencyAmount) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override FixedRateStubCalculation build()
		{
		  return new FixedRateStubCalculation(fixedRate, knownAmount);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("FixedRateStubCalculation.Builder{");
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate)).Append(',').Append(' ');
		  buf.Append("knownAmount").Append('=').Append(JodaBeanUtils.ToString(knownAmount));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}