using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.BusinessDayConventions.MODIFIED_FOLLOWING;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.AFMA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.product.fra.FraDiscountingMethod.ISDA;


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

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;

	/// <summary>
	/// A market convention for forward rate agreement (FRA) trades.
	/// <para>
	/// This defines the market convention for a FRA against a particular index.
	/// In most cases, the index contains sufficient information to fully define the convention.
	/// As such, no other fields need to be specified when creating an instance.
	/// The name of the convention is the same as the name of the index by default.
	/// The getters will default any missing information on the fly, avoiding both null and <seealso cref="Optional"/>.
	/// </para>
	/// <para>
	/// The convention is defined by six dates.
	/// <ul>
	/// <li>Trade date, the date that the trade is agreed
	/// <li>Spot date, the base for date calculations, typically 2 business days after the trade date
	/// <li>Start date, the date on which the implied deposit starts, typically a number of months after the spot date
	/// <li>End date, the date on which the implied deposit ends, typically a number of months after the spot date
	/// <li>Fixing date, the date on which the index is to be observed, typically 2 business days before the start date
	/// <li>Payment date, the date on which payment is made, typically the same as the start date
	/// </ul>
	/// The period between the spot date and the start/end date is specified by <seealso cref="FraTemplate"/>, not by this convention.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ImmutableFraConvention implements FraConvention, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ImmutableFraConvention : FraConvention, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
		private readonly IborIndex index;
	  /// <summary>
	  /// The convention name, such as 'GBP-LIBOR-3M', optional with defaulting getter.
	  /// <para>
	  /// This will default to the name of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final String name;
	  private readonly string name;
	  /// <summary>
	  /// The primary currency, optional with defaulting getter.
	  /// <para>
	  /// This is the currency of the FRA and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the currency of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The day count convention applicable, optional with defaulting getter.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the day count of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The offset of the spot value date from the trade date, optional with defaulting getter.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days.
	  /// The start and end date of the FRA term are relative to the spot date.
	  /// </para>
	  /// <para>
	  /// This will default to the effective date offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment spotDateOffset;
	  private readonly DaysAdjustment spotDateOffset;
	  /// <summary>
	  /// The business day adjustment to apply to the start and end date, optional with defaulting getter.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The offset of the fixing date from the start date, optional with defaulting getter.
	  /// <para>
	  /// The offset is applied to the start date and is typically minus 2 business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the fixing date offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The offset of the payment date from the start date, optional with defaulting getter.
	  /// <para>
	  /// Defines the offset from the start date to the payment date.
	  /// In most cases, the payment date is the same as the start date, so the default of zero is appropriate.
	  /// </para>
	  /// <para>
	  /// This will default to zero if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.basics.date.DaysAdjustment paymentDateOffset;
	  private readonly DaysAdjustment paymentDateOffset;
	  /// <summary>
	  /// The method to use for discounting, optional with defaulting getter.
	  /// <para>
	  /// There are different approaches FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// <para>
	  /// This will default 'AFMA' if the index has the currency
	  /// 'AUD' or 'NZD' and to 'ISDA' otherwise.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "field") private final com.opengamma.strata.product.fra.FraDiscountingMethod discounting;
	  private readonly FraDiscountingMethod discounting;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains a convention based on the specified index.
	  /// <para>
	  /// The standard market convention for a FRA is based exclusively on the index.
	  /// This creates an instance that contains the index.
	  /// The instance is not dereferenced using the {@code FraConvention} name, as such
	  /// the result of this method and <seealso cref="FraConvention#of(IborIndex)"/> can differ.
	  /// </para>
	  /// <para>
	  /// Use the <seealso cref="#builder() builder"/> for unusual conventions.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index, the market convention values are extracted from the index </param>
	  /// <returns> the convention </returns>
	  public static ImmutableFraConvention of(IborIndex index)
	  {
		return ImmutableFraConvention.builder().index(index).build();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the convention name, such as 'GBP-LIBOR-3M'.
	  /// This is the same as the name of the index by default.
	  /// <para>
	  /// This will default to the name of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the convention name </returns>
	  public string Name
	  {
		  get
		  {
			return !string.ReferenceEquals(name, null) ? name : index.Name;
		  }
	  }

	  /// <summary>
	  /// Gets the primary currency,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// This is the currency of the FRA and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the currency of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the currency, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency != null ? currency : index.Currency;
		  }
	  }

	  /// <summary>
	  /// Gets the day count convention applicable,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the day count of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the day count, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount != null ? dayCount : index.DayCount;
		  }
	  }

	  /// <summary>
	  /// Gets the offset of the spot value date from the trade date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The offset is applied to the trade date and is typically plus 2 business days.
	  /// The start and end date of the FRA term are relative to the spot date.
	  /// </para>
	  /// <para>
	  /// This will default to the effective date offset of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the spot date offset, not null </returns>
	  public DaysAdjustment SpotDateOffset
	  {
		  get
		  {
			return spotDateOffset != null ? spotDateOffset : index.EffectiveDateOffset;
		  }
	  }

	  /// <summary>
	  /// Gets the business day adjustment to apply to the start and end date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// <para>
	  /// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the business day adjustment, not null </returns>
	  public BusinessDayAdjustment BusinessDayAdjustment
	  {
		  get
		  {
			return businessDayAdjustment != null ? businessDayAdjustment : BusinessDayAdjustment.of(MODIFIED_FOLLOWING, index.FixingCalendar);
		  }
	  }

	  /// <summary>
	  /// Gets the offset of the fixing date from the start date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// The offset is applied to the start date and is typically minus 2 business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// This will default to the fixing date offset of the index if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the fixing date offset, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset != null ? fixingDateOffset : index.FixingDateOffset;
		  }
	  }

	  /// <summary>
	  /// Gets the offset of the payment date from the start date,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// Defines the offset from the start date to the payment date.
	  /// In most cases, the payment date is the same as the start date, so the default of zero is appropriate.
	  /// </para>
	  /// <para>
	  /// This will default to zero if not specified.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the payment date offset, not null </returns>
	  public DaysAdjustment PaymentDateOffset
	  {
		  get
		  {
			return paymentDateOffset != null ? paymentDateOffset : DaysAdjustment.NONE;
		  }
	  }

	  /// <summary>
	  /// Gets the method to use for discounting,
	  /// providing a default result if no override specified.
	  /// <para>
	  /// There are different approaches FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// <para>
	  /// This will default 'AFMA' if the index has the currency
	  /// 'AUD' or 'NZD' and to 'ISDA' otherwise.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the discounting method, not null </returns>
	  public FraDiscountingMethod Discounting
	  {
		  get
		  {
			if (discounting != null)
			{
			  return discounting;
			}
			Currency indexCcy = index.Currency;
			return indexCcy.Equals(AUD) || indexCcy.Equals(NZD) ? AFMA : ISDA;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public FraTrade createTrade(LocalDate tradeDate, Period periodToStart, Period periodToEnd, BuySell buySell, double notional, double fixedRate, ReferenceData refData)
	  {

		LocalDate spotValue = calculateSpotDateFromTradeDate(tradeDate, refData);
		LocalDate startDate = spotValue.plus(periodToStart);
		LocalDate endDate = spotValue.plus(periodToEnd);
		DateAdjuster bda = BusinessDayAdjustment.resolve(refData);
		// start/end dates are adjusted when FRA trade is created and not adjusted later
		// payment date is adjusted when FRA trade is created and potentially adjusted again when resolving
		LocalDate adjustedStart = bda.adjust(startDate);
		LocalDate adjustedEnd = bda.adjust(endDate);
		LocalDate adjustedPay = PaymentDateOffset.adjust(adjustedStart, refData);
		return toTrade(tradeDate, adjustedStart, adjustedEnd, adjustedPay, buySell, notional, fixedRate);
	  }

	  public FraTrade toTrade(TradeInfo tradeInfo, LocalDate startDate, LocalDate endDate, LocalDate paymentDate, BuySell buySell, double notional, double fixedRate)
	  {

		Optional<LocalDate> tradeDate = tradeInfo.TradeDate;
		if (tradeDate.Present)
		{
		  ArgChecker.inOrderOrEqual(tradeDate.get(), startDate, "tradeDate", "startDate");
		}
		// business day adjustment is not passed through as start/end date are fixed at
		// trade creation and should not be adjusted later
		return FraTrade.builder().info(tradeInfo).product(Fra.builder().buySell(buySell).currency(Currency).notional(notional).startDate(startDate).endDate(endDate).paymentDate(AdjustableDate.of(paymentDate, PaymentDateOffset.Adjustment)).fixedRate(fixedRate).index(index).fixingDateOffset(FixingDateOffset).dayCount(DayCount).discounting(Discounting).build()).build();
	  }

	  public override string ToString()
	  {
		return Name;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFraConvention}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ImmutableFraConvention.Meta meta()
	  {
		return ImmutableFraConvention.Meta.INSTANCE;
	  }

	  static ImmutableFraConvention()
	  {
		MetaBean.register(ImmutableFraConvention.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ImmutableFraConvention.Builder builder()
	  {
		return new ImmutableFraConvention.Builder();
	  }

	  private ImmutableFraConvention(IborIndex index, string name, Currency currency, DayCount dayCount, DaysAdjustment spotDateOffset, BusinessDayAdjustment businessDayAdjustment, DaysAdjustment fixingDateOffset, DaysAdjustment paymentDateOffset, FraDiscountingMethod discounting)
	  {
		JodaBeanUtils.notNull(index, "index");
		this.index = index;
		this.name = name;
		this.currency = currency;
		this.dayCount = dayCount;
		this.spotDateOffset = spotDateOffset;
		this.businessDayAdjustment = businessDayAdjustment;
		this.fixingDateOffset = fixingDateOffset;
		this.paymentDateOffset = paymentDateOffset;
		this.discounting = discounting;
	  }

	  public override ImmutableFraConvention.Meta metaBean()
	  {
		return ImmutableFraConvention.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return index;
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
		  ImmutableFraConvention other = (ImmutableFraConvention) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(spotDateOffset, other.spotDateOffset) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(paymentDateOffset, other.paymentDateOffset) && JodaBeanUtils.equal(discounting, other.discounting);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spotDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discounting);
		return hash;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ImmutableFraConvention}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(ImmutableFraConvention), typeof(IborIndex));
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ImmutableFraConvention), typeof(string));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(ImmutableFraConvention), typeof(Currency));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ImmutableFraConvention), typeof(DayCount));
			  spotDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "spotDateOffset", typeof(ImmutableFraConvention), typeof(DaysAdjustment));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(ImmutableFraConvention), typeof(BusinessDayAdjustment));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(ImmutableFraConvention), typeof(DaysAdjustment));
			  paymentDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDateOffset", typeof(ImmutableFraConvention), typeof(DaysAdjustment));
			  discounting_Renamed = DirectMetaProperty.ofImmutable(this, "discounting", typeof(ImmutableFraConvention), typeof(FraDiscountingMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "index", "name", "currency", "dayCount", "spotDateOffset", "businessDayAdjustment", "fixingDateOffset", "paymentDateOffset", "discounting");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> spotDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> paymentDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code discounting} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FraDiscountingMethod> discounting_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "index", "name", "currency", "dayCount", "spotDateOffset", "businessDayAdjustment", "fixingDateOffset", "paymentDateOffset", "discounting");
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
			case 100346066: // index
			  return index_Renamed;
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -536441087: // discounting
			  return discounting_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ImmutableFraConvention.Builder builder()
		{
		  return new ImmutableFraConvention.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ImmutableFraConvention);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spotDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> spotDateOffset()
		{
		  return spotDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> paymentDateOffset()
		{
		  return paymentDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discounting} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FraDiscountingMethod> discounting()
		{
		  return discounting_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return ((ImmutableFraConvention) bean).Index;
			case 3373707: // name
			  return ((ImmutableFraConvention) bean).name;
			case 575402001: // currency
			  return ((ImmutableFraConvention) bean).currency;
			case 1905311443: // dayCount
			  return ((ImmutableFraConvention) bean).dayCount;
			case 746995843: // spotDateOffset
			  return ((ImmutableFraConvention) bean).spotDateOffset;
			case -1065319863: // businessDayAdjustment
			  return ((ImmutableFraConvention) bean).businessDayAdjustment;
			case 873743726: // fixingDateOffset
			  return ((ImmutableFraConvention) bean).fixingDateOffset;
			case -716438393: // paymentDateOffset
			  return ((ImmutableFraConvention) bean).paymentDateOffset;
			case -536441087: // discounting
			  return ((ImmutableFraConvention) bean).discounting;
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
	  /// The bean-builder for {@code ImmutableFraConvention}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ImmutableFraConvention>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment spotDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment paymentDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FraDiscountingMethod discounting_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ImmutableFraConvention beanToCopy)
		{
		  this.index_Renamed = beanToCopy.Index;
		  this.name_Renamed = beanToCopy.name;
		  this.currency_Renamed = beanToCopy.currency;
		  this.dayCount_Renamed = beanToCopy.dayCount;
		  this.spotDateOffset_Renamed = beanToCopy.spotDateOffset;
		  this.businessDayAdjustment_Renamed = beanToCopy.businessDayAdjustment;
		  this.fixingDateOffset_Renamed = beanToCopy.fixingDateOffset;
		  this.paymentDateOffset_Renamed = beanToCopy.paymentDateOffset;
		  this.discounting_Renamed = beanToCopy.discounting;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  return index_Renamed;
			case 3373707: // name
			  return name_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 746995843: // spotDateOffset
			  return spotDateOffset_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case -716438393: // paymentDateOffset
			  return paymentDateOffset_Renamed;
			case -536441087: // discounting
			  return discounting_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case 3373707: // name
			  this.name_Renamed = (string) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 746995843: // spotDateOffset
			  this.spotDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -716438393: // paymentDateOffset
			  this.paymentDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case -536441087: // discounting
			  this.discounting_Renamed = (FraDiscountingMethod) newValue;
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

		public override ImmutableFraConvention build()
		{
		  return new ImmutableFraConvention(index_Renamed, name_Renamed, currency_Renamed, dayCount_Renamed, spotDateOffset_Renamed, businessDayAdjustment_Renamed, fixingDateOffset_Renamed, paymentDateOffset_Renamed, discounting_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the Ibor index.
		/// <para>
		/// The floating rate to be paid is based on this index
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// </para>
		/// </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the convention name, such as 'GBP-LIBOR-3M', optional with defaulting getter.
		/// <para>
		/// This will default to the name of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="name">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(string name)
		{
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the primary currency, optional with defaulting getter.
		/// <para>
		/// This is the currency of the FRA and the currency that payment is made in.
		/// The data model permits this currency to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the currency of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable, optional with defaulting getter.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// The data model permits the day count to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the day count of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the spot value date from the trade date, optional with defaulting getter.
		/// <para>
		/// The offset is applied to the trade date and is typically plus 2 business days.
		/// The start and end date of the FRA term are relative to the spot date.
		/// </para>
		/// <para>
		/// This will default to the effective date offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="spotDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spotDateOffset(DaysAdjustment spotDateOffset)
		{
		  this.spotDateOffset_Renamed = spotDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start and end date, optional with defaulting getter.
		/// <para>
		/// The start and end date are typically defined as valid business days and thus
		/// do not need to be adjusted. If this optional property is present, then the
		/// start and end date will be adjusted as defined here.
		/// </para>
		/// <para>
		/// This will default to 'ModifiedFollowing' using the index fixing calendar if not specified.
		/// </para>
		/// </summary>
		/// <param name="businessDayAdjustment">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder businessDayAdjustment(BusinessDayAdjustment businessDayAdjustment)
		{
		  this.businessDayAdjustment_Renamed = businessDayAdjustment;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the fixing date from the start date, optional with defaulting getter.
		/// <para>
		/// The offset is applied to the start date and is typically minus 2 business days.
		/// The data model permits the offset to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// This will default to the fixing date offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the payment date from the start date, optional with defaulting getter.
		/// <para>
		/// Defines the offset from the start date to the payment date.
		/// In most cases, the payment date is the same as the start date, so the default of zero is appropriate.
		/// </para>
		/// <para>
		/// This will default to zero if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentDateOffset">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDateOffset(DaysAdjustment paymentDateOffset)
		{
		  this.paymentDateOffset_Renamed = paymentDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the method to use for discounting, optional with defaulting getter.
		/// <para>
		/// There are different approaches FRA pricing in the area of discounting.
		/// This method specifies the approach for this FRA.
		/// </para>
		/// <para>
		/// This will default 'AFMA' if the index has the currency
		/// 'AUD' or 'NZD' and to 'ISDA' otherwise.
		/// </para>
		/// </summary>
		/// <param name="discounting">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discounting(FraDiscountingMethod discounting)
		{
		  this.discounting_Renamed = discounting;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(320);
		  buf.Append("ImmutableFraConvention.Builder{");
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("spotDateOffset").Append('=').Append(JodaBeanUtils.ToString(spotDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDateOffset").Append('=').Append(JodaBeanUtils.ToString(paymentDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("discounting").Append('=').Append(JodaBeanUtils.ToString(discounting_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}