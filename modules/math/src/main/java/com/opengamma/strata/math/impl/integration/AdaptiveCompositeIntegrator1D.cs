using System;

/*
 * Copyright (C) 2014 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Adaptive composite integrator: step size is set to be small if functional variation of integrand is large
	/// The integrator in individual intervals (base integrator) should be specified by constructor.
	/// </summary>
	public class AdaptiveCompositeIntegrator1D : Integrator1D<double, double>
	{
	  private static readonly Logger log = LoggerFactory.getLogger(typeof(AdaptiveCompositeIntegrator1D));
	  private readonly Integrator1D<double, double> integrator;
	  private const int MAX_IT = 15;
	  private readonly double gain;
	  private readonly double tol;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="integrator"> The base integrator  </param>
	  public AdaptiveCompositeIntegrator1D(Integrator1D<double, double> integrator)
	  {
		ArgChecker.notNull(integrator, "integrator");
		this.integrator = integrator;
		this.gain = 15.0;
		this.tol = 1.e-13;
	  }

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="integrator"> The base integrator. </param>
	  /// <param name="gain"> The gain ratio </param>
	  /// <param name="tol"> The tolerance </param>
	  public AdaptiveCompositeIntegrator1D(Integrator1D<double, double> integrator, double gain, double tol)
	  {
		ArgChecker.notNull(integrator, "integrator");
		this.integrator = integrator;
		this.gain = gain;
		this.tol = tol;
	  }

	  public virtual double? integrate(System.Func<double, double> f, double? lower, double? upper)
	  {
		ArgChecker.notNull(f, "f");
		ArgChecker.notNull(lower, "lower bound");
		ArgChecker.notNull(upper, "upper bound");
		try
		{
		  if (lower < upper)
		  {
			return integration(f, lower, upper);
		  }
		  log.info("Upper bound was less than lower bound; swapping bounds and negating result");
		  return -integration(f, upper, lower);
		}
		catch (Exception)
		{
		  throw new System.InvalidOperationException("function evaluation returned NaN or Inf");
		}
	  }

	  private double? integration(System.Func<double, double> f, double? lower, double? upper)
	  {
		double res = integrator.integrate(f, lower, upper);
		return integrationRec(f, lower.Value, upper.Value, res, MAX_IT);
	  }

	  private double integrationRec(System.Func<double, double> f, double lower, double upper, double res, double counter)
	  {
		double localTol = gain * tol;
		double half = 0.5 * (lower + upper);
		double newResDw = integrator.integrate(f, lower, half);
		double newResUp = integrator.integrate(f, half, upper);
		double newRes = newResUp + newResDw;

		if (Math.Abs(res - newRes) < localTol || counter == 0 || (Math.Abs(res) < 1.e-14 && Math.Abs(newResUp) < 1.e-14 && Math.Abs(newResDw) < 1.e-14))
		{
		  return newRes + (newRes - res) / gain;
		}

		return integrationRec(f, lower, half, newResDw, counter - 1) + integrationRec(f, half, upper, newResUp, counter - 1);
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(gain);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		result = prime * result + integrator.GetHashCode();
		temp = System.BitConverter.DoubleToInt64Bits(tol);
		result = prime * result + (int)(temp ^ ((long)((ulong)temp >> 32)));
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (!(obj is AdaptiveCompositeIntegrator1D))
		{
		  return false;
		}
		AdaptiveCompositeIntegrator1D other = (AdaptiveCompositeIntegrator1D) obj;
		if (System.BitConverter.DoubleToInt64Bits(this.gain) != Double.doubleToLongBits(other.gain))
		{
		  return false;
		}
		if (!this.integrator.Equals(other.integrator))
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(this.tol) != Double.doubleToLongBits(other.tol))
		{
		  return false;
		}
		return true;
	  }

	}

}