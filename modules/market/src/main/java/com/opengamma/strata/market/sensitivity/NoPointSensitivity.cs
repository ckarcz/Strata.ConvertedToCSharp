/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

	using Currency = com.opengamma.strata.basics.currency.Currency;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;

	/// <summary>
	/// An empty implementation of the point sensitivity builder, used where there is no sensitivity.
	/// </summary>
	internal sealed class NoPointSensitivity : PointSensitivityBuilder
	{

	  /// <summary>
	  /// Singleton instance.
	  /// </summary>
	  internal static readonly PointSensitivityBuilder INSTANCE = new NoPointSensitivity();

	  /// <summary>
	  /// Restricted constructor.
	  /// </summary>
	  private NoPointSensitivity()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public PointSensitivityBuilder withCurrency(Currency currency)
	  {
		return this;
	  }

	  public PointSensitivityBuilder mapSensitivity(System.Func<double, double> @operator)
	  {
		return this;
	  }

	  public override PointSensitivityBuilder combinedWith(PointSensitivityBuilder other)
	  {
		return ArgChecker.notNull(other, "other");
	  }

	  public NoPointSensitivity normalize()
	  {
		return this;
	  }

	  public MutablePointSensitivities buildInto(MutablePointSensitivities combination)
	  {
		return combination;
	  }

	  public NoPointSensitivity cloned()
	  {
		return this;
	  }

	  public override string ToString()
	  {
		return "NoPointSensitivity";
	  }

	}

}