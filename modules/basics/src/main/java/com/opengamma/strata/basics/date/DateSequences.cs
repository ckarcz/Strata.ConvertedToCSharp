/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.basics.date
{
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;

	/// <summary>
	/// Constants and implementations for standard date sequences.
	/// <para>
	/// This class provides instances of <seealso cref="DateSequence"/> representing standard financial industry
	/// sequences of dates. The most common are the quarterly IMM dates, which are on the third
	/// Wednesday of March, June, September and December.
	/// </para>
	/// <para>
	/// Additional date sequences may be registered by name using {@code DateSequence.ini}.
	/// </para>
	/// </summary>
	public sealed class DateSequences
	{
	  // constants are indirected via ENUM_LOOKUP to allow them to be replaced by config

	  /// <summary>
	  /// The extended enum lookup from name to instance.
	  /// </summary>
	  internal static readonly ExtendedEnum<DateSequence> ENUM_LOOKUP = ExtendedEnum.of(typeof(DateSequence));

	  /// <summary>
	  /// The 'Quarterly-IMM' date sequence.
	  /// <para>
	  /// An instance defining the sequence of quarterly IMM dates.
	  /// The quarterly IMM dates are the third Wednesday of March, June, September and December.
	  /// </para>
	  /// </summary>
	  public static readonly DateSequence QUARTERLY_IMM = DateSequence.of(StandardDateSequences.QUARTERLY_IMM.Name);
	  /// <summary>
	  /// The 'Monthly-IMM' date sequence.
	  /// <para>
	  /// An instance defining the sequence of monthly IMM dates.
	  /// The monthly IMM dates are the third Wednesday of each month.
	  /// </para>
	  /// </summary>
	  public static readonly DateSequence MONTHLY_IMM = DateSequence.of(StandardDateSequences.MONTHLY_IMM.Name);
	  /// <summary>
	  /// The 'Quarterly-10th' date sequence.
	  /// <para>
	  /// An instance defining the sequence of quarterly dates on the 10th of each month.
	  /// The quarterly months are March, June, September and December.
	  /// </para>
	  /// </summary>
	  public static readonly DateSequence QUARTERLY_10TH = DateSequence.of(StandardDateSequences.QUARTERLY_10TH.Name);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private DateSequences()
	  {
	  }

	}

}