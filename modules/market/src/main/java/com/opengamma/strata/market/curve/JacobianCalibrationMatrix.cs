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
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Jacobian matrix information produced during curve calibration.
	/// <para>
	/// The inverse Jacobian matrix produced using curve calibration is stored here.
	/// The information is used to calculate market quote sensitivity.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class JacobianCalibrationMatrix implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class JacobianCalibrationMatrix : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<CurveParameterSize> order;
		private readonly ImmutableList<CurveParameterSize> order;
	  /// <summary>
	  /// The inverse Jacobian matrix produced during curve calibration.
	  /// This is the derivative of the curve parameters with respect to the market quotes.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleMatrix jacobianMatrix;
	  private readonly DoubleMatrix jacobianMatrix;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the curve order and Jacobian matrix.
	  /// <para>
	  /// This creates an instance from the inverse Jacobian matrix produced during curve calibration.
	  /// This is the derivative of the curve parameters with respect to the market quotes.
	  /// The curve order defines the order of the curves during calibration, which
	  /// can be used as a key to interpret the matrix.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="order">  the order of the curves during calibration </param>
	  /// <param name="jacobianMatrix">  the inverse Jacobian matrix produced during curve calibration </param>
	  /// <returns> the info </returns>
	  public static JacobianCalibrationMatrix of(IList<CurveParameterSize> order, DoubleMatrix jacobianMatrix)
	  {
		return new JacobianCalibrationMatrix(order, jacobianMatrix);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the total number of curves.
	  /// </summary>
	  /// <returns> the number of curves </returns>
	  public int CurveCount
	  {
		  get
		  {
			return order.size();
		  }
	  }

	  /// <summary>
	  /// Gets the total number of parameters.
	  /// </summary>
	  /// <returns> the number of curves </returns>
	  public int TotalParameterCount
	  {
		  get
		  {
	//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
			return order.Select(CurveParameterSize::getParameterCount).Sum();
		  }
	  }

	  /// <summary>
	  /// Checks if this info contains the specified curve.
	  /// </summary>
	  /// <param name="name">  the curve to find </param>
	  /// <returns> true if the curve is matched </returns>
	  public bool containsCurve(CurveName name)
	  {
		return order.Any(o => o.Name.Equals(name));
	  }

	  /// <summary>
	  /// Splits the array according to the curve order.
	  /// <para>
	  /// The input array must be of the same size as the total number of parameters.
	  /// The result consists of a map of arrays, where each array is the appropriate
	  /// section of the input array as defined by the curve order.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="array">  the array to split </param>
	  /// <returns> a map splitting the array by curve name </returns>
	  public IDictionary<CurveName, DoubleArray> splitValues(DoubleArray array)
	  {
		LinkedHashMap<CurveName, DoubleArray> result = new LinkedHashMap<CurveName, DoubleArray>();
		int start = 0;
		foreach (CurveParameterSize paramSizes in order)
		{
		  int size = paramSizes.ParameterCount;
		  result.put(paramSizes.Name, array.subArray(start, start + size));
		  start += size;
		}
		return result;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code JacobianCalibrationMatrix}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static JacobianCalibrationMatrix.Meta meta()
	  {
		return JacobianCalibrationMatrix.Meta.INSTANCE;
	  }

	  static JacobianCalibrationMatrix()
	  {
		MetaBean.register(JacobianCalibrationMatrix.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private JacobianCalibrationMatrix(IList<CurveParameterSize> order, DoubleMatrix jacobianMatrix)
	  {
		JodaBeanUtils.notNull(order, "order");
		JodaBeanUtils.notNull(jacobianMatrix, "jacobianMatrix");
		this.order = ImmutableList.copyOf(order);
		this.jacobianMatrix = jacobianMatrix;
	  }

	  public override JacobianCalibrationMatrix.Meta metaBean()
	  {
		return JacobianCalibrationMatrix.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the curve order.
	  /// This defines the order of the curves during calibration, which can be used
	  /// as a key to interpret the Jacobian matrix. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<CurveParameterSize> Order
	  {
		  get
		  {
			return order;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the inverse Jacobian matrix produced during curve calibration.
	  /// This is the derivative of the curve parameters with respect to the market quotes. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleMatrix JacobianMatrix
	  {
		  get
		  {
			return jacobianMatrix;
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
		  JacobianCalibrationMatrix other = (JacobianCalibrationMatrix) obj;
		  return JodaBeanUtils.equal(order, other.order) && JodaBeanUtils.equal(jacobianMatrix, other.jacobianMatrix);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(order);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(jacobianMatrix);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("JacobianCalibrationMatrix{");
		buf.Append("order").Append('=').Append(order).Append(',').Append(' ');
		buf.Append("jacobianMatrix").Append('=').Append(JodaBeanUtils.ToString(jacobianMatrix));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code JacobianCalibrationMatrix}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  order_Renamed = DirectMetaProperty.ofImmutable(this, "order", typeof(JacobianCalibrationMatrix), (Type) typeof(ImmutableList));
			  jacobianMatrix_Renamed = DirectMetaProperty.ofImmutable(this, "jacobianMatrix", typeof(JacobianCalibrationMatrix), typeof(DoubleMatrix));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "order", "jacobianMatrix");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code order} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<CurveParameterSize>> order = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "order", JacobianCalibrationMatrix.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<CurveParameterSize>> order_Renamed;
		/// <summary>
		/// The meta-property for the {@code jacobianMatrix} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleMatrix> jacobianMatrix_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "order", "jacobianMatrix");
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
			case 106006350: // order
			  return order_Renamed;
			case 1656240056: // jacobianMatrix
			  return jacobianMatrix_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends JacobianCalibrationMatrix> builder()
		public override BeanBuilder<JacobianCalibrationMatrix> builder()
		{
		  return new JacobianCalibrationMatrix.Builder();
		}

		public override Type beanType()
		{
		  return typeof(JacobianCalibrationMatrix);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code order} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<CurveParameterSize>> order()
		{
		  return order_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code jacobianMatrix} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleMatrix> jacobianMatrix()
		{
		  return jacobianMatrix_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 106006350: // order
			  return ((JacobianCalibrationMatrix) bean).Order;
			case 1656240056: // jacobianMatrix
			  return ((JacobianCalibrationMatrix) bean).JacobianMatrix;
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
	  /// The bean-builder for {@code JacobianCalibrationMatrix}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<JacobianCalibrationMatrix>
	  {

		internal IList<CurveParameterSize> order = ImmutableList.of();
		internal DoubleMatrix jacobianMatrix;

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
			case 106006350: // order
			  return order;
			case 1656240056: // jacobianMatrix
			  return jacobianMatrix;
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
			case 106006350: // order
			  this.order = (IList<CurveParameterSize>) newValue;
			  break;
			case 1656240056: // jacobianMatrix
			  this.jacobianMatrix = (DoubleMatrix) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override JacobianCalibrationMatrix build()
		{
		  return new JacobianCalibrationMatrix(order, jacobianMatrix);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("JacobianCalibrationMatrix.Builder{");
		  buf.Append("order").Append('=').Append(JodaBeanUtils.ToString(order)).Append(',').Append(' ');
		  buf.Append("jacobianMatrix").Append('=').Append(JodaBeanUtils.ToString(jacobianMatrix));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}