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
	using FxRate = com.opengamma.strata.basics.currency.FxRate;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
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
	using XCcyIborIborSwapTemplate = com.opengamma.strata.product.swap.type.XCcyIborIborSwapTemplate;

	/// <summary>
	/// A curve node whose instrument is a cross-currency Ibor-Ibor interest rate swap.
	/// <para>
	/// Two market quotes are required, one for the spread and one for the FX rate.
	/// </para>
	/// <para>
	/// The spread or market quote is on the first Ibor leg.
	/// </para>
	/// <para>
	/// The trade produced by the node will be a spread receiver (SELL) for a positive quantity
	/// and a payer (BUY) for a negative quantity.
	/// This convention is line with other nodes where a positive quantity is similar to long a bond or deposit.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class XCcyIborIborSwapCurveNode implements com.opengamma.strata.market.curve.CurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class XCcyIborIborSwapCurveNode : CurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.swap.type.XCcyIborIborSwapTemplate template;
		private readonly XCcyIborIborSwapTemplate template;
	  /// <summary>
	  /// The identifier used to obtain the FX rate market value, defaulted from the template.
	  /// This only needs to be specified if using multiple market data sources.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.FxRateId fxRateId;
	  private readonly FxRateId fxRateId;
	  /// <summary>
	  /// The identifier of the market data value which provides the spread.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableId spreadId;
	  private readonly ObservableId spreadId;
	  /// <summary>
	  /// The additional spread added to the market quote.
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
	  /// Returns a curve node for a cross-currency Ibor-Ibor interest rate swap using the
	  /// specified instrument template and rate.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template used for building the instrument for the node </param>
	  /// <param name="spreadId">  the identifier of the market spread used when building the instrument for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static XCcyIborIborSwapCurveNode of(XCcyIborIborSwapTemplate template, ObservableId spreadId)
	  {
		return of(template, spreadId, 0d);
	  }

	  /// <summary>
	  /// Returns a curve node for a cross-currency Ibor-Ibor interest rate swap using the
	  /// specified instrument template, rate key and spread.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="spreadId">  the identifier of the market spread used when building the instrument for the node </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the market quote </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static XCcyIborIborSwapCurveNode of(XCcyIborIborSwapTemplate template, ObservableId spreadId, double additionalSpread)
	  {

		return builder().template(template).spreadId(spreadId).additionalSpread(additionalSpread).build();
	  }

	  /// <summary>
	  /// Returns a curve node for a cross-currency Ibor-Ibor interest rate swap using the
	  /// specified instrument template, rate key, spread and label.
	  /// </summary>
	  /// <param name="template">  the template defining the node instrument </param>
	  /// <param name="spreadId">  the identifier of the market spread used when building the instrument for the node </param>
	  /// <param name="additionalSpread">  the additional spread amount added to the market quote </param>
	  /// <param name="label">  the label to use for the node, if null or empty an appropriate default label will be used </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static XCcyIborIborSwapCurveNode of(XCcyIborIborSwapTemplate template, ObservableId spreadId, double additionalSpread, string label)
	  {

		FxRateId fxRateId = FxRateId.of(template.CurrencyPair);
		return new XCcyIborIborSwapCurveNode(template, fxRateId, spreadId, additionalSpread, label, CurveNodeDate.END, CurveNodeDateOrder.DEFAULT);
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
		if (builder.template_Renamed != null)
		{
		  if (string.ReferenceEquals(builder.label_Renamed, null))
		  {
			builder.label_Renamed = builder.template_Renamed.Tenor.ToString();
		  }
		  if (builder.fxRateId_Renamed == null)
		  {
			builder.fxRateId_Renamed = FxRateId.of(builder.template_Renamed.CurrencyPair);
		  }
		  else
		  {
			ArgChecker.isTrue(builder.fxRateId_Renamed.Pair.toConventional().Equals(builder.template_Renamed.CurrencyPair.toConventional()), "FxRateId currency pair '{}' must match that of the template '{}'", builder.fxRateId_Renamed.Pair, builder.template_Renamed.CurrencyPair);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Set<? extends com.opengamma.strata.data.MarketDataId<?>> requirements()
	  public ISet<MarketDataId<object>> requirements()
	  {
		return ImmutableSet.of(fxRateId, spreadId);
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
		SwapTrade trade = template.createTrade(valuationDate, BuySell.BUY, 1, 1, 0, refData);
		return trade.Product.EndDate.adjusted(refData);
	  }

	  // calculate the last fixing date
	  private LocalDate calculateLastFixingDate(LocalDate valuationDate, ReferenceData refData)
	  {
		SwapTrade trade = template.createTrade(valuationDate, BuySell.BUY, 1, 1, 0, refData);
		SwapLeg iborLeg = trade.Product.getLegs(SwapLegType.IBOR).get(1);
		// Select the 'second' Ibor leg, i.e. the flat leg
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
		double marketQuote = marketData.getValue(spreadId) + additionalSpread;
		FxRate fxRate = marketData.getValue(fxRateId);
		double rate = fxRate.fxRate(template.CurrencyPair);
		BuySell buySell = quantity > 0 ? BuySell.SELL : BuySell.BUY;
		return template.createTrade(marketData.ValuationDate, buySell, Math.Abs(quantity), rate, marketQuote, refData);
	  }

	  public ResolvedSwapTrade resolvedTrade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		return trade(quantity, marketData, refData).resolve(refData);
	  }

	  public double initialGuess(MarketData marketData, ValueType valueType)
	  {
		if (ValueType.DISCOUNT_FACTOR.Equals(valueType))
		{
		  return 1.0d;
		}
		return 0.0d;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this node with the specified date.
	  /// </summary>
	  /// <param name="date">  the date to use </param>
	  /// <returns> the node based on this node with the specified date </returns>
	  public XCcyIborIborSwapCurveNode withDate(CurveNodeDate date)
	  {
		return new XCcyIborIborSwapCurveNode(template, fxRateId, spreadId, additionalSpread, label, date, dateOrder);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code XCcyIborIborSwapCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static XCcyIborIborSwapCurveNode.Meta meta()
	  {
		return XCcyIborIborSwapCurveNode.Meta.INSTANCE;
	  }

	  static XCcyIborIborSwapCurveNode()
	  {
		MetaBean.register(XCcyIborIborSwapCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static XCcyIborIborSwapCurveNode.Builder builder()
	  {
		return new XCcyIborIborSwapCurveNode.Builder();
	  }

	  private XCcyIborIborSwapCurveNode(XCcyIborIborSwapTemplate template, FxRateId fxRateId, ObservableId spreadId, double additionalSpread, string label, CurveNodeDate date, CurveNodeDateOrder dateOrder)
	  {
		JodaBeanUtils.notNull(template, "template");
		JodaBeanUtils.notNull(fxRateId, "fxRateId");
		JodaBeanUtils.notNull(spreadId, "spreadId");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(dateOrder, "dateOrder");
		this.template = template;
		this.fxRateId = fxRateId;
		this.spreadId = spreadId;
		this.additionalSpread = additionalSpread;
		this.label = label;
		this.date_Renamed = date;
		this.dateOrder = dateOrder;
	  }

	  public override XCcyIborIborSwapCurveNode.Meta metaBean()
	  {
		return XCcyIborIborSwapCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for the swap associated with this node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public XCcyIborIborSwapTemplate Template
	  {
		  get
		  {
			return template;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier used to obtain the FX rate market value, defaulted from the template.
	  /// This only needs to be specified if using multiple market data sources. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxRateId FxRateId
	  {
		  get
		  {
			return fxRateId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier of the market data value which provides the spread. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableId SpreadId
	  {
		  get
		  {
			return spreadId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the additional spread added to the market quote. </summary>
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
		  XCcyIborIborSwapCurveNode other = (XCcyIborIborSwapCurveNode) obj;
		  return JodaBeanUtils.equal(template, other.template) && JodaBeanUtils.equal(fxRateId, other.fxRateId) && JodaBeanUtils.equal(spreadId, other.spreadId) && JodaBeanUtils.equal(additionalSpread, other.additionalSpread) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(date_Renamed, other.date_Renamed) && JodaBeanUtils.equal(dateOrder, other.dateOrder);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(template);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRateId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spreadId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(additionalSpread);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateOrder);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("XCcyIborIborSwapCurveNode{");
		buf.Append("template").Append('=').Append(template).Append(',').Append(' ');
		buf.Append("fxRateId").Append('=').Append(fxRateId).Append(',').Append(' ');
		buf.Append("spreadId").Append('=').Append(spreadId).Append(',').Append(' ');
		buf.Append("additionalSpread").Append('=').Append(additionalSpread).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(date_Renamed).Append(',').Append(' ');
		buf.Append("dateOrder").Append('=').Append(JodaBeanUtils.ToString(dateOrder));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code XCcyIborIborSwapCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(XCcyIborIborSwapCurveNode), typeof(XCcyIborIborSwapTemplate));
			  fxRateId_Renamed = DirectMetaProperty.ofImmutable(this, "fxRateId", typeof(XCcyIborIborSwapCurveNode), typeof(FxRateId));
			  spreadId_Renamed = DirectMetaProperty.ofImmutable(this, "spreadId", typeof(XCcyIborIborSwapCurveNode), typeof(ObservableId));
			  additionalSpread_Renamed = DirectMetaProperty.ofImmutable(this, "additionalSpread", typeof(XCcyIborIborSwapCurveNode), Double.TYPE);
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(XCcyIborIborSwapCurveNode), typeof(string));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(XCcyIborIborSwapCurveNode), typeof(CurveNodeDate));
			  dateOrder_Renamed = DirectMetaProperty.ofImmutable(this, "dateOrder", typeof(XCcyIborIborSwapCurveNode), typeof(CurveNodeDateOrder));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "template", "fxRateId", "spreadId", "additionalSpread", "label", "date", "dateOrder");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code template} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<XCcyIborIborSwapTemplate> template_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxRateId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxRateId> fxRateId_Renamed;
		/// <summary>
		/// The meta-property for the {@code spreadId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableId> spreadId_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "template", "fxRateId", "spreadId", "additionalSpread", "label", "date", "dateOrder");
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
			case -1054985843: // fxRateId
			  return fxRateId_Renamed;
			case -1759090194: // spreadId
			  return spreadId_Renamed;
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

		public override XCcyIborIborSwapCurveNode.Builder builder()
		{
		  return new XCcyIborIborSwapCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(XCcyIborIborSwapCurveNode);
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
		public MetaProperty<XCcyIborIborSwapTemplate> template()
		{
		  return template_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fxRateId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxRateId> fxRateId()
		{
		  return fxRateId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code spreadId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableId> spreadId()
		{
		  return spreadId_Renamed;
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
			  return ((XCcyIborIborSwapCurveNode) bean).Template;
			case -1054985843: // fxRateId
			  return ((XCcyIborIborSwapCurveNode) bean).FxRateId;
			case -1759090194: // spreadId
			  return ((XCcyIborIborSwapCurveNode) bean).SpreadId;
			case 291232890: // additionalSpread
			  return ((XCcyIborIborSwapCurveNode) bean).AdditionalSpread;
			case 102727412: // label
			  return ((XCcyIborIborSwapCurveNode) bean).Label;
			case 3076014: // date
			  return ((XCcyIborIborSwapCurveNode) bean).Date;
			case -263699392: // dateOrder
			  return ((XCcyIborIborSwapCurveNode) bean).DateOrder;
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
	  /// The bean-builder for {@code XCcyIborIborSwapCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<XCcyIborIborSwapCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal XCcyIborIborSwapTemplate template_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxRateId fxRateId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableId spreadId_Renamed;
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
		internal Builder(XCcyIborIborSwapCurveNode beanToCopy)
		{
		  this.template_Renamed = beanToCopy.Template;
		  this.fxRateId_Renamed = beanToCopy.FxRateId;
		  this.spreadId_Renamed = beanToCopy.SpreadId;
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
			case -1054985843: // fxRateId
			  return fxRateId_Renamed;
			case -1759090194: // spreadId
			  return spreadId_Renamed;
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
			  this.template_Renamed = (XCcyIborIborSwapTemplate) newValue;
			  break;
			case -1054985843: // fxRateId
			  this.fxRateId_Renamed = (FxRateId) newValue;
			  break;
			case -1759090194: // spreadId
			  this.spreadId_Renamed = (ObservableId) newValue;
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

		public override XCcyIborIborSwapCurveNode build()
		{
		  preBuild(this);
		  return new XCcyIborIborSwapCurveNode(template_Renamed, fxRateId_Renamed, spreadId_Renamed, additionalSpread_Renamed, label_Renamed, date_Renamed, dateOrder_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the template for the swap associated with this node. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(XCcyIborIborSwapTemplate template)
		{
		  JodaBeanUtils.notNull(template, "template");
		  this.template_Renamed = template;
		  return this;
		}

		/// <summary>
		/// Sets the identifier used to obtain the FX rate market value, defaulted from the template.
		/// This only needs to be specified if using multiple market data sources. </summary>
		/// <param name="fxRateId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fxRateId(FxRateId fxRateId)
		{
		  JodaBeanUtils.notNull(fxRateId, "fxRateId");
		  this.fxRateId_Renamed = fxRateId;
		  return this;
		}

		/// <summary>
		/// Sets the identifier of the market data value which provides the spread. </summary>
		/// <param name="spreadId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder spreadId(ObservableId spreadId)
		{
		  JodaBeanUtils.notNull(spreadId, "spreadId");
		  this.spreadId_Renamed = spreadId;
		  return this;
		}

		/// <summary>
		/// Sets the additional spread added to the market quote. </summary>
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
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("XCcyIborIborSwapCurveNode.Builder{");
		  buf.Append("template").Append('=').Append(JodaBeanUtils.ToString(template_Renamed)).Append(',').Append(' ');
		  buf.Append("fxRateId").Append('=').Append(JodaBeanUtils.ToString(fxRateId_Renamed)).Append(',').Append(' ');
		  buf.Append("spreadId").Append('=').Append(JodaBeanUtils.ToString(spreadId_Renamed)).Append(',').Append(' ');
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