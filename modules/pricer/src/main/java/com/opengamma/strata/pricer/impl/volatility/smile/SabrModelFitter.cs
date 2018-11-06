using System.Collections;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.volatility.smile
{

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.DoubleRangeLimitTransform;
	using NonLinearParameterTransforms = com.opengamma.strata.math.impl.minimization.NonLinearParameterTransforms;
	using ParameterLimitsTransform = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform;
	using ParameterLimitsTransform_LimitType = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform_LimitType;
	using SingleRangeLimitTransform = com.opengamma.strata.math.impl.minimization.SingleRangeLimitTransform;
	using UncoupledParameterTransforms = com.opengamma.strata.math.impl.minimization.UncoupledParameterTransforms;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;

	/// <summary>
	/// SABR model fitter.
	/// <para>
	/// Attempts to calibrate SABR model to the implied volatilities of European vanilla options, by minimizing the sum of 
	/// squares between the market and model implied volatilities.
	/// </para>
	/// <para>
	/// All the options must be for the same expiry and (implicitly) on the same underlying.
	/// </para>
	/// </summary>
	public sealed class SabrModelFitter : SmileModelFitter<SabrFormulaData>
	{

	  private const double RHO_LIMIT = 0.999;
	  // Allowing for rho to be equal to 1 or -1 does not make sense from a financial point of view and creates numerical instability
	  private static readonly ParameterLimitsTransform[] DEFAULT_TRANSFORMS;
	  static SabrModelFitter()
	  {
		DEFAULT_TRANSFORMS = new ParameterLimitsTransform[4];
		DEFAULT_TRANSFORMS[0] = new SingleRangeLimitTransform(0, ParameterLimitsTransform_LimitType.GREATER_THAN); // alpha > 0
		DEFAULT_TRANSFORMS[1] = new DoubleRangeLimitTransform(0, 1.0); // 0 <= beta <= 1
		DEFAULT_TRANSFORMS[2] = new DoubleRangeLimitTransform(-RHO_LIMIT, RHO_LIMIT); // -RHO_LIMIT <= rho <= RHO_LIMIT
		DEFAULT_TRANSFORMS[3] = new DoubleRangeLimitTransform(0.01d, 2.50d);
		// nu > 0  and limit on Nu to avoid numerical instability in formula for large nu.
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Constructs SABR model fitter from forward, strikes, time to expiry, implied volatilities and error values.
	  /// <para>
	  /// {@code strikes}, {@code impliedVols} and {@code error} should be the same length and ordered coherently.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strikes">  the ordered values of strikes </param>
	  /// <param name="timeToExpiry">  the time-to-expiry </param>
	  /// <param name="impliedVols">  the market implied volatilities </param>
	  /// <param name="error">  the 'measurement' error to apply to the market volatility of a particular option </param>
	  /// <param name="sabrVolatilityFormula">  the volatility formula </param>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public SabrModelFitter(double forward, com.opengamma.strata.collect.array.DoubleArray strikes, double timeToExpiry, com.opengamma.strata.collect.array.DoubleArray impliedVols, com.opengamma.strata.collect.array.DoubleArray error, com.opengamma.strata.pricer.model.SabrVolatilityFormula sabrVolatilityFormula)
	  public SabrModelFitter(double forward, DoubleArray strikes, double timeToExpiry, DoubleArray impliedVols, DoubleArray error, SabrVolatilityFormula sabrVolatilityFormula) : base(forward, strikes, timeToExpiry, impliedVols, error, (VolatilityFunctionProvider<SabrFormulaData>) sabrVolatilityFormula)
	  {

	  }

	  /// <summary>
	  /// Constructs SABR model fitter from forward, strikes, time to expiry, implied volatilities and error values.
	  /// <para>
	  /// {@code strikes}, {@code impliedVols} and {@code error} should be the same length and ordered coherently.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward value of the underlying </param>
	  /// <param name="strikes">  the ordered values of strikes </param>
	  /// <param name="timeToExpiry">  the time-to-expiry </param>
	  /// <param name="impliedVols">  the market implied volatilities </param>
	  /// <param name="error">  the 'measurement' error to apply to the market volatility of a particular option </param>
	  /// <param name="model">  the volatility function provider </param>
	  public SabrModelFitter(double forward, DoubleArray strikes, double timeToExpiry, DoubleArray impliedVols, DoubleArray error, VolatilityFunctionProvider<SabrFormulaData> model) : base(forward, strikes, timeToExpiry, impliedVols, error, model)
	  {

	  }

	  //-------------------------------------------------------------------------
	  public override SabrFormulaData toSmileModelData(DoubleArray modelParameters)
	  {
		return SabrFormulaData.of(modelParameters.toArray());
	  }

	  protected internal override NonLinearParameterTransforms getTransform(DoubleArray start)
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.BitSet fixed = new java.util.BitSet();
		BitArray @fixed = new BitArray();
		return new UncoupledParameterTransforms(start, DEFAULT_TRANSFORMS, @fixed);
	  }

	  protected internal override NonLinearParameterTransforms getTransform(DoubleArray start, BitArray @fixed)
	  {
		return new UncoupledParameterTransforms(start, DEFAULT_TRANSFORMS, @fixed);
	  }

	  protected internal override DoubleArray MaximumStep
	  {
		  get
		  {
			return null;
		  }
	  }

	}

}