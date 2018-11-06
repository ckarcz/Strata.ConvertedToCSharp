using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.model
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

	/// <summary>
	/// Data bundle related to the Hull-White one factor (extended Vasicek) model with piecewise constant volatility.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class HullWhiteOneFactorPiecewiseConstantParameters implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class HullWhiteOneFactorPiecewiseConstantParameters : ImmutableBean
	{

	  /// <summary>
	  /// The time used to represent infinity.
	  /// <para>
	  /// The last element of {@code volatilityTime} must be this value.
	  /// </para>
	  /// </summary>
	  private const double VOLATILITY_TIME_INFINITY = 1000d;

	  /// <summary>
	  /// The mean reversion speed parameter.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final double meanReversion;
	  private readonly double meanReversion;
	  /// <summary>
	  /// The volatility parameters.
	  /// <para>
	  /// The volatility is constant between the volatility times, i.e., volatility value at t is {@code volatility.get(i)} 
	  /// for any t between {@code volatilityTime.get(i)} and {@code volatilityTime.get(i+1)}.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray volatility;
	  private readonly DoubleArray volatility;
	  /// <summary>
	  /// The times separating the constant volatility periods.
	  /// <para>
	  /// The time should be sorted by increasing order. The first time is 0 and the last time is 1000 (represents infinity).
	  /// These extra times are added in <seealso cref="#of(double, DoubleArray, DoubleArray)"/>.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray volatilityTime;
	  private readonly DoubleArray volatilityTime;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the model parameters.
	  /// <para>
	  /// {@code volatilityTime} should be sorted in increasing order. The first time (0) and the last time (1000) will be 
	  /// added within this method. Thus the size of {@code volatility} should be greater than that of {@code volatilityTime}
	  /// by one.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="meanReversion">  the mean reversion speed (a) parameter </param>
	  /// <param name="volatility">  the volatility parameters </param>
	  /// <param name="volatilityTime">  the times separating the constant volatility periods </param>
	  /// <returns> the instance </returns>
	  public static HullWhiteOneFactorPiecewiseConstantParameters of(double meanReversion, DoubleArray volatility, DoubleArray volatilityTime)
	  {

		double[] volatilityTimeArray = new double[volatilityTime.size() + 2];
		volatilityTimeArray[0] = 0d;
		volatilityTimeArray[volatilityTime.size() + 1] = VOLATILITY_TIME_INFINITY;
		Array.Copy(volatilityTime.toArray(), 0, volatilityTimeArray, 1, volatilityTime.size());
		return new HullWhiteOneFactorPiecewiseConstantParameters(meanReversion, volatility, DoubleArray.copyOf(volatilityTimeArray));
	  }

	  //-------------------------------------------------------------------------
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		int sizeTime = volatilityTime.size();
		ArgChecker.isTrue(sizeTime == volatility.size() + 1, "size mismatch between volatility and volatilityTime");
		for (int i = 1; i < sizeTime; ++i)
		{
		  ArgChecker.isTrue(volatilityTime.get(i - 1) < volatilityTime.get(i), "volatility times should be increasing");
		}
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns a copy with the volatility parameters changed.
	  /// </summary>
	  /// <param name="volatility">  the new volatility parameters </param>
	  /// <returns> the new instance </returns>
	  public HullWhiteOneFactorPiecewiseConstantParameters withVolatility(DoubleArray volatility)
	  {
		return new HullWhiteOneFactorPiecewiseConstantParameters(meanReversion, volatility, volatilityTime);
	  }

	  /// <summary>
	  /// Gets the last volatility of the volatility parameters.
	  /// </summary>
	  /// <returns> the last volatility </returns>
	  public double LastVolatility
	  {
		  get
		  {
			return volatility.get(volatility.size() - 1);
		  }
	  }

	  /// <summary>
	  /// Returns a copy with the last volatility of the volatility parameters changed.
	  /// </summary>
	  /// <param name="volatility">  the new volatility </param>
	  /// <returns> the new instance </returns>
	  public HullWhiteOneFactorPiecewiseConstantParameters withLastVolatility(double volatility)
	  {
		double[] volatilityArray = this.volatility.toArray();
		volatilityArray[volatilityArray.Length - 1] = volatility;
		return new HullWhiteOneFactorPiecewiseConstantParameters(meanReversion, DoubleArray.copyOf(volatilityArray), volatilityTime);
	  }

	  /// <summary>
	  /// Returns a copy with an extra volatility and volatility time added at the end of the respective arrays.
	  /// </summary>
	  /// <param name="volatility">  the volatility </param>
	  /// <param name="volatilityTime">  the times separating the constant volatility periods. Must be larger than the previous one </param>
	  /// <returns> the new instance </returns>
	  public HullWhiteOneFactorPiecewiseConstantParameters withVolatilityAdded(double volatility, double volatilityTime)
	  {
		double[] volatilityArray = this.volatility.toArray();
		double[] volatilityTimeArray = this.volatilityTime.toArray();
		ArgChecker.isTrue(volatilityTime > volatilityTimeArray[volatilityTimeArray.Length - 2], "volatility times should be increasing");
		double[] newVolatilityArray = new double[volatilityArray.Length + 1];
		double[] newVolatilityTimeArray = new double[volatilityTimeArray.Length + 1];
		Array.Copy(volatilityArray, 0, newVolatilityArray, 0, volatilityArray.Length);
		Array.Copy(volatilityTimeArray, 0, newVolatilityTimeArray, 0, volatilityTimeArray.Length - 1);
		newVolatilityArray[volatilityArray.Length] = volatility;
		newVolatilityTimeArray[volatilityTimeArray.Length - 1] = volatilityTime;
		newVolatilityTimeArray[volatilityTimeArray.Length] = VOLATILITY_TIME_INFINITY;
		return new HullWhiteOneFactorPiecewiseConstantParameters(meanReversion, DoubleArray.copyOf(newVolatilityArray), DoubleArray.copyOf(newVolatilityTimeArray));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantParameters}.
	  /// </summary>
	  private static readonly TypedMetaBean<HullWhiteOneFactorPiecewiseConstantParameters> META_BEAN = LightMetaBean.of(typeof(HullWhiteOneFactorPiecewiseConstantParameters), MethodHandles.lookup(), new string[] {"meanReversion", "volatility", "volatilityTime"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code HullWhiteOneFactorPiecewiseConstantParameters}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<HullWhiteOneFactorPiecewiseConstantParameters> meta()
	  {
		return META_BEAN;
	  }

	  static HullWhiteOneFactorPiecewiseConstantParameters()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private HullWhiteOneFactorPiecewiseConstantParameters(double meanReversion, DoubleArray volatility, DoubleArray volatilityTime)
	  {
		JodaBeanUtils.notNull(meanReversion, "meanReversion");
		JodaBeanUtils.notNull(volatility, "volatility");
		JodaBeanUtils.notNull(volatilityTime, "volatilityTime");
		this.meanReversion = meanReversion;
		this.volatility = volatility;
		this.volatilityTime = volatilityTime;
		validate();
	  }

	  public override TypedMetaBean<HullWhiteOneFactorPiecewiseConstantParameters> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the mean reversion speed parameter. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public double MeanReversion
	  {
		  get
		  {
			return meanReversion;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatility parameters.
	  /// <para>
	  /// The volatility is constant between the volatility times, i.e., volatility value at t is {@code volatility.get(i)}
	  /// for any t between {@code volatilityTime.get(i)} and {@code volatilityTime.get(i+1)}.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Volatility
	  {
		  get
		  {
			return volatility;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the times separating the constant volatility periods.
	  /// <para>
	  /// The time should be sorted by increasing order. The first time is 0 and the last time is 1000 (represents infinity).
	  /// These extra times are added in <seealso cref="#of(double, DoubleArray, DoubleArray)"/>.
	  /// </para>
	  /// </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray VolatilityTime
	  {
		  get
		  {
			return volatilityTime;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  HullWhiteOneFactorPiecewiseConstantParameters other = (HullWhiteOneFactorPiecewiseConstantParameters) obj;
		  return JodaBeanUtils.equal(meanReversion, other.meanReversion) && JodaBeanUtils.equal(volatility, other.volatility) && JodaBeanUtils.equal(volatilityTime, other.volatilityTime);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(meanReversion);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatility);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatilityTime);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(128);
		buf.Append("HullWhiteOneFactorPiecewiseConstantParameters{");
		buf.Append("meanReversion").Append('=').Append(meanReversion).Append(',').Append(' ');
		buf.Append("volatility").Append('=').Append(volatility).Append(',').Append(' ');
		buf.Append("volatilityTime").Append('=').Append(JodaBeanUtils.ToString(volatilityTime));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}