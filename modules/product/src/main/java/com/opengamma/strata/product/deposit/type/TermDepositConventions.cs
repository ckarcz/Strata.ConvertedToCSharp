/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.product.deposit.type
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Market standard term deposit conventions.
	/// <para>
	/// The conventions form two groups, those typically used for deposits of one month
	/// and over and those for deposits of less than one month, which have "Short" in the name.
	/// </para>
	/// <para>
	/// The conventions also differ by spot date. Most currencies have a T+2 spot date, where
	/// the start date is two days after the trade date.
	/// There are special cases for trades that have a T+0 or T+1 convention.
	/// The name of each convention includes "T0", "T1" or "T2" two indicate the spot date.
	/// </para>
	/// </summary>
	public sealed class TermDepositConventions
	{

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<TermDepositConvention> ENUM_LOOKUP = ExtendedEnum.of(typeof(TermDepositConvention));

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// The 'GBP-Deposit-T0' term deposit convention with T+0 settlement date.
	  /// This has the modified following business day convention and is typically used
	  /// for deposits of one month and over.
	  /// </summary>
	  public static readonly TermDepositConvention GBP_DEPOSIT_T0 = TermDepositConvention.of(StandardTermDepositConventions.GBP_DEPOSIT_T0.Name);
	  /// <summary>
	  /// The 'GBP-ShortDeposit-T0' term deposit convention with T+0 settlement date.
	  /// This has the following business day convention and is typically used for O/N and deposits up to one month.
	  /// </summary>
	  public static readonly TermDepositConvention GBP_SHORT_DEPOSIT_T0 = TermDepositConvention.of(StandardTermDepositConventions.GBP_SHORT_DEPOSIT_T0.Name);
	  /// <summary>
	  /// The 'GBP-ShortDeposit-T1' term deposit convention with T+1 settlement date.
	  /// This has the following business day convention and is typically used for T/N.
	  /// </summary>
	  public static readonly TermDepositConvention GBP_SHORT_DEPOSIT_T1 = TermDepositConvention.of(StandardTermDepositConventions.GBP_SHORT_DEPOSIT_T1.Name);

	  /// <summary>
	  /// The 'EUR-Deposit-T2' term deposit convention with T+2 settlement date.
	  /// This has the modified following business day convention and is typically used
	  /// for deposits of one month and over.
	  /// </summary>
	  public static readonly TermDepositConvention EUR_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.EUR_DEPOSIT_T2.Name);
	  /// <summary>
	  /// The 'EUR-ShortDeposit-T0' term deposit convention with T+0 settlement date.
	  /// This has the following business day convention and is typically used for O/N.
	  /// </summary>
	  public static readonly TermDepositConvention EUR_SHORT_DEPOSIT_T0 = TermDepositConvention.of(StandardTermDepositConventions.EUR_SHORT_DEPOSIT_T0.Name);
	  /// <summary>
	  /// The 'EUR-ShortDeposit-T1' term deposit convention with T+1 settlement date
	  /// This has the following business day convention and is typically used for T/N.
	  /// </summary>
	  public static readonly TermDepositConvention EUR_SHORT_DEPOSIT_T1 = TermDepositConvention.of(StandardTermDepositConventions.EUR_SHORT_DEPOSIT_T1.Name);
	  /// <summary>
	  /// The 'EUR-ShortDeposit-T2' term deposit convention with T+2 settlement date
	  /// This has the following business day convention and is typically used for deposits up to one month.
	  /// </summary>
	  public static readonly TermDepositConvention EUR_SHORT_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.EUR_SHORT_DEPOSIT_T2.Name);

	  /// <summary>
	  /// The 'USD-Deposit-T2' term deposit convention with T+2 settlement date.
	  /// This has the modified following business day convention and is typically used
	  /// for deposits of one month and over.
	  /// </summary>
	  public static readonly TermDepositConvention USD_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.USD_DEPOSIT_T2.Name);
	  /// <summary>
	  /// The 'USD-ShortDeposit-T0' term deposit convention with T+0 settlement date.
	  /// This has the following business day convention and is typically used for O/N.
	  /// </summary>
	  public static readonly TermDepositConvention USD_SHORT_DEPOSIT_T0 = TermDepositConvention.of(StandardTermDepositConventions.USD_SHORT_DEPOSIT_T0.Name);
	  /// <summary>
	  /// The 'USD-ShortDeposit-T1' term deposit convention with T+1 settlement date
	  /// This has the following business day convention and is typically used for T/N.
	  /// </summary>
	  public static readonly TermDepositConvention USD_SHORT_DEPOSIT_T1 = TermDepositConvention.of(StandardTermDepositConventions.USD_SHORT_DEPOSIT_T1.Name);
	  /// <summary>
	  /// The 'USD-ShortDeposit-T2' term deposit convention with T+2 settlement date
	  /// This has the following business day convention and is typically used for deposits up to one month.
	  /// </summary>
	  public static readonly TermDepositConvention USD_SHORT_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.USD_SHORT_DEPOSIT_T2.Name);

	  /// <summary>
	  /// The 'CHF-Deposit-T2' term deposit convention with T+2 settlement date.
	  /// This has the modified following business day convention and is typically used
	  /// for deposits of one month and over.
	  /// </summary>
	  public static readonly TermDepositConvention CHF_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.CHF_DEPOSIT_T2.Name);
	  /// <summary>
	  /// The 'CHF-ShortDeposit-T0' term deposit convention with T+0 settlement date.
	  /// This has the following business day convention and is typically used for O/N.
	  /// </summary>
	  public static readonly TermDepositConvention CHF_SHORT_DEPOSIT_T0 = TermDepositConvention.of(StandardTermDepositConventions.CHF_SHORT_DEPOSIT_T0.Name);
	  /// <summary>
	  /// The 'CHF-ShortDeposit-T1' term deposit convention with T+1 settlement date
	  /// This has the following business day convention and is typically used for T/N.
	  /// </summary>
	  public static readonly TermDepositConvention CHF_SHORT_DEPOSIT_T1 = TermDepositConvention.of(StandardTermDepositConventions.CHF_SHORT_DEPOSIT_T1.Name);
	  /// <summary>
	  /// The 'CHF-ShortDeposit-T2' term deposit convention with T+2 settlement date
	  /// This has the following business day convention and is typically used for deposits up to one month.
	  /// </summary>
	  public static readonly TermDepositConvention CHF_SHORT_DEPOSIT_T2 = TermDepositConvention.of(StandardTermDepositConventions.CHF_SHORT_DEPOSIT_T2.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private TermDepositConventions()
	  {
	  }

	}

}