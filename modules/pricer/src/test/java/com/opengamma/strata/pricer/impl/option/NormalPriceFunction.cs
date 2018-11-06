/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Computes the price of an option in the normally distributed assets hypothesis (Bachelier model).
	/// </summary>
	public sealed class NormalPriceFunction
	{
	  // this class has been replaced by NormalFormulaRepository
	  // it is retained for testing purposes

	  /// <summary>
	  /// Gets the price function for the option.
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <returns> the price function </returns>
	  public System.Func<NormalFunctionData, double> getPriceFunction(EuropeanVanillaOption option)
	  {
		ArgChecker.notNull(option, "option");
		return new FuncAnonymousInnerClass(this, option);
	  }

	  private class FuncAnonymousInnerClass : System.Func<NormalFunctionData, double>
	  {
		  private readonly NormalPriceFunction outerInstance;

		  private com.opengamma.strata.pricer.impl.option.EuropeanVanillaOption option;

		  public FuncAnonymousInnerClass(NormalPriceFunction outerInstance, com.opengamma.strata.pricer.impl.option.EuropeanVanillaOption option)
		  {
			  this.outerInstance = outerInstance;
			  this.option = option;
		  }


//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("synthetic-access") @Override public System.Nullable<double> apply(NormalFunctionData data)
		  public override double? apply(NormalFunctionData data)
		  {
			ArgChecker.notNull(data, "data");
			return data.Numeraire * NormalFormulaRepository.price(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, option.PutCall);
		  }
	  }

	  /// <summary>
	  /// Computes the price of an option in the normally distributed assets hypothesis (Bachelier model).
	  /// The first order price derivatives are also provided.
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> a <seealso cref="ValueDerivatives"/> with the price in the value and the derivatives with
	  ///  respect to [0] the forward, [1] the volatility and [2] the strike </returns>
	  public ValueDerivatives getPriceAdjoint(EuropeanVanillaOption option, NormalFunctionData data)
	  {
		ArgChecker.notNull(option, "option");
		ArgChecker.notNull(data, "data");
		return NormalFormulaRepository.priceAdjoint(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, data.Numeraire, option.PutCall);
	  }

	  /// <summary>
	  /// Computes forward delta of an option in the normally distributed assets hypothesis (Bachelier model).
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> delta </returns>
	  public double getDelta(EuropeanVanillaOption option, NormalFunctionData data)
	  {
		ArgChecker.notNull(option, "option");
		ArgChecker.notNull(data, "data");
		return data.Numeraire * NormalFormulaRepository.delta(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, option.PutCall);
	  }

	  /// <summary>
	  /// Computes forward gamma of an option in the normally distributed assets hypothesis (Bachelier model).
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> gamma </returns>
	  public double getGamma(EuropeanVanillaOption option, NormalFunctionData data)
	  {
		ArgChecker.notNull(option, "option");
		ArgChecker.notNull(data, "data");
		return data.Numeraire * NormalFormulaRepository.gamma(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, option.PutCall);
	  }

	  /// <summary>
	  /// Computes vega of an option in the normally distributed assets hypothesis (Bachelier model).
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> vega </returns>
	  public double getVega(EuropeanVanillaOption option, NormalFunctionData data)
	  {
		ArgChecker.notNull(option, "option");
		ArgChecker.notNull(data, "data");
		return data.Numeraire * NormalFormulaRepository.vega(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, option.PutCall);
	  }

	  /// <summary>
	  /// Computes theta of an option in the normally distributed assets hypothesis (Bachelier model).
	  /// </summary>
	  /// <param name="option">  the option description </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> theta </returns>
	  public double getTheta(EuropeanVanillaOption option, NormalFunctionData data)
	  {
		ArgChecker.notNull(option, "option");
		ArgChecker.notNull(data, "data");
		return data.Numeraire * NormalFormulaRepository.theta(data.Forward, option.Strike, option.TimeToExpiry, data.NormalVolatility, option.PutCall);
	  }

	}

}