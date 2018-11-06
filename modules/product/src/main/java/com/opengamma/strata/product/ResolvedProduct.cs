/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product
{
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;

	/// <summary>
	/// A product that has been resolved for pricing.
	/// <para>
	/// This is the resolved form of <seealso cref="Product"/>. Applications will typically create
	/// a {@code ResolvedProduct} from a {@code Product} using <seealso cref="ReferenceData"/>.
	/// </para>
	/// <para>
	/// Resolved objects may be bound to data that changes over time, such as holiday calendars.
	/// If the data changes, such as the addition of a new holiday, the resolved form will not be updated.
	/// Care must be taken when placing the resolved form in a cache or persistence layer.
	/// </para>
	/// </summary>
	public interface ResolvedProduct
	{

	}

}