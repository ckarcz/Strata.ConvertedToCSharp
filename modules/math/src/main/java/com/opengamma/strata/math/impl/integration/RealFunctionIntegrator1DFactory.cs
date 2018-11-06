using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2009 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.math.impl.integration
{

	/// <summary>
	/// Factory class for 1-D integrators that do not take arguments.
	/// </summary>
	public sealed class RealFunctionIntegrator1DFactory
	{
	  // TODO add more integration types
	  /// <summary>
	  /// Romberg integrator name </summary>
	  public const string ROMBERG = "Romberg";
	  /// <summary>
	  /// <seealso cref="RombergIntegrator1D"/> </summary>
	  public static readonly RombergIntegrator1D ROMBERG_INSTANCE = new RombergIntegrator1D();
	  /// <summary>
	  /// Simpson integrator name </summary>
	  public const string SIMPSON = "Simpson";
	  /// <summary>
	  /// <seealso cref="SimpsonIntegrator1D"/> </summary>
	  public static readonly SimpsonIntegrator1D SIMPSON_INSTANCE = new SimpsonIntegrator1D();
	  /// <summary>
	  /// Extended trapezoid integrator name </summary>
	  public const string EXTENDED_TRAPEZOID = "ExtendedTrapezoid";
	  /// <summary>
	  /// <seealso cref="ExtendedTrapezoidIntegrator1D"/> </summary>
	  public static readonly ExtendedTrapezoidIntegrator1D EXTENDED_TRAPEZOID_INSTANCE = new ExtendedTrapezoidIntegrator1D();

	  private static readonly IDictionary<string, Integrator1D<double, double>> STATIC_INSTANCES;
	  private static readonly IDictionary<Type, string> INSTANCE_NAMES;

	  static RealFunctionIntegrator1DFactory()
	  {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<String, Integrator1D<double, double>> staticInstances = new java.util.HashMap<>();
		IDictionary<string, Integrator1D<double, double>> staticInstances = new Dictionary<string, Integrator1D<double, double>>();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<Class, String> instanceNames = new java.util.HashMap<>();
		IDictionary<Type, string> instanceNames = new Dictionary<Type, string>();
		staticInstances[ROMBERG] = ROMBERG_INSTANCE;
		instanceNames[ROMBERG_INSTANCE.GetType()] = ROMBERG;
		staticInstances[SIMPSON] = SIMPSON_INSTANCE;
		instanceNames[SIMPSON_INSTANCE.GetType()] = SIMPSON;
		staticInstances[EXTENDED_TRAPEZOID] = EXTENDED_TRAPEZOID_INSTANCE;
		instanceNames[EXTENDED_TRAPEZOID_INSTANCE.GetType()] = EXTENDED_TRAPEZOID;
		STATIC_INSTANCES = new Dictionary<>(staticInstances);
		INSTANCE_NAMES = new Dictionary<>(instanceNames);
	  }

	  private RealFunctionIntegrator1DFactory()
	  {
	  }

	  /// <summary>
	  /// Given a name, returns an instance of that integrator.
	  /// </summary>
	  /// <param name="integratorName">  the name of the integrator </param>
	  /// <returns> the integrator </returns>
	  /// <exception cref="IllegalArgumentException"> if the integrator name is null or there is no integrator for that name </exception>
	  public static Integrator1D<double, double> getIntegrator(string integratorName)
	  {
		Integrator1D<double, double> integrator = STATIC_INSTANCES[integratorName];
		if (integrator != null)
		{
		  return integrator;
		}
		throw new System.ArgumentException("Integrator " + integratorName + " not handled");
	  }

	  /// <summary>
	  /// Given an integrator, returns its name.
	  /// </summary>
	  /// <param name="integrator">  the integrator </param>
	  /// <returns> the name of that integrator (null if not found) </returns>
	  public static string getIntegratorName(Integrator1D<double, double> integrator)
	  {
		if (integrator == null)
		{
		  return null;
		}
		return INSTANCE_NAMES[integrator.GetType()];
	  }

	}

}