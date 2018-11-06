/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using FxRateProvider = com.opengamma.strata.basics.currency.FxRateProvider;

	/// <summary>
	/// Testing implementation of <seealso cref="ScenarioFxRateProvider"/>.
	/// </summary>
	public class TestScenarioFxRateProvider : ScenarioFxRateProvider
	{

	  private readonly FxRateScenarioArray rates1;
	  private readonly FxRateScenarioArray rates2;
	  private readonly FxRateScenarioArray rates3;

	  public TestScenarioFxRateProvider(FxRateScenarioArray rates1)
	  {
		this.rates1 = rates1;
		this.rates2 = rates1;
		this.rates3 = rates1;
	  }

	  public TestScenarioFxRateProvider(FxRateScenarioArray rates1, FxRateScenarioArray rates2)
	  {
		this.rates1 = rates1;
		this.rates2 = rates2;
		this.rates3 = rates2;
	  }

	  public TestScenarioFxRateProvider(FxRateScenarioArray rates1, FxRateScenarioArray rates2, FxRateScenarioArray rates3)
	  {
		this.rates1 = rates1;
		this.rates2 = rates2;
		this.rates3 = rates3;
	  }

	  public virtual int ScenarioCount
	  {
		  get
		  {
			return rates1.ScenarioCount;
		  }
	  }

	  public virtual FxRateProvider fxRateProvider(int scenarioIndex)
	  {
		return new FxRateProviderAnonymousInnerClass(this, scenarioIndex);
	  }

	  private class FxRateProviderAnonymousInnerClass : FxRateProvider
	  {
		  private readonly TestScenarioFxRateProvider outerInstance;

		  private int scenarioIndex;

		  public FxRateProviderAnonymousInnerClass(TestScenarioFxRateProvider outerInstance, int scenarioIndex)
		  {
			  this.outerInstance = outerInstance;
			  this.scenarioIndex = scenarioIndex;
		  }


		  public double fxRate(Currency baseCurrency, Currency counterCurrency)
		  {
			if (baseCurrency.Equals(counterCurrency))
			{
			  return 1;
			}
			if (baseCurrency.Equals(outerInstance.rates1.Pair.Base))
			{
			  return outerInstance.rates1.fxRate(baseCurrency, counterCurrency, scenarioIndex);
			}
			else if (baseCurrency.Equals(outerInstance.rates2.Pair.Base))
			{
			  return outerInstance.rates2.fxRate(baseCurrency, counterCurrency, scenarioIndex);
			}
			else
			{
			  return outerInstance.rates3.fxRate(baseCurrency, counterCurrency, scenarioIndex);
			}
		  }
	  }

	}

}