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
	/// Interface for extrapolators which extrapolate beyond the ends of a curve.
	/// </summary>
	public interface CurveExtrapolator : Named
	{

	  /// <summary>
	  /// Obtains an instance from the specified unique name.
	  /// </summary>
	  /// <param name="uniqueName">  the unique name </param>
	  /// <returns> the index </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static CurveExtrapolator of(String uniqueName)
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static CurveExtrapolator of(String uniqueName)
	//  {
	//	ArgChecker.notNull(uniqueName, "uniqueName");
	//	return extendedEnum().lookup(uniqueName);
	//  }

	  /// <summary>
	  /// Gets the extended enum helper.
	  /// <para>
	  /// This helper allows instances of the extrapolator to be looked up.
	  /// It also provides the complete set of available instances.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the extended enum helper </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static com.opengamma.strata.collect.named.ExtendedEnum<CurveExtrapolator> extendedEnum()
	//  {
	//	return CurveExtrapolators.ENUM_LOOKUP;
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Binds this extrapolator to a curve.
	  /// <para>
	  /// The bind process takes the definition of the extrapolator and combines it with the x-y values.
	  /// This allows implementations to optimize extrapolation calculations.
	  /// </para>
	  /// <para>
	  /// This method is intended to be called from within
	  /// <seealso cref="CurveInterpolator#bind(DoubleArray, DoubleArray, CurveExtrapolator, CurveExtrapolator)"/>.
	  /// Callers should ensure that the interpolator instance passed in fully constructed.
	  /// For example, it is incorrect to call this method from a <seealso cref="BoundCurveInterpolator"/> constructor.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="xValues">  the x-values of the curve, must be sorted from low to high </param>
	  /// <param name="yValues">  the y-values of the curve </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <returns> the bound extrapolator </returns>
	  BoundCurveExtrapolator bind(DoubleArray xValues, DoubleArray yValues, BoundCurveInterpolator interpolator);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name that uniquely identifies this extrapolator.
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