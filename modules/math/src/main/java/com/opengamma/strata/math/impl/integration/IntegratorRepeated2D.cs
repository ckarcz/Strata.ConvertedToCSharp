/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	/// <summary>
	/// Two dimensional integration by repeated one dimensional integration using <seealso cref="Integrator1D"/>.
	/// </summary>
	public class IntegratorRepeated2D : Integrator2D<double, double>
	{

	  /// <summary>
	  /// The 1-D integrator to be used for each repeated integral.
	  /// </summary>
	  private readonly Integrator1D<double, double> _integrator1D;

	  /// <summary>
	  /// Constructor.
	  /// </summary>
	  /// <param name="integrator1D">  the 1-D integrator to be used for each repeated integral </param>
	  public IntegratorRepeated2D(Integrator1D<double, double> integrator1D)
	  {
		_integrator1D = integrator1D;
	  }

	  //-------------------------------------------------------------------------
	  public virtual double? integrate(System.Func<double, double, double> f, Double[] lower, Double[] upper)
	  {
		return _integrator1D.integrate(innerIntegral(f, lower[0], upper[0]), lower[1], upper[1]);
	  }

	  /// <summary>
	  /// The inner integral function of the repeated 1-D integrations.
	  /// For a given $y$ it returns $\int_{x_1}^{x_2} f(x,y) dx$.
	  /// </summary>
	  /// <param name="f">  the bi-function </param>
	  /// <param name="lower">  the lower bound (for the inner-first variable) </param>
	  /// <param name="upper">  the upper bound (for the inner-first variable) </param>
	  /// <returns> the inner integral function </returns>
	  private System.Func<double, double> innerIntegral(System.Func<double, double, double> f, double? lower, double? upper)
	  {
		return y => _integrator1D.integrate(x => f(x, y), lower, upper);
	  }

	}

}