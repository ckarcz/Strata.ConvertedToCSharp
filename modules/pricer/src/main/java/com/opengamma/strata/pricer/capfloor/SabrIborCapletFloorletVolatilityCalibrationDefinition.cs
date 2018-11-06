using System;
using System.Collections.Generic;
using System.Text;

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
//	import static com.opengamma.strata.market.ValueType.SABR_ALPHA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_BETA;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_NU;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.market.ValueType.SABR_RHO;


	using Bean = org.joda.beans.Bean;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectFieldsBeanBuilder = org.joda.beans.impl.direct.DirectFieldsBeanBuilder;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using DayCount = com.opengamma.strata.basics.date.DayCount;
	using IborIndex = com.opengamma.strata.basics.index.IborIndex;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using ConstantCurve = com.opengamma.strata.market.curve.ConstantCurve;
	using ConstantNodalCurve = com.opengamma.strata.market.curve.ConstantNodalCurve;
	using Curve = com.opengamma.strata.market.curve.Curve;
	using CurveMetadata = com.opengamma.strata.market.curve.CurveMetadata;
	using Curves = com.opengamma.strata.market.curve.Curves;
	using InterpolatedNodalCurve = com.opengamma.strata.market.curve.InterpolatedNodalCurve;
	using CurveExtrapolator = com.opengamma.strata.market.curve.interpolator.CurveExtrapolator;
	using CurveInterpolator = com.opengamma.strata.market.curve.interpolator.CurveInterpolator;
	using SurfaceMetadata = com.opengamma.strata.market.surface.SurfaceMetadata;
	using Surfaces = com.opengamma.strata.market.surface.Surfaces;
	using ParameterLimitsTransform = com.opengamma.strata.math.impl.minimization.ParameterLimitsTransform;
	using SabrVolatilityFormula = com.opengamma.strata.pricer.model.SabrVolatilityFormula;
	using RawOptionData = com.opengamma.strata.pricer.option.RawOptionData;

	/// <summary>
	/// Definition of caplet volatilities calibration.
	/// <para>
	/// This definition is used with <seealso cref="SabrIborCapletFloorletVolatilityCalibrator"/>. 
	/// The term structure of SABR model parameters is calibrated to cap volatilities. 
	/// The SABR parameters are represented by {@code NodalCurve} and the node positions on the curves are flexible.
	/// </para>
	/// <para>
	/// Either rho or beta must be fixed. 
	/// Then the calibration is computed in terms of the other three SABR parameter curves.
	/// The resulting volatilities object will be <seealso cref="SabrParametersIborCapletFloorletVolatilities"/>.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition public final class SabrIborCapletFloorletVolatilityCalibrationDefinition implements IborCapletFloorletVolatilityDefinition, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SabrIborCapletFloorletVolatilityCalibrationDefinition : IborCapletFloorletVolatilityDefinition, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final IborCapletFloorletVolatilitiesName name;
		private readonly IborCapletFloorletVolatilitiesName name;
	  /// <summary>
	  /// The Ibor index for which the data is valid.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.index.IborIndex index;
	  private readonly IborIndex index;
	  /// <summary>
	  /// The day count to measure the time in the expiry dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.basics.date.DayCount dayCount;
	  private readonly DayCount dayCount;
	  /// <summary>
	  /// The beta (elasticity) curve.
	  /// <para>
	  /// This represents the beta parameter of SABR model.
	  /// </para>
	  /// <para>
	  /// The beta will be treated as one of the calibration parameters if this field is not specified.
	  /// Either {@code betaCurve} or {@code rhoCurve} must be present. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.market.curve.Curve betaCurve;
	  private readonly Curve betaCurve;
	  /// <summary>
	  /// The rho (correlation) curve.
	  /// <para>
	  /// This represents the rho parameter of SABR model.
	  /// </para>
	  /// <para>
	  /// The rho will be treated as one of the calibration parameters if this field is not specified.
	  /// Either {@code betaCurve} or {@code rhoCurve} must be present. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.market.curve.Curve rhoCurve;
	  private readonly Curve rhoCurve;
	  /// <summary>
	  /// The shift curve.
	  /// <para>
	  /// This represents the shift parameter of shifted SABR model.
	  /// </para>
	  /// <para>
	  /// The shift is set to be zero if this field is not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.Curve shiftCurve;
	  private readonly Curve shiftCurve;
	  /// <summary>
	  /// The nodes of SABR parameter curves.
	  /// <para>
	  /// The size of the list must be 4, ordered as alpha, beta, rho and nu. 
	  /// </para>
	  /// <para>
	  /// If the number of nodes is greater than 1, the curve will be created with {@code CurveInterpolator} and 
	  /// {@code CurveExtrapolator} specified below. Otherwise, {@code ConstantNodalCurve} will be created.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray> parameterCurveNodes;
	  private readonly ImmutableList<DoubleArray> parameterCurveNodes;
	  /// <summary>
	  /// The initial parameter values used in calibration. 
	  /// <para>
	  /// Default values will be used if not specified. 
	  /// The size of this field must be 4, ordered as alpha, beta, rho and nu. 
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray initialParameters;
	  private readonly DoubleArray initialParameters;
	  /// <summary>
	  /// The interpolator for the SABR parameters.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveInterpolator interpolator;
	  private readonly CurveInterpolator interpolator;
	  /// <summary>
	  /// The left extrapolator for the SABR parameters.
	  /// <para>
	  /// The flat extrapolation is used if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorLeft;
	  private readonly CurveExtrapolator extrapolatorLeft;
	  /// <summary>
	  /// The right extrapolator for the SABR parameters.
	  /// <para>
	  /// The flat extrapolation is used if not specified.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.curve.interpolator.CurveExtrapolator extrapolatorRight;
	  private readonly CurveExtrapolator extrapolatorRight;
	  /// <summary>
	  /// The SABR formula.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.pricer.model.SabrVolatilityFormula sabrVolatilityFormula;
	  private readonly SabrVolatilityFormula sabrVolatilityFormula;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance with fixed beta and nonzero shift.
	  /// <para>
	  /// The beta and shift are constant in time.
	  /// The default initial values will be used in the calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="beta">  the beta </param>
	  /// <param name="shift">  the shift </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="rhoCurveNodes">  the rho curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedBeta(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double beta, double shift, DoubleArray alphaCurveNodes, DoubleArray rhoCurveNodes, DoubleArray nuCurveNodes, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		DoubleArray initialValues = DoubleArray.of(0.1, beta, -0.2, 0.5);
		return ofFixedBeta(name, index, dayCount, shift, alphaCurveNodes, rhoCurveNodes, nuCurveNodes, initialValues, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed beta and zero shift.
	  /// <para>
	  /// The default initial values will be used in the calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="beta">  the beta </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="rhoCurveNodes">  the rho curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedBeta(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double beta, DoubleArray alphaCurveNodes, DoubleArray rhoCurveNodes, DoubleArray nuCurveNodes, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		DoubleArray initialValues = DoubleArray.of(0.1, beta, -0.2, 0.5);
		return ofFixedBeta(name, index, dayCount, alphaCurveNodes, rhoCurveNodes, nuCurveNodes, initialValues, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed beta, nonzero shift and initial values.
	  /// <para>
	  /// The beta and shift are constant in time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="shift">  the shift </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="rhoCurveNodes">  the rho curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="initialParameters">  the initial parameters </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedBeta(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double shift, DoubleArray alphaCurveNodes, DoubleArray rhoCurveNodes, DoubleArray nuCurveNodes, DoubleArray initialParameters, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		ConstantCurve betaCurve = ConstantCurve.of(Curves.sabrParameterByExpiry(name.Name + "-Beta", dayCount, SABR_BETA), initialParameters.get(1));
		ConstantCurve shiftCurve = ConstantCurve.of("Shift curve", shift);
		return new SabrIborCapletFloorletVolatilityCalibrationDefinition(name, index, dayCount, betaCurve, null, shiftCurve, ImmutableList.of(alphaCurveNodes, DoubleArray.of(), rhoCurveNodes, nuCurveNodes), initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed beta, zero shift and initial values.
	  /// <para>
	  /// The beta and shift are constant in time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="rhoCurveNodes">  the rho curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="initialParameters">  the initial parameters </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedBeta(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, DoubleArray alphaCurveNodes, DoubleArray rhoCurveNodes, DoubleArray nuCurveNodes, DoubleArray initialParameters, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		ConstantCurve betaCurve = ConstantCurve.of(Curves.sabrParameterByExpiry(name.Name + "-Beta", dayCount, SABR_BETA), initialParameters.get(1));
		Curve shiftCurve = ConstantCurve.of("Zero shift", 0d);
		return new SabrIborCapletFloorletVolatilityCalibrationDefinition(name, index, dayCount, betaCurve, null, shiftCurve, ImmutableList.of(alphaCurveNodes, DoubleArray.of(), rhoCurveNodes, nuCurveNodes), initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed rho and nonzero shift.
	  /// <para>
	  /// The rho and shift are constant in time.
	  /// The default initial values will be used in the calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="rho">  the rho </param>
	  /// <param name="shift">  the shift </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="betaCurveNodes">  the beta curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedRho(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double rho, double shift, DoubleArray alphaCurveNodes, DoubleArray betaCurveNodes, DoubleArray nuCurveNodes, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		DoubleArray initialParameters = DoubleArray.of(0.1, 0.7, rho, 0.5);
		return ofFixedRho(name, index, dayCount, shift, alphaCurveNodes, betaCurveNodes, nuCurveNodes, initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed rho and zero shift.
	  /// <para>
	  /// The rho is constant in time.
	  /// The default initial values will be used in the calibration.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="rho">  the rho </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="betaCurveNodes">  the beta curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedRho(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double rho, DoubleArray alphaCurveNodes, DoubleArray betaCurveNodes, DoubleArray nuCurveNodes, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		DoubleArray initialParameters = DoubleArray.of(0.1, 0.7, rho, 0.5);
		return ofFixedRho(name, index, dayCount, alphaCurveNodes, betaCurveNodes, nuCurveNodes, initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed rho, nonzero shift and initial values.
	  /// <para>
	  /// The rho and shift are constant in time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="shift">  the shift </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="betaCurveNodes">  the beta curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="initialParameters">  the initial parameters </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedRho(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, double shift, DoubleArray alphaCurveNodes, DoubleArray betaCurveNodes, DoubleArray nuCurveNodes, DoubleArray initialParameters, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		ConstantCurve rhoCurve = ConstantCurve.of(Curves.sabrParameterByExpiry(name.Name + "-Rho", dayCount, SABR_RHO), initialParameters.get(2));
		ConstantCurve shiftCurve = ConstantCurve.of("Shift curve", shift);
		return new SabrIborCapletFloorletVolatilityCalibrationDefinition(name, index, dayCount, null, rhoCurve, shiftCurve, ImmutableList.of(alphaCurveNodes, betaCurveNodes, DoubleArray.of(), nuCurveNodes), initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

	  /// <summary>
	  /// Obtains an instance with fixed rho, zero shift and initial values.
	  /// <para>
	  /// The rho is constant in time.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name of volatilities </param>
	  /// <param name="index">  the Ibor index </param>
	  /// <param name="dayCount">  the day count </param>
	  /// <param name="alphaCurveNodes">  the alpha curve nodes </param>
	  /// <param name="betaCurveNodes">  the beta curve nodes </param>
	  /// <param name="nuCurveNodes">  the nu curve nodes </param>
	  /// <param name="initialParameters">  the initial parameters </param>
	  /// <param name="interpolator">  the interpolator </param>
	  /// <param name="extrapolatorLeft">  the left extrapolator </param>
	  /// <param name="extrapolatorRight">  the right extrapolator </param>
	  /// <param name="sabrVolatilityFormula">  the SABR formula </param>
	  /// <returns> the instance </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition ofFixedRho(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, DoubleArray alphaCurveNodes, DoubleArray betaCurveNodes, DoubleArray nuCurveNodes, DoubleArray initialParameters, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {

		ConstantCurve rhoCurve = ConstantCurve.of(Curves.sabrParameterByExpiry(name.Name + "-Rho", dayCount, SABR_RHO), initialParameters.get(2));
		Curve shiftCurve = ConstantCurve.of("Zero shift", 0d);
		return new SabrIborCapletFloorletVolatilityCalibrationDefinition(name, index, dayCount, null, rhoCurve, shiftCurve, ImmutableList.of(alphaCurveNodes, betaCurveNodes, DoubleArray.of(), nuCurveNodes), initialParameters, interpolator, extrapolatorLeft, extrapolatorRight, sabrVolatilityFormula);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		ArgChecker.isTrue(initialParameters.size() == 4, "The size of initialParameters must be 4");
		ArgChecker.isTrue(parameterCurveNodes.size() == 4, "The size of parameterCurveNodes must be 4");
		ArgChecker.isFalse(parameterCurveNodes.get(0).Empty, "The alpha curve nodes must not be empty");
		ArgChecker.isFalse(parameterCurveNodes.get(3).Empty, "The nu curve nodes must not be empty");
		if (betaCurve == null)
		{ // rho fixed
		  ArgChecker.isFalse(rhoCurve == null, "Either betaCurve or rhoCurve must be set");
		  ArgChecker.isFalse(parameterCurveNodes.get(1).Empty, "The beta curve nodes must not be empty");
		}
		else
		{ // beta fixed
		  ArgChecker.isTrue(rhoCurve == null, "Only betaCurve or rhoCurve must be set, not both");
		  ArgChecker.isFalse(parameterCurveNodes.get(2).Empty, "The rho curve nodes must not be empty");
		}
	  }

	  //-------------------------------------------------------------------------
	  public SurfaceMetadata createMetadata(RawOptionData capFloorData)
	  {
		SurfaceMetadata metadata;
		if (capFloorData.DataType.Equals(BLACK_VOLATILITY))
		{
		  metadata = Surfaces.blackVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else if (capFloorData.DataType.Equals(NORMAL_VOLATILITY))
		{
		  metadata = Surfaces.normalVolatilityByExpiryStrike(name.Name, dayCount);
		}
		else
		{
		  throw new System.ArgumentException("Data type not supported");
		}
		return metadata;
	  }

	  /// <summary>
	  /// Creates curve metadata for SABR parameters.
	  /// <para>
	  /// The metadata in the list are ordered as alpha, beta, rho, then nu.  
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the curve metadata </returns>
	  public ImmutableList<CurveMetadata> createSabrParameterMetadata()
	  {
		CurveMetadata alphaMetadata = Curves.sabrParameterByExpiry(name.Name + "-Alpha", dayCount, SABR_ALPHA);
		CurveMetadata betaMetadata = Curves.sabrParameterByExpiry(name.Name + "-Beta", dayCount, SABR_BETA);
		CurveMetadata rhoMetadata = Curves.sabrParameterByExpiry(name.Name + "-Rho", dayCount, SABR_RHO);
		CurveMetadata nuMetadata = Curves.sabrParameterByExpiry(name.Name + "-Nu", dayCount, SABR_NU);
		return ImmutableList.of(alphaMetadata, betaMetadata, rhoMetadata, nuMetadata);
	  }

	  /// <summary>
	  /// Creates the parameter curves with parameter node values. 
	  /// <para>
	  /// The node values must be combined nodes ordered as 
	  /// alpha, beta (if beta is not fixed), rho (if rho is not fixed), then nu. 
	  /// </para>
	  /// <para>
	  /// The returned curves are ordered in the same way. 
	  /// If the beta is fixed, {@code betaCurve} is returned as the second element.
	  /// If the rho is fixed, {@code rhoCurve} is returned as the third element.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="metadata">  the metadata </param>
	  /// <param name="nodeValues">  the parameter node values </param>
	  /// <returns> the curves </returns>
	  public IList<Curve> createSabrParameterCurve(IList<CurveMetadata> metadata, DoubleArray nodeValues)
	  {
		IList<Curve> res = new List<Curve>();
		int offset = 0;
		for (int i = 0; i < 4; ++i)
		{
		  if (isFixed(i))
		  {
			res.Add(BetaCurve.orElse(rhoCurve));
		  }
		  else
		  {
			int nNodes = parameterCurveNodes.get(i).size();
			int currentOffset = offset;
			if (nNodes > 1)
			{
			  res.Add(InterpolatedNodalCurve.of(metadata[i], parameterCurveNodes.get(i), DoubleArray.of(nNodes, n => nodeValues.get(n + currentOffset)), interpolator, extrapolatorLeft, extrapolatorRight));
			}
			else
			{
			  res.Add(ConstantNodalCurve.of(metadata[i], parameterCurveNodes.get(i).get(0), nodeValues.get(currentOffset)));
			}
			offset += nNodes;
		  }
		}
		return res;
	  }

	  /// <summary>
	  /// Creates the transformation definition for all the curve parameters. 
	  /// <para>
	  /// The elements in {@code transform} must be ordered as alpha, beta, rho, then nu.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="transform">  the transform </param>
	  /// <returns> the full transform </returns>
	  public ParameterLimitsTransform[] createFullTransform(ParameterLimitsTransform[] transform)
	  {
		ArgChecker.isTrue(transform.Length == 4, "transform must contain transformation defintion for alpha, beta, rho and nu");
		IList<ParameterLimitsTransform> fullTransformList = new List<ParameterLimitsTransform>();
		int length = 0;
		for (int i = 0; i < 4; ++i)
		{
		  if (isFixed(i))
		  {
			// fixed parameter
		  }
		  else
		  {
			int nNodes = parameterCurveNodes.get(i).size();
			((IList<ParameterLimitsTransform>)fullTransformList).AddRange(Collections.nCopies(nNodes, transform[i]));
			length += nNodes;
		  }
		}
		return fullTransformList.ToArray();
	  }

	  /// <summary>
	  /// Create initial values for all the curve parameters. 
	  /// </summary>
	  /// <returns> the initial values </returns>
	  public DoubleArray createFullInitialValues()
	  {
		IList<double> fullInitialValues = new List<double>();
		for (int i = 0; i < 4; ++i)
		{
		  if (isFixed(i))
		  {
			// fixed parameter
		  }
		  else
		  {
			int nNodes = parameterCurveNodes.get(i).size();
			((IList<double>)fullInitialValues).AddRange(Collections.nCopies(nNodes, initialParameters.get(i)));
		  }
		}
		return DoubleArray.copyOf(fullInitialValues);
	  }

	  private bool isFixed(int index)
	  {
		return (index == 1 && BetaCurve.Present) || (index == 2 && RhoCurve.Present);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrIborCapletFloorletVolatilityCalibrationDefinition}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition.Meta meta()
	  {
		return SabrIborCapletFloorletVolatilityCalibrationDefinition.Meta.INSTANCE;
	  }

	  static SabrIborCapletFloorletVolatilityCalibrationDefinition()
	  {
		MetaBean.register(SabrIborCapletFloorletVolatilityCalibrationDefinition.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// Returns a builder used to create an instance of the bean. </summary>
	  /// <returns> the builder, not null </returns>
	  public static SabrIborCapletFloorletVolatilityCalibrationDefinition.Builder builder()
	  {
		return new SabrIborCapletFloorletVolatilityCalibrationDefinition.Builder();
	  }

	  private SabrIborCapletFloorletVolatilityCalibrationDefinition(IborCapletFloorletVolatilitiesName name, IborIndex index, DayCount dayCount, Curve betaCurve, Curve rhoCurve, Curve shiftCurve, IList<DoubleArray> parameterCurveNodes, DoubleArray initialParameters, CurveInterpolator interpolator, CurveExtrapolator extrapolatorLeft, CurveExtrapolator extrapolatorRight, SabrVolatilityFormula sabrVolatilityFormula)
	  {
		JodaBeanUtils.notNull(name, "name");
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(dayCount, "dayCount");
		JodaBeanUtils.notNull(shiftCurve, "shiftCurve");
		JodaBeanUtils.notNull(parameterCurveNodes, "parameterCurveNodes");
		JodaBeanUtils.notNull(initialParameters, "initialParameters");
		JodaBeanUtils.notNull(interpolator, "interpolator");
		JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		JodaBeanUtils.notNull(sabrVolatilityFormula, "sabrVolatilityFormula");
		this.name = name;
		this.index = index;
		this.dayCount = dayCount;
		this.betaCurve = betaCurve;
		this.rhoCurve = rhoCurve;
		this.shiftCurve = shiftCurve;
		this.parameterCurveNodes = ImmutableList.copyOf(parameterCurveNodes);
		this.initialParameters = initialParameters;
		this.interpolator = interpolator;
		this.extrapolatorLeft = extrapolatorLeft;
		this.extrapolatorRight = extrapolatorRight;
		this.sabrVolatilityFormula = sabrVolatilityFormula;
		validate();
	  }

	  public override SabrIborCapletFloorletVolatilityCalibrationDefinition.Meta metaBean()
	  {
		return SabrIborCapletFloorletVolatilityCalibrationDefinition.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the name of the volatilities. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborCapletFloorletVolatilitiesName Name
	  {
		  get
		  {
			return name;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the Ibor index for which the data is valid. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public IborIndex Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the day count to measure the time in the expiry dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DayCount DayCount
	  {
		  get
		  {
			return dayCount;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the beta (elasticity) curve.
	  /// <para>
	  /// This represents the beta parameter of SABR model.
	  /// </para>
	  /// <para>
	  /// The beta will be treated as one of the calibration parameters if this field is not specified.
	  /// Either {@code betaCurve} or {@code rhoCurve} must be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<Curve> BetaCurve
	  {
		  get
		  {
			return Optional.ofNullable(betaCurve);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the rho (correlation) curve.
	  /// <para>
	  /// This represents the rho parameter of SABR model.
	  /// </para>
	  /// <para>
	  /// The rho will be treated as one of the calibration parameters if this field is not specified.
	  /// Either {@code betaCurve} or {@code rhoCurve} must be present.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<Curve> RhoCurve
	  {
		  get
		  {
			return Optional.ofNullable(rhoCurve);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift curve.
	  /// <para>
	  /// This represents the shift parameter of shifted SABR model.
	  /// </para>
	  /// <para>
	  /// The shift is set to be zero if this field is not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Curve ShiftCurve
	  {
		  get
		  {
			return shiftCurve;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the nodes of SABR parameter curves.
	  /// <para>
	  /// The size of the list must be 4, ordered as alpha, beta, rho and nu.
	  /// </para>
	  /// <para>
	  /// If the number of nodes is greater than 1, the curve will be created with {@code CurveInterpolator} and
	  /// {@code CurveExtrapolator} specified below. Otherwise, {@code ConstantNodalCurve} will be created.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<DoubleArray> ParameterCurveNodes
	  {
		  get
		  {
			return parameterCurveNodes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the initial parameter values used in calibration.
	  /// <para>
	  /// Default values will be used if not specified.
	  /// The size of this field must be 4, ordered as alpha, beta, rho and nu.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray InitialParameters
	  {
		  get
		  {
			return initialParameters;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the interpolator for the SABR parameters. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveInterpolator Interpolator
	  {
		  get
		  {
			return interpolator;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the left extrapolator for the SABR parameters.
	  /// <para>
	  /// The flat extrapolation is used if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorLeft
	  {
		  get
		  {
			return extrapolatorLeft;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the right extrapolator for the SABR parameters.
	  /// <para>
	  /// The flat extrapolation is used if not specified.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public CurveExtrapolator ExtrapolatorRight
	  {
		  get
		  {
			return extrapolatorRight;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the SABR formula. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public SabrVolatilityFormula SabrVolatilityFormula
	  {
		  get
		  {
			return sabrVolatilityFormula;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Returns a builder that allows this bean to be mutated. </summary>
	  /// <returns> the mutable builder, not null </returns>
	  public Builder toBuilder()
	  {
		return new Builder(this);
	  }

	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  SabrIborCapletFloorletVolatilityCalibrationDefinition other = (SabrIborCapletFloorletVolatilityCalibrationDefinition) obj;
		  return JodaBeanUtils.equal(name, other.name) && JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(dayCount, other.dayCount) && JodaBeanUtils.equal(betaCurve, other.betaCurve) && JodaBeanUtils.equal(rhoCurve, other.rhoCurve) && JodaBeanUtils.equal(shiftCurve, other.shiftCurve) && JodaBeanUtils.equal(parameterCurveNodes, other.parameterCurveNodes) && JodaBeanUtils.equal(initialParameters, other.initialParameters) && JodaBeanUtils.equal(interpolator, other.interpolator) && JodaBeanUtils.equal(extrapolatorLeft, other.extrapolatorLeft) && JodaBeanUtils.equal(extrapolatorRight, other.extrapolatorRight) && JodaBeanUtils.equal(sabrVolatilityFormula, other.sabrVolatilityFormula);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(name);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dayCount);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(betaCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(rhoCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shiftCurve);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterCurveNodes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(initialParameters);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(interpolator);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorLeft);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(extrapolatorRight);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(sabrVolatilityFormula);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(416);
		buf.Append("SabrIborCapletFloorletVolatilityCalibrationDefinition{");
		buf.Append("name").Append('=').Append(name).Append(',').Append(' ');
		buf.Append("index").Append('=').Append(index).Append(',').Append(' ');
		buf.Append("dayCount").Append('=').Append(dayCount).Append(',').Append(' ');
		buf.Append("betaCurve").Append('=').Append(betaCurve).Append(',').Append(' ');
		buf.Append("rhoCurve").Append('=').Append(rhoCurve).Append(',').Append(' ');
		buf.Append("shiftCurve").Append('=').Append(shiftCurve).Append(',').Append(' ');
		buf.Append("parameterCurveNodes").Append('=').Append(parameterCurveNodes).Append(',').Append(' ');
		buf.Append("initialParameters").Append('=').Append(initialParameters).Append(',').Append(' ');
		buf.Append("interpolator").Append('=').Append(interpolator).Append(',').Append(' ');
		buf.Append("extrapolatorLeft").Append('=').Append(extrapolatorLeft).Append(',').Append(' ');
		buf.Append("extrapolatorRight").Append('=').Append(extrapolatorRight).Append(',').Append(' ');
		buf.Append("sabrVolatilityFormula").Append('=').Append(JodaBeanUtils.ToString(sabrVolatilityFormula));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SabrIborCapletFloorletVolatilityCalibrationDefinition}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  name_Renamed = DirectMetaProperty.ofImmutable(this, "name", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(IborCapletFloorletVolatilitiesName));
			  index_Renamed = DirectMetaProperty.ofImmutable(this, "index", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(IborIndex));
			  dayCount_Renamed = DirectMetaProperty.ofImmutable(this, "dayCount", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(DayCount));
			  betaCurve_Renamed = DirectMetaProperty.ofImmutable(this, "betaCurve", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(Curve));
			  rhoCurve_Renamed = DirectMetaProperty.ofImmutable(this, "rhoCurve", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(Curve));
			  shiftCurve_Renamed = DirectMetaProperty.ofImmutable(this, "shiftCurve", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(Curve));
			  parameterCurveNodes_Renamed = DirectMetaProperty.ofImmutable(this, "parameterCurveNodes", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), (Type) typeof(ImmutableList));
			  initialParameters_Renamed = DirectMetaProperty.ofImmutable(this, "initialParameters", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(DoubleArray));
			  interpolator_Renamed = DirectMetaProperty.ofImmutable(this, "interpolator", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(CurveInterpolator));
			  extrapolatorLeft_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorLeft", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(CurveExtrapolator));
			  extrapolatorRight_Renamed = DirectMetaProperty.ofImmutable(this, "extrapolatorRight", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(CurveExtrapolator));
			  sabrVolatilityFormula_Renamed = DirectMetaProperty.ofImmutable(this, "sabrVolatilityFormula", typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition), typeof(SabrVolatilityFormula));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "betaCurve", "rhoCurve", "shiftCurve", "parameterCurveNodes", "initialParameters", "interpolator", "extrapolatorLeft", "extrapolatorRight", "sabrVolatilityFormula");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code name} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborCapletFloorletVolatilitiesName> name_Renamed;
		/// <summary>
		/// The meta-property for the {@code index} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<IborIndex> index_Renamed;
		/// <summary>
		/// The meta-property for the {@code dayCount} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DayCount> dayCount_Renamed;
		/// <summary>
		/// The meta-property for the {@code betaCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> betaCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code rhoCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> rhoCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code shiftCurve} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<Curve> shiftCurve_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterCurveNodes} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.collect.array.DoubleArray>> parameterCurveNodes = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterCurveNodes", SabrIborCapletFloorletVolatilityCalibrationDefinition.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<DoubleArray>> parameterCurveNodes_Renamed;
		/// <summary>
		/// The meta-property for the {@code initialParameters} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> initialParameters_Renamed;
		/// <summary>
		/// The meta-property for the {@code interpolator} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveInterpolator> interpolator_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorLeft_Renamed;
		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<CurveExtrapolator> extrapolatorRight_Renamed;
		/// <summary>
		/// The meta-property for the {@code sabrVolatilityFormula} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<SabrVolatilityFormula> sabrVolatilityFormula_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "name", "index", "dayCount", "betaCurve", "rhoCurve", "shiftCurve", "parameterCurveNodes", "initialParameters", "interpolator", "extrapolatorLeft", "extrapolatorRight", "sabrVolatilityFormula");
		internal IDictionary<string, MetaProperty<object>> metaPropertyMap$;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Meta()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override protected org.joda.beans.MetaProperty<?> metaPropertyGet(String propertyName)
		protected internal override MetaProperty<object> metaPropertyGet(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 1607020767: // betaCurve
			  return betaCurve_Renamed;
			case -2128671882: // rhoCurve
			  return rhoCurve_Renamed;
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
			case -1431162997: // parameterCurveNodes
			  return parameterCurveNodes_Renamed;
			case 1451864142: // initialParameters
			  return initialParameters_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
			case -683564541: // sabrVolatilityFormula
			  return sabrVolatilityFormula_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

		public override SabrIborCapletFloorletVolatilityCalibrationDefinition.Builder builder()
		{
		  return new SabrIborCapletFloorletVolatilityCalibrationDefinition.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SabrIborCapletFloorletVolatilityCalibrationDefinition);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code name} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborCapletFloorletVolatilitiesName> name()
		{
		  return name_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code index} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<IborIndex> index()
		{
		  return index_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code dayCount} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DayCount> dayCount()
		{
		  return dayCount_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code betaCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> betaCurve()
		{
		  return betaCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code rhoCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> rhoCurve()
		{
		  return rhoCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code shiftCurve} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<Curve> shiftCurve()
		{
		  return shiftCurve_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterCurveNodes} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<DoubleArray>> parameterCurveNodes()
		{
		  return parameterCurveNodes_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code initialParameters} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> initialParameters()
		{
		  return initialParameters_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code interpolator} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveInterpolator> interpolator()
		{
		  return interpolator_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorLeft} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorLeft()
		{
		  return extrapolatorLeft_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code extrapolatorRight} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<CurveExtrapolator> extrapolatorRight()
		{
		  return extrapolatorRight_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code sabrVolatilityFormula} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<SabrVolatilityFormula> sabrVolatilityFormula()
		{
		  return sabrVolatilityFormula_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).Name;
			case 100346066: // index
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).Index;
			case 1905311443: // dayCount
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).DayCount;
			case 1607020767: // betaCurve
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).betaCurve;
			case -2128671882: // rhoCurve
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).rhoCurve;
			case 1908090253: // shiftCurve
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).ShiftCurve;
			case -1431162997: // parameterCurveNodes
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).ParameterCurveNodes;
			case 1451864142: // initialParameters
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).InitialParameters;
			case 2096253127: // interpolator
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).Interpolator;
			case 1271703994: // extrapolatorLeft
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).ExtrapolatorLeft;
			case 773779145: // extrapolatorRight
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).ExtrapolatorRight;
			case -683564541: // sabrVolatilityFormula
			  return ((SabrIborCapletFloorletVolatilityCalibrationDefinition) bean).SabrVolatilityFormula;
		  }
		  return base.propertyGet(bean, propertyName, quiet);
		}

		protected internal override void propertySet(Bean bean, string propertyName, object newValue, bool quiet)
		{
		  metaProperty(propertyName);
		  if (quiet)
		  {
			return;
		  }
		  throw new System.NotSupportedException("Property cannot be written: " + propertyName);
		}

	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The bean-builder for {@code SabrIborCapletFloorletVolatilityCalibrationDefinition}.
	  /// </summary>
	  public sealed class Builder : DirectFieldsBeanBuilder<SabrIborCapletFloorletVolatilityCalibrationDefinition>
	  {

//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborCapletFloorletVolatilitiesName name_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IborIndex index_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DayCount dayCount_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Curve betaCurve_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Curve rhoCurve_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal Curve shiftCurve_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal IList<DoubleArray> parameterCurveNodes_Renamed = ImmutableList.of();
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal DoubleArray initialParameters_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveInterpolator interpolator_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorLeft_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal CurveExtrapolator extrapolatorRight_Renamed;
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal SabrVolatilityFormula sabrVolatilityFormula_Renamed;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		/// <summary>
		/// Restricted copy constructor. </summary>
		/// <param name="beanToCopy">  the bean to copy from, not null </param>
		internal Builder(SabrIborCapletFloorletVolatilityCalibrationDefinition beanToCopy)
		{
		  this.name_Renamed = beanToCopy.Name;
		  this.index_Renamed = beanToCopy.Index;
		  this.dayCount_Renamed = beanToCopy.DayCount;
		  this.betaCurve_Renamed = beanToCopy.betaCurve;
		  this.rhoCurve_Renamed = beanToCopy.rhoCurve;
		  this.shiftCurve_Renamed = beanToCopy.ShiftCurve;
		  this.parameterCurveNodes_Renamed = beanToCopy.ParameterCurveNodes;
		  this.initialParameters_Renamed = beanToCopy.InitialParameters;
		  this.interpolator_Renamed = beanToCopy.Interpolator;
		  this.extrapolatorLeft_Renamed = beanToCopy.ExtrapolatorLeft;
		  this.extrapolatorRight_Renamed = beanToCopy.ExtrapolatorRight;
		  this.sabrVolatilityFormula_Renamed = beanToCopy.SabrVolatilityFormula;
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  return name_Renamed;
			case 100346066: // index
			  return index_Renamed;
			case 1905311443: // dayCount
			  return dayCount_Renamed;
			case 1607020767: // betaCurve
			  return betaCurve_Renamed;
			case -2128671882: // rhoCurve
			  return rhoCurve_Renamed;
			case 1908090253: // shiftCurve
			  return shiftCurve_Renamed;
			case -1431162997: // parameterCurveNodes
			  return parameterCurveNodes_Renamed;
			case 1451864142: // initialParameters
			  return initialParameters_Renamed;
			case 2096253127: // interpolator
			  return interpolator_Renamed;
			case 1271703994: // extrapolatorLeft
			  return extrapolatorLeft_Renamed;
			case 773779145: // extrapolatorRight
			  return extrapolatorRight_Renamed;
			case -683564541: // sabrVolatilityFormula
			  return sabrVolatilityFormula_Renamed;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		}

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") @Override public Builder set(String propertyName, Object newValue)
		public override Builder set(string propertyName, object newValue)
		{
		  switch (propertyName.GetHashCode())
		  {
			case 3373707: // name
			  this.name_Renamed = (IborCapletFloorletVolatilitiesName) newValue;
			  break;
			case 100346066: // index
			  this.index_Renamed = (IborIndex) newValue;
			  break;
			case 1905311443: // dayCount
			  this.dayCount_Renamed = (DayCount) newValue;
			  break;
			case 1607020767: // betaCurve
			  this.betaCurve_Renamed = (Curve) newValue;
			  break;
			case -2128671882: // rhoCurve
			  this.rhoCurve_Renamed = (Curve) newValue;
			  break;
			case 1908090253: // shiftCurve
			  this.shiftCurve_Renamed = (Curve) newValue;
			  break;
			case -1431162997: // parameterCurveNodes
			  this.parameterCurveNodes_Renamed = (IList<DoubleArray>) newValue;
			  break;
			case 1451864142: // initialParameters
			  this.initialParameters_Renamed = (DoubleArray) newValue;
			  break;
			case 2096253127: // interpolator
			  this.interpolator_Renamed = (CurveInterpolator) newValue;
			  break;
			case 1271703994: // extrapolatorLeft
			  this.extrapolatorLeft_Renamed = (CurveExtrapolator) newValue;
			  break;
			case 773779145: // extrapolatorRight
			  this.extrapolatorRight_Renamed = (CurveExtrapolator) newValue;
			  break;
			case -683564541: // sabrVolatilityFormula
			  this.sabrVolatilityFormula_Renamed = (SabrVolatilityFormula) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override Builder set<T1>(MetaProperty<T1> property, object value)
		{
		  base.set(property, value);
		  return this;
		}

		public override SabrIborCapletFloorletVolatilityCalibrationDefinition build()
		{
		  return new SabrIborCapletFloorletVolatilityCalibrationDefinition(name_Renamed, index_Renamed, dayCount_Renamed, betaCurve_Renamed, rhoCurve_Renamed, shiftCurve_Renamed, parameterCurveNodes_Renamed, initialParameters_Renamed, interpolator_Renamed, extrapolatorLeft_Renamed, extrapolatorRight_Renamed, sabrVolatilityFormula_Renamed);
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// Sets the name of the volatilities. </summary>
		/// <param name="name">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder name(IborCapletFloorletVolatilitiesName name)
		{
		  JodaBeanUtils.notNull(name, "name");
		  this.name_Renamed = name;
		  return this;
		}

		/// <summary>
		/// Sets the Ibor index for which the data is valid. </summary>
		/// <param name="index">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder index(IborIndex index)
		{
		  JodaBeanUtils.notNull(index, "index");
		  this.index_Renamed = index;
		  return this;
		}

		/// <summary>
		/// Sets the day count to measure the time in the expiry dimension. </summary>
		/// <param name="dayCount">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder dayCount(DayCount dayCount)
		{
		  JodaBeanUtils.notNull(dayCount, "dayCount");
		  this.dayCount_Renamed = dayCount;
		  return this;
		}

		/// <summary>
		/// Sets the beta (elasticity) curve.
		/// <para>
		/// This represents the beta parameter of SABR model.
		/// </para>
		/// <para>
		/// The beta will be treated as one of the calibration parameters if this field is not specified.
		/// Either {@code betaCurve} or {@code rhoCurve} must be present.
		/// </para>
		/// </summary>
		/// <param name="betaCurve">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder betaCurve(Curve betaCurve)
		{
		  this.betaCurve_Renamed = betaCurve;
		  return this;
		}

		/// <summary>
		/// Sets the rho (correlation) curve.
		/// <para>
		/// This represents the rho parameter of SABR model.
		/// </para>
		/// <para>
		/// The rho will be treated as one of the calibration parameters if this field is not specified.
		/// Either {@code betaCurve} or {@code rhoCurve} must be present.
		/// </para>
		/// </summary>
		/// <param name="rhoCurve">  the new value </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder rhoCurve(Curve rhoCurve)
		{
		  this.rhoCurve_Renamed = rhoCurve;
		  return this;
		}

		/// <summary>
		/// Sets the shift curve.
		/// <para>
		/// This represents the shift parameter of shifted SABR model.
		/// </para>
		/// <para>
		/// The shift is set to be zero if this field is not specified.
		/// </para>
		/// </summary>
		/// <param name="shiftCurve">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder shiftCurve(Curve shiftCurve)
		{
		  JodaBeanUtils.notNull(shiftCurve, "shiftCurve");
		  this.shiftCurve_Renamed = shiftCurve;
		  return this;
		}

		/// <summary>
		/// Sets the nodes of SABR parameter curves.
		/// <para>
		/// The size of the list must be 4, ordered as alpha, beta, rho and nu.
		/// </para>
		/// <para>
		/// If the number of nodes is greater than 1, the curve will be created with {@code CurveInterpolator} and
		/// {@code CurveExtrapolator} specified below. Otherwise, {@code ConstantNodalCurve} will be created.
		/// </para>
		/// </summary>
		/// <param name="parameterCurveNodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameterCurveNodes(IList<DoubleArray> parameterCurveNodes)
		{
		  JodaBeanUtils.notNull(parameterCurveNodes, "parameterCurveNodes");
		  this.parameterCurveNodes_Renamed = parameterCurveNodes;
		  return this;
		}

		/// <summary>
		/// Sets the {@code parameterCurveNodes} property in the builder
		/// from an array of objects. </summary>
		/// <param name="parameterCurveNodes">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder parameterCurveNodes(params DoubleArray[] parameterCurveNodes)
		{
		  return this.parameterCurveNodes(ImmutableList.copyOf(parameterCurveNodes));
		}

		/// <summary>
		/// Sets the initial parameter values used in calibration.
		/// <para>
		/// Default values will be used if not specified.
		/// The size of this field must be 4, ordered as alpha, beta, rho and nu.
		/// </para>
		/// </summary>
		/// <param name="initialParameters">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder initialParameters(DoubleArray initialParameters)
		{
		  JodaBeanUtils.notNull(initialParameters, "initialParameters");
		  this.initialParameters_Renamed = initialParameters;
		  return this;
		}

		/// <summary>
		/// Sets the interpolator for the SABR parameters. </summary>
		/// <param name="interpolator">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder interpolator(CurveInterpolator interpolator)
		{
		  JodaBeanUtils.notNull(interpolator, "interpolator");
		  this.interpolator_Renamed = interpolator;
		  return this;
		}

		/// <summary>
		/// Sets the left extrapolator for the SABR parameters.
		/// <para>
		/// The flat extrapolation is used if not specified.
		/// </para>
		/// </summary>
		/// <param name="extrapolatorLeft">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorLeft(CurveExtrapolator extrapolatorLeft)
		{
		  JodaBeanUtils.notNull(extrapolatorLeft, "extrapolatorLeft");
		  this.extrapolatorLeft_Renamed = extrapolatorLeft;
		  return this;
		}

		/// <summary>
		/// Sets the right extrapolator for the SABR parameters.
		/// <para>
		/// The flat extrapolation is used if not specified.
		/// </para>
		/// </summary>
		/// <param name="extrapolatorRight">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder extrapolatorRight(CurveExtrapolator extrapolatorRight)
		{
		  JodaBeanUtils.notNull(extrapolatorRight, "extrapolatorRight");
		  this.extrapolatorRight_Renamed = extrapolatorRight;
		  return this;
		}

		/// <summary>
		/// Sets the SABR formula. </summary>
		/// <param name="sabrVolatilityFormula">  the new value, not null </param>
		/// <returns> this, for chaining, not null </returns>
		public Builder sabrVolatilityFormula(SabrVolatilityFormula sabrVolatilityFormula)
		{
		  JodaBeanUtils.notNull(sabrVolatilityFormula, "sabrVolatilityFormula");
		  this.sabrVolatilityFormula_Renamed = sabrVolatilityFormula;
		  return this;
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(416);
		  buf.Append("SabrIborCapletFloorletVolatilityCalibrationDefinition.Builder{");
		  buf.Append("name").Append('=').Append(JodaBeanUtils.ToString(name_Renamed)).Append(',').Append(' ');
		  buf.Append("index").Append('=').Append(JodaBeanUtils.ToString(index_Renamed)).Append(',').Append(' ');
		  buf.Append("dayCount").Append('=').Append(JodaBeanUtils.ToString(dayCount_Renamed)).Append(',').Append(' ');
		  buf.Append("betaCurve").Append('=').Append(JodaBeanUtils.ToString(betaCurve_Renamed)).Append(',').Append(' ');
		  buf.Append("rhoCurve").Append('=').Append(JodaBeanUtils.ToString(rhoCurve_Renamed)).Append(',').Append(' ');
		  buf.Append("shiftCurve").Append('=').Append(JodaBeanUtils.ToString(shiftCurve_Renamed)).Append(',').Append(' ');
		  buf.Append("parameterCurveNodes").Append('=').Append(JodaBeanUtils.ToString(parameterCurveNodes_Renamed)).Append(',').Append(' ');
		  buf.Append("initialParameters").Append('=').Append(JodaBeanUtils.ToString(initialParameters_Renamed)).Append(',').Append(' ');
		  buf.Append("interpolator").Append('=').Append(JodaBeanUtils.ToString(interpolator_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorLeft").Append('=').Append(JodaBeanUtils.ToString(extrapolatorLeft_Renamed)).Append(',').Append(' ');
		  buf.Append("extrapolatorRight").Append('=').Append(JodaBeanUtils.ToString(extrapolatorRight_Renamed)).Append(',').Append(' ');
		  buf.Append("sabrVolatilityFormula").Append('=').Append(JodaBeanUtils.ToString(sabrVolatilityFormula_Renamed));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}