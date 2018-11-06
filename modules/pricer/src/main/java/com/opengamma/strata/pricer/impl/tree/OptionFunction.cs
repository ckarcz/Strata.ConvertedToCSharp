/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.tree
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;

	/// <summary>
	/// Option function interface used in trinomial tree option pricing.
	/// </summary>
	public interface OptionFunction
	{

	  /// <summary>
	  /// Obtains time to expiry.
	  /// </summary>
	  /// <returns> time to expiry </returns>
	  double TimeToExpiry {get;}

	  /// <summary>
	  /// Obtains number of time steps.
	  /// </summary>
	  /// <returns> number of time steps </returns>
	  int NumberOfSteps {get;}

	  /// <summary>
	  /// Computes payoff at expiry for trinomial tree.
	  /// <para>
	  /// The payoff values for individual nodes at expiry are computed.
	  /// If trinomial tree has {@code n} steps, the returned {@code DoubleArray} has the size {@code 2 * n + 1}.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="spot">  the spot </param>
	  /// <param name="downFactor">  the down factor </param>
	  /// <param name="middleFactor">  the middle factor </param>
	  /// <returns> the payoff at expiry </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.array.DoubleArray getPayoffAtExpiryTrinomial(double spot, double downFactor, double middleFactor)
	//  {
	//
	//	int nNodes = 2 * getNumberOfSteps() + 1;
	//	double[] values = new double[nNodes];
	//	for (int i = 0; i < nNodes; ++i)
	//	{
	//	  values[i] = spot * Math.pow(downFactor, getNumberOfSteps() - i) * Math.pow(middleFactor, i);
	//	}
	//	return getPayoffAtExpiryTrinomial(DoubleArray.ofUnsafe(values));
	//  }

	  /// <summary>
	  /// Computes payoff at expiry for trinomial tree.
	  /// <para>
	  /// The payoff values for individual nodes at expiry are computed from state values at the final layer.
	  /// For example, the state values represent underlying prices of an option at respective nodes.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="stateValue">  the state values </param>
	  /// <returns> the payoff at expiry </returns>
	  DoubleArray getPayoffAtExpiryTrinomial(DoubleArray stateValue);

	  /// <summary>
	  /// Computes the option values in the intermediate nodes.
	  /// <para>
	  /// Given a set of option values in the (i+1)-th layer, option values in the i-th layer are derived.
	  /// For an option with path-dependence, <seealso cref="#getNextOptionValues(double, DoubleMatrix, DoubleArray, DoubleArray, int)"/> 
	  /// should be overridden rather than this method.
	  /// </para>
	  /// <para>
	  /// The size of {@code values} must be (2*i+3). However, this is not checked because of its repeated usage.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountFactor">  the discount factor between the two layers </param>
	  /// <param name="upProbability">  the up probability </param>
	  /// <param name="middleProbability">  the middle probability </param>
	  /// <param name="downProbability">  the down probability </param>
	  /// <param name="value">  the option values in the (i+1)-th layer </param>
	  /// <param name="spot">  the spot </param>
	  /// <param name="downFactor">  the down factor </param>
	  /// <param name="middleFactor">  the middle factor </param>
	  /// <param name="i">  the step number for which the next option values are computed </param>
	  /// <returns> the option values in the i-th layer </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.array.DoubleArray getNextOptionValues(double discountFactor, double upProbability, double middleProbability, double downProbability, com.opengamma.strata.collect.array.DoubleArray value, double spot, double downFactor, double middleFactor, int i)
	//  {
	//
	//	int nNodes = 2 * i + 1;
	//	double[] probsAtNode = new double[] {downProbability, middleProbability, upProbability};
	//	double[][] probs = new double[nNodes][];
	//	Arrays.fill(probs, probsAtNode);
	//	DoubleArray stateValue = DoubleArray.of(nNodes, k -> spot * Math.pow(downFactor, i - k) * Math.pow(middleFactor, k));
	//	return getNextOptionValues(discountFactor, DoubleMatrix.ofUnsafe(probs), stateValue, value, i);
	//  }

	  /// <summary>
	  /// Computes the option values in the intermediate nodes.
	  /// <para>
	  /// Given a set of option values in the (i+1)-th layer, option values in the i-th layer are derived.
	  /// The down, middle and up probabilities of the j-th lowest node are stored in the {i,0}, {i,1}, {i,2} components of  
	  /// {@code transitionProbability}, respectively.
	  /// </para>
	  /// <para>
	  /// For an option with path-dependence, this method should be overridden.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="discountFactor">  the discount factor between the two layers </param>
	  /// <param name="transitionProbability">  the transition probability </param>
	  /// <param name="stateValue">  the state value </param>
	  /// <param name="value">  the option value </param>
	  /// <param name="i">  the step number for which the next option values are computed </param>
	  /// <returns> the option values in the i-th layer </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default com.opengamma.strata.collect.array.DoubleArray getNextOptionValues(double discountFactor, com.opengamma.strata.collect.array.DoubleMatrix transitionProbability, com.opengamma.strata.collect.array.DoubleArray stateValue, com.opengamma.strata.collect.array.DoubleArray value, int i)
	//  {
	//
	//	int nNodes = 2 * i + 1;
	//	return DoubleArray.of(nNodes, j -> discountFactor * (transitionProbability.get(j, 2) * value.get(j + 2) + transitionProbability.get(j, 1) * value.get(j + 1) + transitionProbability.get(j, 0) * value.get(j)));
	//  }

	}

}