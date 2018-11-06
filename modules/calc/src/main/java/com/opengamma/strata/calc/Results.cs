using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioArray = com.opengamma.strata.data.scenario.ScenarioArray;

	/// <summary>
	/// Calculation results of performing calculations for a set of targets and columns.
	/// <para>
	/// This defines a grid of results where the grid contains a row for each target and a column for each measure.
	/// Each result may be a single value or a multi-scenario value.
	/// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class Results implements org.joda.beans.ImmutableBean
	public sealed class Results : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<ColumnHeader> columns;
		private readonly ImmutableList<ColumnHeader> columns;
	  /// <summary>
	  /// The grid of results, stored as a flat list.
	  /// <para>
	  /// This list contains the calculated result for each cell in the grid.
	  /// The cells are grouped by target, then column.
	  /// Thus, the index of a given cell is {@code (targetRowIndex * columnCount) + columnIndex}.
	  /// </para>
	  /// <para>
	  /// For example, given a set of results with two targets, t1 and t2,
	  /// and three columns c1, c2, and c3, the results will be:
	  /// <pre>
	  ///   [t1c1, t1c2, t1c3, t2c1, t2c2, t2c3]
	  /// </pre>
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", builderType = "List<? extends Result<?>>") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.result.Result<?>> cells;
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
	  private readonly ImmutableList<Result<object>> cells;
	  /// <summary>
	  /// The number of rows.
	  /// </summary>
	  [NonSerialized]
	  private readonly int rowCount; // derived, not a property
	  /// <summary>
	  /// The number of columns.
	  /// </summary>
	  [NonSerialized]
	  private readonly int columnCount; // derived, not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance containing the results of the calculation for each cell.
	  /// <para>
	  /// The number of cells must be exactly divisible by the number of columns.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="columns">  the names of each column </param>
	  /// <param name="cells">  the calculated results, one for each cell </param>
	  /// <returns> a set of results for the calculations </returns>
	  public static Results of<T1>(IList<ColumnHeader> columns, IList<T1> cells) where T1 : com.opengamma.strata.collect.result.Result<T1>
	  {
		return new Results(columns, cells);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private Results(java.util.List<ColumnHeader> columns, java.util.List<? extends com.opengamma.strata.collect.result.Result<?>> cells)
	  private Results<T1>(IList<ColumnHeader> columns, IList<T1> cells) where T1 : com.opengamma.strata.collect.result.Result<T1>
	  {
		JodaBeanUtils.notNull(columns, "columns");
		JodaBeanUtils.notNull(cells, "cells");
		this.columns = ImmutableList.copyOf(columns);
		this.cells = ImmutableList.copyOf(cells);
		this.columnCount = columns.Count;
		this.rowCount = (columnCount == 0 ? 0 : cells.Count / columnCount);

		if (rowCount * columnCount != cells.Count)
		{
		  throw new System.ArgumentException(Messages.format("The number of cells ({}) must equal the number of rows ({}) multiplied by the number of columns ({})", this.cells.size(), this.rowCount, this.columnCount));
		}
	  }

	  // ensure standard constructor is invoked
	  private object readResolve()
	  {
		return new Results(columns, cells);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the number of rows in the results.
	  /// <para>
	  /// The number of rows equals the number of targets input to the calculation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the number of rows </returns>
	  public int RowCount
	  {
		  get
		  {
			return rowCount;
		  }
	  }

	  /// <summary>
	  /// Gets the number of columns in the results.
	  /// </summary>
	  /// <returns> the number of columns </returns>
	  public int ColumnCount
	  {
		  get
		  {
			return columnCount;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the results for a target and column index.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rowIndex">   the index of the row containing the results for a target </param>
	  /// <param name="columnIndex">  the index of the column </param>
	  /// <returns> the result for the specified row and column for a set of scenarios </returns>
	  /// <exception cref="IllegalArgumentException"> if the row or column index is invalid </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.result.Result<?> get(int rowIndex, int columnIndex)
	  public Result<object> get(int rowIndex, int columnIndex)
	  {
		if (rowIndex < 0 || rowIndex >= rowCount)
		{
		  throw new System.ArgumentException(invalidRowIndexMessage(rowIndex));
		}
		if (columnIndex < 0 || columnIndex >= columnCount)
		{
		  throw new System.ArgumentException(invalidColumnIndexMessage(columnIndex));
		}
		int index = (rowIndex * columnCount) + columnIndex;
		return cells.get(index);
	  }

	  private string invalidRowIndexMessage(int rowIndex)
	  {
		return Messages.format("Row index must be greater than or equal to zero and less than the row count ({}), but it was {}", rowCount, rowIndex);
	  }

	  private string invalidColumnIndexMessage(int columnIndex)
	  {
		return Messages.format("Column index must be greater than or equal to zero and less than the column count ({}), but it was {}", columnCount, columnIndex);
	  }

	  /// <summary>
	  /// Returns the results for a target and column index, casting the result to a known type.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the result type </param>
	  /// <param name="rowIndex">   the index of the row containing the results for a target </param>
	  /// <param name="columnIndex">  the index of the column </param>
	  /// <param name="type">  the result type </param>
	  /// <returns> the result for the specified row and column for a set of scenarios, cast to the specified type </returns>
	  /// <exception cref="IllegalArgumentException"> if the row or column index is invalid </exception>
	  /// <exception cref="ClassCastException"> if the result is not of the specified type </exception>
	  public Result<T> get<T>(int rowIndex, int columnIndex, Type<T> type)
	  {
		return cast(get(rowIndex, columnIndex), type);
	  }

	  /// <summary>
	  /// Returns the results for a target and column name.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="rowIndex">   the index of the row containing the results for a target </param>
	  /// <param name="columnName">  the name of the column </param>
	  /// <returns> the result for the specified row and column for a set of scenarios </returns>
	  /// <exception cref="IllegalArgumentException"> if the row index or column name is invalid </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.opengamma.strata.collect.result.Result<?> get(int rowIndex, ColumnName columnName)
	  public Result<object> get(int rowIndex, ColumnName columnName)
	  {
		for (int i = 0; i < columns.size(); i++)
		{
		  if (columns.get(i).Name.Equals(columnName))
		  {
			return get(rowIndex, i);
		  }
		}
		throw new System.ArgumentException(invalidColumnNameMessage(columnName));
	  }

	  private string invalidColumnNameMessage(ColumnName columnName)
	  {
		return Messages.format("Column name not found: {}", columnName);
	  }

	  /// <summary>
	  /// Returns the results for a target and column name, casting the result to a known type.
	  /// <para>
	  /// The result may be a single value or a multi-scenario value.
	  /// A multi-scenario value will implement <seealso cref="ScenarioArray"/> unless it has been aggregated.
	  /// </para>
	  /// <para>
	  /// If the calculation did not complete successfully, a failure result will be returned
	  /// explaining the problem. Callers must check whether the result is a success or failure
	  /// before examining the result value.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the result type </param>
	  /// <param name="rowIndex">   the index of the row containing the results for a target </param>
	  /// <param name="columnName">  the name of the column </param>
	  /// <param name="type">  the result type </param>
	  /// <returns> the result for the specified row and column for a set of scenarios, cast to the specified type </returns>
	  /// <exception cref="IllegalArgumentException"> if the row index or column name is invalid </exception>
	  /// <exception cref="ClassCastException"> if the result is not of the specified type </exception>
	  public Result<T> get<T>(int rowIndex, ColumnName columnName, Type<T> type)
	  {
		return cast(get(rowIndex, columnName), type);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private <T> com.opengamma.strata.collect.result.Result<T> cast(com.opengamma.strata.collect.result.Result<?> result, Class<T> type)
	  private Result<T> cast<T, T1>(Result<T1> result, Type<T> type)
	  {
		// cannot use result.map() as we want the exception to be thrown
		if (result.Failure || type.IsInstanceOfType(result.Value))
		{
		  return (Result<T>) result;
		}
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
		throw new System.InvalidCastException(Messages.format("Result queried with type '{}' but was '{}'", type.FullName, result.Value.GetType().FullName));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code Results}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static Results.Meta meta()
	  {
		return Results.Meta.INSTANCE;
	  }

	  static Results()
	  {
		MetaBean.register(Results.Meta.INSTANCE);
	  }

	  public override Results.Meta metaBean()
	  {
		return Results.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the column headers.
	  /// <para>
	  /// Each column in the results is defined by a header consisting of the name and measure.
	  /// The size of this list defines the number of columns, which is needed to interpret the list of cells.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<ColumnHeader> Columns
	  {
		  get
		  {
			return columns;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the grid of results, stored as a flat list.
	  /// <para>
	  /// This list contains the calculated result for each cell in the grid.
	  /// The cells are grouped by target, then column.
	  /// Thus, the index of a given cell is {@code (targetRowIndex * columnCount) + columnIndex}.
	  /// </para>
	  /// <para>
	  /// For example, given a set of results with two targets, t1 and t2,
	  /// and three columns c1, c2, and c3, the results will be:
	  /// <pre>
	  /// [t1c1, t1c2, t1c3, t2c1, t2c2, t2c3]
	  /// </pre>
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public com.google.common.collect.ImmutableList<com.opengamma.strata.collect.result.Result<?>> getCells()
	  public ImmutableList<Result<object>> Cells
	  {
		  get
		  {
			return cells;
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
		  Results other = (Results) obj;
		  return JodaBeanUtils.equal(columns, other.columns) && JodaBeanUtils.equal(cells, other.cells);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(columns);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(cells);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(96);
		buf.Append("Results{");
		buf.Append("columns").Append('=').Append(columns).Append(',').Append(' ');
		buf.Append("cells").Append('=').Append(JodaBeanUtils.ToString(cells));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code Results}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  columns_Renamed = DirectMetaProperty.ofImmutable(this, "columns", typeof(Results), (Type) typeof(ImmutableList));
			  cells_Renamed = DirectMetaProperty.ofImmutable(this, "cells", typeof(Results), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "columns", "cells");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code columns} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<ColumnHeader>> columns = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "columns", Results.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ColumnHeader>> columns_Renamed;
		/// <summary>
		/// The meta-property for the {@code cells} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.result.Result<?>>> cells = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "cells", Results.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		internal MetaProperty<ImmutableList<Result<object>>> cells_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "columns", "cells");
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
			case 949721053: // columns
			  return columns_Renamed;
			case 94544721: // cells
			  return cells_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends Results> builder()
		public override BeanBuilder<Results> builder()
		{
		  return new Results.Builder();
		}

		public override Type beanType()
		{
		  return typeof(Results);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code columns} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ColumnHeader>> columns()
		{
		  return columns_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code cells} property. </summary>
		/// <returns> the meta-property, not null </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.result.Result<?>>> cells()
		public MetaProperty<ImmutableList<Result<object>>> cells()
		{
		  return cells_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 949721053: // columns
			  return ((Results) bean).Columns;
			case 94544721: // cells
			  return ((Results) bean).Cells;
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
	  /// The bean-builder for {@code Results}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<Results>
	  {

		internal IList<ColumnHeader> columns = ImmutableList.of();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<? extends com.opengamma.strata.collect.result.Result<?>> cells = com.google.common.collect.ImmutableList.of();
		internal IList<Result<object>> cells = ImmutableList.of();

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
			case 949721053: // columns
			  return columns;
			case 94544721: // cells
			  return cells;
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
			case 949721053: // columns
			  this.columns = (IList<ColumnHeader>) newValue;
			  break;
			case 94544721: // cells
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: this.cells = (java.util.List<? extends com.opengamma.strata.collect.result.Result<?>>) newValue;
			  this.cells = (IList<Result<object>>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Results build()
		{
		  return new Results(columns, cells);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(96);
		  buf.Append("Results.Builder{");
		  buf.Append("columns").Append('=').Append(JodaBeanUtils.ToString(columns)).Append(',').Append(' ');
		  buf.Append("cells").Append('=').Append(JodaBeanUtils.ToString(cells));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}