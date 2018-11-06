using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using DerivedProperty = org.joda.beans.gen.DerivedProperty;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Resolvable = com.opengamma.strata.basics.Resolvable;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using Index = com.opengamma.strata.basics.index.Index;
	using ValueSchedule = com.opengamma.strata.basics.value.ValueSchedule;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using SummarizerUtils = com.opengamma.strata.product.common.SummarizerUtils;

	/// <summary>
	/// A rate swap.
	/// <para>
	/// A rate swap is a financial instrument that represents the exchange of streams of payments.
	/// The swap is formed of legs, where each leg typically represents the obligations
	/// of the seller or buyer of the swap. In the simplest vanilla interest rate swap,
	/// there are two legs, one with a fixed rate and the other a floating rate.
	/// Many other more complex swaps can also be represented.
	/// </para>
	/// <para>
	/// For example, a swap might involve an agreement to exchange the difference between
	/// the fixed rate of 1% and the 'GBP-LIBOR-3M' rate every 3 months for 2 years.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class Swap implements com.opengamma.strata.product.Product, com.opengamma.strata.basics.Resolvable<ResolvedSwap>, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class Swap : Product, Resolvable<ResolvedSwap>, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", builderType = "List<? extends SwapLeg>") private final com.google.common.collect.ImmutableList<SwapLeg> legs;
		private readonly ImmutableList<SwapLeg> legs;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates a swap from one or more swap legs.
	  /// <para>
	  /// While most swaps have two legs, other combinations are possible.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legs">  the array of legs </param>
	  /// <returns> the swap </returns>
	  public static Swap of(params SwapLeg[] legs)
	  {
		ArgChecker.notEmpty(legs, "legs");
		return new Swap(ImmutableList.copyOf(legs));
	  }

	  /// <summary>
	  /// Creates a swap from one or more swap legs.
	  /// <para>
	  /// While most swaps have two legs, other combinations are possible.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="legs">  the list of legs </param>
	  /// <returns> the swap </returns>
	  public static Swap of<T1>(IList<T1> legs) where T1 : SwapLeg
	  {
		ArgChecker.notEmpty(legs, "legs");
		return new Swap(ImmutableList.copyOf(legs));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legs of the swap with the specified type.
	  /// <para>
	  /// This returns all the legs with the given type.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="type">  the type to find </param>
	  /// <returns> the matching legs of the swap </returns>
	  public ImmutableList<SwapLeg> getLegs(SwapLegType type)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return legs.Where(leg => leg.Type == type).collect(toImmutableList());
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the first pay or receive leg of the swap.
	  /// <para>
	  /// This returns the first pay or receive leg of the swap, empty if no matching leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="payReceive">  the pay or receive flag </param>
	  /// <returns> the first matching leg of the swap </returns>
	  public Optional<SwapLeg> getLeg(PayReceive payReceive)
	  {
		return legs.Where(leg => leg.PayReceive == payReceive).First();
	  }

	  /// <summary>
	  /// Gets the first pay leg of the swap.
	  /// <para>
	  /// This returns the first pay leg of the swap, empty if no pay leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the first pay leg of the swap </returns>
	  public Optional<SwapLeg> PayLeg
	  {
		  get
		  {
			return getLeg(PayReceive.PAY);
		  }
	  }

	  /// <summary>
	  /// Gets the first receive leg of the swap.
	  /// <para>
	  /// This returns the first receive leg of the swap, empty if no receive leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the first receive leg of the swap </returns>
	  public Optional<SwapLeg> ReceiveLeg
	  {
		  get
		  {
			return getLeg(PayReceive.RECEIVE);
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the accrual start date of the swap.
	  /// <para>
	  /// This is the earliest accrual date of the legs, often known as the effective date.
	  /// The latest date is chosen by examining the unadjusted end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the start date of the swap </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getStartDate()
	  public AdjustableDate StartDate
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			return legs.Select(SwapLeg::getStartDate).Min(System.Collections.IComparer.comparing(adjDate => adjDate.Unadjusted)).get(); // always at least one leg, so get() is safe
		  }
	  }

	  /// <summary>
	  /// Gets the accrual end date of the swap.
	  /// <para>
	  /// This is the latest accrual date of the legs, often known as the termination date.
	  /// The latest date is chosen by examining the unadjusted end date.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the end date of the swap </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @DerivedProperty public com.opengamma.strata.basics.date.AdjustableDate getEndDate()
	  public AdjustableDate EndDate
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			return legs.Select(SwapLeg::getEndDate).Max(System.Collections.IComparer.comparing(adjDate => adjDate.Unadjusted)).get(); // always at least one leg, so get() is safe
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of payment currencies referred to by the swap.
	  /// <para>
	  /// This returns the complete set of payment currencies for the swap.
	  /// This will typically return one or two currencies.
	  /// </para>
	  /// <para>
	  /// If there is an FX reset, then this set contains the currency of the payment,
	  /// not the currency of the notional. Note that in many cases, the currency of
	  /// the FX reset notional will be the currency of the other leg.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of payment currencies referred to by this swap </returns>
	  public override ImmutableSet<Currency> allPaymentCurrencies()
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return legs.Select(leg => leg.Currency).collect(toImmutableSet());
	  }

	  /// <summary>
	  /// Returns the set of currencies referred to by the swap.
	  /// <para>
	  /// This returns the complete set of currencies for the swap, not just the payment currencies.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of currencies referred to by this swap </returns>
	  public ImmutableSet<Currency> allCurrencies()
	  {
		ImmutableSet.Builder<Currency> builder = ImmutableSet.builder();
		legs.ForEach(leg => leg.collectCurrencies(builder));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the set of indices referred to by the swap.
	  /// <para>
	  /// A swap will typically refer to at least one index, such as 'GBP-LIBOR-3M'.
	  /// Calling this method will return the complete list of indices, including
	  /// any associated with FX reset.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the set of indices referred to by this swap </returns>
	  public ImmutableSet<Index> allIndices()
	  {
		ImmutableSet.Builder<Index> builder = ImmutableSet.builder();
		legs.ForEach(leg => leg.collectIndices(builder));
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Summarizes this swap into string form.
	  /// </summary>
	  /// <returns> the summary description </returns>
	  public string summaryDescription()
	  {
		// 5Y USD 2mm Rec USD-LIBOR-6M / Pay 1% : 21Jan17-21Jan22
		StringBuilder buf = new StringBuilder(64);
		buf.Append(SummarizerUtils.datePeriod(StartDate.Unadjusted, EndDate.Unadjusted));
		buf.Append(' ');
		if (Legs.size() == 2 && PayLeg.Present && ReceiveLeg.Present && Legs.All(leg => leg is RateCalculationSwapLeg))
		{

		  // normal swap
		  SwapLeg payLeg = PayLeg.get();
		  SwapLeg recLeg = ReceiveLeg.get();
		  string payNotional = notional(payLeg);
		  string recNotional = notional(recLeg);
		  if (payNotional.Equals(recNotional))
		  {
			buf.Append(recNotional);
			buf.Append(" Rec ");
			buf.Append(legSummary(recLeg));
			buf.Append(" / Pay ");
			buf.Append(legSummary(payLeg));
		  }
		  else
		  {
			buf.Append("Rec ");
			buf.Append(legSummary(recLeg));
			buf.Append(' ');
			buf.Append(recNotional);
			buf.Append(" / Pay ");
			buf.Append(legSummary(payLeg));
			buf.Append(' ');
			buf.Append(payNotional);
		  }
		}
		else
		{
		  // abnormal swap
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  buf.Append(Legs.Select(leg => (SummarizerUtils.payReceive(leg.PayReceive) + " " + legSummary(leg) + " " + notional(leg)).Trim()).collect(joining(" / ")));
		}
		buf.Append(" : ");
		buf.Append(SummarizerUtils.dateRange(StartDate.Unadjusted, EndDate.Unadjusted));
		return buf.ToString();
	  }

	  // the notional, with trailing space if present
	  private string notional(SwapLeg leg)
	  {
		if (leg is RateCalculationSwapLeg)
		{
		  RateCalculationSwapLeg rcLeg = (RateCalculationSwapLeg) leg;
		  NotionalSchedule notionalSchedule = rcLeg.NotionalSchedule;
		  ValueSchedule amount = notionalSchedule.Amount;
		  double notional = amount.InitialValue;
		  string vary = amount.Steps.Count > 0 || amount.StepSequence.Present ? " variable" : "";
		  Currency currency = notionalSchedule.FxReset.map(fxr => fxr.ReferenceCurrency).orElse(rcLeg.Currency);
		  return SummarizerUtils.amount(currency, notional) + vary;
		}
		if (leg is RatePeriodSwapLeg)
		{
		  RatePeriodSwapLeg rpLeg = (RatePeriodSwapLeg) leg;
		  return SummarizerUtils.amount(rpLeg.PaymentPeriods.get(0).NotionalAmount);
		}
		return "";
	  }

	  // a summary of the leg
	  private string legSummary(SwapLeg leg)
	  {
		if (leg is RateCalculationSwapLeg)
		{
		  RateCalculationSwapLeg rcLeg = (RateCalculationSwapLeg) leg;
		  RateCalculation calculation = rcLeg.Calculation;
		  if (calculation is FixedRateCalculation)
		  {
			FixedRateCalculation calc = (FixedRateCalculation) calculation;
			string vary = calc.Rate.Steps.Count > 0 || calc.Rate.StepSequence.Present ? " variable" : "";
			return SummarizerUtils.percent(calc.Rate.InitialValue) + vary;
		  }
		  if (calculation is IborRateCalculation)
		  {
			IborRateCalculation calc = (IborRateCalculation) calculation;
			string gearing = calc.Gearing.map(g => " * " + SummarizerUtils.value(g.InitialValue)).orElse("");
			string spread = calc.Spread.map(s => " + " + SummarizerUtils.percent(s.InitialValue)).orElse("");
			return calc.Index.Name + gearing + spread;
		  }
		  if (calculation is OvernightRateCalculation)
		  {
			OvernightRateCalculation calc = (OvernightRateCalculation) calculation;
			string avg = calc.AccrualMethod == OvernightAccrualMethod.AVERAGED ? " avg" : "";
			string gearing = calc.Gearing.map(g => " * " + SummarizerUtils.value(g.InitialValue)).orElse("");
			string spread = calc.Spread.map(s => " + " + SummarizerUtils.percent(s.InitialValue)).orElse("");
			return calc.Index.Name + avg + gearing + spread;
		  }
		  if (calculation is InflationRateCalculation)
		  {
			InflationRateCalculation calc = (InflationRateCalculation) calculation;
			string gearing = calc.Gearing.map(g => " * " + SummarizerUtils.value(g.InitialValue)).orElse("");
			return calc.Index.Name + gearing;
		  }
		}
		if (leg is KnownAmountSwapLeg)
		{
		  KnownAmountSwapLeg kaLeg = (KnownAmountSwapLeg) leg;
		  string vary = kaLeg.Amount.Steps.Count > 0 || kaLeg.Amount.StepSequence.Present ? " variable" : "";
		  return SummarizerUtils.amount(kaLeg.Currency, kaLeg.Amount.InitialValue) + vary;
		}
		ImmutableSet<Index> allIndices = leg.allIndices();
		return allIndices.Empty ? "Fixed" : allIndices.ToString();
	  }

	  //-------------------------------------------------------------------------
	  public ResolvedSwap resolve(ReferenceData refData)
	  {
		// avoid streams as profiling showed a hotspot
		// most efficient to loop around legs once
		ImmutableList.Builder<ResolvedSwapLeg> resolvedLegs = ImmutableList.builder();
		ImmutableSet.Builder<Currency> currencies = ImmutableSet.builder();
		ImmutableSet.Builder<Index> indices = ImmutableSet.builder();
		foreach (SwapLeg leg in legs)
		{
		  ResolvedSwapLeg resolvedLeg = leg.resolve(refData);
		  resolvedLegs.add(resolvedLeg);
		  currencies.add(resolvedLeg.Currency);
		  leg.collectIndices(indices);
		}
		return new ResolvedSwap(resolvedLegs.build(), currencies.build(), indices.build());
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Swap}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Swap.Meta meta()
	  {
		return Swap.Meta.INSTANCE;
	  }

	  static Swap()
	  {
		MetaBean.register(Swap.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static Swap.Builder builder()
	  {
		return new Swap.Builder();
	  }

	  private Swap<T1>(IList<T1> legs) where T1 : SwapLeg
	  {
		JodaBeanUtils.notEmpty(legs, "legs");
		this.legs = ImmutableList.copyOf(legs);
	  }

	  public override Swap.Meta metaBean()
	  {
		return Swap.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legs of the swap.
	  /// <para>
	  /// A swap consists of one or more legs.
	  /// The legs of a swap are essentially unordered, however it is more efficient
	  /// and closer to user expectation to treat them as being ordered.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public ImmutableList<SwapLeg> Legs
	  {
		  get
		  {
			return legs;
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
		  Swap other = (Swap) obj;
		  return JodaBeanUtils.equal(legs, other.legs);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legs);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("Swap{");
		buf.Append("legs").Append('=').Append(JodaBeanUtils.ToString(legs));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Swap}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  legs_Renamed = DirectMetaProperty.ofImmutable(this, "legs", typeof(Swap), (Type) typeof(ImmutableList));
			  startDate_Renamed = DirectMetaProperty.ofDerived(this, "startDate", typeof(Swap), typeof(AdjustableDate));
			  endDate_Renamed = DirectMetaProperty.ofDerived(this, "endDate", typeof(Swap), typeof(AdjustableDate));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "legs", "startDate", "endDate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code legs} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SwapLeg>> legs = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "legs", Swap.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SwapLeg>> legs_Renamed;
		/// <summary>
		/// The meta-property for the {@code startDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> startDate_Renamed;
		/// <summary>
		/// The meta-property for the {@code endDate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<AdjustableDate> endDate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "legs", "startDate", "endDate");
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
			case 3317797: // legs
			  return legs_Renamed;
			case -2129778896: // startDate
			  return startDate_Renamed;
			case -1607727319: // endDate
			  return endDate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override Swap.Builder builder()
		{
		  return new Swap.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Swap);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code legs} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SwapLeg>> legs()
		{
		  return legs_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code startDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> startDate()
		{
		  return startDate_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code endDate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<AdjustableDate> endDate()
		{
		  return endDate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3317797: // legs
			  return ((Swap) bean).Legs;
			case -2129778896: // startDate
			  return ((Swap) bean).StartDate;
			case -1607727319: // endDate
			  return ((Swap) bean).EndDate;
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
	  /// The bean-builder for {@code Swap}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<Swap>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends SwapLeg> legs = com.google.common.collect.ImmutableList.of();
		internal IList<SwapLeg> legs_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(Swap beanToCopy)
		{
		  this.legs_Renamed = beanToCopy.Legs;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3317797: // legs
			  return legs_Renamed;
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
			case 3317797: // legs
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.legs = (java.util.List<? extends SwapLeg>) newValue;
			  this.legs_Renamed = (IList<SwapLeg>) newValue;
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

		public override Swap build()
		{
		  return new Swap(legs_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the legs of the swap.
		/// <para>
		/// A swap consists of one or more legs.
		/// The legs of a swap are essentially unordered, however it is more efficient
		/// and closer to user expectation to treat them as being ordered.
		/// </para>
		/// </summary>
		/// <param name="legs">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legs<T1>(IList<T1> legs) where T1 : SwapLeg
		{
		  JodaBeanUtils.notEmpty(legs, "legs");
		  this.legs_Renamed = legs;
		  return this;
		}

		/// <summary>
		/// Sets the {@code legs} property in the builder
		/// from an array of objects. </summary>
		/// <param name="legs">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legs(params SwapLeg[] legs)
		{
		  return this.legs(ImmutableList.copyOf(legs));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(64);
		  buf.Append("Swap.Builder{");
		  buf.Append("legs").Append('=').Append(JodaBeanUtils.ToString(legs_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}