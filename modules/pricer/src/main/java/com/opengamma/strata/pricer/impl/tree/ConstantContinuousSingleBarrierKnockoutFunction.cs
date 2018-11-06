using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
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

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using PutCall = com.opengamma.strata.product.common.PutCall;
	using BarrierType = com.opengamma.strata.product.option.BarrierType;

	/// <summary>
	/// Single barrier knock-out option function.
	/// <para>
	/// The barrier is continuous and the level is constant.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class ConstantContinuousSingleBarrierKnockoutFunction extends SingleBarrierKnockoutFunction implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class ConstantContinuousSingleBarrierKnockoutFunction : SingleBarrierKnockoutFunction, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double strike;
		private readonly double strike;
	  /// <summary>
	  /// The time to expiry.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double timeToExpiry;
	  private readonly double timeToExpiry;
	  /// <summary>
	  /// The sign.
	  /// <para>
	  /// The sign is +1 for call and -1 for put.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final double sign;
	  private readonly double sign;

	  /// <summary>
	  /// The number of time steps.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(overrideGet = true) private final int numberOfSteps;
	  private readonly int numberOfSteps;

	  /// <summary>
	  /// The barrier type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.product.option.BarrierType barrierType;
	  private readonly BarrierType barrierType;

	  /// <summary>
	  /// The constant barrier level.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double barrierLevel;
	  private readonly double barrierLevel;

	  /// <summary>
	  /// The rebate.
	  /// <para>
	  /// The rebate amounts for individual time layer.
	  /// The size must be equal to {@code numberOfSteps + 1} 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray rebate;
	  private readonly DoubleArray rebate;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance.
	  /// </summary>
	  /// <param name="strike">  the strike </param>
	  /// <param name="timeToExpiry">  the time to expiry </param>
	  /// <param name="putCall">  put or call </param>
	  /// <param name="numberOfSteps">  number of steps </param>
	  /// <param name="barrierType">  the barrier type </param>
	  /// <param name="barrierLevel">  the barrier level </param>
	  /// <param name="rebate">  the rebate </param>
	  /// <returns> the instance </returns>
	  public static ConstantContinuousSingleBarrierKnockoutFunction of(double strike, double timeToExpiry, PutCall putCall, int numberOfSteps, BarrierType barrierType, double barrierLevel, DoubleArray rebate)
	  {

		ArgChecker.isTrue(numberOfSteps > 0, "the number of steps should be positive");
		ArgChecker.isTrue(numberOfSteps + 1 == rebate.size(), "the size of rebate should be numberOfSteps + 1");
		double sign = putCall.Call ? 1d : -1d;
		return new ConstantContinuousSingleBarrierKnockoutFunction(strike, timeToExpiry, sign, numberOfSteps, barrierType, barrierLevel, rebate);
	  }

	  //-------------------------------------------------------------------------
	  public override double getBarrierLevel(int step)
	  {
		return barrierLevel;
	  }

	  public override double getRebate(int step)
	  {
		return rebate.get(step);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantContinuousSingleBarrierKnockoutFunction}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ConstantContinuousSingleBarrierKnockoutFunction.Meta meta()
	  {
		return ConstantContinuousSingleBarrierKnockoutFunction.Meta.INSTANCE;
	  }

	  static ConstantContinuousSingleBarrierKnockoutFunction()
	  {
		MetaBean.register(ConstantContinuousSingleBarrierKnockoutFunction.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private ConstantContinuousSingleBarrierKnockoutFunction(double strike, double timeToExpiry, double sign, int numberOfSteps, BarrierType barrierType, double barrierLevel, DoubleArray rebate)
	  {
		JodaBeanUtils.notNull(barrierType, "barrierType");
		JodaBeanUtils.notNull(rebate, "rebate");
		this.strike = strike;
		this.timeToExpiry = timeToExpiry;
		this.sign = sign;
		this.numberOfSteps = numberOfSteps;
		this.barrierType = barrierType;
		this.barrierLevel = barrierLevel;
		this.rebate = rebate;
	  }

	  public override ConstantContinuousSingleBarrierKnockoutFunction.Meta metaBean()
	  {
		return ConstantContinuousSingleBarrierKnockoutFunction.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike value. </summary>
	  /// <returns> the value of the property </returns>
	  public override double Strike
	  {
		  get
		  {
			return strike;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time to expiry. </summary>
	  /// <returns> the value of the property </returns>
	  public override double TimeToExpiry
	  {
		  get
		  {
			return timeToExpiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the sign.
	  /// <para>
	  /// The sign is +1 for call and -1 for put.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property </returns>
	  public override double Sign
	  {
		  get
		  {
			return sign;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of time steps. </summary>
	  /// <returns> the value of the property </returns>
	  public override int NumberOfSteps
	  {
		  get
		  {
			return numberOfSteps;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the barrier type. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public override BarrierType BarrierType
	  {
		  get
		  {
			return barrierType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the constant barrier level. </summary>
	  /// <returns> the value of the property </returns>
	  public double BarrierLevel
	  {
		  get
		  {
			return barrierLevel;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rebate.
	  /// <para>
	  /// The rebate amounts for individual time layer.
	  /// The size must be equal to {@code numberOfSteps + 1}
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Rebate
	  {
		  get
		  {
			return rebate;
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
		  ConstantContinuousSingleBarrierKnockoutFunction other = (ConstantContinuousSingleBarrierKnockoutFunction) obj;
		  return JodaBeanUtils.equal(strike, other.strike) && JodaBeanUtils.equal(timeToExpiry, other.timeToExpiry) && JodaBeanUtils.equal(sign, other.sign) && (numberOfSteps == other.numberOfSteps) && JodaBeanUtils.equal(barrierType, other.barrierType) && JodaBeanUtils.equal(barrierLevel, other.barrierLevel) && JodaBeanUtils.equal(rebate, other.rebate);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strike);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeToExpiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sign);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(numberOfSteps);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(barrierType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(barrierLevel);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rebate);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("ConstantContinuousSingleBarrierKnockoutFunction{");
		buf.Append("strike").Append('=').Append(strike).Append(',').Append(' ');
		buf.Append("timeToExpiry").Append('=').Append(timeToExpiry).Append(',').Append(' ');
		buf.Append("sign").Append('=').Append(sign).Append(',').Append(' ');
		buf.Append("numberOfSteps").Append('=').Append(numberOfSteps).Append(',').Append(' ');
		buf.Append("barrierType").Append('=').Append(barrierType).Append(',').Append(' ');
		buf.Append("barrierLevel").Append('=').Append(barrierLevel).Append(',').Append(' ');
		buf.Append("rebate").Append('=').Append(JodaBeanUtils.ToString(rebate));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ConstantContinuousSingleBarrierKnockoutFunction}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  strike_Renamed = DirectMetaProperty.ofImmutable(this, "strike", typeof(ConstantContinuousSingleBarrierKnockoutFunction), Double.TYPE);
			  timeToExpiry_Renamed = DirectMetaProperty.ofImmutable(this, "timeToExpiry", typeof(ConstantContinuousSingleBarrierKnockoutFunction), Double.TYPE);
			  sign_Renamed = DirectMetaProperty.ofImmutable(this, "sign", typeof(ConstantContinuousSingleBarrierKnockoutFunction), Double.TYPE);
			  numberOfSteps_Renamed = DirectMetaProperty.ofImmutable(this, "numberOfSteps", typeof(ConstantContinuousSingleBarrierKnockoutFunction), Integer.TYPE);
			  barrierType_Renamed = DirectMetaProperty.ofImmutable(this, "barrierType", typeof(ConstantContinuousSingleBarrierKnockoutFunction), typeof(BarrierType));
			  barrierLevel_Renamed = DirectMetaProperty.ofImmutable(this, "barrierLevel", typeof(ConstantContinuousSingleBarrierKnockoutFunction), Double.TYPE);
			  rebate_Renamed = DirectMetaProperty.ofImmutable(this, "rebate", typeof(ConstantContinuousSingleBarrierKnockoutFunction), typeof(DoubleArray));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "strike", "timeToExpiry", "sign", "numberOfSteps", "barrierType", "barrierLevel", "rebate");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code strike} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> strike_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeToExpiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> timeToExpiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code sign} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> sign_Renamed;
		/// <summary>
		/// The meta-property for the {@code numberOfSteps} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<int> numberOfSteps_Renamed;
		/// <summary>
		/// The meta-property for the {@code barrierType} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<BarrierType> barrierType_Renamed;
		/// <summary>
		/// The meta-property for the {@code barrierLevel} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> barrierLevel_Renamed;
		/// <summary>
		/// The meta-property for the {@code rebate} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> rebate_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "strike", "timeToExpiry", "sign", "numberOfSteps", "barrierType", "barrierLevel", "rebate");
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
			case -891985998: // strike
			  return strike_Renamed;
			case -1831499397: // timeToExpiry
			  return timeToExpiry_Renamed;
			case 3530173: // sign
			  return sign_Renamed;
			case -1323103225: // numberOfSteps
			  return numberOfSteps_Renamed;
			case 1029043089: // barrierType
			  return barrierType_Renamed;
			case 1827586573: // barrierLevel
			  return barrierLevel_Renamed;
			case -934952029: // rebate
			  return rebate_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends ConstantContinuousSingleBarrierKnockoutFunction> builder()
		public override BeanBuilder<ConstantContinuousSingleBarrierKnockoutFunction> builder()
		{
		  return new ConstantContinuousSingleBarrierKnockoutFunction.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ConstantContinuousSingleBarrierKnockoutFunction);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code strike} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> strike()
		{
		  return strike_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeToExpiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> timeToExpiry()
		{
		  return timeToExpiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sign} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> sign()
		{
		  return sign_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code numberOfSteps} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<int> numberOfSteps()
		{
		  return numberOfSteps_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code barrierType} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<BarrierType> barrierType()
		{
		  return barrierType_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code barrierLevel} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> barrierLevel()
		{
		  return barrierLevel_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rebate} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> rebate()
		{
		  return rebate_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -891985998: // strike
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).Strike;
			case -1831499397: // timeToExpiry
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).TimeToExpiry;
			case 3530173: // sign
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).Sign;
			case -1323103225: // numberOfSteps
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).NumberOfSteps;
			case 1029043089: // barrierType
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).BarrierType;
			case 1827586573: // barrierLevel
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).BarrierLevel;
			case -934952029: // rebate
			  return ((ConstantContinuousSingleBarrierKnockoutFunction) bean).Rebate;
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
	  /// The bean-builder for {@code ConstantContinuousSingleBarrierKnockoutFunction}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<ConstantContinuousSingleBarrierKnockoutFunction>
	  {

		internal double strike;
		internal double timeToExpiry;
		internal double sign;
		internal int numberOfSteps;
		internal BarrierType barrierType;
		internal double barrierLevel;
		internal DoubleArray rebate;

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
			case -891985998: // strike
			  return strike;
			case -1831499397: // timeToExpiry
			  return timeToExpiry;
			case 3530173: // sign
			  return sign;
			case -1323103225: // numberOfSteps
			  return numberOfSteps;
			case 1029043089: // barrierType
			  return barrierType;
			case 1827586573: // barrierLevel
			  return barrierLevel;
			case -934952029: // rebate
			  return rebate;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -891985998: // strike
			  this.strike = (double?) newValue.Value;
			  break;
			case -1831499397: // timeToExpiry
			  this.timeToExpiry = (double?) newValue.Value;
			  break;
			case 3530173: // sign
			  this.sign = (double?) newValue.Value;
			  break;
			case -1323103225: // numberOfSteps
			  this.numberOfSteps = (int?) newValue.Value;
			  break;
			case 1029043089: // barrierType
			  this.barrierType = (BarrierType) newValue;
			  break;
			case 1827586573: // barrierLevel
			  this.barrierLevel = (double?) newValue.Value;
			  break;
			case -934952029: // rebate
			  this.rebate = (DoubleArray) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override ConstantContinuousSingleBarrierKnockoutFunction build()
		{
		  return new ConstantContinuousSingleBarrierKnockoutFunction(strike, timeToExpiry, sign, numberOfSteps, barrierType, barrierLevel, rebate);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(256);
		  buf.Append("ConstantContinuousSingleBarrierKnockoutFunction.Builder{");
		  buf.Append("strike").Append('=').Append(JodaBeanUtils.ToString(strike)).Append(',').Append(' ');
		  buf.Append("timeToExpiry").Append('=').Append(JodaBeanUtils.ToString(timeToExpiry)).Append(',').Append(' ');
		  buf.Append("sign").Append('=').Append(JodaBeanUtils.ToString(sign)).Append(',').Append(' ');
		  buf.Append("numberOfSteps").Append('=').Append(JodaBeanUtils.ToString(numberOfSteps)).Append(',').Append(' ');
		  buf.Append("barrierType").Append('=').Append(JodaBeanUtils.ToString(barrierType)).Append(',').Append(' ');
		  buf.Append("barrierLevel").Append('=').Append(JodaBeanUtils.ToString(barrierLevel)).Append(',').Append(' ');
		  buf.Append("rebate").Append('=').Append(JodaBeanUtils.ToString(rebate));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}