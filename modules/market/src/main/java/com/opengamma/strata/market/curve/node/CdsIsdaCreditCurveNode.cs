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
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using TenorDateParameterMetadata = com.opengamma.strata.market.param.TenorDateParameterMetadata;
	using BuySell = com.opengamma.strata.product.common.BuySell;
	using CdsCalibrationTrade = com.opengamma.strata.product.credit.CdsCalibrationTrade;
	using CdsQuote = com.opengamma.strata.product.credit.CdsQuote;
	using CdsTrade = com.opengamma.strata.product.credit.CdsTrade;
	using CdsQuoteConvention = com.opengamma.strata.product.credit.type.CdsQuoteConvention;
	using CdsTemplate = com.opengamma.strata.product.credit.type.CdsTemplate;
	using DatesCdsTemplate = com.opengamma.strata.product.credit.type.DatesCdsTemplate;
	using TenorCdsTemplate = com.opengamma.strata.product.credit.type.TenorCdsTemplate;

	/// <summary>
	/// An ISDA compliant curve node whose instrument is a credit default swap.
	/// <para>
	/// The trade produced by the node will be a protection receiver (BUY) for a positive quantity
	/// and a protection payer (SELL) for a negative quantity.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class CdsIsdaCreditCurveNode implements com.opengamma.strata.market.curve.IsdaCreditCurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CdsIsdaCreditCurveNode : IsdaCreditCurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.credit.type.CdsTemplate template;
		private readonly CdsTemplate template;
	  /// <summary>
	  /// The label to use for the node.
	  /// <para>
	  /// When building, this will default based on {@code template} if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
	  private readonly string label;
	  /// <summary>
	  /// The identifier of the market data value that provides the quoted value.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.ObservableId observableId;
	  private readonly ObservableId observableId;
	  /// <summary>
	  /// The legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of the CDS.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.StandardId legalEntityId;
	  private readonly StandardId legalEntityId;
	  /// <summary>
	  /// The market quote convention.
	  /// <para>
	  /// The CDS is quoted in par spread, points upfront or quoted spread.
	  /// See <seealso cref="CdsQuoteConvention"/> for detail.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.product.credit.type.CdsQuoteConvention quoteConvention;
	  private readonly CdsQuoteConvention quoteConvention;
	  /// <summary>
	  /// The fixed coupon rate.
	  /// <para>
	  /// This must be represented in decimal form.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> fixedRate;
	  private readonly double? fixedRate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a curve node with par spread convention.
	  /// </summary>
	  /// <param name="template">  the template </param>
	  /// <param name="observableId">  the observable ID </param>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <returns> the curve node </returns>
	  public static CdsIsdaCreditCurveNode ofParSpread(CdsTemplate template, ObservableId observableId, StandardId legalEntityId)
	  {

		return builder().template(template).observableId(observableId).legalEntityId(legalEntityId).quoteConvention(CdsQuoteConvention.PAR_SPREAD).build();
	  }

	  /// <summary>
	  /// Returns a curve node with points upfront convention.
	  /// </summary>
	  /// <param name="template">  the template </param>
	  /// <param name="observableId">  the observable ID </param>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <returns> the curve node </returns>
	  public static CdsIsdaCreditCurveNode ofPointsUpfront(CdsTemplate template, ObservableId observableId, StandardId legalEntityId, double? fixedRate)
	  {

		return builder().template(template).observableId(observableId).legalEntityId(legalEntityId).quoteConvention(CdsQuoteConvention.POINTS_UPFRONT).fixedRate(fixedRate).build();
	  }

	  /// <summary>
	  /// Returns a curve node with quoted spread convention.
	  /// </summary>
	  /// <param name="template">  the template </param>
	  /// <param name="observableId">  the observable ID </param>
	  /// <param name="legalEntityId">  the legal entity ID </param>
	  /// <param name="fixedRate">  the fixed rate </param>
	  /// <returns> the curve node </returns>
	  public static CdsIsdaCreditCurveNode ofQuotedSpread(CdsTemplate template, ObservableId observableId, StandardId legalEntityId, double? fixedRate)
	  {

		return builder().template(template).observableId(observableId).legalEntityId(legalEntityId).quoteConvention(CdsQuoteConvention.QUOTED_SPREAD).fixedRate(fixedRate).build();
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.template_Renamed != null)
		{
		  if (string.ReferenceEquals(builder.label_Renamed, null))
		  {
			builder.label_Renamed = builder.template_Renamed is TenorCdsTemplate ? ((TenorCdsTemplate) builder.template_Renamed).Tenor.ToString() : ((DatesCdsTemplate) builder.template_Renamed).EndDate.ToString();
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (quoteConvention.Equals(CdsQuoteConvention.PAR_SPREAD))
		{
		  ArgChecker.isTrue(fixedRate == null, "The fixed rate must be empty for par spread quote");
		}
		else
		{
		  ArgChecker.isTrue(fixedRate != null, "The fixed rate must be specifed if quote convention is points upfront or quoted spread");
		}
	  }

	  //-------------------------------------------------------------------------
	  public LocalDate date(LocalDate tradeDate, ReferenceData refData)
	  {
		CdsTrade trade = template.createTrade(legalEntityId, tradeDate, BuySell.BUY, 1, 1, refData);
		return trade.Product.resolve(refData).ProtectionEndDate;
	  }

	  public override DatedParameterMetadata metadata(LocalDate nodeDate)
	  {
		return template is TenorCdsTemplate ? TenorDateParameterMetadata.of(nodeDate, ((TenorCdsTemplate) template).Tenor, label) : LabelDateParameterMetadata.of(nodeDate, label);
	  }

	  /// <summary>
	  /// Creates a trade representing the CDS at the node.
	  /// <para>
	  /// This uses the observed market data to build the CDS trade that the node represents.
	  /// The resulting trade is not resolved.
	  /// The notional of the trade is taken from the 'quantity' variable.
	  /// The quantity is signed and will affect whether the trade is Buy or Sell.
	  /// The valuation date is defined by the market data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="quantity">  the quantity or notional of the trade </param>
	  /// <param name="marketData">  the market data required to build a trade for the instrument, including the valuation date </param>
	  /// <param name="refData">  the reference data, used to resolve the trade dates </param>
	  /// <returns> a trade representing the instrument at the node </returns>
	  public CdsCalibrationTrade trade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		BuySell buySell = quantity > 0 ? BuySell.BUY : BuySell.SELL;
		LocalDate valuationDate = marketData.ValuationDate;
		double quoteValue = marketData.getValue(observableId);
		CdsQuote quote = CdsQuote.of(quoteConvention, quoteValue);
		double notional = Math.Abs(quantity);
		if (quoteConvention.Equals(CdsQuoteConvention.PAR_SPREAD))
		{
		  return CdsCalibrationTrade.of(template.createTrade(legalEntityId, valuationDate, buySell, notional, quoteValue, refData), quote);
		}
		double coupon = FixedRate.Value; // always success
		return CdsCalibrationTrade.of(template.createTrade(legalEntityId, valuationDate, buySell, notional, coupon, refData), quote);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CdsIsdaCreditCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static CdsIsdaCreditCurveNode.Meta meta()
	  {
		return CdsIsdaCreditCurveNode.Meta.INSTANCE;
	  }

	  static CdsIsdaCreditCurveNode()
	  {
		MetaBean.register(CdsIsdaCreditCurveNode.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static CdsIsdaCreditCurveNode.Builder builder()
	  {
		return new CdsIsdaCreditCurveNode.Builder();
	  }

	  private CdsIsdaCreditCurveNode(CdsTemplate template, string label, ObservableId observableId, StandardId legalEntityId, CdsQuoteConvention quoteConvention, double? fixedRate)
	  {
		JodaBeanUtils.notNull(template, "template");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(observableId, "observableId");
		JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		JodaBeanUtils.notNull(quoteConvention, "quoteConvention");
		this.template = template;
		this.label = label;
		this.observableId = observableId;
		this.legalEntityId = legalEntityId;
		this.quoteConvention = quoteConvention;
		this.fixedRate = fixedRate;
		validate();
	  }

	  public override CdsIsdaCreditCurveNode.Meta metaBean()
	  {
		return CdsIsdaCreditCurveNode.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the template for the CDS associated with this node. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CdsTemplate Template
	  {
		  get
		  {
			return template;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label to use for the node.
	  /// <para>
	  /// When building, this will default based on {@code template} if not specified.
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
	  /// Gets the identifier of the market data value that provides the quoted value. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableId ObservableId
	  {
		  get
		  {
			return observableId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the legal entity identifier.
	  /// <para>
	  /// This identifier is used for the reference legal entity of the CDS.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public StandardId LegalEntityId
	  {
		  get
		  {
			return legalEntityId;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market quote convention.
	  /// <para>
	  /// The CDS is quoted in par spread, points upfront or quoted spread.
	  /// See <seealso cref="CdsQuoteConvention"/> for detail.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CdsQuoteConvention QuoteConvention
	  {
		  get
		  {
			return quoteConvention;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the fixed coupon rate.
	  /// <para>
	  /// This must be represented in decimal form.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? FixedRate
	  {
		  get
		  {
			return fixedRate != null ? double?.of(fixedRate) : double?.empty();
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
		  CdsIsdaCreditCurveNode other = (CdsIsdaCreditCurveNode) obj;
		  return JodaBeanUtils.equal(template, other.template) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(observableId, other.observableId) && JodaBeanUtils.equal(legalEntityId, other.legalEntityId) && JodaBeanUtils.equal(quoteConvention, other.quoteConvention) && JodaBeanUtils.equal(fixedRate, other.fixedRate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(template);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(observableId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(legalEntityId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(quoteConvention);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(fixedRate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("CdsIsdaCreditCurveNode{");
		buf.Append("template").Append('=').Append(template).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("observableId").Append('=').Append(observableId).Append(',').Append(' ');
		buf.Append("legalEntityId").Append('=').Append(legalEntityId).Append(',').Append(' ');
		buf.Append("quoteConvention").Append('=').Append(quoteConvention).Append(',').Append(' ');
		buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code CdsIsdaCreditCurveNode}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  template_Renamed = DirectMetaProperty.ofImmutable(this, "template", typeof(CdsIsdaCreditCurveNode), typeof(CdsTemplate));
			  label_Renamed = DirectMetaProperty.ofImmutable(this, "label", typeof(CdsIsdaCreditCurveNode), typeof(string));
			  observableId_Renamed = DirectMetaProperty.ofImmutable(this, "observableId", typeof(CdsIsdaCreditCurveNode), typeof(ObservableId));
			  legalEntityId_Renamed = DirectMetaProperty.ofImmutable(this, "legalEntityId", typeof(CdsIsdaCreditCurveNode), typeof(StandardId));
			  quoteConvention_Renamed = DirectMetaProperty.ofImmutable(this, "quoteConvention", typeof(CdsIsdaCreditCurveNode), typeof(CdsQuoteConvention));
			  fixedRate_Renamed = DirectMetaProperty.ofImmutable(this, "fixedRate", typeof(CdsIsdaCreditCurveNode), typeof(Double));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "template", "label", "observableId", "legalEntityId", "quoteConvention", "fixedRate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code template} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsTemplate> template_Renamed;
		/// <summary>
		/// The meta-property for the {@code label} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<string> label_Renamed;
		/// <summary>
		/// The meta-property for the {@code observableId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ObservableId> observableId_Renamed;
		/// <summary>
		/// The meta-property for the {@code legalEntityId} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<StandardId> legalEntityId_Renamed;
		/// <summary>
		/// The meta-property for the {@code quoteConvention} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CdsQuoteConvention> quoteConvention_Renamed;
		/// <summary>
		/// The meta-property for the {@code fixedRate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> fixedRate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "template", "label", "observableId", "legalEntityId", "quoteConvention", "fixedRate");
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
			case 102727412: // label
			  return label_Renamed;
			case -518800962: // observableId
			  return observableId_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 2049149709: // quoteConvention
			  return quoteConvention_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override CdsIsdaCreditCurveNode.Builder builder()
		{
		  return new CdsIsdaCreditCurveNode.Builder();
		}

		public override Type beanType()
		{
		  return typeof(CdsIsdaCreditCurveNode);
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
		public MetaProperty<CdsTemplate> template()
		{
		  return template_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code label} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<string> label()
		{
		  return label_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code observableId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ObservableId> observableId()
		{
		  return observableId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code legalEntityId} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<StandardId> legalEntityId()
		{
		  return legalEntityId_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code quoteConvention} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CdsQuoteConvention> quoteConvention()
		{
		  return quoteConvention_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code fixedRate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> fixedRate()
		{
		  return fixedRate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  return ((CdsIsdaCreditCurveNode) bean).Template;
			case 102727412: // label
			  return ((CdsIsdaCreditCurveNode) bean).Label;
			case -518800962: // observableId
			  return ((CdsIsdaCreditCurveNode) bean).ObservableId;
			case 866287159: // legalEntityId
			  return ((CdsIsdaCreditCurveNode) bean).LegalEntityId;
			case 2049149709: // quoteConvention
			  return ((CdsIsdaCreditCurveNode) bean).QuoteConvention;
			case 747425396: // fixedRate
			  return ((CdsIsdaCreditCurveNode) bean).fixedRate;
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
	  /// The bean-builder for {@code CdsIsdaCreditCurveNode}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<CdsIsdaCreditCurveNode>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CdsTemplate template_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal string label_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ObservableId observableId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal StandardId legalEntityId_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CdsQuoteConvention quoteConvention_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal double? fixedRate_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(CdsIsdaCreditCurveNode beanToCopy)
		{
		  this.template_Renamed = beanToCopy.Template;
		  this.label_Renamed = beanToCopy.Label;
		  this.observableId_Renamed = beanToCopy.ObservableId;
		  this.legalEntityId_Renamed = beanToCopy.LegalEntityId;
		  this.quoteConvention_Renamed = beanToCopy.QuoteConvention;
		  this.fixedRate_Renamed = beanToCopy.fixedRate;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  return template_Renamed;
			case 102727412: // label
			  return label_Renamed;
			case -518800962: // observableId
			  return observableId_Renamed;
			case 866287159: // legalEntityId
			  return legalEntityId_Renamed;
			case 2049149709: // quoteConvention
			  return quoteConvention_Renamed;
			case 747425396: // fixedRate
			  return fixedRate_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1321546630: // template
			  this.template_Renamed = (CdsTemplate) newValue;
			  break;
			case 102727412: // label
			  this.label_Renamed = (string) newValue;
			  break;
			case -518800962: // observableId
			  this.observableId_Renamed = (ObservableId) newValue;
			  break;
			case 866287159: // legalEntityId
			  this.legalEntityId_Renamed = (StandardId) newValue;
			  break;
			case 2049149709: // quoteConvention
			  this.quoteConvention_Renamed = (CdsQuoteConvention) newValue;
			  break;
			case 747425396: // fixedRate
			  this.fixedRate_Renamed = (double?) newValue;
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

		public override CdsIsdaCreditCurveNode build()
		{
		  preBuild(this);
		  return new CdsIsdaCreditCurveNode(template_Renamed, label_Renamed, observableId_Renamed, legalEntityId_Renamed, quoteConvention_Renamed, fixedRate_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the template for the CDS associated with this node. </summary>
		/// <param name="template">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder template(CdsTemplate template)
		{
		  JodaBeanUtils.notNull(template, "template");
		  this.template_Renamed = template;
		  return this;
		}

		/// <summary>
		/// Sets the label to use for the node.
		/// <para>
		/// When building, this will default based on {@code template} if not specified.
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
		/// Sets the identifier of the market data value that provides the quoted value. </summary>
		/// <param name="observableId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder observableId(ObservableId observableId)
		{
		  JodaBeanUtils.notNull(observableId, "observableId");
		  this.observableId_Renamed = observableId;
		  return this;
		}

		/// <summary>
		/// Sets the legal entity identifier.
		/// <para>
		/// This identifier is used for the reference legal entity of the CDS.
		/// </para>
		/// </summary>
		/// <param name="legalEntityId">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder legalEntityId(StandardId legalEntityId)
		{
		  JodaBeanUtils.notNull(legalEntityId, "legalEntityId");
		  this.legalEntityId_Renamed = legalEntityId;
		  return this;
		}

		/// <summary>
		/// Sets the market quote convention.
		/// <para>
		/// The CDS is quoted in par spread, points upfront or quoted spread.
		/// See <seealso cref="CdsQuoteConvention"/> for detail.
		/// </para>
		/// </summary>
		/// <param name="quoteConvention">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder quoteConvention(CdsQuoteConvention quoteConvention)
		{
		  JodaBeanUtils.notNull(quoteConvention, "quoteConvention");
		  this.quoteConvention_Renamed = quoteConvention;
		  return this;
		}

		/// <summary>
		/// Sets the fixed coupon rate.
		/// <para>
		/// This must be represented in decimal form.
		/// </para>
		/// </summary>
		/// <param name="fixedRate">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder fixedRate(double? fixedRate)
		{
		  this.fixedRate_Renamed = fixedRate;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("CdsIsdaCreditCurveNode.Builder{");
		  buf.Append("template").Append('=').Append(JodaBeanUtils.ToString(template_Renamed)).Append(',').Append(' ');
		  buf.Append("label").Append('=').Append(JodaBeanUtils.ToString(label_Renamed)).Append(',').Append(' ');
		  buf.Append("observableId").Append('=').Append(JodaBeanUtils.ToString(observableId_Renamed)).Append(',').Append(' ');
		  buf.Append("legalEntityId").Append('=').Append(JodaBeanUtils.ToString(legalEntityId_Renamed)).Append(',').Append(' ');
		  buf.Append("quoteConvention").Append('=').Append(JodaBeanUtils.ToString(quoteConvention_Renamed)).Append(',').Append(' ');
		  buf.Append("fixedRate").Append('=').Append(JodaBeanUtils.ToString(fixedRate_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}