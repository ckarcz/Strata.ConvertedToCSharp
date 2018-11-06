/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.minimization
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// 
	public abstract class MinimizationTestFunctions
	{
	  public static readonly System.Func<DoubleArray, double> ROSENBROCK = (DoubleArray x) =>
	  {

  return FunctionUtils.square(1 - x.get(0)) + 100 * FunctionUtils.square(x.get(1) - FunctionUtils.square(x.get(0)));
	  };

	  public static readonly System.Func<DoubleArray, DoubleArray> ROSENBROCK_GRAD = (DoubleArray x) =>
	  {
	  return DoubleArray.of(2 * (x.get(0) - 1) + 400 * x.get(0) * (FunctionUtils.square(x.get(0)) - x.get(1)), 200 * (x.get(1) - FunctionUtils.square(x.get(0))));
	  };

	  public static readonly System.Func<DoubleArray, double> UNCOUPLED_ROSENBROCK = (final DoubleArray x) =>
	  {

  int n = x.size();
  if (n % 2 != 0)
  {
	throw new System.ArgumentException("vector length must be even");
  }
  double sum = 0;
  for (int i = 0; i < n / 2; i++)
  {
	sum += FunctionUtils.square(1 - x.get(2 * i)) + 100 * FunctionUtils.square(x.get(2 * i + 1) - FunctionUtils.square(x.get(2 * i)));
  }
  return sum;
	  };

	  public static readonly System.Func<DoubleArray, double> COUPLED_ROSENBROCK = (DoubleArray x) =>
	  {

  int n = x.size();

  double sum = 0;
  for (int i = 0; i < n - 1; i++)
  {
	sum += FunctionUtils.square(1 - x.get(i)) + 100 * FunctionUtils.square(x.get(i + 1) - FunctionUtils.square(x.get(i)));
  }
  return sum;
	  };

	  public static readonly System.Func<DoubleArray, DoubleArray> COUPLED_ROSENBROCK_GRAD = (DoubleArray x) =>
	  {

  int n = x.size();

  double[] res = new double[n];
  res[0] = 2 * (x.get(0) - 1) + 400 * x.get(0) * (FunctionUtils.square(x.get(0)) - x.get(1));
  res[n - 1] = 200 * (x.get(n - 1) - FunctionUtils.square(x.get(n - 2)));
  for (int i = 1; i < n - 1; i++)
  {
	res[i] = 2 * (x.get(i) - 1) + 400 * x.get(i) * (FunctionUtils.square(x.get(i)) - x.get(i + 1)) + 200 * (x.get(i) - FunctionUtils.square(x.get(i - 1)));
  }
	  return DoubleArray.copyOf(res);
	  };
	}

}