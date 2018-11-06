/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{
	/// <summary>
	/// Provides a null implementation of parameter transformation; the functions return unchanged values.
	/// </summary>
	public class NullTransform : ParameterLimitsTransform
	{

	  /// <summary>
	  /// Performs the null inverse transform {y -> y}.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransform(double y)
	  {
		return y;
	  }

	  /// <summary>
	  /// The gradient of a null transform is one.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double inverseTransformGradient(double y)
	  {
		return 1;
	  }

	  /// <summary>
	  /// Performs the null transform {x -> x}.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double transform(double x)
	  {
		return x;
	  }

	  /// <summary>
	  /// The gradient of a null transform is one.
	  /// {@inheritDoc}
	  /// </summary>
	  public virtual double transformGradient(double x)
	  {
		return 1;
	  }

	  public override int GetHashCode()
	  {
		return 37;
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
		return this.GetType() == obj.GetType();
	  }

	}

}