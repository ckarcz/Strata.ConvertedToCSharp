/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Helper for conventions.
	/// </summary>
	internal sealed class IborFixingDepositConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<IborFixingDepositConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(IborFixingDepositConvention));

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborFixingDepositConventions()
	  {
	  }

	}

}