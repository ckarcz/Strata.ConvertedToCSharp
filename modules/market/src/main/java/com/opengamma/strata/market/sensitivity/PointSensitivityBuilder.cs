/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.sensitivity
{

	using Currency = com.opengamma.strata.basics.currency.Currency;

	/// <summary>
	/// Builder used to create point sensitivities.
	/// <para>
	/// The sensitivity to a single point on a curve is known as <i>point sensitivity</i>.
	/// This builder allows the individual sensitivities to be built into a combined result.
	/// </para>
	/// <para>
	/// Implementations may be mutable, however the methods are intended to be used in an immutable style.
	/// Once a method is called, code should refer and use only the result, not the original instance.
	/// </para>
	/// </summary>
	public interface PointSensitivityBuilder
	{

	  /// <summary>
	  /// Returns a builder representing no sensitivity.
	  /// <para>
	  /// This would be used if the rate was fixed, or if the rate was obtained from a historic
	  /// time-series rather than a forward curve.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the empty builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PointSensitivityBuilder none()
	//  {
	//	return NoPointSensitivity.INSTANCE;
	//  }

	  /// <summary>
	  /// Returns a builder with the specified sensitivities.
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivities, which is copied </param>
	  /// <returns> the builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PointSensitivityBuilder of(PointSensitivity... sensitivities)
	//  {
	//	switch (sensitivities.length)
	//	{
	//	  case 0:
	//		return PointSensitivityBuilder.none();
	//	  case 1:
	//		PointSensitivity sens = sensitivities[0];
	//		if (sens instanceof PointSensitivityBuilder)
	//		{
	//		  return (PointSensitivityBuilder) sens;
	//		}
	//		return new MutablePointSensitivities(sens);
	//	  default:
	//		return new MutablePointSensitivities(Arrays.asList(sensitivities));
	//	}
	//  }

	  /// <summary>
	  /// Returns a builder with the specified sensitivities.
	  /// </summary>
	  /// <param name="sensitivities">  the list of sensitivities, which is copied </param>
	  /// <returns> the builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java static interface methods:
//	  public static PointSensitivityBuilder of(java.util.List<JavaToDotNetGenericWildcard extends PointSensitivity> sensitivities)
	//  {
	//	switch (sensitivities.size())
	//	{
	//	  case 0:
	//		return PointSensitivityBuilder.none();
	//	  case 1:
	//		PointSensitivity sens = sensitivities.get(0);
	//		if (sens instanceof PointSensitivityBuilder)
	//		{
	//		  return (PointSensitivityBuilder) sens;
	//		}
	//		return new MutablePointSensitivities(sens);
	//	  default:
	//		return new MutablePointSensitivities(sensitivities);
	//	}
	//  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns an instance with the specified currency applied to the sensitivities in this builder.
	  /// <para>
	  /// The result will consists of the same points, but with the sensitivity currency assigned.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// Instead, the result of the method must be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="currency">  the currency to be applied to the sensitivities </param>
	  /// <returns> the resulting builder, replacing this builder </returns>
	  PointSensitivityBuilder withCurrency(Currency currency);

	  /// <summary>
	  /// Multiplies the sensitivities in this builder by the specified factor.
	  /// <para>
	  /// The result will consist of the same points, but with each sensitivity multiplied.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// Instead, the result of the method must be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="factor">  the multiplicative factor </param>
	  /// <returns> the resulting builder, replacing this builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PointSensitivityBuilder multipliedBy(double factor)
	//  {
	//	return mapSensitivity(s -> s * factor);
	//  }

	  /// <summary>
	  /// Returns an instance with the specified operation applied to the sensitivities in this builder.
	  /// <para>
	  /// The result will consist of the same points, but with the operator applied to each sensitivity.
	  /// </para>
	  /// <para>
	  /// This is used to apply a mathematical operation to the sensitivities.
	  /// For example, the operator could multiply the sensitivities by a constant, or take the inverse.
	  /// <pre>
	  ///   inverse = base.mapSensitivities(value -> 1 / value);
	  /// </pre>
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// Instead, the result of the method must be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="operator">  the operator to be applied to the sensitivities </param>
	  /// <returns> the resulting builder, replacing this builder </returns>
	  PointSensitivityBuilder mapSensitivity(System.Func<double, double> @operator);

	  /// <summary>
	  /// Normalizes the point sensitivities by sorting and merging.
	  /// <para>
	  /// The sensitivities in the builder are sorted and then merged.
	  /// Any two entries that represent the same curve query are merged.
	  /// For example, if there are two point sensitivities that were created based on the same curve,
	  /// currency and fixing date, then the entries are combined, summing the sensitivity value.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// Instead, the result of the method must be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the resulting builder, replacing this builder </returns>
	  PointSensitivityBuilder normalize();

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Combines this sensitivity with another instance.
	  /// <para>
	  /// This returns an instance with a combined list of point sensitivities.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// Instead, the result of the method must be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="other">  the other sensitivity builder </param>
	  /// <returns> the combined builder, replacing this builder and the specified builder </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PointSensitivityBuilder combinedWith(PointSensitivityBuilder other)
	//  {
	//	if (other instanceof MutablePointSensitivities)
	//	{
	//	  MutablePointSensitivities otherCombination = (MutablePointSensitivities) other;
	//	  return buildInto(otherCombination);
	//	}
	//	MutablePointSensitivities combination = new MutablePointSensitivities();
	//	return other.buildInto(this.buildInto(combination));
	//  }

	  /// <summary>
	  /// Builds the point sensitivity, adding to the specified mutable instance.
	  /// </summary>
	  /// <param name="combination">  the combination object to add to </param>
	  /// <returns> the specified mutable point sensitivities instance is returned, for method chaining </returns>
	  MutablePointSensitivities buildInto(MutablePointSensitivities combination);

	  /// <summary>
	  /// Builds the resulting point sensitivity.
	  /// <para>
	  /// This returns a <seealso cref="PointSensitivities"/> instance.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable.
	  /// Once this method is called, this instance must not be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the built combined sensitivity </returns>
//JAVA TO C# CONVERTER TODO TASK: There is no equivalent in C# to Java default interface methods:
//	  public default PointSensitivities build()
	//  {
	//	return buildInto(new MutablePointSensitivities()).toImmutable();
	//  }

	  /// <summary>
	  /// Clones the point sensitivity builder.
	  /// <para>
	  /// This returns a <seealso cref="PointSensitivityBuilder"/> instance that is independent
	  /// from the original. Immutable implementations may return themselves.
	  /// </para>
	  /// <para>
	  /// Builders may be mutable. Using this method allows a copy of the original
	  /// to be obtained, so both the original and the clone can be used.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the built combined sensitivity </returns>
	  PointSensitivityBuilder cloned();

	}

}