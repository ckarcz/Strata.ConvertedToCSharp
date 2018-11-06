using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{
	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using RecombiningTrinomialTreeData = com.opengamma.strata.pricer.fxopt.RecombiningTrinomialTreeData;

	/// <summary>
	/// Trinomial tree.
	/// <para>
	/// Option pricing model based on trinomial tree. Trinomial lattice is defined by {@code LatticeSpecification} 
	/// and the option to price is specified by {@code OptionFunction}. 
	/// </para>
	/// <para>
	/// Option pricing with non-uniform tree is realised by specifying {@code RecombiningTrinomialTreeData}.
	/// </para>
	/// </summary>
	public class TrinomialTree
	{

	  /// <summary>
	  /// Price an option under the specified trinomial lattice.
	  /// <para>
	  /// It is assumed that the volatility, interest rate and continuous dividend rate are constant 
	  /// over the lifetime of the option.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="function">  the option </param>
	  /// <param name="lattice">  the lattice specification </param>
	  /// <param name="spot">  the spot </param>
	  /// <param name="volatility">  the volatility </param>
	  /// <param name="interestRate">  the interest rate </param>
	  /// <param name="dividendRate">  the dividend rate </param>
	  /// <returns> the option price </returns>
	  public virtual double optionPrice(OptionFunction function, LatticeSpecification lattice, double spot, double volatility, double interestRate, double dividendRate)
	  {

		int nSteps = function.NumberOfSteps;
		double timeToExpiry = function.TimeToExpiry;
		double dt = timeToExpiry / (double) nSteps;
		double discount = Math.Exp(-interestRate * dt);
		DoubleArray @params = lattice.getParametersTrinomial(volatility, interestRate - dividendRate, dt);
		double middleFactor = @params.get(1);
		double downFactor = @params.get(2);
		double upProbability = @params.get(3);
		double midProbability = @params.get(4);
		double downProbability = @params.get(5);
		ArgChecker.isTrue(upProbability > 0d, "upProbability should be greater than 0");
		ArgChecker.isTrue(upProbability < 1d, "upProbability should be smaller than 1");
		ArgChecker.isTrue(midProbability > 0d, "midProbability should be greater than 0");
		ArgChecker.isTrue(midProbability < 1d, "midProbability should be smaller than 1");
		ArgChecker.isTrue(downProbability > 0d, "downProbability should be greater than 0");
		DoubleArray values = function.getPayoffAtExpiryTrinomial(spot, downFactor, middleFactor);
		for (int i = nSteps - 1; i > -1; --i)
		{
		  values = function.getNextOptionValues(discount, upProbability, midProbability, downProbability, values, spot, downFactor, middleFactor, i);
		}
		return values.get(0);
	  }

	  /// <summary>
	  /// Price an option under the specified trinomial tree gird.
	  /// </summary>
	  /// <param name="function">  the option </param>
	  /// <param name="data">  the trinomial tree data </param>
	  /// <returns> the option price </returns>
	  public virtual double optionPrice(OptionFunction function, RecombiningTrinomialTreeData data)
	  {

		int nSteps = data.NumberOfSteps;
		ArgChecker.isTrue(nSteps == function.NumberOfSteps, "mismatch in number of steps");
		DoubleArray values = function.getPayoffAtExpiryTrinomial(data.getStateValueAtLayer(nSteps));
		for (int i = nSteps - 1; i > -1; --i)
		{
		  values = function.getNextOptionValues(data.getDiscountFactorAtLayer(i), data.getProbabilityAtLayer(i), data.getStateValueAtLayer(i), values, i);
		}
		return values.get(0);
	  }

	  /// <summary>
	  /// Compute option price and delta under the specified trinomial tree gird.
	  /// <para>
	  /// The delta is the first derivative of the price with respect to spot, and approximated by the data embedded in 
	  /// the trinomial tree.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="function">  the option </param>
	  /// <param name="data">  the trinomial tree data </param>
	  /// <returns> the option price and spot delta </returns>
	  public virtual ValueDerivatives optionPriceAdjoint(OptionFunction function, RecombiningTrinomialTreeData data)
	  {

		int nSteps = data.NumberOfSteps;
		ArgChecker.isTrue(nSteps == function.NumberOfSteps, "mismatch in number of steps");
		DoubleArray values = function.getPayoffAtExpiryTrinomial(data.getStateValueAtLayer(nSteps));
		double delta = 0d;
		for (int i = nSteps - 1; i > -1; --i)
		{
		  values = function.getNextOptionValues(data.getDiscountFactorAtLayer(i), data.getProbabilityAtLayer(i), data.getStateValueAtLayer(i), values, i);
		  if (i == 1)
		  {
			DoubleArray stateValue = data.getStateValueAtLayer(1);
			double d1 = (values.get(2) - values.get(1)) / (stateValue.get(2) - stateValue.get(1));
			double d2 = (values.get(1) - values.get(0)) / (stateValue.get(1) - stateValue.get(0));
			delta = 0.5 * (d1 + d2);
		  }
		}
		return ValueDerivatives.of(values.get(0), DoubleArray.of(delta));
	  }

	}

}