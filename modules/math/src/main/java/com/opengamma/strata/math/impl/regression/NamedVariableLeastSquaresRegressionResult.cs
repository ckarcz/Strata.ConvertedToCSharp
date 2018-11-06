using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.regression
{

	using Logger = org.slf4j.Logger;
	using LoggerFactory = org.slf4j.LoggerFactory;

	/// 
	//CSOFF: JavadocMethod
	public class NamedVariableLeastSquaresRegressionResult : LeastSquaresRegressionResult
	{

	  private static readonly Logger log = LoggerFactory.getLogger(typeof(NamedVariableLeastSquaresRegressionResult));
	  private readonly IList<string> _independentVariableNames;
	  private readonly LeastSquaresRegressionResult _result;
	  private const string INTERCEPT_STRING = "Intercept";

	  public NamedVariableLeastSquaresRegressionResult(IList<string> independentVariableNames, LeastSquaresRegressionResult result) : base(result)
	  {

		if (independentVariableNames == null)
		{
		  throw new System.ArgumentException("List of independent variable names was null");
		}
		_independentVariableNames = new List<>();
		if (result.hasIntercept())
		{
		  if (independentVariableNames.Count != result.Betas.Length - 1)
		  {
			throw new System.ArgumentException("Length of variable name array did not match number of results in the regression");
		  }
		  _independentVariableNames.Add(INTERCEPT_STRING);
		}
		else
		{
		  if (independentVariableNames.Count != result.Betas.Length)
		  {
			throw new System.ArgumentException("Length of variable name array did not match number of results in the regression");
		  }
		}
		((IList<string>)_independentVariableNames).AddRange(independentVariableNames);
		_result = result;
	  }

	  /// <returns> the _independentVariableNames </returns>
	  public virtual IList<string> IndependentVariableNames
	  {
		  get
		  {
			return _independentVariableNames;
		  }
	  }

	  /// <returns> the _result </returns>
	  public virtual LeastSquaresRegressionResult Result
	  {
		  get
		  {
			return _result;
		  }
	  }

	  public virtual double? getPredictedValue(IDictionary<string, double> namesAndValues)
	  {
		if (namesAndValues == null)
		{
		  throw new System.ArgumentException("Map was null");
		}
		if (namesAndValues.Count == 0)
		{
		  log.warn("Map was empty: returning 0");
		  return 0.0;
		}
		double[] betas = Betas;
		double sum = 0;
		if (hasIntercept())
		{
		  if (namesAndValues.Count < betas.Length - 1)
		  {
			throw new System.ArgumentException("Number of named variables in map was smaller than that in regression");
		  }
		}
		else
		{
		  if (namesAndValues.Count < betas.Length)
		  {
			throw new System.ArgumentException("Number of named variables in map was smaller than that in regression");
		  }
		}
		int i = hasIntercept() ? 1 : 0;
		foreach (string name in IndependentVariableNames)
		{
		  if (name.Equals(INTERCEPT_STRING))
		  {
			sum += betas[0];
		  }
		  else
		  {
			if (!namesAndValues.ContainsKey(name) || namesAndValues[name] == null)
			{
			  throw new System.ArgumentException("Do not have value for " + name);
			}
			sum += betas[i++] * namesAndValues[name];
		  }
		}
		return sum;
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = base.GetHashCode();
		result = prime * result + (_independentVariableNames == null ? 0 : _independentVariableNames.GetHashCode());
		result = prime * result + (_result == null ? 0 : _result.GetHashCode());
		return result;
	  }

	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (!base.Equals(obj))
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		NamedVariableLeastSquaresRegressionResult other = (NamedVariableLeastSquaresRegressionResult) obj;
		if (_independentVariableNames == null)
		{
		  if (other._independentVariableNames != null)
		  {
			return false;
		  }
		}
//JAVA TO C# CONVERTER WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: else if (!_independentVariableNames.equals(other._independentVariableNames))
		else if (!_independentVariableNames.SequenceEqual(other._independentVariableNames))
		{
		  return false;
		}
		if (_result == null)
		{
		  if (other._result != null)
		  {
			return false;
		  }
		}
		else if (!_result.Equals(other._result))
		{
		  return false;
		}
		return true;
	  }
	}

}