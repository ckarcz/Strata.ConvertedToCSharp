using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Provides functions that return volatility and its sensitivity to volatility model parameters.
	/// </summary>
	/// @param <T> Type of the data needed for the volatility function </param>
	public abstract class VolatilityFunctionProvider<T> where T : SmileModelData
	{

	  private const double EPS = 1.0e-6;

	  /// <summary>
	  /// Calculates the volatility.
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> the volatility </returns>
	  public abstract double volatility(double forward, double strike, double timeToExpiry, T data);

	  /// <summary>
	  /// Calculates volatility and the adjoint (volatility sensitivity to forward, strike and model parameters). 
	  /// <para>
	  /// By default the derivatives are computed by central finite difference approximation.
	  /// This should be overridden in each subclass.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the model data </param>
	  /// <returns> the volatility and associated derivatives </returns>
	  public virtual ValueDerivatives volatilityAdjoint(double forward, double strike, double timeToExpiry, T data)
	  {
		ArgChecker.isTrue(forward >= 0.0, "forward must be greater than zero");

		double[] res = new double[2 + data.NumberOfParameters]; // fwd, strike, the model parameters
		double volatility = this.volatility(forward, strike, timeToExpiry, data);
		res[0] = forwardBar(forward, strike, timeToExpiry, data);
		res[1] = strikeBar(forward, strike, timeToExpiry, data);
		System.Func<T, double> func = getVolatilityFunction(forward, strike, timeToExpiry);
		double[] modelAdjoint = paramBar(func, data);
		Array.Copy(modelAdjoint, 0, res, 2, data.NumberOfParameters);
		return ValueDerivatives.of(volatility, DoubleArray.ofUnsafe(res));
	  }

	  /// <summary>
	  /// Computes the first and second order derivatives of the volatility.
	  /// <para>
	  /// The first derivative values will be stored in the input array {@code volatilityD} 
	  /// The array contains, [0] Derivative w.r.t the forward, [1] the derivative w.r.t the strike, then followed by model 
	  /// parameters.
	  /// Thus the length of the array should be 2 + (number of model parameters).  
	  /// </para>
	  /// <para>
	  /// The second derivative values will be stored in the input array {@code volatilityD2}. 
	  /// Only the second order derivative with respect to the forward and strike must be implemented.
	  /// The array contains [0][0] forward-forward; [0][1] forward-strike; [1][1] strike-strike.
	  /// Thus the size should be 2 x 2.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strike">  the strike value of the option </param>
	  /// <param name="timeToExpiry">  the time to expiry of the option </param>
	  /// <param name="data">  the model data </param>
	  /// <param name="volatilityD">  the array used to return the first order derivative </param>
	  /// <param name="volatilityD2">  the array of array used to return the second order derivative </param>
	  /// <returns> the volatility </returns>
	  public abstract double volatilityAdjoint2(double forward, double strike, double timeToExpiry, T data, double[] volatilityD, double[][] volatilityD2);

	  //-------------------------------------------------------------------------
	  private double forwardBar(double forward, double strike, double timeToExpiry, T data)
	  {
		double volUp = volatility(forward + EPS, strike, timeToExpiry, data);
		double volDown = volatility(forward - EPS, strike, timeToExpiry, data);
		return 0.5 * (volUp - volDown) / EPS;
	  }

	  private double strikeBar(double forward, double strike, double timeToExpiry, T data)
	  {
		double volUp = volatility(forward, strike + EPS, timeToExpiry, data);
		double volDown = volatility(forward, strike - EPS, timeToExpiry, data);
		return 0.5 * (volUp - volDown) / EPS;
	  }

	  private System.Func<T, double> getVolatilityFunction(double forward, double strike, double timeToExpiry)
	  {
		return (T data) =>
		{
	ArgChecker.notNull(data, "data");
	return volatility(forward, strike, timeToExpiry, data);
		};
	  }

	  private double[] paramBar(System.Func<T, double> func, T data)
	  {
		int n = data.NumberOfParameters;
		double[] res = new double[n];
		for (int i = 0; i < n; i++)
		{
		  res[i] = paramBar(func, data, i);
		}
		return res;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private double paramBar(System.Func<T, double> func, T data, int index)
	  private double paramBar(System.Func<T, double> func, T data, int index)
	  {
		double mid = data.getParameter(index);
		double up = mid + EPS;
		double down = mid - EPS;
		if (data.isAllowed(index, down))
		{
		  if (data.isAllowed(index, up))
		  {
			T dUp = (T) data.with(index, up);
			T dDown = (T) data.with(index, down);
			return 0.5 * (func(dUp) - func(dDown)) / EPS;
		  }
		  T dDown = (T) data.with(index, down);
		  return (func(data) - func(dDown)) / EPS;
		}
		ArgChecker.isTrue(data.isAllowed(index, up), "No values and index {} = {} are allowed", index, mid);
		T dUp = (T) data.with(index, up);
		return (func(dUp) - func(data)) / EPS;
	  }

	}

}