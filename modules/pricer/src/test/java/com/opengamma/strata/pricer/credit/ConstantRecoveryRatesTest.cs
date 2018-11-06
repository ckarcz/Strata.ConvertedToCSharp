/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.credit
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using ValueType = com.opengamma.strata.market.ValueType;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using CurveName = com.opengamma.strata.market.curve.CurveName;
	using DefaultCurveMetadata = com.opengamma.strata.market.curve.DefaultCurveMetadata;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;

	/// <summary>
	/// Test <seealso cref="ConstantRecoveryRates"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ConstantRecoveryRatesTest
	public class ConstantRecoveryRatesTest
	{

	  private static readonly LocalDate VALUATION = LocalDate.of(2016, 5, 6);
	  private const double RECOVERY_RATE = 0.35;
	  private static readonly StandardId LEGAL_ENTITY = StandardId.of("OG", "ABC");
	  private static readonly LocalDate DATE_AFTER = LocalDate.of(2017, 2, 24);

	  public virtual void test_of()
	  {
		ConstantRecoveryRates test = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, RECOVERY_RATE);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.RecoveryRate, RECOVERY_RATE);
		assertEquals(test.ValuationDate, VALUATION);
		assertEquals(test.recoveryRate(DATE_AFTER), RECOVERY_RATE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		assertEquals(test.getParameter(0), RECOVERY_RATE);
		assertEquals(test.ParameterCount, 1);
		assertEquals(test.getParameterMetadata(0), ParameterMetadata.empty());
		assertEquals(test.withParameter(0, 0.5), ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, 0.5));
	  }

	  public virtual void test_of_interface()
	  {
		ConstantCurve curve = ConstantCurve.of(DefaultCurveMetadata.builder().yValueType(ValueType.RECOVERY_RATE).curveName("recoveryRate").build(), RECOVERY_RATE);
		ConstantRecoveryRates test = (ConstantRecoveryRates) RecoveryRates.of(LEGAL_ENTITY, VALUATION, curve);
		assertEquals(test.LegalEntityId, LEGAL_ENTITY);
		assertEquals(test.RecoveryRate, RECOVERY_RATE);
		assertEquals(test.ValuationDate, VALUATION);
		assertEquals(test.recoveryRate(DATE_AFTER), RECOVERY_RATE);
		assertEquals(test.findData(CurveName.of("Rubbish")), null);
		assertEquals(test.getParameter(0), RECOVERY_RATE);
		assertEquals(test.ParameterCount, 1);
		assertEquals(test.getParameterMetadata(0), ParameterMetadata.empty());
		assertEquals(test.withParameter(0, 0.5), ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, 0.5));
	  }

	  public virtual void test_of_rateOutOfRange()
	  {
		assertThrowsIllegalArg(() => ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, -0.5));
		assertThrowsIllegalArg(() => ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, 1.5));
		ConstantRecoveryRates test = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, RECOVERY_RATE);
		assertThrowsIllegalArg(() => test.getParameter(1));
		assertThrowsIllegalArg(() => test.withParameter(1, 0.5));
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ConstantRecoveryRates test1 = ConstantRecoveryRates.of(LEGAL_ENTITY, VALUATION, RECOVERY_RATE);
		coverImmutableBean(test1);
		ConstantRecoveryRates test2 = ConstantRecoveryRates.of(StandardId.of("OG", "DEF"), DATE_AFTER, 0.2d);
		coverBeanEquals(test1, test2);
	  }

	}

}