using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using MapStream = com.opengamma.strata.collect.MapStream;

	/// <summary>
	/// A scenario definition defines how to create multiple sets of market data for running calculations over
	/// a set of scenarios. The scenario data is created by applying perturbations to a set of base market data.
	/// A different set of perturbations is used for each scenario.
	/// <para>
	/// Each scenario definition contains market data filters and perturbations. Filters
	/// are used to choose items of market data that are shocked in the scenario, and the perturbations
	/// define those shocks.
	/// </para>
	/// <para>
	/// Perturbations are applied in the order they are defined in scenario. An item of market data
	/// can only be perturbed once, so if multiple mappings apply to it, only the first will be used.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class ScenarioDefinition implements org.joda.beans.ImmutableBean
	public sealed class ScenarioDefinition : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends PerturbationMapping<?>>") private final com.google.common.collect.ImmutableList<PerturbationMapping<?>> mappings;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		private readonly ImmutableList<PerturbationMapping<object>> mappings;

	  /// <summary>
	  /// The names of the scenarios. </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<String> scenarioNames;
	  private readonly ImmutableList<string> scenarioNames;

	  /// <summary>
	  /// An empty scenario definition. </summary>
	  private static readonly ScenarioDefinition EMPTY = ScenarioDefinition.builder().build();

	  /// <summary>
	  /// Returns an empty scenario definition.
	  /// </summary>
	  /// <returns> an empty scenario definition </returns>
	  public static ScenarioDefinition empty()
	  {
		return EMPTY;
	  }

	  /// <summary>
	  /// Returns a scenario definition containing the perturbations in {@code mappings}.
	  /// <para>
	  /// Each mapping must contain the same number of perturbations. The definition will contain the
	  /// same number of scenarios as the number of perturbations in each mapping.
	  /// </para>
	  /// <para>
	  /// The first scenario contains the first perturbation from each mapping, the second scenario contains
	  /// the second perturbation from each mapping, and so on.
	  /// </para>
	  /// <para>
	  /// Given three mappings, A, B and C, each containing two perturbations, 1 and 2, there will be two
	  /// scenarios generated:
	  /// <pre>
	  /// |            |  A   |  B   |  C   |
	  /// |------------|------|------|------|
	  /// | Scenario 1 | A[1] | B[1] | C[1] |
	  /// | Scenario 2 | A[2] | B[2] | C[2] |
	  /// </pre>
	  /// For example, consider the following perturbation mappings:
	  /// <ul>
	  ///   <li>Filter: USD Curves, Shocks: [-10bp, 0, +10bp]</li>
	  ///   <li>Filter: EUR/USD Rate, Shocks: [+5%, 0, -5%]</li>
	  /// </ul>
	  /// The scenario definition would contain the following three scenarios:
	  /// <pre>
	  /// |            | USD Curves | EUR/USD Rate |
	  /// |------------|------------|--------------|
	  /// | Scenario 1 |     -10bp  |     +5%      |
	  /// | Scenario 2 |       0    |      0       |
	  /// | Scenario 3 |     +10bp  |     -5%      |
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mapping">  the filters and perturbations that define the scenario. Each mapping must contain the same
	  ///   number of perturbations </param>
	  /// <returns> a scenario definition containing the perturbations in the mappings </returns>
	  public static ScenarioDefinition ofMappings<T1>(IList<T1> mapping) where T1 : PerturbationMapping<T1>
	  {
		ArgChecker.notEmpty(mapping, "mappings");

		int numScenarios = countScenarios(mapping, false);

		for (int i = 1; i < mapping.Count; i++)
		{
		  if (mapping[i].ScenarioCount != numScenarios)
		  {
			throw new System.ArgumentException("All mappings must have the same number of perturbations. First mapping" + " has " + numScenarios + " perturbations, mapping " + i + " has " + mapping[i].ScenarioCount);
		  }
		}
		return new ScenarioDefinition(mapping, generateNames(numScenarios));
	  }

	  /// <summary>
	  /// Returns a scenario definition containing the perturbations in {@code mappings}.
	  /// <para>
	  /// Each mapping must contain the same number of perturbations. The definition will contain the
	  /// same number of scenarios as the number of perturbations in each mapping.
	  /// </para>
	  /// <para>
	  /// The first scenario contains the first perturbation from each mapping, the second scenario contains
	  /// the second perturbation from each mapping, and so on.
	  /// </para>
	  /// <para>
	  /// Given three mappings, A, B and C, each containing two perturbations, 1 and 2, there will be two
	  /// scenarios generated:
	  /// <pre>
	  /// |            |  A   |  B   |  C   |
	  /// |------------|------|------|------|
	  /// | Scenario 1 | A[1] | B[1] | C[1] |
	  /// | Scenario 2 | A[2] | B[2] | C[2] |
	  /// </pre>
	  /// For example, consider the following perturbation mappings:
	  /// <ul>
	  ///   <li>Filter: USD Curves, Shocks: [-10bp, 0, +10bp]</li>
	  ///   <li>Filter: EUR/USD Rate, Shocks: [+5%, 0, -5%]</li>
	  /// </ul>
	  /// The scenario definition would contain the following three scenarios:
	  /// <pre>
	  /// |            | USD Curves | EUR/USD Rate |
	  /// |------------|------------|--------------|
	  /// | Scenario 1 |     -10bp  |     +5%      |
	  /// | Scenario 2 |       0    |      0       |
	  /// | Scenario 3 |     +10bp  |     -5%      |
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mappings">  the filters and perturbations that define the scenario. Each mapping must contain the same
	  ///   number of perturbations </param>
	  /// <returns> a scenario definition containing the perturbations in the mappings </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static ScenarioDefinition ofMappings(PerturbationMapping<?>... mappings)
	  public static ScenarioDefinition ofMappings(params PerturbationMapping<object>[] mappings)
	  {
		return ofMappings(Arrays.asList(mappings));
	  }

	  /// <summary>
	  /// Returns a scenario definition containing the perturbations in {@code mappings}.
	  /// <para>
	  /// Each mapping must contain the same number of perturbations. The definition will contain the
	  /// same number of scenarios as the number of perturbations in each mapping.
	  /// </para>
	  /// <para>
	  /// The first scenario contains the first perturbation from each mapping, the second scenario contains
	  /// the second perturbation from each mapping, and so on.
	  /// </para>
	  /// <para>
	  /// The set of scenario names must contain the same number of elements as the mappings.
	  /// </para>
	  /// <para>
	  /// Given three mappings, A, B and C, each containing two perturbations, 1 and 2, there will be two
	  /// scenarios generated:
	  /// <pre>
	  /// |            |  A   |  B   |  C   |
	  /// |------------|------|------|------|
	  /// | Scenario 1 | A[1] | B[1] | C[1] |
	  /// | Scenario 2 | A[2] | B[2] | C[2] |
	  /// </pre>
	  /// For example, consider the following perturbation mappings:
	  /// <ul>
	  ///   <li>Filter: USD Curves, Shocks: [-10bp, 0, +10bp]</li>
	  ///   <li>Filter: EUR/USD Rate, Shocks: [+5%, 0, -5%]</li>
	  /// </ul>
	  /// The scenario definition would contain the following three scenarios:
	  /// <pre>
	  /// |            | USD Curves | EUR/USD Rate |
	  /// |------------|------------|--------------|
	  /// | Scenario 1 |     -10bp  |     +5%      |
	  /// | Scenario 2 |       0    |      0       |
	  /// | Scenario 3 |     +10bp  |     -5%      |
	  /// </pre>
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="mappings">  the filters and perturbations that define the scenario. Each mapping must contain the same
	  ///   number of perturbations </param>
	  /// <param name="scenarioNames">  the names of the scenarios. This must be the same size as the list of perturbations
	  ///   in each mapping and the names must be unique </param>
	  /// <returns> a scenario definition containing the perturbations in the mappings </returns>
	  /// <exception cref="IllegalArgumentException"> if there are any duplicate scenario names </exception>
	  public static ScenarioDefinition ofMappings<T1>(IList<T1> mappings, IList<string> scenarioNames) where T1 : PerturbationMapping<T1>
	  {

		ArgChecker.notNull(scenarioNames, "scenarioNames");

		int numScenarios = scenarioNames.Count;

		for (int i = 0; i < mappings.Count; i++)
		{
		  if (mappings[i].ScenarioCount != numScenarios)
		  {
			throw new System.ArgumentException("Each mapping must contain the same number of scenarios as the definition. There are " + numScenarios + " scenarios in the definition, mapping " + i + " has " + mappings[i].ScenarioCount + " scenarios.");
		  }
		}
		return new ScenarioDefinition(mappings, scenarioNames);
	  }

	  /// <summary>
	  /// Counts the number of scenarios implied by the mappings and the {@code allCombinations} flag.
	  /// </summary>
	  /// <param name="mappings">  the mappings that make up the scenarios </param>
	  /// <param name="allCombinations">  whether the scenarios are generated by taking all combinations of perturbations
	  ///   formed by taking one from each mapping </param>
	  /// <returns> the number of scenarios </returns>
	  private static int countScenarios<T1>(IList<T1> mappings, bool allCombinations) where T1 : PerturbationMapping<T1>
	  {
		ArgChecker.notEmpty(mappings, "mappings");

		if (allCombinations)
		{
//JAVA TO C# CONVERTER TODO TASK: Method reference arbitrary object instance method syntax is not converted by Java to C# Converter:
		  return mappings.Select(PerturbationMapping::getScenarioCount).Aggregate(1, (s1, s2) => s1 * s2);
		}
		else
		{
		  return mappings[0].ScenarioCount;
		}
	  }

	  /// <summary>
	  /// Returns a list created by repeating the items in the input list multiple times, with each item repeated
	  /// in groups.
	  /// <para>
	  /// For example, given a list [1, 2, 3, 4], total count 12, group size 3 the result is
	  /// [1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4].
	  /// </para>
	  /// <para>
	  /// This is used when creating scenarios from every possible combination of a set of perturbations.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="inputs">  an input list whose elements are repeated in the output </param>
	  /// <param name="totalCount">  the number of elements in the output list </param>
	  /// <param name="groupSize">  the number of times each element should be repeated in each group </param>
	  /// @param <T>  the type of the elements </param>
	  /// <returns> a list created by repeating the elements of the input list </returns>
	  internal static IList<T> repeatItems<T>(IList<T> inputs, int totalCount, int groupSize)
	  {
		ImmutableList.Builder<T> builder = ImmutableList.builder();

		for (int i = 0; i < (totalCount / groupSize / inputs.Count); i++)
		{
		  foreach (T input in inputs)
		  {
			builder.addAll(Collections.nCopies(groupSize, input));
		  }
		}
		return builder.build();
	  }

	  /// <summary>
	  /// Generates simple names for the scenarios of the form 'Scenario 1' etc.
	  /// </summary>
	  private static ImmutableList<string> generateNames(int numScenarios)
	  {
		return IntStream.range(1, numScenarios + 1).mapToObj(i => "Scenario " + i).collect(toImmutableList());
	  }

	  // validates that there are no duplicate scenario names
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IDictionary<string, IList<string>> nameMap = scenarioNames.collect(groupingBy(name => name));
		IList<string> duplicateNames = MapStream.of(nameMap).filterValues(names => names.size() > 1).map((name, names) => name).collect(toImmutableList());

		if (duplicateNames.Count > 0)
		{
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		  string duplicates = duplicateNames.collect(joining(", "));
		  throw new System.ArgumentException("Scenario names must be unique but duplicates were found: " + duplicates);
		}
	  }

	  /// <summary>
	  /// Returns the number of scenarios.
	  /// </summary>
	  /// <returns> the number of scenarios </returns>
	  public int ScenarioCount
	  {
		  get
		  {
			return scenarioNames.size();
		  }
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code ScenarioDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static ScenarioDefinition.Meta meta()
	  {
		return ScenarioDefinition.Meta.INSTANCE;
	  }

	  static ScenarioDefinition()
	  {
		MetaBean.register(ScenarioDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static ScenarioDefinition.Builder builder()
	  {
		return new ScenarioDefinition.Builder();
	  }

	  private ScenarioDefinition<T1>(IList<T1> mappings, IList<string> scenarioNames) where T1 : PerturbationMapping<T1>
	  {
		JodaBeanUtils.notNull(mappings, "mappings");
		JodaBeanUtils.notNull(scenarioNames, "scenarioNames");
		this.mappings = ImmutableList.copyOf(mappings);
		this.scenarioNames = ImmutableList.copyOf(scenarioNames);
		validate();
	  }

	  public override ScenarioDefinition.Meta metaBean()
	  {
		return ScenarioDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data filters and perturbations that define the scenarios. </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableList<PerturbationMapping<?>> getMappings()
	  public ImmutableList<PerturbationMapping<object>> Mappings
	  {
		  get
		  {
			return mappings;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the names of the scenarios. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<string> ScenarioNames
	  {
		  get
		  {
			return scenarioNames;
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
		  ScenarioDefinition other = (ScenarioDefinition) obj;
		  return JodaBeanUtils.equal(mappings, other.mappings) && JodaBeanUtils.equal(scenarioNames, other.scenarioNames);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(mappings);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(scenarioNames);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("ScenarioDefinition{");
		buf.Append("mappings").Append('=').Append(mappings).Append(',').Append(' ');
		buf.Append("scenarioNames").Append('=').Append(JodaBeanUtils.ToString(scenarioNames));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code ScenarioDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  mappings_Renamed = DirectMetaProperty.ofImmutable(this, "mappings", typeof(ScenarioDefinition), (Type) typeof(ImmutableList));
			  scenarioNames_Renamed = DirectMetaProperty.ofImmutable(this, "scenarioNames", typeof(ScenarioDefinition), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "mappings", "scenarioNames");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code mappings} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<PerturbationMapping<?>>> mappings = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "mappings", ScenarioDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableList<PerturbationMapping<object>>> mappings_Renamed;
		/// <summary>
		/// The meta-property for the {@code scenarioNames} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<String>> scenarioNames = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "scenarioNames", ScenarioDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<string>> scenarioNames_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "mappings", "scenarioNames");
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
			case 194445669: // mappings
			  return mappings_Renamed;
			case -1193464424: // scenarioNames
			  return scenarioNames_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override ScenarioDefinition.Builder builder()
		{
		  return new ScenarioDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(ScenarioDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code mappings} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<PerturbationMapping<?>>> mappings()
		public MetaProperty<ImmutableList<PerturbationMapping<object>>> mappings()
		{
		  return mappings_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code scenarioNames} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<string>> scenarioNames()
		{
		  return scenarioNames_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 194445669: // mappings
			  return ((ScenarioDefinition) bean).Mappings;
			case -1193464424: // scenarioNames
			  return ((ScenarioDefinition) bean).ScenarioNames;
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
	  /// The bean-builder for {@code ScenarioDefinition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<ScenarioDefinition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends PerturbationMapping<?>> mappings = com.google.common.collect.ImmutableList.of();
		internal IList<PerturbationMapping<object>> mappings_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<string> scenarioNames_Renamed = ImmutableList.of();

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(ScenarioDefinition beanToCopy)
		{
		  this.mappings_Renamed = beanToCopy.Mappings;
		  this.scenarioNames_Renamed = beanToCopy.ScenarioNames;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 194445669: // mappings
			  return mappings_Renamed;
			case -1193464424: // scenarioNames
			  return scenarioNames_Renamed;
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
			case 194445669: // mappings
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.mappings = (java.util.List<? extends PerturbationMapping<?>>) newValue;
			  this.mappings_Renamed = (IList<PerturbationMapping<object>>) newValue;
			  break;
			case -1193464424: // scenarioNames
			  this.scenarioNames_Renamed = (IList<string>) newValue;
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

		public override ScenarioDefinition build()
		{
		  return new ScenarioDefinition(mappings_Renamed, scenarioNames_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the market data filters and perturbations that define the scenarios. </summary>
		/// <param name="mappings">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder mappings<T1>(IList<T1> mappings) where T1 : PerturbationMapping<T1>
		{
		  JodaBeanUtils.notNull(mappings, "mappings");
		  this.mappings_Renamed = mappings;
		  return this;
		}

		/// <summary>
		/// Sets the {@code mappings} property in the builder
		/// from an array of objects. </summary>
		/// <param name="mappings">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SafeVarargs public final Builder mappings(PerturbationMapping<?>... mappings)
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		public Builder mappings(params PerturbationMapping<object>[] mappings)
		{
		  return this.mappings(ImmutableList.copyOf(mappings));
		}

		/// <summary>
		/// Sets the names of the scenarios. </summary>
		/// <param name="scenarioNames">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder scenarioNames(IList<string> scenarioNames)
		{
		  JodaBeanUtils.notNull(scenarioNames, "scenarioNames");
		  this.scenarioNames_Renamed = scenarioNames;
		  return this;
		}

		/// <summary>
		/// Sets the {@code scenarioNames} property in the builder
		/// from an array of objects. </summary>
		/// <param name="scenarioNames">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder scenarioNames(params string[] scenarioNames)
		{
		  return this.scenarioNames(ImmutableList.copyOf(scenarioNames));
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("ScenarioDefinition.Builder{");
		  buf.Append("mappings").Append('=').Append(JodaBeanUtils.ToString(mappings_Renamed)).Append(',').Append(' ');
		  buf.Append("scenarioNames").Append('=').Append(JodaBeanUtils.ToString(scenarioNames_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}