using System;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
	using Preconditions = com.google.common.@base.Preconditions;

	/// <summary>
	/// Testing implementation.
	/// </summary>
	public class TestingParameterizedData : ParameterizedData
	{

	  private readonly double value;

	  public TestingParameterizedData(double value)
	  {
		this.value = value;
	  }

	  public virtual int ParameterCount
	  {
		  get
		  {
			return 1;
		  }
	  }

	  public virtual double getParameter(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, 1);
		return value;
	  }

	  public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, 1);
		return ParameterMetadata.empty();
	  }

	  public virtual ParameterizedData withParameter(int parameterIndex, double newValue)
	  {
		return new TestingParameterizedData(newValue);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj is TestingParameterizedData)
		{
		  TestingParameterizedData other = (TestingParameterizedData) obj;
		  return Double.doubleToRawLongBits(value) == Double.doubleToRawLongBits(other.value);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return Convert.ToDouble(value).GetHashCode();
	  }

	  public override string ToString()
	  {
		return Convert.ToString(value);
	  }

	}

}