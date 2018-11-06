using System;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl
{
	/// <summary>
	/// A complex number.
	/// </summary>
	public class ComplexNumber : Number
	{

	  /// <summary>
	  /// Defining <i>i</i> </summary>
	  public static readonly ComplexNumber I = new ComplexNumber(0, 1);
	  /// <summary>
	  /// Defining <i>-i</i> </summary>
	  public static readonly ComplexNumber MINUS_I = new ComplexNumber(0, -1);
	  /// <summary>
	  /// Defining 0 + 0<i>i</i> </summary>
	  public static readonly ComplexNumber ZERO = new ComplexNumber(0);

	  /// <summary>
	  /// Serialization version. </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The real part. </summary>
	  private readonly double real;
	  /// <summary>
	  /// The imaginary part. </summary>
	  private readonly double imaginary;

	  /// <summary>
	  /// Creates an instance from the real part.
	  /// </summary>
	  /// <param name="real">  the real part </param>
	  public ComplexNumber(double real)
	  {
		this.real = real;
		this.imaginary = 0.0;
	  }

	  /// <summary>
	  /// Creates an instance from the real and imaginary parts.
	  /// </summary>
	  /// <param name="real">  the real part </param>
	  /// <param name="imaginary">  the imaginary part </param>
	  public ComplexNumber(double real, double imaginary)
	  {
		this.real = real;
		this.imaginary = imaginary;
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the real part.
	  /// </summary>
	  /// <returns> the real part </returns>
	  public virtual double Real
	  {
		  get
		  {
			return real;
		  }
	  }

	  /// <summary>
	  /// Gets the imaginary part.
	  /// </summary>
	  /// <returns> the imaginary part </returns>
	  public virtual double Imaginary
	  {
		  get
		  {
			return imaginary;
		  }
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override double doubleValue()
	  {
		throw new System.NotSupportedException("Cannot get the doubleValue of a ComplexNumber");
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override float floatValue()
	  {
		throw new System.NotSupportedException("Cannot get the floatValue of a ComplexNumber");
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override int intValue()
	  {
		throw new System.NotSupportedException("Cannot get the intValue of a ComplexNumber");
	  }

	  /// <summary>
	  /// {@inheritDoc} </summary>
	  /// <exception cref="UnsupportedOperationException"> always </exception>
	  public override long longValue()
	  {
		throw new System.NotSupportedException("Cannot get the longValue of a ComplexNumber");
	  }

	  //-------------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (this == obj)
		{
		  return true;
		}
		if (obj == null)
		{
		  return false;
		}
		if (this.GetType() != obj.GetType())
		{
		  return false;
		}
		ComplexNumber other = (ComplexNumber) obj;
		if (System.BitConverter.DoubleToInt64Bits(this.imaginary) != Double.doubleToLongBits(other.imaginary))
		{
		  return false;
		}
		if (System.BitConverter.DoubleToInt64Bits(this.real) != Double.doubleToLongBits(other.real))
		{
		  return false;
		}
		return true;
	  }

	  public override int GetHashCode()
	  {
		int prime = 31;
		int result = 1;
		long temp;
		temp = System.BitConverter.DoubleToInt64Bits(imaginary);
		result = prime * result + (int)(temp ^ (long)((ulong)temp >> 32));
		temp = System.BitConverter.DoubleToInt64Bits(real);
		result = prime * result + (int)(temp ^ (long)((ulong)temp >> 32));
		return result;
	  }

	  public override string ToString()
	  {
		bool negative = imaginary < 0;
		double abs = Math.Abs(imaginary);
		return Convert.ToString(real) + (negative ? " - " : " + ") + Convert.ToString(abs) + "i";
	  }

	}

}