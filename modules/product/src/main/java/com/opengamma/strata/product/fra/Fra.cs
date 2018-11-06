using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.fra
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.AUD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.NZD;
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
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DateAdjuster = com.opengamma.strata.basics.date.DateAdjuster;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using IborInterpolatedRateComputation = com.opengamma.strata.product.rate.IborInterpolatedRateComputation;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateComputation = com.opengamma.strata.product.rate.RateComputation;

	/// <summary>
	/// A forward rate agreement (FRA).
	/// <para>
	/// A FRA is a financial instrument that represents the one off exchange of a fixed
	/// rate of interest for a floating rate at a future date.
	/// </para>
	/// <para>
	/// For example, a FRA might involve an agreement to exchange the difference between
	/// the fixed rate of 1% and the 'GBP-LIBOR-3M' rate in 2 months time.
	/// </para>
	/// <para>
	/// The FRA is defined by four dates.
	/// <ul>
	/// <li>Start date, the date on which the implied deposit starts
	/// <li>End date, the date on which the implied deposit ends
	/// <li>Fixing date, the date on which the index is to be observed, typically 2 business days before the start date
	/// <li>Payment date, the date on which payment is made, typically the same as the start date
	/// </ul>
	/// </para>
	/// <para>
	/// The start date, end date and payment date are determined when the trade if created,
	/// adjusting to valid business days based on the holiday calendar dates known on the trade trade.
	/// The payment date may be further adjusted when the FRA is resolved if an additional holiday has been added.
	/// The data model does allow for the start and end dates to be adjusted when the FRA is resolved,
	/// but this is typically not used.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Fra implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedFra>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Fra : Product, Resolvable<ResolvedFra>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.common.BuySell buySell;
		private readonly BuySell buySell;
	  /// <summary>
	  /// The primary currency, defaulted to the currency of the index.
	  /// <para>
	  /// This is the currency of the FRA and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the currency of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.currency.Currency currency;
	  private readonly Currency currency;
	  /// <summary>
	  /// The notional amount.
	  /// <para>
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "ArgChecker.notNegative") private final double notional;
	  private readonly double notional;
	  /// <summary>
	  /// The start date, which is the effective date of the FRA.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate startDate;
	  private readonly LocalDate startDate;
	  /// <summary>
	  /// The end date, which is the termination date of the FRA.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.LocalDate endDate;
	  private readonly LocalDate endDate;
	  /// <summary>
	  /// The business day adjustment to apply to the start and end date, optional.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.BusinessDayAdjustment businessDayAdjustment;
	  private readonly BusinessDayAdjustment businessDayAdjustment;
	  /// <summary>
	  /// The payment date.
	  /// <para>
	  /// The payment date is typically the same as the start date.
	  /// The date may be subject to adjustment to ensure it is a business day.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date with no adjustments if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.AdjustableDate paymentDate;
	  private readonly AdjustableDate paymentDate;
	  /// <summary>
	  /// The fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// See {@code buySell} to determine whether this rate is paid or received.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double fixedRate;
	  private readonly double fixedRate;
	  /// <summary>
	  /// The Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// This will be used throughout unless {@code indexInterpolated} is present.
	  /// </para>
	  /// <para>
	  /// See {@code buySell} to determine whether this rate is paid or received.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.IborIndex index;
	  private readonly IborIndex index;
	  /// <summary>
	  /// The second Ibor index to be used for linear interpolation, optional.
	  /// <para>
	  /// This will be used with {@code index} to linearly interpolate the rate.
	  /// It will be a well known market index such as 'GBP-LIBOR-6M'.
	  /// This index may be shorter or longer than {@code index}, but not the same.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.index.IborIndex indexInterpolated;
	  private readonly IborIndex indexInterpolated;
	  /// <summary>
	  /// The offset of the fixing date from the start date.
	  /// <para>
	  /// The offset is applied to the start date and is typically minus 2 business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing date offset of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DaysAdjustment fixingDateOffset;
	  private readonly DaysAdjustment fixingDateOffset;
	  /// <summary>
	  /// The day count convention applicable, defaulted to the day count of the index.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the day count of the index if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The method to use for discounting, defaulted to 'ISDA' or 'AFMA'.
	  /// <para>
	  /// There are different approaches FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// <para>
	  /// When building, this will default 'AFMA' if the index has the currency
	  /// 'AUD' or 'NZD' and to 'ISDA' otherwise.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final FraDiscountingMethod discounting;
	  private readonly FraDiscountingMethod discounting;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.index_Renamed != null)
		{
		  if (builder.dayCount_Renamed == null)
		  {
			builder.dayCount_Renamed = builder.index_Renamed.DayCount;
		  }
		  if (builder.fixingDateOffset_Renamed == null)
		  {
			builder.fixingDateOffset_Renamed = builder.index_Renamed.FixingDateOffset;
		  }
		  if (builder.currency_Renamed == null)
		  {
			builder.currency_Renamed = builder.index_Renamed.Currency;
		  }
		  if (builder.discounting_Renamed == null)
		  {
			Currency curr = builder.index_Renamed.Currency;
			builder.discounting_Renamed = (curr.Equals(AUD) || curr.Equals(NZD) ? AFMA : ISDA);
		  }
		}
		if (builder.paymentDate_Renamed == null && builder.startDate_Renamed != null)
		{
		  builder.paymentDate_Renamed = AdjustableDate.of(builder.startDate_Renamed);
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.inOrderNotEqual(startDate, endDate, "startDate", "endDate");
		if (index.Equals(indexInterpolated))
		{
		  throw new System.ArgumentException("Interpolation requires two different indices");
		}
	  }

	  //-------------------------------------------------------------------------
	  public ImmutableSet<Currency> allCurrencies()
	  {
		return ImmutableSet.of(currency);
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedFra resolve(ReferenceData refData)
	  {
		DateAdjuster bda = BusinessDayAdjustment.orElse(BusinessDayAdjustment.NONE).resolve(refData);
		LocalDate start = bda.adjust(startDate);
		LocalDate end = bda.adjust(endDate);
		LocalDate pay = paymentDate.adjusted(refData);
		return ResolvedFra.builder().paymentDate(pay).startDate(start).endDate(end).yearFraction(dayCount.yearFraction(start, end)).fixedRate(fixedRate).floatingRate(createRateComputation(refData)).currency(currency).notional(buySell.normalize(notional)).discounting(discounting).build();
	  }

	  // creates an Ibor or IborInterpolated computation
	  private RateComputation createRateComputation(ReferenceData refData)
	  {
		LocalDate fixingDate = fixingDateOffset.adjust(startDate, refData);
		if (indexInterpolated != null)
		{
		  return IborInterpolatedRateComputation.of(index, indexInterpolated, fixingDate, refData);
		}
		else
		{
		  return IborRateComputation.of(index, fixingDate, refData);
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Fra}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Fra.Meta meta()
	  {
		return Fra.Meta.INSTANCE;
	  }

	  static Fra()
	  {
		MetaBean.register(Fra.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Fra.Builder builder()
	  {
		return new Fra.Builder();
	  }

	  private Fra(BuySell buySell, Currency currency, double notional, LocalDate startDate, LocalDate endDate, BusinessDayAdjustment businessDayAdjustment, AdjustableDate paymentDate, double fixedRate, IborIndex index, IborIndex indexInterpolated, DaysAdjustment fixingDateOffset, DayCount dayCount, FraDiscountingMethod discounting)
	  {
		JodaBeanUtils.notNull(buySell, "buySell");
		JodaBeanUtils.notNull(currency, "currency");
		ArgChecker.notNegative(notional, "notional");
		JodaBeanUtils.notNull(startDate, "startDate");
		JodaBeanUtils.notNull(endDate, "endDate");
		JodaBeanUtils.notNull(paymentDate, "paymentDate");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(discounting, "discounting");
		this.buySell = buySell;
		this.currency = currency;
		this.notional = notional;
		this.startDate = startDate;
		this.endDate = endDate;
		this.businessDayAdjustment = businessDayAdjustment;
		this.paymentDate = paymentDate;
		this.fixedRate = fixedRate;
		this.index = index;
		this.indexInterpolated = indexInterpolated;
		this.fixingDateOffset = fixingDateOffset;
		this.dayCount = dayCount;
		this.discounting = discounting;
		validate();
	  }

	  public override Fra.Meta metaBean()
	  {
		return Fra.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets whether the FRA is buy or sell.
	  /// <para>
	  /// A value of 'Buy' implies that the floating rate is received from the counterparty,
	  /// with the fixed rate being paid. A value of 'Sell' implies that the floating rate
	  /// is paid to the counterparty, with the fixed rate being received.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public BuySell BuySell
	  {
		  get
		  {
			return buySell;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the primary currency, defaulted to the currency of the index.
	  /// <para>
	  /// This is the currency of the FRA and the currency that payment is made in.
	  /// The data model permits this currency to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the currency of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Currency Currency
	  {
		  get
		  {
			return currency;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the notional amount.
	  /// <para>
	  /// The notional expressed here must be positive.
	  /// The currency of the notional is specified by {@code currency}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double Notional
	  {
		  get
		  {
			return notional;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the start date, which is the effective date of the FRA.
	  /// <para>
	  /// This is the first date that interest accrues.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate StartDate
	  {
		  get
		  {
			return startDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the end date, which is the termination date of the FRA.
	  /// <para>
	  /// This is the last day that interest accrues.
	  /// This date must be after the start date.
	  /// </para>
	  /// <para>
	  /// This date is typically set to be a valid business day.
	  /// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public LocalDate EndDate
	  {
		  get
		  {
			return endDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the business day adjustment to apply to the start and end date, optional.
	  /// <para>
	  /// The start and end date are typically defined as valid business days and thus
	  /// do not need to be adjusted. If this optional property is present, then the
	  /// start and end date will be adjusted as defined here.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<BusinessDayAdjustment> BusinessDayAdjustment
	  {
		  get
		  {
			return Optional.ofNullable(businessDayAdjustment);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the payment date.
	  /// <para>
	  /// The payment date is typically the same as the start date.
	  /// The date may be subject to adjustment to ensure it is a business day.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the start date with no adjustments if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public AdjustableDate PaymentDate
	  {
		  get
		  {
			return paymentDate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed rate of interest.
	  /// A 5% rate will be expressed as 0.05.
	  /// <para>
	  /// See {@code buySell} to determine whether this rate is paid or received.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public double FixedRate
	  {
		  get
		  {
			return fixedRate;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index.
	  /// <para>
	  /// The floating rate to be paid is based on this index
	  /// It will be a well known market index such as 'GBP-LIBOR-3M'.
	  /// This will be used throughout unless {@code indexInterpolated} is present.
	  /// </para>
	  /// <para>
	  /// See {@code buySell} to determine whether this rate is paid or received.
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
	  /// Gets the second Ibor index to be used for linear interpolation, optional.
	  /// <para>
	  /// This will be used with {@code index} to linearly interpolate the rate.
	  /// It will be a well known market index such as 'GBP-LIBOR-6M'.
	  /// This index may be shorter or longer than {@code index}, but not the same.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<IborIndex> IndexInterpolated
	  {
		  get
		  {
			return Optional.ofNullable(indexInterpolated);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the offset of the fixing date from the start date.
	  /// <para>
	  /// The offset is applied to the start date and is typically minus 2 business days.
	  /// The data model permits the offset to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the fixing date offset of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DaysAdjustment FixingDateOffset
	  {
		  get
		  {
			return fixingDateOffset;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention applicable, defaulted to the day count of the index.
	  /// <para>
	  /// This is used to convert dates to a numerical value.
	  /// The data model permits the day count to differ from that of the index,
	  /// however the two are typically the same.
	  /// </para>
	  /// <para>
	  /// When building, this will default to the day count of the index if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method to use for discounting, defaulted to 'ISDA' or 'AFMA'.
	  /// <para>
	  /// There are different approaches FRA pricing in the area of discounting.
	  /// This method specifies the approach for this FRA.
	  /// </para>
	  /// <para>
	  /// When building, this will default 'AFMA' if the index has the currency
	  /// 'AUD' or 'NZD' and to 'ISDA' otherwise.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FraDiscountingMethod Discounting
	  {
		  get
		  {
			return discounting;
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
		  Fra other = (Fra) obj;
		  return JodaBeanUtils.equal(buySell, other.buySell) && JodaBeanUtils.equal(currency, other.currency) && JodaBeanUtils.equal(notional, other.notional) && JodaBeanUtils.equal(startDate, other.startDate) && JodaBeanUtils.equal(endDate, other.endDate) && JodaBeanUtils.equal(businessDayAdjustment, other.businessDayAdjustment) && JodaBeanUtils.equal(paymentDate, other.paymentDate) && JodaBeanUtils.equal(fixedRate, other.fixedRate) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(indexInterpolated, other.indexInterpolated) && JodaBeanUtils.equal(fixingDateOffset, other.fixingDateOffset) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(discounting, other.discounting);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(buySell);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currency);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(notional);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(startDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(endDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(businessDayAdjustment);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(paymentDate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(indexInterpolated);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixingDateOffset);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discounting);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(448);
		buf.Append("Fra{");
		buf.Append("buySell").Append('=').Append(buySell).Append(',').Append(' ');
		buf.Append("currency").Append('=').Append(currency).Append(',').Append(' ');
		buf.Append("notional").Append('=').Append(notional).Append(',').Append(' ');
		buf.Append("startDate").Append('=').Append(startDate).Append(',').Append(' ');
		buf.Append("endDate").Append('=').Append(endDate).Append(',').Append(' ');
		buf.Append("businessDayAdjustment").Append('=').Append(businessDayAdjustment).Append(',').Append(' ');
		buf.Append("paymentDate").Append('=').Append(paymentDate).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(fixedRate).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("indexInterpolated").Append('=').Append(indexInterpolated).Append(',').Append(' ');
		buf.Append("fixingDateOffset").Append('=').Append(fixingDateOffset).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("discounting").Append('=').Append(JodaBeanUtils.ToString(discounting));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Fra}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  buySell_Renamed = DirectMetaProperty.ofImmutable(this, "buySell", typeof(Fra), typeof(BuySell));
			  currency_Renamed = DirectMetaProperty.ofImmutable(this, "currency", typeof(Fra), typeof(Currency));
			  notional_Renamed = DirectMetaProperty.ofImmutable(this, "notional", typeof(Fra), Double.TYPE);
			  startDate_Renamed = DirectMetaProperty.ofImmutable(this, "startDate", typeof(Fra), typeof(LocalDate));
			  endDate_Renamed = DirectMetaProperty.ofImmutable(this, "endDate", typeof(Fra), typeof(LocalDate));
			  businessDayAdjustment_Renamed = DirectMetaProperty.ofImmutable(this, "businessDayAdjustment", typeof(Fra), typeof(BusinessDayAdjustment));
			  paymentDate_Renamed = DirectMetaProperty.ofImmutable(this, "paymentDate", typeof(Fra), typeof(AdjustableDate));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(Fra), Double.TYPE);
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(Fra), typeof(IborIndex));
			  indexInterpolated_Renamed = DirectMetaProperty.ofImmutable(this, "indexInterpolated", typeof(Fra), typeof(IborIndex));
			  fixingDateOffset_Renamed = DirectMetaProperty.ofImmutable(this, "fixingDateOffset", typeof(Fra), typeof(DaysAdjustment));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(Fra), typeof(DayCount));
			  discounting_Renamed = DirectMetaProperty.ofImmutable(this, "discounting", typeof(Fra), typeof(FraDiscountingMethod));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "buySell", "currency", "notional", "startDate", "endDate", "businessDayAdjustment", "paymentDate", "fixedRate", "index", "indexInterpolated", "fixingDateOffset", "dayCount", "discounting");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code buySell} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BuySell> buySell_Renamed;
		/// <summary>
		/// The meta-property for the {@code currency} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Currency> currency_Renamed;
		/// <summary>
		/// The meta-property for the {@code notional} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> notional_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<LocalDate> endDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BusinessDayAdjustment> businessDayAdjustment_Renamed;
		/// <summary>
		/// The meta-property for the {@code paymentDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> paymentDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code indexInterpolated} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> indexInterpolated_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DaysAdjustment> fixingDateOffset_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code discounting} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FraDiscountingMethod> discounting_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "buySell", "currency", "notional", "startDate", "endDate", "businessDayAdjustment", "paymentDate", "fixedRate", "index", "indexInterpolated", "fixingDateOffset", "dayCount", "discounting");
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
			case 244977400: // buySell
			  return buySell_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1934091915: // indexInterpolated
			  return indexInterpolated_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -536441087: // discounting
			  return discounting_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Fra.Builder builder()
		{
		  return new Fra.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Fra);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code buySell} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BuySell> buySell()
		{
		  return buySell_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currency} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Currency> currency()
		{
		  return currency_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code notional} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> notional()
		{
		  return notional_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<LocalDate> endDate()
		{
		  return endDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code businessDayAdjustment} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BusinessDayAdjustment> businessDayAdjustment()
		{
		  return businessDayAdjustment_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code paymentDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> paymentDate()
		{
		  return paymentDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code indexInterpolated} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> indexInterpolated()
		{
		  return indexInterpolated_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixingDateOffset} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DaysAdjustment> fixingDateOffset()
		{
		  return fixingDateOffset_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
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
			case 244977400: // buySell
			  return ((Fra) bean).BuySell;
			case 575402001: // currency
			  return ((Fra) bean).Currency;
			case 1585636160: // notional
			  return ((Fra) bean).Notional;
			case -2129778896: // startDate
			  return ((Fra) bean).StartDate;
			case -1607727319: // endDate
			  return ((Fra) bean).EndDate;
			case -1065319863: // businessDayAdjustment
			  return ((Fra) bean).businessDayAdjustment;
			case -1540873516: // paymentDate
			  return ((Fra) bean).PaymentDate;
			case 747425396: // fixedRate
			  return ((Fra) bean).FixedRate;
			case 100346066: // index
			  return ((Fra) bean).Index;
			case -1934091915: // indexInterpolated
			  return ((Fra) bean).indexInterpolated;
			case 873743726: // fixingDateOffset
			  return ((Fra) bean).FixingDateOffset;
			case 1905311443: // dayCount
			  return ((Fra) bean).DayCount;
			case -536441087: // discounting
			  return ((Fra) bean).Discounting;
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
	  /// The bean-builder for {@code Fra}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Fra>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BuySell buySell_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Currency currency_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double notional_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate startDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal LocalDate endDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal BusinessDayAdjustment businessDayAdjustment_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal AdjustableDate paymentDate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double fixedRate_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex indexInterpolated_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DaysAdjustment fixingDateOffset_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
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
		internal Builder(Fra beanToCopy)
		{
		  this.buySell_Renamed = beanToCopy.BuySell;
		  this.currency_Renamed = beanToCopy.Currency;
		  this.notional_Renamed = beanToCopy.Notional;
		  this.startDate_Renamed = beanToCopy.StartDate;
		  this.endDate_Renamed = beanToCopy.EndDate;
		  this.businessDayAdjustment_Renamed = beanToCopy.businessDayAdjustment;
		  this.paymentDate_Renamed = beanToCopy.PaymentDate;
		  this.fixedRate_Renamed = beanToCopy.FixedRate;
		  this.index_Renamed = beanToCopy.Index;
		  this.indexInterpolated_Renamed = beanToCopy.indexInterpolated;
		  this.fixingDateOffset_Renamed = beanToCopy.FixingDateOffset;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.discounting_Renamed = beanToCopy.Discounting;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 244977400: // buySell
			  return buySell_Renamed;
			case 575402001: // currency
			  return currency_Renamed;
			case 1585636160: // notional
			  return notional_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
			case -1065319863: // businessDayAdjustment
			  return businessDayAdjustment_Renamed;
			case -1540873516: // paymentDate
			  return paymentDate_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case -1934091915: // indexInterpolated
			  return indexInterpolated_Renamed;
			case 873743726: // fixingDateOffset
			  return fixingDateOffset_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
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
			case 244977400: // buySell
			  this.buySell_Renamed = (BuySell) newValue;
			  break;
			case 575402001: // currency
			  this.currency_Renamed = (Currency) newValue;
			  break;
			case 1585636160: // notional
			  this.notional_Renamed = (double?) newValue.Value;
			  break;
			case -2129778896: // startDate
			  this.startDate_Renamed = (LocalDate) newValue;
			  break;
			case -1607727319: // endDate
			  this.endDate_Renamed = (LocalDate) newValue;
			  break;
			case -1065319863: // businessDayAdjustment
			  this.businessDayAdjustment_Renamed = (BusinessDayAdjustment) newValue;
			  break;
			case -1540873516: // paymentDate
			  this.paymentDate_Renamed = (AdjustableDate) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue.Value;
			  break;
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case -1934091915: // indexInterpolated
			  this.indexInterpolated_Renamed = (IborIndex) newValue;
			  break;
			case 873743726: // fixingDateOffset
			  this.fixingDateOffset_Renamed = (DaysAdjustment) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
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

		public override Fra build()
		{
		  preBuild(this);
		  return new Fra(buySell_Renamed, currency_Renamed, notional_Renamed, startDate_Renamed, endDate_Renamed, businessDayAdjustment_Renamed, paymentDate_Renamed, fixedRate_Renamed, index_Renamed, indexInterpolated_Renamed, fixingDateOffset_Renamed, dayCount_Renamed, discounting_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets whether the FRA is buy or sell.
		/// <para>
		/// A value of 'Buy' implies that the floating rate is received from the counterparty,
		/// with the fixed rate being paid. A value of 'Sell' implies that the floating rate
		/// is paid to the counterparty, with the fixed rate being received.
		/// </para>
		/// </summary>
		/// <param name="buySell">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder buySell(BuySell buySell)
		{
		  JodaBeanUtils.notNull(buySell, "buySell");
		  this.buySell_Renamed = buySell;
		  return this;
		}

		/// <summary>
		/// Sets the primary currency, defaulted to the currency of the index.
		/// <para>
		/// This is the currency of the FRA and the currency that payment is made in.
		/// The data model permits this currency to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// When building, this will default to the currency of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="currency">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currency(Currency currency)
		{
		  JodaBeanUtils.notNull(currency, "currency");
		  this.currency_Renamed = currency;
		  return this;
		}

		/// <summary>
		/// Sets the notional amount.
		/// <para>
		/// The notional expressed here must be positive.
		/// The currency of the notional is specified by {@code currency}.
		/// </para>
		/// </summary>
		/// <param name="notional">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder notional(double notional)
		{
		  ArgChecker.notNegative(notional, "notional");
		  this.notional_Renamed = notional;
		  return this;
		}

		/// <summary>
		/// Sets the start date, which is the effective date of the FRA.
		/// <para>
		/// This is the first date that interest accrues.
		/// </para>
		/// <para>
		/// This date is typically set to be a valid business day.
		/// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
		/// </para>
		/// </summary>
		/// <param name="startDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder startDate(LocalDate startDate)
		{
		  JodaBeanUtils.notNull(startDate, "startDate");
		  this.startDate_Renamed = startDate;
		  return this;
		}

		/// <summary>
		/// Sets the end date, which is the termination date of the FRA.
		/// <para>
		/// This is the last day that interest accrues.
		/// This date must be after the start date.
		/// </para>
		/// <para>
		/// This date is typically set to be a valid business day.
		/// Optionally, the {@code businessDayAdjustment} property may be set to provide a rule for adjustment.
		/// </para>
		/// </summary>
		/// <param name="endDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder endDate(LocalDate endDate)
		{
		  JodaBeanUtils.notNull(endDate, "endDate");
		  this.endDate_Renamed = endDate;
		  return this;
		}

		/// <summary>
		/// Sets the business day adjustment to apply to the start and end date, optional.
		/// <para>
		/// The start and end date are typically defined as valid business days and thus
		/// do not need to be adjusted. If this optional property is present, then the
		/// start and end date will be adjusted as defined here.
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
		/// Sets the payment date.
		/// <para>
		/// The payment date is typically the same as the start date.
		/// The date may be subject to adjustment to ensure it is a business day.
		/// </para>
		/// <para>
		/// When building, this will default to the start date with no adjustments if not specified.
		/// </para>
		/// </summary>
		/// <param name="paymentDate">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder paymentDate(AdjustableDate paymentDate)
		{
		  JodaBeanUtils.notNull(paymentDate, "paymentDate");
		  this.paymentDate_Renamed = paymentDate;
		  return this;
		}

		/// <summary>
		/// Sets the fixed rate of interest.
		/// A 5% rate will be expressed as 0.05.
		/// <para>
		/// See {@code buySell} to determine whether this rate is paid or received.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		/// <summary>
		/// Sets the Ibor index.
		/// <para>
		/// The floating rate to be paid is based on this index
		/// It will be a well known market index such as 'GBP-LIBOR-3M'.
		/// This will be used throughout unless {@code indexInterpolated} is present.
		/// </para>
		/// <para>
		/// See {@code buySell} to determine whether this rate is paid or received.
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
		/// Sets the second Ibor index to be used for linear interpolation, optional.
		/// <para>
		/// This will be used with {@code index} to linearly interpolate the rate.
		/// It will be a well known market index such as 'GBP-LIBOR-6M'.
		/// This index may be shorter or longer than {@code index}, but not the same.
		/// </para>
		/// </summary>
		/// <param name="indexInterpolated">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder indexInterpolated(IborIndex indexInterpolated)
		{
		  this.indexInterpolated_Renamed = indexInterpolated;
		  return this;
		}

		/// <summary>
		/// Sets the offset of the fixing date from the start date.
		/// <para>
		/// The offset is applied to the start date and is typically minus 2 business days.
		/// The data model permits the offset to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// When building, this will default to the fixing date offset of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="fixingDateOffset">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixingDateOffset(DaysAdjustment fixingDateOffset)
		{
		  JodaBeanUtils.notNull(fixingDateOffset, "fixingDateOffset");
		  this.fixingDateOffset_Renamed = fixingDateOffset;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention applicable, defaulted to the day count of the index.
		/// <para>
		/// This is used to convert dates to a numerical value.
		/// The data model permits the day count to differ from that of the index,
		/// however the two are typically the same.
		/// </para>
		/// <para>
		/// When building, this will default to the day count of the index if not specified.
		/// </para>
		/// </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the method to use for discounting, defaulted to 'ISDA' or 'AFMA'.
		/// <para>
		/// There are different approaches FRA pricing in the area of discounting.
		/// This method specifies the approach for this FRA.
		/// </para>
		/// <para>
		/// When building, this will default 'AFMA' if the index has the currency
		/// 'AUD' or 'NZD' and to 'ISDA' otherwise.
		/// </para>
		/// </summary>
		/// <param name="discounting">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder discounting(FraDiscountingMethod discounting)
		{
		  JodaBeanUtils.notNull(discounting, "discounting");
		  this.discounting_Renamed = discounting;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(448);
		  buf.Append("Fra.Builder{");
		  buf.Append("buySell").Append('=').Append(JodaBeanUtils.ToString(buySell_Renamed)).Append(',').Append(' ');
		  buf.Append("currency").Append('=').Append(JodaBeanUtils.ToString(currency_Renamed)).Append(',').Append(' ');
		  buf.Append("notional").Append('=').Append(JodaBeanUtils.ToString(notional_Renamed)).Append(',').Append(' ');
		  buf.Append("startDate").Append('=').Append(JodaBeanUtils.ToString(startDate_Renamed)).Append(',').Append(' ');
		  buf.Append("endDate").Append('=').Append(JodaBeanUtils.ToString(endDate_Renamed)).Append(',').Append(' ');
		  buf.Append("businessDayAdjustment").Append('=').Append(JodaBeanUtils.ToString(businessDayAdjustment_Renamed)).Append(',').Append(' ');
		  buf.Append("paymentDate").Append('=').Append(JodaBeanUtils.ToString(paymentDate_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("indexInterpolated").Append('=').Append(JodaBeanUtils.ToString(indexInterpolated_Renamed)).Append(',').Append(' ');
		  buf.Append("fixingDateOffset").Append('=').Append(JodaBeanUtils.ToString(fixingDateOffset_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("discounting").Append('=').Append(JodaBeanUtils.ToString(discounting_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}