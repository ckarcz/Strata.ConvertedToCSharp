/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.function.special
{

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// Class representing the top-hat function, defined as:
	/// $$
	/// \begin{align*}
	/// T(x)=
	/// \begin{cases}
	/// 0 & x < x_1\\
	/// y & x_1 < x < x_2\\
	/// 0 & x > x_2
	/// \end{cases}
	/// \end{align*}
	/// $$
	/// where $x_1$ is the lower edge of the "hat", $x_2$ is the upper edge and $y$
	/// is the height of the function.
	/// 
	/// This function is discontinuous at $x_1$ and $x_2$.
	/// </summary>
	public class TopHatFunction : System.Func<double, double>
	{

	  private readonly double _x1;
	  private readonly double _x2;
	  private readonly double _y;

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  /// <param name="x1">  the lower edge </param>
	  /// <param name="x2">  the upper edge, must be greater than x1 </param>
	  /// <param name="y">  the height  </param>
	  public TopHatFunction(double x1, double x2, double y)
	  {
		ArgChecker.isTrue(x1 < x2, "x1 must be less than x2");
		_x1 = x1;
		_x2 = x2;
		_y = y;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Evaluates the function.
	  /// </summary>
	  /// <param name="x"> The argument of the function, not null. Must have $x_1 < x < x_2$ </param>
	  /// <returns> The value of the function </returns>
	  public override double? apply(double? x)
	  {
		ArgChecker.notNull(x, "x");
		ArgChecker.isTrue(x.Value != _x1, "Function is undefined for x = x1");
		ArgChecker.isTrue(x.Value != _x2, "Function is undefined for x = x2");
		if (x.Value > _x1 && x.Value < _x2)
		{
		  return _y;
		}
		return 0.0;
	  }

	}

}