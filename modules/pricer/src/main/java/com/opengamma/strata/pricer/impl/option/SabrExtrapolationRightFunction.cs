using System;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.impl.option
{

	using ValueDerivatives = com.opengamma.strata.basics.value.ValueDerivatives;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using SVDecompositionCommons = com.opengamma.strata.math.impl.linearalgebra.SVDecompositionCommons;
	using BracketRoot = com.opengamma.strata.math.impl.rootfinding.BracketRoot;
	using RidderSingleRootFinder = com.opengamma.strata.math.impl.rootfinding.RidderSingleRootFinder;
	using DecompositionResult = com.opengamma.strata.math.linearalgebra.DecompositionResult;
	using SabrFormulaData = com.opengamma.strata.pricer.impl.volatility.smile.SabrFormulaData;
	using SabrHaganVolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.SabrHaganVolatilityFunctionProvider;
	using VolatilityFunctionProvider = com.opengamma.strata.pricer.impl.volatility.smile.VolatilityFunctionProvider;
	using PutCall = com.opengamma.strata.product.common.PutCall;

	/// <summary>
	/// Pricing function in the SABR model with Hagan et al. volatility function and controlled extrapolation 
	/// for large strikes by extrapolation on call prices.
	/// <para>
	/// The form of the extrapolation as a function of the strike is
	/// \begin{equation*}
	/// f(K) = K^{-\mu} \exp\left( a + \frac{b}{K} + \frac{c}{K^2} \right).
	/// \end{equation*}
	/// <P>
	/// Benaim, S., Dodgson, M., and Kainth, D. (2008). An arbitrage-free method for smile extrapolation.
	/// Technical report, Royal Bank of Scotland.
	/// <P>
	/// OpenGamma implementation note: Smile extrapolation, version 1.2, May 2011.
	/// </para>
	/// </summary>
	public sealed class SabrExtrapolationRightFunction
	{

	  /// <summary>
	  /// Matrix decomposition.
	  /// </summary>
	  private static readonly SVDecompositionCommons SVD = new SVDecompositionCommons();
	  /// <summary>
	  /// Value below which the time-to-expiry is considered to be 0 and the price of the fitting parameters fit a price of 0 (OTM).
	  /// </summary>
	  private const double SMALL_EXPIRY = 1.0E-6;
	  /// <summary>
	  /// If the time-to-expiry is smaller than {@code SMALL_EXPIRY}, the parameter 'a' is set to be this value.
	  /// </summary>
	  private const double SMALL_PARAMETER = -1.0E4;
	  /// <summary>
	  /// Value below which the price is considered to be 0.
	  /// </summary>
	  private const double SMALL_PRICE = 1.0E-15;

	  /// <summary>
	  /// The volatility provider.
	  /// </summary>
	  private readonly VolatilityFunctionProvider<SabrFormulaData> sabrFunction;
	  /// <summary>
	  /// The forward.
	  /// </summary>
	  private readonly double forward;
	  /// <summary>
	  /// The time to option expiry.
	  /// </summary>
	  private readonly double timeToExpiry;
	  /// <summary>
	  /// The SABR parameter for the pricing below the cut-off strike.
	  /// </summary>
	  private readonly SabrFormulaData sabrData;
	  /// <summary>
	  /// The cut-off strike. The smile is extrapolated above that level.
	  /// </summary>
	  private readonly double cutOffStrike;
	  /// <summary>
	  /// The tail thickness parameter.
	  /// </summary>
	  private readonly double mu;
	  /// <summary>
	  /// The three fitting parameters.
	  /// </summary>
	  private readonly double[] parameter;
	  /// <summary>
	  /// The derivative of the three fitting parameters with respect to the forward.
	  /// <para>
	  /// Those parameters are computed only when and if required.
	  /// </para>
	  /// </summary>
	  private volatile double[] parameterDerivativeForward;
	  /// <summary>
	  /// The derivative of the three fitting parameters with respect to the SABR parameters.
	  /// <para>
	  /// Those parameters are computed only when and if required.
	  /// </para>
	  /// </summary>
	  private volatile double[][] parameterDerivativeSabr;
	  /// <summary>
	  /// The Black implied volatility at the cut-off strike.
	  /// </summary>
	  private volatile double volatilityK;
	  /// <summary>
	  /// The price and its derivatives of order 1 and 2 at the cut-off strike.
	  /// </summary>
	  private readonly double[] priceK = new double[3];

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with default volatility provider.
	  /// <para>
	  /// The default volatility provider is <seealso cref="SabrHaganVolatilityFunctionProvider"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward </param>
	  /// <param name="timeToExpiry">  the time to expiration </param>
	  /// <param name="sabrData">  the SABR formula data </param>
	  /// <param name="cutOffStrike">  the cut-off-strike </param>
	  /// <param name="mu">  the mu parameter </param>
	  /// <returns> the instance </returns>
	  public static SabrExtrapolationRightFunction of(double forward, double timeToExpiry, SabrFormulaData sabrData, double cutOffStrike, double mu)
	  {

		return new SabrExtrapolationRightFunction(forward, sabrData, cutOffStrike, timeToExpiry, mu, SabrHaganVolatilityFunctionProvider.DEFAULT);
	  }

	  /// <summary>
	  /// Obtains an instance with volatility provider specified.
	  /// </summary>
	  /// <param name="forward">  the forward </param>
	  /// <param name="sabrData">  the SABR formula data </param>
	  /// <param name="cutOffStrike">  the cut-off-strike </param>
	  /// <param name="timeToExpiry">  the time to expiration </param>
	  /// <param name="mu">  the mu parameter </param>
	  /// <param name="volatilityFunction">  the volatility function </param>
	  /// <returns> the instance </returns>
	  public static SabrExtrapolationRightFunction of(double forward, SabrFormulaData sabrData, double cutOffStrike, double timeToExpiry, double mu, VolatilityFunctionProvider<SabrFormulaData> volatilityFunction)
	  {

		return new SabrExtrapolationRightFunction(forward, sabrData, cutOffStrike, timeToExpiry, mu, volatilityFunction);
	  }

	  // private constructor
	  private SabrExtrapolationRightFunction(double forward, SabrFormulaData sabrData, double cutOffStrike, double timeToExpiry, double mu, VolatilityFunctionProvider<SabrFormulaData> volatilityFunction)
	  {
		ArgChecker.notNull(sabrData, "sabrData");
		ArgChecker.notNull(volatilityFunction, "volatilityFunction");
		this.sabrFunction = volatilityFunction;
		this.forward = forward;
		this.sabrData = sabrData;
		this.cutOffStrike = cutOffStrike;
		this.timeToExpiry = timeToExpiry;
		this.mu = mu;
		if (timeToExpiry > SMALL_EXPIRY)
		{
		  parameter = computesFittingParameters();
		}
		else
		{ // Implementation note: when time to expiry is very small, the price above the cut-off strike and its derivatives should be 0 (or at least very small).
		  parameter = new double[] {SMALL_PARAMETER, 0.0, 0.0};
		  parameterDerivativeForward = new double[3];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: parameterDerivativeSabr = new double[4][3];
		  parameterDerivativeSabr = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Computes the option price with numeraire=1.
	  /// <para>
	  /// The price is SABR below the cut-off strike and extrapolated beyond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="strike">  the strike of the option </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <returns> the option price </returns>
	  public double price(double strike, PutCall putCall)
	  {
		// Uses Hagan et al SABR function.
		if (strike <= cutOffStrike)
		{
		  double volatility = sabrFunction.volatility(forward, strike, timeToExpiry, sabrData);
		  return BlackFormulaRepository.price(forward, strike, timeToExpiry, volatility, putCall.Call);
		}
		// Uses extrapolation for call.
		double price = extrapolation(strike);
		if (putCall.Put)
		{ // Put by call/put parity
		  price -= (forward - strike);
		}
		return price;
	  }

	  /// <summary>
	  /// Computes the option price derivative with respect to the strike.
	  /// <para>
	  /// The price is SABR below the cut-off strike and extrapolated beyond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="strike">  the strike of the option </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <returns> the option price derivative </returns>
	  public double priceDerivativeStrike(double strike, PutCall putCall)
	  {
		// Uses Hagan et al SABR function.
		if (strike <= cutOffStrike)
		{
		  ValueDerivatives volatilityAdjoint = sabrFunction.volatilityAdjoint(forward, strike, timeToExpiry, sabrData);
		  ValueDerivatives bsAdjoint = BlackFormulaRepository.priceAdjoint(forward, strike, timeToExpiry, volatilityAdjoint.Value, putCall.Equals(PutCall.CALL));
		  return bsAdjoint.getDerivative(1) + bsAdjoint.getDerivative(3) * volatilityAdjoint.getDerivative(1);
		}
		// Uses extrapolation for call.
		double pDK = extrapolationDerivative(strike);
		if (putCall.Put)
		{ // Put by call/put parity
		  pDK += 1.0;
		}
		return pDK;
	  }

	  /// <summary>
	  /// Computes the option price derivative with respect to the forward.
	  /// <para>
	  /// The price is SABR below the cut-off strike and extrapolated beyond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="strike">  the strike of the option </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <returns> the option price derivative </returns>
	  public double priceDerivativeForward(double strike, PutCall putCall)
	  {
		// Uses Hagan et al SABR function.
		if (strike <= cutOffStrike)
		{
		  ValueDerivatives volatilityA = sabrFunction.volatilityAdjoint(forward, strike, timeToExpiry, sabrData);
		  ValueDerivatives pA = BlackFormulaRepository.priceAdjoint(forward, strike, timeToExpiry, volatilityA.Value, putCall == PutCall.CALL);
		  return pA.getDerivative(0) + pA.getDerivative(3) * volatilityA.getDerivative(0);
		}
		// Uses extrapolation for call.
		if (parameterDerivativeForward == null)
		{
		  parameterDerivativeForward = computesParametersDerivativeForward();
		}
		double f = extrapolation(strike);
		double fDa = f;
		double fDb = f / strike;
		double fDc = fDb / strike;
		double priceDerivative = fDa * parameterDerivativeForward[0] + fDb * parameterDerivativeForward[1] + fDc * parameterDerivativeForward[2];
		if (putCall.Put)
		{ // Put by call/put parity
		  priceDerivative -= 1;
		}
		return priceDerivative;
	  }

	  /// <summary>
	  /// Computes the option price derivative with respect to the SABR parameters.
	  /// <para>
	  /// The price is SABR below the cut-off strike and extrapolated beyond.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="strike">  the strike of the option </param>
	  /// <param name="putCall">  whether the option is put or call </param>
	  /// <returns> the option and its derivative </returns>
	  public ValueDerivatives priceAdjointSabr(double strike, PutCall putCall)
	  {
		double[] priceDerivativeSabr = new double[4];
		double price;
		if (strike <= cutOffStrike)
		{ // Uses Hagan et al SABR function.
		  ValueDerivatives volatilityA = sabrFunction.volatilityAdjoint(forward, strike, timeToExpiry, sabrData);
		  ValueDerivatives pA = BlackFormulaRepository.priceAdjoint(forward, strike, timeToExpiry, volatilityA.Value, putCall == PutCall.CALL);
		  price = pA.Value;
		  for (int loopparam = 0; loopparam < 4; loopparam++)
		  {
			priceDerivativeSabr[loopparam] = pA.getDerivative(3) * volatilityA.getDerivative(loopparam + 2);
		  }
		}
		else
		{ // Uses extrapolation for call.
		  if (parameterDerivativeSabr == null)
		  {
			parameterDerivativeSabr = computesParametersDerivativeSabr();
			// Derivatives computed only once and only when required
		  }
		  double f = extrapolation(strike);
		  double fDa = f;
		  double fDb = f / strike;
		  double fDc = fDb / strike;
		  price = putCall.Call ? f : f - forward + strike; // Put by call/put parity
		  for (int loopparam = 0; loopparam < 4; loopparam++)
		  {
			priceDerivativeSabr[loopparam] = fDa * parameterDerivativeSabr[loopparam][0] + fDb * parameterDerivativeSabr[loopparam][1] + fDc * parameterDerivativeSabr[loopparam][2];
		  }
		}
		return ValueDerivatives.of(price, DoubleArray.ofUnsafe(priceDerivativeSabr));
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the underlying SABR data.
	  /// </summary>
	  /// <returns> the sabrData </returns>
	  public SabrFormulaData SabrData
	  {
		  get
		  {
			return sabrData;
		  }
	  }

	  /// <summary>
	  /// Gets the cut-off strike.
	  /// <para>
	  /// The smile is extrapolated above that level.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the cutOffStrike </returns>
	  public double CutOffStrike
	  {
		  get
		  {
			return cutOffStrike;
		  }
	  }

	  /// <summary>
	  /// Gets the tail thickness parameter.
	  /// </summary>
	  /// <returns> the mu parameter </returns>
	  public double Mu
	  {
		  get
		  {
			return mu;
		  }
	  }

	  /// <summary>
	  /// Gets the time to expiry.
	  /// </summary>
	  /// <returns> the time to expiry </returns>
	  public double TimeToExpiry
	  {
		  get
		  {
			return timeToExpiry;
		  }
	  }

	  /// <summary>
	  /// Gets the three fitting parameters.
	  /// </summary>
	  /// <returns> the parameters </returns>
	  public double[] Parameter
	  {
		  get
		  {
			return parameter;
		  }
	  }

	  /// <summary>
	  /// Gets the three fitting parameters derivatives with respect to the forward.
	  /// </summary>
	  /// <returns> the parameters derivative </returns>
	  public double[] ParameterDerivativeForward
	  {
		  get
		  {
			if (parameterDerivativeForward == null)
			{
			  parameterDerivativeForward = computesParametersDerivativeForward();
			}
			return parameterDerivativeForward;
		  }
	  }

	  /// <summary>
	  /// Gets the three fitting parameters derivatives with respect to the SABR parameters.
	  /// </summary>
	  /// <returns> the parameters derivative </returns>
	  public double[][] ParameterDerivativeSabr
	  {
		  get
		  {
			if (parameterDerivativeSabr == null)
			{
			  parameterDerivativeSabr = computesParametersDerivativeSabr();
			}
			return parameterDerivativeSabr;
		  }
	  }

	  //-------------------------------------------------------------------------
	  private double[] computesFittingParameters()
	  {
		double[] param = new double[3]; // Implementation note: called a,b,c in the note.
		// Computes derivatives at cut-off.
		double[] vD = new double[6];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] vD2 = new double[2][2];
		double[][] vD2 = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		volatilityK = sabrFunction.volatilityAdjoint2(forward, cutOffStrike, timeToExpiry, sabrData, vD, vD2);
		Pair<ValueDerivatives, double[][]> pa2 = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike, timeToExpiry, volatilityK, true);
		double[] bsD = pa2.First.Derivatives.toArrayUnsafe();
		double[][] bsD2 = pa2.Second;
		priceK[0] = pa2.First.Value;
		priceK[1] = bsD[1] + bsD[3] * vD[1];
		priceK[2] = bsD2[1][1] + bsD2[1][2] * vD[1] + (bsD2[2][1] + bsD2[2][2] * vD[1]) * vD[1] + bsD[3] * vD2[1][1];
		if (Math.Abs(priceK[0]) < SMALL_PRICE && Math.Abs(priceK[1]) < SMALL_PRICE && Math.Abs(priceK[2]) < SMALL_PRICE)
		{
		  // Implementation note: If value and its derivatives is too small, then parameters are such that the extrapolated price is "very small".
		  return new double[] {-100.0, 0, 0};
		}
		System.Func<double, double> toSolveC = getCFunction(priceK, cutOffStrike, mu);
		BracketRoot bracketer = new BracketRoot();
		double accuracy = 1.0E-5;
		RidderSingleRootFinder rootFinder = new RidderSingleRootFinder(accuracy);
		double[] range = bracketer.getBracketedPoints(toSolveC, -1.0, 1.0);
		param[2] = rootFinder.getRoot(toSolveC, range[0], range[1]).Value;
		param[1] = -2 * param[2] / cutOffStrike - (priceK[1] / priceK[0] * cutOffStrike + mu) * cutOffStrike;
		param[0] = Math.Log(priceK[0] / Math.Pow(cutOffStrike, -mu)) - param[1] / cutOffStrike - param[2] / (cutOffStrike * cutOffStrike);
		return param;
	  }

	  /// <summary>
	  /// Computes the derivative of the three fitting parameters with respect to the forward.
	  /// The computation requires some third order derivatives; they are computed by finite
	  /// difference on the second order derivatives.
	  /// Used to compute the derivative of the price with respect to the forward.
	  /// </summary>
	  /// <returns> the derivatives </returns>
	  private double[] computesParametersDerivativeForward()
	  {
		if (Math.Abs(priceK[0]) < SMALL_PRICE && Math.Abs(priceK[1]) < SMALL_PRICE && Math.Abs(priceK[2]) < SMALL_PRICE)
		{
		  // Implementation note: If value and its derivatives is too small, then parameters are such that the extrapolated price is "very small".
		  return new double[] {0.0, 0.0, 0.0};
		}
		// Derivative of price with respect to forward.
		double[] pDF = new double[3];
		double shift = 0.00001;
		double[] vD = new double[6];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] vD2 = new double[2][2];
		double[][] vD2 = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		sabrFunction.volatilityAdjoint2(forward, cutOffStrike, timeToExpiry, sabrData, vD, vD2);
		Pair<ValueDerivatives, double[][]> pa2 = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike, timeToExpiry, volatilityK, true);
		double[] bsD = pa2.First.Derivatives.toArrayUnsafe();
		double[][] bsD2 = pa2.Second;
		pDF[0] = bsD[0] + bsD[3] * vD[0];
		pDF[1] = bsD2[0][1] + bsD2[2][0] * vD[1] + (bsD2[1][2] + bsD2[2][2] * vD[1]) * vD[0] + bsD[3] * vD2[1][0];
		Pair<ValueDerivatives, double[][]> pa2KP = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike * (1 + shift), timeToExpiry, volatilityK, true);
		double[][] bsD2KP = pa2KP.Second;
		double bsD3FKK = (bsD2KP[1][0] - bsD2[1][0]) / (cutOffStrike * shift);
		Pair<ValueDerivatives, double[][]> pa2VP = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike, timeToExpiry, volatilityK * (1 + shift), true);
		double[][] bsD2VP = pa2VP.Second;
		double bsD3sss = (bsD2VP[2][2] - bsD2[2][2]) / (volatilityK * shift);
		double bsD3sFK = (bsD2VP[0][1] - bsD2[0][1]) / (volatilityK * shift);
		double bsD3sFs = (bsD2VP[0][2] - bsD2[0][2]) / (volatilityK * shift);
		double bsD3sKK = (bsD2VP[1][1] - bsD2[1][1]) / (volatilityK * shift);
		double bsD3ssK = (bsD2VP[2][1] - bsD2[2][1]) / (volatilityK * shift);
		double[] vDKP = new double[6];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] vD2KP = new double[2][2];
		double[][] vD2KP = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		sabrFunction.volatilityAdjoint2(forward, cutOffStrike * (1 + shift), timeToExpiry, sabrData, vDKP, vD2KP);
		double vD3KKF = (vD2KP[1][0] - vD2[1][0]) / (cutOffStrike * shift);
		pDF[2] = bsD3FKK + bsD3sFK * vD[1] + (bsD3sFK + bsD3sFs * vD[1]) * vD[1] + bsD2[2][0] * vD2[1][1] + (bsD3sKK + bsD3ssK * vD[1] + (bsD3ssK + bsD3sss * vD[1]) * vD[1] + bsD2[2][2] * vD2[1][1]) * vD[0] + 2 * (bsD2[1][2] + bsD2[2][2] * vD[1]) * vD2[1][0] + bsD[3] * vD3KKF;
		// Derivative of f with respect to abc.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] fD = new double[3][3]; // fD[i][j]: derivative with respect to jth variable of f_i
		double[][] fD = RectangularArrays.ReturnRectangularDoubleArray(3, 3); // fD[i][j]: derivative with respect to jth variable of f_i
		double f = priceK[0];
		double fp = priceK[1];
		double fpp = priceK[2];
		fD[0][0] = f;
		fD[0][1] = f / cutOffStrike;
		fD[0][2] = fD[0][1] / cutOffStrike;
		fD[1][0] = fp;
		fD[1][1] = (fp - fD[0][1]) / cutOffStrike;
		fD[1][2] = (fp - 2 * fD[0][1]) / (cutOffStrike * cutOffStrike);
		fD[2][0] = fpp;
		fD[2][1] = (fpp + fD[0][2] * (2 * (mu + 1) + 2 * parameter[1] / cutOffStrike + 4 * parameter[2] / (cutOffStrike * cutOffStrike))) / cutOffStrike;
		fD[2][2] = (fpp + fD[0][2] * (2 * (2 * mu + 3) + 4 * parameter[1] / cutOffStrike + 8 * parameter[2] / (cutOffStrike * cutOffStrike))) / (cutOffStrike * cutOffStrike);
		DecompositionResult decmp = SVD.apply(DoubleMatrix.ofUnsafe(fD));
		return decmp.solve(pDF);
	  }

	  /// <summary>
	  /// Computes the derivative of the three fitting parameters with respect to the SABR parameters.
	  /// The computation requires some third order derivatives; they are computed by finite difference
	  /// on the second order derivatives.
	  /// Used to compute the derivative of the price with respect to the SABR parameters.
	  /// </summary>
	  /// <returns> the derivatives </returns>
	  private double[][] computesParametersDerivativeSabr()
	  {
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] result = new double[4][3];
		double[][] result = RectangularArrays.ReturnRectangularDoubleArray(4, 3);
		if (Math.Abs(priceK[0]) < SMALL_PRICE && Math.Abs(priceK[1]) < SMALL_PRICE && Math.Abs(priceK[2]) < SMALL_PRICE)
		{
		  // Implementation note: If value and its derivatives is too small, then parameters are such that the extrapolated price is "very small".
		  return result;
		}
		// Derivative of price with respect to SABR parameters.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] pdSabr = new double[4][3]; // parameter SABR - equation
		double[][] pdSabr = RectangularArrays.ReturnRectangularDoubleArray(4, 3); // parameter SABR - equation
		double shift = 1.0E-5;
		double[] vD = new double[6];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] vD2 = new double[2][2];
		double[][] vD2 = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		sabrFunction.volatilityAdjoint2(forward, cutOffStrike, timeToExpiry, sabrData, vD, vD2);
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  int paramIndex = 2 + loopparam;
		  Pair<ValueDerivatives, double[][]> pa2 = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike, timeToExpiry, volatilityK, true);
		  double[] bsD = pa2.First.Derivatives.toArrayUnsafe();
		  double[][] bsD2 = pa2.Second;
		  pdSabr[loopparam][0] = bsD[3] * vD[paramIndex];
		  double[] vDpP = new double[6];
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] vD2pP = new double[2][2];
		  double[][] vD2pP = RectangularArrays.ReturnRectangularDoubleArray(2, 2);
		  SabrFormulaData sabrDatapP;
		  double param;
		  double paramShift;
		  switch (loopparam)
		  {
			case 0:
			  param = sabrData.Alpha;
			  paramShift = param * shift; // Relative shift to cope with difference in order of magnitude.
			  sabrDatapP = sabrData.withAlpha(param + paramShift);
			  break;
			case 1:
			  param = sabrData.Beta;
			  paramShift = shift; // Absolute shift as usually 0 <= beta <= 1; beta can be zero, so relative shift is not possible.
			  sabrDatapP = sabrData.withBeta(param + paramShift);
			  break;
			case 2:
			  param = sabrData.Rho;
			  paramShift = shift; // Absolute shift as -1 <= rho <= 1; rho can be zero, so relative shift is not possible.
			  sabrDatapP = sabrData.withRho(param + paramShift);
			  break;
			default:
			  param = sabrData.Nu;
			  paramShift = shift; // nu can be zero, so relative shift is not possible.
			  sabrDatapP = sabrData.withNu(param + paramShift);
			  break;
		  }
		  sabrFunction.volatilityAdjoint2(forward, cutOffStrike, timeToExpiry, sabrDatapP, vDpP, vD2pP);
		  double vD2Kp = (vDpP[1] - vD[1]) / paramShift;
		  double vD3KKa = (vD2pP[1][1] - vD2[1][1]) / paramShift;
		  pdSabr[loopparam][1] = (bsD2[1][2] + bsD2[2][2] * vD[1]) * vD[paramIndex] + bsD[3] * vD2Kp;
		  Pair<ValueDerivatives, double[][]> pa2VP = BlackFormulaRepository.priceAdjoint2(forward, cutOffStrike, timeToExpiry, volatilityK * (1d + shift), true);
		  double[][] bsD2VP = pa2VP.Second;
		  double bsD3sss = (bsD2VP[2][2] - bsD2[2][2]) / (volatilityK * shift);
		  double bsD3sKK = (bsD2VP[1][1] - bsD2[1][1]) / (volatilityK * shift);
		  double bsD3ssK = (bsD2VP[2][1] - bsD2[2][1]) / (volatilityK * shift);
		  pdSabr[loopparam][2] = (bsD3sKK + bsD3ssK * vD[1] + (bsD3ssK + bsD3sss * vD[1]) * vD[1] + bsD2[2][2] * vD2[1][1]) * vD[paramIndex] + 2 * (bsD2[2][1] + bsD2[2][2] * vD[1]) * vD2Kp + bsD[3] * vD3KKa;
		}
		// Derivative of f with respect to abc.
//JAVA TO C# CONVERTER NOTE: The following call to the 'RectangularArrays' helper class reproduces the rectangular array initialization that is automatic in Java:
//ORIGINAL LINE: double[][] fD = new double[3][3]; // fD[i][j]: derivative with respect to jth variable of f_i
		double[][] fD = RectangularArrays.ReturnRectangularDoubleArray(3, 3); // fD[i][j]: derivative with respect to jth variable of f_i
		double f = priceK[0];
		double fp = priceK[1];
		double fpp = priceK[2];
		fD[0][0] = f;
		fD[0][1] = f / cutOffStrike;
		fD[0][2] = fD[0][1] / cutOffStrike;
		fD[1][0] = fp;
		fD[1][1] = (fp - fD[0][1]) / cutOffStrike;
		fD[1][2] = (fp - 2 * fD[0][1]) / (cutOffStrike * cutOffStrike);
		fD[2][0] = fpp;
		fD[2][1] = (fpp + fD[0][2] * (2 * (mu + 1) + 2 * parameter[1] / cutOffStrike + 4 * parameter[2] / (cutOffStrike * cutOffStrike))) / cutOffStrike;
		fD[2][2] = (fpp + fD[0][2] * (2 * (2 * mu + 3) + 4 * parameter[1] / cutOffStrike + 8 * parameter[2] / (cutOffStrike * cutOffStrike))) / (cutOffStrike * cutOffStrike);
		DoubleMatrix fDmatrix = DoubleMatrix.ofUnsafe(fD);

		DecompositionResult decmp = SVD.apply(fDmatrix);
		for (int loopparam = 0; loopparam < 4; loopparam++)
		{
		  result[loopparam] = decmp.solve(pdSabr[loopparam]);
		}
		return result;
	  }

	  // The extrapolation function.
	  private double extrapolation(double strike)
	  {
		return Math.Pow(strike, -mu) * Math.Exp(parameter[0] + parameter[1] / strike + parameter[2] / (strike * strike));
	  }

	  // The first order derivative of the extrapolation function with respect to the strike.
	  private double extrapolationDerivative(double strike)
	  {
		return -extrapolation(strike) * (mu + (parameter[1] + 2 * parameter[2] / strike) / strike) / strike;
	  }

	  // The c parameter as a function of price, cutoff and mu.
	  private System.Func<double, double> getCFunction(double[] price, double cutOffStrike, double mu)
	  {
		double[] cPrice = Arrays.copyOf(price, price.Length);
		return (double? c) =>
		{
	double b = -2 * c / cutOffStrike - (cPrice[1] / cPrice[0] * cutOffStrike + mu) * cutOffStrike;
	double k2 = cutOffStrike * cutOffStrike;
	double res = -cPrice[2] / cPrice[0] * k2 + mu * (mu + 1) + 2 * b * (mu + 1) / cutOffStrike + (2 * c * (2 * mu + 3) + b * b) / k2 + 4 * b * c / (k2 * cutOffStrike) + 4 * c * c / (k2 * k2);
	return res;
		};
	  }

	}

}