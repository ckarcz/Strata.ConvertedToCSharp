using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc
{
	using FromString = org.joda.convert.FromString;

	using TypedString = com.opengamma.strata.collect.TypedString;

	/// <summary>
	/// The name of a column in the grid of calculation results.
	/// </summary>
	[Serializable]
	public sealed class ColumnName : TypedString<ColumnName>
	{

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Column names may contain any character, but must not be empty.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of the column </param>
	  /// <returns> a column with the specified name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static ColumnName of(String name)
	  public static ColumnName of(string name)
	  {
		return new ColumnName(name);
	  }

	  /// <summary>
	  /// Obtains an instance from the specified measure.
	  /// <para>
	  /// The column name will be the same as the name of the measure.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="measure">  the measure to extract the name from </param>
	  /// <returns> a column with the same name as the measure </returns>
	  public static ColumnName of(Measure measure)
	  {
		return new ColumnName(measure.Name);
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="name">  the name of the column </param>
	  private ColumnName(string name) : base(name)
	  {
	  }

	}

}