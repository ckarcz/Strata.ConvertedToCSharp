using System;
using System.Collections.Generic;
using System.Text;

/*
 * Copyright (C) 2011 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.pricer.fxopt
{

	using Bean = org.joda.beans.Bean;
	using BeanBuilder = org.joda.beans.BeanBuilder;
	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using MetaProperty = org.joda.beans.MetaProperty;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using ImmutablePreBuild = org.joda.beans.gen.ImmutablePreBuild;
	using ImmutableValidator = org.joda.beans.gen.ImmutableValidator;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using DirectMetaBean = org.joda.beans.impl.direct.DirectMetaBean;
	using DirectMetaProperty = org.joda.beans.impl.direct.DirectMetaProperty;
	using DirectMetaPropertyMap = org.joda.beans.impl.direct.DirectMetaPropertyMap;
	using DirectPrivateBeanBuilder = org.joda.beans.impl.direct.DirectPrivateBeanBuilder;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ArgChecker = com.opengamma.strata.collect.ArgChecker;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using DeltaStrike = com.opengamma.strata.market.option.DeltaStrike;
	using ParameterMetadata = com.opengamma.strata.market.param.ParameterMetadata;
	using ParameterPerturbation = com.opengamma.strata.market.param.ParameterPerturbation;
	using ParameterizedData = com.opengamma.strata.market.param.ParameterizedData;
	using GenericVolatilitySurfaceYearFractionParameterMetadata = com.opengamma.strata.pricer.common.GenericVolatilitySurfaceYearFractionParameterMetadata;
	using BlackFormulaRepository = com.opengamma.strata.pricer.impl.option.BlackFormulaRepository;

	/// <summary>
	/// A delta dependent smile as used in Forex market.
	/// <para>
	/// This contains the data for delta dependent smile from at-the-money, risk reversal and strangle.
	/// The delta used is the delta with respect to forward.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(builderScope = "private") public final class SmileDeltaParameters implements com.opengamma.strata.market.param.ParameterizedData, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class SmileDeltaParameters : ParameterizedData, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final double expiry;
		private readonly double expiry;
	  /// <summary>
	  /// The delta of the different data points.
	  /// Must be positive and sorted in ascending order.
	  /// The put will have as delta the opposite of the numbers.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.collect.array.DoubleArray delta;
	  private readonly DoubleArray delta;
	  /// <summary>
	  /// The volatilities associated with the strikes.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.opengamma.strata.collect.array.DoubleArray volatility;
	  private readonly DoubleArray volatility;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition private final com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata> parameterMetadata;
	  private readonly ImmutableList<ParameterMetadata> parameterMetadata;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from volatility.
	  /// <para>
	  /// {@code GenericVolatilitySurfaceYearFractionParameterMetadata} is used for parameter metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry associated to the data </param>
	  /// <param name="delta">  the delta of the different data points, must be positive and sorted in ascending order,
	  ///   the put will have as delta the opposite of the numbers </param>
	  /// <param name="volatility">  the volatilities </param>
	  /// <returns> the smile definition </returns>
	  public static SmileDeltaParameters of(double expiry, DoubleArray delta, DoubleArray volatility)
	  {

		return of(expiry, delta, volatility, createParameterMetadata(expiry, delta));
	  }

	  /// <summary>
	  /// Obtains an instance from volatility.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry associated to the data </param>
	  /// <param name="delta">  the delta of the different data points, must be positive and sorted in ascending order,
	  ///   the put will have as delta the opposite of the numbers </param>
	  /// <param name="volatility">  the volatilities </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the smile definition </returns>
	  public static SmileDeltaParameters of(double expiry, DoubleArray delta, DoubleArray volatility, IList<ParameterMetadata> parameterMetadata)
	  {

		ArgChecker.notNull(delta, "delta");
		ArgChecker.notNull(volatility, "volatility");
		return new SmileDeltaParameters(expiry, delta, volatility, parameterMetadata);
	  }

	  /// <summary>
	  /// Obtains an instance from market data at-the-money, delta, risk-reversal and strangle.
	  /// <para>
	  /// {@code GenericVolatilitySurfaceYearFractionParameterMetadata} is used for parameter metadata.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="expiry">  the time to expiry associated to the data </param>
	  /// <param name="atmVolatility">  the at-the-money volatility </param>
	  /// <param name="delta">  the delta of the different data points, must be positive and sorted in ascending order,
	  ///   the put will have as delta the opposite of the numbers </param>
	  /// <param name="riskReversal">  the risk reversal volatility figures, in the same order as the delta </param>
	  /// <param name="strangle">  the strangle volatility figures, in the same order as the delta </param>
	  /// <returns> the smile definition </returns>
	  public static SmileDeltaParameters of(double expiry, double atmVolatility, DoubleArray delta, DoubleArray riskReversal, DoubleArray strangle)
	  {

		return of(expiry, atmVolatility, delta, riskReversal, strangle, createParameterMetadata(expiry, delta));
	  }

	  /// <summary>
	  /// Obtains an instance from market data at-the-money, delta, risk-reversal and strangle.
	  /// </summary>
	  /// <param name="expiry">  the time to expiry associated to the data </param>
	  /// <param name="atmVolatility">  the at-the-money volatility </param>
	  /// <param name="delta">  the delta of the different data points, must be positive and sorted in ascending order,
	  ///   the put will have as delta the opposite of the numbers </param>
	  /// <param name="riskReversal">  the risk reversal volatility figures, in the same order as the delta </param>
	  /// <param name="strangle">  the strangle volatility figures, in the same order as the delta </param>
	  /// <param name="parameterMetadata">  the parameter metadata </param>
	  /// <returns> the smile definition </returns>
	  public static SmileDeltaParameters of(double expiry, double atmVolatility, DoubleArray delta, DoubleArray riskReversal, DoubleArray strangle, IList<ParameterMetadata> parameterMetadata)
	  {

		ArgChecker.notNull(delta, "delta");
		ArgChecker.notNull(riskReversal, "riskReversal");
		ArgChecker.notNull(strangle, "strangle");
		int nbDelta = delta.size();
		ArgChecker.isTrue(nbDelta == riskReversal.size(), "Length of delta {} should be equal to length of riskReversal {}", delta.size(), riskReversal.size());
		ArgChecker.isTrue(nbDelta == strangle.size(), "Length of delta {} should be equal to length of strangle {} ", delta.size(), strangle.size());

		double[] volatility = new double[2 * nbDelta + 1];
		volatility[nbDelta] = atmVolatility;
		for (int i = 0; i < nbDelta; i++)
		{
		  volatility[i] = strangle.get(i) + atmVolatility - riskReversal.get(i) / 2.0; // Put
		  volatility[2 * nbDelta - i] = strangle.get(i) + atmVolatility + riskReversal.get(i) / 2.0; // Call
		}
		return of(expiry, delta, DoubleArray.ofUnsafe(volatility), parameterMetadata);
	  }

	  //-------------------------------------------------------------------------
	  private static ImmutableList<ParameterMetadata> createParameterMetadata(double expiry, DoubleArray delta)
	  {
		ArgChecker.notNull(delta, "delta");
		int nbDelta = delta.size();
		ParameterMetadata[] parameterMetadata = new ParameterMetadata[2 * nbDelta + 1];
		parameterMetadata[nbDelta] = GenericVolatilitySurfaceYearFractionParameterMetadata.of(expiry, DeltaStrike.of(0.5d));
		for (int i = 0; i < nbDelta; i++)
		{
		  parameterMetadata[i] = GenericVolatilitySurfaceYearFractionParameterMetadata.of(expiry, DeltaStrike.of(1d - delta.get(i))); // Put
		  parameterMetadata[2 * nbDelta - i] = GenericVolatilitySurfaceYearFractionParameterMetadata.of(expiry, DeltaStrike.of(delta.get(i))); // Call
		}
		return ImmutableList.copyOf(parameterMetadata);
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutablePreBuild private static void preBuild(Builder builder)
	  private static void preBuild(Builder builder)
	  {
		if (builder.parameterMetadata == null)
		{
		  if (builder.delta != null)
		  {
			builder.parameterMetadata = createParameterMetadata(builder.expiry, builder.delta);
		  }
		}
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ImmutableValidator private void validate()
	  private void validate()
	  {
		int nbDelta = delta.size();
		ArgChecker.isTrue(2 * nbDelta + 1 == volatility.size(), "Length of delta {} should be coherent with volatility length {}", 2 * delta.size() + 1, volatility.size());
		ArgChecker.isTrue(2 * nbDelta + 1 == parameterMetadata.size(), "Length of delta {} should be coherent with parameterMetadata length {}", 2 * delta.size() + 1, parameterMetadata.size());
		if (nbDelta > 1)
		{
		  for (int i = 1; i < nbDelta; ++i)
		  {
			ArgChecker.isTrue(delta.get(i - 1) < delta.get(i), "delta should be sorted in ascending order");
		  }
		}
	  }

	  //-------------------------------------------------------------------------
	  public int ParameterCount
	  {
		  get
		  {
			return volatility.size();
		  }
	  }

	  public double getParameter(int parameterIndex)
	  {
		return volatility.get(parameterIndex);
	  }

	  public ParameterMetadata getParameterMetadata(int parameterIndex)
	  {
		return parameterMetadata.get(parameterIndex);
	  }

	  public SmileDeltaParameters withParameter(int parameterIndex, double newValue)
	  {
		return new SmileDeltaParameters(expiry, delta, volatility.with(parameterIndex, newValue), parameterMetadata);
	  }

	  public override SmileDeltaParameters withPerturbation(ParameterPerturbation perturbation)
	  {
		int size = volatility.size();
		DoubleArray perturbedValues = DoubleArray.of(size, i => perturbation(i, volatility.get(i), getParameterMetadata(i)));
		return new SmileDeltaParameters(expiry, delta, perturbedValues, parameterMetadata);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Calculates the strikes in ascending order.
	  /// <para>
	  /// The result has twice the number of values plus one as the delta/volatility.
	  /// The put with lower delta (in absolute value) first, at-the-money and call with larger delta first.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="forward">  the forward </param>
	  /// <returns> the strikes </returns>
	  public DoubleArray strike(double forward)
	  {
		int nbDelta = delta.size();
		double[] strike = new double[2 * nbDelta + 1];
		strike[nbDelta] = forward * Math.Exp(volatility.get(nbDelta) * volatility.get(nbDelta) * expiry / 2.0);
		for (int loopdelta = 0; loopdelta < nbDelta; loopdelta++)
		{
		  strike[loopdelta] = BlackFormulaRepository.impliedStrike(-delta.get(loopdelta), false, forward, expiry, volatility.get(loopdelta)); // Put
		  strike[2 * nbDelta - loopdelta] = BlackFormulaRepository.impliedStrike(delta.get(loopdelta), true, forward, expiry, volatility.get(2 * nbDelta - loopdelta)); // Call
		}
		return DoubleArray.ofUnsafe(strike);
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code SmileDeltaParameters}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static SmileDeltaParameters.Meta meta()
	  {
		return SmileDeltaParameters.Meta.INSTANCE;
	  }

	  static SmileDeltaParameters()
	  {
		MetaBean.register(SmileDeltaParameters.Meta.INSTANCE);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  private SmileDeltaParameters(double expiry, DoubleArray delta, DoubleArray volatility, IList<ParameterMetadata> parameterMetadata)
	  {
		this.expiry = expiry;
		this.delta = delta;
		this.volatility = volatility;
		this.parameterMetadata = (parameterMetadata != null ? ImmutableList.copyOf(parameterMetadata) : null);
		validate();
	  }

	  public override SmileDeltaParameters.Meta metaBean()
	  {
		return SmileDeltaParameters.Meta.INSTANCE;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the time to expiry associated with the data. </summary>
	  /// <returns> the value of the property </returns>
	  public double Expiry
	  {
		  get
		  {
			return expiry;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the delta of the different data points.
	  /// Must be positive and sorted in ascending order.
	  /// The put will have as delta the opposite of the numbers. </summary>
	  /// <returns> the value of the property </returns>
	  public DoubleArray Delta
	  {
		  get
		  {
			return delta;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the volatilities associated with the strikes. </summary>
	  /// <returns> the value of the property </returns>
	  public DoubleArray Volatility
	  {
		  get
		  {
			return volatility;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the parameterMetadata. </summary>
	  /// <returns> the value of the property </returns>
	  public ImmutableList<ParameterMetadata> ParameterMetadata
	  {
		  get
		  {
			return parameterMetadata;
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
		  SmileDeltaParameters other = (SmileDeltaParameters) obj;
		  return JodaBeanUtils.equal(expiry, other.expiry) && JodaBeanUtils.equal(delta, other.delta) && JodaBeanUtils.equal(volatility, other.volatility) && JodaBeanUtils.equal(parameterMetadata, other.parameterMetadata);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = this.GetType().GetHashCode();
		hash = hash * 31 + JodaBeanUtils.GetHashCode(expiry);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(delta);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(volatility);
		hash = hash * 31 + JodaBeanUtils.GetHashCode(parameterMetadata);
		return hash;
	  }

	  public override string ToString()
	  {
		StringBuilder buf = new StringBuilder(160);
		buf.Append("SmileDeltaParameters{");
		buf.Append("expiry").Append('=').Append(expiry).Append(',').Append(' ');
		buf.Append("delta").Append('=').Append(delta).Append(',').Append(' ');
		buf.Append("volatility").Append('=').Append(volatility).Append(',').Append(' ');
		buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		buf.Append('}');
		return buf.ToString();
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// The meta-bean for {@code SmileDeltaParameters}.
	  /// </summary>
	  public sealed class Meta : DirectMetaBean
	  {
		  internal bool InstanceFieldsInitialized = false;

		  internal void InitializeInstanceFields()
		  {
			  expiry_Renamed = DirectMetaProperty.ofImmutable(this, "expiry", typeof(SmileDeltaParameters), Double.TYPE);
			  delta_Renamed = DirectMetaProperty.ofImmutable(this, "delta", typeof(SmileDeltaParameters), typeof(DoubleArray));
			  volatility_Renamed = DirectMetaProperty.ofImmutable(this, "volatility", typeof(SmileDeltaParameters), typeof(DoubleArray));
			  parameterMetadata_Renamed = DirectMetaProperty.ofImmutable(this, "parameterMetadata", typeof(SmileDeltaParameters), (Type) typeof(ImmutableList));
			  metaPropertyMap$ = new DirectMetaPropertyMap(this, null, "expiry", "delta", "volatility", "parameterMetadata");
		  }

		/// <summary>
		/// The singleton instance of the meta-bean.
		/// </summary>
		internal static readonly Meta INSTANCE = new Meta();

		/// <summary>
		/// The meta-property for the {@code expiry} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<double> expiry_Renamed;
		/// <summary>
		/// The meta-property for the {@code delta} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> delta_Renamed;
		/// <summary>
		/// The meta-property for the {@code volatility} property.
		/// </summary>
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<DoubleArray> volatility_Renamed;
		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property.
		/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings({"unchecked", "rawtypes" }) private final org.joda.beans.MetaProperty<com.google.common.collect.ImmutableList<com.opengamma.strata.market.param.ParameterMetadata>> parameterMetadata = org.joda.beans.impl.direct.DirectMetaProperty.ofImmutable(this, "parameterMetadata", SmileDeltaParameters.class, (Class) com.google.common.collect.ImmutableList.class);
//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		internal MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata_Renamed;
		/// <summary>
		/// The meta-properties.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap$ = new org.joda.beans.impl.direct.DirectMetaPropertyMap(this, null, "expiry", "delta", "volatility", "parameterMetadata");
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
			case -1289159373: // expiry
			  return expiry_Renamed;
			case 95468472: // delta
			  return delta_Renamed;
			case -1917967323: // volatility
			  return volatility_Renamed;
			case -1169106440: // parameterMetadata
			  return parameterMetadata_Renamed;
		  }
		  return base.metaPropertyGet(propertyName);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public org.joda.beans.BeanBuilder<? extends SmileDeltaParameters> builder()
		public override BeanBuilder<SmileDeltaParameters> builder()
		{
		  return new SmileDeltaParameters.Builder();
		}

		public override Type beanType()
		{
		  return typeof(SmileDeltaParameters);
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: @Override public java.util.Map<String, org.joda.beans.MetaProperty<?>> metaPropertyMap()
		public override IDictionary<string, MetaProperty<object>> metaPropertyMap()
		{
		  return metaPropertyMap$;
		}

		//-----------------------------------------------------------------------
		/// <summary>
		/// The meta-property for the {@code expiry} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<double> expiry()
		{
		  return expiry_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code delta} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> delta()
		{
		  return delta_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code volatility} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<DoubleArray> volatility()
		{
		  return volatility_Renamed;
		}

		/// <summary>
		/// The meta-property for the {@code parameterMetadata} property. </summary>
		/// <returns> the meta-property, not null </returns>
		public MetaProperty<ImmutableList<ParameterMetadata>> parameterMetadata()
		{
		  return parameterMetadata_Renamed;
		}

		//-----------------------------------------------------------------------
		protected internal override object propertyGet(Bean bean, string propertyName, bool quiet)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1289159373: // expiry
			  return ((SmileDeltaParameters) bean).Expiry;
			case 95468472: // delta
			  return ((SmileDeltaParameters) bean).Delta;
			case -1917967323: // volatility
			  return ((SmileDeltaParameters) bean).Volatility;
			case -1169106440: // parameterMetadata
			  return ((SmileDeltaParameters) bean).ParameterMetadata;
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
	  /// The bean-builder for {@code SmileDeltaParameters}.
	  /// </summary>
	  private sealed class Builder : DirectPrivateBeanBuilder<SmileDeltaParameters>
	  {

		internal double expiry;
		internal DoubleArray delta;
		internal DoubleArray volatility;
		internal IList<ParameterMetadata> parameterMetadata;

		/// <summary>
		/// Restricted constructor.
		/// </summary>
		internal Builder()
		{
		}

		//-----------------------------------------------------------------------
		public override object get(string propertyName)
		{
		  switch (propertyName.GetHashCode())
		  {
			case -1289159373: // expiry
			  return expiry;
			case 95468472: // delta
			  return delta;
			case -1917967323: // volatility
			  return volatility;
			case -1169106440: // parameterMetadata
			  return parameterMetadata;
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
			case -1289159373: // expiry
			  this.expiry = (double?) newValue.Value;
			  break;
			case 95468472: // delta
			  this.delta = (DoubleArray) newValue;
			  break;
			case -1917967323: // volatility
			  this.volatility = (DoubleArray) newValue;
			  break;
			case -1169106440: // parameterMetadata
			  this.parameterMetadata = (IList<ParameterMetadata>) newValue;
			  break;
			default:
			  throw new NoSuchElementException("Unknown property: " + propertyName);
		  }
		  return this;
		}

		public override SmileDeltaParameters build()
		{
		  preBuild(this);
		  return new SmileDeltaParameters(expiry, delta, volatility, parameterMetadata);
		}

		//-----------------------------------------------------------------------
		public override string ToString()
		{
		  StringBuilder buf = new StringBuilder(160);
		  buf.Append("SmileDeltaParameters.Builder{");
		  buf.Append("expiry").Append('=').Append(JodaBeanUtils.ToString(expiry)).Append(',').Append(' ');
		  buf.Append("delta").Append('=').Append(JodaBeanUtils.ToString(delta)).Append(',').Append(' ');
		  buf.Append("volatility").Append('=').Append(JodaBeanUtils.ToString(volatility)).Append(',').Append(' ');
		  buf.Append("parameterMetadata").Append('=').Append(JodaBeanUtils.ToString(parameterMetadata));
		  buf.Append('}');
		  return buf.ToString();
		}

	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}