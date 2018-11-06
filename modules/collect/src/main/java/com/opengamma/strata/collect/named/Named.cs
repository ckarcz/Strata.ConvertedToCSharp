/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{

	/// <summary>
	/// A named instance.
	/// <para>
	/// This simple interface is used to define objects that can be identified by a unique name.
	/// The name contains enough information to be able to recreate the instance.
	/// </para>
	/// <para>
	/// Implementations should provide a static method {@code of(String)} that allows the
	/// instance to be created from the name.
	/// </para>
	/// </summary>
	public interface Named
	{

	  /// <summary>
	  /// Obtains an instance of the specified named type by name.
	  /// <para>
	  /// This method operates by reflection.
	  /// It requires a static method {@code of(String)} method to be present on the type specified.
	  /// If the method does not exist an exception is thrown.
	  /// 
	  /// </para>
	  /// </summary>
	  /// @param <T>  the named type </param>
	  /// <param name="type">  the named type with the {@code of(String)} method </param>
	  /// <param name="name">  the name to find </param>
	  /// <returns> the instance of the named type </returns>
	  /// <exception cref="IllegalArgumentException"> if the specified name could not be found </exception>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static <T> T of(Class<T> type, String name)
	//  {
	//	return Unchecked.wrap(() ->
	//	{
	//		Method method = type.getMethod("of", String.class);
	//		return type.cast(method.invoke(null, name));
	//	}
	//   );
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the unique name of the instance.
	  /// <para>
	  /// The name contains enough information to be able to recreate the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
	  string Name {get;}

	}

}