/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_ACT_ISDA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.GBP_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.index.IborIndices.USD_LIBOR_3M;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;

	/// <summary>
	/// Test <seealso cref="IborCapletFloorletVolatilityCalibrationResult"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class IborCapletFloorletVolatilityCalibrationResultTest
	public class IborCapletFloorletVolatilityCalibrationResultTest
	{

	  protected internal static readonly ZonedDateTime VALUATION = LocalDate.of(2016, 3, 3).atTime(10, 0).atZone(ZoneId.of("America/New_York"));
	  private static readonly ConstantSurface SURFACE = ConstantSurface.of(Surfaces.blackVolatilityByExpiryStrike("volSurface", ACT_ACT_ISDA), 0.15);
	  private static readonly BlackIborCapletFloorletExpiryStrikeVolatilities VOLS = BlackIborCapletFloorletExpiryStrikeVolatilities.of(USD_LIBOR_3M, VALUATION, SURFACE);

	  public virtual void test_ofLestSquare()
	  {
		double chiSq = 5.5e-6;
		IborCapletFloorletVolatilityCalibrationResult test = IborCapletFloorletVolatilityCalibrationResult.ofLeastSquare(VOLS, chiSq);
		assertEquals(test.Volatilities, VOLS);
		assertEquals(test.ChiSquare, chiSq);
	  }

	  public virtual void test_ofRootFind()
	  {
		IborCapletFloorletVolatilityCalibrationResult test = IborCapletFloorletVolatilityCalibrationResult.ofRootFind(VOLS);
		assertEquals(test.Volatilities, VOLS);
		assertEquals(test.ChiSquare, 0d);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		double chiSq = 5.5e-12;
		IborCapletFloorletVolatilityCalibrationResult test1 = IborCapletFloorletVolatilityCalibrationResult.ofLeastSquare(VOLS, chiSq);
		coverImmutableBean(test1);
		IborCapletFloorletVolatilityCalibrationResult test2 = IborCapletFloorletVolatilityCalibrationResult.ofRootFind(BlackIborCapletFloorletExpiryStrikeVolatilities.of(GBP_LIBOR_3M, VALUATION, SURFACE));
		coverBeanEquals(test1, test2);
	  }

	  public virtual void test_serialization()
	  {
		IborCapletFloorletVolatilityCalibrationResult test = IborCapletFloorletVolatilityCalibrationResult.ofRootFind(VOLS);
		assertSerialization(test);
	  }

	}

}