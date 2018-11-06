using System;

/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Weighting function based on {@code Math.sin}.
	/// </summary>
	internal sealed class SineWeightingFunction : WeightingFunction
	{

	  internal static readonly SineWeightingFunction INSTANCE = new SineWeightingFunction();

	  private SineWeightingFunction()
	  {
	  }

	  public double getWeight(double y)
	  {
		ArgChecker.inRangeInclusive(y, 0d, 1d, "y");
		return 0.5 * (Math.Sin(Math.PI * (y - 0.5)) + 1);
	  }

	  public string Name
	  {
		  get
		  {
			return "Sine";
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
		return "Sine weighting function";
	  }

	}

}