using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.option
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DoubleMatrix = com.opengamma.strata.collect.array.DoubleMatrix;
	using Pair = com.opengamma.strata.collect.tuple.Pair;
	using ValueType = com.opengamma.strata.market.ValueType;

	/// <summary>
	/// Raw data from the volatility market.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light") public final class RawOptionData implements org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class RawOptionData : ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.google.common.collect.ImmutableList<java.time.Period> expiries;
		private readonly ImmutableList<Period> expiries;
	  /// <summary>
	  /// The strike values. Can be directly strike or moneyness (simple or log)
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleArray strikes;
	  private readonly DoubleArray strikes;
	  /// <summary>
	  /// The value type of the strike-like dimension.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType strikeType;
	  private readonly ValueType strikeType;
	  /// <summary>
	  /// The data. The values can be model parameters (like Black or normal volatilities) or direct 
	  /// option prices. The first (outer) dimension is the expiry, the second dimension is the strike.
	  /// A 'NaN' value indicates that the data is not available.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.collect.array.DoubleMatrix data;
	  private readonly DoubleMatrix data;
	  /// <summary>
	  /// The measurement error of the option data.
	  /// <para>
	  /// These will be used if the option data is calibrated by a least square method. 
	  /// {@code data} and {@code error} must have the same number of elements.
	  /// </para>
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final com.opengamma.strata.collect.array.DoubleMatrix error;
	  private readonly DoubleMatrix error;
	  /// <summary>
	  /// The type of the raw data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.market.ValueType dataType;
	  private readonly ValueType dataType;
	  /// <summary>
	  /// The shift for which the raw data is valid. Used only if the dataType is 'BlackVolatility'.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(get = "optional") private final System.Nullable<double> shift;
	  private readonly double? shift;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance of the raw volatility.
	  /// <para>
	  /// The data values can be model parameters (like Black or normal volatilities) or direct option prices.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiries </param>
	  /// <param name="strikes">  the strikes-like data </param>
	  /// <param name="strikeType">  the value type of the strike-like dimension </param>
	  /// <param name="data">  the data </param>
	  /// <param name="dataType">  the data type </param>
	  /// <returns> the instance </returns>
	  public static RawOptionData of(IList<Period> expiries, DoubleArray strikes, ValueType strikeType, DoubleMatrix data, ValueType dataType)
	  {

		ArgChecker.isTrue(expiries.Count == data.rowCount(), "expiries list should be of the same size as the external data dimension");
		for (int i = 0; i < expiries.Count; i++)
		{
		  ArgChecker.isTrue(strikes.size() == data.columnCount(), "strikes should be of the same size as the inner data dimension");
		}
		return new RawOptionData(expiries, strikes, strikeType, data, null, dataType, 0.0);
	  }

	  /// <summary>
	  /// Obtains an instance of the raw volatility for shifted Black (log-normal) volatility.
	  /// </summary>
	  /// <param name="expiries">  the expiries </param>
	  /// <param name="strikes">  the strikes-like data </param>
	  /// <param name="strikeType">  the value type of the strike-like dimension </param>
	  /// <param name="data">  the data </param>
	  /// <param name="shift">  the shift </param>
	  /// <returns> the instance </returns>
	  public static RawOptionData ofBlackVolatility(IList<Period> expiries, DoubleArray strikes, ValueType strikeType, DoubleMatrix data, double? shift)
	  {

		ArgChecker.isTrue(expiries.Count == data.rowCount(), "expiries list should be of the same size as the external data dimension");
		for (int i = 0; i < expiries.Count; i++)
		{
		  ArgChecker.isTrue(strikes.size() == data.columnCount(), "strikes should be of the same size as the inner data dimension");
		}
		return new RawOptionData(expiries, strikes, strikeType, data, null, ValueType.BLACK_VOLATILITY, shift);
	  }

	  /// <summary>
	  /// Obtains an instance of the raw data with error.
	  /// <para>
	  /// The data values can be model parameters (like Black or normal volatilities) or direct option prices.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiries">  the expiries </param>
	  /// <param name="strikes">  the strikes-like data </param>
	  /// <param name="strikeType">  the value type of the strike-like dimension </param>
	  /// <param name="data">  the data </param>
	  /// <param name="error">  the error </param>
	  /// <param name="dataType">  the data type </param>
	  /// <returns> the instance </returns>
	  public static RawOptionData of(IList<Period> expiries, DoubleArray strikes, ValueType strikeType, DoubleMatrix data, DoubleMatrix error, ValueType dataType)
	  {

		ArgChecker.isTrue(expiries.Count == data.rowCount(), "expiries list should be of the same size as the external data dimension");
		ArgChecker.isTrue(error.rowCount() == data.rowCount(), "the error row count should be the same as the data raw count");
		ArgChecker.isTrue(error.columnCount() == data.columnCount(), "the error column count should the same as the data column count");
		for (int i = 0; i < expiries.Count; i++)
		{
		  ArgChecker.isTrue(strikes.size() == data.columnCount(), "strikes should be of the same size as the inner data dimension");
		}
		return new RawOptionData(expiries, strikes, strikeType, data, error, dataType, 0.0);
	  }

	  /// <summary>
	  /// Obtains an instance of the raw data with error for shifted Black (log-normal) volatility.
	  /// </summary>
	  /// <param name="expiries">  the expiries </param>
	  /// <param name="strikes">  the strikes-like data </param>
	  /// <param name="strikeType">  the value type of the strike-like dimension </param>
	  /// <param name="data">  the data </param>
	  /// <param name="error">  the error </param>
	  /// <param name="shift">  the shift </param>
	  /// <returns> the instance </returns>
	  public static RawOptionData ofBlackVolatility(IList<Period> expiries, DoubleArray strikes, ValueType strikeType, DoubleMatrix data, DoubleMatrix error, double? shift)
	  {

		ArgChecker.isTrue(expiries.Count == data.rowCount(), "expiries list should be of the same size as the external data dimension");
		for (int i = 0; i < expiries.Count; i++)
		{
		  ArgChecker.isTrue(strikes.size() == data.columnCount(), "strikes should be of the same size as the inner data dimension");
		}
		return new RawOptionData(expiries, strikes, strikeType, data, error, ValueType.BLACK_VOLATILITY, shift);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// For a given expiration returns all the data available.
	  /// </summary>
	  /// <param name="expiry">  the expiration </param>
	  /// <returns> the strikes and related volatilities for all available data at the given expiration </returns>
	  public Pair<DoubleArray, DoubleArray> availableSmileAtExpiry(Period expiry)
	  {
		int index = expiries.indexOf(expiry);
		ArgChecker.isTrue(index >= 0, "expiry not available");
		IList<double> strikesAvailable = new List<double>();
		IList<double> volatilitiesAvailable = new List<double>();
		for (int i = 0; i < strikes.size(); i++)
		{
		  if (!double.IsNaN(data.get(index, i)))
		  {
			strikesAvailable.Add(strikes.get(i));
			volatilitiesAvailable.Add(data.get(index, i));
		  }
		}
		return Pair.of(DoubleArray.copyOf(strikesAvailable), DoubleArray.copyOf(volatilitiesAvailable));
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code RawOptionData}.
	  /// </summary>
	  private static readonly TypedMetaBean<RawOptionData> META_BEAN = LightMetaBean.of(typeof(RawOptionData), MethodHandles.lookup(), new string[] {"expiries", "strikes", "strikeType", "data", "error", "dataType", "shift"}, ImmutableList.of(), null, null, null, null, null, null);

	  /// <summary>
	  /// The meta-bean for {@code RawOptionData}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<RawOptionData> meta()
	  {
		return META_BEAN;
	  }

	  static RawOptionData()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private RawOptionData(IList<Period> expiries, DoubleArray strikes, ValueType strikeType, DoubleMatrix data, DoubleMatrix error, ValueType dataType, double? shift)
	  {
		JodaBeanUtils.notNull(expiries, "expiries");
		JodaBeanUtils.notNull(strikes, "strikes");
		JodaBeanUtils.notNull(strikeType, "strikeType");
		JodaBeanUtils.notNull(data, "data");
		JodaBeanUtils.notNull(dataType, "dataType");
		this.expiries = ImmutableList.copyOf(expiries);
		this.strikes = strikes;
		this.strikeType = strikeType;
		this.data = data;
		this.error = error;
		this.dataType = dataType;
		this.shift = shift;
	  }

	  public override TypedMetaBean<RawOptionData> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the expiry values. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ImmutableList<Period> Expiries
	  {
		  get
		  {
			return expiries;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the strike values. Can be directly strike or moneyness (simple or log) </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleArray Strikes
	  {
		  get
		  {
			return strikes;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the value type of the strike-like dimension. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType StrikeType
	  {
		  get
		  {
			return strikeType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the data. The values can be model parameters (like Black or normal volatilities) or direct
	  /// option prices. The first (outer) dimension is the expiry, the second dimension is the strike.
	  /// A 'NaN' value indicates that the data is not available. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public DoubleMatrix Data
	  {
		  get
		  {
			return data;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the measurement error of the option data.
	  /// <para>
	  /// These will be used if the option data is calibrated by a least square method.
	  /// {@code data} and {@code error} must have the same number of elements.
	  /// </para>
	  /// </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public Optional<DoubleMatrix> Error
	  {
		  get
		  {
			return Optional.ofNullable(error);
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the type of the raw data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ValueType DataType
	  {
		  get
		  {
			return dataType;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the shift for which the raw data is valid. Used only if the dataType is 'BlackVolatility'. </summary>
	  /// <returns> the optional value of the property, not null </returns>
	  public double? Shift
	  {
		  get
		  {
			return shift != null ? double?.of(shift) : double?.empty();
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
		  RawOptionData other = (RawOptionData) obj;
		  return JodaBeanUtils.equal(expiries, other.expiries) && JodaBeanUtils.equal(strikes, other.strikes) && JodaBeanUtils.equal(strikeType, other.strikeType) && JodaBeanUtils.equal(data, other.data) && JodaBeanUtils.equal(error, other.error) && JodaBeanUtils.equal(dataType, other.dataType) && JodaBeanUtils.equal(shift, other.shift);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiries);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikes);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(strikeType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(data);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(error);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(dataType);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(shift);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(256);
		buf.Append("RawOptionData{");
		buf.Append("expiries").Append('=').Append(expiries).Append(',').Append(' ');
		buf.Append("strikes").Append('=').Append(strikes).Append(',').Append(' ');
		buf.Append("strikeType").Append('=').Append(strikeType).Append(',').Append(' ');
		buf.Append("data").Append('=').Append(data).Append(',').Append(' ');
		buf.Append("error").Append('=').Append(error).Append(',').Append(' ');
		buf.Append("dataType").Append('=').Append(dataType).Append(',').Append(' ');
		buf.Append("shift").Append('=').Append(JodaBeanUtils.ToString(shift));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}