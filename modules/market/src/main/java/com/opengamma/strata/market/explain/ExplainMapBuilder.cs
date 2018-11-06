using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.explain
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// A builder for the map of explanatory values.
	/// <para>
	/// This is a mutable builder for <seealso cref="ExplainMap"/> that must be used from a single thread.
	/// </para>
	/// </summary>
	public sealed class ExplainMapBuilder
	{

	  /// <summary>
	  /// The parent builder.
	  /// </summary>
	  private readonly ExplainMapBuilder parent;
	  /// <summary>
	  /// The map of explanatory values.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<ExplainKey<?>, Object> map = new java.util.LinkedHashMap<>();
	  private readonly IDictionary<ExplainKey<object>, object> map = new LinkedHashMap<ExplainKey<object>, object>();

	  /// <summary>
	  /// Creates a new instance.
	  /// </summary>
	  internal ExplainMapBuilder()
	  {
		this.parent = null;
	  }

	  /// <summary>
	  /// Creates a new instance.
	  /// </summary>
	  /// <param name="parent">  the parent builder </param>
	  internal ExplainMapBuilder(ExplainMapBuilder parent)
	  {
		this.parent = parent;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Opens a list entry to be populated.
	  /// <para>
	  /// This returns the builder for the new list entry.
	  /// If the list does not exist, it is created and the first entry added.
	  /// If the list has already been created, the entry is appended.
	  /// </para>
	  /// <para>
	  /// Once opened, the child builder resulting from this method must be used.
	  /// The method <seealso cref="#closeListEntry(ExplainKey)"/> must be used to close the
	  /// child and receive an instance of the parent back again.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value </param>
	  /// <param name="key">  the list key to open </param>
	  /// <returns> the child builder </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <R extends java.util.List<?>> ExplainMapBuilder openListEntry(ExplainKey<R> key)
	  public ExplainMapBuilder openListEntry<R>(ExplainKey<R> key)
	  {
		// list entry is a ExplainMapBuilder, making use of erasure in generics
		// builder is converted to ExplainMap when entry is closed
		ExplainMapBuilder child = new ExplainMapBuilder(this);
		object value = map[key];
		List<object> list;
		if (value is List<object>)
		{
		  list = (List<object>) value;
		}
		else
		{
		  list = new List<>();
		  map[key] = list;
		}
		list.Add(child);
		return child;
	  }

	  /// <summary>
	  /// Closes the currently open list.
	  /// <para>
	  /// This returns the parent builder.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value </param>
	  /// <param name="key">  the list key to close </param>
	  /// <returns> the parent builder </returns>
	  public ExplainMapBuilder closeListEntry<R>(ExplainKey<R> key)
	  {
		object value = parent.map[key];
		if (value is ArrayList == false)
		{
		  throw new System.InvalidOperationException("ExplainMapBuilder.closeList() called but no list found to close");
		}
		// close entry by converting it from ExplainMapBuilder to ExplainMap
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.ArrayList<Object> list = (java.util.ArrayList<Object>) value;
		List<object> list = (List<object>) value;
		ExplainMapBuilder closedEntry = (ExplainMapBuilder) list[list.Count - 1];
		list[list.Count - 1] = closedEntry.build();
		return parent;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Adds a list entry using a consumer callback function.
	  /// <para>
	  /// This is an alternative to using <seealso cref="#openListEntry(ExplainKey)"/> and
	  /// <seealso cref="#closeListEntry(ExplainKey)"/> directly.
	  /// The consumer function receives the child builder and must add data to it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value </param>
	  /// <param name="key">  the list key to open </param>
	  /// <param name="consumer">  the consumer that receives the list entry builder and adds to it </param>
	  /// <returns> this builder </returns>
	  public ExplainMapBuilder addListEntry<R>(ExplainKey<R> key, System.Action<ExplainMapBuilder> consumer)
	  {
		ExplainMapBuilder child = openListEntry(key);
		consumer(child);
		return child.closeListEntry(key);
	  }

	  /// <summary>
	  /// Adds a list entry using a consumer callback function, including the list index.
	  /// <para>
	  /// This is an alternative to using <seealso cref="#openListEntry(ExplainKey)"/> and
	  /// <seealso cref="#closeListEntry(ExplainKey)"/> directly.
	  /// The consumer function receives the child builder and must add data to it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value </param>
	  /// <param name="key">  the list key to open </param>
	  /// <param name="consumer">  the consumer that receives the list entry builder and adds to it </param>
	  /// <returns> this builder </returns>
	  public ExplainMapBuilder addListEntryWithIndex<R>(ExplainKey<R> key, System.Action<ExplainMapBuilder> consumer)
	  {
		ExplainMapBuilder child = openListEntry(key);
		// find index
		object value = map[key];
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.ArrayList<Object> list = (java.util.ArrayList<Object>) value;
		List<object> list = (List<object>) value;
		child.put(ExplainKey.ENTRY_INDEX, list.Count - 1);
		consumer(child);
		return child.closeListEntry(key);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Puts a single value into the map.
	  /// <para>
	  /// If the key already exists, the value will be replaced.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <R>  the type of the value </param>
	  /// <param name="key">  the key to add </param>
	  /// <param name="value">  the value to add </param>
	  /// <returns> this builder </returns>
	  public ExplainMapBuilder put<R>(ExplainKey<R> key, R value)
	  {
		ArgChecker.notNull(key, "key");
		ArgChecker.notNull(value, "value");
		map[key] = value;
		return this;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Builds the map.
	  /// </summary>
	  /// <returns> the resulting map </returns>
	  public ExplainMap build()
	  {
		return ExplainMap.of(map);
	  }

	}

}