using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.runner
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;


	using Test = org.testng.annotations.Test;

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using Result = com.opengamma.strata.collect.result.Result;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CompositeCalculationFunctionsTest
	public class CompositeCalculationFunctionsTest
	{

	  public virtual void compose()
	  {
		Fn1 fn1a = new Fn1();
		Fn1 fn1b = new Fn1();
		Fn2 fn2 = new Fn2();

		CalculationFunctions fns1 = CalculationFunctions.of(fn1a);
		CalculationFunctions fns2 = CalculationFunctions.of(fn1b, fn2);
		CalculationFunctions composed = fns1.composedWith(fns2);

		assertEquals(composed.getFunction(new Target1()), fn1a);
		assertEquals(composed.getFunction(new Target2()), fn2);
	  }

	  //-------------------------------------------------------------------------
	  private sealed class Fn1 : CalculationFunction<Target1>
	  {

		public Type<Target1> targetType()
		{
		  return typeof(Target1);
		}

		public ISet<Measure> supportedMeasures()
		{
		  throw new System.NotSupportedException("supportedMeasures not implemented");
		}

		public Currency naturalCurrency(Target1 target, ReferenceData refData)
		{
		  throw new System.NotSupportedException("naturalCurrency not implemented");
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(Target1 target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(Target1 target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  throw new System.NotSupportedException("calculate not implemented");
		}

		public FunctionRequirements requirements(Target1 target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  throw new System.NotSupportedException("requirements not implemented");
		}
	  }

	  private sealed class Fn2 : CalculationFunction<Target2>
	  {

		public Type<Target2> targetType()
		{
		  return typeof(Target2);
		}

		public ISet<Measure> supportedMeasures()
		{
		  throw new System.NotSupportedException("supportedMeasures not implemented");
		}

		public Currency naturalCurrency(Target2 target, ReferenceData refData)
		{
		  throw new System.NotSupportedException("naturalCurrency not implemented");
		}

		public FunctionRequirements requirements(Target2 target, ISet<Measure> measures, CalculationParameters parameters, ReferenceData refData)
		{

		  throw new System.NotSupportedException("requirements not implemented");
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<com.opengamma.strata.calc.Measure, com.opengamma.strata.collect.result.Result<?>> calculate(Target2 target, java.util.Set<com.opengamma.strata.calc.Measure> measures, CalculationParameters parameters, com.opengamma.strata.data.scenario.ScenarioMarketData marketData, com.opengamma.strata.basics.ReferenceData refData)
		public IDictionary<Measure, Result<object>> calculate(Target2 target, ISet<Measure> measures, CalculationParameters parameters, ScenarioMarketData marketData, ReferenceData refData)
		{

		  throw new System.NotSupportedException("calculate not implemented");
		}
	  }

	  private sealed class Target1 : CalculationTarget
	  {

	  }

	  private sealed class Target2 : CalculationTarget
	  {

	  }
	}

}