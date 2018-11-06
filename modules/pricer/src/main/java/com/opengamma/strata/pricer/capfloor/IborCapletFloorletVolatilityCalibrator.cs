using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.capfloor
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.BLACK_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.NORMAL_VOLATILITY;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.STRIKE;


	using ReferenceData = com.opengamma.strata.basics.ReferenceData;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ConstantSurface = com.opengamma.strata.market.surface.ConstantSurface;
	using Surface = com.opengamma.strata.market.surface.Surface;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;
	using RatesProvider = com.opengamma.strata.pricer.rate.RatesProvider;
	using ResolvedIborCapFloorLeg = com.opengamma.strata.product.capfloor.ResolvedIborCapFloorLeg;

	/// <summary>
	/// Caplet volatilities calibration to cap volatilities.
	/// </summary>
	internal abstract class IborCapletFloorletVolatilityCalibrator
	{

	  /// <summary>
	  /// The cap/floor pricer. 
	  /// <para>
	  /// This pricer is used for converting market cap volatilities to cap prices. 
	  /// </para>
	  /// </summary>
	  private readonly VolatilityIborCapFloorLegPricer pricer;
	  /// <summary>
	  /// The reference data.
	  /// </summary>
	  private readonly ReferenceData referenceData;

	  /// <summary>
	  /// Constructor with cap pricer and reference data.
	  /// </summary>
	  /// <param name="pricer">  the cap pricer </param>
	  /// <param name="referenceData">  the reference data </param>
	  public IborCapletFloorletVolatilityCalibrator(VolatilityIborCapFloorLegPricer pricer, ReferenceData referenceData)
	  {
		this.pricer = ArgChecker.notNull(pricer, "pricer");
		this.referenceData = ArgChecker.notNull(referenceData, "referenceData");
	  }

	  /// <summary>
	  /// Calibrates caplet volatilities to cap volatilities.
	  /// </summary>
	  /// <param name="definition">  the caplet volatility definition </param>
	  /// <param name="calibrationDateTime">  the calibration time </param>
	  /// <param name="capFloorData">  the cap data </param>
	  /// <param name="ratesProvider">  the rates provider </param>
	  /// <returns> the calibration result </returns>
	  public abstract IborCapletFloorletVolatilityCalibrationResult calibrate(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData, RatesProvider ratesProvider);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the reference data.
	  /// </summary>
	  /// <returns> the reference data </returns>
	  protected internal virtual ReferenceData ReferenceData
	  {
		  get
		  {
			return referenceData;
		  }
	  }

	  /// <summary>
	  /// Gets the leg pricer.
	  /// </summary>
	  /// <returns> the leg pricer </returns>
	  protected internal virtual VolatilityIborCapFloorLegPricer LegPricer
	  {
		  get
		  {
			return pricer;
		  }
	  }

	  //-------------------------------------------------------------------------
	  // create complete lists of caps, volatilities, strikes, expiries
	  protected internal virtual void reduceRawData(IborCapletFloorletVolatilityDefinition definition, RatesProvider ratesProvider, DoubleArray strikes, DoubleArray volatilityData, DoubleArray errors, LocalDate startDate, LocalDate endDate, SurfaceMetadata metadata, System.Func<Surface, IborCapletFloorletVolatilities> volatilityFunction, IList<double> timeList, IList<double> strikeList, IList<double> volList, IList<ResolvedIborCapFloorLeg> capList, IList<double> priceList, IList<double> errorList)
	  {

		int nStrikes = strikes.size();
		for (int i = 0; i < nStrikes; ++i)
		{
		  if (Double.isFinite(volatilityData.get(i)))
		  {
			ResolvedIborCapFloorLeg capFloor = definition.createCap(startDate, endDate, strikes.get(i)).resolve(referenceData);
			capList.Add(capFloor);
			strikeList.Add(strikes.get(i));
			volList.Add(volatilityData.get(i));
			ConstantSurface constVolSurface = ConstantSurface.of(metadata, volatilityData.get(i));
			IborCapletFloorletVolatilities vols = volatilityFunction(constVolSurface);
			timeList.Add(vols.relativeTime(capFloor.FinalFixingDateTime));
			priceList.Add(pricer.presentValue(capFloor, ratesProvider, vols).Amount);
			errorList.Add(errors.get(i));
		  }
		}
	  }

	  // function creating volatilities object from surface
	  protected internal virtual System.Func<Surface, IborCapletFloorletVolatilities> volatilitiesFunction(IborCapletFloorletVolatilityDefinition definition, ZonedDateTime calibrationDateTime, RawOptionData capFloorData)
	  {

		IborIndex index = definition.Index;
		if (capFloorData.StrikeType.Equals(STRIKE))
		{
		  if (capFloorData.DataType.Equals(BLACK_VOLATILITY))
		  {
			return blackVolatilitiesFunction(index, calibrationDateTime);
		  }
		  else if (capFloorData.DataType.Equals(NORMAL_VOLATILITY))
		  {
			return normalVolatilitiesFunction(index, calibrationDateTime);
		  }
		  throw new System.ArgumentException("Data type not supported");
		}
		throw new System.ArgumentException("strike type must be ValueType.STRIKE");
	  }

	  private System.Func<Surface, IborCapletFloorletVolatilities> blackVolatilitiesFunction(IborIndex index, ZonedDateTime calibrationDateTime)
	  {

		System.Func<Surface, IborCapletFloorletVolatilities> func = (Surface s) =>
		{
	return BlackIborCapletFloorletExpiryStrikeVolatilities.of(index, calibrationDateTime, s);
		};
		return func;
	  }

	  private System.Func<Surface, IborCapletFloorletVolatilities> normalVolatilitiesFunction(IborIndex index, ZonedDateTime calibrationDateTime)
	  {

		System.Func<Surface, IborCapletFloorletVolatilities> func = (Surface s) =>
		{
	return NormalIborCapletFloorletExpiryStrikeVolatilities.of(index, calibrationDateTime, s);
		};
		return func;
	  }

	}

}