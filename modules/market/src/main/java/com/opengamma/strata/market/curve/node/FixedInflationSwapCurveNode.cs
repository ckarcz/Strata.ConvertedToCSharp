using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.node
{

	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using PriceIndex = com.opengamma.strata.basics.index.PriceIndex;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using IndexQuoteId = com.opengamma.strata.market.observable.IndexQuoteId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using InflationEndInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationEndInterpolatedRateComputation;
	using InflationEndMonthRateComputation = com.opengamma.strata.product.rate.InflationEndMonthRateComputation;
	using InflationInterpolatedRateComputation = com.opengamma.strata.product.rate.InflationInterpolatedRateComputation;
	using InflationMonthlyRateComputation = com.opengamma.strata.product.rate.InflationMonthlyRateComputation;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedInflationSwapTemplate = com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate;

	/// <summary>
	/// A curve node whose instrument is a Fixed-Inflation swap.
	/// <para>
	/// The trade produced by the node will be a fixed receiver (SELL) for a positive quantity
	/// and a fixed payer (BUY) for a negative quantity.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FixedInflationSwapCurveNode implements com.opengamma.strata.market.curve.CurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedInflationSwapCurveNode : CurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.type.FixedInflationSwapTemplate template;
		private readonly FixedInflationSwapTemplate template;
	  /// <summary>
	  /// The identifier of the market data value that provides the rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableId rateId;
	  private readonly ObservableId rateId;
	  /// <summary>
	  /// The additional spread added to the fixed rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double additionalSpread;
	  private readonly double additionalSpread;
	  /// <summary>
	  /// The label to use for the node, defaulted.
	  /// <para>
	  /// When building, this will default based on the tenor if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
	  private readonly string label;
	  /// <summary>
	  /// The method by which the date of the node is calculated, defaulted to 'End'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.CurveNodeDate date;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly CurveNodeDate date_Renamed;
	  /// <summary>
	  /// The date order rules, used to ensure that the dates in the curve are in order.
	  /// If not specified, this will default to <seealso cref="CurveNodeDateOrder#DEFAULT"/>.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.curve.CurveNodeDateOrder dateOrder;
	  private readonly CurveNodeDateOrder dateOrder;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve node for a Fixed-Inflation swap using the specified instrument template and rate key.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template used for building the instrument for the node </param>
	  /// <param name="rateId">  the identifier of the market rate used when building the instrument for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedInflationSwapCurveNode of(FixedInflationSwapTemplate template, ObservableId rateId)
	  {
		return of(template, rateId, 0d);
	  }

	  /// <summary>
	  /// Returns a curve node for a Fixed-Inflation swap using the specified instrument template, rate key and spread.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="rateId">  the identifier of the market data providing the rate for the node instrument </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the rate </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedInflationSwapCurveNode of(FixedInflationSwapTemplate template, ObservableId rateId, double additionalSpread)
	  {

		return builder().template(template).rateId(rateId).additionalSpread(additionalSpread).build();
	  }

	  /// <summary>
	  /// Returns a curve node for a Fixed-Inflation swap using the specified instrument template,
	  /// rate key, spread and label.
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="rateId">  the identifier of the market data providing the rate for the node instrument </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the rate </param>
	  /// <param name="label">  the label to use for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedInflationSwapCurveNode of(FixedInflationSwapTemplate template, ObservableId rateId, double additionalSpread, string label)
	  {

		return new FixedInflationSwapCurveNode(template, rateId, additionalSpread, label, CurveNodeDate.END, CurveNodeDateOrder.DEFAULT);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.date_Renamed = CurveNodeDate.END;
		builder.dateOrder_Renamed = CurveNodeDateOrder.DEFAULT;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (string.ReferenceEquals(builder.label_Renamed, null) && builder.template_Renamed != null)
		{
		  builder.label_Renamed = builder.template_Renamed.Tenor.ToString();
		}
	  }

	  //-------------------------------------------------------------------------
	  public ISet<ObservableId> requirements()
	  {
		return ImmutableSet.of(rateId);
	  }

	  public LocalDate date(LocalDate valuationDate, ReferenceData refData)
	  {
		return date_Renamed.calculate(() => calculateEnd(valuationDate, refData), () => calculateLastFixingDate(valuationDate, refData));
	  }

	  public DatedParameterMetadata metadata(LocalDate valuationDate, ReferenceData refData)
	  {
		LocalDate nodeDate = date(valuationDate, refData);
		if (date_Renamed.Fixed)
		{
		  return LabelDateParameterMetadata.of(nodeDate, label);
		}
		return TenorDateParameterMetadata.of(nodeDate, template.Tenor, label);
	  }

	  // calculate the end date
	  private LocalDate calculateEnd(LocalDate valuationDate, ReferenceData refData)
	  {
		SwapTrade trade = template.createTrade(valuationDate, BuySell.BUY, 1, 1, refData);
		return trade.Product.EndDate.adjusted(refData);
	  }

	  // calculate the last fixing date
	  private LocalDate calculateLastFixingDate(LocalDate valuationDate, ReferenceData refData)
	  {
		SwapTrade trade = template.createTrade(valuationDate, BuySell.BUY, 1, 1, refData);
		SwapLeg inflationLeg = trade.Product.getLegs(SwapLegType.INFLATION).get(0);
		ResolvedSwapLeg inflationLegExpanded = inflationLeg.resolve(refData);
		IList<SwapPaymentPeriod> periods = inflationLegExpanded.PaymentPeriods;
		int nbPeriods = periods.Count;
		RatePaymentPeriod lastPeriod = (RatePaymentPeriod) periods[nbPeriods - 1];
		IList<RateAccrualPeriod> accruals = lastPeriod.AccrualPeriods;
		int nbAccruals = accruals.Count;
		RateAccrualPeriod lastAccrual = accruals[nbAccruals - 1];
		if (lastAccrual.RateComputation is InflationMonthlyRateComputation)
		{
		  return ((InflationMonthlyRateComputation) lastAccrual.RateComputation).EndObservation.FixingMonth.atEndOfMonth();
		}
		if (lastAccrual.RateComputation is InflationInterpolatedRateComputation)
		{
		  return ((InflationInterpolatedRateComputation) lastAccrual.RateComputation).EndSecondObservation.FixingMonth.atEndOfMonth();
		}
		if (lastAccrual.RateComputation is InflationEndMonthRateComputation)
		{
		  return ((InflationEndMonthRateComputation) lastAccrual.RateComputation).EndObservation.FixingMonth.atEndOfMonth();
		}
		if (lastAccrual.RateComputation is InflationEndInterpolatedRateComputation)
		{
		  return ((InflationEndInterpolatedRateComputation) lastAccrual.RateComputation).EndSecondObservation.FixingMonth.atEndOfMonth();
		}
		throw new System.ArgumentException("Rate computation type not supported for last fixing date of an inflation swap.");
	  }

	  public SwapTrade trade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		double fixedRate = marketData.getValue(rateId) + additionalSpread;
		BuySell buySell = quantity > 0 ? BuySell.SELL : BuySell.BUY;
		return template.createTrade(marketData.ValuationDate, buySell, Math.Abs(quantity), fixedRate, refData);
	  }

	  public ResolvedSwapTrade resolvedTrade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		return trade(quantity, marketData, refData).resolve(refData);
	  }

	  public double initialGuess(MarketData marketData, ValueType valueType)
	  {
		if (ValueType.PRICE_INDEX.Equals(valueType))
		{
		  PriceIndex index = template.Convention.FloatingLeg.Index;
		  LocalDateDoubleTimeSeries ts = marketData.getTimeSeries(IndexQuoteId.of(index));
		  double latestIndex = ts.LatestValue;
		  double rate = marketData.getValue(rateId);
		  double year = template.Tenor.Period.Years;
		  return latestIndex * Math.Pow(1.0 + rate, year);
		}
		if (ValueType.ZERO_RATE.Equals(valueType))
		{
		  return marketData.getValue(rateId);
		}
		throw new System.ArgumentException("No default initial guess when value type is not 'PriceIndex' or 'ZeroRate'.");
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this node with the specified date.
	  /// </summary>
	  /// <param name="date">  the date to use </param>
	  /// <returns> the node based on this node with the specified date </returns>
	  public FixedInflationSwapCurveNode withDate(CurveNodeDate date)
	  {
		return new FixedInflationSwapCurveNode(template, rateId, additionalSpread, label, date, dateOrder);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedInflationSwapCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedInflationSwapCurveNode.Meta meta()
	  {
		return FixedInflationSwapCurveNode.Meta.INSTANCE;
	  }

	  static FixedInflationSwapCurveNode()
	  {
		MetaBean.register(FixedInflationSwapCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FixedInflationSwapCurveNode.Builder builder()
	  {
		return new FixedInflationSwapCurveNode.Builder();
	  }

	  private FixedInflationSwapCurveNode(FixedInflationSwapTemplate template, ObservableId rateId, double additionalSpread, string label, CurveNodeDate date, CurveNodeDateOrder dateOrder)
	  {
		JodaBeanUtils.notNull(template, "template");
		JodaBeanUtils.notNull(rateId, "rateId");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(dateOrder, "dateOrder");
		this.template = template;
		this.rateId = rateId;
		this.additionalSpread = additionalSpread;
		this.label = label;
		this.date_Renamed = date;
		this.dateOrder = dateOrder;
	  }

	  public override FixedInflationSwapCurveNode.Meta metaBean()
	  {
		return FixedInflationSwapCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for the swap associated with this node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedInflationSwapTemplate Template
	  {
		  get
		  {
			return template;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier of the market data value that provides the rate. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableId RateId
	  {
		  get
		  {
			return rateId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional spread added to the fixed rate. </summary>
	  /// <returns> the value of the property </returns>
	  public double AdditionalSpread
	  {
		  get
		  {
			return additionalSpread;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label to use for the node, defaulted.
	  /// <para>
	  /// When building, this will default based on the tenor if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public string Label
	  {
		  get
		  {
			return label;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the method by which the date of the node is calculated, defaulted to 'End'. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveNodeDate Date
	  {
		  get
		  {
			return date_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the date order rules, used to ensure that the dates in the curve are in order.
	  /// If not specified, this will default to <seealso cref="CurveNodeDateOrder#DEFAULT"/>. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveNodeDateOrder DateOrder
	  {
		  get
		  {
			return dateOrder;
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
		  FixedInflationSwapCurveNode other = (FixedInflationSwapCurveNode) obj;
		  return JodaBeanUtils.equal(template, other.template) && JodaBeanUtils.equal(rateId, other.rateId) && JodaBeanUtils.equal(additionalSpread, other.additionalSpread) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(date_Renamed, other.date_Renamed) && JodaBeanUtils.equal(dateOrder, other.dateOrder);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(template);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(additionalSpread);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateOrder);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("FixedInflationSwapCurveNode{");
		buf.Append("template").Append('=').Append(template).Append(',').Append(' ');
		buf.Append("rateId").Append('=').Append(rateId).Append(',').Append(' ');
		buf.Append("additionalSpread").Append('=').Append(additionalSpread).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(date_Renamed).Append(',').Append(' ');
		buf.Append("dateOrder").Append('=').Append(JodaBeanUtils.ToString(dateOrder));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedInflationSwapCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(FixedInflationSwapCurveNode), typeof(FixedInflationSwapTemplate));
			  rateId_Renamed = DirectMetaProperty.ofImmutable(this, "rateId", typeof(FixedInflationSwapCurveNode), typeof(ObservableId));
			  additionalSpread_Renamed = DirectMetaProperty.ofImmutable(this, "additionalSpread", typeof(FixedInflationSwapCurveNode), Double.TYPE);
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(FixedInflationSwapCurveNode), typeof(string));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(FixedInflationSwapCurveNode), typeof(CurveNodeDate));
			  dateOrder_Renamed = DirectMetaProperty.ofImmutable(this, "dateOrder", typeof(FixedInflationSwapCurveNode), typeof(CurveNodeDateOrder));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "template", "rateId", "additionalSpread", "label", "date", "dateOrder");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code template} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FixedInflationSwapTemplate> template_Renamed;
		/// <summary>
		/// The meta-property for the {@code rateId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableId> rateId_Renamed;
		/// <summary>
		/// The meta-property for the {@code additionalSpread} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> additionalSpread_Renamed;
		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
		/// <summary>
		/// The meta-property for the {@code date} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveNodeDate> date_Renamed;
		/// <summary>
		/// The meta-property for the {@code dateOrder} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveNodeDateOrder> dateOrder_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "template", "rateId", "additionalSpread", "label", "date", "dateOrder");
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
			case -1321546630: // template
			  return template_Renamed;
			case -938107365: // rateId
			  return rateId_Renamed;
			case 291232890: // additionalSpread
			  return additionalSpread_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case 3076014: // date
			  return date_Renamed;
			case -263699392: // dateOrder
			  return dateOrder_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FixedInflationSwapCurveNode.Builder builder()
		{
		  return new FixedInflationSwapCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedInflationSwapCurveNode);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code template} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FixedInflationSwapTemplate> template()
		{
		  return template_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rateId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableId> rateId()
		{
		  return rateId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code additionalSpread} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> additionalSpread()
		{
		  return additionalSpread_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code date} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveNodeDate> date()
		{
		  return date_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dateOrder} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveNodeDateOrder> dateOrder()
		{
		  return dateOrder_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  return ((FixedInflationSwapCurveNode) bean).Template;
			case -938107365: // rateId
			  return ((FixedInflationSwapCurveNode) bean).RateId;
			case 291232890: // additionalSpread
			  return ((FixedInflationSwapCurveNode) bean).AdditionalSpread;
			case 102727412: // label
			  return ((FixedInflationSwapCurveNode) bean).Label;
			case 3076014: // date
			  return ((FixedInflationSwapCurveNode) bean).Date;
			case -263699392: // dateOrder
			  return ((FixedInflationSwapCurveNode) bean).DateOrder;
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
	  /// The bean-builder for {@code FixedInflationSwapCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FixedInflationSwapCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedInflationSwapTemplate template_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableId rateId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double additionalSpread_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string label_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveNodeDate date_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveNodeDateOrder dateOrder_Renamed;

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
		internal Builder(FixedInflationSwapCurveNode beanToCopy)
		{
		  this.template_Renamed = beanToCopy.Template;
		  this.rateId_Renamed = beanToCopy.RateId;
		  this.additionalSpread_Renamed = beanToCopy.AdditionalSpread;
		  this.label_Renamed = beanToCopy.Label;
		  this.date_Renamed = beanToCopy.Date;
		  this.dateOrder_Renamed = beanToCopy.DateOrder;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  return template_Renamed;
			case -938107365: // rateId
			  return rateId_Renamed;
			case 291232890: // additionalSpread
			  return additionalSpread_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case 3076014: // date
			  return date_Renamed;
			case -263699392: // dateOrder
			  return dateOrder_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  this.template_Renamed = (FixedInflationSwapTemplate) newValue;
			  break;
			case -938107365: // rateId
			  this.rateId_Renamed = (ObservableId) newValue;
			  break;
			case 291232890: // additionalSpread
			  this.additionalSpread_Renamed = (double?) newValue.Value;
			  break;
			case 102727412: // label
			  this.label_Renamed = (string) newValue;
			  break;
			case 3076014: // date
			  this.date_Renamed = (CurveNodeDate) newValue;
			  break;
			case -263699392: // dateOrder
			  this.dateOrder_Renamed = (CurveNodeDateOrder) newValue;
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

		public override FixedInflationSwapCurveNode build()
		{
		  preBuild(this);
		  return new FixedInflationSwapCurveNode(template_Renamed, rateId_Renamed, additionalSpread_Renamed, label_Renamed, date_Renamed, dateOrder_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the template for the swap associated with this node. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(FixedInflationSwapTemplate template)
		{
		  JodaBeanUtils.notNull(template, "template");
		  this.template_Renamed = template;
		  return this;
		}

		/// <summary>
		/// Sets the identifier of the market data value that provides the rate. </summary>
		/// <param name="rateId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rateId(ObservableId rateId)
		{
		  JodaBeanUtils.notNull(rateId, "rateId");
		  this.rateId_Renamed = rateId;
		  return this;
		}

		/// <summary>
		/// Sets the additional spread added to the fixed rate. </summary>
		/// <param name="additionalSpread">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder additionalSpread(double additionalSpread)
		{
		  this.additionalSpread_Renamed = additionalSpread;
		  return this;
		}

		/// <summary>
		/// Sets the label to use for the node, defaulted.
		/// <para>
		/// When building, this will default based on the tenor if not specified.
		/// </para>
		/// </summary>
		/// <param name="label">  the new value, not empty </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder label(string label)
		{
		  JodaBeanUtils.notEmpty(label, "label");
		  this.label_Renamed = label;
		  return this;
		}

		/// <summary>
		/// Sets the method by which the date of the node is calculated, defaulted to 'End'. </summary>
		/// <param name="date">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder date(CurveNodeDate date)
		{
		  this.date_Renamed = date;
		  return this;
		}

		/// <summary>
		/// Sets the date order rules, used to ensure that the dates in the curve are in order.
		/// If not specified, this will default to <seealso cref="CurveNodeDateOrder#DEFAULT"/>. </summary>
		/// <param name="dateOrder">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dateOrder(CurveNodeDateOrder dateOrder)
		{
		  JodaBeanUtils.notNull(dateOrder, "dateOrder");
		  this.dateOrder_Renamed = dateOrder;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("FixedInflationSwapCurveNode.Builder{");
		  buf.Append("template").Append('=').Append(JodaBeanUtils.ToString(template_Renamed)).Append(',').Append(' ');
		  buf.Append("rateId").Append('=').Append(JodaBeanUtils.ToString(rateId_Renamed)).Append(',').Append(' ');
		  buf.Append("additionalSpread").Append('=').Append(JodaBeanUtils.ToString(additionalSpread_Renamed)).Append(',').Append(' ');
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label_Renamed)).Append(',').Append(' ');
		  buf.Append("date").Append('=').Append(JodaBeanUtils.ToString(date_Renamed)).Append(',').Append(' ');
		  buf.Append("dateOrder").Append('=').Append(JodaBeanUtils.ToString(dateOrder_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}