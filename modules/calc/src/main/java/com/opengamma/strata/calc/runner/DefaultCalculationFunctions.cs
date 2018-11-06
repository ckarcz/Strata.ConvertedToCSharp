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


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;

	/// <summary>
	/// The default calculation functions implementation.
	/// <para>
	/// This provides the complete set of functions that will be used in a calculation.
	/// Each <seealso cref="CalculationFunction"/> handles a specific type of <seealso cref="CalculationTarget"/>,
	/// thus the functions are keyed in a {@code Map} by the target type {@code Class}.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") final class DefaultCalculationFunctions implements CalculationFunctions, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	internal sealed class DefaultCalculationFunctions : CalculationFunctions, ImmutableBean
	{

	  /// <summary>
	  /// An empty instance.
	  /// </summary>
	  internal static readonly DefaultCalculationFunctions EMPTY = new DefaultCalculationFunctions(ImmutableMap.of());

	  /// <summary>
	  /// The functions, keyed by target type.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableMap<Class, CalculationFunction<?>> functions;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableMap<Type, CalculationFunction<object>> functions;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified functions.
	  /// <para>
	  /// The map will be validated to ensure the {@code Class} is consistent with
	  /// <seealso cref="CalculationFunction#targetType()"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="functions">  the functions </param>
	  /// <returns> the calculation functions </returns>
	  internal static DefaultCalculationFunctions of<T1>(IDictionary<T1> functions) where T1 : CalculationFunction<T1>
	  {
		return new DefaultCalculationFunctions(ImmutableMap.copyOf(functions));
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<Class, CalculationFunction<?>> entry : functions.entrySet())
		foreach (KeyValuePair<Type, CalculationFunction<object>> entry in functions.entrySet())
		{
		  ArgChecker.isTrue(entry.Key.IsAssignableFrom(entry.Value.targetType()), "Invalid map, key and function mismatch: {} and {}", entry.Key, entry.Value.targetType());
		}
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <T extends com.opengamma.strata.basics.CalculationTarget> CalculationFunction<? super T> getFunction(T target)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  public override CalculationFunction<object> getFunction<T>(T target) where T : com.opengamma.strata.basics.CalculationTarget
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") CalculationFunction<? super T> function = (CalculationFunction<? super T>) functions.get(target.getClass());
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		  CalculationFunction<object> function = (CalculationFunction<object>) functions.get(target.GetType());
		return function != null ? function : MissingConfigCalculationFunction.INSTANCE;
	  }

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public <T extends com.opengamma.strata.basics.CalculationTarget> java.util.Optional<CalculationFunction<? super T>> findFunction(T target)
	  public Optional<CalculationFunction> findFunction<T>(T target) where T : com.opengamma.strata.basics.CalculationTarget
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") CalculationFunction<? super T> function = (CalculationFunction<? super T>) functions.get(target.getClass());
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		  CalculationFunction<object> function = (CalculationFunction<object>) functions.get(target.GetType());
		return Optional.ofNullable(function);
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public CalculationFunctions composedWith(DerivedCalculationFunction<?, ?>... derivedFunctions)
	  public override CalculationFunctions composedWith(params DerivedCalculationFunction<object, ?>[] derivedFunctions)
	  {
		// Override the default implementation for efficiency.
		// The default implementation uses DerivedCalculationFunctions which creates a function instance for every target.
		// This class can do better and can create a single function instance for each target type.
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<Class, java.util.List<DerivedCalculationFunction<?, ?>>> functionsByTargetType = java.util.Arrays.stream(derivedFunctions).collect(groupingBy(fn -> fn.targetType()));
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<Type, IList<DerivedCalculationFunction<object, ?>>> functionsByTargetType = java.util.derivedFunctions.collect(groupingBy(fn => fn.targetType()));

		// The calculation functions wrapped up with the derived functions which use them
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<CalculationFunction<?>> wrappedFunctions = com.opengamma.strata.collect.MapStream.of(functionsByTargetType).map((targetType, fns) -> wrap(targetType, fns)).collect(toList());
		IList<CalculationFunction<object>> wrappedFunctions = MapStream.of(functionsByTargetType).map((targetType, fns) => wrap(targetType, fns)).collect(toList());

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<Class, CalculationFunction<?>> allFunctions = new java.util.HashMap<>(functions);
		IDictionary<Type, CalculationFunction<object>> allFunctions = new Dictionary<Type, CalculationFunction<object>>(functions);
		wrappedFunctions.ForEach(fn => allFunctions.put(fn.targetType(), fn));
		return CalculationFunctions.of(allFunctions);
	  }

//JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
	  SuppressWarnings("unchecked") private <T extends com.opengamma.strata.basics.CalculationTarget, R> CalculationFunction<?> wrap(Class targetType, java.util.List<DerivedCalculationFunction<?, ?>> derivedFunctions)
	  {

//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super T> function = (CalculationFunction<? super T>) functions.get(targetType);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> function = (CalculationFunction<object>) functions.get(targetType);

		if (function == null)
		{
		  function = MissingConfigCalculationFunction.INSTANCE;
		}
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super T> wrappedFn = function;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> wrappedFn = function;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (DerivedCalculationFunction<?, ?> derivedFn : derivedFunctions)
		foreach (DerivedCalculationFunction<object, ?> derivedFn in derivedFunctions)
		{
		  // These casts are necessary because the type information is lost when the functions are stored in the map.
		  // They are safe because T is the target type which is is the map key and R isn't actually used
		  CalculationFunction<T> wrappedFnCast = (CalculationFunction<T>) wrappedFn;
		  DerivedCalculationFunction<T, R> derivedFnCast = (DerivedCalculationFunction<T, R>) derivedFn;
		  wrappedFn = new DerivedCalculationFunctionWrapper<>(derivedFnCast, wrappedFnCast);
		}
		return wrappedFn;
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code DefaultCalculationFunctions}.
	  /// </summary>
	  private static final TypedMetaBean<DefaultCalculationFunctions> META_BEAN = LightMetaBean.of(typeof(DefaultCalculationFunctions), MethodHandles.lookup(), new string[] {"functions"}, ImmutableMap.of());

	  /// <summary>
	  /// The meta-bean for {@code DefaultCalculationFunctions}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<DefaultCalculationFunctions> meta()
	  {
		return META_BEAN;
	  }

	  static DefaultCalculationFunctions()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private static final long serialVersionUID = 1L;

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private DefaultCalculationFunctions(java.util.Map<Class, CalculationFunction<?>> functions)
	  private DefaultCalculationFunctions(IDictionary<Type, CalculationFunction<object>> functions)
	  {
		JodaBeanUtils.notNull(functions, "functions");
		this.functions = ImmutableMap.copyOf(functions);
		validate();
	  }

	  public TypedMetaBean<DefaultCalculationFunctions> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the functions, keyed by target type. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableMap<Class, CalculationFunction<?>> getFunctions()
	  public ImmutableMap<Type, CalculationFunction<object>> Functions
	  {
		return functions;
	  }

	  //-----------------------------------------------------------------------
	  public bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  DefaultCalculationFunctions other = (DefaultCalculationFunctions) obj;
		  return JodaBeanUtils.equal(functions, other.functions);
		}
		return false;
	  }

	  public int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(functions);
		return hash;
	  }

	  public string ToString()
	  {
		StringBuilder buf = new StringBuilder(64);
		buf.Append("DefaultCalculationFunctions{");
		buf.Append("functions").Append('=').Append(JodaBeanUtils.ToString(functions));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}