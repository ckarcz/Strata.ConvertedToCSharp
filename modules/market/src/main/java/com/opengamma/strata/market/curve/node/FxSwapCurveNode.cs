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
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using FxRateId = com.opengamma.strata.data.FxRateId;
	using MarketData = com.opengamma.strata.data.MarketData;
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using FxSwapTrade = com.opengamma.strata.product.fx.FxSwapTrade;
	using ResolvedFxSwapTrade = com.opengamma.strata.product.fx.ResolvedFxSwapTrade;
	using FxSwapTemplate = com.opengamma.strata.product.fx.type.FxSwapTemplate;

	/// <summary>
	/// A curve node whose instrument is an FX Swap.
	/// <para>
	/// The trade produced by the node will pay near and receive far in the second currency (BUY)
	/// for a positive quantity and a receive near and pay far (SELL) for a negative quantity.
	/// This convention is line with other nodes where a positive quantity is similar to long a bond or deposit,
	/// here the long deposit-like is in the second currency.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class FxSwapCurveNode implements com.opengamma.strata.market.curve.CurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class FxSwapCurveNode : CurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.fx.type.FxSwapTemplate template;
		private readonly FxSwapTemplate template;
	  /// <summary>
	  /// The identifier used to obtain the FX rate market value, defaulted from the template.
	  /// This only needs to be specified if using multiple market data sources.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.FxRateId fxRateId;
	  private readonly FxRateId fxRateId;
	  /// <summary>
	  /// The identifier of the market data value which provides the FX forward points.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableId farForwardPointsId;
	  private readonly ObservableId farForwardPointsId;
	  /// <summary>
	  /// The label to use for the node, defaulted.
	  /// <para>
	  /// When building, this will default based on the far period if not specified.
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
	  /// Returns a curve node for an FX Swap using the specified instrument template and keys.
	  /// <para>
	  /// A suitable default label will be created.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="template">  the template used for building the instrument for the node </param>
	  /// <param name="farForwardPointsId">  the identifier of the FX points at the far date </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FxSwapCurveNode of(FxSwapTemplate template, ObservableId farForwardPointsId)
	  {
		return builder().template(template).farForwardPointsId(farForwardPointsId).build();
	  }

	  /// <summary>
	  /// Returns a curve node for an FX Swap using the specified instrument template and keys and label.
	  /// </summary>
	  /// <param name="template">  the template used for building the instrument for the node </param>
	  /// <param name="farForwardPointsId">  the identifier of the FX points at the far date </param>
	  /// <param name="label">  the label to use for the node </param>
	  /// <returns> a node whose instrument is built from the template using a market rate </returns>
	  public static FxSwapCurveNode of(FxSwapTemplate template, ObservableId farForwardPointsId, string label)
	  {
		return builder().template(template).farForwardPointsId(farForwardPointsId).label(label).build();
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
			builder.label_Renamed = Tenor.of(builder.template_Renamed.PeriodToFar).ToString();
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
		// TODO: extra identifier for near forward points
		return ImmutableSet.of(fxRateId, farForwardPointsId);
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
		Tenor tenor = Tenor.of(template.PeriodToFar);
		return TenorDateParameterMetadata.of(nodeDate, tenor, label);
	  }

	  // calculate the end date
	  private LocalDate calculateEnd(LocalDate valuationDate, ReferenceData refData)
	  {
		FxSwapTrade trade = template.createTrade(valuationDate, BuySell.BUY, 1, 1, 0, refData);
		return trade.Product.FarLeg.resolve(refData).PaymentDate;
	  }

	  // calculate the last fixing date
	  private LocalDate calculateLastFixingDate(LocalDate valuationDate, ReferenceData refData)
	  {
		throw new System.NotSupportedException("Node date of 'LastFixing' is not supported for FxSwap");
	  }

	  public FxSwapTrade trade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		FxRate fxRate = marketData.getValue(fxRateId);
		double rate = fxRate.fxRate(template.CurrencyPair);
		double fxPts = marketData.getValue(farForwardPointsId);
		BuySell buySell = quantity > 0 ? BuySell.BUY : BuySell.SELL;
		return template.createTrade(marketData.ValuationDate, buySell, Math.Abs(quantity), rate, fxPts, refData);
	  }

	  public ResolvedFxSwapTrade resolvedTrade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		return trade(quantity, marketData, refData).resolve(refData);
	  }

	  public double initialGuess(MarketData marketData, ValueType valueType)
	  {
		if (ValueType.DISCOUNT_FACTOR.Equals(valueType))
		{
		  return 1d;
		}
		return 0d;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy of this node with the specified date.
	  /// </summary>
	  /// <param name="date">  the date to use </param>
	  /// <returns> the node based on this node with the specified date </returns>
	  public FxSwapCurveNode withDate(CurveNodeDate date)
	  {
		return new FxSwapCurveNode(template, fxRateId, farForwardPointsId, label, date, dateOrder);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwapCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static FxSwapCurveNode.Meta meta()
	  {
		return FxSwapCurveNode.Meta.INSTANCE;
	  }

	  static FxSwapCurveNode()
	  {
		MetaBean.register(FxSwapCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static FxSwapCurveNode.Builder builder()
	  {
		return new FxSwapCurveNode.Builder();
	  }

	  private FxSwapCurveNode(FxSwapTemplate template, FxRateId fxRateId, ObservableId farForwardPointsId, string label, CurveNodeDate date, CurveNodeDateOrder dateOrder)
	  {
		JodaBeanUtils.notNull(template, "template");
		JodaBeanUtils.notNull(fxRateId, "fxRateId");
		JodaBeanUtils.notNull(farForwardPointsId, "farForwardPointsId");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(dateOrder, "dateOrder");
		this.template = template;
		this.fxRateId = fxRateId;
		this.farForwardPointsId = farForwardPointsId;
		this.label = label;
		this.date_Renamed = date;
		this.dateOrder = dateOrder;
	  }

	  public override FxSwapCurveNode.Meta metaBean()
	  {
		return FxSwapCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for the FX Swap associated with this node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxSwapTemplate Template
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
	  /// Gets the identifier of the market data value which provides the FX forward points. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableId FarForwardPointsId
	  {
		  get
		  {
			return farForwardPointsId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label to use for the node, defaulted.
	  /// <para>
	  /// When building, this will default based on the far period if not specified.
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
		  FxSwapCurveNode other = (FxSwapCurveNode) obj;
		  return JodaBeanUtils.equal(template, other.template) && JodaBeanUtils.equal(fxRateId, other.fxRateId) && JodaBeanUtils.equal(farForwardPointsId, other.farForwardPointsId) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(date_Renamed, other.date_Renamed) && JodaBeanUtils.equal(dateOrder, other.dateOrder);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(template);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fxRateId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(farForwardPointsId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(date_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dateOrder);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("FxSwapCurveNode{");
		buf.Append("template").Append('=').Append(template).Append(',').Append(' ');
		buf.Append("fxRateId").Append('=').Append(fxRateId).Append(',').Append(' ');
		buf.Append("farForwardPointsId").Append('=').Append(farForwardPointsId).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("date").Append('=').Append(date_Renamed).Append(',').Append(' ');
		buf.Append("dateOrder").Append('=').Append(JodaBeanUtils.ToString(dateOrder));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code FxSwapCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(FxSwapCurveNode), typeof(FxSwapTemplate));
			  fxRateId_Renamed = DirectMetaProperty.ofImmutable(this, "fxRateId", typeof(FxSwapCurveNode), typeof(FxRateId));
			  farForwardPointsId_Renamed = DirectMetaProperty.ofImmutable(this, "farForwardPointsId", typeof(FxSwapCurveNode), typeof(ObservableId));
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(FxSwapCurveNode), typeof(string));
			  date_Renamed = DirectMetaProperty.ofImmutable(this, "date", typeof(FxSwapCurveNode), typeof(CurveNodeDate));
			  dateOrder_Renamed = DirectMetaProperty.ofImmutable(this, "dateOrder", typeof(FxSwapCurveNode), typeof(CurveNodeDateOrder));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "template", "fxRateId", "farForwardPointsId", "label", "date", "dateOrder");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code template} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxSwapTemplate> template_Renamed;
		/// <summary>
		/// The meta-property for the {@code fxRateId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxRateId> fxRateId_Renamed;
		/// <summary>
		/// The meta-property for the {@code farForwardPointsId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableId> farForwardPointsId_Renamed;
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
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "template", "fxRateId", "farForwardPointsId", "label", "date", "dateOrder");
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
			case -566044884: // farForwardPointsId
			  return farForwardPointsId_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case 3076014: // date
			  return date_Renamed;
			case -263699392: // dateOrder
			  return dateOrder_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override FxSwapCurveNode.Builder builder()
		{
		  return new FxSwapCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(FxSwapCurveNode);
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
		public MetaProperty<FxSwapTemplate> template()
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
		/// The meta-property for the {@code farForwardPointsId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableId> farForwardPointsId()
		{
		  return farForwardPointsId_Renamed;
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
			  return ((FxSwapCurveNode) bean).Template;
			case -1054985843: // fxRateId
			  return ((FxSwapCurveNode) bean).FxRateId;
			case -566044884: // farForwardPointsId
			  return ((FxSwapCurveNode) bean).FarForwardPointsId;
			case 102727412: // label
			  return ((FxSwapCurveNode) bean).Label;
			case 3076014: // date
			  return ((FxSwapCurveNode) bean).Date;
			case -263699392: // dateOrder
			  return ((FxSwapCurveNode) bean).DateOrder;
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
	  /// The bean-builder for {@code FxSwapCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<FxSwapCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxSwapTemplate template_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxRateId fxRateId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableId farForwardPointsId_Renamed;
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
		internal Builder(FxSwapCurveNode beanToCopy)
		{
		  this.template_Renamed = beanToCopy.Template;
		  this.fxRateId_Renamed = beanToCopy.FxRateId;
		  this.farForwardPointsId_Renamed = beanToCopy.FarForwardPointsId;
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
			case -566044884: // farForwardPointsId
			  return farForwardPointsId_Renamed;
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
			  this.template_Renamed = (FxSwapTemplate) newValue;
			  break;
			case -1054985843: // fxRateId
			  this.fxRateId_Renamed = (FxRateId) newValue;
			  break;
			case -566044884: // farForwardPointsId
			  this.farForwardPointsId_Renamed = (ObservableId) newValue;
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

		public override FxSwapCurveNode build()
		{
		  preBuild(this);
		  return new FxSwapCurveNode(template_Renamed, fxRateId_Renamed, farForwardPointsId_Renamed, label_Renamed, date_Renamed, dateOrder_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the template for the FX Swap associated with this node. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(FxSwapTemplate template)
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
		/// Sets the identifier of the market data value which provides the FX forward points. </summary>
		/// <param name="farForwardPointsId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder farForwardPointsId(ObservableId farForwardPointsId)
		{
		  JodaBeanUtils.notNull(farForwardPointsId, "farForwardPointsId");
		  this.farForwardPointsId_Renamed = farForwardPointsId;
		  return this;
		}

		/// <summary>
		/// Sets the label to use for the node, defaulted.
		/// <para>
		/// When building, this will default based on the far period if not specified.
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
		  buf.Append("FxSwapCurveNode.Builder{");
		  buf.Append("template").Append('=').Append(JodaBeanUtils.ToString(template_Renamed)).Append(',').Append(' ');
		  buf.Append("fxRateId").Append('=').Append(JodaBeanUtils.ToString(fxRateId_Renamed)).Append(',').Append(' ');
		  buf.Append("farForwardPointsId").Append('=').Append(JodaBeanUtils.ToString(farForwardPointsId_Renamed)).Append(',').Append(' ');
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