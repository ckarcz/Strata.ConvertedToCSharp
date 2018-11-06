using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.linearalgebra
{

	using Decomposition = com.opengamma.strata.math.linearalgebra.Decomposition;

	/// <summary>
	/// Factory class for different types of decompositions.
	/// </summary>
	public sealed class DecompositionFactory
	{

	  /// <summary>
	  /// Commons LU decomposition </summary>
	  public const string LU_COMMONS_NAME = "LU_COMMONS";
	  /// <summary>
	  /// Commons QR decomposition </summary>
	  public const string QR_COMMONS_NAME = "QR_COMMONS";
	  /// <summary>
	  /// Commons SV decomposition </summary>
	  public const string SV_COMMONS_NAME = "SV_COMMONS";
	  /// <summary>
	  /// <seealso cref="LUDecompositionCommons"/> </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static final com.opengamma.strata.math.linearalgebra.Decomposition<?> LU_COMMONS = new LUDecompositionCommons();
	  public static readonly Decomposition<object> LU_COMMONS = new LUDecompositionCommons();
	  /// <summary>
	  /// <seealso cref="QRDecompositionCommons"/> </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static final com.opengamma.strata.math.linearalgebra.Decomposition<?> QR_COMMONS = new QRDecompositionCommons();
	  public static readonly Decomposition<object> QR_COMMONS = new QRDecompositionCommons();
	  /// <summary>
	  /// <seealso cref="SVDecompositionCommons"/> </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static final com.opengamma.strata.math.linearalgebra.Decomposition<?> SV_COMMONS = new SVDecompositionCommons();
	  public static readonly Decomposition<object> SV_COMMONS = new SVDecompositionCommons();
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Map<String, com.opengamma.strata.math.linearalgebra.Decomposition<?>> STATIC_INSTANCES;
	  private static readonly IDictionary<string, Decomposition<object>> STATIC_INSTANCES;
	  private static readonly IDictionary<Type, string> INSTANCE_NAMES;

	  static DecompositionFactory()
	  {
		STATIC_INSTANCES = new Dictionary<>();
		STATIC_INSTANCES[LU_COMMONS_NAME] = LU_COMMONS;
		STATIC_INSTANCES[QR_COMMONS_NAME] = QR_COMMONS;
		STATIC_INSTANCES[SV_COMMONS_NAME] = SV_COMMONS;
		INSTANCE_NAMES = new Dictionary<>();
		INSTANCE_NAMES[LU_COMMONS.GetType()] = LU_COMMONS_NAME;
		INSTANCE_NAMES[QR_COMMONS.GetType()] = QR_COMMONS_NAME;
		INSTANCE_NAMES[SV_COMMONS.GetType()] = SV_COMMONS_NAME;
	  }

	  private DecompositionFactory()
	  {
	  }

	  /// <summary>
	  /// Given a name, returns an instance of that decomposition method.
	  /// </summary>
	  /// <param name="decompositionName"> The name of the decomposition method </param>
	  /// <returns> The decomposition method </returns>
	  /// <exception cref="IllegalArgumentException"> If the decomposition name is null or there is no decomposition method of that name </exception>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: public static com.opengamma.strata.math.linearalgebra.Decomposition<?> getDecomposition(String decompositionName)
	  public static Decomposition<object> getDecomposition(string decompositionName)
	  {
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.math.linearalgebra.Decomposition<?> decomposition = STATIC_INSTANCES.get(decompositionName);
		Decomposition<object> decomposition = STATIC_INSTANCES[decompositionName];
		if (decomposition != null)
		{
		  return decomposition;
		}
		throw new System.ArgumentException("Could not get decomposition " + decompositionName);
	  }

	  /// <summary>
	  /// Given a decomposition method, returns its name.
	  /// </summary>
	  /// <param name="decomposition"> The decomposition method </param>
	  /// <returns> The name of the decomposition method (null if not found) </returns>
	  public static string getDecompositionName<T1>(Decomposition<T1> decomposition)
	  {
		if (decomposition == null)
		{
		  return null;
		}
		return INSTANCE_NAMES[decomposition.GetType()];
	  }

	}

}