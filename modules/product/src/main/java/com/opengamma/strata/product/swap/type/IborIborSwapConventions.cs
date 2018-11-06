/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.swap.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard Ibor-Ibor swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	public sealed class IborIborSwapConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<IborIborSwapConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(IborIborSwapConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-LIBOR-3M-LIBOR-6M' swap convention.
	  /// <para>
	  /// USD standard LIBOR 3M vs LIBOR 6M swap.
	  /// The LIBOR 3M leg pays semi-annually with 'Flat' compounding method.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention USD_LIBOR_3M_LIBOR_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.USD_LIBOR_3M_LIBOR_6M.Name);

	  /// <summary>
	  /// The 'USD-LIBOR-1M-LIBOR-3M' swap convention.
	  /// <para>
	  /// USD standard LIBOR 1M vs LIBOR 3M swap.
	  /// The LIBOR 1M leg pays quarterly with 'Flat' compounding method.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention USD_LIBOR_1M_LIBOR_3M = IborIborSwapConvention.of(StandardIborIborSwapConventions.USD_LIBOR_1M_LIBOR_3M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY-LIBOR-1M-LIBOR-6M' swap convention.
	  /// <para>
	  /// JPY standard LIBOR 1M vs LIBOR 6M swap.
	  /// The LIBOR 1M leg pays monthly, the LIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_LIBOR_1M_LIBOR_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_LIBOR_1M_LIBOR_6M.Name);

	  /// <summary>
	  /// The 'JPY-LIBOR-3M-LIBOR-6M' swap convention.
	  /// <para>
	  /// JPY standard LIBOR 3M vs LIBOR 6M swap.
	  /// The LIBOR 3M leg pays quarterly, the LIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_LIBOR_3M_LIBOR_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_LIBOR_3M_LIBOR_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY-LIBOR-6M-TIBOR-JAPAN-6M' swap convention.
	  /// <para>
	  /// JPY standard LIBOR 6M vs TIBOR JAPAN 6M swap.
	  /// The two legs pay semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_LIBOR_6M_TIBOR_JAPAN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_LIBOR_6M_TIBOR_JAPAN_6M.Name);

	  /// <summary>
	  /// The 'JPY-LIBOR-6M-TIBOR-EUROYEN-6M' swap convention.
	  /// <para>
	  /// JPY standard LIBOR 6M vs TIBOR EUROYEN 6M swap.
	  /// The two legs pay semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_LIBOR_6M_TIBOR_EUROYEN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_LIBOR_6M_TIBOR_EUROYEN_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY-TIBORJ-1M-TIBOR-JAPAN-6M' swap convention.
	  /// <para>
	  /// JPY standard TIBOR JAPAN 1M vs TIBOR JAPAN 6M swap.
	  /// The TIBOR 1M leg pays monthly, the TIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_TIBOR_JAPAN_1M_TIBOR_JAPAN_6M.Name);

	  /// <summary>
	  /// The 'JPY-TIBOR-JAPAN-3M-TIBOR-JAPAN-6M' swap convention.
	  /// <para>
	  /// JPY standard TIBOR JAPAN 3M vs TIBOR JAPAN 6M swap.
	  /// The TIBOR 3M leg pays quarterly, the TIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_TIBOR_JAPAN_3M_TIBOR_JAPAN_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'JPY-TIBOR-EUROYEN-1M-TIBOR-EUROYEN-6M' swap convention.
	  /// <para>
	  /// JPY standard TIBOR EUROYEN 1M vs TIBOR EUROYEN 6M swap.
	  /// The TIBOR 1M leg pays monthly, the TIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_TIBOR_EUROYEN_1M_TIBOR_EUROYEN_6M.Name);

	  /// <summary>
	  /// The 'JPY-TIBOR-EUROYEN-3M-TIBOR-EUROYEN-6M' swap convention.
	  /// <para>
	  /// JPY standard TIBOR EUROYEN 3M vs TIBOR EUROYEN 6M swap.
	  /// The TIBOR 3M leg pays quarterly, the TIBOR 6M leg pays semi-annually.
	  /// </para>
	  /// </summary>
	  public static readonly IborIborSwapConvention JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M = IborIborSwapConvention.of(StandardIborIborSwapConventions.JPY_TIBOR_EUROYEN_3M_TIBOR_EUROYEN_6M.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private IborIborSwapConventions()
	  {
	  }

	}

}