using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using HolidayCalendars = com.opengamma.strata.basics.date.HolidayCalendars;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;

	/// <summary>
	/// Dummy curve node.
	/// Based on a FRA.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class DummyFraCurveNode implements CurveNode, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class DummyFraCurveNode : CurveNode, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToStart;
		private readonly Period periodToStart;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final java.time.Period periodToEnd;
	  private readonly Period periodToEnd;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.data.ObservableId rateId;
	  private readonly ObservableId rateId;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double spread;
	  private readonly double spread;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty", overrideGet = true) private final String label;
	  private readonly string label;
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final CurveNodeDateOrder order;
	  private readonly CurveNodeDateOrder order;

	  //-------------------------------------------------------------------------
	  public static DummyFraCurveNode of(Period periodToStart, IborIndex index, ObservableId rateId)
	  {
		return new DummyFraCurveNode(periodToStart, periodToStart.plus(index.Tenor.Period), rateId, 0, "Dummy:" + periodToStart, CurveNodeDateOrder.DEFAULT);
	  }

	  public static DummyFraCurveNode of(Period periodToStart, IborIndex index, ObservableId rateId, double spread)
	  {
		return new DummyFraCurveNode(periodToStart, periodToStart.plus(index.Tenor.Period), rateId, spread, "Dummy:" + periodToStart, CurveNodeDateOrder.DEFAULT);
	  }

	  public static DummyFraCurveNode of(Period periodToStart, IborIndex index, ObservableId rateId, CurveNodeDateOrder order)
	  {
		return new DummyFraCurveNode(periodToStart, periodToStart.plus(index.Tenor.Period), rateId, 0, "Dummy:" + periodToStart, order);
	  }

	  //-------------------------------------------------------------------------
	  public ISet<ObservableId> requirements()
	  {
		return ImmutableSet.of(rateId);
	  }

	  public LocalDate date(LocalDate valuationDate, ReferenceData refData)
	  {
		return HolidayCalendars.SAT_SUN.nextOrSame(valuationDate.plus(periodToEnd));
	  }

	  public DatedParameterMetadata metadata(LocalDate valuationDate, ReferenceData refData)
	  {
		return LabelDateParameterMetadata.of(HolidayCalendars.SAT_SUN.nextOrSame(valuationDate.plus(periodToEnd)), periodToEnd.ToString());
	  }

	  public DummyFraTrade trade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		double fixedRate = marketData.getValue(rateId) + spread;
		return DummyFraTrade.of(marketData.ValuationDate, fixedRate);
	  }

	  public DummyFraTrade resolvedTrade(double quantity, MarketData marketData, ReferenceData refData)
	  {
		return trade(quantity, marketData, refData);
	  }

	  public double initialGuess(MarketData marketData, ValueType valueType)
	  {
		if (ValueType.ZERO_RATE.Equals(valueType))
		{
		  return marketData.getValue(rateId);
		}
		return 0d;
	  }

	  public CurveNodeDateOrder DateOrder
	  {
		  get
		  {
			return order;
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DummyFraCurveNode}.
	  /// </summary>
	  private static readonly TypedMetaBean<DummyFraCurveNode> META_BEAN = LightMetaBean.of(typeof(DummyFraCurveNode), MethodHandles.lookup(), new string[] {"periodToStart", "periodToEnd", "rateId", "spread", "label", "order"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code DummyFraCurveNode}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DummyFraCurveNode> meta()
	  {
		return META_BEAN;
	  }

	  static DummyFraCurveNode()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private DummyFraCurveNode(Period periodToStart, Period periodToEnd, ObservableId rateId, double spread, string label, CurveNodeDateOrder order)
	  {
		JodaBeanUtils.notNull(periodToStart, "periodToStart");
		JodaBeanUtils.notNull(periodToEnd, "periodToEnd");
		JodaBeanUtils.notNull(rateId, "rateId");
		JodaBeanUtils.notEmpty(label, "label");
		JodaBeanUtils.notNull(order, "order");
		this.periodToStart = periodToStart;
		this.periodToEnd = periodToEnd;
		this.rateId = rateId;
		this.spread = spread;
		this.label = label;
		this.order = order;
	  }

	  public override TypedMetaBean<DummyFraCurveNode> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodToStart. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToStart
	  {
		  get
		  {
			return periodToStart;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the periodToEnd. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Period PeriodToEnd
	  {
		  get
		  {
			return periodToEnd;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rateId. </summary>
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
	  /// Gets the spread. </summary>
	  /// <returns> the value of the property </returns>
	  public double Spread
	  {
		  get
		  {
			return spread;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label. </summary>
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
	  /// Gets the order. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveNodeDateOrder Order
	  {
		  get
		  {
			return order;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  DummyFraCurveNode other = (DummyFraCurveNode) obj;
		  return JodaBeanUtils.equal(periodToStart, other.periodToStart) && JodaBeanUtils.equal(periodToEnd, other.periodToEnd) && JodaBeanUtils.equal(rateId, other.rateId) && JodaBeanUtils.equal(spread, other.spread) && JodaBeanUtils.equal(label, other.label) && JodaBeanUtils.equal(order, other.order);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToStart);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(periodToEnd);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rateId);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(spread);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(label);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(order);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("DummyFraCurveNode{");
		buf.Append("periodToStart").Append('=').Append(periodToStart).Append(',').Append(' ');
		buf.Append("periodToEnd").Append('=').Append(periodToEnd).Append(',').Append(' ');
		buf.Append("rateId").Append('=').Append(rateId).Append(',').Append(' ');
		buf.Append("spread").Append('=').Append(spread).Append(',').Append(' ');
		buf.Append("label").Append('=').Append(label).Append(',').Append(' ');
		buf.Append("order").Append('=').Append(JodaBeanUtils.ToString(order));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}