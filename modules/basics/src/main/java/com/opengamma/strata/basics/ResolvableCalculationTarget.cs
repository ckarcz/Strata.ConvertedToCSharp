/*
 * Copyright (C) 2018 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	/// <summary>
	/// A calculation target that can be resolved using reference data.
	/// <para>
	/// This is implemented by targets that must be resolved against reference data at the
	/// start of the calculation process. For example, this allows the security on a trade
	/// or position to be resolved before deciding which function performs the processing.
	/// </para>
	/// </summary>
	public interface ResolvableCalculationTarget : CalculationTarget
	{

	  /// <summary>
	  /// Resolves this target, returning the resolved instance.
	  /// <para>
	  /// For example, if this represents a position where the security is referred to
	  /// only by identifier, this method can be used to convert the position to an
	  /// equivalent instance with the security looked up from reference data.
	  /// </para>
	  /// <para>
	  /// The resulting target is bound to data from reference data.
	  /// If the data changes, the resulting target form will not be updated.
	  /// Care must be taken when placing the resolved form in a cache or persistence layer.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="refData">  the reference data to use when resolving </param>
	  /// <returns> the resolved target </returns>
	  /// <exception cref="ReferenceDataNotFoundException"> if an identifier cannot be resolved in the reference data </exception>
	  /// <exception cref="RuntimeException"> if unable to resolve due to an invalid definition </exception>
	  CalculationTarget resolveTarget(ReferenceData refData);

	}

}