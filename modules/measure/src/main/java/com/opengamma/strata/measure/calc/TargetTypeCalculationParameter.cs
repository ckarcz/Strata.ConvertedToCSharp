﻿using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.calc
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// A calculation parameter that selects the parameter based on the type of the target.
	/// <para>
	/// This can be used where a <seealso cref="CalculationParameter"/> is required, and will
	/// select an underlying parameter based on the target type.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class TargetTypeCalculationParameter implements com.opengamma.strata.calc.runner.CalculationParameter, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class TargetTypeCalculationParameter : CalculationParameter, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final Class queryType;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private readonly Type queryType_Renamed;
	  /// <summary>
	  /// The underlying parameters, keyed by target type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<Class, com.opengamma.strata.calc.runner.CalculationParameter> parameters;
	  private readonly ImmutableMap<Type, CalculationParameter> parameters;
	  /// <summary>
	  /// The default underlying parameter.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.calc.runner.CalculationParameter defaultParameter;
	  private readonly CalculationParameter defaultParameter;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified parameters.
	  /// <para>
	  /// The map provides a lookup from the <seealso cref="CalculationTarget"/> implementation type
	  /// to the appropriate parameter to use for that target. If a target is requested that
	  /// is not in the map, the default parameter is used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters, keyed by target type </param>
	  /// <param name="defaultParameter">  the default parameter </param>
	  /// <returns> the target aware parameter </returns>
	  public static TargetTypeCalculationParameter of(IDictionary<Type, CalculationParameter> parameters, CalculationParameter defaultParameter)
	  {

		ArgChecker.notEmpty(parameters, "values");
		ArgChecker.notNull(defaultParameter, "defaultValue");
		Type queryType = defaultParameter.queryType();
		foreach (CalculationParameter value in parameters.Values)
		{
		  if (value.queryType() != queryType)
		  {
			throw new System.ArgumentException(Messages.format("Map contained a parameter '{}' that did not match the expected query type '{}'", value, queryType.GetType().Name));
		  }
		}
		return new TargetTypeCalculationParameter(queryType, ImmutableMap.copyOf(parameters), defaultParameter);
	  }

	  //-------------------------------------------------------------------------
	  public override Type queryType()
	  {
		return queryType_Renamed;
	  }

	  public override Optional<CalculationParameter> filter(CalculationTarget target, Measure measure)
	  {
		CalculationParameter value = parameters.getOrDefault(target.GetType(), defaultParameter);
		return value.filter(target, measure);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code TargetTypeCalculationParameter}.
	  /// </summary>
	  private static readonly TypedMetaBean<TargetTypeCalculationParameter> META_BEAN = LightMetaBean.of(typeof(TargetTypeCalculationParameter), MethodHandles.lookup(), new string[] {"queryType", "parameters", "defaultParameter"}, null, ImmutableMap.of(), null);

	  /// <summary>
	  /// The meta-bean for {@code TargetTypeCalculationParameter}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<TargetTypeCalculationParameter> meta()
	  {
		return META_BEAN;
	  }

	  static TargetTypeCalculationParameter()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private TargetTypeCalculationParameter(Type queryType, IDictionary<Type, CalculationParameter> parameters, CalculationParameter defaultParameter)
	  {
		JodaBeanUtils.notNull(queryType, "queryType");
		JodaBeanUtils.notNull(parameters, "parameters");
		JodaBeanUtils.notNull(defaultParameter, "defaultParameter");
		this.queryType_Renamed = queryType;
		this.parameters = ImmutableMap.copyOf(parameters);
		this.defaultParameter = defaultParameter;
	  }

	  public override TypedMetaBean<TargetTypeCalculationParameter> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameter query type. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Type QueryType
	  {
		  get
		  {
			return queryType_Renamed;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying parameters, keyed by target type. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Type, CalculationParameter> Parameters
	  {
		  get
		  {
			return parameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the default underlying parameter. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CalculationParameter DefaultParameter
	  {
		  get
		  {
			return defaultParameter;
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
		  TargetTypeCalculationParameter other = (TargetTypeCalculationParameter) obj;
		  return JodaBeanUtils.equal(queryType_Renamed, other.queryType_Renamed) && JodaBeanUtils.equal(parameters, other.parameters) && JodaBeanUtils.equal(defaultParameter, other.defaultParameter);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(queryType_Renamed);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(defaultParameter);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("TargetTypeCalculationParameter{");
		buf.Append("queryType").Append('=').Append(queryType_Renamed).Append(',').Append(' ');
		buf.Append("parameters").Append('=').Append(parameters).Append(',').Append(' ');
		buf.Append("defaultParameter").Append('=').Append(JodaBeanUtils.ToString(defaultParameter));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}