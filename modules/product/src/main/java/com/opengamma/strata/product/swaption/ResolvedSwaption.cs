using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swaption
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

	using Iterables = com.google.common.collect.Iterables;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using LongShort = com.opengamma.strata.product.common.LongShort;
	using ResolvedSwap = com.opengamma.strata.product.swap.ResolvedSwap;

	/// <summary>
	/// A swaption, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Swaption"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedSwaption} from a {@code Swaption}
	/// using <seealso cref="Swaption#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedSwaption} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ResolvedSwaption implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedSwaption : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.LongShort longShort;
		private readonly LongShort longShort;
	  /// <summary>
	  /// Settlement method.
	  /// <para>
	  /// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final SwaptionSettlement swaptionSettlement;
	  private readonly SwaptionSettlement swaptionSettlement;
	  /// <summary>
	  /// The expiry date-time of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.ZonedDateTime expiry;
	  private readonly ZonedDateTime expiry;
	  /// <summary>
	  /// The underlying swap.
	  /// <para>
	  /// At expiry, if the option is exercised, this swap will be entered into. The swap description is the swap 
	  /// as viewed by the party long the option.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.ResolvedSwap underlying;
	  private readonly ResolvedSwap underlying;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date of the option.
	  /// </summary>
	  /// <returns> the expiry date </returns>
	  public LocalDate ExpiryDate
	  {
		  get
		  {
			return expiry.toLocalDate();
		  }
	  }

	  /// <summary>
	  /// Gets the currency of the swaption.
	  /// <para>
	  /// This is the currency of the underlying swap, which is not allowed to be cross-currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return Iterables.getOnlyElement(underlying.allPaymentCurrencies());
		  }
	  }

	  /// <summary>
	  /// Gets the index of the underlying swap.
	  /// </summary>
	  /// <returns> the Ibor index of the underlying swap </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return (IborIndex) underlying.allIndices().GetEnumerator().next();
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwaption}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedSwaption.Meta meta()
	  {
		return ResolvedSwaption.Meta.INSTANCE;
	  }

	  static ResolvedSwaption()
	  {
		MetaBean.register(ResolvedSwaption.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedSwaption.Builder builder()
	  {
		return new ResolvedSwaption.Builder();
	  }

	  private ResolvedSwaption(LongShort longShort, SwaptionSettlement swaptionSettlement, ZonedDateTime expiry, ResolvedSwap underlying)
	  {
		JodaBeanUtils.notNull(longShort, "longShort");
		JodaBeanUtils.notNull(swaptionSettlement, "swaptionSettlement");
		JodaBeanUtils.notNull(expiry, "expiry");
		JodaBeanUtils.notNull(underlying, "underlying");
		this.longShort = longShort;
		this.swaptionSettlement = swaptionSettlement;
		this.expiry = expiry;
		this.underlying = underlying;
	  }

	  public override ResolvedSwaption.Meta metaBean()
	  {
		return ResolvedSwaption.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the option is long or short.
	  /// <para>
	  /// Long indicates that the owner wants the option to be in the money at expiry.
	  /// Short indicates that the owner wants the option to be out of the money at expiry.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LongShort LongShort
	  {
		  get
		  {
			return longShort;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets settlement method.
	  /// <para>
	  /// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SwaptionSettlement SwaptionSettlement
	  {
		  get
		  {
			return swaptionSettlement;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry date-time of the option.
	  /// <para>
	  /// The option is European, and can only be exercised on the expiry date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ZonedDateTime Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying swap.
	  /// <para>
	  /// At expiry, if the option is exercised, this swap will be entered into. The swap description is the swap
	  /// as viewed by the party long the option.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ResolvedSwap Underlying
	  {
		  get
		  {
			return underlying;
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
		  ResolvedSwaption other = (ResolvedSwaption) obj;
		  return JodaBeanUtils.equal(longShort, other.longShort) && JodaBeanUtils.equal(swaptionSettlement, other.swaptionSettlement) && JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(underlying, other.underlying);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(longShort);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(swaptionSettlement);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(underlying);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("ResolvedSwaption{");
		buf.Append("longShort").Append('=').Append(longShort).Append(',').Append(' ');
		buf.Append("swaptionSettlement").Append('=').Append(swaptionSettlement).Append(',').Append(' ');
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedSwaption}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  longShort_Renamed = DirectMetaProperty.ofImmutable(this, "longShort", typeof(ResolvedSwaption), typeof(LongShort));
			  swaptionSettlement_Renamed = DirectMetaProperty.ofImmutable(this, "swaptionSettlement", typeof(ResolvedSwaption), typeof(SwaptionSettlement));
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(ResolvedSwaption), typeof(ZonedDateTime));
			  underlying_Renamed = DirectMetaProperty.ofImmutable(this, "underlying", typeof(ResolvedSwaption), typeof(ResolvedSwap));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "longShort", "swaptionSettlement", "expiry", "underlying");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code longShort} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LongShort> longShort_Renamed;
		/// <summary>
		/// The meta-property for the {@code swaptionSettlement} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SwaptionSettlement> swaptionSettlement_Renamed;
		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ZonedDateTime> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code underlying} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ResolvedSwap> underlying_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "longShort", "swaptionSettlement", "expiry", "underlying");
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
			case 116685664: // longShort
			  return longShort_Renamed;
			case -1937554512: // swaptionSettlement
			  return swaptionSettlement_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedSwaption.Builder builder()
		{
		  return new ResolvedSwaption.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedSwaption);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code longShort} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LongShort> longShort()
		{
		  return longShort_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code swaptionSettlement} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SwaptionSettlement> swaptionSettlement()
		{
		  return swaptionSettlement_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ZonedDateTime> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code underlying} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ResolvedSwap> underlying()
		{
		  return underlying_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return ((ResolvedSwaption) bean).LongShort;
			case -1937554512: // swaptionSettlement
			  return ((ResolvedSwaption) bean).SwaptionSettlement;
			case -1289159373: // expiry
			  return ((ResolvedSwaption) bean).Expiry;
			case -1770633379: // underlying
			  return ((ResolvedSwaption) bean).Underlying;
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
	  /// The bean-builder for {@code ResolvedSwaption}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedSwaption>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LongShort longShort_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SwaptionSettlement swaptionSettlement_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ZonedDateTime expiry_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ResolvedSwap underlying_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedSwaption beanToCopy)
		{
		  this.longShort_Renamed = beanToCopy.LongShort;
		  this.swaptionSettlement_Renamed = beanToCopy.SwaptionSettlement;
		  this.expiry_Renamed = beanToCopy.Expiry;
		  this.underlying_Renamed = beanToCopy.Underlying;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  return longShort_Renamed;
			case -1937554512: // swaptionSettlement
			  return swaptionSettlement_Renamed;
			case -1289159373: // expiry
			  return expiry_Renamed;
			case -1770633379: // underlying
			  return underlying_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 116685664: // longShort
			  this.longShort_Renamed = (LongShort) newValue;
			  break;
			case -1937554512: // swaptionSettlement
			  this.swaptionSettlement_Renamed = (SwaptionSettlement) newValue;
			  break;
			case -1289159373: // expiry
			  this.expiry_Renamed = (ZonedDateTime) newValue;
			  break;
			case -1770633379: // underlying
			  this.underlying_Renamed = (ResolvedSwap) newValue;
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

		public override ResolvedSwaption build()
		{
		  return new ResolvedSwaption(longShort_Renamed, swaptionSettlement_Renamed, expiry_Renamed, underlying_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the option is long or short.
		/// <para>
		/// Long indicates that the owner wants the option to be in the money at expiry.
		/// Short indicates that the owner wants the option to be out of the money at expiry.
		/// </para>
		/// </summary>
		/// <param name="longShort">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder longShort(LongShort longShort)
		{
		  JodaBeanUtils.notNull(longShort, "longShort");
		  this.longShort_Renamed = longShort;
		  return this;
		}

		/// <summary>
		/// Sets settlement method.
		/// <para>
		/// The settlement of the option is specified by <seealso cref="SwaptionSettlement"/>.
		/// </para>
		/// </summary>
		/// <param name="swaptionSettlement">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder swaptionSettlement(SwaptionSettlement swaptionSettlement)
		{
		  JodaBeanUtils.notNull(swaptionSettlement, "swaptionSettlement");
		  this.swaptionSettlement_Renamed = swaptionSettlement;
		  return this;
		}

		/// <summary>
		/// Sets the expiry date-time of the option.
		/// <para>
		/// The option is European, and can only be exercised on the expiry date.
		/// </para>
		/// </summary>
		/// <param name="expiry">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder expiry(ZonedDateTime expiry)
		{
		  JodaBeanUtils.notNull(expiry, "expiry");
		  this.expiry_Renamed = expiry;
		  return this;
		}

		/// <summary>
		/// Sets the underlying swap.
		/// <para>
		/// At expiry, if the option is exercised, this swap will be entered into. The swap description is the swap
		/// as viewed by the party long the option.
		/// </para>
		/// </summary>
		/// <param name="underlying">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder underlying(ResolvedSwap underlying)
		{
		  JodaBeanUtils.notNull(underlying, "underlying");
		  this.underlying_Renamed = underlying;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("ResolvedSwaption.Builder{");
		  buf.Append("longShort").Append('=').Append(JodaBeanUtils.ToString(longShort_Renamed)).Append(',').Append(' ');
		  buf.Append("swaptionSettlement").Append('=').Append(JodaBeanUtils.ToString(swaptionSettlement_Renamed)).Append(',').Append(' ');
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry_Renamed)).Append(',').Append(' ');
		  buf.Append("underlying").Append('=').Append(JodaBeanUtils.ToString(underlying_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}