/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure
{
	using Measure = com.opengamma.strata.calc.Measure;

	/// <summary>
	/// The advanced set of measures which can be calculated by Strata.
	/// <para>
	/// These measures are rarely needed and should be used with care.
	/// </para>
	/// <para>
	/// Note that not all measures will be available for all targets.
	/// </para>
	/// </summary>
	public sealed class AdvancedMeasures
	{

	  /// <summary>
	  /// Measure representing the semi-parallel bucketed gamma PV01 of the calculation target.
	  /// </summary>
	  public static readonly Measure PV01_SEMI_PARALLEL_GAMMA_BUCKETED = Measure.of(StandardMeasures.PV01_SEMI_PARALLEL_GAMMA_BUCKETED.Name);
	  /// <summary>
	  /// Measure representing the single-node bucketed gamma PV01 of the calculation target.
	  /// </summary>
	  public static readonly Measure PV01_SINGLE_NODE_GAMMA_BUCKETED = Measure.of(StandardMeasures.PV01_SINGLE_NODE_GAMMA_BUCKETED.Name);

	  //-------------------------------------------------------------------------
	  private AdvancedMeasures()
	  {
	  }

	}

}