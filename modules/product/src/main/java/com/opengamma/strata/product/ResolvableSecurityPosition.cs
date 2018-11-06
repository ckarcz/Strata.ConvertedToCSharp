/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using ReferenceDataNotFoundException = com.opengamma.strata.basics.ReferenceDataNotFoundException;
	using ResolvableCalculationTarget = com.opengamma.strata.basics.ResolvableCalculationTarget;

	/// <summary>
	/// A position that has a security identifier that can be resolved using reference data.
	/// <para>
	/// This represents those positions that hold a security identifier. It allows the position
	/// to be resolved, returning an alternate representation of the same position with complete
	/// security information.
	/// </para>
	/// </summary>
	public interface ResolvableSecurityPosition : Position, ResolvableCalculationTarget
	{

	  /// <summary>
	  /// Resolves the security identifier using the specified reference data.
	  /// <para>
	  /// This takes the security identifier of this position, looks it up in reference data,
	  /// and returns the equivalent position with full security information.
	  /// If the security has underlying securities, they will also have been resolved in the result.
	  /// </para>
	  /// <para>
	  /// The resulting position is bound to data from reference data.
	  /// If the data changes, the resulting position form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved position </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public abstract SecuritizedProductPosition<?> resolveTarget(com.opengamma.strata.basics.ReferenceData refData);
	  SecuritizedProductPosition<object> resolveTarget(ReferenceData refData);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified info.
	  /// </summary>
	  /// <param name="info">  the new info </param>
	  /// <returns> the instance with the specified info </returns>
	  ResolvableSecurityPosition withInfo(PositionInfo info);

	  /// <summary>
	  /// Returns an instance with the specified quantity.
	  /// </summary>
	  /// <param name="quantity">  the new quantity </param>
	  /// <returns> the instance with the specified quantity </returns>
	  ResolvableSecurityPosition withQuantity(double quantity);

	}

}