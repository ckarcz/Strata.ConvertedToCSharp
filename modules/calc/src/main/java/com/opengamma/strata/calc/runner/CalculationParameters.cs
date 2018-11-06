using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableMap;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;

	/// <summary>
	/// The calculation parameters.
	/// <para>
	/// This provides a set of parameters that will be used in a calculation.
	/// Each parameter defines a <seealso cref="CalculationParameter#queryType() query type"/>,
	/// thus the functions are keyed in a {@code Map} by the query type {@code Class}.
	/// </para>
	/// <para>
	/// Parameters exist to provide control over the calculation.
	/// For example, <seealso cref="ReportingCurrency"/> is a parameter that controls currency conversion.
	/// If specified, on a <seealso cref="Column"/>, or in <seealso cref="CalculationRules"/>, then the output will
	/// be converted to the specified currency.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class CalculationParameters implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class CalculationParameters : ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  private static readonly CalculationParameters EMPTY = new CalculationParameters(ImmutableMap.of());

	  /// <summary>
	  /// The parameters, keyed by query type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<Class, CalculationParameter> parameters;
	  private readonly ImmutableMap<Type, CalculationParameter> parameters;
	  /// <summary>
	  /// The aliases.
	  /// </summary>
	  private readonly ImmutableMap<Type, Type> aliases;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an empty instance with no parameters.
	  /// </summary>
	  /// <returns> the empty instance </returns>
	  public static CalculationParameters empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Obtains an instance from the specified parameters.
	  /// <para>
	  /// The list will be converted to a {@code Map} using <seealso cref="CalculationParameter#queryType()"/>.
	  /// Each parameter must refer to a different query type.
	  /// </para>
	  /// <para>
	  /// If a parameter implements an interface that also extends <seealso cref="CalculationParameter"/>,
	  /// that type will also be able to be searched for (unless it has been directly registered).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters </param>
	  /// <returns> the calculation parameters </returns>
	  /// <exception cref="IllegalArgumentException"> if two parameters have same query type </exception>
	  public static CalculationParameters of(params CalculationParameter[] parameters)
	  {
		if (parameters.Length == 0)
		{
		  return EMPTY;
		}
		return new CalculationParameters(Stream.of(parameters).collect(toImmutableMap(p => p.queryType())));
	  }

	  /// <summary>
	  /// Obtains an instance from the specified parameters.
	  /// <para>
	  /// The list will be converted to a {@code Map} using <seealso cref="CalculationParameter#queryType()"/>.
	  /// Each parameter must refer to a different query type.
	  /// </para>
	  /// <para>
	  /// If a parameter implements an interface that also extends <seealso cref="CalculationParameter"/>,
	  /// that type will also be able to be searched for (unless it has been directly registered).
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameters">  the parameters </param>
	  /// <returns> the calculation parameters </returns>
	  /// <exception cref="IllegalArgumentException"> if two parameters have same query type </exception>
	  public static CalculationParameters of<T1>(IList<T1> parameters) where T1 : CalculationParameter
	  {
		if (parameters.Count == 0)
		{
		  return EMPTY;
		}
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return new CalculationParameters(parameters.collect(toImmutableMap(p => p.queryType())));
	  }

	  // create checking for empty
	  private CalculationParameters of(IDictionary<Type, CalculationParameter> map)
	  {
		if (map.Count == 0)
		{
		  return EMPTY;
		}
		return new CalculationParameters(map);
	  }

	  // the input map is treated as being ordered
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private CalculationParameters(java.util.Map<Class, CalculationParameter> parameters)
	  private CalculationParameters(IDictionary<Type, CalculationParameter> parameters)
	  {
		JodaBeanUtils.notNull(parameters, "parameters");
		this.parameters = ImmutableMap.copyOf(parameters);
		// find parameters that are super-interfaces of the specified objects
		IDictionary<Type, Type> aliases = new Dictionary<Type, Type>();
		foreach (Type type in parameters.Keys)
		{
		  Type[] interfaces = type.GetInterfaces();
		  foreach (Type iface in interfaces)
		  {
			if (iface != typeof(CalculationParameter) && iface.IsAssignableFrom(typeof(CalculationParameter)) && !parameters.ContainsKey(iface))
			{
			  // first registration wins with aliases
			  Type aliasType = iface.asSubclass(typeof(CalculationParameter));
			  if (!aliases.ContainsKey(aliasType)) aliases.Add(aliasType, type);
			}
		  }
		}
		this.aliases = ImmutableMap.copyOf(aliases);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this set of parameters with the specified set.
	  /// <para>
	  /// This set of parameters takes priority.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other parameters </param>
	  /// <returns> the combined calculation parameters </returns>
	  public CalculationParameters combinedWith(CalculationParameters other)
	  {
		if (other.parameters.Empty)
		{
		  return this;
		}
		if (parameters.Empty)
		{
		  return other;
		}
		IDictionary<Type, CalculationParameter> map = new Dictionary<Type, CalculationParameter>(other.Parameters);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
		map.putAll(parameters);
		return of(map);
	  }

	  /// <summary>
	  /// Returns a copy of this instance with the specified parameter added.
	  /// <para>
	  /// If this instance already has a parameter with the query type, it will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="parameter">  the parameter to add </param>
	  /// <returns> the new instance based on this with the parameter added </returns>
	  public CalculationParameters with(CalculationParameter parameter)
	  {
		IDictionary<Type, CalculationParameter> map = new Dictionary<Type, CalculationParameter>(parameters);
		map[parameter.queryType()] = parameter;
		return of(map);
	  }

	  /// <summary>
	  /// Filters the parameters, returning a set without the specified type.
	  /// </summary>
	  /// <param name="type">  the type to remove </param>
	  /// <returns> the filtered calculation parameters </returns>
	  public CalculationParameters without(Type type)
	  {
		if (!parameters.containsKey(type))
		{
		  return this;
		}
		IDictionary<Type, CalculationParameter> map = new Dictionary<Type, CalculationParameter>(parameters);
		map.Remove(type);
		return of(map);
	  }

	  /// <summary>
	  /// Filters the parameters, matching only those that are applicable for the target and measure.
	  /// <para>
	  /// The resulting parameters are filtered to the target and measure.
	  /// The implementation of each parameter may be changed by this process.
	  /// If two parameters are filtered to the same <seealso cref="CalculationParameter#queryType() query type"/>
	  /// then an exception will be thrown
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="target">  the calculation target, such as a trade </param>
	  /// <param name="measure">  the measure to be calculated </param>
	  /// <returns> the filtered calculation parameters </returns>
	  /// <exception cref="IllegalArgumentException"> if two parameters are filtered to the same query type </exception>
	  public CalculationParameters filter(CalculationTarget target, Measure measure)
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ImmutableList<CalculationParameter> filtered = parameters.values().Select(cp => cp.filter(target, measure)).Where(opt => opt.Present).Select(opt => opt.get()).collect(toImmutableList());
		return of(filtered);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Finds the parameter that matches the specified query type.
	  /// <para>
	  /// This method may throw an exception if the parameters have not been filtered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the parameter </param>
	  /// <param name="type">  the query type to find </param>
	  /// <returns> the parameter </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T extends CalculationParameter> java.util.Optional<T> findParameter(Class<T> type)
	  public Optional<T> findParameter<T>(Type<T> type) where T : CalculationParameter
	  {
		Type lookupType = aliases.getOrDefault(type, type);
		return Optional.ofNullable(type.cast(parameters.get(lookupType)));
	  }

	  /// <summary>
	  /// Returns the parameter that matches the specified query type throwing an exception if not available.
	  /// <para>
	  /// This method may throw an exception if the parameters have not been filtered.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the type of the parameter </param>
	  /// <param name="type">  the query type to return </param>
	  /// <returns> the parameter </returns>
	  /// <exception cref="IllegalArgumentException"> if no parameter if found for the type </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T extends CalculationParameter> T getParameter(Class<T> type)
	  public T getParameter<T>(Type<T> type) where T : CalculationParameter
	  {
		Type lookupType = aliases.getOrDefault(type, type);
		object calculationParameter = parameters.get(lookupType);
		if (calculationParameter == null)
		{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		  throw new System.ArgumentException("No parameter found for query type " + type.FullName);
		}
		return type.cast(calculationParameter);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationParameters}.
	  /// </summary>
	  private static readonly TypedMetaBean<CalculationParameters> META_BEAN = LightMetaBean.of(typeof(CalculationParameters), MethodHandles.lookup(), new string[] {"parameters"}, ImmutableMap.of());

	  /// <summary>
	  /// The meta-bean for {@code CalculationParameters}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CalculationParameters> meta()
	  {
		return META_BEAN;
	  }

	  static CalculationParameters()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override TypedMetaBean<CalculationParameters> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameters, keyed by query type. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableMap<Type, CalculationParameter> Parameters
	  {
		  get
		  {
			return parameters;
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
		  CalculationParameters other = (CalculationParameters) obj;
		  return JodaBeanUtils.equal(parameters, other.parameters);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameters);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("CalculationParameters{");
		buf.Append("parameters").Append('=').Append(JodaBeanUtils.ToString(parameters));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}