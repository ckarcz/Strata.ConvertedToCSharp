using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.matrix
{

	/// 
	/// <summary>
	/// Factory class for various types of matrix algebra calculators.
	/// </summary>
	public sealed class MatrixAlgebraFactory
	{

	  /// <summary>
	  /// Label for Commons matrix algebra </summary>
	  public const string COMMONS = "Commons";
	  /// <summary>
	  /// Label for OpenGamma matrix algebra </summary>
	  public const string OG = "OG";
	  /// <summary>
	  /// <seealso cref="CommonsMatrixAlgebra"/> </summary>
	  public static readonly CommonsMatrixAlgebra COMMONS_ALGEBRA = new CommonsMatrixAlgebra();
	  /// <summary>
	  /// <seealso cref="OGMatrixAlgebra"/> </summary>
	  public static readonly OGMatrixAlgebra OG_ALGEBRA = new OGMatrixAlgebra();
	  private static readonly IDictionary<string, MatrixAlgebra> STATIC_INSTANCES;
	  private static readonly IDictionary<Type, string> INSTANCE_NAMES;

	  static MatrixAlgebraFactory()
	  {
		STATIC_INSTANCES = new Dictionary<>();
		INSTANCE_NAMES = new Dictionary<>();
		STATIC_INSTANCES[COMMONS] = COMMONS_ALGEBRA;
		INSTANCE_NAMES[typeof(CommonsMatrixAlgebra)] = COMMONS;
		STATIC_INSTANCES[OG] = OG_ALGEBRA;
		INSTANCE_NAMES[typeof(OGMatrixAlgebra)] = OG;
	  }

	  private MatrixAlgebraFactory()
	  {
	  }

	  /// <summary>
	  /// Given a name, returns an instance of the matrix algebra calculator.
	  /// </summary>
	  /// <param name="algebraName"> The name of the matrix algebra calculator </param>
	  /// <returns> The matrix algebra calculator </returns>
	  /// <exception cref="IllegalArgumentException"> If the calculator name is null or there is no calculator for that name </exception>
	  public static MatrixAlgebra getMatrixAlgebra(string algebraName)
	  {
		if (STATIC_INSTANCES.ContainsKey(algebraName))
		{
		  return STATIC_INSTANCES[algebraName];
		}
		throw new System.ArgumentException("Matrix algebra " + algebraName + " not found");
	  }

	  /// <summary>
	  /// Given a matrix algebra calculator, returns its name.
	  /// </summary>
	  /// <param name="algebra"> The algebra </param>
	  /// <returns> The name of that calculator (null if not found) </returns>
	  public static string getMatrixAlgebraName(MatrixAlgebra algebra)
	  {
		if (algebra == null)
		{
		  return null;
		}
		return INSTANCE_NAMES[algebra.GetType()];
	  }

	}

}