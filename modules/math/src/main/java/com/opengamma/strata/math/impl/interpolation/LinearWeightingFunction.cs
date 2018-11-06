/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Linear weighting function.
	/// </summary>
	internal sealed class LinearWeightingFunction : WeightingFunction
	{

	  internal static readonly LinearWeightingFunction INSTANCE = new LinearWeightingFunction();

	  private LinearWeightingFunction()
	  {
	  }

	  public double getWeight(double y)
	  {
		ArgChecker.inRangeInclusive(y, 0d, 1d, "y");
		return y;
	  }

	  public string Name
	  {
		  get
		  {
			return "Linear";
		  }
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == null)
		{
		  return false;
		}
		if (this == obj)
		{
		  return true;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		return true;
	  }

	  public override int GetHashCode()
	  {
		return ToString().GetHashCode();
	  }

	  public override string ToString()
	  {
		return "Linear weighting function";
	  }

	}

}