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
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;

	/// <summary>
	/// A constant maturity swap (CMS) or CMS cap/floor, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Cms"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedCms} from a {@code Cms}
	/// using <seealso cref="Cms#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedCms} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ResolvedCms implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedCms : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final ResolvedCmsLeg cmsLeg;
		private readonly ResolvedCmsLeg cmsLeg;
	  /// <summary>
	  /// The optional pay leg of the product.
	  /// <para>
	  /// Typically this is associated with periodic fixed or Ibor rate payments without compounding or notional exchange.
	  /// </para>
	  /// <para>
	  /// These periodic payments are not made for certain CMS products. Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.product.swap.ResolvedSwapLeg payLeg;
	  private readonly ResolvedSwapLeg payLeg;

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
	  public static ResolvedCms of(ResolvedCmsLeg cmsLeg)
	  {
		return new ResolvedCms(cmsLeg, null);
	  }

	  /// <summary>
	  /// Obtains an instance from a CMS leg and a pay leg.
	  /// </summary>
	  /// <param name="cmsLeg">  the CMS leg </param>
	  /// <param name="payLeg">  the pay leg </param>
	  /// <returns> the CMS </returns>
	  public static ResolvedCms of(ResolvedCmsLeg cmsLeg, ResolvedSwapLeg payLeg)
	  {
		ArgChecker.notNull(cmsLeg, "cmsLeg");
		ArgChecker.notNull(payLeg, "payLeg");
		return new ResolvedCms(cmsLeg, payLeg);
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
	  /// <summary>
	  /// Returns the set of currencies referred to by the CMS.
	  /// <para>
	  /// This returns the complete set of payment currencies for the CMS.
	  /// This will typically return one currency, but could return two.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of payment currencies referred to by this swap </returns>
	  public ImmutableSet<Currency> allPaymentCurrencies()
	  {
		if (payLeg == null)
		{
		  return ImmutableSet.of(cmsLeg.Currency);
		}
		return ImmutableSet.of(cmsLeg.Currency, payLeg.Currency);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCms}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedCms.Meta meta()
	  {
		return ResolvedCms.Meta.INSTANCE;
	  }

	  static ResolvedCms()
	  {
		MetaBean.register(ResolvedCms.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ResolvedCms(ResolvedCmsLeg cmsLeg, ResolvedSwapLeg payLeg)
	  {
		JodaBeanUtils.notNull(cmsLeg, "cmsLeg");
		this.cmsLeg = cmsLeg;
		this.payLeg = payLeg;
		validate();
	  }

	  public override ResolvedCms.Meta metaBean()
	  {
		return ResolvedCms.Meta.INSTANCE;
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
	  public ResolvedCmsLeg CmsLeg
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
	  /// These periodic payments are not made for certain CMS products. Instead the premium is paid upfront.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<ResolvedSwapLeg> PayLeg
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
		  ResolvedCms other = (ResolvedCms) obj;
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
		buf.Append("ResolvedCms{");
		buf.Append("cmsLeg").Append('=').Append(cmsLeg).Append(',').Append(' ');
		buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedCms}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  cmsLeg_Renamed = DirectMetaProperty.ofImmutable(this, "cmsLeg", typeof(ResolvedCms), typeof(ResolvedCmsLeg));
			  payLeg_Renamed = DirectMetaProperty.ofImmutable(this, "payLeg", typeof(ResolvedCms), typeof(ResolvedSwapLeg));
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
		internal MetaProperty<ResolvedCmsLeg> cmsLeg_Renamed;
		/// <summary>
		/// The meta-property for the {@code payLeg} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedSwapLeg> payLeg_Renamed;
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
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ResolvedCms> builder()
		public override BeanBuilder<ResolvedCms> builder()
		{
		  return new ResolvedCms.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedCms);
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
		public MetaProperty<ResolvedCmsLeg> cmsLeg()
		{
		  return cmsLeg_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code payLeg} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedSwapLeg> payLeg()
		{
		  return payLeg_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1356515323: // cmsLeg
			  return ((ResolvedCms) bean).CmsLeg;
			case -995239866: // payLeg
			  return ((ResolvedCms) bean).payLeg;
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
	  /// The bean-builder for {@code ResolvedCms}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ResolvedCms>
	  {

		internal ResolvedCmsLeg cmsLeg;
		internal ResolvedSwapLeg payLeg;

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
			  this.cmsLeg = (ResolvedCmsLeg) newValue;
			  break;
			case -995239866: // payLeg
			  this.payLeg = (ResolvedSwapLeg) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ResolvedCms build()
		{
		  return new ResolvedCms(cmsLeg, payLeg);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ResolvedCms.Builder{");
		  buf.Append("cmsLeg").Append('=').Append(JodaBeanUtils.ToString(cmsLeg)).Append(',').Append(' ');
		  buf.Append("payLeg").Append('=').Append(JodaBeanUtils.ToString(payLeg));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}