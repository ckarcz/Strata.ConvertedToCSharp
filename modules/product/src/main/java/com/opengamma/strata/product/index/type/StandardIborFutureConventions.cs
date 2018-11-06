/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.index.type
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DateSequences.MONTHLY_IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DateSequences.QUARTERLY_IMM;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.EUR_EURIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;

	/// <summary>
	/// Market standard Fixed-Ibor swap conventions.
	/// <para>
	/// https://developers.opengamma.com/quantitative-research/Interest-Rate-Instruments-and-Market-Conventions.pdf
	/// </para>
	/// </summary>
	internal sealed class StandardIborFutureConventions
	{

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'GBP-LIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention GBP_LIBOR_3M_QUARTERLY_IMM = ImmutableIborFutureConvention.of(GBP_LIBOR_3M, QUARTERLY_IMM);

	  /// <summary>
	  /// The 'GBP-LIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'GBP-LIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention GBP_LIBOR_3M_MONTHLY_IMM = ImmutableIborFutureConvention.of(GBP_LIBOR_3M, MONTHLY_IMM);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'EUR-EURIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'EUR-EURIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention EUR_EURIBOR_3M_QUARTERLY_IMM = ImmutableIborFutureConvention.of(EUR_EURIBOR_3M, QUARTERLY_IMM);

	  /// <summary>
	  /// The 'EUR-EURIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'EUR-EURIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention EUR_EURIBOR_3M_MONTHLY_IMM = ImmutableIborFutureConvention.of(EUR_EURIBOR_3M, MONTHLY_IMM);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'USD-LIBOR-3M-Quarterly-IMM' convention.
	  /// <para>
	  /// The 'USD-LIBOR-3M' index based on quarterly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention USD_LIBOR_3M_QUARTERLY_IMM = ImmutableIborFutureConvention.of(USD_LIBOR_3M, QUARTERLY_IMM);

	  /// <summary>
	  /// The 'USD-LIBOR-3M-Monthly-IMM' convention.
	  /// <para>
	  /// The 'USD-LIBOR-3M' index based on monthly IMM dates.
	  /// </para>
	  /// </summary>
	  public static readonly IborFutureConvention USD_LIBOR_3M_MONTHLY_IMM = ImmutableIborFutureConvention.of(USD_LIBOR_3M, MONTHLY_IMM);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private StandardIborFutureConventions()
	  {
	  }

	}

}