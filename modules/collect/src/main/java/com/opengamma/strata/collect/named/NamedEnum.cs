/*
 * Copyright (C) 2017 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.collect.named
{
	/// <summary>
	/// A named enum instance.
	/// <para>
	/// This extends <seealso cref="Named"/> for implementations of <seealso cref="Enum"/>.
	/// The name is provided by the <seealso cref="Enum#toString()"/> method of the enum, typically
	/// using the <seealso cref="EnumNames"/> helper class.
	/// </para>
	/// <para>
	/// Implementations must provide a static method {@code of(String)} that allows the
	/// instance to be created from the name, see <seealso cref="Named#of(Class, String)"/>.
	/// </para>
	/// </summary>
	public interface NamedEnum : Named
	{

	  /// <summary>
	  /// Gets the unique name of the instance.
	  /// <para>
	  /// The name contains enough information to be able to recreate the instance.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default String getName()
	//  {
	//	return toString();
	//  }

	}

}