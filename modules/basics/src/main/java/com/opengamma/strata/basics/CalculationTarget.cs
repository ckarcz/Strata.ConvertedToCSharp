/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics
{
	/// <summary>
	/// The target of calculation within a system.
	/// <para>
	/// All financial instruments that can be the target of calculations implement this marker interface.
	/// For example, a trade or position.
	/// </para>
	/// <para>
	/// All implementations of this interface must be immutable and thread-safe.
	/// </para>
	/// </summary>
	public interface CalculationTarget
	{

	}

}