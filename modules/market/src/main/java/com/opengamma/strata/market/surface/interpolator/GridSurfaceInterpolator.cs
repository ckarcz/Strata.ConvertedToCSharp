using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface.interpolator
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using BoundCurveExtrapolator = com.opengamma.strata.market.curve.interpolator.BoundCurveExtrapolator;
	using BoundCurveInterpolator = com.opengamma.strata.market.curve.interpolator.BoundCurveInterpolator;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;

	/// <summary>
	/// A surface interpolator that is based on two curve interpolators.
	/// <para>
	/// The surface parameters are divided into rows and columns based on the x-values
	/// and y-values. There must be at least two y-values for each x-value.
	/// In most cases, the parameters will form a rectangular grid.
	/// </para>
	/// <para>
	/// The interpolation operates in two stages.
	/// First, the parameters are grouped into sets, each with the same x value.
	/// Second, the y curve interpolator is used on each set of y values.
	/// Finally, the x curve interpolator is used on the results of the y interpolation.
	/// </para>
	/// <para>
	/// There should be at least two different y-values for each x-value.
	/// If there is only one, then the associated z-value will always be returned.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class GridSurfaceInterpolator implements SurfaceInterpolator, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class GridSurfaceInterpolator : SurfaceInterpolator, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator xInterpolator;
		private readonly CurveInterpolator xInterpolator;
	  /// <summary>
	  /// The x-value left extrapolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator xExtrapolatorLeft;
	  private readonly CurveExtrapolator xExtrapolatorLeft;
	  /// <summary>
	  /// The x-value right extrapolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator xExtrapolatorRight;
	  private readonly CurveExtrapolator xExtrapolatorRight;
	  /// <summary>
	  /// The y-value interpolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator yInterpolator;
	  private readonly CurveInterpolator yInterpolator;
	  /// <summary>
	  /// The y-value left extrapolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator yExtrapolatorLeft;
	  private readonly CurveExtrapolator yExtrapolatorLeft;
	  /// <summary>
	  /// The y-value right extrapolator.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator yExtrapolatorRight;
	  private readonly CurveExtrapolator yExtrapolatorRight;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified interpolators, using flat extrapolation.
	  /// </summary>
	  /// <param name="xInterpolator">  the x-value interpolator </param>
	  /// <param name="yInterpolator">  the y-value interpolator </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
	  public static GridSurfaceInterpolator of(CurveInterpolator xInterpolator, CurveInterpolator yInterpolator)
	  {
		return new GridSurfaceInterpolator(xInterpolator, FLAT, FLAT, yInterpolator, FLAT, FLAT);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified interpolators and extrapolators.
	  /// </summary>
	  /// <param name="xInterpolator">  the x-value interpolator </param>
	  /// <param name="xExtrapolator">  the x-value extrapolator </param>
	  /// <param name="yInterpolator">  the y-value interpolator </param>
	  /// <param name="yExtrapolator">  the y-value extrapolator </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
	  public static GridSurfaceInterpolator of(CurveInterpolator xInterpolator, CurveExtrapolator xExtrapolator, CurveInterpolator yInterpolator, CurveExtrapolator yExtrapolator)
	  {

		return new GridSurfaceInterpolator(xInterpolator, xExtrapolator, xExtrapolator, yInterpolator, yExtrapolator, yExtrapolator);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified interpolators and extrapolators.
	  /// </summary>
	  /// <param name="xInterpolator">  the x-value interpolator </param>
	  /// <param name="xExtrapolatorLeft">  the x-value left extrapolator </param>
	  /// <param name="xExtrapolatorRight">  the x-value right extrapolator </param>
	  /// <param name="yInterpolator">  the y-value interpolator </param>
	  /// <param name="yExtrapolatorLeft">  the y-value left extrapolator </param>
	  /// <param name="yExtrapolatorRight">  the y-value right extrapolator </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
	  public static GridSurfaceInterpolator of(CurveInterpolator xInterpolator, CurveExtrapolator xExtrapolatorLeft, CurveExtrapolator xExtrapolatorRight, CurveInterpolator yInterpolator, CurveExtrapolator yExtrapolatorLeft, CurveExtrapolator yExtrapolatorRight)
	  {

		return new GridSurfaceInterpolator(xInterpolator, xExtrapolatorLeft, xExtrapolatorRight, yInterpolator, yExtrapolatorLeft, yExtrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  public BoundSurfaceInterpolator bind(DoubleArray xValues, DoubleArray yValues, DoubleArray zValues)
	  {
		// single loop around all parameters, collecting data
		int size = xValues.size();
		int countUniqueX = 0;
		double[] uniqueX = new double[size];
		double[] tempY = new double[size];
		double[] tempZ = new double[size];
		ImmutableList.Builder<BoundCurveInterpolator> yInterpBuilder = ImmutableList.builder();
		int i = 0;
		while (i < size)
		{
		  double currentX = xValues.get(i);
		  uniqueX[countUniqueX] = currentX;
		  if (countUniqueX > 0 && uniqueX[countUniqueX - 1] > uniqueX[countUniqueX])
		  {
			throw new System.ArgumentException("Array of x-values must be sorted");
		  }
		  int countSameX = 0;
		  while (i < size && xValues.get(i) == currentX)
		  {
			tempY[countSameX] = yValues.get(i);
			tempZ[countSameX] = zValues.get(i);
			if (countSameX > 0 && tempY[countSameX - 1] >= tempY[countSameX])
			{
			  throw new System.ArgumentException("Array of y-values must be sorted and unique within x-values");
			}
			countSameX++;
			i++;
		  }
		  // create a curve for the same x-value
		  if (countSameX == 1)
		  {
			// when there is only one point, there is not enough data for a curve
			// so the value must be returned without using the configured interpolator or extrapolator
			yInterpBuilder.add(new ConstantCurveInterpolator(tempZ[0]));
		  }
		  else
		  {
			// normal case, where the curve is created
			DoubleArray yValuesSameX = DoubleArray.ofUnsafe(Arrays.copyOf(tempY, countSameX));
			DoubleArray zValuesSameX = DoubleArray.ofUnsafe(Arrays.copyOf(tempZ, countSameX));
			yInterpBuilder.add(yInterpolator.bind(yValuesSameX, zValuesSameX, yExtrapolatorLeft, yExtrapolatorRight));
		  }
		  countUniqueX++;
		}
		if (countUniqueX == 1)
		{
		  throw new System.ArgumentException("Surface interpolator requires at least two different x-values");
		}
		DoubleArray uniqueXArray = DoubleArray.ofUnsafe(Arrays.copyOf(uniqueX, countUniqueX));
		BoundCurveInterpolator[] yInterps = yInterpBuilder.build().toArray(new BoundCurveInterpolator[0]);
		return new Bound(xInterpolator, xExtrapolatorLeft, xExtrapolatorRight, size, uniqueXArray, yInterps);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Bound interpolator.
	  /// </summary>
	  internal class Bound : BoundSurfaceInterpolator
	  {
		internal readonly CurveInterpolator xInterpolator;
		internal readonly CurveExtrapolator xExtrapolatorLeft;
		internal readonly CurveExtrapolator xExtrapolatorRight;
		internal readonly DoubleArray xValuesUnique;
		internal readonly int paramSize;
		internal readonly BoundCurveInterpolator[] yInterpolators;

		internal Bound(CurveInterpolator xInterpolator, CurveExtrapolator xExtrapolatorLeft, CurveExtrapolator xExtrapolatorRight, int paramSize, DoubleArray xValuesUnique, BoundCurveInterpolator[] yInterpolators)
		{

		  this.xInterpolator = xInterpolator;
		  this.xExtrapolatorLeft = xExtrapolatorLeft;
		  this.xExtrapolatorRight = xExtrapolatorRight;
		  this.xValuesUnique = xValuesUnique;
		  this.paramSize = paramSize;
		  this.yInterpolators = yInterpolators;
		}

		//-------------------------------------------------------------------------
		public virtual double interpolate(double x, double y)
		{
		  // use each y-interpolator to find the z-value for each unique x
		  DoubleArray zValuesEffective = DoubleArray.of(yInterpolators.Length, i => yInterpolators[i].interpolate(y));
		  // interpolate unique x-values against derived z-values
		  return xInterpolator.bind(xValuesUnique, zValuesEffective, xExtrapolatorLeft, xExtrapolatorRight).interpolate(x);
		}

		public virtual DoubleArray parameterSensitivity(double x, double y)
		{
		  int uniqueX = yInterpolators.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.opengamma.strata.collect.array.DoubleArray[] ySens = new com.opengamma.strata.collect.array.DoubleArray[uniqueX];
		  DoubleArray[] ySens = new DoubleArray[uniqueX];
		  // use each y-interpolator to find the z-value sensitivity for each unique x
		  for (int i = 0; i < uniqueX; i++)
		  {
			ySens[i] = yInterpolators[i].parameterSensitivity(y);
		  }
		  // use each y-interpolator to find the z-value for each unique x
		  DoubleArray zValuesEffective = DoubleArray.of(uniqueX, i => yInterpolators[i].interpolate(y));
		  // find the sensitivity of the unique x-values against derived z-values
		  DoubleArray xSens = xInterpolator.bind(xValuesUnique, zValuesEffective, xExtrapolatorLeft, xExtrapolatorRight).parameterSensitivity(x);

		  return project(xSens, ySens);
		}

		// project sensitivities back to parameters
		internal virtual DoubleArray project(DoubleArray xSens, DoubleArray[] ySens)
		{
		  int countParam = 0;
		  double[] paramSens = new double[paramSize];
		  for (int i = 0; i < xSens.size(); i++)
		  {
			double xs = xSens.get(i);
			DoubleArray ys = ySens[i];
			for (int j = 0; j < ys.size(); j++)
			{
			  paramSens[countParam++] = xs * ys.get(j);
			}
		  }
		  return DoubleArray.ofUnsafe(paramSens);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// An interpolator that returns the single known value.
	  /// </summary>
	  internal class ConstantCurveInterpolator : BoundCurveInterpolator
	  {
		internal readonly double value;

		public ConstantCurveInterpolator(double value)
		{
		  this.value = value;
		}

		public virtual double interpolate(double x)
		{
		  return value;
		}

		public virtual double firstDerivative(double x)
		{
		  return 0;
		}

		public virtual DoubleArray parameterSensitivity(double x)
		{
		  return DoubleArray.of(1);
		}

		public virtual BoundCurveInterpolator bind(BoundCurveExtrapolator extrapolatorLeft, BoundCurveExtrapolator extrapolatorRight)
		{
		  return this;
		}
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code GridSurfaceInterpolator}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static GridSurfaceInterpolator.Meta meta()
	  {
		return GridSurfaceInterpolator.Meta.INSTANCE;
	  }

	  static GridSurfaceInterpolator()
	  {
		MetaBean.register(GridSurfaceInterpolator.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private GridSurfaceInterpolator(CurveInterpolator xInterpolator, CurveExtrapolator xExtrapolatorLeft, CurveExtrapolator xExtrapolatorRight, CurveInterpolator yInterpolator, CurveExtrapolator yExtrapolatorLeft, CurveExtrapolator yExtrapolatorRight)
	  {
		this.xInterpolator = xInterpolator;
		this.xExtrapolatorLeft = xExtrapolatorLeft;
		this.xExtrapolatorRight = xExtrapolatorRight;
		this.yInterpolator = yInterpolator;
		this.yExtrapolatorLeft = yExtrapolatorLeft;
		this.yExtrapolatorRight = yExtrapolatorRight;
	  }

	  public override GridSurfaceInterpolator.Meta metaBean()
	  {
		return GridSurfaceInterpolator.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value interpolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveInterpolator XInterpolator
	  {
		  get
		  {
			return xInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value left extrapolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveExtrapolator XExtrapolatorLeft
	  {
		  get
		  {
			return xExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the x-value right extrapolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveExtrapolator XExtrapolatorRight
	  {
		  get
		  {
			return xExtrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value interpolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveInterpolator YInterpolator
	  {
		  get
		  {
			return yInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value left extrapolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveExtrapolator YExtrapolatorLeft
	  {
		  get
		  {
			return yExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the y-value right extrapolator. </summary>
	  /// <returns> the value of the property </returns>
	  public CurveExtrapolator YExtrapolatorRight
	  {
		  get
		  {
			return yExtrapolatorRight;
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
		  GridSurfaceInterpolator other = (GridSurfaceInterpolator) obj;
		  return JodaBeanUtils.equal(xInterpolator, other.xInterpolator) && JodaBeanUtils.equal(xExtrapolatorLeft, other.xExtrapolatorLeft) && JodaBeanUtils.equal(xExtrapolatorRight, other.xExtrapolatorRight) && JodaBeanUtils.equal(yInterpolator, other.yInterpolator) && JodaBeanUtils.equal(yExtrapolatorLeft, other.yExtrapolatorLeft) && JodaBeanUtils.equal(yExtrapolatorRight, other.yExtrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(xExtrapolatorRight);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(yExtrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(224);
		buf.Append("GridSurfaceInterpolator{");
		buf.Append("xInterpolator").Append('=').Append(xInterpolator).Append(',').Append(' ');
		buf.Append("xExtrapolatorLeft").Append('=').Append(xExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("xExtrapolatorRight").Append('=').Append(xExtrapolatorRight).Append(',').Append(' ');
		buf.Append("yInterpolator").Append('=').Append(yInterpolator).Append(',').Append(' ');
		buf.Append("yExtrapolatorLeft").Append('=').Append(yExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("yExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(yExtrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code GridSurfaceInterpolator}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  xInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "xInterpolator", typeof(GridSurfaceInterpolator), typeof(CurveInterpolator));
			  xExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "xExtrapolatorLeft", typeof(GridSurfaceInterpolator), typeof(CurveExtrapolator));
			  xExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "xExtrapolatorRight", typeof(GridSurfaceInterpolator), typeof(CurveExtrapolator));
			  yInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "yInterpolator", typeof(GridSurfaceInterpolator), typeof(CurveInterpolator));
			  yExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "yExtrapolatorLeft", typeof(GridSurfaceInterpolator), typeof(CurveExtrapolator));
			  yExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "yExtrapolatorRight", typeof(GridSurfaceInterpolator), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "xInterpolator", "xExtrapolatorLeft", "xExtrapolatorRight", "yInterpolator", "yExtrapolatorLeft", "yExtrapolatorRight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code xInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> xInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code xExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> xExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code xExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> xExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-property for the {@code yInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> yInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code yExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> yExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code yExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> yExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "xInterpolator", "xExtrapolatorLeft", "xExtrapolatorRight", "yInterpolator", "yExtrapolatorLeft", "yExtrapolatorRight");
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
			case 1411950943: // xInterpolator
			  return xInterpolator_Renamed;
			case -382665134: // xExtrapolatorLeft
			  return xExtrapolatorLeft_Renamed;
			case 1027943729: // xExtrapolatorRight
			  return xExtrapolatorRight_Renamed;
			case 1118547936: // yInterpolator
			  return yInterpolator_Renamed;
			case 970644563: // yExtrapolatorLeft
			  return yExtrapolatorLeft_Renamed;
			case 30871376: // yExtrapolatorRight
			  return yExtrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends GridSurfaceInterpolator> builder()
		public override BeanBuilder<GridSurfaceInterpolator> builder()
		{
		  return new GridSurfaceInterpolator.Builder();
		}

		public override Type beanType()
		{
		  return typeof(GridSurfaceInterpolator);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code xInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> xInterpolator()
		{
		  return xInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code xExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> xExtrapolatorLeft()
		{
		  return xExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code xExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> xExtrapolatorRight()
		{
		  return xExtrapolatorRight_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> yInterpolator()
		{
		  return yInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> yExtrapolatorLeft()
		{
		  return yExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code yExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> yExtrapolatorRight()
		{
		  return yExtrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1411950943: // xInterpolator
			  return ((GridSurfaceInterpolator) bean).XInterpolator;
			case -382665134: // xExtrapolatorLeft
			  return ((GridSurfaceInterpolator) bean).XExtrapolatorLeft;
			case 1027943729: // xExtrapolatorRight
			  return ((GridSurfaceInterpolator) bean).XExtrapolatorRight;
			case 1118547936: // yInterpolator
			  return ((GridSurfaceInterpolator) bean).YInterpolator;
			case 970644563: // yExtrapolatorLeft
			  return ((GridSurfaceInterpolator) bean).YExtrapolatorLeft;
			case 30871376: // yExtrapolatorRight
			  return ((GridSurfaceInterpolator) bean).YExtrapolatorRight;
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
	  /// The bean-builder for {@code GridSurfaceInterpolator}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<GridSurfaceInterpolator>
	  {

		internal CurveInterpolator xInterpolator;
		internal CurveExtrapolator xExtrapolatorLeft;
		internal CurveExtrapolator xExtrapolatorRight;
		internal CurveInterpolator yInterpolator;
		internal CurveExtrapolator yExtrapolatorLeft;
		internal CurveExtrapolator yExtrapolatorRight;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1411950943: // xInterpolator
			  return xInterpolator;
			case -382665134: // xExtrapolatorLeft
			  return xExtrapolatorLeft;
			case 1027943729: // xExtrapolatorRight
			  return xExtrapolatorRight;
			case 1118547936: // yInterpolator
			  return yInterpolator;
			case 970644563: // yExtrapolatorLeft
			  return yExtrapolatorLeft;
			case 30871376: // yExtrapolatorRight
			  return yExtrapolatorRight;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 1411950943: // xInterpolator
			  this.xInterpolator = (CurveInterpolator) newValue;
			  break;
			case -382665134: // xExtrapolatorLeft
			  this.xExtrapolatorLeft = (CurveExtrapolator) newValue;
			  break;
			case 1027943729: // xExtrapolatorRight
			  this.xExtrapolatorRight = (CurveExtrapolator) newValue;
			  break;
			case 1118547936: // yInterpolator
			  this.yInterpolator = (CurveInterpolator) newValue;
			  break;
			case 970644563: // yExtrapolatorLeft
			  this.yExtrapolatorLeft = (CurveExtrapolator) newValue;
			  break;
			case 30871376: // yExtrapolatorRight
			  this.yExtrapolatorRight = (CurveExtrapolator) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override GridSurfaceInterpolator build()
		{
		  return new GridSurfaceInterpolator(xInterpolator, xExtrapolatorLeft, xExtrapolatorRight, yInterpolator, yExtrapolatorLeft, yExtrapolatorRight);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(224);
		  buf.Append("GridSurfaceInterpolator.Builder{");
		  buf.Append("xInterpolator").Append('=').Append(JodaBeanUtils.ToString(xInterpolator)).Append(',').Append(' ');
		  buf.Append("xExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(xExtrapolatorLeft)).Append(',').Append(' ');
		  buf.Append("xExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(xExtrapolatorRight)).Append(',').Append(' ');
		  buf.Append("yInterpolator").Append('=').Append(JodaBeanUtils.ToString(yInterpolator)).Append(',').Append(' ');
		  buf.Append("yExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(yExtrapolatorLeft)).Append(',').Append(' ');
		  buf.Append("yExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(yExtrapolatorRight));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}