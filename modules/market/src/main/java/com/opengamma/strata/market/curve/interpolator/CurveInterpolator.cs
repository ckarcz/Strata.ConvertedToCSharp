/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.curve.interpolator
{
	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ExtendedEnum = com.opengamma.strata.collect.named.ExtendedEnum;
	using Named = com.opengamma.strata.collect.named.Named;

	/// <summary>
	/// Interface for interpolators that interpolate between points on a curve.
	/// </summary>
	public interface CurveInterpolator : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CurveInterpolator of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CurveInterpolator of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the interpolator to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<CurveInterpolator> extendedEnum()
	//  {
	//	return CurveInterpolators.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Binds this interpolator to a curve where no extrapolation is permitted.
	  /// <para>
	  /// The bound interpolator provides methods to interpolate the y-value for a x-value.
	  /// If an attempt is made to interpolate an x-value outside the range defined by
	  /// the first and last nodes, an exception will be thrown.
	  /// </para>
	  /// <para>
	  /// The bind process takes the definition of the interpolator and combines it with the x-y values.
	  /// This allows implementations to optimize interpolation calculations.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValues">  the x-values of the curve, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values of the curve </param>
	  /// <returns> the bound interpolator </returns>
	  BoundCurveInterpolator bind(DoubleArray xValues, DoubleArray yValues);

	  /// <summary>
	  /// Binds this interpolator to a curve specifying the extrapolators to use.
	  /// <para>
	  /// The bound interpolator provides methods to interpolate the y-value for a x-value.
	  /// If an attempt is made to interpolate an x-value outside the range defined by
	  /// the first and last nodes, the appropriate extrapolator will be used.
	  /// </para>
	  /// <para>
	  /// The bind process takes the definition of the interpolator and combines it with the x-y values.
	  /// This allows implementations to optimize interpolation calculations.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValues">  the x-values of the curve, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values of the curve </param>
	  /// <param name="extrapolatorLeft">  the extrapolator for x-values on the left </param>
	  /// <param name="extrapolatorRight">  the extrapolator for x-values on the right </param>
	  /// <returns> the bound interpolator </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default BoundCurveInterpolator bind(com.opengamma.strata.collect.array.DoubleArray xValues, com.opengamma.strata.collect.array.DoubleArray yValues, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight)
	//  {
	//
	//	// interpolators depend on extrapolators and vice versa
	//	// this makes it hard to satisfy the Java Memory Model for immutability
	//	// handle this by creating an interpolator instance that cannot extrapolate
	//	// use that interpolator to bind the extrapolators
	//	// finally, create the bound interpolator for the caller
	//	BoundCurveInterpolator interpolatorOnly = bind(xValues, yValues);
	//	BoundCurveExtrapolator boundLeft = extrapolatorLeft.bind(xValues, yValues, interpolatorOnly);
	//	BoundCurveExtrapolator boundRight = extrapolatorRight.bind(xValues, yValues, interpolatorOnly);
	//	return interpolatorOnly.bind(boundLeft, boundRight);
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this interpolator.
	  /// <para>
	  /// This name is used in serialization and can be parsed using <seealso cref="#of(String)"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the unique name </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public abstract String getName();
	  string Name {get;}

	}

}