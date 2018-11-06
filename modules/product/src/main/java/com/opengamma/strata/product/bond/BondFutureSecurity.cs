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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


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
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Rounding = com.opengamma.strata.basics.value.Rounding;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A security representing a futures contract, based on a basket of fixed coupon bonds.
	/// <para>
	/// A bond future is a financial instrument that is based on the future value of
	/// a basket of fixed coupon bonds. The profit or loss of a bond future is settled daily.
	/// 
	/// <h4>Price</h4>
	/// Strata uses <i>decimal prices</i> for bond futures in the trade model, pricers and market data.
	/// This is coherent with the pricing of <seealso cref="FixedCouponBond"/>. The bond futures delivery is a bond
	/// for an amount computed from the bond future price, a conversion factor and the accrued interest.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BondFutureSecurity implements com.opengamma.strata.product.Security, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BondFutureSecurity : Security, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.SecurityInfo info;
		private readonly SecurityInfo info;
	  /// <summary>
	  /// The currency that the future is traded in.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The basket of deliverable bonds.
	  /// <para>
	  /// The underlying which will be delivered in the future time is chosen from
	  /// a basket of underling securities. This must not be empty.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final com.google.common.collect.ImmutableList<com.opengamma.strata.product.SecurityId> deliveryBasketIds;
	  private readonly ImmutableList<SecurityId> deliveryBasketIds;
	  /// <summary>
	  /// The conversion factor for each bond in the basket.
	  /// <para>
	  /// The price of each underlying security in the basket is rescaled by the conversion factor.
	  /// This must not be empty, and its size must be the same as the size of {@code deliveryBasketIds}.
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
	  /// <para>
	  /// If not specified, the date will be computed from {@code firstNoticeDate} by using
	  /// {@code settlementDateOffset} in the first element of the delivery basket
	  /// when the future is resolved.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate firstDeliveryDate;
	  private readonly LocalDate firstDeliveryDate;
	  /// <summary>
	  /// The last delivery date.
	  /// <para>
	  /// The last date on which the underlying is delivered.
	  /// </para>
	  /// <para>
	  /// If not specified, the date will be computed from {@code lastNoticeDate} by using
	  /// {@code settlementDateOffset} in the first element of the delivery basket
	  /// when the future is resolved.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final java.time.LocalDate lastDeliveryDate;
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
		int size = deliveryBasketIds.size();
		ArgChecker.isTrue(size == conversionFactors.size(), "The delivery basket size should be the same as the conversion factor size");
		ArgChecker.inOrderOrEqual(firstNoticeDate, lastNoticeDate, "firstNoticeDate", "lastNoticeDate");
		if (firstDeliveryDate != null && lastDeliveryDate != null)
		{
		  ArgChecker.inOrderOrEqual(firstDeliveryDate, lastDeliveryDate, "firstDeliveryDate", "lastDeliveryDate");
		  ArgChecker.inOrderOrEqual(firstNoticeDate, firstDeliveryDate, "firstNoticeDate", "firstDeliveryDate");
		  ArgChecker.inOrderOrEqual(lastNoticeDate, lastDeliveryDate, "lastNoticeDate", "lastDeliveryDate");
		}
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<SecurityId> UnderlyingIds
	  {
		  get
		  {
			return ImmutableSet.copyOf(deliveryBasketIds);
		  }
	  }

	  //-------------------------------------------------------------------------
	  public BondFutureSecurity withInfo(SecurityInfo info)
	  {
		return toBuilder().info(info).build();
	  }

	  //-------------------------------------------------------------------------
	  public BondFuture createProduct(ReferenceData refData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<FixedCouponBond> bonds = deliveryBasketIds.Select(id => resolveBond(id, refData)).collect(toImmutableList());
		return new BondFuture(SecurityId, bonds, conversionFactors, lastTradeDate, firstNoticeDate, lastNoticeDate, firstDeliveryDate, lastDeliveryDate, rounding);
	  }

	  // resolve an underlying bond
	  private FixedCouponBond resolveBond(SecurityId id, ReferenceData refData)
	  {
		Security security = refData.getValue(id);
		if (!(security is FixedCouponBondSecurity))
		{
		  throw new System.InvalidCastException(Messages.format("{} underlying bond '{}' resolved to '{}' when '{}' was expected", typeof(BondFutureSecurity).Name, id, security.GetType().Name, typeof(FixedCouponBondSecurity).Name));
		}
		FixedCouponBondSecurity bondSec = (FixedCouponBondSecurity) security;
		return bondSec.createProduct(refData);
	  }

	  public BondFutureTrade createTrade(TradeInfo info, double quantity, double tradePrice, ReferenceData refData)
	  {
		BondFuture product = createProduct(refData);
		return new BondFutureTrade(info, product, quantity, tradePrice);
	  }

	  public BondFuturePosition createPosition(PositionInfo positionInfo, double quantity, ReferenceData refData)
	  {
		return BondFuturePosition.ofNet(positionInfo, createProduct(refData), quantity);
	  }

	  public BondFuturePosition createPosition(PositionInfo positionInfo, double longQuantity, double shortQuantity, ReferenceData refData)
	  {

		return BondFuturePosition.ofLongShort(positionInfo, createProduct(refData), longQuantity, shortQuantity);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BondFutureSecurity}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BondFutureSecurity.Meta meta()
	  {
		return BondFutureSecurity.Meta.INSTANCE;
	  }

	  static BondFutureSecurity()
	  {
		MetaBean.register(BondFutureSecurity.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BondFutureSecurity.Builder builder()
	  {
		return new BondFutureSecurity.Builder();
	  }

	  private BondFutureSecurity(SecurityInfo info, Currency currency, IList<SecurityId> deliveryBasketIds, IList<double> conversionFactors, LocalDate lastTradeDate, LocalDate firstNoticeDate, LocalDate lastNoticeDate, LocalDate firstDeliveryDate, LocalDate lastDeliveryDate, Rounding rounding)
	  {
		JodaBeanUtils.notNull(info, "info");
		JodaBeanUtils.notNull(currency, "currency");
		JodaBeanUtils.notEmpty(deliveryBasketIds, "deliveryBasketIds");
		JodaBeanUtils.notEmpty(conversionFactors, "conversionFactors");
		JodaBeanUtils.notNull(lastTradeDate, "lastTradeDate");
		JodaBeanUtils.notNull(firstNoticeDate, "firstNoticeDate");
		JodaBeanUtils.notNull(lastNoticeDate, "lastNoticeDate");
		JodaBeanUtils.notNull(rounding, "rounding");
		this.info = info;
		this.currency = currency;
		this.deliveryBasketIds = ImmutableList.copyOf(deliveryBasketIds);
		this.conversionFactors = ImmutableList.copyOf(conversionFactors);
		this.lastTradeDate = lastTradeDate;
		this.firstNoticeDate = firstNoticeDate;
		this.lastNoticeDate = lastNoticeDate;
		this.firstDeliveryDate = firstDeliveryDate;
		this.lastDeliveryDate = lastDeliveryDate;
		this.rounding = rounding;
		validate();
	  }

	  public override BondFutureSecurity.Meta metaBean()
	  {
		return BondFutureSecurity.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the standard security information.
	  /// <para>
	  /// This includes the security identifier.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SecurityInfo Info
	  {
		  get
		  {
			return info;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency that the future is traded in. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the basket of deliverable bonds.
	  /// <para>
	  /// The underlying which will be delivered in the future time is chosen from
	  /// a basket of underling securities. This must not be empty.
	  /// </para>
	  /// <para>
	  /// All of the underlying bonds must have the same notional and currency.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<SecurityId> DeliveryBasketIds
	  {
		  get
		  {
			return deliveryBasketIds;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the conversion factor for each bond in the basket.
	  /// <para>
	  /// The price of each underlying security in the basket is rescaled by the conversion factor.
	  /// This must not be empty, and its size must be the same as the size of {@code deliveryBasketIds}.
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
	  /// <para>
	  /// If not specified, the date will be computed from {@code firstNoticeDate} by using
	  /// {@code settlementDateOffset} in the first element of the delivery basket
	  /// when the future is resolved.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> FirstDeliveryDate
	  {
		  get
		  {
			return Optional.ofNullable(firstDeliveryDate);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the last delivery date.
	  /// <para>
	  /// The last date on which the underlying is delivered.
	  /// </para>
	  /// <para>
	  /// If not specified, the date will be computed from {@code lastNoticeDate} by using
	  /// {@code settlementDateOffset} in the first element of the delivery basket
	  /// when the future is resolved.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<LocalDate> LastDeliveryDate
	  {
		  get
		  {
			return Optional.ofNullable(lastDeliveryDate);
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
		  BondFutureSecurity other = (BondFutureSecurity) obj;
		  return JodaBeanUtils.equal(info, other.info) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(deliveryBasketIds, other.deliveryBasketIds) && JodaBeanUtils.equal(conversionFactors, other.conversionFactors) && JodaBeanUtils.equal(lastTradeDate, other.lastTradeDate) && JodaBeanUtils.equal(firstNoticeDate, other.firstNoticeDate) && JodaBeanUtils.equal(lastNoticeDate, other.lastNoticeDate) && JodaBeanUtils.equal(firstDeliveryDate, other.firstDeliveryDate) && JodaBeanUtils.equal(lastDeliveryDate, other.lastDeliveryDate) && JodaBeanUtils.equal(rounding, other.rounding);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(info);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(deliveryBasketIds);
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
		StringBuilder buf = new StringBuilder(352);
		buf.Append("BondFutureSecurity{");
		buf.Append("info").Append('=').Append(info).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("deliveryBasketIds").Append('=').Append(deliveryBasketIds).Append(',').Append(' ');
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
	  /// The meta-bean for {@code BondFutureSecurity}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  info_Renamed = DirectMetaProperty.ofImmutable(this, "info", typeof(BondFutureSecurity), typeof(SecurityInfo));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(BondFutureSecurity), typeof(Currency));
			  deliveryBasketIds_Renamed = DirectMetaProperty.ofImmutable(this, "deliveryBasketIds", typeof(BondFutureSecurity), (Type) typeof(ImmutableList));
			  conversionFactors_Renamed = DirectMetaProperty.ofImmutable(this, "conversionFactors", typeof(BondFutureSecurity), (Type) typeof(ImmutableList));
			  lastTradeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastTradeDate", typeof(BondFutureSecurity), typeof(LocalDate));
			  firstNoticeDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstNoticeDate", typeof(BondFutureSecurity), typeof(LocalDate));
			  lastNoticeDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastNoticeDate", typeof(BondFutureSecurity), typeof(LocalDate));
			  firstDeliveryDate_Renamed = DirectMetaProperty.ofImmutable(this, "firstDeliveryDate", typeof(BondFutureSecurity), typeof(LocalDate));
			  lastDeliveryDate_Renamed = DirectMetaProperty.ofImmutable(this, "lastDeliveryDate", typeof(BondFutureSecurity), typeof(LocalDate));
			  rounding_Renamed = DirectMetaProperty.ofImmutable(this, "rounding", typeof(BondFutureSecurity), typeof(Rounding));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "info", "currency", "deliveryBasketIds", "conversionFactors", "lastTradeDate", "firstNoticeDate", "lastNoticeDate", "firstDeliveryDate", "lastDeliveryDate", "rounding");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code info} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SecurityInfo> info_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code deliveryBasketIds} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.product.SecurityId>> deliveryBasketIds = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "deliveryBasketIds", BondFutureSecurity.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SecurityId>> deliveryBasketIds_Renamed;
		/// <summary>
		/// The meta-property for the {@code conversionFactors} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<double>> conversionFactors = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "conversionFactors", BondFutureSecurity.class, (Class) com.google.common.collect.ImmutableList.class);
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "info", "currency", "deliveryBasketIds", "conversionFactors", "lastTradeDate", "firstNoticeDate", "lastNoticeDate", "firstDeliveryDate", "lastDeliveryDate", "rounding");
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
			case 3237038: // info
			  return info_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -516424322: // deliveryBasketIds
			  return deliveryBasketIds_Renamed;
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

		public override BondFutureSecurity.Builder builder()
		{
		  return new BondFutureSecurity.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BondFutureSecurity);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code info} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SecurityInfo> info()
		{
		  return info_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code deliveryBasketIds} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SecurityId>> deliveryBasketIds()
		{
		  return deliveryBasketIds_Renamed;
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
			case 3237038: // info
			  return ((BondFutureSecurity) bean).Info;
			case 575402001: // currency
			  return ((BondFutureSecurity) bean).Currency;
			case -516424322: // deliveryBasketIds
			  return ((BondFutureSecurity) bean).DeliveryBasketIds;
			case 1655488270: // conversionFactors
			  return ((BondFutureSecurity) bean).ConversionFactors;
			case -1041950404: // lastTradeDate
			  return ((BondFutureSecurity) bean).LastTradeDate;
			case -1085415050: // firstNoticeDate
			  return ((BondFutureSecurity) bean).FirstNoticeDate;
			case -1060668964: // lastNoticeDate
			  return ((BondFutureSecurity) bean).LastNoticeDate;
			case 1755448466: // firstDeliveryDate
			  return ((BondFutureSecurity) bean).firstDeliveryDate;
			case -233366664: // lastDeliveryDate
			  return ((BondFutureSecurity) bean).lastDeliveryDate;
			case -142444: // rounding
			  return ((BondFutureSecurity) bean).Rounding;
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
	  /// The bean-builder for {@code BondFutureSecurity}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BondFutureSecurity>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SecurityInfo info_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<SecurityId> deliveryBasketIds_Renamed = ImmutableList.of();
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
		internal Builder(BondFutureSecurity beanToCopy)
		{
		  this.info_Renamed = beanToCopy.Info;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.deliveryBasketIds_Renamed = beanToCopy.DeliveryBasketIds;
		  this.conversionFactors_Renamed = beanToCopy.ConversionFactors;
		  this.lastTradeDate_Renamed = beanToCopy.LastTradeDate;
		  this.firstNoticeDate_Renamed = beanToCopy.FirstNoticeDate;
		  this.lastNoticeDate_Renamed = beanToCopy.LastNoticeDate;
		  this.firstDeliveryDate_Renamed = beanToCopy.firstDeliveryDate;
		  this.lastDeliveryDate_Renamed = beanToCopy.lastDeliveryDate;
		  this.rounding_Renamed = beanToCopy.Rounding;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3237038: // info
			  return info_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case -516424322: // deliveryBasketIds
			  return deliveryBasketIds_Renamed;
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
			case 3237038: // info
			  this.info_Renamed = (SecurityInfo) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case -516424322: // deliveryBasketIds
			  this.deliveryBasketIds_Renamed = (IList<SecurityId>) newValue;
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

		public override BondFutureSecurity build()
		{
		  return new BondFutureSecurity(info_Renamed, currency_Renamed, deliveryBasketIds_Renamed, conversionFactors_Renamed, lastTradeDate_Renamed, firstNoticeDate_Renamed, lastNoticeDate_Renamed, firstDeliveryDate_Renamed, lastDeliveryDate_Renamed, rounding_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the standard security information.
		/// <para>
		/// This includes the security identifier.
		/// </para>
		/// </summary>
		/// <param name="info">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder info(SecurityInfo info)
		{
		  JodaBeanUtils.notNull(info, "info");
		  this.info_Renamed = info;
		  return this;
		}

		/// <summary>
		/// Sets the currency that the future is traded in. </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the basket of deliverable bonds.
		/// <para>
		/// The underlying which will be delivered in the future time is chosen from
		/// a basket of underling securities. This must not be empty.
		/// </para>
		/// <para>
		/// All of the underlying bonds must have the same notional and currency.
		/// </para>
		/// </summary>
		/// <param name="deliveryBasketIds">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deliveryBasketIds(IList<SecurityId> deliveryBasketIds)
		{
		  JodaBeanUtils.notEmpty(deliveryBasketIds, "deliveryBasketIds");
		  this.deliveryBasketIds_Renamed = deliveryBasketIds;
		  return this;
		}

		/// <summary>
		/// Sets the {@code deliveryBasketIds} property in the builder
		/// from an array of objects. </summary>
		/// <param name="deliveryBasketIds">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder deliveryBasketIds(params SecurityId[] deliveryBasketIds)
		{
		  return this.deliveryBasketIds(ImmutableList.copyOf(deliveryBasketIds));
		}

		/// <summary>
		/// Sets the conversion factor for each bond in the basket.
		/// <para>
		/// The price of each underlying security in the basket is rescaled by the conversion factor.
		/// This must not be empty, and its size must be the same as the size of {@code deliveryBasketIds}.
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
		/// <para>
		/// If not specified, the date will be computed from {@code firstNoticeDate} by using
		/// {@code settlementDateOffset} in the first element of the delivery basket
		/// when the future is resolved.
		/// </para>
		/// </summary>
		/// <param name="firstDeliveryDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder firstDeliveryDate(LocalDate firstDeliveryDate)
		{
		  this.firstDeliveryDate_Renamed = firstDeliveryDate;
		  return this;
		}

		/// <summary>
		/// Sets the last delivery date.
		/// <para>
		/// The last date on which the underlying is delivered.
		/// </para>
		/// <para>
		/// If not specified, the date will be computed from {@code lastNoticeDate} by using
		/// {@code settlementDateOffset} in the first element of the delivery basket
		/// when the future is resolved.
		/// </para>
		/// </summary>
		/// <param name="lastDeliveryDate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder lastDeliveryDate(LocalDate lastDeliveryDate)
		{
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
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("BondFutureSecurity.Builder{");
		  buf.Append("info").Append('=').Append(JodaBeanUtils.ToString(info_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("deliveryBasketIds").Append('=').Append(JodaBeanUtils.ToString(deliveryBasketIds_Renamed)).Append(',').Append(' ');
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