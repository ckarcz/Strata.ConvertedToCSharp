/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard Ibor future conventions.
	/// </summary>
	public sealed class IborFutureConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<IborFutureConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(IborFutureConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'GBP-LIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'GBP-LIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention GBP_LIBOR_3M_QUARTERLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.GBP_LIBOR_3M_QUARTERLY_IMM.Name);

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'GBP-LIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention GBP_LIBOR_3M_MONTHLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.GBP_LIBOR_3M_MONTHLY_IMM.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-EURIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'EUR-EURIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention EUR_EURIBOR_3M_QUARTERLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.EUR_EURIBOR_3M_QUARTERLY_IMM.Name);

	  /// <summary>
	  /// The 'EUR-EURIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'EUR-EURIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention EUR_EURIBOR_3M_MONTHLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.EUR_EURIBOR_3M_MONTHLY_IMM.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-LIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'USD-LIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention USD_LIBOR_3M_QUARTERLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.USD_LIBOR_3M_QUARTERLY_IMM.Name);

	  /// <summary>
	  /// The 'USD-LIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'USD-LIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention USD_LIBOR_3M_MONTHLY_IMM = IborFutureConvention.of(StandardIborFutureConventions.USD_LIBOR_3M_MONTHLY_IMM.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborFutureConventions()
	  {
	  }

	}

}