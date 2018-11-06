using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.cms
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

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using Index = com.opengamma.strata.basics.index.Index;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using SwapIndex = com.opengamma.strata.product.swap.SwapIndex;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;

	/// <summary>
	/// A constant maturity swap (CMS) or CMS cap/floor.
	/// <para>
	/// The CMS product consists of two legs, a CMS leg and a pay leg.
	/// The CMS leg of CMS periodically pays coupons based on swap rate, which is the observed
	/// value of a <seealso cref="SwapIndex swap index"/>.
	/// The pay leg is any swap leg from a standard interest rate swap. The pay leg may be absent
	/// for certain CMS products, with the premium paid upfront instead, as defined on <seealso cref="CmsTrade"/>.
	/// </para>
	/// <para>
	/// CMS cap/floor instruments can be created. These are defined as a set of call/put options
	/// on successive swap rates, creating CMS caplets/floorlets.
	/// </para>
	/// <para>
	/// For example, a CMS trade might involve an agreement to exchange the difference between
	/// the fixed rate of 1% and the swap rate of 5-year 'GBP-FIXED-6M-LIBOR-6M' swaps every 6 months for 2 years.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class Cms implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedCms>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Cms : Product, Resolvable<ResolvedCms>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CmsLeg cmsLeg;
		private readonly CmsLeg cmsLeg;
	  /// <summary>
	  /// The optional pay leg of the product.
	  /// <para>
	  /// Typically this is associated with periodic fixed or Ibor rate payments without compounding or notional exchange.
	  /// </para>
	  /// <para>
	  /// These periodic payments are not made over the lifetime of the product for certain CMS products.
	  /// Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.product.swap.SwapLeg payLeg;
	  private readonly SwapLeg payLeg;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a CMS leg with no pay leg.
	  /// <para>
	  /// The pay leg is absent in the resulting CMS.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <returns> the CMS </returns>
	  public static Cms of(CmsLeg cmsLeg)
	  {
		return new Cms(cmsLeg, null);
	  }

	  /// <summary>
	  /// Obtains an instance from a CMS leg and a pay leg.
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="payLeg">  the pay leg </param>
	  /// <returns> the CMS </returns>
	  public static Cms of(CmsLeg cmsLeg, SwapLeg payLeg)
	  {
		return new Cms(cmsLeg, payLeg);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (PayLeg.Present)
		{
		  ArgChecker.isFalse(payLeg.PayReceive.Equals(cmsLeg.PayReceive), "Two legs should have different Pay/Receive flags");
		}
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedCms resolve(ReferenceData refData)
	  {
		if (payLeg == null)
		{
		  return ResolvedCms.of(cmsLeg.resolve(refData));
		}
		return ResolvedCms.of(cmsLeg.resolve(refData), payLeg.resolve(refData));
	  }

	  //-------------------------------------------------------------------------
	  public override ImmutableSet<Currency> allPaymentCurrencies()
	  {
		if (payLeg == null)
		{
		  return ImmutableSet.of(cmsLeg.Currency);
		}
		else
		{
		  return ImmutableSet.of(cmsLeg.Currency, payLeg.Currency);
		}
	  }

	  public ImmutableSet<Currency> allCurrencies()
	  {
		if (payLeg == null)
		{
		  return ImmutableSet.of(cmsLeg.Currency);
		}
		else
		{
		  ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
		  builder.add(cmsLeg.Currency);
		  builder.addAll(payLeg.allCurrencies());
		  return builder.build();
		}
	  }

	  /// <summary>
	  /// Returns the set of rate indices referred to by the CMS.
	  /// <para>
	  /// The CMS leg will refer to one index, such as 'GBP-LIBOR-3M'.
	  /// The pay leg may refer to a different index.
	  /// The swap index will not be included.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this CMS </returns>
	  public ImmutableSet<Index> allRateIndices()
	  {
		IborIndex cmsIndex = cmsLeg.UnderlyingIndex;
		if (payLeg == null)
		{
		  return ImmutableSet.of(cmsIndex);
		}
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		payLeg.collectIndices(builder);
		builder.add(cmsIndex);
		return builder.build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Cms}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Cms.Meta meta()
	  {
		return Cms.Meta.INSTANCE;
	  }

	  static Cms()
	  {
		MetaBean.register(Cms.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private Cms(CmsLeg cmsLeg, SwapLeg payLeg)
	  {
		JodaBeanUtils.notNull(cmsLeg, "cmsLeg");
		this.cmsLeg = cmsLeg;
		this.payLeg = payLeg;
		validate();
	  }

	  public override Cms.Meta metaBean()
	  {
		return Cms.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the CMS leg of the product.
	  /// <para>
	  /// This is associated with periodic payments based on swap rate.
	  /// The payments are CMS coupons, CMS caplets or CMS floors.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CmsLeg CmsLeg
	  {
		  get
		  {
			return cmsLeg;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the optional pay leg of the product.
	  /// <para>
	  /// Typically this is associated with periodic fixed or Ibor rate payments without compounding or notional exchange.
	  /// </para>
	  /// <para>
	  /// These periodic payments are not made over the lifetime of the product for certain CMS products.
	  /// Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<SwapLeg> PayLeg
	  {
		  get
		  {
			return Optional.ofNullable(payLeg);
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
		  Cms other = (Cms) obj;
		  return JodaBeanUtils.equal(cmsLeg, other.cmsLeg) && JodaBeanUtils.equal(payLeg, other.payLeg);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cmsLeg);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(payLeg);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("Cms{");
		buf.Append("cmsLeg").Append('=').Append(cmsLeg).Append(',').Append(' ');
		buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Cms}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  cmsLeg_Renamed = DirectMetaProperty.ofImmutable(this, "cmsLeg", typeof(Cms), typeof(CmsLeg));
			  payLeg_Renamed = DirectMetaProperty.ofImmutable(this, "payLeg", typeof(Cms), typeof(SwapLeg));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "cmsLeg", "payLeg");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code cmsLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CmsLeg> cmsLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code payLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwapLeg> payLeg_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "cmsLeg", "payLeg");
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
			case -1356515323: // cmsLeg
			  return cmsLeg_Renamed;
			case -995239866: // payLeg
			  return payLeg_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends Cms> builder()
		public override BeanBuilder<Cms> builder()
		{
		  return new Cms.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Cms);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code cmsLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CmsLeg> cmsLeg()
		{
		  return cmsLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwapLeg> payLeg()
		{
		  return payLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1356515323: // cmsLeg
			  return ((Cms) bean).CmsLeg;
			case -995239866: // payLeg
			  return ((Cms) bean).payLeg;
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
	  /// The bean-builder for {@code Cms}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<Cms>
	  {

		internal CmsLeg cmsLeg;
		internal SwapLeg payLeg;

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
			case -1356515323: // cmsLeg
			  return cmsLeg;
			case -995239866: // payLeg
			  return payLeg;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1356515323: // cmsLeg
			  this.cmsLeg = (CmsLeg) newValue;
			  break;
			case -995239866: // payLeg
			  this.payLeg = (SwapLeg) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Cms build()
		{
		  return new Cms(cmsLeg, payLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("Cms.Builder{");
		  buf.Append("cmsLeg").Append('=').Append(JodaBeanUtils.ToString(cmsLeg)).Append(',').Append(' ');
		  buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}