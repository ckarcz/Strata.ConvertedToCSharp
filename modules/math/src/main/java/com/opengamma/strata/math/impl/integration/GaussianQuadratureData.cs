/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Class holding the results of calculations of weights and abscissas by <seealso cref="QuadratureWeightAndAbscissaFunction"/>. 
	/// </summary>
	public class GaussianQuadratureData
	{

	  private readonly double[] _weights;
	  private readonly double[] _abscissas;

	  /// <param name="abscissas"> An array containing the abscissas, not null </param>
	  /// <param name="weights"> An array containing the weights, not null, must be the same length as the abscissa array </param>
	  public GaussianQuadratureData(double[] abscissas, double[] weights)
	  {
		ArgChecker.notNull(abscissas, "abscissas");
		ArgChecker.notNull(weights, "weights");
		ArgChecker.isTrue(abscissas.Length == weights.Length, "Abscissa and weight arrays must be the same length");
		_weights = weights;
		_abscissas = abscissas;
	  }

	  /// <returns> The weights </returns>
	  public virtual double[] Weights
	  {
		  get
		  {
			return _weights;
		  }
	  }

	  /// <returns> The abscissas </returns>
	  public virtual double[] Abscissas
	  {
		  get
		  {
			return _abscissas;
		  }
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		result = prime * result + Arrays.GetHashCode(_abscissas);
		result = prime * result + Arrays.GetHashCode(_weights);
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
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		GaussianQuadratureData other = (GaussianQuadratureData) obj;
		if (!Arrays.Equals(_abscissas, other._abscissas))
		{
		  return false;
		}
		return Arrays.Equals(_weights, other._weights);
	  }

	}

}