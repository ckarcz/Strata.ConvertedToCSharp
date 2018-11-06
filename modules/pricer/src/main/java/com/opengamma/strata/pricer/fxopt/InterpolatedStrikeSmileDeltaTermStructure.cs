using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.TIME_SQUARE;


	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableConstructor = org.joda.beans.gen.ImmutableConstructor;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using BoundCurveInterpolator = com.opengamma.strata.market.curve.interpolator.BoundCurveInterpolator;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using ParameterizedDataCombiner = com.opengamma.strata.market.param.ParameterizedDataCombiner;

	/// <summary>
	/// An interpolated term structure of smiles as used in Forex market.
	/// <para>
	/// The term structure defined here is composed of smile descriptions at different times.
	/// The data of each smile contains delta and volatility in <seealso cref="SmileDeltaParameters"/>. 
	/// The delta values must be common to all of the smiles.
	/// </para>
	/// <para>
	/// Time interpolation and extrapolation are used to obtain a smile for the objective time.
	/// Strike interpolation and extrapolation are used in the expiry-strike space where the
	/// delta values are converted to strikes using the Black formula.
	/// </para>
	/// <para>
	/// The default for the time direction is time squire interpolation with flat extrapolation.
	/// The default for the strike direction is linear interpolation with flat extrapolation.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class InterpolatedStrikeSmileDeltaTermStructure implements SmileDeltaTermStructure, com.opengamma.strata.market.param.ParameterizedData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class InterpolatedStrikeSmileDeltaTermStructure : SmileDeltaTermStructure, ParameterizedData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.google.common.collect.ImmutableList<SmileDeltaParameters> volatilityTerm;
		private readonly ImmutableList<SmileDeltaParameters> volatilityTerm;
	  /// <summary>
	  /// The day count convention used for the expiry.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The interpolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator timeInterpolator;
	  private readonly CurveInterpolator timeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorLeft;
	  private readonly CurveExtrapolator timeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the time dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorRight;
	  private readonly CurveExtrapolator timeExtrapolatorRight;
	  /// <summary>
	  /// The interpolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator strikeInterpolator;
	  private readonly CurveInterpolator strikeInterpolator;
	  /// <summary>
	  /// The left extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorLeft;
	  private readonly CurveExtrapolator strikeExtrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator used in the strike dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorRight;
	  private readonly CurveExtrapolator strikeExtrapolatorRight;
	  /// <summary>
	  /// A set of expiry times for the smile descriptions.
	  /// <para>
	  /// This set must be consistent with the expiry values in {@code volatilityTerm},
	  /// thus can be derived if {@code volatilityTerm} is the primary input.
	  /// </para>
	  /// </summary>
	  [NonSerialized]
	  private readonly DoubleArray expiries; // derived
	  /// <summary>
	  /// The parameter combiner.
	  /// </summary>
	  [NonSerialized]
	  private readonly ParameterizedDataCombiner paramCombiner; // not a property

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains volatility term structure from a set of smile descriptions.
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// The strike dimension will use 'Linear' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityTerm">  the volatility descriptions </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(IList<SmileDeltaParameters> volatilityTerm, DayCount dayCount)
	  {

		return of(volatilityTerm, dayCount, FLAT, TIME_SQUARE, FLAT, FLAT, LINEAR, FLAT);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from a set of smile descriptions 
	  /// with strike interpolator and extrapolators specified.
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="volatilityTerm">  the volatility descriptions </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(IList<SmileDeltaParameters> volatilityTerm, DayCount dayCount, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {

		return of(volatilityTerm, dayCount, FLAT, TIME_SQUARE, FLAT, strikeExtrapolatorLeft, strikeInterpolator, strikeExtrapolatorRight);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from a set of smile descriptions 
	  /// with interpolator and extrapolators fully specified.
	  /// </summary>
	  /// <param name="volatilityTerm">  the volatility descriptions </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="timeExtrapolatorLeft">  left extrapolator used in the time dimension </param>
	  /// <param name="timeInterpolator">  interpolator used in the time dimension </param>
	  /// <param name="timeExtrapolatorRight">  right extrapolator used in the time dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(IList<SmileDeltaParameters> volatilityTerm, DayCount dayCount, CurveExtrapolator timeExtrapolatorLeft, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorRight, CurveExtrapolator strikeExtrapolatorLeft, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorRight)
	  {

		ArgChecker.notEmpty(volatilityTerm, "volatilityTerm");
		ArgChecker.notNull(dayCount, "dayCount");
		int nSmiles = volatilityTerm.Count;
		DoubleArray deltaBase = volatilityTerm[0].Delta;
		for (int i = 1; i < nSmiles; ++i)
		{
		  ArgChecker.isTrue(deltaBase.Equals(volatilityTerm[i].Delta), "delta must be common to all smiles");
		}
		return new InterpolatedStrikeSmileDeltaTermStructure(volatilityTerm, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values and volatilities.
	  /// <para>
	  /// The market date consists of time to expiry, delta and volatility.
	  /// The delta must be positive and sorted in ascending order.
	  /// The range of delta is common to all time to expiry.
	  /// </para>
	  /// <para>
	  /// {@code volatility} should be {@code n * (2 * m + 1)}, where {@code n} is the length of {@code expiry}
	  /// and {@code m} is the length of {@code delta}.
	  /// </para>
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// The strike dimension will use 'Linear' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="volatility">  the volatilities </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleMatrix volatility, DayCount dayCount)
	  {

		return of(expiries, delta, volatility, dayCount, TIME_SQUARE, FLAT, FLAT, LINEAR, FLAT, FLAT);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values and volatilities
	  /// with strike interpolator and extrapolators specified.
	  /// <para>
	  /// The market date consists of time to expiry, delta and volatility.
	  /// The delta must be positive and sorted in ascending order.
	  /// The range of delta is common to all time to expiry.
	  /// </para>
	  /// <para>
	  /// {@code volatility} should be {@code n * (2 * m + 1)}, where {@code n} is the length of {@code expiry}
	  /// and {@code m} is the length of {@code delta}.
	  /// </para>
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="volatility">  the volatilities </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleMatrix volatility, DayCount dayCount, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {

		return of(expiries, delta, volatility, dayCount, TIME_SQUARE, FLAT, FLAT, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values and volatilities 
	  /// with interpolator and extrapolators fully specified.
	  /// <para>
	  /// The market date consists of time to expiry, delta and volatility.
	  /// The delta must be positive and sorted in ascending order.
	  /// The range of delta is common to all time to expiry.
	  /// </para>
	  /// <para>
	  /// {@code volatility} should be {@code n * (2 * m + 1)}, where {@code n} is the length of {@code expiry}
	  /// and {@code m} is the length of {@code delta}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="volatility">  the volatilities </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="timeInterpolator">  interpolator used in the time dimension </param>
	  /// <param name="timeExtrapolatorLeft">  left extrapolator used in the time dimension </param>
	  /// <param name="timeExtrapolatorRight">  right extrapolator used in the time dimension </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleMatrix volatility, DayCount dayCount, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {

		ArgChecker.notNull(delta, "delta");
		ArgChecker.notNull(volatility, "volatility");
		ArgChecker.notNull(expiries, "expiries");
		ArgChecker.notNull(dayCount, "dayCount");
		ArgChecker.isTrue(delta.size() > 0, "Need more than one volatility value to perform strike interpolation");
		int nbExp = expiries.size();
		ArgChecker.isTrue(volatility.rowCount() == nbExp, "Volatility array length {} should be equal to the number of expiries {}", volatility.rowCount(), nbExp);
		ArgChecker.isTrue(volatility.columnCount() == 2 * delta.size() + 1, "Volatility array {} should be equal to (2 * number of deltas) + 1, have {}", volatility.columnCount(), 2 * delta.size() + 1);
		ImmutableList.Builder<SmileDeltaParameters> vt = ImmutableList.builder();
		for (int loopexp = 0; loopexp < nbExp; loopexp++)
		{
		  vt.add(SmileDeltaParameters.of(expiries.get(loopexp), delta, volatility.row(loopexp)));
		}
		return new InterpolatedStrikeSmileDeltaTermStructure(vt.build(), dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight, expiries);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values, ATM volatilities, risk reversal figures and
	  /// strangle figures.
	  /// <para>
	  /// The range of delta is common to all time to expiry.
	  /// {@code riskReversal} and {@code strangle} should be {@code n * m}, and the length of {@code atm} should {@code n}, 
	  /// where {@code n} is the length of {@code expiry} and {@code m} is the length of {@code delta}.
	  /// </para>
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// The strike dimension will use 'Linear' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="atm">  the ATM volatilities </param>
	  /// <param name="riskReversal">  the risk reversal figures </param>
	  /// <param name="strangle">  the strangle figures </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleArray atm, DoubleMatrix riskReversal, DoubleMatrix strangle, DayCount dayCount)
	  {

		return of(expiries, delta, atm, riskReversal, strangle, dayCount, TIME_SQUARE, FLAT, FLAT, LINEAR, FLAT, FLAT);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values, ATM volatilities, risk reversal figures and
	  /// strangle figures with strike interpolator and extrapolators specified.
	  /// <para>
	  /// The range of delta is common to all time to expiry.
	  /// {@code riskReversal} and {@code strangle} should be {@code n * m}, and the length of {@code atm} should {@code n}, 
	  /// where {@code n} is the length of {@code expiry} and {@code m} is the length of {@code delta}.
	  /// </para>
	  /// <para>
	  /// The time dimension will use 'TimeSquare' interpolation with flat extrapolation.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="atm">  the ATM volatilities </param>
	  /// <param name="riskReversal">  the risk reversal figures </param>
	  /// <param name="strangle">  the strangle figures </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleArray atm, DoubleMatrix riskReversal, DoubleMatrix strangle, DayCount dayCount, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {

		return of(expiries, delta, atm, riskReversal, strangle, dayCount, TIME_SQUARE, FLAT, FLAT, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  /// <summary>
	  /// Obtains volatility term structure from expiry times, delta values, ATM volatilities, risk reversal figures and
	  /// strangle figures with interpolator and extrapolators fully specified.
	  /// <para>
	  /// The range of delta is common to all time to expiry.
	  /// {@code riskReversal} and {@code strangle} should be {@code n * m}, and the length of {@code atm} should {@code n}, 
	  /// where {@code n} is the length of {@code expiry} and {@code m} is the length of {@code delta}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiry times of individual volatility smiles </param>
	  /// <param name="delta">  the delta values </param>
	  /// <param name="atm">  the ATM volatilities </param>
	  /// <param name="riskReversal">  the risk reversal figures </param>
	  /// <param name="strangle">  the strangle figures </param>
	  /// <param name="dayCount">  the day count used for the expiry year-fraction </param>
	  /// <param name="timeInterpolator">  interpolator used in the time dimension </param>
	  /// <param name="timeExtrapolatorLeft">  left extrapolator used in the time dimension </param>
	  /// <param name="timeExtrapolatorRight">  right extrapolator used in the time dimension </param>
	  /// <param name="strikeInterpolator">  interpolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorLeft">  left extrapolator used in the strike dimension </param>
	  /// <param name="strikeExtrapolatorRight">  right extrapolator used in the strike dimension </param>
	  /// <returns> the instance </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure of(DoubleArray expiries, DoubleArray delta, DoubleArray atm, DoubleMatrix riskReversal, DoubleMatrix strangle, DayCount dayCount, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight)
	  {

		ArgChecker.notNull(expiries, "expiries");
		ArgChecker.notNull(delta, "delta");
		ArgChecker.notNull(atm, "ATM");
		ArgChecker.notNull(riskReversal, "risk reversal");
		ArgChecker.notNull(strangle, "strangle");
		ArgChecker.notNull(dayCount, "dayCount");
		int nbExp = expiries.size();
		ArgChecker.isTrue(atm.size() == nbExp, "ATM length should be coherent with time to expiry length");
		ArgChecker.isTrue(riskReversal.rowCount() == nbExp, "Risk reversal length should be coherent with time to expiry length");
		ArgChecker.isTrue(strangle.rowCount() == nbExp, "Strangle length should be coherent with time to expiry length");
		ArgChecker.isTrue(riskReversal.columnCount() == delta.size(), "Risk reversal size should be coherent with time to delta length");
		ArgChecker.isTrue(strangle.columnCount() == delta.size(), "Strangle size should be coherent with time to delta length");
		ImmutableList.Builder<SmileDeltaParameters> vt = ImmutableList.builder();
		for (int loopexp = 0; loopexp < nbExp; loopexp++)
		{
		  vt.add(SmileDeltaParameters.of(expiries.get(loopexp), atm.get(loopexp), delta, riskReversal.row(loopexp), strangle.row(loopexp)));
		}
		return new InterpolatedStrikeSmileDeltaTermStructure(vt.build(), dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight, expiries);
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableConstructor private InterpolatedStrikeSmileDeltaTermStructure(java.util.List<SmileDeltaParameters> volatilityTerm, com.opengamma.strata.basics.date.DayCount dayCount, com.opengamma.strata.market.curve.interpolator.CurveInterpolator timeInterpolator, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorLeft, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator timeExtrapolatorRight, com.opengamma.strata.market.curve.interpolator.CurveInterpolator strikeInterpolator, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorLeft, com.opengamma.strata.market.curve.interpolator.CurveExtrapolator strikeExtrapolatorRight)
	  private InterpolatedStrikeSmileDeltaTermStructure(IList<SmileDeltaParameters> volatilityTerm, DayCount dayCount, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight) : this(volatilityTerm, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight, DoubleArray.copyOf(volatilityTerm.Select(vt -> vt.Expiry).ToList()))
	  {

	  }

	  private InterpolatedStrikeSmileDeltaTermStructure(IList<SmileDeltaParameters> volatilityTerm, DayCount dayCount, CurveInterpolator timeInterpolator, CurveExtrapolator timeExtrapolatorLeft, CurveExtrapolator timeExtrapolatorRight, CurveInterpolator strikeInterpolator, CurveExtrapolator strikeExtrapolatorLeft, CurveExtrapolator strikeExtrapolatorRight, DoubleArray expiries)
	  {

		JodaBeanUtils.notNull(volatilityTerm, "volatilityTerm");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(timeInterpolator, "timeInterpolator");
		JodaBeanUtils.notNull(timeExtrapolatorLeft, "timeExtrapolatorLeft");
		JodaBeanUtils.notNull(timeExtrapolatorRight, "timeExtrapolatorRight");
		JodaBeanUtils.notNull(strikeInterpolator, "strikeInterpolator");
		JodaBeanUtils.notNull(strikeExtrapolatorLeft, "strikeExtrapolatorLeft");
		JodaBeanUtils.notNull(strikeExtrapolatorRight, "strikeExtrapolatorRight");
		this.volatilityTerm = ImmutableList.copyOf(volatilityTerm);
		this.dayCount = dayCount;
		this.timeExtrapolatorLeft = timeExtrapolatorLeft;
		this.timeInterpolator = timeInterpolator;
		this.timeExtrapolatorRight = timeExtrapolatorRight;
		this.strikeExtrapolatorLeft = strikeExtrapolatorLeft;
		this.strikeInterpolator = strikeInterpolator;
		this.strikeExtrapolatorRight = strikeExtrapolatorRight;
		this.expiries = expiries;
		this.paramCombiner = ParameterizedDataCombiner.of(volatilityTerm);
	  }

	  private object readResolve()
	  {
		return new InterpolatedStrikeSmileDeltaTermStructure(volatilityTerm, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return paramCombiner.ParameterCount;
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return paramCombiner.getParameter(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return paramCombiner.getParameterMetadata(parameterIndex);
	  }

	  public InterpolatedStrikeSmileDeltaTermStructure withParameter(int parameterIndex, double newValue)
	  {
		IList<SmileDeltaParameters> updated = paramCombiner.withParameter(typeof(SmileDeltaParameters), parameterIndex, newValue);
		return new InterpolatedStrikeSmileDeltaTermStructure(updated, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  public InterpolatedStrikeSmileDeltaTermStructure withPerturbation(ParameterPerturbation perturbation)
	  {
		IList<SmileDeltaParameters> updated = paramCombiner.withPerturbation(typeof(SmileDeltaParameters), perturbation);
		return new InterpolatedStrikeSmileDeltaTermStructure(updated, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
	  }

	  //-------------------------------------------------------------------------
	  public DoubleArray Expiries
	  {
		  get
		  {
			return expiries;
		  }
	  }

	  //-------------------------------------------------------------------------
	  public double volatility(double time, double strike, double forward)
	  {
		ArgChecker.isTrue(time >= 0, "Positive time");
		SmileDeltaParameters smile = smileForExpiry(time);
		DoubleArray strikes = smile.strike(forward);
		BoundCurveInterpolator bound = strikeInterpolator.bind(strikes, smile.Volatility, strikeExtrapolatorLeft, strikeExtrapolatorRight);
		return bound.interpolate(strike);
	  }

	  public VolatilityAndBucketedSensitivities volatilityAndSensitivities(double time, double strike, double forward)
	  {
		ArgChecker.isTrue(time >= 0, "Positive time");
		SmileDeltaParameters smile = smileForExpiry(time);
		DoubleArray strikes = smile.strike(forward);
		BoundCurveInterpolator bound = strikeInterpolator.bind(strikes, smile.Volatility, strikeExtrapolatorLeft, strikeExtrapolatorRight);
		double volatility = bound.interpolate(strike);
		DoubleArray smileVolatilityBar = bound.parameterSensitivity(strike);
		SmileAndBucketedSensitivities smileAndSensitivities = smileAndSensitivitiesForExpiry(time, smileVolatilityBar);
		return VolatilityAndBucketedSensitivities.of(volatility, smileAndSensitivities.Sensitivities);
	  }

	  //-------------------------------------------------------------------------
	  public SmileDeltaParameters smileForExpiry(double expiry)
	  {
		int nbVol = StrikeCount;
		int nbTime = SmileCount;
		ArgChecker.isTrue(nbTime > 1, "Need more than one time value to perform interpolation");
		double[] volatilityT = new double[nbVol];
		for (int loopvol = 0; loopvol < nbVol; loopvol++)
		{
		  double[] volDelta = new double[nbTime];
		  for (int looptime = 0; looptime < nbTime; looptime++)
		  {
			volDelta[looptime] = volatilityTerm.get(looptime).Volatility.get(loopvol);
		  }
		  BoundCurveInterpolator bound = timeInterpolator.bind(Expiries, DoubleArray.ofUnsafe(volDelta), timeExtrapolatorLeft, timeExtrapolatorRight);
		  volatilityT[loopvol] = bound.interpolate(expiry);
		}
		return SmileDeltaParameters.of(expiry, Delta, DoubleArray.ofUnsafe(volatilityT));
	  }

	  public SmileAndBucketedSensitivities smileAndSensitivitiesForExpiry(double expiry, DoubleArray volatilityAtTimeSensitivity)
	  {

		int nbVol = StrikeCount;
		ArgChecker.isTrue(volatilityAtTimeSensitivity.size() == nbVol, "Sensitivity with incorrect size");
		ArgChecker.isTrue(nbVol > 1, "Need more than one volatility value to perform interpolation");
		int nbTime = SmileCount;
		ArgChecker.isTrue(nbTime > 1, "Need more than one time value to perform interpolation");
		double[] volatilityT = new double[nbVol];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] volatilitySensitivity = new double[nbTime][nbVol];
		double[][] volatilitySensitivity = RectangularArrays.ReturnRectangularDoubleArray(nbTime, nbVol);
		for (int loopvol = 0; loopvol < nbVol; loopvol++)
		{
		  double[] volDelta = new double[nbTime];
		  for (int looptime = 0; looptime < nbTime; looptime++)
		  {
			volDelta[looptime] = volatilityTerm.get(looptime).Volatility.get(loopvol);
		  }
		  BoundCurveInterpolator bound = timeInterpolator.bind(Expiries, DoubleArray.ofUnsafe(volDelta), timeExtrapolatorLeft, timeExtrapolatorRight);
		  DoubleArray volatilitySensitivityVol = bound.parameterSensitivity(expiry);
		  for (int looptime = 0; looptime < nbTime; looptime++)
		  {
			volatilitySensitivity[looptime][loopvol] = volatilitySensitivityVol.get(looptime) * volatilityAtTimeSensitivity.get(loopvol);
		  }
		  volatilityT[loopvol] = bound.interpolate(expiry);
		}
		SmileDeltaParameters smile = SmileDeltaParameters.of(expiry, Delta, DoubleArray.ofUnsafe(volatilityT));
		return SmileAndBucketedSensitivities.of(smile, DoubleMatrix.ofUnsafe(volatilitySensitivity));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedStrikeSmileDeltaTermStructure}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static InterpolatedStrikeSmileDeltaTermStructure.Meta meta()
	  {
		return InterpolatedStrikeSmileDeltaTermStructure.Meta.INSTANCE;
	  }

	  static InterpolatedStrikeSmileDeltaTermStructure()
	  {
		MetaBean.register(InterpolatedStrikeSmileDeltaTermStructure.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  public override InterpolatedStrikeSmileDeltaTermStructure.Meta metaBean()
	  {
		return InterpolatedStrikeSmileDeltaTermStructure.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the smile description at the different time to expiry. All item should have the same deltas. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<SmileDeltaParameters> VolatilityTerm
	  {
		  get
		  {
			return volatilityTerm;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count convention used for the expiry. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator TimeInterpolator
	  {
		  get
		  {
			return timeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorLeft
	  {
		  get
		  {
			return timeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the time dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator TimeExtrapolatorRight
	  {
		  get
		  {
			return timeExtrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator StrikeInterpolator
	  {
		  get
		  {
			return strikeInterpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorLeft
	  {
		  get
		  {
			return strikeExtrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator used in the strike dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator StrikeExtrapolatorRight
	  {
		  get
		  {
			return strikeExtrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  InterpolatedStrikeSmileDeltaTermStructure other = (InterpolatedStrikeSmileDeltaTermStructure) obj;
		  return JodaBeanUtils.equal(volatilityTerm, other.volatilityTerm) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(timeInterpolator, other.timeInterpolator) && JodaBeanUtils.equal(timeExtrapolatorLeft, other.timeExtrapolatorLeft) && JodaBeanUtils.equal(timeExtrapolatorRight, other.timeExtrapolatorRight) && JodaBeanUtils.equal(strikeInterpolator, other.strikeInterpolator) && JodaBeanUtils.equal(strikeExtrapolatorLeft, other.strikeExtrapolatorLeft) && JodaBeanUtils.equal(strikeExtrapolatorRight, other.strikeExtrapolatorRight);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilityTerm);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(timeExtrapolatorRight);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeInterpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeExtrapolatorRight);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(288);
		buf.Append("InterpolatedStrikeSmileDeltaTermStructure{");
		buf.Append("volatilityTerm").Append('=').Append(volatilityTerm).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("timeInterpolator").Append('=').Append(timeInterpolator).Append(',').Append(' ');
		buf.Append("timeExtrapolatorLeft").Append('=').Append(timeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("timeExtrapolatorRight").Append('=').Append(timeExtrapolatorRight).Append(',').Append(' ');
		buf.Append("strikeInterpolator").Append('=').Append(strikeInterpolator).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorLeft").Append('=').Append(strikeExtrapolatorLeft).Append(',').Append(' ');
		buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code InterpolatedStrikeSmileDeltaTermStructure}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  volatilityTerm_Renamed = DirectMetaProperty.ofImmutable(this, "volatilityTerm", typeof(InterpolatedStrikeSmileDeltaTermStructure), (Type) typeof(ImmutableList));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(DayCount));
			  timeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "timeInterpolator", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveInterpolator));
			  timeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorLeft", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveExtrapolator));
			  timeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "timeExtrapolatorRight", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveExtrapolator));
			  strikeInterpolator_Renamed = DirectMetaProperty.ofImmutable(this, "strikeInterpolator", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveInterpolator));
			  strikeExtrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorLeft", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveExtrapolator));
			  strikeExtrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "strikeExtrapolatorRight", typeof(InterpolatedStrikeSmileDeltaTermStructure), typeof(CurveExtrapolator));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "volatilityTerm", "dayCount", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code volatilityTerm} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<SmileDeltaParameters>> volatilityTerm = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "volatilityTerm", InterpolatedStrikeSmileDeltaTermStructure.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<SmileDeltaParameters>> volatilityTerm_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> timeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> timeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> strikeInterpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> strikeExtrapolatorRight_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "volatilityTerm", "dayCount", "timeInterpolator", "timeExtrapolatorLeft", "timeExtrapolatorRight", "strikeInterpolator", "strikeExtrapolatorLeft", "strikeExtrapolatorRight");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 70074929: // volatilityTerm
			  return volatilityTerm_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case -587914188: // timeInterpolator
			  return timeInterpolator_Renamed;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft_Renamed;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight_Renamed;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator_Renamed;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft_Renamed;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends InterpolatedStrikeSmileDeltaTermStructure> builder()
		public override BeanBuilder<InterpolatedStrikeSmileDeltaTermStructure> builder()
		{
		  return new InterpolatedStrikeSmileDeltaTermStructure.Builder();
		}

		public override Type beanType()
		{
		  return typeof(InterpolatedStrikeSmileDeltaTermStructure);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code volatilityTerm} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<SmileDeltaParameters>> volatilityTerm()
		{
		  return volatilityTerm_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> timeInterpolator()
		{
		  return timeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorLeft()
		{
		  return timeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code timeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> timeExtrapolatorRight()
		{
		  return timeExtrapolatorRight_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeInterpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> strikeInterpolator()
		{
		  return strikeInterpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorLeft()
		{
		  return strikeExtrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code strikeExtrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> strikeExtrapolatorRight()
		{
		  return strikeExtrapolatorRight_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 70074929: // volatilityTerm
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).VolatilityTerm;
			case 1905311443: // dayCount
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).DayCount;
			case -587914188: // timeInterpolator
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).TimeInterpolator;
			case -286652761: // timeExtrapolatorLeft
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).TimeExtrapolatorLeft;
			case -290640004: // timeExtrapolatorRight
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).TimeExtrapolatorRight;
			case 815202713: // strikeInterpolator
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).StrikeInterpolator;
			case -1176196724: // strikeExtrapolatorLeft
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).StrikeExtrapolatorLeft;
			case -2096699081: // strikeExtrapolatorRight
			  return ((InterpolatedStrikeSmileDeltaTermStructure) bean).StrikeExtrapolatorRight;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code InterpolatedStrikeSmileDeltaTermStructure}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<InterpolatedStrikeSmileDeltaTermStructure>
	  {

		internal IList<SmileDeltaParameters> volatilityTerm = ImmutableList.of();
		internal DayCount dayCount;
		internal CurveInterpolator timeInterpolator;
		internal CurveExtrapolator timeExtrapolatorLeft;
		internal CurveExtrapolator timeExtrapolatorRight;
		internal CurveInterpolator strikeInterpolator;
		internal CurveExtrapolator strikeExtrapolatorLeft;
		internal CurveExtrapolator strikeExtrapolatorRight;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 70074929: // volatilityTerm
			  return volatilityTerm;
			case 1905311443: // dayCount
			  return dayCount;
			case -587914188: // timeInterpolator
			  return timeInterpolator;
			case -286652761: // timeExtrapolatorLeft
			  return timeExtrapolatorLeft;
			case -290640004: // timeExtrapolatorRight
			  return timeExtrapolatorRight;
			case 815202713: // strikeInterpolator
			  return strikeInterpolator;
			case -1176196724: // strikeExtrapolatorLeft
			  return strikeExtrapolatorLeft;
			case -2096699081: // strikeExtrapolatorRight
			  return strikeExtrapolatorRight;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 70074929: // volatilityTerm
			  this.volatilityTerm = (IList<SmileDeltaParameters>) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount = (DayCount) newValue;
			  break;
			case -587914188: // timeInterpolator
			  this.timeInterpolator = (CurveInterpolator) newValue;
			  break;
			case -286652761: // timeExtrapolatorLeft
			  this.timeExtrapolatorLeft = (CurveExtrapolator) newValue;
			  break;
			case -290640004: // timeExtrapolatorRight
			  this.timeExtrapolatorRight = (CurveExtrapolator) newValue;
			  break;
			case 815202713: // strikeInterpolator
			  this.strikeInterpolator = (CurveInterpolator) newValue;
			  break;
			case -1176196724: // strikeExtrapolatorLeft
			  this.strikeExtrapolatorLeft = (CurveExtrapolator) newValue;
			  break;
			case -2096699081: // strikeExtrapolatorRight
			  this.strikeExtrapolatorRight = (CurveExtrapolator) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override InterpolatedStrikeSmileDeltaTermStructure build()
		{
		  return new InterpolatedStrikeSmileDeltaTermStructure(volatilityTerm, dayCount, timeInterpolator, timeExtrapolatorLeft, timeExtrapolatorRight, strikeInterpolator, strikeExtrapolatorLeft, strikeExtrapolatorRight);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(288);
		  buf.Append("InterpolatedStrikeSmileDeltaTermStructure.Builder{");
		  buf.Append("volatilityTerm").Append('=').Append(JodaBeanUtils.ToString(volatilityTerm)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount)).Append(',').Append(' ');
		  buf.Append("timeInterpolator").Append('=').Append(JodaBeanUtils.ToString(timeInterpolator)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorLeft)).Append(',').Append(' ');
		  buf.Append("timeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(timeExtrapolatorRight)).Append(',').Append(' ');
		  buf.Append("strikeInterpolator").Append('=').Append(JodaBeanUtils.ToString(strikeInterpolator)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorLeft)).Append(',').Append(' ');
		  buf.Append("strikeExtrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(strikeExtrapolatorRight));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}