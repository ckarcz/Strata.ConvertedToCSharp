/*
 * Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.interpolation
{
	using FromString = org.joda.convert.FromString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// A function to allow a smooth weighing between two functions.
	/// <para>
	/// If two functions f(x) and g(x) fit the data set (x_i,y_i) at the points x_a and x_b
	/// (i.e. f(x_a) = g(x_a) = y_a and  f(x_b) = g(x_b) = y_b), then a weighted function
	/// h(x) = w(x)f(x) + (1-w(x))*g(x) with 0 <= w(x) <= 1 will also fit the points a and b
	/// </para>
	/// </summary>
	public interface WeightingFunction : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static WeightingFunction of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static WeightingFunction of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	if (uniqueName.equals(LinearWeightingFunction.INSTANCE.getName()))
	//	{
	//	  return LinearWeightingFunction.INSTANCE;
	//	}
	//	if (uniqueName.equals(SineWeightingFunction.INSTANCE.getName()))
	//	{
	//	  return SineWeightingFunction.INSTANCE;
	//	}
	//	throw new IllegalArgumentException("WeightingFunction name not found: " + uniqueName);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the function weight for point x, based on the lower bound index.
	  /// </summary>
	  /// <param name="xs">  the independent data points </param>
	  /// <param name="index">  the index of the data point below x </param>
	  /// <param name="x">  the x-point to find the weight for </param>
	  /// <returns> the weight </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default double getWeight(double[] xs, int index, double x)
	//  {
	//	ArgChecker.notNull(xs, "strikes");
	//	ArgChecker.notNegative(index, "index");
	//	ArgChecker.isTrue(index <= xs.length - 2, "index cannot be larger than {}, have {}", xs.length - 2, index);
	//	double y = (xs[index + 1] - x) / (xs[index + 1] - xs[index]);
	//	return getWeight(y);
	//  }

	  /// <summary>
	  /// Gets the weight.
	  /// <para>
	  /// The condition that must be satisfied by all weight functions is that
	  /// w(1) = 1, w(0) = 0 and dw(y)/dy <= 0 - i.e. w(y) is monotonically decreasing.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="y">  a value between 0 and 1 </param>
	  /// <returns> the weight </returns>
	  double getWeight(double y);

	}

}