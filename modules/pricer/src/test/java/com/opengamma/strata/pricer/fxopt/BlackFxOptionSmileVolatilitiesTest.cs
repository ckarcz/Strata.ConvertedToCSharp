using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.EUR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.GBP;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.currency.Currency.USD;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_365F;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveExtrapolators.FLAT;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.curve.interpolator.CurveInterpolators.LINEAR;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using CurrencyPair = com.opengamma.strata.basics.currency.CurrencyPair;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="BlackFxOptionSmileVolatilities"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class BlackFxOptionSmileVolatilitiesTest
	public class BlackFxOptionSmileVolatilitiesTest
	{

	  private static readonly FxOptionVolatilitiesName NAME = FxOptionVolatilitiesName.of("Test");
	  private static readonly DoubleArray TIME_TO_EXPIRY = DoubleArray.of(0.01, 0.252, 0.501, 1.0, 2.0, 5.0);
	  private static readonly DoubleArray ATM = DoubleArray.of(0.175, 0.185, 0.18, 0.17, 0.16, 0.16);
	  private static readonly DoubleArray DELTA = DoubleArray.of(0.10, 0.25);
	  private static readonly DoubleMatrix RISK_REVERSAL = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {-0.010, -0.0050},
		  new double[] {-0.011, -0.0060},
		  new double[] {-0.012, -0.0070},
		  new double[] {-0.013, -0.0080},
		  new double[] {-0.014, -0.0090},
		  new double[] {-0.014, -0.0090}
	  });
	  private static readonly DoubleMatrix STRANGLE = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.0300, 0.0100},
		  new double[] {0.0310, 0.0110},
		  new double[] {0.0320, 0.0120},
		  new double[] {0.0330, 0.0130},
		  new double[] {0.0340, 0.0140},
		  new double[] {0.0340, 0.0140}
	  });
	  private static readonly InterpolatedStrikeSmileDeltaTermStructure SMILE_TERM = InterpolatedStrikeSmileDeltaTermStructure.of(TIME_TO_EXPIRY, DELTA, ATM, RISK_REVERSAL, STRANGLE, ACT_365F);
	  private static readonly LocalDate VAL_DATE = date(2015, 2, 17);
	  private static readonly LocalTime VAL_TIME = LocalTime.of(13, 45);
	  private static readonly ZoneId LONDON_ZONE = ZoneId.of("Europe/London");
	  private static readonly ZonedDateTime VAL_DATE_TIME = VAL_DATE.atTime(VAL_TIME).atZone(LONDON_ZONE);
	  private static readonly CurrencyPair CURRENCY_PAIR = CurrencyPair.of(EUR, USD);

	  private static readonly BlackFxOptionSmileVolatilities VOLS = BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, VAL_DATE_TIME, SMILE_TERM);
	  private static readonly LocalTime TIME = LocalTime.of(11, 45);
	  private static readonly ZonedDateTime[] TEST_EXPIRY = new ZonedDateTime[] {date(2015, 2, 18).atTime(LocalTime.MIDNIGHT).atZone(LONDON_ZONE), date(2015, 9, 17).atTime(TIME).atZone(LONDON_ZONE), date(2016, 6, 17).atTime(TIME).atZone(LONDON_ZONE), date(2018, 7, 17).atTime(TIME).atZone(LONDON_ZONE)};
	  private static readonly double[] FORWARD = new double[] {1.4, 1.395, 1.39, 1.38, 1.35};
	  private static readonly int NB_EXPIRY = TEST_EXPIRY.Length;
	  private static readonly double[] TEST_STRIKE = new double[] {1.1, 1.28, 1.45, 1.62, 1.8};
	  private static readonly int NB_STRIKE = TEST_STRIKE.Length;

	  private const double TOLERANCE = 1.0E-12;
	  private const double EPS = 1.0E-7;

	  //-------------------------------------------------------------------------
	  public virtual void test_builder()
	  {
		BlackFxOptionSmileVolatilities test = BlackFxOptionSmileVolatilities.builder().name(NAME).currencyPair(CURRENCY_PAIR).smile(SMILE_TERM).valuationDateTime(VAL_DATE_TIME).build();
		assertEquals(test.Name, NAME);
		assertEquals(test.ValuationDateTime, VAL_DATE_TIME);
		assertEquals(test.CurrencyPair, CURRENCY_PAIR);
		assertEquals(test.Smile, SMILE_TERM);
		assertEquals(VOLS, test);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_volatility()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_EXPIRY[i]);
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double volExpected = SMILE_TERM.volatility(expiryTime, TEST_STRIKE[j], FORWARD[i]);
			double volComputed = VOLS.volatility(CURRENCY_PAIR, TEST_EXPIRY[i], TEST_STRIKE[j], FORWARD[i]);
			assertEquals(volComputed, volExpected, TOLERANCE);
		  }
		}
	  }

	  public virtual void test_volatility_inverse()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  double expiryTime = VOLS.relativeTime(TEST_EXPIRY[i]);
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double volExpected = SMILE_TERM.volatility(expiryTime, TEST_STRIKE[j], FORWARD[i]);
			double volComputed = VOLS.volatility(CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i]);
			assertEquals(volComputed, volExpected, TOLERANCE);
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void test_surfaceParameterSensitivity()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR, timeToExpiry, TEST_STRIKE[j], FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivity computed = VOLS.parameterSensitivity(sensi).Sensitivities.get(0);
			IEnumerator<ParameterMetadata> itr = computed.ParameterMetadata.GetEnumerator();
			foreach (double value in computed.Sensitivity.toArray())
			{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			  FxVolatilitySurfaceYearFractionParameterMetadata meta = ((FxVolatilitySurfaceYearFractionParameterMetadata) itr.next());
			  double nodeExpiry = meta.YearFraction;
			  double nodeDelta = meta.Strike.Value;
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR, TEST_EXPIRY[i], TEST_STRIKE[j], FORWARD[i], nodeExpiry, nodeDelta);
			  assertEquals(value, expected, EPS);
			}

		  }
		}
	  }

	  public virtual void test_surfaceParameterSensitivity_inverse()
	  {
		for (int i = 0; i < NB_EXPIRY; i++)
		{
		  for (int j = 0; j < NB_STRIKE; ++j)
		  {
			double timeToExpiry = VOLS.relativeTime(TEST_EXPIRY[i]);
			FxOptionSensitivity sensi = FxOptionSensitivity.of(VOLS.Name, CURRENCY_PAIR.inverse(), timeToExpiry, 1d / TEST_STRIKE[j], 1d / FORWARD[i], GBP, 1d);
			CurrencyParameterSensitivity computed = VOLS.parameterSensitivity(sensi).Sensitivities.get(0);
			IEnumerator<ParameterMetadata> itr = computed.ParameterMetadata.GetEnumerator();
			foreach (double value in computed.Sensitivity.toArray())
			{
//JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
			  FxVolatilitySurfaceYearFractionParameterMetadata meta = ((FxVolatilitySurfaceYearFractionParameterMetadata) itr.next());
			  double nodeExpiry = meta.YearFraction;
			  double nodeDelta = meta.Strike.Value;
			  double expected = nodeSensitivity(VOLS, CURRENCY_PAIR.inverse(), TEST_EXPIRY[i], 1d / TEST_STRIKE[j], 1d / FORWARD[i], nodeExpiry, nodeDelta);
			  assertEquals(value, expected, EPS);
			}
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		BlackFxOptionSmileVolatilities test1 = BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, VAL_DATE_TIME, SMILE_TERM);
		coverImmutableBean(test1);
		BlackFxOptionSmileVolatilities test2 = BlackFxOptionSmileVolatilities.of(FxOptionVolatilitiesName.of("Boo"), CURRENCY_PAIR.inverse(), ZonedDateTime.of(2015, 12, 21, 11, 15, 0, 0, ZoneId.of("Z")), SMILE_TERM);
		coverBeanEquals(test1, test2);
	  }

	  //-------------------------------------------------------------------------
	  // bumping a node point at (nodeExpiry, nodeDelta)
	  private double nodeSensitivity(BlackFxOptionSmileVolatilities provider, CurrencyPair pair, ZonedDateTime expiry, double strike, double forward, double nodeExpiry, double nodeDelta)
	  {

		double strikeMod = provider.CurrencyPair.Equals(pair) ? strike : 1.0 / strike;
		double forwardMod = provider.CurrencyPair.Equals(pair) ? forward : 1.0 / forward;

		InterpolatedStrikeSmileDeltaTermStructure smileTerm = (InterpolatedStrikeSmileDeltaTermStructure) provider.Smile;
		double[] times = smileTerm.Expiries.toArray();
		int nTimes = times.Length;
		SmileDeltaParameters[] volTermUp = new SmileDeltaParameters[nTimes];
		SmileDeltaParameters[] volTermDw = new SmileDeltaParameters[nTimes];
		int deltaIndex = -1;
		for (int i = 0; i < nTimes; ++i)
		{
		  DoubleArray deltas = smileTerm.VolatilityTerm.get(i).Delta;
		  int nDeltas = deltas.size();
		  int nDeltasTotal = 2 * nDeltas + 1;
		  double[] deltasTotal = new double[nDeltasTotal];
		  deltasTotal[nDeltas] = 0.5d;
		  for (int j = 0; j < nDeltas; ++j)
		  {
			deltasTotal[j] = 1d - deltas.get(j);
			deltasTotal[2 * nDeltas - j] = deltas.get(j);
		  }
		  double[] volsUp = smileTerm.VolatilityTerm.get(i).Volatility.toArray();
		  double[] volsDw = smileTerm.VolatilityTerm.get(i).Volatility.toArray();
		  if (Math.Abs(times[i] - nodeExpiry) < TOLERANCE)
		  {
			for (int j = 0; j < nDeltasTotal; ++j)
			{
			  if (Math.Abs(deltasTotal[j] - nodeDelta) < TOLERANCE)
			  {
				deltaIndex = j;
				volsUp[j] += EPS;
				volsDw[j] -= EPS;
			  }
			}
		  }
		  volTermUp[i] = SmileDeltaParameters.of(times[i], deltas, DoubleArray.copyOf(volsUp));
		  volTermDw[i] = SmileDeltaParameters.of(times[i], deltas, DoubleArray.copyOf(volsDw));
		}
		InterpolatedStrikeSmileDeltaTermStructure smileTermUp = InterpolatedStrikeSmileDeltaTermStructure.of(ImmutableList.copyOf(volTermUp), ACT_365F);
		InterpolatedStrikeSmileDeltaTermStructure smileTermDw = InterpolatedStrikeSmileDeltaTermStructure.of(ImmutableList.copyOf(volTermDw), ACT_365F);
		BlackFxOptionSmileVolatilities provUp = BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, VAL_DATE_TIME, smileTermUp);
		BlackFxOptionSmileVolatilities provDw = BlackFxOptionSmileVolatilities.of(NAME, CURRENCY_PAIR, VAL_DATE_TIME, smileTermDw);
		double volUp = provUp.volatility(pair, expiry, strike, forward);
		double volDw = provDw.volatility(pair, expiry, strike, forward);
		double totalSensi = 0.5 * (volUp - volDw) / EPS;

		double expiryTime = provider.relativeTime(expiry);
		SmileDeltaParameters singleSmile = smileTerm.smileForExpiry(expiryTime);
		double[] strikesUp = singleSmile.strike(forwardMod).toArray();
		double[] strikesDw = strikesUp.Clone();
		double[] vols = singleSmile.Volatility.toArray();
		strikesUp[deltaIndex] += EPS;
		strikesDw[deltaIndex] -= EPS;
		double volStrikeUp = LINEAR.bind(DoubleArray.ofUnsafe(strikesUp), DoubleArray.ofUnsafe(vols), FLAT, FLAT).interpolate(strikeMod);
		double volStrikeDw = LINEAR.bind(DoubleArray.ofUnsafe(strikesDw), DoubleArray.ofUnsafe(vols), FLAT, FLAT).interpolate(strikeMod);
		double sensiStrike = 0.5 * (volStrikeUp - volStrikeDw) / EPS;
		SmileDeltaParameters singleSmileUp = smileTermUp.smileForExpiry(expiryTime);
		double strikeUp = singleSmileUp.strike(forwardMod).get(deltaIndex);
		SmileDeltaParameters singleSmileDw = smileTermDw.smileForExpiry(expiryTime);
		double strikeDw = singleSmileDw.strike(forwardMod).get(deltaIndex);
		double sensiVol = 0.5 * (strikeUp - strikeDw) / EPS;

		return totalSensi - sensiStrike * sensiVol;
	  }

	}

}