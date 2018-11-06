using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;


	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ArrayListMultimap = com.google.common.collect.ArrayListMultimap;
	using ImmutableList = com.google.common.collect.ImmutableList;
	using ListMultimap = com.google.common.collect.ListMultimap;
	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ResolvableCalculationTarget = com.opengamma.strata.basics.ResolvableCalculationTarget;
	using MarketDataRequirements = com.opengamma.strata.calc.marketdata.MarketDataRequirements;
	using MarketDataRequirementsBuilder = com.opengamma.strata.calc.marketdata.MarketDataRequirementsBuilder;
	using Messages = com.opengamma.strata.collect.Messages;

	/// <summary>
	/// The tasks that will be used to perform the calculations.
	/// <para>
	/// This captures the targets, columns and tasks that define the result grid.
	/// Each task can be executed to produce the result. Applications will typically
	/// use <seealso cref="CalculationRunner"/> or <seealso cref="CalculationTaskRunner"/> to execute the tasks.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class CalculationTasks implements org.joda.beans.ImmutableBean
	public sealed class CalculationTasks : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final java.util.List<com.opengamma.strata.basics.CalculationTarget> targets;
		private readonly IList<CalculationTarget> targets;
	  /// <summary>
	  /// The columns that will be calculated.
	  /// <para>
	  /// The result of the calculations will be a grid where each column is taken from this list.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final java.util.List<com.opengamma.strata.calc.Column> columns;
	  private readonly IList<Column> columns;
	  /// <summary>
	  /// The tasks that perform the individual calculations.
	  /// <para>
	  /// The results can be visualized as a grid, with a row for each target and a column for each measure.
	  /// Each task can calculate the result for one or more cells in the grid.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notEmpty") private final java.util.List<CalculationTask> tasks;
	  private readonly IList<CalculationTask> tasks;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a set of targets, columns and rules.
	  /// <para>
	  /// The targets will typically be trades.
	  /// The columns represent the measures to calculate.
	  /// </para>
	  /// <para>
	  /// Any target that implements <seealso cref="ResolvableCalculationTarget"/> will result in a failed task.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the columns that will be calculated </param>
	  /// <returns> the calculation tasks </returns>
	  public static CalculationTasks of<T1>(CalculationRules rules, IList<T1> targets, IList<Column> columns) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		return of(rules, targets, columns, ReferenceData.empty());
	  }

	  /// <summary>
	  /// Obtains an instance from a set of targets, columns and rules, resolving the targets.
	  /// <para>
	  /// The targets will typically be trades and positions.
	  /// The columns represent the measures to calculate.
	  /// </para>
	  /// <para>
	  /// The targets will be resolved if they implement <seealso cref="ResolvableCalculationTarget"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rules">  the rules defining how the calculation is performed </param>
	  /// <param name="targets">  the targets for which values of the measures will be calculated </param>
	  /// <param name="columns">  the columns that will be calculated </param>
	  /// <param name="refData">  the reference data to use to resolve the targets </param>
	  /// <returns> the calculation tasks </returns>
	  public static CalculationTasks of<T1>(CalculationRules rules, IList<T1> targets, IList<Column> columns, ReferenceData refData) where T1 : com.opengamma.strata.basics.CalculationTarget
	  {

		// create columns that are a combination of the column overrides and the defaults
		// this is done once as it is the same for all targets
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<Column> effectiveColumns = columns.Select(column => column.combineWithDefaults(rules.ReportingCurrency, rules.Parameters)).collect(toImmutableList());

		// loop around the targets, then the columns, to build the tasks
		ImmutableList.Builder<CalculationTask> taskBuilder = ImmutableList.builder();
		for (int rowIndex = 0; rowIndex < targets.Count; rowIndex++)
		{
		  CalculationTarget target = resolveTarget(targets[rowIndex], refData);

		  // find the applicable function, resolving the target if necessary
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: CalculationFunction<?> fn = target instanceof UnresolvableTarget ? UnresolvableTargetCalculationFunction.INSTANCE : rules.getFunctions().getFunction(target);
		  CalculationFunction<object> fn = target is UnresolvableTarget ? UnresolvableTargetCalculationFunction.INSTANCE : rules.Functions.getFunction(target);

		  // create the tasks
		  IList<CalculationTask> targetTasks = createTargetTasks(target, rowIndex, fn, effectiveColumns);
		  taskBuilder.addAll(targetTasks);
		}

		// calculation tasks holds the original user-specified columns, not the derived ones
		return new CalculationTasks(taskBuilder.build(), columns);
	  }

	  // resolves the target
	  private static CalculationTarget resolveTarget(CalculationTarget target, ReferenceData refData)
	  {
		if (target is ResolvableCalculationTarget)
		{
		  ResolvableCalculationTarget resolvable = (ResolvableCalculationTarget) target;
		  try
		  {
			return resolvable.resolveTarget(refData);
		  }
		  catch (Exception ex)
		  {
			return new UnresolvableTarget(resolvable, ex.Message);
		  }
		}
		return target;
	  }

	  // creates the tasks for a single target
	  private static IList<CalculationTask> createTargetTasks<T1>(CalculationTarget resolvedTarget, int rowIndex, CalculationFunction<T1> function, IList<Column> columns)
	  {

		// create the cells and group them
		ListMultimap<CalculationParameters, CalculationTaskCell> grouped = ArrayListMultimap.create();
		for (int colIndex = 0; colIndex < columns.Count; colIndex++)
		{
		  Column column = columns[colIndex];
		  Measure measure = column.Measure;

		  ReportingCurrency reportingCurrency = column.ReportingCurrency.orElse(ReportingCurrency.NATURAL);
		  CalculationTaskCell cell = CalculationTaskCell.of(rowIndex, colIndex, measure, reportingCurrency);
		  // group to find cells that can be shared, with same mappings and params (minus reporting currency)
		  CalculationParameters @params = column.Parameters.filter(resolvedTarget, measure);
		  grouped.put(@params, cell);
		}

		// build tasks
		ImmutableList.Builder<CalculationTask> taskBuilder = ImmutableList.builder();
		foreach (CalculationParameters @params in grouped.Keys)
		{
		  taskBuilder.add(CalculationTask.of(resolvedTarget, function, @params, grouped.get(@params)));
		}
		return taskBuilder.build();
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from a set of tasks and columns.
	  /// </summary>
	  /// <param name="tasks">  the tasks that perform the calculations </param>
	  /// <param name="columns">  the columns that define the calculations </param>
	  /// <returns> the calculation tasks </returns>
	  public static CalculationTasks of(IList<CalculationTask> tasks, IList<Column> columns)
	  {
		return new CalculationTasks(tasks, columns);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="tasks">  the tasks that perform the calculations </param>
	  /// <param name="columns">  the columns that define the calculations </param>
	  private CalculationTasks(IList<CalculationTask> tasks, IList<Column> columns)
	  {
		this.columns = ImmutableList.copyOf(columns);
		this.tasks = ImmutableList.copyOf(tasks);

		// validate the number of tasks and number of columns tally
		long cellCount = tasks.stream().flatMap(task => task.Cells.stream()).count();
		int columnCount = columns.Count;
		if (cellCount != 0)
		{
		  if (columnCount == 0)
		  {
			throw new System.ArgumentException("There must be at least one column");
		  }
		  if (cellCount % columnCount != 0)
		  {
			throw new System.ArgumentException(Messages.format("Number of cells ({}) must be exactly divisible by the number of columns ({})", cellCount, columnCount));
		  }
		}

		// pull out the targets from the tasks
		int targetCount = (int) cellCount / columnCount;
		CalculationTarget[] targets = new CalculationTarget[targetCount];
		foreach (CalculationTask task in tasks)
		{
		  int rowIdx = task.RowIndex;
		  if (targets[rowIdx] == null)
		  {
			targets[rowIdx] = task.Target;
		  }
		  else if (targets[rowIdx] != task.Target)
		  {
			throw new System.ArgumentException(Messages.format("Tasks define two different targets for row {}: {} and {}", rowIdx, targets[rowIdx], task.Target));
		  }
		}
		this.targets = ImmutableList.copyOf(targets); // missing targets will be caught here by null check
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the market data that is required to perform the calculations.
	  /// <para>
	  /// This can be used to pass into the market data system to obtain and calibrate data.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data </param>
	  /// <returns> the market data required for all calculations </returns>
	  /// <exception cref="RuntimeException"> if unable to obtain the requirements </exception>
	  public MarketDataRequirements requirements(ReferenceData refData)
	  {
		// use for loop not streams for shorter stack traces
		MarketDataRequirementsBuilder builder = MarketDataRequirements.builder();
		foreach (CalculationTask task in tasks)
		{
		  builder.addRequirements(task.requirements(refData));
		}
		return builder.build();
	  }

	  //-------------------------------------------------------------------------
	  public override string ToString()
	  {
		return Messages.format("CalculationTasks[grid={}x{}]", targets.Count, columns.Count);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code CalculationTasks}.
	  /// </summary>
	  private static readonly TypedMetaBean<CalculationTasks> META_BEAN = LightMetaBean.of(typeof(CalculationTasks), MethodHandles.lookup(), new string[] {"targets", "columns", "tasks"}, ImmutableList.of(), ImmutableList.of(), ImmutableList.of());

	  /// <summary>
	  /// The meta-bean for {@code CalculationTasks}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<CalculationTasks> meta()
	  {
		return META_BEAN;
	  }

	  static CalculationTasks()
	  {
		MetaBean.register(META_BEAN);
	  }

	  private CalculationTasks(IList<CalculationTarget> targets, IList<Column> columns, IList<CalculationTask> tasks)
	  {
		JodaBeanUtils.notEmpty(targets, "targets");
		JodaBeanUtils.notEmpty(columns, "columns");
		JodaBeanUtils.notEmpty(tasks, "tasks");
		this.targets = ImmutableList.copyOf(targets);
		this.columns = ImmutableList.copyOf(columns);
		this.tasks = ImmutableList.copyOf(tasks);
	  }

	  public override TypedMetaBean<CalculationTasks> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the targets that calculations will be performed on.
	  /// <para>
	  /// The result of the calculations will be a grid where each row is taken from this list.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public IList<CalculationTarget> Targets
	  {
		  get
		  {
			return targets;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the columns that will be calculated.
	  /// <para>
	  /// The result of the calculations will be a grid where each column is taken from this list.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public IList<Column> Columns
	  {
		  get
		  {
			return columns;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the tasks that perform the individual calculations.
	  /// <para>
	  /// The results can be visualized as a grid, with a row for each target and a column for each measure.
	  /// Each task can calculate the result for one or more cells in the grid.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not empty </returns>
	  public IList<CalculationTask> Tasks
	  {
		  get
		  {
			return tasks;
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
		  CalculationTasks other = (CalculationTasks) obj;
		  return JodaBeanUtils.equal(targets, other.targets) && JodaBeanUtils.equal(columns, other.columns) && JodaBeanUtils.equal(tasks, other.tasks);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(targets);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(columns);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(tasks);
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}