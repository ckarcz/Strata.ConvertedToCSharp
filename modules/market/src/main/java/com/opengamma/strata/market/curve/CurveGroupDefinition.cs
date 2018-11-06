/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve
{
	using MarketDataId = com.opengamma.strata.data.MarketDataId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// The definition of how to calibrate a group of curves.
	/// <para>
	/// A curve group contains information that allows a group of curves to be calibrated.
	/// </para>
	/// <para>
	/// In Strata v2, this type was converted to an interface.
	/// If migrating, change your code to <seealso cref="RatesCurveGroupDefinition"/>.
	/// </para>
	/// </summary>
	public interface CurveGroupDefinition
	{

	  /// <summary>
	  /// Gets the name of the curve group.
	  /// </summary>
	  /// <returns> the group name </returns>
	  CurveGroupName Name {get;}

	  /// <summary>
	  /// Creates an identifier that can be used to resolve this definition.
	  /// </summary>
	  /// <param name="source">  the source of data </param>
	  /// <returns> the curve, empty if not found </returns>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public abstract com.opengamma.strata.data.MarketDataId<? extends CurveGroup> createGroupId(com.opengamma.strata.data.ObservableSource source);
	  MarketDataId<CurveGroup> createGroupId(ObservableSource source);

	}

}