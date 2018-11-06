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
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableDefaults = org.joda.beans.gen.ImmutableDefaults;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
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
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using DatedParameterMetadata = com.opengamma.strata.market.param.DatedParameterMetadata;

	/// <summary>
	/// Provides the definition of how to calibrate an interpolated nodal curve.
	/// <para>
	/// A nodal curve is built from a number of parameters and described by metadata.
	/// Calibration is based on a list of <seealso cref="CurveNode"/> instances, one for each parameter,
	/// that specify the underlying instruments.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class InterpolatedNodalCurveDefinition implements NodalCurveDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InterpolatedNodalCurveDefinition : NodalCurveDefinition, ImmutableBean
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
	  /// The nodes in the curve.
	  /// <para>
	  /// The nodes are used to find the par rates and calibrate the curve.
	  /// There must be at least two nodes in the curve.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends CurveNode>", overrideGet = true) private final com.google.common.collect.ImmutableList<CurveNode> nodes;
	  private readonly ImmutableList<CurveNode> nodes;
	  /// <summary>
	  /// The interpolator used to find points on the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator interpolator;
	  private readonly CurveInterpolator interpolator;
	  /// <summary>
	  /// The extrapolator used to find points to the left of the leftmost point on the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorLeft;
	  private readonly CurveExtrapolator extrapolatorLeft;
	  /// <summary>
	  /// The extrapolator used to find points to the right of the rightmost point on the curve.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorRight;
	  private readonly CurveExtrapolator extrapolatorRight;

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableDefaults private static void applyDefaults(Builder builder)
	  private static void applyDefaults(Builder builder)
	  {
		builder.xValueType_Renamed = ValueType.UNKNOWN;
		builder.yValueType_Renamed = ValueType.UNKNOWN;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		if (nodes.size() < 2)
		{
		  throw new System.ArgumentException("Curve must have at least two nodes");
		}
	  }

	  //-------------------------------------------------------------------------
	  public InterpolatedNodalCurveDefinition filtered(LocalDate valuationDate, ReferenceData refData)
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
		return new InterpolatedNodalCurveDefinition(name, xValueType, yValueType, dayCount, filteredNodes, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  public CurveMetadata metadata(LocalDate valuationDate, ReferenceData refData)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<DatedParameterMetadata> nodeMetadata = nodes.Select(node => node.metadata(valuationDate, refData)).collect(toImmutableList());
		return DefaultCurveMetadata.builder().curveName(name).xValueType(xValueType).yValueType(yValueType).dayCount(dayCount).parameterMetadata(nodeMetadata).build();
	  }

	  //-------------------------------------------------------------------------
	  public NodalCurve curve(LocalDate valuationDate, CurveMetadata metadata, DoubleArray parameters)
	  {

		DoubleArray nodeTimes = buildNodeTimes(valuationDate, metadata);
		return InterpolatedNodalCurve.builder().metadata(metadata).xValues(nodeTimes).yValues(parameters).extrapolatorLeft(extrapolatorLeft).interpolator(interpolator).extrapolatorRight(extrapolatorRight).build();
	  }

	  // builds node times from node dates
	  private DoubleArray buildNodeTimes(LocalDate valuationDate, CurveMetadata metadata)
	  {
		if (metadata.XValueType.Equals(ValueType.YEAR_FRACTION))
		{
		  return DoubleArray.of(ParameterCount, i =>
		  {
		  LocalDate nodeDate = ((DatedParameterMetadata) metadata.ParameterMetadata.get().get(i)).Date;
		  return DayCount.get().yearFraction(valuationDate, nodeDate);
		  });

		}
		else if (metadata.XValueType.Equals(ValueType.MONTHS))
		{
		  return DoubleArray.of(ParameterCount, i =>
		  {
		  LocalDate nodeDate = ((DatedParameterMetadata) metadata.ParameterMetadata.get().get(i)).Date;
		  return YearMonth.from(valuationDate).until(YearMonth.from(nodeDate), MONTHS);
		  });

		}
		else
		{
		  throw new System.ArgumentException("Metadata XValueType should be YearFraction or Months in curve definition");
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedNodalCurveDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InterpolatedNodalCurveDefinition.Meta meta()
	  {
		return InterpolatedNodalCurveDefinition.Meta.INSTANCE;
	  }

	  static InterpolatedNodalCurveDefinition()
	  {
		MetaBean.register(InterpolatedNodalCurveDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static InterpolatedNodalCurveDefinition.Builder builder()
	  {
		return new InterpolatedNodalCurveDefinition.Builder();
	  }

	  private InterpolatedNodalCurveDefinition<T1>(CurveName name, ValueType xValueType, ValueType yValueType, DayCount dayCount, IList<T1> nodes, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight) where T1 : CurveNode
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(xValueType, "xValueType");
		JodaBeanUtils.notNull(yValueType, "yValueType");
		JodaBeanUtils.notNull(nodes, "nodes");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		this.name = name;
		this.xValueType = xValueType;
		this.yValueType = yValueType;
		this.dayCount = dayCount;
		this.nodes = ImmutableList.copyOf(nodes);
		this.interpolator = interpolator;
		this.extrapolatorLeft = extrapolatorLeft;
		this.extrapolatorRight = extrapolatorRight;
		validate();
	  }

	  public override InterpolatedNodalCurveDefinition.Meta metaBean()
	  {
		return InterpolatedNodalCurveDefinition.Meta.INSTANCE;
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
	  /// Gets the nodes in the curve.
	  /// <para>
	  /// The nodes are used to find the par rates and calibrate the curve.
	  /// There must be at least two nodes in the curve.
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
	  /// Gets the interpolator used to find points on the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator Interpolator
	  {
		  get
		  {
			return interpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the extrapolator used to find points to the left of the leftmost point on the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorLeft
	  {
		  get
		  {
			return extrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the extrapolator used to find points to the right of the rightmost point on the curve. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorRight
	  {
		  get
		  {
			return extrapolatorRight;
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
		  InterpolatedNodalCurveDefinition other = (InterpolatedNodalCurveDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(xValueType, other.xValueType) && JodaBeanUtils.equal(yValueType, other.yValueType) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(nodes, other.nodes) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(extrapolatorLeft, other.extrapolatorLeft) && JodaBeanUtils.equal(extrapolatorRight, other.extrapolatorRight);
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
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("InterpolatedNodalCurveDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("xValueType").Append('=').Append(xValueType).Append(',').Append(' ');
		buf.Append("yValueType").Append('=').Append(yValueType).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("nodes").Append('=').Append(nodes).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("extrapolatorLeft").Append('=').Append(extrapolatorLeft).Append(',').Append(' ');
		buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedNodalCurveDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(InterpolatedNodalCurveDefinition), typeof(CurveName));
			  xValueType_Renamed = DirectMetaProperty.ofImmutable(this, "xValueType", typeof(InterpolatedNodalCurveDefinition), typeof(ValueType));
			  yValueType_Renamed = DirectMetaProperty.ofImmutable(this, "yValueType", typeof(InterpolatedNodalCurveDefinition), typeof(ValueType));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(InterpolatedNodalCurveDefinition), typeof(DayCount));
			  nodes_Renamed = DirectMetaProperty.ofImmutable(this, "nodes", typeof(InterpolatedNodalCurveDefinition), (Type) typeof(ImmutableList));
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(InterpolatedNodalCurveDefinition), typeof(CurveInterpolator));
			  extrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorLeft", typeof(InterpolatedNodalCurveDefinition), typeof(CurveExtrapolator));
			  extrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorRight", typeof(InterpolatedNodalCurveDefinition), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "xValueType", "yValueType", "dayCount", "nodes", "interpolator", "extrapolatorLeft", "extrapolatorRight");
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
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CurveNode>> nodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "nodes", InterpolatedNodalCurveDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CurveNode>> nodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code interpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> interpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "xValueType", "yValueType", "dayCount", "nodes", "interpolator", "extrapolatorLeft", "extrapolatorRight");
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
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override InterpolatedNodalCurveDefinition.Builder builder()
		{
		  return new InterpolatedNodalCurveDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InterpolatedNodalCurveDefinition);
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
		/// The meta-property for the {@code interpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> interpolator()
		{
		  return interpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorLeft()
		{
		  return extrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorRight()
		{
		  return extrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((InterpolatedNodalCurveDefinition) bean).Name;
			case -868509005: // xValueType
			  return ((InterpolatedNodalCurveDefinition) bean).XValueType;
			case -1065022510: // yValueType
			  return ((InterpolatedNodalCurveDefinition) bean).YValueType;
			case 1905311443: // dayCount
			  return ((InterpolatedNodalCurveDefinition) bean).dayCount;
			case 104993457: // nodes
			  return ((InterpolatedNodalCurveDefinition) bean).Nodes;
			case 2096253127: // interpolator
			  return ((InterpolatedNodalCurveDefinition) bean).Interpolator;
			case 1271703994: // extrapolatorLeft
			  return ((InterpolatedNodalCurveDefinition) bean).ExtrapolatorLeft;
			case 773779145: // extrapolatorRight
			  return ((InterpolatedNodalCurveDefinition) bean).ExtrapolatorRight;
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
	  /// The bean-builder for {@code InterpolatedNodalCurveDefinition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<InterpolatedNodalCurveDefinition>
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
		internal CurveInterpolator interpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorRight_Renamed;

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
		internal Builder(InterpolatedNodalCurveDefinition beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.xValueType_Renamed = beanToCopy.XValueType;
		  this.yValueType_Renamed = beanToCopy.YValueType;
		  this.dayCount_Renamed = beanToCopy.dayCount;
		  this.nodes_Renamed = beanToCopy.Nodes;
		  this.interpolator_Renamed = beanToCopy.Interpolator;
		  this.extrapolatorLeft_Renamed = beanToCopy.ExtrapolatorLeft;
		  this.extrapolatorRight_Renamed = beanToCopy.ExtrapolatorRight;
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
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
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
			case 2096253127: // interpolator
			  this.interpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case 1271703994: // extrapolatorLeft
			  this.extrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case 773779145: // extrapolatorRight
			  this.extrapolatorRight_Renamed = (CurveExtrapolator) newValue;
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

		public override InterpolatedNodalCurveDefinition build()
		{
		  return new InterpolatedNodalCurveDefinition(name_Renamed, xValueType_Renamed, yValueType_Renamed, dayCount_Renamed, nodes_Renamed, interpolator_Renamed, extrapolatorLeft_Renamed, extrapolatorRight_Renamed);
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
		/// Sets the nodes in the curve.
		/// <para>
		/// The nodes are used to find the par rates and calibrate the curve.
		/// There must be at least two nodes in the curve.
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
		/// Sets the interpolator used to find points on the curve. </summary>
		/// <param name="interpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder interpolator(CurveInterpolator interpolator)
		{
		  JodaBeanUtils.notNull(interpolator, "interpolator");
		  this.interpolator_Renamed = interpolator;
		  return this;
		}

		/// <summary>
		/// Sets the extrapolator used to find points to the left of the leftmost point on the curve. </summary>
		/// <param name="extrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorLeft(CurveExtrapolator extrapolatorLeft)
		{
		  JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		  this.extrapolatorLeft_Renamed = extrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the extrapolator used to find points to the right of the rightmost point on the curve. </summary>
		/// <param name="extrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorRight(CurveExtrapolator extrapolatorRight)
		{
		  JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		  this.extrapolatorRight_Renamed = extrapolatorRight;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("InterpolatedNodalCurveDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("xValueType").Append('=').Append(JodaBeanUtils.ToString(xValueType_Renamed)).Append(',').Append(' ');
		  buf.Append("yValueType").Append('=').Append(JodaBeanUtils.ToString(yValueType_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("nodes").Append('=').Append(JodaBeanUtils.ToString(nodes_Renamed)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(extrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}