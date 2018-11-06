using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.rootfinding.newton
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	using Doubles = com.google.common.primitives.Doubles;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using VectorFieldFirstOrderDifferentiator = com.opengamma.strata.math.impl.differentiation.VectorFieldFirstOrderDifferentiator;
	using MatrixAlgebra = com.opengamma.strata.math.impl.matrix.MatrixAlgebra;
	using OGMatrixAlgebra = com.opengamma.strata.math.impl.matrix.OGMatrixAlgebra;
	using NewtonVectorRootFinder = com.opengamma.strata.math.rootfind.NewtonVectorRootFinder;

	/// <summary>
	/// Base implementation for all Newton-Raphson style multi-dimensional root finding (i.e. using the Jacobian matrix as a basis for some iterative process)
	/// </summary>
	// CSOFF: JavadocMethod
	public class BaseNewtonVectorRootFinder : VectorRootFinder, NewtonVectorRootFinder
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(BaseNewtonVectorRootFinder));
	  private const double ALPHA = 1e-4;
	  private const double BETA = 1.5;
	  private const int FULL_RECALC_FREQ = 20;
	  private readonly double _absoluteTol, _relativeTol;
	  private readonly int _maxSteps;
	  private readonly NewtonRootFinderDirectionFunction _directionFunction;
	  private readonly NewtonRootFinderMatrixInitializationFunction _initializationFunction;
	  private readonly NewtonRootFinderMatrixUpdateFunction _updateFunction;
	  private readonly MatrixAlgebra _algebra = new OGMatrixAlgebra();

	  public BaseNewtonVectorRootFinder(double absoluteTol, double relativeTol, int maxSteps, NewtonRootFinderDirectionFunction directionFunction, NewtonRootFinderMatrixInitializationFunction initializationFunction, NewtonRootFinderMatrixUpdateFunction updateFunction)
	  {

		ArgChecker.notNegative(absoluteTol, "absolute tolerance");
		ArgChecker.notNegative(relativeTol, "relative tolerance");
		ArgChecker.notNegative(maxSteps, "maxSteps");
		_absoluteTol = absoluteTol;
		_relativeTol = relativeTol;
		_maxSteps = maxSteps;
		_directionFunction = directionFunction;
		_initializationFunction = initializationFunction;
		_updateFunction = updateFunction;
	  }

	  //-------------------------------------------------------------------------
	  public virtual DoubleArray getRoot(System.Func<DoubleArray, DoubleArray> function, DoubleArray startPosition)
	  {
		return findRoot(function, startPosition);
	  }

	  public virtual DoubleArray findRoot(System.Func<DoubleArray, DoubleArray> function, DoubleArray startPosition)
	  {
		VectorFieldFirstOrderDifferentiator jac = new VectorFieldFirstOrderDifferentiator();
		return findRoot(function, jac.differentiate(function), startPosition);
	  }

	  public virtual DoubleArray findRoot(System.Func<DoubleArray, DoubleArray> function, System.Func<DoubleArray, DoubleMatrix> jacobianFunction, DoubleArray startPosition)
	  {

		checkInputs(function, startPosition);

		DataBundle data = new DataBundle();
		DoubleArray y = function(startPosition);
		data.X = startPosition;
		data.Y = y;
		data.G0 = _algebra.getInnerProduct(y, y);
		DoubleMatrix estimate = _initializationFunction.getInitializedMatrix(jacobianFunction, startPosition);

		if (!getNextPosition(function, estimate, data))
		{
		  if (isConverged(data))
		  {
			return data.X; // this can happen if the starting position is the root
		  }
		  throw new MathException("Cannot work with this starting position. Please choose another point");
		}

		int count = 0;
		int jacReconCount = 1;
		while (!isConverged(data))
		{
		  // Want to reset the Jacobian every so often even if backtracking is working
		  if ((jacReconCount) % FULL_RECALC_FREQ == 0)
		  {
			estimate = _initializationFunction.getInitializedMatrix(jacobianFunction, data.X);
			jacReconCount = 1;
		  }
		  else
		  {
			estimate = _updateFunction.getUpdatedMatrix(jacobianFunction, data.X, data.DeltaX, data.DeltaY, estimate);
			jacReconCount++;
		  }
		  // if backtracking fails, could be that Jacobian estimate has drifted too far
		  if (!getNextPosition(function, estimate, data))
		  {
			estimate = _initializationFunction.getInitializedMatrix(jacobianFunction, data.X);
			jacReconCount = 1;
			if (!getNextPosition(function, estimate, data))
			{
			  if (isConverged(data))
			  {
				// non-standard exit. Cannot find an improvement from this position,
				// so provided we are close enough to the root, exit.
				return data.X;
			  }
			  string msg = "Failed to converge in backtracking, even after a Jacobian recalculation." + getErrorMessage(data, jacobianFunction);
			  log.info(msg);
			  throw new MathException(msg);
			}
		  }
		  count++;
		  if (count > _maxSteps)
		  {
			throw new MathException("Failed to converge - maximum iterations of " + _maxSteps + " reached." + getErrorMessage(data, jacobianFunction));
		  }
		}
		return data.X;
	  }

	  private string getErrorMessage(DataBundle data, System.Func<DoubleArray, DoubleMatrix> jacobianFunction)
	  {
		return "Final position:" + data.X + "\nlast deltaX:" + data.DeltaX + "\n function value:" + data.Y + "\nJacobian: \n" + jacobianFunction(data.X);
	  }

	  private bool getNextPosition(System.Func<DoubleArray, DoubleArray> function, DoubleMatrix estimate, DataBundle data)
	  {

		DoubleArray p = _directionFunction.getDirection(estimate, data.Y);
		if (data.Lambda0 < 1.0)
		{
		  data.Lambda0 = 1.0;
		}
		else
		{
		  data.Lambda0 = data.Lambda0 * BETA;
		}
		updatePosition(p, function, data);
		double g1 = data.G1;
		if (!Doubles.isFinite(g1))
		{
		  bisectBacktrack(p, function, data);
		}
		if (data.G1 > data.G0 / (1 + ALPHA * data.Lambda0))
		{
		  quadraticBacktrack(p, function, data);
		  int count = 0;
		  while (data.G1 > data.G0 / (1 + ALPHA * data.Lambda0))
		  {
			if (count > 5)
			{
			  return false;
			}
			cubicBacktrack(p, function, data);
			count++;
		  }
		}
		DoubleArray deltaX = data.DeltaX;
		DoubleArray deltaY = data.DeltaY;
		data.G0 = data.G1;
		data.X = (DoubleArray) _algebra.add(data.X, deltaX);
		data.Y = (DoubleArray) _algebra.add(data.Y, deltaY);
		return true;
	  }

	  protected internal virtual void updatePosition(DoubleArray p, System.Func<DoubleArray, DoubleArray> function, DataBundle data)
	  {
		double lambda0 = data.Lambda0;
		DoubleArray deltaX = (DoubleArray) _algebra.scale(p, -lambda0);
		DoubleArray xNew = (DoubleArray) _algebra.add(data.X, deltaX);
		DoubleArray yNew = function(xNew);
		data.DeltaX = deltaX;
		data.DeltaY = (DoubleArray) _algebra.subtract(yNew, data.Y);
		data.G2 = data.G1;
		data.G1 = _algebra.getInnerProduct(yNew, yNew);
	  }

	  private void bisectBacktrack(DoubleArray p, System.Func<DoubleArray, DoubleArray> function, DataBundle data)
	  {
		do
		{
		  data.Lambda0 = data.Lambda0 * 0.1;
		  updatePosition(p, function, data);

		  if (data.Lambda0 == 0.0)
		  {
			throw new MathException("Failed to converge");
		  }
		} while (double.IsNaN(data.G1) || double.IsInfinity(data.G1) || double.IsNaN(data.G2) || double.IsInfinity(data.G2));

	  }

	  private void quadraticBacktrack(DoubleArray p, System.Func<DoubleArray, DoubleArray> function, DataBundle data)
	  {

		double lambda0 = data.Lambda0;
		double g0 = data.G0;
		double lambda = Math.Max(0.01 * lambda0, g0 * lambda0 * lambda0 / (data.G1 + g0 * (2 * lambda0 - 1)));
		data.swapLambdaAndReplace(lambda);
		updatePosition(p, function, data);
	  }

	  private void cubicBacktrack(DoubleArray p, System.Func<DoubleArray, DoubleArray> function, DataBundle data)
	  {
		double temp1, temp2, temp3, temp4, temp5;
		double lambda0 = data.Lambda0;
		double lambda1 = data.Lambda1;
		double g0 = data.G0;
		temp1 = 1.0 / lambda0 / lambda0;
		temp2 = 1.0 / lambda1 / lambda1;
		temp3 = data.G1 + g0 * (2 * lambda0 - 1.0);
		temp4 = data.G2 + g0 * (2 * lambda1 - 1.0);
		temp5 = 1.0 / (lambda0 - lambda1);
		double a = temp5 * (temp1 * temp3 - temp2 * temp4);
		double b = temp5 * (-lambda1 * temp1 * temp3 + lambda0 * temp2 * temp4);
		double lambda = (-b + Math.Sqrt(b * b + 6 * a * g0)) / 3 / a;
		lambda = Math.Min(Math.Max(lambda, 0.01 * lambda0), 0.75 * lambda1); // make sure new lambda is between 1% & 75% of old value
		data.swapLambdaAndReplace(lambda);
		updatePosition(p, function, data);
	  }

	  private bool isConverged(DataBundle data)
	  {
		DoubleArray deltaX = data.DeltaX;
		DoubleArray x = data.X;
		int n = deltaX.size();
		double diff, scale;
		for (int i = 0; i < n; i++)
		{
		  diff = Math.Abs(deltaX.get(i));
		  scale = Math.Abs(x.get(i));
		  if (diff > _absoluteTol + scale * _relativeTol)
		  {
			return false;
		  }
		}
		return (Math.Sqrt(data.G0) < _absoluteTol);
	  }

	  private class DataBundle
	  {
		internal double _g0;
		internal double _g1;
		internal double _g2;
		internal double _lambda0;
		internal double _lambda1;
		internal DoubleArray _deltaY;
		internal DoubleArray _y;
		internal DoubleArray _deltaX;
		internal DoubleArray _x;

		public virtual double G0
		{
			get
			{
			  return _g0;
			}
			set
			{
			  _g0 = value;
			}
		}

		public virtual double G1
		{
			get
			{
			  return _g1;
			}
			set
			{
			  _g1 = value;
			}
		}

		public virtual double G2
		{
			get
			{
			  return _g2;
			}
			set
			{
			  _g2 = value;
			}
		}

		public virtual double Lambda0
		{
			get
			{
			  return _lambda0;
			}
			set
			{
			  _lambda0 = value;
			}
		}

		public virtual double Lambda1
		{
			get
			{
			  return _lambda1;
			}
		}

		public virtual DoubleArray DeltaY
		{
			get
			{
			  return _deltaY;
			}
			set
			{
			  _deltaY = value;
			}
		}

		public virtual DoubleArray Y
		{
			get
			{
			  return _y;
			}
			set
			{
			  _y = value;
			}
		}

		public virtual DoubleArray DeltaX
		{
			get
			{
			  return _deltaX;
			}
			set
			{
			  _deltaX = value;
			}
		}

		public virtual DoubleArray X
		{
			get
			{
			  return _x;
			}
			set
			{
			  _x = value;
			}
		}









		public virtual void swapLambdaAndReplace(double lambda0)
		{
		  _lambda1 = _lambda0;
		  _lambda0 = lambda0;
		}
	  }

	}

}