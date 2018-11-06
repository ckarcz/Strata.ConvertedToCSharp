using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using Messages = com.opengamma.strata.collect.Messages;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using MarketData = com.opengamma.strata.data.MarketData;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Provides the definition of how to calibrate a parameterized functional curve.
	/// <para>
	/// A parameterized functional curve is built from a number of parameters and described by metadata.
	/// Calibration is based on a list of <seealso cref="CurveNode"/> instances that specify the underlying instruments.
	/// </para>
	/// <para>
	/// The number of the curve parameters is in general different from the number of the instruments.
	/// However, the number mismatch tends to cause the root-finding failure in the curve calibration.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ParameterizedFunctionalCurveDefinition implements CurveDefinition, org.joda.beans.ImmutableBean
	public sealed class ParameterizedFunctionalCurveDefinition : CurveDefinition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final CurveName name;
		private readonly CurveName name;
	  /// <summary>
	  /// The x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType xValueType;
	  private readonly ValueType xValueType;
	  /// <summary>
	  /// The y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.market.ValueType yValueType;
	  private readonly ValueType yValueType;
	  /// <summary>
	  /// The day count, optional.
	  /// <para>
	  /// If the x-value of the curve represents time as a year fraction, the day count
	  /// can be specified to define how the year fraction is calculated.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The nodes of the underlying instruments.
	  /// <para>
	  /// The nodes are used to find the quoted values to which the curve is calibrated.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends CurveNode>", overrideGet = true) private final com.google.common.collect.ImmutableList<CurveNode> nodes;
	  private readonly ImmutableList<CurveNode> nodes;
	  /// <summary>
	  /// The initial guess values for the curve parameters.
	  /// <para>
	  /// The size must be the same as the number of the curve parameters. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<double> initialGuess;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
	  private readonly ImmutableList<double> initialGuess_Renamed;
	  /// <summary>
	  /// The parameter metadata of the curve, defaulted to empty metadata instances.
	  /// <para>
	  /// The size of the list must be the same as the number of the curve parameters.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends ParameterMetadata>") private final com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata;
	  private readonly ImmutableList<ParameterMetadata> parameterMetadata;
	  /// <summary>
	  /// The y-value function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns y-value.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> valueFunction;
	  private readonly System.Func<DoubleArray, double, double> valueFunction;
	  /// <summary>
	  /// The derivative function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x, 
	  /// i.e., the gradient of the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double> derivativeFunction;
	  private readonly System.Func<DoubleArray, double, double> derivativeFunction;
	  /// <summary>
	  /// The parameter sensitivity function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final System.Func<com.opengamma.strata.collect.array.DoubleArray, double, com.opengamma.strata.collect.array.DoubleArray> sensitivityFunction;
	  private readonly System.Func<DoubleArray, double, DoubleArray> sensitivityFunction;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.parameterMetadata_Renamed.Count == 0)
		{
		  if (builder.initialGuess_Renamed != null)
		  {
			builder.parameterMetadata_Renamed = ParameterMetadata.listOfEmpty(builder.initialGuess_Renamed.Count);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public ParameterizedFunctionalCurveDefinition filtered(LocalDate valuationDate, ReferenceData refData)
	  {
		// mutable list of date-node pairs
//JAVA TO C# CONVERTER TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		List<Pair<LocalDate, CurveNode>> nodeDates = nodes.Select(node => Pair.of(node.date(valuationDate, refData), node)).collect(toCollection(List<object>::new));
		// delete nodes if clash, but don't throw exceptions yet
		for (int i = 0; i < nodeDates.Count; i++)
		{
		  Pair<LocalDate, CurveNode> pair = nodeDates[i];
		  CurveNodeDateOrder restriction = pair.Second.DateOrder;
		  // compare node to previous node
		  if (i > 0)
		  {
			Pair<LocalDate, CurveNode> pairBefore = nodeDates[i - 1];
			if (DAYS.between(pairBefore.First, pair.First) < restriction.MinGapInDays)
			{
			  switch (restriction.Action)
			  {
				case DROP_THIS:
				  nodeDates.RemoveAt(i);
				  i = -1; // restart loop
				  goto loopContinue;
				case DROP_OTHER:
				  nodeDates.RemoveAt(i - 1);
				  i = -1; // restart loop
				  goto loopContinue;
				case EXCEPTION:
				  break; // do nothing yet
			  break;
			  }
			}
		  }
		  // compare node to next node
		  if (i < nodeDates.Count - 1)
		  {
			Pair<LocalDate, CurveNode> pairAfter = nodeDates[i + 1];
			if (DAYS.between(pair.First, pairAfter.First) < restriction.MinGapInDays)
			{
			  switch (restriction.Action)
			  {
				case DROP_THIS:
				  nodeDates.Remove(i);
				  i = -1; // restart loop
				  goto loopContinue;
				case DROP_OTHER:
				  nodeDates.Remove(i + 1);
				  i = -1; // restart loop
				  goto loopContinue;
				case EXCEPTION:
				  break; // do nothing yet
			  break;
			  }
			}
		  }
			loopContinue:;
		}
		loopBreak:
		// throw exceptions if rules breached
		for (int i = 0; i < nodeDates.Count; i++)
		{
		  Pair<LocalDate, CurveNode> pair = nodeDates[i];
		  CurveNodeDateOrder restriction = pair.Second.DateOrder;
		  // compare node to previous node
		  if (i > 0)
		  {
			Pair<LocalDate, CurveNode> pairBefore = nodeDates[i - 1];
			if (DAYS.between(pairBefore.First, pair.First) < restriction.MinGapInDays)
			{
			  throw new System.ArgumentException(Messages.format("Curve node dates clash, node '{}' and '{}' resolved to dates '{}' and '{}' respectively", pairBefore.Second.Label, pair.Second.Label, pairBefore.First, pair.First));
			}
		  }
		  // compare node to next node
		  if (i < nodeDates.Count - 1)
		  {
			Pair<LocalDate, CurveNode> pairAfter = nodeDates[i + 1];
			if (DAYS.between(pair.First, pairAfter.First) < restriction.MinGapInDays)
			{
			  throw new System.ArgumentException(Messages.format("Curve node dates clash, node '{}' and '{}' resolved to dates '{}' and '{}' respectively", pair.Second.Label, pairAfter.Second.Label, pair.First, pairAfter.First));
			}
		  }
		}
		// return the resolved definition
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<CurveNode> filteredNodes = nodeDates.Select(p => p.Second).collect(toImmutableList());
		return new ParameterizedFunctionalCurveDefinition(name, xValueType, yValueType, dayCount, filteredNodes, initialGuess_Renamed, parameterMetadata, valueFunction, derivativeFunction, sensitivityFunction);
	  }

	  public CurveMetadata metadata(LocalDate valuationDate, ReferenceData refData)
	  {
		return DefaultCurveMetadata.builder().curveName(name).xValueType(xValueType).yValueType(yValueType).dayCount(dayCount).parameterMetadata(parameterMetadata).build();
	  }

	  public ParameterizedFunctionalCurve curve(LocalDate valuationDate, CurveMetadata metadata, DoubleArray parameters)
	  {
		return ParameterizedFunctionalCurve.of(metadata, parameters, valueFunction, derivativeFunction, sensitivityFunction);
	  }

	  public int ParameterCount
	  {
		  get
		  {
			return initialGuess_Renamed.size();
		  }
	  }

	  public ImmutableList<double> initialGuess(MarketData marketData)
	  {
		return InitialGuess;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ParameterizedFunctionalCurveDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ParameterizedFunctionalCurveDefinition.Meta meta()
	  {
		return ParameterizedFunctionalCurveDefinition.Meta.INSTANCE;
	  }

	  static ParameterizedFunctionalCurveDefinition()
	  {
		MetaBean.register(ParameterizedFunctionalCurveDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ParameterizedFunctionalCurveDefinition.Builder builder()
	  {
		return new ParameterizedFunctionalCurveDefinition.Builder();
	  }

	  private ParameterizedFunctionalCurveDefinition<T1, T2>(CurveName name, ValueType xValueType, ValueType yValueType, DayCount dayCount, IList<T1> nodes, IList<double> initialGuess, IList<T2> parameterMetadata, System.Func<DoubleArray, double, double> valueFunction, System.Func<DoubleArray, double, double> derivativeFunction, System.Func<DoubleArray, double, DoubleArray> sensitivityFunction) where T1 : CurveNode where T2 : com.opengamma.strata.market.param.ParameterMetadata
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(xValueType, "xValueType");
		JodaBeanUtils.notNull(yValueType, "yValueType");
		JodaBeanUtils.notNull(nodes, "nodes");
		JodaBeanUtils.notNull(initialGuess, "initialGuess");
		JodaBeanUtils.notNull(parameterMetadata, "parameterMetadata");
		JodaBeanUtils.notNull(valueFunction, "valueFunction");
		JodaBeanUtils.notNull(derivativeFunction, "derivativeFunction");
		JodaBeanUtils.notNull(sensitivityFunction, "sensitivityFunction");
		this.name = name;
		this.xValueType = xValueType;
		this.yValueType = yValueType;
		this.dayCount = dayCount;
		this.nodes = ImmutableList.copyOf(nodes);
		this.initialGuess_Renamed = ImmutableList.copyOf(initialGuess);
		this.parameterMetadata = ImmutableList.copyOf(parameterMetadata);
		this.valueFunction = valueFunction;
		this.derivativeFunction = derivativeFunction;
		this.sensitivityFunction = sensitivityFunction;
	  }

	  public override ParameterizedFunctionalCurveDefinition.Meta metaBean()
	  {
		return ParameterizedFunctionalCurveDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value type, providing meaning to the x-values of the curve.
	  /// <para>
	  /// This type provides meaning to the x-values. For example, the x-value might
	  /// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType XValueType
	  {
		  get
		  {
			return xValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value type, providing meaning to the y-values of the curve.
	  /// <para>
	  /// This type provides meaning to the y-values. For example, the y-value might
	  /// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
	  /// </para>
	  /// <para>
	  /// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType YValueType
	  {
		  get
		  {
			return yValueType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count, optional.
	  /// <para>
	  /// If the x-value of the curve represents time as a year fraction, the day count
	  /// can be specified to define how the year fraction is calculated.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<DayCount> DayCount
	  {
		  get
		  {
			return Optional.ofNullable(dayCount);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nodes of the underlying instruments.
	  /// <para>
	  /// The nodes are used to find the quoted values to which the curve is calibrated.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CurveNode> Nodes
	  {
		  get
		  {
			return nodes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial guess values for the curve parameters.
	  /// <para>
	  /// The size must be the same as the number of the curve parameters.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<double> InitialGuess
	  {
		  get
		  {
			return initialGuess_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter metadata of the curve, defaulted to empty metadata instances.
	  /// <para>
	  /// The size of the list must be the same as the number of the curve parameters.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<ParameterMetadata> ParameterMetadata
	  {
		  get
		  {
			return parameterMetadata;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns y-value.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, double> ValueFunction
	  {
		  get
		  {
			return valueFunction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the derivative function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x,
	  /// i.e., the gradient of the curve.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, double> DerivativeFunction
	  {
		  get
		  {
			return derivativeFunction;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter sensitivity function.
	  /// <para>
	  /// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public System.Func<DoubleArray, double, DoubleArray> SensitivityFunction
	  {
		  get
		  {
			return sensitivityFunction;
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
		  ParameterizedFunctionalCurveDefinition other = (ParameterizedFunctionalCurveDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(xValueType, other.xValueType) && JodaBeanUtils.equal(yValueType, other.yValueType) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(nodes, other.nodes) && JodaBeanUtils.equal(initialGuess_Renamed, other.initialGuess_Renamed) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata) && JodaBeanUtils.equal(valueFunction, other.valueFunction) && JodaBeanUtils.equal(derivativeFunction, other.derivativeFunction) && JodaBeanUtils.equal(sensitivityFunction, other.sensitivityFunction);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(nodes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialGuess_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(valueFunction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(derivativeFunction);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sensitivityFunction);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(352);
		buf.Append("ParameterizedFunctionalCurveDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("xValueType").Append('=').Append(xValueType).Append(',').Append(' ');
		buf.Append("yValueType").Append('=').Append(yValueType).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("nodes").Append('=').Append(nodes).Append(',').Append(' ');
		buf.Append("initialGuess").Append('=').Append(initialGuess_Renamed).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(parameterMetadata).Append(',').Append(' ');
		buf.Append("valueFunction").Append('=').Append(valueFunction).Append(',').Append(' ');
		buf.Append("derivativeFunction").Append('=').Append(derivativeFunction).Append(',').Append(' ');
		buf.Append("sensitivityFunction").Append('=').Append(JodaBeanUtils.ToString(sensitivityFunction));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ParameterizedFunctionalCurveDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(ParameterizedFunctionalCurveDefinition), typeof(CurveName));
			  xValueType_Renamed = DirectMetaProperty.ofImmutable(this, "xValueType", typeof(ParameterizedFunctionalCurveDefinition), typeof(ValueType));
			  yValueType_Renamed = DirectMetaProperty.ofImmutable(this, "yValueType", typeof(ParameterizedFunctionalCurveDefinition), typeof(ValueType));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(ParameterizedFunctionalCurveDefinition), typeof(DayCount));
			  nodes_Renamed = DirectMetaProperty.ofImmutable(this, "nodes", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(ImmutableList));
			  initialGuess_Renamed = DirectMetaProperty.ofImmutable(this, "initialGuess", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(ImmutableList));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(ImmutableList));
			  valueFunction_Renamed = DirectMetaProperty.ofImmutable(this, "valueFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
			  derivativeFunction_Renamed = DirectMetaProperty.ofImmutable(this, "derivativeFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
			  sensitivityFunction_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivityFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "xValueType", "yValueType", "dayCount", "nodes", "initialGuess", "parameterMetadata", "valueFunction", "derivativeFunction", "sensitivityFunction");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code xValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> xValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code yValueType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ValueType> yValueType_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code nodes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CurveNode>> nodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nodes", ParameterizedFunctionalCurveDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CurveNode>> nodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialGuess} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<double>> initialGuess = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "initialGuess", ParameterizedFunctionalCurveDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<double>> initialGuess_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", ParameterizedFunctionalCurveDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-property for the {@code valueFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double>> valueFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "valueFunction", ParameterizedFunctionalCurveDefinition.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, double>> valueFunction_Renamed = DirectMetaProperty.ofImmutable(this, "valueFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-property for the {@code derivativeFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, double>> derivativeFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "derivativeFunction", ParameterizedFunctionalCurveDefinition.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, double>> derivativeFunction_Renamed = DirectMetaProperty.ofImmutable(this, "derivativeFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-property for the {@code sensitivityFunction} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<System.Func<com.opengamma.strata.collect.array.DoubleArray, double, com.opengamma.strata.collect.array.DoubleArray>> sensitivityFunction = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "sensitivityFunction", ParameterizedFunctionalCurveDefinition.class, (Class) System.Func.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<System.Func<DoubleArray, double, DoubleArray>> sensitivityFunction_Renamed = DirectMetaProperty.ofImmutable(this, "sensitivityFunction", typeof(ParameterizedFunctionalCurveDefinition), (Type) typeof(System.Func));
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "xValueType", "yValueType", "dayCount", "nodes", "initialGuess", "parameterMetadata", "valueFunction", "derivativeFunction", "sensitivityFunction");
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
			case -868509005: // xValueType
			  return xValueType_Renamed;
			case -1065022510: // yValueType
			  return yValueType_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -431632141: // initialGuess
			  return initialGuess_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
			case 636119145: // valueFunction
			  return valueFunction_Renamed;
			case 1663351423: // derivativeFunction
			  return derivativeFunction_Renamed;
			case -1353652329: // sensitivityFunction
			  return sensitivityFunction_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ParameterizedFunctionalCurveDefinition.Builder builder()
		{
		  return new ParameterizedFunctionalCurveDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ParameterizedFunctionalCurveDefinition);
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
		public MetaProperty<CurveName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code xValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> xValueType()
		{
		  return xValueType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yValueType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ValueType> yValueType()
		{
		  return yValueType_Renamed;
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
		public MetaProperty<ImmutableList<CurveNode>> nodes()
		{
		  return nodes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialGuess} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<double>> initialGuess()
		{
		  return initialGuess_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata()
		{
		  return parameterMetadata_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code valueFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, double>> valueFunction()
		{
		  return valueFunction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code derivativeFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, double>> derivativeFunction()
		{
		  return derivativeFunction_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sensitivityFunction} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<System.Func<DoubleArray, double, DoubleArray>> sensitivityFunction()
		{
		  return sensitivityFunction_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((ParameterizedFunctionalCurveDefinition) bean).Name;
			case -868509005: // xValueType
			  return ((ParameterizedFunctionalCurveDefinition) bean).XValueType;
			case -1065022510: // yValueType
			  return ((ParameterizedFunctionalCurveDefinition) bean).YValueType;
			case 1905311443: // dayCount
			  return ((ParameterizedFunctionalCurveDefinition) bean).dayCount;
			case 104993457: // nodes
			  return ((ParameterizedFunctionalCurveDefinition) bean).Nodes;
			case -431632141: // initialGuess
			  return ((ParameterizedFunctionalCurveDefinition) bean).InitialGuess;
			case -1169106440: // parameterMetadata
			  return ((ParameterizedFunctionalCurveDefinition) bean).ParameterMetadata;
			case 636119145: // valueFunction
			  return ((ParameterizedFunctionalCurveDefinition) bean).ValueFunction;
			case 1663351423: // derivativeFunction
			  return ((ParameterizedFunctionalCurveDefinition) bean).DerivativeFunction;
			case -1353652329: // sensitivityFunction
			  return ((ParameterizedFunctionalCurveDefinition) bean).SensitivityFunction;
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
	  /// The bean-builder for {@code ParameterizedFunctionalCurveDefinition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ParameterizedFunctionalCurveDefinition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueType xValueType_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal ValueType yValueType_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends CurveNode> nodes = com.google.common.collect.ImmutableList.of();
		internal IList<CurveNode> nodes_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<double> initialGuess_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata = com.google.common.collect.ImmutableList.of();
		internal IList<ParameterMetadata> parameterMetadata_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, double> valueFunction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, double> derivativeFunction_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal System.Func<DoubleArray, double, DoubleArray> sensitivityFunction_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ParameterizedFunctionalCurveDefinition beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.xValueType_Renamed = beanToCopy.XValueType;
		  this.yValueType_Renamed = beanToCopy.YValueType;
		  this.dayCount_Renamed = beanToCopy.dayCount;
		  this.nodes_Renamed = beanToCopy.Nodes;
		  this.initialGuess_Renamed = beanToCopy.InitialGuess;
		  this.parameterMetadata_Renamed = beanToCopy.ParameterMetadata;
		  this.valueFunction_Renamed = beanToCopy.ValueFunction;
		  this.derivativeFunction_Renamed = beanToCopy.DerivativeFunction;
		  this.sensitivityFunction_Renamed = beanToCopy.SensitivityFunction;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case -868509005: // xValueType
			  return xValueType_Renamed;
			case -1065022510: // yValueType
			  return yValueType_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 104993457: // nodes
			  return nodes_Renamed;
			case -431632141: // initialGuess
			  return initialGuess_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
			case 636119145: // valueFunction
			  return valueFunction_Renamed;
			case 1663351423: // derivativeFunction
			  return derivativeFunction_Renamed;
			case -1353652329: // sensitivityFunction
			  return sensitivityFunction_Renamed;
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
			  this.name_Renamed = (CurveName) newValue;
			  break;
			case -868509005: // xValueType
			  this.xValueType_Renamed = (ValueType) newValue;
			  break;
			case -1065022510: // yValueType
			  this.yValueType_Renamed = (ValueType) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 104993457: // nodes
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.nodes = (java.util.List<? extends CurveNode>) newValue;
			  this.nodes_Renamed = (IList<CurveNode>) newValue;
			  break;
			case -431632141: // initialGuess
			  this.initialGuess_Renamed = (IList<double>) newValue;
			  break;
			case -1169106440: // parameterMetadata
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.parameterMetadata = (java.util.List<? extends com.opengamma.strata.market.param.ParameterMetadata>) newValue;
			  this.parameterMetadata_Renamed = (IList<ParameterMetadata>) newValue;
			  break;
			case 636119145: // valueFunction
			  this.valueFunction_Renamed = (System.Func<DoubleArray, double, double>) newValue;
			  break;
			case 1663351423: // derivativeFunction
			  this.derivativeFunction_Renamed = (System.Func<DoubleArray, double, double>) newValue;
			  break;
			case -1353652329: // sensitivityFunction
			  this.sensitivityFunction_Renamed = (System.Func<DoubleArray, double, DoubleArray>) newValue;
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

		public override ParameterizedFunctionalCurveDefinition build()
		{
		  preBuild(this);
		  return new ParameterizedFunctionalCurveDefinition(name_Renamed, xValueType_Renamed, yValueType_Renamed, dayCount_Renamed, nodes_Renamed, initialGuess_Renamed, parameterMetadata_Renamed, valueFunction_Renamed, derivativeFunction_Renamed, sensitivityFunction_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the curve name. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(CurveName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the x-value type, providing meaning to the x-values of the curve.
		/// <para>
		/// This type provides meaning to the x-values. For example, the x-value might
		/// represent a year fraction, as represented using <seealso cref="ValueType#YEAR_FRACTION"/>.
		/// </para>
		/// <para>
		/// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
		/// </para>
		/// </summary>
		/// <param name="xValueType">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder xValueType(ValueType xValueType)
		{
		  JodaBeanUtils.notNull(xValueType, "xValueType");
		  this.xValueType_Renamed = xValueType;
		  return this;
		}

		/// <summary>
		/// Sets the y-value type, providing meaning to the y-values of the curve.
		/// <para>
		/// This type provides meaning to the y-values. For example, the y-value might
		/// represent a zero rate, as represented using <seealso cref="ValueType#ZERO_RATE"/>.
		/// </para>
		/// <para>
		/// If using the builder, this defaults to <seealso cref="ValueType#UNKNOWN"/>.
		/// </para>
		/// </summary>
		/// <param name="yValueType">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder yValueType(ValueType yValueType)
		{
		  JodaBeanUtils.notNull(yValueType, "yValueType");
		  this.yValueType_Renamed = yValueType;
		  return this;
		}

		/// <summary>
		/// Sets the day count, optional.
		/// <para>
		/// If the x-value of the curve represents time as a year fraction, the day count
		/// can be specified to define how the year fraction is calculated.
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
		/// Sets the nodes of the underlying instruments.
		/// <para>
		/// The nodes are used to find the quoted values to which the curve is calibrated.
		/// </para>
		/// </summary>
		/// <param name="nodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder nodes<T1>(IList<T1> nodes) where T1 : CurveNode
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
		public Builder nodes(params CurveNode[] nodes)
		{
		  return this.nodes(ImmutableList.copyOf(nodes));
		}

		/// <summary>
		/// Sets the initial guess values for the curve parameters.
		/// <para>
		/// The size must be the same as the number of the curve parameters.
		/// </para>
		/// </summary>
		/// <param name="initialGuess">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialGuess(IList<double> initialGuess)
		{
		  JodaBeanUtils.notNull(initialGuess, "initialGuess");
		  this.initialGuess_Renamed = initialGuess;
		  return this;
		}

		/// <summary>
		/// Sets the {@code initialGuess} property in the builder
		/// from an array of objects. </summary>
		/// <param name="initialGuess">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialGuess(params Double[] initialGuess)
		{
		  return this.initialGuess(ImmutableList.copyOf(initialGuess));
		}

		/// <summary>
		/// Sets the parameter metadata of the curve, defaulted to empty metadata instances.
		/// <para>
		/// The size of the list must be the same as the number of the curve parameters.
		/// </para>
		/// </summary>
		/// <param name="parameterMetadata">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameterMetadata<T1>(IList<T1> parameterMetadata) where T1 : com.opengamma.strata.market.param.ParameterMetadata
		{
		  JodaBeanUtils.notNull(parameterMetadata, "parameterMetadata");
		  this.parameterMetadata_Renamed = parameterMetadata;
		  return this;
		}

		/// <summary>
		/// Sets the {@code parameterMetadata} property in the builder
		/// from an array of objects. </summary>
		/// <param name="parameterMetadata">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameterMetadata(params ParameterMetadata[] parameterMetadata)
		{
		  return this.parameterMetadata(ImmutableList.copyOf(parameterMetadata));
		}

		/// <summary>
		/// Sets the y-value function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns y-value.
		/// </para>
		/// </summary>
		/// <param name="valueFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder valueFunction(System.Func<DoubleArray, double, double> valueFunction)
		{
		  JodaBeanUtils.notNull(valueFunction, "valueFunction");
		  this.valueFunction_Renamed = valueFunction;
		  return this;
		}

		/// <summary>
		/// Sets the derivative function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns the first derivative of y-value with respective to x,
		/// i.e., the gradient of the curve.
		/// </para>
		/// </summary>
		/// <param name="derivativeFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder derivativeFunction(System.Func<DoubleArray, double, double> derivativeFunction)
		{
		  JodaBeanUtils.notNull(derivativeFunction, "derivativeFunction");
		  this.derivativeFunction_Renamed = derivativeFunction;
		  return this;
		}

		/// <summary>
		/// Sets the parameter sensitivity function.
		/// <para>
		/// The function takes {@code parameters} and x-value, then returns the sensitivities of y-value to the parameters.
		/// </para>
		/// </summary>
		/// <param name="sensitivityFunction">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder sensitivityFunction(System.Func<DoubleArray, double, DoubleArray> sensitivityFunction)
		{
		  JodaBeanUtils.notNull(sensitivityFunction, "sensitivityFunction");
		  this.sensitivityFunction_Renamed = sensitivityFunction;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(352);
		  buf.Append("ParameterizedFunctionalCurveDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("xValueType").Append('=').Append(JodaBeanUtils.ToString(xValueType_Renamed)).Append(',').Append(' ');
		  buf.Append("yValueType").Append('=').Append(JodaBeanUtils.ToString(yValueType_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("nodes").Append('=').Append(JodaBeanUtils.ToString(nodes_Renamed)).Append(',').Append(' ');
		  buf.Append("initialGuess").Append('=').Append(JodaBeanUtils.ToString(initialGuess_Renamed)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata_Renamed)).Append(',').Append(' ');
		  buf.Append("valueFunction").Append('=').Append(JodaBeanUtils.ToString(valueFunction_Renamed)).Append(',').Append(' ');
		  buf.Append("derivativeFunction").Append('=').Append(JodaBeanUtils.ToString(derivativeFunction_Renamed)).Append(',').Append(' ');
		  buf.Append("sensitivityFunction").Append('=').Append(JodaBeanUtils.ToString(sensitivityFunction_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}