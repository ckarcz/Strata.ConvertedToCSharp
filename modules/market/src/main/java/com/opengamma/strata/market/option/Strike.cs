/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.option
{
	/// <summary>
	/// The strike of an option, describing both type and value.
	/// <para>
	/// The strike of option instruments is represented in different ways.
	/// For example, the strike types include delta, moneyness, log-moneyness, and strike itself.
	/// </para>
	/// </summary>
	public interface Strike
	{

	  /// <summary>
	  /// Gets the type of the strike.
	  /// </summary>
	  /// <returns> the strike type </returns>
	  StrikeType Type {get;}

	  /// <summary>
	  /// Gets the value of the strike.
	  /// </summary>
	  /// <returns> the value </returns>
	  double Value {get;}

	  /// <summary>
	  /// Gets a label describing the strike.
	  /// </summary>
	  /// <returns> the label </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default String getLabel()
	//  {
	//	return getType() + "=" + getValue();
	//  }

	  /// <summary>
	  /// Creates an new instance of the same strike type with value.
	  /// </summary>
	  /// <param name="value">  the new value </param>
	  /// <returns> the new strike instance </returns>
	  Strike withValue(double value);

	}

}