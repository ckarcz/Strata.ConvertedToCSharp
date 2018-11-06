using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fxopt
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FxProduct = com.opengamma.strata.product.fx.FxProduct;
	using Barrier = com.opengamma.strata.product.option.Barrier;

	/// <summary>
	/// FX (European) single barrier option.
	/// <para>
	/// An FX option is a financial instrument that provides an option to exchange two currencies at a specified future time 
	/// only when barrier event occurs (knock-in option) or does not occur (knock-out option).
	/// </para>
	/// <para>
	/// Depending on the barrier defined in <seealso cref="Barrier"/>, the options are classified into four types: up-and-in, 
	/// down-and-in, up-and-out and down-and-out.
	/// </para>
	/// <para>
	/// For example, an up-and-out call on a 'EUR 1.00 / USD -1.41' exchange with barrier of 1.5 is the option to
	/// perform a foreign exchange on the expiry date, where USD 1.41 is paid to receive EUR 1.00, only when EUR/USD rate does 
	/// not exceed 1.5 during the barrier event observation period.
	/// </para>
	/// <para>
	/// In case of the occurrence (non-occurrence for knock-in options) of the barrier event, the option becomes worthless, 
	/// or alternatively, a rebate is made.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxSingleBarrierOption implements com.opengamma.strata.product.fx.FxProduct, com.opengamma.strata.basics.Resolvable<ResolvedFxSingleBarrierOption>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSingleBarrierOption : FxProduct, Resolvable<ResolvedFxSingleBarrierOption>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FxVanillaOption underlyingOption;
		private readonly FxVanillaOption underlyingOption;
	  /// <summary>
	  /// The barrier description.
	  /// <para>
	  /// The barrier level stored in this field must be represented based on the direction of the currency pair in the 
	  /// underlying FX transaction.
	  /// </para>
	  /// <para>
	  /// For example, if the underlying option is an option on EUR/GBP, the barrier should be a certain level of EUR/GBP rate.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.option.Barrier barrier;
	  private readonly Barrier barrier;
	  /// <summary>
	  /// For a 'out' option, the amount is paid when the barrier is reached; 
	  /// for a 'in' option, the amount is paid at expiry if the barrier is not reached.
	  /// <para>
	  /// This is the notional amount represented in one of the currency pair.
	  /// The amount should be positive.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.currency.CurrencyAmount rebate;
	  private readonly CurrencyAmount rebate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains FX single barrier option with rebate.
	  /// </summary>
	  /// <param name="underlyingOption">  the underlying FX vanilla option </param>
	  /// <param name="barrier">  the barrier </param>
	  /// <param name="rebate">  the rebate </param>
	  /// <returns> the instance </returns>
	  public static FxSingleBarrierOption of(FxVanillaOption underlyingOption, Barrier barrier, CurrencyAmount rebate)
	  {
		return new FxSingleBarrierOption(underlyingOption, barrier, rebate);
	  }

	  /// <summary>
	  /// Obtains FX single barrier option without rebate.
	  /// </summary>
	  /// <param name="underlyingOption">  the underlying FX vanilla option </param>
	  /// <param name="barrier">  the barrier </param>
	  /// <returns> the instance </returns>
	  public static FxSingleBarrierOption of(FxVanillaOption underlyingOption, Barrier barrier)
	  {
		return new FxSingleBarrierOption(underlyingOption, barrier, null);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (rebate != null)
		{
		  ArgChecker.isTrue(rebate.Amount > 0d, "rebate must be positive");
		  ArgChecker.isTrue(underlyingOption.Underlying.CurrencyPair.contains(rebate.Currency), "The rebate currency must be one of underlying currency pair");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets currency pair of the base currency and counter currency.
	  /// <para>
	  /// This currency pair is conventional, thus indifferent to the direction of FX.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency pair </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return underlyingOption.CurrencyPair;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedFxSingleBarrierOption resolve(ReferenceData refData)
	  {
		if (rebate != null)
		{
		  return ResolvedFxSingleBarrierOption.of(underlyingOption.resolve(refData), barrier, rebate);
		}
		return ResolvedFxSingleBarrierOption.of(underlyingOption.resolve(refData), barrier);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingleBarrierOption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSingleBarrierOption.Meta meta()
	  {
		return FxSingleBarrierOption.Meta.INSTANCE;
	  }

	  static FxSingleBarrierOption()
	  {
		MetaBean.register(FxSingleBarrierOption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxSingleBarrierOption.Builder builder()
	  {
		return new FxSingleBarrierOption.Builder();
	  }

	  private FxSingleBarrierOption(FxVanillaOption underlyingOption, Barrier barrier, CurrencyAmount rebate)
	  {
		JodaBeanUtils.notNull(underlyingOption, "underlyingOption");
		JodaBeanUtils.notNull(barrier, "barrier");
		this.underlyingOption = underlyingOption;
		this.barrier = barrier;
		this.rebate = rebate;
		validate();
	  }

	  public override FxSingleBarrierOption.Meta metaBean()
	  {
		return FxSingleBarrierOption.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying FX vanilla option. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxVanillaOption UnderlyingOption
	  {
		  get
		  {
			return underlyingOption;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the barrier description.
	  /// <para>
	  /// The barrier level stored in this field must be represented based on the direction of the currency pair in the
	  /// underlying FX transaction.
	  /// </para>
	  /// <para>
	  /// For example, if the underlying option is an option on EUR/GBP, the barrier should be a certain level of EUR/GBP rate.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Barrier Barrier
	  {
		  get
		  {
			return barrier;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets for a 'out' option, the amount is paid when the barrier is reached;
	  /// for a 'in' option, the amount is paid at expiry if the barrier is not reached.
	  /// <para>
	  /// This is the notional amount represented in one of the currency pair.
	  /// The amount should be positive.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<CurrencyAmount> Rebate
	  {
		  get
		  {
			return Optional.ofNullable(rebate);
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
		  FxSingleBarrierOption other = (FxSingleBarrierOption) obj;
		  return JodaBeanUtils.equal(underlyingOption, other.underlyingOption) && JodaBeanUtils.equal(barrier, other.barrier) && JodaBeanUtils.equal(rebate, other.rebate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlyingOption);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(barrier);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rebate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("FxSingleBarrierOption{");
		buf.Append("underlyingOption").Append('=').Append(underlyingOption).Append(',').Append(' ');
		buf.Append("barrier").Append('=').Append(barrier).Append(',').Append(' ');
		buf.Append("rebate").Append('=').Append(JodaBeanUtils.ToString(rebate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSingleBarrierOption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  underlyingOption_Renamed = DirectMetaProperty.ofImmutable(this, "underlyingOption", typeof(FxSingleBarrierOption), typeof(FxVanillaOption));
			  barrier_Renamed = DirectMetaProperty.ofImmutable(this, "barrier", typeof(FxSingleBarrierOption), typeof(Barrier));
			  rebate_Renamed = DirectMetaProperty.ofImmutable(this, "rebate", typeof(FxSingleBarrierOption), typeof(CurrencyAmount));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "underlyingOption", "barrier", "rebate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code underlyingOption} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxVanillaOption> underlyingOption_Renamed;
		/// <summary>
		/// The meta-property for the {@code barrier} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Barrier> barrier_Renamed;
		/// <summary>
		/// The meta-property for the {@code rebate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyAmount> rebate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "underlyingOption", "barrier", "rebate");
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
			case 87556658: // underlyingOption
			  return underlyingOption_Renamed;
			case -333143113: // barrier
			  return barrier_Renamed;
			case -934952029: // rebate
			  return rebate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxSingleBarrierOption.Builder builder()
		{
		  return new FxSingleBarrierOption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSingleBarrierOption);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code underlyingOption} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxVanillaOption> underlyingOption()
		{
		  return underlyingOption_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code barrier} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Barrier> barrier()
		{
		  return barrier_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rebate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyAmount> rebate()
		{
		  return rebate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 87556658: // underlyingOption
			  return ((FxSingleBarrierOption) bean).UnderlyingOption;
			case -333143113: // barrier
			  return ((FxSingleBarrierOption) bean).Barrier;
			case -934952029: // rebate
			  return ((FxSingleBarrierOption) bean).rebate;
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
	  /// The bean-builder for {@code FxSingleBarrierOption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxSingleBarrierOption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxVanillaOption underlyingOption_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Barrier barrier_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyAmount rebate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(FxSingleBarrierOption beanToCopy)
		{
		  this.underlyingOption_Renamed = beanToCopy.UnderlyingOption;
		  this.barrier_Renamed = beanToCopy.Barrier;
		  this.rebate_Renamed = beanToCopy.rebate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 87556658: // underlyingOption
			  return underlyingOption_Renamed;
			case -333143113: // barrier
			  return barrier_Renamed;
			case -934952029: // rebate
			  return rebate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 87556658: // underlyingOption
			  this.underlyingOption_Renamed = (FxVanillaOption) newValue;
			  break;
			case -333143113: // barrier
			  this.barrier_Renamed = (Barrier) newValue;
			  break;
			case -934952029: // rebate
			  this.rebate_Renamed = (CurrencyAmount) newValue;
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

		public override FxSingleBarrierOption build()
		{
		  return new FxSingleBarrierOption(underlyingOption_Renamed, barrier_Renamed, rebate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the underlying FX vanilla option. </summary>
		/// <param name="underlyingOption">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlyingOption(FxVanillaOption underlyingOption)
		{
		  JodaBeanUtils.notNull(underlyingOption, "underlyingOption");
		  this.underlyingOption_Renamed = underlyingOption;
		  return this;
		}

		/// <summary>
		/// Sets the barrier description.
		/// <para>
		/// The barrier level stored in this field must be represented based on the direction of the currency pair in the
		/// underlying FX transaction.
		/// </para>
		/// <para>
		/// For example, if the underlying option is an option on EUR/GBP, the barrier should be a certain level of EUR/GBP rate.
		/// </para>
		/// </summary>
		/// <param name="barrier">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder barrier(Barrier barrier)
		{
		  JodaBeanUtils.notNull(barrier, "barrier");
		  this.barrier_Renamed = barrier;
		  return this;
		}

		/// <summary>
		/// Sets for a 'out' option, the amount is paid when the barrier is reached;
		/// for a 'in' option, the amount is paid at expiry if the barrier is not reached.
		/// <para>
		/// This is the notional amount represented in one of the currency pair.
		/// The amount should be positive.
		/// </para>
		/// </summary>
		/// <param name="rebate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rebate(CurrencyAmount rebate)
		{
		  this.rebate_Renamed = rebate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(128);
		  buf.Append("FxSingleBarrierOption.Builder{");
		  buf.Append("underlyingOption").Append('=').Append(JodaBeanUtils.ToString(underlyingOption_Renamed)).Append(',').Append(' ');
		  buf.Append("barrier").Append('=').Append(JodaBeanUtils.ToString(barrier_Renamed)).Append(',').Append(' ');
		  buf.Append("rebate").Append('=').Append(JodaBeanUtils.ToString(rebate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}