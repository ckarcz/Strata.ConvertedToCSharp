using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.loader.csv
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ValueType = com.opengamma.strata.market.ValueType;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using CurveNode = com.opengamma.strata.market.curve.CurveNode;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using InterpolatedNodalCurveDefinition = com.opengamma.strata.market.curve.InterpolatedNodalCurveDefinition;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using LabelDateParameterMetadata = com.opengamma.strata.market.param.LabelDateParameterMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Represents curve settings, used when loading curves.
	/// <para>
	/// This contains settings that apply across all instances of a particular curve.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class LoadedCurveSettings implements org.joda.beans.ImmutableBean
	internal sealed class LoadedCurveSettings : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.CurveName curveName;
		private readonly CurveName curveName;
	  /// <summary>
	  /// The x-value type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType xValueType;
	  private readonly ValueType xValueType;
	  /// <summary>
	  /// The y-value type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType yValueType;
	  private readonly ValueType yValueType;
	  /// <summary>
	  /// The day count convention.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The interpolator.
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
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="curveName">  the curve name </param>
	  /// <param name="yValueType">  the value type </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <returns> the curve settings </returns>
	  internal static LoadedCurveSettings of(CurveName curveName, ValueType xValueType, ValueType yValueType, DayCount dayCount, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight)
	  {

		return new LoadedCurveSettings(curveName, xValueType, yValueType, dayCount, interpolator, extrapolatorLeft, extrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  // constructs an interpolated nodal curve
	  internal InterpolatedNodalCurve createCurve(LocalDate date, IList<LoadedCurveNode> curveNodes)
	  {
		// copy and sort
		IList<LoadedCurveNode> nodes = new List<LoadedCurveNode>(curveNodes);
		nodes.sort(System.Collections.IComparer.naturalOrder());

		// build each node
		double[] xValues = new double[nodes.Count];
		double[] yValues = new double[nodes.Count];
		IList<ParameterMetadata> pointsMetadata = new List<ParameterMetadata>(nodes.Count);
		for (int i = 0; i < nodes.Count; i++)
		{
		  LoadedCurveNode point = nodes[i];
		  double yearFraction = dayCount.yearFraction(date, point.Date);
		  xValues[i] = yearFraction;
		  yValues[i] = point.Value;
		  ParameterMetadata pointMetadata = LabelDateParameterMetadata.of(point.Date, point.Label);
		  pointsMetadata.Add(pointMetadata);
		}

		// create metadata
		CurveMetadata curveMetadata = DefaultCurveMetadata.builder().curveName(curveName).xValueType(xValueType).yValueType(yValueType).dayCount(dayCount).parameterMetadata(pointsMetadata).build();
		return InterpolatedNodalCurve.builder().metadata(curveMetadata).xValues(DoubleArray.copyOf(xValues)).yValues(DoubleArray.copyOf(yValues)).interpolator(interpolator).extrapolatorLeft(extrapolatorLeft).extrapolatorRight(extrapolatorRight).build();
	  }

	  // constructs an interpolated nodal curve definition
	  internal InterpolatedNodalCurveDefinition createCurveDefinition(IList<CurveNode> nodes)
	  {
		return InterpolatedNodalCurveDefinition.builder().name(curveName).xValueType(xValueType).yValueType(yValueType).dayCount(dayCount).nodes(nodes).interpolator(interpolator).extrapolatorLeft(extrapolatorLeft).extrapolatorRight(extrapolatorRight).build();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code LoadedCurveSettings}.
	  /// </summary>
	  private static readonly TypedMetaBean<LoadedCurveSettings> META_BEAN = LightMetaBean.of(typeof(LoadedCurveSettings), MethodHandles.lookup(), new string[] {"curveName", "xValueType", "yValueType", "dayCount", "interpolator", "extrapolatorLeft", "extrapolatorRight"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code LoadedCurveSettings}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<LoadedCurveSettings> meta()
	  {
		return META_BEAN;
	  }

	  static LoadedCurveSettings()
	  {
		MetaBean.register(META_BEAN);
	  }

	  private LoadedCurveSettings(CurveName curveName, ValueType xValueType, ValueType yValueType, DayCount dayCount, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight)
	  {
		JodaBeanUtils.notNull(curveName, "curveName");
		JodaBeanUtils.notNull(xValueType, "xValueType");
		JodaBeanUtils.notNull(yValueType, "yValueType");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		this.curveName = curveName;
		this.xValueType = xValueType;
		this.yValueType = yValueType;
		this.dayCount = dayCount;
		this.interpolator = interpolator;
		this.extrapolatorLeft = extrapolatorLeft;
		this.extrapolatorRight = extrapolatorRight;
	  }

	  public override TypedMetaBean<LoadedCurveSettings> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve name. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveName CurveName
	  {
		  get
		  {
			return curveName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value type. </summary>
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
	  /// Gets the y-value type. </summary>
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
	  /// Gets the day count convention. </summary>
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
	  /// Gets the interpolator. </summary>
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
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  LoadedCurveSettings other = (LoadedCurveSettings) obj;
		  return JodaBeanUtils.equal(curveName, other.curveName) && JodaBeanUtils.equal(xValueType, other.xValueType) && JodaBeanUtils.equal(yValueType, other.yValueType) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(extrapolatorLeft, other.extrapolatorLeft) && JodaBeanUtils.equal(extrapolatorRight, other.extrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(curveName);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yValueType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("LoadedCurveSettings{");
		buf.Append("curveName").Append('=').Append(curveName).Append(',').Append(' ');
		buf.Append("xValueType").Append('=').Append(xValueType).Append(',').Append(' ');
		buf.Append("yValueType").Append('=').Append(yValueType).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("extrapolatorLeft").Append('=').Append(extrapolatorLeft).Append(',').Append(' ');
		buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}