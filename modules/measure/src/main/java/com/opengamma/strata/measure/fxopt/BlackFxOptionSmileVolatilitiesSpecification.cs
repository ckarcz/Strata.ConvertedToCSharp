using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableListMultimap = com.google.common.collect.ImmutableListMultimap;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using BusinessDayAdjustment = com.opengamma.strata.basics.date.BusinessDayAdjustment;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DaysAdjustment = com.opengamma.strata.basics.date.DaysAdjustment;
	using Tenor = com.opengamma.strata.basics.date.Tenor;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Guavate = com.opengamma.strata.collect.Guavate;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using Strike = com.opengamma.strata.market.option.Strike;
	using BlackFxOptionSmileVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionSmileVolatilities;
	using FxOptionVolatilitiesName = com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName;
	using InterpolatedStrikeSmileDeltaTermStructure = com.opengamma.strata.pricer.fxopt.InterpolatedStrikeSmileDeltaTermStructure;

	/// <summary>
	/// The specification of how to build FX option volatilities. 
	/// <para>
	/// This is the specification for a single volatility object, <seealso cref="BlackFxOptionSmileVolatilities"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class BlackFxOptionSmileVolatilitiesSpecification implements FxOptionVolatilitiesSpecification, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class BlackFxOptionSmileVolatilitiesSpecification : FxOptionVolatilitiesSpecification, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName name;
		private readonly FxOptionVolatilitiesName name;
	  /// <summary>
	  /// The currency pair that the volatilities are for.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.currency.CurrencyPair currencyPair;
	  private readonly CurrencyPair currencyPair;
	  /// <summary>
	  /// The day count convention used for the expiry.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The nodes in the FX option volatilities.
	  /// <para>
	  /// The nodes are used to find the quotes and build the volatilities.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.google.common.collect.ImmutableList<FxOptionVolatilitiesNode> nodes;
	  private readonly ImmutableList<FxOptionVolatilitiesNode> nodes;
	  /// <summary>
	  /// The interpolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator timeInterpolator;
	  private readonly CurveInterpolator timeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorLeft;
	  private readonly CurveExtrapolator timeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorRight;
	  private readonly CurveExtrapolator timeExtrapolatorRight;
	  /// <summary>
	  /// The interpolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator strikeInterpolator;
	  private readonly CurveInterpolator strikeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorLeft;
	  private readonly CurveExtrapolator strikeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorRight;
	  private readonly CurveExtrapolator strikeExtrapolatorRight;

	  /// <summary>
	  /// Entries for the volatilities, keyed by tenor.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableListMultimap<Tenor, FxOptionVolatilitiesNode> nodesByTenor; // not a property
	  /// <summary>
	  /// The range of the delta.
	  /// </summary>
	  [NonSerialized]
	  private readonly ImmutableList<double> deltas; // not a property

	  //-------------------------------------------------------------------------
	  public BlackFxOptionSmileVolatilities volatilities(ZonedDateTime valuationDateTime, DoubleArray parameters, ReferenceData refData)
	  {

		ArgChecker.isTrue(parameters.size() == ParameterCount, Messages.format("Size of parameters must be {}, but found {}", ParameterCount, parameters.size()));
		ImmutableListMultimap.Builder<Tenor, Pair<FxOptionVolatilitiesNode, double>> builder = ImmutableListMultimap.builder();
		foreach (Tenor tenor in nodesByTenor.keys())
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  ImmutableList<Pair<FxOptionVolatilitiesNode, double>> nodesAndQuotes = nodesByTenor.get(tenor).Select(node => Pair.of(node, parameters.get(nodes.indexOf(node)))).collect(toImmutableList());
		  builder.putAll(tenor, nodesAndQuotes);
		}
		ImmutableListMultimap<Tenor, Pair<FxOptionVolatilitiesNode, double>> nodesAndQuotesByTenor = builder.build();

		IList<Tenor> tenors = new List<Tenor>(nodesByTenor.Keys);
		tenors.Sort();
		int nTenors = tenors.Count;
		int nDeltas = deltas.size();

		double[] expiries = new double[nTenors];
		double[] atm = new double[nTenors];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] rr = new double[nTenors][nDeltas];
		double[][] rr = RectangularArrays.ReturnRectangularDoubleArray(nTenors, nDeltas);
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] str = new double[nTenors][nDeltas];
		double[][] str = RectangularArrays.ReturnRectangularDoubleArray(nTenors, nDeltas);
		for (int i = 0; i < nTenors; ++i)
		{
		  parametersForPeriod(valuationDateTime, nodesAndQuotesByTenor.get(tenors[i]), i, expiries, atm, rr, str, refData);
		}
		InterpolatedStrikeSmileDeltaTermStructure smiles = InterpolatedStrikeSmileDeltaTermStructure.of(DoubleArray.copyOf(expiries), DoubleArray.copyOf(deltas.subList(0, nDeltas)), DoubleArray.copyOf(atm), DoubleMatrix.copyOf(rr), DoubleMatrix.copyOf(str), dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);

		return BlackFxOptionSmileVolatilities.of(name, currencyPair, valuationDateTime, smiles);
	  }

	  private void parametersForPeriod(ZonedDateTime valuationDateTime, IList<Pair<FxOptionVolatilitiesNode, double>> nodesAndQuotes, int index, double[] expiries, double[] atm, double[][] rr, double[][] str, ReferenceData refData)
	  {

		int nDeltas = deltas.size();
		foreach (Pair<FxOptionVolatilitiesNode, double> entry in nodesAndQuotes)
		{
		  FxOptionVolatilitiesNode node = entry.First;
		  ValueType quoteValyeType = node.QuoteValueType;
		  if (quoteValyeType.Equals(ValueType.BLACK_VOLATILITY))
		  {
			atm[index] = entry.Second;
			expiries[index] = node.timeToExpiry(valuationDateTime, dayCount, refData);
		  }
		  else if (quoteValyeType.Equals(ValueType.RISK_REVERSAL))
		  {
			for (int i = 0; i < nDeltas; ++i)
			{
			  if (node.Strike.Value == deltas.get(i))
			  {
				rr[index][i] = entry.Second;
			  }
			}
		  }
		  else if (quoteValyeType.Equals(ValueType.STRANGLE))
		  {
			for (int i = 0; i < nDeltas; ++i)
			{
			  if (node.Strike.Value == deltas.get(i))
			  {
				str[index][i] = entry.Second;
			  }
			}
		  }
		  else
		  {
			throw new System.ArgumentException("Unsupported value type");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.strikeExtrapolatorLeft_Renamed = FLAT;
		builder.strikeExtrapolatorRight_Renamed = FLAT;
		builder.timeExtrapolatorLeft_Renamed = FLAT;
		builder.timeExtrapolatorRight_Renamed = FLAT;
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private BlackFxOptionSmileVolatilitiesSpecification(com.opengamma.strata.pricer.fxopt.FxOptionVolatilitiesName name, com.opengamma.strata.basics.currency.CurrencyPair currencyPair, com.opengamma.strata.basics.date.DayCount dayCount, java.util.List<FxOptionVolatilitiesNode> nodes, com.opengamma.strata.market.curve.interpolator.CurveInterpolator timeInterpolator, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorLeft, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorRight, com.opengamma.strata.market.curve.interpolator.CurveInterpolator strikeInterpolator, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorLeft, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorRight)
	  private BlackFxOptionSmileVolatilitiesSpecification(FxOptionVolatilitiesName name, CurrencyPair currencyPair, DayCount dayCount, IList<FxOptionVolatilitiesNode> nodes, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(currencyPair, "currencyPair");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(nodes, "nodes");
		JodaBeanUtils.notNull(timeInterpolator, "timeInterpolator");
		JodaBeanUtils.notNull(timeExtrapolatorLeft, "timeExtrapolatorLeft");
		JodaBeanUtils.notNull(timeExtrapolatorRight, "timeExtrapolatorRight");
		JodaBeanUtils.notNull(strikeInterpolator, "strikeInterpolator");
		JodaBeanUtils.notNull(strikeExtrapolatorLeft, "strikeExtrapolatorLeft");
		JodaBeanUtils.notNull(strikeExtrapolatorRight, "strikeExtrapolatorRight");
		this.name = name;
		this.currencyPair = currencyPair;
		this.dayCount = dayCount;
		this.nodes = ImmutableList.copyOf(nodes);
		this.timeInterpolator = timeInterpolator;
		this.timeExtrapolatorLeft = timeExtrapolatorLeft;
		this.timeExtrapolatorRight = timeExtrapolatorRight;
		this.strikeInterpolator = strikeInterpolator;
		this.strikeExtrapolatorLeft = strikeExtrapolatorLeft;
		this.strikeExtrapolatorRight = strikeExtrapolatorRight;
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		this.nodesByTenor = nodes.collect(Guavate.toImmutableListMultimap(FxOptionVolatilitiesNode::getTenor));
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<double> fullDeltas = nodes.Select(FxOptionVolatilitiesNode::getStrike).Distinct().Select(Strike::getValue).OrderBy(c => c).collect(toImmutableList());

		int nDeltas = fullDeltas.size() - 1;
		ArgChecker.isTrue(fullDeltas.get(nDeltas) == 0.5, "0 < delta <= 0.5");
		this.deltas = fullDeltas.subList(0, nDeltas); // ATM removed
		int nParams = nodes.Count;
		for (int i = 0; i < nParams; ++i)
		{
		  ArgChecker.isTrue(nodes[i].CurrencyPair.Equals(currencyPair), "currency pair must be the same");
		  ArgChecker.isTrue(nodes[i].Strike is DeltaStrike, "Strike must be DeltaStrike");
		}
		foreach (Tenor tenor in nodesByTenor.keys())
		{
		  ImmutableList<FxOptionVolatilitiesNode> nodesForTenor = nodesByTenor.get(tenor);
		  // value type, delta, size
		  IList<double> atmDelta = nodesForTenor.Where(node => node.QuoteValueType.Equals(ValueType.BLACK_VOLATILITY)).Select(node => node.Strike.Value).OrderBy(c => c).ToList();
//JAVA TO C# CONVERTER WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: com.opengamma.strata.collect.ArgChecker.isTrue(atmDelta.equals(fullDeltas.subList(nDeltas, nDeltas + 1)), "The ATM delta set must be " + fullDeltas.subList(nDeltas, nDeltas + 1) + ", but found " + atmDelta + ", for " + tenor);
		  ArgChecker.isTrue(atmDelta.SequenceEqual(fullDeltas.subList(nDeltas, nDeltas + 1)), "The ATM delta set must be " + fullDeltas.subList(nDeltas, nDeltas + 1) + ", but found " + atmDelta + ", for " + tenor);
		  IList<double> rrDelta = nodesForTenor.Where(node => node.QuoteValueType.Equals(ValueType.RISK_REVERSAL)).Select(node => node.Strike.Value).OrderBy(c => c).ToList();
//JAVA TO C# CONVERTER WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: com.opengamma.strata.collect.ArgChecker.isTrue(rrDelta.equals(deltas), "The delta set for risk reversal must be " + deltas + ", but found " + rrDelta + ", for " + tenor);
		  ArgChecker.isTrue(rrDelta.SequenceEqual(deltas), "The delta set for risk reversal must be " + deltas + ", but found " + rrDelta + ", for " + tenor);
		  IList<double> strDelta = nodesForTenor.Where(node => node.QuoteValueType.Equals(ValueType.STRANGLE)).Select(node => node.Strike.Value).OrderBy(c => c).ToList();
//JAVA TO C# CONVERTER WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: com.opengamma.strata.collect.ArgChecker.isTrue(strDelta.equals(deltas), "The delta set for strangle must be " + deltas + ", but found " + strDelta + ", for " + tenor);
		  ArgChecker.isTrue(strDelta.SequenceEqual(deltas), "The delta set for strangle must be " + deltas + ", but found " + strDelta + ", for " + tenor);
		  // convention
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  ISet<BusinessDayAdjustment> busAdj = nodesForTenor.Select(FxOptionVolatilitiesNode::getBusinessDayAdjustment).collect(toSet());
		  ArgChecker.isTrue(busAdj.Count == 1, "BusinessDayAdjustment must be common to all the nodes");
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  ISet<DaysAdjustment> offset = nodesForTenor.Select(FxOptionVolatilitiesNode::getSpotDateOffset).collect(toSet());
		  ArgChecker.isTrue(offset.Count == 1, "DaysAdjustment must be common to all the nodes");
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSmileVolatilitiesSpecification}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static BlackFxOptionSmileVolatilitiesSpecification.Meta meta()
	  {
		return BlackFxOptionSmileVolatilitiesSpecification.Meta.INSTANCE;
	  }

	  static BlackFxOptionSmileVolatilitiesSpecification()
	  {
		MetaBean.register(BlackFxOptionSmileVolatilitiesSpecification.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static BlackFxOptionSmileVolatilitiesSpecification.Builder builder()
	  {
		return new BlackFxOptionSmileVolatilitiesSpecification.Builder();
	  }

	  public override BlackFxOptionSmileVolatilitiesSpecification.Meta metaBean()
	  {
		return BlackFxOptionSmileVolatilitiesSpecification.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FxOptionVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the currency pair that the volatilities are for. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurrencyPair CurrencyPair
	  {
		  get
		  {
			return currencyPair;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention used for the expiry. </summary>
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
	  /// Gets the nodes in the FX option volatilities.
	  /// <para>
	  /// The nodes are used to find the quotes and build the volatilities.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<FxOptionVolatilitiesNode> Nodes
	  {
		  get
		  {
			return nodes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator TimeInterpolator
	  {
		  get
		  {
			return timeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorLeft
	  {
		  get
		  {
			return timeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorRight
	  {
		  get
		  {
			return timeExtrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator StrikeInterpolator
	  {
		  get
		  {
			return strikeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorLeft
	  {
		  get
		  {
			return strikeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorRight
	  {
		  get
		  {
			return strikeExtrapolatorRight;
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
		  BlackFxOptionSmileVolatilitiesSpecification other = (BlackFxOptionSmileVolatilitiesSpecification) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(currencyPair, other.currencyPair) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(nodes, other.nodes) && JodaBeanUtils.equal(timeInterpolator, other.timeInterpolator) && JodaBeanUtils.equal(timeExtrapolatorLeft, other.timeExtrapolatorLeft) && JodaBeanUtils.equal(timeExtrapolatorRight, other.timeExtrapolatorRight) && JodaBeanUtils.equal(strikeInterpolator, other.strikeInterpolator) && JodaBeanUtils.equal(strikeExtrapolatorLeft, other.strikeExtrapolatorLeft) && JodaBeanUtils.equal(strikeExtrapolatorRight, other.strikeExtrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(currencyPair);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nodes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorRight);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("BlackFxOptionSmileVolatilitiesSpecification{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("currencyPair").Append('=').Append(currencyPair).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("nodes").Append('=').Append(nodes).Append(',').Append(' ');
		buf.Append("timeInterpolator").Append('=').Append(timeInterpolator).Append(',').Append(' ');
		buf.Append("timeExtrapolatorLeft").Append('=').Append(timeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("timeExtrapolatorRight").Append('=').Append(timeExtrapolatorRight).Append(',').Append(' ');
		buf.Append("strikeInterpolator").Append('=').Append(strikeInterpolator).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorLeft").Append('=').Append(strikeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code BlackFxOptionSmileVolatilitiesSpecification}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(FxOptionVolatilitiesName));
			  currencyPair_Renamed = DirectMetaProperty.ofImmutable(this, "currencyPair", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurrencyPair));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(DayCount));
			  nodes_Renamed = DirectMetaProperty.ofImmutable(this, "nodes", typeof(BlackFxOptionSmileVolatilitiesSpecification), (Type) typeof(ImmutableList));
			  timeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "timeInterpolator", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveInterpolator));
			  timeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorLeft", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveExtrapolator));
			  timeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorRight", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveExtrapolator));
			  strikeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "strikeInterpolator", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveInterpolator));
			  strikeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorLeft", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveExtrapolator));
			  strikeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorRight", typeof(BlackFxOptionSmileVolatilitiesSpecification), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "currencyPair", "dayCount", "nodes", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<FxOptionVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code currencyPair} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurrencyPair> currencyPair_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code nodes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<FxOptionVolatilitiesNode>> nodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nodes", BlackFxOptionSmileVolatilitiesSpecification.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<FxOptionVolatilitiesNode>> nodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> timeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> strikeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "currencyPair", "dayCount", "nodes", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
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
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -587914188: // timeInterpolator
			  return timeInterpolator_Renamed;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft_Renamed;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight_Renamed;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator_Renamed;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft_Renamed;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override BlackFxOptionSmileVolatilitiesSpecification.Builder builder()
		{
		  return new BlackFxOptionSmileVolatilitiesSpecification.Builder();
		}

		public override Type beanType()
		{
		  return typeof(BlackFxOptionSmileVolatilitiesSpecification);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<FxOptionVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code currencyPair} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurrencyPair> currencyPair()
		{
		  return currencyPair_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code nodes} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<FxOptionVolatilitiesNode>> nodes()
		{
		  return nodes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> timeInterpolator()
		{
		  return timeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorLeft()
		{
		  return timeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorRight()
		{
		  return timeExtrapolatorRight_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> strikeInterpolator()
		{
		  return strikeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft()
		{
		  return strikeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorRight()
		{
		  return strikeExtrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).Name;
			case 1005147787: // currencyPair
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).CurrencyPair;
			case 1905311443: // dayCount
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).DayCount;
			case 104993457: // nodes
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).Nodes;
			case -587914188: // timeInterpolator
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).TimeInterpolator;
			case -286652761: // timeExtrapolatorLeft
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).TimeExtrapolatorLeft;
			case -290640004: // timeExtrapolatorRight
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).TimeExtrapolatorRight;
			case 815202713: // strikeInterpolator
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).StrikeInterpolator;
			case -1176196724: // strikeExtrapolatorLeft
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).StrikeExtrapolatorLeft;
			case -2096699081: // strikeExtrapolatorRight
			  return ((BlackFxOptionSmileVolatilitiesSpecification) bean).StrikeExtrapolatorRight;
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
	  /// The bean-builder for {@code BlackFxOptionSmileVolatilitiesSpecification}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<BlackFxOptionSmileVolatilitiesSpecification>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal FxOptionVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurrencyPair currencyPair_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<FxOptionVolatilitiesNode> nodes_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator timeInterpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator timeExtrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator timeExtrapolatorRight_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator strikeInterpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator strikeExtrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator strikeExtrapolatorRight_Renamed;

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
		internal Builder(BlackFxOptionSmileVolatilitiesSpecification beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.currencyPair_Renamed = beanToCopy.CurrencyPair;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.nodes_Renamed = beanToCopy.Nodes;
		  this.timeInterpolator_Renamed = beanToCopy.TimeInterpolator;
		  this.timeExtrapolatorLeft_Renamed = beanToCopy.TimeExtrapolatorLeft;
		  this.timeExtrapolatorRight_Renamed = beanToCopy.TimeExtrapolatorRight;
		  this.strikeInterpolator_Renamed = beanToCopy.StrikeInterpolator;
		  this.strikeExtrapolatorLeft_Renamed = beanToCopy.StrikeExtrapolatorLeft;
		  this.strikeExtrapolatorRight_Renamed = beanToCopy.StrikeExtrapolatorRight;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 1005147787: // currencyPair
			  return currencyPair_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -587914188: // timeInterpolator
			  return timeInterpolator_Renamed;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft_Renamed;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight_Renamed;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator_Renamed;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft_Renamed;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight_Renamed;
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
			case 3373707: // name
			  this.name_Renamed = (FxOptionVolatilitiesName) newValue;
			  break;
			case 1005147787: // currencyPair
			  this.currencyPair_Renamed = (CurrencyPair) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 104993457: // nodes
			  this.nodes_Renamed = (IList<FxOptionVolatilitiesNode>) newValue;
			  break;
			case -587914188: // timeInterpolator
			  this.timeInterpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case -286652761: // timeExtrapolatorLeft
			  this.timeExtrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case -290640004: // timeExtrapolatorRight
			  this.timeExtrapolatorRight_Renamed = (CurveExtrapolator) newValue;
			  break;
			case 815202713: // strikeInterpolator
			  this.strikeInterpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case -1176196724: // strikeExtrapolatorLeft
			  this.strikeExtrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case -2096699081: // strikeExtrapolatorRight
			  this.strikeExtrapolatorRight_Renamed = (CurveExtrapolator) newValue;
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

		public override BlackFxOptionSmileVolatilitiesSpecification build()
		{
		  return new BlackFxOptionSmileVolatilitiesSpecification(name_Renamed, currencyPair_Renamed, dayCount_Renamed, nodes_Renamed, timeInterpolator_Renamed, timeExtrapolatorLeft_Renamed, timeExtrapolatorRight_Renamed, strikeInterpolator_Renamed, strikeExtrapolatorLeft_Renamed, strikeExtrapolatorRight_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the volatilities. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(FxOptionVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the currency pair that the volatilities are for. </summary>
		/// <param name="currencyPair">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder currencyPair(CurrencyPair currencyPair)
		{
		  JodaBeanUtils.notNull(currencyPair, "currencyPair");
		  this.currencyPair_Renamed = currencyPair;
		  return this;
		}

		/// <summary>
		/// Sets the day count convention used for the expiry. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the nodes in the FX option volatilities.
		/// <para>
		/// The nodes are used to find the quotes and build the volatilities.
		/// </para>
		/// </summary>
		/// <param name="nodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nodes(IList<FxOptionVolatilitiesNode> nodes)
		{
		  JodaBeanUtils.notNull(nodes, "nodes");
		  this.nodes_Renamed = nodes;
		  return this;
		}

		/// <summary>
		/// Sets the {@code nodes} property in the builder
		/// from an array of objects. </summary>
		/// <param name="nodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nodes(params FxOptionVolatilitiesNode[] nodes)
		{
		  return this.nodes(ImmutableList.copyOf(nodes));
		}

		/// <summary>
		/// Sets the interpolator used in the time dimension. </summary>
		/// <param name="timeInterpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeInterpolator(CurveInterpolator timeInterpolator)
		{
		  JodaBeanUtils.notNull(timeInterpolator, "timeInterpolator");
		  this.timeInterpolator_Renamed = timeInterpolator;
		  return this;
		}

		/// <summary>
		/// Sets the left extrapolator used in the time dimension. </summary>
		/// <param name="timeExtrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeExtrapolatorLeft(CurveExtrapolator timeExtrapolatorLeft)
		{
		  JodaBeanUtils.notNull(timeExtrapolatorLeft, "timeExtrapolatorLeft");
		  this.timeExtrapolatorLeft_Renamed = timeExtrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the right extrapolator used in the time dimension. </summary>
		/// <param name="timeExtrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder timeExtrapolatorRight(CurveExtrapolator timeExtrapolatorRight)
		{
		  JodaBeanUtils.notNull(timeExtrapolatorRight, "timeExtrapolatorRight");
		  this.timeExtrapolatorRight_Renamed = timeExtrapolatorRight;
		  return this;
		}

		/// <summary>
		/// Sets the interpolator used in the strike dimension. </summary>
		/// <param name="strikeInterpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeInterpolator(CurveInterpolator strikeInterpolator)
		{
		  JodaBeanUtils.notNull(strikeInterpolator, "strikeInterpolator");
		  this.strikeInterpolator_Renamed = strikeInterpolator;
		  return this;
		}

		/// <summary>
		/// Sets the left extrapolator used in the strike dimension. </summary>
		/// <param name="strikeExtrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeExtrapolatorLeft(CurveExtrapolator strikeExtrapolatorLeft)
		{
		  JodaBeanUtils.notNull(strikeExtrapolatorLeft, "strikeExtrapolatorLeft");
		  this.strikeExtrapolatorLeft_Renamed = strikeExtrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the right extrapolator used in the strike dimension. </summary>
		/// <param name="strikeExtrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder strikeExtrapolatorRight(CurveExtrapolator strikeExtrapolatorRight)
		{
		  JodaBeanUtils.notNull(strikeExtrapolatorRight, "strikeExtrapolatorRight");
		  this.strikeExtrapolatorRight_Renamed = strikeExtrapolatorRight;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("BlackFxOptionSmileVolatilitiesSpecification.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("currencyPair").Append('=').Append(JodaBeanUtils.ToString(currencyPair_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("nodes").Append('=').Append(JodaBeanUtils.ToString(nodes_Renamed)).Append(',').Append(' ');
		  buf.Append("timeInterpolator").Append('=').Append(JodaBeanUtils.ToString(timeInterpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorRight_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeInterpolator").Append('=').Append(JodaBeanUtils.ToString(strikeInterpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}