/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.surface
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.basics.date.DayCounts.ACT_360;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverPrivateConstructor;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	using MoneynessType = com.opengamma.strata.market.model.MoneynessType;

	/// <summary>
	/// Test <seealso cref="Surfaces"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SurfacesTest
	public class SurfacesTest
	{

	  private const string NAME = "Foo";
	  private static readonly SurfaceName SURFACE_NAME = SurfaceName.of(NAME);

	  //-------------------------------------------------------------------------
	  public virtual void blackVolatilityByExpiryTenor_string()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryTenor(NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void blackVolatilityByExpiryTenor_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryTenor(SURFACE_NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void blackVolatilityByExpiryStrike_string()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryStrike(NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void blackVolatilityByExpiryStrike_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryStrike(SURFACE_NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void blackVolatilityByExpiryLogMoneyness_string()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryLogMoneyness(NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void blackVolatilityByExpiryLogMoneyness_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.blackVolatilityByExpiryLogMoneyness(SURFACE_NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.LOG_MONEYNESS).zValueType(ValueType.BLACK_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void normalVolatilityByExpiryTenor_string()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpiryTenor(NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void normalVolatilityByExpiryTenor_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpiryTenor(SURFACE_NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void normalVolatilityByExpiryStrike_string()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpiryStrike(NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void normalVolatilityByExpiryStrike_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpiryStrike(SURFACE_NAME, ACT_360);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.STRIKE).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void normalVolatilityByExpirySimpleMoneyness_string()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpirySimpleMoneyness(NAME, ACT_360, MoneynessType.PRICE);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SIMPLE_MONEYNESS).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).addInfo(SurfaceInfoType.MONEYNESS_TYPE, MoneynessType.PRICE).build();
		assertEquals(test, expected);
	  }

	  public virtual void normalVolatilityByExpirySimpleMoneyness_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.normalVolatilityByExpirySimpleMoneyness(SURFACE_NAME, ACT_360, MoneynessType.PRICE);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.SIMPLE_MONEYNESS).zValueType(ValueType.NORMAL_VOLATILITY).dayCount(ACT_360).addInfo(SurfaceInfoType.MONEYNESS_TYPE, MoneynessType.PRICE).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void sabrParameterByExpiryTenor_string()
	  {
		SurfaceMetadata test = Surfaces.sabrParameterByExpiryTenor(NAME, ACT_360, ValueType.SABR_BETA);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  public virtual void sabrParameterByExpiryTenor_surfaceName()
	  {
		SurfaceMetadata test = Surfaces.sabrParameterByExpiryTenor(SURFACE_NAME, ACT_360, ValueType.SABR_BETA);
		SurfaceMetadata expected = DefaultSurfaceMetadata.builder().surfaceName(SURFACE_NAME).xValueType(ValueType.YEAR_FRACTION).yValueType(ValueType.YEAR_FRACTION).zValueType(ValueType.SABR_BETA).dayCount(ACT_360).build();
		assertEquals(test, expected);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		coverPrivateConstructor(typeof(Surfaces));
	  }

	}

}