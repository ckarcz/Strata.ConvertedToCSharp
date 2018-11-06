using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.BUCKETED_PV01;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.CASH_FLOWS;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.PAR_RATE;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.calc.TestingMeasures.PRESENT_VALUE_MULTI_CCY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Result = com.opengamma.strata.collect.result.Result;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DefaultCalculationFunctionsTest
	public class DefaultCalculationFunctionsTest
	{

	  public virtual void oneDerivedFunction()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5));
		DelegateFn delegateFn = new DelegateFn(delegateResults);
		DerivedFn derivedFn = new DerivedFn();
		CalculationFunctions calculationFunctions = CalculationFunctions.of(delegateFn);
		CalculationFunctions derivedFunctions = calculationFunctions.composedWith(derivedFn);
		TestTarget target = new TestTarget(42);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super TestTarget> function = derivedFunctions.getFunction(target);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> function = derivedFunctions.getFunction(target);

		ImmutableSet<Measure> expectedMeasures = ImmutableSet.of(BUCKETED_PV01, CASH_FLOWS, PAR_RATE);
		assertThat(function.supportedMeasures()).isEqualTo(expectedMeasures);
	  }

	  /// <summary>
	  /// Test that multiple derived functions for the same target type are correctly combined.
	  /// </summary>
	  public virtual void multipleDerivedFunctionsForSameTargetType()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5));
		DelegateFn delegateFn = new DelegateFn(delegateResults);
		DerivedFn derivedFn1 = new DerivedFn();
		DerivedFn derivedFn2 = new DerivedFn(PRESENT_VALUE_MULTI_CCY);

		CalculationFunctions calculationFunctions = CalculationFunctions.of(delegateFn);
		CalculationFunctions derivedFunctions = calculationFunctions.composedWith(derivedFn1, derivedFn2);
		TestTarget target = new TestTarget(42);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super TestTarget> function = derivedFunctions.getFunction(target);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> function = derivedFunctions.getFunction(target);

		ImmutableSet<Measure> expectedMeasures = ImmutableSet.of(BUCKETED_PV01, CASH_FLOWS, PAR_RATE, PRESENT_VALUE_MULTI_CCY);
		assertThat(function.supportedMeasures()).isEqualTo(expectedMeasures);
	  }

	  /// <summary>
	  /// Test that multiple derived functions for the same target type are correctly combined when one derived function
	  /// depends on another.
	  /// </summary>
	  public virtual void multipleDerivedFunctionsForSameTargetTypeWithDependencyBetweenDerivedFunctions()
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> delegateResults = com.google.common.collect.ImmutableMap.of(CASH_FLOWS, com.opengamma.strata.collect.result.Result.success(3), PAR_RATE, com.opengamma.strata.collect.result.Result.success(5));
		IDictionary<Measure, Result<object>> delegateResults = ImmutableMap.of(CASH_FLOWS, Result.success(3), PAR_RATE, Result.success(5));
		DelegateFn delegateFn = new DelegateFn(delegateResults);
		DerivedFn derivedFn1 = new DerivedFn();
		// This depends on the measure calculated by derivedFn1
		DerivedFn derivedFn2 = new DerivedFn(PRESENT_VALUE_MULTI_CCY, ImmutableSet.of(BUCKETED_PV01));

		CalculationFunctions calculationFunctions = CalculationFunctions.of(delegateFn);
		// The derived functions must be specified in the correct order.
		// The function higher up the dependency chain must come second
		CalculationFunctions derivedFunctions = calculationFunctions.composedWith(derivedFn1, derivedFn2);
		TestTarget target = new TestTarget(42);
//JAVA TO C# CONVERTER TODO TASK: There is no .NET equivalent to the Java 'super' constraint:
//ORIGINAL LINE: CalculationFunction<? super TestTarget> function = derivedFunctions.getFunction(target);
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
		CalculationFunction<object> function = derivedFunctions.getFunction(target);

		ImmutableSet<Measure> expectedMeasures = ImmutableSet.of(BUCKETED_PV01, CASH_FLOWS, PAR_RATE, PRESENT_VALUE_MULTI_CCY);
		assertThat(function.supportedMeasures()).isEqualTo(expectedMeasures);
	  }
	}

}