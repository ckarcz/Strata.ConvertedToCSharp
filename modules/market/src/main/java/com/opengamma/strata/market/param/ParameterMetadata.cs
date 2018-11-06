/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{

	using ImmutableBean = org.joda.beans.ImmutableBean;

	/// <summary>
	/// Information about a single parameter.
	/// <para>
	/// Implementations of this interface are used to store metadata about a parameter.
	/// Parameters are an abstraction over curves, surfaces and other types of data.
	/// </para>
	/// </summary>
	public interface ParameterMetadata : ImmutableBean
	{

	  /// <summary>
	  /// Gets an empty metadata instance.
	  /// <para>
	  /// This is used when the actual metadata is not known.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the empty instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static ParameterMetadata empty()
	//  {
	//	return EmptyParameterMetadata.empty();
	//  }

	  /// <summary>
	  /// Gets a list of empty metadata instances.
	  /// <para>
	  /// This is used when there the actual metadata is not known.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="size">  the size of the resulting list </param>
	  /// <returns> the empty instance </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static java.util.List<ParameterMetadata> listOfEmpty(int size)
	//  {
	//	return Collections.nCopies(size, EmptyParameterMetadata.empty());
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the label that describes the parameter.
	  /// <para>
	  /// It is intended that the label is relatively short, however there is no formal restriction.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the label </returns>
	  string Label {get;}

	  /// <summary>
	  /// Returns an object used to identify the parameter.
	  /// <para>
	  /// A good choice of identifier is one that makes sense to the user and can easily be created as part of a
	  /// scenario definition. For example, many nodes types are naturally identified by a tenor.
	  /// </para>
	  /// <para>
	  /// The identifier must satisfy the following criteria:
	  /// <ul>
	  ///   <li>It must be non-null</li>
	  ///   <li>It should be unique within a single data set</li>
	  ///   <li>It should have a sensible implementation of {@code hashCode()} and {@code equals()}.</li>
	  /// </ul>
	  /// Otherwise the choice of identifier is free and the system makes no assumptions about it.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> an object used to uniquely identify the parameter within the data </returns>
	  object Identifier {get;}

	}

}