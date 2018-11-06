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
	public class TestingParameterizedData2 : ParameterizedData
	{

	  private readonly double value1;
	  private readonly double value2;

	  public TestingParameterizedData2(double value1, double value2)
	  {
		this.value1 = value1;
		this.value2 = value2;
	  }

	  public virtual int ParameterCount
	  {
		  get
		  {
			return 2;
		  }
	  }

	  public virtual double getParameter(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, 2);
		return parameterIndex == 0 ? value1 : value2;
	  }

	  public virtual ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		Preconditions.checkElementIndex(parameterIndex, 2);
		return ParameterMetadata.empty();
	  }

	  public virtual ParameterizedData withParameter(int parameterIndex, double newValue)
	  {
		if (parameterIndex == 0)
		{
		  return new TestingParameterizedData2(newValue, value2);
		}
		return new TestingParameterizedData2(value1, newValue);
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj is TestingParameterizedData2)
		{
		  TestingParameterizedData2 other = (TestingParameterizedData2) obj;
		  return Double.doubleToRawLongBits(value1) == Double.doubleToRawLongBits(other.value1) && Double.doubleToRawLongBits(value2) == Double.doubleToRawLongBits(other.value2);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return Convert.ToDouble(value1).GetHashCode() ^ Convert.ToDouble(value2).GetHashCode();
	  }

	  public override string ToString()
	  {
		return value1 + ":" + value2;
	  }

	}

}