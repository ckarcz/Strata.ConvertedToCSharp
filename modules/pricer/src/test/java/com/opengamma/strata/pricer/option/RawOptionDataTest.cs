using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.option
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertFalse;


	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;

	/// <summary>
	/// Tests <seealso cref="RawOptionData"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class RawOptionDataTest
	public class RawOptionDataTest
	{

	  private static readonly DoubleArray MONEYNESS = DoubleArray.of(-0.010, 0.00, 0.0100, 0.0200);
	  private static readonly DoubleArray STRIKES = DoubleArray.of(-0.0050, 0.0050, 0.0150, 0.0250);
	  private static readonly IList<Period> EXPIRIES = new List<Period>();
	  static RawOptionDataTest()
	  {
		EXPIRIES.Add(Period.ofMonths(1));
		EXPIRIES.Add(Period.ofMonths(3));
		EXPIRIES.Add(Period.ofYears(1));
	  }

	  private static readonly DoubleMatrix DATA_FULL = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {0.08, 0.09, 0.10, 0.11},
		  new double[] {0.09, 0.10, 0.11, 0.12},
		  new double[] {0.10, 0.11, 0.12, 0.13}
	  });
	  private static readonly DoubleMatrix DATA_SPARSE = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {Double.NaN, Double.NaN, Double.NaN, Double.NaN},
		  new double[] {Double.NaN, 0.10, 0.11, 0.12},
		  new double[] {0.10, 0.11, 0.12, 0.13}
	  });
	  private static readonly DoubleMatrix ERROR = DoubleMatrix.ofUnsafe(new double[][]
	  {
		  new double[] {1.0e-4, 1.0e-4, 1.0e-4, 1.0e-4},
		  new double[] {1.0e-4, 1.0e-4, 1.0e-4, 1.0e-4},
		  new double[] {1.0e-3, 1.0e-3, 1.0e-3, 1.0e-3}
	  });

	  //-------------------------------------------------------------------------
	  public virtual void of()
	  {
		RawOptionData test = sut();
		assertEquals(test.Strikes, MONEYNESS);
		assertEquals(test.StrikeType, ValueType.SIMPLE_MONEYNESS);
		assertEquals(test.Data, DATA_FULL);
		assertEquals(test.DataType, ValueType.NORMAL_VOLATILITY);
	  }

	  public virtual void ofBlackVolatility()
	  {
		double shift = 0.0075;
		RawOptionData test = RawOptionData.ofBlackVolatility(EXPIRIES, STRIKES, ValueType.STRIKE, DATA_SPARSE, shift);
		assertEquals(test.Strikes, STRIKES);
		assertEquals(test.StrikeType, ValueType.STRIKE);
		assertEquals(test.Data, DATA_SPARSE);
		assertEquals(test.DataType, ValueType.BLACK_VOLATILITY);
		assertEquals(test.Shift, double?.of(shift));
		assertFalse(test.Error.Present);
	  }

	  public virtual void available_smile_at_expiry()
	  {
		double shift = 0.0075;
		RawOptionData test = RawOptionData.ofBlackVolatility(EXPIRIES, STRIKES, ValueType.STRIKE, DATA_SPARSE, shift);
		DoubleArray[] strikesAvailable = new DoubleArray[3];
		strikesAvailable[0] = DoubleArray.EMPTY;
		strikesAvailable[1] = DoubleArray.of(0.0050, 0.0150, 0.0250);
		strikesAvailable[2] = DoubleArray.of(-0.0050, 0.0050, 0.0150, 0.0250);
		DoubleArray[] volAvailable = new DoubleArray[3];
		volAvailable[0] = DoubleArray.EMPTY;
		volAvailable[1] = DoubleArray.of(0.10, 0.11, 0.12);
		volAvailable[2] = DoubleArray.of(0.10, 0.11, 0.12, 0.13);
		for (int i = 0; i < DATA_SPARSE.rowCount(); i++)
		{
		  Pair<DoubleArray, DoubleArray> smile = test.availableSmileAtExpiry(EXPIRIES[i]);
		  assertEquals(smile.First, strikesAvailable[i]);
		}
	  }

	  public virtual void of_error()
	  {
		RawOptionData test = sut3();
		assertEquals(test.Strikes, MONEYNESS);
		assertEquals(test.StrikeType, ValueType.SIMPLE_MONEYNESS);
		assertEquals(test.Data, DATA_FULL);
		assertEquals(test.DataType, ValueType.NORMAL_VOLATILITY);
		assertEquals(test.Error.get(), ERROR);
	  }

	  public virtual void ofBlackVolatility_error()
	  {
		double shift = 0.0075;
		RawOptionData test = RawOptionData.ofBlackVolatility(EXPIRIES, STRIKES, ValueType.STRIKE, DATA_SPARSE, ERROR, shift);
		assertEquals(test.Strikes, STRIKES);
		assertEquals(test.StrikeType, ValueType.STRIKE);
		assertEquals(test.Data, DATA_SPARSE);
		assertEquals(test.DataType, ValueType.BLACK_VOLATILITY);
		assertEquals(test.Shift, double?.of(shift));
		assertEquals(test.Error.get(), ERROR);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		RawOptionData test = sut();
		coverImmutableBean(test);
		RawOptionData test2 = sut2();
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		RawOptionData test = RawOptionData.of(EXPIRIES, MONEYNESS, ValueType.SIMPLE_MONEYNESS, DATA_FULL, ValueType.BLACK_VOLATILITY);
		assertSerialization(test);
	  }

	  //-------------------------------------------------------------------------
	  internal static RawOptionData sut()
	  {
		return RawOptionData.of(EXPIRIES, MONEYNESS, ValueType.SIMPLE_MONEYNESS, DATA_FULL, ValueType.NORMAL_VOLATILITY);
	  }

	  internal static RawOptionData sut2()
	  {
		IList<Period> expiries2 = new List<Period>();
		expiries2.Add(Period.ofMonths(3));
		expiries2.Add(Period.ofYears(1));
		expiries2.Add(Period.ofYears(5));
		RawOptionData test2 = RawOptionData.of(expiries2, STRIKES, ValueType.STRIKE, DATA_SPARSE, ERROR, ValueType.BLACK_VOLATILITY);
		return test2;
	  }

	  internal static RawOptionData sut3()
	  {
		return RawOptionData.of(EXPIRIES, MONEYNESS, ValueType.SIMPLE_MONEYNESS, DATA_FULL, ERROR, ValueType.NORMAL_VOLATILITY);
	  }

	}

}