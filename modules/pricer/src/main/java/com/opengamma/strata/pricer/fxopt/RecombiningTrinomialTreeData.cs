using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
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
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Recombining trinomial tree data.
	/// <para>
	/// This includes state values and transition probabilities for all of the nodes,
	/// as well as discount factors and time (time from valuation date) for individual time steps.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class RecombiningTrinomialTreeData implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RecombiningTrinomialTreeData : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.collect.array.DoubleMatrix stateValue;
		private readonly DoubleMatrix stateValue;
	  /// <summary>
	  /// The transition probability.
	  /// <para>
	  /// The {@code i}-th element of the list represents the transition probability values for the nodes 
	  /// at the {@code i}-th time layer.
	  /// The matrix is {@code (2*i+1)} times {@code 3}, and its {@code j}-th row involves [0] down probability, 
	  /// [1] middle probability and [2] up probability for the {@code j}-th lowest node.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleMatrix> transitionProbability;
	  private readonly ImmutableList<DoubleMatrix> transitionProbability;
	  /// <summary>
	  /// The discount factor.
	  /// <para>
	  /// The {@code i}-th element is the discount factor between the {@code i}-th layer and the {@code (i+1)}-th layer.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.collect.array.DoubleArray discountFactor;
	  private readonly DoubleArray discountFactor;
	  /// <summary>
	  /// The time.
	  /// <para>
	  /// The {@code i}-th element is the year fraction between the {@code 0}-th time layer and the {@code i}-th layer.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.collect.array.DoubleArray time;
	  private readonly DoubleArray time;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="stateValue">  the state value </param>
	  /// <param name="transitionProbability">  the transition probability </param>
	  /// <param name="discountFactor">  the discount factor </param>
	  /// <param name="time">  the time </param>
	  /// <returns> the instance </returns>
	  public static RecombiningTrinomialTreeData of(DoubleMatrix stateValue, IList<DoubleMatrix> transitionProbability, DoubleArray discountFactor, DoubleArray time)
	  {

		int nSteps = discountFactor.size();
		ArgChecker.isTrue(stateValue.rowCount() == nSteps + 1, "the number of rows of stateValue must be (nSteps + 1)");
		ArgChecker.isTrue(transitionProbability.Count == nSteps, "the size of transitionProbability list must be nSteps");
		ArgChecker.isTrue(time.size() == nSteps + 1, "the size of time must be (nSteps + 1)");
		for (int i = 0; i < nSteps; ++i)
		{
		  ArgChecker.isTrue(stateValue.row(i).size() == 2 * i + 1, "the i-th row of stateValue must have the size (2 * i + 1)");
		  ArgChecker.isTrue(transitionProbability[i].rowCount() == 2 * i + 1, "the i-th element of transitionProbability list must have (2 * i + 1) rows");
		  ArgChecker.isTrue(transitionProbability[i].columnCount() == 3, "the i-th element of transitionProbability list must have 3 columns");
		}
		ArgChecker.isTrue(stateValue.row(nSteps).size() == 2 * nSteps + 1);
		return new RecombiningTrinomialTreeData(stateValue, transitionProbability, discountFactor, time);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the number of time steps.
	  /// </summary>
	  /// <returns> the number of time steps </returns>
	  public int NumberOfSteps
	  {
		  get
		  {
			return transitionProbability.size();
		  }
	  }

	  /// <summary>
	  /// Obtains the state values at the {@code i}-th time layer.
	  /// </summary>
	  /// <param name="i">  the layer </param>
	  /// <returns> the state values </returns>
	  public DoubleArray getStateValueAtLayer(int i)
	  {
		return stateValue.row(i);
	  }

	  /// <summary>
	  /// Obtains the transition probability values at the {@code i}-th time layer.
	  /// </summary>
	  /// <param name="i">  the layer </param>
	  /// <returns> the transition probability </returns>
	  public DoubleMatrix getProbabilityAtLayer(int i)
	  {
		return transitionProbability.get(i);
	  }

	  /// <summary>
	  /// Obtains discount factor between the {@code i}-th layer to the {@code (i+1)}-th layer.
	  /// </summary>
	  /// <param name="i">  the layer </param>
	  /// <returns> the discount factor </returns>
	  public double getDiscountFactorAtLayer(int i)
	  {
		return discountFactor.get(i);
	  }

	  /// <summary>
	  /// Obtains the spot.
	  /// </summary>
	  /// <returns> the spot </returns>
	  public double Spot
	  {
		  get
		  {
			return stateValue.get(0, 0);
		  }
	  }

	  /// <summary>
	  /// Obtains the time for the {@code i}-th layer.
	  /// <para>
	  /// The time is the year fraction between the {@code 0}-th layer and the {@code i}-th layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="i">  the layer </param>
	  /// <returns> the time </returns>
	  public double getTime(int i)
	  {
		return time.get(i);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RecombiningTrinomialTreeData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static RecombiningTrinomialTreeData.Meta meta()
	  {
		return RecombiningTrinomialTreeData.Meta.INSTANCE;
	  }

	  static RecombiningTrinomialTreeData()
	  {
		MetaBean.register(RecombiningTrinomialTreeData.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private RecombiningTrinomialTreeData(DoubleMatrix stateValue, IList<DoubleMatrix> transitionProbability, DoubleArray discountFactor, DoubleArray time)
	  {
		this.stateValue = stateValue;
		this.transitionProbability = (transitionProbability != null ? ImmutableList.copyOf(transitionProbability) : null);
		this.discountFactor = discountFactor;
		this.time = time;
	  }

	  public override RecombiningTrinomialTreeData.Meta metaBean()
	  {
		return RecombiningTrinomialTreeData.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the state value.
	  /// <para>
	  /// The {@code (i,j)} component of this matrix represents the underlying asset price at the {@code j}-th lowest node
	  /// at the {@code i}-th time layer.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public DoubleMatrix StateValue
	  {
		  get
		  {
			return stateValue;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the transition probability.
	  /// <para>
	  /// The {@code i}-th element of the list represents the transition probability values for the nodes
	  /// at the {@code i}-th time layer.
	  /// The matrix is {@code (2*i+1)} times {@code 3}, and its {@code j}-th row involves [0] down probability,
	  /// [1] middle probability and [2] up probability for the {@code j}-th lowest node.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public ImmutableList<DoubleMatrix> TransitionProbability
	  {
		  get
		  {
			return transitionProbability;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the discount factor.
	  /// <para>
	  /// The {@code i}-th element is the discount factor between the {@code i}-th layer and the {@code (i+1)}-th layer.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public DoubleArray DiscountFactor
	  {
		  get
		  {
			return discountFactor;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time.
	  /// <para>
	  /// The {@code i}-th element is the year fraction between the {@code 0}-th time layer and the {@code i}-th layer.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public DoubleArray Time
	  {
		  get
		  {
			return time;
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
		  RecombiningTrinomialTreeData other = (RecombiningTrinomialTreeData) obj;
		  return JodaBeanUtils.equal(stateValue, other.stateValue) && JodaBeanUtils.equal(transitionProbability, other.transitionProbability) && JodaBeanUtils.equal(discountFactor, other.discountFactor) && JodaBeanUtils.equal(time, other.time);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(stateValue);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(transitionProbability);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(discountFactor);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(time);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("RecombiningTrinomialTreeData{");
		buf.Append("stateValue").Append('=').Append(stateValue).Append(',').Append(' ');
		buf.Append("transitionProbability").Append('=').Append(transitionProbability).Append(',').Append(' ');
		buf.Append("discountFactor").Append('=').Append(discountFactor).Append(',').Append(' ');
		buf.Append("time").Append('=').Append(JodaBeanUtils.ToString(time));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code RecombiningTrinomialTreeData}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  stateValue_Renamed = DirectMetaProperty.ofImmutable(this, "stateValue", typeof(RecombiningTrinomialTreeData), typeof(DoubleMatrix));
			  transitionProbability_Renamed = DirectMetaProperty.ofImmutable(this, "transitionProbability", typeof(RecombiningTrinomialTreeData), (Type) typeof(ImmutableList));
			  discountFactor_Renamed = DirectMetaProperty.ofImmutable(this, "discountFactor", typeof(RecombiningTrinomialTreeData), typeof(DoubleArray));
			  time_Renamed = DirectMetaProperty.ofImmutable(this, "time", typeof(RecombiningTrinomialTreeData), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "stateValue", "transitionProbability", "discountFactor", "time");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code stateValue} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleMatrix> stateValue_Renamed;
		/// <summary>
		/// The meta-property for the {@code transitionProbability} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleMatrix>> transitionProbability = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "transitionProbability", RecombiningTrinomialTreeData.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleMatrix>> transitionProbability_Renamed;
		/// <summary>
		/// The meta-property for the {@code discountFactor} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> discountFactor_Renamed;
		/// <summary>
		/// The meta-property for the {@code time} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> time_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "stateValue", "transitionProbability", "discountFactor", "time");
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
			case -236449952: // stateValue
			  return stateValue_Renamed;
			case 734501792: // transitionProbability
			  return transitionProbability_Renamed;
			case -557144592: // discountFactor
			  return discountFactor_Renamed;
			case 3560141: // time
			  return time_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends RecombiningTrinomialTreeData> builder()
		public override BeanBuilder<RecombiningTrinomialTreeData> builder()
		{
		  return new RecombiningTrinomialTreeData.Builder();
		}

		public override Type beanType()
		{
		  return typeof(RecombiningTrinomialTreeData);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code stateValue} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleMatrix> stateValue()
		{
		  return stateValue_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code transitionProbability} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleMatrix>> transitionProbability()
		{
		  return transitionProbability_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code discountFactor} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> discountFactor()
		{
		  return discountFactor_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code time} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> time()
		{
		  return time_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -236449952: // stateValue
			  return ((RecombiningTrinomialTreeData) bean).StateValue;
			case 734501792: // transitionProbability
			  return ((RecombiningTrinomialTreeData) bean).TransitionProbability;
			case -557144592: // discountFactor
			  return ((RecombiningTrinomialTreeData) bean).DiscountFactor;
			case 3560141: // time
			  return ((RecombiningTrinomialTreeData) bean).Time;
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
	  /// The bean-builder for {@code RecombiningTrinomialTreeData}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<RecombiningTrinomialTreeData>
	  {

		internal DoubleMatrix stateValue;
		internal IList<DoubleMatrix> transitionProbability;
		internal DoubleArray discountFactor;
		internal DoubleArray time;

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
			case -236449952: // stateValue
			  return stateValue;
			case 734501792: // transitionProbability
			  return transitionProbability;
			case -557144592: // discountFactor
			  return discountFactor;
			case 3560141: // time
			  return time;
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
			case -236449952: // stateValue
			  this.stateValue = (DoubleMatrix) newValue;
			  break;
			case 734501792: // transitionProbability
			  this.transitionProbability = (IList<DoubleMatrix>) newValue;
			  break;
			case -557144592: // discountFactor
			  this.discountFactor = (DoubleArray) newValue;
			  break;
			case 3560141: // time
			  this.time = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override RecombiningTrinomialTreeData build()
		{
		  return new RecombiningTrinomialTreeData(stateValue, transitionProbability, discountFactor, time);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("RecombiningTrinomialTreeData.Builder{");
		  buf.Append("stateValue").Append('=').Append(JodaBeanUtils.ToString(stateValue)).Append(',').Append(' ');
		  buf.Append("transitionProbability").Append('=').Append(JodaBeanUtils.ToString(transitionProbability)).Append(',').Append(' ');
		  buf.Append("discountFactor").Append('=').Append(JodaBeanUtils.ToString(discountFactor)).Append(',').Append(' ');
		  buf.Append("time").Append('=').Append(JodaBeanUtils.ToString(time));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}