using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.bond
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A futures contract based on a basket of fixed coupon bonds, resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="BondFuture"/> and is an input to the pricers.
	/// Applications will typically create a {@code ResolvedBondFuture} from a {@code BondFuture}
	/// using <seealso cref="BondFuture#resolve(ReferenceData)"/>.
	/// </para>
	/// <para>
	/// A {@code ResolvedBondFuture} is bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="FixedCouponBond"/>. The bond futures delivery is a bond
	/// for an amount computed from the bond future price, a conversion factor and the accrued interest.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @BeanDefinition(constructorScope = "package") public final class ResolvedBondFuture implements com.opengamma.strata.product.ResolvedProduct, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ResolvedBondFuture : ResolvedProduct, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.SecurityId securityId;
		private readonly SecurityId securityId;
	  /// <summary>
	  /// The basket of deliverable bonds.
	  /// <para>
	  /// The underling which will be delivered in the future time is chosen from
	  /// a basket of underling securities. This must not be empty.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<ResolvedFixedCouponBond> deliveryBasket;
	  private readonly ImmutableList<ResolvedFixedCouponBond> deliveryBasket;
	  /// <summary>
	  /// The conversion factor for each bond in the basket.
	  /// <para>
	  /// The price of each underlying security in the basket is rescaled by the conversion factor.
	  /// This must not be empty, and its size must be the same as the size of {@code deliveryBasket}.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<double> conversionFactors;
	  private readonly ImmutableList<double> conversionFactors;
	  /// <summary>
	  /// The last trading date.
	  /// <para>
	  /// The future security is traded until this date.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastTradeDate;
	  private readonly LocalDate lastTradeDate;
	  /// <summary>
	  /// The first notice date.
	  /// <para>
	  /// The first date on which the delivery of the underlying is authorized.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate firstNoticeDate;
	  private readonly LocalDate firstNoticeDate;
	  /// <summary>
	  /// The last notice date.
	  /// <para>
	  /// The last date on which the delivery of the underlying is authorized.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastNoticeDate;
	  private readonly LocalDate lastNoticeDate;
	  /// <summary>
	  /// The first delivery date.
	  /// <para>
	  /// The first date on which the underlying is delivered.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate firstDeliveryDate;
	  private readonly LocalDate firstDeliveryDate;
	  /// <summary>
	  /// The last delivery date.
	  /// <para>
	  /// The last date on which the underlying is delivered.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate lastDeliveryDate;
	  private readonly LocalDate lastDeliveryDate;
	  /// <summary>
	  /// The definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.value.Rounding rounding;
	  private readonly Rounding rounding;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.rounding(Rounding.none());
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		int size = deliveryBasket.size();
		ArgChecker.isTrue(size == conversionFactors.size(), "The delivery basket size should be the same as the conversion factor size");
		ArgChecker.inOrderOrEqual(firstNoticeDate, lastNoticeDate, "firstNoticeDate", "lastNoticeDate");
		ArgChecker.inOrderOrEqual(firstDeliveryDate, lastDeliveryDate, "firstDeliveryDate", "lastDeliveryDate");
		ArgChecker.inOrderOrEqual(firstNoticeDate, firstDeliveryDate, "firstNoticeDate", "firstDeliveryDate");
		ArgChecker.inOrderOrEqual(lastNoticeDate, lastDeliveryDate, "lastNoticeDate", "lastDeliveryDate");
		if (size > 1)
		{
		  double notional = Notional;
		  Currency currency = Currency;
		  for (int i = 1; i < size; ++i)
		  {
			ArgChecker.isTrue(deliveryBasket.get(i).Notional == notional);
			ArgChecker.isTrue(deliveryBasket.get(i).Currency.Equals(currency));
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the currency of the underlying fixed coupon bonds.
	  /// <para>
	  /// All of the bonds in the delivery basket have the same currency.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency </returns>
	  public Currency Currency
	  {
		  get
		  {
			return deliveryBasket.get(0).Currency;
		  }
	  }

	  /// <summary>
	  /// Obtains the notional of underlying fixed coupon bonds.
	  /// <para>
	  /// All of the bonds in the delivery basket have the same notional.
	  /// The currency of the notional is specified by <seealso cref="#getCurrency()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the notional </returns>
	  public double Notional
	  {
		  get
		  {
			return deliveryBasket.get(0).Notional;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBondFuture}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ResolvedBondFuture.Meta meta()
	  {
		return ResolvedBondFuture.Meta.INSTANCE;
	  }

	  static ResolvedBondFuture()
	  {
		MetaBean.register(ResolvedBondFuture.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ResolvedBondFuture.Builder builder()
	  {
		return new ResolvedBondFuture.Builder();
	  }

	  /// <summary>
	  /// Creates an instance. </summary>
	  /// <param name="securityId">  the value of the property, not null </param>
	  /// <param name="deliveryBasket">  the value of the property, not empty </param>
	  /// <param name="conversionFactors">  the value of the property, not empty </param>
	  /// <param name="lastTradeDate">  the value of the property, not null </param>
	  /// <param name="firstNoticeDate">  the value of the property, not null </param>
	  /// <param name="lastNoticeDate">  the value of the property, not null </param>
	  /// <param name="firstDeliveryDate">  the value of the property, not null </param>
	  /// <param name="lastDeliveryDate">  the value of the property, not null </param>
	  /// <param name="rounding">  the value of the property, not null </param>
	  internal ResolvedBondFuture(SecurityId securityId, IList<ResolvedFixedCouponBond> deliveryBasket, IList<double> conversionFactors, LocalDate lastTradeDate, LocalDate firstNoticeDate, LocalDate lastNoticeDate, LocalDate firstDeliveryDate, LocalDate lastDeliveryDate, Rounding rounding)
	  {
		JodaBeanUtils.notNull(securityId, "securityId");
		JodaBeanUtils.notEmpty(deliveryBasket, "deliveryBasket");
		JodaBeanUtils.notEmpty(conversionFactors, "conversionFactors");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(firstNoticeDate, "firstNoticeDate");
		JodaBeanUtils.notNull(lastNoticeDate, "lastNoticeDate");
		JodaBeanUtils.notNull(firstDeliveryDate, "firstDeliveryDate");
		JodaBeanUtils.notNull(lastDeliveryDate, "lastDeliveryDate");
		JodaBeanUtils.notNull(rounding, "rounding");
		this.securityId = securityId;
		this.deliveryBasket = ImmutableList.copyOf(deliveryBasket);
		this.conversionFactors = ImmutableList.copyOf(conversionFactors);
		this.lastTradeDate = lastTradeDate;
		this.firstNoticeDate = firstNoticeDate;
		this.lastNoticeDate = lastNoticeDate;
		this.firstDeliveryDate = firstDeliveryDate;
		this.lastDeliveryDate = lastDeliveryDate;
		this.rounding = rounding;
		validate();
	  }

	  public override ResolvedBondFuture.Meta metaBean()
	  {
		return ResolvedBondFuture.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the security identifier.
	  /// <para>
	  /// This identifier uniquely identifies the security within the system.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityId SecurityId
	  {
		  get
		  {
			return securityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the basket of deliverable bonds.
	  /// <para>
	  /// The underling which will be delivered in the future time is chosen from
	  /// a basket of underling securities. This must not be empty.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<ResolvedFixedCouponBond> DeliveryBasket
	  {
		  get
		  {
			return deliveryBasket;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the conversion factor for each bond in the basket.
	  /// <para>
	  /// The price of each underlying security in the basket is rescaled by the conversion factor.
	  /// This must not be empty, and its size must be the same as the size of {@code deliveryBasket}.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<double> ConversionFactors
	  {
		  get
		  {
			return conversionFactors;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last trading date.
	  /// <para>
	  /// The future security is traded until this date.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastTradeDate
	  {
		  get
		  {
			return lastTradeDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first notice date.
	  /// <para>
	  /// The first date on which the delivery of the underlying is authorized.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FirstNoticeDate
	  {
		  get
		  {
			return firstNoticeDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last notice date.
	  /// <para>
	  /// The last date on which the delivery of the underlying is authorized.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastNoticeDate
	  {
		  get
		  {
			return lastNoticeDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first delivery date.
	  /// <para>
	  /// The first date on which the underlying is delivered.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate FirstDeliveryDate
	  {
		  get
		  {
			return firstDeliveryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last delivery date.
	  /// <para>
	  /// The last date on which the underlying is delivered.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate LastDeliveryDate
	  {
		  get
		  {
			return lastDeliveryDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the definition of how to round the futures price, defaulted to no rounding.
	  /// <para>
	  /// The price is represented in decimal form, not percentage form.
	  /// As such, the decimal places expressed by the rounding refers to this decimal form.
	  /// For example, the common market price of 99.7125 for a 0.2875% rate is
	  /// represented as 0.997125 which has 6 decimal places.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Rounding Rounding
	  {
		  get
		  {
			return rounding;
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
		  ResolvedBondFuture other = (ResolvedBondFuture) obj;
		  return JodaBeanUtils.equal(securityId, other.securityId) && JodaBeanUtils.equal(deliveryBasket, other.deliveryBasket) && JodaBeanUtils.equal(conversionFactors, other.conversionFactors) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(firstNoticeDate, other.firstNoticeDate) && JodaBeanUtils.equal(lastNoticeDate, other.lastNoticeDate) && JodaBeanUtils.equal(firstDeliveryDate, other.firstDeliveryDate) && JodaBeanUtils.equal(lastDeliveryDate, other.lastDeliveryDate) && JodaBeanUtils.equal(rounding, other.rounding);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(securityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(deliveryBasket);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(conversionFactors);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastTradeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstNoticeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastNoticeDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(firstDeliveryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(lastDeliveryDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rounding);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(320);
		buf.Append("ResolvedBondFuture{");
		buf.Append("securityId").Append('=').Append(securityId).Append(',').Append(' ');
		buf.Append("deliveryBasket").Append('=').Append(deliveryBasket).Append(',').Append(' ');
		buf.Append("conversionFactors").Append('=').Append(conversionFactors).Append(',').Append(' ');
		buf.Append("lastTradeDate").Append('=').Append(lastTradeDate).Append(',').Append(' ');
		buf.Append("firstNoticeDate").Append('=').Append(firstNoticeDate).Append(',').Append(' ');
		buf.Append("lastNoticeDate").Append('=').Append(lastNoticeDate).Append(',').Append(' ');
		buf.Append("firstDeliveryDate").Append('=').Append(firstDeliveryDate).Append(',').Append(' ');
		buf.Append("lastDeliveryDate").Append('=').Append(lastDeliveryDate).Append(',').Append(' ');
		buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ResolvedBondFuture}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  securityId_Renamed = DirectMetaProperty.ofImmutable(this, "securityId", typeof(ResolvedBondFuture), typeof(SecurityId));
			  deliveryBasket_Renamed = DirectMetaProperty.ofImmutable(this, "deliveryBasket", typeof(ResolvedBondFuture), (Type) typeof(ImmutableList));
			  conversionFactors_Renamed = DirectMetaProperty.ofImmutable(this, "conversionFactors", typeof(ResolvedBondFuture), (Type) typeof(ImmutableList));
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(ResolvedBondFuture), typeof(LocalDate));
			  firstNoticeDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstNoticeDate", typeof(ResolvedBondFuture), typeof(LocalDate));
			  lastNoticeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastNoticeDate", typeof(ResolvedBondFuture), typeof(LocalDate));
			  firstDeliveryDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstDeliveryDate", typeof(ResolvedBondFuture), typeof(LocalDate));
			  lastDeliveryDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastDeliveryDate", typeof(ResolvedBondFuture), typeof(LocalDate));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(ResolvedBondFuture), typeof(Rounding));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "securityId", "deliveryBasket", "conversionFactors", "lastTradeDate", "firstNoticeDate", "lastNoticeDate", "firstDeliveryDate", "lastDeliveryDate", "rounding");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code securityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityId> securityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code deliveryBasket} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<ResolvedFixedCouponBond>> deliveryBasket = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "deliveryBasket", ResolvedBondFuture.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ResolvedFixedCouponBond>> deliveryBasket_Renamed;
		/// <summary>
		/// The meta-property for the {@code conversionFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<double>> conversionFactors = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "conversionFactors", ResolvedBondFuture.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<double>> conversionFactors_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastTradeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstNoticeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> firstNoticeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastNoticeDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastNoticeDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code firstDeliveryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> firstDeliveryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code lastDeliveryDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> lastDeliveryDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code rounding} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Rounding> rounding_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "securityId", "deliveryBasket", "conversionFactors", "lastTradeDate", "firstNoticeDate", "lastNoticeDate", "firstDeliveryDate", "lastDeliveryDate", "rounding");
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
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1999764186: // deliveryBasket
			  return deliveryBasket_Renamed;
			case 1655488270: // conversionFactors
			  return conversionFactors_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -1085415050: // firstNoticeDate
			  return firstNoticeDate_Renamed;
			case -1060668964: // lastNoticeDate
			  return lastNoticeDate_Renamed;
			case 1755448466: // firstDeliveryDate
			  return firstDeliveryDate_Renamed;
			case -233366664: // lastDeliveryDate
			  return lastDeliveryDate_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ResolvedBondFuture.Builder builder()
		{
		  return new ResolvedBondFuture.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ResolvedBondFuture);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code securityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityId> securityId()
		{
		  return securityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code deliveryBasket} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ResolvedFixedCouponBond>> deliveryBasket()
		{
		  return deliveryBasket_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code conversionFactors} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<double>> conversionFactors()
		{
		  return conversionFactors_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastTradeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastTradeDate()
		{
		  return lastTradeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstNoticeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> firstNoticeDate()
		{
		  return firstNoticeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastNoticeDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastNoticeDate()
		{
		  return lastNoticeDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code firstDeliveryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> firstDeliveryDate()
		{
		  return firstDeliveryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code lastDeliveryDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> lastDeliveryDate()
		{
		  return lastDeliveryDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rounding} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Rounding> rounding()
		{
		  return rounding_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return ((ResolvedBondFuture) bean).SecurityId;
			case 1999764186: // deliveryBasket
			  return ((ResolvedBondFuture) bean).DeliveryBasket;
			case 1655488270: // conversionFactors
			  return ((ResolvedBondFuture) bean).ConversionFactors;
			case -1041950404: // lastTradeDate
			  return ((ResolvedBondFuture) bean).LastTradeDate;
			case -1085415050: // firstNoticeDate
			  return ((ResolvedBondFuture) bean).FirstNoticeDate;
			case -1060668964: // lastNoticeDate
			  return ((ResolvedBondFuture) bean).LastNoticeDate;
			case 1755448466: // firstDeliveryDate
			  return ((ResolvedBondFuture) bean).FirstDeliveryDate;
			case -233366664: // lastDeliveryDate
			  return ((ResolvedBondFuture) bean).LastDeliveryDate;
			case -142444: // rounding
			  return ((ResolvedBondFuture) bean).Rounding;
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
	  /// The bean-builder for {@code ResolvedBondFuture}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ResolvedBondFuture>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityId securityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<ResolvedFixedCouponBond> deliveryBasket_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<double> conversionFactors_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastTradeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate firstNoticeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastNoticeDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate firstDeliveryDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate lastDeliveryDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Rounding rounding_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		  applyDefaults(this);
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ResolvedBondFuture beanToCopy)
		{
		  this.securityId_Renamed = beanToCopy.SecurityId;
		  this.deliveryBasket_Renamed = beanToCopy.DeliveryBasket;
		  this.conversionFactors_Renamed = beanToCopy.ConversionFactors;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.firstNoticeDate_Renamed = beanToCopy.FirstNoticeDate;
		  this.lastNoticeDate_Renamed = beanToCopy.LastNoticeDate;
		  this.firstDeliveryDate_Renamed = beanToCopy.FirstDeliveryDate;
		  this.lastDeliveryDate_Renamed = beanToCopy.LastDeliveryDate;
		  this.rounding_Renamed = beanToCopy.Rounding;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  return securityId_Renamed;
			case 1999764186: // deliveryBasket
			  return deliveryBasket_Renamed;
			case 1655488270: // conversionFactors
			  return conversionFactors_Renamed;
			case -1041950404: // lastTradeDate
			  return lastTradeDate_Renamed;
			case -1085415050: // firstNoticeDate
			  return firstNoticeDate_Renamed;
			case -1060668964: // lastNoticeDate
			  return lastNoticeDate_Renamed;
			case 1755448466: // firstDeliveryDate
			  return firstDeliveryDate_Renamed;
			case -233366664: // lastDeliveryDate
			  return lastDeliveryDate_Renamed;
			case -142444: // rounding
			  return rounding_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1574023291: // securityId
			  this.securityId_Renamed = (SecurityId) newValue;
			  break;
			case 1999764186: // deliveryBasket
			  this.deliveryBasket_Renamed = (IList<ResolvedFixedCouponBond>) newValue;
			  break;
			case 1655488270: // conversionFactors
			  this.conversionFactors_Renamed = (IList<double>) newValue;
			  break;
			case -1041950404: // lastTradeDate
			  this.lastTradeDate_Renamed = (LocalDate) newValue;
			  break;
			case -1085415050: // firstNoticeDate
			  this.firstNoticeDate_Renamed = (LocalDate) newValue;
			  break;
			case -1060668964: // lastNoticeDate
			  this.lastNoticeDate_Renamed = (LocalDate) newValue;
			  break;
			case 1755448466: // firstDeliveryDate
			  this.firstDeliveryDate_Renamed = (LocalDate) newValue;
			  break;
			case -233366664: // lastDeliveryDate
			  this.lastDeliveryDate_Renamed = (LocalDate) newValue;
			  break;
			case -142444: // rounding
			  this.rounding_Renamed = (Rounding) newValue;
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

		public override ResolvedBondFuture build()
		{
		  return new ResolvedBondFuture(securityId_Renamed, deliveryBasket_Renamed, conversionFactors_Renamed, lastTradeDate_Renamed, firstNoticeDate_Renamed, lastNoticeDate_Renamed, firstDeliveryDate_Renamed, lastDeliveryDate_Renamed, rounding_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the security identifier.
		/// <para>
		/// This identifier uniquely identifies the security within the system.
		/// </para>
		/// </summary>
		/// <param name="securityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder securityId(SecurityId securityId)
		{
		  JodaBeanUtils.notNull(securityId, "securityId");
		  this.securityId_Renamed = securityId;
		  return this;
		}

		/// <summary>
		/// Sets the basket of deliverable bonds.
		/// <para>
		/// The underling which will be delivered in the future time is chosen from
		/// a basket of underling securities. This must not be empty.
		/// </para>
		/// <para>
		/// All of the underlying bonds must have the same notional and currency.
		/// </para>
		/// </summary>
		/// <param name="deliveryBasket">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deliveryBasket(IList<ResolvedFixedCouponBond> deliveryBasket)
		{
		  JodaBeanUtils.notEmpty(deliveryBasket, "deliveryBasket");
		  this.deliveryBasket_Renamed = deliveryBasket;
		  return this;
		}

		/// <summary>
		/// Sets the {@code deliveryBasket} property in the builder
		/// from an array of objects. </summary>
		/// <param name="deliveryBasket">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deliveryBasket(params ResolvedFixedCouponBond[] deliveryBasket)
		{
		  return this.deliveryBasket(ImmutableList.copyOf(deliveryBasket));
		}

		/// <summary>
		/// Sets the conversion factor for each bond in the basket.
		/// <para>
		/// The price of each underlying security in the basket is rescaled by the conversion factor.
		/// This must not be empty, and its size must be the same as the size of {@code deliveryBasket}.
		/// </para>
		/// <para>
		/// All of the underlying bonds must have the same notional and currency.
		/// </para>
		/// </summary>
		/// <param name="conversionFactors">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder conversionFactors(IList<double> conversionFactors)
		{
		  JodaBeanUtils.notEmpty(conversionFactors, "conversionFactors");
		  this.conversionFactors_Renamed = conversionFactors;
		  return this;
		}

		/// <summary>
		/// Sets the {@code conversionFactors} property in the builder
		/// from an array of objects. </summary>
		/// <param name="conversionFactors">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder conversionFactors(params Double[] conversionFactors)
		{
		  return this.conversionFactors(ImmutableList.copyOf(conversionFactors));
		}

		/// <summary>
		/// Sets the last trading date.
		/// <para>
		/// The future security is traded until this date.
		/// </para>
		/// </summary>
		/// <param name="lastTradeDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastTradeDate(LocalDate lastTradeDate)
		{
		  JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		  this.lastTradeDate_Renamed = lastTradeDate;
		  return this;
		}

		/// <summary>
		/// Sets the first notice date.
		/// <para>
		/// The first date on which the delivery of the underlying is authorized.
		/// </para>
		/// </summary>
		/// <param name="firstNoticeDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstNoticeDate(LocalDate firstNoticeDate)
		{
		  JodaBeanUtils.notNull(firstNoticeDate, "firstNoticeDate");
		  this.firstNoticeDate_Renamed = firstNoticeDate;
		  return this;
		}

		/// <summary>
		/// Sets the last notice date.
		/// <para>
		/// The last date on which the delivery of the underlying is authorized.
		/// </para>
		/// </summary>
		/// <param name="lastNoticeDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastNoticeDate(LocalDate lastNoticeDate)
		{
		  JodaBeanUtils.notNull(lastNoticeDate, "lastNoticeDate");
		  this.lastNoticeDate_Renamed = lastNoticeDate;
		  return this;
		}

		/// <summary>
		/// Sets the first delivery date.
		/// <para>
		/// The first date on which the underlying is delivered.
		/// </para>
		/// </summary>
		/// <param name="firstDeliveryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstDeliveryDate(LocalDate firstDeliveryDate)
		{
		  JodaBeanUtils.notNull(firstDeliveryDate, "firstDeliveryDate");
		  this.firstDeliveryDate_Renamed = firstDeliveryDate;
		  return this;
		}

		/// <summary>
		/// Sets the last delivery date.
		/// <para>
		/// The last date on which the underlying is delivered.
		/// </para>
		/// </summary>
		/// <param name="lastDeliveryDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastDeliveryDate(LocalDate lastDeliveryDate)
		{
		  JodaBeanUtils.notNull(lastDeliveryDate, "lastDeliveryDate");
		  this.lastDeliveryDate_Renamed = lastDeliveryDate;
		  return this;
		}

		/// <summary>
		/// Sets the definition of how to round the futures price, defaulted to no rounding.
		/// <para>
		/// The price is represented in decimal form, not percentage form.
		/// As such, the decimal places expressed by the rounding refers to this decimal form.
		/// For example, the common market price of 99.7125 for a 0.2875% rate is
		/// represented as 0.997125 which has 6 decimal places.
		/// </para>
		/// </summary>
		/// <param name="rounding">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rounding(Rounding rounding)
		{
		  JodaBeanUtils.notNull(rounding, "rounding");
		  this.rounding_Renamed = rounding;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("ResolvedBondFuture.Builder{");
		  buf.Append("securityId").Append('=').Append(JodaBeanUtils.ToString(securityId_Renamed)).Append(',').Append(' ');
		  buf.Append("deliveryBasket").Append('=').Append(JodaBeanUtils.ToString(deliveryBasket_Renamed)).Append(',').Append(' ');
		  buf.Append("conversionFactors").Append('=').Append(JodaBeanUtils.ToString(conversionFactors_Renamed)).Append(',').Append(' ');
		  buf.Append("lastTradeDate").Append('=').Append(JodaBeanUtils.ToString(lastTradeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("firstNoticeDate").Append('=').Append(JodaBeanUtils.ToString(firstNoticeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("lastNoticeDate").Append('=').Append(JodaBeanUtils.ToString(lastNoticeDate_Renamed)).Append(',').Append(' ');
		  buf.Append("firstDeliveryDate").Append('=').Append(JodaBeanUtils.ToString(firstDeliveryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("lastDeliveryDate").Append('=').Append(JodaBeanUtils.ToString(lastDeliveryDate_Renamed)).Append(',').Append(' ');
		  buf.Append("rounding").Append('=').Append(JodaBeanUtils.ToString(rounding_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}