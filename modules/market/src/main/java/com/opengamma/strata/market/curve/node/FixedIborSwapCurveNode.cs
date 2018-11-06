using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
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
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using IborRateComputation = com.opengamma.strata.product.rate.IborRateComputation;
	using RateAccrualPeriod = com.opengamma.strata.product.swap.RateAccrualPeriod;
	using RatePaymentPeriod = com.opengamma.strata.product.swap.RatePaymentPeriod;
	using ResolvedSwapLeg = com.opengamma.strata.product.swap.ResolvedSwapLeg;
	using ResolvedSwapTrade = com.opengamma.strata.product.swap.ResolvedSwapTrade;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;
	using SwapPaymentPeriod = com.opengamma.strata.product.swap.SwapPaymentPeriod;
	using SwapTrade = com.opengamma.strata.product.swap.SwapTrade;
	using FixedIborSwapTemplate = com.opengamma.strata.product.swap.type.FixedIborSwapTemplate;

	/// <summary>
	/// A curve node whose instrument is a Fixed-Ibor interest rate swap.
	/// <para>
	/// The trade produced by the node will be a receiver (SELL) for a positive quantity
	/// and a payer (BUY) for a negative quantity.
	/// This convention is line with other nodes where a positive quantity is similar to long a bond or deposit.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FixedIborSwapCurveNode implements com.opengamma.strata.market.curve.CurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FixedIborSwapCurveNode : CurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.type.FixedIborSwapTemplate template;
		private readonly FixedIborSwapTemplate template;
	  /// <summary>
	  /// The identifier of the market data value that provides the rate.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableId rateId;
	  private readonly ObservableId rateId;
	  /// <summary>
	  /// The additional spread added to the rate.
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
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.CurveNodeDate date;
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
	  /// Returns a curve node for a Fixed-Ibor interest rate swap using the
	  /// specified instrument template and rate.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template used for building the instrument for the node </param>
	  /// <param name="rateId">  the identifier of the market rate used when building the instrument for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedIborSwapCurveNode of(FixedIborSwapTemplate template, ObservableId rateId)
	  {
		return of(template, rateId, 0d);
	  }

	  /// <summary>
	  /// Returns a curve node for a Fixed-Ibor interest rate swap using the
	  /// specified instrument template, rate key and spread.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="rateId">  the identifier of the market data providing the rate for the node instrument </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the rate </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedIborSwapCurveNode of(FixedIborSwapTemplate template, ObservableId rateId, double additionalSpread)
	  {
		return builder().template(template).rateId(rateId).additionalSpread(additionalSpread).build();
	  }

	  /// <summary>
	  /// Returns a curve node for a Fixed-Ibor interest rate swap using the
	  /// specified instrument template, rate key, spread and label.
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="rateId">  the identifier of the market data providing the rate for the node instrument </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the rate </param>
	  /// <param name="label">  the label to use for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FixedIborSwapCurveNode of(FixedIborSwapTemplate template, ObservableId rateId, double additionalSpread, string label)
	  {

		return new FixedIborSwapCurveNode(template, rateId, additionalSpread, label, CurveNodeDate.END, CurveNodeDateOrder.DEFAULT);
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
		SwapLeg iborLeg = trade.Product.getLegs(SwapLegType.IBOR).get(0);
		ResolvedSwapLeg iborLegExpanded = iborLeg.resolve(refData);
		IList<SwapPaymentPeriod> periods = iborLegExpanded.PaymentPeriods;
		int nbPeriods = periods.Count;
		RatePaymentPeriod lastPeriod = (RatePaymentPeriod) periods[nbPeriods - 1];
		IList<RateAccrualPeriod> accruals = lastPeriod.AccrualPeriods;
		int nbAccruals = accruals.Count;
		IborRateComputation ibor = (IborRateComputation) accruals[nbAccruals - 1].RateComputation;
		return ibor.FixingDate;
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
		if (ValueType.ZERO_RATE.Equals(valueType) || ValueType.FORWARD_RATE.Equals(valueType))
		{
		  return marketData.getValue(rateId);
		}
		if (ValueType.DISCOUNT_FACTOR.Equals(valueType))
		{
		  double approximateMaturity = template.PeriodToStart.plus(template.Tenor).toTotalMonths() / 12.0d;
		  return Math.Exp(-approximateMaturity * marketData.getValue(rateId));
		}
		return 0d;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this node with the specified date.
	  /// </summary>
	  /// <param name="date">  the date to use </param>
	  /// <returns> the node based on this node with the specified date </returns>
	  public FixedIborSwapCurveNode withDate(CurveNodeDate date)
	  {
		return new FixedIborSwapCurveNode(template, rateId, additionalSpread, label, date, dateOrder);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FixedIborSwapCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FixedIborSwapCurveNode.Meta meta()
	  {
		return FixedIborSwapCurveNode.Meta.INSTANCE;
	  }

	  static FixedIborSwapCurveNode()
	  {
		MetaBean.register(FixedIborSwapCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FixedIborSwapCurveNode.Builder builder()
	  {
		return new FixedIborSwapCurveNode.Builder();
	  }

	  private FixedIborSwapCurveNode(FixedIborSwapTemplate template, ObservableId rateId, double additionalSpread, string label, CurveNodeDate date, CurveNodeDateOrder dateOrder)
	  {
		JodaBeanUtils.notNull(template, "template");
		JodaBeanUtils.notNull(rateId, "rateId");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(date, "date");
		JodaBeanUtils.notNull(dateOrder, "dateOrder");
		this.template = template;
		this.rateId = rateId;
		this.additionalSpread = additionalSpread;
		this.label = label;
		this.date_Renamed = date;
		this.dateOrder = dateOrder;
	  }

	  public override FixedIborSwapCurveNode.Meta metaBean()
	  {
		return FixedIborSwapCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for the swap associated with this node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FixedIborSwapTemplate Template
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
	  /// Gets the additional spread added to the rate. </summary>
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
	  /// <returns> the value of the property, not null </returns>
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
		  FixedIborSwapCurveNode other = (FixedIborSwapCurveNode) obj;
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
		buf.Append("FixedIborSwapCurveNode{");
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
	  /// The meta-bean for {@code FixedIborSwapCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(FixedIborSwapCurveNode), typeof(FixedIborSwapTemplate));
			  rateId_Renamed = DirectMetaProperty.ofImmutable(this, "rateId", typeof(FixedIborSwapCurveNode), typeof(ObservableId));
			  additionalSpread_Renamed = DirectMetaProperty.ofImmutable(this, "additionalSpread", typeof(FixedIborSwapCurveNode), Double.TYPE);
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(FixedIborSwapCurveNode), typeof(string));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(FixedIborSwapCurveNode), typeof(CurveNodeDate));
			  dateOrder_Renamed = DirectMetaProperty.ofImmutable(this, "dateOrder", typeof(FixedIborSwapCurveNode), typeof(CurveNodeDateOrder));
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
		internal MetaProperty<FixedIborSwapTemplate> template_Renamed;
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

		public override FixedIborSwapCurveNode.Builder builder()
		{
		  return new FixedIborSwapCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FixedIborSwapCurveNode);
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
		public MetaProperty<FixedIborSwapTemplate> template()
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
			  return ((FixedIborSwapCurveNode) bean).Template;
			case -938107365: // rateId
			  return ((FixedIborSwapCurveNode) bean).RateId;
			case 291232890: // additionalSpread
			  return ((FixedIborSwapCurveNode) bean).AdditionalSpread;
			case 102727412: // label
			  return ((FixedIborSwapCurveNode) bean).Label;
			case 3076014: // date
			  return ((FixedIborSwapCurveNode) bean).Date;
			case -263699392: // dateOrder
			  return ((FixedIborSwapCurveNode) bean).DateOrder;
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
	  /// The bean-builder for {@code FixedIborSwapCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FixedIborSwapCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FixedIborSwapTemplate template_Renamed;
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
		internal Builder(FixedIborSwapCurveNode beanToCopy)
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
			  this.template_Renamed = (FixedIborSwapTemplate) newValue;
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

		public override FixedIborSwapCurveNode build()
		{
		  preBuild(this);
		  return new FixedIborSwapCurveNode(template_Renamed, rateId_Renamed, additionalSpread_Renamed, label_Renamed, date_Renamed, dateOrder_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the template for the swap associated with this node. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(FixedIborSwapTemplate template)
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
		/// Sets the additional spread added to the rate. </summary>
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
		/// <param name="date">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder date(CurveNodeDate date)
		{
		  JodaBeanUtils.notNull(date, "date");
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
		  buf.Append("FixedIborSwapCurveNode.Builder{");
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